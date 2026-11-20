#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Rtl;

using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Units;

// system library, libraries
public class ItemLibraryManager : EngineObject
{
    // Army.PAS:4967
    public ItemLibraryManager(CustomNode owner) : base(owner)
    {
        SystemLibrary = new ItemLibrary(null) { Holder = CreationHolder() };
        Libraries = new OwningNode(null) { Holder = CreationHolder() };
    }

    public ItemLibrary SystemLibrary { get; }
    public OwningNode Libraries { get; }

    // Army.PAS:4974
    public override void Dispose()
    {
        Libraries?.Dispose();
        SystemLibrary?.Dispose();
        base.Dispose();
    }

    // Army.PAS:4981
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // Under the text-only mode of the translate stream the original visits the system library
        // in place and touches no file.
        if (PersistSupport.Translating(persister))
        {
            persister.Object("SystemLibrary", objectValue: SystemLibrary);
            return;
        }

        string directory = ExtractFilePath(GetEngine().HssFileName);
        if (persister.Loading)
        {
            ReadUserLibraries();
            GetEngine().ReadFromFile($"{directory}ITEMS.PFS", SystemLibrary, null);
        }
        else
        {
            GetEngine().WriteToFile($"{directory}ITEMS.PFS", SystemLibrary, null);
            WriteUserLibraries();
        }
    }

    // Army.PAS:5002
    public ItemLibrary GetLibrary(int index) => Libraries.GetChildInRange<ItemLibrary>(index);

    // Army.PAS:5007
    public int GetCount() => Libraries.Count;

    // Army.PAS:5012
    public void ReadUserLibraries()
    {
        Libraries.Clear();
        _ = Libraries.AddChild(SystemLibrary);
        // Both extensions list, and a list present in both forms loads its own document once
        // (own-formats.md 4.2).
        foreach (string listed in FileBoundary.FindDocuments($"{GetEngine().StartupDirectory}User\\", DocumentKind.ItemList))
        {
            ItemLibrary itemLibrary = new(null) { Holder = CreationHolder() };
            try
            {
                itemLibrary.LoadFromFile($"{GetEngine().StartupDirectory}User\\{listed}");
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
        for (int i = 1, last = Libraries.Count - 1; i <= last; i++)
        {
            ItemLibrary itemLibrary = GetLibrary(i);
            string name = itemLibrary.GetName();
            itemLibrary.SaveToFile($"{GetEngine().StartupDirectory}User\\{name}{FileBoundary.WriteExtension(DocumentKind.ItemList)}");
        }
    }

    // Army.PAS:5043
    public int IndexOfLibraryName(string name)
    {
        int result = Libraries.Count;
        while (result != 0)
        {
            result -= 1;
            if (CompareText(name, Libraries.GetChild<ItemLibrary>(result).Name) == 0)
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
        int index = IndexOfLibraryName(libraryName);
        return index != -1 ? Libraries.GetChild<ItemLibrary>(index).FindLibraryId(libraryId) : null;
    }

    // Army.PAS:5068
    public ItemLibrary CreateLibrary(string name)
    {
        string libraryName = name;
        // { Original retains the period when stripping the extension. }
        if (Pos(".", libraryName) != 0)
        {
            libraryName = DelphiStrings.Copy(libraryName, 1, Pos(".", libraryName));
        }

        ItemLibrary result;
        if (!IsEmpty(libraryName) && IndexOfLibraryName(libraryName) == -1)
        {
            result = new ItemLibrary(Libraries)
            {
                Name = libraryName
            };
            // { Original creates in Release while reading and deleting from User. }
            result.SaveToFile($"{GetEngine().StartupDirectory}Release\\{libraryName}{FileBoundary.WriteExtension(DocumentKind.ItemList)}");
        }
        else
        {
            return null;
        }

        return result;
    }

    // Army.PAS:5084
    public void DeleteLibrary(int index)
    {
        if (index < Libraries.Count && index > 0)
        {
            string name = GetLibrary(index).GetName();
            // The list goes in both forms, so an original of the same name does not return.
            _ = DeleteFile($"{GetEngine().StartupDirectory}User\\{name}{DocumentKind.ItemList.OriginalExtension}");
            _ = DeleteFile($"{GetEngine().StartupDirectory}User\\{name}{DocumentKind.ItemList.OwnExtension}");
            GetLibrary(index)?.Dispose();
        }
    }
}
