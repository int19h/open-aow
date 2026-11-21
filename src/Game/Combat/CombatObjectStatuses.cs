// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatObjectStatuses : byte
{
    None = 0,
    Destroyed = 1 << 0,
    TurnDone = 1 << 1,
    Activated = 1 << 2,
    Fled = 1 << 3,
    Damaged = 1 << 4,
}
