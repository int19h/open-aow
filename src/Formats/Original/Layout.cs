using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Aow.Formats.Json;

namespace Aow.Formats.Original;

/// <summary>
/// Encoding residue of one object (own-formats.md 2.5): the parts that the default original
/// encoding does not reproduce. The fingerprint binds the parts to the visible subtree, and the
/// encoder still reconstructs and verifies before using them.
/// </summary>
public static class LayoutResidue
{
    public const string Codec = "property-table-1";
    public const string TableFormOrder = "table-form-order";
    public const string Gaps = "gaps";
    public const string ValuePrefixes = "value-prefixes";
    public const string ValueEncoding = "value-encoding";
    public const string NestedCompression = "nested-compression";
    public const string HeaderExtensions = "header-extensions";

    /// <summary>
    /// SHA-256 over the profile, the codec and the subtree with every $layout removed and
    /// members in ordinal name order, so member order and whitespace do not matter.
    /// </summary>
    public static string Fingerprint(JsonValue value, string profile)
    {
        var builder = new StringBuilder();
        builder.Append(Codec).Append('\n').Append(profile).Append('\n');
        Append(builder, value);
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString())));
    }

    private static void Append(StringBuilder builder, JsonValue value)
    {
        switch (value)
        {
            case JsonObject obj:
                builder.Append('{');
                bool first = true;
                foreach (var m in obj.Members.Where(m => m.Name != "$layout").OrderBy(m => m.Name, StringComparer.Ordinal))
                {
                    if (!first) builder.Append(',');
                    first = false;
                    JsonWriter.WriteString(builder, m.Name);
                    builder.Append(':');
                    Append(builder, m.Value);
                }
                builder.Append('}');
                break;
            case JsonArray array:
                builder.Append('[');
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (i > 0) builder.Append(',');
                    Append(builder, array.Items[i]);
                }
                builder.Append(']');
                break;
            default:
                JsonWriter.WriteCompact(builder, value);
                break;
        }
    }

    public static string Hex(ReadOnlySpan<byte> bytes) => Convert.ToHexStringLower(bytes);
}

/// <summary>Collects the parts of one object while it is decoded.</summary>
internal sealed class LayoutBuilder
{
    public List<JsonObject> Parts { get; } = [];

    public void Table(IReadOnlyList<TableEntry> entries, bool emptyLongMarker, int? property)
    {
        var part = new JsonObject().Add("category", new JsonString(LayoutResidue.TableFormOrder));
        if (property is int p) part.Add("property", new JsonInteger(p));
        var list = new JsonArray();
        foreach (var e in entries)
            list.Add(new JsonArray().Add(new JsonInteger(e.Id)).Add(new JsonInteger(e.Offset)).Add(new JsonInteger(e.Long ? 1 : 0)));
        part.Add("entries", list);
        if (emptyLongMarker) part.Add("longMarker", new JsonBool(true));
        Parts.Add(part);
    }

    public void Gap(int offset, ReadOnlySpan<byte> bytes, int? property)
    {
        var part = new JsonObject().Add("category", new JsonString(LayoutResidue.Gaps));
        if (property is int p) part.Add("property", new JsonInteger(p));
        part.Add("offset", new JsonInteger(offset)).Add("bytes", new JsonString(LayoutResidue.Hex(bytes)));
        Parts.Add(part);
    }

    public void ValuePrefix(int id) =>
        Parts.Add(new JsonObject().Add("category", new JsonString(LayoutResidue.ValuePrefixes)).Add("id", new JsonInteger(id)));

    public void ValueEncoding(int id, ReadOnlySpan<byte> bytes) =>
        Parts.Add(new JsonObject().Add("category", new JsonString(LayoutResidue.ValueEncoding))
            .Add("id", new JsonInteger(id)).Add("bytes", new JsonString(LayoutResidue.Hex(bytes))));

    public void Header(int offset, ReadOnlySpan<byte> bytes) =>
        Parts.Add(new JsonObject().Add("category", new JsonString(LayoutResidue.HeaderExtensions))
            .Add("offset", new JsonInteger(offset)).Add("bytes", new JsonString(LayoutResidue.Hex(bytes))));

    public void Nested(ReadOnlySpan<byte> bytes) =>
        Parts.Add(new JsonObject().Add("category", new JsonString(LayoutResidue.NestedCompression))
            .Add("bytes", new JsonString(LayoutResidue.Hex(bytes))));

