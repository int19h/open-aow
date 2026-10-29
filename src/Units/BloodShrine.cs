// Translated from BloodShrine.pas.
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

namespace Aow.Units.BloodShrine
{
    // { TBloodShrineAnimation: size $3C, VMT 557E26D0; no additional fields. }
    public partial class TBloodShrineAnimation : Aow.Units.Animation.TAnimation
    {
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
        public static new Aow.Units.BloodShrine.TBloodShrineAnimation Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BloodShrine.TBloodShrineAnimation o = new Aow.Units.BloodShrine.TBloodShrineAnimation();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Animation.TAnimation.Meta
        {
            // BloodShrine.pas:53
            public override TSet<Aow.Units.Animation.TAnimationControlModeElement> AnimationControlMode()
            {
                TSet<Aow.Units.Animation.TAnimationControlModeElement> Result = default;
                Result = TSet<Aow.Units.Animation.TAnimationControlModeElement>.Of(Aow.Units.Animation.TAnimationControlModeElement.amLinkToNewFrame, Aow.Units.Animation.TAnimationControlModeElement.amOwnedByMap);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.BloodShrine.TBloodShrineAnimation => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.BloodShrine.TBloodShrineAnimation Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.BloodShrine.TBloodShrineAnimation)base.Create(AOwner);
        }
    }

    // { TBloodShrine: size $30, VMT 557E2880; no additional fields. }
    public partial class TBloodShrine : Aow.Units.AoWE.TStructure
    {
        // BloodShrine.pas:65
        public new const int ClassID = 131376;

        // BloodShrine.pas:62
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // BloodShrine.pas:103
        public override void ArmyPlaced(Aow.Units.AoWE.TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            ExecuteBloodlust(A1);
        }

        // BloodShrine.pas:68
        public void ShowBlessingAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
        }

        // BloodShrine.pas:77
        public void ExecuteBloodlust(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Aow.Rtl.TRect R = default;
            int I = default;
            Aow.Units.BloodShrine.TBloodShrineAnimation Anim = default;
            {
                I = 0;
                int end__1 = ArmyHS.FArmy.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = ArmyHS.FArmy.GetUnit(I);
                    AUnit = call__2;
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(168);
                    call__3.Expand(AUnit);
                }
            }

            bool bool__4 = (ArmyHS.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__4)
            {
                var call__5 = GetLhx();
                var call__6 = GetYhx();
                var call__7 = GetXhx();
                var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__7), unchecked((sbyte)call__6), unchecked((sbyte)call__5), 0);
                bool__4 = call__8;
            }

            if (bool__4)
            {
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\ATTACK\\ENCHP02.WAV");
            }

            var call__9 = GetLhx();
            var call__10 = GetYhx();
            var call__11 = GetXhx();
            var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)call__11), unchecked((sbyte)call__10), unchecked((sbyte)call__9), 0);
            if (call__12)
            {
                Anim = Aow.Units.BloodShrine.TBloodShrineAnimation.Create(null);
                R = Aow.Rtl.Rtl.Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var call__13 = GetLhx();
                var call__14 = GetYhx();
                var call__15 = GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, call__15, call__14, call__13);
            }
        }

        // BloodShrine.pas:26
        public static new Aow.Units.BloodShrine.TBloodShrine Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BloodShrine.TBloodShrine o = new Aow.Units.BloodShrine.TBloodShrine();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructure.Meta
        {
            // BloodShrine.pas:65
            public override int ClassID() => Aow.Units.BloodShrine.TBloodShrine.ClassID;

            // BloodShrine.pas:26
            public new Aow.Units.BloodShrine.TBloodShrine Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.BloodShrine.TBloodShrine)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.BloodShrine.TBloodShrine => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TBloodShrineResource: size $64, VMT 557E2AEC; no additional fields. }
    public partial class TBloodShrineResource : Aow.Units.AoWE.TStructureResource
    {
        // BloodShrine.pas:116
        public new const int ClassID = 131377;

        // BloodShrine.pas:109
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainBound = true;
            FConnectRoads = false;
        }

        // BloodShrine.pas:122
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131376;
            return Result;
        }

        // BloodShrine.pas:36
        public static new Aow.Units.BloodShrine.TBloodShrineResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.BloodShrine.TBloodShrineResource o = new Aow.Units.BloodShrine.TBloodShrineResource();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructureResource.Meta
        {
            // BloodShrine.pas:116
            public override int ClassID() => Aow.Units.BloodShrine.TBloodShrineResource.ClassID;

            // BloodShrine.pas:119
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__16 = Aow.Units.AoWE.AoWE.BloodShrineRStr;
                var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(call__16);
                Result = call__17;
                return Result;
            }

            // BloodShrine.pas:36
            public new Aow.Units.BloodShrine.TBloodShrineResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.BloodShrine.TBloodShrineResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.BloodShrine.TBloodShrineResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class BloodShrine
    {
        // BloodShrine.pas:125
        public static void RegisterBloodShrine(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.BloodShrine.TBloodShrine>(), Aow.Units.Engine.EClass.Of<Aow.Units.BloodShrine.TBloodShrineResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
