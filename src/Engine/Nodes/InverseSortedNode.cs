// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEInverseSortNode: the same, sorted the other way }
public class InverseSortedNode(CustomNode owner) : SortedNode(owner)
{
    public new const int ClassId = 262;

    // Engine.pas:1635
    public override int AddSortChild(EngineObject child, int sortValue)
    {
        CheckChild(child);
        if (StoredCount >= Capacity)
        {
            Grow();
        }

        int result;
        if (Sorted)
        {
            int i = StoredCount;
            while (i > 0 && sortValue < Items[i - 1].SortValue)
            {
                i -= 1;
            }

            if (i != StoredCount)
            {
                Array.Copy(Items, i, Items, i + 1, StoredCount - i);
            }

            SortItem[] items = Items;
            items[i].Child = child;
            Items = items;
            SortItem[] items2 = Items;
            items2[i].SortValue = sortValue;
            Items = items2;
            result = i;
        }
        else
        {
            SortItem[] items3 = Items;
            items3[StoredCount].Child = child;
            Items = items3;
            SortItem[] items4 = Items;
            items4[StoredCount].SortValue = sortValue;
            Items = items4;
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
                    (sortList[number2], sortList[number]) = (sortList[number], sortList[number2]);
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
