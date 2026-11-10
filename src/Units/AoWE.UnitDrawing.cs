#nullable disable
using System.Runtime.CompilerServices;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw
{

    public partial class UnitHS
    {
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            ImageSequence Sequence;
            int Index;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            float Progress;
            Assert(FDirection < FUnitGFXResource.FSequences.Count, "Invalid Unit Index", "UnitHS.PAS", 853);
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FMoveStep != -1)
            {
                HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
                if ((FStatus & 16) == 0)
                {
                    var moveSteps = GetMoveSteps();
                    Progress = (float)((double)FMoveStep / moveSteps);
                    XhpSurf += Round((double)DX * Progress);
                    YhpSurf += Round((double)DY * Progress);
                }

                if ((FStatus & 8) == 0)
                {
                    Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                    if (Sequence != null && GetSelected())
                    {
                        Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        // sound timing gate
                        if (Sequence.SpeedCount == Sequence.SequenceSpeed)
                        {
                            FUnitGFXResource.FSFXLibraries[1].Play(Index % 100);
                        }
                    }
                }
                else
                {
                    Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
                }
            }
            else
            {
                Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
            }

            if (!GetSelected() || (AoWHSMap.Current.Frame & 6) != 6)
            {
                while (Sequence != null)
                {
                    Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (Index != -1)
                    {
                        Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }
        }

        public virtual void ShowShadow(int X, int Y, in SurfaceDesc SurfaceDesc)
        {
            ImageSequence Sequence;
            int Index;
            float Progress;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
            X = Xhp - X;
            Y = Yhp - Y;
            if (FMoveStep != -1)
            {
                HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
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
                Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                Sequence.ImageLibrary[Index].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                Sequence = Sequence.Next;
            }
        }
    }
}

namespace Aow.Game.Magic.Abilities
{

    public partial class AbstractUnit
    {
        public virtual int ShowEx(int X, int Y, in SurfaceDesc SD, in P2UnitShowInfo Info)
        {
            ImageSequence Sequence;
            int Frame;
            int TotalFrames;
            int ResourceIndex;
            int Result = 0;
            ResourceIndex = GetUnitGFXResourceIndex();
            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).AddRef();
            if (Info.Animation != UnitAnimation.None)
            {
                Sequence = (ImageSequence)AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).FSequences.List[(int)Info.Animation * 6 + Info.Direction - 1];
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
                            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).Show(Info.Direction, X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
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
                    AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
                }
                else
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
                }
            }

            if (GetAbilityEnabled(98))
            {
                if (Info.Looped)
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
                }
                else
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
                }
            }

            if (GetAbilityEnabled(93))
            {
                if (Info.Looped)
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
                }
                else
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
                }
            }

            if (Info.Overlays.Contains(UnitOverlayModesElement.HitPoints))
            {
                var hits = GetHits();
                var hitPointsToHitPcnt = (byte)HitChance.HitPointsToHitPcnt(GetHitPoints(), hits);
                HitChance.ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
            }

            if (Info.Overlays.Contains(UnitOverlayModesElement.Combat))
            {
                if (GetHitPoints() == 0)
                {
                    AoWHSSet.Current.FCombatImages[4].Show(X + 18, Y + 10, SD);
                }

                if (GetAbilityEnabled(48))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(48).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(150))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(150).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(149))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(149).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(97))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(97).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(92))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(92).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(95))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(95).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(94))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(94).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(127))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(127).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(107))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(107).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }

                if (GetAbilityEnabled(34))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(34).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }
            }

            if (FUnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                if (Info.Looped)
                {
                    AoWHSSet.Current.FImageSequences.ShowLooped(70, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
                }
                else
                {
                    AoWHSSet.Current.FImageSequences.Show(70, X + 32, Y + 40, 5, SD);
                }
            }

            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).Release();
            return Result;
        }
    }
}

namespace Aow.Game.Draw
{

