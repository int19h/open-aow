using System.Buffers.Binary;
using System.Reflection;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>The stored shape of one record field. Names and types come from the C# struct.</summary>
internal enum RecordFieldKind
{
    Integer,
    Bool,
    Enum,
    Set,
    Record,
    Array,
}

/// <summary>
/// One named field of an explicit record, in storage order.
/// Each kind transfers its own bytes and builds its own codec.
/// </summary>
internal abstract class RecordFieldInfo
{
    public required string Name { get; init; }
    public required int Offset { get; init; }
    public required int Size { get; init; }
    public abstract RecordFieldKind Kind { get; }
    public virtual int Width => 0;
    public virtual bool Signed => false;
    public virtual string? TypeName => null;
    public virtual int Count => 0;

    /// <summary>The C# type of an enum, a set element, a nested record or an array element.</summary>
    internal virtual Type? ClrType => null;

    internal abstract void Transfer(IPersister persister, Span<byte> image);

    internal abstract PropertyCodec CreateCodec();

    /// <summary>A zero integer and an explicit false are absent in the document.</summary>
    internal virtual bool OmitsDocument(JsonValue value) => value is JsonBoolean { Value: false };

    /// <summary>A zero integer is absent in the tree of a record.</summary>
    internal virtual bool OmitsImage(TreeValue value) => false;

    protected static long ReadBytes(ReadOnlySpan<byte> bytes, int width, bool signed) => width switch
    {
        1 => signed ? (sbyte)bytes[0] : bytes[0],
        2 => signed ? BinaryPrimitives.ReadInt16LittleEndian(bytes) : BinaryPrimitives.ReadUInt16LittleEndian(bytes),
        _ => signed ? BinaryPrimitives.ReadInt32LittleEndian(bytes) : BinaryPrimitives.ReadUInt32LittleEndian(bytes),
    };

    protected static void WriteBytes(Span<byte> bytes, long value, int width)
    {
        if (width == 1) bytes[0] = (byte)value;
        else if (width == 2) BinaryPrimitives.WriteInt16LittleEndian(bytes, (short)value);
        else BinaryPrimitives.WriteInt32LittleEndian(bytes, (int)value);
    }
}

internal sealed class IntegerRecordField : RecordFieldInfo
{
    public required int ElementWidth { get; init; }
    public required bool ElementSigned { get; init; }
    public override RecordFieldKind Kind => RecordFieldKind.Integer;
    public override int Width => ElementWidth;
    public override bool Signed => ElementSigned;

    internal override bool OmitsDocument(JsonValue value) => value is JsonInteger { Value: 0 } || value is JsonBoolean { Value: false };

    internal override bool OmitsImage(TreeValue value) => value is IntegerNode { Value: 0 };

    internal override PropertyCodec CreateCodec() => new IntegerCodec(ElementWidth, ElementSigned);

    internal override void Transfer(IPersister persister, Span<byte> image)
    {
        if (ElementWidth == 4 && ElementSigned) Move(persister, image, static (IPersister body, string name, ref int value) => body.Int(name, ref value));
        else if (ElementWidth == 4 && !ElementSigned) Move(persister, image, static (IPersister body, string name, ref uint value) => body.UInt(name, ref value));
        else if (ElementWidth == 2 && ElementSigned) Move(persister, image, static (IPersister body, string name, ref short value) => body.Short(name, ref value));
        else if (ElementWidth == 2 && !ElementSigned) Move(persister, image, static (IPersister body, string name, ref ushort value) => body.UShort(name, ref value));
        else if (ElementWidth == 1 && ElementSigned) Move(persister, image, static (IPersister body, string name, ref sbyte value) => body.SByte(name, ref value));
        else if (ElementWidth == 1 && !ElementSigned) Move(persister, image, static (IPersister body, string name, ref byte value) => body.Byte(name, ref value));
        else throw new InvalidOperationException(Name + " is not a transferred integer.");
    }

    private void Move<T>(IPersister persister, Span<byte> image, IntegerMove<T> move) where T : unmanaged
    {
        T value = persister.Loading ? default : ReadNumber<T>(image);
        move(persister, Name, ref value);
        if (persister.Loading) WriteNumber(image, value);
    }

    private static T ReadNumber<T>(ReadOnlySpan<byte> bytes) where T : unmanaged
    {
        if (typeof(T) == typeof(int)) return (T)(object)BinaryPrimitives.ReadInt32LittleEndian(bytes);
        if (typeof(T) == typeof(uint)) return (T)(object)BinaryPrimitives.ReadUInt32LittleEndian(bytes);
        if (typeof(T) == typeof(short)) return (T)(object)BinaryPrimitives.ReadInt16LittleEndian(bytes);
        if (typeof(T) == typeof(ushort)) return (T)(object)BinaryPrimitives.ReadUInt16LittleEndian(bytes);
        if (typeof(T) == typeof(byte)) return (T)(object)bytes[0];
        if (typeof(T) == typeof(sbyte)) return (T)(object)(sbyte)bytes[0];
        throw new InvalidOperationException("An integer width is 1, 2 or 4 bytes.");
    }

