// Translated from IsoHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class UpperIsometricHex(CustomNode owner) : IsometricHex(owner)
{
    public new const int ClassId = 131440;

    // IsoHex.pas:355
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        CustomImageLibrary imageLibrary;
        if (Dynamic is null)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY -= pixelYSurface;
            imageLibrary = base.Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
            imageLibrary[(int)(((uint)Sides) >> 6) + 40].Show(pixelX, pixelY, lockedSurface);
            pixelX -= 6;
            pixelY -= 9;
            if ((Sides & 1) != 0)
            {
                imageLibrary[31].Show(pixelX, pixelY, lockedSurface);
            }

            if ((Sides & 2) != 0)
            {
                imageLibrary[32].Show(pixelX, pixelY, lockedSurface);
            }

            if ((Sides & 32) != 0)
            {
                imageLibrary[30].Show(pixelX, pixelY, lockedSurface);
            }
        }
    }

    public new partial class Resource(CustomNode owner) : IsometricHexResource(owner)
    {
        public new const int ClassId = 131441;

        // IsoHex.pas:443
        public override int ResourceUserClassId() => 131440;

        // IsoHex.pas:448
        public override string ResourceEditName() => s_upperIsometricHexText;

        private static readonly string s_upperIsometricHexText = "Upper Isometric Hexagon";

    }

}
