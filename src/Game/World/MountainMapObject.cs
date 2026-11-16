// Translated from Mountain.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.World.Map;

namespace Aow.Game.World;

public partial class MountainMapObject : Hexes.Terrain
{
    public MountainMapObject(CustomNode owner) : base(owner)
    { }

    // Mountain.pas:101
    public new const int ClassId = 131432;

    // Mountain.pas:104
    public override sbyte GetOverlay(int index)
    {
        sbyte result = ((MapObject)this).Resource.TerrainList.Overlay[index];
        if (result >= 0)
        {
            result = 0;
        }

        return result;
    }

    // Mountain.pas:110
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => true;

    // Mountain.pas:136
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        int i;
        bool valid;
        TerrainList list;
        MapField mapField;
        int x = default;
        int y = default;
        HexSprite hexSprite;
        i = 0;
        valid = true;
        list = ((MapObject)this).Resource.TerrainList;
        while (valid && i < list.Count)
        {
            if (list.Terrain[i] >= 0 || list.Overlay[i] >= 0)
            {
                HexGeometry.CenterHexNumberToHex(hexX, hexY, i, ref x, ref y);
                if (x >= 0 && y >= 0 && x < mapContainer.Width && y < mapContainer.Height)
                {
                    mapField = mapContainer.GetField(x, y, hexLevel);
                    hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
                    valid = hexSprite != null && hexSprite.EngineClass.ClassId == 131432
                        ? ValidMountainOverlap(mapField, list.Overlay[i])
                        : hexSprite == null || hexSprite.GetLevel() < 10;
                }
            }

            i += 1;
        }

        return valid && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, ((MapObject)this).Resource.TerrainList);
    }

    // Mountain.pas:113
    public bool ValidMountainOverlap(MapField mapField, sbyte overlay)
    {
        bool valid;
        int i;
        MapObject hexSprite;
        sbyte otherOverlay;
        valid = true;
        i = 0;
        while (i != mapField.Count && valid)
        {
            hexSprite = (MapObject)mapField.HexSpriteList[i];
            if (hexSprite.EngineClass.ClassId == 131432)
            {
                int hexNumberOfHexSprite = mapField.GetHexNumberOfHexSprite(hexSprite);
                otherOverlay = hexSprite.Resource.TerrainList.Overlay[hexNumberOfHexSprite];
                valid = !((otherOverlay == 0 && overlay == 0) || (otherOverlay == 1 && overlay == 1));
            }

            i += 1;
        }

        return valid;
    }

}
