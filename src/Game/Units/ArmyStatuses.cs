// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum ArmyStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
    UnfogEnabled = 1 << 1,
    Selected = 1 << 2,
    Concealed = 1 << 3,
    FullyConcealed = 1 << 4,
    ShowCrown = 1 << 5,
}
