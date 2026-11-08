using System.Buffers.Binary;
using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Rtl;

namespace Aow.Formats.Original;

/// <summary>
/// The codec of one property kind (own-formats.md 2.3). Each kind maps stored bytes to a
/// document value and back through its own virtual methods, so new kinds join the family
/// without a switch in the object codec.
/// </summary>
public abstract partial class PropertyCodec
{
    /// <summary>A codec whose presence is its value stores no bytes.</summary>
    public virtual bool ZeroSize => false;

    /// <summary>A generic list is stored when the schema declares it. A fixed integer array is stored from version 2.</summary>
    internal virtual bool AcceptsList(int version) => false;

    /// <summary>The document value is a JSON array for this codec at this version.</summary>
    internal virtual bool AcceptsDocumentArray(int version) => AcceptsList(version);

    internal static PropertyCodec Create(JsonObject source, FormatSchema schema, string path)
    {
        string codec = ((JsonString)source["codec"]!).Value;
        int? size = source["size"] is JsonInteger s ? (int)s.Value : null;
        string? cls = (source["class"] as JsonString)?.Value;
        string? family = (source["family"] as JsonString)?.Value;
        return codec switch
        {
            "i32" => new IntegerCodec(4, true, (source["domain"] as JsonString)?.Value),
            "u8" => new IntegerCodec(1, false),
            "u16" => new IntegerCodec(2, false),
            "i8" => new IntegerCodec(1, true),
            "bool" => new BoolCodec(),
            "string" => new StringCodec(),
            "strings" => new StringsCodec(),
            "strings-ex" => new StringsExCodec(),
            "data" => new DataCodec(size),
            "list" => new ListCodec(Create((JsonObject)source["element"]!, schema, path + ".element"), schema.StoresLists),
            "record" => new RecordCodec(source, schema, path),
            "float32" => new FloatCodec(4),
            "float64" => new FloatCodec(8),
            "int-list" => new RunListCodec(4),
            "byte-list" => new RunListCodec(1),
            "bit-list" => new BitListCodec(),
            "stream" => new DataCodec(null),
            "object" => new FixedObjectCodec(cls ?? throw new InvalidOperationException("An object codec needs a class.")),
            "created" => new CreatedObjectCodec(family),
            "children" => new ChildrenCodec(),
            "map-fields" => new MapFieldsCodec(),
            "campaign-map" => new CampaignMapCodec(),
            "enum" => new EnumCodec(TypeName(source, "enum"), CheckedWidth(source, path, "enum"), Signed(source)),
            "set" => new SetCodec(TypeName(source, "set"), CheckedWidth(source, path, "set")),
            "int-array" => new FixedArrayCodec(CheckedWidth(source, path, "int-array"), Signed(source), Count(source)),
            _ => throw new InvalidOperationException($"Unknown codec {codec}."),
        };
    }

    private static string TypeName(JsonObject source, string key) =>
        (source[key] as JsonString)?.Value ?? throw new InvalidOperationException("A " + key + " codec names its C# type.");

    private static int Width(JsonObject source) =>
        source["width"] is JsonInteger width ? (int)width.Value : throw new InvalidOperationException("The codec needs a width.");

    private static int CheckedWidth(JsonObject source, string path, string kind)
    {
        int width = Width(source);
        if (width is 1 or 2 or 4) return width;
        throw new InvalidOperationException(path + " has width " + width + ". An " + kind + " width is 1, 2 or 4.");
    }

    private static bool Signed(JsonObject source) => source["signed"] is JsonBool { Value: true };

    private static int Count(JsonObject source) =>
        source["count"] is JsonInteger count ? (int)count.Value : throw new InvalidOperationException("An integer array needs a count.");

