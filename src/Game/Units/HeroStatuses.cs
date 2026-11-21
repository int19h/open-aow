// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum HeroStatuses : byte
{
    None = 0,
    Died = 1 << 0,
}
