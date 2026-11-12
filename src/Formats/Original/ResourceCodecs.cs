using System.Buffers.Binary;
using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Formats.Original;

/// <summary>
/// The rwData of a Delphi <c>Single</c>. From version 5 the document holds the number in the
/// shortest text that reads back to the same single: an integral value as an integer, any other
/// value as a decimal with a lowercase exponent where the text needs one. A NaN, an infinity and
/// negative zero, which a JSON number cannot carry exactly, stay <c>{ "$bits": ... }</c> with the
/// exact little-endian bytes. A document can also state any value as <c>$bits</c>. The tree holds
/// a <see cref="SingleNode"/>.
/// </summary>
internal sealed class SingleCodec : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        if (end - start != 4) throw Mismatch(context, start, site, "A single is stored in " + (end - start) + " bytes.");
        return Value(context.Int32(start));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) =>
        data.Int32(Bits(context, value, site.Path, site.Property?.Name ?? "The value"));

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not SingleNode node) throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a single.", at: null, path: frame.Path);
        data.Int32(node.Bits);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != 4) throw context.Fail(Codes.Corrupt, "A single is stored in " + (end - start) + " bytes.", start, frame.Path);
        return new SingleNode(context.Int32(start));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not SingleNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is a single.");
        return Value(node.Bits);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) =>
        new SingleNode(Bits(site.Context, value, site.Path, site.Property?.Name ?? "The value"));

    /// <summary>The document value of the bits of a single.</summary>
    internal static JsonValue Value(int bits)
    {
        float number = BitConverter.Int32BitsToSingle(bits);
        if (!float.IsFinite(number) || bits == int.MinValue)
        {
            Span<byte> bytes = stackalloc byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, bits);
            return new JsonObject().Add("$bits", new JsonString(LayoutResidue.Hex(bytes)));
        }
        string text = number.ToString(CultureInfo.InvariantCulture);
        int exponent = text.IndexOf('E');
        if (exponent < 0)
            return text.Contains('.') ? new JsonNumber(text) : new JsonInteger(long.Parse(text, CultureInfo.InvariantCulture));
        int power = int.Parse(text.AsSpan(exponent + 1), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        return new JsonNumber(text[..exponent] + "e" + power.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>The bits of a document value: a number, an integer or <c>$bits</c>.</summary>
    internal static int Bits(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        switch (value)
        {
            case JsonInteger integer:
                return BitConverter.SingleToInt32Bits(integer.Value);
            case JsonNumber number:
                float parsed = float.Parse(number.Text, NumberStyles.Float, CultureInfo.InvariantCulture);
                if (!float.IsFinite(parsed))
                    throw context.Fail(Codes.Range, name + " is outside the single range.", value, path, number.Text, "a finite single");
                return BitConverter.SingleToInt32Bits(parsed);
            case JsonObject objectValue:
                foreach (var member in objectValue.Members)
                    if (member.Name != "$bits")
                        throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                            "Member \"" + member.Name + "\" is not allowed in a single.", member.Value, path.Member(member.Name), member.Name);
                if (objectValue["$bits"] is not JsonString hex || !HexText.IsLowerHex(hex.Value) || hex.Value.Length != 8)
                    throw context.Fail(Codes.Type, "$bits holds 4 bytes as lowercase hexadecimal.", objectValue["$bits"] ?? objectValue, path.Member("$bits"));
                return BinaryPrimitives.ReadInt32LittleEndian(HexText.Decode(hex.Value));
            default:
                throw context.Fail(Codes.Type, name + " is a number or { \"$bits\": ... }.", value, path, value.KindName);
        }
    }
}