    /// <summary>Decodes one stored value that occupies data[start..end) exactly.</summary>
    internal abstract JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site);

    /// <summary>Encodes one value into the data buffer.</summary>
    internal abstract void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site);

    /// <summary>Encodes a member. A member stores no entry, one entry or several.</summary>
    internal virtual void EncodeMember(EncodeContext ctx, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        writer.Begin(site.Id, site.Property);
        Encode(ctx, value, writer.Data, site);
        writer.End();
    }

    internal virtual JsonValue DecodeElement(DecodeContext ctx, int start, int end, ValueSite site) =>
        throw new InvalidOperationException($"{GetType().Name} has no dynamic elements.");

    internal virtual int ElementId(EncodeContext ctx, JsonValue value, SchemaProperty property, int index, DocPath path) =>
        throw new InvalidOperationException($"{GetType().Name} has no dynamic elements.");

    internal virtual void EncodeElement(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site) => Encode(ctx, value, data, site);

    /// <summary>Writes a tree value as its document value. Null means the member is omitted.</summary>
    internal virtual JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site) =>
        throw site.Mismatch($"{site.Property?.Name ?? "The value"} does not store this value.");

    /// <summary>Reads a document value into a tree value. Null means the member is absent.</summary>
    internal virtual Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site) =>
        throw site.Mismatch($"{site.Property?.Name ?? "The value"} does not store this value.", value);

    internal virtual JsonValue DecodePair(DecodeContext ctx, (int Start, int End)? first, (int Start, int End)? second, ValueSite site) =>
        throw new InvalidOperationException($"{GetType().Name} stores one entry.");

    private protected static ConversionException Mismatch(DecodeContext ctx, int start, ValueSite site, string message) =>
        ctx.Fail(Codes.Corrupt, message, start, site.Path);

    protected static string Text(long value) => value.ToString(CultureInfo.InvariantCulture);
}

internal sealed partial class IntegerCodec(int width, bool signed, string? domain = null) : PropertyCodec
{
    private long Min => signed ? -(1L << (width * 8 - 1)) : 0;
    private long Max => signed ? (1L << (width * 8 - 1)) - 1 : (1L << (width * 8)) - 1;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        if (end - start != width)
            throw Mismatch(ctx, start, site, $"An integer of {width} bytes is stored in {end - start} bytes.");
        var bytes = ctx.Data.AsSpan(start, width);
        long value = width switch
        {
            1 => signed ? (sbyte)bytes[0] : bytes[0],
            2 => signed ? BinaryPrimitives.ReadInt16LittleEndian(bytes) : BinaryPrimitives.ReadUInt16LittleEndian(bytes),
            _ => BinaryPrimitives.ReadInt32LittleEndian(bytes),
        };
        return new JsonInteger(value);
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonInteger n) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is an integer.", value, site.Path, value.KindName);
        if (n.Value < Min || n.Value > Max)
            throw ctx.Fail(Codes.Range, $"{site.Property.Name} is outside its storage range.", value, site.Path, Text(n.Value), $"{Min} to {Max}");
        switch (width)
        {
            case 1: data.Byte((byte)n.Value); break;
            case 2: data.Int16((ushort)n.Value); break;
            default: data.Int32((int)n.Value); break;
        }
    }
}

/// <summary>rwBool: true when present. Explicit false normalizes to absence.</summary>
internal sealed partial class BoolCodec : PropertyCodec
{
    public override bool ZeroSize => true;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site) => new JsonBool(true);

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site) { }

    internal override void EncodeMember(EncodeContext ctx, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        if (value is not JsonBool b) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is a boolean.", value, site.Path, value.KindName);
        if (!b.Value) return;
        writer.Begin(site.Id, site.Property);
        writer.End();
    }
}

/// <summary>Text through the frozen code page table. Strict input refuses unmapped characters.</summary>
internal static class TextCodec
{
    public static string Decode(ReadOnlySpan<byte> bytes) => Cp1252.Decode(bytes);

    public static byte[] Encode(EncodeContext ctx, string text, JsonValue at, DocPath path)
    {
        if (Problem(text, ctx.Limits.StringBytes) is { } problem)
            throw ctx.Fail(problem.Code, problem.Message, at, path, problem.Value, problem.Constraint);
        return Cp1252.Encode(text);
    }

    /// <summary>The code page and length check shared by the converter and the document backend.</summary>
    internal readonly record struct TextProblem(string Code, string Message, string? Value, string? Constraint);

    internal static TextProblem? Problem(string text, int limit)
    {
        for (int i = 0; i < text.Length; i++)
            if (!Cp1252.IsMapped(text[i]))
                return new TextProblem(Codes.Text, $"Character U+{(int)text[i]:X4} at index {i} has no code page 1252 byte.",
                    ((int)text[i]).ToString("X4", CultureInfo.InvariantCulture), "code page 1252 characters");
        if (text.Length > limit)
            return new TextProblem(Codes.Limit, "A string is too long.", null, $"at most {limit} bytes");
        return null;
    }
}

