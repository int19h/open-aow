// Translated from IsoCombatHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class IsometricCombatHex : UpperIsometricHex
{
    public IsometricCombatHex(ECustomNode owner) : base(owner)
    { }

    // IsoCombatHex.pas:40
    public new const int ClassId = 2229514;

    // IsoCombatHex.pas:43
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // IsoCombatHex.pas:46
    public override void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        CustomImageLibrary imageLibrary;
        int pixelX = default;
        int pixelY = default;
        int variant;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY = pixelY - pixelYSurface;
        imageLibrary = _resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        variant = FrameFixtureInputs.CombatTextureVariant(this);
        if (variant == 3)
        {
            variant = 0;
        }

        imageLibrary[(int)(((uint)Sides) >> 6) + 40].Show(pixelX, pixelY, lockedSurface);
        if (OpenSides != 0)
        {
            pixelY -= 38;
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
                        imageLibrary[variant + 0].Show(pixelX - 19 + 16 + 16, pixelY - 16, lockedSurface);
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

    public new partial class Resource : UpperIsometricHex.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // IsoCombatHex.pas:108
        public new const int ClassId = 2229515;

        // IsoCombatHex.pas:114
        public override int ResourceUserClassId() => 2229514;

        // IsoCombatHex.pas:117
        public override HexControl GetHexControl() => Engine.HexSpriteSet.CombatHexes;

        // IsoCombatHex.pas:111
        public override string ResourceEditName() => "Combat ISO-Hex";

    }

}
