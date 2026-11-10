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
    public TextControlItem GetItem(int Index)
    {
        TextControlItem Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (TextControlItem)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:4426
    public void Delete(int Index)
    {
        if (GetItem(Index) != null)
        {
            GetItem(Index)?.Dispose();
        }
    }

    // Structur.PAS:4431
    public TextControlItem New() => new TextControlItem(this);

    // Structur.PAS:4436
    public TextControlItem FindID(int ID)
    {
        int I;
        I = Count;
        TextControlItem Result = null;
        while (Result == null && I != 0)
        {
            I -= 1;
            if (((TextControlItem)FChildren.List[I]).FID == ID)
            {
                Result = (TextControlItem)FChildren.List[I];
            }
        }

        return Result;
    }

    // Structur.PAS:4458
    public void Sort()
    {
        FChildren.SortStable(CompareTextControlItems);
    }

    // Structur.PAS:4463
    public int IndexOf(TextControlItem Item) => FindChildIndex(Item);

    // Structur.PAS:4468
    public void RetrieveText(int ID, StringCollection Text)
    {
        TextControlItem Item;
        Text.Clear();
        Item = FindID(ID);
        if (Item != null)
        {
            Text.Text = Item.FText.Text;
        }
    }

    public static int CompareTextControlItems(object Item1, object Item2)
    {
        int num = ((TextControlItem)Item1).FID - ((TextControlItem)Item2).FID;
        if (num == 0)
        {
            num = ((TextControlItem)Item1).FText.Count - ((TextControlItem)Item2).FText.Count;
        }
        return num;
    }

    // Engine.pas:631

    // Engine.pas:631

}
