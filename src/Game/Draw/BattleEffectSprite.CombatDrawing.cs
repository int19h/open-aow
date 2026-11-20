#nullable disable
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.PCharStrings;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Draw;

public partial class BattleEffectSprite
{
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        ImageSequence sequence;
        void SelectSequence(int number)
        {
            sequence = AbilityIndex == 52
                ? GamePack.Current.SpellControl[SpellIndex].CastImages.ImageSequences[number]
                : GamePack.Current.AbilityControl[AbilityIndex].ImageSequences.ImageSequences[number];
        }

        int index;
        ImageSequence firstSequence;
        void SelectFrame()
        {
            if (sequence is not null)
            {
                index = sequence.GetUnloopedFrameIndex(Frame);
            }

            firstSequence = sequence;
        }

        void SelectDamageSequence()
        {
            int n = EffectType switch
            {
                BattleEffectType.DamLightning => 40,
                BattleEffectType.DamFire => 41,
                BattleEffectType.DamPoison => 42,
                BattleEffectType.DamMagic => 43,
                BattleEffectType.DamHoly => 44,
                BattleEffectType.DamDeath => 45,
                BattleEffectType.None or BattleEffectType.Arrow or BattleEffectType.Rock or BattleEffectType.Comet or BattleEffectType.Touch or BattleEffectType.Breath or BattleEffectType.CometBall or BattleEffectType.Effect or BattleEffectType.FBExplosion or BattleEffectType.ScreenNESW or BattleEffectType.ScreenNS or BattleEffectType.Ray or BattleEffectType.Chain or BattleEffectType.Swarm or BattleEffectType.ScreenSwarm or BattleEffectType.SFOverlay or BattleEffectType.DamCold or BattleEffectType.SacredWrath or BattleEffectType.SWComet or BattleEffectType.Dispel or BattleEffectType.WallSmoke or BattleEffectType.Blood or BattleEffectType.AltArrow or BattleEffectType.Tremors or BattleEffectType.Geyser or BattleEffectType.GeyserWater or BattleEffectType.HailImpact or BattleEffectType.Cloud or BattleEffectType.CloudPuff or BattleEffectType.HitGround or BattleEffectType.HitWater or BattleEffectType.HighPrayer or BattleEffectType.PrayerGlow or BattleEffectType.Resurrect or BattleEffectType.FrostBeam or BattleEffectType.Frost or BattleEffectType.MindDecay or BattleEffectType.DecayLeg or BattleEffectType.ShockWave or BattleEffectType.DecayComet or BattleEffectType.SacFlame or BattleEffectType.Winds or BattleEffectType.SacFire or BattleEffectType.BreathDam or _ => 46,
            };
            sequence = GamePack.Current.ImageSequences.ImageSequences[n];
        }

