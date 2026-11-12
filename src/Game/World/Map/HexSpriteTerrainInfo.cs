// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// { THexagonSprite.ChangeTerrain, TerrainChanged, NeighbourTerrainChanged (12 bytes) }
public struct HexSpriteTerrainInfo
{
    public sbyte TerrainType { get; set; }
    public sbyte OverlayType { get; set; }
    public MapField MapField { get; set; }
    public sbyte HexNumber { get; set; }
}
