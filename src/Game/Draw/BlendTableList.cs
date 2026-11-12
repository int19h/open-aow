// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// _list
public partial class BlendTableList : IDisposable
{
    private readonly List<BlendTable> _list;

    // GFXE.pas:9642
    internal int GetCount() => _list.Count;

    // GFXE.pas:9656
    internal BlendTable GetTables(int index) => _list[index];

    // GFXE.pas:9629
    public BlendTableList()
    {
        _list = new List<BlendTable>();
    }

    // GFXE.pas:9635
    public virtual void Dispose()
    {
        Clear();
        _list?.Clear();
    }

    // GFXE.pas:9647
    public void Clear()
    {
        while (_list.Count != 0)
        {
            _list.RemoveAt(_list.Count - 1);
        }
    }

    // GFXE.pas:9661
    public void Add(BlendTable table)
    {
        _list.Add(table);
    }

    // GFXE.pas:9666
    public void Delete(int index)
    {
        _list.RemoveAt(index);
    }

    public int Count { get => GetCount(); }

    public sealed class TablesAccessor(BlendTableList target)
    {
        public BlendTable this[int index] { get => target.GetTables(index); }
    }

    public TablesAccessor Tables => new TablesAccessor(this);

}
