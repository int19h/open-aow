// Translated from MagicSanctuary.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Draw;

// Structure. FadeEndFrame
public partial class MagicSanctuaryOverlayHexSprite : SingleHexSprite
{
    public MagicSanctuaryOverlayHexSprite(ECustomNode owner) : base(owner)
    {
        FadeEndFrame = -1;
    }

    public MagicSanctuary Structure;
    public int FadeEndFrame;

    // MagicSanctuary.pas:67

    // MagicSanctuary.pas:73
    public override int GetShowPriority() => Structure.GetShowPriority() + 134217728;

    // MagicSanctuary.pas:78
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int frameIndex;
        int blend;
        int pixelX = default;
        int pixelY = default;
        int framesLeft;
        HexGeometry.HexToPixel(Structure._hexX, Structure._hexY, ref pixelX, ref pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        sequence = Structure._resource.ImageSequenceList.GetImageSequence(19);
        if (sequence != null)
        {
            blend = 50;
            if (FadeEndFrame != -1 && FadeEndFrame > AoWHSMap.Current.Frame)
            {
                framesLeft = FadeEndFrame - AoWHSMap.Current.Frame;
                if (framesLeft <= 20)
                {
                    blend += (10 - Math.Abs(framesLeft - 10)) * 3;
                }
                else
                {
                    FadeEndFrame = -1;
                }
            }
            else
            {
                FadeEndFrame = -1;
            }

            frameIndex = sequence.GetFrameIndex(0, 0);
            sequence.ImageLibrary[frameIndex].ShowBlended(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[blend]);
        }
    }

    // MagicSanctuary.pas:105
    public void FadeUp()
    {
        FadeEndFrame = AoWHSMap.Current.Frame + 20;
    }

    // MagicSanctuary.pas:22

}
