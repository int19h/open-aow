// Translated from AoWE.pas.

namespace Aow.Game.Session;

[Flags]
public enum PlayerEnchantmentTypes : byte
{
    None = 0,
    EnchantedRoads = 1 << 1,
    AirMastery = 1 << 2,
    WaterMastery = 1 << 3,
    LifeMastery = 1 << 4,
    DeathMastery = 1 << 5,
    FireMastery = 1 << 6,
    EarthMastery = 1 << 7,
}
