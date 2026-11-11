// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.Units;

// Retain both original assertion diagnostics
[StructLayout(LayoutKind.Explicit, Size = 136)]
public struct UnitEnchantmentAbility_Expand__TOriginalAssertText
{
    [FieldOffset(0)]
    public int RefCount;
    [FieldOffset(4)]
    public int Len;
    [FieldOffset(8)]
    public ByteArray128 Text;
}
