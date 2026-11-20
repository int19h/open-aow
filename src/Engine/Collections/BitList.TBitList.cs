using System.Runtime.InteropServices;

namespace Aow.Engine.Collections;

public partial class BitList
{
    // Engine.pas:4646. The unsigned comparison sends negative indices to the default.
    internal bool Get(int index) => (uint)index < (uint)_count
        ? (List.Span[index >> 3] & (1 << (index & 0b0111))) != 0
        : (_default & 1) != 0;

    // Engine.pas:4660. The write is relative to the buffer, including negative indices.
    internal void Put(int index, bool item)
    {
        if ((uint)index >= (uint)_count)
        {
            SetCount(index + 1);
        }

        if (List.Equals(default))
        {
            throw new NullReferenceException();
        }

        if (!MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)List, segment: out ArraySegment<byte> storage))
        {
            throw new InvalidOperationException("Bit list storage has no backing array");
        }

        ref byte value = ref storage.Array![storage.Offset + (index >> 3)];
        byte mask = (byte)(1 << (index & 0b0111));
        if (item)
        {
            value |= mask;
        }
        else
        {
            value &= (byte)~mask;
        }
    }
}
