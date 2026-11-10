// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// FFrame. FOffsetX. FOffsetY
public partial class CBloodHS : SingleHS
{
    public CBloodHS(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2228487;

    public int FFrame;
    public int FOffsetX;
    public int FOffsetY;

    // AoWTC.pas:6393
    public override byte GetLevel() => 26;

    // AoWTC.pas:6398
    public override int GetShowPriority() => 1744830464 + (FOffsetY << 13) + FOffsetX;

    // AoWTC.pas:6403
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Frame;
        ImageSequence Sequence;
        int X = default;
        int Y = default;
        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[4];
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
        if (Sequence != null)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrame);
            Sequence.ImageLibrary[Frame].ShowClipped(X - XhpSurf + FOffsetX, Y - YhpSurf + FOffsetY, SurfaceDesc);
        }
    }

    // AoWTC.pas:6414
    public override void Activate()
    {
        base.Activate();
    }

    // AoWTC.pas:6383
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // Engine.pas:524

}
