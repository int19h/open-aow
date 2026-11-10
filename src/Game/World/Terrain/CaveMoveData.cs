// Translated from Cave.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Terrain;

[StructLayout(LayoutKind.Explicit, Size = 305)]
public struct CaveMoveData
{
    [FieldOffset(0)]
    public FixedArray_byte_49 Reserved;
    [FieldOffset(49)]
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
}
