// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum WallCombatFeatures : byte
{
    None = 0,
    WallBreaching = 1 << 0,
    WallPassing = 1 << 1,
    Participate = 1 << 2,
}
