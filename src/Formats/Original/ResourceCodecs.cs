using System.Buffers.Binary;
using System.Globalization;
using Aow.Formats.Json;
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
        return end - start != 4
            ? throw Mismatch(context, start, site, message: $"A single is stored in {end - start} bytes.")
            : Value(context.Int32(start));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => data.Int32(Bits(context, value, site.Path, name: site.Property?.Name ?? "The value"));

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not SingleNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a single.", at: null, path: frame.Path);
        }

        data.Int32(node.Bits);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        return end - start != 4
            ? throw context.Fail(Codes.Corrupt, message: $"A single is stored in {end - start} bytes.", offset: start, frame.Path)
            : (TreeValue)new SingleNode(context.Int32(start));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
        => value is not SingleNode node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a single.")
            : Value(node.Bits);

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
        => new SingleNode(Bits(site.Context, value, site.Path, name: site.Property?.Name ?? "The value"));

    /// <summary>The document value of the bits of a single.</summary>
    internal static JsonValue Value(int bits)
    {
        float number = BitConverter.Int32BitsToSingle(bits);
        if (!float.IsFinite(number) || bits == int.MinValue)
        {
            Span<byte> bytes = stackalloc byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value: bits);
            return new JsonObject().Add("$bits", new JsonString(HexText.Encode(bytes)));
        }
        string text = number.ToString(CultureInfo.InvariantCulture);
        int exponent = text.IndexOf('E');
        if (exponent < 0)
        {
            return text.Contains('.') ? new JsonNumber(text) : new JsonInteger(long.Parse(text, provider: CultureInfo.InvariantCulture));
        }

        int power = int.Parse(text.AsSpan(exponent + 1), style: NumberStyles.AllowLeadingSign, provider: CultureInfo.InvariantCulture);
        return new JsonNumber(string.Create(CultureInfo.InvariantCulture, handler: $"{text[..exponent]}e{power}"));
    }

    /// <summary>The bits of a document value: a number, an integer or <c>$bits</c>.</summary>
    internal static int Bits(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        switch (value)
        {
            case JsonInteger integer:
                return BitConverter.SingleToInt32Bits(integer.Value);
            case JsonNumber number:
                float parsed = float.Parse(number.Text, style: NumberStyles.Float, provider: CultureInfo.InvariantCulture);
                if (!float.IsFinite(parsed))
                {
                    throw context.Fail(Codes.Range, message: $"{name} is outside the single range.", at: value, path, value: number.Text, constraint: "a finite single");
                }

                return BitConverter.SingleToInt32Bits(parsed);
            case JsonObject objectValue:
                foreach (JsonMember member in objectValue.Members)
                {
                    if (member.Name != "$bits")
                    {
                        throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                            message: $"Member \"{member.Name}\" is not allowed in a single.", at: member.Value, path: path.Member(member.Name), value: member.Name);
                    }
                }

                if (objectValue["$bits"] is not JsonString hex || !HexText.IsLowerHex(hex.Value) || hex.Value.Length != 8)
                {
                    throw context.Fail(Codes.Type, message: "$bits holds 4 bytes as lowercase hexadecimal.", at: objectValue["$bits"] ?? objectValue, path: path.Member("$bits"));
                }

                return BinaryPrimitives.ReadInt32LittleEndian(HexText.Decode(hex.Value));
            default:
                throw context.Fail(Codes.Type, message: $"{name} is a number or {{ \"$bits\": ... }}.", at: value, path, value: value.KindName);
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

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
        => Document(Parse(context, start, end, site.Path));

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => Write(Pairs(context, value, site.Path, name: site.Property?.Name ?? "The value"), data);

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not PairListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is pairs of signed bytes.", at: null, path: frame.Path);
        }

        if (node.Pairs.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: null, path: frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        Write(node.Pairs, data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
        => new PairListNode(Parse(context, start, end, frame.Path));

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not PairListNode node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is pairs of signed bytes.")
            : node.Pairs.Count > site.Context.Limits.CollectionItems
            ? throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items")
            : (JsonValue)Document(node.Pairs);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
        => new PairListNode(Pairs(site.Context, value, site.Path, name: site.Property?.Name ?? "The value"));

    private static List<(sbyte First, sbyte Second)> Parse(DecodeContext context, int start, int end, DocumentNodePath path)
    {
        context.Need(start, count: 4, end, path, what: "terrain count");
        int count = context.Int32(start);
        if (count < 0)
        {
            throw context.Fail(Codes.Corrupt, message: "A terrain list has a negative count.", offset: start, path);
        }

        context.CheckItems(count, offset: start, path);
        if (start + 4L + (2L * count) != end)
        {
            throw context.Fail(Codes.Corrupt, message: $"A terrain list of {count} pairs does not fill its {end - start} stored bytes.", offset: start, path);
        }

        List<(sbyte First, sbyte Second)> pairs = [with(count)];
        for (int i = 0; i < count; i++)
        {
            pairs.Add(((sbyte)context.Data[start + 4 + (2 * i)], (sbyte)context.Data[start + 5 + (2 * i)]));
        }

        return pairs;
    }

    private static void Write(IReadOnlyList<(sbyte First, sbyte Second)> pairs, ByteBuffer data)
    {
        data.Int32(pairs.Count);
        foreach ((sbyte first, sbyte second) in pairs)
        {
            data.Byte((byte)first);
            data.Byte((byte)second);
        }
    }

    private static JsonArray Document(IReadOnlyList<(sbyte First, sbyte Second)> pairs)
    {
        JsonArray array = new();
        foreach ((sbyte first, sbyte second) in pairs)
        {
            _ = array.Add(new JsonArray().Add(new JsonInteger(first)).Add(new JsonInteger(second)));
        }

        return array;
    }

    private static List<(sbyte First, sbyte Second)> Pairs(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: $"{name} is an array of [terrain, overlay] pairs.", at: value, path, value: value.KindName);
        }

        if (array.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: array, path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        List<(sbyte First, sbyte Second)> pairs = [with(array.Items.Count)];
        for (int i = 0; i < array.Items.Count; i++)
        {
            if (array.Items[i] is not JsonArray { Items.Count: 2 } pair)
            {
                throw context.Fail(Codes.Type, message: "A terrain pair is an array of two signed bytes.", at: array.Items[i], path: path.Item(i), value: array.Items[i].KindName);
            }

            pairs.Add((SignedByte(context, value: pair.Items[0], path: path.Item(i).Item(0)), SignedByte(context, value: pair.Items[1], path: path.Item(i).Item(1))));
        }
        return pairs;
    }

    internal static sbyte SignedByte(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        return value is not JsonInteger number
            ? throw context.Fail(Codes.Type, message: "A terrain value is an integer.", at: value, path, value: value.KindName)
            : number.Value is < sbyte.MinValue or > sbyte.MaxValue
            ? throw context.Fail(Codes.Range, message: "A terrain value is a signed byte.", at: value, path, value: Text(number.Value), constraint: "-128 to 127")
            : (sbyte)number.Value;
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
        (int width, int height) = Dimensions(site.Owner, parent: null, (code, message) => context.Fail(code, message, offset: start, site.Path));
        Check(width, height, length: end - start, (code, message) => context.Fail(code, message, offset: start, site.Path));
        return Document(context.Data.AsSpan(start, length: end - start), width);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        (int width, int height) = Dimensions(site.Owner, parent: null, (code, message) => context.Fail(code, message, at: value, site.Path));
        data.Bytes(Bytes(context, value, site.Path, width, height));
    }

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BytesNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a terrain grid.", at: null, path: frame.Path);
        }

        (int width, int height) = Dimensions(owner: null, frame.Parent, (code, message) => context.Fail(code, message, at: null, path: frame.Path));
        Check(width, height, node.Data.Length, (code, message) => context.Fail(code, message, at: null, path: frame.Path));
        data.Bytes(node.Data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        (int width, int height) = Dimensions(owner: null, frame.Parent, (code, message) => context.Fail(code, message, offset: start, frame.Path));
        Check(width, height, length: end - start, (code, message) => context.Fail(code, message, offset: start, frame.Path));
        return new BytesNode(context.Data.AsSpan(start, length: end - start).ToArray());
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not BytesNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a terrain grid.");
        }

        (int width, int height) = Dimensions(owner: null, site.Parent, (code, message) => site.Fail(code, message));
        Check(width, height, node.Data.Length, (code, message) => site.Fail(code, message));
        return Document(node.Data, width);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        (int width, int height) = Dimensions(site.Owner, parent: null, (code, message) => site.Fail(code, message, at: value));
        return new BytesNode(Bytes(site.Context, value, site.Path, width, height));
    }

    private (int Width, int Height) Dimensions(JsonObject? owner, ObjectNode? parent, Func<string, string, ConversionException> fail)
    {
        long Dimension(string name) => owner?[name] is JsonInteger json ? json.Value : parent?.Find(name) is IntegerNode node ? node.Value : 0;
        long width = Dimension(WidthName), height = Dimension(HeightName);
        return width < 0 || height < 0 || width > int.MaxValue || height > int.MaxValue
            ? throw fail(Codes.Range, $"{WidthName} and {HeightName} are not negative.")
            : ((int Width, int Height))((int)width, (int)height);
    }

    private static void Check(int width, int height, int length, Func<string, string, ConversionException> fail)
    {
        if ((long)width * height * 2 != length)
        {
            throw fail(Codes.Dimensions, $"A terrain grid of {width} by {height} hexagons is stored in {length} bytes.");
        }
    }

    private static JsonArray Document(ReadOnlySpan<byte> bytes, int width)
    {
        JsonArray array = new();
        for (int i = 0; i + 1 < bytes.Length; i += 2)
        {
            if (bytes[i] == 0 && bytes[i + 1] == 0)
            {
                continue;
            }

            int y = Math.DivRem(i / 2, width, result: out int x);
            _ = array.Add(new JsonObject()
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
        {
            throw context.Fail(Codes.Limit, message: "The terrain grid has too many hexagons.", at: value, path, value: Text(cells), constraint: $"at most {context.Limits.CollectionItems} hexagons");
        }

        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: "A terrain grid is an array of changed hexagons.", at: value, path, value: value.KindName);
        }

        byte[] bytes = new byte[cells * 2];
        long previous = -1;
        for (int i = 0; i < array.Items.Count; i++)
        {
            DocumentNodePath at = path.Item(i);
            if (array.Items[i] is not JsonObject hexagon)
            {
                throw context.Fail(Codes.Type, message: "A changed hexagon is an object.", at: array.Items[i], path: at, value: array.Items[i].KindName);
            }

            foreach (JsonMember member in hexagon.Members)
            {
                if (member.Name is not ("X" or "Y" or "Terrain" or "Overlay"))
                {
                    throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                        message: $"Member \"{member.Name}\" is not allowed in a changed hexagon.", at: member.Value, path: at.Member(member.Name), value: member.Name);
                }
            }

            long x = Coordinate(context, hexagon, name: "X", size: width, at);
            long y = Coordinate(context, hexagon, name: "Y", size: height, at);
            sbyte terrain = TerrainListCodec.SignedByte(context, value: hexagon["Terrain"] ?? throw Missing(context, hexagon, at, name: "Terrain"), path: at.Member("Terrain"));
            sbyte overlay = TerrainListCodec.SignedByte(context, value: hexagon["Overlay"] ?? throw Missing(context, hexagon, at, name: "Overlay"), path: at.Member("Overlay"));
            if (terrain == 0 && overlay == 0)
            {
                throw context.Fail(Codes.Range, message: "A changed hexagon does not hold terrain 0 and overlay 0.", at: hexagon, path: at);
            }

            long index = (y * width) + x;
            if (index == previous)
            {
                throw context.Fail(Codes.Duplicate, message: $"Hexagon {x}, {y} is listed twice.", at: hexagon, path: at);
            }

            if (index < previous)
            {
                throw context.Fail(Codes.Range, message: "The changed hexagons are not in row order.", at: hexagon, path: at);
            }

            previous = index;
            bytes[2 * index] = (byte)terrain;
            bytes[(2 * index) + 1] = (byte)overlay;
        }
        return bytes;
    }

    private static long Coordinate(EncodeContext context, JsonObject hexagon, string name, int size, DocumentNodePath at)
    {
        JsonValue value = hexagon[name] ?? throw Missing(context, hexagon, at, name);
        return value is not JsonInteger number
            ? throw context.Fail(Codes.Type, message: $"{name} is an integer.", at: value, path: at.Member(name), value: value.KindName)
            : number.Value < 0 || number.Value >= size
            ? throw context.Fail(Codes.Range, message: $"{name} is outside the grid.", at: value, path: at.Member(name), value: Text(number.Value), constraint: $"0 to {size - 1}")
            : number.Value;
    }

    private static ConversionException Missing(EncodeContext context, JsonObject hexagon, DocumentNodePath at, string name)
        => context.Fail(Codes.MissingMember, message: $"A changed hexagon needs {name}.", at: hexagon, path: at);
}

