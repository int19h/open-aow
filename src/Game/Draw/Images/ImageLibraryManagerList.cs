// Translated from ImageLib.pas.
using Aow.Engine.Collections;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// library file names (upper case, relative to the search path) with their TILManagerObject.
// FHashTable (the objects by name), FOwner
public partial class ImageLibraryManagerList : StringList<ImageLibraryManagerObject>
{
    internal Dictionary<string, ImageLibraryManagerObject> HashTable { get; }

    private ImageLibraryManager _owner;

    // ImageLib.pas:8093
    public ImageLibraryManagerList(ImageLibraryManager owner)
    {
        _owner = owner;
        HashTable = new Dictionary<string, ImageLibraryManagerObject>(AnsiCompareComparer.Instance);
    }

    // ImageLib.pas:8102
    // { Original bug: the inherited destructor does not call Clear, so the TILManagerObjects and
    //   their libraries are never freed. }
    public override void Dispose()
    {
        base.Dispose();
    }

    // ImageLib.pas:8108
    public CustomImageLibrary GetImageLibrary(int index) => Objects[index].ImageLibrary;

    // ImageLib.pas:8113
    public void UnloadImageLibrary()
    {
        ImageLibraryManagerObject objectValue;
        for (int I = Count - 1; I >= 0; I--)
        {
            objectValue = Objects[I];
            if (objectValue != null && objectValue.ImageLibrary != null)
            {
                objectValue.ImageLibrary.ReleaseDiscardableImageData();
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
        HashTables.Add(HashTable, key: fileName, value: objectValue);
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
        objectValue.ImageLibrary = imageLibrary;
        AddObject(S, objectValue);
        HashTable.Remove(S);
        return objectValue;
    }

    // ImageLib.pas:8153
    public override void Delete(int index)
    {
        ImageLibraryManagerObject objectValue;
        objectValue = Objects[index];
        if (objectValue != null)
        {
            objectValue.ImageLibrary?.Dispose();
        }

        var item = Strings[index];
        HashTable.Remove(item);
        base.Delete(index);
    }

    // ImageLib.pas:8167
    public override void Clear()
    {
        ImageLibraryManagerObject objectValue;
        HashTable.Clear();
        for (int I = Count - 1; I >= 0; I--)
        {
            objectValue = Objects[I];
            if (objectValue != null)
            {
                objectValue.ImageLibrary?.Dispose();
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
        HashTable.TryGetValue(fileName, out objectValue);
        if (objectValue != null)
        {
            objectValue.RefCount += 1;
            if (objectValue.ImageLibrary == null)
            {
                objectValue.ImageLibrary = new ImageLibrary(null);
                objectValue.ImageLibrary.LoadOnDemand = true;
                try
                {
                    objectValue.ImageLibrary.LoadFromFile(_owner.SearchPath + fileName);
                }
                catch
                {
                    throw new IOException("Error loading: " + _owner.SearchPath + fileName);
                }
            }

            result = objectValue.ImageLibrary;
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
        HashTable.TryGetValue(fileName, out objectValue);
        if (objectValue != null)
        {
            objectValue.RefCount -= 1;
            if (objectValue.RefCount == 0)
            {
                objectValue.ImageLibrary.ReleaseDiscardableImageData();
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
            Objects[I].ImageLibrary.SaveToStream(stream);
        }
    }

}
