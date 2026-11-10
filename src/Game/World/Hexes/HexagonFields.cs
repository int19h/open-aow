// Translated from AoWHex.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Hexes;

[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct HexagonFields
{
    [FieldOffset(0)]
    public FixedArray_byte_16 Reserved;
    [FieldOffset(16)]
    public FixedArray_byte_3 Transitions;
    [FieldOffset(19)]
    public byte Sides;
}
