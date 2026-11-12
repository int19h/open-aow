// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// inherits the children list
public partial class TextControl : ENode
{
    public TextControl(ECustomNode owner) : base(owner)
    { }

    // Structur.PAS:4419
    public TextControlItem GetItem(int index)
    {
        TextControlItem result;
        if (index >= 0 && index < _children.Count)
        {
            result = (TextControlItem)_children.List[index];
        }
        else
        {
            result = null;
        }

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
    public TextControlItem New() => new TextControlItem(this);

    // Structur.PAS:4436
    public TextControlItem FindId(int id)
    {
        int I;
        I = Count;
        TextControlItem result = null;
        while (result == null && I != 0)
        {
            I -= 1;
            if (((TextControlItem)_children.List[I]).Id == id)
            {
                result = (TextControlItem)_children.List[I];
            }
        }

        return result;
    }

    // Structur.PAS:4458
    public void Sort()
    {
        _children.SortStable(CompareTextControlItems);
    }

    // Structur.PAS:4463
    public int IndexOf(TextControlItem item) => FindChildIndex(item);

    // Structur.PAS:4468
    public void RetrieveText(int id, StringCollection text)
    {
        TextControlItem item;
        text.Clear();
        item = FindId(id);
        if (item != null)
        {
            text.Text = item.Text.Text;
        }
    }

    public static int CompareTextControlItems(object item1, object item2)
    {
        int number = ((TextControlItem)item1).Id - ((TextControlItem)item2).Id;
        if (number == 0)
        {
            number = ((TextControlItem)item1).Text.Count - ((TextControlItem)item2).Text.Count;
        }
        return number;
    }

    // Engine.pas:631

    // Engine.pas:631

}
