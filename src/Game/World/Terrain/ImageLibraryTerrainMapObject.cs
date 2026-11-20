// Translated from ILTer.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Terrain;

// { The ILTer unit of HSEPack.dpl: image library terrain, multi-hexagon objects that show one image
//   per terrain type (TILTerrainMO, ClassID $20186; resource TILTerrainMOR, $20187), and the fixed
//   variant that keeps its terrain type and updates the map terrain instead
//   (TFixedILTerrainMO, $20188; TFixedILTerrainMOR, $20189). }
// _imageIndex, _terrainType
public class ImageLibraryTerrainMapObject(CustomNode owner) : MultiHexMapObject(owner)
{

    // ILTer.pas:79
    public new const int ClassId = 131_462;

    // ILTer.pas:164
    public override sbyte GetTerrain(int index)
    {
        sbyte result = Resource.TerrainList.GetTerrain(index);
        return result >= 0 ? TerrainType : result;
    }

    // ILTer.pas:131
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => Resource.TerrainList.GetTerrain(info.HexNumber) < 0 || GetMostFrequentMapFieldTerrainType(Owner.Owner, StoredHexX, StoredHexY, defaultValue: TerrainType) == info.MapField.TerrainType;

    // ILTer.pas:152
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
            }
            else
            {
                Dispose();
            }
        }
    }

    // ILTer.pas:140
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
            }
            else
            {
                Dispose();
            }
        }
    }

    // ILTer.pas:170
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        if (Resource.ImageSequenceList is null)
        {
            // The offsets live on the multi-hex resource. Another resource throws, as the old cast did.
            MultiHexMapObjectResource placed = GetResource<MultiHexMapObjectResource>();
            Resource.ImageLibrary[ImageIndex].Show(pixelX - pixelXSurface + placed.ImageOffsetX, pixelY - pixelYSurface + placed.ImageOffsetY, lockedSurface);
        }
        else
        {
            Resource.ShowComplexImage(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, index: ImageIndex, offset: (uint)(pixelX + pixelY));
        }
    }

    // ILTer.pas:184
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            TerrainType = GetValidTerrainType(Owner.Owner, StoredHexX, StoredHexY);
        }

        return result;
    }

    // ILTer.pas:191
    public override bool Loaded() => base.PlaceOnMap();

    // ILTer.pas:255
    public override void Activate()
    {
        base.Activate();
        TerrainType = GetValidTerrainType(Owner.Owner, StoredHexX, StoredHexY);
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // ILTer.pas:196
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool canPlaceHere = true;
        int count = Resource.TerrainList.Count;
        TerrainType = GetValidTerrainType(mapContainer.GetMapLevel(hexLevel), hexX, hexY);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (canPlaceHere && walk.HexNumber < count)
        {
            sbyte terrainType = GetTerrain(walk.HexNumber);
            sbyte overlay = GetOverlay(walk.HexNumber);
            if ((terrainType >= 0 || overlay >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapContainer.Width && walk.Y < mapContainer.Height)
            {
                HexSprite hexSprite = mapContainer.GetField(walk.X, walk.Y, hexLevel).GetMaxLevelNoneTransparentHexSprite();
                canPlaceHere = hexSprite is null || CanPlaceOnHexSprite(hexSprite);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (canPlaceHere)
        {
            TerrainList terrainList = new();
            terrainList.Assign(Resource.TerrainList);
            for (int i = 0, last = terrainList.Count - 1; i <= last; i++)
            {
                if (terrainList.GetTerrain(i) >= 0)
                {
                    terrainList.SetTerrain(i, value: TerrainType);
                }
            }

            canPlaceHere = canPlaceHere && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, terrain: terrainList);
            terrainList?.Dispose();
        }

        return canPlaceHere;
    }

    // ILTer.pas:236
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            result = false;
            TerrainType = GetValidTerrainType(mapContainer.GetMapLevel(hexLevel), hexX, hexY);
            if (TerrainType >= 0)
            {
                ImageIndex = GetTerrainTypeImage(TerrainType);
                if (ImageIndex != -1)
                {
                    if (!EditMode)
                    {
                        OverrideTerrain();
                    }

                    return true;
                }
            }
        }

        return result;
    }

    // ILTer.pas:89
    public virtual bool ValidTerrainType(int terrainType)
    {
        if (terrainType >= 0)
        {
            int index = GetTerrainTypeImage(terrainType);
            ImageSequenceList sequences = Resource.ImageSequenceList;
            return sequences is null ? Resource.ImageLibrary.ImageExist(index) : sequences.GetImageSequence(index) is not null;
        }
        else
        {
            return false;
        }
    }

    // ILTer.pas:126
    public virtual int GetTerrainTypeImage(int terrainType) => terrainType;

    // ILTer.pas:84
    public virtual sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => -2;

    // ILTer.pas:107
    public virtual sbyte GetValidTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY)
    {
        sbyte result = GetMostFrequentMapFieldTerrainType(mapLevel, hexX, hexY, defaultValue: -1);
        if (!ValidTerrainType(result))
        {
            result = ForceTerrainType(mapLevel, hexX, hexY);
            if (EditMode || result != -2)
            {
                if (!ValidTerrainType(result))
                {
                    result = 0;
                    int count = Resource.Engine.HexSpriteSet.TerrainTypes.Count;
                    while (result < count && !ValidTerrainType(result))
                    {
                        result = (sbyte)(result + 1);
                    }

                    if (result == count)
                    {
                        return -1;
                    }
                }
            }
        }

        return result;
    }

    public int ImageIndex { get; protected set; }
    public sbyte TerrainType { get; protected set; } = -1;
}
