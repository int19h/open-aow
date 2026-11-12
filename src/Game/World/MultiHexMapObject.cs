// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World;

// a map object that covers the hexagons of its terrain list around its main hexagon. FXhx, FYhx
// (the main hexagon, ids 3 and 4)
public partial class MultiHexMapObject : MapObject
{

    public MultiHexMapObject(ECustomNode owner) : base(owner)
    { }

    protected internal sbyte _hexX;
    protected internal sbyte _hexY;

    // MAPOBJ.PAS:923
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _hexX), 3);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _hexY), 4);
    }

    // MAPOBJ.PAS:900
    public override sbyte GetHexX() => _hexX;

    // MAPOBJ.PAS:905
    public override sbyte GetHexY() => _hexY;

    // MAPOBJ.PAS:910
    public override int GetMapCoordinate() => (GetHexLevel() << 16) + (_hexX + (_hexY << 8));

    // MAPOBJ.PAS:915
    public override int GetShowPriority()
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref pixelX, ref pixelY);
        return _resource._showPriorityOffset + (pixelY << 13) + pixelX;
    }

    // MAPOBJ.PAS:944
    // { Connect adds the object to the fields of its terrain list around the main hexagon }
    public override void Connect()
    {
        int count;
        MapLevel mapLevel;
        MapField mapField;
        HexNumberWalkData walk = default;
        if (!_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            _status = _status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            count = GetTerrainCount();
            mapLevel = (MapLevel)Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
            while (walk.HexNumber < count)
            {
                if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    if (mapField == Owner)
                    {
                        mapField.RemoveHexSprite(this);
                    }

                    mapField.AddHexSprite(this, walk.HexNumber);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // MAPOBJ.PAS:972
    public override void Disconnect()
    {
        int count;
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        if (_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            count = GetTerrainCount();
            mapLevel = (MapLevel)Owner.Owner;
            _status = _status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
            while (walk.HexNumber < count)
            {
                if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                {
                    mapLevel.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            Owner = null;
        }
    }

    // MAPOBJ.PAS:930
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref pixelX, ref pixelY);
        if (_resource._imageSequenceList == null)
        {
            _resource._imageLibrary.Get(0).Show(pixelX - pixelXSurface + ((MultiHexMapObjectResource)_resource)._imageOffsetX, pixelY - pixelYSurface + ((MultiHexMapObjectResource)_resource)._imageOffsetY, lockedSurface);
        }
        else
        {
            _resource.ShowComplexImage(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, 0, (uint)(pixelX + pixelY));
        }
    }

    // MAPOBJ.PAS:995
    public override bool PlaceOnMap()
    {
        int count;
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        if (!_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Connect();
            count = GetTerrainCount();
            mapLevel = (MapLevel)Owner.Owner;
            if (GetLevel() <= 15)
            {
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
                while (walk.HexNumber < count)
                {
                    if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                    {
                        mapLevel._map.InvalidateStaticScene(walk.X, walk.Y, ((MapField)Owner)._hexLevel);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }

            if (!GetEditMode())
            {
                var hexNumberToRadius = HexGeometry.HexNumberToRadius(count);
                mapLevel._map.TerrainChangedRadius(_hexX, _hexY, (sbyte)mapLevel._hexLevel, hexNumberToRadius);
            }
        }

        return true;
    }

    // MAPOBJ.PAS:1023
    public override void RemoveFromMap()
    {
        int count;
        MapLevel mapLevel;
        MapField mapField;
        sbyte terrainType;
        sbyte overlayType;
        HexNumberWalkData walk = default;
        if (_status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            mapLevel = (MapLevel)Owner.Owner;
            Disconnect();
            if (!mapLevel._map._status.Contains(HexSpriteMapStatusElement.Destroying))
            {
                count = GetTerrainCount();
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
                while (walk.HexNumber < count)
                {
                    terrainType = GetTerrain(walk.HexNumber);
                    overlayType = GetOverlay(walk.HexNumber);
                    if ((terrainType >= -1 || overlayType >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                    {
                        mapField = mapLevel.GetField(walk.X, walk.Y);
                        if (terrainType >= 0 || overlayType >= 0)
                        {
                            mapField.UpdateTerrain();
                        }

                        if (GetLevel() <= 15)
                        {
                            mapLevel._map.InvalidateStaticScene(walk.X, walk.Y, mapLevel._hexLevel);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                var hexNumberToRadius = HexGeometry.HexNumberToRadius(count);
                mapLevel._map.TerrainChangedRadius(_hexX, _hexY, (sbyte)mapLevel._hexLevel, hexNumberToRadius);
            }
        }
    }

    // MAPOBJ.PAS:1122
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result;
        int hexX = default;
        int hexY = default;
        int hexNumber;
        if (_resource._engine._hexSpriteMap._status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, ref hexX, ref hexY);
            hexNumber = HexGeometry.DeltaHexToHexNumber(((MapField)Owner)._hexX, ((MapField)Owner)._hexY, hexX, hexY);
            result = hexNumber < _resource._terrainList._count && (_resource._terrainList.GetTerrain(hexNumber) >= 0 || _resource._terrainList.GetOverlay(hexNumber) >= 0);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MAPOBJ.PAS:1096
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        int count;
        sbyte terrainType;
        sbyte overlayType;
        HexSprite hexSprite;
        HexNumberWalkData walk = default;
        bool result = true;
        count = _resource._terrainList._count;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 0);
        while (result && walk.HexNumber < count)
        {
            terrainType = GetTerrain(walk.HexNumber);
            overlayType = GetOverlay(walk.HexNumber);
            if ((terrainType >= 0 || overlayType >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapContainer._width && walk.Y < mapContainer._height)
            {
                hexSprite = mapContainer.GetField(walk.X, walk.Y, hexLevel).GetMaxLevelNoneTransparentHexSprite();
                result = hexSprite == null || CanPlaceOnHexSprite(hexSprite);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result && mapContainer._map.CanChangeTerrain(this, hexX, hexY, hexLevel, _resource._terrainList);
    }

    // MAPOBJ.PAS:1138
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        _hexX = hexX;
        _hexY = hexY;
        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // MAPOBJ.PAS:1214
    public virtual void SortMapFields()
    {
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        mapLevel = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
        while (true)
        {
            var terrainCount = GetTerrainCount();
            if (!(walk.HexNumber < terrainCount))
                break;
            if (GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                // { Original skips bounds for terrain. }
                mapLevel.GetField(walk.X, walk.Y).Sort();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // MAPOBJ.PAS:1091
    public virtual bool CanPlaceOnHexSprite(HexSprite hexSprite) => hexSprite.GetLevel() < 10;

    // MAPOBJ.PAS:1146
    // { The terrain type that most of the fields under the object have (the counts of Default win ties) }
    public virtual sbyte GetMostFrequentMapFieldTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY, sbyte defaultValue)
    {
        TerrainList terrainList;
        sbyte terrainType;
        ByteArray128 counts;
        HexNumberWalkData walk = default;
        sbyte result = defaultValue;
        terrainList = _resource._terrainList;
        if (result < 0)
        {
            result = 0;
        }

        counts = default;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 0);
        while (walk.HexNumber < terrainList._count)
        {
            if ((terrainList.GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                terrainType = mapLevel.GetField(walk.X, walk.Y)._terrainType;
                if (terrainType >= 0)
                {
                    counts[terrainType] = (byte)(counts[terrainType] + 1);
                    if (counts[terrainType] > counts[result])
                    {
                        result = terrainType;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (defaultValue >= 0 && counts[result] == counts[defaultValue])
        {
            result = defaultValue;
        }

        return result;
    }

    // MAPOBJ.PAS:1059
    public void UpdateTerrain(MapLevel mapLevel, int hexX, int hexY)
    {
        int count;
        HexNumberWalkData walk = default;
        count = _resource._terrainList._count;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 0);
        while (walk.HexNumber != count)
        {
            if ((GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                mapLevel.GetField(walk.X, walk.Y).UpdateTerrain();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // MAPOBJ.PAS:1075
    public void UpdateMapFields(MapLevel mapLevel, int hexX, int hexY)
    {
        int count;
        HexNumberWalkData walk = default;
        count = _resource._terrainList._count;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 0);
        while (walk.HexNumber != count)
        {
            if ((GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                mapLevel.GetField(walk.X, walk.Y).UpdateMapField();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // MAPOBJ.PAS:1178
    public void OverrideTerrain()
    {
        int count;
        MapLevel mapLevel;
        MapField mapField;
        SByteArray2 Terrain = default;
        HexNumberWalkData walk = default;
        mapLevel = (MapLevel)Owner.Owner;
        count = GetTerrainCount();
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
        while (walk.HexNumber < count)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                var terrain = GetTerrain(walk.HexNumber);
                Terrain[0] = terrain;
                var overlay = GetOverlay(walk.HexNumber);
                Terrain[1] = overlay;
                mapField = mapLevel.GetField(walk.X, walk.Y);
                if (Terrain[0] >= 0)
                {
                    mapField._terrainType = Terrain[0];
                }

                if (Terrain[1] >= 0)
                {
                    mapField._overlayType = Terrain[1];
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
        while (walk.HexNumber < count)
        {
            if (GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                // { Original skips bounds for terrain. }
                mapLevel.GetField(walk.X, walk.Y).TerrainChanged();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
