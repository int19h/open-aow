// Translated from MapCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Ui.Grids;

[StructLayout(LayoutKind.Explicit, Size = 376)]
public struct MapState
{
    [FieldOffset(0)]
    public FixedArray_byte_164 Reserved00;
    [FieldOffset(164)]
    public sbyte CurrentPlayer;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
    [FieldOffset(166)]
    public FixedArray_byte_115 ReservedA6;
    [FieldOffset(281)]
    public sbyte Result;
    [FieldOffset(282)]
    public FixedArray_byte_90 Reserved11A;
    [FieldOffset(372)]
    public int Day;
}
