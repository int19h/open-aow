// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited children, update lock. change event
public partial class ItemList : EChangeNotifyNode, IChildList<Item>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an Item, so the cast never fails.
    public Item this[int index] => (Item)GetChild(index);

    // An item list takes items only. A list of one kind of item narrows that through AcceptsItem,
    // and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item item && AcceptsItem(item);

    protected virtual bool AcceptsItem(Item item) => true;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    // Army.PAS:4516
    public ItemList(ECustomNode owner) : base(owner)
    {

    }

    public event NotifyEvent<object> OnChanged;

    internal void SetChangedHandler(NotifyEvent<object> handler) => OnChanged = handler;

    internal void ClearChangedHandler() => OnChanged = null;

    // Army.PAS:4537
    // code, data
    public override void MainReadWrite(StorageStream stream)
    {
        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if (GetChild(I) != null)
                {
                    this[I].Release();
                }
            }
        }
    }

    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if (GetChild(I) != null)
                {
                    this[I].Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        EngineObject child;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (GetChild(I) != null)
            {
                child = this[I];
                if (child != null)
                {
                    if (child.Owner != this)
                    {
                        child.Release();
                        DeleteChildAt(I);
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
    public override void Changed()
    {
        base.Changed();
        if (OnChanged != null)
        {
            OnChanged(this);
        }
    }

    // Army.PAS:4587
    public virtual Item GetItem(int index)
    {
        Item result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:4596
    public virtual void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        X = -1;
        Y = -1;
        L = -1;
    }

    // Army.PAS:4570
    public Item FindItem(int itemId)
    {
        Item result;
        int I;
        Item item;
        if (this != null)
        {
            I = 0;
            result = null;
            while (I != Count && result == null)
            {
                item = this[I];
                if (item != null && itemId == item.ItemId)
                {
                    result = item;
                }

                I += 1;
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:4593
    public void Update()
    {
        SortOnType();
    }

    // Army.PAS:4609
    public void SortOnName()
    {
        SortChildrenStable<Item>(CompareItemNames);
    }

    // Army.PAS:4619
    public void SortOnType()
    {
        SortChildrenStable<Item>(CompareItemTypes);
    }

    // Army.PAS:4622
    public void RegisterItems()
    {
        Item item;
        for (int I = Count - 1; I >= 0; I--)
        {
            item = this[I];
            if (item != null && !item.Status.Contains(ItemStatusElement.Registered))
            {
                StrategicMap.Current.ItemControl.RegisterItem(item);
            }
        }
    }

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

    private static int CompareItemTypes(Item item1, Item item2)
    {
        return item1.ItemType - item2.ItemType;
    }

    // intf_P2_04.inc:118

    // intf_P2_04.inc:118

}
