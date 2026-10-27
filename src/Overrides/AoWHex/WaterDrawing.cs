#nullable disable
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
namespace Aow.Units.AoWHex
{
    public partial class TAoWWaterHexagon
    {
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int X = default;
            int Y = default;
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Variant = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Image = default;
            bool Showing = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            // { Original uses the shared engine settings and the Flood enchantment. }
            if ((Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0) && ((((((Aow.Units.AoWE.TAoWMapField)Owner).FFieldFlags >> 16) & 255) != 0) || !((Aow.Units.AoWE.TAbstractAoWHSMap)((Aow.Units.HSEngine.TMapLevel)Owner.Owner).Map).FFogEnabled))
            {
                FAnimationFrame = (sbyte)(((int)FAnimationFrame) + 1);
                if (unchecked((short)FAnimationFrame) >= 0)
                {
                    Showing = false;
                    if (unchecked((short)FAnimationFrame) == 0)
                    {
                        // { Original tests the Flood enchantment. }
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TGlobalEnchantmentType.getFlood))
                        {
                            bool bool__27 = (FOpenSides == 0);
                            if (bool__27)
                            {
                                var call__28 = Aow.Rtl.Rtl.Random(300);
                                bool__27 = (call__28 == 0);
                            }

                            if (bool__27 && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                            {
                                var call__29 = Aow.Rtl.Rtl.Random(10);
                                FAnimationIndex = unchecked((byte)(call__29 + 30));
                            }
                            else
                            {
                                if (Aow.Rtl.Rtl.Random(10) < 3)
                                {
                                    var call__30 = Aow.Rtl.Rtl.Random(9);
                                    FAnimationIndex = unchecked((byte)(call__30 + 50));
                                }
                                else
                                {
                                    var call__31 = Aow.Rtl.Rtl.Random(10);
                                    FAnimationIndex = unchecked((byte)(call__31 + 20));
                                }
                            }
                        }
                        else
                        {
                            bool bool__32 = (FOpenSides == 0);
                            if (bool__32)
                            {
                                var call__33 = Aow.Rtl.Rtl.Random(700);
                                bool__32 = (call__33 == 0);
                            }

                            if (bool__32 && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                            {
                                var call__34 = Aow.Rtl.Rtl.Random(10);
                                FAnimationIndex = unchecked((byte)(call__34 + 30));
                            }
                            else
                            {
                                var call__35 = Aow.Rtl.Rtl.Random(10);
                                FAnimationIndex = unchecked((byte)(call__35 + 20));
                            }
                        }
                    }

                    if (FAnimationIndex < FResource.ImageSequenceList.Count)
                    {
                        Sequence = (Aow.Units.ImageLib.TImageSequence)FResource.ImageSequenceList.List[FAnimationIndex];
                        while (Sequence != null)
                        {
                            var call__36 = Sequence.GetUnloopedFrameIndex(FAnimationFrame);
                            Image = call__36;
                            if (Image != -1)
                            {
                                Showing = true;
                                var call__37 = Sequence.ImageLibrary.ImageExist(Image);
                                if (call__37)
                                {
                                    var call__38 = Sequence.ImageLibrary[Image];
                                    call__38.Show(Sequence.dXshift + Xhp, Sequence.dYshift + Yhp, SurfaceDesc);
                                }
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    if (!Showing)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 1)
                        {
                            var call__39 = Aow.Rtl.Rtl.Random(25);
                            FAnimationFrame = unchecked((sbyte)(-20 - call__39));
                        }
                        else
                        {
                            var call__40 = Aow.Rtl.Rtl.Random(25);
                            FAnimationFrame = unchecked((sbyte)(-10 - call__40));
                        }
                    }
                }
            }

            base.ShowDynamic(XhpSurf, YhpSurf, SurfaceDesc);
            var call__41 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__41.ImageLibrary;
            Variant = Aow.Units.AoWE.FrameFixtureInputs.WaterVariantPhase ?? (((int)((uint)Aow.Rtl.Rtl.ObjectToInt(this) >> 8)) & 3);
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
                        var call__42 = IL[FShoreTerrain[1 - 1] + 6];
                        call__42.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 35) == 1)
                        {
                            var call__43 = IL[FShoreTerrain[1 - 1] + 13];
                            call__43.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 7) == 7)
                    {
                        var call__44 = IL[FShoreTerrain[2 - 1] + 25];
                        call__44.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 3) == 3)
                        {
                            var call__45 = IL[(Variant + 3) + FShoreTerrain[2 - 1]];
                            call__45.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 49) == 49)
                    {
                        var call__46 = IL[FShoreTerrain[6 - 1] + 24];
                        call__46.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 33) == 33)
                        {
                            var call__47 = IL[Variant + FShoreTerrain[6 - 1]];
                            call__47.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 6) == 6)
                    {
                        var call__48 = IL[(Variant + 10) + FShoreTerrain[3 - 1]];
                        call__48.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 2) == 2)
                        {
                            var call__49 = IL[FShoreTerrain[2 - 1] + 17];
                            call__49.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 48) == 48)
                    {
                        var call__50 = IL[(Variant + 7) + FShoreTerrain[5 - 1]];
                        call__50.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 32) == 32)
                        {
                            var call__51 = IL[FShoreTerrain[6 - 1] + 16];
                            call__51.Show(X, Y, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 14) == 14)
                    {
                        var call__52 = IL[FShoreTerrain[3 - 1] + 22];
                        call__52.Show(X, Y + 1, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 12) == 12)
                        {
                            if (Variant == 2)
                            {
                                var call__53 = IL[FShoreTerrain[3 - 1] + 19];
                                call__53.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__54 = IL[(Variant + 30) + FShoreTerrain[3 - 1]];
                                call__54.Show(X, Y + 1, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 56) == 56)
                    {
                        var call__55 = IL[FShoreTerrain[5 - 1] + 21];
                        call__55.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 24) == 24)
                        {
                            if (Variant == 2)
                            {
                                var call__56 = IL[FShoreTerrain[5 - 1] + 18];
                                call__56.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__57 = IL[(Variant + 28) + FShoreTerrain[5 - 1]];
                                call__57.Show(X, Y, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 28) == 8)
                    {
                        var call__58 = IL[FShoreTerrain[4 - 1] + 23];
                        call__58.Show(X, Y, SurfaceDesc);
                    }
                }
                else
                {
                    if ((FOpenSides & 14) == 4)
                    {
                        var call__59 = IL[FShoreTerrain[3 - 1] + 15];
                        call__59.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 56) == 16)
                        {
                            var call__60 = IL[FShoreTerrain[5 - 1] + 14];
                            call__60.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
            }
        }

    }
}
