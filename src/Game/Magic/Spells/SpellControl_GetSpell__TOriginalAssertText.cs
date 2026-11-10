// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells;

[StructLayout(LayoutKind.Explicit, Size = 136)]
public struct SpellControl_GetSpell__TOriginalAssertText
{
    [FieldOffset(0)]
    public int RefCount;
    [FieldOffset(4)]
    public int Len;
    [FieldOffset(8)]
    public FixedArray_byte_128 Text;
}
