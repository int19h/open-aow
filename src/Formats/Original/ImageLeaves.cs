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
                throw context.Fail(Codes.Type, ImageName(frame) + " is an integer.", null, frame.Path);
            number = integer.Value;
        }
        else if (value is ReferenceNode reference && width == 4 && signed)
        {
            if (domain is not null && reference.Domain != domain)
                throw context.Fail(Codes.Reference, ImageName(frame) + " is in domain " + reference.Domain + ".", null, frame.Path, reference.Domain, domain);
            number = reference.Key;
        }
        else throw context.Fail(Codes.Type, ImageName(frame) + " is an integer.", null, frame.Path);
        if (number < Min || number > Max)
            throw context.Fail(Codes.Range, ImageName(frame) + " is outside its storage range.", null, frame.Path, Text(number), Min + " to " + Max);
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
            throw Mismatch(context, start, frame.Value(context), "An integer of " + width + " bytes is stored in " + (end - start) + " bytes.");
        var bytes = context.Data.AsSpan(start, width);
        long number = width switch
        {
            1 => signed ? (sbyte)bytes[0] : bytes[0],
            2 => signed ? BinaryPrimitives.ReadInt16LittleEndian(bytes) : BinaryPrimitives.ReadUInt16LittleEndian(bytes),
            _ => signed ? BinaryPrimitives.ReadInt32LittleEndian(bytes) : BinaryPrimitives.ReadUInt32LittleEndian(bytes),
        };
        if (number < Min || number > Max)
            throw context.Fail(Codes.Range, ImageName(frame) + " is outside its storage range.", start, frame.Path, Text(number), Min + " to " + Max);
        if (domain is not null) return new ReferenceNode(domain, (int)number);
        return new IntegerNode(width, signed, number);
    }

    private static string ImageName(ImageFrame frame) => frame.Property?.Name ?? "The value";
}

internal sealed partial class BooleanCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BooleanNode)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a boolean.", null, frame.Path);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame) => new BooleanNode(true);
}

internal sealed partial class StringCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not StringNode node)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a string.", null, frame.Path);
        var bytes = TextCodec.Bytes(context, node.Value, frame.Path);
        if (bytes.Length >= 255)
        {
            data.Byte(255);
            data.Int32(bytes.Length);
        }
        else data.Byte((byte)bytes.Length);
        data.Bytes(bytes);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var site = frame.Value(context);
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
            throw Mismatch(context, start, site, "A string of " + length + " bytes does not fill its " + (end - start) + " stored bytes.");
        if (length > context.Limits.StringBytes)
            throw context.Fail(Codes.Limit, "A string is too long.", start, site.Path, Text(length), "at most " + context.Limits.StringBytes + " bytes");
        return new StringNode(TextCodec.Decode(context.Data.AsSpan(at, length)));
    }
}

internal partial class StringsCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not LinesNode node)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is lines.", null, frame.Path);
        string text = node.RawText ?? Join(node.Lines ?? []);
        var bytes = TextCodec.Bytes(context, text, frame.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var site = frame.Value(context);
        var (text, after) = Block(context, start, end, site);
        if (after != end) throw Mismatch(context, start, site, "Bytes follow the text block.");
        return LinesNode.FromText(text);
    }
}

internal sealed partial class StringsWithValuesCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not LinesWithValuesNode node)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is lines with objects.", null, frame.Path);
        string text = node.RawText ?? Join(node.Lines ?? []);
        int lines = LineCount(text);
        if (lines != node.Objects.Count)
            throw context.Fail(Codes.Corrupt, "The text has " + lines + " lines and " + node.Objects.Count + " object values.", null, frame.Path);
        var bytes = TextCodec.Bytes(context, text, frame.Path);
        data.Int32(bytes.Length);
        data.Bytes(bytes);
        foreach (var id in node.Objects) data.Int32(id);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var site = frame.Value(context);
        var (text, after) = Block(context, start, end, site);
        if ((end - after) % 4 != 0) throw Mismatch(context, start, site, "The object values are not whole 32-bit integers.");
        int count = (end - after) / 4;
        if (count != LineCount(text)) throw Mismatch(context, start, site, "The text has " + LineCount(text) + " lines and " + count + " object values.");
        var ids = new int[count];
        for (int i = 0; i < count; i++) ids[i] = context.Int32(after + 4 * i);
        return LinesWithValuesNode.FromText(text, ids);
    }
}

