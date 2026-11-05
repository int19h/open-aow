// Translated from FleeLoc.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.FleeLoc
{
    public partial class TFleeLocation : TMapObject
    {
        public TFleeLocation(TECustomNode owner) : base(owner)
        {
        }

        // FleeLoc.pas:76
        public new const int ClassID = 2229544;

        public bool FHighlighted;
        public int FAlpha;
        public int FAlphaStep;
        // FleeLoc.pas:71


        // FleeLoc.pas:82
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = Yhx;
            var call__2 = Xhx;
            Result = 1744830464 + (call__1 << 13) + call__2;
            return Result;
        }

        // FleeLoc.pas:151
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            if (FAlpha > 0)
            {
                HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
                var call__3 = FResource.ImageLibrary.Get(0);
                call__3.ShowBlended(X - XhpSurf - 8, Y - YhpSurf, SurfaceDesc, AlphaBlendTable.LookupTables[FAlpha]);
            }
        }

        // FleeLoc.pas:120
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                var call__4 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__4 == TCombatSideIndex.csAttacker)
                {
                    MoveResult.Code = 25;
                }
                else
                {
                    MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
                }

                MoveResult.Done = true;
            }
        }

        // FleeLoc.pas:111
        public override void Activate()
        {
            base.Activate();
            AoWCombatMap.NewFrameEvents.Add(NewFrame);
            FHighlighted = AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting);
            if (FHighlighted)
            {
                FAlpha = 50;
            }
            else
            {
                FAlpha = 0;
            }

            AoWCombatMap.FFleeLocations.Add(this);
        }

        // FleeLoc.pas:65
        public override void Deactivate()
        {
            AoWCombatMap.NewFrameEvents.Remove(NewFrame);
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
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // FleeLoc.pas:87
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // FleeLoc.pas:93
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TEObject OldHexagon = default;
            TMapField MapField = default;
            if (!GetEditMode())
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                OldHexagon = MapField.FindOwnedChild(132439);
                if (OldHexagon != null)
                {
                    OldHexagon.Free();
                    AoWHSSet.FCombatHexagons.Place(AoWCombatMap.MapContainer, Xhx, Yhx, 0, 1);
                }
            }

            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            return Result;
        }

        // FleeLoc.pas:132
        public void NewFrame(TObject Sender)
        {
            bool Highlight = default;
            Highlight = false;
            if (AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                Highlight = true;
            }
            else
            {
                var call__5 = AoWCombatMap.FUnitSelection.GetTCUnitHS();
                bool bool__6 = call__5 != null;
                if (bool__6)
                {
                    var call__7 = AoWCombatMap.FUnitSelection.GetTCUnitHS();
                    bool__6 = call__7.FUnit.FCombatPlayer == AoWCombatMap.FSeatedCombatPlayer;
                }

                bool bool__8 = bool__6;
                if (bool__8)
                {
                    var call__9 = AoWCombatMap.FUnitSelection.GetTCUnitHS();
                    bool__8 = call__9.FUnit.FCombatPlayer != AoWCombatMap.FCombat.FCombatData.FDefenderPlayer;
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
                if ((FAlpha > 0 && FAlphaStep < 0) || (FAlpha < 50 && FAlphaStep > 0))
                {
                    FAlpha = FAlpha + FAlphaStep;
                }
            }
        }

        // FleeLoc.pas:27


    }

    public partial class TFleeLocationResource : TMapObjectResource
    {
        public TFleeLocationResource(TECustomNode owner) : base(owner)
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

        // FleeLoc.pas:166
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Flee Location";
            return Result;
        }

    }

    public partial class TMovementHS : TSingleHS
    {
        public TMovementHS(TECustomNode owner) : base(owner)
        {}

        // FleeLoc.pas:172
        public new const int ClassID = 2228490;

        public bool FEnabled;
        // FleeLoc.pas:175
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 32;
            return Result;
        }

        // FleeLoc.pas:187
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
        }

        // FleeLoc.pas:178
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0 && !FEnabled)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
            }
        }

        // Engine.pas:524


    }

    public static partial class FleeLoc
    {
        // FleeLoc.pas:191
        public static void RegisterFleeLocation(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TFleeLocationResource>(), EClass.Of<TFleeLocation>(), EClass.Of<TMovementHS>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
