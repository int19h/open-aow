// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum ArmyUserModes : byte
{
    None = 0,
    Guard = 1 << 0,
    TurnDone = 1 << 1,
}
