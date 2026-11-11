// Translated from ImageLib.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class FrameTableList : Persistent
{
    internal int[] FList;
    internal int FCount;
    internal int FCapacity;
    internal int FDefaultValue;

    // ImageLib.pas:9434
    protected internal int Get(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        return FList[Index];
    }

    // ImageLib.pas:9440
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

    // ImageLib.pas:9486
    // { Unlike TList.Put, an index past the end grows the list }
    protected internal void Put(int Index, int Item)
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

    // ImageLib.pas:9506
    protected internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > 134217727)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            FList = ReallocArray(FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // ImageLib.pas:9517
    protected internal void SetCount(int NewCount)
    {
        if (NewCount < 0 || NewCount > 134217727)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            FillBytes(MemoryMarshal.AsBytes(FList.AsSpan(FCount, NewCount - FCount)), FDefaultValue);
        }

        FCount = NewCount;
    }

    // ImageLib.pas:9360
    public override void Dispose()
    {
        Clear();
    }

    // ImageLib.pas:9365
    public override void Assign(Persistent Source)
    {
        if (Source is FrameTableList)
        {
            var frameTableList = (FrameTableList)Source;
            this.SetCapacity(frameTableList.FCount);
            this.FCount = frameTableList.FCount;
            MoveElements(frameTableList.FList, 0, this.FList, 0, this.FCount);
            this.FDefaultValue = frameTableList.FDefaultValue;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // ImageLib.pas:9379
    public int Add(int Item)
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

    // ImageLib.pas:9387
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // ImageLib.pas:9393
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            MoveElements(FList, Index + 1, FList, Index, FCount - Index);
        }
    }

    // ImageLib.pas:9412
    public void Exchange(int Index1, int Index2)
    {
        int Item;
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

    // ImageLib.pas:9423
    public FrameTableList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // ImageLib.pas:9429
    public int First() => Get(0);

    // ImageLib.pas:9450
    public int IndexOf(int Item)
    {
        int Result = 0;
        while (Result < FCount && FList[Result] != Item)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:9457
    public void Insert(int Index, int Item)
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
            MoveElements(FList, Index, FList, Index + 1, FCount - Index);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // ImageLib.pas:9467
    public int Last() => Get(FCount - 1);

    // ImageLib.pas:9472
    public void Move(int CurIndex, int NewIndex)
    {
        int Item;
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

    // ImageLib.pas:9493
    public int Remove(int Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // ImageLib.pas:9499
    public void Pack()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (this[I] == 0)
            {
                Delete(I);
            }
        }
    }

    // ImageLib.pas:9554
    public void Sort(FrameTableSortCompare Compare)
    {
        if (FList != null && Count > 0)
        {
            FrameTableQuickSort(FList, 0, Count - 1, Compare);
        }
    }

    // ImageLib.pas:9560
    public bool Compare(FrameTableList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

    // ImageLib.pas:9566
    public void LoadFromStream(Stream Stream)
    {
        int N = default;
        Clear();
        Stream.ReadExactly(Bytes.Of(ref FDefaultValue, 4));
        Stream.ReadExactly(Bytes.Of(ref N, 4));
        SetCapacity(N);
        Stream.ReadExactly(MemoryMarshal.AsBytes(FList.AsSpan(0, N)));
        FCount = N;
    }

    // ImageLib.pas:9578
    public void SaveToStream(Stream Stream)
    {
        Stream.Write(Bytes.Of(ref FDefaultValue, 4));
        Stream.Write(Bytes.Of(ref FCount, 4));
        Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)));
    }

    // ImageLib.pas:9586
    // { The items separated by commas }
    public string GetItemString()
    {
        string Result = "";
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            Result = Result + IntToStr(FList[I]) + ",";
        }

        if (!IsEmpty(Result))
        {
            Result = Copy(Result, 1, Result.Length - 1);
        }

        return Result;
    }

    // ImageLib.pas:9597
    // { Reads items separated by commas; other characters than digits are dropped, an empty item
    //   is 0 }
    public void SetItemString(string Value)
    {
        int I;
        int N;
        string S;
        I = Value.Length;
        for (; I >= 1; I -= 1)
        {
            if (!Set<byte>.WithBounds(0, 255).With(44).WithRange(48, 57).Contains(Cp1252.Byte(Value[I - 1])))
            {
                Value = RtlUnit.Delete(Value, I, 1);
            }
        }

        Clear();
        I = Pos(",", Value);
        while (!IsEmpty(Value))
        {
            if (I != 0)
            {
                S = Copy(Value, 1, I - 1);
                Value = RtlUnit.Delete(Value, 1, I);
            }
            else
            {
                S = Value;
                Value = "";
            }

            if (!IsEmpty(S))
            {
                N = StrToInt(S);
            }
            else
            {
                N = 0;
            }

            Add(N);
            I = Pos(",", Value);
        }
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }

    public int this[int Index] { get => Get(Index); set => Put(Index, value); }

    public int[] List { get => FList; }
    public int DefaultValue { get => FDefaultValue; set => FDefaultValue = value; }
    public string ItemString { get => GetItemString(); set => SetItemString(value); }

    // ImageLib.pas:9401
    public static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void FrameTableQuickSort(int[] SortList, int L, int R, FrameTableSortCompare SCompare)
    {
        int num = 0;
        int num2;
        do
        {
            num2 = L;
            int num3 = R;
            int item = SortList[L + R >>> 1];
            while (true)
            {
                if (SCompare(SortList[num2], item) < 0)
                {
                    num2++;
                    continue;
                }
                while (SCompare(SortList[num3], item) > 0)
                {
                    num3--;
                }
                if (num2 <= num3)
                {
                    num = SortList[num2];
                    SortList[num2] = SortList[num3];
                    SortList[num3] = num;
                    num2++;
                    num3--;
                }
                if (num2 > num3)
                {
                    break;
                }
            }
            if (L < num3)
            {
                FrameTableQuickSort(SortList, L, num3, SCompare);
            }
            L = num2;
        }
        while (num2 < R);
    }

    public delegate int FrameTableSortCompare(int Item1, int Item2);

}
