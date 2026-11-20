using System.Buffers.Binary;
using Aow.Formats.Json;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>
/// Direct image readers and writers for every leaf the tree stores.
/// These methods move bytes and tree nodes. They do not build a JSON document.
/// </summary>
internal sealed partial class IntegerCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        long number;
        if (value is IntegerNode integer)
        {
            if (integer.Width != width || integer.Signed != signed)
            {
                throw context.Fail(Codes.Type, message: $"{ImageName(frame)} is an integer.", at: null, frame.Path);
            }

            number = integer.Value;
        }
        else if (value is ReferenceNode reference && width == 4 && signed)
        {
            if (domain is not null && reference.Domain != domain)
            {
                throw context.Fail(Codes.Reference, message: $"{ImageName(frame)} is in domain {reference.Domain}.", at: null, frame.Path, value: reference.Domain, constraint: domain);
            }

            number = reference.Key;
        }
        else
        {
            throw context.Fail(Codes.Type, message: $"{ImageName(frame)} is an integer.", at: null, frame.Path);
        }

        if (number < Min || number > Max)
        {
            throw context.Fail(Codes.Range, message: $"{ImageName(frame)} is outside its storage range.", at: null, frame.Path, value: Text(number), constraint: $"{Min} to {Max}");
        }

        switch (width)
        {
            case 1: data.Byte((byte)number); break;
            case 2: data.Int16((ushort)number); break;
            default: data.Int32((int)number); break;
        }
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != width)
        {
            throw Mismatch(context, start, site: frame.Value(context), message: $"An integer of {width} bytes is stored in {end - start} bytes.");
        }

        Span<byte> bytes = context.Data.AsSpan(start, length: width);
        long number = width switch
        {
            1 => signed ? (sbyte)bytes[0] : bytes[0],
            2 => signed ? BinaryPrimitives.ReadInt16LittleEndian(bytes) : BinaryPrimitives.ReadUInt16LittleEndian(bytes),
            _ => signed ? BinaryPrimitives.ReadInt32LittleEndian(bytes) : BinaryPrimitives.ReadUInt32LittleEndian(bytes),
        };
        return number < Min || number > Max
            ? throw context.Fail(Codes.Range, message: $"{ImageName(frame)} is outside its storage range.", offset: start, frame.Path, value: Text(number), constraint: $"{Min} to {Max}")
            : domain is not null ? new ReferenceNode(domain, key: (int)number) : new IntegerNode(width, signed, value: number);
    }

    private static string ImageName(ImageFrame frame) => frame.Property?.Name ?? "The value";
}

internal sealed partial class BooleanCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BooleanNode)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a boolean.", at: null, frame.Path);
        }
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame) => new BooleanNode(true);
}

internal sealed partial class StringCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not StringNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a string.", at: null, frame.Path);
        }

        byte[] bytes = TextCodec.Bytes(context, text: node.Value, frame.Path);
        if (bytes.Length >= 255)
        {
            data.Byte(255);
            data.Int32(bytes.Length);
        }
        else
        {
            data.Byte((byte)bytes.Length);
        }

        data.Bytes(bytes);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        ValueSite site = frame.Value(context);
        context.Need(start, count: 1, end, site.Path, what: "string length");
        int at = start + 1;
        int length = context.Data[start];
        bool longPrefix = length == 255;
        if (longPrefix)
        {
            context.Need(at, count: 4, end, site.Path, what: "string length");
            length = context.Int32(at);
            at += 4;
        }
        return length < 0 || at + (long)length != end
            ? throw Mismatch(context, start, site, message: $"A string of {length} bytes does not fill its {end - start} stored bytes.")
            : length > context.Limits.StringBytes
            ? throw context.Fail(Codes.Limit, message: "A string is too long.", offset: start, site.Path, value: Text(length), constraint: $"at most {context.Limits.StringBytes} bytes")
            : (TreeValue)new StringNode(TextCodec.Decode(context.Data.AsSpan(at, length)));
    }
}

internal partial class StringsCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not LinesNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is lines.", at: null, frame.Path);
        }

        byte[] bytes = TextCodec.Bytes(text: node.RawText ?? Join(node.Lines ?? []), context: context, path: frame.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        ValueSite site = frame.Value(context);
        (string? text, int after) = Block(context, start, end, site);
        return after != end ? throw Mismatch(context, start, site, message: "Bytes follow the text block.") : (TreeValue)LinesNode.FromText(text);
    }
}

