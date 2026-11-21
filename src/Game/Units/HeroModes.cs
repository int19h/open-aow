// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum HeroModes : byte
{
    None = 0,
    UpdatingFromLibrary = 1 << 0,
    SkipLibraryUpdate = 1 << 1,
}
