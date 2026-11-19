#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Draw.Images;

// the libraries a program shares, linked by file name. FSearchPath (upper case, ends with a
// backslash), _diskList, _ownedList
public partial class ImageLibraryManager : Component
{

    // ImageLib.pas:8260
    public ImageLibraryManager(Component owner) : base(owner)
    {
        DiskList = new ImageLibraryManagerList(this);
        OwnedList = new ImageLibraryManagerList(this);
    }

    // ImageLib.pas:8267
    public override void Dispose()
    {
        Clear();
        DiskList?.Dispose();
        OwnedList?.Dispose();
        base.Dispose();
    }

    // ImageLib.pas:8275
    public void Clear()
    {
        DiskList.Clear();
        OwnedList.Clear();
    }

    // ImageLib.pas:8281
    public void UnloadImageLibrary(Set<ImageLibraryManagerCategoryElement> category)
    {
        if (category.Contains(ImageLibraryManagerCategoryElement.Disk))
        {
            DiskList.UnloadImageLibrary();
        }

        if (category.Contains(ImageLibraryManagerCategoryElement.Owned))
        {
            OwnedList.UnloadImageLibrary();
        }
    }

    // ImageLib.pas:8288
    // { Lists the .ILB files of the search path and of its subdirectories in FDiskList }
    public void ExecutePathSearch(string path)
    {
        string directory;
        void SearchDirectory(string subDirectory)
        {
            DelphiStringList list;
            foreach (RtlFileEntry entry in Files.FindEntries($"{SearchPath}{subDirectory}*.ilb", attributes: 63 - 16))
            {
                _ = DiskList.AddImageLibraryFile(subDirectory + UpperCase(entry.Name));
            }

            list = new DelphiStringList();
            foreach (RtlFileEntry entry in Files.FindEntries($"{subDirectory}*.*", attributes: 16))
            {
                if ((entry.Attribute & 16) != 0 && entry.Name != "." && entry.Name != "..")
                {
                    _ = list.Add($"{subDirectory}{UpperCase(entry.Name)}\\");
                }
            }

            for (int i = 0, last = list.Count - 1; i <= last; i++)
            {
                SearchDirectory(list[i]);
            }

            list?.Dispose();
        }

        DiskList.Clear();
        SearchPath = UpperCase(path);
        if (SearchPath[^1] != 92)
        {
            SearchPath += "\\";
        }

        directory = GetCurrentDir();
        _ = SetCurrentDir(path);
        SearchDirectory("");
        _ = SetCurrentDir(directory);
    }

    // ImageLib.pas:8331
    // { A missing owned library is created empty and added. The exception for a missing library is
    //   raised and swallowed at once (it only shows in the debugger), as in the original. }
    public CustomImageLibrary LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement> category, string fileName)
    {
        CustomImageLibrary result = null;
        if (category.Contains(ImageLibraryManagerCategoryElement.Disk))
        {
            result = DiskList.LinkToImageLibrary(fileName);
        }

        if (category.Contains(ImageLibraryManagerCategoryElement.Owned) && result == null)
        {
            result = OwnedList.LinkToImageLibrary(fileName);
            if (result == null)
            {
                result = new ImageLibrary(null);
                _ = OwnedList.AddImageLibraryManagerObject(fileName, result);
            }
        }

        if (result == null)
        {
            try
            {
                throw new FileNotFoundException($"Missing IL Resource: {fileName}");
            }
            catch
            {
            }
        }

        return result;
    }

    // ImageLib.pas:8352
    public void ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement> category, string fileName)
    {
        bool released;
        released = false;
        if (category.Contains(ImageLibraryManagerCategoryElement.Disk))
        {
            released = DiskList.ReleaseImageLibrary(fileName);
        }

        if (category.Contains(ImageLibraryManagerCategoryElement.Owned) && !released)
        {
            _ = OwnedList.ReleaseImageLibrary(fileName);
        }
    }

    // ImageLib.pas:8361
    public void SaveToStream(Stream stream) => OwnedList.SaveToStream(stream);

    // ImageLib.pas:8366
    public void LoadFromStream(Stream stream) => OwnedList.LoadFromStream(stream);

    public ImageLibraryManagerList DiskList { get; }
    public ImageLibraryManagerList OwnedList { get; }

    [Published]
    public string SearchPath { get; set; } = "";

}
