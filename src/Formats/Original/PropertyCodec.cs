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

    /// <summary>False when a present tree value writes no image entry. A false rwBool is that case.</summary>
    internal virtual bool StoresImage(Tree.TreeValue value) => true;

    /// <summary>Set when this codec has no original image. The direct path refuses a present member.</summary>
    internal virtual string? ImageRefusal => null;

    /// <summary>Writes a tree value as original image bytes. A leaf overrides this. The base refuses.</summary>
    internal virtual void WriteImage(Tree.TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (ImageRefusal is string reason) throw new InvalidOperationException(reason);
        throw new InvalidOperationException(GetType().Name + " has no image writer.");
    }

    /// <summary>Reads original image bytes into a tree value. A leaf overrides this. The base refuses.</summary>
    internal virtual Tree.TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (ImageRefusal is string reason) throw new InvalidOperationException(reason);
        throw new InvalidOperationException(GetType().Name + " has no image reader.");
    }

    /// <summary>
    /// Writes the entries of a dynamic member, one per slot, into the object's table. The base
    /// refuses the member. A keyed created member overrides it.
    /// </summary>
    internal virtual void WriteSlotsImage(Tree.TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame) =>
        throw context.Fail(Codes.Type, "PE-3 still needs a dynamic member. The direct path does not store it.", null, frame.Path);

    /// <summary>Reads the stored entries of a dynamic member, in data order. The base refuses the member.</summary>
    internal virtual Tree.TreeValue ReadSlotsImage(DecodeContext context, IReadOnlyList<SlotImage> entries, ImageFrame frame) =>
        throw context.Fail(Codes.Type, "PE-3 still needs a dynamic member. The direct path does not store it.", entries[0].Start, frame.Path);

    /// <summary>Writes a dynamic member as its document array. The base refuses the member.</summary>
    internal virtual JsonValue WriteSlotsTree(Tree.TreeValue value, Tree.DocumentSite site) =>
        throw site.Fail(Codes.Type, "PE-1 does not store a dynamic member.");

    /// <summary>Reads a dynamic member's document array. The base refuses the member.</summary>
    internal virtual Tree.TreeValue ReadSlotsTree(JsonValue value, Tree.DocumentSite site) =>
        throw site.Fail(Codes.Type, "PE-1 does not store a dynamic member.", value);

    /// <summary>The document value is a JSON array for this codec at this version.</summary>
    internal virtual bool AcceptsDocumentArray(int version) => AcceptsList(version);

    internal static PropertyCodec Create(JsonObject source, FormatSchema schema, string path)
    {
        string codec = ((JsonString)source["codec"]!).Value;
        int? size = source["size"] is JsonInteger s ? (int)s.Value : null;
        string? classValue = (source["class"] as JsonString)?.Value;
        string? family = (source["family"] as JsonString)?.Value;
        return codec switch
        {
            "i32" => new IntegerCodec(4, true, (source["domain"] as JsonString)?.Value, Raw(source)),
            "u8" => new IntegerCodec(1, false, raw: Raw(source)),
            "u16" => new IntegerCodec(2, false, raw: Raw(source)),
            "i8" => new IntegerCodec(1, true, raw: Raw(source)),
            "bool" => new BooleanCodec(),
            "raw-bool" => new RawBooleanCodec(),
            "string" => new StringCodec(),
            "strings" => new StringsCodec(),
            "strings-ex" => new StringsWithValuesCodec(),
            "data" => new DataCodec(size),
            "list" => new ListCodec(Create((JsonObject)source["element"]!, schema, path + ".element"), schema.StoresLists),
            "record" => source["record"] is JsonString named
                ? new CatalogRecordCodec(named.Value, CheckedSize(source, path))
                : new RecordCodec(source, schema, path),
            "bit-array" => new BitArrayCodec(CountName(source, path)),
            "float32" => new FloatCodec(4),
            "float64" => new FloatCodec(8),
            "int-list" => new RunListCodec(4),
            "byte-list" => new RunListCodec(1),
            "bit-list" => new BitListCodec(),
            "stream" => new DataCodec(null),
            "object" => new FixedObjectCodec(classValue ?? throw new InvalidOperationException("An object codec needs a class.")),
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

    private static bool Signed(JsonObject source) => source["signed"] is JsonBoolean { Value: true };

    private static int Count(JsonObject source) =>
        source["count"] is JsonInteger count ? (int)count.Value : throw new InvalidOperationException("An integer array needs a count.");

    private static string CountName(JsonObject source, string path) =>
        (source["count"] as JsonString)?.Value ?? throw new InvalidOperationException(path + " bit array names its count property.");

    private static int CheckedSize(JsonObject source, string path) =>
        source["size"] is JsonInteger size && size.Value > 0 ? (int)size.Value : throw new InvalidOperationException(path + " record needs a size.");

    private static bool Raw(JsonObject source) => source["raw"] is JsonBoolean { Value: true };

    /// <summary>Decodes one stored value that occupies data[start..end) exactly.</summary>
    internal abstract JsonValue Decode(DecodeContext context, int start, int end, ValueSite site);

    /// <summary>Encodes one value into the data buffer.</summary>
    internal abstract void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site);

    /// <summary>Encodes a member. A member stores no entry, one entry or several.</summary>
    internal virtual void EncodeMember(EncodeContext context, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        writer.Begin(site.Id, site.Property);
        Encode(context, value, writer.Data, site);
        writer.End();
    }

    internal virtual JsonValue DecodeElement(DecodeContext context, int start, int end, ValueSite site) =>
        throw new InvalidOperationException($"{GetType().Name} has no dynamic elements.");

    internal virtual int ElementId(EncodeContext context, JsonValue value, SchemaProperty property, int index, DocumentNodePath path) =>
        throw new InvalidOperationException($"{GetType().Name} has no dynamic elements.");

    internal virtual void EncodeElement(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) => Encode(context, value, data, site);

    /// <summary>Writes a tree value as its document value. Null means the member is omitted.</summary>
    internal virtual JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site) =>
        throw site.Mismatch($"{site.Property?.Name ?? "The value"} does not store this value.");

    /// <summary>Reads a document value into a tree value. Null means the member is absent.</summary>
    internal virtual Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site) =>
        throw site.Mismatch($"{site.Property?.Name ?? "The value"} does not store this value.", value);

    internal virtual JsonValue DecodePair(DecodeContext context, (int Start, int End)? first, (int Start, int End)? second, ValueSite site) =>
        throw new InvalidOperationException($"{GetType().Name} stores one entry.");

    private protected static ConversionException Mismatch(DecodeContext context, int start, ValueSite site, string message) =>
        context.Fail(Codes.Corrupt, message, start, site.Path);

    protected static string Text(long value) => value.ToString(CultureInfo.InvariantCulture);
}

