// Translated from GlobalTargetSpells.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells.GlobalTarget;

[StructLayout(LayoutKind.Explicit, Size = 28)]
public struct MapFieldFlagView
{
    [FieldOffset(0)]
    public FixedArray_byte_24 Reserved;
    [FieldOffset(24)]
    public int Flags;
}
