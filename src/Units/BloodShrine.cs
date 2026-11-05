// Translated from BloodShrine.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.Sound.Sound;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.BloodShrine
{
    public partial class TBloodShrineAnimation : TAnimation
    {
        public TBloodShrineAnimation(TECustomNode owner) : base(owner)
        {}

        // BloodShrine.pas:56
        public override void NewFrame()
        {
            base.NewFrame();
            if (FFrameCounter > 20)
            {
                Destroy();
            }
        }

        // Animation.pas:43


        // BloodShrine.pas:53
        public override TSet<TAnimationControlModeElement> AnimationControlMode() => TSet<TAnimationControlModeElement>.Of(TAnimationControlModeElement.amLinkToNewFrame, TAnimationControlModeElement.amOwnedByMap);

    }

    public partial class TBloodShrine : TStructure
    {
        public TBloodShrine(TECustomNode owner) : base(owner)
        {
        }

        // BloodShrine.pas:65
        public new const int ClassID = 131376;

        // BloodShrine.pas:62


        // BloodShrine.pas:103
        public override void ArmyPlaced(TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            ExecuteBloodlust(A1);
        }

        // BloodShrine.pas:68
        public void ShowBlessingAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
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
                AoWHSSet.FAbilityControl.GetAbility(168).Expand(AUnit);
            }

            bool condition = ArmyHS.FArmy.FPlayer == AoWHSMap.FSeatedPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = AoWHSMap.Watching(xhx, yhx, lhx, 0);
            }

            if (condition)
            {
                PlaySample((AnsiString)"SFX\\ATTACK\\ENCHP02.WAV");
            }

            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            var xhx2 = GetXhx();
            if (AoWHSMap.Visible(xhx2, yhx2, lhx2, 0))
            {
                Anim = new TBloodShrineAnimation(null);
                R = Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                AoWHSMap.PlaceHX(Anim, xhx, yhx, lhx);
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
        public override AnsiString ResourceEditName() => TranslateRStr(BloodShrineRStr);

    }

    public static partial class BloodShrine
    {
        // BloodShrine.pas:125
        public static void RegisterBloodShrine(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TBloodShrine>(), EClass.Of<TBloodShrineResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
