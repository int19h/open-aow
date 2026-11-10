// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class QuadItemList : Persistent
{
    internal QuadInteger[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal QuadInteger FDefault;

    // Engine.pas:5538
    internal QuadInteger Get(int Index)
    {
        QuadInteger Result;
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

    // Engine.pas:5621
    internal void Put(int Index, QuadInteger Item)
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

    // Engine.pas:5643
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

    // Engine.pas:5654
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
            FillQuadInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:5546
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

    // Engine.pas:5452
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:5457
    public override void Assign(Persistent Source)
    {
        if (Source is QuadItemList)
        {
            var quadItemList = (QuadItemList)Source;
            this.FCount = quadItemList.FCount;
            this.SetCapacity(quadItemList.FCount);
            Array.Copy(quadItemList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = quadItemList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:5471
    public int Add(in QuadInteger Item)
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

    // Engine.pas:5479
    public int AddABCD(int A, int B, int C, int D)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result].A = A;
        FList[Result].B = B;
        FList[Result].C = C;
        FList[Result].D = D;
        FCount += 1;
        return Result;
    }

    // Engine.pas:5490
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:5496
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 16);
        }
    }

    // Engine.pas:5516
    public void Exchange(int Index1, int Index2)
    {
        QuadInteger Item;
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

    // Engine.pas:5527
    public QuadItemList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:5533
    public QuadInteger First() => Get(0);

    // Engine.pas:5556
    public int IndexOf(in QuadInteger Item)
    {
        int Result = 0;
        while (Result < FCount && (FList[Result].A != Item.A || FList[Result].B != Item.B || FList[Result].C != Item.C || FList[Result].D != Item.D))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:5564
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

    // Engine.pas:5571
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

    // Engine.pas:5578
    public int IndexOfItemC(int C)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].C != C)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:5585
    public int IndexOfItemD(int D)
    {
        int Result = 0;
        while (Result < FCount && FList[Result].D != D)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:5592
    public void Insert(int Index, in QuadInteger Item)
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
            RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 16);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:5603
    public QuadInteger Last() => Get(FCount - 1);

    // Engine.pas:5608
    public void Move(int CurIndex, int NewIndex)
    {
        QuadInteger Item;
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

    // Engine.pas:5628
    public int Remove(in QuadInteger Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:5634
    public void Pack()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (FList[I].A == FDefault.A && FList[I].B == FDefault.B && FList[I].C == FDefault.C && FList[I].D == FDefault.D)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5691
    public void Sort(QuadItemSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortQuadItem(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:5737
    public void SortAscendingItemA()
    {
        Sort(CompareQuadItemAAscending);
    }

    // Engine.pas:5742
    public void SortAscendingItemB()
    {
        Sort(CompareQuadItemBAscending);
    }

    // Engine.pas:5747
    public void SortAscendingItemC()
    {
        Sort(CompareQuadItemCAscending);
    }

    // Engine.pas:5752
    public void SortAscendingItemD()
    {
        Sort(CompareQuadItemDAscending);
    }

    // Engine.pas:5757
    public void SortDescendingItemA()
    {
        Sort(CompareQuadItemADescending);
    }

    // Engine.pas:5762
    public void SortDescendingItemB()
    {
        Sort(CompareQuadItemBDescending);
    }

    // Engine.pas:5767
    public void SortDescendingItemC()
    {
        Sort(CompareQuadItemCDescending);
    }

    // Engine.pas:5772
    public void SortDescendingItemD()
    {
        Sort(CompareQuadItemDDescending);
    }

    // Engine.pas:5777
    public bool Compare(QuadItemList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 16);

    // Engine.pas:5784
    public void LoadFromStream(Stream Stream)
    {
        int Run = default;
        int Count = default;
        QuadInteger Item = default;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref FDefault, 16));
        Stream.ReadBytes(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.ReadBytes(Bytes.Of(ref Item, 16));
            if (Item.A == FDefault.A && Item.B == FDefault.B && Item.C == FDefault.C && Item.D == FDefault.D)
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

    // Engine.pas:5807
    public void SaveToStream(Stream Stream)
    {
        int I;
        int Run = default;
        int DefaultRun()
        {
            int J;
            int Result = 0;
            J = I;
            while (J < FCount && (FList[J].A == FDefault.A && FList[J].B == FDefault.B && FList[J].C == FDefault.C && FList[J].D == FDefault.D))
            {
                Result += 1;
                J += 1;
            }

            return Result;
        }

        I = 0;
        Stream.Write(Bytes.Of(ref FDefault, 16));
        Stream.Write(Bytes.Of(ref FCount, 4));
        while (FCount != I)
        {
            Run = DefaultRun();
            if (Run > 0)
            {
                Stream.Write(Bytes.Of(ref FDefault, 16));
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

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public QuadInteger Default { get => FDefault; set => FDefault = value; }

    public QuadInteger this[int Index] { get => Get(Index); set => Put(Index, value); }

    public QuadInteger[] List { get => FList; }

    // Engine.pas:5505
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortQuadItem(Span<QuadInteger> SortList, int L, int R, QuadItemSortCompare SCompare)
    {
        QuadInteger tQuadInteger = default(QuadInteger);
        int num;
        do
        {
            num = L;
            int num2 = R;
            QuadInteger Item = SortList[L + R >>> 1];
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
                    tQuadInteger = SortList[num];
                    SortList[num] = SortList[num2];
                    SortList[num2] = tQuadInteger;
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
                QuickSortQuadItem(SortList, L, num2, SCompare);
            }
            L = num;
        }
        while (num < R);
    }

    private static int CompareQuadItemAAscending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item1.A - Item2.A;
    }

    private static int CompareQuadItemBAscending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item1.B - Item2.B;
    }

    private static int CompareQuadItemCAscending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item1.C - Item2.C;
    }

    private static int CompareQuadItemDAscending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item1.D - Item2.D;
    }

    private static int CompareQuadItemADescending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item2.A - Item1.A;
    }

    private static int CompareQuadItemBDescending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item2.B - Item1.B;
    }

    private static int CompareQuadItemCDescending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item2.C - Item1.C;
    }

    private static int CompareQuadItemDDescending(in QuadInteger Item1, in QuadInteger Item2)
    {
        return Item2.D - Item1.D;
    }

    internal static void FillQuadInteger(Span<QuadInteger> buffer, int count, in QuadInteger value)
    {
        QuadInteger value2 = value;
        EngineServices.Fill(buffer, count, value2);
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct QuadInteger
    {
        [FieldOffset(0)]
        public int A;
        [FieldOffset(4)]
        public int B;
        [FieldOffset(8)]
        public int C;
        [FieldOffset(12)]
        public int D;
    }

    public delegate int QuadItemSortCompare(in QuadInteger Item1, in QuadInteger Item2);

}
