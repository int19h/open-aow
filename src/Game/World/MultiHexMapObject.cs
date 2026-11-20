#nullable disable
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
public class MultiHexMapObject(CustomNode owner) : MapObject(owner)
{
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
    public override sbyte HexX => StoredHexX;

    // MAPOBJ.PAS:905
    public override sbyte HexY => StoredHexY;

    // MAPOBJ.PAS:910
    public override MapCoordinate MapCoordinate => HexGeometry.PackMapCoordinate(StoredHexX, StoredHexY, HexLevel);

    // MAPOBJ.PAS:915
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        return Resource.ShowPriorityOffset + (pixelY << 13) + pixelX;
    }

    // MAPOBJ.PAS:944
    // { Connect adds the object to the fields of its terrain list around the main hexagon }
    public override void Connect()
    {
        if (!Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Status += Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            int count = TerrainCount;
            MapLevel mapLevel = Owner.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
            while (walk.HexNumber < count)
            {
                if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    MapField mapField = mapLevel.GetField(walk.X, walk.Y);
                    if (mapField == Owner)
                    {
                        _ = mapField.RemoveHexSprite(this);
                    }

                    _ = mapField.AddHexSprite(this, walk.HexNumber);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // MAPOBJ.PAS:972
    public override void Disconnect()
    {
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            int count = TerrainCount;
            MapLevel mapLevel = Owner.Owner;
            Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
            while (walk.HexNumber < count)
            {
                if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    _ = mapLevel.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            Owner = null;
        }
    }

    // MAPOBJ.PAS:930
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        if (Resource.ImageSequenceList is null)
        {
            // The offsets live on the multi-hex resource. Another resource throws, as the old cast did.
            MultiHexMapObjectResource placed = GetResource<MultiHexMapObjectResource>();
            Resource.ImageLibrary.Get(0).Show(pixelX - pixelXSurface + placed.ImageOffsetX, pixelY - pixelYSurface + placed.ImageOffsetY, lockedSurface);
        }
        else
        {
            Resource.ShowComplexImage(pixelX - pixelXSurface, pixelY - pixelYSurface, lockedSurface, index: 0, offset: (uint)(pixelX + pixelY));
        }
    }

    // MAPOBJ.PAS:995
    public override bool PlaceOnMap()
    {
        if (!Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Connect();
            int count = TerrainCount;
            MapLevel mapLevel = Owner.Owner;
            if (Level <= 15)
            {
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
                while (walk.HexNumber < count)
                {
                    if ((GetTerrain(walk.HexNumber) >= -1 || GetOverlay(walk.HexNumber) >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                    {
                        mapLevel.Map.InvalidateStaticScene(walk.X, walk.Y, Owner.HexLevel);
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }

            if (!EditMode)
            {
                int hexNumberToRadius = HexGeometry.HexNumberToRadius(count);
                mapLevel.Map.TerrainChangedRadius(StoredHexX, StoredHexY, L: (sbyte)mapLevel.HexLevel, radius: hexNumberToRadius);
            }
        }

        return true;
    }

    // MAPOBJ.PAS:1023
    public override void RemoveFromMap()
    {
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            MapLevel mapLevel = Owner.Owner;
            Disconnect();
            if (!mapLevel.Map.Status.Contains(HexSpriteMapStatusElement.Destroying))
            {
                int count = TerrainCount;
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
                while (walk.HexNumber < count)
                {
                    sbyte terrainType = GetTerrain(walk.HexNumber);
                    sbyte overlayType = GetOverlay(walk.HexNumber);
                    if ((terrainType >= -1 || overlayType >= -1) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                    {
                        MapField mapField = mapLevel.GetField(walk.X, walk.Y);
                        if (terrainType >= 0 || overlayType >= 0)
                        {
                            mapField.UpdateTerrain();
                        }

                        if (Level <= 15)
                        {
                            mapLevel.Map.InvalidateStaticScene(walk.X, walk.Y, mapLevel.HexLevel);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }

                int hexNumberToRadius = HexGeometry.HexNumberToRadius(count);
                mapLevel.Map.TerrainChangedRadius(StoredHexX, StoredHexY, L: (sbyte)mapLevel.HexLevel, radius: hexNumberToRadius);
            }
        }
    }

    // MAPOBJ.PAS:1122
    public override bool CanSelect(int pixelX, int pixelY)
    {
        if (Resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int hexX, out int hexY);
            int hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, hexX, hexY);
            return hexNumber < Resource.TerrainList.Count && (Resource.TerrainList.GetTerrain(hexNumber) >= 0 || Resource.TerrainList.GetOverlay(hexNumber) >= 0);
        }
        else
        {
            return false;
        }
    }

    // MAPOBJ.PAS:1096
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = true;
        int count = Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (result && walk.HexNumber < count)
        {
            sbyte terrainType = GetTerrain(walk.HexNumber);
            sbyte overlayType = GetOverlay(walk.HexNumber);
            if ((terrainType >= 0 || overlayType >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapContainer.Width && walk.Y < mapContainer.Height)
            {
                HexSprite hexSprite = mapContainer.GetField(walk.X, walk.Y, hexLevel).GetMaxLevelNoneTransparentHexSprite();
                result = hexSprite is null || CanPlaceOnHexSprite(hexSprite);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result && mapContainer.Map.CanChangeTerrain(this, hexX, hexY, hexLevel, terrain: Resource.TerrainList);
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
        MapLevel mapLevel = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (true)
        {
            int terrainCount = TerrainCount;
            if (!(walk.HexNumber < terrainCount))
            {
                break;
            }

            if (GetTerrain(walk.HexNumber) >= 0 || (GetOverlay(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height))
            {
                // { Original skips bounds for terrain. }
                mapLevel.GetField(walk.X, walk.Y).Sort();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // MAPOBJ.PAS:1091
    public virtual bool CanPlaceOnHexSprite(HexSprite hexSprite) => hexSprite.Level < 10;

    // MAPOBJ.PAS:1146
    // { The terrain type that most of the fields under the object have (the counts of Default win ties) }
    public virtual sbyte GetMostFrequentMapFieldTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY, sbyte defaultValue)
    {
        Span<byte> counts = stackalloc byte[128];
        sbyte result = defaultValue;
        TerrainList terrainList = Resource.TerrainList;
        if (result < 0)
        {
            result = 0;
        }

        counts.Clear();
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (walk.HexNumber < terrainList.Count)
        {
            if ((terrainList.GetTerrain(walk.HexNumber) >= 0 || GetOverlay(walk.HexNumber) >= 0) && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                sbyte terrainType = mapLevel.GetField(walk.X, walk.Y).TerrainType;
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

        return defaultValue >= 0 && counts[result] == counts[defaultValue] ? defaultValue : result;
    }

    // MAPOBJ.PAS:1059
    public void UpdateTerrain(MapLevel mapLevel, int hexX, int hexY)
    {
        int count = Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
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
        int count = Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
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
        Span<sbyte> terrainAndOverlay = stackalloc sbyte[2];
        MapLevel mapLevel = Owner.Owner;
        int count = TerrainCount;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (walk.HexNumber < count)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                sbyte terrain = GetTerrain(walk.HexNumber);
                terrainAndOverlay[0] = terrain;
                sbyte overlay = GetOverlay(walk.HexNumber);
                terrainAndOverlay[1] = overlay;
                MapField mapField = mapLevel.GetField(walk.X, walk.Y);
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

        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, radius: 0);
        while (walk.HexNumber < count)
        {
            if (GetTerrain(walk.HexNumber) >= 0 || (GetOverlay(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height))
            {
                // { Original skips bounds for terrain. }
                mapLevel.GetField(walk.X, walk.Y).TerrainChanged();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
