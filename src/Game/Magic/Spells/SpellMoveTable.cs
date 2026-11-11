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
    public SByteArray16By16 Cost;
}
