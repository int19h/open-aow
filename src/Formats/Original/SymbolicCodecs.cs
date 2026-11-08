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

    public static long Min(int width, bool signed) => signed ? -(1L << (width * 8 - 1)) : 0;

    public static long Max(int width, bool signed) => signed ? (1L << (width * 8 - 1)) - 1 : (1L << (width * 8)) - 1;

    public static byte[] Bytes(long value, int width)
    {
        var data = new ByteBuffer(width);
        Write(data, value, width);
        return data.ToArray();
    }
}

/// <summary>One enum byte or wider integer. A declared value is its C# member name. Any other value is its integer.</summary>
internal sealed partial class EnumCodec : PropertyCodec
{
    public EnumCodec(string typeName, int width, bool signed)
    {
        TypeName = typeName;
        Width = width;
        Signed = signed;
    }

    public string TypeName { get; }
    public int Width { get; }
    public bool Signed { get; }

    private EnumTypeInfo? known;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        if (end - start != Width)
            throw Mismatch(ctx, start, site, "An enum of " + Width + " bytes is stored in " + (end - start) + " bytes.");
        return MemberText(Read(ctx.Data.AsSpan(start, Width)), ctx.File, site.Path.ToString());
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        long number = Parse(ctx, value, site.Path, site.Property.Name);
        IntegerBytes.Write(data, number, Width);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not EnumNode node || node.TypeName != TypeName || node.Width != Width || node.Signed != Signed)
            throw site.Mismatch(Name(site) + " is a " + TypeName + ".");
        if (node.Value < IntegerBytes.Min(Width, Signed) || node.Value > IntegerBytes.Max(Width, Signed))
            throw site.Fail(Codes.Range, Name(site) + " is outside its storage range.", value: TextNumber(node.Value), constraint: Range());
        return MemberText(node.Value, site.Context.File, site.Path.ToString());
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        long number = Parse(site.Context, value, site.Path, Name(site));
        return new EnumNode(TypeName, Width, Signed, number);
    }

    internal long Parse(EncodeContext ctx, JsonValue value, DocPath path, string name)
    {
        var type = Catalog(ctx.File, path.ToString());
        long number;
        if (value is JsonString text)
        {
            if (!type.ByName.TryGetValue(text.Value, out number))
                throw ctx.Fail(Codes.Type, name + " is not a value of " + TypeName + ".", value, path, text.Value);
        }
        else if (value is JsonInteger integer)
        {
            number = integer.Value;
        }
        else throw ctx.Fail(Codes.Type, name + " is a " + TypeName + " name or an integer.", value, path, value.KindName);
        if (number < IntegerBytes.Min(Width, Signed) || number > IntegerBytes.Max(Width, Signed))
            throw ctx.Fail(Codes.Range, name + " is outside its storage range.", value, path, TextNumber(number), Range());
        return number;
    }

    internal JsonValue MemberText(long number, string? file, string path)
    {
        var type = Catalog(file, path);
        return type.ByValue.TryGetValue(number, out var name) ? new JsonString(name) : new JsonInteger(number);
    }

    internal byte[] Raw(long number) => IntegerBytes.Bytes(number, Width);

    internal long Read(ReadOnlySpan<byte> bytes) => IntegerBytes.Read(bytes, Width, Signed);

    private EnumTypeInfo Catalog(string? file, string path)
    {
        if (known is not null) return known;
        var type = EnumCatalog.Require(TypeName, file, path);
        if (type.Width != Width || type.Signed != Signed)
            throw new InvalidOperationException("Enum " + TypeName + " is " + type.Width + " bytes in the catalog and " + Width + " bytes in the schema.");
        known = type;
        return type;
    }

    private string Range() => IntegerBytes.Min(Width, Signed) + " to " + IntegerBytes.Max(Width, Signed);

    private static string TextNumber(long number) => number.ToString(System.Globalization.CultureInfo.InvariantCulture);

    private static string Name(DocumentSite site) => site.Property?.Name ?? "The value";
}

/// <summary>A set stored in a fixed number of bytes. The document is member names in ascending ordinal order, and integers for bits the enum does not name.</summary>
internal sealed partial class SetCodec : PropertyCodec
{
    public SetCodec(string typeName, int width)
    {
        TypeName = typeName;
        Width = width;
    }

    public string TypeName { get; }
    public int Width { get; }

