// Translated from HxBridge.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

// FTerrain, FDirection (-1 invalid), FSideA, FSideB
public partial class HexagonBridge : AbstractRoad
{
    public HexagonBridge(ECustomNode owner) : base(owner)
    {
        FDirection = -1;
        FSideA = new ByteList();
        FSideB = new ByteList();
    }

    // HxBridge.pas:84
    public new const int ClassID = 133242;

    public sbyte FTerrain;
    public sbyte FDirection;
    public ByteList FSideA;
    public ByteList FSideB;

    // HxBridge.pas:77
    public override void Dispose()
    {
        base.Dispose();
        FSideA?.Dispose();
        FSideB?.Dispose();
    }

    // HxBridge.pas:69

    // HxBridge.pas:100
    public override sbyte GetTerrain(int Index)
    {
        sbyte Result;
        if (Index == 0)
        {
            Result = FTerrain;
        }
        else
        {
            Result = -2;
        }

        return Result;
    }

    // HxBridge.pas:89
    public override byte GetLevel() => 26;

    // HxBridge.pas:105
    public override int GetShowPriority()
    {
        int Xhp;
        int Yhp;
        var mapField = (MapField)Owner;
        Xhp = mapField.Xhx * 32 + 8;
        Yhp = (mapField.Xhx & 1) * 16 + mapField.Yhx * 32;

        return 1744830464 + (Yhp << 13) + (Xhp + 8);
    }

    // HxBridge.pas:117
    public override void Connect()
    {
        HNWalkData Walk = default;
        MapLevel Level;
        Level = (MapLevel)Owner.Owner;
        HNWalkData.InitHNWalk(ref Walk, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, 1);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Level.GetField(Walk.X, Walk.Y).AddHS(this, Walk.HN);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        base.Connect();
    }

    // HxBridge.pas:135
    public override void Disconnect()
    {
        HNWalkData Walk = default;
        MapLevel Level;
        Level = (MapLevel)Owner.Owner;
        HNWalkData.InitHNWalk(ref Walk, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, 1);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Level.GetField(Walk.X, Walk.Y).RemoveHS(this);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        base.Disconnect();
    }

    // HxBridge.pas:327
    public override void ChangeTerrain(in HSTerrainInfo Msg)
    {
        base.ChangeTerrain(Msg);
    }

    // HxBridge.pas:321
    public override void NeighbourTerrainChanged(int HN, in HSTerrainInfo Msg)
    {
        UpdateTransitions();
        if (FDirection == -1)
        {
            Dispose();
        }
    }

