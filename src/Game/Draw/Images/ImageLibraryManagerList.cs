// Translated from ImageLib.pas.
using Aow.Engine.Collections;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// library file names (upper case, relative to the search path) with their TILManagerObject.
// FHashTable (the objects by name), FOwner
public partial class ImageLibraryManagerList : StringList
{
    internal Dictionary<string, ImageLibraryManagerObject> FHashTable;
    internal ImageLibraryManager FOwner;

    // ImageLib.pas:8093
    public ImageLibraryManagerList(ImageLibraryManager owner)
    {
        FOwner = owner;
        FHashTable = new Dictionary<string, ImageLibraryManagerObject>(AnsiCompareComparer.Instance);
    }

    // ImageLib.pas:8102
    // { Original bug: the inherited destructor does not call Clear, so the TILManagerObjects and
    //   their libraries are never freed. }
    public override void Dispose()
    {
        base.Dispose();
    }

    // ImageLib.pas:8108
    public CustomImageLibrary GetIL(int Index) => ((ImageLibraryManagerObject)Objects[Index]).FImageLibrary;

    // ImageLib.pas:8113
    public void UnloadIL()
    {
        ImageLibraryManagerObject Obj;
        for (int I = Count - 1; I >= 0; I--)
        {
            Obj = (ImageLibraryManagerObject)Objects[I];
            if (Obj != null && Obj.FImageLibrary != null)
            {
                Obj.FImageLibrary.ReleaseDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:8126
    public ImageLibraryManagerObject AddILFile(string FileName)
    {
        ImageLibraryManagerObject Obj;
        Obj = new ImageLibraryManagerObject();
        FileName = UpperCase(FileName);
        AddObject(FileName, Obj);
        HashTables.Add(FHashTable, key: FileName, value: Obj);
        return Obj;
    }

    // ImageLib.pas:8139
    // { Original bug: the object is deleted from the hash table instead of added, so LinkToIL never
    //   finds the owned libraries. }
    public ImageLibraryManagerObject AddILManagerObject(string Name, CustomImageLibrary ImageLibrary)
    {
        string S;
        ImageLibraryManagerObject Obj;
        S = UpperCase(Name);
        Obj = new ImageLibraryManagerObject();
        Obj.FImageLibrary = ImageLibrary;
        AddObject(S, Obj);
        FHashTable.Remove(S);
        return Obj;
    }

    // ImageLib.pas:8153
    public override void Delete(int Index)
    {
        ImageLibraryManagerObject Obj;
        Obj = (ImageLibraryManagerObject)Objects[Index];
        if (Obj != null)
        {
            Obj.FImageLibrary?.Dispose();
        }

        var item = Strings[Index];
        FHashTable.Remove(item);
        base.Delete(Index);
    }

    // ImageLib.pas:8167
    public override void Clear()
    {
        ImageLibraryManagerObject Obj;
        FHashTable.Clear();
        for (int I = Count - 1; I >= 0; I--)
        {
            Obj = (ImageLibraryManagerObject)Objects[I];
            if (Obj != null)
            {
                Obj.FImageLibrary?.Dispose();
            }
        }

        base.Clear();
    }

    // ImageLib.pas:8187
    // { Links to the library of FileName (relative to the search path), loading it on the first link.
    //   The original clears FileName at the end (a second @LStrClr of the parameter). }
    public CustomImageLibrary LinkToIL(string FileName)
    {
        CustomImageLibrary Result;
        ImageLibraryManagerObject Obj;
        FileName = UpperCase(FileName);
        FHashTable.TryGetValue(FileName, out Obj);
        if (Obj != null)
        {
            Obj.FRefCount += 1;
            if (Obj.FImageLibrary == null)
            {
                Obj.FImageLibrary = new ImageLibrary(null);
                Obj.FImageLibrary.LoadOnDemand = true;
                try
                {
                    Obj.FImageLibrary.LoadFromFile(FOwner.FSearchPath + FileName);
                }
                catch
                {
                    throw new IOException("Error loading: " + FOwner.FSearchPath + FileName);
                }
            }

            Result = Obj.FImageLibrary;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:8213
    public bool ReleaseIL(string FileName)
    {
        bool Result;
        ImageLibraryManagerObject Obj;
        FileName = UpperCase(FileName);
        FHashTable.TryGetValue(FileName, out Obj);
        if (Obj != null)
        {
            Obj.FRefCount -= 1;
            if (Obj.FRefCount == 0)
            {
                Obj.FImageLibrary.ReleaseDiscardableImageData();
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ImageLib.pas:8229
    public override void LoadFromStream(Stream Stream)
    {
        int N = default;
        string Name = "";
        CustomImageLibrary IL;
        Stream.ReadExactly(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            LibraryDrawing.ReadStringFromStream(ref Name, Stream);
            IL = new ImageLibrary(null);
            IL.LoadFromStream(Stream);
            AddILManagerObject(Name, IL);
        }
    }

    // ImageLib.pas:8245
    public override void SaveToStream(Stream Stream)
    {
        int I;
        I = Count;
        Stream.Write(Bytes.Of(ref I, 4));
        I = 0;
        int last = Count - 1;
        for (; I <= last; I += 1)
        {
            LibraryDrawing.WriteStringToStream(Strings[I], Stream);
            ((ImageLibraryManagerObject)Objects[I]).FImageLibrary.SaveToStream(Stream);
        }
    }

}
