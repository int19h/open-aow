// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIMoveTargetSelectorModes : byte
{
    None = 0,
    CreateMovePaths = 1 << 0,
}
