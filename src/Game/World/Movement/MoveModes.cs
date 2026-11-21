// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

[Flags]
public enum MoveModes : byte
{
    None = 0,
    LogProcessedMapFields = 1 << 0,
    ExploredTerrainOnly = 1 << 1,
    AI = 1 << 2,
    IgnoreArmies = 1 << 3,
    AvoidArmies = 1 << 4,
    Surround = 1 << 5,
}