internal sealed partial class StringsWithValuesCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not LinesWithValuesNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is lines with objects.", at: null, frame.Path);
        }

        string text = node.RawText ?? Join(node.Lines ?? []);
        int lines = LineCount(text);
        if (lines != node.Objects.Count)
        {
            throw context.Fail(Codes.Corrupt, message: $"The text has {lines} lines and {node.Objects.Count} object values.", at: null, frame.Path);
        }

        byte[] bytes = TextCodec.Bytes(context, text, frame.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
        foreach (int id in node.Objects)
        {
            data.Int32(id);
        }
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        ValueSite site = frame.Value(context);
        (string? text, int after) = Block(context, start, end, site);
        if ((end - after) % 4 != 0)
        {
            throw Mismatch(context, start, site, message: "The object values are not whole 32-bit integers.");
        }

        int count = (end - after) / 4;
        if (count != LineCount(text))
        {
            throw Mismatch(context, start, site, message: $"The text has {LineCount(text)} lines and {count} object values.");
        }

        int[] ids = new int[count];
        for (int i = 0; i < count; i++)
        {
            ids[i] = context.Int32(after + (4 * i));
        }

        return LinesWithValuesNode.FromText(text, objects: ids);
    }
}

internal sealed partial class DataCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BytesNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is lowercase hexadecimal bytes.", at: null, frame.Path);
        }

        if (Size is int n && node.Data.Length != n)
        {
            throw context.Fail(Codes.Range, message: $"{frame.Property?.Name ?? "The value"} holds exactly {n} bytes.", at: null, frame.Path, value: Text(node.Data.Length), constraint: $"{n} bytes");
        }

        if (node.Data.Length > context.Limits.DecompressedBytes)
        {
            throw context.Fail(Codes.Limit, message: "A payload is too large.", at: null, frame.Path);
        }

        data.Bytes(node.Data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        int length = end - start;
        if (Size is int n && length != n)
        {
            throw Mismatch(context, start, site: frame.Value(context), message: $"A record of {n} bytes is stored in {length} bytes.");
        }

        if (length > context.Limits.DecompressedBytes)
        {
            throw context.Fail(Codes.Limit, message: "A payload is too large.", offset: start, frame.Path);
        }

        byte[] bytes = new byte[length];
        context.Data.AsSpan(start, length).CopyTo(bytes);
        return new BytesNode(bytes);
    }
}

internal sealed partial class EnumCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not EnumNode node || node.TypeName != TypeName || node.Width != Width || node.Signed != Signed)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a {TypeName}.", at: null, frame.Path);
        }

        if (node.Value < IntegerBytes.Min(Width, Signed) || node.Value > IntegerBytes.Max(Width, Signed))
        {
            throw context.Fail(Codes.Range, message: $"{frame.Property?.Name ?? "The value"} is outside its storage range.", at: null, frame.Path, value: Text(node.Value), constraint: Range());
        }

        IntegerBytes.Write(data, node.Value, Width);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        return end - start != Width
            ? throw Mismatch(context, start, site: frame.Value(context), message: $"An enum of {Width} bytes is stored in {end - start} bytes.")
            : (TreeValue)new EnumNode(TypeName, Width, Signed, value: Read(context.Data.AsSpan(start, length: Width)));
    }
}

internal sealed partial class SetCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not SetNode node || node.TypeName != TypeName || node.Width != Width)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a set of {TypeName}.", at: null, frame.Path);
        }

        IntegerBytes.Write(data, value: unchecked((long)node.Mask), Width);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        return end - start != Width
            ? throw Mismatch(context, start, site: frame.Value(context), message: $"A set of {Width} bytes is stored in {end - start} bytes.")
            : (TreeValue)new SetNode(TypeName, Width, mask: Mask(context.Data.AsSpan(start, length: Width)));
    }
}