/// <summary>One stored entry of a dynamic member: its id and the bytes it occupies.</summary>
internal readonly record struct SlotImage(int Id, int Start, int End);

internal sealed partial class IntegerCodec(int width, bool signed, string? domain = null, bool raw = false) : PropertyCodec
{
    /// <summary>True when rwData stores the integer, so a zero is present.</summary>
    internal bool Raw { get; } = raw;

    private long Min => signed ? -(1L << (width * 8 - 1)) : 0;
    private long Max => signed ? (1L << (width * 8 - 1)) - 1 : (1L << (width * 8)) - 1;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        if (end - start != width)
            throw Mismatch(context, start, site, $"An integer of {width} bytes is stored in {end - start} bytes.");
        var bytes = context.Data.AsSpan(start, width);
        long value = width switch
        {
            1 => signed ? (sbyte)bytes[0] : bytes[0],
            2 => signed ? BinaryPrimitives.ReadInt16LittleEndian(bytes) : BinaryPrimitives.ReadUInt16LittleEndian(bytes),
            _ => signed ? BinaryPrimitives.ReadInt32LittleEndian(bytes) : BinaryPrimitives.ReadUInt32LittleEndian(bytes),
        };
        return new JsonInteger(value);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        string name = site.Property?.Name ?? "The value";
        if (value is not JsonInteger n) throw context.Fail(Codes.Type, $"{name} is an integer.", value, site.Path, value.KindName);
        if (n.Value < Min || n.Value > Max)
            throw context.Fail(Codes.Range, $"{name} is outside its storage range.", value, site.Path, Text(n.Value), $"{Min} to {Max}");
        switch (width)
        {
            case 1: data.Byte((byte)n.Value); break;
            case 2: data.Int16((ushort)n.Value); break;
            default: data.Int32((int)n.Value); break;
        }
    }
}

