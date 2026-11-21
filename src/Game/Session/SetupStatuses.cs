// Translated from AoWSetup.pas.

namespace Aow.Game.Session;

[Flags]
public enum SetupStatuses : byte
{
    None = 0,
    Busy = 1 << 0,
    Aborted = 1 << 1,
    Remote = 1 << 2,
}
