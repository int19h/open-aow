// Translated from GlobalSpells.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells.Global;

[StructLayout(LayoutKind.Explicit, Size = 110)]
public struct GlobalSpellEngineFields
{
    [FieldOffset(0)]
    public FixedArray_byte_109 Reserved;
    [FieldOffset(109)]
    public byte Detail;
}
