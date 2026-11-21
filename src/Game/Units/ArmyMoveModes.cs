// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum ArmyMoveModes : byte
{
    None = 0,
    SelectEndLocation = 1 << 0,
    BuildRoads = 1 << 1,
    DoNotNegotiate = 1 << 2,
}