/// <summary>rwString: a one-byte length, or $FF and a 32-bit length, then the bytes.</summary>
internal sealed partial class StringCodec : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        ctx.Need(start, 1, end, site.Path, "string length");
        int at = start + 1;
        int length = ctx.Data[start];
        bool longPrefix = length == 255;
        if (longPrefix)
        {
            ctx.Need(at, 4, end, site.Path, "string length");
            length = ctx.Int32(at);
            at += 4;
        }
        if (length < 0 || at + (long)length != end)
            throw Mismatch(ctx, start, site, $"A string of {length} bytes does not fill its {end - start} stored bytes.");
        if (length > ctx.Limits.StringBytes)
            throw ctx.Fail(Codes.Limit, "A string is too long.", start, site.Path, Text(length), $"at most {ctx.Limits.StringBytes} bytes");
        if (longPrefix && length < 255) site.Builder?.ValuePrefix(site.Id);
        return new JsonString(TextCodec.Decode(ctx.Data.AsSpan(at, length)));
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonString s) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is a string.", value, site.Path, value.KindName);
        var bytes = TextCodec.Encode(ctx, s.Value, value, site.Path);
        if (bytes.Length >= 255 || site.Reader?.Prefixes.Contains(site.Id) == true)
        {
            data.Byte(255);
            data.Int32(bytes.Length);
        }
        else data.Byte((byte)bytes.Length);
        data.Bytes(bytes);
    }
}

/// <summary>
/// rwStrings: a 32-bit byte length and the TStrings text. An array of lines when every line
/// ends in CR LF and holds no CR, LF or NUL, otherwise { "$text": ... }.
/// </summary>
internal partial class StringsCodec : PropertyCodec
{
    protected static (string Text, int End) Block(DecodeContext ctx, int start, int end, ValueSite site)
    {
        ctx.Need(start, 4, end, site.Path, "text length");
        int length = ctx.Int32(start);
        if (length < 0 || start + 4L + length > end) throw Mismatch(ctx, start, site, $"A text block of {length} bytes does not fit its value.");
        if (length > ctx.Limits.StringBytes)
            throw ctx.Fail(Codes.Limit, "A text block is too long.", start, site.Path, Text(length), $"at most {ctx.Limits.StringBytes} bytes");
        return (TextCodec.Decode(ctx.Data.AsSpan(start + 4, length)), start + 4 + length);
    }

    internal static List<string>? Lines(string text)
    {
        if (text.Length == 0) return [];
        if (!text.EndsWith("\r\n", StringComparison.Ordinal)) return null;
        var lines = text[..^2].Split("\r\n");
        foreach (var line in lines)
            if (line.AsSpan().IndexOfAny('\r', '\n', '\0') >= 0) return null;
        return [.. lines];
    }

    /// <summary>The line count of TStrings.SetTextStr: CR, LF or CR LF end a line, NUL ends the text.</summary>
    internal static int LineCount(string text)
    {
        int count = 0, i = 0;
        while (i < text.Length && text[i] != '\0')
        {
            while (i < text.Length && text[i] is not ('\0' or '\r' or '\n')) i++;
            count++;
            if (i < text.Length && text[i] == '\r') i++;
            if (i < text.Length && text[i] == '\n') i++;
        }
        return count;
    }

    internal static JsonValue TextValue(string text)
    {
        var lines = Lines(text);
        if (lines is null) return new JsonObject().Add("$text", new JsonString(text));
        var array = new JsonArray();
        foreach (var line in lines) array.Add(new JsonString(line));
        return array;
    }

