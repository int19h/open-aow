// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class ShortIntList : Persistent
{
    internal sbyte[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal sbyte FDefault;

    // Engine.pas:4025
    internal sbyte Get(int Index)
    {
        sbyte Result;
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

    // Engine.pas:4079
    internal void Put(int Index, sbyte Item)
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

    // Engine.pas:4099
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FList, NewCapacity * 1);
            FCapacity = NewCapacity;
        }
    }

    // Engine.pas:4110
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
            FillBytes(MemoryMarshal.AsBytes(FList.AsSpan(FCount, NewCount - FCount)), FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:4033
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

    // Engine.pas:3950
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:3955
    public override void Assign(Persistent Source)
    {
        if (Source is ShortIntList)
        {
            var shortIntList = (ShortIntList)Source;
            this.FCount = shortIntList.FCount;
            this.SetCapacity(shortIntList.FCount);
            Array.Copy(shortIntList.FList, 0, this.FList, 0, this.FCount * 1);
            this.FDefault = shortIntList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:3969
    public int Add(sbyte Item)
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

    // Engine.pas:3977
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3983
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            Array.Copy(FList, Index + 1, FList, Index, (FCount - Index) * 1);
        }
    }

    // Engine.pas:4003
    public void Exchange(int Index1, int Index2)
    {
        sbyte Item;
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

    // Engine.pas:4014
    public ShortIntList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4020
    public sbyte First() => Get(0);

    // Engine.pas:4043
    public int IndexOf(sbyte Item)
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

    // Engine.pas:4050
    public void Insert(int Index, sbyte Item)
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
            Array.Copy(FList, Index, FList, Index + 1, (FCount - Index) * 1);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:4061
    public sbyte Last() => Get(FCount - 1);

    // Engine.pas:4066
    public void Move(int CurIndex, int NewIndex)
    {
        sbyte Item;
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

    // Engine.pas:4086
    public int Remove(sbyte Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:4092
    public void Pack()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (this[I] == FDefault)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:4147
    public void Sort(ShortIntSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortShortInt(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:4153
    public bool Compare(ShortIntList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount * 1)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount * 1)), FCount * 1);

    // Engine.pas:4160
    public void LoadFromStream(Stream Stream)
    {
        byte Run = default;
        sbyte Item = default;
        int Count = default;
        Clear();
        Stream.ReadExactly(Bytes.Of(ref FDefault, 1));
        Stream.ReadExactly(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.ReadExactly(Bytes.Of(ref Item, 1));
            if (Item == FDefault)
            {
                Stream.ReadExactly(Bytes.Of(ref Run, 1));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:4183
    public void SaveToStream(Stream Stream)
    {
        int I;
        int Run;
        byte B = default;
        int DefaultRun()
        {
            int J;
            int Result = 0;
            J = I;
            while (J < FCount && FList[J] == FDefault)
            {
                Result += 1;
                J += 1;
            }

            return Result;
        }

        I = 0;
        Stream.Write(Bytes.Of(ref FDefault, 1));
        Stream.Write(Bytes.Of(ref FCount, 4));
        while (FCount != I)
        {
            Run = DefaultRun();
            if (Run > 0)
            {
                while (Run > 255)
                {
                    B = 255;
                    Stream.Write(Bytes.Of(ref FDefault, 1));
                    Stream.Write(Bytes.Of(ref B, 1));
                    Run -= 255;
                    I += 255;
                }

                B = (byte)Run;
                Stream.Write(Bytes.Of(ref FDefault, 1));
                Stream.Write(Bytes.Of(ref B, 1));
                I += Run;
            }
            else
            {
                Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:4231
    public int RandomChanceItem(int Bonus)
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
                Chance = FList[I] + Bonus;
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
                Chance = FList[Result] + Bonus;
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

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public sbyte Default { get => FDefault; set => FDefault = value; }

    public sbyte this[int Index] { get => Get(Index); set => Put(Index, value); }

    public sbyte[] List { get => FList; }

    // Engine.pas:3992
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortShortInt(Span<sbyte> SortList, int L, int R, ShortIntSortCompare SCompare)
    {
        sbyte b = 0;
        int num;
        do
        {
            num = L;
            int num2 = R;
            sbyte item = SortList[L + R >>> 1];
            while (true)
            {
                if (SCompare(SortList[num], item) < 0)
                {
                    num++;
                    continue;
                }
                while (SCompare(SortList[num2], item) > 0)
                {
                    num2--;
                }
                if (num <= num2)
                {
                    b = SortList[num];
                    SortList[num] = SortList[num2];
                    SortList[num2] = b;
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
                QuickSortShortInt(SortList, L, num2, SCompare);
            }
            L = num;
        }
        while (num < R);
    }

    public delegate int ShortIntSortCompare(sbyte Item1, sbyte Item2);

}
