// Translated from HxBridge.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.HxBridge;

// FTerrain, FDirection (-1 invalid), FSideA, FSideB
public partial class THexagonBridge : TAbstractRoad
{
    public THexagonBridge(TECustomNode owner) : base(owner)
    {
        FDirection = -1;
        FSideA = new TByteList();
        FSideB = new TByteList();
    }

    // HxBridge.pas:84
    public new const int ClassID = 133242;

    public sbyte FTerrain;
    public sbyte FDirection;
    public TByteList FSideA;
    public TByteList FSideB;

    // HxBridge.pas:77
    public override void Destroy()
    {
        base.Destroy();
        FSideA.Free();
        FSideB.Free();
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
        var mapField = (TMapField)Owner;
        Xhp = mapField.Xhx * 32 + 8;
        Yhp = (mapField.Xhx & 1) * 16 + mapField.Yhx * 32;

        return 1744830464 + (Yhp << 13) + (Xhp + 8);
    }

    // HxBridge.pas:117
    public override void Connect()
    {
        THNWalkData Walk = default;
        TMapLevel Level;
        Level = (TMapLevel)Owner.Owner;
        InitHNWalk(ref Walk, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, 1);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Level.GetField(Walk.X, Walk.Y).AddHS(this, Walk.HN);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                WalkToNextHN(ref Walk);
            }
        }

        base.Connect();
    }

    // HxBridge.pas:135
    public override void Disconnect()
    {
        THNWalkData Walk = default;
        TMapLevel Level;
        Level = (TMapLevel)Owner.Owner;
        InitHNWalk(ref Walk, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, 1);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Level.GetField(Walk.X, Walk.Y).RemoveHS(this);
                // { Original bug: the walk never advances when a neighbour is out of bounds. }
                WalkToNextHN(ref Walk);
            }
        }

        base.Disconnect();
    }

    // HxBridge.pas:327
    public override void ChangeTerrain(in THSTerrainInfo Msg)
    {
        base.ChangeTerrain(Msg);
    }

    // HxBridge.pas:321
    public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Msg)
    {
        UpdateTransitions();
        if (FDirection == -1)
        {
            Destroy();
        }
    }

    // HxBridge.pas:186
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        FResource.ImageLibrary[FDirection].Show(Xhp - XhpSurf - 23, Yhp - YhpSurf - 20, SurfaceDesc);
    }

    // HxBridge.pas:160
    public override void MoveExclusive(in THSSenderInfo Info)
    {
        int Xhx = default;
        int Yhx = default;
        TMapField MapField;
        if (Info.HN != 0)
        {
            if (FSideA.IndexOf((byte)Info.HN) != -1)
            {
                for (int I = 0, last = FSideB.Count - 1; I <= last; I++)
                {
                    var sideB = FSideB[I];
                    CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, sideB, ref Xhx, ref Yhx);
                    MapField = AoWHSMap.MapContainer.GetField(Xhx, Yhx, ((TMapField)Owner).Lhx);
                    ((TMoveControl)Info.Sender).ExecuteExternalMove((TAoWMapField)Info.MapField, (TAoWMapField)MapField, ((TMoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
                }
            }
            else
            {
                if (FSideB.IndexOf((byte)Info.HN) != -1)
                {
                    for (int I = 0, last = FSideA.Count - 1; I <= last; I++)
                    {
                        var sideA = FSideA[I];
                        CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, sideA, ref Xhx, ref Yhx);
                        MapField = AoWHSMap.MapContainer.GetField(Xhx, Yhx, ((TMapField)Owner).Lhx);
                        ((TMoveControl)Info.Sender).ExecuteExternalMove((TAoWMapField)Info.MapField, (TAoWMapField)MapField, ((TMoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
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
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        if (Info.HN != 0)
        {
            ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 32768;
        }
    }

    // HxBridge.pas:94
    public override bool LinkToResource(TEngine AEngine)
    {
        bool Result = base.LinkToResource(AEngine);
        FTerrain = FResource.TerrainList.Terrain[0];
        return Result;
    }

    // HxBridge.pas:200
    public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        TMapField MapField;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        return base.CanPlace(MapContainer, Xhx, Yhx, Lhx) && (DirectionValid(MapField, 0) || DirectionValid(MapField, 1) || DirectionValid(MapField, 2));
    }

    // HxBridge.pas:209
    public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        THexagonSprite Road;
        TMapField MapField;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        Road = MapField.FindHS(typeof(TAbstractRoad));
        if (Road != null)
        {
            Road.Destroy();
        }

        Road = (THexagonSprite)(object)Owner;
        Owner = MapField;
        FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
        FTerrain = MapField.TerrainType;
        // { Original bug: refresh the old owner's owner as a map field. }
        if (Road != null)
        {
            ((TMapField)Road.Owner).UpdateOverlayType();
        }

        MapField.UpdateOverlayType();
        MapContainer.Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
        UpdateTransitions();
        return FDirection != -1;
    }

    // HxBridge.pas:309
    public override void UpdateTransitions()
    {
        if (FDirection == -1 || !DirectionValid((TMapField)Owner, FDirection))
        {
            if (DirectionValid((TMapField)Owner, 1))
            {
                FDirection = 1;
            }
            else
            {
                if (DirectionValid((TMapField)Owner, 2))
                {
                    FDirection = 2;
                }
                else
                {
                    if (DirectionValid((TMapField)Owner, 0))
                    {
                        FDirection = 0;
                    }
                    else
                    {
                        if (AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
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
    public bool DirectionValid(TMapField MapField, sbyte Direction)
    {
        TMapLevel Level;
        int Xhx = default;
        int Yhx = default;
        bool Result = false;
        Level = (TMapLevel)MapField.Owner;
        CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 1, ref Xhx, ref Yhx);
        if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
        {
            if (Land(Level.GetField(Xhx, Yhx).TerrainType))
            {
                CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 4, ref Xhx, ref Yhx);
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
        TMapLevel Level;
        FSideB.Clear();
        FSideA.Clear();
        if (FDirection != -1)
        {
            Level = (TMapLevel)Owner.Owner;
            Xhx = ((TMapField)Owner).Xhx;
            Yhx = ((TMapField)Owner).Yhx;
            // { Original bug: the neighbour lookups here have no bounds checks. }
            switch (FDirection)
            {
                case 0:
                    CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(2);
                    }

                    CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(3);
                    }

                    CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(5);
                    }

                    CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(6);
                    }

                    break;
                case 1:
                    CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(3);
                    }

                    CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(4);
                    }

                    CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(6);
                    }

                    CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(1);
                    }

                    break;
                case 2:
                    CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(1);
                    }

                    CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideB.Add(2);
                    }

                    CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(4);
                    }

                    CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                    if (!Land(Level.GetField(X, Y).TerrainType))
                    {
                        FSideA.Add(5);
                    }

                    break;
            }
        }
    }

    // HxBridge.pas:25

}

public partial class THexagonBridgeResource : TAbstractRoadResource
{
    public THexagonBridgeResource(TECustomNode owner) : base(owner)
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
    public override string ResourceEditName() => BridgeStr;

}

[StructLayout(LayoutKind.Explicit, Size = 305)]
public struct TBridgeMoveData
{
    [FieldOffset(0)]
    public FixedArray_byte_49 Reserved;
    [FieldOffset(49)]
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
}

public static partial class HxBridgeUnit
{
    // HxBridge.pas:353
    public static void RegisterHexagonBridge(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<THexagonBridge>(), EClass.Of<THexagonBridgeResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