/// <summary>rwBool: true when present. Explicit false normalizes to absence.</summary>
internal sealed partial class BooleanCodec : PropertyCodec
{
    public override bool ZeroSize => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site) => new JsonBoolean(true);

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) { }

    internal override void EncodeMember(EncodeContext context, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        if (value is not JsonBoolean b) throw context.Fail(Codes.Type, $"{site.Property.Name} is a boolean.", value, site.Path, value.KindName);
        if (!b.Value) return;
        writer.Begin(site.Id, site.Property);
        writer.End();
    }
}

/// <summary>Text through the frozen code page table. Strict input refuses unmapped characters.</summary>
internal static class TextCodec
{
    public static string Decode(ReadOnlySpan<byte> bytes) => Cp1252.Decode(bytes);

    public static byte[] Encode(EncodeContext context, string text, JsonValue at, DocumentNodePath path)
    {
        if (Problem(text, context.Limits.StringBytes) is { } problem)
            throw context.Fail(problem.Code, problem.Message, at, path, problem.Value, problem.Constraint);
        return Cp1252.Encode(text);
    }

    /// <summary>The code page bytes of <paramref name="text"/>, or the same refusal the document path raises.</summary>
    internal static byte[] Bytes(EncodeContext context, string text, DocumentNodePath path)
    {
        if (Problem(text, context.Limits.StringBytes) is { } problem)
            throw context.Fail(problem.Code, problem.Message, null, path, problem.Value, problem.Constraint);
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
    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        context.Need(start, 1, end, site.Path, "string length");
        int at = start + 1;
        int length = context.Data[start];
        bool longPrefix = length == 255;
        if (longPrefix)
        {
            context.Need(at, 4, end, site.Path, "string length");
            length = context.Int32(at);
            at += 4;
        }
        if (length < 0 || at + (long)length != end)
            throw Mismatch(context, start, site, $"A string of {length} bytes does not fill its {end - start} stored bytes.");
        if (length > context.Limits.StringBytes)
            throw context.Fail(Codes.Limit, "A string is too long.", start, site.Path, Text(length), $"at most {context.Limits.StringBytes} bytes");
        if (longPrefix && length < 255) site.Builder?.ValuePrefix(site.Id);
        return new JsonString(TextCodec.Decode(context.Data.AsSpan(at, length)));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonString s) throw context.Fail(Codes.Type, $"{site.Property.Name} is a string.", value, site.Path, value.KindName);
        var bytes = TextCodec.Encode(context, s.Value, value, site.Path);
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
    protected static (string Text, int End) Block(DecodeContext context, int start, int end, ValueSite site)
    {
        context.Need(start, 4, end, site.Path, "text length");
        int length = context.Int32(start);
        if (length < 0 || start + 4L + length > end) throw Mismatch(context, start, site, $"A text block of {length} bytes does not fit its value.");
        if (length > context.Limits.StringBytes)
            throw context.Fail(Codes.Limit, "A text block is too long.", start, site.Path, Text(length), $"at most {context.Limits.StringBytes} bytes");
        return (TextCodec.Decode(context.Data.AsSpan(start + 4, length)), start + 4 + length);
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

    internal static string TextOf(EncodeContext context, JsonValue value, DocumentNodePath path, out int lines)
    {
        if (value is JsonArray array)
        {
            var parts = new string[array.Items.Count];
            for (int i = 0; i < array.Items.Count; i++)
            {
                if (array.Items[i] is not JsonString line) throw context.Fail(Codes.Type, "A line is a string.", array.Items[i], path.Item(i));
                if (line.Value.AsSpan().IndexOfAny('\r', '\n', '\0') >= 0)
                    throw context.Fail(Codes.Text, "A line holds CR, LF or NUL. Use { \"$text\": ... } for such text.", line, path.Item(i));
                parts[i] = line.Value;
            }
            lines = array.Items.Count;
            return Join(parts);
        }
        if (value is JsonObject objectValue && objectValue["$text"] is JsonString text)
        {
            lines = LineCount(text.Value);
            return text.Value;
        }
        throw context.Fail(Codes.Type, "Text is an array of lines or { \"$text\": ... }.", value, path, value.KindName);
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var (text, after) = Block(context, start, end, site);
        if (after != end) throw Mismatch(context, start, site, "Bytes follow the text block.");
        return TextValue(text);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is JsonObject objectValue) CheckOnly(context, objectValue, site.Path, "$text");
        var bytes = TextCodec.Encode(context, TextOf(context, value, site.Path, out _), value, site.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
    }

    protected static void CheckOnly(EncodeContext context, JsonObject objectValue, DocumentNodePath path, params string[] names)
    {
        foreach (var m in objectValue.Members)
            if (Array.IndexOf(names, m.Name) < 0)
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    $"Member \"{m.Name}\" is not allowed here.")
                { File = context.File, Span = m.NameSpan, Path = path.Member(m.Name).ToString() });
    }
}

