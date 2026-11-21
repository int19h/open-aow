// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum FastBattleUnitStates : byte
{
    None = 0,
    OutsideWall = 1 << 0,
    WallSiegeObjectRegistered = 1 << 1,
    Vulnerable = 1 << 2,
}
