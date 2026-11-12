// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the sorted list of the visible sprites of a scene (THSMap). a sprite removed from it is made
// invisible
public partial class VisibilityList : EControlStyleInverseSortNode
{
    public VisibilityList(ECustomNode owner) : base(owner)
    { }

    // HSEngine.pas:3871
    public override void QuickClear()
    {
        _count = 0;
    }

    // HSEngine.pas:3876
    public override int RemoveChild(EngineObject child)
    {
        var hexSprite = (HexSprite)child;
        if (hexSprite.GetVisible())
        {
            hexSprite.SetVisible(false);
        }

        return base.RemoveChild(child);
    }

    // HSEngine.pas:3866
    public override void Grow()
    {
        Resize(Capacity + 500);
    }

}
