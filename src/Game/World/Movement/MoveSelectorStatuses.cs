// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

[Flags]
public enum MoveSelectorStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
    ShowSelector = 1 << 1,
}
