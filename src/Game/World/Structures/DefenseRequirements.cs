// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.World.Structures;

[StructLayout(LayoutKind.Explicit, Size = 40)]
public struct DefenseRequirements
{
    [FieldOffset(0)]
    public ByteArray28 Reserved00;
    [FieldOffset(28)]
    public int Strength;
    [FieldOffset(32)]
    public ByteArray8 Reserved20;
    [FieldOffset(0)]
    public Int32Array10 DamageStrengths;
}
