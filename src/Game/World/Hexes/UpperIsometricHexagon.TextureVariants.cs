#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class UpperIsometricHexagon
{
    public override void ShowDynamic(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        CustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        int Variant;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        Xhp = Xhp - XhpSurf - 8;
        Yhp = Yhp - YhpSurf;
        IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        Variant = FrameFixtureInputs.TextureVariant(this);
        if (Variant == 3)
        {
            Variant = 0;
        }

        IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
        if (FOpenSides != 0)
        {
            if ((FOpenSides & 1) != 0)
            {
                if ((FOpenSides & 32) == 0)
                {
                    IL[18].Show(Xhp - 19, Yhp - 48 - 16, SurfaceDesc);
                }

                if ((FOpenSides & 2) == 0)
                {
                    IL[19].Show(Xhp - 19, Yhp - 48 - 16, SurfaceDesc);
                }

                if ((FOpenSides & 34) == 34)
                {
                    IL[23].Show(Xhp - 20, Yhp - 48 - 16, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 34) == 0)
                    {
                        IL[20].Show(Xhp - 20, Yhp - 50 - 16, SurfaceDesc);
                    }
                }
            }

            if ((FOpenSides & 8) != 0)
            {
                if ((FOpenSides & 16) != 0)
                {
                    if ((FOpenSides & 32) != 0)
                    {
                        IL[17].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                    }
                    else
                    {
                        IL[Variant + 3].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 4) != 0)
                {
                    if ((FOpenSides & 2) != 0)
                    {
                        IL[16].Show(Xhp - 19 + 16 + 16, Yhp - 16, SurfaceDesc);
                    }
                    else
                    {
                        IL[Variant].Show(Xhp - 19 + 16 + 16, Yhp - 16, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 20) == 20)
                {
                    IL[13].Show(Xhp - 19, Yhp, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 20) == 0)
                    {
                        IL[6].Show(Xhp - 19, Yhp, SurfaceDesc);
                    }
                }
            }
            else
            {
                if ((FOpenSides & 16) != 0)
                {
                    if ((FOpenSides & 32) == 0)
                    {
                        IL[25].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 1) != 0)
                        {
                            IL[15].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 10].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 4) != 0)
                {
                    if ((FOpenSides & 2) == 0)
                    {
                        IL[24].Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 1) != 0)
                        {
                            IL[14].Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 7].Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                        }
                    }
                }
            }
        }
    }
}
