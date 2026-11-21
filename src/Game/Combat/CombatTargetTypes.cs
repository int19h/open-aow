// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatTargetTypes : byte
{
    None = 0,
    Friend = 1 << 0,
    Enemy = 1 << 1,
}
