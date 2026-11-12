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
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        ImageSequence sequence;
        int index;
        ImageSequence firstSequence;
        int I = default;
        int J = default;
        void SelectSequence(int number)
        {
            if (AbilityIndex == 52)
            {
                sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[number];
            }
            else
            {
                sequence = GamePack.Current.AbilityControl.GetAbility(AbilityIndex).ImageSequences.ImageSequences[number];
            }
        }

        void SelectFrame()
        {
            if (sequence != null)
            {
                index = sequence.GetUnloopedFrameIndex(Frame);
            }

            firstSequence = sequence;
        }

        void SelectDamageSequence()
        {
            int N = default;
            switch (EffectType)
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

            sequence = GamePack.Current.ImageSequences.ImageSequences[N];
        }

        void ShowMovingLoop()
        {
            while (sequence != null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    var pathY = Path[Frame];
                    var pathZ = Path[Frame];
                    var pathX = Path[Frame];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowSecondaryFrame()
        {
            while (sequence != null)
            {
                index = sequence.GetUnloopedFrameIndex(Variant);
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    var pathY = Path[Frame];
                    var pathZ = Path[Frame];
                    var pathX = Path[Frame];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowTrail()
        {
            int lastFrame = default;
            int half = default;
            if (sequence != null)
            {
                lastFrame = sequence.GetTotalSequenceFrames() - 1;
                for (int N = 0; N <= lastFrame; N++)
                {
                    half = N / 2;
                    bool condition = Frame - 9 + half + 5 > -1;
                    if (condition)
                    {
                        var count = Path.Count;
                        condition = Frame - 9 + half + 5 < count;
                    }

                    if (condition)
                    {
                        index = sequence.GetUnloopedFrameIndex(lastFrame - N);
                        if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                        {
                            var count = Path.Count;
                            if (Frame - 9 + half + 5 == count - 1 || half * 2 == N)
                            {
                                var pathY = Path[Frame - 9 + half + 5];
                                var pathZ = Path[Frame - 9 + half + 5];
                                var pathX = Path[Frame - 9 + half + 5];
                                sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                            }
                            else
                            {
                                var pathY = Path[Frame - 9 + half + 5];
                                var pathZ = Path[Frame - 9 + half + 5];
                                var path = Path[Frame - 9 + half + 6];
                                var path2 = Path[Frame - 9 + half + 6];
                                var pathX = Path[Frame - 9 + half + 5];
                                var path3 = Path[Frame - 9 + half + 6];
                                sequence.ImageLibrary[index].ShowClipped((pathX.X + path3.X) / 2 - pixelXSurface + sequence.OffsetX, (pathY.Y - pathZ.Z + path.Y - path2.Z) / 2 - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                            }
                        }
                    }
                }
            }
        }

        void ShowRay()
        {
            if (sequence != null)
            {
                for (int N = 0, last = Frame; N <= last; N++)
                {
                    index = sequence.GetUnloopedFrameIndex(0);
                    if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                    {
                        var pathY = Path[N];
                        var pathZ = Path[N];
                        var pathX = Path[N];
                        sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                    }
                }
            }
        }

        void ShowFrostBeam()
        {
            int N = default;
            int alpha = default;
            switch (Frame)
            {
                case >= 0 and <= 1999:
                    SelectSequence(0);
                    break;
                default:
                    SelectSequence(1);
                    break;
            }

            if (sequence != null)
            {
                N = 0;
                while (N <= Frame && N < Path.Count)
                {
                    switch (Frame)
                    {
                        case >= 0 and <= 1999:
                            alpha = 25;
                            // { Original uses the object address as the animation phase. }
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            index = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, totalSequenceFrames));
                            break;
                        case >= 2000 and <= 2998:
                            if (N <= Frame - 2000)
                            {
                                var count = Path.Count;
                                alpha = (Frame - 2000) * 75 / (count - 1) + 25;
                                index = sequence.GetUnloopedFrameIndex(0);
                            }
                            else
                            {
                                if (N == Frame - 1999)
                                {
                                    SelectSequence(0);
                                }

                                alpha = 25;
                                var sequenceTotalSequenceFrames = sequence.GetTotalSequenceFrames();
                                index = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, sequenceTotalSequenceFrames));
                            }

                            break;
                        default:
                            alpha = 100;
                            index = sequence.GetUnloopedFrameIndex(0);
                            break;
                    }

                    // { Stop at the path end in the loop condition. }
                    if (index != -1)
                    {
                        var pathY = Path[N];
                        var pathZ = Path[N];
                        var pathX = Path[N];
                        sequence.ImageLibrary[index].ShowBlended(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[alpha]);
                    }

                    N += 1;
                }
            }
        }

        void ShowAtStart()
        {
            while (sequence != null)
            {
                index = sequence.GetUnloopedFrameIndex(Frame);
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    var pathY = Path[0];
                    var pathX = Path[0];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowAtStartLooped()
        {
            while (sequence != null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    var pathY = Path[0];
                    var pathX = Path[0];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowScreen()
        {
            while (sequence != null)
            {
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    var imageLibraryHeight = sequence.ImageLibrary[index];
                    var pathY = Path[0];
                    var imageLibrary = sequence.ImageLibrary[index];
                    var imageLibraryWidth = sequence.ImageLibrary[index];
                    var pathX = Path[0];
                    var imageLibrary2 = sequence.ImageLibrary[index];
                    sequence.ImageLibrary[index].ShowClipped((imageLibraryWidth.Width + lockedSurfaceValue.Width) * pathX.X / 100 - imageLibrary2.Width, (imageLibraryHeight.Height + lockedSurfaceValue.Height) * pathY.Y / 100 - imageLibrary.Height, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowScreenSwarm()
        {
            int alpha = default;
            alpha = (100 - Path[Frame].Z) / 2 - FrameFixtureInputs.ShimmerOffset(this, Frame);
            if (alpha < 0)
            {
                alpha = 0;
            }

            while (sequence != null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    var pathX = Path[0];
                    var pathZ = Path[Frame];
                    var path = Path[0];
                    var path2 = Path[Frame];
                    var pathY = Path[0];
                    var path3 = Path[Frame];
                    var path4 = Path[Frame];
                    sequence.ImageLibrary[index].ShowBlended((pixelXSurface + lockedSurfaceValue.Width - pathX.X + 100) * pathZ.Z / 100 + (path.X - pixelXSurface) + path2.X, (pathY.Y - pixelYSurface + 100) * (100 - path3.Z) / 100 + path4.Y - 100, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[alpha]);
                }

                sequence = sequence.Next;
            }
        }

        void ShowParticle(bool shadow)
        {
            int alpha = default;
            var count = Path.Count;
            alpha = 100 - Frame * 100 / count - FrameFixtureInputs.ShimmerOffset(this, Frame);
            if (alpha < 0)
            {
                alpha = 0;
            }

            if (shadow)
            {
                alpha += 20;
            }

            if (alpha > 100)
            {
                alpha = 100;
            }

            while (sequence != null)
            {
                // { Original uses the object address rather than the current frame. }
                index = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (index != -1)
                {
                    if (shadow)
                    {
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            var pathY = Path[Frame];
                            var pathZ = Path[Frame];
                            var pathX = Path[Frame];
                            sequence.ImageLibrary[index].ShowBlended(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue, GraphicsState.ShadowBlendTable.LookupTables[alpha]);
                        }
                    }
                    else
                    {
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            var pathY = Path[Frame];
                            var pathZ = Path[Frame];
                            var pathX = Path[Frame];
                            sequence.ImageLibrary[index].ShowBlended(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[alpha]);
                        }
                    }
                }

                sequence = sequence.Next;
            }
        }

        void FinishPath()
        {
            var count = Path.Count;
            if (Frame >= count - 1)
            {
                Done = true;
                Frame = -1;
            }
        }

        void FinishSequence()
        {
            bool condition = firstSequence == null;
            if (!condition)
            {
                bool test = firstSequence != null;
                if (test)
                {
                    var totalSequenceFrames = firstSequence.GetTotalSequenceFrames();
                    test = Frame >= totalSequenceFrames - 1;
                }

                condition = test;
            }

            if (condition)
            {
                Done = true;
                Frame = -1;
            }
        }

        try
        {
            if (Frame >= 0)
            {
                sequence = null;
                index = 0;
                switch (EffectType)
                {
                    case BattleEffectType.Arrow:
                        SelectSequence(Variant);
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
                        if (sequence != null)
                        {
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                        }

                        break;
                    case BattleEffectType.SFOverlay:
                        SelectSequence(2);
                        if (sequence != null)
                        {
                            index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            ShowAtStart();
                            FinishPath();
                        }

                        break;
                    case BattleEffectType.ScreenNESW:
                        SelectSequence(FrameFixtureInputs.PhaseModulo(this, 3));
                        index = sequence.GetUnloopedFrameIndex(0);
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
                        var count = Path.Count;
                        if (Frame >= count - 1 && Path[Frame].Z == 1)
                        {
                            Frame = -1;
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
                        ShowParticle(EffectType == BattleEffectType.DamDeath);
                        FinishPath();
                        break;
                    case BattleEffectType.SacredWrath:
                        if (Set<byte>.WithBounds(0, 255).WithRange(5, 6).Contains((byte)Frame))
                        {
                            SelectSequence(2);
                            while (sequence != null)
                            {
                                index = sequence.GetUnloopedFrameIndex(0);
                                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                                {
                                    I = 0;
                                    int last = lockedSurfaceValue.Width / sequence.ImageLibrary[index].Width;
                                    for (; I <= last; I += 1)
                                    {
                                        J = 0;
                                        int limit = lockedSurfaceValue.Height / sequence.ImageLibrary[index].Height;
                                        for (; J <= limit; J += 1)
                                        {
                                            var imageLibraryHeight = sequence.ImageLibrary[index];
                                            var imageLibraryWidth = sequence.ImageLibrary[index];
                                            sequence.ImageLibrary[index].ShowClipped(imageLibraryWidth.Width * I, imageLibraryHeight.Height * J, lockedSurfaceValue);
                                        }
                                    }
                                }

                                sequence = sequence.Next;
                            }
                        }

                        SelectSequence(1);
                        if (sequence != null)
                        {
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                        }

                        break;
                    case BattleEffectType.SWComet:
                        var hexY = ((TacticalBattleUnit)Target).UnitHexSprite.GetHexY();
                        HexGeometry.HexToPixel(((TacticalBattleUnit)Target).UnitHexSprite.GetHexX(), hexY, out I, out J);
                        EffectPaths.MakeArcPathXY((short)(lockedSurfaceValue.Width / 2 + pixelXSurface), (short)(lockedSurfaceValue.Height / 2 + pixelYSurface), (short)I, (short)J, 25, 50, ref Path);
                        EffectType = BattleEffectType.Comet;
                        SelectSequence(0);
                        ShowTrail();
                        break;
                    case BattleEffectType.WallSmoke:
                        sequence = GamePack.Current.ImageSequences.ImageSequences[5];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.HitGround:
                        sequence = GamePack.Current.ImageSequences.ImageSequences[9];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.HitWater:
                        sequence = GamePack.Current.ImageSequences.ImageSequences[10];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.Blood:
                        var path = Path[0];
                        sequence = GamePack.Current.ImageSequences.ImageSequences[path.Z];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case BattleEffectType.Geyser:
                        if (Frame < 5)
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
                        ShowParticle(SpellIndex == 130 || SpellIndex == 106);
                        FinishPath();
                        break;
                    case BattleEffectType.Frost:
                        SelectSequence(1);
                        ShowParticle(SpellIndex == 130);
                        FinishPath();
                        break;
                    case BattleEffectType.DecayLeg:
                        SelectSequence(0);
                        ShowRay();
                        Cancelled = true;
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
            MessageBox(0, "Error during Show", NullTerminatedText("Error - fxHS " + IntToStr(AbilityIndex) + " " + IntToStr(SpellIndex)), 0);
        }
    }
}
