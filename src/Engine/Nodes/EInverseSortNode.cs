// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEInverseSortNode: the same, sorted the other way }
public partial class EInverseSortNode : ESortNode
{
    public new const int ClassID = 262;

    public EInverseSortNode(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:1635
    public override int AddSortChild(EngineObject Child, int SortValue)
    {
        int Result;
        int I;
        if (FCount >= FCapacity)
        {
            Grow();
        }

        if (FSorted)
        {
            I = FCount;
            while (I > 0 && SortValue < FItems[I - 1].SortValue)
            {
                I -= 1;
            }

            if (I != FCount)
            {
                Array.Copy(FItems, I, FItems, I + 1, FCount - I);
            }

            FItems[I].Child = Child;
            FItems[I].SortValue = SortValue;
            Result = I;
        }
        else
        {
            FItems[FCount].Child = Child;
            FItems[FCount].SortValue = SortValue;
            Result = FCount;
        }

        FCount += 1;
        return Result;
    }

    // Engine.pas:1658
    public override void Sort()
    {
        if (!FSorted && FCount > 0)
        {
            InverseSortItems(FItems, 0, FCount - 1);
        }

        FSorted = true;
    }

    private static void InverseSortItems(Span<SortItem> SortList, int L, int R)
    {
        int num;
        do
        {
            num = L;
            int num2 = R;
            int sortValue = SortList[L + R >>> 1].SortValue;
            while (true)
            {
                if (sortValue < SortList[num].SortValue)
                {
                    num++;
                    continue;
                }
                while (sortValue > SortList[num2].SortValue)
                {
                    num2--;
                }
                if (num <= num2)
                {
                    SortItem tSortItem = SortList[num];
                    SortList[num] = SortList[num2];
                    SortList[num2] = tSortItem;
                    num++;
                    num2--;
                }
                if (num > num2)
                {
                    break;
                }
            }
            if (L < num2)
            {
                InverseSortItems(SortList, L, num2);
            }
            L = num;
        }
        while (num < R);
    }

}
