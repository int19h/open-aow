// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.World.Structures;

[StructLayout(LayoutKind.Explicit, Size = 40)]
public struct DefenseRequirements
{
    [FieldOffset(0)]
    public FixedArray_byte_28 Reserved00;
    [FieldOffset(28)]
    public int Strength;
    [FieldOffset(32)]
    public FixedArray_byte_8 Reserved20;
    [FieldOffset(0)]
    public FixedArray_int_10 DamageStrengths;
}
