#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class LowerIsometricHexagon
{
    public override void ShowDynamic(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        CustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        int Variant;
        int X;
        int Y;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        Xhp = Xhp - XhpSurf - 8;
        Yhp = Yhp - YhpSurf;
        IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        Variant = FrameFixtureInputs.TextureVariant(this);
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
                    IL[6].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 35) == 1)
                    {
                        IL[13].Show(X, Y, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 7) == 7)
                {
                    IL[25].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 3) == 3)
                    {
                        IL[Variant + 3].Show(X, Y, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 49) == 49)
                {
                    IL[24].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 33) == 33)
                    {
                        IL[Variant + 0].Show(X, Y, SurfaceDesc);
                    }
                }
            }
            else
            {
                if ((FOpenSides & 6) == 6)
                {
                    IL[Variant + 10].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 2) == 2)
                    {
                        IL[17].Show(X, Y, SurfaceDesc);
                    }
                }

                if ((FOpenSides & 48) == 48)
                {
                    IL[Variant + 7].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 32) == 32)
                    {
                        IL[16].Show(X, Y, SurfaceDesc);
                    }
                }
            }

            if ((FOpenSides & 8) != 0)
            {
                if ((FOpenSides & 14) == 14)
                {
                    IL[22].Show(X, Y + 1, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 12) == 12)
                    {
                        if (Variant == 2)
                        {
                            IL[19].Show(X, Y + 1, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 30].Show(X, Y + 1, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 56) == 56)
                {
                    IL[21].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 24) == 24)
                    {
                        if (Variant == 2)
                        {
                            IL[18].Show(X, Y + 1, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 28].Show(X, Y, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 28) == 8)
                {
                    IL[23].Show(X, Y, SurfaceDesc);
                }
            }
            else
            {
                if ((FOpenSides & 14) == 4)
                {
                    IL[15].Show(X, Y, SurfaceDesc);
                }
                else
                {
                    if ((FOpenSides & 56) == 16)
                    {
                        IL[14].Show(X, Y, SurfaceDesc);
                    }
                }
            }
        }
    }
}
