// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

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
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int X = default;
        int Y = default;
        if (FAlpha > 0)
        {
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
            FResource.ImageLibrary.Get(0).ShowBlended(X - XhpSurf - 8, Y - YhpSurf, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[FAlpha]);
        }
    }

    // FleeLoc.pas:120
    public override void CanMoveOn(in HexSpriteMoveOnInfo Info, ref HexSpriteMoveOnResult MoveResult)
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
        CombatMap.Current.NewFrameEvents += NewFrame;
        FHighlighted = CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting);
        if (FHighlighted)
        {
            FAlpha = 50;
        }
        else
        {
            FAlpha = 0;
        }

        CombatMap.Current.FFleeLocations.Add(this);
    }

    // FleeLoc.pas:65
    public override void Deactivate()
    {
        CombatMap.Current.NewFrameEvents -= NewFrame;
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
        EngineObject OldHexagon;
        MapField MapField;
        if (!GetEditMode())
        {
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldHexagon = MapField.FindOwnedChild(132439);
            if (OldHexagon != null)
            {
                OldHexagon?.Dispose();
                AoWHSSet.Current.FCombatHexagons.Place(CombatMap.Current.MapContainer, Xhx, Yhx, 0, 1);
            }
        }

        return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
    }

    // FleeLoc.pas:132
    public void NewFrame(object Sender)
    {
        bool Highlight;
        Highlight = false;
        if (CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Highlight = true;
        }
        else
        {
            if (CombatMap.Current.FUnitSelection.GetTCUnitHS() != null && CombatMap.Current.FUnitSelection.GetTCUnitHS().FUnit.FCombatPlayer == CombatMap.Current.FSeatedCombatPlayer && CombatMap.Current.FUnitSelection.GetTCUnitHS().FUnit.FCombatPlayer != CombatMap.Current.FCombat.FCombatData.FDefenderPlayer)
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