/// <summary>
/// A whole ILB library (ImageLib.pas 9960 load and 10038 save). The 24-byte header holds the
/// short string <c>ILB</c> and a zero, three bytes the original writer leaves uninitialized, the
/// single 4 for the version, the header size 24, the end of the index and the size of the whole
/// library. From version 5 the document is <c>{ "$ilb": "&lt;base64&gt;" }</c> with the standard
/// alphabet and padding. Every path checks the header and clears the two uninitialized regions of
/// the original writer: header bytes 5 to 7 and the unused last dword of each 16-bit RLE sprite
/// entry (classes 17 and 18, ImageLib.pas 2945 to 2971). The rest of the library stays opaque.
/// The tree holds a <see cref="BytesNode"/>.
/// </summary>
internal sealed class ImageLibraryCodec : PropertyCodec
{
    private const int HeaderSize = 24;

    private static ReadOnlySpan<byte> Identifier => "\u0004ILB\0"u8;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        Span<byte> bytes = context.Data.AsSpan(start, length: end - start);
        return Problem(bytes) is string problem ? throw context.Fail(Codes.Corrupt, message: problem, offset: start, site.Path) : (JsonValue)Document(Normalize(bytes));
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => data.Bytes(Bytes(context, value, site.Path));

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BytesNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is an ILB library.", at: null, path: frame.Path);
        }

        if (Problem(node.Data) is string problem)
        {
            throw context.Fail(Codes.Corrupt, message: problem, at: null, path: frame.Path);
        }

        data.Bytes(Normalize(node.Data));
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        Span<byte> bytes = context.Data.AsSpan(start, length: end - start);
        return Problem(bytes) is string problem
            ? throw context.Fail(Codes.Corrupt, message: problem, offset: start, frame.Path)
            : (TreeValue)new BytesNode(Normalize(bytes));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not BytesNode node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an ILB library.")
            : Problem(node.Data) is string problem
            ? throw site.Fail(Codes.Corrupt, message: problem)
            : node.Data.Length > site.Context.Limits.DecompressedBytes
            ? throw site.Fail(Codes.Limit, message: "A payload is too large.")
            : (JsonValue)Document(Normalize(node.Data));
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) => new BytesNode(Bytes(site.Context, value, site.Path));

    private static JsonObject Document(ReadOnlySpan<byte> bytes) => new JsonObject().Add("$ilb", new JsonString(Convert.ToBase64String(bytes)));

    private static byte[] Bytes(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: "An ILB library is { \"$ilb\": ... }.", at: value, path, value: value.KindName);
        }

        foreach (JsonMember member in objectValue.Members)
        {
            if (member.Name != "$ilb")
            {
                throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    message: $"Member \"{member.Name}\" is not allowed in an ILB library.", at: member.Value, path: path.Member(member.Name), value: member.Name);
            }
        }

        if (objectValue["$ilb"] is not JsonString text)
        {
            throw context.Fail(Codes.MissingMember, message: "An ILB library needs $ilb.", at: objectValue, path);
        }

        if (text.Value.Length / 4L * 3 > context.Limits.DecompressedBytes)
        {
            throw context.Fail(Codes.Limit, message: "A payload is too large.", at: text, path: path.Member("$ilb"));
        }

        byte[] buffer = new byte[text.Value.Length / 4 * 3];
        // Only the canonical text decodes: the standard alphabet, padding, no white space.
        if (text.Value.Length % 4 != 0 || !Convert.TryFromBase64String(text.Value, bytes: buffer, bytesWritten: out int length)
            || Convert.ToBase64String(buffer.AsSpan(0, length)) != text.Value)
        {
            throw context.Fail(Codes.Type, message: "$ilb is base64 with the standard alphabet and padding.", at: text, path: path.Member("$ilb"));
        }

        ReadOnlySpan<byte> bytes = buffer.AsSpan(0, length);
        return Problem(bytes) is string problem ? throw context.Fail(Codes.Corrupt, message: problem, at: text, path: path.Member("$ilb")) : Normalize(bytes);
    }

    /// <summary>
    /// Clears header bytes 5 to 7 and the unused last dword of each class 17 and 18 sprite entry.
    /// </summary>
    internal static byte[] Normalize(ReadOnlySpan<byte> bytes)
    {
        byte[] copy = bytes.ToArray();
        if (copy.Length >= 8)
        {
            copy[5] = 0;
            copy[6] = 0;
            copy[7] = 0;
        }

        List<int> spriteDwords = [];
        if (TryFindSpriteReservedOffsets(copy, offsets: spriteDwords))
        {
            foreach (int offset in spriteDwords)
            {
                copy[offset] = 0;
                copy[offset + 1] = 0;
                copy[offset + 2] = 0;
                copy[offset + 3] = 0;
            }
        }

        return copy;
    }

    /// <summary>
    /// Walks an ILB version 4 index and collects the byte offset of each class 17 or 18 sprite's
    /// unused last image-data dword (ImageLib.pas 2945 to 2971).
    /// </summary>
    internal static bool TryFindSpriteReservedOffsets(ReadOnlySpan<byte> bytes, List<int> offsets)
    {
        if (Problem(bytes) is not null)
        {
            return false;
        }

        int indexEnd = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..]);
        int pos = HeaderSize;
        if (!TryReadInt32(bytes, ref pos, limit: indexEnd, value: out int paletteCount) || paletteCount < 0)
        {
            return false;
        }

        long paletteBytes = paletteCount * 1028L;
        if (pos + paletteBytes > indexEnd)
        {
            return false;
        }

        pos += (int)paletteBytes;
        while (true)
        {
            if (!TryReadInt32(bytes, ref pos, limit: indexEnd, value: out int index))
            {
                return false;
            }

            if (index == -1)
            {
                break;
            }

            if (!WalkImage(bytes, ref pos, limit: indexEnd, offsets: offsets, depth: 0, hasImage: out _))
            {
                return false;
            }

            if (!TrySkip(ref pos, count: 4, limit: indexEnd))
            {
                return false;
            }
        }

        return pos == indexEnd;
    }

    private static bool WalkImage(ReadOnlySpan<byte> bytes, ref int pos, int limit, List<int> offsets, int depth, out bool hasImage)
    {
        hasImage = false;
        if (depth > 32 || !TryReadInt32(bytes, ref pos, limit: limit, value: out int classId))
        {
            return false;
        }

        if (classId == 0)
        {
            return true;
        }

        if (classId == 256)
        {
            while (true)
            {
                if (!WalkImage(bytes, ref pos, limit: limit, offsets: offsets, depth: depth + 1, hasImage: out bool subImage))
                {
                    return false;
                }

                if (!subImage)
                {
                    hasImage = true;
                    return true;
                }
            }
        }

        if (classId is not (1 or 2 or 16 or 17 or 18 or 19 or 20 or 21 or 22))
        {
            return false;
        }

        if (pos >= limit)
        {
            return false;
        }

        byte version = bytes[pos++];
        if (!TryReadInt32(bytes, ref pos, limit: limit, value: out int nameLength) || nameLength < 0 || !TrySkip(ref pos, count: nameLength, limit: limit))
        {
            return false;
        }

        // Five 32-bit common metrics and LoadMode byte.
        if (!TrySkip(ref pos, count: 21, limit: limit))
        {
            return false;
        }

        if (!TryReadInt32(bytes, ref pos, limit: limit, value: out int dataSize) || dataSize < 0)
        {
            return false;
        }

        int dataPosition = -1;
        if (version >= 2 && !TryReadInt32(bytes, ref pos, limit: limit, value: out dataPosition))
        {
            return false;
        }

        if (version > 0 && !TrySkip(ref pos, count: 8, limit: limit))
        {
            return false;
        }

        if (version >= 3 && !TrySkip(ref pos, count: 8, limit: limit))
        {
            return false;
        }

        if (classId is 16 or 17 or 18 or 21 or 22)
        {
            if (!TrySkip(ref pos, count: 4, limit: limit))
            {
                return false;
            }
        }
        else if (classId is 1 or 2)
        {
            if (pos >= limit)
            {
                return false;
            }

            byte hasPalette = bytes[pos++];
            if (!TrySkip(ref pos, count: hasPalette != 0 ? 1028 : 4, limit: limit))
            {
                return false;
            }
        }

        if (classId is 2 or 17 or 18)
        {
            if (pos + 24L > limit)
            {
                return false;
            }

            if (classId is 17 or 18)
            {
                offsets.Add(pos + 20);
            }

            pos += 24;
        }
        else if (classId == 22)
        {
            if (!TrySkip(ref pos, count: 20, limit: limit))
            {
                return false;
            }
        }
        else if (classId is 19 or 20)
        {
            if (!TrySkip(ref pos, count: 16, limit: limit))
            {
                return false;
            }
        }

        if (dataPosition == -1 && !TrySkip(ref pos, count: dataSize, limit: limit))
        {
            return false;
        }

        hasImage = true;
        return true;
    }

    private static bool TryReadInt32(ReadOnlySpan<byte> bytes, ref int pos, int limit, out int value)
    {
        if (pos + 4L > limit)
        {
            value = 0;
            return false;
        }

        value = BinaryPrimitives.ReadInt32LittleEndian(bytes[pos..]);
        pos += 4;
        return true;
    }

    private static bool TrySkip(ref int pos, int count, int limit)
    {
        if (count < 0 || pos + (long)count > limit)
        {
            return false;
        }

        pos += count;
        return true;
    }

    /// <summary>The header check, or null when the header holds.</summary>
    internal static string? Problem(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length < HeaderSize)
        {
            return $"An ILB library of {bytes.Length} bytes is shorter than its 24-byte header.";
        }

        if (!bytes[..5].SequenceEqual(Identifier))
        {
            return "An ILB library does not start with the identifier ILB.";
        }

        if (BinaryPrimitives.ReadInt32LittleEndian(bytes[8..]) != BitConverter.SingleToInt32Bits(4f))
        {
            return "An ILB library is not version 4.";
        }

        if (BinaryPrimitives.ReadInt32LittleEndian(bytes[12..]) != HeaderSize)
        {
            return "An ILB library header does not state the size 24.";
        }

        int size = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..]);
        if (size != bytes.Length)
        {
            return $"An ILB library states {size} bytes and holds {bytes.Length}.";
        }

        int index = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..]);
        return index < HeaderSize || index > size ? "The index of an ILB library ends outside the library." : null;
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
/// reads as true on import and writes back as 1. The tree holds an
/// <see cref="ImageSequenceListNode"/>.
/// </summary>
internal sealed class ImageSequencesCodec : PropertyCodec
{
    private static readonly string[] s_members = ["Library", "Layer", "Speed", "Mode", "ImageIndex", "OffsetX", "OffsetY", "Frames"];

