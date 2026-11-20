#nullable disable
// Translated from MagicSanctuary.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Draw;

// Structure. FadeEndFrame
public sealed class MagicSanctuaryOverlayHexSprite(CustomNode owner) : SingleHexSprite(owner)
{

    public MagicSanctuary Structure { get; init; }

    private int _fadeEndFrame = -1;

    // MagicSanctuary.pas:73
    public override int GetShowPriority() => Structure.GetShowPriority() + 134_217_728;

    // MagicSanctuary.pas:78
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Structure.StoredHexX, Structure.StoredHexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        ImageSequence sequence = ((World.MapObject)Structure).Resource.ImageSequenceList.GetImageSequence(19);
        if (sequence is not null)
        {
            int blend = 50;
            if (_fadeEndFrame != -1 && _fadeEndFrame > StrategicMap.Current.Frame)
            {
                int framesLeft = _fadeEndFrame - StrategicMap.Current.Frame;
                if (framesLeft <= 20)
                {
                    blend += (10 - Math.Abs(framesLeft - 10)) * 3;
                }
                else
                {
                    _fadeEndFrame = -1;
                }
            }
            else
            {
                _fadeEndFrame = -1;
            }

            int frameIndex = sequence.GetFrameIndex(0, offset: 0);
            sequence.ImageLibrary[frameIndex].ShowBlended(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[blend]);
        }
    }

    // MagicSanctuary.pas:105
    public void FadeUp() => _fadeEndFrame = StrategicMap.Current.Frame + 20;
}
