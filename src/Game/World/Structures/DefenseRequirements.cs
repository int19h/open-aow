// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.World.Structures;

[StructLayout(LayoutKind.Explicit, Size = 40)]
public struct DefenseRequirements
{
    [FieldOffset(0)]
    public Array28<byte> Reserved00;
    [FieldOffset(28)]
    public int Strength;
    [FieldOffset(32)]
    public Array8<byte> Reserved20;
    [FieldOffset(0)]
    public Array10<int> DamageStrengths;
}
