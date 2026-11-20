#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Units;

// The original's TItemLibrary derives from TItemList. The port's library owns its items as its
// node children and reads them through Items.
public class ItemLibrary(CustomNode owner) : ObservableNode(owner), IChildList<Item>, IItemHolder
{
    /// <summary>The items of this library, in their stored order.</summary>
    public ItemCollection Items => new(this);

    Item IOwner<Item>.this[int index] => Items[index];

    // A library takes items only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Item;

    int IChildSink<Item>.AddChild(Item child) => AddChild(child);

    public event NotifyEvent<object> OnChanged;

    // Army.PAS:4596
    public virtual MapCoordinate GetLocation() => MapCoordinate.None;

    public string Name { get; set; } = "";

    private int _lastId;

    // Army.PAS:4933
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        _lastId = persister.Int32("LastId", value: _lastId);
    }

    // Army.PAS:4909
    public override int AddChild(EngineObject child)
    {
        // AddChild receives an engine object. The library holds only items. Another class throws,
        // and null fails at the LibraryId read.
        Item item = (Item)child;
        // { Original checks the world item ID, not the library ID. }
        if (item.LibraryId <= 0 || Items.FindItem(item.LibraryId) is not null)
        {
            item.LibraryId = CreateUniqueLibraryId();
        }

        int result = base.AddChild(child);
        if (result != -1)
        {
            _ = child.AddRef();
        }

        return result;
    }

    // Army.PAS:4516, the item list's release of each loaded child.
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                if (GetChild(i) is not null)
                {
                    _ = Items[i].Release();
                }
            }
        }
    }

    // Army.PAS:4546
    public override void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            if (GetChild(i) is not null)
            {
                EngineObject child = Items[i];
                if (child is not null)
                {
                    if (child.Owner != this)
                    {
                        _ = child.Release();
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
            _ = child.Release();
        }

        return result;
    }

    // Army.PAS:4564
    protected override void OnChange()
    {
        base.OnChange();
        OnChanged?.Invoke(this);
    }

    // Army.PAS:4940
    public virtual string GetName()
    {
        string result = Name;
        return IsEmpty(result) ? AbilityRules.TranslateResourceString(GetEngine(), s_systemItemLibraryText) : result;
    }

    // Army.PAS:4897
    public int CreateUniqueLibraryId()
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

            while (i != Items.Count && Items[i].LibraryId != _lastId)
            {
                i += 1;
            }
        }
        while (!(i == Items.Count));
        return _lastId;
    }

    // Army.PAS:4917
    public Item FindLibraryId(int libraryId)
    {
        int i = Items.Count;
        while (i != 0)
        {
            i -= 1;
            if (Items[i].LibraryId == libraryId)
            {
                return Items[i];
            }
        }

        return null;
    }

    // Army.PAS:4946
    public Item CreateItem()
    {
        Item item = new(null)
        {
            Holder = CreationHolder(),
            LibraryId = CreateUniqueLibraryId(),
            LibraryName = Name
        };
        item.SetOwner(this);
        _ = item.Release();
        return item;
    }

    // Army.PAS:4957
    public void LoadFromFile(string fileName) => GetEngine().ReadFromFile(fileName, this, null, GameProfile.ItemList);

    // Army.PAS:4962
    public void SaveToFile(string fileName) => GetEngine().WriteToFile(fileName, this, null, GameProfile.ItemList);

    private static readonly string s_systemItemLibraryText = "System";
}