/// <summary>
/// A TTerrainList stream (HSEngine.pas 1562 and 1569): a 32-bit count, then the pairs of signed
/// bytes, terrain type and overlay type, -2 for none. From version 5 the document is the array of
/// <c>[terrain, overlay]</c> pairs. The tree holds a <see cref="PairListNode"/>.
/// </summary>
internal sealed class TerrainListCodec : PropertyCodec
{
    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site) =>
        Document(Parse(context, start, end, site.Path));

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) =>
        Write(Pairs(context, value, site.Path, site.Property?.Name ?? "The value"), data);

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not PairListNode node) throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is pairs of signed bytes.", at: null, path: frame.Path);
        if (node.Pairs.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", at: null, path: frame.Path, value: null, constraint: "at most " + context.Limits.CollectionItems + " items");
        Write(node.Pairs, data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame) =>
        new PairListNode(Parse(context, start, end, frame.Path));

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not PairListNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is pairs of signed bytes.");
        if (node.Pairs.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", constraint: "at most " + site.Context.Limits.CollectionItems + " items");
        return Document(node.Pairs);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) =>
        new PairListNode(Pairs(site.Context, value, site.Path, site.Property?.Name ?? "The value"));

    private static List<(sbyte First, sbyte Second)> Parse(DecodeContext context, int start, int end, DocumentNodePath path)
    {
        context.Need(start, 4, end, path, "terrain count");
        int count = context.Int32(start);
        if (count < 0) throw context.Fail(Codes.Corrupt, "A terrain list has a negative count.", start, path);
        context.CheckItems(count, start, path);
        if (start + 4L + 2L * count != end)
            throw context.Fail(Codes.Corrupt, "A terrain list of " + count + " pairs does not fill its " + (end - start) + " stored bytes.", start, path);
        var pairs = new List<(sbyte First, sbyte Second)>(count);
        for (int i = 0; i < count; i++)
            pairs.Add(((sbyte)context.Data[start + 4 + 2 * i], (sbyte)context.Data[start + 5 + 2 * i]));
        return pairs;
    }

    private static void Write(IReadOnlyList<(sbyte First, sbyte Second)> pairs, ByteBuffer data)
    {
        data.Int32(pairs.Count);
        foreach (var (first, second) in pairs)
        {
            data.Byte((byte)first);
            data.Byte((byte)second);
        }
    }

    private static JsonArray Document(IReadOnlyList<(sbyte First, sbyte Second)> pairs)
    {
        var array = new JsonArray();
        foreach (var (first, second) in pairs)
            array.Add(new JsonArray().Add(new JsonInteger(first)).Add(new JsonInteger(second)));
        return array;
    }

    private static List<(sbyte First, sbyte Second)> Pairs(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonArray array) throw context.Fail(Codes.Type, name + " is an array of [terrain, overlay] pairs.", value, path, value.KindName);
        if (array.Items.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", array, path, value: null, constraint: "at most " + context.Limits.CollectionItems + " items");
        var pairs = new List<(sbyte First, sbyte Second)>(array.Items.Count);
        for (int i = 0; i < array.Items.Count; i++)
        {
            if (array.Items[i] is not JsonArray { Items.Count: 2 } pair)
                throw context.Fail(Codes.Type, "A terrain pair is an array of two signed bytes.", array.Items[i], path.Item(i), array.Items[i].KindName);
            pairs.Add((SignedByte(context, pair.Items[0], path.Item(i).Item(0)), SignedByte(context, pair.Items[1], path.Item(i).Item(1))));
        }
        return pairs;
    }

    internal static sbyte SignedByte(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonInteger number) throw context.Fail(Codes.Type, "A terrain value is an integer.", value, path, value.KindName);
        if (number.Value is < sbyte.MinValue or > sbyte.MaxValue)
            throw context.Fail(Codes.Range, "A terrain value is a signed byte.", value, path, Text(number.Value), "-128 to 127");
        return (sbyte)number.Value;
    }
}

/// <summary>
/// The flood's record of the terrain it replaced (GlobalEnchantmentSpells.pas 1086): Width times
/// Height pairs of signed bytes, terrain type and overlay type, in row order, 0 0 where the flood
/// left a hexagon alone. The width and height are the sibling members the schema names. From
/// version 5 the document lists the changed hexagons only, in row order, each
/// <c>{"X", "Y", "Terrain", "Overlay"}</c> with all four members. The tree holds the exact bytes.
/// </summary>
internal sealed class TerrainGridCodec(string widthName, string heightName) : PropertyCodec
{
    public string WidthName { get; } = widthName;

