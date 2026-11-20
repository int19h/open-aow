#nullable disable
// Translated from IsoHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

// FDynamic, FOpenSides (bit HN-1: the neighbour HN is no hexagon of this kind), FSides (bits
// 0.5: the neighbour HN has another terrain, bits 6.7: the image variant)
public partial class IsometricHex(CustomNode owner) : AbstractHexagon(owner)
{

    // IsoHex.pas:214
    public new const int ClassId = 131437;

    protected internal DynamicIsometricHex Dynamic { get; set; }
    protected internal byte OpenSides { get; private set; }
    protected internal byte Sides { get; protected set; } = (byte)(Random(4) << 6);

    // IsoHex.pas:204
    public override void Dispose()
    {
        if (Dynamic is not null)
        {
            Dynamic?.Dispose();
            Dynamic = null;
        }

        base.Dispose();
    }

    // IsoHex.pas:198

    // IsoHex.pas:224
    public override void Disconnect()
    {
        if (Dynamic is not null)
        {
            Dynamic?.Dispose();
            Dynamic = null;
        }

        base.Disconnect();
    }

    // IsoHex.pas:234
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info) => UpdateTransitions();

    // IsoHex.pas:239
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        CustomImageLibrary imageLibrary;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelX = pixelX - pixelXSurface - 8;
        pixelY -= pixelYSurface;
        imageLibrary = Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
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

    // IsoHex.pas:256
    public virtual void ShowDynamic(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
    }

    // IsoHex.pas:50

}
