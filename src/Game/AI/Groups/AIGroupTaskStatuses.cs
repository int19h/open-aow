// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIGroupTaskStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
}