    public partial class ArmyHS
    {
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            ImageSequence Sequence;
            int Frame;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            double Part;
            if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo != 0 || !AoWHSMap.Current.FFogEnabled)
            {
                if (FDirection >= FUnitGFXResource.FSequences.Count)
                {
                    /* TODO: Implement the inline assembly operation. */
                    throw new NotImplementedException();
                }

                HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
                XhpSurf = Xhp - XhpSurf;
                YhpSurf = Yhp - YhpSurf;
                if (FMoveStep != -1)
                {
                    HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
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
                            Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[9];
                        }
                        else
                        {
                            Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                        }
                    }
                    else
                    {
                        if (FDirection == 0)
                        {
                            Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[3];
                        }
                        else
                        {
                            Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
                        }
                    }
                }
                else
                {
                    if (FDirection == 0)
                    {
                        Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[3];
                    }
                    else
                    {
                        Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
                    }
                }

                if (!FArmy.FStatus.Contains(ArmyStatusElement.Concealed))
                {
                    if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) || AoWHSMap.Current.FSeatedPlayer != FArmy.FPlayer || FArmy.Count != FArmy.ConcealedUnitsCount())
                    {
                        while (Sequence != null)
                        {
                            Frame = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (Frame != -1)
                            {
                                Sequence.ImageLibrary[Frame].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        ShowEnchantments(XhpSurf, YhpSurf + FUnitYOffset, SurfaceDesc);
                        if (!FArmy.FStatus.Contains(ArmyStatusElement.Selected) || (AoWHSMap.Current.Frame & 6) != 6)
                        {
                            if (FFlagID == 32)
                            {
                                var count = FArmy.Count;
                                AoWHSSet.Current.FFlagControl.FFlags[count + 39].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                            }
                            else
                            {
                                var count = FArmy.Count;
                                AoWHSSet.Current.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                                if (FArmy.FStatus.Contains(ArmyStatusElement.ShowCrown))
                                {
                                    AoWHSSet.Current.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                        }
                    }
                    else
                    {
                        while (Sequence != null)
                        {
                            Frame = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (Frame != -1)
                            {
                                Sequence.ImageLibrary[Frame].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        ShowEnchantments(XhpSurf, YhpSurf + FUnitYOffset, SurfaceDesc);
                        if (FArmy.FStatus.Contains(ArmyStatusElement.Selected))
                        {
                            if ((AoWHSMap.Current.Frame & 6) != 6)
                            {
                                if (FFlagID != 32)
                                {
                                    var count = FArmy.Count;
                                    AoWHSSet.Current.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[50]);
                                    if (FArmy.FStatus.Contains(ArmyStatusElement.ShowCrown))
                                    {
                                        AoWHSSet.Current.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                    }
                                }
                                else
                                {
                                    var count = FArmy.Count;
                                    AoWHSSet.Current.FFlagControl.FFlags[count + 39].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[50]);
                                }
                            }
                        }
                        else
                        {
                            if (FFlagID != 32)
                            {
                                var count = FArmy.Count;
                                AoWHSSet.Current.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Math.Abs((AoWHSMap.Current.Frame << 2) % 60 - 30) + 40]);
                                if (FArmy.FStatus.Contains(ArmyStatusElement.ShowCrown))
                                {
                                    AoWHSSet.Current.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                            else
                            {
                                var count = FArmy.Count;
                                AoWHSSet.Current.FFlagControl.FFlags[count + 39].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Math.Abs((AoWHSMap.Current.Frame << 2) % 60 - 30) + 40]);
                            }
                        }
                    }
                }
                else
                {
                    if (!FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed))
                    {
                        while (Sequence != null)
                        {
                            Frame = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (Frame != -1)
                            {
                                Sequence.ImageLibrary[Frame].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift + FUnitYOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        if (!FArmy.FStatus.Contains(ArmyStatusElement.Selected) || (AoWHSMap.Current.Frame & 6) != 6)
                        {
                            if (FFlagID == 32)
                            {
                                var count = FArmy.Count;
                                var concealedUnitsCount = FArmy.ConcealedUnitsCount();
                                AoWHSSet.Current.FFlagControl.FFlags[count - concealedUnitsCount + 39].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                            }
                            else
                            {
                                var count = FArmy.Count;
                                var concealedUnitsCount = FArmy.ConcealedUnitsCount();
                                AoWHSSet.Current.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count - concealedUnitsCount + 29].Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                                if (FArmy.FStatus.Contains(ArmyStatusElement.ShowCrown))
                                {
                                    AoWHSSet.Current.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void ShowShadow(int X, int Y, in SurfaceDesc SurfaceDesc)
        {
            ImageSequence Sequence;
            float Part;
            int Xhp = default;
            int Yhp = default;
            int DX = default;
            int DY = default;
            int Frame;
            if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo != 0 || !AoWHSMap.Current.FFogEnabled) && !FArmy.FStatus.Contains(ArmyStatusElement.FullyConcealed))
            {
                HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
                X = Xhp - X;
                Y = Yhp - Y;
                if (FMoveStep != -1)
                {
                    HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
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
                    Frame = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                    Sequence = Sequence.Next;
                }
            }
        }

        public void PlayMoveSFX()
        {
            ImageSequence Sequence;
            int Frame;
            if (FMoveStep != -1 && (FStatus & 8) == 0)
            {
                if (FDirection == 0)
                {
                    Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[9];
                }
                else
                {
                    Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                }

                if (Sequence != null)
                {
                    Frame = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (Sequence.SpeedCount == Sequence.SequenceSpeed)
                    {
                        FUnitGFXResource.FSFXLibraries[1].Play(Frame % 100);
                    }
                }
            }
        }
    }
}