    public string HeightName { get; } = heightName;

    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var (width, height) = Dimensions(site.Owner, parent: null, (code, message) => context.Fail(code, message, start, site.Path));
        Check(width, height, end - start, (code, message) => context.Fail(code, message, start, site.Path));
        return Document(context.Data.AsSpan(start, end - start), width);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var (width, height) = Dimensions(site.Owner, parent: null, (code, message) => context.Fail(code, message, value, site.Path));
        data.Bytes(Bytes(context, value, site.Path, width, height));
    }

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BytesNode node) throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a terrain grid.", at: null, path: frame.Path);
        var (width, height) = Dimensions(owner: null, frame.Parent, (code, message) => context.Fail(code, message, at: null, path: frame.Path));
        Check(width, height, node.Data.Length, (code, message) => context.Fail(code, message, at: null, path: frame.Path));
        data.Bytes(node.Data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var (width, height) = Dimensions(owner: null, frame.Parent, (code, message) => context.Fail(code, message, start, frame.Path));
        Check(width, height, end - start, (code, message) => context.Fail(code, message, start, frame.Path));
        return new BytesNode(context.Data.AsSpan(start, end - start).ToArray());
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not BytesNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is a terrain grid.");
        var (width, height) = Dimensions(owner: null, site.Parent, (code, message) => site.Fail(code, message));
        Check(width, height, node.Data.Length, (code, message) => site.Fail(code, message));
        return Document(node.Data, width);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        var (width, height) = Dimensions(site.Owner, parent: null, (code, message) => site.Fail(code, message, value));
        return new BytesNode(Bytes(site.Context, value, site.Path, width, height));
    }

    private (int Width, int Height) Dimensions(JsonObject? owner, ObjectNode? parent, Func<string, string, ConversionException> fail)
    {
        long Dimension(string name)
        {
            if (owner?[name] is JsonInteger json) return json.Value;
            if (parent?.Find(name) is IntegerNode node) return node.Value;
            return 0;
        }
        long width = Dimension(WidthName), height = Dimension(HeightName);
        if (width < 0 || height < 0 || width > int.MaxValue || height > int.MaxValue)
            throw fail(Codes.Range, WidthName + " and " + HeightName + " are not negative.");
        return ((int)width, (int)height);
    }

    private static void Check(int width, int height, int length, Func<string, string, ConversionException> fail)
    {
        if ((long)width * height * 2 != length)
            throw fail(Codes.Dimensions, "A terrain grid of " + width + " by " + height + " hexagons is stored in " + length + " bytes.");
    }

    private static JsonArray Document(ReadOnlySpan<byte> bytes, int width)
    {
        var array = new JsonArray();
        for (int i = 0; i + 1 < bytes.Length; i += 2)
        {
            if (bytes[i] == 0 && bytes[i + 1] == 0) continue;
            int y = Math.DivRem(i / 2, width, out int x);
            array.Add(new JsonObject()
                .Add("X", new JsonInteger(x))
                .Add("Y", new JsonInteger(y))
                .Add("Terrain", new JsonInteger((sbyte)bytes[i]))
                .Add("Overlay", new JsonInteger((sbyte)bytes[i + 1])));
        }
        return array;
    }

    private static byte[] Bytes(EncodeContext context, JsonValue value, DocumentNodePath path, int width, int height)
    {
        long cells = (long)width * height;
        if (cells > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "The terrain grid has too many hexagons.", value, path, Text(cells), "at most " + context.Limits.CollectionItems + " hexagons");
        if (value is not JsonArray array) throw context.Fail(Codes.Type, "A terrain grid is an array of changed hexagons.", value, path, value.KindName);
        var bytes = new byte[cells * 2];
        long previous = -1;
        for (int i = 0; i < array.Items.Count; i++)
        {
            var at = path.Item(i);
            if (array.Items[i] is not JsonObject hexagon) throw context.Fail(Codes.Type, "A changed hexagon is an object.", array.Items[i], at, array.Items[i].KindName);
            foreach (var member in hexagon.Members)
                if (member.Name is not ("X" or "Y" or "Terrain" or "Overlay"))
                    throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                        "Member \"" + member.Name + "\" is not allowed in a changed hexagon.", member.Value, at.Member(member.Name), member.Name);
            long x = Coordinate(context, hexagon, "X", width, at);
            long y = Coordinate(context, hexagon, "Y", height, at);
            sbyte terrain = TerrainListCodec.SignedByte(context, hexagon["Terrain"] ?? throw Missing(context, hexagon, at, "Terrain"), at.Member("Terrain"));
            sbyte overlay = TerrainListCodec.SignedByte(context, hexagon["Overlay"] ?? throw Missing(context, hexagon, at, "Overlay"), at.Member("Overlay"));
            if (terrain == 0 && overlay == 0)
                throw context.Fail(Codes.Range, "A changed hexagon does not hold terrain 0 and overlay 0.", hexagon, at);
            long index = y * width + x;
            if (index == previous) throw context.Fail(Codes.Duplicate, "Hexagon " + x + ", " + y + " is listed twice.", hexagon, at);
            if (index < previous) throw context.Fail(Codes.Range, "The changed hexagons are not in row order.", hexagon, at);
            previous = index;
            bytes[2 * index] = (byte)terrain;
            bytes[2 * index + 1] = (byte)overlay;
        }
        return bytes;
    }

    private static long Coordinate(EncodeContext context, JsonObject hexagon, string name, int size, DocumentNodePath at)
    {
        var value = hexagon[name] ?? throw Missing(context, hexagon, at, name);
        if (value is not JsonInteger number) throw context.Fail(Codes.Type, name + " is an integer.", value, at.Member(name), value.KindName);
        if (number.Value < 0 || number.Value >= size)
            throw context.Fail(Codes.Range, name + " is outside the grid.", value, at.Member(name), Text(number.Value), "0 to " + (size - 1));
        return number.Value;
    }

    private static ConversionException Missing(EncodeContext context, JsonObject hexagon, DocumentNodePath at, string name) =>
        context.Fail(Codes.MissingMember, "A changed hexagon needs " + name + ".", hexagon, at);
}

