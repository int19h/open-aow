// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Engine.Collections;

public class RecordList<TRecord> : Persistent where TRecord : struct
{
    internal TRecord[] FList = [];
    internal int FCount;
    internal int FCapacity;

    internal TRecord Get(int Index) => Index < 0 || Index >= FCount ? default : FList[Index];

    internal void Put(int Index, TRecord Item)
    {
        if (Index < 0)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        FList[Index] = Item;
    }

    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    internal void SetCount(int NewCount)
    {
        if (NewCount < 0 || NewCount > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            Array.Clear(FList, FCount, NewCount - FCount);
        }
        else
        {
            Array.Clear(FList, NewCount, FCount - NewCount);
        }

        FCount = NewCount;
    }

    protected internal virtual void Grow()
    {
        int Delta;
        if (FCapacity > 64)
        {
            Delta = FCapacity / 4;
        }
        else if (FCapacity > 8)
        {
            Delta = 16;
        }
        else
        {
            Delta = 4;
        }

        SetCapacity(FCapacity + Delta);
    }

    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    public override void Assign(Persistent Source)
    {
        if (Source is RecordList<TRecord> list)
        {
            Clear();
            SetCapacity(list.FCount);
            Array.Copy(list.FList, FList, list.FCount);
            FCount = list.FCount;
        }
        else
        {
            base.Assign(Source);
        }
    }

    public int Add(in TRecord Item)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result] = Item;
        FCount += 1;
        return Result;
    }

    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        Array.Copy(FList, Index + 1, FList, Index, FCount - Index);
        FList[FCount] = default;
    }

    public void Exchange(int Index1, int Index2)
    {
        if (Index1 < 0 || Index1 >= FCount)
        {
            Error("List index out of bounds (%d)", Index1);
        }

        if (Index2 < 0 || Index2 >= FCount)
        {
            Error("List index out of bounds (%d)", Index2);
        }

        (FList[Index1], FList[Index2]) = (FList[Index2], FList[Index1]);
    }

    public TRecord First() => Get(0);

    public void Insert(int Index, in TRecord Item)
    {
        if (Index < 0 || Index > FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (FCount == FCapacity)
        {
            Grow();
        }

        Array.Copy(FList, Index, FList, Index + 1, FCount - Index);
        FList[Index] = Item;
        FCount += 1;
    }

    public TRecord Last() => Get(FCount - 1);

    public void Sort(RecordSortCompare<TRecord> Compare)
    {
        if (FCount > 0)
        {
            QuickSort(FList, 0, FCount - 1, Compare);
        }
    }

    // Engine.pas:5254, the quicksort of QuickSortDoubleItem and QuickSortQuadItem.
    private static void QuickSort(Span<TRecord> SortList, int L, int R, RecordSortCompare<TRecord> SCompare)
    {
        int I;
        int J;
        TRecord P;
        do
        {
            I = L;
            J = R;
            P = SortList[(int)(((uint)(L + R)) >> 1)];
            do
            {
                while (SCompare(SortList[I], P) < 0)
                {
                    I += 1;
                }

                while (SCompare(SortList[J], P) > 0)
                {
                    J -= 1;
                }

                if (I <= J)
                {
                    (SortList[I], SortList[J]) = (SortList[J], SortList[I]);
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSort(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }

    public TRecord this[int Index] { get => Get(Index); set => Put(Index, value); }

    public TRecord[] List { get => FList; }

    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    // The record lists above with an object column. Engine.pas stored Integer(Object) in column A of a
    // TDoubleItemList or TQuadItemList and cast it back on read. These lists keep the reference itself,
    // so no id table holds the object and the owner of the list alone decides its lifetime. Every
    // column that held a number stays an int. Sort runs the quicksort of EngineUnit.QuickSortDoubleItem,
    // so a list sorted on its int columns keeps the original order.
    public delegate int RecordSortCompare<TItem>(in TItem Item1, in TItem Item2);
}
