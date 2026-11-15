// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEInverseSortNode: the same, sorted the other way }
public partial class InverseSortedNode : SortedNode
{
    public new const int ClassId = 262;

    public InverseSortedNode(CustomNode owner) : base(owner)
    { }

    // Engine.pas:1635
    public override int AddSortChild(EngineObject child, int sortValue)
    {
        int result;
        int i;
        CheckChild(child);
        if (StoredCount >= Capacity)
        {
            Grow();
        }

        if (Sorted)
        {
            i = StoredCount;
            while (i > 0 && sortValue < Items[i - 1].SortValue)
            {
                i -= 1;
            }

            if (i != StoredCount)
            {
                Array.Copy(Items, i, Items, i + 1, StoredCount - i);
            }

            Items[i].Child = child;
            Items[i].SortValue = sortValue;
            result = i;
        }
        else
        {
            Items[StoredCount].Child = child;
            Items[StoredCount].SortValue = sortValue;
            result = StoredCount;
        }

        StoredCount += 1;
        return result;
    }

    // Engine.pas:1658
    public override void Sort()
    {
        if (!Sorted && StoredCount > 0)
        {
            InverseSortItems(Items, 0, StoredCount - 1);
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
