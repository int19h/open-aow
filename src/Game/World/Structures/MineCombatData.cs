// Translated from Mine.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Structures;

[StructLayout(LayoutKind.Explicit, Size = 21)]
public struct MineCombatData
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public byte Outcome;
}
