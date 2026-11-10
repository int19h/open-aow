// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public class DoubleItemList<TA> : RecordList<DoubleItem<TA>> where TA : class
{
    public int AddAB(TA A, int B) => Add(new DoubleItem<TA> { A = A, B = B });

    public int IndexOfItemA(TA A)
    {
        int Result = 0;
        while (Result < FCount && !ReferenceEquals(FList[Result].A, A))
        {
            Result += 1;
        }

        return Result == FCount ? -1 : Result;
    }

    public int IndexOfItemB(int B)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].B != B)
        {
            Result += 1;
        }

        return Result == FCount ? -1 : Result;
    }

    public void SortAscendingItemB() => Sort(static (in Item1, in Item2) => Item1.B - Item2.B);

    public void SortDescendingItemB() => Sort(static (in Item1, in Item2) => Item2.B - Item1.B);

    // Engine.pas:5422
    public int RandomChanceItemB(int Bonus)
    {
        int Result;
        int Total;
        int Chance;
        int R;
        if (FCount != 0)
        {
            Total = 0;
            for (int I = FCount - 1; I >= 0; I--)
            {
                Chance = FList[I].B + Bonus;
                if (Chance > 0)
                {
                    Total += Chance;
                }
            }

            R = Random(Total);
            Result = 0;
            Total = 0;
            Chance = FList[0].B + Bonus;
            if (Chance > 0)
            {
                Total += Chance;
            }

            while (Total <= R)
            {
                Result += 1;
                Chance = FList[Result].B + Bonus;
                if (Chance > 0)
                {
                    Total += Chance;
                }
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }
}
