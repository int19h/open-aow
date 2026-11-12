// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// SystemLibrary. Libraries
public partial class HeroLibraryManager : EngineObject
{
    // Hero.PAS:2646
    public HeroLibraryManager(ECustomNode owner) : base(owner)
    {
        SystemLibrary = new HeroLibrary(null);
        _libraries = new ENode(null);
    }

    public HeroLibrary SystemLibrary { get; }

    private readonly ENode _libraries;

    // Hero.PAS:2653
    public override void Dispose()
    {
        _libraries?.Dispose();
        SystemLibrary?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:2656
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
                GameTables.GameEngine.ReadFromFile(directory + "Heroes.PFS", SystemLibrary, null);
            }
            else
            {
                GameTables.GameEngine.WriteToFile(directory + "Heroes.PFS", SystemLibrary, null);
                WriteUserLibraries();
            }
        }
        else
        {
            stream.Object(SystemLibrary, 20);
        }
    }

    // Hero.PAS:2677
    public HeroLibrary GetLibrary(int index) => (HeroLibrary)_libraries.GetChildInRange(index);

    // Hero.PAS:2680
    public int GetCount() => _libraries.GetCount();

    // Hero.PAS:2683
    public void ReadUserLibraries()
    {
        HeroLibrary library;
        _libraries.Clear();
        _libraries.AddChild(SystemLibrary);
        // Both extensions list, and a list present in both forms loads its own document once
        // (own-formats.md 4.2).
        foreach (var listed in FileBoundary.FindDocuments(GameTables.GameEngine.StartupDirectory + "User\\", DocumentKind.HeroList))
        {
            library = new HeroLibrary(null);
            try
            {
                library.LoadFromFile(GameTables.GameEngine.StartupDirectory + "User\\" + listed);
                library.SetOwner(_libraries);
            }
            catch
            {
                library?.Dispose();
            }
        }
    }

    // Hero.PAS:2703
    public void WriteUserLibraries()
    {
        HeroLibrary library;
        for (int I = 1, last = _libraries.GetCount() - 1; I <= last; I++)
        {
            library = GetLibrary(I);
            var name = library.GetName();
            library.SaveToFile(GameTables.GameEngine.StartupDirectory + "User\\" + name + FileBoundary.WriteExtension(DocumentKind.HeroList));
        }
    }

    // Hero.PAS:2713
    public int IndexOfLibraryName(string name)
    {
        int result = _libraries.GetCount();
        while (result != 0)
        {
            result -= 1;
            if (CompareText(name, ((HeroLibrary)_libraries.GetChild(result)).LibraryName) == 0)
            {
                return result;
            }
        }

        return -1;
    }

    // Hero.PAS:2724
    public int IndexOfLibrary(HeroLibrary library) => _libraries.IndexOfChild(library);

    // Hero.PAS:2727
    public Hero FindHero(string libraryName, int libraryId)
    {
        Hero result;
        int index;
        index = IndexOfLibraryName(libraryName);
        if (index != -1)
        {
            result = ((HeroLibrary)_libraries.GetChild(index)).FindLibraryId(libraryId);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:2735
    public HeroLibrary CreateLibrary(string name)
    {
        HeroLibrary result;
        string libraryName;
        libraryName = name;
        // { Original retains the period when stripping the extension. }
        if (Pos(".", libraryName) != 0)
        {
            libraryName = RtlUnit.Copy(libraryName, 1, Pos(".", libraryName));
        }

        if (!IsEmpty(libraryName) && IndexOfLibraryName(libraryName) == -1)
        {
            result = new HeroLibrary(_libraries);
            result.LibraryName = libraryName;
            result.SaveToFile(GameTables.GameEngine.StartupDirectory + "Release\\" + libraryName + FileBoundary.WriteExtension(DocumentKind.HeroList));
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:2750
    public void DeleteLibrary(int index)
    {
        if (index < _libraries.GetCount() && index > 0)
        {
            var name = GetLibrary(index).GetName();
            // The list goes in both forms, so an original of the same name does not return.
            DeleteFile(GameTables.GameEngine.StartupDirectory + "User\\" + name + DocumentKind.HeroList.OriginalExtension);
            DeleteFile(GameTables.GameEngine.StartupDirectory + "User\\" + name + DocumentKind.HeroList.OwnExtension);
            GetLibrary(index)?.Dispose();
        }
    }

    // intf_P2_07.inc:311

}
