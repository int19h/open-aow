// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIGroupTargetFlags : byte
{
    None = 0,
    Done = 1 << 0,
    Valid = 1 << 1,
    Recruit = 1 << 2,
    Produce = 1 << 3,
    Cooperate = 1 << 4,
    Ready = 1 << 5,
}
