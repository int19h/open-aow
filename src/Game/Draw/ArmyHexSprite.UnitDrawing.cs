#nullable disable
using System.Runtime.CompilerServices;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class ArmyHexSprite
{
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
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
                if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || AoWHSMap.Current.FSeatedPlayer != FArmy.FPlayer || FArmy.Count != FArmy.ConcealedUnitsCount())
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
                                AoWHSSet.Current.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[50]);
                                if (FArmy.FStatus.Contains(ArmyStatusElement.ShowCrown))
                                {
                                    AoWHSSet.Current.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                                }
                            }
                            else
                            {
                                var count = FArmy.Count;
                                AoWHSSet.Current.FFlagControl.FFlags[count + 39].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[50]);
                            }
                        }
                    }
                    else
                    {
                        if (FFlagID != 32)
                        {
                            var count = FArmy.Count;
                            AoWHSSet.Current.FRaceResources.GetRaceResource(FFlagID).FImageLibrary[count + 29].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[Math.Abs((AoWHSMap.Current.Frame << 2) % 60 - 30) + 40]);
                            if (FArmy.FStatus.Contains(ArmyStatusElement.ShowCrown))
                            {
                                AoWHSSet.Current.FFlagControl.FFlags[FFlagID + 80].Show(XhpSurf - 5, YhpSurf - 14, SurfaceDesc);
                            }
                        }
                        else
                        {
                            var count = FArmy.Count;
                            AoWHSSet.Current.FFlagControl.FFlags[count + 39].ShowBlended(XhpSurf - 5, YhpSurf - 12, SurfaceDesc, GraphicsState.AlphaBlendTable.LookupTables[Math.Abs((AoWHSMap.Current.Frame << 2) % 60 - 30) + 40]);
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

    public override void ShowShadow(int X, int Y, in LockedSurface SurfaceDesc)
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
