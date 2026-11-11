using System.Buffers.Binary;
using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>
/// The document form of a mapped class (own-formats.md 2.3): `$case` names the case by its
/// registration key, the case fields hold the stored members the case keeps with their stored
/// presence, and `$residue` holds any other stored member of the mapping exactly. An unknown
/// key keeps the discriminator and every value in the unknown case. The stored image is the
/// flat object the schema encodes, so G1 and G2 hold through the mapping.
/// </summary>
internal static class CaseCodec
{
    private static ConversionException Missing(SchemaClass cls, SchemaMapping mapping, string? file, long offset, DocumentNodePath path) =>
        new(new FormatDiagnostic(Codes.Mapping, $"Class {cls.Name} needs its unit mapping {mapping.Name}, which Aow.Game registers.")
        { File = file, Offset = offset, Path = path.ToString() });

    /// <summary>The number a stored discriminator or field holds: an integer, or little-endian record bytes.</summary>
    private static long Number(JsonValue value) => value switch
    {
        JsonInteger i => i.Value,
        JsonString { Value.Length: > 0 and <= 16 } s when s.Value.Length % 2 == 0 =>
            (long)BinaryPrimitives.ReadUInt64LittleEndian(Convert.FromHexString(s.Value).Concat(new byte[8]).Take(8).ToArray()),
        _ => 0,
    };

    private static JsonValue Stored(SchemaClass cls, string member, long key)
    {
        var codec = cls.PropertyByName(member)!.Codec;
        if (codec is DataCodec { Size: int size })
        {
            var bytes = new byte[8];
            BinaryPrimitives.WriteInt64LittleEndian(bytes, key);
            return new JsonString(LayoutResidue.Hex(bytes.AsSpan(0, size)));
        }
        return new JsonInteger(key);
    }

    private static void InsertReserved(JsonObject obj, JsonMember member)
    {
        int rank = Array.IndexOf(Reserved.Order, member.Name);
        int at = 0;
        while (at < obj.Members.Count && obj.Members[at].Name.StartsWith('$') && Array.IndexOf(Reserved.Order, obj.Members[at].Name) < rank) at++;
        obj.Members.Insert(at, member);
    }

    /// <summary>Turns the decoded stored members of a mapped object into its case.</summary>
    public static void ToDocument(DecodeContext ctx, SchemaClass cls, SchemaMapping mapping, JsonObject target, DocumentNodePath path, int offset)
    {
        if (CaseMappings.Find(cls.Name) is null) throw Missing(cls, mapping, ctx.File, offset, path);
        var stored = new Dictionary<string, JsonValue>(StringComparer.Ordinal);
        int first = -1;
        for (int i = 0; i < target.Members.Count;)
        {
            var m = target.Members[i];
            if (mapping.StoredMembers.Contains(m.Name))
            {
                if (first < 0) first = i;
                stored[m.Name] = m.Value;
                target.Members.RemoveAt(i);
            }
            else i++;
        }
        if (first < 0) first = target.Members.Count;
        SchemaCase chosen = mapping.Unknown;
        if (stored.TryGetValue(mapping.Discriminator, out var discriminator))
        {
            long key = Number(discriminator);
            chosen = mapping.Cases.FirstOrDefault(c => c.Key == key) ?? mapping.Unknown;
        }
        var fields = new List<JsonMember>();
        var used = new HashSet<string>(StringComparer.Ordinal);
        if (chosen.Key is not null) used.Add(mapping.Discriminator);
        foreach (var (field, member) in chosen.Fields)
        {
            used.Add(member);
            if (stored.TryGetValue(member, out var value)) fields.Add(new JsonMember(field, value));
        }
        var residue = new JsonObject();
        foreach (var member in mapping.StoredMembers)
            if (!used.Contains(member) && stored.TryGetValue(member, out var value)) residue.Add(member, value);
        int before = target.Members.Count;
        InsertReserved(target, new JsonMember("$case", new JsonString(chosen.Name)));
        if (residue.Members.Count > 0) InsertReserved(target, new JsonMember("$residue", residue));
        first += target.Members.Count - before;
        target.Members.InsertRange(Math.Min(first, target.Members.Count), fields);
    }

