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
public partial class ItemLibraryManager : EngineObject
{
    // Army.PAS:4967
    public ItemLibraryManager(ECustomNode owner) : base(owner)
    {
        SystemLibrary = new ItemLibrary(null);
        Libraries = new ENode(null);
    }

    public ItemLibrary SystemLibrary { get; }
    public ENode Libraries { get; }

    // Army.PAS:4974
    public override void Dispose()
    {
        Libraries?.Dispose();
        SystemLibrary?.Dispose();
        base.Dispose();
    }

    // Army.PAS:4981
    public override void ReadWrite(StorageStream stream)
    {
        string directory;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
        {
            directory = ExtractFilePath(GameTables.GameEngine.HssFileName);
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                ReadUserLibraries();
                GameTables.GameEngine.ReadFromFile(directory + "ITEMS.PFS", SystemLibrary, null);
            }
            else
            {
                GameTables.GameEngine.WriteToFile(directory + "ITEMS.PFS", SystemLibrary, null);
                WriteUserLibraries();
            }
        }
        else
        {
            stream.Object(SystemLibrary, 20);
        }
    }

    // Army.PAS:5002
    public ItemLibrary GetLibrary(int index) => (ItemLibrary)Libraries.GetChildInRange(index);

    // Army.PAS:5007
    public int GetCount() => Libraries.Count;

    // Army.PAS:5012
    public void ReadUserLibraries()
    {
        ItemLibrary itemLibrary;
        Libraries.Clear();
        Libraries.AddChild(SystemLibrary);
        // Both extensions list, and a list present in both forms loads its own document once
        // (own-formats.md 4.2).
        foreach (var listed in FileBoundary.FindDocuments(GameTables.GameEngine.StartupDirectory + "User\\", DocumentKind.ItemList))
        {
            itemLibrary = new ItemLibrary(null);
            try
            {
                itemLibrary.LoadFromFile(GameTables.GameEngine.StartupDirectory + "User\\" + listed);
                itemLibrary.SetOwner(Libraries);
            }
            catch
            {
                itemLibrary?.Dispose();
            }
        }
    }

    // Army.PAS:5033
    public void WriteUserLibraries()
    {
        ItemLibrary itemLibrary;
        for (int I = 1, last = Libraries.Count - 1; I <= last; I++)
        {
            itemLibrary = GetLibrary(I);
            var name = itemLibrary.GetName();
            itemLibrary.SaveToFile(GameTables.GameEngine.StartupDirectory + "User\\" + name + FileBoundary.WriteExtension(DocumentKind.ItemList));
        }
    }

    // Army.PAS:5043
    public int IndexOfLibraryName(string name)
    {
        int result = Libraries.Count;
        while (result != 0)
        {
            result -= 1;
            if (CompareText(name, ((ItemLibrary)Libraries.GetChild(result)).Name) == 0)
            {
                return result;
            }
        }

        return -1;
    }

    // Army.PAS:5054
    public int IndexOfLibrary(ItemLibrary itemLibrary) => Libraries.IndexOfChild(itemLibrary);

    // Army.PAS:5059
    public Item FindItem(string libraryName, int libraryId)
    {
        Item result;
        int index;
        index = IndexOfLibraryName(libraryName);
        if (index != -1)
        {
            result = ((ItemLibrary)Libraries.GetChild(index)).FindLibraryId(libraryId);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:5068
    public ItemLibrary CreateLibrary(string name)
    {
        ItemLibrary result;
        string libraryName;
        libraryName = name;
        // { Original retains the period when stripping the extension. }
        if (Pos(".", libraryName) != 0)
        {
            libraryName = RtlUnit.Copy(libraryName, 1, Pos(".", libraryName));
        }

        if (!IsEmpty(libraryName) && IndexOfLibraryName(libraryName) == -1)
        {
            result = new ItemLibrary(Libraries);
            result.Name = libraryName;
            // { Original creates in Release while reading and deleting from User. }
            result.SaveToFile(GameTables.GameEngine.StartupDirectory + "Release\\" + libraryName + FileBoundary.WriteExtension(DocumentKind.ItemList));
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:5084
    public void DeleteLibrary(int index)
    {
        if (index < Libraries.Count && index > 0)
        {
            var name = GetLibrary(index).GetName();
            // The list goes in both forms, so an original of the same name does not return.
            DeleteFile(GameTables.GameEngine.StartupDirectory + "User\\" + name + DocumentKind.ItemList.OriginalExtension);
            DeleteFile(GameTables.GameEngine.StartupDirectory + "User\\" + name + DocumentKind.ItemList.OwnExtension);
            GetLibrary(index)?.Dispose();
        }
    }

    // intf_P2_04.inc:157

}
