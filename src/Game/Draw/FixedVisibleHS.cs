// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// a sprite that is always in the visibility list (not on a field). FStatus: bit 1 visible
public partial class FixedVisibleHS : HexagonSprite
{
    public FixedVisibleHS(ECustomNode owner) : base(owner)
    { }

    internal byte FStatus;

    // HXSPRITE.PAS:770
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FStatus = (byte)(FStatus | 1);
        }
        else
        {
            FStatus = (byte)(FStatus & 254);
        }
    }

    // HXSPRITE.PAS:775
    public override bool GetVisible() => (FStatus & 1) != 0;

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
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if ((FStatus & 1) == 0)
        {
            FStatus = (byte)(FStatus | 1);
            VisibilityList.AddSortChild(this, GetShowPriority());
        }
    }
}