/// <summary>rwStringsEx: the text block, then one signed 32-bit object value per line.</summary>
internal sealed partial class StringsWithValuesCodec : StringsCodec
{
    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var (text, after) = Block(context, start, end, site);
        if ((end - after) % 4 != 0) throw Mismatch(context, start, site, "The object values are not whole 32-bit integers.");
        int count = (end - after) / 4;
        if (count != LineCount(text)) throw Mismatch(context, start, site, $"The text has {LineCount(text)} lines and {count} object values.");
        var objects = new JsonArray();
        for (int i = 0; i < count; i++) objects.Add(new JsonInteger(context.Int32(after + 4 * i)));
        var lines = Lines(text);
        if (lines is null) return new JsonObject().Add("$text", new JsonString(text)).Add("objects", objects);
        var array = new JsonArray();
        foreach (var line in lines) array.Add(new JsonString(line));
        return new JsonObject().Add("lines", array).Add("objects", objects);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue) throw context.Fail(Codes.Type, $"{site.Property.Name} is an object with lines or $text, and objects.", value, site.Path, value.KindName);
        var (text, ids) = ReadMembers(context, objectValue, site.Path);
        var bytes = TextCodec.Encode(context, text, value, site.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
        foreach (var id in ids) data.Int32(id);
    }

    internal static (string Text, List<int> Ids) ReadMembers(EncodeContext context, JsonObject objectValue, DocumentNodePath path)
    {
        bool hasText = objectValue.Find("$text") is not null;
        CheckOnly(context, objectValue, path, hasText ? "$text" : "lines", "objects");
        var textValue = hasText ? objectValue : objectValue["lines"] ?? throw context.Fail(Codes.MissingMember, "lines or $text is required.", objectValue, path);
        string text = TextOf(context, textValue, path.Member(hasText ? "$text" : "lines"), out int lines);
        var objects = objectValue["objects"] as JsonArray ?? throw context.Fail(Codes.MissingMember, "objects is an array of integers.", objectValue, path);
        if (objects.Items.Count != lines)
            throw context.Fail(Codes.Corrupt, $"The text has {lines} lines and {objects.Items.Count} object values.", objects, path.Member("objects"));
        var ids = new List<int>(objects.Items.Count);
        for (int i = 0; i < objects.Items.Count; i++)
        {
            if (objects.Items[i] is not JsonInteger n || n.Value < int.MinValue || n.Value > int.MaxValue)
                throw context.Fail(Codes.Range, "An object value is a signed 32-bit integer.", objects.Items[i], path.Member("objects").Item(i));
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

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        if (size is int n && end - start != n) throw Mismatch(context, start, site, $"A record of {n} bytes is stored in {end - start} bytes.");
        return new JsonString(LayoutResidue.Hex(context.Data.AsSpan(start, end - start)));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var bytes = HexValue(context, value, site);
        if (size is int n && bytes.Length != n)
            throw context.Fail(Codes.Range, $"{site.Property.Name} holds exactly {n} bytes.", value, site.Path, Text(bytes.Length), $"{n} bytes");
        data.Bytes(bytes);
    }

    internal static byte[] HexValue(EncodeContext context, JsonValue value, ValueSite site)
    {
        if (value is not JsonString s || !HexText.IsLowerHex(s.Value))
            throw context.Fail(Codes.Type, $"{site.Property.Name} is lowercase hexadecimal bytes.", value, site.Path, value.KindName);
        if (s.Value.Length / 2 > context.Limits.DecompressedBytes) throw context.Fail(Codes.Limit, "A payload is too large.", value, site.Path);
        return HexText.Decode(s.Value);
    }
}

