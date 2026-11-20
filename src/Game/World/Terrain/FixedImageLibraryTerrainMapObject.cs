// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public class FixedImageLibraryTerrainMapObject(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{
    public new const int ClassId = 131464;

    // ILTer.pas:295
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            bool validTerrainType = ValidTerrainType(info.MapField.TerrainType);
            if (validTerrainType)
            {
                validTerrainType = GetMostFrequentMapFieldTerrainType(defaultValue: Resource.TerrainList.GetTerrain(info.HexNumber), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY) == info.MapField.TerrainType;
            }

            return validTerrainType;
        }
        else
        {
            return true;
        }
    }

    // ILTer.pas:305
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
            }
        }
    }

    // ILTer.pas:315
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                UpdateTerrain(Owner.Owner, StoredHexX, StoredHexY);
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
            }
            else
            {
                try
                {
                    throw new InvalidOperationException("TILFixedTerrain Failure");
                }
                catch
                {
                    Dispose();
                }
            }
        }
    }

    // ILTer.pas:290
    public override sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => -1;

}
