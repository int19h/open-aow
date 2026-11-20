// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the sorted list of the visible sprites of a scene (THSMap). a sprite removed from it is made
// invisible
public sealed class VisibilityList(CustomNode owner) : ControlStyleInverseSortedNode(owner), IChildList<HexSprite>, IOwned<GameMap>, IOwnedSink<GameMap>
{

    // The sprite in slot index, read from the sorted items as the original reads them.
    // AcceptsChild admits only a sprite, so the cast never fails.
    public HexSprite this[int index] => GetChild<HexSprite>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is HexSprite;

    int IChildSink<HexSprite>.AddChild(HexSprite child) => AddChild(child);

    // The map the list belongs to. AcceptsOwner admits only a map, so the cast never fails.
    public new GameMap Owner => GetOwner<GameMap>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is GameMap;

    void IOwnedSink<GameMap>.SetOwner(GameMap owner) => SetOwner(owner);

    // HSEngine.pas:3871
    public override void QuickClear() => StoredCount = 0;

    // HSEngine.pas:3876
    public override int RemoveChild(EngineObject child)
    {
        // The list stores hex sprites, so the override's EngineObject child always is one.
        HexSprite hexSprite = (HexSprite)child;
        if (hexSprite.Visible)
        {
            hexSprite.Visible = false;
        }

        return base.RemoveChild(child);
    }

    // HSEngine.pas:3866
    public override void Grow() => Resize(Capacity + 500);

}
