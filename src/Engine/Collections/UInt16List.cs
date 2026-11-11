// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class UInt16List : Persistent
{
    internal ushort[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal ushort FDefault;

    // Engine.pas:4336
    internal ushort Get(int Index)
    {
        ushort Result;
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

    // Engine.pas:4390
    internal void Put(int Index, ushort Item)
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

    // Engine.pas:4410
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

    // Engine.pas:4421
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
            FillWord(FList.AsSpan(FCount), NewCount - FCount, FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:4344
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

    // Engine.pas:4261
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:4266
    public override void Assign(Persistent Source)
    {
        if (Source is UInt16List)
        {
            var wordList = (UInt16List)Source;
            this.FCount = wordList.FCount;
            this.SetCapacity(wordList.FCount);
            Array.Copy(wordList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = wordList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:4280
    public int Add(ushort Item)
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

    // Engine.pas:4288
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:4294
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            Array.Copy(FList, Index + 1, FList, Index, FCount - Index);
        }
    }

    // Engine.pas:4314
    public void Exchange(int Index1, int Index2)
    {
        ushort Item;
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

    // Engine.pas:4325
    public UInt16List Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4331
    public ushort First() => Get(0);

    // Engine.pas:4354
    public int IndexOf(ushort Item)
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

    // Engine.pas:4361
    public void Insert(int Index, ushort Item)
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
            Array.Copy(FList, Index, FList, Index + 1, FCount - Index);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:4372
    public ushort Last() => Get(FCount - 1);

    // Engine.pas:4377
    public void Move(int CurIndex, int NewIndex)
    {
        ushort Item;
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

    // Engine.pas:4397
    public int Remove(ushort Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:4403
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

    // Engine.pas:4458
    public void Sort(WordSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortWord(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:4464
    public bool Compare(UInt16List List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 2);

    // Engine.pas:4471
    public void LoadFromStream(Stream Stream)
    {
        ushort Item = default;
        ushort Run = default;
        int Count = default;
        Clear();
        Stream.ReadExactly(Bytes.Of(ref FDefault, 2));
        Stream.ReadExactly(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.ReadExactly(Bytes.Of(ref Item, 2));
            if (Item == FDefault)
            {
                Stream.ReadExactly(Bytes.Of(ref Run, 2));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:4493
    public void SaveToStream(Stream Stream)
    {
        int I;
        int Run;
        ushort B = default;
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
        Stream.Write(Bytes.Of(ref FDefault, 2));
        Stream.Write(Bytes.Of(ref FCount, 4));
        while (FCount != I)
        {
            Run = DefaultRun();
            if (Run > 0)
            {
                while (Run > 65535)
                {
                    B = 65535;
                    Stream.Write(Bytes.Of(ref FDefault, 2));
                    Stream.Write(Bytes.Of(ref B, 2));
                    // { sic: the original subtracts $FF here but advances I by $FFFF }
                    Run -= 255;
                    I += 65535;
                }

                B = (ushort)Run;
                Stream.Write(Bytes.Of(ref FDefault, 2));
                Stream.Write(Bytes.Of(ref B, 2));
                I += Run;
            }
            else
            {
                Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:4541
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
    public ushort Default { get => FDefault; set => FDefault = value; }

    public ushort this[int Index] { get => Get(Index); set => Put(Index, value); }

    public ushort[] List { get => FList; }

    // Engine.pas:4303
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortWord(Span<ushort> SortList, int L, int R, WordSortCompare SCompare)
    {
        ushort num = 0;
        int num2;
        do
        {
            num2 = L;
            int num3 = R;
            ushort item = SortList[L + R >>> 1];
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
                QuickSortWord(SortList, L, num3, SCompare);
            }
            L = num2;
        }
        while (num2 < R);
    }

    internal static void FillWord(Span<ushort> buffer, int count, ushort value)
    {
        EngineServices.Fill(buffer, count, value);
    }

    public delegate int WordSortCompare(ushort Item1, ushort Item2);

}
