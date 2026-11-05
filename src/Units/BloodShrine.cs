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
        public override TSet<TAnimationControlModeElement> AnimationControlMode()
        {
            TSet<TAnimationControlModeElement> Result = default;
            Result = TSet<TAnimationControlModeElement>.Of(TAnimationControlModeElement.amLinkToNewFrame, TAnimationControlModeElement.amOwnedByMap);
            return Result;
        }

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
            int I = default;
            TBloodShrineAnimation Anim = default;
            {
                I = 0;
                int end__1 = ArmyHS.FArmy.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = ArmyHS.FArmy.GetUnit(I);
                    AUnit = call__2;
                    var call__3 = AoWHSSet.FAbilityControl.GetAbility(168);
                    call__3.Expand(AUnit);
                }
            }

            bool bool__4 = ArmyHS.FArmy.FPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__4)
            {
                var call__5 = GetLhx();
                var call__6 = GetYhx();
                var call__7 = GetXhx();
                var call__8 = AoWHSMap.Watching(call__7, call__6, call__5, 0);
                bool__4 = call__8;
            }

            if (bool__4)
            {
                PlaySample((AnsiString)"SFX\\ATTACK\\ENCHP02.WAV");
            }

            var call__9 = GetLhx();
            var call__10 = GetYhx();
            var call__11 = GetXhx();
            var call__12 = AoWHSMap.Visible(call__11, call__10, call__9, 0);
            if (call__12)
            {
                Anim = new TBloodShrineAnimation(null);
                R = Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var call__13 = GetLhx();
                var call__14 = GetYhx();
                var call__15 = GetXhx();
                AoWHSMap.PlaceHX(Anim, call__15, call__14, call__13);
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131376;
            return Result;
        }

        // BloodShrine.pas:36


        // BloodShrine.pas:119
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__16 = BloodShrineRStr;
            var call__17 = TranslateRStr(call__16);
            Result = call__17;
            return Result;
        }

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
