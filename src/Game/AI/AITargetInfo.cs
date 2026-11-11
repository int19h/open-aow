// Translated from AoWE.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.AI;

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct AITargetInfo
{
    [FieldOffset(0)]
    public sbyte Player;
    [FieldOffset(1)]
    public sbyte IndependentRelation;
    [FieldOffset(2)]
    public AITargetMode Mode;
    [FieldOffset(3)]
    public byte Reserved03;
    [FieldOffset(4)]
    public int Reserved04;
    [FieldOffset(8)]
    public int Reserved08;
}
