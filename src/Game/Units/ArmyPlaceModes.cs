// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum ArmyPlaceModes : byte
{
    None = 0,
    Safe = 1 << 0,
    ScanArea = 1 << 1,
    EnableOwnPlayerLocation = 1 << 2,
}