    /// <summary>Adds $layout after the reserved members already present.</summary>
    public void Attach(JsonObject target, string profile, LayoutCounts counts)
    {
        if (Parts.Count == 0) return;
        var parts = new JsonArray();
        foreach (var p in Parts)
        {
            parts.Add(p);
            counts.Add(((JsonString)p["category"]!).Value);
        }
        counts.Objects++;
        var layout = new JsonObject()
            .Add("codec", new JsonString(LayoutResidue.Codec))
            .Add("fingerprint", new JsonString(LayoutResidue.Fingerprint(target, profile)))
            .Add("parts", parts);
        int at = 0;
        while (at < target.Members.Count && target.Members[at].Name.StartsWith('$')) at++;
        target.Members.Insert(at, new JsonMember("$layout", layout));
    }
}

/// <summary>The validated parts of one object's $layout, read for export.</summary>
internal sealed class LayoutReader
{
    public List<(int? Property, List<TableEntry> Entries, bool LongMarker, JsonValue At)> Tables { get; } = [];
    public List<(int? Property, int Offset, byte[] Bytes, JsonValue At)> Gaps { get; } = [];
    public HashSet<int> Prefixes { get; } = [];
    public Dictionary<int, (byte[] Bytes, JsonValue At)> Encodings { get; } = [];
    public (byte[] Bytes, JsonValue At)? Nested { get; private set; }
    public List<(int Offset, byte[] Bytes, JsonValue At)> Headers { get; } = [];

    public (List<TableEntry> Entries, bool LongMarker, JsonValue At)? Table(int? property)
    {
        foreach (var t in Tables)
            if (t.Property == property) return (t.Entries, t.LongMarker, t.At);
        return null;
    }

    public IEnumerable<(int Offset, byte[] Bytes, JsonValue At)> GapsOf(int? property) =>
        Gaps.Where(g => g.Property == property).Select(g => (g.Offset, g.Bytes, g.At));

    /// <summary>
    /// Parses $layout and checks its fingerprint against the visible subtree. A stale layout
    /// returns null and is reported, or fails under --exact-layout. Malformed parts always fail.
    /// </summary>
    public static LayoutReader? Read(EncodeContext ctx, JsonObject owner, DocPath path)
    {
        var member = owner.Find("$layout");
        if (member is null) return null;
        var at = path.Member("$layout");
        if (member.Value is not JsonObject layout) throw ctx.Fail(Codes.Type, "$layout is not an object.", member.Value, at);
        string? fingerprint = null;
        JsonArray? parts = null;
        foreach (var m in layout.Members)
        {
            switch (m.Name)
            {
                case "codec":
                    if (m.Value is not JsonString { Value: LayoutResidue.Codec })
                        throw ctx.Fail(Codes.Layout, "$layout names an unknown codec.", m.Value, at.Member("codec"), null, LayoutResidue.Codec);
                    break;
                case "fingerprint":
                    fingerprint = m.Value is JsonString { Value.Length: 64 } s && IsHex(s.Value) ? s.Value
                        : throw ctx.Fail(Codes.Layout, "The $layout fingerprint is not 64 lowercase hexadecimal digits.", m.Value, at.Member("fingerprint"));
                    break;
                case "parts":
                    parts = m.Value as JsonArray ?? throw ctx.Fail(Codes.Type, "$layout parts is not an array.", m.Value, at.Member("parts"));
                    break;
                default:
                    throw ctx.Fail(Codes.UnknownMember, $"$layout has no member \"{m.Name}\".", m.Value, at.Member(m.Name));
            }
        }
        if (fingerprint is null || parts is null)
            throw ctx.Fail(Codes.MissingMember, "$layout needs codec, fingerprint and parts.", layout, at);
        var result = new LayoutReader();
        for (int i = 0; i < parts.Items.Count; i++) result.ReadPart(ctx, parts.Items[i], at.Member("parts").Item(i));
        string actual = LayoutResidue.Fingerprint(owner, ctx.Profile);
        if (actual != fingerprint)
        {
            var diagnostic = new FormatDiagnostic(Codes.Layout, "The retained layout no longer matches the edited content and is rebuilt.")
            {
                File = ctx.File,
                Span = layout.Span,
                Path = at.ToString(),
            };
            if (ctx.Options.ExactLayout)
                throw new ConversionException(diagnostic with { Message = "The retained exact layout is stale after an edit. --exact-layout refuses to rebuild it." });
            ctx.Invalidated.Add(diagnostic);
            return null;
        }
        return result;
    }

    private static bool IsHex(string s)
    {
        foreach (char c in s)
            if (c is not (>= '0' and <= '9' or >= 'a' and <= 'f')) return false;
        return s.Length % 2 == 0;
    }

