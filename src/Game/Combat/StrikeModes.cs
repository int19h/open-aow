// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum StrikeModes : byte
{
    None = 0,
    CausedFear = 1 << 0,
    LifeStealing = 1 << 1,
    Entangled = 1 << 2,
}