internal sealed partial class FixedArrayCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a list of integers.", at: null, frame.Path);
        }

        string name = frame.Property?.Name ?? "The value";
        if (node.Items.Count > Count)
        {
            throw context.Fail(Codes.Range, message: $"{name} holds at most {Count} values. Export does not truncate.", at: null, frame.Path, value: Text(node.Items.Count), constraint: $"{Count} values");
        }

        for (int i = 0; i < Count; i++)
        {
            if (i >= node.Items.Count)
            {
                IntegerBytes.Write(data, value: 0, Width);
                continue;
            }
            if (node.Items[i] is not IntegerNode integer || integer.Width != Width || integer.Signed != Signed)
            {
                throw context.Fail(Codes.Type, message: $"{name} is an integer.", at: null, path: frame.Path.Item(i));
            }

            Check(context, number: integer.Value, path: frame.Path.Item(i), name);
            IntegerBytes.Write(data, integer.Value, Width);
        }
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        int bytes = Count * Width;
        if (end - start != bytes)
        {
            throw Mismatch(context, start, site: frame.Value(context), message: $"An array of {Count} integers is stored in {end - start} bytes.");
        }

        ListNode node = new();
        for (int i = 0; i < Count; i++)
        {
            node.Items.Add(new IntegerNode(Width, Signed, value: IntegerBytes.Read(context.Data.AsSpan(start + (i * Width), length: Width), Width, Signed)));
        }

        return node;
    }
}

internal sealed partial class RawBooleanCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame) => data.Byte(ImageByte(context, value, frame));

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != 1)
        {
            throw Mismatch(context, start, site: frame.Value(context), message: $"A raw boolean is stored in {end - start} bytes.");
        }

        byte stored = context.Data[start];
        return stored is 0 or 1 ? new BooleanNode(stored != 0) : new IntegerNode(1, signed: false, value: stored);
    }

    private static byte ImageByte(EncodeContext context, TreeValue value, ImageFrame frame)
    {
        string name = frame.Property?.Name ?? "The value";
        if (value is BooleanNode bit)
        {
            return bit.Value ? (byte)1 : (byte)0;
        }

        if (value is IntegerNode { Width: 1, Signed: false, Value: >= 0 and <= 255 } number)
        {
            return (byte)number.Value;
        }

        if (value is IntegerNode integer)
        {
            throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: null, frame.Path, value: Text(integer.Value), constraint: "0 to 255");
        }

        throw context.Fail(Codes.Type, message: $"{name} is a boolean.", at: null, frame.Path);
    }
}

/// <summary>
/// A boolean field inside a record. 0 is absent, 1 is true, and any other byte stays that integer.
/// The tree stores the byte as an integer node, including 1.
/// </summary>
internal sealed class RecordBooleanCodec(string name) : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        if (end - start != 1)
        {
            throw Mismatch(context, start, site, message: $"{name} is one byte.");
        }

        byte stored = context.Data[start];
        return stored == 0 ? new JsonBoolean(false) : stored == 1 ? new JsonBoolean(true) : new JsonInteger(stored);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) => data.Byte(Stored(context, value, site.Path));

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is BooleanNode bit
            ? bit.Value ? new JsonBoolean(true) : null
            : value is IntegerNode { Width: 1, Signed: false } number
            ? number.Value == 0
                ? null
                : number.Value == 1
                ? new JsonBoolean(true)
                : number.Value is < 0 or > 255
                ? throw site.Fail(Codes.Range, message: $"{site.Property?.Name ?? name} is outside its storage range.", value: Text(number.Value), constraint: "0 to 255")
                : (JsonValue)new JsonInteger(number.Value)
            : throw site.Mismatch($"{site.Property?.Name ?? name} is a boolean.");
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        return value is JsonBoolean bit
            ? bit.Value ? new IntegerNode(1, signed: false, value: 1) : null
            : value is JsonInteger number
            ? number.Value is < 0 or > 255
                ? throw site.Fail(Codes.Range, message: $"{site.Property?.Name ?? name} is outside its storage range.", at: value, value: Text(number.Value), constraint: "0 to 255")
                : number.Value == 0 ? null : (TreeValue)new IntegerNode(1, signed: false, number.Value)
            : throw site.Mismatch($"{site.Property?.Name ?? name} is a boolean.", at: value);
    }

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame) => data.Byte(ImageByte(context, value, frame));

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        return end - start != 1
            ? throw Mismatch(context, start, site: frame.Value(context), message: $"{name} is one byte.")
            : (TreeValue)new IntegerNode(1, signed: false, value: context.Data[start]);
    }

    private byte Stored(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is JsonBoolean bit)
        {
            return bit.Value ? (byte)1 : (byte)0;
        }

        if (value is JsonInteger { Value: >= 0 and <= 255 } number)
        {
            return (byte)number.Value;
        }

        if (value is JsonInteger integer)
        {
            throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: integer, path, value: Text(integer.Value), constraint: "0 to 255");
        }

        throw context.Fail(Codes.Type, message: $"{name} is a boolean.", at: value, path, value: value.KindName);
    }

    private static byte ImageByte(EncodeContext context, TreeValue value, ImageFrame frame)
    {
        if (value is BooleanNode bit)
        {
            return bit.Value ? (byte)1 : (byte)0;
        }

        if (value is IntegerNode { Width: 1, Signed: false, Value: >= 0 and <= 255 } number)
        {
            return (byte)number.Value;
        }

        if (value is IntegerNode integer)
        {
            throw context.Fail(Codes.Range, message: $"{frame.Property?.Name ?? "The value"} is outside its storage range.", at: null, frame.Path, value: Text(integer.Value), constraint: "0 to 255");
        }

        throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a boolean.", at: null, frame.Path);
    }
}

