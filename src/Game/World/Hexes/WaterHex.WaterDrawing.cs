#nullable disable warnings
#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

public partial class WaterHex
{
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY -= pixelYSurface;
        // { Original uses the shared engine settings and the Flood enchantment. }
        if (GetEngine().AnimationDetail != 0 && (Owner.FogCount != 0 || !Owner.Owner.Map.FogEnabled))
        {
            _animationFrame = (_animationFrame + 1).TruncateToSByte();
            if (_animationFrame >= 0)
            {
                bool showing = false;
                if (_animationFrame == 0)
                {
                    // { Original tests the Flood enchantment. }
                    _animationIndex = GetEngine().StrategicMap.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentTypes.Flood)
                        ? OpenSides == 0 && Random(300) == 0 && !GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Editting)
                            ? (byte)(Random(10) + 30)
                            : Random(10) < 3 ? (byte)(Random(9) + 50) : (byte)(Random(10) + 20)
                        : OpenSides == 0 && Random(700) == 0 && !GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Editting)
                            ? (byte)(Random(10) + 30)
                            : (byte)(Random(10) + 20);
                }

                if (_animationIndex < ((MapObject)this).Resource.ImageSequenceList.Count)
                {
                    ImageSequence sequence = ((MapObject)this).Resource.ImageSequenceList.List[_animationIndex];
                    while (sequence is not null)
                    {
                        int image = sequence.GetUnloopedFrameIndex(_animationFrame);
                        if (image != -1)
                        {
                            showing = true;
                            if (sequence.ImageLibrary.ImageExist(image))
                            {
                                sequence.ImageLibrary[image].Show(sequence.OffsetX + pixelX, sequence.OffsetY + pixelY, lockedSurface);
                            }
                        }

                        sequence = sequence.Next;
                    }
                }

                if (!showing)
                {
                    _animationFrame = GetEngine().AnimationDetail == 1 ? (-20 - Random(25)).TruncateToSByte() : (-10 - Random(25)).TruncateToSByte();
                }
            }
        }

        base.ShowDynamic(pixelXSurface, pixelYSurface, lockedSurface);
        CustomImageLibrary imageLibrary = ((MapObject)this).Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
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
                    imageLibrary[ShoreTerrain[1 - 1] + 6].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0011) == 1)
                    {
                        imageLibrary[ShoreTerrain[1 - 1] + 13].Show(x, y, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0111) == 0b0111)
                {
                    imageLibrary[ShoreTerrain[2 - 1] + 25].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0011) == 0b0011)
                    {
                        imageLibrary[variant + 3 + ShoreTerrain[2 - 1]].Show(x, y, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0011_0001) == 0b0011_0001)
                {
                    imageLibrary[ShoreTerrain[6 - 1] + 24].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0001) == 0b0010_0001)
                    {
                        imageLibrary[variant + ShoreTerrain[6 - 1]].Show(x, y, lockedSurface);
                    }
                }
            }
            else
            {
                if ((OpenSides & 0b0110) == 0b0110)
                {
                    imageLibrary[variant + 10 + ShoreTerrain[3 - 1]].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010) == 0b0010)
                    {
                        imageLibrary[ShoreTerrain[2 - 1] + 17].Show(x, y, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0011_0000) == 0b0011_0000)
                {
                    imageLibrary[variant + 7 + ShoreTerrain[5 - 1]].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0000) == 0b0010_0000)
                    {
                        imageLibrary[ShoreTerrain[6 - 1] + 16].Show(x, y, lockedSurface);
                    }
                }
            }

            if ((OpenSides & 0b1000) != 0)
            {
                if ((OpenSides & 0b1110) == 0b1110)
                {
                    imageLibrary[ShoreTerrain[3 - 1] + 22].Show(x, y + 1, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b1100) == 0b1100)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[ShoreTerrain[3 - 1] + 19].Show(x, y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 30 + ShoreTerrain[3 - 1]].Show(x, y + 1, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 0b0011_1000) == 0b0011_1000)
                {
                    imageLibrary[ShoreTerrain[5 - 1] + 21].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0001_1000) == 0b0001_1000)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[ShoreTerrain[5 - 1] + 18].Show(x, y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 28 + ShoreTerrain[5 - 1]].Show(x, y, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 0b0001_1100) == 0b1000)
                {
                    imageLibrary[ShoreTerrain[4 - 1] + 23].Show(x, y, lockedSurface);
                }
            }
            else
            {
                if ((OpenSides & 0b1110) == 0b0100)
                {
                    imageLibrary[ShoreTerrain[3 - 1] + 15].Show(x, y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0011_1000) == 0b0001_0000)
                    {
                        imageLibrary[ShoreTerrain[5 - 1] + 14].Show(x, y, lockedSurface);
                    }
                }
            }
        }
    }
}
