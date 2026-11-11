#nullable disable
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class BattleEffectSprite
{
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        LockedSurface SurfaceDesc__value = SurfaceDesc;
        ImageSequence Sequence;
        int Index;
        ImageSequence FirstSequence;
        int I = default;
        int J = default;
        void SelectSequence(int Number)
        {
            if (FAbilityIndex == 52)
            {
                Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[Number];
            }
            else
            {
                Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[Number];
            }
        }

        void SelectFrame()
        {
            if (Sequence != null)
            {
                Index = Sequence.GetUnloopedFrameIndex(FFrame);
            }

            FirstSequence = Sequence;
        }

        void SelectDamageSequence()
        {
            int N = default;
            switch (FEffectType)
            {
                case BattleEffectType.DamLightning:
                    N = 40;
                    break;
                case BattleEffectType.DamFire:
                    N = 41;
                    break;
                case BattleEffectType.DamPoison:
                    N = 42;
                    break;
                case BattleEffectType.DamMagic:
                    N = 43;
                    break;
                case BattleEffectType.DamHoly:
                    N = 44;
                    break;
                case BattleEffectType.DamDeath:
                    N = 45;
                    break;
                default:
                    N = 46;
                    break;
            }

            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[N];
        }

        void ShowMovingLoop()
        {
            while (Sequence != null)
            {
                Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var pathY = FPath[FFrame];
                    var pathZ = FPath[FFrame];
                    var pathX = FPath[FFrame];
                    Sequence.ImageLibrary[Index].ShowClipped(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowSecondaryFrame()
        {
            while (Sequence != null)
            {
                Index = Sequence.GetUnloopedFrameIndex(FVariant);
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var pathY = FPath[FFrame];
                    var pathZ = FPath[FFrame];
                    var pathX = FPath[FFrame];
                    Sequence.ImageLibrary[Index].ShowClipped(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowTrail()
        {
            int LastFrame = default;
            int Half = default;
            if (Sequence != null)
            {
                LastFrame = Sequence.GetTotalSequenceFrames() - 1;
                for (int N = 0; N <= LastFrame; N++)
                {
                    Half = N / 2;
                    bool condition = FFrame - 9 + Half + 5 > -1;
                    if (condition)
                    {
                        var count = FPath.Count;
                        condition = FFrame - 9 + Half + 5 < count;
                    }

                    if (condition)
                    {
                        Index = Sequence.GetUnloopedFrameIndex(LastFrame - N);
                        if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                        {
                            var count = FPath.Count;
                            if (FFrame - 9 + Half + 5 == count - 1 || Half * 2 == N)
                            {
                                var pathY = FPath[FFrame - 9 + Half + 5];
                                var pathZ = FPath[FFrame - 9 + Half + 5];
                                var pathX = FPath[FFrame - 9 + Half + 5];
                                Sequence.ImageLibrary[Index].ShowClipped(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
                            }
                            else
                            {
                                var pathY = FPath[FFrame - 9 + Half + 5];
                                var pathZ = FPath[FFrame - 9 + Half + 5];
                                var path = FPath[FFrame - 9 + Half + 6];
                                var path2 = FPath[FFrame - 9 + Half + 6];
                                var pathX = FPath[FFrame - 9 + Half + 5];
                                var path3 = FPath[FFrame - 9 + Half + 6];
                                Sequence.ImageLibrary[Index].ShowClipped((pathX.X + path3.X) / 2 - XhpSurf + Sequence.dXshift, (pathY.Y - pathZ.Z + path.Y - path2.Z) / 2 - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
                            }
                        }
                    }
                }
            }
        }

        void ShowRay()
        {
            if (Sequence != null)
            {
                for (int N = 0, last = FFrame; N <= last; N++)
                {
                    Index = Sequence.GetUnloopedFrameIndex(0);
                    if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                    {
                        var pathY = FPath[N];
                        var pathZ = FPath[N];
                        var pathX = FPath[N];
                        Sequence.ImageLibrary[Index].ShowClipped(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
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
                while (N <= FFrame && N < FPath.Count)
                {
                    switch (FFrame)
                    {
                        case >= 0 and <= 1999:
                            Alpha = 25;
                            // { Original uses the object address as the animation phase. }
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            Index = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, totalSequenceFrames));
                            break;
                        case >= 2000 and <= 2998:
                            if (N <= FFrame - 2000)
                            {
                                var count = FPath.Count;
                                Alpha = (FFrame - 2000) * 75 / (count - 1) + 25;
                                Index = Sequence.GetUnloopedFrameIndex(0);
                            }
                            else
                            {
                                if (N == FFrame - 1999)
                                {
                                    SelectSequence(0);
                                }

                                Alpha = 25;
                                var sequenceTotalSequenceFrames = Sequence.GetTotalSequenceFrames();
                                Index = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, sequenceTotalSequenceFrames));
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
                        var pathY = FPath[N];
                        var pathZ = FPath[N];
                        var pathX = FPath[N];
                        Sequence.ImageLibrary[Index].ShowBlended(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value, GraphicsState.IntensityBlendTable.LookupTables[Alpha]);
                    }

                    N += 1;
                }
            }
        }

        void ShowAtStart()
        {
            while (Sequence != null)
            {
                Index = Sequence.GetUnloopedFrameIndex(FFrame);
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var pathY = FPath[0];
                    var pathX = FPath[0];
                    Sequence.ImageLibrary[Index].ShowClipped(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowAtStartLooped()
        {
            while (Sequence != null)
            {
                Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var pathY = FPath[0];
                    var pathX = FPath[0];
                    Sequence.ImageLibrary[Index].ShowClipped(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - YhpSurf + Sequence.dYshift, SurfaceDesc__value);
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowScreen()
        {
            while (Sequence != null)
            {
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var imageLibraryHeight = Sequence.ImageLibrary[Index];
                    var pathY = FPath[0];
                    var imageLibrary = Sequence.ImageLibrary[Index];
                    var imageLibraryWidth = Sequence.ImageLibrary[Index];
                    var pathX = FPath[0];
                    var imageLibrary2 = Sequence.ImageLibrary[Index];
                    Sequence.ImageLibrary[Index].ShowClipped((imageLibraryWidth.Width + SurfaceDesc__value.Width) * pathX.X / 100 - imageLibrary2.Width, (imageLibraryHeight.Height + SurfaceDesc__value.Height) * pathY.Y / 100 - imageLibrary.Height, SurfaceDesc__value);
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowScreenSwarm()
        {
            int Alpha = default;
            Alpha = (100 - FPath[FFrame].Z) / 2 - FrameFixtureInputs.ShimmerOffset(this, FFrame);
            if (Alpha < 0)
            {
                Alpha = 0;
            }

            while (Sequence != null)
            {
                Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var pathX = FPath[0];
                    var pathZ = FPath[FFrame];
                    var path = FPath[0];
                    var path2 = FPath[FFrame];
                    var pathY = FPath[0];
                    var path3 = FPath[FFrame];
                    var path4 = FPath[FFrame];
                    Sequence.ImageLibrary[Index].ShowBlended((XhpSurf + SurfaceDesc__value.Width - pathX.X + 100) * pathZ.Z / 100 + (path.X - XhpSurf) + path2.X, (pathY.Y - YhpSurf + 100) * (100 - path3.Z) / 100 + path4.Y - 100, SurfaceDesc__value, GraphicsState.IntensityBlendTable.LookupTables[Alpha]);
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowParticle(bool Shadow)
        {
            int Alpha = default;
            var count = FPath.Count;
            Alpha = 100 - FFrame * 100 / count - FrameFixtureInputs.ShimmerOffset(this, FFrame);
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
                Index = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (Index != -1)
                {
                    if (Shadow)
                    {
                        if (Sequence.ImageLibrary.ImageExist(Index))
                        {
                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            var pathX = FPath[FFrame];
                            Sequence.ImageLibrary[Index].ShowBlended(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value, GraphicsState.ShadowBlendTable.LookupTables[Alpha]);
                        }
                    }
                    else
                    {
                        if (Sequence.ImageLibrary.ImageExist(Index))
                        {
                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            var pathX = FPath[FFrame];
                            Sequence.ImageLibrary[Index].ShowBlended(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value, GraphicsState.IntensityBlendTable.LookupTables[Alpha]);
                        }
                    }
                }

                Sequence = Sequence.Next;
            }
        }

        void FinishPath()
        {
            var count = FPath.Count;
            if (FFrame >= count - 1)
            {
                FDone = true;
                FFrame = -1;
            }
        }

        void FinishSequence()
        {
            bool condition = FirstSequence == null;
            if (!condition)
            {
                bool test = FirstSequence != null;
                if (test)
                {
                    var totalSequenceFrames = FirstSequence.GetTotalSequenceFrames();
                    test = FFrame >= totalSequenceFrames - 1;
                }

                condition = test;
            }

            if (condition)
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
                    case BattleEffectType.Arrow:
                        SelectSequence(FVariant);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                    case BattleEffectType.AltArrow:
                        SelectSequence(0);
                        ShowSecondaryFrame();
                        SelectSequence(1);
                        ShowTrail();
                        FinishPath();
                        break;
                    case BattleEffectType.Rock:
                        SelectSequence(0);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                    case BattleEffectType.Breath:
                        SelectSequence(0);
                        ShowTrail();
                        FinishPath();
                        break;
                    case BattleEffectType.CometBall:
                    case BattleEffectType.ScreenNS:
                        SelectSequence(0);
                        ShowMovingLoop();
                        SelectSequence(1);
                        ShowTrail();
                        FinishPath();
                        break;
                    case BattleEffectType.Comet:
                    case BattleEffectType.DecayComet:
                        SelectSequence(0);
                        ShowTrail();
                        FinishPath();
                        break;
                    case BattleEffectType.Touch:
                    case BattleEffectType.Effect:
                        SelectSequence(0);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.FBExplosion:
                        SelectSequence(1);
                        if (Sequence != null)
                        {
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                        }

                        break;
                    case BattleEffectType.SFOverlay:
                        SelectSequence(2);
                        if (Sequence != null)
                        {
                            Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            ShowAtStart();
                            FinishPath();
                        }

                        break;
                    case BattleEffectType.ScreenNESW:
                        SelectSequence(FrameFixtureInputs.PhaseModulo(this, 3));
                        Index = Sequence.GetUnloopedFrameIndex(0);
                        ShowScreen();
                        break;
                    case BattleEffectType.Ray:
                        SelectSequence(0);
                        ShowRay();
                        FinishPath();
                        break;
                    case BattleEffectType.FrostBeam:
                        ShowFrostBeam();
                        break;
                    case BattleEffectType.Chain:
                        SelectSequence(0);
                        ShowRay();
                        var count = FPath.Count;
                        if (FFrame >= count - 1 && FPath[FFrame].Z == 1)
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.Swarm:
                        SelectSequence(0);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                    case BattleEffectType.ScreenSwarm:
                        SelectSequence(0);
                        ShowScreenSwarm();
                        FinishPath();
                        break;
                    case >= BattleEffectType.DamLightning and <= BattleEffectType.DamCold:
                        SelectDamageSequence();
                        ShowParticle(FEffectType == BattleEffectType.DamDeath);
                        FinishPath();
                        break;
                    case BattleEffectType.SacredWrath:
                        if (Set<byte>.WithBounds(0, 255).WithRange(5, 6).Contains((byte)FFrame))
                        {
                            SelectSequence(2);
                            while (Sequence != null)
                            {
                                Index = Sequence.GetUnloopedFrameIndex(0);
                                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                                {
                                    I = 0;
                                    int last = SurfaceDesc__value.Width / Sequence.ImageLibrary[Index].Width;
                                    for (; I <= last; I += 1)
                                    {
                                        J = 0;
                                        int limit = SurfaceDesc__value.Height / Sequence.ImageLibrary[Index].Height;
                                        for (; J <= limit; J += 1)
                                        {
                                            var imageLibraryHeight = Sequence.ImageLibrary[Index];
                                            var imageLibraryWidth = Sequence.ImageLibrary[Index];
                                            Sequence.ImageLibrary[Index].ShowClipped(imageLibraryWidth.Width * I, imageLibraryHeight.Height * J, SurfaceDesc__value);
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
                    case BattleEffectType.SWComet:
                        var yhx = ((TacticalBattleUnit)FTarget).FUnitHS.GetYhx();
                        HexGeometry.HXtoHP(((TacticalBattleUnit)FTarget).FUnitHS.GetXhx(), yhx, ref I, ref J);
                        EffectPaths.MakeArcPathXY((short)(SurfaceDesc__value.Width / 2 + XhpSurf), (short)(SurfaceDesc__value.Height / 2 + YhpSurf), (short)I, (short)J, 25, 50, ref FPath);
                        FEffectType = BattleEffectType.Comet;
                        SelectSequence(0);
                        ShowTrail();
                        break;
                    case BattleEffectType.WallSmoke:
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[5];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.HitGround:
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[9];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.HitWater:
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[10];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.Blood:
                        var path = FPath[0];
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[path.Z];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.Geyser:
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
                    case BattleEffectType.HailImpact:
                        SelectSequence(2);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.PrayerGlow:
                        SelectSequence(1);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.CloudPuff:
                        SelectSequence(0);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.GeyserWater:
                        SelectSequence(0);
                        ShowParticle(FSpellIndex == 130 || FSpellIndex == 106);
                        FinishPath();
                        break;
                    case BattleEffectType.Frost:
                        SelectSequence(1);
                        ShowParticle(FSpellIndex == 130);
                        FinishPath();
                        break;
                    case BattleEffectType.DecayLeg:
                        SelectSequence(0);
                        ShowRay();
                        FCancelled = true;
                        break;
                    case BattleEffectType.SacFire:
                        SelectSequence(0);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                }
            }
        }
        catch
        {
            // { Original puts the diagnostic in the message-box caption. }
            MessageBox(0, "Error during Show", NullTerminatedText("Error - fxHS " + IntToStr(FAbilityIndex) + " " + IntToStr(FSpellIndex)), 0);
        }
    }
}
