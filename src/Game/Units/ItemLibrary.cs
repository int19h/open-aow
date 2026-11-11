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

    public string FName = "";
    public int FLastID;

    // Army.PAS:4933
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 20);
        Stream.rwInteger(ref FLastID, 21);
    }

    // Army.PAS:4909
    public override int AddChild(EngineObject Child)
    {
        var item = (Item)Child;
        // { Original checks the world item ID, not the library ID. }
        if (item.FLibraryID <= 0 || FindItem(item.FLibraryID) != null)
        {
            item.FLibraryID = CreateUniqueLibraryID();
        }

        return base.AddChild(Child);
    }

    // Army.PAS:4940
    public virtual string GetName()
    {
        string Result = FName;
        if (IsEmpty(Result))
        {
            Result = AbilityRules.TranslateRStr(SystemItemLibraryRStr);
        }

        return Result;
    }

    // Army.PAS:4897
    public int CreateUniqueLibraryID()
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

            while (I != FChildren.Count && ((Item)FChildren.List[I]).FLibraryID != FLastID)
            {
                I += 1;
            }
        }
        while (!(I == FChildren.Count));
        return FLastID;
    }

    // Army.PAS:4917
    public Item FindLibraryID(int LibraryID)
    {
        int I;
        I = FChildren.Count;
        while (I != 0)
        {
            I -= 1;
            if (((Item)FChildren.List[I]).FLibraryID == LibraryID)
            {
                return (Item)FChildren.List[I];
            }
        }

        return null;
    }

    // Army.PAS:4946
    public Item CreateItem()
    {
        Item Item;
        Item = new Item(null);
        Item.FLibraryID = CreateUniqueLibraryID();
        Item.FLibraryName = FName;
        Item.SetOwner(this);
        Item.Release();
        return Item;
    }

    // Army.PAS:4957
    public void LoadFromFile(string FileName)
    {
        GameTables.AoWEngine.ReadFromFile(FileName, this, null, GameProfile.ItemList);
    }

    // Army.PAS:4962
    public void SaveToFile(string FileName)
    {
        GameTables.AoWEngine.WriteToFile(FileName, this, null, GameProfile.ItemList);
    }

    private static readonly string SystemItemLibraryRStr = "System";

    // intf_P2_04.inc:118

    // intf_P2_04.inc:118

}
