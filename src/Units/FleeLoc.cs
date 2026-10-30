// Translated from FleeLoc.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
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
using Aow.Units.CombatObstacle;
using static Aow.Units.CombatObstacle.CombatObstacle;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.FleeLoc
{
    // { TFleeLocation (instance size 28, VMT 00430EB4) }
    public partial class TFleeLocation : Aow.Units.HSEngine.TMapObject
    {
        public TFleeLocation(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // FleeLoc.pas:76
        public new const int ClassID = 2229544;

        public bool FHighlighted;
        // { $10 }
        public Aow.Units.FixedArray_byte_3 F_FleeLocation_11;
        public int FAlpha;
        // { $14 }
        public int FAlphaStep;
        // FleeLoc.pas:71
        // { $18 }


        // FleeLoc.pas:82
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = Yhx;
            var call__2 = Xhx;
            Result = (1744830464 + (call__1 << 13)) + call__2;
            return Result;
        }

        // FleeLoc.pas:151
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            if (FAlpha > 0)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
                var call__3 = FResource.ImageLibrary.Get(0);
                call__3.ShowBlended((X - XhpSurf) - 8, Y - YhpSurf, SurfaceDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[FAlpha]);
            }
        }

        // FleeLoc.pas:120
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                var call__4 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__4 == Aow.Units.AoWE.TCombatSideIndex.csAttacker)
                {
                    MoveResult.Code = 25;
                }
                else
                {
                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
                }

                MoveResult.Done = true;
            }
        }

        // FleeLoc.pas:111
        public override void Activate()
        {
            base.Activate();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Add(NewFrame);
            FHighlighted = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (FHighlighted)
            {
                FAlpha = 50;
            }
            else
            {
                FAlpha = 0;
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFleeLocations.Add(this);
        }

        // FleeLoc.pas:65
        public override void Deactivate()
        {
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
            base.Deactivate();
        }

        // FleeLoc.pas:90
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // FleeLoc.pas:79
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // FleeLoc.pas:87
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // FleeLoc.pas:93
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.Engine.TEObject OldHexagon = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            if (!GetEditMode())
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                OldHexagon = MapField.FindOwnedChild(132439);
                if (OldHexagon != null)
                {
                    OldHexagon.Free();
                    Aow.Units.AoWE.AoWE.AoWHSSet.FCombatHexagons.Place(Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), 0, 1);
                }
            }

            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            return Result;
        }

        // FleeLoc.pas:132
        public void NewFrame(Aow.Rtl.TObject Sender)
        {
            bool Highlight = default;
            Highlight = false;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Highlight = true;
            }
            else
            {
                var call__5 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                bool bool__6 = (call__5 != null);
                if (bool__6)
                {
                    var call__7 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                    bool__6 = (call__7.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer);
                }

                bool bool__8 = bool__6;
                if (bool__8)
                {
                    var call__9 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                    bool__8 = (call__9.FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FDefenderPlayer);
                }

                if (bool__8)
                {
                    Highlight = true;
                }
            }

            if (Highlight != FHighlighted)
            {
                FHighlighted = Highlight;
                if (FHighlighted)
                {
                    FAlphaStep = 5;
                }
                else
                {
                    FAlphaStep = -5;
                }
            }
            else
            {
                if (((FAlpha > 0) && (FAlphaStep < 0)) || ((FAlpha < 50) && (FAlphaStep > 0)))
                {
                    FAlpha = FAlpha + FAlphaStep;
                }
            }
        }

        // FleeLoc.pas:27


        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // FleeLoc.pas:76
            public override int ClassID() => Aow.Units.FleeLoc.TFleeLocation.ClassID;

            // FleeLoc.pas:27
            public new Aow.Units.FleeLoc.TFleeLocation Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FleeLoc.TFleeLocation)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.FleeLoc.TFleeLocation => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFleeLocationResource (instance size 52, VMT 00431048) }
    public partial class TFleeLocationResource : Aow.Units.HSEngine.TMapObjectResource
    {
        public TFleeLocationResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // FleeLoc.pas:163
        public new const int ClassID = 2229545;

        // FleeLoc.pas:169
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229544;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // FleeLoc.pas:163
            public override int ClassID() => Aow.Units.FleeLoc.TFleeLocationResource.ClassID;

            // FleeLoc.pas:166
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Flee Location";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.FleeLoc.TFleeLocationResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1124
            public new Aow.Units.FleeLoc.TFleeLocationResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FleeLoc.TFleeLocationResource)base.Create(AOwner);
        }
    }

    // { TMovementHS (instance size 16, VMT 00431140) }
    public partial class TMovementHS : Aow.Units.HSEngine.TSingleHS
    {
        public TMovementHS(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // FleeLoc.pas:172
        public new const int ClassID = 2228490;

        public bool FEnabled;
        // { $0C }
        public Aow.Units.FixedArray_byte_3 F_MovementHS_0D;
        // FleeLoc.pas:175
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 32;
            return Result;
        }

        // FleeLoc.pas:187
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
        }

        // FleeLoc.pas:178
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            if ((Info.HN == 0) && !FEnabled)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
            }
        }

        // Engine.pas:524


        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // FleeLoc.pas:172
            public override int ClassID() => Aow.Units.FleeLoc.TMovementHS.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.FleeLoc.TMovementHS => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.FleeLoc.TMovementHS Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FleeLoc.TMovementHS)base.Create(AOwner);
        }
    }

    public static partial class FleeLoc
    {
        // FleeLoc.pas:191
        public static void RegisterFleeLocation(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.FleeLoc.TFleeLocationResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.FleeLoc.TFleeLocation>(), Aow.Units.Engine.EClass.Of<Aow.Units.FleeLoc.TMovementHS>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
