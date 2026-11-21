// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum BattleFieldInfoRequests : byte
{
    None = 0,
    WallPassing = 1 << 0,
    ReturnImmunityTypes = 1 << 1,
    IgnorePassiveIndependents = 1 << 2,
    EffectiveStrength = 1 << 3,
    IgnoreIndependents = 1 << 4,
}
