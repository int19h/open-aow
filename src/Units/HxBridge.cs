// Translated from HxBridge.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.HxBridge
{
    // FTerrain, FDirection (-1 invalid), FSideA, FSideB
    public partial class THexagonBridge : TAbstractRoad
    {
        public THexagonBridge(TECustomNode owner) : base(owner)
        {
            FDirection = -1;
            var call__1 = new TByteList();
            FSideA = call__1;
            var call__2 = new TByteList();
            FSideB = call__2;
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
            sbyte Result = default;
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
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 26;
            return Result;
        }

        // HxBridge.pas:105
        public override int GetShowPriority()
        {
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            var mapField = (TMapField)Owner;
            Xhp = mapField.Xhx * 32 + 8;
            Yhp = (mapField.Xhx & 1) * 16 + mapField.Yhx * 32;

            Result = 1744830464 + (Yhp << 13) + (Xhp + 8);
            return Result;
        }

        // HxBridge.pas:117
        public override void Connect()
        {
            THNWalkData Walk = default;
            TMapLevel Level = default;
            Level = (TMapLevel)Owner.Owner;
            InitHNWalk(ref Walk, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, 1);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__4 = Level.GetField(Walk.X, Walk.Y);
                    call__4.AddHS(this, Walk.HN);
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
            TMapLevel Level = default;
            Level = (TMapLevel)Owner.Owner;
            InitHNWalk(ref Walk, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, 1);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__5 = Level.GetField(Walk.X, Walk.Y);
                    call__5.RemoveHS(this);
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
            var call__6 = FResource.ImageLibrary[FDirection];
            call__6.Show(Xhp - XhpSurf - 23, Yhp - YhpSurf - 20, SurfaceDesc);
        }

        // HxBridge.pas:160
        public override void MoveExclusive(in THSSenderInfo Info)
        {
            int I = default;
            int Xhx = default;
            int Yhx = default;
            TMapField MapField = default;
            if (Info.HN != 0)
            {
                var call__7 = FSideA.IndexOf((byte)Info.HN);
                if (call__7 != -1)
                {
                    {
                        I = 0;
                        int end__8 = FSideB.Count - 1;
                        for (; I <= end__8; I += 1)
                        {
                            var call__9 = FSideB[I];
                            CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, call__9, ref Xhx, ref Yhx);
                            var call__10 = AoWHSMap.MapContainer.GetField(Xhx, Yhx, ((TMapField)Owner).Lhx);
                            MapField = call__10;
                            ((TMoveControl)Info.Sender).ExecuteExternalMove((TAoWMapField)Info.MapField, (TAoWMapField)MapField, ((TMoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
                        }
                    }
                }
                else
                {
                    var call__11 = FSideB.IndexOf((byte)Info.HN);
                    if (call__11 != -1)
                    {
                        {
                            I = 0;
                            int end__12 = FSideA.Count - 1;
                            for (; I <= end__12; I += 1)
                            {
                                var call__13 = FSideA[I];
                                CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, call__13, ref Xhx, ref Yhx);
                                var call__14 = AoWHSMap.MapContainer.GetField(Xhx, Yhx, ((TMapField)Owner).Lhx);
                                MapField = call__14;
                                ((TMoveControl)Info.Sender).ExecuteExternalMove((TAoWMapField)Info.MapField, (TAoWMapField)MapField, ((TMoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
                            }
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
            bool Result = default;
            Result = base.LinkToResource(AEngine);
            var call__15 = FResource.TerrainList.Terrain[0];
            FTerrain = call__15;
            return Result;
        }

        // HxBridge.pas:200
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TMapField MapField = default;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            var call__16 = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
            bool bool__17 = call__16;
            if (bool__17)
            {
                var call__18 = DirectionValid(MapField, 0);
                bool bool__19 = call__18;
                if (!bool__19)
                {
                    var call__20 = DirectionValid(MapField, 1);
                    bool__19 = call__20;
                }

                bool bool__21 = bool__19;
                if (!bool__21)
                {
                    var call__22 = DirectionValid(MapField, 2);
                    bool__21 = call__22;
                }

                bool__17 = bool__21;
            }

            Result = bool__17;
            return Result;
        }

        // HxBridge.pas:209
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            THexagonSprite Road = default;
            TMapField MapField = default;
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
            Result = FDirection != -1;
            return Result;
        }

        // HxBridge.pas:309
        public override void UpdateTransitions()
        {
            bool bool__23 = FDirection == -1;
            if (!bool__23)
            {
                var call__24 = DirectionValid((TMapField)Owner, FDirection);
                bool__23 = !call__24;
            }

            if (bool__23)
            {
                var call__25 = DirectionValid((TMapField)Owner, 1);
                if (call__25)
                {
                    FDirection = 1;
                }
                else
                {
                    var call__26 = DirectionValid((TMapField)Owner, 2);
                    if (call__26)
                    {
                        FDirection = 2;
                    }
                    else
                    {
                        var call__27 = DirectionValid((TMapField)Owner, 0);
                        if (call__27)
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
            bool Result = default;
            TMapLevel Level = default;
            int Xhx = default;
            int Yhx = default;
            Result = false;
            Level = (TMapLevel)MapField.Owner;
            CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 1, ref Xhx, ref Yhx);
            if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
            {
                var call__28 = Level.GetField(Xhx, Yhx);
                var call__29 = Land(call__28.TerrainType);
                if (call__29)
                {
                    CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 4, ref Xhx, ref Yhx);
                    if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
                    {
                        var call__30 = Level.GetField(Xhx, Yhx);
                        var call__31 = Land(call__30.TerrainType);
                        Result = call__31;
                    }
                }
            }

            return Result;
        }

        // HxBridge.pas:246
        public void UpdateExclusiveMoves()
        {
            int Xhx = default;
            int Yhx = default;
            int X = default;
            int Y = default;
            TMapLevel Level = default;
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
                        var call__32 = Level.GetField(X, Y);
                        var call__33 = Land(call__32.TerrainType);
                        if (!call__33)
                        {
                            FSideB.Add(2);
                        }

                        CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                        var call__34 = Level.GetField(X, Y);
                        var call__35 = Land(call__34.TerrainType);
                        if (!call__35)
                        {
                            FSideB.Add(3);
                        }

                        CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                        var call__36 = Level.GetField(X, Y);
                        var call__37 = Land(call__36.TerrainType);
                        if (!call__37)
                        {
                            FSideA.Add(5);
                        }

                        CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                        var call__38 = Level.GetField(X, Y);
                        var call__39 = Land(call__38.TerrainType);
                        if (!call__39)
                        {
                            FSideA.Add(6);
                        }

                        break;
                    case 1:
                        CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                        var call__40 = Level.GetField(X, Y);
                        var call__41 = Land(call__40.TerrainType);
                        if (!call__41)
                        {
                            FSideB.Add(3);
                        }

                        CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                        var call__42 = Level.GetField(X, Y);
                        var call__43 = Land(call__42.TerrainType);
                        if (!call__43)
                        {
                            FSideB.Add(4);
                        }

                        CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                        var call__44 = Level.GetField(X, Y);
                        var call__45 = Land(call__44.TerrainType);
                        if (!call__45)
                        {
                            FSideA.Add(6);
                        }

                        CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                        var call__46 = Level.GetField(X, Y);
                        var call__47 = Land(call__46.TerrainType);
                        if (!call__47)
                        {
                            FSideA.Add(1);
                        }

                        break;
                    case 2:
                        CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                        var call__48 = Level.GetField(X, Y);
                        var call__49 = Land(call__48.TerrainType);
                        if (!call__49)
                        {
                            FSideB.Add(1);
                        }

                        CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                        var call__50 = Level.GetField(X, Y);
                        var call__51 = Land(call__50.TerrainType);
                        if (!call__51)
                        {
                            FSideB.Add(2);
                        }

                        CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                        var call__52 = Level.GetField(X, Y);
                        var call__53 = Land(call__52.TerrainType);
                        if (!call__53)
                        {
                            FSideA.Add(4);
                        }

                        CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                        var call__54 = Level.GetField(X, Y);
                        var call__55 = Land(call__54.TerrainType);
                        if (!call__55)
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133242;
            return Result;
        }

        // HxBridge.pas:49


        // HxBridge.pas:343
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = BridgeStr;
            return Result;
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 305)]
    public struct TBridgeMoveData
    {
        [FieldOffset(0)]
        public FixedArray_byte_49 Reserved;
        [FieldOffset(49)]
        public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
    }

    public static partial class HxBridge
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
}
