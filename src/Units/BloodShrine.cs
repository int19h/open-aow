// Translated from BloodShrine.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animation
{

    public partial class TBloodShrineAnimation : TAnimation
    {
        public TBloodShrineAnimation(TECustomNode owner) : base(owner)
        { }

        // BloodShrine.pas:56
        public override void NewFrame()
        {
            base.NewFrame();
            if (FFrameCounter > 20)
            {
                Dispose();
            }
        }

        // Animation.pas:43

        // BloodShrine.pas:53
        public override TSet<TAnimationControlModeElement> AnimationControlMode() => TSet<TAnimationControlModeElement>.Of(TAnimationControlModeElement.amLinkToNewFrame, TAnimationControlModeElement.amOwnedByMap);

    }
}

namespace Aow.Game.World.Structures
{

    public partial class TBloodShrine : TStructure
    {
        public TBloodShrine(TECustomNode owner) : base(owner)
        {
        }

        // BloodShrine.pas:65
        public new const int ClassID = 131376;

        // BloodShrine.pas:62

        // BloodShrine.pas:103
        public override void ArmyPlaced(TArmyHS PlacedArmy)
        {
            base.ArmyPlaced(PlacedArmy);
            ExecuteBloodlust(PlacedArmy);
        }

        // BloodShrine.pas:68
        public void ShowBlessingAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
        }

        // BloodShrine.pas:77
        public void ExecuteBloodlust(TArmyHS ArmyHS)
        {
            TAbstractUnit AUnit = default;
            TRect R = default;
            TBloodShrineAnimation Anim = default;
            for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
            {
                AUnit = ArmyHS.FArmy.GetUnit(I);
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(168).Expand(AUnit);
            }

            bool condition = ArmyHS.FArmy.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(xhx, yhx, lhx, 0);
            }

            if (condition)
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
            }

            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            var xhx2 = GetXhx();
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Visible(xhx2, yhx2, lhx2, 0))
            {
                Anim = new TBloodShrineAnimation(null);
                R = Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, xhx, yhx, lhx);
            }
        }

        // BloodShrine.pas:26

    }

    public partial class TBloodShrineResource : TStructureResource
    {
        public TBloodShrineResource(TECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TBloodShrineResource.BloodShrineRStr);

        private static readonly string BloodShrineRStr = "Blood Shrine";

    }
}
