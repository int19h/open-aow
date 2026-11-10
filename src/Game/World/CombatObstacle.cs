// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

public partial class CombatObstacle : MultiHexMO
{
    public CombatObstacle(ECustomNode owner) : base(owner)
    { }

    // CombatObstacle.pas:94
    public new const int ClassID = 2229546;

    public CombatObstacleShadow FShadow;
    public CombatObstacleOverlay FOverlay;

    // CombatObstacle.pas:97
    public override void Dispose()
    {
        DestroyShadow();
        DestroyOverlay();
        base.Dispose();
    }

    // CombatObstacle.pas:110
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            var yhx = Yhx;
            var xhx = Xhx;
            UpdateMapFields((MapLevel)Owner.Owner, xhx, yhx);
        }

        return Result;
    }

    // CombatObstacle.pas:116
    public override void RemoveFromMap()
    {
        MapLevel MapLevel;
        MapLevel = (MapLevel)Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(MapLevel, FXhx, FYhx);
    }

    // CombatObstacle.pas:229
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
        {
            if (FResource.TerrainList.GetOverlay(Info.HN) == 8)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
            }
        }
    }

    // CombatObstacle.pas:208
    public override void Activate()
    {
        base.Activate();
        CreateShadow();
        CreateOverlay();
    }

    // CombatObstacle.pas:215
    public override void Deactivate()
    {
        DestroyShadow();
        DestroyOverlay();
        base.Deactivate();
    }

    // CombatObstacle.pas:222
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        base.UpdateMapField(Info);
        if (Info.HN < FResource.TerrainList.Count)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16384;
        }
    }

    // CombatObstacle.pas:193
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if (!FStatus.Contains(MapObjectStatusElement.Visible))
        {
            base.MakeVisible(VisibilityList);
            if (FShadow != null)
            {
                var showPriority = FShadow.GetShowPriority();
                VisibilityList.AddSortChild(FShadow, showPriority);
            }

            if (FOverlay != null)
            {
                var showPriority = FOverlay.GetShowPriority();
                VisibilityList.AddSortChild(FOverlay, showPriority);
            }
        }
    }

    // CombatObstacle.pas:107
    public override MapObjectResourceList GetResourceList(GameEngine AEngine) => AoWHSSet.Current.FCombatResources;

    // CombatObstacle.pas:104
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

    // CombatObstacle.pas:203
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);

    // CombatObstacle.pas:125
    public void CreateShadow()
    {
        if (FResource.ImageSequenceList.GetImageSequence(40) != null)
        {
            if (FShadow != null)
            {
                DestroyShadow();
            }

            if (AoWHSMap.Current == null || AoWHSMap.Current.FShowUnitShadows)
            {
                FShadow = new CombatObstacleShadow(null);
                FShadow.FObstacle = this;
            }
        }
    }

    // CombatObstacle.pas:138
    public void DestroyShadow()
    {
        if (FShadow != null)
        {
            if (GetVisible())
            {
                AoWCombatMap.Current.DynamicVisibleHS.RemoveChild(FShadow);
            }

            FShadow?.Dispose();
            FShadow = null;
        }
    }

    // CombatObstacle.pas:148
    public void CreateOverlay()
    {
        if (FResource.ImageSequenceList.GetImageSequence(41) != null)
        {
            if (FOverlay != null)
            {
                DestroyOverlay();
            }

            if (AoWHSMap.Current == null || AoWHSMap.Current.FShowUnitShadows)
            {
                FOverlay = new CombatObstacleOverlay(null);
                FOverlay.FObstacle = this;
            }
        }
    }

    // CombatObstacle.pas:161
    public void DestroyOverlay()
    {
        if (FOverlay != null)
        {
            if (GetVisible())
            {
                AoWCombatMap.Current.DynamicVisibleHS.RemoveChild(FOverlay);
            }

            FOverlay?.Dispose();
            FOverlay = null;
        }
    }

    // CombatObstacle.pas:171
    public void ShowShadow(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
        XhpSurf = X - XhpSurf;
        YhpSurf = Y - YhpSurf;
        FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((MapField)Owner).Xhx + (((MapField)Owner).Yhx << 4)));
    }

    // CombatObstacle.pas:182
    public void ShowOverlay(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
        XhpSurf = X - XhpSurf;
        YhpSurf = Y - YhpSurf;
        FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 41, (uint)(((MapField)Owner).Xhx + (((MapField)Owner).Yhx << 4)));
    }

    public new partial class Resource : MultiHexMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CombatObstacle.pas:239
        public new const int ClassID = 2229547;

        // CombatObstacle.pas:245
        public override int ResourceUserClassID() => 2229546;

        // CombatObstacle.pas:242
        public override string ResourceEditName() => "Combat " + AbilityRules.TranslateRStr(GameText.ObstacleRStr);

    }

}
