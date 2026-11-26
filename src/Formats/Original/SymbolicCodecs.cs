using System.Buffers.Binary;
using Aow.Formats.Json;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

internal static class IntegerBytes
{
    public static long Read(ReadOnlySpan<byte> bytes, int width, bool signed) => width switch
    {
        1 => signed ? (sbyte)bytes[0] : bytes[0],
        2 => signed ? BinaryPrimitives.ReadInt16LittleEndian(bytes) : BinaryPrimitives.ReadUInt16LittleEndian(bytes),
        4 => signed ? BinaryPrimitives.ReadInt32LittleEndian(bytes) : BinaryPrimitives.ReadUInt32LittleEndian(bytes),
        _ => throw new InvalidOperationException("An integer width is 1, 2 or 4 bytes."),
    };

    public static void Write(ByteBuffer data, long value, int width)
    {
        switch (width)
        {
            case 1: data.Byte((byte)value); break;
            case 2: data.Int16((ushort)value); break;
            default: data.Int32((int)value); break;
        }
    }

    public static long Min(int width, bool signed) => signed ? -(1L << ((width * 8) - 1)) : 0;

    public static long Max(int width, bool signed) => signed ? (1L << ((width * 8) - 1)) - 1 : (1L << (width * 8)) - 1;

    public static byte[] Bytes(long value, int width)
    {
        ByteBuffer data = new(width);
        Write(data, value, width);
        return data.ToArray();
    }
}

/// <summary>One enum byte or wider integer. A declared value is its C# member name. Any other value is its integer.</summary>
internal sealed partial class EnumCodec(string typeName, int width, bool signed) : PropertyCodec
{
    public string TypeName { get; } = typeName;
    public int Width { get; } = width;
    public bool Signed { get; } = signed;

    private EnumTypeInfo? _known;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        return end - start != Width
            ? throw Mismatch(context, start, site, message: $"An enum of {Width} bytes is stored in {end - start} bytes.")
            : MemberText(Read(context.Data.AsSpan(start, length: Width)), context.File, path: site.Path.ToString());
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) => IntegerBytes.Write(value: Parse(context, value, site.Path, site.Property.Name), data: data, width: Width);

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not EnumNode node || node.TypeName != TypeName || node.Width != Width || node.Signed != Signed
            ? throw site.Mismatch($"{Name(site)} is a {TypeName}.")
            : node.Value < IntegerBytes.Min(Width, Signed) || node.Value > IntegerBytes.Max(Width, Signed)
            ? throw site.Fail(Codes.Range, message: $"{Name(site)} is outside its storage range.", value: TextNumber(node.Value), constraint: Range())
            : MemberText(node.Value, site.Context.File, path: site.Path.ToString());
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) => new EnumNode(value: Parse(site.Context, value, site.Path, name: Name(site)), typeName: TypeName, width: Width, signed: Signed);

    internal long Parse(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        EnumTypeInfo type = Catalog(context.File, path: path.ToString());
        long number;
        if (value is JsonString text)
        {
            if (!type.TryNumber(text.Value, out number))
            {
                throw context.Fail(Codes.Type, message: $"{name} is not a value of {TypeName}.", at: value, path, text.Value);
            }
        }
        else
        {
            number = value is JsonInteger integer
                ? integer.Value
                : throw context.Fail(Codes.Type, message: $"{name} is a {TypeName} name or an integer.", at: value, path, value: value.KindName);
        }

        return number < IntegerBytes.Min(Width, Signed) || number > IntegerBytes.Max(Width, Signed)
            ? throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: value, path, value: TextNumber(number), constraint: Range())
            : number;
    }

    internal JsonValue MemberText(long number, string? file, string path)
    {
        EnumTypeInfo type = Catalog(file, path);
        return type.ByValue.TryGetValue(number, out string? name) ? new JsonString(name) : new JsonInteger(number);
    }

    internal byte[] Raw(long number) => IntegerBytes.Bytes(number, Width);

    internal long Read(ReadOnlySpan<byte> bytes) => IntegerBytes.Read(bytes, Width, Signed);

    private EnumTypeInfo Catalog(string? file, string path)
    {
        if (_known is not null)
        {
            return _known;
        }

        EnumTypeInfo type = EnumCatalog.Require(TypeName, file, path);
        if (type.Width != Width || type.Signed != Signed)
        {
            throw new InvalidOperationException($"Enum {TypeName} is {type.Width} bytes in the catalog and {Width} bytes in the schema.");
        }

        _known = type;
        return type;
    }

    private string Range() => $"{IntegerBytes.Min(Width, Signed)} to {IntegerBytes.Max(Width, Signed)}";

    private static string TextNumber(long number) => number.ToString(System.Globalization.CultureInfo.InvariantCulture);

    private static string Name(DocumentSite site) => site.Property?.Name ?? "The value";
}

