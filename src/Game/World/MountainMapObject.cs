// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.World;

public partial class MountainMapObject : Hexes.Terrain
{
    public MountainMapObject(ECustomNode owner) : base(owner)
    { }

    // Mountain.pas:101
    public new const int ClassId = 131432;

    // Mountain.pas:104
    public override sbyte GetOverlay(int index)
    {
        sbyte result = _resource.TerrainList.Overlay[index];
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
        int I;
        bool valid;
        TerrainList list;
        MapField mapField;
        int X = default;
        int Y = default;
        HexSprite hexSprite;
        I = 0;
        valid = true;
        list = _resource.TerrainList;
        while (valid && I < list.Count)
        {
            if (list.Terrain[I] >= 0 || list.Overlay[I] >= 0)
            {
                HexGeometry.CenterHexNumberToHex(hexX, hexY, I, ref X, ref Y);
                if (X >= 0 && Y >= 0 && X < mapContainer.Width && Y < mapContainer.Height)
                {
                    mapField = mapContainer.GetField(X, Y, hexLevel);
                    hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
                    if (hexSprite != null && hexSprite.EngineClass.ClassId == 131432)
                    {
                        valid = ValidMountainOverlap(mapField, list.Overlay[I]);
                    }
                    else
                    {
                        valid = hexSprite == null || hexSprite.GetLevel() < 10;
                    }
                }
            }

            I += 1;
        }

        return valid && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, _resource.TerrainList);
    }

    // Mountain.pas:113
    public bool ValidMountainOverlap(MapField mapField, sbyte overlay)
    {
        bool valid;
        int I;
        MapObject hexSprite;
        sbyte otherOverlay;
        valid = true;
        I = 0;
        while (I != mapField.Count && valid)
        {
            hexSprite = (MapObject)mapField.HexSpriteList[I];
            if (hexSprite.EngineClass.ClassId == 131432)
            {
                var hexNumberOfHexSprite = mapField.GetHexNumberOfHexSprite(hexSprite);
                otherOverlay = hexSprite.Resource.TerrainList.Overlay[hexNumberOfHexSprite];
                valid = !((otherOverlay == 0 && overlay == 0) || (otherOverlay == 1 && overlay == 1));
            }

            I += 1;
        }

        return valid;
    }

}
