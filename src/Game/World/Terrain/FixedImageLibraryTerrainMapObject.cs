#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public class FixedImageLibraryTerrainMapObject(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{
    public new const int ClassId = 131_464;

    // ILTer.pas:295
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            bool validTerrainType = ValidTerrainType(info.MapField.TerrainNumber);
            if (validTerrainType)
            {
                validTerrainType = GetMostFrequentMapFieldTerrainType(defaultValue: Resource.TerrainList.GetTerrain(info.HexNumber).ToOriginal(), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY) == info.MapField.TerrainNumber;
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
        if (Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            if (ValidTerrainType(info.TerrainNumber))
            {
                TerrainType = info.TerrainNumber;
                ImageIndex = GetTerrainTypeImage(info.TerrainNumber);
            }
        }
    }

    // ILTer.pas:315
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            if (ValidTerrainType(info.TerrainNumber))
            {
                TerrainType = info.TerrainNumber;
                UpdateTerrain(Owner.Owner, StoredHexX, StoredHexY);
                ImageIndex = GetTerrainTypeImage(info.TerrainNumber);
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
    public override int ForceTerrainType(MapLevel mapLevel, int hexX, int hexY) => -1;

}
