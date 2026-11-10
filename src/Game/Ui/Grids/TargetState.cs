// Translated from MapCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Ui.Grids;

[StructLayout(LayoutKind.Explicit, Size = 57)]
public struct TargetState
{
    [FieldOffset(0)]
    public FixedArray_byte_13 Reserved00;
    [FieldOffset(13)]
    public byte Status;
    [FieldOffset(14)]
    public FixedArray_byte_42 Reserved0E;
    [FieldOffset(56)]
    public byte Mode;
}
