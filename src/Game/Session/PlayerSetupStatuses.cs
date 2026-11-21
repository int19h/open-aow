// Translated from AoWSetup.pas.

namespace Aow.Game.Session;

[Flags]
public enum PlayerSetupStatuses : byte
{
    None = 0,
    Fixed = 1 << 0,
    Forced = 1 << 1,
    LeaderCustomized = 1 << 2,
    Loaded = 1 << 3,
}
