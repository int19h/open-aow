#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class LowerIsometricHex
{
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        CustomImageLibrary imageLibrary;
        int pixelX = default;
        int pixelY = default;
        int variant;
        int X;
        int Y;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY = pixelY - pixelYSurface;
        imageLibrary = _resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        variant = FrameFixtureInputs.TextureVariant(this);
        if (variant == 3)
        {
            variant = 0;
        }

        if (OpenSides != 0)
        {
            X = pixelX - 19;
            Y = pixelY - 32;
            if ((OpenSides & 1) != 0)
            {
                if ((OpenSides & 35) == 35)
                {
                    imageLibrary[6].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 35) == 1)
                    {
                        imageLibrary[13].Show(X, Y, lockedSurface);
                    }
                }

                if ((OpenSides & 7) == 7)
                {
                    imageLibrary[25].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 3) == 3)
                    {
                        imageLibrary[variant + 3].Show(X, Y, lockedSurface);
                    }
                }

                if ((OpenSides & 49) == 49)
                {
                    imageLibrary[24].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 33) == 33)
                    {
                        imageLibrary[variant + 0].Show(X, Y, lockedSurface);
                    }
                }
            }
            else
            {
                if ((OpenSides & 6) == 6)
                {
                    imageLibrary[variant + 10].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 2) == 2)
                    {
                        imageLibrary[17].Show(X, Y, lockedSurface);
                    }
                }

                if ((OpenSides & 48) == 48)
                {
                    imageLibrary[variant + 7].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 32) == 32)
                    {
                        imageLibrary[16].Show(X, Y, lockedSurface);
                    }
                }
            }

            if ((OpenSides & 8) != 0)
            {
                if ((OpenSides & 14) == 14)
                {
                    imageLibrary[22].Show(X, Y + 1, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 12) == 12)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[19].Show(X, Y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 30].Show(X, Y + 1, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 56) == 56)
                {
                    imageLibrary[21].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 24) == 24)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[18].Show(X, Y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 28].Show(X, Y, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 28) == 8)
                {
                    imageLibrary[23].Show(X, Y, lockedSurface);
                }
            }
            else
            {
                if ((OpenSides & 14) == 4)
                {
                    imageLibrary[15].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 56) == 16)
                    {
                        imageLibrary[14].Show(X, Y, lockedSurface);
                    }
                }
            }
        }
    }
}
