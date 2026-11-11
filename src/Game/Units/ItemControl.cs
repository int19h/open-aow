// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// items, change events, update lock, item change events, last ID, status
public partial class ItemControl : EObject
{
    // Army.PAS:4639
    public ItemControl(ECustomNode owner) : base(owner)
    {
        FItems = new DelphiList();
        FOnChangedStore = new();
        FOnItemChangedStore = new();
    }

    public DelphiList FItems;

    internal EventStore<ItemControl> FOnChangedStore;

    public event NotifyEvent<ItemControl> FOnChanged
    {
        add => FOnChangedStore.Add(value);
        remove => FOnChangedStore.Remove(value);
    }

    public int FUpdateLock;

    internal EventStore<Item> FOnItemChangedStore;

    public event NotifyEvent<Item> FOnItemChanged
    {
        add => FOnItemChangedStore.Add(value);
        remove => FOnItemChangedStore.Remove(value);
    }

    public int FLastID;
    public Set<StatusElement> FStatus;

    // Army.PAS:4647
    public override void Dispose()
    {
        BeginUpdate();
        for (int I = FItems.Count - 1; I >= 0; I--)
        {
            UnRegisterItem((Item)FItems.List[I]);
        }

        FItems.Clear();
        Changed();
        FItems?.Dispose();
        FOnItemChangedStore.Clear();
        FOnChangedStore.Clear();
        base.Dispose();
    }

