// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

public enum MoveModeElement : byte
{
    LogProcessedMapFields = 0,
    ExploredTerrainOnly = 1,
    AI = 2,
    IgnoreArmies = 3,
    AvoidArmies = 4,
    Surround = 5
}
