// Translated from AoWHex.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Hexes;

[StructLayout(LayoutKind.Explicit, Size = 17)]
public struct WaterViewFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_16 Reserved;
    [FieldOffset(16)]
    public byte Flags;
}
