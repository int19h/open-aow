// Translated from AoWE.pas.

namespace Aow.Game.Magic.Abilities;

[Flags]
public enum AbilityControlTypes : byte
{
    None = 0,
    Action = 1 << 0,
    CombatAction = 1 << 1,
}
