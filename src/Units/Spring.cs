// Translated from Spring.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Spring
{
    public partial class TSpringOfLifeAnimation : TAnimation
    {
        public TSpringOfLifeAnimation(TECustomNode owner) : base(owner)
        {}

        // Spring.pas:64
        public override void NewFrame()
        {
            base.NewFrame();
            if (FFrameCounter > 20)
            {
                Destroy();
            }
        }

        // Animation.pas:43


        // Spring.pas:59
        public override TSet<TAnimationControlModeElement> AnimationControlMode() => Spring.SpringMode;

    }

    public partial class TSpringOfLife : TStructure
    {
        public TSpringOfLife(TECustomNode owner) : base(owner)
        {
        }

        // Spring.pas:73
        public new const int ClassID = 131328;

        // Spring.pas:70


        // Spring.pas:111
        public override void ArmyPlaced(TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            if (!TSet<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)((TMapField)Owner).TerrainType))
            {
                ExecuteNaturesBlessing(A1);
            }
        }

        // Spring.pas:76
        public void ShowBlessingAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
        }

        // Spring.pas:86
        public void ExecuteNaturesBlessing(TArmyHS ArmyHS)
        {
            TSpringOfLifeAnimation Anim = default;
            TRect R = default;
            int I = default;
            object Spill = default;
            // { Keep the parameter in its original stack slot. }
            ;
            {
                I = 0;
                int end__1 = ArmyHS.FArmy.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = ArmyHS.FArmy.GetUnit(I);
                    call__2.ExpandAbility(99);
                }
            }

            bool bool__3 = ArmyHS.FArmy.FPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__3)
            {
                var call__4 = GetLhx();
                var call__5 = GetYhx();
                var call__6 = GetXhx();
                var call__7 = AoWHSMap.Watching(call__6, call__5, call__4, 0);
                bool__3 = call__7;
            }

            if (bool__3)
            {
                PlaySample((AnsiString)"SFX\\ATTACK\\ENCHP02.WAV");
            }

            var call__8 = GetLhx();
            var call__9 = GetYhx();
            var call__10 = GetXhx();
            var call__11 = AoWHSMap.Visible(call__10, call__9, call__8, 0);
            if (call__11)
            {
                Anim = new TSpringOfLifeAnimation(null);
                R = Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var call__12 = GetLhx();
                var call__13 = GetYhx();
                var call__14 = GetXhx();
                AoWHSMap.PlaceHX(Anim, call__14, call__13, call__12);
            }
        }

        // Spring.pas:27


    }

    public partial class TSpringOfLifeResource : TStructureResource
    {
        public TSpringOfLifeResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
        }

        // Spring.pas:123
        public new const int ClassID = 131329;

        // Spring.pas:117


        // Spring.pas:129
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131328;
            return Result;
        }

        // Spring.pas:37


        // Spring.pas:126
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__15 = SpringOfLifeRStr;
            var call__16 = TranslateRStr(call__15);
            Result = call__16;
            return Result;
        }

    }

    public struct TSpringAnimationCallback
    {
        public FixedArray_byte_36 Reserved;
        public TShowAnimationEvent OnShow;
    }

    public static partial class Spring
    {
        // Spring.pas:132
        public static void RegisterSpringOfLife(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TSpringOfLife>(), EClass.Of<TSpringOfLifeResource>()]);
        }

        public static TSet<TAnimationControlModeElement> SpringMode = TSet<TAnimationControlModeElement>.FromMask(17UL);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
