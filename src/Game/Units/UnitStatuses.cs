// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum UnitStatuses : byte
{
    None = 0,
    Registered = 1 << 0,
    Activated = 1 << 1,
    Moving = 1 << 2,
    MoveDone = 1 << 3,
    Killed = 1 << 4,
    Enchanted = 1 << 5,
}