/// <summary>A set stored in a fixed number of bytes. The document is member names in ascending ordinal order, and integers for bits the enum does not name.</summary>
internal sealed partial class SetCodec(string typeName, int width) : PropertyCodec
{
    public string TypeName { get; } = typeName;
    public int Width { get; } = width;

    private EnumTypeInfo? _known;

    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        return end - start != Width
            ? throw Mismatch(context, start, site, message: $"A set of {Width} bytes is stored in {end - start} bytes.")
            : (JsonValue)Members(Mask(context.Data.AsSpan(start, length: Width)), context.File, path: site.Path.ToString());
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) => IntegerBytes.Write(data, value: (long)Parse(context, value, site.Path, site.Property.Name), Width);

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not SetNode node || node.TypeName != TypeName || node.Width != Width
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a set of {TypeName}.")
            : (JsonValue)Members(node.Mask, site.Context.File, path: site.Path.ToString());
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
        => new SetNode(TypeName, Width, mask: Parse(site.Context, value, site.Path, name: site.Property?.Name ?? "The value"));

    internal JsonArray Members(ulong mask, string? file, string path)
    {
        EnumTypeInfo type = Catalog(file, path);
        JsonArray array = new();
        int bits = Width * 8;
        for (int bit = 0; bit < bits; bit++)
        {
            if ((mask & (1UL << bit)) == 0)
            {
                continue;
            }

            _ = array.Add(type.ByValue.TryGetValue(bit, out string? name) ? new JsonString(name) : new JsonInteger(bit));
        }
        return array;
    }

    internal ulong Parse(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: $"{name} is an array of {TypeName} names.", at: value, path, value: value.KindName);
        }

        EnumTypeInfo type = Catalog(context.File, path: path.ToString());
        ulong mask = 0;
        int bits = Width * 8;
        HashSet<int> seen = [];
        for (int i = 0; i < array.Items.Count; i++)
        {
            JsonValue item = array.Items[i];
            DocumentNodePath at = path.Item(i);
            long ordinal;
            if (item is JsonString text)
            {
                if (!type.TryNumber(text.Value, number: out ordinal))
                {
                    throw context.Fail(Codes.Type, message: $"{name} is not a value of {TypeName}.", at: item, path: at, text.Value);
                }
            }
            else
            {
                ordinal = item is JsonInteger integer
                    ? integer.Value
                    : throw context.Fail(Codes.Type, message: $"{name} is a {TypeName} name or an integer.", at: item, path: at, value: item.KindName);
            }

            if (ordinal < 0 || ordinal >= bits)
            {
                throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: item, path: at, value: ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"0 to {bits - 1}");
            }

            if (!seen.Add((int)ordinal))
            {
                throw context.Fail(Codes.Duplicate, message: $"{name} lists bit {ordinal} twice.", at: item, path: at);
            }

            mask |= 1UL << (int)ordinal;
        }
        return mask;
    }

    private EnumTypeInfo Catalog(string? file, string path)
    {
        if (_known is not null)
        {
            return _known;
        }

        _known = EnumCatalog.Require(TypeName, file, path);
        return _known;
    }

    internal static ulong Mask(ReadOnlySpan<byte> bytes)
    {
        ulong mask = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            mask |= (ulong)bytes[i] << (8 * i);
        }

        return mask;
    }
}

