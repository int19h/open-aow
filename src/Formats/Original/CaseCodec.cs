using System.Buffers.Binary;
using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>
/// The document form of a mapped class (own-formats.md 2.3): `$case` names the case by its
/// registration key and the case fields hold the stored members the case keeps with their stored
/// presence. Unused stored members are constant 0 in 1.36: members an original reader acts on are
/// refused when nonzero on load, and the rest are ignored on load and omitted (0) on save. An
/// unknown key keeps the discriminator and every value in the unknown case.
/// </summary>
internal static class CaseCodec
{
    private static ConversionException Missing(SchemaClass classValue, SchemaMapping mapping, string? file, long offset, DocumentNodePath path) =>
        new(new FormatDiagnostic(Codes.Mapping, $"Class {classValue.Name} needs its unit mapping {mapping.Name}, which Aow.Game registers.")
        { File = file, Offset = offset, Path = path.ToString() });

    /// <summary>The number a stored discriminator or field holds: an integer, or little-endian record bytes.</summary>
    private static long Number(JsonValue value) => value switch
    {
        JsonInteger i => i.Value,
        JsonString { Value.Length: > 0 and <= 16 } s when s.Value.Length % 2 == 0 =>
            (long)BinaryPrimitives.ReadUInt64LittleEndian(Convert.FromHexString(s.Value).Concat(new byte[8]).Take(8).ToArray()),
        _ => 0,
    };

    private static long TreeNumber(TreeValue value) => value switch
    {
        IntegerNode i => i.Value,
        BytesNode { Data.Length: > 0 and <= 8 } b =>
            (long)BinaryPrimitives.ReadUInt64LittleEndian([.. b.Data, .. new byte[8 - b.Data.Length]]),
        _ => 0,
    };

    private static JsonValue Stored(SchemaClass classValue, string member, long key)
    {
        PropertyCodec codec = classValue.PropertyByName(member)!.Codec;
        if (codec is DataCodec { Size: int size })
        {
            byte[] bytes = new byte[8];
            BinaryPrimitives.WriteInt64LittleEndian(bytes, key);
            return new JsonString(LayoutResidue.Hex(bytes.AsSpan(0, size)));
        }
        return new JsonInteger(key);
    }

    private static void InsertReserved(JsonObject objectValue, JsonMember member)
    {
        int rank = Array.IndexOf(Reserved.Order, member.Name);
        int at = 0;
        while (at < objectValue.Members.Count && objectValue.Members[at].Name.StartsWith('$') && Array.IndexOf(Reserved.Order, objectValue.Members[at].Name) < rank)
        {
            at++;
        }

        objectValue.Members.Insert(at, member);
    }

    /// <summary>Turns the decoded stored members of a mapped object into its case.</summary>
    public static void ToDocument(DecodeContext context, SchemaClass classValue, SchemaMapping mapping, JsonObject target, DocumentNodePath path, int offset)
    {
        if (CaseMappings.Find(classValue.Name) is null)
        {
            throw Missing(classValue, mapping, context.File, offset, path);
        }

        Choose(mapping, target, (chosen, member, value) => context.Fail(Codes.Range,
            $"Case {chosen.Name} does not accept a nonzero {member}.", offset, path.Member(member), Number(value).ToString(CultureInfo.InvariantCulture), "0"));
    }

    /// <summary>The same case for a tree's stored members, which the direct document backend writes.</summary>
    public static void ToDocument(SchemaClass classValue, SchemaMapping mapping, JsonObject target, DocumentSite site)
    {
        if (CaseMappings.Find(classValue.Name) is null)
        {
            throw Missing(classValue, mapping, file: site.Context.File, offset: 0, site.Path);
        }

        Choose(mapping, target, (chosen, member, value) => site.At(site.Path.Member(member), element: false).Fail(Codes.Range,
            $"Case {chosen.Name} does not accept a nonzero {member}.", value: Number(value).ToString(CultureInfo.InvariantCulture), constraint: "0"));
    }

    /// <summary>Checks an original image read directly into a tree against the case's refused unused members.</summary>
    public static void CheckTree(DecodeContext context, SchemaMapping mapping, ObjectNode node, DocumentNodePath path, int offset)
    {
        if (node.Find(mapping.Discriminator) is not { } discriminator)
        {
            return;
        }

        long key = TreeNumber(discriminator);
        SchemaCase? chosen = mapping.Cases.FirstOrDefault(c => c.Key == key);
        if (chosen is null || chosen.Refused.Count == 0)
        {
            return;
        }

        foreach (string member in chosen.Refused)
        {
            if (node.Find(member) is { } value && TreeNumber(value) != 0)
            {
                throw context.Fail(Codes.Range, $"Case {chosen.Name} does not accept a nonzero {member}.", offset, path.Member(member), TreeNumber(value).ToString(CultureInfo.InvariantCulture), "0");
            }
        }
    }