/// <summary>Floating-point fields keep their exact little-endian bits.</summary>
internal sealed class FloatCodec(int width) : PropertyCodec
{
    internal override string? ImageRefusal => "The direct path does not store a float.";

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        if (end - start != width) throw Mismatch(context, start, site, $"A {width * 8}-bit float is stored in {end - start} bytes.");
        return new JsonObject().Add("$bits", new JsonString(LayoutResidue.Hex(context.Data.AsSpan(start, width))));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue || objectValue.Members.Count != 1 || objectValue["$bits"] is not JsonString)
            throw context.Fail(Codes.Type, $"{site.Property.Name} is {{ \"$bits\": ... }}.", value, site.Path, value.KindName);
        var bytes = DataCodec.HexValue(context, objectValue["$bits"]!, site);
        if (bytes.Length != width) throw context.Fail(Codes.Range, $"$bits holds {width} bytes.", objectValue["$bits"], site.Path.Member("$bits"));
        data.Bytes(bytes);
    }
}

/// <summary>
/// TIntegerList and TByteList streams: the default value, the 32-bit count, then the values
/// where a default value is followed by a run length. The document holds the default and the
/// decoded items. A run split the writer does not make is kept as a value-encoding part.
/// The tree holds a <see cref="Tree.DefaultListNode"/>. The direct path writes the canonical runs,
/// splitting a byte run longer than 255, and reads any run split the stream decodes.
/// </summary>
internal sealed class RunListCodec(int width) : PropertyCodec
{
    private long Min => width == 1 ? 0 : int.MinValue;
    private long Max => width == 1 ? 255 : int.MaxValue;

    internal override void WriteImage(Tree.TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        string name = frame.Property?.Name ?? "The value";
        if (value is not Tree.DefaultListNode node || node.Width != width)
            throw context.Fail(Codes.Type, name + " is a list with a default.", null, frame.Path);
        if (node.Items.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", null, frame.Path, null, "at most " + context.Limits.CollectionItems + " items");
        if (node.Default < Min || node.Default > Max)
            throw context.Fail(Codes.Range, "A list value is outside its storage range.", null, frame.Path.Member("default"), Text(node.Default), Min + " to " + Max);
        var items = new List<long>(node.Items.Count);
        for (int i = 0; i < node.Items.Count; i++)
        {
            long item = node.Items[i];
            if (item < Min || item > Max)
                throw context.Fail(Codes.Range, "A list value is outside its storage range.", null, frame.Path.Member("items").Item(i), Text(item), Min + " to " + Max);
            items.Add(item);
        }
        data.Bytes(Canonical(node.Default, items));
    }

    internal override Tree.TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var items = Parse(context, start, end, frame.Value(context), out long defense);
        return new Tree.DefaultListNode(width, defense, items);
    }

