// Translated from Spring.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Spring
{
    // { TSpringOfLifeAnimation (instance size 60, VMT 557A3858) }
    public partial class TSpringOfLifeAnimation : Aow.Units.Animation.TAnimation
    {
        public TSpringOfLifeAnimation(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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
        // { needs: AnimationControlMode returns TAnimationControlMode in AL. }
        public override TSet<Aow.Units.Animation.TAnimationControlModeElement> AnimationControlMode() => Aow.Units.Spring.Spring.SpringMode;

    }

    // { TSpringOfLife (instance size 48, VMT 557A3A04) }
    public partial class TSpringOfLife : Aow.Units.AoWE.TStructure
    {
        public TSpringOfLife(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // Spring.pas:73
        public new const int ClassID = 131328;

        // Spring.pas:70


        // Spring.pas:111
        public override void ArmyPlaced(Aow.Units.AoWE.TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            if (!TSet<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)((Aow.Units.HSEngine.TMapField)Owner).TerrainType))
            {
                ExecuteNaturesBlessing(A1);
            }
        }

        // Spring.pas:76
        public void ShowBlessingAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
        }

        // Spring.pas:86
        public void ExecuteNaturesBlessing(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            Aow.Units.Spring.TSpringOfLifeAnimation Anim = default;
            Aow.Rtl.TRect R = default;
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

            bool bool__3 = (ArmyHS.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__3)
            {
                var call__4 = GetLhx();
                var call__5 = GetYhx();
                var call__6 = GetXhx();
                var call__7 = ((Aow.Units.AoWE.TAoWHSMap)Aow.Units.AoWE.AoWE.AoWHSMap).Watching(unchecked((sbyte)call__6), unchecked((sbyte)call__5), unchecked((sbyte)call__4), 0);
                bool__3 = call__7;
            }

            if (bool__3)
            {
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\ATTACK\\ENCHP02.WAV");
            }

            var call__8 = GetLhx();
            var call__9 = GetYhx();
            var call__10 = GetXhx();
            var call__11 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)call__10), unchecked((sbyte)call__9), unchecked((sbyte)call__8), 0);
            if (call__11)
            {
                Anim = new Aow.Units.Spring.TSpringOfLifeAnimation(null);
                R = Aow.Rtl.Rtl.Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var call__12 = GetLhx();
                var call__13 = GetYhx();
                var call__14 = GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, call__14, call__13, call__12);
            }
        }

        // Spring.pas:27


    }

    // { TSpringOfLifeResource (instance size 100, VMT 557A3C6C) }
    public partial class TSpringOfLifeResource : Aow.Units.AoWE.TStructureResource
    {
        public TSpringOfLifeResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__15 = Aow.Units.AoWE.AoWE.SpringOfLifeRStr;
            var call__16 = Aow.Units.AoWE.AoWE.TranslateRStr(call__15);
            Result = call__16;
            return Result;
        }

    }

    public struct TSpringAnimationCallback
    {
        public Aow.Units.FixedArray_byte_36 Reserved;
        public Aow.Units.Animation.TShowAnimationEvent OnShow;
    }

    public static partial class Spring
    {
        // Spring.pas:132
        public static void RegisterSpringOfLife(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Spring.TSpringOfLife>(), Aow.Units.Engine.EClass.Of<Aow.Units.Spring.TSpringOfLifeResource>()]);
        }

        public static TSet<Aow.Units.Animation.TAnimationControlModeElement> SpringMode = TSet<Aow.Units.Animation.TAnimationControlModeElement>.FromMask(17UL);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