    private static void Choose(SchemaMapping mapping, JsonObject target, Func<SchemaCase, string, JsonValue, ConversionException> refuse)
    {
        Dictionary<string, JsonValue> stored = [with(StringComparer.Ordinal)];
        int first = -1;
        for (int i = 0; i < target.Members.Count;)
        {
            JsonMember m = target.Members[i];
            if (mapping.StoredMembers.Contains(m.Name))
            {
                if (first < 0)
                {
                    first = i;
                }

                stored[m.Name] = m.Value;
                target.Members.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        if (first < 0)
        {
            first = target.Members.Count;
        }

        SchemaCase chosen = mapping.Unknown;
        if (stored.TryGetValue(mapping.Discriminator, out JsonValue? discriminator))
        {
            long key = Number(discriminator);
            chosen = mapping.Cases.FirstOrDefault(c => c.Key == key) ?? mapping.Unknown;
        }
        List<JsonMember> fields = [];
        HashSet<string> used = [with(StringComparer.Ordinal)];
        if (chosen.Key is not null)
        {
            _ = used.Add(mapping.Discriminator);
        }

        foreach ((string? field, string? member) in chosen.Fields)
        {
            _ = used.Add(member);
            if (stored.TryGetValue(member, out JsonValue? value))
            {
                fields.Add(new JsonMember(field, value));
            }
        }
        foreach (string member in mapping.StoredMembers)
        {
            if (!used.Contains(member) && stored.TryGetValue(member, out JsonValue? value) && chosen.Refused.Contains(member) && Number(value) != 0)
            {
                throw refuse(chosen, member, value);
            }
        }

        int before = target.Members.Count;
        InsertReserved(target, new JsonMember("$case", new JsonString(chosen.Name)));

        first += target.Members.Count - before;
        target.Members.InsertRange(Math.Min(first, target.Members.Count), fields);
    }

    /// <summary>
    /// The flat object the schema encodes for a mapped object. The case fields go back to their
    /// stored members with their presence and the discriminator gets the case's key. The kind must
    /// keep every case field: the unit side's ToFields writes it back unchanged.
    /// </summary>
    public static JsonObject FromDocument(EncodeContext context, SchemaClass classValue, SchemaMapping mapping, JsonObject objectValue, DocumentNodePath path)
    {
        ICaseUnit unit = CaseMappings.Find(classValue.Name)
            ?? throw context.Fail(Codes.Mapping, $"Class {classValue.Name} needs its unit mapping {mapping.Name}, which Aow.Game registers.", objectValue, path);
        if (objectValue["$case"] is not JsonString caseName)
        {
            throw context.Fail(objectValue["$case"] is null ? Codes.MissingMember : Codes.Type, $"A {classValue.Name} names its {mapping.Name} case in $case.", objectValue["$case"] ?? objectValue, path.Member("$case"));
        }

        SchemaCase chosen = mapping.Cases.FirstOrDefault(c => c.Name == caseName.Value) ?? (mapping.Unknown.Name == caseName.Value ? mapping.Unknown : null)
            ?? throw context.Fail(Codes.Case, $"{mapping.Name} has no case {caseName.Value}.", caseName, path.Member("$case"), caseName.Value,
                string.Join(", ", mapping.Cases.Select(c => c.Name).Append(mapping.Unknown.Name)));
        // The unknown case keeps only keys no case registers, so its export reads back the same.
        string keyField = mapping.Unknown.Fields.First(f => f.Member == mapping.Discriminator).Field;
        if (chosen.Key is null && objectValue[keyField] is JsonValue kind && mapping.Cases.FirstOrDefault(c => c.Key == Number(kind)) is { } known)
        {
            throw context.Fail(Codes.Case, $"Key {Number(kind).ToString(CultureInfo.InvariantCulture)} has the case {known.Name}, not {chosen.Name}.",
                kind, path.Member(keyField), Number(kind).ToString(CultureInfo.InvariantCulture), known.Name);
        }

        JsonObject flat = new() { Span = objectValue.Span };
        Dictionary<string, string> fieldMembers = chosen.Fields.ToDictionary(f => f.Field, f => f.Member, StringComparer.Ordinal);
        Dictionary<string, long> numbers = [with(StringComparer.Ordinal)];
        foreach (JsonMember m in objectValue.Members)
        {
            if (m.Name == "$case")
            {
                continue;
            }

            if (fieldMembers.TryGetValue(m.Name, out string? member))
            {
                flat.Members.Add(new JsonMember(member, m.Value) { NameSpan = m.NameSpan });
                numbers[member] = Number(m.Value);
                continue;
            }
            if (mapping.StoredMembers.Contains(m.Name))
            {
                throw context.Fail(Codes.UnknownMember, $"Case {chosen.Name} has no field \"{m.Name}\".", m.Value, path.Member(m.Name), m.Name);
            }

            flat.Members.Add(m);
        }
        if (chosen.Key is long key)
        {
            flat.Members.Add(new JsonMember(mapping.Discriminator, Stored(classValue, mapping.Discriminator, key)) { NameSpan = caseName.Span });
            numbers[mapping.Discriminator] = key;
            IReadOnlyDictionary<string, long> written = unit.Written(key, numbers);
            foreach ((string? field, string? member) in chosen.Fields)
            {
                if (written.GetValueOrDefault(member) != numbers.GetValueOrDefault(member))
                {
                    throw context.Fail(Codes.Mapping, $"Case {chosen.Name} keeps {field}, but {mapping.Name} kind {key} writes {written.GetValueOrDefault(member).ToString(CultureInfo.InvariantCulture)}.",
                        objectValue.Find(field)?.Value ?? caseName, path.Member(field));
                }
            }
        }
        return flat;
    }
}
