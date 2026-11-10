// Translated from ItemGrid.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Ui.Grids;

[StructLayout(LayoutKind.Explicit, Size = 29)]
public struct ItemStatus
{
    [FieldOffset(0)]
    public FixedArray_byte_28 Reserved;
    [FieldOffset(28)]
    public byte Status;
}
