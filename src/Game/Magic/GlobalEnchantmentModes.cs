// Translated from AoWE.pas.

namespace Aow.Game.Magic;

[Flags]
public enum GlobalEnchantmentModes : byte
{
    None = 0,
    TrueGlobal = 1 << 0,
    Owned = 1 << 1,
}
