#nullable disable
#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class WaterHex
{
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        int X;
        int Y;
        CustomImageLibrary imageLibrary;
        int variant;
        ImageSequence sequence;
        int image;
        bool showing;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref pixelX, ref pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY = pixelY - pixelYSurface;
        // { Original uses the shared engine settings and the Flood enchantment. }
        if (GameTables.GameEngine.AnimationDetail != 0 && (((((AoWMapField)Owner).FieldFlags >> 16) & 255) != 0 || !((GameMap)((MapLevel)Owner.Owner).Map).FogEnabled))
        {
            _animationFrame = (sbyte)(_animationFrame + 1);
            if (_animationFrame >= 0)
            {
                showing = false;
                if (_animationFrame == 0)
                {
                    // { Original tests the Flood enchantment. }
                    if (AoWHSMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.Flood))
                    {
                        if (OpenSides == 0 && Random(300) == 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                        {
                            _animationIndex = (byte)(Random(10) + 30);
                        }
                        else
                        {
                            if (Random(10) < 3)
                            {
                                _animationIndex = (byte)(Random(9) + 50);
                            }
                            else
                            {
                                _animationIndex = (byte)(Random(10) + 20);
                            }
                        }
                    }
                    else
                    {
                        if (OpenSides == 0 && Random(700) == 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                        {
                            _animationIndex = (byte)(Random(10) + 30);
                        }
                        else
                        {
                            _animationIndex = (byte)(Random(10) + 20);
                        }
                    }
                }

                if (_animationIndex < _resource.ImageSequenceList.Count)
                {
                    sequence = (ImageSequence)_resource.ImageSequenceList.List[_animationIndex];
                    while (sequence != null)
                    {
                        image = sequence.GetUnloopedFrameIndex(_animationFrame);
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
                    if (GameTables.GameEngine.AnimationDetail == 1)
                    {
                        _animationFrame = (sbyte)(-20 - Random(25));
                    }
                    else
                    {
                        _animationFrame = (sbyte)(-10 - Random(25));
                    }
                }
            }
        }

        base.ShowDynamic(pixelXSurface, pixelYSurface, lockedSurface);
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
                    imageLibrary[ShoreTerrain[1 - 1] + 6].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 35) == 1)
                    {
                        imageLibrary[ShoreTerrain[1 - 1] + 13].Show(X, Y, lockedSurface);
                    }
                }

                if ((OpenSides & 7) == 7)
                {
                    imageLibrary[ShoreTerrain[2 - 1] + 25].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 3) == 3)
                    {
                        imageLibrary[variant + 3 + ShoreTerrain[2 - 1]].Show(X, Y, lockedSurface);
                    }
                }

                if ((OpenSides & 49) == 49)
                {
                    imageLibrary[ShoreTerrain[6 - 1] + 24].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 33) == 33)
                    {
                        imageLibrary[variant + ShoreTerrain[6 - 1]].Show(X, Y, lockedSurface);
                    }
                }
            }
            else
            {
                if ((OpenSides & 6) == 6)
                {
                    imageLibrary[variant + 10 + ShoreTerrain[3 - 1]].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 2) == 2)
                    {
                        imageLibrary[ShoreTerrain[2 - 1] + 17].Show(X, Y, lockedSurface);
                    }
                }

                if ((OpenSides & 48) == 48)
                {
                    imageLibrary[variant + 7 + ShoreTerrain[5 - 1]].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 32) == 32)
                    {
                        imageLibrary[ShoreTerrain[6 - 1] + 16].Show(X, Y, lockedSurface);
                    }
                }
            }

            if ((OpenSides & 8) != 0)
            {
                if ((OpenSides & 14) == 14)
                {
                    imageLibrary[ShoreTerrain[3 - 1] + 22].Show(X, Y + 1, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 12) == 12)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[ShoreTerrain[3 - 1] + 19].Show(X, Y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 30 + ShoreTerrain[3 - 1]].Show(X, Y + 1, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 56) == 56)
                {
                    imageLibrary[ShoreTerrain[5 - 1] + 21].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 24) == 24)
                    {
                        if (variant == 2)
                        {
                            imageLibrary[ShoreTerrain[5 - 1] + 18].Show(X, Y + 1, lockedSurface);
                        }
                        else
                        {
                            imageLibrary[variant + 28 + ShoreTerrain[5 - 1]].Show(X, Y, lockedSurface);
                        }
                    }
                }

                if ((OpenSides & 28) == 8)
                {
                    imageLibrary[ShoreTerrain[4 - 1] + 23].Show(X, Y, lockedSurface);
                }
            }
            else
            {
                if ((OpenSides & 14) == 4)
                {
                    imageLibrary[ShoreTerrain[3 - 1] + 15].Show(X, Y, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 56) == 16)
                    {
                        imageLibrary[ShoreTerrain[5 - 1] + 14].Show(X, Y, lockedSurface);
                    }
                }
            }
        }
    }

}
