// Translated from AoWTer.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Hexes;

[StructLayout(LayoutKind.Explicit, Size = 31)]
public struct BurnInfo
{
    [FieldOffset(0)]
    public FixedArray_byte_30 Reserved;
    [FieldOffset(30)]
    public byte Severity;
}
