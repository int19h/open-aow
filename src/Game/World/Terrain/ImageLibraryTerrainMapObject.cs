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
public partial class ImageLibraryTerrainMapObject : MultiHexMapObject
{

    public ImageLibraryTerrainMapObject(ECustomNode owner) : base(owner)
    {
        _terrainType = -1;
    }

    // ILTer.pas:79
    public new const int ClassId = 131462;

    private int _imageIndex;
    private sbyte _terrainType;

    // ILTer.pas:164
    public override sbyte GetTerrain(int index)
    {
        sbyte result = _resource.TerrainList.GetTerrain(index);
        if (result >= 0)
        {
            result = _terrainType;
        }

        return result;
    }

    // ILTer.pas:131
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        bool result;
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            result = GetMostFrequentMapFieldTerrainType(Owner.Owner, _hexX, _hexY, _terrainType) == info.MapField.TerrainType;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // ILTer.pas:152
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                _terrainType = info.TerrainType;
                _imageIndex = GetTerrainTypeImage(info.TerrainType);
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
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                _terrainType = info.TerrainType;
                _imageIndex = GetTerrainTypeImage(info.TerrainType);
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
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(_hexX, _hexY, out pixelX, out pixelY);
        if (this._resource.ImageSequenceList == null)
        {
            this._resource.ImageLibrary[_imageIndex].Show(pixelX - pixelXSurface + ((MultiHexMapObjectResource)this._resource).ImageOffsetX, pixelY - pixelYSurface + ((MultiHexMapObjectResource)this._resource).ImageOffsetY, lockedSurface);
        }
        else
        {
            this._resource.ShowComplexImage(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, _imageIndex, (uint)(pixelX + pixelY));
        }
    }

    // ILTer.pas:184
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            _terrainType = GetValidTerrainType(Owner.Owner, _hexX, _hexY);
        }

        return result;
    }

    // ILTer.pas:191
    public override bool Loaded() => base.PlaceOnMap();

    // ILTer.pas:255
    public override void Activate()
    {
        base.Activate();
        _terrainType = GetValidTerrainType(Owner.Owner, _hexX, _hexY);
        _imageIndex = GetTerrainTypeImage(_terrainType);
    }

    // ILTer.pas:196
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool canPlaceHere;
        int count;
        sbyte terrainType;
        HexNumberWalkData walk = default;
        sbyte overlay;
        HexSprite hexSprite;
        TerrainList terrainList;
        canPlaceHere = true;
        count = _resource.TerrainList.Count;
        _terrainType = GetValidTerrainType(mapContainer.GetMapLevel(hexLevel), hexX, hexY);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (canPlaceHere && walk.HexNumber < count)
        {
            terrainType = GetTerrain(walk.HexNumber);
            overlay = GetOverlay(walk.HexNumber);
            if ((terrainType >= 0 || overlay >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapContainer.Width && walk.Y < mapContainer.Height)
            {
                hexSprite = mapContainer.GetField(walk.X, walk.Y, hexLevel).GetMaxLevelNoneTransparentHexSprite();
                canPlaceHere = hexSprite == null || CanPlaceOnHexSprite(hexSprite);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (canPlaceHere)
        {
            terrainList = new TerrainList();
            terrainList.Assign(_resource.TerrainList);
            for (int I = 0, last = terrainList.Count - 1; I <= last; I++)
            {
                if (terrainList.GetTerrain(I) >= 0)
                {
                    terrainList.SetTerrain(I, _terrainType);
                }
            }

            canPlaceHere = canPlaceHere && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, terrainList);
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
            _terrainType = GetValidTerrainType(mapContainer.GetMapLevel(hexLevel), hexX, hexY);
            if (_terrainType >= 0)
            {
                _imageIndex = GetTerrainTypeImage(_terrainType);
                if (_imageIndex != -1)
                {
                    if (!GetEditMode())
                    {
                        OverrideTerrain();
                    }

                    result = true;
                }
            }
        }

        return result;
    }

    // ILTer.pas:89
    public virtual bool ValidTerrainType(int terrainType)
    {
        bool result;
        int index;
        ImageSequenceList sequences;
        if (terrainType >= 0)
        {
            index = GetTerrainTypeImage(terrainType);
            sequences = _resource.ImageSequenceList;
            if (sequences == null)
            {
                result = _resource.ImageLibrary.ImageExist(index);
            }
            else
            {
                result = sequences.GetImageSequence(index) != null;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // ILTer.pas:126
    public virtual int GetTerrainTypeImage(int terrainType) => terrainType;

    // ILTer.pas:84
    public virtual sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => -2;

    // ILTer.pas:107
    public virtual sbyte GetValidTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY)
    {
        int count;
        sbyte result = GetMostFrequentMapFieldTerrainType(mapLevel, hexX, hexY, -1);
        if (!ValidTerrainType(result))
        {
            result = ForceTerrainType(mapLevel, hexX, hexY);
            if (GetEditMode() || result != -2)
            {
                if (!ValidTerrainType(result))
                {
                    result = 0;
                    count = _resource.Engine.HexSpriteSet.TerrainTypes.Count;
                    while (result < count && !ValidTerrainType(result))
                    {
                        result = (sbyte)(result + 1);
                    }

                    if (result == count)
                    {
                        result = -1;
                    }
                }
            }
        }

        return result;
    }

    public int ImageIndex { get => _imageIndex; set => _imageIndex = value; }
    public sbyte TerrainType { get => _terrainType; protected set => _terrainType = value; }

    // ILTer.pas:20

}