internal sealed partial class DataCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BytesNode node)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is lowercase hexadecimal bytes.", null, frame.Path);
        if (Size is int n && node.Data.Length != n)
            throw context.Fail(Codes.Range, (frame.Property?.Name ?? "The value") + " holds exactly " + n + " bytes.", null, frame.Path, Text(node.Data.Length), n + " bytes");
        if (node.Data.Length > context.Limits.DecompressedBytes)
            throw context.Fail(Codes.Limit, "A payload is too large.", null, frame.Path);
        data.Bytes(node.Data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        int length = end - start;
        if (Size is int n && length != n)
            throw Mismatch(context, start, frame.Value(context), "A record of " + n + " bytes is stored in " + length + " bytes.");
        if (length > context.Limits.DecompressedBytes)
            throw context.Fail(Codes.Limit, "A payload is too large.", start, frame.Path);
        var bytes = new byte[length];
        context.Data.AsSpan(start, length).CopyTo(bytes);
        return new BytesNode(bytes);
    }
}

internal sealed partial class EnumCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not EnumNode node || node.TypeName != TypeName || node.Width != Width || node.Signed != Signed)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a " + TypeName + ".", null, frame.Path);
        if (node.Value < IntegerBytes.Min(Width, Signed) || node.Value > IntegerBytes.Max(Width, Signed))
            throw context.Fail(Codes.Range, (frame.Property?.Name ?? "The value") + " is outside its storage range.", null, frame.Path, Text(node.Value), Range());
        IntegerBytes.Write(data, node.Value, Width);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != Width)
            throw Mismatch(context, start, frame.Value(context), "An enum of " + Width + " bytes is stored in " + (end - start) + " bytes.");
        return new EnumNode(TypeName, Width, Signed, Read(context.Data.AsSpan(start, Width)));
    }
}

internal sealed partial class SetCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not SetNode node || node.TypeName != TypeName || node.Width != Width)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a set of " + TypeName + ".", null, frame.Path);
        IntegerBytes.Write(data, unchecked((long)node.Mask), Width);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != Width)
            throw Mismatch(context, start, frame.Value(context), "A set of " + Width + " bytes is stored in " + (end - start) + " bytes.");
        return new SetNode(TypeName, Width, Mask(context.Data.AsSpan(start, Width)));
    }
}

internal sealed partial class FixedArrayCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ListNode node)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a list of integers.", null, frame.Path);
        string name = frame.Property?.Name ?? "The value";
        if (node.Items.Count > Count)
            throw context.Fail(Codes.Range, name + " holds at most " + Count + " values. Export does not truncate.", null, frame.Path, Text(node.Items.Count), Count + " values");
        for (int i = 0; i < Count; i++)
        {
            if (i >= node.Items.Count)
            {
                IntegerBytes.Write(data, 0, Width);
                continue;
            }
            if (node.Items[i] is not IntegerNode integer || integer.Width != Width || integer.Signed != Signed)
                throw context.Fail(Codes.Type, name + " is an integer.", null, frame.Path.Item(i));
            Check(context, integer.Value, frame.Path.Item(i), name);
            IntegerBytes.Write(data, integer.Value, Width);
        }
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        int bytes = Count * Width;
        if (end - start != bytes)
            throw Mismatch(context, start, frame.Value(context), "An array of " + Count + " integers is stored in " + (end - start) + " bytes.");
        var node = new ListNode();
        for (int i = 0; i < Count; i++)
            node.Items.Add(new IntegerNode(Width, Signed, IntegerBytes.Read(context.Data.AsSpan(start + i * Width, Width), Width, Signed)));
        return node;
    }
}