        void ShowMovingLoop()
        {
            while (sequence is not null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    Point3D pathY = Path[Frame];
                    Point3D pathZ = Path[Frame];
                    Point3D pathX = Path[Frame];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowSecondaryFrame()
        {
            while (sequence is not null)
            {
                index = sequence.GetUnloopedFrameIndex(Variant);
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    Point3D pathY = Path[Frame];
                    Point3D pathZ = Path[Frame];
                    Point3D pathX = Path[Frame];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowTrail()
        {
            if (sequence is not null)
            {
                int lastFrame = sequence.TotalSequenceFrames - 1;
                for (int n = 0; n <= lastFrame; n++)
                {
                    int half = n / 2;
                    bool condition = Frame - 9 + half + 5 > -1;
                    if (condition)
                    {
                        int count = Path.Count;
                        condition = Frame - 9 + half + 5 < count;
                    }

                    if (condition)
                    {
                        index = sequence.GetUnloopedFrameIndex(lastFrame - n);
                        if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                        {
                            int count = Path.Count;
                            if (Frame - 9 + half + 5 == count - 1 || half * 2 == n)
                            {
                                Point3D pathY = Path[Frame - 9 + half + 5];
                                Point3D pathZ = Path[Frame - 9 + half + 5];
                                Point3D pathX = Path[Frame - 9 + half + 5];
                                sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                            }
                            else
                            {
                                Point3D pathY = Path[Frame - 9 + half + 5];
                                Point3D pathZ = Path[Frame - 9 + half + 5];
                                Point3D path = Path[Frame - 9 + half + 6];
                                Point3D path2 = Path[Frame - 9 + half + 6];
                                Point3D pathX = Path[Frame - 9 + half + 5];
                                Point3D path3 = Path[Frame - 9 + half + 6];
                                sequence.ImageLibrary[index].ShowClipped(((pathX.X + path3.X) / 2) - pixelXSurface + sequence.OffsetX, ((pathY.Y - pathZ.Z + path.Y - path2.Z) / 2) - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                            }
                        }
                    }
                }
            }
        }

        void ShowRay()
        {
            if (sequence is not null)
            {
                for (int n = 0, last = Frame; n <= last; n++)
                {
                    index = sequence.GetUnloopedFrameIndex(0);
                    if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                    {
                        Point3D pathY = Path[n];
                        Point3D pathZ = Path[n];
                        Point3D pathX = Path[n];
                        sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                    }
                }
            }
        }

        void ShowFrostBeam()
        {
            switch (Frame)
            {
                case >= 0 and <= 1999:
                    SelectSequence(0);
                    break;
                default:
                    SelectSequence(1);
                    break;
            }

            if (sequence is not null)
            {
                int n = 0;
                while (n <= Frame && n < Path.Count)
                {
                    int alpha;
                    switch (Frame)
                    {
                        case >= 0 and <= 1999:
                            alpha = 25;
                            // { Original uses the object address as the animation phase. }
                            int totalSequenceFrames = sequence.TotalSequenceFrames;
                            index = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, totalSequenceFrames));
                            break;
                        case >= 2000 and <= 2998:
                            if (n <= Frame - 2000)
                            {
                                int count = Path.Count;
                                alpha = ((Frame - 2000) * 75 / (count - 1)) + 25;
                                index = sequence.GetUnloopedFrameIndex(0);
                            }
                            else
                            {
                                if (n == Frame - 1999)
                                {
                                    SelectSequence(0);
                                }

                                alpha = 25;
                                int sequenceTotalSequenceFrames = sequence.TotalSequenceFrames;
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
                        Point3D pathY = Path[n];
                        Point3D pathZ = Path[n];
                        Point3D pathX = Path[n];
                        sequence.ImageLibrary[index].ShowBlended(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[alpha]);
                    }

                    n += 1;
                }
            }
        }

        void ShowAtStart()
        {
            while (sequence is not null)
            {
                index = sequence.GetUnloopedFrameIndex(Frame);
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    Point3D pathY = Path[0];
                    Point3D pathX = Path[0];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowAtStartLooped()
        {
            while (sequence is not null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    Point3D pathY = Path[0];
                    Point3D pathX = Path[0];
                    sequence.ImageLibrary[index].ShowClipped(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pixelYSurface + sequence.OffsetY, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowScreen()
        {
            while (sequence is not null)
            {
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    LibraryImage imageLibraryHeight = sequence.ImageLibrary[index];
                    Point3D pathY = Path[0];
                    LibraryImage imageLibrary = sequence.ImageLibrary[index];
                    LibraryImage imageLibraryWidth = sequence.ImageLibrary[index];
                    Point3D pathX = Path[0];
                    LibraryImage imageLibrary2 = sequence.ImageLibrary[index];
                    sequence.ImageLibrary[index].ShowClipped(((imageLibraryWidth.Width + lockedSurfaceValue.Width) * pathX.X / 100) - imageLibrary2.Width, ((imageLibraryHeight.Height + lockedSurfaceValue.Height) * pathY.Y / 100) - imageLibrary.Height, lockedSurfaceValue);
                }

                sequence = sequence.Next;
            }
        }

        void ShowScreenSwarm()
        {
            int alpha = ((100 - Path[Frame].Z) / 2) - FrameFixtureInputs.ShimmerOffset(this, Frame);
            if (alpha < 0)
            {
                alpha = 0;
            }

            while (sequence is not null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                {
                    Point3D pathX = Path[0];
                    Point3D pathZ = Path[Frame];
                    Point3D path = Path[0];
                    Point3D path2 = Path[Frame];
                    Point3D pathY = Path[0];
                    Point3D path3 = Path[Frame];
                    Point3D path4 = Path[Frame];
                    sequence.ImageLibrary[index].ShowBlended(((pixelXSurface + lockedSurfaceValue.Width - pathX.X + 100) * pathZ.Z / 100) + (path.X - pixelXSurface) + path2.X, ((pathY.Y - pixelYSurface + 100) * (100 - path3.Z) / 100) + path4.Y - 100, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[alpha]);
                }

                sequence = sequence.Next;
            }
        }

        void ShowParticle(bool shadow)
        {
            int count = Path.Count;
            int alpha = 100 - (Frame * 100 / count) - FrameFixtureInputs.ShimmerOffset(this, Frame);
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

            while (sequence is not null)
            {
                // { Original uses the object address rather than the current frame. }
                index = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (index != -1)
                {
                    if (shadow)
                    {
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            Point3D pathY = Path[Frame];
                            Point3D pathZ = Path[Frame];
                            Point3D pathX = Path[Frame];
                            sequence.ImageLibrary[index].ShowBlended(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue, GraphicsState.ShadowBlendTable.LookupTables[alpha]);
                        }
                    }
                    else
                    {
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            Point3D pathY = Path[Frame];
                            Point3D pathZ = Path[Frame];
                            Point3D pathX = Path[Frame];
                            sequence.ImageLibrary[index].ShowBlended(pathX.X - pixelXSurface + sequence.OffsetX, pathY.Y - pathZ.Z - pixelYSurface + sequence.OffsetY, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[alpha]);
                        }
                    }
                }

                sequence = sequence.Next;
            }
        }

        void FinishPath()
        {
            int count = Path.Count;
            if (Frame >= count - 1)
            {
                Done = true;
                Frame = -1;
            }
        }

        void FinishSequence()
        {
            bool condition = firstSequence is null;
            if (!condition)
            {
                bool test = firstSequence is not null;
                if (test)
                {
                    int totalSequenceFrames = firstSequence.TotalSequenceFrames;
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
                int i;
                int j;
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
                        if (sequence is not null)
                        {
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                        }

                        break;
                    case BattleEffectType.SFOverlay:
                        SelectSequence(2);
                        if (sequence is not null)
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
                        int count = Path.Count;
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
                    case BattleEffectType.DamLightning:
                    case BattleEffectType.DamFire:
                    case BattleEffectType.DamPoison:
                    case BattleEffectType.DamMagic:
                    case BattleEffectType.DamHoly:
                    case BattleEffectType.DamDeath:
                    case BattleEffectType.DamCold:
                        SelectDamageSequence();
                        ShowParticle(EffectType == BattleEffectType.DamDeath);
                        FinishPath();
                        break;
                    case BattleEffectType.SacredWrath:
                        if (Set<byte>.WithBounds(0, 255).WithRange(5, 6).Contains((byte)Frame))
                        {
                            SelectSequence(2);
                            while (sequence is not null)
                            {
                                index = sequence.GetUnloopedFrameIndex(0);
                                if (index != -1 && sequence.ImageLibrary.ImageExist(index))
                                {
                                    i = 0;
                                    int last = lockedSurfaceValue.Width / sequence.ImageLibrary[index].Width;
                                    for (; i <= last; i += 1)
                                    {
                                        j = 0;
                                        int limit = lockedSurfaceValue.Height / sequence.ImageLibrary[index].Height;
                                        for (; j <= limit; j += 1)
                                        {
                                            LibraryImage imageLibraryHeight = sequence.ImageLibrary[index];
                                            LibraryImage imageLibraryWidth = sequence.ImageLibrary[index];
                                            sequence.ImageLibrary[index].ShowClipped(imageLibraryWidth.Width * i, imageLibraryHeight.Height * j, lockedSurfaceValue);
                                        }
                                    }
                                }

                                sequence = sequence.Next;
                            }
                        }

                        SelectSequence(1);
                        if (sequence is not null)
                        {
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                        }

                        break;
                    case BattleEffectType.SWComet:
                        sbyte hexY = Target.UnitHexSprite.HexY;
                        HexGeometry.HexToPixel(Target.UnitHexSprite.HexX, hexY, out i, out j);
                        Point3DList path5 = Path;
                        EffectPaths.MakeArcPathXY((short)((lockedSurfaceValue.Width / 2) + pixelXSurface), (short)((lockedSurfaceValue.Height / 2) + pixelYSurface), (short)i, (short)j, 25, 50, path5);
                        Path = path5;
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
                        Point3D path = Path[0];
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
                        ShowParticle(SpellIndex is 130 or 106);
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
                    case BattleEffectType.None:
                    case BattleEffectType.Dispel:
                    case BattleEffectType.Tremors:
                    case BattleEffectType.Cloud:
                    case BattleEffectType.HighPrayer:
                    case BattleEffectType.Resurrect:
                    case BattleEffectType.MindDecay:
                    case BattleEffectType.ShockWave:
                    case BattleEffectType.SacFlame:
                    case BattleEffectType.Winds:
                    case BattleEffectType.BreathDam:
                    default:
                        break;
                }
            }
        }
        catch
        {
            // { Original puts the diagnostic in the message-box caption. }
            _ = MessageBox(0, "Error during Show", NullTerminatedText($"Error - fxHS {IntToStr(AbilityIndex)} {IntToStr(SpellIndex)}"), 0);
        }
    }
}
