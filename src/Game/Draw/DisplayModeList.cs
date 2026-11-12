// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// _list
public partial class DisplayModeList : IDisposable
{
    internal List<DisplayModeItem> _list;

    // GFXE.pas:12629
    internal DisplayModeItem GetItems(int index) => _list[index];

    // GFXE.pas:12634
    internal int GetCount() => _list.Count;

    // GFXE.pas:12611
    public DisplayModeList()
    {
        _list = new List<DisplayModeItem>();
    }

    // GFXE.pas:12617
    public virtual void Dispose()
    {
        Clear();
        _list?.Clear();
    }

    // GFXE.pas:12624
    public void Clear()
    {
        while (_list.Count != 0)
        {
            Delete(_list.Count - 1);
        }
    }

    // GFXE.pas:12639
    public void Add(int width, int height, int pixelFormatId)
    {
        DisplayModeItem item;
        item = new DisplayModeItem();
        item.Width = width;
        item.Height = height;
        item.PixelFormatId = pixelFormatId;
        _list.Add(item);
    }

    // GFXE.pas:12650
    public void Delete(int index)
    {
        _list.RemoveAt(index);
    }

    public sealed class ItemsAccessor(DisplayModeList target)
    {
        public DisplayModeItem this[int index] { get => target.GetItems(index); }
    }

    public ItemsAccessor Items => new ItemsAccessor(this);
    public int Count { get => GetCount(); }

}
