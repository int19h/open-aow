#nullable disable
using static Aow.Rtl.RtlUnit;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
namespace Aow.Units.AoWHex;

public partial class TAoWWaterHexagon
{
    public override void ShowDynamic(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        int X;
        int Y;
        TCustomImageLibrary IL;
        int Variant;
        TImageSequence Sequence;
        int Image;
        bool Showing;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        Xhp = Xhp - XhpSurf - 8;
        Yhp = Yhp - YhpSurf;
        // { Original uses the shared engine settings and the Flood enchantment. }
        if (AoWEngine.FAnimationDetail != 0 && (((((TAoWMapField)Owner).FFieldFlags >> 16) & 255) != 0 || !((TAbstractAoWHSMap)((TMapLevel)Owner.Owner).Map).FFogEnabled))
        {
            FAnimationFrame = (sbyte)(FAnimationFrame + 1);
            if (FAnimationFrame >= 0)
            {
                Showing = false;
                if (FAnimationFrame == 0)
                {
                    // { Original tests the Flood enchantment. }
                    if (AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(TGlobalEnchantmentType.getFlood))
                    {
                        if (FOpenSides == 0 && Random(300) == 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
                        {
                            FAnimationIndex = (byte)(Random(10) + 30);
                        }
                        else
                        {
                            if (Random(10) < 3)
                            {
                                FAnimationIndex = (byte)(Random(9) + 50);
                            }
                            else
                            {
                                FAnimationIndex = (byte)(Random(10) + 20);
                            }
                        }
                    }
                    else
                    {
                        if (FOpenSides == 0 && Random(700) == 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
                        {
                            FAnimationIndex = (byte)(Random(10) + 30);
                        }
                        else
                        {
                            FAnimationIndex = (byte)(Random(10) + 20);
                        }
                    }
                }

                if (FAnimationIndex < FResource.ImageSequenceList.Count)
                {
                    Sequence = (TImageSequence)FResource.ImageSequenceList.List[FAnimationIndex];
                    while (Sequence != null)
                    {
                        Image = Sequence.GetUnloopedFrameIndex(FAnimationFrame);
                        if (Image != -1)
                        {
                            Showing = true;
                            if (Sequence.ImageLibrary.ImageExist(Image))
                            {
                                Sequence.ImageLibrary[Image].Show(Sequence.dXshift + Xhp, Sequence.dYshift + Yhp, SurfaceDesc);
                            }
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (!Showing)
                {
                    if (AoWEngine.FAnimationDetail == 1)
                    {
                        FAnimationFrame = (sbyte)(-20 - Random(25));
                    }
                    else
                    {
                        FAnimationFrame = (sbyte)(-10 - Random(25));
                    }
                }
            }
        }

        base.ShowDynamic(XhpSurf, YhpSurf, SurfaceDesc);
        IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        Variant = FrameFixtureInputs.WaterVariantPhase ?? (((int)((uint)ObjectToInt(this) >> 8)) & 3);
        if (Variant == 3)
        {
            Variant = 0;
        }

        if (FOpenSides != 0)
        {
            X = Xhp - 19;
            Y = Yhp - 32;
            if ((FOpenSides & 1) != 0)
            {
                if ((FOpenSides & 35) == 35)
                {
                    IL[FShoreTerrain[1 - 1] + 6].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 35) == 1)
                    {
                        IL[FShoreTerrain[1 - 1] + 13].Show(X, Y, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 7) == 7)
                {
                    IL[FShoreTerrain[2 - 1] + 25].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 3) == 3)
                    {
                        IL[Variant + 3 + FShoreTerrain[2 - 1]].Show(X, Y, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 49) == 49)
                {
                    IL[FShoreTerrain[6 - 1] + 24].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 33) == 33)
                    {
                        IL[Variant + FShoreTerrain[6 - 1]].Show(X, Y, SurfaceDesc);
                    }
                }
            }
            else
            {
                if ((FOpenSides & 6) == 6)
                {
                    IL[Variant + 10 + FShoreTerrain[3 - 1]].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 2) == 2)
                    {
                        IL[FShoreTerrain[2 - 1] + 17].Show(X, Y, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 48) == 48)
                {
                    IL[Variant + 7 + FShoreTerrain[5 - 1]].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 32) == 32)
                    {
                        IL[FShoreTerrain[6 - 1] + 16].Show(X, Y, SurfaceDesc);
                    }
                }
            }

            if ((FOpenSides & 8) != 0)
            {
                if ((FOpenSides & 14) == 14)
                {
                    IL[FShoreTerrain[3 - 1] + 22].Show(X, Y + 1, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 12) == 12)
                    {
                        if (Variant == 2)
                        {
                            IL[FShoreTerrain[3 - 1] + 19].Show(X, Y + 1, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 30 + FShoreTerrain[3 - 1]].Show(X, Y + 1, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 56) == 56)
                {
                    IL[FShoreTerrain[5 - 1] + 21].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 24) == 24)
                    {
                        if (Variant == 2)
                        {
                            IL[FShoreTerrain[5 - 1] + 18].Show(X, Y + 1, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 28 + FShoreTerrain[5 - 1]].Show(X, Y, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 28) == 8)
                {
                    IL[FShoreTerrain[4 - 1] + 23].Show(X, Y, SurfaceDesc);
                }
            }
            else
            {
                if ((FOpenSides & 14) == 4)
                {
                    IL[FShoreTerrain[3 - 1] + 15].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 56) == 16)
                    {
                        IL[FShoreTerrain[5 - 1] + 14].Show(X, Y, SurfaceDesc);
                    }
                }
            }
        }
    }

}
