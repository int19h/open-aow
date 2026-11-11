// Translated from AoWE.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.Units;

// { P2 recovery in original code order. }
[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct HeroLevel
{
    [FieldOffset(0)]
    public int DailyExperience;
    [FieldOffset(4)]
    public int Experience;
}
