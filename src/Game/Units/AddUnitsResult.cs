// Translated from AoWE.pas.

namespace Aow.Game.Units;

public enum AddUnitsResult : byte
{
    Ok = 0,
    InvalidMoveType = 1,
    TooManyUnits = 2,
    MultipleTransporters = 3
}
