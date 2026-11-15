// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World;

// a map object that covers the hexagons of its terrain list around its main hexagon. FXhx, FYhx
// (the main hexagon, ids 3 and 4)
public partial class MultiHexMapObject : MapObject
{

    public MultiHexMapObject(CustomNode owner) : base(owner)
    { }

    protected internal sbyte StoredHexX { get; set; }
    protected internal sbyte StoredHexY { get; set; }

    // MAPOBJ.PAS:923
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        StoredHexX = PersistSupport.UnsignedByte(persister, name: "HexX", value: StoredHexX);
        StoredHexY = PersistSupport.UnsignedByte(persister, name: "HexY", value: StoredHexY);
    }

    // MAPOBJ.PAS:900
    public override sbyte GetHexX() => StoredHexX;

    // MAPOBJ.PAS:905
    public override sbyte GetHexY() => StoredHexY;

    // MAPOBJ.PAS:910
    public override MapCoordinate GetMapCoordinate() => HexGeometry.PackMapCoordinate(StoredHexX, StoredHexY, GetHexLevel());

    // MAPOBJ.PAS:915
    public override int GetShowPriority()
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out pixelX, out pixelY);
        return Resource.ShowPriorityOffset + (pixelY << 13) + pixelX;
    }

    // MAPOBJ.PAS:944
    // { Connect adds the object to the fields of its terrain list around the main hexagon }
    public override void Connect()
    {
        int count;
        MapLevel mapLevel;
        MapField mapField;
        HexNumberWalkData walk = default;
        if (!Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Status = Status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            count = GetTerrainCount();
            mapLevel = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
            while (walk.HexNumber < count)
            {
                if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
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
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            count = GetTerrainCount();
            mapLevel = Owner.Owner;
            Status = Status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
            while (walk.HexNumber < count)
            {
                if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
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
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out pixelX, out pixelY);
        if (Resource.ImageSequenceList == null)
        {
            Resource.ImageLibrary.Get(0).Show(pixelX - pixelXSurface + ((MultiHexMapObjectResource)Resource).ImageOffsetX, pixelY - pixelYSurface + ((MultiHexMapObjectResource)Resource).ImageOffsetY, lockedSurface);
        }
        else
        {
            Resource.ShowComplexImage(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, 0, (uint)(pixelX + pixelY));
        }
    }

    // MAPOBJ.PAS:995
    public override bool PlaceOnMap()
    {
        int count;
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        if (!Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Connect();
            count = GetTerrainCount();
            mapLevel = Owner.Owner;
            if (GetLevel() <= 15)
            {
                HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
                while (walk.HexNumber < count)
                {
                    if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                    {
                        mapLevel.Map.InvalidateStaticScene(walk.X, walk.Y, Owner.HexLevel);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }

            if (!GetEditMode())
            {
                var hexNumberToRadius = HexGeometry.HexNumberToRadius(count);
                mapLevel.Map.TerrainChangedRadius(StoredHexX, StoredHexY, (sbyte)mapLevel.HexLevel, hexNumberToRadius);
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
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            mapLevel = Owner.Owner;
            Disconnect();
            if (!mapLevel.Map.Status.Contains(HexSpriteMapStatusElement.Destroying))
            {
                count = GetTerrainCount();
                HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
                while (walk.HexNumber < count)
                {
                    terrainType = GetTerrain(walk.HexNumber);
                    overlayType = GetOverlay(walk.HexNumber);
                    if ((terrainType >= -1 || overlayType >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                    {
                        mapField = mapLevel.GetField(walk.X, walk.Y);
                        if (terrainType >= 0 || overlayType >= 0)
                        {
                            mapField.UpdateTerrain();
                        }

                        if (GetLevel() <= 15)
                        {
                            mapLevel.Map.InvalidateStaticScene(walk.X, walk.Y, mapLevel.HexLevel);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                var hexNumberToRadius = HexGeometry.HexNumberToRadius(count);
                mapLevel.Map.TerrainChangedRadius(StoredHexX, StoredHexY, (sbyte)mapLevel.HexLevel, hexNumberToRadius);
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
        if (Resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
            hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, hexX, hexY);
            result = hexNumber < Resource.TerrainList.Count && (Resource.TerrainList.GetTerrain(hexNumber) >= 0 || Resource.TerrainList.GetOverlay(hexNumber) >= 0);
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
        count = Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (result && walk.HexNumber < count)
        {
            terrainType = GetTerrain(walk.HexNumber);
            overlayType = GetOverlay(walk.HexNumber);
            if ((terrainType >= 0 || overlayType >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapContainer.Width && walk.Y < mapContainer.Height)
            {
                hexSprite = mapContainer.GetField(walk.X, walk.Y, hexLevel).GetMaxLevelNoneTransparentHexSprite();
                result = hexSprite == null || CanPlaceOnHexSprite(hexSprite);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, Resource.TerrainList);
    }

    // MAPOBJ.PAS:1138
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        StoredHexX = hexX;
        StoredHexY = hexY;
        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // MAPOBJ.PAS:1214
    public virtual void SortMapFields()
    {
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        mapLevel = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
        while (true)
        {
            var terrainCount = GetTerrainCount();
            if (!(walk.HexNumber < terrainCount))
                break;
            if (GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
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
        Span<byte> counts = stackalloc byte[128];
        HexNumberWalkData walk = default;
        sbyte result = defaultValue;
        terrainList = Resource.TerrainList;
        if (result < 0)
        {
            result = 0;
        }

        counts.Clear();
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (walk.HexNumber < terrainList.Count)
        {
            if ((terrainList.GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                terrainType = mapLevel.GetField(walk.X, walk.Y).TerrainType;
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
        count = Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (walk.HexNumber != count)
        {
            if ((GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
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
        count = Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (walk.HexNumber != count)
        {
            if ((GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
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
        Span<sbyte> terrainAndOverlay = stackalloc sbyte[2];
        HexNumberWalkData walk = default;
        mapLevel = Owner.Owner;
        count = GetTerrainCount();
        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
        while (walk.HexNumber < count)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                var terrain = GetTerrain(walk.HexNumber);
                terrainAndOverlay[0] = terrain;
                var overlay = GetOverlay(walk.HexNumber);
                terrainAndOverlay[1] = overlay;
                mapField = mapLevel.GetField(walk.X, walk.Y);
                if (terrainAndOverlay[0] >= 0)
                {
                    mapField.TerrainType = terrainAndOverlay[0];
                }

                if (terrainAndOverlay[1] >= 0)
                {
                    mapField.OverlayType = terrainAndOverlay[1];
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
        while (walk.HexNumber < count)
        {
            if (GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                // { Original skips bounds for terrain. }
                mapLevel.GetField(walk.X, walk.Y).TerrainChanged();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
