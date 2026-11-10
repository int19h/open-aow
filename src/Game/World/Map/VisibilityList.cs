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
        FCount = 0;
    }

    // HSEngine.pas:3876
    public override int RemoveChild(EObject Child)
    {
        var hexagonSprite = (HexagonSprite)Child;
        if (hexagonSprite.GetVisible())
        {
            hexagonSprite.SetVisible(false);
        }

        return base.RemoveChild(Child);
    }

    // HSEngine.pas:3866
    public override void Grow()
    {
        Resize(FCapacity + 500);
    }

}
