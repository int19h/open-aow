// Translated from AoWE.pas.

namespace Aow.Game.Combat;

public enum CombatResult : byte
{
    Idle = 0,
    Busy = 1,
    CannotInflictDamage = 2,
    Victory = 3,
    Defeat = 4,
    BothDefeated = 5,
    NoDefenders = 6,
    Undecided = 7,
    CannotBreachWall = 8,
    Fled = 9
}