    internal override JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site)
    {
        if (value is not Tree.DefaultListNode node || node.Width != width)
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is {{ \"default\": ..., \"items\": [...] }}.");
        if (node.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        if (node.Default < Min || node.Default > Max)
            throw site.At(site.Path.Member("default"), element: false).Fail(Codes.Range, "A list value is outside its storage range.", value: Text(node.Default), constraint: $"{Min} to {Max}");
        var array = new JsonArray();
        for (int i = 0; i < node.Items.Count; i++)
        {
            long item = node.Items[i];
            if (item < Min || item > Max)
                throw site.At(site.Path.Member("items").Item(i), element: true).Fail(Codes.Range, "A list value is outside its storage range.", value: Text(item), constraint: $"{Min} to {Max}");
            array.Add(new JsonInteger(item));
        }
        return new JsonObject().Add("default", new JsonInteger(node.Default)).Add("items", array);
    }

    internal override Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site)
    {
        if (value is not JsonObject objectValue)
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is {{ \"default\": ..., \"items\": [...] }}.", value);
        foreach (var member in objectValue.Members)
            if (member.Name is not ("default" or "items"))
                throw site.At(site.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, $"Member \"{member.Name}\" is not allowed in a list.", member.Value, member.Name);
        long Item(JsonValue? item, Tree.DocumentSite at) => item is JsonInteger n && n.Value >= Min && n.Value <= Max ? n.Value
            : throw at.Fail(item is JsonInteger ? Codes.Range : Codes.Type, "A list value is outside its storage range.", item ?? objectValue, null, $"{Min} to {Max}");
        long defense = Item(objectValue["default"], site.At(site.Path.Member("default"), element: false));
        var array = objectValue["items"] as JsonArray ?? throw site.Fail(Codes.MissingMember, "items is an array.", objectValue);
        if (array.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", array, null, $"at most {site.Context.Limits.CollectionItems} items");
        var items = new long[array.Items.Count];
        for (int i = 0; i < items.Length; i++) items[i] = Item(array.Items[i], site.At(site.Path.Member("items").Item(i), element: true));
        return new Tree.DefaultListNode(width, defense, items);
    }

    private long Read(byte[] data, int at) => width == 1 ? data[at] : BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at, 4));

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var items = Parse(context, start, end, site, out long defense);
        var bytes = Canonical(defense, items);
        if (!bytes.AsSpan().SequenceEqual(context.Data.AsSpan(start, end - start)))
            site.Builder?.ValueEncoding(site.Id, context.Data.AsSpan(start, end - start));
        var array = new JsonArray();
        foreach (var v in items) array.Add(new JsonInteger(v));
        return new JsonObject().Add("default", new JsonInteger(defense)).Add("items", array);
    }

    private List<long> Parse(DecodeContext context, int start, int end, ValueSite site, out long defense)
    {
        context.Need(start, width + 4, end, site.Path, "list header");
        defense = Read(context.Data, start);
        int count = context.Int32(start + width);
        if (count < 0) throw Mismatch(context, start, site, "A list has a negative count.");
        context.CheckItems(count, start, site.Path);
        var items = new List<long>(count);
        int at = start + width + 4;
        while (items.Count < count)
        {
            context.Need(at, width, end, site.Path, "list item");
            long v = Read(context.Data, at);
            at += width;
            if (v == defense)
            {
                context.Need(at, width, end, site.Path, "list run");
                long run = Read(context.Data, at);
                at += width;
                if (run < 0 || items.Count + run > count) throw Mismatch(context, at, site, "A list run passes the stored count.");
                for (long k = 0; k < run; k++) items.Add(defense);
            }
            else items.Add(v);
        }
        if (at != end) throw Mismatch(context, at, site, "Bytes follow the list items.");
        return items;
    }

    private void Put(ByteBuffer b, long v)
    {
        if (width == 1) b.Byte((byte)v);
        else b.Int32((int)v);
    }

    private byte[] Canonical(long defense, List<long> items)
    {
        var b = new ByteBuffer(items.Count * width + 8);
        Put(b, defense);
        b.Int32(items.Count);
        int i = 0;
        while (i < items.Count)
        {
            if (items[i] == defense)
            {
                int j = i;
                while (j < items.Count && items[j] == defense) j++;
                int run = j - i;
                while (width == 1 && run > 255)
                {
                    Put(b, defense);
                    Put(b, 255);
                    run -= 255;
                }
                Put(b, defense);
                Put(b, run);
                i = j;
            }
            else Put(b, items[i++]);
        }
        return b.ToArray();
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue) throw context.Fail(Codes.Type, $"{site.Property.Name} is {{ \"default\": ..., \"items\": [...] }}.", value, site.Path, value.KindName);
        foreach (var m in objectValue.Members)
            if (m.Name is not ("default" or "items"))
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a list.")
                { File = context.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
        long min = width == 1 ? 0 : int.MinValue, max = width == 1 ? 255 : int.MaxValue;
        long Item(JsonValue? v, DocumentNodePath p) => v is JsonInteger n && n.Value >= min && n.Value <= max ? n.Value
            : throw context.Fail(v is JsonInteger ? Codes.Range : Codes.Type, "A list value is outside its storage range.", v ?? objectValue, p, null, $"{min} to {max}");
        long defense = Item(objectValue["default"], site.Path.Member("default"));
        var array = objectValue["items"] as JsonArray ?? throw context.Fail(Codes.MissingMember, "items is an array.", objectValue, site.Path);
        var items = new List<long>(array.Items.Count);
        for (int i = 0; i < array.Items.Count; i++) items.Add(Item(array.Items[i], site.Path.Member("items").Item(i)));
        var bytes = Canonical(defense, items);
        if (site.Reader?.Encodings.TryGetValue(site.Id, out var retained) == true)
        {
            // The retained encoding is used only if it decodes to the visible list.
            if (!SameList(retained.Bytes, defense, items))
                throw context.Fail(Codes.Layout, "A retained list encoding does not decode to the visible list.", retained.At, site.Path);
            bytes = retained.Bytes;
        }
        data.Bytes(bytes);
    }

    private bool SameList(byte[] bytes, long defense, List<long> items)
    {
        try
        {
            var probe = new DecodeContext(bytes, FormatSchema.Current, "game", "", null, FormatLimits.Default, new LayoutCounts());
            var site = new ValueSite { Property = null!, Id = 0, Path = DocumentNodePath.Root, Owner = new JsonObject() };
            var parsed = Parse(probe, 0, bytes.Length, site, out long d);
            return d == defense && parsed.SequenceEqual(items);
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
    internal override string? ImageRefusal => "PE-2c still needs a bit list. The direct path does not store it.";

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        context.Need(start, 5, end, site.Path, "bit list header");
        int count = context.Int32(start + 1);
        if (count < 0 || start + 5L + ((count + 7L) >> 3) != end)
            throw Mismatch(context, start, site, $"A bit list of {count} bits does not fill its {end - start} stored bytes.");
        return new JsonObject().Add("default", new JsonInteger(context.Data[start])).Add("count", new JsonInteger(count))
            .Add("bits", new JsonString(LayoutResidue.Hex(context.Data.AsSpan(start + 5, end - start - 5))));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue) throw context.Fail(Codes.Type, $"{site.Property.Name} is {{ \"default\", \"count\", \"bits\" }}.", value, site.Path, value.KindName);
        foreach (var m in objectValue.Members)
            if (m.Name is not ("default" or "count" or "bits"))
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a bit list.")
                { File = context.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
        if (objectValue["default"] is not JsonInteger { Value: >= 0 and <= 255 } defense)
            throw context.Fail(Codes.Range, "default is a byte.", objectValue["default"] ?? objectValue, site.Path.Member("default"));
        if (objectValue["count"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } count)
            throw context.Fail(Codes.Range, "count is a non-negative 32-bit integer.", objectValue["count"] ?? objectValue, site.Path.Member("count"));
        var bits = DataCodec.HexValue(context, objectValue["bits"] ?? new JsonNull(), site);
        if (bits.Length != (count.Value + 7) >> 3)
            throw context.Fail(Codes.Range, $"A bit list of {count.Value} bits holds {(count.Value + 7) >> 3} bytes.", objectValue["bits"], site.Path.Member("bits"));
        data.Byte((byte)defense.Value);
        data.Int32((int)count.Value);
        data.Bytes(bits);
    }
}
