// Translated from AoWE.pas.

namespace Aow.Game.Magic.Abilities;

[Flags]
public enum AbilityInfoModes : byte
{
    None = 0,
    Id = 1 << 0,
    Description = 1 << 1,
    Statistics = 1 << 2,
    Strength = 1 << 3,
}
