// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIGroupStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
    Done = 1 << 1,
    Changed = 1 << 2,
    Empty = 1 << 3,
    Split = 1 << 4,
}
