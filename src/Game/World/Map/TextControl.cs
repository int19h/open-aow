#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// inherits the children list
public partial class TextControl(CustomNode owner) : OwningNode(owner), IChildList<TextControlItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a TextControlItem, so the cast never fails.
    public TextControlItem this[int index] => GetChild<TextControlItem>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is TextControlItem;

    int IChildSink<TextControlItem>.AddChild(TextControlItem child) => AddChild(child);

    // Structur.PAS:4419
    public TextControlItem GetItem(int index)
    {
        TextControlItem result = index >= 0 && index < Count ? this[index] : null;
        return result;
    }

    // Structur.PAS:4426
    public void Delete(int index)
    {
        if (GetItem(index) != null)
        {
            GetItem(index)?.Dispose();
        }
    }

    // Structur.PAS:4431
    public TextControlItem New() => new(this);

    // Structur.PAS:4436
    public TextControlItem FindId(int id)
    {
        int i;
        i = Count;
        TextControlItem result = null;
        while (result == null && i != 0)
        {
            i -= 1;
            if (this[i].Id == id)
            {
                result = this[i];
            }
        }

        return result;
    }

    // Structur.PAS:4458
    public void Sort() => SortChildrenStable<TextControlItem>(CompareTextControlItems);

    // Structur.PAS:4463
    public int IndexOf(TextControlItem item) => FindChildIndex(item);

    // Structur.PAS:4468
    public void RetrieveText(int id, DelphiStringCollection text)
    {
        TextControlItem item;
        text.Clear();
        item = FindId(id);
        if (item != null)
        {
            text.Text = item.Text.Text;
        }
    }

    public static int CompareTextControlItems(TextControlItem item1, TextControlItem item2)
    {
        int number = item1.Id - item2.Id;
        if (number == 0)
        {
            number = item1.Text.Count - item2.Text.Count;
        }
        return number;
    }

    // Engine.pas:631

    // Engine.pas:631

}
