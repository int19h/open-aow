// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// name, last library ID
public partial class ItemLibrary : ItemList
{
    public ItemLibrary(ECustomNode owner) : base(owner)
    { }

    public string Name = "";
    public int LastId;

    // Army.PAS:4933
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Name, 20);
        stream.rwInteger(ref LastId, 21);
    }

    // Army.PAS:4909
    public override int AddChild(EngineObject child)
    {
        var item = (Item)child;
        // { Original checks the world item ID, not the library ID. }
        if (item.LibraryId <= 0 || FindItem(item.LibraryId) != null)
        {
            item.LibraryId = CreateUniqueLibraryId();
        }

        return base.AddChild(child);
    }

    // Army.PAS:4940
    public virtual string GetName()
    {
        string result = Name;
        if (IsEmpty(result))
        {
            result = AbilityRules.TranslateResourceString(s_systemItemLibraryText);
        }

        return result;
    }

    // Army.PAS:4897
    public int CreateUniqueLibraryId()
    {
        int I;
        do
        {
            I = 0;
            LastId += 1;
            if (LastId > 268435455)
            {
                LastId = 0;
            }

            while (I != _children.Count && ((Item)_children.List[I]).LibraryId != LastId)
            {
                I += 1;
            }
        }
        while (!(I == _children.Count));
        return LastId;
    }

    // Army.PAS:4917
    public Item FindLibraryId(int libraryId)
    {
        int I;
        I = _children.Count;
        while (I != 0)
        {
            I -= 1;
            if (((Item)_children.List[I]).LibraryId == libraryId)
            {
                return (Item)_children.List[I];
            }
        }

        return null;
    }

    // Army.PAS:4946
    public Item CreateItem()
    {
        Item item;
        item = new Item(null);
        item.LibraryId = CreateUniqueLibraryId();
        item.LibraryName = Name;
        item.SetOwner(this);
        item.Release();
        return item;
    }

    // Army.PAS:4957
    public void LoadFromFile(string fileName)
    {
        GameTables.GameEngine.ReadFromFile(fileName, this, null, GameProfile.ItemList);
    }

    // Army.PAS:4962
    public void SaveToFile(string fileName)
    {
        GameTables.GameEngine.WriteToFile(fileName, this, null, GameProfile.ItemList);
    }

    private static readonly string s_systemItemLibraryText = "System";

    // intf_P2_04.inc:118

    // intf_P2_04.inc:118

}
