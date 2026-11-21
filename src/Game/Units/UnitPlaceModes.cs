// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum UnitPlaceModes : byte
{
    None = 0,
    ScanArea = 1 << 0,
    NeglectEmptyPlayerLocation = 1 << 1,
    CreateNewArmy = 1 << 2,
}
