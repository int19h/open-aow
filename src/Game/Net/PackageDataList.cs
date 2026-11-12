// Translated from NetworkE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FList, the memory streams of the packages that wait to be received
public partial class PackageDataList : IDisposable
{
    private List<MemoryStream> _list;

    // NetworkE.pas:870
    public PackageDataList()
    {
        _list = new List<MemoryStream>();
    }

    // NetworkE.pas:876
    public virtual void Dispose()
    {
        Clear();
        _list?.Clear();
    }

    // NetworkE.pas:883
    public int GetCount() => _list.Count;

    // NetworkE.pas:888
    public MemoryStream GetPackageData(int index) => _list[index];

    // NetworkE.pas:893
    public void Clear()
    {
        List<MemoryStream> L;
        L = _list;
        while (L.Count != 0)
        {
            (L[L.Count - 1] as IDisposable)?.Dispose();
            L.RemoveAt(L.Count - 1);
        }
    }

    // NetworkE.pas:905
    public void Add(MemoryStream stream)
    {
        MemoryStream S;
        S = new MemoryStream();
        S.SetLength(stream.Length);
        var size = (int)S.Length;
        Move(stream.GetBuffer().AsSpan(0, size), S.GetBuffer().AsSpan(0, size), size);
        _list.Add(S);
    }

    // NetworkE.pas:915
    public void AddData(object data, int size)
    {
        MemoryStream S;
        S = new MemoryStream();
        S.SetLength(size);
        Move(BufferBytes(data)[..size], S.GetBuffer().AsSpan(0, size), size);
        _list.Add(S);
    }

    // NetworkE.pas:925
    public void Insert(int index, MemoryStream stream)
    {
        MemoryStream S;
        S = new MemoryStream();
        S.SetLength(stream.Length);
        var size = (int)S.Length;
        Move(stream.GetBuffer().AsSpan(0, size), S.GetBuffer().AsSpan(0, size), size);
        _list.Insert(index, S);
    }

    // NetworkE.pas:935
    public void Delete(int index)
    {
        (_list[index] as IDisposable)?.Dispose();
        _list.RemoveAt(index);
    }

    public int Count { get => GetCount(); }

    public sealed class PackageDataAccessor(PackageDataList target)
    {
        public MemoryStream this[int index] { get => target.GetPackageData(index); }
    }

    public PackageDataAccessor PackageData => new PackageDataAccessor(this);

}
