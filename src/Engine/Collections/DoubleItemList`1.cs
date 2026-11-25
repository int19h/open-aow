// Translated from Engine.pas.
using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Engine.Collections;

public sealed class DoubleItemList<TA> : RecordList<DoubleItem<TA>> where TA : class
{
    public int AddAbility(TA a, int b) => Add(new DoubleItem<TA> { First = a, Second = b });

    // In range the indexer returns List[i]. ReferenceEquals keeps the loop's test.
    public int IndexOfItemA(TA A) => this.FindIndex(entry => ReferenceEquals(entry.First, A));

    public int IndexOfItemB(int B) => this.FindIndex(entry => entry.Second == B);

    public void SortAscendingItemB() => Sort(static (in item1, in item2) => item1.Second - item2.Second);

    public void SortDescendingItemB() => Sort(static (in item1, in item2) => item2.Second - item1.Second);

    // Engine.pas:5422
    public int RandomChanceItemB(int bonus)
    {
        int result;
        if (Count != 0)
        {
            // The draw below stays a loop. A non-positive weight adds nothing, as the loop did.
            int total = this.Sum(entry => Math.Max(entry.Second + bonus, val2: 0));
            int chance;

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
