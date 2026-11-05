#nullable disable
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.AoWE
{
    public partial class TUnitHS
    {
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int Index = default;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            float Progress = default;
            Assert(FDirection < FUnitGFXResource.FSequences.Count, (AnsiString)"Invalid Unit Index", "UnitHS.PAS", 853);
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FMoveStep != -1)
            {
                HNtoHP(FDirection, ref DX, ref DY);
                if ((FStatus & 16) == 0)
                {
                    var call__2062 = GetMoveSteps();
                    Progress = (float)((double)FMoveStep / call__2062);
                    XhpSurf += Round((double)DX * Progress);
                    YhpSurf += Round((double)DY * Progress);
                }

                if ((FStatus & 8) == 0)
                {
                    Sequence = (TImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                    if (Sequence != null && GetSelected())
                    {
                        var call__2063 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        Index = call__2063;
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

            var call__2064 = GetSelected();
            if (!call__2064 || (AoWE.AoWHSMap.Frame & 6) != 6)
            {
                while (Sequence != null)
                {
                    var call__2065 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                    Index = call__2065;
                    if (Index != -1)
                    {
                        var call__2066 = Sequence.ImageLibrary[Index];
                        call__2066.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }
        }

        public virtual void ShowShadow(int X, int Y, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int Index = default;
            float Progress = default;
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
                var call__2073 = GetMoveSteps();
                Progress = (float)((double)FMoveStep / call__2073);
                X += Round((double)DX * Progress);
                Y += Round((double)DY * Progress);
            }

            if (FDirection == 0)
            {
                var call__2074 = FUnitGFXResource.FSequences.GetImageSequence(27);
                Sequence = call__2074;
            }
            else
            {
                var call__2075 = FUnitGFXResource.FSequences.GetImageSequence(FDirection + 23);
                Sequence = call__2075;
            }

            while (Sequence != null)
            {
                var call__2076 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                Index = call__2076;
                var call__2077 = Sequence.ImageLibrary[Index];
                call__2077.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                Sequence = Sequence.Next;
            }
        }
    }
    public partial class TAbstractUnit
    {
        public virtual int ShowEx(int X, int Y, in TSurfaceDesc SD, in TP2UnitShowInfo Info)
        {
            int Result = default;
            TImageSequence Sequence = default;
            int Frame = default;
            int TotalFrames = default;
            int ResourceIndex = default;
            Result = 0;
            ResourceIndex = GetUnitGFXResourceIndex();
            var call__2481 = AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex);
            call__2481.AddRef();
            if (Info.Animation != TUnitAnimation.uaNone)
            {
                var call__2482 = AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex);
                Sequence = (TImageSequence)call__2482.FSequences.List[(int)Info.Animation * 6 + Info.Direction - 1];
                if (Sequence != null)
                {
                    if (!Info.Looped)
                    {
                        TotalFrames = Sequence.GetTotalSequenceFrames();
                        var call__2483 = Sequence.GetUnloopedFrameIndex(Info.Frame);
                        Frame = call__2483;
                        if (Frame >= 0)
                        {
                            var call__2484 = Sequence.ImageLibrary[Frame];
                            call__2484.Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                        }
                        else
                        {
                            var call__2485 = AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex);
                            call__2485.Show(Info.Direction, X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                        }

                        if (TotalFrames - Info.Frame > Result)
                        {
                            Result = TotalFrames - Info.Frame;
                        }
                    }
                    else
                    {
                        var call__2486 = Sequence.GetFrameIndex(Info.Frame, FrameFixtureInputs.AnimationOffset(this));
                        Frame = call__2486;
                        if (Frame >= 0)
                        {
                            var call__2487 = Sequence.ImageLibrary[Frame];
                            call__2487.Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                        }

                        Result = 1;
                    }

                    Sequence = Sequence.Next;
                    while (Sequence != null)
                    {
                        var call__2488 = Sequence.GetFrameIndex(Info.Frame, 0);
                        Frame = call__2488;
                        if (Frame >= 0)
                        {
                            var call__2489 = Sequence.ImageLibrary[Frame];
                            call__2489.Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
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
                    var call__2490 = AoWE.AoWHSSet.FAbilityControl.GetAbility(96);
                    call__2490.FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWE.AoWHSMap.Frame, SD);
                }
                else
                {
                    var call__2491 = AoWE.AoWHSSet.FAbilityControl.GetAbility(96);
                    call__2491.FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
                }
            }

            if (GetAbilityEnabled(98))
            {
                if (Info.Looped)
                {
                    var call__2492 = AoWE.AoWHSSet.FAbilityControl.GetAbility(98);
                    call__2492.FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWE.AoWHSMap.Frame, SD);
                }
                else
                {
                    var call__2493 = AoWE.AoWHSSet.FAbilityControl.GetAbility(98);
                    call__2493.FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
                }
            }

            if (GetAbilityEnabled(93))
            {
                if (Info.Looped)
                {
                    var call__2494 = AoWE.AoWHSSet.FAbilityControl.GetAbility(93);
                    call__2494.FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWE.AoWHSMap.Frame, SD);
                }
                else
                {
                    var call__2495 = AoWE.AoWHSSet.FAbilityControl.GetAbility(93);
                    call__2495.FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
                }
            }

            if (Info.Overlays.Contains(TUnitOverlayModesElement.uoHitPoints))
            {
                var call__2496 = GetHits();
                var call__2497 = GetHitPoints();
                var call__2498 = (byte)AoWE.HitPointsToHitPcnt(call__2497, call__2496);
                AoWE.ShowHitPcnt(X, Y, SD, call__2498);
            }

            if (Info.Overlays.Contains(TUnitOverlayModesElement.uoCombat))
            {
                if (GetHitPoints() == 0)
                {
                    var call__2499 = AoWE.AoWHSSet.FCombatImages[4];
                    call__2499.Show(X + 18, Y + 10, SD);
                }

                if (GetAbilityEnabled(48))
                {
                    var call__2500 = AoWE.AoWHSSet.FAbilityControl.GetAbility(48);
                    call__2500.FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(150))
                {
                    var call__2501 = AoWE.AoWHSSet.FAbilityControl.GetAbility(150);
                    call__2501.FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(149))
                {
                    var call__2502 = AoWE.AoWHSSet.FAbilityControl.GetAbility(149);
                    call__2502.FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(97))
                {
                    var call__2503 = AoWE.AoWHSSet.FAbilityControl.GetAbility(97);
                    call__2503.FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(92))
                {
                    var call__2504 = AoWE.AoWHSSet.FAbilityControl.GetAbility(92);
                    call__2504.FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(95))
                {
                    var call__2505 = AoWE.AoWHSSet.FAbilityControl.GetAbility(95);
                    call__2505.FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(94))
                {
                    var call__2506 = AoWE.AoWHSSet.FAbilityControl.GetAbility(94);
                    call__2506.FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(127))
                {
                    var call__2507 = AoWE.AoWHSSet.FAbilityControl.GetAbility(127);
                    call__2507.FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(107))
                {
                    var call__2508 = AoWE.AoWHSSet.FAbilityControl.GetAbility(107);
                    call__2508.FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }

                if (GetAbilityEnabled(34))
                {
                    var call__2509 = AoWE.AoWHSSet.FAbilityControl.GetAbility(34);
                    call__2509.FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }
            }

            if (FUnitStatus.Contains(TUnitStatusElement.usEnchanted))
            {
                if (Info.Looped)
                {
                    AoWE.AoWHSSet.FImageSequences.ShowLooped(70, X + 32, Y + 40, AoWE.AoWHSMap.Frame, SD);
                }
                else
                {
                    AoWE.AoWHSSet.FImageSequences.Show(70, X + 32, Y + 40, 5, SD);
                }
            }

            var call__2510 = AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(ResourceIndex);
            call__2510.Release();
            return Result;
        }
    }
    public partial class TArmyHS
    {
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int Frame = default;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            double Part = default;
            if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((TAoWMapField)Owner).FFieldFlags).Hi).Lo != 0 || !AoWE.AoWHSMap.FFogEnabled)
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
                    bool bool__3698 = AoWE.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) || AoWE.AoWHSMap.FSeatedPlayer != FArmy.FPlayer;
                    if (!bool__3698)
                    {
                        var call__3699 = FArmy.Count;
                        var call__3700 = FArmy.ConcealedUnitsCount();
                        bool__3698 = call__3699 != call__3700;
                    }

                    if (bool__3698)
                    {
                        while (Sequence != null)
                        {
                            var call__3701 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            Frame = call__3701;
                            if (Frame != -1)
                            {
                                var call__3702 = Sequence.ImageLibrary[Frame];
                                call__3702.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        ShowEnchantments(XhpSurf, YhpSurf + FUnitYOffset, SurfaceDesc);
                        if (!FArmy.FStatus.Contains(TArmyStatusElement.asSelected) || (AoWE.AoWHSMap.Frame & 6) != 6)
                        {
                            if (FFlagID == 32)
                            {
                                var call__3703 = FArmy.Count;
                                var call__3704 = AoWE.AoWHSSet.FFlagControl.FFlags[call__3703 + 39];
                                call__3704.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                            }
                            else
                            {
                                var call__3705 = FArmy.Count;
                                var call__3706 = AoWE.AoWHSSet.FRaceResources.GetRaceResource(FFlagID);
                                var call__3707 = call__3706.FImageLibrary[call__3705 + 29];
                                call__3707.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                                if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                                {
                                    var call__3708 = AoWE.AoWHSSet.FFlagControl.FFlags[FFlagID + 80];
                                    call__3708.Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                        }
                    }
                    else
                    {
                        while (Sequence != null)
                        {
                            var call__3709 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            Frame = call__3709;
                            if (Frame != -1)
                            {
                                var call__3710 = Sequence.ImageLibrary[Frame];
                                call__3710.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        ShowEnchantments(XhpSurf, YhpSurf + FUnitYOffset, SurfaceDesc);
                        if (FArmy.FStatus.Contains(TArmyStatusElement.asSelected))
                        {
                            if ((AoWE.AoWHSMap.Frame & 6) != 6)
                            {
                                if (FFlagID != 32)
                                {
                                    var call__3711 = FArmy.Count;
                                    var call__3712 = AoWE.AoWHSSet.FRaceResources.GetRaceResource(FFlagID);
                                    var call__3713 = call__3712.FImageLibrary[call__3711 + 29];
                                    call__3713.ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[50]);
                                    if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                                    {
                                        var call__3714 = AoWE.AoWHSSet.FFlagControl.FFlags[FFlagID + 80];
                                        call__3714.Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                    }
                                }
                                else
                                {
                                    var call__3715 = FArmy.Count;
                                    var call__3716 = AoWE.AoWHSSet.FFlagControl.FFlags[call__3715 + 39];
                                    call__3716.ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[50]);
                                }
                            }
                        }
                        else
                        {
                            if (FFlagID != 32)
                            {
                                var call__3717 = FArmy.Count;
                                var call__3718 = AoWE.AoWHSSet.FRaceResources.GetRaceResource(FFlagID);
                                var call__3719 = call__3718.FImageLibrary[call__3717 + 29];
                                call__3719.ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[Math.Abs((AoWE.AoWHSMap.Frame << 2) % 60 - 30) + 40]);
                                if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                                {
                                    var call__3720 = AoWE.AoWHSSet.FFlagControl.FFlags[FFlagID + 80];
                                    call__3720.Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                            else
                            {
                                var call__3721 = FArmy.Count;
                                var call__3722 = AoWE.AoWHSSet.FFlagControl.FFlags[call__3721 + 39];
                                call__3722.ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, AlphaBlendTable.LookupTables[Math.Abs((AoWE.AoWHSMap.Frame << 2) % 60 - 30) + 40]);
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
                            var call__3723 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            Frame = call__3723;
                            if (Frame != -1)
                            {
                                var call__3724 = Sequence.ImageLibrary[Frame];
                                call__3724.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        if (!FArmy.FStatus.Contains(TArmyStatusElement.asSelected) || (AoWE.AoWHSMap.Frame & 6) != 6)
                        {
                            if (FFlagID == 32)
                            {
                                var call__3725 = FArmy.Count;
                                var call__3726 = FArmy.ConcealedUnitsCount();
                                var call__3727 = AoWE.AoWHSSet.FFlagControl.FFlags[call__3725 - call__3726 + 39];
                                call__3727.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                            }
                            else
                            {
                                var call__3728 = FArmy.Count;
                                var call__3729 = FArmy.ConcealedUnitsCount();
                                var call__3730 = AoWE.AoWHSSet.FRaceResources.GetRaceResource(FFlagID);
                                var call__3731 = call__3730.FImageLibrary[call__3728 - call__3729 + 29];
                                call__3731.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                                if (FArmy.FStatus.Contains(TArmyStatusElement.asShowCrown))
                                {
                                    var call__3732 = AoWE.AoWHSSet.FFlagControl.FFlags[FFlagID + 80];
                                    call__3732.Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void ShowShadow(int X, int Y, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            float Part = default;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            int Frame = default;
            if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((TAoWMapField)Owner).FFieldFlags).Hi).Lo != 0 || !AoWE.AoWHSMap.FFogEnabled) && !FArmy.FStatus.Contains(TArmyStatusElement.asFullyConcealed))
            {
                HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
                X = Xhp - X;
                Y = Yhp - Y;
                if (FMoveStep != -1)
                {
                    HNtoHP(FDirection, ref DX, ref DY);
                    if ((FStatus & 16) == 0)
                    {
                        var call__3784 = GetMoveSteps();
                        Part = (float)((double)FMoveStep / call__3784);
                        X += Round((double)Part * DX);
                        Y += Round((double)Part * DY);
                    }
                }

                if (FDirection == 0)
                {
                    var call__3785 = FUnitGFXResource.FSequences.GetImageSequence(27);
                    Sequence = call__3785;
                }
                else
                {
                    var call__3786 = FUnitGFXResource.FSequences.GetImageSequence(FDirection + 23);
                    Sequence = call__3786;
                }

                while (Sequence != null)
                {
                    var call__3787 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                    Frame = call__3787;
                    var call__3788 = Sequence.ImageLibrary[Frame];
                    call__3788.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                    Sequence = Sequence.Next;
                }
            }
        }

        public void PlayMoveSFX()
        {
            TImageSequence Sequence = default;
            int Frame = default;
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
                    var call__3810 = Sequence.GetFrameIndex(AoWE.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                    Frame = call__3810;
                    if (Sequence.SpeedCount == Sequence.SequenceSpeed)
                    {
                        FUnitGFXResource.FSFXLibraries[1].Play(Frame % 100);
                    }
                }
            }
        }
    }
}
