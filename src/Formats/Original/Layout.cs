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
        StringBuilder builder = new();
        _ = builder.Append(Codec).Append('\n').Append(profile).Append('\n');
        Append(builder, value);
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString())));
    }

    private static void Append(StringBuilder builder, JsonValue value)
    {
        switch (value)
        {
            case JsonObject objectValue:
                _ = builder.Append('{');
                bool first = true;
                foreach (JsonMember? m in objectValue.Members.Where(m => m.Name != "$layout").OrderBy(m => m.Name, StringComparer.Ordinal))
                {
                    if (!first)
                    {
                        _ = builder.Append(',');
                    }

                    first = false;
                    JsonWriter.WriteString(builder, m.Name);
                    _ = builder.Append(':');
                    Append(builder, m.Value);
                }
                _ = builder.Append('}');
                break;
            case JsonArray array:
                _ = builder.Append('[');
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (i > 0)
                    {
                        _ = builder.Append(',');
                    }

                    Append(builder, array.Items[i]);
                }
                _ = builder.Append(']');
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
        JsonObject part = new JsonObject().Add("category", new JsonString(LayoutResidue.TableFormOrder));
        if (property is int p)
        {
            _ = part.Add("property", new JsonInteger(p));
        }

        JsonArray list = new();
        foreach (TableEntry e in entries)
        {
            _ = list.Add(new JsonArray().Add(new JsonInteger(e.Id)).Add(new JsonInteger(e.Offset)).Add(new JsonInteger(e.Long ? 1 : 0)));
        }

        _ = part.Add("entries", list);
        if (emptyLongMarker)
        {
            _ = part.Add("longMarker", new JsonBoolean(true));
        }

        Parts.Add(part);
    }

    public void Gap(int offset, ReadOnlySpan<byte> bytes, int? property)
    {
        JsonObject part = new JsonObject().Add("category", new JsonString(LayoutResidue.Gaps));
        if (property is int p)
        {
            _ = part.Add("property", new JsonInteger(p));
        }

        _ = part.Add("offset", new JsonInteger(offset)).Add("bytes", new JsonString(LayoutResidue.Hex(bytes)));
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
        if (Parts.Count == 0)
        {
            return;
        }

        JsonArray parts = new();
        foreach (JsonObject p in Parts)
        {
            _ = parts.Add(p);
            counts.Add(((JsonString)p["category"]!).Value);
        }
        counts.Objects++;
        JsonObject layout = new JsonObject()
            .Add("codec", new JsonString(LayoutResidue.Codec))
            .Add("fingerprint", new JsonString(LayoutResidue.Fingerprint(target, profile)))
            .Add("parts", parts);
        int at = 0;
        while (at < target.Members.Count && target.Members[at].Name.StartsWith('$'))
        {
            at++;
        }

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
        foreach ((int? tableProperty, List<TableEntry> entries, bool longMarker, JsonValue at) in Tables)
        {
            if (tableProperty == property)
            {
                return (entries, longMarker, at);
            }
        }

        return null;
    }

    public IEnumerable<(int Offset, byte[] Bytes, JsonValue At)> GapsOf(int? property) =>
        Gaps.Where(g => g.Property == property).Select(g => (g.Offset, g.Bytes, g.At));

    /// <summary>
    /// Parses $layout and checks its fingerprint against the visible subtree. A stale layout
    /// returns null and is reported, or fails under --exact-layout. Malformed parts always fail.
    /// </summary>
    public static LayoutReader? Read(EncodeContext context, JsonObject owner, DocumentNodePath path)
    {
        JsonMember? member = owner.Find("$layout");
        if (member is null)
        {
            return null;
        }

        DocumentNodePath at = path.Member("$layout");
        if (member.Value is not JsonObject layout)
        {
            throw context.Fail(Codes.Type, "$layout is not an object.", member.Value, at);
        }

        string? fingerprint = null;
        JsonArray? parts = null;
        foreach (JsonMember m in layout.Members)
        {
            switch (m.Name)
            {
                case "codec":
                    if (m.Value is not JsonString { Value: LayoutResidue.Codec })
                    {
                        throw context.Fail(Codes.Layout, "$layout names an unknown codec.", m.Value, at.Member("codec"), null, LayoutResidue.Codec);
                    }

                    break;
                case "fingerprint":
                    fingerprint = m.Value is JsonString { Value.Length: 64 } s && IsHex(s.Value) ? s.Value
                        : throw context.Fail(Codes.Layout, "The $layout fingerprint is not 64 lowercase hexadecimal digits.", m.Value, at.Member("fingerprint"));
                    break;
                case "parts":
                    parts = m.Value as JsonArray ?? throw context.Fail(Codes.Type, "$layout parts is not an array.", m.Value, at.Member("parts"));
                    break;
                default:
                    throw context.Fail(Codes.UnknownMember, $"$layout has no member \"{m.Name}\".", m.Value, at.Member(m.Name));
            }
        }
        if (fingerprint is null || parts is null)
        {
            throw context.Fail(Codes.MissingMember, "$layout needs codec, fingerprint and parts.", layout, at);
        }

        LayoutReader result = new();
        for (int i = 0; i < parts.Items.Count; i++)
        {
            result.ReadPart(context, parts.Items[i], at.Member("parts").Item(i));
        }

        string actual = LayoutResidue.Fingerprint(owner, context.Profile);
        if (actual != fingerprint)
        {
            FormatDiagnostic diagnostic = new(Codes.Layout, "The retained layout no longer matches the edited content and is rebuilt.")
            {
                File = context.File,
                Span = layout.Span,
                Path = at.ToString(),
            };
            if (context.Options.ExactLayout)
            {
                throw new ConversionException(diagnostic with { Message = "The retained exact layout is stale after an edit. --exact-layout refuses to rebuild it." });
            }

            context.Invalidated.Add(diagnostic);
            return null;
        }
        return result;
    }

    private static bool IsHex(string s)
    {
        foreach (char c in s)
        {
            if (c is not ((>= '0' and <= '9') or (>= 'a' and <= 'f')))
            {
                return false;
            }
        }

        return s.Length % 2 == 0;
    }

    private static byte[] Bytes(EncodeContext context, JsonValue? value, DocumentNodePath path)
    {
        return value is not JsonString s || !IsHex(s.Value)
            ? throw context.Fail(Codes.Layout, "A layout part needs bytes as lowercase hexadecimal digits of even length.", value, path)
            : s.Value.Length / 2 > context.Limits.DecompressedBytes
            ? throw context.Fail(Codes.Limit, "A layout part is too large.", value, path)
            : Convert.FromHexString(s.Value);
    }

    private static int Int32(EncodeContext context, JsonValue? value, DocumentNodePath path, long min, long max)
    {
        return value is not JsonInteger n || n.Value < min || n.Value > max
            ? throw context.Fail(Codes.Layout, "A layout part has an integer out of bounds.", value, path,
                value is JsonInteger k ? k.Value.ToString(CultureInfo.InvariantCulture) : null, $"{min} to {max}")
            : (int)n.Value;
    }

    private void ReadPart(EncodeContext context, JsonValue item, DocumentNodePath path)
    {
        if (item is not JsonObject part)
        {
            throw context.Fail(Codes.Type, "A layout part is not an object.", item, path);
        }

        string category = part["category"] is JsonString c ? c.Value : throw context.Fail(Codes.MissingMember, "A layout part needs a category.", part, path);
        int? property = part.Find("property") is { } p ? Int32(context, p.Value, path.Member("property"), 0, int.MaxValue) : null;
        string[] allowed;
        switch (category)
        {
            case LayoutResidue.TableFormOrder:
                {
                    allowed = ["category", "property", "entries", "longMarker"];
                    JsonArray list = part["entries"] as JsonArray ?? throw context.Fail(Codes.Layout, "A table part needs entries.", part, path);
                    List<TableEntry> entries = [];
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        DocumentNodePath ep = path.Member("entries").Item(i);
                        if (list.Items[i] is not JsonArray e || e.Items.Count != 3)
                        {
                            throw context.Fail(Codes.Layout, "A table entry is [id, offset, long].", list.Items[i], ep);
                        }

                        entries.Add(new TableEntry(Int32(context, e.Items[0], ep, int.MinValue, int.MaxValue), Int32(context, e.Items[1], ep, 0, int.MaxValue),
                            Int32(context, e.Items[2], ep, 0, 1) == 1, i));
                    }
                    bool marker = part["longMarker"] switch
                    {
                        null => false,
                        JsonBoolean { Value: true } => true,
                        var v => throw context.Fail(Codes.Layout, "longMarker is true or absent.", v, path.Member("longMarker")),
                    };
                    if (Tables.Any(t => t.Property == property))
                    {
                        throw context.Fail(Codes.Duplicate, "Two table parts describe the same table.", part, path);
                    }

                    Tables.Add((property, entries, marker, part));
                    break;
                }
            case LayoutResidue.Gaps:
                allowed = ["category", "property", "offset", "bytes"];
                Gaps.Add((property, Int32(context, part["offset"], path.Member("offset"), 0, int.MaxValue), Bytes(context, part["bytes"], path.Member("bytes")), part));
                break;
            case LayoutResidue.ValuePrefixes:
                allowed = ["category", "id"];
                if (!Prefixes.Add(Int32(context, part["id"], path.Member("id"), int.MinValue, int.MaxValue)))
                {
                    throw context.Fail(Codes.Duplicate, "Two prefix parts name the same property.", part, path);
                }

                break;
            case LayoutResidue.ValueEncoding:
                allowed = ["category", "id", "bytes"];
                if (!Encodings.TryAdd(Int32(context, part["id"], path.Member("id"), int.MinValue, int.MaxValue), (Bytes(context, part["bytes"], path.Member("bytes")), part)))
                {
                    throw context.Fail(Codes.Duplicate, "Two encoding parts name the same property.", part, path);
                }

                break;
            case LayoutResidue.HeaderExtensions:
                allowed = ["category", "offset", "bytes"];
                Headers.Add((Int32(context, part["offset"], path.Member("offset"), 0, int.MaxValue), Bytes(context, part["bytes"], path.Member("bytes")), part));
                break;
            case LayoutResidue.NestedCompression:
                allowed = ["category", "bytes"];
                if (Nested is not null)
                {
                    throw context.Fail(Codes.Duplicate, "Two nested compression parts.", part, path);
                }

                Nested = (Bytes(context, part["bytes"], path.Member("bytes")), part);
                break;
            default:
                throw context.Fail(Codes.Layout, $"Unknown layout category \"{category}\".", part, path.Member("category"));
        }
        foreach (JsonMember m in part.Members)
        {
            if (Array.IndexOf(allowed, m.Name) < 0)
            {
                throw context.Fail(Codes.UnknownMember, $"A {category} part has no member \"{m.Name}\".", m.Value, path.Member(m.Name));
            }
        }
    }
}
