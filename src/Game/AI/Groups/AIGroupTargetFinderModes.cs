// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIGroupTargetFinderModes : byte
{
    None = 0,
    CalculateMovePathBfs = 1 << 0,
}
