// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public class DoubleItemList<TA> : RecordList<DoubleItem<TA>> where TA : class
{
    public int AddAbility(TA a, int b) => Add(new DoubleItem<TA> { First = a, Second = b });

    public int IndexOfItemA(TA A)
    {
        int result = 0;
        while (result < Count && !ReferenceEquals(_list[result].First, A))
        {
            result += 1;
        }

        return result == Count ? -1 : result;
    }

    public int IndexOfItemB(int B)
    {
        int result = 0;
        while (result < Count && _list[result].Second != B)
        {
            result += 1;
        }

        return result == Count ? -1 : result;
    }

    public void SortAscendingItemB() => Sort(static (in item1, in item2) => item1.Second - item2.Second);

    public void SortDescendingItemB() => Sort(static (in item1, in item2) => item2.Second - item1.Second);

    // Engine.pas:5422
    public int RandomChanceItemB(int bonus)
    {
        int result;
        int total;
        int chance;
        int r;
        if (Count != 0)
        {
            total = 0;
            for (int i = Count - 1; i >= 0; i--)
            {
                chance = _list[i].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }

            r = Random(total);
            result = 0;
            total = 0;
            chance = _list[0].Second + bonus;
            if (chance > 0)
            {
                total += chance;
            }

            while (total <= r)
            {
                result += 1;
                chance = _list[result].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }
}
