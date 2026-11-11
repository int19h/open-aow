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
    internal List<HexSprite> FLocationHS;

    internal EventStore<ECustomNode> FOnSelectStore;

    internal event NotifyEvent<ECustomNode> FOnSelect
    {
        add => FOnSelectStore.Add(value);
        remove => FOnSelectStore.Remove(value);
    }

    internal EventStore<ECustomNode> FOnDeselectStore;

    internal event NotifyEvent<ECustomNode> FOnDeselect
    {
        add => FOnDeselectStore.Add(value);
        remove => FOnDeselectStore.Remove(value);
    }

    internal HexSprite FEventHS;

    // HSEngine.pas:3681
    public HexSpriteSelection(ECustomNode owner) : base(owner)
    {
        FLocationHS = new List<HexSprite>();
        FOnSelectStore = new();
        FOnDeselectStore = new();
    }

    // HSEngine.pas:3689
    public override void Dispose()
    {
        base.Dispose();
        FLocationHS?.Clear();
        FOnSelectStore.Clear();
        FOnDeselectStore.Clear();
    }

    // HSEngine.pas:3761
    public override void Clear()
    {
        FChildren.Clear();
        FLocationHS.Clear();
    }

    // HSEngine.pas:3699
    public void TriggerDeselectEvent(HexSprite HS)
    {
        FEventHS = HS;
        FOnDeselectStore.Raise(Owner);
    }

    // HSEngine.pas:3705
    public void TriggerSelectEvent(HexSprite HS)
    {
        FEventHS = HS;
        FOnSelectStore.Raise(Owner);
    }

    // HSEngine.pas:3711
    public void Unselect(HexSprite HS)
    {
        int I;
        I = FChildren.IndexOf(HS);
        FLocationHS.Remove(HS);
        if (I != -1)
        {
            FChildren.Remove(HS);
            if (FChildren.IndexOf(HS) == -1)
            {
                if (HS.GetSelected())
                {
                    HS.Unselect();
                }
            }

            TriggerDeselectEvent(HS);
        }
    }

    // HSEngine.pas:3727
    public void Select(HexSprite HS)
    {
        FChildren.Add(HS);
        HS.Select();
        TriggerSelectEvent(HS);
    }

    // HSEngine.pas:3734
    public void UnselectAll()
    {
        while (FChildren.Count != 0)
        {
            Unselect((HexSprite)FChildren.List[FChildren.Count - 1]);
        }
    }

    // HSEngine.pas:3740
    public HexSprite FindClass(Type AClass)
    {
        HexSprite HS;
        int I;
        HexSprite Result = null;
        I = 0;
        while (I != Count && Result == null)
        {
            HS = (HexSprite)FChildren.List[I];
            if (AClass.IsInstanceOfType(HS))
            {
                Result = (HexSprite)FChildren.List[I];
            }

            I += 1;
        }

        return Result;
    }

    // HSEngine.pas:3756
    public HexSprite GetHS(int Index) => (HexSprite)FChildren.List[Index];

    // HSEngine.pas:3790
    public void SelectLocationHS(HexSprite HS)
    {
        FLocationHS.Add(HS);
        Select(HS);
    }

    // HSEngine.pas:3829
    public void SelectLocationHX(int Xhx, int Yhx, int Lhx)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
        SelectLocation(Xhp, Yhp, Lhx);
    }

    // HSEngine.pas:3837
    public void UnselectLocation()
    {
        while (FLocationHS.Count != 0)
        {
            Unselect(FLocationHS[FLocationHS.Count - 1]);
        }
    }

    public sealed class HexSpriteAccessor(HexSpriteSelection target)
    {
        public HexSprite this[int Index] { get => target.GetHS(Index); }
    }

    public HexSpriteAccessor HS => new HexSpriteAccessor(this);
    public List<HexSprite> LocationHS { get => FLocationHS; }

    public event NotifyEvent<ECustomNode> OnSelect
    {
        add => FOnSelectStore.Add(value);
        remove => FOnSelectStore.Remove(value);
    }

    public event NotifyEvent<ECustomNode> OnDeselect
    {
        add => FOnDeselectStore.Add(value);
        remove => FOnDeselectStore.Remove(value);
    }

    public HexSprite EventHS { get => FEventHS; }

}
