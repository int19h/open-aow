// Translated from AoWE.pas.

using Aow.Engine;

namespace Aow.Game.World.Map;

/// <summary>The terrain of a map field. A field without a terrain holds null, the original -1.</summary>
[OriginalNone(-1)]
public enum TerrainType : byte
{
    Water = 0,
    Grass = 1,
    Desert = 2,
    Snow = 3,
    Steppe = 4,
    Wasteland = 5,
    Ice = 6,
    Earth = 7,
    Rock = 8,
    Lava = 9,
    UndergroundWater = 10,
    Reserved = 11,
    Dirt = 12,
    UndergroundIce = 13,
    Coast = 14,
    Border = 15
}