    private static byte[] Bytes(EncodeContext ctx, JsonValue? value, DocPath path)
    {
        if (value is not JsonString s || !IsHex(s.Value))
            throw ctx.Fail(Codes.Layout, "A layout part needs bytes as lowercase hexadecimal digits of even length.", value, path);
        if (s.Value.Length / 2 > ctx.Limits.DecompressedBytes)
            throw ctx.Fail(Codes.Limit, "A layout part is too large.", value, path);
        return Convert.FromHexString(s.Value);
    }

    private static int Int(EncodeContext ctx, JsonValue? value, DocPath path, long min, long max)
    {
        if (value is not JsonInteger n || n.Value < min || n.Value > max)
            throw ctx.Fail(Codes.Layout, "A layout part has an integer out of bounds.", value, path,
                value is JsonInteger k ? k.Value.ToString(CultureInfo.InvariantCulture) : null, $"{min} to {max}");
        return (int)n.Value;
    }

    private void ReadPart(EncodeContext ctx, JsonValue item, DocPath path)
    {
        if (item is not JsonObject part) throw ctx.Fail(Codes.Type, "A layout part is not an object.", item, path);
        string category = part["category"] is JsonString c ? c.Value : throw ctx.Fail(Codes.MissingMember, "A layout part needs a category.", part, path);
        int? property = part.Find("property") is { } p ? Int(ctx, p.Value, path.Member("property"), 0, int.MaxValue) : null;
        string[] allowed;
        switch (category)
        {
            case LayoutResidue.TableFormOrder:
                {
                    allowed = ["category", "property", "entries", "longMarker"];
                    var list = part["entries"] as JsonArray ?? throw ctx.Fail(Codes.Layout, "A table part needs entries.", part, path);
                    var entries = new List<TableEntry>();
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        var e = list.Items[i] as JsonArray;
                        var ep = path.Member("entries").Item(i);
                        if (e is null || e.Items.Count != 3) throw ctx.Fail(Codes.Layout, "A table entry is [id, offset, long].", list.Items[i], ep);
                        entries.Add(new TableEntry(Int(ctx, e.Items[0], ep, int.MinValue, int.MaxValue), Int(ctx, e.Items[1], ep, 0, int.MaxValue),
                            Int(ctx, e.Items[2], ep, 0, 1) == 1, i));
                    }
                    bool marker = part["longMarker"] switch
                    {
                        null => false,
                        JsonBool { Value: true } => true,
                        var v => throw ctx.Fail(Codes.Layout, "longMarker is true or absent.", v, path.Member("longMarker")),
                    };
                    if (Tables.Any(t => t.Property == property)) throw ctx.Fail(Codes.Duplicate, "Two table parts describe the same table.", part, path);
                    Tables.Add((property, entries, marker, part));
                    break;
                }
            case LayoutResidue.Gaps:
                allowed = ["category", "property", "offset", "bytes"];
                Gaps.Add((property, Int(ctx, part["offset"], path.Member("offset"), 0, int.MaxValue), Bytes(ctx, part["bytes"], path.Member("bytes")), part));
                break;
            case LayoutResidue.ValuePrefixes:
                allowed = ["category", "id"];
                if (!Prefixes.Add(Int(ctx, part["id"], path.Member("id"), int.MinValue, int.MaxValue)))
                    throw ctx.Fail(Codes.Duplicate, "Two prefix parts name the same property.", part, path);
                break;
            case LayoutResidue.ValueEncoding:
                allowed = ["category", "id", "bytes"];
                if (!Encodings.TryAdd(Int(ctx, part["id"], path.Member("id"), int.MinValue, int.MaxValue), (Bytes(ctx, part["bytes"], path.Member("bytes")), part)))
                    throw ctx.Fail(Codes.Duplicate, "Two encoding parts name the same property.", part, path);
                break;
            case LayoutResidue.HeaderExtensions:
                allowed = ["category", "offset", "bytes"];
                Headers.Add((Int(ctx, part["offset"], path.Member("offset"), 0, int.MaxValue), Bytes(ctx, part["bytes"], path.Member("bytes")), part));
                break;
            case LayoutResidue.NestedCompression:
                allowed = ["category", "bytes"];
                if (Nested is not null) throw ctx.Fail(Codes.Duplicate, "Two nested compression parts.", part, path);
                Nested = (Bytes(ctx, part["bytes"], path.Member("bytes")), part);
                break;
            default:
                throw ctx.Fail(Codes.Layout, $"Unknown layout category \"{category}\".", part, path.Member("category"));
        }
        foreach (var m in part.Members)
            if (Array.IndexOf(allowed, m.Name) < 0)
                throw ctx.Fail(Codes.UnknownMember, $"A {category} part has no member \"{m.Name}\".", m.Value, path.Member(m.Name));
    }
}