    private static void WriteNumber<T>(Span<byte> bytes, T value) where T : unmanaged
    {
        if (typeof(T) == typeof(int)) BinaryPrimitives.WriteInt32LittleEndian(bytes, (int)(object)value!);
        else if (typeof(T) == typeof(uint)) BinaryPrimitives.WriteUInt32LittleEndian(bytes, (uint)(object)value!);
        else if (typeof(T) == typeof(short)) BinaryPrimitives.WriteInt16LittleEndian(bytes, (short)(object)value!);
        else if (typeof(T) == typeof(ushort)) BinaryPrimitives.WriteUInt16LittleEndian(bytes, (ushort)(object)value!);
        else if (typeof(T) == typeof(byte)) bytes[0] = (byte)(object)value!;
        else if (typeof(T) == typeof(sbyte)) bytes[0] = (byte)(sbyte)(object)value!;
        else throw new InvalidOperationException("An integer width is 1, 2 or 4 bytes.");
    }

    private delegate void IntegerMove<T>(IPersister persister, string name, ref T value);
}

internal sealed class BooleanRecordField : RecordFieldInfo
{
    public override RecordFieldKind Kind => RecordFieldKind.Bool;
    public override int Width => 1;

    internal override bool OmitsImage(TreeValue value) => value is IntegerNode { Value: 0 } || value is BooleanNode { Value: false };

    internal override PropertyCodec CreateCodec() => new RecordBooleanCodec(Name);

    internal override void Transfer(IPersister persister, Span<byte> image)
    {
        byte value = persister.Loading ? (byte)0 : image[0];
        persister.Byte(Name, ref value);
        if (persister.Loading) image[0] = value;
    }
}

internal sealed class EnumRecordField : RecordFieldInfo
{
    public required string ElementType { get; init; }
    public required int ElementWidth { get; init; }
    public required bool ElementSigned { get; init; }
    internal Type? ElementClr { get; init; }
    public override RecordFieldKind Kind => RecordFieldKind.Enum;
    public override int Width => ElementWidth;
    public override bool Signed => ElementSigned;
    public override string? TypeName => ElementType;
    internal override Type? ClrType => ElementClr;

    internal override PropertyCodec CreateCodec() => new EnumCodec(ElementType, ElementWidth, ElementSigned);

    internal override void Transfer(IPersister persister, Span<byte> image)
    {
        var box = new object[] { ReadBytes(image, ElementWidth, ElementSigned) };
        typeof(EnumRecordField).GetMethod(nameof(Move), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(ElementClr!)
            .Invoke(null, [persister, Name, box]);
        if (persister.Loading) WriteBytes(image, (long)box[0]!, ElementWidth);
    }

    private static void Move<TEnum>(IPersister persister, string name, object[] box) where TEnum : struct, Enum
    {
        var value = (TEnum)Enum.ToObject(typeof(TEnum), (long)box[0]!);
        persister.Enum(name, ref value);
        box[0] = Convert.ToInt64(value);
    }
}

internal sealed class SetRecordField : RecordFieldInfo
{
    public required string ElementType { get; init; }
    public required int ElementWidth { get; init; }
    internal Type? ElementClr { get; init; }
    public override RecordFieldKind Kind => RecordFieldKind.Set;
    public override int Width => ElementWidth;
    public override string? TypeName => ElementType;
    internal override Type? ClrType => ElementClr;

    internal override PropertyCodec CreateCodec() => new SetCodec(ElementType, ElementWidth);

    internal override void Transfer(IPersister persister, Span<byte> image) => TransferNamed(persister, Name, image);

    internal void TransferNamed(IPersister persister, string name, Span<byte> image)
    {
        var box = new object[] { image.ToArray() };
        typeof(SetRecordField).GetMethod(nameof(Move), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(ElementClr!)
            .Invoke(null, [persister, name, box]);
        if (persister.Loading) ((byte[])box[0]!).CopyTo(image);
    }

    private static void Move<E>(IPersister persister, string name, object[] box) where E : unmanaged, Enum
    {
        var image = (byte[])box[0]!;
        ulong mask = 0;
        for (int i = 0; i < image.Length; i++) mask |= (ulong)image[i] << (8 * i);
        var set = Set<E>.FromMask(mask);
        persister.Set(name, ref set);
        var written = new byte[image.Length];
        set.CopyImageTo(written);
        box[0] = written;
    }
}

internal sealed class ArrayRecordField : RecordFieldInfo
{
    public required int ElementWidth { get; init; }
    public required bool ElementSigned { get; init; }
    public required int ElementCount { get; init; }
    internal Type? ElementClr { get; init; }
    public override RecordFieldKind Kind => RecordFieldKind.Array;
    public override int Width => ElementWidth;
    public override bool Signed => ElementSigned;
    public override int Count => ElementCount;
    internal override Type? ClrType => ElementClr;

