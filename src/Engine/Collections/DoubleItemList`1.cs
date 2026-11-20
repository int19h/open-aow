// Translated from Engine.pas.
using static Aow.Rtl.DelphiRandom;

namespace Aow.Engine.Collections;

public sealed class DoubleItemList<TA> : RecordList<DoubleItem<TA>> where TA : class
{
    public int AddAbility(TA a, int b) => Add(new DoubleItem<TA> { First = a, Second = b });

    public int IndexOfItemA(TA A)
    {
        int result = 0;
        while (result < Count && !ReferenceEquals(List[result].First, A))
        {
            result += 1;
        }

        return result == Count ? -1 : result;
    }

    public int IndexOfItemB(int B)
    {
        int result = 0;
        while (result < Count && List[result].Second != B)
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
        if (Count != 0)
        {
            int total = 0;
            int chance;
            for (int i = Count - 1; i >= 0; i--)
            {
                chance = List[i].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }

            int r = Random(total);
            result = 0;
            total = 0;
            chance = List[0].Second + bonus;
            if (chance > 0)
            {
                total += chance;
            }

            while (total <= r)
            {
                result += 1;
                chance = List[result].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }
        }
        else
        {
            return -1;
        }

        return result;
    }
}
