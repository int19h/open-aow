#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// the libraries a program shares, linked by file name. FSearchPath (upper case, ends with a
// backslash), _diskList, _ownedList
public partial class ImageLibraryManager : Component
{

    private string _searchPath = "";
    private readonly ImageLibraryManagerList _diskList;
    private readonly ImageLibraryManagerList _ownedList;

    // ImageLib.pas:8260
    public ImageLibraryManager(Component owner) : base(owner)
    {
        _diskList = new ImageLibraryManagerList(this);
        _ownedList = new ImageLibraryManagerList(this);
    }

    // ImageLib.pas:8267
    public override void Dispose()
    {
        Clear();
        _diskList?.Dispose();
        _ownedList?.Dispose();
        base.Dispose();
    }

    // ImageLib.pas:8275
    public void Clear()
    {
        _diskList.Clear();
        _ownedList.Clear();
    }

    // ImageLib.pas:8281
    public void UnloadImageLibrary(Set<ImageLibraryManagerCategoryElement> category)
    {
        if (category.Contains(ImageLibraryManagerCategoryElement.Disk))
        {
            _diskList.UnloadImageLibrary();
        }

        if (category.Contains(ImageLibraryManagerCategoryElement.Owned))
        {
            _ownedList.UnloadImageLibrary();
        }
    }

    // ImageLib.pas:8288
    // { Lists the .ILB files of the search path and of its subdirectories in FDiskList }
    public void ExecutePathSearch(string path)
    {
        string directory;
        void SearchDirectory(string subDirectory)
        {
            StringList list;
            foreach (RtlFileEntry entry in Files.FindEntries(_searchPath + subDirectory + "*.ilb", attributes: 63 - 16))
            {
                _ = _diskList.AddImageLibraryFile(subDirectory + UpperCase(entry.Name));
            }

            list = new StringList();
            foreach (RtlFileEntry entry in Files.FindEntries(subDirectory + "*.*", attributes: 16))
            {
                if ((entry.Attribute & 16) != 0 && entry.Name != "." && entry.Name != "..")
                {
                    _ = list.Add(subDirectory + UpperCase(entry.Name) + "\\");
                }
            }

            for (int i = 0, last = list.Count - 1; i <= last; i++)
            {
                SearchDirectory(list[i]);
            }

            list?.Dispose();
        }

        _diskList.Clear();
        _searchPath = UpperCase(path);
        if (_searchPath[^1] != 92)
        {
            _searchPath += "\\";
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
            result = _diskList.LinkToImageLibrary(fileName);
        }

        if (category.Contains(ImageLibraryManagerCategoryElement.Owned) && result == null)
        {
            result = _ownedList.LinkToImageLibrary(fileName);
            if (result == null)
            {
                result = new ImageLibrary(null);
                _ = _ownedList.AddImageLibraryManagerObject(fileName, result);
            }
        }

        if (result == null)
        {
            try
            {
                throw new FileNotFoundException("Missing IL Resource: " + fileName);
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
            released = _diskList.ReleaseImageLibrary(fileName);
        }

        if (category.Contains(ImageLibraryManagerCategoryElement.Owned) && !released)
        {
            _ = _ownedList.ReleaseImageLibrary(fileName);
        }
    }

    // ImageLib.pas:8361
    public void SaveToStream(Stream stream) => _ownedList.SaveToStream(stream);

    // ImageLib.pas:8366
    public void LoadFromStream(Stream stream) => _ownedList.LoadFromStream(stream);

    public ImageLibraryManagerList DiskList => _diskList;
    public ImageLibraryManagerList OwnedList => _ownedList;

    [Published]
    public string SearchPath { get => _searchPath; set => _searchPath = value; }

}
