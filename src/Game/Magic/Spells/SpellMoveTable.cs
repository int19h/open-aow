// Translated from GlobalTargetSpells.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells;

[StructLayout(LayoutKind.Explicit, Size = 257)]
public struct SpellMoveTable
{
    [FieldOffset(0)]
    public byte Flags;
    [FieldOffset(1)]
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Cost;
}
