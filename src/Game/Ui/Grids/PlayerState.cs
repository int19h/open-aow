// Translated from MapCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Ui.Grids;

[StructLayout(LayoutKind.Explicit, Size = 167)]
public struct PlayerState
{
    [FieldOffset(0)]
    public FixedArray_byte_44 Reserved00;
    [FieldOffset(44)]
    public bool Human;
    [FieldOffset(45)]
    public FixedArray_byte_121 Reserved2D;
    [FieldOffset(166)]
    public sbyte PlayerID;
}
