#nullable disable
// Translated from ImageLib.pas.
using Aow.Engine.Collections;

using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Draw.Images;

// library file names (upper case, relative to the search path) with their TILManagerObject.
// FHashTable (the objects by name), FOwner
public class ImageLibraryManagerList(ImageLibraryManager owner) : DelphiStringList<ImageLibraryManagerObject>
{
    internal Dictionary<string, ImageLibraryManagerObject> HashTable { get; } = [with(AnsiCompareComparer.Instance)];

    private readonly ImageLibraryManager _owner = owner;

    // ImageLib.pas:8102
    // { Original bug: the inherited destructor does not call Clear, so the TILManagerObjects and
    //   their libraries are never freed. }
    public override void Dispose() => base.Dispose();

    // ImageLib.pas:8108
    public CustomImageLibrary GetImageLibrary(int index) => Objects[index].ImageLibrary;

    // ImageLib.pas:8113
    public void UnloadImageLibrary()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            ImageLibraryManagerObject objectValue = Objects[i];
            if (objectValue is not null && objectValue.ImageLibrary is not null)
            {
                objectValue.ImageLibrary.ReleaseDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:8126
    public ImageLibraryManagerObject AddImageLibraryFile(string fileName)
    {
        ImageLibraryManagerObject objectValue = new();
        fileName = UpperCase(fileName);
        _ = AddObject(fileName, objectValue);
        HashTables.Add(HashTable, key: fileName, value: objectValue);
        return objectValue;
    }

    // ImageLib.pas:8139
    // { Original bug: the object is deleted from the hash table instead of added, so LinkToIL never
    //   finds the owned libraries. }
    public ImageLibraryManagerObject AddImageLibraryManagerObject(string name, CustomImageLibrary imageLibrary)
    {
        string s = UpperCase(name);
        ImageLibraryManagerObject objectValue = new()
        {
            ImageLibrary = imageLibrary
        };
        _ = AddObject(s, objectValue);
        _ = HashTable.Remove(s);
        return objectValue;
    }

    // ImageLib.pas:8153
    public override void Delete(int index)
    {
        ImageLibraryManagerObject objectValue = Objects[index];
        objectValue?.ImageLibrary?.Dispose();

        string item = Strings[index];
        _ = HashTable.Remove(item);
        base.Delete(index);
    }

    // ImageLib.pas:8167
    public override void Clear()
    {
        HashTable.Clear();
        for (int i = Count - 1; i >= 0; i--)
        {
            ImageLibraryManagerObject objectValue = Objects[i];
            objectValue?.ImageLibrary?.Dispose();
        }

        base.Clear();
    }

    // ImageLib.pas:8187
    // { Links to the library of FileName (relative to the search path), loading it on the first link.
    //   The original clears FileName at the end (a second @LStrClr of the parameter). }
    public CustomImageLibrary LinkToImageLibrary(string fileName)
    {
        fileName = UpperCase(fileName);
        _ = HashTable.TryGetValue(fileName, out ImageLibraryManagerObject objectValue);
        if (objectValue is not null)
        {
            objectValue.RefCount += 1;
            if (objectValue.ImageLibrary is null)
            {
                objectValue.ImageLibrary = new ImageLibrary(null)
                {
                    LoadOnDemand = true
                };
                try
                {
                    objectValue.ImageLibrary.LoadFromFile(_owner.SearchPath + fileName);
                }
                catch
                {
                    throw new IOException($"Error loading: {_owner.SearchPath}{fileName}");
                }
            }

            return objectValue.ImageLibrary;
        }
        else
        {
            return null;
        }
    }

    // ImageLib.pas:8213
    public bool ReleaseImageLibrary(string fileName)
    {
        fileName = UpperCase(fileName);
        _ = HashTable.TryGetValue(fileName, out ImageLibraryManagerObject objectValue);
        if (objectValue is not null)
        {
            objectValue.RefCount -= 1;
            if (objectValue.RefCount == 0)
            {
                objectValue.ImageLibrary.ReleaseDiscardableImageData();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // ImageLib.pas:8229
    public override void LoadFromStream(Stream stream)
    {
        int n = default;
        string name = "";
        stream.ReadExactly(Bytes.Of(ref n, 4));
        for (int i = 0, last = n - 1; i <= last; i++)
        {
            LibraryDrawing.ReadStringFromStream(ref name, stream);
            CustomImageLibrary imageLibrary = new ImageLibrary(null);
            imageLibrary.LoadFromStream(stream);
            _ = AddImageLibraryManagerObject(name, imageLibrary);
        }
    }

    // ImageLib.pas:8245
    public override void SaveToStream(Stream stream)
    {
        int i = Count;
        stream.Write(Bytes.Of(ref i, 4));
        i = 0;
        int last = Count - 1;
        for (; i <= last; i += 1)
        {
            LibraryDrawing.WriteStringToStream(Strings[i], stream);
            Objects[i].ImageLibrary.SaveToStream(stream);
        }
    }
}
