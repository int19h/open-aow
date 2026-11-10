// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World
{

    public partial class FleeLocation : MapObject
    {
        public FleeLocation(ECustomNode owner) : base(owner)
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
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            if (FAlpha > 0)
            {
                HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
                FResource.ImageLibrary.Get(0).ShowBlended(X - XhpSurf - 8, Y - YhpSurf, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[FAlpha]);
            }
        }

        // FleeLoc.pas:120
        public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
        {
            if (Info.HN == 0)
            {
                if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == CombatSideIndex.Attacker)
                {
                    MoveResult.Code = 25;
                }
                else
                {
                    MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
                }

                MoveResult.Done = true;
            }
        }

        // FleeLoc.pas:111
        public override void Activate()
        {
            base.Activate();
            AoWCombatMap.Current.NewFrameEvents += NewFrame;
            FHighlighted = AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting);
            if (FHighlighted)
            {
                FAlpha = 50;
            }
            else
            {
                FAlpha = 0;
            }

            AoWCombatMap.Current.FFleeLocations.Add(this);
        }

        // FleeLoc.pas:65
        public override void Deactivate()
        {
            AoWCombatMap.Current.NewFrameEvents -= NewFrame;
            base.Deactivate();
        }

        // FleeLoc.pas:90
        public override bool CanSelect(int Xhp, int Yhp) => true;

        // FleeLoc.pas:79
        public override MapObjectResourceList GetResourceList(GameEngine AEngine) => AoWHSSet.Current.FCombatResources;

        // FleeLoc.pas:87
        public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

        // FleeLoc.pas:93
        public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            EObject OldHexagon;
            MapField MapField;
            if (!GetEditMode())
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                OldHexagon = MapField.FindOwnedChild(132439);
                if (OldHexagon != null)
                {
                    OldHexagon?.Dispose();
                    AoWHSSet.Current.FCombatHexagons.Place(AoWCombatMap.Current.MapContainer, Xhx, Yhx, 0, 1);
                }
            }

            return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        }

        // FleeLoc.pas:132
        public void NewFrame(object Sender)
        {
            bool Highlight;
            Highlight = false;
            if (AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                Highlight = true;
            }
            else
            {
                if (AoWCombatMap.Current.FUnitSelection.GetTCUnitHS() != null && AoWCombatMap.Current.FUnitSelection.GetTCUnitHS().FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer && AoWCombatMap.Current.FUnitSelection.GetTCUnitHS().FUnit.FCombatPlayer != AoWCombatMap.Current.FCombat.FCombatData.FDefenderPlayer)
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

        public new partial class Resource : MapObjectResource
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // FleeLoc.pas:163
            public new const int ClassID = 2229545;

            // FleeLoc.pas:169
            public override int ResourceUserClassID() => 2229544;

            // FleeLoc.pas:166
            public override string ResourceEditName() => "Combat Flee Location";

        }

        // FleeLoc.pas:27

    }
}

namespace Aow.Game.Draw
{

    public partial class MovementHS : SingleHS
    {
        public MovementHS(ECustomNode owner) : base(owner)
        { }

        // FleeLoc.pas:172
        public new const int ClassID = 2228490;

        public bool FEnabled;

        // FleeLoc.pas:175
        public override int GetShowPriority() => 32;

        // FleeLoc.pas:187
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
        }

        // FleeLoc.pas:178
        public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
        {
            if (Info.HN == 0 && !FEnabled)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
            }
        }

        // Engine.pas:524

    }
}
