#nullable disable
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Game.Draw.aowFXUnit;
using static Aow.Game.Draw.GFXEUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.World.Map.HSEngineUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class TfxHS
{
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TSurfaceDesc SurfaceDesc__value = SurfaceDesc;
        TImageSequence Sequence;
        int Index;
        TImageSequence FirstSequence;
        int I = default;
        int J = default;
        void SelectSequence(int Number)
        {
            if (FAbilityIndex == 52)
            {
                Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[Number];
            }
            else
            {
                Sequence = AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[Number];
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
                case TfxType.fxDamLightning:
                    N = 40;
                    break;
                case TfxType.fxDamFire:
                    N = 41;
                    break;
                case TfxType.fxDamPoison:
                    N = 42;
                    break;
                case TfxType.fxDamMagic:
                    N = 43;
                    break;
                case TfxType.fxDamHoly:
                    N = 44;
                    break;
                case TfxType.fxDamDeath:
                    N = 45;
                    break;
                default:
                    N = 46;
                    break;
            }

            Sequence = AoWHSSet.FImageSequences.ImageSequences[N];
        }

        void ShowMovingLoop()
        {
            while (Sequence != null)
            {
                Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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
                        Sequence.ImageLibrary[Index].ShowBlended(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value, IntensityBlendTable.LookupTables[Alpha]);
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
                Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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
                Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Index != -1 && Sequence.ImageLibrary.ImageExist(Index))
                {
                    var pathX = FPath[0];
                    var pathZ = FPath[FFrame];
                    var path = FPath[0];
                    var path2 = FPath[FFrame];
                    var pathY = FPath[0];
                    var path3 = FPath[FFrame];
                    var path4 = FPath[FFrame];
                    Sequence.ImageLibrary[Index].ShowBlended((XhpSurf + SurfaceDesc__value.Width - pathX.X + 100) * pathZ.Z / 100 + (path.X - XhpSurf) + path2.X, (pathY.Y - YhpSurf + 100) * (100 - path3.Z) / 100 + path4.Y - 100, SurfaceDesc__value, IntensityBlendTable.LookupTables[Alpha]);
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
                            Sequence.ImageLibrary[Index].ShowBlended(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value, ShadowBlendTable.LookupTables[Alpha]);
                        }
                    }
                    else
                    {
                        if (Sequence.ImageLibrary.ImageExist(Index))
                        {
                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            var pathX = FPath[FFrame];
                            Sequence.ImageLibrary[Index].ShowBlended(pathX.X - XhpSurf + Sequence.dXshift, pathY.Y - pathZ.Z - YhpSurf + Sequence.dYshift, SurfaceDesc__value, IntensityBlendTable.LookupTables[Alpha]);
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
                    case TfxType.fxArrow:
                        SelectSequence(FVariant);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                    case TfxType.fxAltArrow:
                        SelectSequence(0);
                        ShowSecondaryFrame();
                        SelectSequence(1);
                        ShowTrail();
                        FinishPath();
                        break;
                    case TfxType.fxRock:
                        SelectSequence(0);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                    case TfxType.fxBreath:
                        SelectSequence(0);
                        ShowTrail();
                        FinishPath();
                        break;
                    case TfxType.fxCometBall:
                    case TfxType.fxScreenNS:
                        SelectSequence(0);
                        ShowMovingLoop();
                        SelectSequence(1);
                        ShowTrail();
                        FinishPath();
                        break;
                    case TfxType.fxComet:
                    case TfxType.fxDecayComet:
                        SelectSequence(0);
                        ShowTrail();
                        FinishPath();
                        break;
                    case TfxType.fxTouch:
                    case TfxType.fxEffect:
                        SelectSequence(0);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxFBExplosion:
                        SelectSequence(1);
                        if (Sequence != null)
                        {
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                        }

                        break;
                    case TfxType.fxSFOverlay:
                        SelectSequence(2);
                        if (Sequence != null)
                        {
                            Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            ShowAtStart();
                            FinishPath();
                        }

                        break;
                    case TfxType.fxScreenNESW:
                        SelectSequence(FrameFixtureInputs.PhaseModulo(this, 3));
                        Index = Sequence.GetUnloopedFrameIndex(0);
                        ShowScreen();
                        break;
                    case TfxType.fxRay:
                        SelectSequence(0);
                        ShowRay();
                        FinishPath();
                        break;
                    case TfxType.fxFrostBeam:
                        ShowFrostBeam();
                        break;
                    case TfxType.fxChain:
                        SelectSequence(0);
                        ShowRay();
                        var count = FPath.Count;
                        if (FFrame >= count - 1 && FPath[FFrame].Z == 1)
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxSwarm:
                        SelectSequence(0);
                        ShowMovingLoop();
                        FinishPath();
                        break;
                    case TfxType.fxScreenSwarm:
                        SelectSequence(0);
                        ShowScreenSwarm();
                        FinishPath();
                        break;
                    case >= TfxType.fxDamLightning and <= TfxType.fxDamCold:
                        SelectDamageSequence();
                        ShowParticle(FEffectType == TfxType.fxDamDeath);
                        FinishPath();
                        break;
                    case TfxType.fxSacredWrath:
                        if (TSet<byte>.WithBounds(0, 255).WithRange(5, 6).Contains((byte)FFrame))
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
                    case TfxType.fxSWComet:
                        var yhx = ((TTacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                        HXtoHP(((TTacticalCombatUnit)FTarget).FUnitHS.GetXhx(), yhx, ref I, ref J);
                        MakeArcPathXY((short)(SurfaceDesc__value.Width / 2 + XhpSurf), (short)(SurfaceDesc__value.Height / 2 + YhpSurf), (short)I, (short)J, 25, 50, ref FPath);
                        FEffectType = TfxType.fxComet;
                        SelectSequence(0);
                        ShowTrail();
                        break;
                    case TfxType.fxWallSmoke:
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[5];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxHitGround:
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[9];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxHitWater:
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[10];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxBlood:
                        var path = FPath[0];
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[path.Z];
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxGeyser:
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
                    case TfxType.fxHailImpact:
                        SelectSequence(2);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxPrayerGlow:
                        SelectSequence(1);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxCloudPuff:
                        SelectSequence(0);
                        SelectFrame();
                        ShowAtStart();
                        FinishSequence();
                        break;
                    case TfxType.fxGeyserWater:
                        SelectSequence(0);
                        ShowParticle(FSpellIndex == 130 || FSpellIndex == 106);
                        FinishPath();
                        break;
                    case TfxType.fxFrost:
                        SelectSequence(1);
                        ShowParticle(FSpellIndex == 130);
                        FinishPath();
                        break;
                    case TfxType.fxDecayLeg:
                        SelectSequence(0);
                        ShowRay();
                        FCancelled = true;
                        break;
                    case TfxType.fxSacFire:
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

public partial class TTacticalCombatUnitHS
{
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int Index;
        int X = default;
        int Y = default;
        int DX = default;
        int DY = default;
        double Progress;
        int NX = default;
        int NY = default;
        int FlyingOffset;
        int Race;
        TCityWall Wall;
        TCityWall NextWall;
        TSurfaceDesc SD;
        try
        {
            if (!(FDirection < FUnitGFXResource.FSequences.Count))
            {
                AoWTCUnit.OriginalUnitAssert("Invalid Unit Index", "D:\\AoWDev\\AoWTC\\inc\\TCUnitHS.pas", 4262);
            }

            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
            XhpSurf = X - XhpSurf + FOffsetX;
            YhpSurf = Y - YhpSurf - 8 + FOffsetY;
            if (FUnit.GetPlayer() > 0)
            {
                var player = FUnit.GetPlayer();
                Race = AoWHSMap.FPlayers.GetPlayers(player).FPlayerRace;
            }
            else
            {
                Race = 32;
            }

            if (FUnit.GetAbilityEnabled(1))
            {
                FlyingOffset = -8;
            }
            else
            {
                FlyingOffset = 0;
            }

            if (FUnit.GetAbilityEnabled(36))
            {
                Wall = (TCityWall)Owner.FindOwnedChild(2229508);
                if (Wall == null)
                {
                    Wall = (TCityWall)Owner.FindOwnedChild(2229510);
                }

                if (FMoveStep == -1)
                {
                    if (Wall != null && Wall.FHitPoints > 0)
                    {
                        YhpSurf -= 32;
                    }
                }
                else
                {
                    var yhx = GetYhx();
                    CenterHNtoHX(GetXhx(), yhx, FDirection, ref NX, ref NY);
                    NextWall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229508);
                    if (NextWall == null)
                    {
                        NextWall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229510);
                    }

                    var moveSteps = GetMoveSteps();
                    if (FMoveStep < moveSteps / 2)
                    {
                        if (Wall != null && Wall.FHitPoints > 0)
                        {
                            if (NextWall != null && NextWall.FHitPoints > 0)
                            {
                                YhpSurf -= 32;
                            }
                            else
                            {
                                var moveSteps2 = GetMoveSteps();
                                var moveSteps3 = GetMoveSteps();
                                YhpSurf -= (moveSteps2 - FMoveStep) * 32 / moveSteps3;
                            }
                        }
                    }
                    else
                    {
                        if (NextWall != null && NextWall.FHitPoints > 0)
                        {
                            if (Wall != null && Wall.FHitPoints > 0)
                            {
                                YhpSurf -= 32;
                            }
                            else
                            {
                                var moveSteps2 = GetMoveSteps();
                                YhpSurf -= FMoveStep * 32 / moveSteps2;
                            }
                        }
                    }
                }
            }

            switch (FAction)
            {
                case TTCAction.caMove:
                case TTCAction.caBusy:
                case TTCAction.caFreeStruck:
                    if (FAction == TTCAction.caMove && FMoveStep != -1)
                    {
                        HNtoHP(FDirection, ref DX, ref DY);
                        var moveSteps = GetMoveSteps();
                        Progress = (double)FMoveStep / moveSteps;
                        XhpSurf += Round((double)DX * Progress);
                        YhpSurf += Round((double)DY * Progress);
                        Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                    }
                    else
                    {
                        Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection - 1];
                    }

                    while (Sequence != null)
                    {
                        Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Index != -1)
                        {
                            Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FlyingOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }

                    break;
                case TTCAction.caMeleeHit:
                case TTCAction.caMeleeMiss:
                case TTCAction.caAbility:
                case TTCAction.caBreathAttack:
                    Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    if (Sequence != null)
                    {
                        while (Sequence != null)
                        {
                            Index = Sequence.GetUnloopedFrameIndex(FActionFrame);
                            if (Index != -1)
                            {
                                Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FlyingOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    break;
                case TTCAction.caHit:
                    Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                    if (Sequence != null)
                    {
                        while (Sequence != null)
                        {
                            Index = Sequence.GetUnloopedFrameIndex(FActionFrame);
                            if (Index != -1)
                            {
                                Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FlyingOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    break;
                case TTCAction.caStepUp:
                case TTCAction.caStepBack:
                    Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                    if (Sequence != null)
                    {
                        while (Sequence != null)
                        {
                            Index = Sequence.GetFrameIndex(AoWHSMap.Frame + FActionFrame, FrameFixtureInputs.AnimationOffset(this));
                            if (Index != -1)
                            {
                                Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FlyingOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    break;
                case TTCAction.caRoundAttack:
                case TTCAction.caRoundAttackHit:
                    Sequence = AoWHSSet.FImageSequences.ImageSequences[FDirection + 15];
                    if (Sequence != null)
                    {
                        while (Sequence != null)
                        {
                            Index = Sequence.GetUnloopedFrameIndex(FActionFrame);
                            if (Index != -1)
                            {
                                if (Sequence.Layer == 0)
                                {
                                    while (!FUnitGFXResource.FSequences.ImageSequences[FDirection + 11].ImageLibrary.ImageExist(Index))
                                    {
                                        Index -= 1;
                                    }

                                    FUnitGFXResource.FSequences.ImageSequences[FDirection + 11].ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift + 16, YhpSurf + FlyingOffset + Sequence.dYshift + 16, SurfaceDesc);
                                }
                                else
                                {
                                    Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift + 16, YhpSurf + Sequence.dYshift + FlyingOffset + 16, SurfaceDesc);
                                }
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    break;
            }

            if (FUnit.FUnit.FUnitStatus.Contains(TUnitStatusElement.usEnchanted))
            {
                AoWHSSet.FImageSequences.ShowLooped(70, XhpSurf + 16, YhpSurf + 18 + FlyingOffset, AoWHSMap.Frame, SurfaceDesc);
            }

            NX = 0;
            for (; NX <= 13; NX += 1)
            {
                if (FUnit.GetAbilityEnabled(AoWTCUnit.StatusAbils[NX]))
                {
                    Sequence = AoWHSSet.FAbilityControl.GetAbility(AoWTCUnit.StatusAbils[NX]).FImageSequences.ImageSequences[0];
                    while (Sequence != null)
                    {
                        Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Index != -1)
                        {
                            Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift + 15, YhpSurf + Sequence.dYshift + 10 + FlyingOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                }
            }

            if (!GetSelected() || (AoWTCUnit.AoWCombatMap.Frame & 6) != 6)
            {
                if (Race == 32)
                {
                    if (FUnit.FMovePoints == 0 || FUnit.FCombatPlayer != AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer)
                    {
                        AoWHSSet.FFlagControl.FFlags[70].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                    else
                    {
                        AoWHSSet.FFlagControl.FFlags[FMoveIndicator + 69].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                }
                else
                {
                    if (FUnit.FMovePoints == 0 || FUnit.FCombatPlayer != AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer)
                    {
                        AoWHSSet.FRaceResources.GetRaceResource(Race).FImageLibrary[70].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                    else
                    {
                        AoWHSSet.FRaceResources.GetRaceResource(Race).FImageLibrary[FMoveIndicator + 69].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                }

                SD = SurfaceDesc;
                NX = FUnit.GetHitPoints() * 10 / FUnit.GetHits();
                if (NX == 0)
                {
                    NX = 1;
                }

                if (XhpSurf - 4 + NX < SD.ClipRect.Right)
                {
                    SD.ClipRect.Right = XhpSurf - 4 + NX;
                }

                AoWHSSet.FCombatImages[1].ShowClipped(XhpSurf - 4, YhpSurf - 4, SD);
            }
        }
        catch
        {
            MessageBox(0, "Error during ShowUnit", NullTerminatedText("Error - " + FUnit.FUnit.GetName()), 0);
        }
    }

    public override void ShowShadow(int X, int Y, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        float Progress;
        int PX = default;
        int PY = default;
        int Index;
        int DX = default;
        int DY = default;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref PX, ref PY);
        X = PX - X + FOffsetX;
        if (FBusy)
        {
            Y = PY - Y - 8;
        }
        else
        {
            Y = PY - Y - 8 + FOffsetY;
        }

        if (FMoveStep != -1)
        {
            HNtoHP(FDirection, ref DX, ref DY);
            var moveSteps = GetMoveSteps();
            Progress = (float)((double)FMoveStep / moveSteps);
            X += Round((double)DX * Progress);
            Y += Round((double)DY * Progress);
        }

        Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 23];
        while (Sequence != null)
        {
            Index = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
            if (Index != -1)
            {
                Sequence.ImageLibrary[Index].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    public void NewFrame(object Sender)
    {
        TImageSequence Sequence;
        if (FActionDone)
        {
            SetAction(TTCAction.caMove);
        }

        if (FMoveIndicator == 0)
        {
            FMoveIndicator = (short)(Random(5) + 1);
            FIndicatorStep = 1;
        }
        else
        {
            FMoveIndicator = (short)(FMoveIndicator + FIndicatorStep);
            if (FMoveIndicator == 1)
            {
                FIndicatorStep = 1;
            }
            else
            {
                if (FMoveIndicator == 6)
                {
                    FIndicatorStep = -1;
                }
            }
        }

        switch (FAction)
        {
            case TTCAction.caStepUp:
                var moveSteps = GetMoveSteps();
                if (FActionFrame <= moveSteps)
                {
                    HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                    var moveSteps2 = GetMoveSteps();
                    FOffsetX = FActionFrame * FOffsetX / (moveSteps2 * 2);
                    var moveSteps3 = GetMoveSteps();
                    FOffsetY = FActionFrame * FOffsetY / (moveSteps3 * 2);
                    FActionFrame = (short)(FActionFrame + 2);
                }
                else
                {
                    HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                    FOffsetX = FOffsetX / 2;
                    FOffsetY = FOffsetY / 2;
                    FActionDone = true;
                }

                break;
            case TTCAction.caStepBack:
                var moveSteps4 = GetMoveSteps();
                if (FActionFrame <= moveSteps4)
                {
                    HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                    var moveSteps2 = GetMoveSteps();
                    var moveSteps3 = GetMoveSteps();
                    FOffsetX = (moveSteps2 - FActionFrame) * FOffsetX / (moveSteps3 * 2);
                    var moveSteps5 = GetMoveSteps();
                    var moveSteps6 = GetMoveSteps();
                    FOffsetY = (moveSteps5 - FActionFrame) * FOffsetY / (moveSteps6 * 2);
                    FActionFrame = (short)(FActionFrame + 2);
                }
                else
                {
                    FOffsetX = 0;
                    FOffsetY = 0;
                    FActionDone = true;
                }

                break;
            case TTCAction.caMeleeHit:
            case TTCAction.caMeleeMiss:
            case TTCAction.caAbility:
                Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                if (Sequence != null)
                {
                    FActionFrame = (short)(FActionFrame + 1);
                    var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                    if (FActionFrame >= totalSequenceFrames - 1)
                    {
                        FActionFrame = (short)(Sequence.GetTotalSequenceFrames() - 1);
                        FActionDone = true;
                    }
                }

                break;
            case TTCAction.caBreathAttack:
                Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                if (Sequence != null)
                {
                    var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                    if (FActionFrame < totalSequenceFrames - 1)
                    {
                        FActionFrame = (short)(FActionFrame + 1);
                    }
                    else
                    {
                        FActionDone = true;
                    }
                }

                break;
            case TTCAction.caHit:
                Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                if (Sequence != null)
                {
                    FActionFrame = (short)(FActionFrame + 1);
                    var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                    if (FActionFrame >= totalSequenceFrames - 1)
                    {
                        FActionFrame = (short)(Sequence.GetTotalSequenceFrames() - 1);
                        FActionDone = true;
                    }
                }

                break;
            case TTCAction.caRoundAttack:
            case TTCAction.caRoundAttackHit:
                Sequence = AoWHSSet.FImageSequences.ImageSequences[FDirection + 16];
                if (Sequence != null)
                {
                    FActionFrame = (short)(FActionFrame + 1);
                    var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                    if (FActionFrame >= totalSequenceFrames - 1)
                    {
                        FActionFrame = (short)(Sequence.GetTotalSequenceFrames() - 1);
                        FActionDone = true;
                    }
                }

                break;
            default:
                FActionFrame = -1;
                break;
        }

        switch (FAction)
        {
            case TTCAction.caMove:
            case TTCAction.caStepUp:
            case TTCAction.caStepBack:
                if (FMoveStep != -1)
                {
                    Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                    if (Sequence != null && Sequence.SpeedCount == Sequence.SequenceSpeed && AoWTCUnit.AoWCombatMap.FPlaySounds)
                    {
                        var frameIndex = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        FUnitGFXResource.FSFXLibraries[1].Play(frameIndex % 100);
                    }
                }

                break;
            case TTCAction.caMeleeHit:
            case TTCAction.caMeleeMiss:
            case TTCAction.caAbility:
            case TTCAction.caBreathAttack:
                if (FActionFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds)
                {
                    if (FSelectedAbility > -1)
                    {
                        if (FSelectedAbility == 52)
                        {
                            if (FSelectedSpell != 112 && FSelectedSpell != 122 && FSelectedSpell != 119)
                            {
                                AoWHSSet.FSpellControl.GetSpell(FSelectedSpell).FCastSounds.Play(0);
                            }
                        }
                        else
                        {
                            AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility).FSFXLibrary.Play(0);
                        }
                    }
                    else
                    {
                        if (FAction == TTCAction.caMeleeMiss)
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
            case TTCAction.caHit:
            case TTCAction.caKilled:
                if (FActionFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds)
                {
                    if (FAction == TTCAction.caHit)
                    {
                        FUnitGFXResource.FSFXLibraries[3].Play(0);
                    }
                    else
                    {
                        FUnitGFXResource.FSFXLibraries[4].Play(0);
                    }
                }

                break;
            case TTCAction.caRoundAttack:
            case TTCAction.caRoundAttackHit:
                if (FActionFrame < 18 && AoWTCUnit.AoWCombatMap.FPlaySounds && AoWTCUnit.raDir[FActionFrame] > 0)
                {
                    if (FAction == TTCAction.caRoundAttack)
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
