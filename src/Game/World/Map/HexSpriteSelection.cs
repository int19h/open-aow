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
public partial class HexSpriteSelection : EControlStyleNode
{
    internal List<HexSprite> _locationHexSprite;

    internal EventStore<ECustomNode> _onSelectStore;

    internal event NotifyEvent<ECustomNode> FOnSelect
    {
        add => _onSelectStore.Add(value);
        remove => _onSelectStore.Remove(value);
    }

    internal EventStore<ECustomNode> _onDeselectStore;

    internal event NotifyEvent<ECustomNode> FOnDeselect
    {
        add => _onDeselectStore.Add(value);
        remove => _onDeselectStore.Remove(value);
    }

    internal HexSprite _eventHexSprite;

    // HSEngine.pas:3681
    public HexSpriteSelection(ECustomNode owner) : base(owner)
    {
        _locationHexSprite = new List<HexSprite>();
        _onSelectStore = new();
        _onDeselectStore = new();
    }

    // HSEngine.pas:3689
    public override void Dispose()
    {
        base.Dispose();
        _locationHexSprite?.Clear();
        _onSelectStore.Clear();
        _onDeselectStore.Clear();
    }

    // HSEngine.pas:3761
    public override void Clear()
    {
        _children.Clear();
        _locationHexSprite.Clear();
    }

    // HSEngine.pas:3699
    public void TriggerDeselectEvent(HexSprite hexSprite)
    {
        _eventHexSprite = hexSprite;
        _onDeselectStore.Raise(Owner);
    }

    // HSEngine.pas:3705
    public void TriggerSelectEvent(HexSprite hexSprite)
    {
        _eventHexSprite = hexSprite;
        _onSelectStore.Raise(Owner);
    }

    // HSEngine.pas:3711
    public void Unselect(HexSprite hexSprite)
    {
        int I;
        I = _children.IndexOf(hexSprite);
        _locationHexSprite.Remove(hexSprite);
        if (I != -1)
        {
            _children.Remove(hexSprite);
            if (_children.IndexOf(hexSprite) == -1)
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
        _children.Add(hexSprite);
        hexSprite.Select();
        TriggerSelectEvent(hexSprite);
    }

    // HSEngine.pas:3734
    public void UnselectAll()
    {
        while (_children.Count != 0)
        {
            Unselect((HexSprite)_children.List[_children.Count - 1]);
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
            hexSprite = (HexSprite)_children.List[I];
            if (classValue.IsInstanceOfType(hexSprite))
            {
                result = (HexSprite)_children.List[I];
            }

            I += 1;
        }

        return result;
    }

    // HSEngine.pas:3756
    public HexSprite GetHexSprite(int index) => (HexSprite)_children.List[index];

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

    public sealed class HexSpriteAccessor(HexSpriteSelection target)
    {
        public HexSprite this[int index] { get => target.GetHexSprite(index); }
    }

    public HexSpriteAccessor HexSprite => new HexSpriteAccessor(this);
    public List<HexSprite> LocationHexSprite { get => _locationHexSprite; }

    public event NotifyEvent<ECustomNode> OnSelect
    {
        add => _onSelectStore.Add(value);
        remove => _onSelectStore.Remove(value);
    }

    public event NotifyEvent<ECustomNode> OnDeselect
    {
        add => _onDeselectStore.Add(value);
        remove => _onDeselectStore.Remove(value);
    }

    public HexSprite EventHexSprite { get => _eventHexSprite; }

}
