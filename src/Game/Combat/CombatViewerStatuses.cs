// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatViewerStatuses : byte
{
    None = 0,
    Initialized = 1 << 0,
    Playing = 1 << 1,
    Paused = 1 << 2,
}
