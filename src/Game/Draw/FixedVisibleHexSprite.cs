// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Draw;

// a sprite that is always in the visibility list (not on a field). FStatus: bit 1 visible
public class FixedVisibleHexSprite(CustomNode owner) : HexSprite(owner)
{
    private int _status;

    // HXSPRITE.PAS:775
    public override bool Visible
    {
        get => (_status & 1) != 0;
        // HXSPRITE.PAS:770
        set => _status = value ? _status | 1 : _status & 0b1111_1110;
    }

    // HXSPRITE.PAS:787
    public override int GetShowPriority() => Level << 26;

    // HXSPRITE.PAS:791
    public override void MainRemove()
    {
        Deactivate();
        Remove();
        Assert(!Visible, message: "FixedVisibleHS still in VisibilityList!!!", file: "HXSPRITE.PAS", line: 795);
    }

    // HXSPRITE.PAS:779
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if ((_status & 1) == 0)
        {
            _status |= 1;
            _ = visibilityList.AddSortChild(this, sortValue: GetShowPriority());
        }
    }
}
