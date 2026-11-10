// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// FList
public partial class BlendTableList : IDisposable
{
    internal DelphiList FList;

    // GFXE.pas:9642
    internal int GetCount() => FList.Count;

    // GFXE.pas:9656
    internal BlendTable GetTables(int Index) => (BlendTable)FList.List[Index];

    // GFXE.pas:9629
    public BlendTableList()
    {
        FList = new DelphiList();
    }

    // GFXE.pas:9635
    public virtual void Dispose()
    {
        Clear();
        FList?.Dispose();
    }

    // GFXE.pas:9647
    public void Clear()
    {
        while (FList.Count != 0)
        {
            FList.Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:9661
    public void Add(BlendTable Table)
    {
        FList.Add(Table);
    }

    // GFXE.pas:9666
    public void Delete(int Index)
    {
        FList.Delete(Index);
    }

    public int Count { get => GetCount(); }

    public sealed class TablesAccessor(BlendTableList target)
    {
        public BlendTable this[int Index] { get => target.GetTables(Index); }
    }

    public TablesAccessor Tables => new TablesAccessor(this);

}
