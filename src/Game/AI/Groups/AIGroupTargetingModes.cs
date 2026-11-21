// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIGroupTargetingModes : byte
{
    None = 0,
    MultiTargets = 1 << 0,
    Cooperate = 1 << 1,
    Recruit = 1 << 2,
    Produce = 1 << 3,
}
