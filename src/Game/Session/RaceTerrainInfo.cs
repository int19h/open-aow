// Translated from AoWE.pas.
using Aow.Game.World.Map;

namespace Aow.Game.Session;

// Each set keeps the 16-bit width of the Delphi set: a value is cut to its low 16 bits.
public readonly record struct RaceTerrainInfo
{
    public TerrainTypes Bonus { get; init; }

    public TerrainTypes Penalty { get; init; }
}
