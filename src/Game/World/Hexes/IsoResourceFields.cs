// Translated from AoWHex.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Hexes;

[StructLayout(LayoutKind.Explicit, Size = 53)]
public struct IsoResourceFields
{
    [FieldOffset(0)]
    public FixedArray_byte_52 Reserved;
    [FieldOffset(52)]
    public byte DynamicLevel;
}
