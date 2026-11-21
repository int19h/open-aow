// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatPredictorUnitStatuses : byte
{
    None = 0,
    Useless = 1 << 0,
    Destroyed = 1 << 1,
}