    // HxBridge.pas:186
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        FResource.ImageLibrary[FDirection].Show(Xhp - XhpSurf - 23, Yhp - YhpSurf - 20, SurfaceDesc);
    }

    // HxBridge.pas:160
    public override void MoveExclusive(in HSSenderInfo Info)
    {
        int Xhx = default;
        int Yhx = default;
        MapField MapField;
        if (Info.HN != 0)
        {
            if (FSideA.IndexOf((byte)Info.HN) != -1)
            {
                for (int I = 0, last = FSideB.Count - 1; I <= last; I++)
                {
                    var sideB = FSideB[I];
                    HexGeometry.CenterHNtoHX(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, sideB, ref Xhx, ref Yhx);
                    MapField = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, ((MapField)Owner).Lhx);
                    ((MoveControl)Info.Sender).ExecuteExternalMove((AoWMapField)Info.MapField, (AoWMapField)MapField, ((MoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
                }
            }
            else
            {
                if (FSideB.IndexOf((byte)Info.HN) != -1)
                {
                    for (int I = 0, last = FSideA.Count - 1; I <= last; I++)
                    {
                        var sideA = FSideA[I];
                        HexGeometry.CenterHNtoHX(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, sideA, ref Xhx, ref Yhx);
                        MapField = AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, ((MapField)Owner).Lhx);
                        ((MoveControl)Info.Sender).ExecuteExternalMove((AoWMapField)Info.MapField, (AoWMapField)MapField, ((MoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
                    }
                }
            }
        }
    }

    // HxBridge.pas:194
    public override void Activate()
    {
        base.Activate();
        UpdateExclusiveMoves();
    }

    // HxBridge.pas:153
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN != 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 32768;
        }
    }

    // HxBridge.pas:94
    public override bool LinkToResource(Engine.Engine AEngine)
    {
        bool Result = base.LinkToResource(AEngine);
        FTerrain = FResource.TerrainList.Terrain[0];
        return Result;
    }

    // HxBridge.pas:200
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapField MapField;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        return base.CanPlace(MapContainer, Xhx, Yhx, Lhx) && (DirectionValid(MapField, 0) || DirectionValid(MapField, 1) || DirectionValid(MapField, 2));
    }

    // HxBridge.pas:209
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        HexagonSprite Road;
        MapField MapField;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        Road = MapField.FindHS(typeof(AbstractRoad));
        if (Road != null)
        {
            Road.Dispose();
        }

        ECustomNode previous = Owner;
        Owner = MapField;
        FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
        FTerrain = MapField.TerrainType;
        // { Original bug: refresh the old owner's owner as a map field. }
        // FOwner lives on TEObject, so the old owner has it. A nil owner or a non-field owner is undefined in the original, so skip it.
        if (previous?.Owner is MapField field)
        {
            field.UpdateOverlayType();
        }

        MapField.UpdateOverlayType();
        MapContainer.Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
        UpdateTransitions();
        return FDirection != -1;
    }

    // HxBridge.pas:309
    public override void UpdateTransitions()
    {
        if (FDirection == -1 || !DirectionValid((MapField)Owner, FDirection))
        {
            if (DirectionValid((MapField)Owner, 1))
            {
                FDirection = 1;
            }
            else
            {
                if (DirectionValid((MapField)Owner, 2))
                {
                    FDirection = 2;
                }
                else
                {
                    if (DirectionValid((MapField)Owner, 0))
                    {
                        FDirection = 0;
                    }
                    else
                    {
                        if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
                        {
                            FDirection = -1;
                        }
                    }
                }
            }

            UpdateExclusiveMoves();
        }
    }

    // HxBridge.pas:229
    public bool DirectionValid(MapField MapField, sbyte Direction)
    {
        MapLevel Level;
        int Xhx = default;
        int Yhx = default;
        bool Result = false;
        Level = (MapLevel)MapField.Owner;
        HexGeometry.CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 1, ref Xhx, ref Yhx);
        if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
        {
            if (Land(Level.GetField(Xhx, Yhx).TerrainType))
            {
                HexGeometry.CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 4, ref Xhx, ref Yhx);
                if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
                {
                    Result = Land(Level.GetField(Xhx, Yhx).TerrainType);
                }
            }
        }

        return Result;
    }

    // HxBridge.pas:246
    public void UpdateExclusiveMoves()
    {
        int Xhx;
        int Yhx;
        int X = default;
        int Y = default;
        MapLevel Level;
        FSideB.Clear();
        FSideA.Clear();
        if (FDirection != -1)
        {
            Level = (MapLevel)Owner.Owner;
            Xhx = ((MapField)Owner).Xhx;
            Yhx = ((MapField)Owner).Yhx;
            // { Original bug: the neighbour lookups here have no bounds checks. }
            switch (FDirection)
            {
                case 0:
                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(2);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(3);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(5);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(6);
                    }

                    break;
                case 1:
                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(3);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(4);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(6);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(1);
                    }

                    break;
                case 2:
                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(1);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(2);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(4);
                    }

                    HexGeometry.CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(5);
                    }

                    break;
            }
        }
    }

    private static bool Land(sbyte Terrain)
    {
        return !Set<byte>.WithBounds(0, 255).With((byte)0).With((byte)6)
            .With((byte)9)
            .With((byte)10)
            .With((byte)13)
            .Contains((byte)Terrain);
    }

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            Level = 10;
        }

        // HxBridge.pas:348
        public new const int ClassID = 133243;

        // HxBridge.pas:332

        // HxBridge.pas:338
        public override int ResourceUserClassID() => 133242;

        // HxBridge.pas:49

        // HxBridge.pas:343
        public override string ResourceEditName() => HexText.BridgeStr;

    }

    // HxBridge.pas:25

    // HxBridge.pas:25

}

[StructLayout(LayoutKind.Explicit, Size = 305)]
public struct BridgeMoveData
{
    [FieldOffset(0)]
    public FixedArray_byte_49 Reserved;
    [FieldOffset(49)]
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
}
