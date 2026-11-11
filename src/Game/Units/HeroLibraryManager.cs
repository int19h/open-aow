// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// FSystemLibrary. FLibraries
public partial class HeroLibraryManager : EngineObject
{
    // Hero.PAS:2646
    public HeroLibraryManager(ECustomNode owner) : base(owner)
    {
        FSystemLibrary = new HeroLibrary(null);
        FLibraries = new ENode(null);
    }

    public HeroLibrary FSystemLibrary;
    public ENode FLibraries;

    // Hero.PAS:2653
    public override void Dispose()
    {
        FLibraries?.Dispose();
        FSystemLibrary?.Dispose();
        base.Dispose();
    }

    // Hero.PAS:2656
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
                GameTables.AoWEngine.ReadFromFile(Directory + "Heroes.PFS", FSystemLibrary, null);
            }
            else
            {
                GameTables.AoWEngine.WriteToFile(Directory + "Heroes.PFS", FSystemLibrary, null);
                WriteUserLibraries();
            }
        }
        else
        {
            Stream.rwEObject(FSystemLibrary, 20);
        }
    }

    // Hero.PAS:2677
    public HeroLibrary GetLibrary(int Index) => (HeroLibrary)FLibraries.FChildren.EObjects[Index];

    // Hero.PAS:2680
    public int GetCount() => FLibraries.GetCount();

    // Hero.PAS:2683
    public void ReadUserLibraries()
    {
        HeroLibrary Lib;
        FLibraries.Clear();
        FLibraries.AddChild(FSystemLibrary);
        // Both extensions list, and a list present in both forms loads its own document once
        // (own-formats.md 4.2).
        foreach (var Listed in FileBoundary.FindDocuments(GameTables.AoWEngine.StartupDirectory + "User\\", DocumentKind.HeroList))
        {
            Lib = new HeroLibrary(null);
            try
            {
                Lib.LoadFromFile(GameTables.AoWEngine.StartupDirectory + "User\\" + Listed);
                Lib.SetOwner(FLibraries);
            }
            catch
            {
                Lib?.Dispose();
            }
        }
    }

    // Hero.PAS:2703
    public void WriteUserLibraries()
    {
        HeroLibrary Lib;
        for (int I = 1, last = FLibraries.GetCount() - 1; I <= last; I++)
        {
            Lib = GetLibrary(I);
            var name = Lib.GetName();
            Lib.SaveToFile(GameTables.AoWEngine.StartupDirectory + "User\\" + name + FileBoundary.WriteExtension(DocumentKind.HeroList));
        }
    }

    // Hero.PAS:2713
    public int IndexOfLibraryName(string Name)
    {
        int Result = FLibraries.GetCount();
        while (Result != 0)
        {
            Result -= 1;
            if (CompareText(Name, ((HeroLibrary)FLibraries.FChildren.List[Result]).FLibraryName) == 0)
            {
                return Result;
            }
        }

        return -1;
    }

    // Hero.PAS:2724
    public int IndexOfLibrary(HeroLibrary Lib) => FLibraries.FChildren.IndexOf(Lib);

    // Hero.PAS:2727
    public Hero FindHero(string LibraryName, int LibraryID)
    {
        Hero Result;
        int Index;
        Index = IndexOfLibraryName(LibraryName);
        if (Index != -1)
        {
            Result = ((HeroLibrary)FLibraries.FChildren.List[Index]).FindLibraryID(LibraryID);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Hero.PAS:2735
    public HeroLibrary CreateLibrary(string Name)
    {
        HeroLibrary Result;
        string LibraryName;
        LibraryName = Name;
        // { Original retains the period when stripping the extension. }
        if (Pos(".", LibraryName) != 0)
        {
            LibraryName = RtlUnit.Copy(LibraryName, 1, Pos(".", LibraryName));
        }

        if (!IsEmpty(LibraryName) && IndexOfLibraryName(LibraryName) == -1)
        {
            Result = new HeroLibrary(FLibraries);
            Result.FLibraryName = LibraryName;
            Result.SaveToFile(GameTables.AoWEngine.StartupDirectory + "Release\\" + LibraryName + FileBoundary.WriteExtension(DocumentKind.HeroList));
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Hero.PAS:2750
    public void DeleteLibrary(int Index)
    {
        if (Index < FLibraries.GetCount() && Index > 0)
        {
            var name = GetLibrary(Index).GetName();
            // The list goes in both forms, so an original of the same name does not return.
            DeleteFile(GameTables.AoWEngine.StartupDirectory + "User\\" + name + DocumentKind.HeroList.OriginalExtension);
            DeleteFile(GameTables.AoWEngine.StartupDirectory + "User\\" + name + DocumentKind.HeroList.OwnExtension);
            GetLibrary(Index)?.Dispose();
        }
    }

    // intf_P2_07.inc:311

}
