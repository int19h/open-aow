#nullable disable warnings
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class LowerIsometricHex
{
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY -= pixelYSurface;
        CustomImageLibrary? imageLibrary = base.Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int variant = FrameFixtureInputs.TextureVariant(this);
        if (variant == 3)
        {
            variant = 0;
        }

        if (OpenSides != 0)
        {
            int x = pixelX - 19;
            int y = pixelY - 32;
            if ((OpenSides & 1) != 0)
            {
                if ((OpenSides & 0b0010_0011) == 0b0010_0011)
                {
                    imageLibrary[6].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0011) == 1)
                    {
                        imageLibrary[13].Show(x, y, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0111) == 0b0111)
                {
                    imageLibrary[25].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0011) == 0b0011)
                    {
                        imageLibrary[variant + 3].Show(x, y, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0011_0001) == 0b0011_0001)
                {
                    imageLibrary[24].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0001) == 0b0010_0001)
                    {
                        imageLibrary[variant + 0].Show(x, y, lockedSurface);
                    }
                }
            }
            else
            {
                if ((OpenSides & 0b0110) == 0b0110)
                {
                    imageLibrary[variant + 10].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010) == 0b0010)
                    {
                        imageLibrary[17].Show(x, y, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0011_0000) == 0b0011_0000)
                {
                    imageLibrary[variant + 7].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0000) == 0b0010_0000)
                    {
                        imageLibrary[16].Show(x, y, lockedSurface);
                    }
                }
            }

            if ((OpenSides & 0b1000) != 0)
            {
                if ((OpenSides & 0b1110) == 0b1110)
                {
                    imageLibrary[22].Show(x, y + 1, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b1100) == 0b1100)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[19].Show(x, y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 30].Show(x, y + 1, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 0b0011_1000) == 0b0011_1000)
                {
                    imageLibrary[21].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0001_1000) == 0b0001_1000)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[18].Show(x, y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 28].Show(x, y, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 0b0001_1100) == 0b1000)
                {
                    imageLibrary[23].Show(x, y, lockedSurface);
                }
            }
            else
            {
                if ((OpenSides & 0b1110) == 0b0100)
                {
                    imageLibrary[15].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0011_1000) == 0b0001_0000)
                    {
                        imageLibrary[14].Show(x, y, lockedSurface);
                    }
                }
            }
        }
    }
}
