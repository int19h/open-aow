using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Engine.Collections;

public partial class BitList
{
    // Engine.pas:4646. The unsigned comparison sends negative indices to the default.
    internal bool Get(int index) => (uint)index < (uint)_count
        ? (List.Span[index >> 3] & (1 << (index & 7))) != 0
        : (Unsafe.As<bool, byte>(ref _default) & 1) != 0;

    // Engine.pas:4660. The write is relative to the buffer, including negative indices.
    internal void Put(int index, bool item)
    {
        if ((uint)index >= (uint)_count) SetCount(index + 1);
        if (List.Equals(default(Memory<byte>))) throw new NullReferenceException();
        if (!MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)List, out var storage))
            throw new InvalidOperationException("Bit list storage has no backing array");
        ref byte value = ref storage.Array![storage.Offset + (index >> 3)];
        byte mask = (byte)(1 << (index & 7));
        if (item) value |= mask;
        else value &= (byte)~mask;
    }
}
