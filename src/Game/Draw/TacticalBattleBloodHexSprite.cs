// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// Frame. OffsetX. OffsetY
public partial class TacticalBattleBloodHexSprite : SingleHexSprite
{
    public TacticalBattleBloodHexSprite(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 2228487;

    public int Frame { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }

    // AoWTC.pas:6393
    public override byte GetLevel() => 26;

    // AoWTC.pas:6398
    public override int GetShowPriority() => 1744830464 + (OffsetY << 13) + OffsetX;

    // AoWTC.pas:6403
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame;
        ImageSequence sequence;
        int X = default;
        int Y = default;
        sequence = AoWHSSet.Current.ImageSequences.ImageSequences[4];
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref X, ref Y);
        if (sequence != null)
        {
            frame = sequence.GetUnloopedFrameIndex(Frame);
            sequence.ImageLibrary[frame].ShowClipped(X - pixelXSurface + OffsetX, Y - pixelYSurface + OffsetY, lockedSurface);
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
