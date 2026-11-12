// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public class DoubleItemList<TA> : RecordList<DoubleItem<TA>> where TA : class
{
    public int AddAbility(TA A, int B) => Add(new DoubleItem<TA> { First = A, Second = B });

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

    public void SortAscendingItemB() => Sort(static (in item1, in item2) => item1.Second - item2.Second);

    public void SortDescendingItemB() => Sort(static (in item1, in item2) => item2.Second - item1.Second);

    // Engine.pas:5422
    public int RandomChanceItemB(int bonus)
    {
        int result;
        int total;
        int chance;
        int R;
        if (_count != 0)
        {
            total = 0;
            for (int I = _count - 1; I >= 0; I--)
            {
                chance = _list[I].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }

            R = Random(total);
            result = 0;
            total = 0;
            chance = _list[0].Second + bonus;
            if (chance > 0)
            {
                total += chance;
            }

            while (total <= R)
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