    internal static string TextOf(EncodeContext ctx, JsonValue value, DocPath path, out int lines)
    {
        if (value is JsonArray array)
        {
            var parts = new string[array.Items.Count];
            for (int i = 0; i < array.Items.Count; i++)
            {
                if (array.Items[i] is not JsonString line) throw ctx.Fail(Codes.Type, "A line is a string.", array.Items[i], path.Item(i));
                if (line.Value.AsSpan().IndexOfAny('\r', '\n', '\0') >= 0)
                    throw ctx.Fail(Codes.Text, "A line holds CR, LF or NUL. Use { \"$text\": ... } for such text.", line, path.Item(i));
                parts[i] = line.Value;
            }
            lines = array.Items.Count;
            return Join(parts);
        }
        if (value is JsonObject obj && obj["$text"] is JsonString text)
        {
            lines = LineCount(text.Value);
            return text.Value;
        }
        throw ctx.Fail(Codes.Type, "Text is an array of lines or { \"$text\": ... }.", value, path, value.KindName);
    }

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var (text, after) = Block(ctx, start, end, site);
        if (after != end) throw Mismatch(ctx, start, site, "Bytes follow the text block.");
        return TextValue(text);
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is JsonObject obj) CheckOnly(ctx, obj, site.Path, "$text");
        var bytes = TextCodec.Encode(ctx, TextOf(ctx, value, site.Path, out _), value, site.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
    }

    protected static void CheckOnly(EncodeContext ctx, JsonObject obj, DocPath path, params string[] names)
    {
        foreach (var m in obj.Members)
            if (Array.IndexOf(names, m.Name) < 0)
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    $"Member \"{m.Name}\" is not allowed here.")
                { File = ctx.File, Span = m.NameSpan, Path = path.Member(m.Name).ToString() });
    }
}

/// <summary>rwStringsEx: the text block, then one signed 32-bit object value per line.</summary>
internal sealed partial class StringsExCodec : StringsCodec
{
    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var (text, after) = Block(ctx, start, end, site);
        if ((end - after) % 4 != 0) throw Mismatch(ctx, start, site, "The object values are not whole 32-bit integers.");
        int count = (end - after) / 4;
        if (count != LineCount(text)) throw Mismatch(ctx, start, site, $"The text has {LineCount(text)} lines and {count} object values.");
        var objects = new JsonArray();
        for (int i = 0; i < count; i++) objects.Add(new JsonInteger(ctx.Int32(after + 4 * i)));
        var lines = Lines(text);
        if (lines is null) return new JsonObject().Add("$text", new JsonString(text)).Add("objects", objects);
        var array = new JsonArray();
        foreach (var line in lines) array.Add(new JsonString(line));
        return new JsonObject().Add("lines", array).Add("objects", objects);
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is an object with lines or $text, and objects.", value, site.Path, value.KindName);
        var (text, ids) = ReadMembers(ctx, obj, site.Path);
        var bytes = TextCodec.Encode(ctx, text, value, site.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
        foreach (var id in ids) data.Int32(id);
    }

    internal static (string Text, List<int> Ids) ReadMembers(EncodeContext ctx, JsonObject obj, DocPath path)
    {
        bool hasText = obj.Find("$text") is not null;
        CheckOnly(ctx, obj, path, hasText ? "$text" : "lines", "objects");
        var textValue = hasText ? obj : obj["lines"] ?? throw ctx.Fail(Codes.MissingMember, "lines or $text is required.", obj, path);
        string text = TextOf(ctx, textValue, path.Member(hasText ? "$text" : "lines"), out int lines);
        var objects = obj["objects"] as JsonArray ?? throw ctx.Fail(Codes.MissingMember, "objects is an array of integers.", obj, path);
        if (objects.Items.Count != lines)
            throw ctx.Fail(Codes.Corrupt, $"The text has {lines} lines and {objects.Items.Count} object values.", objects, path.Member("objects"));
        var ids = new List<int>(objects.Items.Count);
        for (int i = 0; i < objects.Items.Count; i++)
        {
            if (objects.Items[i] is not JsonInteger n || n.Value < int.MinValue || n.Value > int.MaxValue)
                throw ctx.Fail(Codes.Range, "An object value is a signed 32-bit integer.", objects.Items[i], path.Member("objects").Item(i));
            ids.Add((int)n.Value);
        }
        return (text, ids);
    }
}

