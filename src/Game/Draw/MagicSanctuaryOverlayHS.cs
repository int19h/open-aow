// Translated from MagicSanctuary.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Draw;

// FStructure. FFadeEndFrame
public partial class MagicSanctuaryOverlayHS : SingleHS
{
    public MagicSanctuaryOverlayHS(ECustomNode owner) : base(owner)
    {
        FFadeEndFrame = -1;
    }

    public MagicSanctuary FStructure;
    public int FFadeEndFrame;

    // MagicSanctuary.pas:67

    // MagicSanctuary.pas:73
    public override int GetShowPriority() => FStructure.GetShowPriority() + 134217728;

    // MagicSanctuary.pas:78
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int FrameIndex;
        int Blend;
        int Xhp = default;
        int Yhp = default;
        int FramesLeft;
        HexGeometry.HXtoHP(FStructure.FXhx, FStructure.FYhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        Sequence = FStructure.FResource.ImageSequenceList.GetImageSequence(19);
        if (Sequence != null)
        {
            Blend = 50;
            if (FFadeEndFrame != -1 && FFadeEndFrame > AoWHSMap.Current.Frame)
            {
                FramesLeft = FFadeEndFrame - AoWHSMap.Current.Frame;
                if (FramesLeft <= 20)
                {
                    Blend += (10 - Math.Abs(FramesLeft - 10)) * 3;
                }
                else
                {
                    FFadeEndFrame = -1;
                }
            }
            else
            {
                FFadeEndFrame = -1;
            }

            FrameIndex = Sequence.GetFrameIndex(0, 0);
            Sequence.ImageLibrary[FrameIndex].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, GfxState.IntensityBlendTable.LookupTables[Blend]);
        }
    }

    // MagicSanctuary.pas:105
    public void FadeUp()
    {
        FFadeEndFrame = AoWHSMap.Current.Frame + 20;
    }

    // MagicSanctuary.pas:22

}
