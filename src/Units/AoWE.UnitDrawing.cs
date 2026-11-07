#nullable disable
using System.Runtime.CompilerServices;
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.AoWE;

public partial class TUnitHS
{
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int Index;
        int Xhp = default;
        int Yhp = default;
        int DX = default;
        int DY = default;
        float Progress;
        Assert(FDirection < FUnitGFXResource.FSequences.Count, "Invalid Unit Index", "UnitHS.PAS", 853);
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FMoveStep != -1)
        {
            HNtoHP(FDirection, ref DX, ref DY);
            if ((FStatus & 16) == 0)
            {
                var moveSteps = GetMoveSteps();
                Progress = (float)((double)FMoveStep / moveSteps);
                XhpSurf += Round((double)DX * Progress);
                YhpSurf += Round((double)DY * Progress);
            }

            if ((FStatus & 8) == 0)
            {
                Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                if (Sequence != null && GetSelected())
                {
                    Index = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                    // sound timing gate
                    if (Sequence.SpeedCount == Sequence.SequenceSpeed)
                    {
                        FUnitGFXResource.FSFXLibraries[1].Play(Index % 100);
                    }
                }
            }
            else
            {
                Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
            }
        }
        else
        {
            Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
        }

        if (!GetSelected() || (AoWEUnit.AoWHSMap.Frame & 6) != 6)
        {
            while (Sequence != null)
            {
                Index = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Index != -1)
                {
                    Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }
    }

    public virtual void ShowShadow(int X, int Y, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int Index;
        float Progress;
        int Xhp = default;
        int Yhp = default;
        int DX = default;
        int DY = default;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        X = Xhp - X;
        Y = Yhp - Y;
        if (FMoveStep != -1)
        {
            HNtoHP(FDirection, ref DX, ref DY);
            var moveSteps = GetMoveSteps();
            Progress = (float)((double)FMoveStep / moveSteps);
            X += Round((double)DX * Progress);
            Y += Round((double)DY * Progress);
        }

        if (FDirection == 0)
        {
            Sequence = FUnitGFXResource.FSequences.GetImageSequence(27);
        }
        else
        {
            Sequence = FUnitGFXResource.FSequences.GetImageSequence(FDirection + 23);
        }

        while (Sequence != null)
        {
            Index = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
            Sequence.ImageLibrary[Index].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
            Sequence = Sequence.Next;
        }
    }
}

public partial class TAbstractUnit
{
    public virtual int ShowEx(int X, int Y, in TSurfaceDesc SD, in TP2UnitShowInfo Info)
    {
        TImageSequence Sequence;
        int Frame;
        int TotalFrames;
        int ResourceIndex;
        int Result = 0;
        ResourceIndex = GetUnitGFXResourceIndex();
        AoWEUnit.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).AddRef();
        if (Info.Animation != TUnitAnimation.uaNone)
        {
            Sequence = (TImageSequence)AoWEUnit.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).FSequences.List[(int)Info.Animation * 6 + Info.Direction - 1];
            if (Sequence != null)
            {
                if (!Info.Looped)
                {
                    TotalFrames = Sequence.GetTotalSequenceFrames();
                    Frame = Sequence.GetUnloopedFrameIndex(Info.Frame);
                    if (Frame >= 0)
                    {
                        Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }
                    else
                    {
                        AoWEUnit.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).Show(Info.Direction, X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }

                    if (TotalFrames - Info.Frame > Result)
                    {
                        Result = TotalFrames - Info.Frame;
                    }
                }
                else
                {
                    Frame = Sequence.GetFrameIndex(Info.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (Frame >= 0)
                    {
                        Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }

                    Result = 1;
                }

                Sequence = Sequence.Next;
                while (Sequence != null)
                {
                    Frame = Sequence.GetFrameIndex(Info.Frame, 0);
                    if (Frame >= 0)
                    {
                        Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }

                    Sequence = Sequence.Next;
                }
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        if (GetAbilityEnabled(96))
        {
            if (Info.Looped)
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(96).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWEUnit.AoWHSMap.Frame, SD);
            }
            else
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(96).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
            }
        }

        if (GetAbilityEnabled(98))
        {
            if (Info.Looped)
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(98).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWEUnit.AoWHSMap.Frame, SD);
            }
            else
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(98).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
            }
        }

        if (GetAbilityEnabled(93))
        {
            if (Info.Looped)
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(93).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWEUnit.AoWHSMap.Frame, SD);
            }
            else
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(93).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
            }
        }

        if (Info.Overlays.Contains(TUnitOverlayModesElement.uoHitPoints))
        {
            var hits = GetHits();
            var hitPointsToHitPcnt = (byte)AoWEUnit.HitPointsToHitPcnt(GetHitPoints(), hits);
            AoWEUnit.ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
        }

        if (Info.Overlays.Contains(TUnitOverlayModesElement.uoCombat))
        {
            if (GetHitPoints() == 0)
            {
                AoWEUnit.AoWHSSet.FCombatImages[4].Show(X + 18, Y + 10, SD);
            }

            if (GetAbilityEnabled(48))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(48).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(150))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(150).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(149))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(149).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(97))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(97).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(92))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(92).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(95))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(95).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(94))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(94).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(127))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(127).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(107))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(107).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }

            if (GetAbilityEnabled(34))
            {
                AoWEUnit.AoWHSSet.FAbilityControl.GetAbility(34).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }
        }

        if (FUnitStatus.Contains(TUnitStatusElement.usEnchanted))
        {
            if (Info.Looped)
            {
                AoWEUnit.AoWHSSet.FImageSequences.ShowLooped(70, X + 32, Y + 40, AoWEUnit.AoWHSMap.Frame, SD);
            }
            else
            {
                AoWEUnit.AoWHSSet.FImageSequences.Show(70, X + 32, Y + 40, 5, SD);
            }
        }

        AoWEUnit.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).Release();
        return Result;
    }
}

