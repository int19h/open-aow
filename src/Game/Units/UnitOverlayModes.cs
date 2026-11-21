// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum UnitOverlayModes : byte
{
    None = 0,
    HitPoints = 1 << 0,
    Rank = 1 << 1,
    Combat = 1 << 2,
}
