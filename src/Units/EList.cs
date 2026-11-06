// Translated from EList.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.EList;

// no fields. TObjectList.LoadFromStream constructs items with their ordinary constructor. The
// stream methods are empty here and overridden by the items
public partial class TListObject : TObject
{
    // EList.pas:50
    public virtual void SaveToStream(TStream Stream)
    {
    }

    // EList.pas:54
    public virtual void LoadFromStream(TStream Stream)
    {
    }

}

// a TList that owns its TListObjects. A slot can be nil. the stream format is the count
// followed by a nil flag per slot and the item's own data
public partial class TObjectList : TList
{
    // EList.pas:66
    internal TListObject GetListObject(int Index) => (TListObject)List[Index];

    // EList.pas:71
    internal void SetListObject(int Index, TListObject Value)
    {
        while (Index >= Count)
        {
            Add(null);
        }

        if (List[Index] != null)
        {
            ((TListObject)List[Index]).Destroy();
        }

        List[Index] = Value;
    }

    // EList.pas:60
    public override void Destroy()
    {
        DestroyItems();
        base.Destroy();
    }

    // EList.pas:78
    public void DestroyItems()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            if (List[I] != null)
            {
                ((TListObject)List[I]).Destroy();
            }

            Delete(I);
        }
    }

    // EList.pas:89
    public void LoadFromStream(TStream Stream)
    {
        int N = default;
        bool IsNil = default;
        TListObject Obj;
        DestroyItems();
        Stream.Read(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            Stream.Read(Bytes.Of(ref IsNil, 1));
            if (!IsNil)
            {
                Obj = new TListObject();
                Obj.LoadFromStream(Stream);
                Add(Obj);
            }
            else
            {
                Add(null);
            }
        }
    }

    // EList.pas:112
    public void SaveToStream(TStream Stream)
    {
        bool IsNil = default;
        Stream.Write(ValueBytes(Count, 4));
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            IsNil = List[I] == null;
            Stream.Write(Bytes.Of(ref IsNil, 1));
            if (!IsNil)
            {
                ((TListObject)List[I]).SaveToStream(Stream);
            }
        }
    }

    public new TListObject this[int Index] { get => GetListObject(Index); set => SetListObject(Index, value); }
}

public static partial class EListUnit
{
}
