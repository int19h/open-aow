#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Mountain.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.World.Map;

namespace Aow.Game.World;

public class MountainMapObject(CustomNode owner) : Hexes.Terrain(owner)
{

    // Mountain.pas:101
    public new const int ClassId = 131_432;

    // Mountain.pas:104
    public override FootprintCell<OverlayType> GetOverlay(int index)
    {
        FootprintCell<OverlayType> result = ((MapObject)this).Resource.TerrainList.Overlay[index];
        return result.Value is not null ? OverlayType.Mountain : result;
    }

    // Mountain.pas:110
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => true;

    // Mountain.pas:136
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        int x = default;
        int y = default;
        int i = 0;
        bool valid = true;
        TerrainList list = ((MapObject)this).Resource.TerrainList;
        while (valid && i < list.Count)
        {
            if (list.Terrain[i].Value is not null || list.Overlay[i].Value is not null)
            {
                HexGeometry.CenterHexNumberToHex(hexX, hexY, hexNumber: i, ref x, ref y);
                if (x >= 0 && y >= 0 && x < mapContainer.Width && y < mapContainer.Height)
                {
                    MapField mapField = mapContainer.GetField(x, y, hexLevel);
                    HexSprite hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
                    valid = hexSprite is not null && hexSprite.EngineClass.ClassId == ClassId
                        ? ValidMountainOverlap(mapField, overlay: list.Overlay[i].ToOriginal())
                        : hexSprite is null || hexSprite.Level < 10;
                }
            }

            i += 1;
        }

        return valid && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, terrain: ((MapObject)this).Resource.TerrainList);
    }

    // Mountain.pas:113
    public bool ValidMountainOverlap(MapField mapField, int overlay)
    {
        bool valid = true;
        int i = 0;
        while (i != mapField.Count && valid)
        {
            MapObject hexSprite = mapField.GetChild<MapObject>(i);
            if (hexSprite.EngineClass.ClassId == ClassId)
            {
                int hexNumberOfHexSprite = mapField.GetHexNumberOfHexSprite(hexSprite);
                FootprintCell<OverlayType> otherOverlay = hexSprite.Resource.TerrainList.Overlay[hexNumberOfHexSprite];
                valid = !((otherOverlay.Value == OverlayType.Mountain && overlay == 0) || (otherOverlay.Value == OverlayType.DenseVegetation && overlay == 1));
            }

            i += 1;
        }

        return valid;
    }
}
