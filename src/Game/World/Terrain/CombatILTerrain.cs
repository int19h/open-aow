// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class CombatILTerrain : ILTerrainMO
{
    public CombatILTerrain(ECustomNode owner) : base(owner)
    { }

    // CombatILTer.pas:105
    public new const int ClassID = 2228534;

    public CombatILTerrainShadow FShadow;

    // CombatILTer.pas:99
    public override void Dispose()
    {
        DestroyShadow();
        base.Dispose();
    }

    // CombatILTer.pas:166
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 135169)
        {
            if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
            {
                if (Msg.Param[0].Int < ((CombatILTerrainResource)FResource).FDefense)
                {
                    Msg.Param[0] = ((CombatILTerrainResource)FResource).FDefense;
                }
            }
        }
    }

    // CombatILTer.pas:151
    public override void Activate()
    {
        base.Activate();
        CreateShadow();
    }

    // CombatILTer.pas:157
    public override void Deactivate()
    {
        DestroyShadow();
        base.Deactivate();
    }

    // CombatILTer.pas:141
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
        }
    }

    // CombatILTer.pas:163
    public override MapObjectResourceList GetResourceList(GameEngine AEngine) => AoWHSSet.Current.FCombatResources;

    // CombatILTer.pas:93
    public override bool CanPlaceOnHS(HexagonSprite HS) => base.CanPlaceOnHS(HS) || HS is AbstractRoad;

    // CombatILTer.pas:108
    public virtual void ShowShadow(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((MapField)Owner).Xhx + (((MapField)Owner).Yhx << 4)));
    }

    // CombatILTer.pas:87
    public sbyte GetDefense() => ((CombatILTerrainResource)FResource).FDefense;

    // CombatILTer.pas:90
    public sbyte GetBurnValue() => ((CombatILTerrainResource)FResource).FBurnValue;

    // CombatILTer.pas:118
    public void DestroyShadow()
    {
        if (FShadow != null)
        {
            if (GetVisible())
            {
                FResource.Engine.HSMap.DynamicVisibleHS.RemoveChild(FShadow);
            }

            FShadow?.Dispose();
            FShadow = null;
        }
    }

    // CombatILTer.pas:128
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
                FShadow = new CombatILTerrainShadow(null);
                FShadow.FTerrain = this;
            }
        }
    }

}
