// Translated from CitySpells.pas.

namespace Aow.Game.Magic.Spells;

[Flags]
public enum CityEnchantmentCastSpellSituations : byte
{
    None = 0,
    LeaderInCity = 1 << 0,
    Hex4City = 1 << 1,
    Hex3City = 1 << 2,
}