/// <summary>
/// A whole ILB library (ImageLib.pas 9960 load and 10038 save). The 24-byte header holds the
/// short string <c>ILB</c> and a zero, three bytes the writer leaves as they were in memory, the
/// single 4 for the version, the header size 24, the end of the index and the size of the whole
/// library. From version 5 the document is <c>{ "$ilb": "&lt;base64&gt;" }</c> with the standard
/// alphabet and padding. Every path checks the header. The bytes after it stay opaque. The tree
/// holds a <see cref="BytesNode"/>.
/// </summary>
internal sealed class ImageLibraryCodec : PropertyCodec
{
    private const int HeaderSize = 24;

    private static ReadOnlySpan<byte> Identifier => "\u0004ILB\0"u8;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var bytes = context.Data.AsSpan(start, end - start);
        if (Problem(bytes) is string problem) throw context.Fail(Codes.Corrupt, problem, start, site.Path);
        return Document(bytes);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) =>
        data.Bytes(Bytes(context, value, site.Path));

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BytesNode node) throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is an ILB library.", at: null, path: frame.Path);
        if (Problem(node.Data) is string problem) throw context.Fail(Codes.Corrupt, problem, at: null, path: frame.Path);
        data.Bytes(node.Data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var bytes = context.Data.AsSpan(start, end - start);
        if (Problem(bytes) is string problem) throw context.Fail(Codes.Corrupt, problem, start, frame.Path);
        return new BytesNode(bytes.ToArray());
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not BytesNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is an ILB library.");
        if (Problem(node.Data) is string problem) throw site.Fail(Codes.Corrupt, problem);
        if (node.Data.Length > site.Context.Limits.DecompressedBytes) throw site.Fail(Codes.Limit, "A payload is too large.");
        return Document(node.Data);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) => new BytesNode(Bytes(site.Context, value, site.Path));

    private static JsonObject Document(ReadOnlySpan<byte> bytes) => new JsonObject().Add("$ilb", new JsonString(Convert.ToBase64String(bytes)));

    private static byte[] Bytes(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonObject objectValue) throw context.Fail(Codes.Type, "An ILB library is { \"$ilb\": ... }.", value, path, value.KindName);
        foreach (var member in objectValue.Members)
            if (member.Name != "$ilb")
                throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    "Member \"" + member.Name + "\" is not allowed in an ILB library.", member.Value, path.Member(member.Name), member.Name);
        if (objectValue["$ilb"] is not JsonString text) throw context.Fail(Codes.MissingMember, "An ILB library needs $ilb.", objectValue, path);
        if (text.Value.Length / 4L * 3 > context.Limits.DecompressedBytes) throw context.Fail(Codes.Limit, "A payload is too large.", text, path.Member("$ilb"));
        var buffer = new byte[text.Value.Length / 4 * 3];
        // Only the canonical text decodes: the standard alphabet, padding, no white space.
        if (text.Value.Length % 4 != 0 || !Convert.TryFromBase64String(text.Value, buffer, out int length)
            || Convert.ToBase64String(buffer.AsSpan(0, length)) != text.Value)
            throw context.Fail(Codes.Type, "$ilb is base64 with the standard alphabet and padding.", text, path.Member("$ilb"));
        var bytes = buffer.AsSpan(0, length).ToArray();
        if (Problem(bytes) is string problem) throw context.Fail(Codes.Corrupt, problem, text, path.Member("$ilb"));
        return bytes;
    }

    /// <summary>The header check, or null when the header holds.</summary>
    internal static string? Problem(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length < HeaderSize) return "An ILB library of " + bytes.Length + " bytes is shorter than its 24-byte header.";
        if (!bytes[..5].SequenceEqual(Identifier)) return "An ILB library does not start with the identifier ILB.";
        if (BinaryPrimitives.ReadInt32LittleEndian(bytes[8..]) != BitConverter.SingleToInt32Bits(4f))
            return "An ILB library is not version 4.";
        if (BinaryPrimitives.ReadInt32LittleEndian(bytes[12..]) != HeaderSize) return "An ILB library header does not state the size 24.";
        int size = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..]);
        if (size != bytes.Length) return "An ILB library states " + size + " bytes and holds " + bytes.Length + ".";
        int index = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..]);
        if (index < HeaderSize || index > size) return "The index of an ILB library ends outside the library.";
        return null;
    }
}

