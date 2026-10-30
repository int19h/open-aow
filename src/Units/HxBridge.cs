// Translated from HxBridge.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.HxBridge
{
    // { THexagonBridge (instance size 28, VMT 557994E0): +$10 FTerrain,
//     +$11 FDirection (-1 invalid), +$14 FSideA, +$18 FSideB. }
    public partial class THexagonBridge : Aow.Units.HSEngine.TAbstractRoad
    {
        public THexagonBridge(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FDirection = unchecked((sbyte)-1);
            var call__1 = new Aow.Units.Engine.TByteList();
            FSideA = call__1;
            var call__2 = new Aow.Units.Engine.TByteList();
            FSideB = call__2;
        }

        // HxBridge.pas:84
        public new const int ClassID = 133242;

        public sbyte FTerrain;
        public sbyte FDirection;
        public Aow.Units.FixedArray_byte_2 FReserved;
        public Aow.Units.Engine.TByteList FSideA;
        public Aow.Units.Engine.TByteList FSideB;
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
                Result = unchecked((sbyte)FTerrain);
            }
            else
            {
                Result = unchecked((sbyte)-2);
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
            var mapField = (Aow.Units.HSEngine.TMapField)Owner;
            Xhp = (mapField.Xhx * 32) + 8;
            Yhp = ((mapField.Xhx & 1) * 16) + (mapField.Yhx * 32);

            Result = (1744830464 + (Yhp << 13)) + (Xhp + 8);
            return Result;
        }

        // HxBridge.pas:117
        public override void Connect()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, 1);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__4 = Level.GetField(Walk.X, Walk.Y);
                    call__4.AddHS(this, Walk.HN);
                    // { Original bug: the walk never advances when a neighbour is out of bounds. }
                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }
            }

            base.Connect();
        }

        // HxBridge.pas:135
        public override void Disconnect()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, 1);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__5 = Level.GetField(Walk.X, Walk.Y);
                    call__5.RemoveHS(this);
                    // { Original bug: the walk never advances when a neighbour is out of bounds. }
                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }
            }

            base.Disconnect();
        }

        // HxBridge.pas:327
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            base.ChangeTerrain(Msg);
        }

        // HxBridge.pas:321
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            UpdateTransitions();
            if (FDirection == -1)
            {
                Destroy();
            }
        }

        // HxBridge.pas:186
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            var call__6 = FResource.ImageLibrary[FDirection];
            call__6.Show((Xhp - XhpSurf) - 23, (Yhp - YhpSurf) - 20, SurfaceDesc);
        }

        // HxBridge.pas:160
        public override void MoveExclusive(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            int I = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            if (Info.HN != 0)
            {
                var call__7 = FSideA.IndexOf(unchecked((byte)Info.HN));
                if (call__7 != -1)
                {
                    {
                        I = 0;
                        int end__8 = FSideB.Count - 1;
                        for (; I <= end__8; I += 1)
                        {
                            var call__9 = FSideB[I];
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, call__9, ref Xhx, ref Yhx);
                            var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Xhx, Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
                            MapField = call__10;
                            ((Aow.Units.AoWE.TMoveControl)Info.Sender).ExecuteExternalMove((Aow.Units.AoWE.TAoWMapField)Info.MapField, (Aow.Units.AoWE.TAoWMapField)MapField, ((Aow.Units.AoWE.TMoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
                        }
                    }
                }
                else
                {
                    var call__11 = FSideB.IndexOf(unchecked((byte)Info.HN));
                    if (call__11 != -1)
                    {
                        {
                            I = 0;
                            int end__12 = FSideA.Count - 1;
                            for (; I <= end__12; I += 1)
                            {
                                var call__13 = FSideA[I];
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, call__13, ref Xhx, ref Yhx);
                                var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Xhx, Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
                                MapField = call__14;
                                ((Aow.Units.AoWE.TMoveControl)Info.Sender).ExecuteExternalMove((Aow.Units.AoWE.TAoWMapField)Info.MapField, (Aow.Units.AoWE.TAoWMapField)MapField, ((Aow.Units.AoWE.TMoveControl)Info.Sender).FMovePointTable[MapField.TerrainType][MapField.OverlayType - -1]);
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
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN != 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 32768;
            }
        }

        // HxBridge.pas:94
        public override bool LinkToResource(Aow.Units.Engine.TEngine AEngine)
        {
            bool Result = default;
            Result = base.LinkToResource(AEngine);
            var call__15 = FResource.TerrainList.Terrain[0];
            FTerrain = unchecked((sbyte)call__15);
            return Result;
        }

        // HxBridge.pas:200
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            var call__16 = base.CanPlace(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
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
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.THexagonSprite Road = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            Road = MapField.FindHS(typeof(Aow.Units.HSEngine.TAbstractRoad));
            if (Road != null)
            {
                Road.Destroy();
            }

            Road = (Aow.Units.HSEngine.THexagonSprite)((System.Object)Owner);
            Owner = MapField;
            FStatus = FStatus + TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap);
            FTerrain = unchecked((sbyte)MapField.TerrainType);
            // { Original bug: refresh the old owner's owner as a map field. }
            if (Road != null)
            {
                ((Aow.Units.HSEngine.TMapField)Road.Owner).UpdateOverlayType();
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
            bool bool__23 = (FDirection == -1);
            if (!bool__23)
            {
                var call__24 = DirectionValid((Aow.Units.HSEngine.TMapField)Owner, unchecked((sbyte)FDirection));
                bool__23 = !call__24;
            }

            if (bool__23)
            {
                var call__25 = DirectionValid((Aow.Units.HSEngine.TMapField)Owner, 1);
                if (call__25)
                {
                    FDirection = 1;
                }
                else
                {
                    var call__26 = DirectionValid((Aow.Units.HSEngine.TMapField)Owner, 2);
                    if (call__26)
                    {
                        FDirection = 2;
                    }
                    else
                    {
                        var call__27 = DirectionValid((Aow.Units.HSEngine.TMapField)Owner, 0);
                        if (call__27)
                        {
                            FDirection = 0;
                        }
                        else
                        {
                            if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                            {
                                FDirection = unchecked((sbyte)-1);
                            }
                        }
                    }
                }

                UpdateExclusiveMoves();
            }
        }

        // HxBridge.pas:229
        public bool DirectionValid(Aow.Units.HSEngine.TMapField MapField, sbyte Direction)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            int Xhx = default;
            int Yhx = default;
            Result = false;
            Level = (Aow.Units.HSEngine.TMapLevel)MapField.Owner;
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 1, ref Xhx, ref Yhx);
            if ((((Xhx >= 0) && (Yhx >= 0)) && (Xhx < Level.Width)) && (Yhx < Level.Height))
            {
                var call__28 = Level.GetField(Xhx, Yhx);
                var call__29 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__28.TerrainType));
                if (call__29)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(MapField.Xhx, MapField.Yhx, Direction + 4, ref Xhx, ref Yhx);
                    if ((((Xhx >= 0) && (Yhx >= 0)) && (Xhx < Level.Width)) && (Yhx < Level.Height))
                    {
                        var call__30 = Level.GetField(Xhx, Yhx);
                        var call__31 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__30.TerrainType));
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
            Aow.Units.HSEngine.TMapLevel Level = default;
            FSideB.Clear();
            FSideA.Clear();
            if (FDirection != -1)
            {
                Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                Xhx = ((Aow.Units.HSEngine.TMapField)Owner).Xhx;
                Yhx = ((Aow.Units.HSEngine.TMapField)Owner).Yhx;
                // { Original bug: the neighbour lookups here have no bounds checks. }
                switch (FDirection)
                {
                    case 0:
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                        var call__32 = Level.GetField(X, Y);
                        var call__33 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__32.TerrainType));
                        if (!call__33)
                        {
                            FSideB.Add(2);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                        var call__34 = Level.GetField(X, Y);
                        var call__35 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__34.TerrainType));
                        if (!call__35)
                        {
                            FSideB.Add(3);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                        var call__36 = Level.GetField(X, Y);
                        var call__37 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__36.TerrainType));
                        if (!call__37)
                        {
                            FSideA.Add(5);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                        var call__38 = Level.GetField(X, Y);
                        var call__39 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__38.TerrainType));
                        if (!call__39)
                        {
                            FSideA.Add(6);
                        }

                        break;
                    case 1:
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 3, ref X, ref Y);
                        var call__40 = Level.GetField(X, Y);
                        var call__41 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__40.TerrainType));
                        if (!call__41)
                        {
                            FSideB.Add(3);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                        var call__42 = Level.GetField(X, Y);
                        var call__43 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__42.TerrainType));
                        if (!call__43)
                        {
                            FSideB.Add(4);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 6, ref X, ref Y);
                        var call__44 = Level.GetField(X, Y);
                        var call__45 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__44.TerrainType));
                        if (!call__45)
                        {
                            FSideA.Add(6);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                        var call__46 = Level.GetField(X, Y);
                        var call__47 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__46.TerrainType));
                        if (!call__47)
                        {
                            FSideA.Add(1);
                        }

                        break;
                    case 2:
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 1, ref X, ref Y);
                        var call__48 = Level.GetField(X, Y);
                        var call__49 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__48.TerrainType));
                        if (!call__49)
                        {
                            FSideB.Add(1);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 2, ref X, ref Y);
                        var call__50 = Level.GetField(X, Y);
                        var call__51 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__50.TerrainType));
                        if (!call__51)
                        {
                            FSideB.Add(2);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 4, ref X, ref Y);
                        var call__52 = Level.GetField(X, Y);
                        var call__53 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__52.TerrainType));
                        if (!call__53)
                        {
                            FSideA.Add(4);
                        }

                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, 5, ref X, ref Y);
                        var call__54 = Level.GetField(X, Y);
                        var call__55 = Aow.Units.AoWE.AoWE.Land(unchecked((sbyte)call__54.TerrainType));
                        if (!call__55)
                        {
                            FSideA.Add(5);
                        }

                        break;
                }
            }
        }

        // HxBridge.pas:25


        public new partial class Meta : Aow.Units.HSEngine.TAbstractRoad.Meta
        {
            // HxBridge.pas:84
            public override int ClassID() => Aow.Units.HxBridge.THexagonBridge.ClassID;

            // HxBridge.pas:25
            public new Aow.Units.HxBridge.THexagonBridge Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.HxBridge.THexagonBridge)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.HxBridge.THexagonBridge => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { THexagonBridgeResource (instance size 52, VMT 5579967C) }
    public partial class THexagonBridgeResource : Aow.Units.HSEngine.TAbstractRoadResource
    {
        public THexagonBridgeResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.HSEngine.HSEngine.BridgeStr;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // HxBridge.pas:348
            public override int ClassID() => Aow.Units.HxBridge.THexagonBridgeResource.ClassID;

            // HxBridge.pas:49
            public new Aow.Units.HxBridge.THexagonBridgeResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.HxBridge.THexagonBridgeResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.HxBridge.THexagonBridgeResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 305)]
    public struct TBridgeMoveData
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_49 Reserved;
        [FieldOffset(49)]
        public Aow.Units.FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
    }

    public static partial class HxBridge
    {
        // HxBridge.pas:353
        public static void RegisterHexagonBridge(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.HxBridge.THexagonBridge>(), Aow.Units.Engine.EClass.Of<Aow.Units.HxBridge.THexagonBridgeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
