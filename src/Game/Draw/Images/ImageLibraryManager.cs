// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// the libraries a program shares, linked by file name. FSearchPath (upper case, ends with a
// backslash), _diskList, _ownedList
public partial class ImageLibraryManager : Component
{

    internal string _searchPath = "";
    internal ImageLibraryManagerList _diskList;
    internal ImageLibraryManagerList _ownedList;

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
            SearchRec sr = default;
            bool found;
            found = FindFirst(_searchPath + subDirectory + "*.ilb", 63 - 16, ref sr) == 0;
            while (found)
            {
                _diskList.AddImageLibraryFile(subDirectory + UpperCase(sr.Name));
                found = FindNext(ref sr) == 0;
            }

            FindClose(ref sr);
            list = new StringList();
            found = FindFirst(subDirectory + "*.*", 16, ref sr) == 0;
            while (found)
            {
                if ((sr.Attr & 16) != 0 && sr.Name != "." && sr.Name != "..")
                {
                    list.Add(subDirectory + UpperCase(sr.Name) + "\\");
                }

                found = FindNext(ref sr) == 0;
            }

            FindClose(ref sr);
            for (int I = 0, last = list.Count - 1; I <= last; I++)
            {
                SearchDirectory(list[I]);
            }

            list?.Dispose();
        }

        _diskList.Clear();
        _searchPath = UpperCase(path);
        if (_searchPath[_searchPath.Length - 1] != 92)
        {
            _searchPath = _searchPath + "\\";
        }

        directory = GetCurrentDir();
        SetCurrentDir(path);
        SearchDirectory("");
        SetCurrentDir(directory);
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
                _ownedList.AddImageLibraryManagerObject(fileName, result);
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
            _ownedList.ReleaseImageLibrary(fileName);
        }
    }

    // ImageLib.pas:8361
    public void SaveToStream(Stream stream)
    {
        _ownedList.SaveToStream(stream);
    }

    // ImageLib.pas:8366
    public void LoadFromStream(Stream stream)
    {
        _ownedList.LoadFromStream(stream);
    }

    public ImageLibraryManagerList DiskList { get => _diskList; }
    public ImageLibraryManagerList OwnedList { get => _ownedList; }

    [Published]
    public string SearchPath { get => _searchPath; set => _searchPath = value; }

}
