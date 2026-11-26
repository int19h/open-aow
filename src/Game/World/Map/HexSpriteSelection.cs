#nullable disable warnings
// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// the selected sprites of a map. The children (TENode.FChildren,) are every selected sprite.
// FLocationHS the sprites selected by location (clicking on a hexagon), Selected, Deselected
// (triggered with the map as the sender), FEventHS the sprite of the last event. The owner is
// the map
public sealed partial class HexSpriteSelection(CustomNode owner) : ControlStyleNode(owner), IChildList<HexSprite>, IOwned<GameMap>, IOwnedSink<GameMap>
{

    // The sprite in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a sprite, so the cast never fails.
    public HexSprite this[int index] => GetChild<HexSprite>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is HexSprite;

    int IChildSink<HexSprite>.AddChild(HexSprite child) => AddChild(child);

    // The map the selection belongs to. AcceptsOwner admits only a map, so the cast never fails.
    public new GameMap Owner => GetOwner<GameMap>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is GameMap;

    void IOwnedSink<GameMap>.SetOwner(GameMap owner) => SetOwner(owner);

    internal event NotifyEvent<CustomNode>? Selected;

    internal void ClearSelectedHandlers() => Selected = null;

    // Issue 326 keeps the ordered store: TargetSelectionControl.DeselectHexSprite reselects the army
    // in nested raises and rewrites EventHexSprite before MapControl's relay reads it.
    internal EventStore<CustomNode> OnDeselectStore { get; } = new();

    internal event NotifyEvent<CustomNode> Deselected
    {
        add => OnDeselectStore.Add(value);
        remove => OnDeselectStore.Remove(value);
    }

    // HSEngine.pas:3689
    public override void Dispose()
    {
        base.Dispose();
        LocationHexSprite?.Clear();
        Selected = null;
        OnDeselectStore.Clear();
    }

    // HSEngine.pas:3761
    public override void Clear()
    {
        ClearChildren();
        LocationHexSprite.Clear();
    }

    // HSEngine.pas:3699
    public void TriggerDeselectEvent(HexSprite? hexSprite)
    {
        EventHexSprite = hexSprite;
        OnDeselectStore.Raise(Owner);
    }

    // HSEngine.pas:3705
    public void TriggerSelectEvent(HexSprite hexSprite)
    {
        EventHexSprite = hexSprite;
        NotifyEvent<CustomNode>? handlers = Selected;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Selected));
        handlers?.Invoke(Owner);
    }

    // HSEngine.pas:3711
    public void Unselect(HexSprite? hexSprite)
    {
        int i = IndexOfChild(hexSprite);
        _ = LocationHexSprite.Remove(hexSprite);
        if (i != -1)
        {
            _ = DropChild(hexSprite);
            if (IndexOfChild(hexSprite) == -1)
            {
                if (hexSprite.GetSelected())
                {
                    hexSprite.Unselect();
                }
            }

            TriggerDeselectEvent(hexSprite);
        }
    }

    // HSEngine.pas:3727
    public void Select(HexSprite? hexSprite)
    {
        _ = AddChild(hexSprite);
        hexSprite.Select();
        TriggerSelectEvent(hexSprite);
    }

    // HSEngine.pas:3734
    public void UnselectAll()
    {
        while (Count != 0)
        {
            Unselect(this[Count - 1]);
        }
    }

    // HSEngine.pas:3740
    public HexSprite? FindClass(Type classValue)
    {
        HexSprite? result = null;
        int i = 0;
        while (i != Count && result is null)
        {
            HexSprite hexSprite = this[i];
            if (classValue.IsInstanceOfType(hexSprite))
            {
                result = this[i];
            }

            i += 1;
        }

        return result;
    }

    // HSEngine.pas:3756
    public HexSprite GetHexSprite(int index) => this[index];

    // HSEngine.pas:3790
    public void SelectLocationHexSprite(HexSprite? hexSprite)
    {
        LocationHexSprite.Add(hexSprite);
        Select(hexSprite);
    }

    // HSEngine.pas:3829
    public void SelectLocationHex(int hexX, int hexY, int hexLevel)
    {
        HexGeometry.HexToPixel(hexX, hexY, out int pixelX, out int pixelY);
        SelectLocation(pixelX, pixelY, lHitPoints: hexLevel);
    }

    // HSEngine.pas:3837
    public void UnselectLocation()
    {
        while (LocationHexSprite.Count != 0)
        {
            Unselect(LocationHexSprite[^1]);
        }
    }

    public List<HexSprite?> LocationHexSprite { get; } = [];

    public HexSprite? EventHexSprite { get; private set; }
}
