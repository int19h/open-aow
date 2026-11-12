// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// the selected sprites of a map. The children (TENode.FChildren,) are every selected sprite.
// FLocationHS the sprites selected by location (clicking on a hexagon), FOnSelect, FOnDeselect
// (triggered with the map as the sender), FEventHS the sprite of the last event. The owner is
// the map
public partial class HexSpriteSelection : EControlStyleNode, IChildList<HexSprite>, IOwned<GameMap>, IOwnedSink<GameMap>
{
    // The sprite in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a sprite, so the cast never fails.
    public HexSprite this[int index] => (HexSprite)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Draw.HexSprite;

    int IChildSink<HexSprite>.AddChild(HexSprite child) => AddChild(child);

    // The map the selection belongs to. AcceptsOwner admits only a map, so the cast never fails.
    public new GameMap Owner { get => (GameMap)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is GameMap;

    void IOwnedSink<GameMap>.SetOwner(GameMap owner) => SetOwner(owner);

    private List<HexSprite> _locationHexSprite;

    internal EventStore<ECustomNode> OnSelectStore { get; }

    internal event NotifyEvent<ECustomNode> FOnSelect
    {
        add => OnSelectStore.Add(value);
        remove => OnSelectStore.Remove(value);
    }

    internal EventStore<ECustomNode> OnDeselectStore { get; }

    internal event NotifyEvent<ECustomNode> FOnDeselect
    {
        add => OnDeselectStore.Add(value);
        remove => OnDeselectStore.Remove(value);
    }

    private HexSprite _eventHexSprite;

    // HSEngine.pas:3681
    public HexSpriteSelection(ECustomNode owner) : base(owner)
    {
        _locationHexSprite = new List<HexSprite>();
        OnSelectStore = new();
        OnDeselectStore = new();
    }

    // HSEngine.pas:3689
    public override void Dispose()
    {
        base.Dispose();
        _locationHexSprite?.Clear();
        OnSelectStore.Clear();
        OnDeselectStore.Clear();
    }

    // HSEngine.pas:3761
    public override void Clear()
    {
        ClearChildren();
        _locationHexSprite.Clear();
    }

    // HSEngine.pas:3699
    public void TriggerDeselectEvent(HexSprite hexSprite)
    {
        _eventHexSprite = hexSprite;
        OnDeselectStore.Raise(Owner);
    }

    // HSEngine.pas:3705
    public void TriggerSelectEvent(HexSprite hexSprite)
    {
        _eventHexSprite = hexSprite;
        OnSelectStore.Raise(Owner);
    }

    // HSEngine.pas:3711
    public void Unselect(HexSprite hexSprite)
    {
        int I;
        I = IndexOfChild(hexSprite);
        _locationHexSprite.Remove(hexSprite);
        if (I != -1)
        {
            DropChild(hexSprite);
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
    public void Select(HexSprite hexSprite)
    {
        AddChild(hexSprite);
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
    public HexSprite FindClass(Type classValue)
    {
        HexSprite hexSprite;
        int I;
        HexSprite result = null;
        I = 0;
        while (I != Count && result == null)
        {
            hexSprite = this[I];
            if (classValue.IsInstanceOfType(hexSprite))
            {
                result = this[I];
            }

            I += 1;
        }

        return result;
    }

    // HSEngine.pas:3756
    public HexSprite GetHexSprite(int index) => this[index];

    // HSEngine.pas:3790
    public void SelectLocationHexSprite(HexSprite hexSprite)
    {
        _locationHexSprite.Add(hexSprite);
        Select(hexSprite);
    }

    // HSEngine.pas:3829
    public void SelectLocationHex(int hexX, int hexY, int hexLevel)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
        SelectLocation(pixelX, pixelY, hexLevel);
    }

    // HSEngine.pas:3837
    public void UnselectLocation()
    {
        while (_locationHexSprite.Count != 0)
        {
            Unselect(_locationHexSprite[_locationHexSprite.Count - 1]);
        }
    }

    public List<HexSprite> LocationHexSprite { get => _locationHexSprite; }

    public event NotifyEvent<ECustomNode> OnSelect
    {
        add => OnSelectStore.Add(value);
        remove => OnSelectStore.Remove(value);
    }

    public event NotifyEvent<ECustomNode> OnDeselect
    {
        add => OnDeselectStore.Add(value);
        remove => OnDeselectStore.Remove(value);
    }

    public HexSprite EventHexSprite { get => _eventHexSprite; }

}
