#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class UpperIsometricHex
{
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        CustomImageLibrary imageLibrary;
        int pixelX = default;
        int pixelY = default;
        int variant;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY = pixelY - pixelYSurface;
        imageLibrary = _resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        variant = FrameFixtureInputs.TextureVariant(this);
        if (variant == 3)
        {
            variant = 0;
        }

        imageLibrary[(int)(((uint)Sides) >> 6) + 40].Show(pixelX, pixelY, lockedSurface);
        if (OpenSides != 0)
        {
            if ((OpenSides & 1) != 0)
            {
                if ((OpenSides & 32) == 0)
                {
                    imageLibrary[18].Show(pixelX - 19, pixelY - 48 - 16, lockedSurface);
                }

                if ((OpenSides & 2) == 0)
                {
                    imageLibrary[19].Show(pixelX - 19, pixelY - 48 - 16, lockedSurface);
                }

                if ((OpenSides & 34) == 34)
                {
                    imageLibrary[23].Show(pixelX - 20, pixelY - 48 - 16, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 34) == 0)
                    {
                        imageLibrary[20].Show(pixelX - 20, pixelY - 50 - 16, lockedSurface);
                    }
                }
            }

            if ((OpenSides & 8) != 0)
            {
                if ((OpenSides & 16) != 0)
                {
                    if ((OpenSides & 32) != 0)
                    {
                        imageLibrary[17].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                    }
                    else
                    {
                        imageLibrary[variant + 3].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                    }
                }

                if ((OpenSides & 4) != 0)
                {
                    if ((OpenSides & 2) != 0)
                    {
                        imageLibrary[16].Show(pixelX - 19 + 16 + 16, pixelY - 16, lockedSurface);
                    }
                    else
                    {
                        imageLibrary[variant].Show(pixelX - 19 + 16 + 16, pixelY - 16, lockedSurface);
                    }
                }

                if ((OpenSides & 20) == 20)
                {
                    imageLibrary[13].Show(pixelX - 19, pixelY, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 20) == 0)
                    {
                        imageLibrary[6].Show(pixelX - 19, pixelY, lockedSurface);
                    }
                }
            }
            else
            {
                if ((OpenSides & 16) != 0)
                {
                    if ((OpenSides & 32) == 0)
                    {
                        imageLibrary[25].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                    }
                    else
                    {
                        if ((OpenSides & 1) != 0)
                        {
                            imageLibrary[15].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 10].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 4) != 0)
                {
                    if ((OpenSides & 2) == 0)
                    {
                        imageLibrary[24].Show(pixelX - 19 + 32, pixelY - 16, lockedSurface);
                    }
                    else
                    {
                        if ((OpenSides & 1) != 0)
                        {
                            imageLibrary[14].Show(pixelX - 19 + 32, pixelY - 16, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 7].Show(pixelX - 19 + 32, pixelY - 16, lockedSurface);
                        }
                    }
                }
            }
        }
    }
}