/// <summary>
/// A TImageSequenceList stream (ImageLib.pas 8959 and 8986): a 32-bit count, then per entry a nil
/// byte and, for an entry that is not nil, a TImageSequence chain (ImageLib.pas 8822 and 8839).
/// A layer is a 32-bit length and the library name, the layer, speed and mode bytes, the 32-bit
/// image index and x and y offsets, a TFrameTableList (a 32-bit default, a 32-bit count and the
/// indices) for the modes FrameTable and RandomFrameTable, then a has-next byte. From version 5
/// the document is an array with null for a nil entry and otherwise the chain's layers, each
/// <c>{"Library", "Layer", "Speed", "Mode", "ImageIndex", "OffsetX", "OffsetY", "Frames"}</c>
/// with zero members omitted, <c>Mode</c> a SequenceMode name and <c>Frames</c>
/// <c>{"default", "items"}</c> on a frame-table layer only. A nil or has-next byte other than 0
/// and 1 reads as true and is kept as a value-encoding part. The tree holds an
/// <see cref="ImageSequenceListNode"/> and the direct path writes 0 and 1.
/// </summary>
internal sealed class ImageSequencesCodec : PropertyCodec
{
    private static readonly string[] s_members = ["Library", "Layer", "Speed", "Mode", "ImageIndex", "OffsetX", "OffsetY", "Frames"];

