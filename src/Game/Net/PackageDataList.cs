// Translated from NetworkE.pas.
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.PointerMemory;

namespace Aow.Game.Net;

// FList, the memory streams of the packages that wait to be received
public class PackageDataList : IDisposable
{
    private readonly List<MemoryStream> _list;

    // NetworkE.pas:870
    public PackageDataList() => _list = [];

    // NetworkE.pas:876
    public virtual void Dispose()
    {
        Clear();
        _list?.Clear();
    }

    // NetworkE.pas:883
    public int GetCount() => _list.Count;

    // NetworkE.pas:888
    public MemoryStream this[int index] => _list[index];

    // NetworkE.pas:893
    public void Clear()
    {
        List<MemoryStream> l = _list;
        while (l.Count != 0)
        {
            (l[^1] as IDisposable)?.Dispose();
            l.RemoveAt(l.Count - 1);
        }
    }

    // NetworkE.pas:905
    public void Add(MemoryStream stream)
    {
        MemoryStream s = new();
        s.SetLength(stream.Length);
        int size = (int)s.Length;
        Move(stream.GetBuffer().AsSpan(0, length: size), destination: s.GetBuffer().AsSpan(0, length: size), count: size);
        _list.Add(s);
    }

    // NetworkE.pas:915
    public void AddData(object data, int size)
    {
        MemoryStream s = new();
        s.SetLength(size);
        Move(BufferBytes(data)[..size], destination: s.GetBuffer().AsSpan(0, length: size), count: size);
        _list.Add(s);
    }

    // NetworkE.pas:925
    public void Insert(int index, MemoryStream stream)
    {
        MemoryStream s = new();
        s.SetLength(stream.Length);
        int size = (int)s.Length;
        Move(stream.GetBuffer().AsSpan(0, length: size), destination: s.GetBuffer().AsSpan(0, length: size), count: size);
        _list.Insert(index, item: s);
    }

    // NetworkE.pas:935
    public void Delete(int index)
    {
        (_list[index] as IDisposable)?.Dispose();
        _list.RemoveAt(index);
    }

    public int Count => GetCount();

    public sealed class PackageDataAccessor(PackageDataList target)
    {
        public MemoryStream this[int index] => target[index];
    }

    public PackageDataAccessor PackageData => new(this);

}
