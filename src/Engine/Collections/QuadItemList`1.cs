// Translated from Engine.pas.

namespace Aow.Engine.Collections;

public class QuadItemList<TA> : RecordList<QuadItem<TA>> where TA : class
{
    public int AddABCD(TA A, int B, int C, int D) => Add(new QuadItem<TA> { First = A, Second = B, Third = C, Fourth = D });

    public int IndexOfItemA(TA A)
    {
        int result = 0;
        while (result < _count && !ReferenceEquals(_list[result].First, A))
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    public int IndexOfItemB(int B)
    {
        int result = 0;
        while (result < _count && _list[result].Second != B)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    public int IndexOfItemC(int C)
    {
        int result = 0;
        while (result < _count && _list[result].Third != C)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    public int IndexOfItemD(int D)
    {
        int result = 0;
        while (result < _count && _list[result].Fourth != D)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    public void SortAscendingItemB() => Sort(static (in item1, in item2) => item1.Second - item2.Second);

    public void SortAscendingItemC() => Sort(static (in item1, in item2) => item1.Third - item2.Third);

    public void SortAscendingItemD() => Sort(static (in item1, in item2) => item1.Fourth - item2.Fourth);

    public void SortDescendingItemB() => Sort(static (in item1, in item2) => item2.Second - item1.Second);

    public void SortDescendingItemC() => Sort(static (in item1, in item2) => item2.Third - item1.Third);

    public void SortDescendingItemD() => Sort(static (in item1, in item2) => item2.Fourth - item1.Fourth);
}
