// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// { THexagonSprite.ChangeTerrain, TerrainChanged, NeighbourTerrainChanged (12 bytes) }
public struct HexSpriteTerrainInfo
{
    // The terrain of the field, as the message carries it. Reading it while the field has no terrain throws.
    public readonly TerrainType Terrain => TerrainType ?? throw new InvalidOperationException("The field has no terrain yet.");

    // The terrain of the field, or false while the field has no terrain yet.
    public readonly bool TryGetTerrain(out TerrainType terrain)
    {
        terrain = TerrainType.GetValueOrDefault();
        return TerrainType is not null;
    }

    // The original terrain number, -1 while the field has no terrain yet. A bridge for numeric callers: the
    // boundary-parameters sub-pass of issue 331 replaces each read with a typed parameter.
    public readonly sbyte TerrainNumber => TerrainType.ToOriginal();

    // The field's terrain, null while the field has no terrain yet. MapField sets it.
    internal TerrainType? TerrainType { get; set; }
    public OverlayType? OverlayType { get; set; }
    public MapField MapField { get; set; }
    public sbyte HexNumber { get; set; }
}
