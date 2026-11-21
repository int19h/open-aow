// Translated from AoWE.pas.

namespace Aow.Game.Magic.Spells;

[Flags]
public enum SpellControlTypes : byte
{
    None = 0,
    Global = 1 << 0,
    Combat = 1 << 1,
}
