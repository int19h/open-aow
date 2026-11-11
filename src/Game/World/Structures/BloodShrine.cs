// Translated from BloodShrine.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class BloodShrine : Structure
{
    public BloodShrine(ECustomNode owner) : base(owner)
    {
    }

    // BloodShrine.pas:65
    public new const int ClassID = 131376;

    // BloodShrine.pas:62

    // BloodShrine.pas:103
    public override void ArmyPlaced(ArmyHexSprite PlacedArmy)
    {
        base.ArmyPlaced(PlacedArmy);
        ExecuteBloodlust(PlacedArmy);
    }

    // BloodShrine.pas:68
    public void ShowBlessingAnimation(Animation Anim, int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        Xhp -= XhpSurf;
        Yhp -= YhpSurf;
        FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
    }

    // BloodShrine.pas:77
    public void ExecuteBloodlust(ArmyHexSprite ArmyHS)
    {
        Troop AUnit = default;
        Rect R = default;
        BloodShrineAnimation Anim = default;
        for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
        {
            AUnit = ArmyHS.FArmy.GetUnit(I);
            AoWHSSet.Current.FAbilityControl.GetAbility(168).Expand(AUnit);
        }

        bool condition = ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer;
        if (!condition)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            condition = AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
        }

        if (condition)
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
        }

        var lhx2 = GetLhx();
        var yhx2 = GetYhx();
        var xhx2 = GetXhx();
        if (AoWHSMap.Current.Visible(xhx2, yhx2, lhx2, 0))
        {
            Anim = new BloodShrineAnimation(null);
            R = new Rect(-16, -8, 32, 40);
            Anim.SetDimensions(R);
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowBlessingAnimation;
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            AoWHSMap.Current.PlaceHX(Anim, xhx, yhx, lhx);
        }
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FTerrainBound = true;
            FConnectRoads = false;
        }

        // BloodShrine.pas:116
        public new const int ClassID = 131377;

        // BloodShrine.pas:109

        // BloodShrine.pas:122
        public override int ResourceUserClassID() => 131376;

        // BloodShrine.pas:36

        // BloodShrine.pas:119
        public override string ResourceEditName() => AbilityRules.TranslateRStr(BloodShrineRStr);

        private static readonly string BloodShrineRStr = "Blood Shrine";

    }

    // BloodShrine.pas:26

}
