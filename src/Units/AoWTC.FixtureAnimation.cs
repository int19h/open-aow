#nullable disable
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.FleeLoc;
using static Aow.Units.FleeLoc.FleeLoc;
using Aow.Units.CombatHex;
using static Aow.Units.CombatHex.CombatHex;
using Aow.Units.CombatTE;
using static Aow.Units.CombatTE.CombatTE;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
using Aow.Units.CombatTerrain;
using static Aow.Units.CombatTerrain.CombatTerrain;
using Aow.Units.HxBridge;
using static Aow.Units.HxBridge.HxBridge;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.CombatRoad;
using static Aow.Units.CombatRoad.CombatRoad;
using Aow.Units.CombatObstacle;
using static Aow.Units.CombatObstacle.CombatObstacle;
using Aow.Units.CityWall;
using static Aow.Units.CityWall.CityWall;
using Aow.Units.CombatFire;
using static Aow.Units.CombatFire.CombatFire;

namespace Aow.Units.AoWTC
{
    public partial class TfxHS
    {
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.GFXE.TSurfaceDesc SurfaceDesc__value = SurfaceDesc;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Index = default;
            Aow.Units.ImageLib.TImageSequence FirstSequence = default;
            int I = default;
            int J = default;
            void SelectSequence(int Number)
            {
                if (FAbilityIndex == 52)
                {
                    var call__825 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                    var call__826 = call__825.FCastImages.ImageSequences[Number];
                    Sequence = call__826;
                }
                else
                {
                    var call__827 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                    var call__828 = call__827.FImageSequences.ImageSequences[Number];
                    Sequence = call__828;
                }
            }

            void SelectFrame()
            {
                if (Sequence != null)
                {
                    var call__829 = Sequence.GetUnloopedFrameIndex(FFrame);
                    Index = call__829;
                }

                FirstSequence = Sequence;
            }

            void SelectDamageSequence()
            {
                int N = default;
                switch (FEffectType)
                {
                    case Aow.Units.AoWTC.TfxType.fxDamLightning:
                        N = 40;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamFire:
                        N = 41;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamPoison:
                        N = 42;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamMagic:
                        N = 43;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamHoly:
                        N = 44;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamDeath:
                        N = 45;
                        break;
                    default:
                        N = 46;
                        break;
                }

                var call__830 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[N];
                Sequence = call__830;
            }

