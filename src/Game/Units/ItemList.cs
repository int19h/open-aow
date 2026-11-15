#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited children, update lock. change event
public partial class ItemList : ObservableNode, IChildList<Item>, IItemHolder
{
    /// <summary>The items of this list.</summary>
    public ItemCollection Items => new(this);

    // The child in slot index, read as the original reads the list, without a range check.
    public Item this[int index] => Items[index];

    // An item list takes items only. A list of one kind of item narrows that through AcceptsItem,
    // and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item item && AcceptsItem(item);

    protected virtual bool AcceptsItem(Item item) => true;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    // Army.PAS:4516
    public ItemList(CustomNode owner) : base(owner)
    {

    }

    public event NotifyEvent<object> OnChanged;

    internal void SetChangedHandler(NotifyEvent<object> handler) => OnChanged = handler;

    internal void ClearChangedHandler() => OnChanged = null;

    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                if (GetChild(i) != null)
                {
                    this[i].Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        EngineObject child;
        for (int i = Count - 1; i >= 0; i--)
        {
            if (GetChild(i) != null)
            {
                child = this[i];
                if (child != null)
                {
                    if (child.Owner != this)
                    {
                        child.Release();
                        DeleteChildAt(i);
                    }
                    else
                    {
                        child.SetOwner(null);
                    }
                }
            }
        }

        base.Clear();
    }

    // Army.PAS:4531
    public override void DestroyAllChildren()
    {
        Clear();
        base.DestroyAllChildren();
    }

    // Army.PAS:4519
    public override int RemoveChild(EngineObject child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            child.Release();
        }

        return result;
    }

    // Army.PAS:4525
    public override int AddChild(EngineObject child)
    {
        int result = base.AddChild(child);
        if (result != -1)
        {
            child.AddRef();
        }

        return result;
    }

    // Army.PAS:4564
    protected override void OnChange()
    {
        base.OnChange();
        if (OnChanged != null)
        {
            OnChanged(this);
        }
    }

    // Army.PAS:4587
    public virtual Item GetItem(int index) => Items.GetItem(index);

    // Army.PAS:4596
    public virtual void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Army.PAS:4570
    public Item FindItem(int itemId) => Items.FindItem(itemId);

    // Army.PAS:4593
    public void Update() => Items.Update();

    // Army.PAS:4609
    public void SortOnName() => Items.SortOnName();

    // Army.PAS:4619
    public void SortOnType() => Items.SortOnType();

    // Army.PAS:4622
    public void RegisterItems() => Items.RegisterItems();

    // Army.PAS:4633
    public new void LockChanged()
    {
        base.LockChanged();
    }

    // Army.PAS:4636
    public new void UnlockChanged()
    {
        base.UnlockChanged();
    }

    public static int CompareItemNames(Item item1, Item item2)
    {
        if (item1 != null && item2 != null)
        {
            return RtlUnit.CompareText(item1.OwnerName, item2.OwnerName);
        }
        return 0;
    }

    // intf_P2_04.inc:118

    // intf_P2_04.inc:118

}
