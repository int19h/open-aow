// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// FList
public partial class DisplayModeList : IDisposable
{
    internal DelphiList FList;

    // GFXE.pas:12629
    internal DisplayModeItem GetItems(int Index) => (DisplayModeItem)FList.List[Index];

    // GFXE.pas:12634
    internal int GetCount() => FList.Count;

    // GFXE.pas:12611
    public DisplayModeList()
    {
        FList = new DelphiList();
    }

    // GFXE.pas:12617
    public virtual void Dispose()
    {
        Clear();
        FList?.Dispose();
    }

    // GFXE.pas:12624
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:12639
    public void Add(int AWidth, int AHeight, int APixelFormatID)
    {
        DisplayModeItem Item;
        Item = new DisplayModeItem();
        Item.Width = AWidth;
        Item.Height = AHeight;
        Item.PixelFormatID = APixelFormatID;
        FList.Add(Item);
    }

    // GFXE.pas:12650
    public void Delete(int Index)
    {
        FList.Delete(Index);
    }

    public sealed class ItemsAccessor(DisplayModeList target)
    {
        public DisplayModeItem this[int Index] { get => target.GetItems(Index); }
    }

    public ItemsAccessor Items => new ItemsAccessor(this);
    public int Count { get => GetCount(); }

}
