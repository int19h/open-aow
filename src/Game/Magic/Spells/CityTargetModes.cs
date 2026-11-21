// Translated from CitySpells.pas.

namespace Aow.Game.Magic.Spells;

[Flags]
public enum CityTargetModes : byte
{
    None = 0,
    Own = 1 << 0,
    Enemy = 1 << 1,
    SkipSource = 1 << 2,
    Razed = 1 << 3,
}