internal sealed partial class CatalogRecordCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (context.Schema.Version < 3)
        {
            throw context.Fail(Codes.Type, message: $"Version {context.Schema.Version} does not store a record.", at: null, frame.Path);
        }

        RecordTypeInfo info = Info(context.File, path: frame.Path.ToString());
        if (info.BareSet)
        {
            FieldCodec(info.Fields[0]).WriteImage(value, context, data, frame);
            return;
        }
        if (value is not RecordNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property?.Name ?? TypeName} is a record.", at: null, frame.Path);
        }

        HashSet<string> seen = [with(StringComparer.Ordinal)];
        byte[] bytes = new byte[info.Size];
        foreach (RecordFieldInfo field in info.Fields)
        {
            _ = seen.Add(field.Name);
            TreeValue? found = node.Find(field.Name);
            if (found is null || field.OmitsImage(found))
            {
                continue;
            }

            ImageFrame fieldFrame = FieldFrame(frame, fieldName: field.Name);
            ByteBuffer buffer = new(field.Size);
            FieldCodec(field).WriteImage(found, context, data: buffer, frame: fieldFrame);
            if (buffer.Length != field.Size)
            {
                throw context.Fail(Codes.Range, message: $"{field.Name} holds exactly {field.Size} bytes.", at: null, fieldFrame.Path, value: Text(buffer.Length), constraint: $"{field.Size} bytes");
            }

            buffer.Span.CopyTo(bytes.AsSpan(field.Offset, length: field.Size));
        }

        foreach ((string? memberName, _) in node.Members)
        {
            if (!seen.Contains(memberName))
            {
                throw context.Fail(Codes.UnknownMember, message: $"The record has no member \"{memberName}\".", at: null, path: frame.Path.Member(memberName), value: memberName);
            }
        }

        data.Bytes(bytes);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (context.Schema.Version < 3)
        {
            throw context.Fail(Codes.Type, message: $"Version {context.Schema.Version} does not store a record.", offset: start, frame.Path);
        }

        RecordTypeInfo info = Info(context.File, path: frame.Path.ToString());
        if (end - start != info.Size)
        {
            throw Mismatch(context, start, site: frame.Value(context), message: $"A record of {info.Size} bytes is stored in {end - start} bytes.");
        }

        if (info.BareSet)
        {
            return FieldCodec(info.Fields[0]).ReadImage(context, start, end, frame);
        }

        RecordNode node = new();
        foreach (RecordFieldInfo field in info.Fields)
        {
            TreeValue read = FieldCodec(field).ReadImage(context, start: start + field.Offset, end: start + field.Offset + field.Size, frame: FieldFrame(frame, fieldName: field.Name));
            if (!field.OmitsImage(read))
            {
                node.Add(field.Name, read);
            }
        }

        return node;
    }

    private static ImageFrame FieldFrame(ImageFrame frame, string fieldName) => new()
    {
        Property = frame.Property,
        Id = frame.Id,
        Path = frame.Path.Member(fieldName),
        Owner = frame.Owner,
        Parent = frame.Parent,
    };
}
