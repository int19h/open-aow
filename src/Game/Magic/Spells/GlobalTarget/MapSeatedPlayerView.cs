// Translated from GlobalTargetSpells.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells.GlobalTarget;

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct MapSeatedPlayerView
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
}