internal sealed partial class RawBooleanCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        data.Byte(ImageByte(context, value, frame));
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != 1)
            throw Mismatch(context, start, frame.Value(context), "A raw boolean is stored in " + (end - start) + " bytes.");
        byte stored = context.Data[start];
        if (stored is 0 or 1) return new BooleanNode(stored != 0);
        return new IntegerNode(1, false, stored);
    }

    private static byte ImageByte(EncodeContext context, TreeValue value, ImageFrame frame)
    {
        string name = frame.Property?.Name ?? "The value";
        if (value is BooleanNode bit) return bit.Value ? (byte)1 : (byte)0;
        if (value is IntegerNode { Width: 1, Signed: false, Value: >= 0 and <= 255 } number) return (byte)number.Value;
        if (value is IntegerNode integer)
            throw context.Fail(Codes.Range, name + " is outside its storage range.", null, frame.Path, Text(integer.Value), "0 to 255");
        throw context.Fail(Codes.Type, name + " is a boolean.", null, frame.Path);
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
        if (end - start != 1) throw Mismatch(context, start, site, name + " is one byte.");
        byte stored = context.Data[start];
        if (stored == 0) return new JsonBoolean(false);
        if (stored == 1) return new JsonBoolean(true);
        return new JsonInteger(stored);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        data.Byte(Stored(context, value, site.Path));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is BooleanNode bit) return bit.Value ? new JsonBoolean(true) : null;
        if (value is IntegerNode { Width: 1, Signed: false } number)
        {
            if (number.Value == 0) return null;
            if (number.Value == 1) return new JsonBoolean(true);
            if (number.Value is < 0 or > 255)
                throw site.Fail(Codes.Range, (site.Property?.Name ?? name) + " is outside its storage range.", value: Text(number.Value), constraint: "0 to 255");
            return new JsonInteger(number.Value);
        }
        throw site.Mismatch((site.Property?.Name ?? name) + " is a boolean.");
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is JsonBoolean bit) return bit.Value ? new IntegerNode(1, false, 1) : null;
        if (value is JsonInteger number)
        {
            if (number.Value is < 0 or > 255)
                throw site.Fail(Codes.Range, (site.Property?.Name ?? name) + " is outside its storage range.", value, Text(number.Value), "0 to 255");
            if (number.Value == 0) return null;
            return new IntegerNode(1, false, number.Value);
        }
        throw site.Mismatch((site.Property?.Name ?? name) + " is a boolean.", value);
    }

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        data.Byte(ImageByte(context, value, frame));
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (end - start != 1) throw Mismatch(context, start, frame.Value(context), name + " is one byte.");
        return new IntegerNode(1, false, context.Data[start]);
    }

    private byte Stored(EncodeContext context, JsonValue value, DocumentNodePath path)
    {
        if (value is JsonBoolean bit) return bit.Value ? (byte)1 : (byte)0;
        if (value is JsonInteger { Value: >= 0 and <= 255 } number) return (byte)number.Value;
        if (value is JsonInteger integer)
            throw context.Fail(Codes.Range, name + " is outside its storage range.", integer, path, Text(integer.Value), "0 to 255");
        throw context.Fail(Codes.Type, name + " is a boolean.", value, path, value.KindName);
    }

    private static byte ImageByte(EncodeContext context, TreeValue value, ImageFrame frame)
    {
        if (value is BooleanNode bit) return bit.Value ? (byte)1 : (byte)0;
        if (value is IntegerNode { Width: 1, Signed: false, Value: >= 0 and <= 255 } number) return (byte)number.Value;
        if (value is IntegerNode integer)
            throw context.Fail(Codes.Range, (frame.Property?.Name ?? "The value") + " is outside its storage range.", null, frame.Path, Text(integer.Value), "0 to 255");
        throw context.Fail(Codes.Type, (frame.Property?.Name ?? "The value") + " is a boolean.", null, frame.Path);
    }
}

