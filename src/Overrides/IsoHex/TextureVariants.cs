#nullable disable
namespace Aow.Units.IsoHex
{
    public partial class TUpperIsometricHexagon
    {
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            int Variant = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__15 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__15.ImageLibrary;
            Variant = Aow.Units.AoWE.FrameFixtureInputs.WaterVariantPhase ?? (((int)((uint)Aow.Rtl.Rtl.ObjectToInt(this) >> 8)) & 3);
            if (Variant == 3)
            {
                Variant = 0;
            }

            var call__16 = IL[((int)(((uint)FSides) >> 6)) + 40];
            call__16.Show(Xhp, Yhp, SurfaceDesc);
            if (FOpenSides != 0)
            {
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 32) == 0)
                    {
                        var call__17 = IL[18];
                        call__17.Show(Xhp - 19, (Yhp - 48) - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 2) == 0)
                    {
                        var call__18 = IL[19];
                        call__18.Show(Xhp - 19, (Yhp - 48) - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 34) == 34)
                    {
                        var call__19 = IL[23];
                        call__19.Show(Xhp - 20, (Yhp - 48) - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 34) == 0)
                        {
                            var call__20 = IL[20];
                            call__20.Show(Xhp - 20, (Yhp - 50) - 16, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) != 0)
                        {
                            var call__21 = IL[17];
                            call__21.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__22 = IL[Variant + 3];
                            call__22.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) != 0)
                        {
                            var call__23 = IL[16];
                            call__23.Show(((Xhp - 19) + 16) + 16, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__24 = IL[Variant];
                            call__24.Show(((Xhp - 19) + 16) + 16, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 20) == 20)
                    {
                        var call__25 = IL[13];
                        call__25.Show(Xhp - 19, Yhp, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 20) == 0)
                        {
                            var call__26 = IL[6];
                            call__26.Show(Xhp - 19, Yhp, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) == 0)
                        {
                            var call__27 = IL[25];
                            call__27.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__28 = IL[15];
                                call__28.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__29 = IL[Variant + 10];
                                call__29.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) == 0)
                        {
                            var call__30 = IL[24];
                            call__30.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__31 = IL[14];
                                call__31.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__32 = IL[Variant + 7];
                                call__32.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }
                }
            }
        }
    }

    public partial class TLowerIsometricHexagon
    {
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            int Variant = default;
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__33 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__33.ImageLibrary;
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
                        var call__34 = IL[6];
                        call__34.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 35) == 1)
                        {
                            var call__35 = IL[13];
                            call__35.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 7) == 7)
                    {
                        var call__36 = IL[25];
                        call__36.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 3) == 3)
                        {
                            var call__37 = IL[Variant + 3];
                            call__37.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 49) == 49)
                    {
                        var call__38 = IL[24];
                        call__38.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 33) == 33)
                        {
                            var call__39 = IL[Variant + 0];
                            call__39.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 6) == 6)
                    {
                        var call__40 = IL[Variant + 10];
                        call__40.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 2) == 2)
                        {
                            var call__41 = IL[17];
                            call__41.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 48) == 48)
                    {
                        var call__42 = IL[Variant + 7];
                        call__42.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 32) == 32)
                        {
                            var call__43 = IL[16];
                            call__43.Show(X, Y, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 14) == 14)
                    {
                        var call__44 = IL[22];
                        call__44.Show(X, Y + 1, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 12) == 12)
                        {
                            if (Variant == 2)
                            {
                                var call__45 = IL[19];
                                call__45.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__46 = IL[Variant + 30];
                                call__46.Show(X, Y + 1, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 56) == 56)
                    {
                        var call__47 = IL[21];
                        call__47.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 24) == 24)
                        {
                            if (Variant == 2)
                            {
                                var call__48 = IL[18];
                                call__48.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__49 = IL[Variant + 28];
                                call__49.Show(X, Y, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 28) == 8)
                    {
                        var call__50 = IL[23];
                        call__50.Show(X, Y, SurfaceDesc);
                    }
                }
                else
                {
                    if ((FOpenSides & 14) == 4)
                    {
                        var call__51 = IL[15];
                        call__51.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 56) == 16)
                        {
                            var call__52 = IL[14];
                            call__52.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
            }
        }
    }
}
