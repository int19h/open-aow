// Translated from Engine.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class DoubleItemList : Persistent
{
    internal DoubleItem[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal DoubleItem FDefault;

    // Engine.pas:5140
    internal DoubleItem Get(int Index)
    {
        DoubleItem Result;
        if (Index < 0 || Index >= FCount)
        {
            Result = FDefault;
        }
        else
        {
            Result = FList[Index];
        }

        return Result;
    }

    // Engine.pas:5208
    internal void Put(int Index, DoubleItem Item)
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

    // Engine.pas:5233
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

    // Engine.pas:5244
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
            FillLargeInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:5148
    protected internal virtual void Grow()
    {
        int Delta;
        if (FCapacity > 64)
        {
            Delta = FCapacity / 4;
        }
        else
        {
            if (FCapacity > 8)
            {
                Delta = 16;
            }
            else
            {
                Delta = 4;
            }
        }

        SetCapacity(FCapacity + Delta);
    }

    // Engine.pas:5056
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:5061
    public override void Assign(Persistent Source)
    {
        if (Source is DoubleItemList)
        {
            var doubleItemList = (DoubleItemList)Source;
            this.FCount = doubleItemList.FCount;
            this.SetCapacity(doubleItemList.FCount);
            Array.Copy(doubleItemList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = doubleItemList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:5075
    public int Add(in DoubleItem Item)
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

    // Engine.pas:5083
    public int AddAB(int A, int B)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result].A = A;
        FList[Result].B = B;
        FCount += 1;
        return Result;
    }

    // Engine.pas:5092
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:5098
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 8);
        }
    }

    // Engine.pas:5118
    public void Exchange(int Index1, int Index2)
    {
        DoubleItem Item;
        if (Index1 < 0 || Index1 >= FCount)
        {
            Error("List index out of bounds (%d)", Index1);
        }

        if (Index2 < 0 || Index2 >= FCount)
        {
            Error("List index out of bounds (%d)", Index2);
        }

        Item = FList[Index1];
        FList[Index1] = FList[Index2];
        FList[Index2] = Item;
    }

    // Engine.pas:5129
    public DoubleItemList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:5135
    public DoubleItem First() => Get(0);

    // Engine.pas:5158
    public int IndexOf(in DoubleItem Item)
    {
        int Result = 0;
        while (Result < FCount && (double)Unsafe.As<DoubleItem, long>(ref FList[Result]) != Unsafe.BitCast<DoubleItem, long>(Item))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:5165
    public int IndexOfItemA(int A)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].A != A)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:5172
    public int IndexOfItemB(int B)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].B != B)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:5179
    public void Insert(int Index, in DoubleItem Item)
    {
        if (Index < 0 || Index > FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (FCount == FCapacity)
        {
            Grow();
        }

        if (Index < FCount)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 8);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:5190
    public DoubleItem Last() => Get(FCount - 1);

    // Engine.pas:5195
    public void Move(int CurIndex, int NewIndex)
    {
        DoubleItem Item;
        if (CurIndex != NewIndex)
        {
            if (NewIndex < 0 || NewIndex >= FCount)
            {
                Error("List index out of bounds (%d)", NewIndex);
            }

            Item = Get(CurIndex);
            Delete(CurIndex);
            Insert(NewIndex, Item);
        }
    }

    // Engine.pas:5215
    public int Remove(in DoubleItem Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:5221
    public void Pack()
    {
        DoubleItem Item = default;
        for (int I = FCount - 1; I >= 0; I--)
        {
            Item = this[I];
            if ((double)Unsafe.As<DoubleItem, long>(ref Item) == Unsafe.As<DoubleItem, long>(ref FDefault))
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5281
    public void Sort(DoubleItemSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortDoubleItem(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:5307
    public void SortAscendingItemA()
    {
        Sort(CompareDoubleItemAAscending);
    }

    // Engine.pas:5312
    public void SortAscendingItemB()
    {
        Sort(CompareDoubleItemBAscending);
    }

    // Engine.pas:5317
    public void SortDescendingItemA()
    {
        Sort(CompareDoubleItemADescending);
    }

    // Engine.pas:5322
    public void SortDescendingItemB()
    {
        Sort(CompareDoubleItemBDescending);
    }

    // Engine.pas:5327
    public bool Compare(DoubleItemList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 8);

    // Engine.pas:5334
    public void LoadFromStream(Stream Stream)
    {
        DoubleItem Item = default;
        int Run = default;
        int Count = default;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref FDefault, 8));
        Stream.ReadBytes(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.ReadBytes(Bytes.Of(ref Item, 8));
            if ((double)Unsafe.As<DoubleItem, long>(ref Item) == Unsafe.As<DoubleItem, long>(ref FDefault))
            {
                Stream.ReadBytes(Bytes.Of(ref Run, 4));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:5356
    public void SaveToStream(Stream Stream)
    {
        int I;
        int Run = default;
        int DefaultRun()
        {
            int J;
            int Result = 0;
            J = I;
            while (J < FCount && (double)Unsafe.As<DoubleItem, long>(ref FList[J]) == Unsafe.As<DoubleItem, long>(ref FDefault))
            {
                Result += 1;
                J += 1;
            }

            return Result;
        }

        I = 0;
        Stream.Write(Bytes.Of(ref FDefault, 8));
        Stream.Write(Bytes.Of(ref FCount, 4));
        while (FCount != I)
        {
            Run = DefaultRun();
            if (Run > 0)
            {
                Stream.Write(Bytes.Of(ref FDefault, 8));
                Stream.Write(Bytes.Of(ref Run, 4));
                I += Run;
            }
            else
            {
                Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:5394
    public int RandomChanceItemA(int Bonus)
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
                Chance = FList[I].A + Bonus;
                if (Chance < 0)
                {
                    Chance = 0;
                }

                Total += Chance;
            }

            R = Random(Total);
            Total = 0;
            Result = 0;
            while (Total < R)
            {
                Chance = FList[Result].A + Bonus;
                if (Chance < 0)
                {
                    Chance = 0;
                }

                Total += Chance;
                Result += 1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

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

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public DoubleItem Default { get => FDefault; set => FDefault = value; }

    public DoubleItem this[int Index] { get => Get(Index); set => Put(Index, value); }

    public DoubleItem[] List { get => FList; }

    // Engine.pas:5107
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortDoubleItem(Span<DoubleItem> SortList, int L, int R, DoubleItemSortCompare SCompare)
    {
        DoubleItem tDoubleItem = default(DoubleItem);
        int num;
        do
        {
            num = L;
            int num2 = R;
            DoubleItem Item = SortList[L + R >>> 1];
            while (true)
            {
                if (SCompare(in SortList[num], in Item) < 0)
                {
                    num++;
                    continue;
                }
                while (SCompare(in SortList[num2], in Item) > 0)
                {
                    num2--;
                }
                if (num <= num2)
                {
                    tDoubleItem = SortList[num];
                    SortList[num] = SortList[num2];
                    SortList[num2] = tDoubleItem;
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
                QuickSortDoubleItem(SortList, L, num2, SCompare);
            }
            L = num;
        }
        while (num < R);
    }

    private static int CompareDoubleItemAAscending(in DoubleItem Item1, in DoubleItem Item2)
    {
        return Item1.A - Item2.A;
    }

    private static int CompareDoubleItemBAscending(in DoubleItem Item1, in DoubleItem Item2)
    {
        return Item1.B - Item2.B;
    }

    private static int CompareDoubleItemADescending(in DoubleItem Item1, in DoubleItem Item2)
    {
        return Item2.A - Item1.A;
    }

    private static int CompareDoubleItemBDescending(in DoubleItem Item1, in DoubleItem Item2)
    {
        return Item2.B - Item1.B;
    }

    private static void FillLargeInteger(Span<DoubleItem> buffer, int count, DoubleItem value)
    {
        EngineServices.Fill(buffer, count, value);
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct DoubleItem
    {
        [FieldOffset(0)]
        public int A;
        [FieldOffset(4)]
        public int B;
    }

    public delegate int DoubleItemSortCompare(in DoubleItem Item1, in DoubleItem Item2);

}
