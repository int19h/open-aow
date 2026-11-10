// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// the libraries a program shares, linked by file name. FSearchPath (upper case, ends with a
// backslash), FDiskList, FOwnedList
public partial class ImageLibraryManager : Component
{

    internal string FSearchPath = "";
    internal ILManagerList FDiskList;
    internal ILManagerList FOwnedList;

    // ImageLib.pas:8260
    public ImageLibraryManager(Component owner) : base(owner)
    {
        FDiskList = new ILManagerList(this);
        FOwnedList = new ILManagerList(this);
    }

    // ImageLib.pas:8267
    public override void Dispose()
    {
        Clear();
        FDiskList?.Dispose();
        FOwnedList?.Dispose();
        base.Dispose();
    }

    // ImageLib.pas:8275
    public void Clear()
    {
        FDiskList.Clear();
        FOwnedList.Clear();
    }

    // ImageLib.pas:8281
    public void UnloadIL(Set<ILManagerCategoryElement> Category)
    {
        if (Category.Contains(ILManagerCategoryElement.Disk))
        {
            FDiskList.UnloadIL();
        }

        if (Category.Contains(ILManagerCategoryElement.Owned))
        {
            FOwnedList.UnloadIL();
        }
    }

    // ImageLib.pas:8288
    // { Lists the .ILB files of the search path and of its subdirectories in FDiskList }
    public void ExecutePathSearch(string Path)
    {
        string Dir;
        void SearchDirectory(string SubDirectory)
        {
            StringList List;
            SearchRec SR = default;
            bool Found;
            Found = FindFirst(FSearchPath + SubDirectory + "*.ilb", 63 - 16, ref SR) == 0;
            while (Found)
            {
                FDiskList.AddILFile(SubDirectory + UpperCase(SR.Name));
                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            List = new StringList();
            Found = FindFirst(SubDirectory + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    List.Add(SubDirectory + UpperCase(SR.Name) + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = List.Count - 1; I <= last; I++)
            {
                SearchDirectory(List[I]);
            }

            List?.Dispose();
        }

        FDiskList.Clear();
        FSearchPath = UpperCase(Path);
        if (FSearchPath[FSearchPath.Length - 1] != 92)
        {
            FSearchPath = FSearchPath + "\\";
        }

        Dir = GetCurrentDir();
        SetCurrentDir(Path);
        SearchDirectory("");
        SetCurrentDir(Dir);
    }

    // ImageLib.pas:8331
    // { A missing owned library is created empty and added. The exception for a missing library is
    //   raised and swallowed at once (it only shows in the debugger), as in the original. }
    public CustomImageLibrary LinkToIL(Set<ILManagerCategoryElement> Category, string FileName)
    {
        CustomImageLibrary Result = null;
        if (Category.Contains(ILManagerCategoryElement.Disk))
        {
            Result = FDiskList.LinkToIL(FileName);
        }

        if (Category.Contains(ILManagerCategoryElement.Owned) && Result == null)
        {
            Result = FOwnedList.LinkToIL(FileName);
            if (Result == null)
            {
                Result = new ImageLibrary(null);
                FOwnedList.AddILManagerObject(FileName, Result);
            }
        }

        if (Result == null)
        {
            try
            {
                throw new FileNotFoundException("Missing IL Resource: " + FileName);
            }
            catch
            {
            }
        }

        return Result;
    }

    // ImageLib.pas:8352
    public void ReleaseIL(Set<ILManagerCategoryElement> Category, string FileName)
    {
        bool Released;
        Released = false;
        if (Category.Contains(ILManagerCategoryElement.Disk))
        {
            Released = FDiskList.ReleaseIL(FileName);
        }

        if (Category.Contains(ILManagerCategoryElement.Owned) && !Released)
        {
            FOwnedList.ReleaseIL(FileName);
        }
    }

    // ImageLib.pas:8361
    public void SaveToStream(Stream Stream)
    {
        FOwnedList.SaveToStream(Stream);
    }

    // ImageLib.pas:8366
    public void LoadFromStream(Stream Stream)
    {
        FOwnedList.LoadFromStream(Stream);
    }

    public ILManagerList DiskList { get => FDiskList; }
    public ILManagerList OwnedList { get => FOwnedList; }

    [Published]
    public string SearchPath { get => FSearchPath; set => FSearchPath = value; }

}
