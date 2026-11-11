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
public partial class HexagonSpriteSelection : EControlStyleNode
{
    internal List<HexagonSprite> FLocationHS;

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

    internal HexagonSprite FEventHS;

    // HSEngine.pas:3681
    public HexagonSpriteSelection(ECustomNode owner) : base(owner)
    {
        FLocationHS = new List<HexagonSprite>();
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
    public void TriggerDeselectEvent(HexagonSprite HS)
    {
        FEventHS = HS;
        FOnDeselectStore.Raise(Owner);
    }

    // HSEngine.pas:3705
    public void TriggerSelectEvent(HexagonSprite HS)
    {
        FEventHS = HS;
        FOnSelectStore.Raise(Owner);
    }

    // HSEngine.pas:3711
    public void Unselect(HexagonSprite HS)
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
    public void Select(HexagonSprite HS)
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
            Unselect((HexagonSprite)FChildren.List[FChildren.Count - 1]);
        }
    }

    // HSEngine.pas:3740
    public HexagonSprite FindClass(Type AClass)
    {
        HexagonSprite HS;
        int I;
        HexagonSprite Result = null;
        I = 0;
        while (I != Count && Result == null)
        {
            HS = (HexagonSprite)FChildren.List[I];
            if (AClass.IsInstanceOfType(HS))
            {
                Result = (HexagonSprite)FChildren.List[I];
            }

            I += 1;
        }

        return Result;
    }

    // HSEngine.pas:3756
    public HexagonSprite GetHS(int Index) => (HexagonSprite)FChildren.List[Index];

    // HSEngine.pas:3790
    public void SelectLocationHS(HexagonSprite HS)
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

    public sealed class HSAccessor(HexagonSpriteSelection target)
    {
        public HexagonSprite this[int Index] { get => target.GetHS(Index); }
    }

    public HSAccessor HS => new HSAccessor(this);
    public List<HexagonSprite> LocationHS { get => FLocationHS; }

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

    public HexagonSprite EventHS { get => FEventHS; }

}
