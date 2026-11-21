// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatStatuses : byte
{
    None = 0,
    Executing = 1 << 0,
    Activated = 1 << 1,
    LogCombatActions = 1 << 2,
}
