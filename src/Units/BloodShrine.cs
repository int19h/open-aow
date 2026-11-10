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

    public partial class BloodShrineAnimation : global::Aow.Game.Draw.Animation.Animation
    {
        public BloodShrineAnimation(ECustomNode owner) : base(owner)
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
        public override Set<AnimationControlModeElement> AnimationControlMode() => Set<AnimationControlModeElement>.Of(AnimationControlModeElement.LinkToNewFrame, AnimationControlModeElement.OwnedByMap);

    }
}

namespace Aow.Game.World.Structures
{

    public partial class BloodShrine : Structure
    {
        public BloodShrine(ECustomNode owner) : base(owner)
        {
        }

        // BloodShrine.pas:65
        public new const int ClassID = 131376;

        // BloodShrine.pas:62

        // BloodShrine.pas:103
        public override void ArmyPlaced(ArmyHS PlacedArmy)
        {
            base.ArmyPlaced(PlacedArmy);
            ExecuteBloodlust(PlacedArmy);
        }

        // BloodShrine.pas:68
        public void ShowBlessingAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
        }

        // BloodShrine.pas:77
        public void ExecuteBloodlust(ArmyHS ArmyHS)
        {
            AbstractUnit AUnit = default;
            global::Aow.Rtl.Rect R = default;
            BloodShrineAnimation Anim = default;
            for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
            {
                AUnit = ArmyHS.FArmy.GetUnit(I);
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(168).Expand(AUnit);
            }

            bool condition = ArmyHS.FArmy.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = Aow.Game.World.Map.AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
            }

            if (condition)
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
            }

            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            var xhx2 = GetXhx();
            if (Aow.Game.World.Map.AoWHSMap.Current.Visible(xhx2, yhx2, lhx2, 0))
            {
                Anim = new BloodShrineAnimation(null);
                R = global::Aow.Rtl.RtlUnit.Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Anim, xhx, yhx, lhx);
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
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.BloodShrine.Resource.BloodShrineRStr);

            private static readonly string BloodShrineRStr = "Blood Shrine";

        }

        // BloodShrine.pas:26

    }
}
