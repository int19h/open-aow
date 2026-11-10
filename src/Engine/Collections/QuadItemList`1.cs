// Translated from Engine.pas.

namespace Aow.Engine.Collections;

public class QuadItemList<TA> : RecordList<QuadItem<TA>> where TA : class
{
    public int AddABCD(TA A, int B, int C, int D) => Add(new QuadItem<TA> { A = A, B = B, C = C, D = D });

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

    public int IndexOfItemC(int C)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].C != C)
        {
            Result += 1;
        }

        return Result == FCount ? -1 : Result;
    }

    public int IndexOfItemD(int D)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].D != D)
        {
            Result += 1;
        }

        return Result == FCount ? -1 : Result;
    }

    public void SortAscendingItemB() => Sort(static (in Item1, in Item2) => Item1.B - Item2.B);

    public void SortAscendingItemC() => Sort(static (in Item1, in Item2) => Item1.C - Item2.C);

    public void SortAscendingItemD() => Sort(static (in Item1, in Item2) => Item1.D - Item2.D);

    public void SortDescendingItemB() => Sort(static (in Item1, in Item2) => Item2.B - Item1.B);

    public void SortDescendingItemC() => Sort(static (in Item1, in Item2) => Item2.C - Item1.C);

    public void SortDescendingItemD() => Sort(static (in Item1, in Item2) => Item2.D - Item1.D);
}
