using System.Buffers.Binary;
using Aow.Rtl;

namespace Aow.Graphics;

// Rows of pixels in a block of memory, what the original passes as a pointer and a pitch: the
// whole block as bytes, the byte index of the first pixel of row 0 in it, the signed distance in
// bytes from one row to the next (negative for a bottom-up bitmap) and, for the routines that
// clip, the clip rectangle. The routines address the block by index, so a view never moves and a
// bottom-up bitmap needs nothing special (graphics.md 5.2).
public readonly ref struct SurfaceView
{
    public readonly Span<byte> Memory;
    public readonly int Start;
    public readonly int Pitch;
    public readonly Rect Clip;

    public SurfaceView(Span<byte> memory, int start, int pitch, Rect clip = default)
    {
        Memory = memory;
        Start = start;
        Pitch = pitch;
        Clip = clip;
    }

    // The view moved by a byte offset, as `Pointer(Integer(P) + Offset)`.
    public SurfaceView At(int offset) => new(Memory, Start + offset, Pitch, Clip);

    public SurfaceView WithPitch(int pitch) => new(Memory, Start, pitch, Clip);

    // The byte index of the first pixel of row y in the block.
    public int RowIndex(int y) => Start + y * Pitch;

    // The `count` bytes of row y from its first pixel. A row that runs past the end of the block
    // ends with the block, so a routine that walks the row writes what lies inside and raises at
    // its first access beyond the end, as the pointer did. A row that starts before the block
    // raises here.
    public Span<byte> Row(int y, int count)
    {
        if (count <= 0) return Span<byte>.Empty;
        int index = Start + y * Pitch;
        if (index < 0) throw new IndexOutOfRangeException();
        return index >= Memory.Length ? Span<byte>.Empty : Memory.Slice(index, Math.Min(count, Memory.Length - index));
    }

    // The `count` bytes at a byte offset from the first pixel of row 0. Bytes past either end of
    // the block raise ArgumentOutOfRangeException, as the pointer's span did.
    public Span<byte> Bytes(int offset, int count) => Memory.Slice(Start + offset, count);

    // The byte, word and dword at a byte offset from the first pixel of row 0, little-endian. The
    // indexer raises IndexOutOfRangeException outside the block, the readers and writers raise
    // ArgumentOutOfRangeException, as the pointer's element and span did.
    public ref byte this[int offset] => ref Memory[Start + offset];

    public byte Byte(int offset) => Memory.Slice(Start + offset, 1)[0];

    public ushort UInt16(int offset) => BinaryPrimitives.ReadUInt16LittleEndian(Memory.Slice(Start + offset, 2));

    public void UInt16(int offset, int value) => BinaryPrimitives.WriteUInt16LittleEndian(Memory.Slice(Start + offset, 2), unchecked((ushort)value));

    public uint Dword(int offset) => BinaryPrimitives.ReadUInt32LittleEndian(Memory.Slice(Start + offset, 4));

    public void Dword(int offset, uint value) => BinaryPrimitives.WriteUInt32LittleEndian(Memory.Slice(Start + offset, 4), value);
}
