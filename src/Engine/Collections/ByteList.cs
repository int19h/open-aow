// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class ByteList : Persistent
{
    internal byte[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal byte FDefault;

    // Engine.pas:3678
    internal byte Get(int Index)
    {
        byte Result;
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

    // Engine.pas:3732
    internal void Put(int Index, byte Item)
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

    // Engine.pas:3752
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

    // Engine.pas:3763
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
            FillBytes(FList.AsSpan(FCount, NewCount - FCount), FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:3883
    internal string GetItemString()
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

    // Engine.pas:3894
    internal void SetItemString(string S)
    {
        string Item;
        int P;
        int V;
        Clear();
        P = Pos(",", S);
        while (!IsEmpty(S))
        {
            if (P != 0)
            {
                Item = Copy(S, 1, P - 1);
                S = RtlUnit.Delete(S, 1, P);
            }
            else
            {
                Item = S;
                S = "";
            }

            if (!IsEmpty(Item))
            {
                V = StrToInt(Item);
            }
            else
            {
                V = 0;
            }

            if (V > 255)
            {
                V = 0;
            }

            Add((byte)V);
            P = Pos(",", S);
        }
    }

    // Engine.pas:3686
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

    // Engine.pas:3603
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:3608
    public override void Assign(Persistent Source)
    {
        if (Source is ByteList)
        {
            var byteList = (ByteList)Source;
            this.FCount = byteList.FCount;
            this.SetCapacity(byteList.FCount);
            Array.Copy(byteList.FList, 0, this.FList, 0, this.FCount * 1);
            this.FDefault = byteList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:3622
    public int Add(byte Item)
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

    // Engine.pas:3630
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3636
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

    // Engine.pas:3656
    public void Exchange(int Index1, int Index2)
    {
        byte Item;
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

    // Engine.pas:3667
    public ByteList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:3673
    public byte First() => Get(0);

    // Engine.pas:3696
    public int IndexOf(byte Item)
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

    // Engine.pas:3703
    public void Insert(int Index, byte Item)
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

    // Engine.pas:3714
    public byte Last() => Get(FCount - 1);

    // Engine.pas:3719
    public void Move(int CurIndex, int NewIndex)
    {
        byte Item;
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

    // Engine.pas:3739
    public int Remove(byte Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:3745
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

    // Engine.pas:3800
    public void Sort(ByteSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortByte(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:3806
    public bool Compare(ByteList List) => List.FCount == FCount && CompareMem(FList.AsSpan(0, FCount * 1), List.FList.AsSpan(0, FCount * 1), FCount * 1);

    // Engine.pas:3813
    public void LoadFromStream(Stream Stream)
    {
        byte Item = default;
        byte Run = default;
        int Count = default;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref FDefault, 1));
        Stream.ReadBytes(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.ReadBytes(Bytes.Of(ref Item, 1));
            if (Item == FDefault)
            {
                Stream.ReadBytes(Bytes.Of(ref Run, 1));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:3835
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
                Stream.Write(FList.AsSpan(I, 1));
                I += 1;
            }
        }
    }

    // Engine.pas:3920
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
    public byte Default { get => FDefault; set => FDefault = value; }

    public byte this[int Index] { get => Get(Index); set => Put(Index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public byte[] List { get => FList; }

    // Engine.pas:3645
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortByte(Span<byte> SortList, int L, int R, ByteSortCompare SCompare)
    {
        byte b = 0;
        int num;
        do
        {
            num = L;
            int num2 = R;
            byte item = SortList[L + R >>> 1];
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
                QuickSortByte(SortList, L, num2, SCompare);
            }
            L = num;
        }
        while (num < R);
    }

    public delegate int ByteSortCompare(byte Item1, byte Item2);

}
