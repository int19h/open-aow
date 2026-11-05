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
        public override int GetShowPriority() => 1744830464 + (Yhx << 13) + Xhx;

        // FleeLoc.pas:151
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            if (FAlpha > 0)
            {
                HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
                FResource.ImageLibrary.Get(0).ShowBlended(X - XhpSurf - 8, Y - YhpSurf, SurfaceDesc, AlphaBlendTable.LookupTables[FAlpha]);
            }
        }

        // FleeLoc.pas:120
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == TCombatSideIndex.csAttacker)
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
        public override bool CanSelect(int Xhp, int Yhp) => true;

        // FleeLoc.pas:79
        public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

        // FleeLoc.pas:87
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

        // FleeLoc.pas:93
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            TEObject OldHexagon;
            TMapField MapField;
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

            return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        }

        // FleeLoc.pas:132
        public void NewFrame(TObject Sender)
        {
            bool Highlight;
            Highlight = false;
            if (AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                Highlight = true;
            }
            else
            {
                if (AoWCombatMap.FUnitSelection.GetTCUnitHS() != null && AoWCombatMap.FUnitSelection.GetTCUnitHS().FUnit.FCombatPlayer == AoWCombatMap.FSeatedCombatPlayer && AoWCombatMap.FUnitSelection.GetTCUnitHS().FUnit.FCombatPlayer != AoWCombatMap.FCombat.FCombatData.FDefenderPlayer)
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
        public override int ResourceUserClassID() => 2229544;

        // FleeLoc.pas:166
        public override AnsiString ResourceEditName() => (AnsiString)"Combat Flee Location";

    }

    public partial class TMovementHS : TSingleHS
    {
        public TMovementHS(TECustomNode owner) : base(owner)
        {}

        // FleeLoc.pas:172
        public new const int ClassID = 2228490;

        public bool FEnabled;
        // FleeLoc.pas:175
        public override int GetShowPriority() => 32;

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