    internal override PropertyCodec CreateCodec() => new FixedArrayCodec(ElementWidth, ElementSigned, ElementCount);

    internal override void Transfer(IPersister persister, Span<byte> image)
    {
        if (ElementWidth == 4 && ElementSigned) Move(persister, image, static (IItemSlot slot, ref int item) => slot.Int(ref item));
        else if (ElementWidth == 4 && !ElementSigned) Move(persister, image, static (IItemSlot slot, ref uint item) => slot.UInt(ref item));
        else if (ElementWidth == 2 && ElementSigned) Move(persister, image, static (IItemSlot slot, ref short item) => slot.Short(ref item));
        else if (ElementWidth == 2 && !ElementSigned) Move(persister, image, static (IItemSlot slot, ref ushort item) => slot.UShort(ref item));
        else if (ElementWidth == 1 && ElementSigned) Move(persister, image, static (IItemSlot slot, ref sbyte item) => slot.SByte(ref item));
        else if (ElementWidth == 1 && !ElementSigned) Move(persister, image, static (IItemSlot slot, ref byte item) => slot.Byte(ref item));
        else throw new InvalidOperationException(Name + " is not a transferred array.");
    }

    private void Move<T>(IPersister persister, Span<byte> image, ItemTransfer<T> transfer)
    {
        var items = new List<T>(ElementCount);
        for (int i = 0; i < ElementCount; i++)
            items.Add(ReadItem<T>(image.Slice(i * ElementWidth, ElementWidth)));
        persister.List(Name, items, transfer);
        if (!persister.Loading) return;
        if (items.Count > ElementCount)
            throw new InvalidOperationException(Name + " holds at most " + ElementCount + " values.");
        for (int i = 0; i < ElementCount; i++)
        {
            var slot = image.Slice(i * ElementWidth, ElementWidth);
            if (i < items.Count) WriteItem(slot, items[i]);
            else slot.Clear();
        }
    }

    private static T ReadItem<T>(ReadOnlySpan<byte> bytes)
    {
        if (typeof(T) == typeof(int)) return (T)(object)BinaryPrimitives.ReadInt32LittleEndian(bytes);
        if (typeof(T) == typeof(uint)) return (T)(object)BinaryPrimitives.ReadUInt32LittleEndian(bytes);
        if (typeof(T) == typeof(short)) return (T)(object)BinaryPrimitives.ReadInt16LittleEndian(bytes);
        if (typeof(T) == typeof(ushort)) return (T)(object)BinaryPrimitives.ReadUInt16LittleEndian(bytes);
        if (typeof(T) == typeof(byte)) return (T)(object)bytes[0];
        if (typeof(T) == typeof(sbyte)) return (T)(object)(sbyte)bytes[0];
        throw new InvalidOperationException("An integer width is 1, 2 or 4 bytes.");
    }

    private static void WriteItem<T>(Span<byte> bytes, T value)
    {
        if (typeof(T) == typeof(int)) BinaryPrimitives.WriteInt32LittleEndian(bytes, (int)(object)value!);
        else if (typeof(T) == typeof(uint)) BinaryPrimitives.WriteUInt32LittleEndian(bytes, (uint)(object)value!);
        else if (typeof(T) == typeof(short)) BinaryPrimitives.WriteInt16LittleEndian(bytes, (short)(object)value!);
        else if (typeof(T) == typeof(ushort)) BinaryPrimitives.WriteUInt16LittleEndian(bytes, (ushort)(object)value!);
        else if (typeof(T) == typeof(byte)) bytes[0] = (byte)(object)value!;
        else if (typeof(T) == typeof(sbyte)) bytes[0] = (byte)(sbyte)(object)value!;
        else throw new InvalidOperationException("An integer width is 1, 2 or 4 bytes.");
    }
}

internal sealed class NestedRecordField : RecordFieldInfo
{
    public required string ElementType { get; init; }
    internal Type? ElementClr { get; init; }
    public override RecordFieldKind Kind => RecordFieldKind.Record;
    public override string? TypeName => ElementType;
    internal override Type? ClrType => ElementClr;

    internal override PropertyCodec CreateCodec() => new CatalogRecordCodec(ElementType, Size);

    internal override void Transfer(IPersister persister, Span<byte> image)
    {
        var nested = RecordCatalog.Require(ElementType, null, Name);
        if (nested.BareSet)
        {
            ((SetRecordField)nested.Fields[0]).TransferNamed(persister, Name, image);
            return;
        }
        var copy = image.ToArray();
        persister.Record(Name, body => RecordValues.Apply(body, nested, copy));
        copy.CopyTo(image);
    }
}