/// <summary>rwData and opaque callback payloads: exact lowercase hexadecimal bytes.</summary>
internal sealed partial class DataCodec(int? size) : PropertyCodec
{
    /// <summary>The fixed record size, or null.</summary>
    internal int? Size => size;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        if (size is int n && end - start != n) throw Mismatch(ctx, start, site, $"A record of {n} bytes is stored in {end - start} bytes.");
        return new JsonString(LayoutResidue.Hex(ctx.Data.AsSpan(start, end - start)));
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var bytes = HexValue(ctx, value, site);
        if (size is int n && bytes.Length != n)
            throw ctx.Fail(Codes.Range, $"{site.Property.Name} holds exactly {n} bytes.", value, site.Path, Text(bytes.Length), $"{n} bytes");
        data.Bytes(bytes);
    }

    internal static byte[] HexValue(EncodeContext ctx, JsonValue value, ValueSite site)
    {
        if (value is not JsonString s || !HexText.IsLowerHex(s.Value))
            throw ctx.Fail(Codes.Type, $"{site.Property.Name} is lowercase hexadecimal bytes.", value, site.Path, value.KindName);
        if (s.Value.Length / 2 > ctx.Limits.DecompressedBytes) throw ctx.Fail(Codes.Limit, "A payload is too large.", value, site.Path);
        return HexText.Decode(s.Value);
    }
}

/// <summary>Floating-point fields keep their exact little-endian bits.</summary>
internal sealed class FloatCodec(int width) : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        if (end - start != width) throw Mismatch(ctx, start, site, $"A {width * 8}-bit float is stored in {end - start} bytes.");
        return new JsonObject().Add("$bits", new JsonString(LayoutResidue.Hex(ctx.Data.AsSpan(start, width))));
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject obj || obj.Members.Count != 1 || obj["$bits"] is not JsonString)
            throw ctx.Fail(Codes.Type, $"{site.Property.Name} is {{ \"$bits\": ... }}.", value, site.Path, value.KindName);
        var bytes = DataCodec.HexValue(ctx, obj["$bits"]!, site);
        if (bytes.Length != width) throw ctx.Fail(Codes.Range, $"$bits holds {width} bytes.", obj["$bits"], site.Path.Member("$bits"));
        data.Bytes(bytes);
    }
}

