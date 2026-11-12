// Translated from ImageLib.pas.
using Aow.Engine.Collections;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// library file names (upper case, relative to the search path) with their TILManagerObject.
// FHashTable (the objects by name), FOwner
public partial class ImageLibraryManagerList : StringList
{
    internal Dictionary<string, ImageLibraryManagerObject> _hashTable;
    internal ImageLibraryManager _owner;

    // ImageLib.pas:8093
    public ImageLibraryManagerList(ImageLibraryManager owner)
    {
        _owner = owner;
        _hashTable = new Dictionary<string, ImageLibraryManagerObject>(AnsiCompareComparer.Instance);
    }

    // ImageLib.pas:8102
    // { Original bug: the inherited destructor does not call Clear, so the TILManagerObjects and
    //   their libraries are never freed. }
    public override void Dispose()
    {
        base.Dispose();
    }

    // ImageLib.pas:8108
    public CustomImageLibrary GetImageLibrary(int index) => ((ImageLibraryManagerObject)Objects[index])._imageLibrary;

    // ImageLib.pas:8113
    public void UnloadImageLibrary()
    {
        ImageLibraryManagerObject objectValue;
        for (int I = Count - 1; I >= 0; I--)
        {
            objectValue = (ImageLibraryManagerObject)Objects[I];
            if (objectValue != null && objectValue._imageLibrary != null)
            {
                objectValue._imageLibrary.ReleaseDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:8126
    public ImageLibraryManagerObject AddImageLibraryFile(string fileName)
    {
        ImageLibraryManagerObject objectValue;
        objectValue = new ImageLibraryManagerObject();
        fileName = UpperCase(fileName);
        AddObject(fileName, objectValue);
        HashTables.Add(_hashTable, key: fileName, value: objectValue);
        return objectValue;
    }

    // ImageLib.pas:8139
    // { Original bug: the object is deleted from the hash table instead of added, so LinkToIL never
    //   finds the owned libraries. }
    public ImageLibraryManagerObject AddImageLibraryManagerObject(string name, CustomImageLibrary imageLibrary)
    {
        string S;
        ImageLibraryManagerObject objectValue;
        S = UpperCase(name);
        objectValue = new ImageLibraryManagerObject();
        objectValue._imageLibrary = imageLibrary;
        AddObject(S, objectValue);
        _hashTable.Remove(S);
        return objectValue;
    }

    // ImageLib.pas:8153
    public override void Delete(int index)
    {
        ImageLibraryManagerObject objectValue;
        objectValue = (ImageLibraryManagerObject)Objects[index];
        if (objectValue != null)
        {
            objectValue._imageLibrary?.Dispose();
        }

        var item = Strings[index];
        _hashTable.Remove(item);
        base.Delete(index);
    }

    // ImageLib.pas:8167
    public override void Clear()
    {
        ImageLibraryManagerObject objectValue;
        _hashTable.Clear();
        for (int I = Count - 1; I >= 0; I--)
        {
            objectValue = (ImageLibraryManagerObject)Objects[I];
            if (objectValue != null)
            {
                objectValue._imageLibrary?.Dispose();
            }
        }

        base.Clear();
    }

    // ImageLib.pas:8187
    // { Links to the library of FileName (relative to the search path), loading it on the first link.
    //   The original clears FileName at the end (a second @LStrClr of the parameter). }
    public CustomImageLibrary LinkToImageLibrary(string fileName)
    {
        CustomImageLibrary result;
        ImageLibraryManagerObject objectValue;
        fileName = UpperCase(fileName);
        _hashTable.TryGetValue(fileName, out objectValue);
        if (objectValue != null)
        {
            objectValue._refCount += 1;
            if (objectValue._imageLibrary == null)
            {
                objectValue._imageLibrary = new ImageLibrary(null);
                objectValue._imageLibrary.LoadOnDemand = true;
                try
                {
                    objectValue._imageLibrary.LoadFromFile(_owner._searchPath + fileName);
                }
                catch
                {
                    throw new IOException("Error loading: " + _owner._searchPath + fileName);
                }
            }

            result = objectValue._imageLibrary;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // ImageLib.pas:8213
    public bool ReleaseImageLibrary(string fileName)
    {
        bool result;
        ImageLibraryManagerObject objectValue;
        fileName = UpperCase(fileName);
        _hashTable.TryGetValue(fileName, out objectValue);
        if (objectValue != null)
        {
            objectValue._refCount -= 1;
            if (objectValue._refCount == 0)
            {
                objectValue._imageLibrary.ReleaseDiscardableImageData();
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // ImageLib.pas:8229
    public override void LoadFromStream(Stream stream)
    {
        int N = default;
        string name = "";
        CustomImageLibrary imageLibrary;
        stream.ReadExactly(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            LibraryDrawing.ReadStringFromStream(ref name, stream);
            imageLibrary = new ImageLibrary(null);
            imageLibrary.LoadFromStream(stream);
            AddImageLibraryManagerObject(name, imageLibrary);
        }
    }

    // ImageLib.pas:8245
    public override void SaveToStream(Stream stream)
    {
        int I;
        I = Count;
        stream.Write(Bytes.Of(ref I, 4));
        I = 0;
        int last = Count - 1;
        for (; I <= last; I += 1)
        {
            LibraryDrawing.WriteStringToStream(Strings[I], stream);
            ((ImageLibraryManagerObject)Objects[I])._imageLibrary.SaveToStream(stream);
        }
    }

}