    /// <summary>
    /// The flat object the schema encodes for a mapped object. The case fields go back to their
    /// stored members with their presence, the discriminator gets the case's key, and the
    /// residue goes back exactly. The kind must keep every case field: the unit side's
    /// ToFields writes it back unchanged.
    /// </summary>
    public static JsonObject FromDocument(EncodeContext ctx, SchemaClass cls, SchemaMapping mapping, JsonObject obj, DocumentNodePath path)
    {
        var unit = CaseMappings.Find(cls.Name)
            ?? throw ctx.Fail(Codes.Mapping, $"Class {cls.Name} needs its unit mapping {mapping.Name}, which Aow.Game registers.", obj, path);
        if (obj["$case"] is not JsonString caseName)
            throw ctx.Fail(obj["$case"] is null ? Codes.MissingMember : Codes.Type, $"A {cls.Name} names its {mapping.Name} case in $case.", obj["$case"] ?? obj, path.Member("$case"));
        var chosen = mapping.Cases.FirstOrDefault(c => c.Name == caseName.Value) ?? (mapping.Unknown.Name == caseName.Value ? mapping.Unknown : null)
            ?? throw ctx.Fail(Codes.Case, $"{mapping.Name} has no case {caseName.Value}.", caseName, path.Member("$case"), caseName.Value,
                string.Join(", ", mapping.Cases.Select(c => c.Name).Append(mapping.Unknown.Name)));
        // The unknown case keeps only keys no case registers, so its export reads back the same.
        string keyField = mapping.Unknown.Fields.First(f => f.Member == mapping.Discriminator).Field;
        if (chosen.Key is null && obj[keyField] is JsonValue kind && mapping.Cases.FirstOrDefault(c => c.Key == Number(kind)) is { } known)
            throw ctx.Fail(Codes.Case, $"Key {Number(kind).ToString(CultureInfo.InvariantCulture)} has the case {known.Name}, not {chosen.Name}.",
                kind, path.Member(keyField), Number(kind).ToString(CultureInfo.InvariantCulture), known.Name);
        var flat = new JsonObject { Span = obj.Span, Origin = obj.Origin };
        var fieldMembers = chosen.Fields.ToDictionary(f => f.Field, f => f.Member, StringComparer.Ordinal);
        var numbers = new Dictionary<string, long>(StringComparer.Ordinal);
        foreach (var m in obj.Members)
        {
            if (m.Name == "$case") continue;
            if (m.Name == "$residue")
            {
                if (m.Value is not JsonObject residue) throw ctx.Fail(Codes.Type, "$residue is an object.", m.Value, path.Member("$residue"));
                foreach (var r in residue.Members)
                {
                    if (!mapping.StoredMembers.Contains(r.Name) || r.Name == mapping.Discriminator && chosen.Key is not null || chosen.Fields.Any(f => f.Member == r.Name))
                        throw ctx.Fail(Codes.UnknownMember, $"$residue of case {chosen.Name} has no member \"{r.Name}\".", r.Value, path.Member("$residue").Member(r.Name), r.Name);
                    flat.Members.Add(new JsonMember(r.Name, r.Value) { NameSpan = r.NameSpan });
                    numbers[r.Name] = Number(r.Value);
                }
                continue;
            }
            if (fieldMembers.TryGetValue(m.Name, out var member))
            {
                flat.Members.Add(new JsonMember(member, m.Value) { NameSpan = m.NameSpan });
                numbers[member] = Number(m.Value);
                continue;
            }
            if (mapping.StoredMembers.Contains(m.Name))
                throw ctx.Fail(Codes.UnknownMember, $"Case {chosen.Name} has no field \"{m.Name}\". A value the case does not keep goes in $residue.", m.Value, path.Member(m.Name), m.Name);
            flat.Members.Add(m);
        }
        if (chosen.Key is long key)
        {
            flat.Members.Add(new JsonMember(mapping.Discriminator, Stored(cls, mapping.Discriminator, key)) { NameSpan = caseName.Span });
            numbers[mapping.Discriminator] = key;
            var written = unit.Written(key, numbers);
            foreach (var (field, member) in chosen.Fields)
                if (written.GetValueOrDefault(member) != numbers.GetValueOrDefault(member))
                    throw ctx.Fail(Codes.Mapping, $"Case {chosen.Name} keeps {field}, but {mapping.Name} kind {key} writes {written.GetValueOrDefault(member).ToString(CultureInfo.InvariantCulture)}.",
                        obj.Find(field)?.Value ?? caseName, path.Member(field));
        }
        return flat;
    }
}
