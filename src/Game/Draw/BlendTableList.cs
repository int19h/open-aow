// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// FList
public partial class BlendTableList : IDisposable
{
    internal List<BlendTable> FList;

    // GFXE.pas:9642
    internal int GetCount() => FList.Count;

    // GFXE.pas:9656
    internal BlendTable GetTables(int Index) => FList[Index];

    // GFXE.pas:9629
    public BlendTableList()
    {
        FList = new List<BlendTable>();
    }

    // GFXE.pas:9635
    public virtual void Dispose()
    {
        Clear();
        FList?.Clear();
    }

    // GFXE.pas:9647
    public void Clear()
    {
        while (FList.Count != 0)
        {
            FList.RemoveAt(FList.Count - 1);
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
        FList.RemoveAt(Index);
    }

    public int Count { get => GetCount(); }

    public sealed class TablesAccessor(BlendTableList target)
    {
        public BlendTable this[int Index] { get => target.GetTables(Index); }
    }

    public TablesAccessor Tables => new TablesAccessor(this);

}