            void ShowMovingLoop()
            {
                while (Sequence != null)
                {
                    var call__831 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                    Index = call__831;
                    bool bool__832 = (Index != -1);
                    if (bool__832)
                    {
                        var call__833 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__832 = call__833;
                    }

                    if (bool__832)
                    {
                        var call__834 = FPath[FFrame];
                        var call__835 = FPath[FFrame];
                        var call__836 = FPath[FFrame];
                        var call__837 = Sequence.ImageLibrary[Index];
                        call__837.ShowClipped((call__836.X - XhpSurf) + Sequence.dXshift, ((call__834.Y - call__835.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowSecondaryFrame()
            {
                while (Sequence != null)
                {
                    var call__838 = Sequence.GetUnloopedFrameIndex(FVariant);
                    Index = call__838;
                    bool bool__839 = (Index != -1);
                    if (bool__839)
                    {
                        var call__840 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__839 = call__840;
                    }

                    if (bool__839)
                    {
                        var call__841 = FPath[FFrame];
                        var call__842 = FPath[FFrame];
                        var call__843 = FPath[FFrame];
                        var call__844 = Sequence.ImageLibrary[Index];
                        call__844.ShowClipped((call__843.X - XhpSurf) + Sequence.dXshift, ((call__841.Y - call__842.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowTrail()
            {
                int N = default;
                int LastFrame = default;
                int Half = default;
                if (Sequence != null)
                {
                    LastFrame = Sequence.GetTotalSequenceFrames() - 1;
                    {
                        N = 0;
                        int end__845 = LastFrame;
                        for (; N <= end__845; N += 1)
                        {
                            Half = N / 2;
                            bool bool__846 = ((((FFrame - 9) + Half) + 5) > -1);
                            if (bool__846)
                            {
                                var call__847 = FPath.Count;
                                bool__846 = ((((FFrame - 9) + Half) + 5) < call__847);
                            }

                            if (bool__846)
                            {
                                Index = Sequence.GetUnloopedFrameIndex(LastFrame - N);
                                bool bool__848 = (Index != -1);
                                if (bool__848)
                                {
                                    var call__849 = Sequence.ImageLibrary.ImageExist(Index);
                                    bool__848 = call__849;
                                }

                                if (bool__848)
                                {
                                    var call__850 = FPath.Count;
                                    if (((((FFrame - 9) + Half) + 5) == (call__850 - 1)) || ((Half * 2) == N))
                                    {
                                        var call__851 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__852 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__853 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__854 = Sequence.ImageLibrary[Index];
                                        call__854.ShowClipped((call__853.X - XhpSurf) + Sequence.dXshift, ((call__851.Y - call__852.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                                    }
                                    else
                                    {
                                        var call__855 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__856 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__857 = FPath[((FFrame - 9) + Half) + 6];
                                        var call__858 = FPath[((FFrame - 9) + Half) + 6];
                                        var call__859 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__860 = FPath[((FFrame - 9) + Half) + 6];
                                        var call__861 = Sequence.ImageLibrary[Index];
                                        call__861.ShowClipped((((call__859.X + call__860.X) / 2) - XhpSurf) + Sequence.dXshift, (((((call__855.Y - call__856.Z) + call__857.Y) - call__858.Z) / 2) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            void ShowRay()
            {
                int N = default;
                if (Sequence != null)
                {
                    {
                        N = 0;
                        int end__862 = FFrame;
                        for (; N <= end__862; N += 1)
                        {
                            Index = Sequence.GetUnloopedFrameIndex(0);
                            bool bool__863 = (Index != -1);
                            if (bool__863)
                            {
                                var call__864 = Sequence.ImageLibrary.ImageExist(Index);
                                bool__863 = call__864;
                            }

                            if (bool__863)
                            {
                                var call__865 = FPath[N];
                                var call__866 = FPath[N];
                                var call__867 = FPath[N];
                                var call__868 = Sequence.ImageLibrary[Index];
                                call__868.ShowClipped((call__867.X - XhpSurf) + Sequence.dXshift, ((call__865.Y - call__866.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                            }
                        }
                    }
                }
            }

            void ShowFrostBeam()
            {
                int N = default;
                int Alpha = default;
                switch (FFrame)
                {
                    case >= 0 and <= 1999:
                        SelectSequence(0);
                        break;
                    default:
                        SelectSequence(1);
                        break;
                }

                if (Sequence != null)
                {
                    N = 0;
                    while (true)
                    {
                        bool bool__869 = (N <= FFrame);
                        if (bool__869)
                        {
                            var call__870 = FPath.Count;
                            bool__869 = (N < call__870);
                        }

                        if (!bool__869)
                            break;
                        {
                            switch (FFrame)
                            {
                                case >= 0 and <= 1999:
                                    Alpha = 25;
                                    // { Original uses the object address as the animation phase. }
                                    var call__871 = Sequence.GetTotalSequenceFrames();
                                    var call__872 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__871);
                                    Index = call__872;
                                    break;
                                case >= 2000 and <= 2998:
                                    if (N <= (FFrame - 2000))
                                    {
                                        var call__873 = FPath.Count;
                                        Alpha = (((FFrame - 2000) * 75) / (call__873 - 1)) + 25;
                                        Index = Sequence.GetUnloopedFrameIndex(0);
                                    }
                                    else
                                    {
                                        if (N == (FFrame - 1999))
                                        {
                                            SelectSequence(0);
                                        }

                                        Alpha = 25;
                                        var call__874 = Sequence.GetTotalSequenceFrames();
                                        var call__875 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__874);
                                        Index = call__875;
                                    }

                                    break;
                                default:
                                    Alpha = 100;
                                    Index = Sequence.GetUnloopedFrameIndex(0);
                                    break;
                            }

                            // { Stop at the path end in the loop condition. }
                            if (Index != -1)
                            {
                                var call__876 = FPath[N];
                                var call__877 = FPath[N];
                                var call__878 = FPath[N];
                                var call__879 = Sequence.ImageLibrary[Index];
                                call__879.ShowBlended((call__878.X - XhpSurf) + Sequence.dXshift, ((call__876.Y - call__877.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Alpha]);
                            }

                            N += 1;
                        }
                    }
                }
            }

            void ShowAtStart()
            {
                while (Sequence != null)
                {
                    var call__880 = Sequence.GetUnloopedFrameIndex(FFrame);
                    Index = call__880;
                    bool bool__881 = (Index != -1);
                    if (bool__881)
                    {
                        var call__882 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__881 = call__882;
                    }

                    if (bool__881)
                    {
                        var call__883 = FPath[0];
                        var call__884 = FPath[0];
                        var call__885 = Sequence.ImageLibrary[Index];
                        call__885.ShowClipped((call__884.X - XhpSurf) + Sequence.dXshift, (call__883.Y - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowAtStartLooped()
            {
                while (Sequence != null)
                {
                    var call__886 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                    Index = call__886;
                    bool bool__887 = (Index != -1);
                    if (bool__887)
                    {
                        var call__888 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__887 = call__888;
                    }

                    if (bool__887)
                    {
                        var call__889 = FPath[0];
                        var call__890 = FPath[0];
                        var call__891 = Sequence.ImageLibrary[Index];
                        call__891.ShowClipped((call__890.X - XhpSurf) + Sequence.dXshift, (call__889.Y - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowScreen()
            {
                while (Sequence != null)
                {
                    bool bool__892 = (Index != -1);
                    if (bool__892)
                    {
                        var call__893 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__892 = call__893;
                    }

                    if (bool__892)
                    {
                        var call__894 = Sequence.ImageLibrary[Index];
                        var call__895 = FPath[0];
                        var call__896 = Sequence.ImageLibrary[Index];
                        var call__897 = Sequence.ImageLibrary[Index];
                        var call__898 = FPath[0];
                        var call__899 = Sequence.ImageLibrary[Index];
                        var call__900 = Sequence.ImageLibrary[Index];
                        call__900.ShowClipped((((call__897.Width + SurfaceDesc__value.Width) * call__898.X) / 100) - call__899.Width, (((call__894.Height + SurfaceDesc__value.Height) * call__895.Y) / 100) - call__896.Height, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowScreenSwarm()
            {
                int Alpha = default;
                var call__901 = FPath[FFrame];
                Alpha = ((100 - call__901.Z) / 2) - ((((FFrame % 10) * 2) + Aow.Rtl.Rtl.ObjectToInt(this)) % 20);
                if (Alpha < 0)
                {
                    Alpha = 0;
                }

                while (Sequence != null)
                {
                    var call__902 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                    Index = call__902;
                    bool bool__903 = (Index != -1);
                    if (bool__903)
                    {
                        var call__904 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__903 = call__904;
                    }

                    if (bool__903)
                    {
                        var call__905 = FPath[0];
                        var call__906 = FPath[FFrame];
                        var call__907 = FPath[0];
                        var call__908 = FPath[FFrame];
                        var call__909 = FPath[0];
                        var call__910 = FPath[FFrame];
                        var call__911 = FPath[FFrame];
                        var call__912 = Sequence.ImageLibrary[Index];
                        call__912.ShowBlended(((((((XhpSurf + SurfaceDesc__value.Width) - call__905.X) + 100) * call__906.Z) / 100) + (call__907.X - XhpSurf)) + call__908.X, (((((call__909.Y - YhpSurf) + 100) * (100 - call__910.Z)) / 100) + call__911.Y) - 100, SurfaceDesc__value, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Alpha]);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowParticle(bool Shadow)
            {
                int Alpha = default;
                var call__913 = FPath.Count;
                Alpha = (100 - ((FFrame * 100) / call__913)) - ((((FFrame % 10) * 2) + Aow.Rtl.Rtl.ObjectToInt(this)) % 20);
                if (Alpha < 0)
                {
                    Alpha = 0;
                }

                if (Shadow)
                {
                    Alpha += 20;
                }

                if (Alpha > 100)
                {
                    Alpha = 100;
                }

                while (Sequence != null)
                {
                    // { Original uses the object address rather than the current frame. }
                    Index = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % 3);
                    if (Index != -1)
                    {
                        if (Shadow)
                        {
                            var call__914 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__914)
                            {
                                var call__915 = FPath[FFrame];
                                var call__916 = FPath[FFrame];
                                var call__917 = FPath[FFrame];
                                var call__918 = Sequence.ImageLibrary[Index];
                                call__918.ShowBlended((call__917.X - XhpSurf) + Sequence.dXshift, ((call__915.Y - call__916.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value, Aow.Units.GFXE.GFXE.ShadowBlendTable.LookupTables[Alpha]);
                            }
                        }
                        else
                        {
                            var call__919 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__919)
                            {
                                var call__920 = FPath[FFrame];
                                var call__921 = FPath[FFrame];
                                var call__922 = FPath[FFrame];
                                var call__923 = Sequence.ImageLibrary[Index];
                                call__923.ShowBlended((call__922.X - XhpSurf) + Sequence.dXshift, ((call__920.Y - call__921.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Alpha]);
                            }
                        }
                    }

                    Sequence = Sequence.Next;
                }
            }

            void FinishPath()
            {
                var call__924 = FPath.Count;
                if (FFrame >= (call__924 - 1))
                {
                    FDone = true;
                    FFrame = -1;
                }
            }

            void FinishSequence()
            {
                bool bool__925 = (FirstSequence == null);
                if (!bool__925)
                {
                    bool bool__926 = (FirstSequence != null);
                    if (bool__926)
                    {
                        var call__927 = FirstSequence.GetTotalSequenceFrames();
                        bool__926 = (FFrame >= (call__927 - 1));
                    }

                    bool__925 = bool__926;
                }

                if (bool__925)
                {
                    FDone = true;
                    FFrame = -1;
                }
            }

            try
            {
                if (FFrame >= 0)
                {
                    Sequence = null;
                    Index = 0;
                    switch (FEffectType)
                    {
                        case Aow.Units.AoWTC.TfxType.fxArrow:
                            SelectSequence(FVariant);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxAltArrow:
                            SelectSequence(0);
                            ShowSecondaryFrame();
                            SelectSequence(1);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxRock:
                            SelectSequence(0);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxBreath:
                            SelectSequence(0);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxCometBall:
                        case Aow.Units.AoWTC.TfxType.fxScreenNS:
                            SelectSequence(0);
                            ShowMovingLoop();
                            SelectSequence(1);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxComet:
                        case Aow.Units.AoWTC.TfxType.fxDecayComet:
                            SelectSequence(0);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxTouch:
                        case Aow.Units.AoWTC.TfxType.fxEffect:
                            SelectSequence(0);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxFBExplosion:
                            SelectSequence(1);
                            if (Sequence != null)
                            {
                                SelectFrame();
                                ShowAtStart();
                                FinishSequence();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSFOverlay:
                            SelectSequence(2);
                            if (Sequence != null)
                            {
                                var call__928 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                                Index = call__928;
                                ShowAtStart();
                                FinishPath();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                            SelectSequence(Aow.Rtl.Rtl.ObjectToInt(this) % 3);
                            Index = Sequence.GetUnloopedFrameIndex(0);
                            ShowScreen();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxRay:
                            SelectSequence(0);
                            ShowRay();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                            ShowFrostBeam();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxChain:
                            SelectSequence(0);
                            ShowRay();
                            var call__929 = FPath.Count;
                            bool bool__930 = (FFrame >= (call__929 - 1));
                            if (bool__930)
                            {
                                var call__931 = FPath[FFrame];
                                bool__930 = (call__931.Z == 1);
                            }

                            if (bool__930)
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSwarm:
                            SelectSequence(0);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                            SelectSequence(0);
                            ShowScreenSwarm();
                            FinishPath();
                            break;
                        case >= Aow.Units.AoWTC.TfxType.fxDamLightning and <= Aow.Units.AoWTC.TfxType.fxDamCold:
                            SelectDamageSequence();
                            ShowParticle(FEffectType == Aow.Units.AoWTC.TfxType.fxDamDeath);
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                            if (TSet<byte>.WithBounds(0, 255).WithRange(5, 6).Contains((byte)FFrame))
                            {
                                SelectSequence(2);
                                while (Sequence != null)
                                {
                                    Index = Sequence.GetUnloopedFrameIndex(0);
                                    bool bool__932 = (Index != -1);
                                    if (bool__932)
                                    {
                                        var call__933 = Sequence.ImageLibrary.ImageExist(Index);
                                        bool__932 = call__933;
                                    }

                                    if (bool__932)
                                    {
                                        {
                                            I = 0;
                                            int end__934 = SurfaceDesc__value.Width / Sequence.ImageLibrary[Index].Width;
                                            for (; I <= end__934; I += 1)
                                            {
                                                {
                                                    J = 0;
                                                    int end__935 = SurfaceDesc__value.Height / Sequence.ImageLibrary[Index].Height;
                                                    for (; J <= end__935; J += 1)
                                                    {
                                                        var call__936 = Sequence.ImageLibrary[Index];
                                                        var call__937 = Sequence.ImageLibrary[Index];
                                                        var call__938 = Sequence.ImageLibrary[Index];
                                                        call__938.ShowClipped(call__937.Width * I, call__936.Height * J, SurfaceDesc__value);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    Sequence = Sequence.Next;
                                }
                            }

                            SelectSequence(1);
                            if (Sequence != null)
                            {
                                SelectFrame();
                                ShowAtStart();
                                FinishSequence();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSWComet:
                            var call__939 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                            var call__940 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetXhx();
                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__940, call__939, ref I, ref J);
                            Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)((SurfaceDesc__value.Width / 2) + XhpSurf)), unchecked((short)((SurfaceDesc__value.Height / 2) + YhpSurf)), unchecked((short)I), unchecked((short)J), 25, 50, ref FPath);
                            FEffectType = Aow.Units.AoWTC.TfxType.fxComet;
                            SelectSequence(0);
                            ShowTrail();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxWallSmoke:
                            var call__941 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[5];
                            Sequence = call__941;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitGround:
                            var call__942 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[9];
                            Sequence = call__942;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitWater:
                            var call__943 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[10];
                            Sequence = call__943;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxBlood:
                            var call__944 = FPath[0];
                            var call__945 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[call__944.Z];
                            Sequence = call__945;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxGeyser:
                            if (FFrame < 5)
                            {
                                SelectSequence(1);
                                ShowAtStart();
                            }
                            else
                            {
                                SelectSequence(2);
                                ShowAtStartLooped();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHailImpact:
                            SelectSequence(2);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxPrayerGlow:
                            SelectSequence(1);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxCloudPuff:
                            SelectSequence(0);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxGeyserWater:
                            SelectSequence(0);
                            ShowParticle((FSpellIndex == 130) || (FSpellIndex == 106));
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxFrost:
                            SelectSequence(1);
                            ShowParticle(FSpellIndex == 130);
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxDecayLeg:
                            SelectSequence(0);
                            ShowRay();
                            FCancelled = true;
                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacFire:
                            SelectSequence(0);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
                // { Original puts the diagnostic in the message-box caption. }
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Show", AnsiString.FromPChar(Ptr<byte>.Unbox(((((AnsiString)"Error - fxHS ") + Aow.Rtl.Rtl.IntToStr(FAbilityIndex)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(FSpellIndex))), (int)0);
            }
        }
    }
    public partial class TTacticalCombatUnitHS
    {
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Index = default;
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            double Progress = default;
            int NX = default;
            int NY = default;
            int FlyingOffset = default;
            int Race = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            Aow.Units.CityWall.TCityWall NextWall = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            try
            {
                if (!(FDirection < FUnitGFXResource.FSequences.Count))
                {
                    Aow.Units.AoWTC.AoWTC.OriginalUnitAssert((AnsiString)"Invalid Unit Index", (AnsiString)"D:\\AoWDev\\AoWTC\\inc\\TCUnitHS.pas", 4262);
                }

                Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
                XhpSurf = (X - XhpSurf) + FOffsetX;
                YhpSurf = ((Y - YhpSurf) - 8) + FOffsetY;
                var call__1591 = FUnit.GetPlayer();
                if (call__1591 > 0)
                {
                    var call__1592 = FUnit.GetPlayer();
                    var call__1593 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1592);
                    Race = call__1593.FPlayerRace;
                }
                else
                {
                    Race = 32;
                }

                var call__1594 = FUnit.GetAbilityEnabled(1);
                if (call__1594)
                {
                    FlyingOffset = -8;
                }
                else
                {
                    FlyingOffset = 0;
                }

                var call__1595 = FUnit.GetAbilityEnabled(36);
                if (call__1595)
                {
                    var call__1596 = Owner.FindOwnedChild(2229508);
                    Wall = (Aow.Units.CityWall.TCityWall)call__1596;
                    if (Wall == null)
                    {
                        var call__1597 = Owner.FindOwnedChild(2229510);
                        Wall = (Aow.Units.CityWall.TCityWall)call__1597;
                    }

                    if (FMoveStep == -1)
                    {
                        if ((Wall != null) && (Wall.FHitPoints > 0))
                        {
                            YhpSurf -= 32;
                        }
                    }
                    else
                    {
                        var call__1598 = GetYhx();
                        var call__1599 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1599, call__1598, FDirection, ref NX, ref NY);
                        var call__1600 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1601 = call__1600.Field[NX, NY];
                        var call__1602 = call__1601.FindOwnedChild(2229508);
                        NextWall = (Aow.Units.CityWall.TCityWall)call__1602;
                        if (NextWall == null)
                        {
                            var call__1603 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1604 = call__1603.Field[NX, NY];
                            var call__1605 = call__1604.FindOwnedChild(2229510);
                            NextWall = (Aow.Units.CityWall.TCityWall)call__1605;
                        }

                        var call__1606 = GetMoveSteps();
                        if (FMoveStep < (call__1606 / 2))
                        {
                            if ((Wall != null) && (Wall.FHitPoints > 0))
                            {
                                if ((NextWall != null) && (NextWall.FHitPoints > 0))
                                {
                                    YhpSurf -= 32;
                                }
                                else
                                {
                                    var call__1607 = GetMoveSteps();
                                    var call__1608 = GetMoveSteps();
                                    YhpSurf -= ((call__1607 - FMoveStep) * 32) / call__1608;
                                }
                            }
                        }
                        else
                        {
                            if ((NextWall != null) && (NextWall.FHitPoints > 0))
                            {
                                if ((Wall != null) && (Wall.FHitPoints > 0))
                                {
                                    YhpSurf -= 32;
                                }
                                else
                                {
                                    var call__1609 = GetMoveSteps();
                                    YhpSurf -= (FMoveStep * 32) / call__1609;
                                }
                            }
                        }
                    }
                }

                switch (FAction)
                {
                    case Aow.Units.AoWTC.TTCAction.caMove:
                    case Aow.Units.AoWTC.TTCAction.caBusy:
                    case Aow.Units.AoWTC.TTCAction.caFreeStruck:
                        if ((FAction == Aow.Units.AoWTC.TTCAction.caMove) && (FMoveStep != -1))
                        {
                            Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref DX, ref DY);
                            var call__1610 = GetMoveSteps();
                            Progress = ((double)FMoveStep) / call__1610;
                            XhpSurf += Aow.Rtl.Rtl.Round(((double)DX) * Progress);
                            YhpSurf += Aow.Rtl.Rtl.Round(((double)DY) * Progress);
                            var call__1611 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                            Sequence = call__1611;
                        }
                        else
                        {
                            var call__1612 = FUnitGFXResource.FSequences.ImageSequences[FDirection - 1];
                            Sequence = call__1612;
                        }

                        while (Sequence != null)
                        {
                            var call__1613 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                            Index = call__1613;
                            if (Index != -1)
                            {
                                var call__1614 = Sequence.ImageLibrary[Index];
                                call__1614.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caMeleeHit:
                    case Aow.Units.AoWTC.TTCAction.caMeleeMiss:
                    case Aow.Units.AoWTC.TTCAction.caAbility:
                    case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                        var call__1615 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                        Sequence = call__1615;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1616 = Sequence.GetUnloopedFrameIndex(FActionFrame);
                                Index = call__1616;
                                if (Index != -1)
                                {
                                    var call__1617 = Sequence.ImageLibrary[Index];
                                    call__1617.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caHit:
                        var call__1618 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                        Sequence = call__1618;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1619 = Sequence.GetUnloopedFrameIndex(FActionFrame);
                                Index = call__1619;
                                if (Index != -1)
                                {
                                    var call__1620 = Sequence.ImageLibrary[Index];
                                    call__1620.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caStepUp:
                    case Aow.Units.AoWTC.TTCAction.caStepBack:
                        var call__1621 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                        Sequence = call__1621;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1622 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame + FActionFrame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                                Index = call__1622;
                                if (Index != -1)
                                {
                                    var call__1623 = Sequence.ImageLibrary[Index];
                                    call__1623.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                    case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                        var call__1624 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FDirection + 15];
                        Sequence = call__1624;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1625 = Sequence.GetUnloopedFrameIndex(FActionFrame);
                                Index = call__1625;
                                if (Index != -1)
                                {
                                    if (Sequence.Layer == 0)
                                    {
                                        while (true)
                                        {
                                            var call__1626 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                                            var call__1627 = call__1626.ImageLibrary.ImageExist(Index);
                                            if (!!call__1627)
                                                break;
                                            {
                                                Index -= 1;
                                            }
                                        }

                                        var call__1628 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                                        var call__1629 = call__1628.ImageLibrary[Index];
                                        call__1629.Show((XhpSurf + Sequence.dXshift) + 16, ((YhpSurf + FlyingOffset) + Sequence.dYshift) + 16, SurfaceDesc);
                                    }
                                    else
                                    {
                                        var call__1630 = Sequence.ImageLibrary[Index];
                                        call__1630.Show((XhpSurf + Sequence.dXshift) + 16, ((YhpSurf + Sequence.dYshift) + FlyingOffset) + 16, SurfaceDesc);
                                    }
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                }

                if (FUnit.FUnit.FUnitStatus.Contains(Aow.Units.AoWE.TUnitStatusElement.usEnchanted))
                {
                    Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ShowLooped(70, XhpSurf + 16, (YhpSurf + 18) + FlyingOffset, Aow.Units.AoWE.AoWE.AoWHSMap.Frame, SurfaceDesc);
                }

                {
                    NX = 0;
                    byte end__1631 = 13;
                    for (; NX <= end__1631; NX += 1)
                    {
                        var call__1632 = FUnit.GetAbilityEnabled(Aow.Units.AoWTC.AoWTC.StatusAbils[NX]);
                        if (call__1632)
                        {
                            var call__1633 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Units.AoWTC.AoWTC.StatusAbils[NX]);
                            var call__1634 = call__1633.FImageSequences.ImageSequences[0];
                            Sequence = call__1634;
                            while (Sequence != null)
                            {
                                var call__1635 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                                Index = call__1635;
                                if (Index != -1)
                                {
                                    var call__1636 = Sequence.ImageLibrary[Index];
                                    call__1636.Show((XhpSurf + Sequence.dXshift) + 15, ((YhpSurf + Sequence.dYshift) + 10) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }
                    }
                }

                var call__1637 = GetSelected();
                if (!call__1637 || ((Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame & 6) != 6))
                {
                    if (Race == 32)
                    {
                        if ((FUnit.FMovePoints == 0) || (FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                        {
                            var call__1638 = Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.FFlags[70];
                            call__1638.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                        else
                        {
                            var call__1639 = Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.FFlags[FMoveIndicator + 69];
                            call__1639.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                    }
                    else
                    {
                        if ((FUnit.FMovePoints == 0) || (FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                        {
                            var call__1640 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Race);
                            var call__1641 = call__1640.FImageLibrary[70];
                            call__1641.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                        else
                        {
                            var call__1642 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Race);
                            var call__1643 = call__1642.FImageLibrary[FMoveIndicator + 69];
                            call__1643.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                    }

                    SD = SurfaceDesc;
                    var call__1644 = FUnit.GetHitPoints();
                    var call__1645 = FUnit.GetHits();
                    NX = (call__1644 * 10) / call__1645;
                    if (NX == 0)
                    {
                        NX = 1;
                    }

                    if (((XhpSurf - 4) + NX) < SD.ClipRect.Right)
                    {
                        SD.ClipRect.Right = (XhpSurf - 4) + NX;
                    }

                    var call__1646 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[1];
                    call__1646.ShowClipped(XhpSurf - 4, YhpSurf - 4, SD);
                }
            }
            catch (System.Exception)
            {
                var call__1590 = FUnit.FUnit.GetName();
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during ShowUnit", AnsiString.FromPChar(Ptr<byte>.Unbox(((AnsiString)"Error - ") + call__1590)), (int)0);
            }
        }

        public override void ShowShadow(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            float Progress = default;
            int PX = default;
            int PY = default;
            int Index = default;
            int DX = default;
            int DY = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref PX, ref PY);
            X = (PX - X) + FOffsetX;
            if (FBusy)
            {
                Y = (PY - Y) - 8;
            }
            else
            {
                Y = ((PY - Y) - 8) + FOffsetY;
            }

            if (FMoveStep != -1)
            {
                Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref DX, ref DY);
                var call__1743 = GetMoveSteps();
                Progress = (float)(((double)FMoveStep) / call__1743);
                X += Aow.Rtl.Rtl.Round(((double)DX) * Progress);
                Y += Aow.Rtl.Rtl.Round(((double)DY) * Progress);
            }

            var call__1744 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 23];
            Sequence = call__1744;
            while (Sequence != null)
            {
                var call__1745 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                Index = call__1745;
                if (Index != -1)
                {
                    var call__1746 = Sequence.ImageLibrary[Index];
                    call__1746.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        public void NewFrame(Aow.Rtl.TObject Sender)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            if (FActionDone)
            {
                SetAction(Aow.Units.AoWTC.TTCAction.caMove);
            }

            if (FMoveIndicator == 0)
            {
                var call__1748 = Aow.Rtl.Rtl.Random(5);
                FMoveIndicator = unchecked((short)(call__1748 + 1));
                FIndicatorStep = 1;
            }
            else
            {
                FMoveIndicator = unchecked((short)(FMoveIndicator + FIndicatorStep));
                if (FMoveIndicator == 1)
                {
                    FIndicatorStep = 1;
                }
                else
                {
                    if (FMoveIndicator == 6)
                    {
                        FIndicatorStep = unchecked((short)-1);
                    }
                }
            }

            switch (FAction)
            {
                case Aow.Units.AoWTC.TTCAction.caStepUp:
                    var call__1749 = GetMoveSteps();
                    if (FActionFrame <= call__1749)
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        var call__1750 = GetMoveSteps();
                        FOffsetX = (FActionFrame * FOffsetX) / (call__1750 * 2);
                        var call__1751 = GetMoveSteps();
                        FOffsetY = (FActionFrame * FOffsetY) / (call__1751 * 2);
                        FActionFrame = (short)(((int)FActionFrame) + 2);
                    }
                    else
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        FOffsetX = FOffsetX / 2;
                        FOffsetY = FOffsetY / 2;
                        FActionDone = true;
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caStepBack:
                    var call__1752 = GetMoveSteps();
                    if (FActionFrame <= call__1752)
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        var call__1753 = GetMoveSteps();
                        var call__1754 = GetMoveSteps();
                        FOffsetX = ((call__1753 - FActionFrame) * FOffsetX) / (call__1754 * 2);
                        var call__1755 = GetMoveSteps();
                        var call__1756 = GetMoveSteps();
                        FOffsetY = ((call__1755 - FActionFrame) * FOffsetY) / (call__1756 * 2);
                        FActionFrame = (short)(((int)FActionFrame) + 2);
                    }
                    else
                    {
                        FOffsetX = 0;
                        FOffsetY = 0;
                        FActionDone = true;
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caMeleeHit:
                case Aow.Units.AoWTC.TTCAction.caMeleeMiss:
                case Aow.Units.AoWTC.TTCAction.caAbility:
                    var call__1757 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    Sequence = call__1757;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__1758 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__1758 - 1))
                        {
                            var call__1759 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__1759 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                    var call__1760 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    Sequence = call__1760;
                    if (Sequence != null)
                    {
                        var call__1761 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame < (call__1761 - 1))
                        {
                            FActionFrame = (short)(((int)FActionFrame) + 1);
                        }
                        else
                        {
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caHit:
                    var call__1762 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                    Sequence = call__1762;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__1763 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__1763 - 1))
                        {
                            var call__1764 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__1764 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                    var call__1765 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FDirection + 16];
                    Sequence = call__1765;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__1766 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__1766 - 1))
                        {
                            var call__1767 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__1767 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                default:
                    FActionFrame = unchecked((short)-1);
                    break;
            }

            switch (FAction)
            {
                case Aow.Units.AoWTC.TTCAction.caMove:
                case Aow.Units.AoWTC.TTCAction.caStepUp:
                case Aow.Units.AoWTC.TTCAction.caStepBack:
                    if (FMoveStep != -1)
                    {
                        var call__1768 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                        Sequence = call__1768;
                        if (((Sequence != null) && (Sequence.SpeedCount == Sequence.SequenceSpeed)) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            var call__1769 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                            FUnitGFXResource.FSFXLibraries[1].Play(call__1769 % 100);
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caMeleeHit:
                case Aow.Units.AoWTC.TTCAction.caMeleeMiss:
                case Aow.Units.AoWTC.TTCAction.caAbility:
                case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                    if ((FActionFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        if (FSelectedAbility > -1)
                        {
                            if (FSelectedAbility == 52)
                            {
                                if (((FSelectedSpell != 112) && (FSelectedSpell != 122)) && (FSelectedSpell != 119))
                                {
                                    var call__1770 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSelectedSpell);
                                    call__1770.FCastSounds.Play(0);
                                }
                            }
                            else
                            {
                                var call__1771 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility);
                                call__1771.FSFXLibrary.Play(0);
                            }
                        }
                        else
                        {
                            if (FAction == Aow.Units.AoWTC.TTCAction.caMeleeMiss)
                            {
                                FUnitGFXResource.FSFXLibraries[2].Play(1);
                            }
                            else
                            {
                                FUnitGFXResource.FSFXLibraries[2].Play(0);
                            }
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caHit:
                case Aow.Units.AoWTC.TTCAction.caKilled:
                    if ((FActionFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        if (FAction == Aow.Units.AoWTC.TTCAction.caHit)
                        {
                            FUnitGFXResource.FSFXLibraries[3].Play(0);
                        }
                        else
                        {
                            FUnitGFXResource.FSFXLibraries[4].Play(0);
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                    if (((FActionFrame < 18) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (Aow.Units.AoWTC.AoWTC.raDir[FActionFrame] > 0))
                    {
                        if (FAction == Aow.Units.AoWTC.TTCAction.caRoundAttack)
                        {
                            FUnitGFXResource.FSFXLibraries[2].Play(1);
                        }
                        else
                        {
                            FUnitGFXResource.FSFXLibraries[2].Play(0);
                        }
                    }

                    break;
            }
        }
    }
}
