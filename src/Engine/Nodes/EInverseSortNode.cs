// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEInverseSortNode: the same, sorted the other way }
public partial class EInverseSortNode : ESortNode
{
    public new const int ClassId = 262;

    public EInverseSortNode(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:1635
    public override int AddSortChild(EngineObject child, int sortValue)
    {
        int result;
        int I;
        CheckChild(child);
        if (_count >= Capacity)
        {
            Grow();
        }

        if (Sorted)
        {
            I = _count;
            while (I > 0 && sortValue < Items[I - 1].SortValue)
            {
                I -= 1;
            }

            if (I != _count)
            {
                Array.Copy(Items, I, Items, I + 1, _count - I);
            }

            Items[I].Child = child;
            Items[I].SortValue = sortValue;
            result = I;
        }
        else
        {
            Items[_count].Child = child;
            Items[_count].SortValue = sortValue;
            result = _count;
        }

        _count += 1;
        return result;
    }

    // Engine.pas:1658
    public override void Sort()
    {
        if (!Sorted && _count > 0)
        {
            InverseSortItems(Items, 0, _count - 1);
        }

        Sorted = true;
    }

    private static void InverseSortItems(Span<SortItem> sortList, int L, int R)
    {
        int number;
        do
        {
            number = L;
            int number2 = R;
            int sortValue = sortList[L + R >>> 1].SortValue;
            while (true)
            {
                if (sortValue < sortList[number].SortValue)
                {
                    number++;
                    continue;
                }
                while (sortValue > sortList[number2].SortValue)
                {
                    number2--;
                }
                if (number <= number2)
                {
                    SortItem sortItem = sortList[number];
                    sortList[number] = sortList[number2];
                    sortList[number2] = sortItem;
                    number++;
                    number2--;
                }
                if (number > number2)
                {
                    break;
                }
            }
            if (L < number2)
            {
                InverseSortItems(sortList, L, number2);
            }
            L = number;
        }
        while (number < R);
    }

}
