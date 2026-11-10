// Translated from NetworkE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// FList, the memory streams of the packages that wait to be received
public partial class PackageDataList : IDisposable
{
    internal DelphiList FList;

    // NetworkE.pas:870
    public PackageDataList()
    {
        FList = new DelphiList();
    }

    // NetworkE.pas:876
    public virtual void Dispose()
    {
        Clear();
        FList?.Dispose();
    }

    // NetworkE.pas:883
    public int GetCount() => FList.Count;

    // NetworkE.pas:888
    public MemoryStream GetPackageData(int Index) => (MemoryStream)FList.List[Index];

    // NetworkE.pas:893
    public void Clear()
    {
        DelphiList L;
        L = FList;
        while (L.Count != 0)
        {
            (L.List[L.Count - 1] as IDisposable)?.Dispose();
            L.Delete(L.Count - 1);
        }
    }

    // NetworkE.pas:905
    public void Add(MemoryStream Stream)
    {
        MemoryStream S;
        S = new MemoryStream();
        S.SetLength(Stream.Length);
        var size = (int)S.Length;
        Move(Stream.GetBuffer().AsSpan(0, size), S.GetBuffer().AsSpan(0, size), size);
        FList.Add(S);
    }

    // NetworkE.pas:915
    public void AddData(object Data, int Size)
    {
        MemoryStream S;
        S = new MemoryStream();
        S.SetLength(Size);
        Move(BufferBytes(Data)[..Size], S.GetBuffer().AsSpan(0, Size), Size);
        FList.Add(S);
    }

    // NetworkE.pas:925
    public void Insert(int Index, MemoryStream Stream)
    {
        MemoryStream S;
        S = new MemoryStream();
        S.SetLength(Stream.Length);
        var size = (int)S.Length;
        Move(Stream.GetBuffer().AsSpan(0, size), S.GetBuffer().AsSpan(0, size), size);
        FList.Insert(Index, S);
    }

    // NetworkE.pas:935
    public void Delete(int Index)
    {
        (FList.List[Index] as IDisposable)?.Dispose();
        FList.Delete(Index);
    }

    public int Count { get => GetCount(); }

    public sealed class PackageDataAccessor(PackageDataList target)
    {
        public MemoryStream this[int Index] { get => target.GetPackageData(Index); }
    }

    public PackageDataAccessor PackageData => new PackageDataAccessor(this);

}
