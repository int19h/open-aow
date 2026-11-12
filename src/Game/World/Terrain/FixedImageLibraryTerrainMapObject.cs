// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class FixedImageLibraryTerrainMapObject : ImageLibraryTerrainMapObject
{

    public FixedImageLibraryTerrainMapObject(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131464;

    // ILTer.pas:295
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        bool result;
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            bool validTerrainType = ValidTerrainType(info.MapField.TerrainType);
            if (validTerrainType)
            {
                var terrain = _resource.TerrainList.GetTerrain(info.HexNumber);
                validTerrainType = GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, _hexX, _hexY, terrain) == info.MapField.TerrainType;
            }

            result = validTerrainType;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // ILTer.pas:305
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
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
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                UpdateTerrain((MapLevel)Owner.Owner, _hexX, _hexY);
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
