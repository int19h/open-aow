// Translated from AoWE.pas.

namespace Aow.Game.AI;

[Flags]
public enum AIExecutorStatuses : byte
{
    None = 0,
    Enabled = 1 << 0,
    TokenLocked = 1 << 1,
    Executing = 1 << 2,
}
