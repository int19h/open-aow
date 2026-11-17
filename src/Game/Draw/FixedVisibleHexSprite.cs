// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// a sprite that is always in the visibility list (not on a field). FStatus: bit 1 visible
public partial class FixedVisibleHexSprite(CustomNode owner) : HexSprite(owner)
{
    private byte _status;

    // HXSPRITE.PAS:770
    public override void SetVisible(bool value) => _status = value ? (byte)(_status | 1) : (byte)(_status & 254);

    // HXSPRITE.PAS:775
    public override bool GetVisible() => (_status & 1) != 0;

    // HXSPRITE.PAS:787
    public override int GetShowPriority() => GetLevel() << 26;

    // HXSPRITE.PAS:791
    public override void MainRemove()
    {
        Deactivate();
        Remove();
        Assert(!GetVisible(), "FixedVisibleHS still in VisibilityList!!!", "HXSPRITE.PAS", 795);
    }

    // HXSPRITE.PAS:779
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if ((_status & 1) == 0)
        {
            _status = (byte)(_status | 1);
            _ = visibilityList.AddSortChild(this, GetShowPriority());
        }
    }
}
