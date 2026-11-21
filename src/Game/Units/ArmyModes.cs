// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum ArmyModes : byte
{
    None = 0,
    FixedBehavior = 1 << 0,
    AIMoveDone = 1 << 1,
    Moving = 1 << 2,
    DisableTerrainValidation = 1 << 3,
}
