// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// system library, libraries
public partial class ItemLibraryManager : EObject
{
    // Army.PAS:4967
    public ItemLibraryManager(ECustomNode owner) : base(owner)
    {
        FSystemLibrary = new ItemLibrary(null);
        FLibraries = new ENode(null);
    }

    public ItemLibrary FSystemLibrary;
    public ENode FLibraries;

    // Army.PAS:4974
    public override void Dispose()
    {
        FLibraries?.Dispose();
        FSystemLibrary?.Dispose();
        base.Dispose();
    }

    // Army.PAS:4981
    public override void ReadWrite(EStorageStream Stream)
    {
        string Directory;
        base.ReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
        {
            Directory = ExtractFilePath(GameTables.AoWEngine.HSSFileName);
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                ReadUserLibraries();
                GameTables.AoWEngine.ReadFromFile(Directory + "ITEMS.PFS", FSystemLibrary, null);
            }
            else
            {
                GameTables.AoWEngine.WriteToFile(Directory + "ITEMS.PFS", FSystemLibrary, null);
                WriteUserLibraries();
            }
        }
        else
        {
            Stream.rwEObject(FSystemLibrary, 20);
        }
    }

    // Army.PAS:5002
    public ItemLibrary GetLibrary(int Index) => (ItemLibrary)FLibraries.FChildren.EObjects[Index];

    // Army.PAS:5007
    public int GetCount() => FLibraries.Count;

    // Army.PAS:5012
    public void ReadUserLibraries()
    {
        ItemLibrary ItemLibrary;
        FLibraries.Clear();
        FLibraries.AddChild(FSystemLibrary);
        // Both extensions list, and a list present in both forms loads its own document once
        // (own-formats.md 4.2).
        foreach (var Listed in FileBoundary.FindDocuments(GameTables.AoWEngine.StartupDirectory + "User\\", DocumentKind.ItemList))
        {
            ItemLibrary = new ItemLibrary(null);
            try
            {
                ItemLibrary.LoadFromFile(GameTables.AoWEngine.StartupDirectory + "User\\" + Listed);
                ItemLibrary.SetOwner(FLibraries);
            }
            catch
            {
                ItemLibrary?.Dispose();
            }
        }
    }

    // Army.PAS:5033
    public void WriteUserLibraries()
    {
        ItemLibrary ItemLibrary;
        for (int I = 1, last = FLibraries.Count - 1; I <= last; I++)
        {
            ItemLibrary = GetLibrary(I);
            var name = ItemLibrary.GetName();
            ItemLibrary.SaveToFile(GameTables.AoWEngine.StartupDirectory + "User\\" + name + FileBoundary.WriteExtension(DocumentKind.ItemList));
        }
    }

    // Army.PAS:5043
    public int IndexOfLibraryName(string Name)
    {
        int Result = FLibraries.Count;
        while (Result != 0)
        {
            Result -= 1;
            if (CompareText(Name, ((ItemLibrary)FLibraries.FChildren.List[Result]).FName) == 0)
            {
                return Result;
            }
        }

        return -1;
    }

    // Army.PAS:5054
    public int IndexOfLibrary(ItemLibrary ItemLibrary) => FLibraries.FChildren.IndexOf(ItemLibrary);

    // Army.PAS:5059
    public Item FindItem(string LibraryName, int LibraryID)
    {
        Item Result;
        int Index;
        Index = IndexOfLibraryName(LibraryName);
        if (Index != -1)
        {
            Result = ((ItemLibrary)FLibraries.FChildren.List[Index]).FindLibraryID(LibraryID);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:5068
    public ItemLibrary CreateLibrary(string Name)
    {
        ItemLibrary Result;
        string LibraryName;
        LibraryName = Name;
        // { Original retains the period when stripping the extension. }
        if (Pos(".", LibraryName) != 0)
        {
            LibraryName = RtlUnit.Copy(LibraryName, 1, Pos(".", LibraryName));
        }

        if (!IsEmpty(LibraryName) && IndexOfLibraryName(LibraryName) == -1)
        {
            Result = new ItemLibrary(FLibraries);
            Result.FName = LibraryName;
            // { Original creates in Release while reading and deleting from User. }
            Result.SaveToFile(GameTables.AoWEngine.StartupDirectory + "Release\\" + LibraryName + FileBoundary.WriteExtension(DocumentKind.ItemList));
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:5084
    public void DeleteLibrary(int Index)
    {
        if (Index < FLibraries.Count && Index > 0)
        {
            var name = GetLibrary(Index).GetName();
            // The list goes in both forms, so an original of the same name does not return.
            DeleteFile(GameTables.AoWEngine.StartupDirectory + "User\\" + name + DocumentKind.ItemList.OriginalExtension);
            DeleteFile(GameTables.AoWEngine.StartupDirectory + "User\\" + name + DocumentKind.ItemList.OwnExtension);
            GetLibrary(Index)?.Dispose();
        }
    }

    // intf_P2_04.inc:157

}
