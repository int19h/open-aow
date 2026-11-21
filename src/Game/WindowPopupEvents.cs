// Translated from AoWReg.pas.

namespace Aow.Game;

[Flags]
public enum WindowPopupEvents : byte
{
    None = 0,
    Combat = 1 << 0,
    Diplomacy = 1 << 1,
    SpellResearched = 1 << 2,
    SpellReady = 1 << 3,
    Rebellion = 1 << 4,
    PartyDamage = 1 << 5,
    HeroUpgrade = 1 << 6,
    HeroJoin = 1 << 7,
}
