// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// items, change events, update lock, item change events, last ID, status
public partial class ItemControl : EngineObject
{
    // Army.PAS:4639
    public ItemControl(ECustomNode owner) : base(owner)
    {
        _items = new List<Item>();
        _onChangedStore = new();
        _onItemChangedStore = new();
    }

    private List<Item> _items;

    private EventStore<ItemControl> _onChangedStore;

    public event NotifyEvent<ItemControl> FOnChanged
    {
        add => _onChangedStore.Add(value);
        remove => _onChangedStore.Remove(value);
    }

    private int _updateLock;

    private EventStore<Item> _onItemChangedStore;

    public event NotifyEvent<Item> FOnItemChanged
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
        for (int I = _items.Count - 1; I >= 0; I--)
        {
            UnregisterItem(_items[I]);
        }

        _items.Clear();
        Changed();
        _items?.Clear();
        _onItemChangedStore.Clear();
        _onChangedStore.Clear();
        base.Dispose();
    }

    // Army.PAS:4660
    public override void ReadWrite(EStorageStream stream)
    {
        int id;
        Item Item = default;
        base.ReadWrite(stream);
        stream.rwInteger(ref _lastId, 9);
        if (stream.StorageMode().Contains(StorageModeElement.Compare))
        {
            id = 10;
            for (int I = 0, last = _items.Count - 1; I <= last; I++)
            {
                Item = _items[I];
                if (Item.Owner == null)
                {
                    stream.rwCreateEObject(ref Item, null, id);
                    id += 1;
                }
            }
        }
        else
        {
            if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly, StorageModeElement.Preview, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
            {
                if (stream.StorageMode().Contains(StorageModeElement.Read))
                {
                    id = 10;
                    do
                    {
                        stream.rwCreateEObject(ref Item, null, id);
                        if (Item != null)
                        {
                            ValidateItem(Item);
                            _items.Add(Item);
                            Item.Status = Item.Status + Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
                        }

                        id += 1;
                    }
                    while (!(Item == null));
                }

                if (stream.StorageMode().Contains(StorageModeElement.TextOnly) || stream.StorageMode().Contains(StorageModeElement.Write))
                {
                    id = 10;
                    for (int I = 0, last = _items.Count - 1; I <= last; I++)
                    {
                        if (_items[I].Owner == null)
                        {
                            Item item = _items[I];
                            stream.rwCreateEObject(ref item, null, id);
                            _items[I] = item;
                            id += 1;
                        }
                    }
                }
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
        int I;
        I = _items.Count;
        while (I != 0)
        {
            I -= 1;
            if ((_items[I]).ItemId == itemId)
            {
                return _items[I];
            }
        }

        return null;
    }

    // Army.PAS:4733
    public void ItemChanged(Item item)
    {
        _onItemChangedStore.Raise(item);
    }

    // Army.PAS:4738
    public void Changed()
    {
        if (_updateLock == 0)
        {
            _onChangedStore.Raise(this);
        }
    }

    // Army.PAS:4743
    public void BeginUpdate()
    {
        _updateLock += 1;
    }

    // Army.PAS:4748
    public void EndUpdate()
    {
        _updateLock -= 1;
        if (_updateLock == 0)
        {
            Changed();
        }
    }

    // Army.PAS:4754
    public int CreateUniqueId()
    {
        int I;
        do
        {
            I = 0;
            _lastId += 1;
            if (_lastId > 268435455)
            {
                _lastId = 0;
            }

            while (I != _items.Count && (_items[I]).ItemId != _lastId)
            {
                I += 1;
            }
        }
        while (!(I == _items.Count));
        return _lastId;
    }

    // Army.PAS:4766
    public void ValidateItem(Item item)
    {
        if (FindItem(item.ItemId) != null)
        {
            item.ItemId = CreateUniqueId();
        }
    }

    // Army.PAS:4771
    public void UpdateLibraryItems()
    {
        BeginUpdate();
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            (_items[I]).UpdateFromLibrary();
        }

        EndUpdate();
    }

    // Army.PAS:4779
    public void RegisterItem(Item item)
    {
        Item existing;
        if (!item.Status.Contains(ItemStatusElement.Registered))
        {
            existing = FindItem(item.ItemId);
            if (existing != null && existing.Compare(item, GameTables.GameEngine) && existing.RefCount == 1)
            {
                UnregisterItem(existing);
            }

            ValidateItem(item);
            item.Status = item.Status + Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
            _items.Add(item);
            item.AddRef();
            if (_status.Contains(StatusElement.Activated) && !item.Status.Contains(ItemStatusElement.Activated))
            {
                item.Activate();
            }

            Changed();
        }
    }

    // Army.PAS:4796
    public void UnregisterItem(Item item)
    {
        if (item.Status.Contains(ItemStatusElement.Registered))
        {
            _items.Remove(item);
            item.Status = item.Status - Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
            item.Release();
            Changed();
        }
    }

    // Army.PAS:4807
    public Item GenerateItem(Set<ItemType> types, int minValue, int maxValue)
    {
        Item result;
        DoubleItemList<Item> list;
        int value;
        Item item;
        list = new DoubleItemList<Item>();
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            item = _items[I];
            if (item.RefCount == 1 && types.Contains(item.ItemType))
            {
                value = item.GetObtainValue();
                if (value >= minValue && value <= maxValue)
                {
                    list.AddAbility(item, (int)item.Availability);
                }
            }
        }

        if (list.Count != 0)
        {
            result = list[list.RandomChanceItemB(1)].First;
        }
        else
        {
            result = null;
        }

        list?.Dispose();
        return result;
    }

    // Army.PAS:4826
    public int GenerateItems(Set<ItemType> types, ECustomNode owner, int value)
    {
        DoubleItemList<Item> list;
        int difference;
        int attempts;
        int itemValue;
        int distance;
        bool done;
        Item item;
        Item candidate;
        int result = 0;
        list = new DoubleItemList<Item>();
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            item = _items[I];
            if (types.Contains(item.ItemType) && item.RefCount == 1)
            {
                list.AddAbility(item, (int)item.Availability);
            }
        }

        done = false;
        while (!done && list.Count != 0)
        {
            candidate = list[list.RandomChanceItemB(1)].First;
            itemValue = candidate.GetObtainValue();
            if (itemValue > value)
            {
                attempts = 10;
                difference = Math.Abs(itemValue - value);
                do
                {
                    item = list[list.RandomChanceItemB(1)].First;
                    distance = Math.Abs(item.GetObtainValue() - value);
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

            if (candidate != null)
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
            _status = _status + Set<StatusElement>.Of(StatusElement.Activated);
            for (int I = _items.Count - 1; I >= 0; I--)
            {
                (_items[I]).Activate();
            }
        }
    }

    // Army.PAS:4887
    public void Deactivate()
    {
        if (_status.Contains(StatusElement.Activated))
        {
            _status = _status - Set<StatusElement>.Of(StatusElement.Activated);
            for (int I = _items.Count - 1; I >= 0; I--)
            {
                (_items[I]).Deactivate();
            }
        }
    }

    public enum StatusElement : byte
    {
        Activated = 0
    }

    // intf_P2_04.inc:182

}
