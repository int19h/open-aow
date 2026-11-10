// Translated from GlobalTargetSpells.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.Magic.Spells.GlobalTarget;

// { Typed call-only methods retain original argument evaluation order without
// altering the foreign headers or their implementation signatures. }
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct SpellLocation
{
    [FieldOffset(0)]
    public sbyte X;
    [FieldOffset(1)]
    public sbyte Y;
    [FieldOffset(2)]
    public sbyte Layer;
    [FieldOffset(3)]
    public byte Reserved;
}
