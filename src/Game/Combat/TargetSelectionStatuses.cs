// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum TargetSelectionStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
    Visible = 1 << 1,
}
