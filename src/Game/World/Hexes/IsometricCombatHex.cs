#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from IsoCombatHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public class IsometricCombatHex(CustomNode owner) : UpperIsometricHex(owner)
{

    // IsoCombatHex.pas:40
    public new const int ClassId = 2_229_514;

    // IsoCombatHex.pas:43
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // IsoCombatHex.pas:46
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY -= pixelYSurface;
        CustomImageLibrary imageLibrary = ((MapObject)this).Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int variant = FrameFixtureInputs.CombatTextureVariant(this);
        if (variant == 3)
        {
            variant = 0;
        }

        imageLibrary[(Sides >>> 6) + 40].Show(pixelX, pixelY, lockedSurface);
        if (OpenSides != 0)
        {
            pixelY -= 38;
            if ((OpenSides & 1) != 0)
            {
                if ((OpenSides & 0b0010_0000) == 0)
                {
                    imageLibrary[18].Show(pixelX - 19, pixelY - 48 - 16, lockedSurface);
                }

                if ((OpenSides & 0b0010) == 0)
                {
                    imageLibrary[19].Show(pixelX - 19, pixelY - 48 - 16, lockedSurface);
                }

                if ((OpenSides & 0b0010_0010) == 0b0010_0010)
                {
                    imageLibrary[23].Show(pixelX - 20, pixelY - 48 - 16, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0010_0010) == 0)
                    {
                        imageLibrary[20].Show(pixelX - 20, pixelY - 50 - 16, lockedSurface);
                    }
                }
            }

            if ((OpenSides & 0b1000) != 0)
            {
                if ((OpenSides & 0b0001_0000) != 0)
                {
                    if ((OpenSides & 0b0010_0000) != 0)
                    {
                        imageLibrary[17].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                    }
                    else
                    {
                        imageLibrary[variant + 3].Show(pixelX - 19 - 32, pixelY - 16, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0100) != 0)
                {
                    if ((OpenSides & 0b0010) != 0)
                    {
                        imageLibrary[16].Show(pixelX - 19 + 16 + 16, pixelY - 16, lockedSurface);
                    }
                    else
                    {
                        imageLibrary[variant + 0].Show(pixelX - 19 + 16 + 16, pixelY - 16, lockedSurface);
                    }
                }

                if ((OpenSides & 0b0001_0100) == 0b0001_0100)
                {
                    imageLibrary[13].Show(pixelX - 19, pixelY, lockedSurface);
                }
                else
                {
                    if ((OpenSides & 0b0001_0100) == 0)
                    {
                        imageLibrary[6].Show(pixelX - 19, pixelY, lockedSurface);
                    }
                }
            }
            else
            {
                if ((OpenSides & 0b0001_0000) != 0)
                {
                    if ((OpenSides & 0b0010_0000) == 0)
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

                if ((OpenSides & 0b0100) != 0)
                {
                    if ((OpenSides & 0b0010) == 0)
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

    public new class Resource(CustomNode owner) : UpperIsometricHex.Resource(owner)
    {

        // IsoCombatHex.pas:108
        public new const int ClassId = 2_229_515;

        // IsoCombatHex.pas:114
        public override int ResourceUserClassId() => IsometricCombatHex.ClassId;

        // IsoCombatHex.pas:117
        public override HexControl HexControl => Engine.HexSpriteSet.CombatHexes;

        // IsoCombatHex.pas:111
        public override string ResourceEditName() => "Combat ISO-Hex";

    }
}
