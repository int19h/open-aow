// Translated from EList.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Collections;

[LegacyObject]
// no fields. TObjectList.LoadFromStream constructs items with their ordinary constructor. The
// stream methods are empty here and overridden by the items
public partial class ListObject : IDisposable
{
    // EList.pas:50
    public virtual void SaveToStream(Stream Stream)
    {
    }

    // EList.pas:54
    public virtual void LoadFromStream(Stream Stream)
    {
    }

    public virtual void Dispose()
    {
    }

    public ListObject()
    {
        CaptureAddress.Assign(this);
    }

}

// a TList that owns its TListObjects. A slot can be nil. the stream format is the count
// followed by a nil flag per slot and the item's own data
public partial class ObjectList : DelphiList
{
    // EList.pas:66
    internal ListObject GetListObject(int Index) => (ListObject)List[Index];

    // EList.pas:71
    internal void SetListObject(int Index, ListObject Value)
    {
        while (Index >= Count)
        {
            Add(null);
        }

        if (List[Index] != null)
        {
            ((List[Index] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        List[Index] = Value;
    }

    // EList.pas:60
    public override void Dispose()
    {
        DestroyItems();
        base.Dispose();
    }

    // EList.pas:78
    public void DestroyItems()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            if (List[I] != null)
            {
                ((List[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            }

            Delete(I);
        }
    }

    // EList.pas:89
    public void LoadFromStream(Stream Stream)
    {
        int N = default;
        bool IsNil = default;
        ListObject Obj;
        DestroyItems();
        Stream.ReadBytes(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            Stream.ReadBytes(Bytes.Of(ref IsNil, 1));
            if (!IsNil)
            {
                Obj = new ListObject();
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
    public void SaveToStream(Stream Stream)
    {
        bool IsNil = default;
        Stream.Write(ValueBytes(Count, 4));
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            IsNil = List[I] == null;
            Stream.Write(Bytes.Of(ref IsNil, 1));
            if (!IsNil)
            {
                ((ListObject)List[I]).SaveToStream(Stream);
            }
        }
    }

    public new ListObject this[int Index] { get => GetListObject(Index); set => SetListObject(Index, value); }
}