public partial class TArmyHS
{
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int Frame;
        int Xhp = default;
        int Yhp = default;
        int DX = default;
        int DY = default;
        double Part;
        if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((TAoWMapField)Owner).FFieldFlags).Hi).Lo != 0 || !AoWEUnit.AoWHSMap.FFogEnabled)
        {
            if (FDirection >= FUnitGFXResource.FSequences.Count)
            {
                /* TODO: Implement the inline assembly operation. */
                throw new NotImplementedException();
            }

            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FMoveStep != -1)
            {
                HNtoHP(FDirection, ref DX, ref DY);
                if ((FStatus & 16) == 0)
                {
                    Part = (double)FMoveStep / FMoveSteps;
                    XhpSurf += Round((double)Part * DX);
                    YhpSurf += Round((double)Part * DY);
                }

                if ((FStatus & 8) == 0)
                {
                    if (FDirection == 0)
                    {
                        Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[9];
                    }
                    else
                    {
                        Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                    }
                }
                else
                {
                    if (FDirection == 0)
                    {
                        Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[3];
                    }
                    else
                    {
                        Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
                    }
                }
            }
            else
            {
                if (FDirection == 0)
                {
                    Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[3];
                }
                else
                {
                    Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
                }
            }

            if (!FArmy.FStatus.Contains(TArmyStatusElement.asConcealed))
            {
                if (AoWEUnit.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) || AoWEUnit.AoWHSMap.FSeatedPlayer != FArmy.FPlayer || FArmy.Count != FArmy.ConcealedUnitsCount())
                {
                    while (Sequence != null)
                    {
                        Frame = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Frame != -1)
                        {
                            Sequence.ImageLibrary[Frame].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }

                    ShowEnchantments(XhpSurf, YhpSurf + FUnitYOffset, SurfaceDesc);
                    if (!FArmy.FStatus.Contains(TArmyStatusElement.asSelected) || (AoWEUnit.AoWHSMap.Frame & 6) != 6)
                    {
                        if (FFlagID == 32)
                        {
                            var count = FArmy.Count;
                            AoWEUnit.AoWHSSet.FFlagControl.FFlags[count + 39].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                        else
                        {
                            var count = FArmy.Count;
                            AoWEUnit.AoWHSSet.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                            if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                            {
                                AoWEUnit.AoWHSSet.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                            }
                        }
                    }
                }
                else
                {
                    while (Sequence != null)
                    {
                        Frame = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Frame != -1)
                        {
                            Sequence.ImageLibrary[Frame].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }

                    ShowEnchantments(XhpSurf, YhpSurf + FUnitYOffset, SurfaceDesc);
                    if (FArmy.FStatus.Contains(TArmyStatusElement.asSelected))
                    {
                        if ((AoWEUnit.AoWHSMap.Frame & 6) != 6)
                        {
                            if (FFlagID != 32)
                            {
                                var count = FArmy.Count;
                                AoWEUnit.AoWHSSet.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[50]);
                                if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                                {
                                    AoWEUnit.AoWHSSet.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                            else
                            {
                                var count = FArmy.Count;
                                AoWEUnit.AoWHSSet.FFlagControl.FFlags[count + 39].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[50]);
                            }
                        }
                    }
                    else
                    {
                        if (FFlagID != 32)
                        {
                            var count = FArmy.Count;
                            AoWEUnit.AoWHSSet.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[Math.Abs((AoWEUnit.AoWHSMap.Frame << 2) % 60 - 30) + 40]);
                            if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                            {
                                AoWEUnit.AoWHSSet.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                            }
                        }
                        else
                        {
                            var count = FArmy.Count;
                            AoWEUnit.AoWHSSet.FFlagControl.FFlags[count + 39].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[Math.Abs((AoWEUnit.AoWHSMap.Frame << 2) % 60 - 30) + 40]);
                        }
                    }
                }
            }
            else
            {
                if (!FArmy.FStatus.Contains(TArmyStatusElement.asFullyConcealed))
                {
                    while (Sequence != null)
                    {
                        Frame = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (Frame != -1)
                        {
                            Sequence.ImageLibrary[Frame].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }

                    if (!FArmy.FStatus.Contains(TArmyStatusElement.asSelected) || (AoWEUnit.AoWHSMap.Frame & 6) != 6)
                    {
                        if (FFlagID == 32)
                        {
                            var count = FArmy.Count;
                            var concealedUnitsCount = FArmy.ConcealedUnitsCount();
                            AoWEUnit.AoWHSSet.FFlagControl.FFlags[count - concealedUnitsCount + 39].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                        else
                        {
                            var count = FArmy.Count;
                            var concealedUnitsCount = FArmy.ConcealedUnitsCount();
                            AoWEUnit.AoWHSSet.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count - concealedUnitsCount + 29].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                            if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                            {
                                AoWEUnit.AoWHSSet.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                            }
                        }
                    }
                }
            }
        }
    }

    public override void ShowShadow(int X, int Y, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        float Part;
        int Xhp = default;
        int Yhp = default;
        int DX = default;
        int DY = default;
        int Frame;
        if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((TAoWMapField)Owner).FFieldFlags).Hi).Lo != 0 || !AoWEUnit.AoWHSMap.FFogEnabled) && !FArmy.FStatus.Contains(TArmyStatusElement.asFullyConcealed))
        {
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            X = Xhp - X;
            Y = Yhp - Y;
            if (FMoveStep != -1)
            {
                HNtoHP(FDirection, ref DX, ref DY);
                if ((FStatus & 16) == 0)
                {
                    var moveSteps = GetMoveSteps();
                    Part = (float)((double)FMoveStep / moveSteps);
                    X += Round((double)Part * DX);
                    Y += Round((double)Part * DY);
                }
            }

            if (FDirection == 0)
            {
                Sequence = FUnitGFXResource.FSequences.GetImageSequence(27);
            }
            else
            {
                Sequence = FUnitGFXResource.FSequences.GetImageSequence(FDirection + 23);
            }

            while (Sequence != null)
            {
                Frame = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                Sequence = Sequence.Next;
            }
        }
    }

    public void PlayMoveSFX()
    {
        TImageSequence Sequence;
        int Frame;
        if (FMoveStep != -1 && (FStatus & 8) == 0)
        {
            if (FDirection == 0)
            {
                Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[9];
            }
            else
            {
                Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
            }

            if (Sequence != null)
            {
                Frame = Sequence.GetFrameIndex(AoWEUnit.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Sequence.SpeedCount == Sequence.SequenceSpeed)
                {
                    FUnitGFXResource.FSFXLibraries[1].Play(Frame % 100);
                }
            }
        }
    }
}