    // Army.PAS:4660
    public override void ReadWrite(EStorageStream Stream)
    {
        int ID;
        Item Item = default;
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FLastID, 9);
        if (Stream.StorageMode().Contains(StorageModeElement.Compare))
        {
            ID = 10;
            for (int I = 0, last = FItems.Count - 1; I <= last; I++)
            {
                Item = (Item)FItems[I];
                if (Item.Owner == null)
                {
                    Stream.rwCreateEObject(ref Item, null, ID);
                    ID += 1;
                }
            }
        }
        else
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly, StorageModeElement.Preview, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
            {
                if (Stream.StorageMode().Contains(StorageModeElement.Read))
                {
                    ID = 10;
                    do
                    {
                        Stream.rwCreateEObject(ref Item, null, ID);
                        if (Item != null)
                        {
                            ValidateItem(Item);
                            FItems.Add(Item);
                            Item.FStatus = Item.FStatus + Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
                        }

                        ID += 1;
                    }
                    while (!(Item == null));
                }

                if (Stream.StorageMode().Contains(StorageModeElement.TextOnly) || Stream.StorageMode().Contains(StorageModeElement.Write))
                {
                    ID = 10;
                    for (int I = 0, last = FItems.Count - 1; I <= last; I++)
                    {
                        EObject stored = (EObject)FItems.List[I];
                        if (stored.Owner == null)
                        {
                            Stream.rwCreateEObject(ref stored, null, ID);
                            FItems.List[I] = stored;
                            ID += 1;
                        }
                    }
                }
            }
        }
    }

    // Army.PAS:4707
    public Item GetItem(int Index) => (Item)FItems.List[Index];

    // Army.PAS:4712
    public int GetCount() => FItems.Count;

    // Army.PAS:4717
    public Item FindItem(int ItemID)
    {
        int I;
        I = FItems.Count;
        while (I != 0)
        {
            I -= 1;
            if (((Item)FItems.List[I]).FItemID == ItemID)
            {
                return (Item)FItems.List[I];
            }
        }

        return null;
    }

    // Army.PAS:4733
    public void ItemChanged(Item Item)
    {
        FOnItemChangedStore.Raise(Item);
    }

    // Army.PAS:4738
    public void Changed()
    {
        if (FUpdateLock == 0)
        {
            FOnChangedStore.Raise(this);
        }
    }

    // Army.PAS:4743
    public void BeginUpdate()
    {
        FUpdateLock += 1;
    }

    // Army.PAS:4748
    public void EndUpdate()
    {
        FUpdateLock -= 1;
        if (FUpdateLock == 0)
        {
            Changed();
        }
    }

    // Army.PAS:4754
    public int CreateUniqueID()
    {
        int I;
        do
        {
            I = 0;
            FLastID += 1;
            if (FLastID > 268435455)
            {
                FLastID = 0;
            }

            while (I != FItems.Count && ((Item)FItems.List[I]).FItemID != FLastID)
            {
                I += 1;
            }
        }
        while (!(I == FItems.Count));
        return FLastID;
    }

    // Army.PAS:4766
    public void ValidateItem(Item Item)
    {
        if (FindItem(Item.FItemID) != null)
        {
            Item.FItemID = CreateUniqueID();
        }
    }

    // Army.PAS:4771
    public void UpdateLibraryItems()
    {
        BeginUpdate();
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            ((Item)FItems.List[I]).UpdateFromLibrary();
        }

        EndUpdate();
    }

    // Army.PAS:4779
    public void RegisterItem(Item Item)
    {
        Item Existing;
        if (!Item.FStatus.Contains(ItemStatusElement.Registered))
        {
            Existing = FindItem(Item.FItemID);
            if (Existing != null && Existing.Compare(Item, GameTables.AoWEngine) && Existing.FRefCount == 1)
            {
                UnRegisterItem(Existing);
            }

            ValidateItem(Item);
            Item.FStatus = Item.FStatus + Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
            FItems.Add(Item);
            Item.AddRef();
            if (FStatus.Contains(StatusElement.Activated) && !Item.FStatus.Contains(ItemStatusElement.Activated))
            {
                Item.Activate();
            }

            Changed();
        }
    }

    // Army.PAS:4796
    public void UnRegisterItem(Item Item)
    {
        if (Item.FStatus.Contains(ItemStatusElement.Registered))
        {
            FItems.Remove(Item);
            Item.FStatus = Item.FStatus - Set<ItemStatusElement>.Of(ItemStatusElement.Registered);
            Item.Release();
            Changed();
        }
    }

    // Army.PAS:4807
    public Item GenerateItem(Set<ItemType> Types, int MinValue, int MaxValue)
    {
        Item Result;
        DoubleItemList<Item> List;
        int Value;
        Item Item;
        List = new DoubleItemList<Item>();
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            Item = (Item)FItems.List[I];
            if (Item.FRefCount == 1 && Types.Contains(Item.FItemType))
            {
                Value = Item.GetObtainValue();
                if (Value >= MinValue && Value <= MaxValue)
                {
                    List.AddAB(Item, (int)Item.FAvailability);
                }
            }
        }

        if (List.Count != 0)
        {
            Result = List[List.RandomChanceItemB(1)].A;
        }
        else
        {
            Result = null;
        }

        List?.Dispose();
        return Result;
    }

    // Army.PAS:4826
    public int GenerateItems(Set<ItemType> Types, ECustomNode Owner, int Value)
    {
        DoubleItemList<Item> List;
        int Difference;
        int Attempts;
        int ItemValue;
        int Distance;
        bool Done;
        Item Item;
        Item Candidate;
        int Result = 0;
        List = new DoubleItemList<Item>();
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            Item = (Item)FItems.List[I];
            if (Types.Contains(Item.FItemType) && Item.FRefCount == 1)
            {
                List.AddAB(Item, (int)Item.FAvailability);
            }
        }

        Done = false;
        while (!Done && List.Count != 0)
        {
            Candidate = List[List.RandomChanceItemB(1)].A;
            ItemValue = Candidate.GetObtainValue();
            if (ItemValue > Value)
            {
                Attempts = 10;
                Difference = Math.Abs(ItemValue - Value);
                do
                {
                    Item = List[List.RandomChanceItemB(1)].A;
                    Distance = Math.Abs(Item.GetObtainValue() - Value);
                    if (Distance < Difference)
                    {
                        Difference = Distance;
                        Candidate = Item;
                    }

                    Attempts -= 1;
                }
                while (!(Attempts == 0));
                if (Value < Difference)
                {
                    Candidate = null;
                    Done = true;
                }
            }

            if (Candidate != null)
            {
                Candidate.SetOwner(Owner);
                List.Delete(List.IndexOfItemA(Candidate));
                Value -= Candidate.GetObtainValue();
                Result += 1;
            }
        }

        List?.Dispose();
        return Result;
    }

    // Army.PAS:4877
    public void Activate()
    {
        if (!FStatus.Contains(StatusElement.Activated))
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Activated);
            for (int I = FItems.Count - 1; I >= 0; I--)
            {
                ((Item)FItems.List[I]).Activate();
            }
        }
    }

    // Army.PAS:4887
    public void Deactivate()
    {
        if (FStatus.Contains(StatusElement.Activated))
        {
            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Activated);
            for (int I = FItems.Count - 1; I >= 0; I--)
            {
                ((Item)FItems.List[I]).Deactivate();
            }
        }
    }

    public enum StatusElement : byte
    {
        Activated = 0
    }

    // intf_P2_04.inc:182

}
