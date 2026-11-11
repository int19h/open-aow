#nullable disable
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class TacticalBattleUnitHexSprite
{
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        ImageSequence Sequence;
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
        CityWall Wall;
        CityWall NextWall;
        LockedSurface SD;
        try
        {
            if (!(FDirection < FUnitGFXResource.FSequences.Count))
            {
                OriginalUnitAssert("Invalid Unit Index", "D:\\AoWDev\\AoWTC\\inc\\TCUnitHS.pas", 4262);
            }

            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
            XhpSurf = X - XhpSurf + FOffsetX;
            YhpSurf = Y - YhpSurf - 8 + FOffsetY;
            if (FUnit.GetPlayer() > 0)
            {
                var player = FUnit.GetPlayer();
                Race = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerRace;
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
                Wall = (CityWall)Owner.FindOwnedChild(2229508);
                if (Wall == null)
                {
                    Wall = Owner.FindOwnedChild(2229510) as CityWall;
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
                    HexGeometry.CenterHNtoHX(GetXhx(), yhx, FDirection, ref NX, ref NY);
                    NextWall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229508);
                    if (NextWall == null)
                    {
                        NextWall = CombatMap.Current.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229510) as CityWall;
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
                case SpriteAction.Move:
                case SpriteAction.Busy:
                case SpriteAction.FreeStruck:
                    if (FAction == SpriteAction.Move && FMoveStep != -1)
                    {
                        HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
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
                        Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Index != -1)
                        {
                            Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FlyingOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }

                    break;
                case SpriteAction.MeleeHit:
                case SpriteAction.MeleeMiss:
                case SpriteAction.Ability:
                case SpriteAction.BreathAttack:
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
                case SpriteAction.Hit:
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
                case SpriteAction.StepUp:
                case SpriteAction.StepBack:
                    Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                    if (Sequence != null)
                    {
                        while (Sequence != null)
                        {
                            Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame + FActionFrame, FrameFixtureInputs.AnimationOffset(this));
                            if (Index != -1)
                            {
                                Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FlyingOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    break;
                case SpriteAction.RoundAttack:
                case SpriteAction.RoundAttackHit:
                    Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[FDirection + 15];
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

            if (FUnit.FUnit.FUnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                AoWHSSet.Current.FImageSequences.ShowLooped(70, XhpSurf + 16, YhpSurf + 18 + FlyingOffset, AoWHSMap.Current.Frame, SurfaceDesc);
            }

            NX = 0;
            for (; NX <= 13; NX += 1)
            {
                if (FUnit.GetAbilityEnabled(StatusAbils[NX]))
                {
                    Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(StatusAbils[NX]).FImageSequences.ImageSequences[0];
                    while (Sequence != null)
                    {
                        Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Index != -1)
                        {
                            Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift + 15, YhpSurf + Sequence.dYshift + 10 + FlyingOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                }
            }

            if (!GetSelected() || (CombatMap.Current.Frame & 6) != 6)
            {
                if (Race == 32)
                {
                    if (FUnit.FMovePoints == 0 || FUnit.FCombatPlayer != CombatMap.Current.FSeatedCombatPlayer)
                    {
                        AoWHSSet.Current.FFlagControl.FFlags[70].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                    else
                    {
                        AoWHSSet.Current.FFlagControl.FFlags[FMoveIndicator + 69].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                }
                else
                {
                    if (FUnit.FMovePoints == 0 || FUnit.FCombatPlayer != CombatMap.Current.FSeatedCombatPlayer)
                    {
                        AoWHSSet.Current.FRaceResources.GetRaceResource(Race).FImageLibrary[70].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                    }
                    else
                    {
                        AoWHSSet.Current.FRaceResources.GetRaceResource(Race).FImageLibrary[FMoveIndicator + 69].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
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

                AoWHSSet.Current.FCombatImages[1].ShowClipped(XhpSurf - 4, YhpSurf - 4, SD);
            }
        }
        catch
        {
            MessageBox(0, "Error during ShowUnit", NullTerminatedText("Error - " + FUnit.FUnit.GetName()), 0);
        }
    }

    public override void ShowShadow(int X, int Y, in LockedSurface SurfaceDesc)
    {
        ImageSequence Sequence;
        float Progress;
        int PX = default;
        int PY = default;
        int Index;
        int DX = default;
        int DY = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref PX, ref PY);
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
            HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
            var moveSteps = GetMoveSteps();
            Progress = (float)((double)FMoveStep / moveSteps);
            X += Round((double)DX * Progress);
            Y += Round((double)DY * Progress);
        }

        Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 23];
        while (Sequence != null)
        {
            Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
            if (Index != -1)
            {
                Sequence.ImageLibrary[Index].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    public void NewFrame(object Sender)
    {
        ImageSequence Sequence;
        if (FActionDone)
        {
            SetAction(SpriteAction.Move);
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
            case SpriteAction.StepUp:
                var moveSteps = GetMoveSteps();
                if (FActionFrame <= moveSteps)
                {
                    HexGeometry.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                    var moveSteps2 = GetMoveSteps();
                    FOffsetX = FActionFrame * FOffsetX / (moveSteps2 * 2);
                    var moveSteps3 = GetMoveSteps();
                    FOffsetY = FActionFrame * FOffsetY / (moveSteps3 * 2);
                    FActionFrame = (short)(FActionFrame + 2);
                }
                else
                {
                    HexGeometry.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                    FOffsetX = FOffsetX / 2;
                    FOffsetY = FOffsetY / 2;
                    FActionDone = true;
                }

                break;
            case SpriteAction.StepBack:
                var moveSteps4 = GetMoveSteps();
                if (FActionFrame <= moveSteps4)
                {
                    HexGeometry.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
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
            case SpriteAction.MeleeHit:
            case SpriteAction.MeleeMiss:
            case SpriteAction.Ability:
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
            case SpriteAction.BreathAttack:
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
            case SpriteAction.Hit:
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
            case SpriteAction.RoundAttack:
            case SpriteAction.RoundAttackHit:
                Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[FDirection + 16];
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
            case SpriteAction.Move:
            case SpriteAction.StepUp:
            case SpriteAction.StepBack:
                if (FMoveStep != -1)
                {
                    Sequence = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                    if (Sequence != null && Sequence.SpeedCount == Sequence.SequenceSpeed && CombatMap.Current.FPlaySounds)
                    {
                        var frameIndex = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        FUnitGFXResource.FSFXLibraries[1].Play(frameIndex % 100);
                    }
                }

                break;
            case SpriteAction.MeleeHit:
            case SpriteAction.MeleeMiss:
            case SpriteAction.Ability:
            case SpriteAction.BreathAttack:
                if (FActionFrame == 0 && CombatMap.Current.FPlaySounds)
                {
                    if (FSelectedAbility > -1)
                    {
                        if (FSelectedAbility == 52)
                        {
                            if (FSelectedSpell != 112 && FSelectedSpell != 122 && FSelectedSpell != 119)
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(FSelectedSpell).FCastSounds.Play(0);
                            }
                        }
                        else
                        {
                            AoWHSSet.Current.FAbilityControl.GetAbility(FSelectedAbility).FSFXLibrary.Play(0);
                        }
                    }
                    else
                    {
                        if (FAction == SpriteAction.MeleeMiss)
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
            case SpriteAction.Hit:
            case SpriteAction.Killed:
                if (FActionFrame == 0 && CombatMap.Current.FPlaySounds)
                {
                    if (FAction == SpriteAction.Hit)
                    {
                        FUnitGFXResource.FSFXLibraries[3].Play(0);
                    }
                    else
                    {
                        FUnitGFXResource.FSFXLibraries[4].Play(0);
                    }
                }

                break;
            case SpriteAction.RoundAttack:
            case SpriteAction.RoundAttackHit:
                if (FActionFrame < 18 && CombatMap.Current.FPlaySounds && TacticalTables.raDir[FActionFrame] > 0)
                {
                    if (FAction == SpriteAction.RoundAttack)
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