    private readonly EnumCodec _mode = new("SequenceMode", width: 1, signed: false);

    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
        => Document(Parse(context, start, end, site.Path), context.File, site.Path);

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => data.Bytes(Canonical(Entries(context, value, site.Path, name: site.Property?.Name ?? "The value")));

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ImageSequenceListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is an image sequence list.", at: null, path: frame.Path);
        }

        Check(node, fail: (code, message, path) => context.Fail(code, message, at: null, path: path), context, frame.Path);
        data.Bytes(Canonical(node.Entries));
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
        => new ImageSequenceListNode(Parse(context, start, end, frame.Path));

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not ImageSequenceListNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an image sequence list.");
        }

        Check(node, fail: (code, message, path) => site.At(path, element: false).Fail(code, message), site.Context, site.Path);
        return Document(node.Entries, site.Context.File, site.Path);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
        => new ImageSequenceListNode(Entries(site.Context, value, site.Path, name: site.Property?.Name ?? "The value"));

    private static void Check(ImageSequenceListNode node, Func<string, string, DocumentNodePath, ConversionException> fail, EncodeContext context, DocumentNodePath path)
    {
        if (node.Entries.Count > context.Limits.CollectionItems)
        {
            throw fail(Codes.Limit, "A collection has too many items.", path);
        }

        for (int i = 0; i < node.Entries.Count; i++)
        {
            if (node.Entries[i] is not { } layers)
            {
                continue;
            }

            if (layers.Count == 0)
            {
                throw fail(Codes.Type, "A sequence has at least one layer.", path.Item(i));
            }

            for (int k = 0; k < layers.Count; k++)
            {
                SequenceLayer layer = layers[k];
                DocumentNodePath at = path.Item(i).Item(k);
                if ((layer.Frames is not null) != SequenceLayer.HasFrameTable(layer.Mode))
                {
                    throw fail(Codes.Type, "A layer stores a frame table exactly when its mode is FrameTable or RandomFrameTable.", at);
                }

                if (TextCodec.Problem(layer.Library, limit: context.Limits.StringBytes) is { } problem)
                {
                    throw fail(problem.Code, problem.Message, at.Member("Library"));
                }

                if (layer.Frames is { } frames && frames.Items.Count > context.Limits.CollectionItems)
                {
                    throw fail(Codes.Limit, "A collection has too many items.", at.Member("Frames"));
                }
            }
        }
    }

    private static List<IReadOnlyList<SequenceLayer>?> Parse(DecodeContext context, int start, int end, DocumentNodePath path)
    {
        context.Need(start, count: 4, end, path, what: "sequence count");
        int count = context.Int32(start);
        if (count < 0)
        {
            throw context.Fail(Codes.Corrupt, message: "An image sequence list has a negative count.", offset: start, path);
        }

        context.CheckItems(count, offset: start, path);
        List<IReadOnlyList<SequenceLayer>?> entries = [with(count)];
        int at = start + 4;
        for (int i = 0; i < count; i++)
        {
            context.Need(at, count: 1, end, path: path.Item(i), what: "nil byte");
            if (context.Data[at++] != 0)
            {
                entries.Add(null);
                continue;
            }
            List<SequenceLayer> layers = [];
            bool next;
            do
            {
                DocumentNodePath layerPath = path.Item(i).Item(layers.Count);
                context.Need(at, count: 4, end, path: layerPath, what: "library name length");
                int length = context.Int32(at);
                at += 4;
                if (length < 0)
                {
                    throw context.Fail(Codes.Corrupt, message: "A library name has a negative length.", offset: at - 4, path: layerPath);
                }

                if (length > context.Limits.StringBytes)
                {
                    throw context.Fail(Codes.Limit, message: "A string is too long.", offset: at - 4, path: layerPath, value: Text(length), constraint: $"at most {context.Limits.StringBytes} bytes");
                }

                context.Need(at, count: length, end, path: layerPath, what: "library name");
                string library = TextCodec.Decode(context.Data.AsSpan(at, length));
                at += length;
                context.Need(at, count: 15, end, path: layerPath, what: "layer");
                byte mode = context.Data[at + 2];
                SequenceLayer layer = new()
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
                    context.Need(at, count: 8, end, path: layerPath, what: "frame table header");
                    int defaultValue = context.Int32(at);
                    int frames = context.Int32(at + 4);
                    at += 8;
                    if (frames < 0)
                    {
                        throw context.Fail(Codes.Corrupt, message: "A frame table has a negative count.", offset: at - 4, path: layerPath);
                    }

                    context.CheckItems(frames, offset: at - 4, path: layerPath);
                    context.Need(at, count: 4 * frames, end, path: layerPath, what: "frame table");
                    int[] items = new int[frames];
                    for (int k = 0; k < frames; k++)
                    {
                        items[k] = context.Int32(at + (4 * k));
                    }

                    at += 4 * frames;
                    layer = layer with { Frames = new FrameTable(defaultValue, items) };
                }
                layers.Add(layer);
                context.Need(at, count: 1, end, path: layerPath, what: "has-next byte");
                next = context.Data[at++] != 0;
            }
            while (next);
            entries.Add(layers);
        }
        return at != end ? throw context.Fail(Codes.Corrupt, message: "Bytes follow the image sequences.", offset: at, path) : entries;
    }

    private static byte[] Canonical(IReadOnlyList<IReadOnlyList<SequenceLayer>?> entries)
    {
        ByteBuffer data = new(64);
        data.Int32(entries.Count);
        foreach (IReadOnlyList<SequenceLayer>? layers in entries)
        {
            if (layers is null)
            {
                data.Byte(1);
                continue;
            }
            data.Byte(0);
            for (int k = 0; k < layers.Count; k++)
            {
                SequenceLayer layer = layers[k];
                byte[] name = Cp1252.Encode(layer.Library);
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
                    foreach (int item in frames.Items)
                    {
                        data.Int32(item);
                    }
                }
                data.Byte(k + 1 < layers.Count ? (byte)1 : (byte)0);
            }
        }
        return data.ToArray();
    }

    private JsonArray Document(IReadOnlyList<IReadOnlyList<SequenceLayer>?> entries, string? file, DocumentNodePath path)
    {
        JsonArray array = new();
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] is not { } layers)
            {
                _ = array.Add(new JsonNull());
                continue;
            }
            JsonArray chain = new();
            for (int k = 0; k < layers.Count; k++)
            {
                SequenceLayer layer = layers[k];
                JsonObject item = new();
                if (layer.Library.Length != 0)
                {
                    _ = item.Add("Library", new JsonString(layer.Library));
                }

                if (layer.Layer != 0)
                {
                    _ = item.Add("Layer", new JsonInteger(layer.Layer));
                }

                if (layer.Speed != 0)
                {
                    _ = item.Add("Speed", new JsonInteger(layer.Speed));
                }

                if (layer.Mode != 0)
                {
                    _ = item.Add("Mode", _mode.MemberText(layer.Mode, file, path: path.Item(i).Item(k).Member("Mode").ToString()));
                }

                if (layer.ImageIndex != 0)
                {
                    _ = item.Add("ImageIndex", new JsonInteger(layer.ImageIndex));
                }

                if (layer.OffsetX != 0)
                {
                    _ = item.Add("OffsetX", new JsonInteger(layer.OffsetX));
                }

                if (layer.OffsetY != 0)
                {
                    _ = item.Add("OffsetY", new JsonInteger(layer.OffsetY));
                }

                if (layer.Frames is { } frames)
                {
                    JsonArray items = new();
                    items.Items.AddRange(frames.Items.Select(static frame => new JsonInteger(frame)));

                    _ = item.Add("Frames", new JsonObject().Add("default", new JsonInteger(frames.Default)).Add("items", items));
                }
                _ = chain.Add(item);
            }
            _ = array.Add(chain);
        }
        return array;
    }

    private List<IReadOnlyList<SequenceLayer>?> Entries(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: $"{name} is an array of sequences.", at: value, path, value: value.KindName);
        }

        if (array.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: array, path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        List<IReadOnlyList<SequenceLayer>?> entries = [with(array.Items.Count)];
        for (int i = 0; i < array.Items.Count; i++)
        {
            DocumentNodePath at = path.Item(i);
            switch (array.Items[i])
            {
                case JsonNull:
                    entries.Add(null);
                    break;
                case JsonArray { Items.Count: > 0 } chain:
                    if (chain.Items.Count > context.Limits.CollectionItems)
                    {
                        throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: chain, path: at, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
                    }

                    List<SequenceLayer> layers = [with(chain.Items.Count)];
                    layers.AddRange(chain.Items.Select((value, index) => Layer(context, value: value, path: at.Item(index))));

                    entries.Add(layers);
                    break;
                case JsonArray empty:
                    throw context.Fail(Codes.Type, message: "A sequence has at least one layer.", at: empty, path: at);
                default:
                    throw context.Fail(Codes.Type, message: "An entry is null or an array of layers.", at: array.Items[i], path: at, value: array.Items[i].KindName);
            }
        }
        return entries;
    }

    private SequenceLayer Layer(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonObject layer)
        {
            throw context.Fail(Codes.Type, message: "A layer is an object.", at: value, path, value: value.KindName);
        }

        foreach (JsonMember member in layer.Members)
        {
            if (Array.IndexOf(s_members, value: member.Name) < 0)
            {
                throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    message: $"Member \"{member.Name}\" is not allowed in a layer.", at: member.Value, path: path.Member(member.Name), value: member.Name);
            }
        }

        string library = "";
        if (layer["Library"] is { } libraryValue)
        {
            if (libraryValue is not JsonString text)
            {
                throw context.Fail(Codes.Type, message: "Library is a string.", at: libraryValue, path: path.Member("Library"), value: libraryValue.KindName);
            }

            _ = TextCodec.Encode(context, text: text.Value, at: text, path: path.Member("Library"));
            library = text.Value;
        }
        byte mode = layer["Mode"] is { } modeValue ? (byte)_mode.Parse(context, value: modeValue, path: path.Member("Mode"), name: "Mode") : (byte)0;
        FrameTable? frames = null;
        if (layer["Frames"] is { } framesValue)
        {
            if (!SequenceLayer.HasFrameTable(mode))
            {
                throw context.Fail(Codes.UnknownMember, message: "Frames belongs only to a FrameTable or RandomFrameTable layer.", at: framesValue, path: path.Member("Frames"), value: "Frames");
            }

            frames = Frames(context, value: framesValue, path: path.Member("Frames"));
        }
        else if (SequenceLayer.HasFrameTable(mode))
        {
            throw context.Fail(Codes.MissingMember, message: "A FrameTable or RandomFrameTable layer needs Frames.", at: layer, path);
        }

        return new SequenceLayer
        {
            Library = library,
            Layer = (byte)Integer(context, value: layer["Layer"], path: path.Member("Layer"), min: 0, max: 255),
            Speed = (byte)Integer(context, value: layer["Speed"], path: path.Member("Speed"), min: 0, max: 255),
            Mode = mode,
            ImageIndex = (int)Integer(context, value: layer["ImageIndex"], path: path.Member("ImageIndex"), min: int.MinValue, max: int.MaxValue),
            OffsetX = (int)Integer(context, value: layer["OffsetX"], path: path.Member("OffsetX"), min: int.MinValue, max: int.MaxValue),
            OffsetY = (int)Integer(context, value: layer["OffsetY"], path: path.Member("OffsetY"), min: int.MinValue, max: int.MaxValue),
            Frames = frames,
        };
    }

    private static FrameTable Frames(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is not JsonObject table)
        {
            throw context.Fail(Codes.Type, message: "Frames is { \"default\": ..., \"items\": [...] }.", at: value, path, value: value.KindName);
        }

        foreach (JsonMember member in table.Members)
        {
            if (member.Name is not ("default" or "items"))
            {
                throw context.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    message: $"Member \"{member.Name}\" is not allowed in a frame table.", at: member.Value, path: path.Member(member.Name), value: member.Name);
            }
        }

        int defaultValue = (int)Integer(context, value: table["default"] ?? throw context.Fail(Codes.MissingMember, message: "Frames needs default.", at: table, path),
            path: path.Member("default"), min: int.MinValue, max: int.MaxValue);
        JsonArray items = table["items"] as JsonArray ?? throw context.Fail(Codes.MissingMember, message: "items is an array.", at: table, path);
        if (items.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: items, path: path.Member("items"), value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        int[] frames = new int[items.Items.Count];
        for (int k = 0; k < frames.Length; k++)
        {
            frames[k] = (int)Integer(context, value: items.Items[k], path: path.Member("items").Item(k), min: int.MinValue, max: int.MaxValue);
        }

        return new FrameTable(defaultValue, Items: frames);
    }

    private static long Integer(EncodeContext context, JsonValue? value, DocumentNodePath path, long min, long max)
    {
        return value is null
            ? 0
            : value is not JsonInteger number
            ? throw context.Fail(Codes.Type, message: "The value is an integer.", at: value, path, value: value.KindName)
            : number.Value < min || number.Value > max
            ? throw context.Fail(Codes.Range, message: "The value is outside its storage range.", at: value, path, value: Text(number.Value), constraint: $"{min} to {max}")
            : number.Value;
    }
}