internal sealed partial class CatalogRecordCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (context.Schema.Version < 3)
            throw context.Fail(Codes.Type, "Version " + context.Schema.Version + " does not store a record.", null, frame.Path);
        var info = Info(context.File, frame.Path.ToString());
        if (info.BareSet)
        {
            FieldCodec(info.Fields[0]).WriteImage(value, context, data, frame);
            return;
        }
        if (value is not RecordNode node)
            throw context.Fail(Codes.Type, (frame.Property?.Name ?? TypeName) + " is a record.", null, frame.Path);
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var bytes = new byte[info.Size];
        int residueAt = info.Residue.Count == 0 ? -1 : info.Residue[0].Offset;
        bool residue = false;
        foreach (var field in info.Fields)
        {
            if (!residue && residueAt >= 0 && residueAt < field.Offset)
            {
                WriteResidue(node, bytes, info, context, frame);
                residue = true;
            }
            seen.Add(field.Name);
            var found = node.Find(field.Name);
            if (found is null || field.OmitsImage(found)) continue;
            var fieldFrame = FieldFrame(frame, field.Name);
            var buffer = new ByteBuffer(field.Size);
            FieldCodec(field).WriteImage(found, context, buffer, fieldFrame);
            if (buffer.Length != field.Size)
                throw context.Fail(Codes.Range, field.Name + " holds exactly " + field.Size + " bytes.", null, fieldFrame.Path, Text(buffer.Length), field.Size + " bytes");
            buffer.Span.CopyTo(bytes.AsSpan(field.Offset, field.Size));
        }
        if (!residue) WriteResidue(node, bytes, info, context, frame);
        seen.Add("$reserved");
        foreach (var (memberName, _) in node.Members)
            if (!seen.Contains(memberName))
                throw context.Fail(Codes.UnknownMember, "The record has no member \"" + memberName + "\".", null, frame.Path.Member(memberName), memberName);
        data.Bytes(bytes);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        if (context.Schema.Version < 3)
            throw context.Fail(Codes.Type, "Version " + context.Schema.Version + " does not store a record.", start, frame.Path);
        var info = Info(context.File, frame.Path.ToString());
        if (end - start != info.Size)
            throw Mismatch(context, start, frame.Value(context), "A record of " + info.Size + " bytes is stored in " + (end - start) + " bytes.");
        if (info.BareSet) return FieldCodec(info.Fields[0]).ReadImage(context, start, end, frame);
        var node = new RecordNode();
        int residueAt = info.Residue.Count == 0 ? -1 : info.Residue[0].Offset;
        bool residue = false;
        var image = context.Data.AsSpan(start, info.Size);
        foreach (var field in info.Fields)
        {
            if (!residue && residueAt >= 0 && residueAt < field.Offset)
            {
                AddImageResidue(image, info, node);
                residue = true;
            }
            var read = FieldCodec(field).ReadImage(context, start + field.Offset, start + field.Offset + field.Size, FieldFrame(frame, field.Name));
            if (!field.OmitsImage(read)) node.Add(field.Name, read);
        }
        if (!residue) AddImageResidue(image, info, node);
        return node;
    }

    private static void WriteResidue(RecordNode node, byte[] image, RecordTypeInfo info, EncodeContext context, ImageFrame frame)
    {
        var found = node.Find("$reserved");
        if (found is null) return;
        if (found is not BytesNode bytes)
            throw context.Fail(Codes.Type, "$reserved is lowercase hexadecimal.", null, frame.Path.Member("$reserved"));
        if (bytes.Data.Length != info.ResidueLength)
            throw context.Fail(Codes.Range, "$reserved holds the record residue.", null, frame.Path.Member("$reserved"), Text(bytes.Data.Length), info.ResidueLength + " bytes");
        Scatter(image, bytes.Data, info);
    }

    private static void AddImageResidue(ReadOnlySpan<byte> image, RecordTypeInfo info, RecordNode node)
    {
        if (info.ResidueLength == 0) return;
        var residue = Gather(image, info);
        if (AllZero(residue)) return;
        node.Add("$reserved", new BytesNode(residue));
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
