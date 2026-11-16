#nullable disable
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
    private readonly ObservableNode _holder;

    // Each holder admits only items (AcceptsChild), so every slot holds an item or is empty.
    internal ItemCollection(ItemList holder) => _holder = holder;

    internal ItemCollection(HeroInventory holder) => _holder = holder;

    internal ItemCollection(HeroItems holder) => _holder = holder;

    internal ItemCollection(ItemLibrary holder) => _holder = holder;

    /// <summary>The list, inventory, worn items or library that holds the items.</summary>
    public ObservableNode Holder => _holder;

    public int Count => _holder.GetCount();

    // The item in slot index, read as the original reads the list, without a range check. The
    // holder admits only items. Another class throws, as the cast did. Null stays null.
    public Item this[int index] => _holder.GetChild<Item>(index);

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
        Item result = index >= 0 && index < Count ? this[index] : null;
        return result;
    }

    // Army.PAS:4570
    public Item FindItem(int itemId)
    {
        Item result = null;
        int i = 0;
        while (i != Count && result == null)
        {
            Item item = this[i];
            if (item != null && itemId == item.ItemId)
            {
                result = item;
            }

            i += 1;
        }

        return result;
    }

    // Army.PAS:4593
    public void Update() => SortOnType();

    // Army.PAS:4609
    public void SortOnName() => _holder.SortChildrenStable<Item>(ItemList.CompareItemNames);

    // Army.PAS:4619
    public void SortOnType() => _holder.SortChildrenStable<Item>(CompareItemTypes);

    // Army.PAS:4622
    public void RegisterItems()
    {
        Item item;
        for (int i = Count - 1; i >= 0; i--)
        {
            item = this[i];
            if (item != null && !item.Status.Contains(ItemStatusElement.Registered))
            {
                StrategicMap.Current.ItemControl.RegisterItem(item);
            }
        }
    }

    private static int CompareItemTypes(Item item1, Item item2) => item1.ItemType - item2.ItemType;
}