/// <summary>
/// A fixed integer array. Version 2 stores every element, including trailing zeros.
/// A longer list is kept by the document and refused on export. A shorter list fills the rest with zero on export.
/// </summary>
internal sealed partial class FixedArrayCodec(int width, bool signed, int count) : PropertyCodec
{
    public int Width { get; } = width;
    public bool Signed { get; } = signed;
    public int Count { get; } = count;

    internal override bool AcceptsList(int version) => version >= 2;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        int bytes = Count * Width;
        if (end - start != bytes)
        {
            throw Mismatch(context, start, site, message: $"An array of {Count} integers is stored in {end - start} bytes.");
        }

        JsonArray array = new();
        for (int i = 0; i < Count; i++)
        {
            _ = array.Add(new JsonInteger(IntegerBytes.Read(context.Data.AsSpan(start + (i * Width), length: Width), Width, Signed)));
        }

        return array;
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        List<long> numbers = Values(context, value, site.Path, site.Property.Name, refuseLong: true);
        for (int i = 0; i < Count; i++)
        {
            IntegerBytes.Write(data, value: i < numbers.Count ? numbers[i] : 0, Width);
        }
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version < 2)
        {
            throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} does not store a list.");
        }

        if (value is not ListNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a list of integers.");
        }

        JsonArray array = new();
        for (int i = 0; i < node.Items.Count; i++)
        {
            DocumentSite item = site.At(site.Path.Item(i), element: true);
            if (node.Items[i] is not IntegerNode integer || integer.Width != Width || integer.Signed != Signed)
            {
                throw item.Mismatch($"{site.Property?.Name ?? "The value"} is an integer.");
            }

            Check(site.Context, number: integer.Value, item.Path, name: site.Property?.Name ?? "The value");
            _ = array.Add(new JsonInteger(integer.Value));
        }
        return array;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version < 2)
        {
            throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} does not store a list.", at: value);
        }

        List<long> numbers = Values(site.Context, value, site.Path, name: site.Property?.Name ?? "The value", refuseLong: false);
        ListNode node = new();
        node.Items.AddRange(numbers.Select(number => new IntegerNode(Width, signed: Signed, value: number)));

        return node;
    }

    private List<long> Values(EncodeContext context, JsonValue value, DocumentNodePath path, string name, bool refuseLong)
    {
        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: $"{name} is an array of integers.", at: value, path, value: value.KindName);
        }

        if (array.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: array, path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        if (refuseLong && array.Items.Count > Count)
        {
            throw context.Fail(Codes.Range, message: $"{name} holds at most {Count} values. Export does not truncate.", at: array, path,
                value: array.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{Count} values");
        }

        List<long> numbers = [with(array.Items.Count)];
        for (int i = 0; i < array.Items.Count; i++)
        {
            if (array.Items[i] is not JsonInteger integer)
            {
                throw context.Fail(Codes.Type, message: $"{name} is an integer.", at: array.Items[i], path: path.Item(i), value: array.Items[i].KindName);
            }

            Check(context, number: integer.Value, path: path.Item(i), name);
            numbers.Add(integer.Value);
        }
        return numbers;
    }

    private void Check(EncodeContext context, long number, DocumentNodePath path, string name)
    {
        if (number < IntegerBytes.Min(Width, Signed) || number > IntegerBytes.Max(Width, Signed))
        {
            throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: null, path, value: number.ToString(System.Globalization.CultureInfo.InvariantCulture),
                constraint: $"{IntegerBytes.Min(Width, Signed)} to {IntegerBytes.Max(Width, Signed)}");
        }
    }
}
