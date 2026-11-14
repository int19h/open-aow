using System.Collections;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

/// <summary>
/// The items of an item holder, in the holder's child slots and their stored order. The holder
/// owns the slots. It checks each item that enters, notifies its changes, routes messages to its
/// items, frees them and stores them as its children. This collection reads them typed and sorts
/// them. A default value has no holder and throws on use, as a missing list does.
/// </summary>
public readonly struct ItemCollection : IReadOnlyList<Item>
{
    private readonly EChangeNotifyNode _holder;

    // Each holder admits only items (AcceptsChild), so every slot holds an item or is empty.
    internal ItemCollection(ItemList holder) => _holder = holder;

    internal ItemCollection(HeroInventory holder) => _holder = holder;

    internal ItemCollection(HeroItems holder) => _holder = holder;

    internal ItemCollection(ItemLibrary holder) => _holder = holder;

    /// <summary>The list, inventory, worn items or library that holds the items.</summary>
    public EChangeNotifyNode Holder => _holder;

    public int Count => _holder.GetCount();

    // The item in slot index, read as the original reads the list, without a range check. The
    // holder admits only items, so the cast never fails.
    public Item this[int index] => (Item)_holder.GetChild(index);

    public IEnumerator<Item> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Army.PAS:4587
    public Item GetItem(int index)
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

    // Army.PAS:4570
    public Item FindItem(int itemId)
    {
        Item result = null;
        int I = 0;
        while (I != Count && result == null)
        {
            Item item = this[I];
            if (item != null && itemId == item.ItemId)
            {
                result = item;
            }

            I += 1;
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
        _holder.SortChildrenStable<Item>(ItemList.CompareItemNames);
    }

    // Army.PAS:4619
    public void SortOnType()
    {
        _holder.SortChildrenStable<Item>(CompareItemTypes);
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

    private static int CompareItemTypes(Item item1, Item item2)
    {
        return item1.ItemType - item2.ItemType;
    }
}
