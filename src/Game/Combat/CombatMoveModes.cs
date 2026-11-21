// Translated from AoWTC.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatMoveModes : byte
{
    None = 0,
    WallCrushing = 1 << 0,
    Melee = 1 << 1,
    MoveRanged = 1 << 2,
    Move = 1 << 3,
    Wall = 1 << 4,
    Climbing = 1 << 5,
}