    private readonly EnumCodec _mode = new("SequenceMode", 1, signed: false);

    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var entries = Parse(context, start, end, site.Path);
        var stored = context.Data.AsSpan(start, end - start);
        if (!Canonical(entries).AsSpan().SequenceEqual(stored)) site.Builder?.ValueEncoding(site.Id, stored);
        return Document(entries, context.File, site.Path);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var bytes = Canonical(Entries(context, value, site.Path, site.Property?.Name ?? "The value"));
        if (site.Reader?.Encodings.TryGetValue(site.Id, out var retained) == true)
        {
            // The retained encoding is used only if it decodes to the visible list.
            if (!SameList(retained.Bytes, bytes))
                throw context.Fail(Codes.Layout, "A retained image sequence encoding does not decode to the visible list.", retained.At, site.Path);
            bytes = retained.Bytes;
        }
        data.Bytes(bytes);
    }

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ImageSequenceListNode node) throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is an image sequence list.", at: null, path: frame.Path);
        Check(node, (code, message, path) => context.Fail(code, message, at: null, path: path), context, frame.Path);
        data.Bytes(Canonical(node.Entries));
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame) =>
        new ImageSequenceListNode(Parse(context, start, end, frame.Path));

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not ImageSequenceListNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is an image sequence list.");
        Check(node, (code, message, path) => site.At(path, element: false).Fail(code, message), site.Context, site.Path);
        return Document(node.Entries, site.Context.File, site.Path);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) =>
        new ImageSequenceListNode(Entries(site.Context, value, site.Path, site.Property?.Name ?? "The value"));

    private static void Check(ImageSequenceListNode node, Func<string, string, DocumentNodePath, ConversionException> fail, EncodeContext context, DocumentNodePath path)
    {
        if (node.Entries.Count > context.Limits.CollectionItems) throw fail(Codes.Limit, "A collection has too many items.", path);
        for (int i = 0; i < node.Entries.Count; i++)
        {
            if (node.Entries[i] is not { } layers) continue;
            if (layers.Count == 0) throw fail(Codes.Type, "A sequence has at least one layer.", path.Item(i));
            for (int k = 0; k < layers.Count; k++)
            {
                var layer = layers[k];
                var at = path.Item(i).Item(k);
                if ((layer.Frames is not null) != SequenceLayer.HasFrameTable(layer.Mode))
                    throw fail(Codes.Type, "A layer stores a frame table exactly when its mode is FrameTable or RandomFrameTable.", at);
                if (TextCodec.Problem(layer.Library, context.Limits.StringBytes) is { } problem) throw fail(problem.Code, problem.Message, at.Member("Library"));
                if (layer.Frames is { } frames && frames.Items.Count > context.Limits.CollectionItems)
                    throw fail(Codes.Limit, "A collection has too many items.", at.Member("Frames"));
            }
        }
    }

    private static List<IReadOnlyList<SequenceLayer>?> Parse(DecodeContext context, int start, int end, DocumentNodePath path)
    {
        context.Need(start, 4, end, path, "sequence count");
        int count = context.Int32(start);
        if (count < 0) throw context.Fail(Codes.Corrupt, "An image sequence list has a negative count.", start, path);
        context.CheckItems(count, start, path);
        var entries = new List<IReadOnlyList<SequenceLayer>?>(count);
        int at = start + 4;
        for (int i = 0; i < count; i++)
        {
            context.Need(at, 1, end, path.Item(i), "nil byte");
            if (context.Data[at++] != 0)
            {
                entries.Add(null);
                continue;
            }
            var layers = new List<SequenceLayer>();
            bool next;
            do
            {
                var layerPath = path.Item(i).Item(layers.Count);
                context.Need(at, 4, end, layerPath, "library name length");
                int length = context.Int32(at);
                at += 4;
                if (length < 0) throw context.Fail(Codes.Corrupt, "A library name has a negative length.", at - 4, layerPath);
                if (length > context.Limits.StringBytes)
                    throw context.Fail(Codes.Limit, "A string is too long.", at - 4, layerPath, Text(length), "at most " + context.Limits.StringBytes + " bytes");
                context.Need(at, length, end, layerPath, "library name");
                string library = TextCodec.Decode(context.Data.AsSpan(at, length));
                at += length;
                context.Need(at, 15, end, layerPath, "layer");
                byte mode = context.Data[at + 2];
                var layer = new SequenceLayer
                {
                    Library = library,
                    Layer = context.Data[at],
                    Speed = context.Data[at + 1],
                    Mode = mode,
                    ImageIndex = context.Int32(at + 3),
                    OffsetX = context.Int32(at + 7),
                    OffsetY = context.Int32(at + 11),
                };
                at += 15;
                if (SequenceLayer.HasFrameTable(mode))
                {
                    context.Need(at, 8, end, layerPath, "frame table header");
                    int defaultValue = context.Int32(at);
                    int frames = context.Int32(at + 4);
                    at += 8;
                    if (frames < 0) throw context.Fail(Codes.Corrupt, "A frame table has a negative count.", at - 4, layerPath);
                    context.CheckItems(frames, at - 4, layerPath);
                    context.Need(at, 4 * frames, end, layerPath, "frame table");
                    var items = new int[frames];
                    for (int k = 0; k < frames; k++) items[k] = context.Int32(at + 4 * k);
                    at += 4 * frames;
                    layer = layer with { Frames = new FrameTable(defaultValue, items) };
                }
                layers.Add(layer);
                context.Need(at, 1, end, layerPath, "has-next byte");
                next = context.Data[at++] != 0;
            }
            while (next);
            entries.Add(layers);
        }
        if (at != end) throw context.Fail(Codes.Corrupt, "Bytes follow the image sequences.", at, path);
        return entries;
    }

    private static byte[] Canonical(IReadOnlyList<IReadOnlyList<SequenceLayer>?> entries)
    {
        var data = new ByteBuffer(64);
        data.Int32(entries.Count);
        foreach (var layers in entries)
        {
            if (layers is null)
            {
                data.Byte(1);
                continue;
            }
            data.Byte(0);
            for (int k = 0; k < layers.Count; k++)
            {
                var layer = layers[k];
                var name = Cp1252.Encode(layer.Library);
                data.Int32(name.Length);
                data.Bytes(name);
                data.Byte(layer.Layer);
                data.Byte(layer.Speed);
                data.Byte(layer.Mode);
                data.Int32(layer.ImageIndex);
                data.Int32(layer.OffsetX);
                data.Int32(layer.OffsetY);
                if (layer.Frames is { } frames)
                {
                    data.Int32(frames.Default);
                    data.Int32(frames.Items.Count);
                    foreach (int item in frames.Items) data.Int32(item);
                }
                data.Byte(k + 1 < layers.Count ? (byte)1 : (byte)0);
            }
        }
        return data.ToArray();
    }

    private static bool SameList(byte[] retained, byte[] canonical)
    {
        try
        {
            var probe = new DecodeContext(retained, FormatSchema.Current, "game", "", null, FormatLimits.Default, new LayoutCounts());
            return Canonical(Parse(probe, 0, retained.Length, DocumentNodePath.Root)).AsSpan().SequenceEqual(canonical);
        }
        catch (ConversionException)
        {
            return false;
        }
    }

    private JsonArray Document(IReadOnlyList<IReadOnlyList<SequenceLayer>?> entries, string? file, DocumentNodePath path)
    {
        var array = new JsonArray();
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] is not { } layers)
            {
                array.Add(new JsonNull());
                continue;
            }
            var chain = new JsonArray();
            for (int k = 0; k < layers.Count; k++)
            {
                var layer = layers[k];
                var item = new JsonObject();
                if (layer.Library.Length != 0) item.Add("Library", new JsonString(layer.Library));
                if (layer.Layer != 0) item.Add("Layer", new JsonInteger(layer.Layer));
                if (layer.Speed != 0) item.Add("Speed", new JsonInteger(layer.Speed));
                if (layer.Mode != 0) item.Add("Mode", _mode.MemberText(layer.Mode, file, path.Item(i).Item(k).Member("Mode").ToString()));
                if (layer.ImageIndex != 0) item.Add("ImageIndex", new JsonInteger(layer.ImageIndex));
                if (layer.OffsetX != 0) item.Add("OffsetX", new JsonInteger(layer.OffsetX));
                if (layer.OffsetY != 0) item.Add("OffsetY", new JsonInteger(layer.OffsetY));
                if (layer.Frames is { } frames)
                {
                    var items = new JsonArray();
                    foreach (int frame in frames.Items) items.Add(new JsonInteger(frame));
                    item.Add("Frames", new JsonObject().Add("default", new JsonInteger(frames.Default)).Add("items", items));
                }
                chain.Add(item);
            }
            array.Add(chain);
        }
        return array;
    }

    private List<IReadOnlyList<SequenceLayer>?> Entries(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonArray array) throw context.Fail(Codes.Type, name + " is an array of sequences.", value, path, value.KindName);
        if (array.Items.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", array, path, value: null, constraint: "at most " + context.Limits.CollectionItems + " items");
        var entries = new List<IReadOnlyList<SequenceLayer>?>(array.Items.Count);
        for (int i = 0; i < array.Items.Count; i++)
        {
            var at = path.Item(i);
            switch (array.Items[i])
            {
                case JsonNull:
                    entries.Add(null);
                    break;
                case JsonArray { Items.Count: > 0 } chain:
                    if (chain.Items.Count > context.Limits.CollectionItems)
                        throw context.Fail(Codes.Limit, "A collection has too many items.", chain, at, value: null, constraint: "at most " + context.Limits.CollectionItems + " items");
                    var layers = new List<SequenceLayer>(chain.Items.Count);
                    for (int k = 0; k < chain.Items.Count; k++) layers.Add(Layer(context, chain.Items[k], at.Item(k)));
                    entries.Add(layers);
                    break;
                case JsonArray empty:
                    throw context.Fail(Codes.Type, "A sequence has at least one layer.", empty, at);
                default:
                    throw context.Fail(Codes.Type, "An entry is null or an array of layers.", array.Items[i], at, array.Items[i].KindName);
            }
        }
        return entries;
    }

    private SequenceLayer Layer(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonObject layer) throw context.Fail(Codes.Type, "A layer is an object.", value, path, value.KindName);
        foreach (var member in layer.Members)
            if (Array.IndexOf(s_members, member.Name) < 0)
                throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    "Member \"" + member.Name + "\" is not allowed in a layer.", member.Value, path.Member(member.Name), member.Name);
        string library = "";
        if (layer["Library"] is { } libraryValue)
        {
            if (libraryValue is not JsonString text) throw context.Fail(Codes.Type, "Library is a string.", libraryValue, path.Member("Library"), libraryValue.KindName);
            TextCodec.Encode(context, text.Value, text, path.Member("Library"));
            library = text.Value;
        }
        byte mode = layer["Mode"] is { } modeValue ? (byte)_mode.Parse(context, modeValue, path.Member("Mode"), "Mode") : (byte)0;
        FrameTable? frames = null;
        if (layer["Frames"] is { } framesValue)
        {
            if (!SequenceLayer.HasFrameTable(mode))
                throw context.Fail(Codes.UnknownMember, "Frames belongs only to a FrameTable or RandomFrameTable layer.", framesValue, path.Member("Frames"), "Frames");
            frames = Frames(context, framesValue, path.Member("Frames"));
        }
        else if (SequenceLayer.HasFrameTable(mode))
            throw context.Fail(Codes.MissingMember, "A FrameTable or RandomFrameTable layer needs Frames.", layer, path);
        return new SequenceLayer
        {
            Library = library,
            Layer = (byte)Integer(context, layer["Layer"], path.Member("Layer"), min: 0, max: 255),
            Speed = (byte)Integer(context, layer["Speed"], path.Member("Speed"), min: 0, max: 255),
            Mode = mode,
            ImageIndex = (int)Integer(context, layer["ImageIndex"], path.Member("ImageIndex"), min: int.MinValue, max: int.MaxValue),
            OffsetX = (int)Integer(context, layer["OffsetX"], path.Member("OffsetX"), min: int.MinValue, max: int.MaxValue),
            OffsetY = (int)Integer(context, layer["OffsetY"], path.Member("OffsetY"), min: int.MinValue, max: int.MaxValue),
            Frames = frames,
        };
    }

    private static FrameTable Frames(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonObject table) throw context.Fail(Codes.Type, "Frames is { \"default\": ..., \"items\": [...] }.", value, path, value.KindName);
        foreach (var member in table.Members)
            if (member.Name is not ("default" or "items"))
                throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    "Member \"" + member.Name + "\" is not allowed in a frame table.", member.Value, path.Member(member.Name), member.Name);
        int defaultValue = (int)Integer(context, table["default"] ?? throw context.Fail(Codes.MissingMember, "Frames needs default.", table, path),
            path.Member("default"), min: int.MinValue, max: int.MaxValue);
        var items = table["items"] as JsonArray ?? throw context.Fail(Codes.MissingMember, "items is an array.", table, path);
        if (items.Items.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", items, path.Member("items"), value: null, constraint: "at most " + context.Limits.CollectionItems + " items");
        var frames = new int[items.Items.Count];
        for (int k = 0; k < frames.Length; k++)
            frames[k] = (int)Integer(context, items.Items[k], path.Member("items").Item(k), min: int.MinValue, max: int.MaxValue);
        return new FrameTable(defaultValue, frames);
    }

    private static long Integer(EncodeContext context, JsonValue? value, DocumentNodePath path, long min, long max)
    {
        if (value is null) return 0;
        if (value is not JsonInteger number) throw context.Fail(Codes.Type, "The value is an integer.", value, path, value.KindName);
        if (number.Value < min || number.Value > max)
            throw context.Fail(Codes.Range, "The value is outside its storage range.", value, path, Text(number.Value), min + " to " + max);
        return number.Value;
    }
}
