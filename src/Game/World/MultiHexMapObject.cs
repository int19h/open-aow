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

    protected internal sbyte FXhx;
    protected internal sbyte FYhx;

    // MAPOBJ.PAS:923
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 3);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 4);
    }

    // MAPOBJ.PAS:900
    public override sbyte GetXhx() => FXhx;

    // MAPOBJ.PAS:905
    public override sbyte GetYhx() => FYhx;

    // MAPOBJ.PAS:910
    public override int GetXYL() => (GetLhx() << 16) + (FXhx + (FYhx << 8));

    // MAPOBJ.PAS:915
    public override int GetShowPriority()
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        return FResource.FShowPriorityOffset + (Yhp << 13) + Xhp;
    }

    // MAPOBJ.PAS:944
    // { Connect adds the object to the fields of its terrain list around the main hexagon }
    public override void Connect()
    {
        int Count;
        MapLevel MapLevel;
        MapField MapField;
        HexNumberWalkData Walk = default;
        if (!FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            Count = GetTerrainCount();
            MapLevel = (MapLevel)Owner.Owner;
            HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count)
            {
                if ((GetTerrain(Walk.HN) >= -1 || GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapField = MapLevel.GetField(Walk.X, Walk.Y);
                    if (MapField == Owner)
                    {
                        MapField.RemoveHS(this);
                    }

                    MapField.AddHS(this, Walk.HN);
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // MAPOBJ.PAS:972
    public override void Disconnect()
    {
        int Count;
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Count = GetTerrainCount();
            MapLevel = (MapLevel)Owner.Owner;
            FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count)
            {
                if ((GetTerrain(Walk.HN) >= -1 || GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).RemoveHS(this);
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            Owner = null;
        }
    }

    // MAPOBJ.PAS:930
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        if (FResource.FImageSequenceList == null)
        {
            FResource.FImageLibrary.Get(0).Show(Xhp - XhpSurf + ((MultiHexMapObjectResource)FResource).FdXImage, Yhp - YhpSurf + ((MultiHexMapObjectResource)FResource).FdYImage, SurfaceDesc);
        }
        else
        {
            FResource.ShowComplexImage(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, 0, (uint)(Xhp + Yhp));
        }
    }

    // MAPOBJ.PAS:995
    public override bool PlaceOnMap()
    {
        int Count;
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        if (!FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            Connect();
            Count = GetTerrainCount();
            MapLevel = (MapLevel)Owner.Owner;
            if (GetLevel() <= 15)
            {
                HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count)
                {
                    if ((GetTerrain(Walk.HN) >= -1 || GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        MapLevel.FMap.InvalidateStaticScene(Walk.X, Walk.Y, ((MapField)Owner).FLhx);
                    }

                    HexNumberWalkData.WalkToNextHN(ref Walk);
                }
            }

            if (!GetEditMode())
            {
                var hNtoRad = HexGeometry.HNtoRad(Count);
                MapLevel.FMap.TerrainChangedRad(FXhx, FYhx, (sbyte)MapLevel.FLhx, hNtoRad);
            }
        }

        return true;
    }

    // MAPOBJ.PAS:1023
    public override void RemoveFromMap()
    {
        int Count;
        MapLevel MapLevel;
        MapField MapField;
        sbyte TerrainType;
        sbyte OverlayType;
        HexNumberWalkData Walk = default;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            MapLevel = (MapLevel)Owner.Owner;
            Disconnect();
            if (!MapLevel.FMap.FStatus.Contains(HexSpriteMapStatusElement.Destroying))
            {
                Count = GetTerrainCount();
                HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count)
                {
                    TerrainType = GetTerrain(Walk.HN);
                    OverlayType = GetOverlay(Walk.HN);
                    if ((TerrainType >= -1 || OverlayType >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        MapField = MapLevel.GetField(Walk.X, Walk.Y);
                        if (TerrainType >= 0 || OverlayType >= 0)
                        {
                            MapField.UpdateTerrain();
                        }

                        if (GetLevel() <= 15)
                        {
                            MapLevel.FMap.InvalidateStaticScene(Walk.X, Walk.Y, MapLevel.FLhx);
                        }
                    }

                    HexNumberWalkData.WalkToNextHN(ref Walk);
                }

                var hNtoRad = HexGeometry.HNtoRad(Count);
                MapLevel.FMap.TerrainChangedRad(FXhx, FYhx, (sbyte)MapLevel.FLhx, hNtoRad);
            }
        }
    }

    // MAPOBJ.PAS:1122
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        int Xhx = default;
        int Yhx = default;
        int HN;
        if (FResource.FEngine.FHSMap.FStatus.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            HN = HexGeometry.dHXtoHN(((MapField)Owner).FXhx, ((MapField)Owner).FYhx, Xhx, Yhx);
            Result = HN < FResource.FTerrainList.FCount && (FResource.FTerrainList.GetTerrain(HN) >= 0 || FResource.FTerrainList.GetOverlay(HN) >= 0);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MAPOBJ.PAS:1096
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        int Count;
        sbyte TerrainType;
        sbyte OverlayType;
        HexSprite HS;
        HexNumberWalkData Walk = default;
        bool Result = true;
        Count = FResource.FTerrainList.FCount;
        HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Result && Walk.HN < Count)
        {
            TerrainType = GetTerrain(Walk.HN);
            OverlayType = GetOverlay(Walk.HN);
            if ((TerrainType >= 0 || OverlayType >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapContainer.FWidth && Walk.Y < MapContainer.FHeight)
            {
                HS = MapContainer.GetField(Walk.X, Walk.Y, Lhx).GetMaxLevelNoneTransparentHS();
                Result = HS == null || CanPlaceOnHS(HS);
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        return Result && MapContainer.FMap.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.FTerrainList);
    }

    // MAPOBJ.PAS:1138
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        FXhx = Xhx;
        FYhx = Yhx;
        return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
    }

    // MAPOBJ.PAS:1214
    public virtual void SortMapFields()
    {
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        MapLevel = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (true)
        {
            var terrainCount = GetTerrainCount();
            if (!(Walk.HN < terrainCount))
                break;
            if (GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                // { Original skips bounds for terrain. }
                MapLevel.GetField(Walk.X, Walk.Y).Sort();
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // MAPOBJ.PAS:1091
    public virtual bool CanPlaceOnHS(HexSprite HS) => HS.GetLevel() < 10;

    // MAPOBJ.PAS:1146
    // { The terrain type that most of the fields under the object have (the counts of Default win ties) }
    public virtual sbyte GetMostFrequentMapFieldTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx, sbyte Default)
    {
        TerrainList TerrainList;
        sbyte TerrainType;
        ByteArray128 Counts;
        HexNumberWalkData Walk = default;
        sbyte Result = Default;
        TerrainList = FResource.FTerrainList;
        if (Result < 0)
        {
            Result = 0;
        }

        Counts = default;
        HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Walk.HN < TerrainList.FCount)
        {
            if ((TerrainList.GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                TerrainType = MapLevel.GetField(Walk.X, Walk.Y).FTerrainType;
                if (TerrainType >= 0)
                {
                    Counts[TerrainType] = (byte)(Counts[TerrainType] + 1);
                    if (Counts[TerrainType] > Counts[Result])
                    {
                        Result = TerrainType;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (Default >= 0 && Counts[Result] == Counts[Default])
        {
            Result = Default;
        }

        return Result;
    }

    // MAPOBJ.PAS:1059
    public void UpdateTerrain(MapLevel MapLevel, int Xhx, int Yhx)
    {
        int Count;
        HexNumberWalkData Walk = default;
        Count = FResource.FTerrainList.FCount;
        HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Walk.HN != Count)
        {
            if ((GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                MapLevel.GetField(Walk.X, Walk.Y).UpdateTerrain();
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // MAPOBJ.PAS:1075
    public void UpdateMapFields(MapLevel MapLevel, int Xhx, int Yhx)
    {
        int Count;
        HexNumberWalkData Walk = default;
        Count = FResource.FTerrainList.FCount;
        HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Walk.HN != Count)
        {
            if ((GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                MapLevel.GetField(Walk.X, Walk.Y).UpdateMapField();
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // MAPOBJ.PAS:1178
    public void OverrideTerrain()
    {
        int Count;
        MapLevel MapLevel;
        MapField MapField;
        SByteArray2 Terrain = default;
        HexNumberWalkData Walk = default;
        MapLevel = (MapLevel)Owner.Owner;
        Count = GetTerrainCount();
        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                var terrain = GetTerrain(Walk.HN);
                Terrain[0] = terrain;
                var overlay = GetOverlay(Walk.HN);
                Terrain[1] = overlay;
                MapField = MapLevel.GetField(Walk.X, Walk.Y);
                if (Terrain[0] >= 0)
                {
                    MapField.FTerrainType = Terrain[0];
                }

                if (Terrain[1] >= 0)
                {
                    MapField.FOverlayType = Terrain[1];
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        while (Walk.HN < Count)
        {
            if (GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                // { Original skips bounds for terrain. }
                MapLevel.GetField(Walk.X, Walk.Y).TerrainChanged();
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }
}
