#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// items, change events, update lock, item change events, last ID, status
public class ItemControl(CustomNode owner) : EngineObject(owner)
{

    private readonly List<Item> _items = [];

    private readonly EventStore<ItemControl> _onChangedStore = new();

    public event NotifyEvent<ItemControl> Changed
    {
        add => _onChangedStore.Add(value);
        remove => _onChangedStore.Remove(value);
    }

    private int _updateLock;

    private readonly EventStore<Item> _onItemChangedStore = new();

    public event NotifyEvent<Item> ItemChanged
    {
        add => _onItemChangedStore.Add(value);
        remove => _onItemChangedStore.Remove(value);
    }

    private int _lastId;
    private Set<StatusElement> _status;

    // Army.PAS:4647
    public override void Dispose()
    {
        BeginUpdate();
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            UnregisterItem(_items[i]);
        }

        _items.Clear();
        NotifyChanged();
        _items?.Clear();
        _onItemChangedStore.Clear();
        _onChangedStore.Clear();
        base.Dispose();
    }

    // Army.PAS:4660
    // The original ReadWrite reads ids 10 and on until the first absent or nil item, and writes the items
    // that have no owner at ids 10 and on. The slot is the id less 10. A preview reads and writes
    // no item and leaves a stored one unread.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _lastId = persister.Int32("LastId", _lastId);
        if (persister.Preview)
        {
            persister.Skip("Items");
            return;
        }

        if (persister.Loading)
        {
            int slot = 0;
            Item item;
            do
            {
                item = persister.CreatedSlot<Item>("Items", slot: slot, objectValue: null);
                if (item is not null)
                {
                    ValidateItem(item);
                    _items.Add(item);
                    item.Status += Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
                }

                slot += 1;
            }
            while (item is not null);
            return;
        }

        int next = 0;
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Owner is null)
            {
                _ = persister.CreatedSlot("Items", slot: next, objectValue: _items[i]);
                next += 1;
            }
        }
    }

    // Army.PAS:4707
    public Item GetItem(int index) => _items[index];

    // Army.PAS:4712
    public int GetCount() => _items.Count;

    // Army.PAS:4717
    public Item FindItem(int itemId)
    {
        int i = _items.Count;
        while (i != 0)
        {
            i -= 1;
            if (_items[i].ItemId == itemId)
            {
                return _items[i];
            }
        }

        return null;
    }

    // Army.PAS:4733
    public void NotifyItemChanged(Item item) => _onItemChangedStore.Raise(item);

    // Army.PAS:4738
    public void NotifyChanged()
    {
        if (_updateLock == 0)
        {
            _onChangedStore.Raise(this);
        }
    }

    // Army.PAS:4743
    public void BeginUpdate() => _updateLock += 1;

    // Army.PAS:4748
    public void EndUpdate()
    {
        _updateLock -= 1;
        if (_updateLock == 0)
        {
            NotifyChanged();
        }
    }

    // Army.PAS:4754
    public int CreateUniqueId()
    {
        int i;
        do
        {
            i = 0;
            _lastId += 1;
            if (_lastId > 268435455)
            {
                _lastId = 0;
            }

            while (i != _items.Count && _items[i].ItemId != _lastId)
            {
                i += 1;
            }
        }
        while (!(i == _items.Count));
        return _lastId;
    }

    // Army.PAS:4766
    public void ValidateItem(Item item)
    {
        if (FindItem(item.ItemId) is not null)
        {
            item.ItemId = CreateUniqueId();
        }
    }

    // Army.PAS:4771
    public void UpdateLibraryItems()
    {
        BeginUpdate();
        for (int i = GetCount() - 1; i >= 0; i--)
        {
            _items[i].UpdateFromLibrary();
        }

        EndUpdate();
    }

    // Army.PAS:4779
    public void RegisterItem(Item item)
    {
        if (!item.Status.Contains(ItemStatusElement.Registered))
        {
            Item existing = FindItem(item.ItemId);
            if (existing is not null && existing.Compare(item, GetEngine()) && existing.RefCount == 1)
            {
                UnregisterItem(existing);
            }

            ValidateItem(item);
            item.Status += Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
            _items.Add(item);
            _ = item.AddRef();
            if (_status.Contains(StatusElement.Activated) && !item.Status.Contains(ItemStatusElement.Activated))
            {
                item.Activate();
            }

            NotifyChanged();
        }
    }

    // Army.PAS:4796
    public void UnregisterItem(Item item)
    {
        if (item.Status.Contains(ItemStatusElement.Registered))
        {
            _ = _items.Remove(item);
            item.Status -= Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
            _ = item.Release();
            NotifyChanged();
        }
    }

    // Army.PAS:4807
    public Item GenerateItem(Set<ItemType> types, int minValue, int maxValue)
    {
        DoubleItemList<Item> list = new();
        for (int i = 0, last = GetCount() - 1; i <= last; i++)
        {
            Item item = _items[i];
            if (item.RefCount == 1 && types.Contains(item.ItemType))
            {
                int value = item.GetObtainValue();
                if (value >= minValue && value <= maxValue)
                {
                    _ = list.AddAbility(item, (int)item.Availability);
                }
            }
        }

        Item result = list.Count != 0 ? list[list.RandomChanceItemB(1)].First : null;

        list?.Dispose();
        return result;
    }

    // Army.PAS:4826
    public int GenerateItems(Set<ItemType> types, CustomNode owner, int value)
    {
        int result = 0;
        DoubleItemList<Item> list = new();
        Item item;
        for (int i = 0, last = GetCount() - 1; i <= last; i++)
        {
            item = _items[i];
            if (types.Contains(item.ItemType) && item.RefCount == 1)
            {
                _ = list.AddAbility(item, (int)item.Availability);
            }
        }

        bool done = false;
        while (!done && list.Count != 0)
        {
            Item candidate = list[list.RandomChanceItemB(1)].First;
            int itemValue = candidate.GetObtainValue();
            if (itemValue > value)
            {
                int attempts = 10;
                int difference = Math.Abs(itemValue - value);
                do
                {
                    item = list[list.RandomChanceItemB(1)].First;
                    int distance = Math.Abs(item.GetObtainValue() - value);
                    if (distance < difference)
                    {
                        difference = distance;
                        candidate = item;
                    }

                    attempts -= 1;
                }
                while (!(attempts == 0));
                if (value < difference)
                {
                    candidate = null;
                    done = true;
                }
            }

            if (candidate is not null)
            {
                candidate.SetOwner(owner);
                list.Delete(list.IndexOfItemA(candidate));
                value -= candidate.GetObtainValue();
                result += 1;
            }
        }

        list?.Dispose();
        return result;
    }

    // Army.PAS:4877
    public void Activate()
    {
        if (!_status.Contains(StatusElement.Activated))
        {
            _status += Set<StatusElement>.Of(StatusElement.Activated);
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                _items[i].Activate();
            }
        }
    }

    // Army.PAS:4887
    public void Deactivate()
    {
        if (_status.Contains(StatusElement.Activated))
        {
            _status -= Set<StatusElement>.Of(StatusElement.Activated);
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                _items[i].Deactivate();
            }
        }
    }

    public enum StatusElement : byte
    {
        Activated = 0
    }
}