/// <summary>
/// TIntegerList and TByteList streams: the default value, the 32-bit count, then the values
/// where a default value is followed by a run length. The document holds the default and the
/// decoded items. A run split the writer does not make is kept as a value-encoding part.
/// </summary>
internal sealed class RunListCodec(int width) : PropertyCodec
{
    private long Read(byte[] data, int at) => width == 1 ? data[at] : BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at, 4));

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var items = Parse(ctx, start, end, site, out long def);
        var bytes = Canonical(def, items);
        if (!bytes.AsSpan().SequenceEqual(ctx.Data.AsSpan(start, end - start)))
            site.Builder?.ValueEncoding(site.Id, ctx.Data.AsSpan(start, end - start));
        var array = new JsonArray();
        foreach (var v in items) array.Add(new JsonInteger(v));
        return new JsonObject().Add("default", new JsonInteger(def)).Add("items", array);
    }

    private List<long> Parse(DecodeContext ctx, int start, int end, ValueSite site, out long def)
    {
        ctx.Need(start, width + 4, end, site.Path, "list header");
        def = Read(ctx.Data, start);
        int count = ctx.Int32(start + width);
        if (count < 0) throw Mismatch(ctx, start, site, "A list has a negative count.");
        ctx.CheckItems(count, start, site.Path);
        var items = new List<long>(count);
        int at = start + width + 4;
        while (items.Count < count)
        {
            ctx.Need(at, width, end, site.Path, "list item");
            long v = Read(ctx.Data, at);
            at += width;
            if (v == def)
            {
                ctx.Need(at, width, end, site.Path, "list run");
                long run = Read(ctx.Data, at);
                at += width;
                if (run < 0 || items.Count + run > count) throw Mismatch(ctx, at, site, "A list run passes the stored count.");
                for (long k = 0; k < run; k++) items.Add(def);
            }
            else items.Add(v);
        }
        if (at != end) throw Mismatch(ctx, at, site, "Bytes follow the list items.");
        return items;
    }

    private void Put(ByteBuffer b, long v)
    {
        if (width == 1) b.Byte((byte)v);
        else b.Int32((int)v);
    }

    private byte[] Canonical(long def, List<long> items)
    {
        var b = new ByteBuffer(items.Count * width + 8);
        Put(b, def);
        b.Int32(items.Count);
        int i = 0;
        while (i < items.Count)
        {
            if (items[i] == def)
            {
                int j = i;
                while (j < items.Count && items[j] == def) j++;
                int run = j - i;
                while (width == 1 && run > 255)
                {
                    Put(b, def);
                    Put(b, 255);
                    run -= 255;
                }
                Put(b, def);
                Put(b, run);
                i = j;
            }
            else Put(b, items[i++]);
        }
        return b.ToArray();
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is {{ \"default\": ..., \"items\": [...] }}.", value, site.Path, value.KindName);
        foreach (var m in obj.Members)
            if (m.Name is not ("default" or "items"))
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a list.")
                { File = ctx.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
        long min = width == 1 ? 0 : int.MinValue, max = width == 1 ? 255 : int.MaxValue;
        long Item(JsonValue? v, DocPath p) => v is JsonInteger n && n.Value >= min && n.Value <= max ? n.Value
            : throw ctx.Fail(v is JsonInteger ? Codes.Range : Codes.Type, "A list value is outside its storage range.", v ?? obj, p, null, $"{min} to {max}");
        long def = Item(obj["default"], site.Path.Member("default"));
        var array = obj["items"] as JsonArray ?? throw ctx.Fail(Codes.MissingMember, "items is an array.", obj, site.Path);
        var items = new List<long>(array.Items.Count);
        for (int i = 0; i < array.Items.Count; i++) items.Add(Item(array.Items[i], site.Path.Member("items").Item(i)));
        var bytes = Canonical(def, items);
        if (site.Reader?.Encodings.TryGetValue(site.Id, out var retained) == true)
        {
            // The retained encoding is used only if it decodes to the visible list.
            if (!SameList(retained.Bytes, def, items))
                throw ctx.Fail(Codes.Layout, "A retained list encoding does not decode to the visible list.", retained.At, site.Path);
            bytes = retained.Bytes;
        }
        data.Bytes(bytes);
    }

    private bool SameList(byte[] bytes, long def, List<long> items)
    {
        try
        {
            var probe = new DecodeContext(bytes, FormatSchema.Current, "game", "", null, FormatLimits.Default, new LayoutCounts());
            var site = new ValueSite { Property = null!, Id = 0, Path = DocPath.Root, Owner = new JsonObject() };
            var parsed = Parse(probe, 0, bytes.Length, site, out long d);
            return d == def && parsed.SequenceEqual(items);
        }
        catch (ConversionException)
        {
            return false;
        }
    }
}

/// <summary>TBitList streams: a default byte, the stored count and the exact bit bytes.</summary>
internal sealed class BitListCodec : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        ctx.Need(start, 5, end, site.Path, "bit list header");
        int count = ctx.Int32(start + 1);
        if (count < 0 || start + 5L + ((count + 7L) >> 3) != end)
            throw Mismatch(ctx, start, site, $"A bit list of {count} bits does not fill its {end - start} stored bytes.");
        return new JsonObject().Add("default", new JsonInteger(ctx.Data[start])).Add("count", new JsonInteger(count))
            .Add("bits", new JsonString(LayoutResidue.Hex(ctx.Data.AsSpan(start + 5, end - start - 5))));
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is {{ \"default\", \"count\", \"bits\" }}.", value, site.Path, value.KindName);
        foreach (var m in obj.Members)
            if (m.Name is not ("default" or "count" or "bits"))
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a bit list.")
                { File = ctx.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
        if (obj["default"] is not JsonInteger { Value: >= 0 and <= 255 } def)
            throw ctx.Fail(Codes.Range, "default is a byte.", obj["default"] ?? obj, site.Path.Member("default"));
        if (obj["count"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } count)
            throw ctx.Fail(Codes.Range, "count is a non-negative 32-bit integer.", obj["count"] ?? obj, site.Path.Member("count"));
        var bits = DataCodec.HexValue(ctx, obj["bits"] ?? new JsonNull(), site);
        if (bits.Length != (count.Value + 7) >> 3)
            throw ctx.Fail(Codes.Range, $"A bit list of {count.Value} bits holds {(count.Value + 7) >> 3} bytes.", obj["bits"], site.Path.Member("bits"));
        data.Byte((byte)def.Value);
        data.Int32((int)count.Value);
        data.Bytes(bits);
    }
}
