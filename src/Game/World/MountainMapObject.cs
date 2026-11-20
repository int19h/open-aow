// Translated from Mountain.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.World.Map;

namespace Aow.Game.World;

public class MountainMapObject(CustomNode owner) : Hexes.Terrain(owner)
{

    // Mountain.pas:101
    public new const int ClassId = 131432;

    // Mountain.pas:104
    public override sbyte GetOverlay(int index)
    {
        sbyte result = ((MapObject)this).Resource.TerrainList.Overlay[index];
        return result >= 0 ? (sbyte)0 : result;
    }

    // Mountain.pas:110
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => true;

    // Mountain.pas:136
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        int x = default;
        int y = default;
        int i = 0;
        bool valid = true;
        TerrainList list = ((MapObject)this).Resource.TerrainList;
        while (valid && i < list.Count)
        {
            if (list.Terrain[i] >= 0 || list.Overlay[i] >= 0)
            {
                HexGeometry.CenterHexNumberToHex(hexX, hexY, i, ref x, ref y);
                if (x >= 0 && y >= 0 && x < mapContainer.Width && y < mapContainer.Height)
                {
                    MapField mapField = mapContainer.GetField(x, y, hexLevel);
                    HexSprite hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
                    valid = hexSprite is not null && hexSprite.EngineClass.ClassId == 131432
                        ? ValidMountainOverlap(mapField, list.Overlay[i])
                        : hexSprite is null || hexSprite.Level < 10;
                }
            }

            i += 1;
        }

        return valid && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, ((MapObject)this).Resource.TerrainList);
    }

    // Mountain.pas:113
    public bool ValidMountainOverlap(MapField mapField, sbyte overlay)
    {
        bool valid = true;
        int i = 0;
        while (i != mapField.Count && valid)
        {
            MapObject hexSprite = mapField.GetChild<MapObject>(i);
            if (hexSprite.EngineClass.ClassId == 131432)
            {
                int hexNumberOfHexSprite = mapField.GetHexNumberOfHexSprite(hexSprite);
                sbyte otherOverlay = hexSprite.Resource.TerrainList.Overlay[hexNumberOfHexSprite];
                valid = !((otherOverlay == 0 && overlay == 0) || (otherOverlay == 1 && overlay == 1));
            }

            i += 1;
        }

        return valid;
    }
}
