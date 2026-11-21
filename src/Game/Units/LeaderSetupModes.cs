// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum LeaderSetupModes : byte
{
    None = 0,
    Look = 1 << 0,
    Settings = 1 << 1,
    Spheres = 1 << 2,
    Campaign = 1 << 3,
}