    private EnumTypeInfo? known;

    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        if (end - start != Width)
            throw Mismatch(ctx, start, site, "A set of " + Width + " bytes is stored in " + (end - start) + " bytes.");
        return Members(Mask(ctx.Data.AsSpan(start, Width)), ctx.File, site.Path.ToString());
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        IntegerBytes.Write(data, (long)Parse(ctx, value, site.Path, site.Property.Name), Width);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not SetNode node || node.TypeName != TypeName || node.Width != Width)
            throw site.Mismatch((site.Property?.Name ?? "The value") + " is a set of " + TypeName + ".");
        return Members(node.Mask, site.Context.File, site.Path.ToString());
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site) =>
        new SetNode(TypeName, Width, Parse(site.Context, value, site.Path, site.Property?.Name ?? "The value"));

    internal JsonArray Members(ulong mask, string? file, string path)
    {
        var type = Catalog(file, path);
        var array = new JsonArray();
        int bits = Width * 8;
        for (int bit = 0; bit < bits; bit++)
        {
            if ((mask & (1UL << bit)) == 0) continue;
            array.Add(type.ByValue.TryGetValue(bit, out var name) ? new JsonString(name) : new JsonInteger(bit));
        }
        return array;
    }

    internal ulong Parse(EncodeContext ctx, JsonValue value, DocPath path, string name)
    {
        if (value is not JsonArray array)
            throw ctx.Fail(Codes.Type, name + " is an array of " + TypeName + " names.", value, path, value.KindName);
        var type = Catalog(ctx.File, path.ToString());
        ulong mask = 0;
        int bits = Width * 8;
        var seen = new HashSet<int>();
        for (int i = 0; i < array.Items.Count; i++)
        {
            var item = array.Items[i];
            var at = path.Item(i);
            long ordinal;
            if (item is JsonString text)
            {
                if (!type.ByName.TryGetValue(text.Value, out ordinal))
                    throw ctx.Fail(Codes.Type, name + " is not a value of " + TypeName + ".", item, at, text.Value);
            }
            else if (item is JsonInteger integer)
            {
                ordinal = integer.Value;
            }
            else throw ctx.Fail(Codes.Type, name + " is a " + TypeName + " name or an integer.", item, at, item.KindName);
            if (ordinal < 0 || ordinal >= bits)
                throw ctx.Fail(Codes.Range, name + " is outside its storage range.", item, at, ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture), "0 to " + (bits - 1));
            if (!seen.Add((int)ordinal))
                throw ctx.Fail(Codes.Duplicate, name + " lists bit " + ordinal + " twice.", item, at);
            mask |= 1UL << (int)ordinal;
        }
        return mask;
    }

    private EnumTypeInfo Catalog(string? file, string path)
    {
        if (known is not null) return known;
        known = EnumCatalog.Require(TypeName, file, path);
        return known;
    }

    internal static ulong Mask(ReadOnlySpan<byte> bytes)
    {
        ulong mask = 0;
        for (int i = 0; i < bytes.Length; i++) mask |= (ulong)bytes[i] << (8 * i);
        return mask;
    }
}

/// <summary>
/// A fixed integer array. Version 2 stores every element, including trailing zeros.
/// A longer list is kept by the document and refused on export. A shorter list fills the rest with zero on export.
/// </summary>
internal sealed partial class FixedArrayCodec : PropertyCodec
{
    public FixedArrayCodec(int width, bool signed, int count)
    {
        Width = width;
        Signed = signed;
        Count = count;
    }

    public int Width { get; }
    public bool Signed { get; }
    public int Count { get; }

    internal override bool AcceptsList(int version) => version >= 2;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        int bytes = Count * Width;
        if (end - start != bytes)
            throw Mismatch(ctx, start, site, "An array of " + Count + " integers is stored in " + (end - start) + " bytes.");
        var array = new JsonArray();
        for (int i = 0; i < Count; i++)
            array.Add(new JsonInteger(IntegerBytes.Read(ctx.Data.AsSpan(start + i * Width, Width), Width, Signed)));
        return array;
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var numbers = Values(ctx, value, site.Path, site.Property.Name, refuseLong: true);
        for (int i = 0; i < Count; i++)
            IntegerBytes.Write(data, i < numbers.Count ? numbers[i] : 0, Width);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version < 2)
            throw site.Fail(Codes.Type, "Version " + site.Schema.Version + " does not store a list.");
        if (value is not ListNode node)
            throw site.Mismatch((site.Property?.Name ?? "The value") + " is a list of integers.");
        var array = new JsonArray();
        for (int i = 0; i < node.Items.Count; i++)
        {
            var item = site.At(site.Path.Item(i), true);
            if (node.Items[i] is not IntegerNode integer || integer.Width != Width || integer.Signed != Signed)
                throw item.Mismatch((site.Property?.Name ?? "The value") + " is an integer.");
            Check(site.Context, integer.Value, item.Path, site.Property?.Name ?? "The value");
            array.Add(new JsonInteger(integer.Value));
        }
        return array;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version < 2)
            throw site.Fail(Codes.Type, "Version " + site.Schema.Version + " does not store a list.", value);
        var numbers = Values(site.Context, value, site.Path, site.Property?.Name ?? "The value", refuseLong: false);
        var node = new ListNode();
        foreach (var number in numbers)
            node.Items.Add(new IntegerNode(Width, Signed, number));
        return node;
    }

    private List<long> Values(EncodeContext ctx, JsonValue value, DocPath path, string name, bool refuseLong)
    {
        if (value is not JsonArray array)
            throw ctx.Fail(Codes.Type, name + " is an array of integers.", value, path, value.KindName);
        if (array.Items.Count > ctx.Limits.CollectionItems)
            throw ctx.Fail(Codes.Limit, "A collection has too many items.", array, path, null, "at most " + ctx.Limits.CollectionItems + " items");
        if (refuseLong && array.Items.Count > Count)
            throw ctx.Fail(Codes.Range, name + " holds at most " + Count + " values. Export does not truncate.", array, path,
                array.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), Count + " values");
        var numbers = new List<long>(array.Items.Count);
        for (int i = 0; i < array.Items.Count; i++)
        {
            if (array.Items[i] is not JsonInteger integer)
                throw ctx.Fail(Codes.Type, name + " is an integer.", array.Items[i], path.Item(i), array.Items[i].KindName);
            Check(ctx, integer.Value, path.Item(i), name);
            numbers.Add(integer.Value);
        }
        return numbers;
    }

    private void Check(EncodeContext ctx, long number, DocPath path, string name)
    {
        if (number < IntegerBytes.Min(Width, Signed) || number > IntegerBytes.Max(Width, Signed))
            throw ctx.Fail(Codes.Range, name + " is outside its storage range.", null, path, number.ToString(System.Globalization.CultureInfo.InvariantCulture),
                IntegerBytes.Min(Width, Signed) + " to " + IntegerBytes.Max(Width, Signed));
    }
}
