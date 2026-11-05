using System.Buffers.Binary;

namespace Aow.Formats.Original;

/// <summary>A growable byte buffer for the original encodings.</summary>
public sealed class ByteBuffer
{
    private byte[] buffer;

    public ByteBuffer(int capacity = 64) => buffer = new byte[Math.Max(capacity, 16)];

    public int Length { get; private set; }

    public ReadOnlySpan<byte> Span => buffer.AsSpan(0, Length);

    public byte[] ToArray() => buffer.AsSpan(0, Length).ToArray();

    private Span<byte> Grow(int count)
    {
        if (Length + count > buffer.Length)
        {
            long size = Math.Max((long)buffer.Length * 2, (long)Length + count);
            if (size > Array.MaxLength) throw new ConversionException(new FormatDiagnostic(Codes.Limit, "An encoded image exceeds the largest buffer."));
            Array.Resize(ref buffer, (int)size);
        }
        var span = buffer.AsSpan(Length, count);
        Length += count;
        return span;
    }

    public void Byte(byte value) => Grow(1)[0] = value;

    public void Int16(ushort value) => BinaryPrimitives.WriteUInt16LittleEndian(Grow(2), value);

    public void Int32(int value) => BinaryPrimitives.WriteInt32LittleEndian(Grow(4), value);

    public void Bytes(ReadOnlySpan<byte> value) => value.CopyTo(Grow(value.Length));

    public void Zeros(int count) => Grow(count).Clear();

    /// <summary>Overwrites four bytes already written.</summary>
    public void PatchInt32(int at, int value) => BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(at, 4), value);

    public void Clear() => Length = 0;
}
