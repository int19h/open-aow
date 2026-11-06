// Translated from Engine.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.FMemStr;
using static Aow.Units.ImageLib.ImageLibUnit;

namespace Aow.Units.Engine;

public enum TEControlStyleElement : byte
{
    csSaveLoad = 0,
    csFixedChild = 1,
    csVerifyFixedChildren = 2,
    csArrayMode = 3
}

public enum TEditModeElement : byte
{
    emEdit = 0,
    emCut = 1,
    emCopy = 2,
    emPaste = 3,
    emDelete = 4
}

public enum TEngineUserElement : byte
{
    euProgrammer = 0,
    euDesigner = 1,
    euUser = 2
}

public enum TEngineModeElement : byte
{
    emVerifyEObjects = 0
}

public enum TStorageModeElement : byte
{
    smRead = 0,
    smWrite = 1,
    smTextOnly = 2,
    smPreview = 3,
    smTranslate = 4,
    smSetupCompare = 5,
    smCompare = 6
}

public delegate int TIntegerSortCompare(int Item1, int Item2);
public partial class TIntegerList : TPersistent
{
    internal int[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal int FDefault;
    // Engine.pas:3343
    public int Get(int Index)
    {
        int Result;
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

    // Engine.pas:3397
    // AoWE
    internal void Put(int Index, int Item)
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

    // Engine.pas:3417
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // Engine.pas:3428
    internal void SetCount(int NewCount)
    {
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            EngineUnit.FillInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:3537
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

    // Engine.pas:3548
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

            Add(V);
            P = Pos(",", S);
        }
    }

    // Engine.pas:3351
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

    // Engine.pas:3268
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:3273
    public override void Assign(TPersistent Source)
    {
        if (Source is TIntegerList)
        {
            var integerList = (TIntegerList)Source;
            this.FCount = integerList.FCount;
            this.SetCapacity(integerList.FCount);
            Array.Copy(integerList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = integerList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:3287
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

    // Engine.pas:3295
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3301
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

    // Engine.pas:3321
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

    // Engine.pas:3332
    public TIntegerList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:3338
    public int First() => Get(0);

    // Engine.pas:3361
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

    // Engine.pas:3368
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
            Array.Copy(FList, Index, FList, Index + 1, FCount - Index);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:3379
    public int Last() => Get(FCount - 1);

    // Engine.pas:3384
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

    // Engine.pas:3404
    public int Remove(int Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:3410
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

    // Engine.pas:3465
    public void Sort(TIntegerSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            EngineUnit.QuickSortInteger(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:3471
    public bool Compare(TIntegerList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

    // Engine.pas:3478
    public void LoadFromStream(TStream Stream)
    {
        int Count = default;
        int Item = default;
        int Run = default;
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 4));
        Stream.Read(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (Count != FCount)
        {
            Stream.Read(Bytes.Of(ref Item, 4));
            if (Item == FDefault)
            {
                Stream.Read(Bytes.Of(ref Run, 4));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:3499
    public void SaveToStream(TStream Stream)
    {
        int I;
        int Run = default;
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
        Stream.Write(Bytes.Of(ref FDefault, 4));
        Stream.Write(Bytes.Of(ref FCount, 4));
        while (FCount != I)
        {
            Run = DefaultRun();
            if (Run > 0)
            {
                Stream.Write(Bytes.Of(ref FDefault, 4));
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

    // Engine.pas:3573
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
    public int Default { get => FDefault; set => FDefault = value; }

    public int this[int Index] { get => Get(Index); set => Put(Index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public int[] List { get => FList; }

    // Engine.pas:3310
    protected internal static void Error(string Msg, int Data)
    {
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

public delegate int TByteSortCompare(byte Item1, byte Item2);
public partial class TByteList : TPersistent
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
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
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
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
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
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:3608
    public override void Assign(TPersistent Source)
    {
        if (Source is TByteList)
        {
            var byteList = (TByteList)Source;
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
    public TByteList Expand()
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
    public void Sort(TByteSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            EngineUnit.QuickSortByte(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:3806
    public bool Compare(TByteList List) => List.FCount == FCount && CompareMem(FList.AsSpan(0, FCount * 1), List.FList.AsSpan(0, FCount * 1), FCount * 1);

    // Engine.pas:3813
    public void LoadFromStream(TStream Stream)
    {
        byte Item = default;
        byte Run = default;
        int Count = default;
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 1));
        Stream.Read(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.Read(Bytes.Of(ref Item, 1));
            if (Item == FDefault)
            {
                Stream.Read(Bytes.Of(ref Run, 1));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:3835
    public void SaveToStream(TStream Stream)
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
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

public delegate int TShortIntSortCompare(sbyte Item1, sbyte Item2);
public partial class TShortIntList : TPersistent
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
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
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
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
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
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:3955
    public override void Assign(TPersistent Source)
    {
        if (Source is TShortIntList)
        {
            var shortIntList = (TShortIntList)Source;
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
    public TShortIntList Expand()
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
    public void Sort(TShortIntSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            EngineUnit.QuickSortShortInt(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:4153
    public bool Compare(TShortIntList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount * 1)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount * 1)), FCount * 1);

    // Engine.pas:4160
    public void LoadFromStream(TStream Stream)
    {
        byte Run = default;
        sbyte Item = default;
        int Count = default;
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 1));
        Stream.Read(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.Read(Bytes.Of(ref Item, 1));
            if (Item == FDefault)
            {
                Stream.Read(Bytes.Of(ref Run, 1));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:4183
    public void SaveToStream(TStream Stream)
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
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

public delegate int TWordSortCompare(ushort Item1, ushort Item2);
public partial class TWordList : TPersistent
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
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
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
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            EngineUnit.FillWord(FList.AsSpan(FCount), NewCount - FCount, FDefault);
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
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:4266
    public override void Assign(TPersistent Source)
    {
        if (Source is TWordList)
        {
            var wordList = (TWordList)Source;
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
    public TWordList Expand()
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
    public void Sort(TWordSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            EngineUnit.QuickSortWord(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:4464
    public bool Compare(TWordList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 2);

    // Engine.pas:4471
    public void LoadFromStream(TStream Stream)
    {
        ushort Item = default;
        ushort Run = default;
        int Count = default;
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 2));
        Stream.Read(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.Read(Bytes.Of(ref Item, 2));
            if (Item == FDefault)
            {
                Stream.Read(Bytes.Of(ref Run, 2));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:4493
    public void SaveToStream(TStream Stream)
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
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

public partial class TBitList : TPersistent
{
    internal Memory<byte> FList = Memory<byte>.Empty;
    internal int FCount;
    internal int FCapacity;
    internal bool FDefault;
    // Engine.pas:4743
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        NewCapacity = (NewCapacity + 31) & ~31;
        if (NewCapacity != FCapacity)
        {
            if (NewCapacity == 0) FList = Memory<byte>.Empty;
            else
            {
                var resized = new byte[(int)((uint)NewCapacity >> 3)];
                FList.Span[..Math.Min(FList.Length, resized.Length)].CopyTo(resized);
                FList = resized;
            }
            FCapacity = NewCapacity;
            for (int I = FCount; I <= NewCapacity - 1; I++)
            {
                Put(I, FDefault);
            }
        }
    }

    // Engine.pas:4758
    internal void SetCount(int NewCount)
    {
        int OldCount;
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            OldCount = FCount;
            FCount = NewCount;
            for (int I = OldCount, last = FCount - 1; I <= last; I++)
            {
                Put(I, FDefault);
            }
        }
        else
        {
            FCount = NewCount;
        }
    }

    // Engine.pas:4798
    internal string GetItemString()
    {
        string Result = "";
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (Get(I) != FDefault)
            {
                Result = Result + IntToStr(I) + ",";
            }
        }

        if (!IsEmpty(Result))
        {
            Result = Copy(Result, 1, Result.Length - 1);
        }

        return Result;
    }

    // Engine.pas:4810
    internal void SetItemString(string S)
    {
        string Item;
        int P;
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
                Put(StrToInt(Item), !FDefault);
            }

            P = Pos(",", S);
        }
    }

    // Engine.pas:4834
    internal int GetBitsSet()
    {
        int Result = 0;
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (Get(I))
            {
                Result += 1;
            }
        }

        return Result;
    }

    // Engine.pas:4843
    internal int GetBitsCleared()
    {
        int Result = 0;
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (!Get(I))
            {
                Result += 1;
            }
        }

        return Result;
    }

    // Engine.pas:4683
    // {Item}
    // {Item}
    protected internal virtual void Grow()
    {
        SetCapacity(FCapacity + 32);
    }

    // Engine.pas:4571
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:4576
    public override void Assign(TPersistent Source)
    {
        if (Source is TBitList)
        {
            var bitList = (TBitList)Source;
            this.FCount = bitList.FCount;
            this.SetCapacity(bitList.FCount);
            RtlUnit.Move(bitList.FList.Span.Slice(0, (int)(((uint)(this.FCount + 31)) >> 5) * 4), this.FList.Span.Slice(0, (int)(((uint)(this.FCount + 31)) >> 5) * 4), (int)(((uint)(this.FCount + 31)) >> 5) * 4);
            this.FDefault = bitList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:4590
    public int Add(bool Item)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        Put(FCount, Item);
        return Result;
    }

    // Engine.pas:4597
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:4603
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            for (int I = Index, last = FCount - 1; I <= last; I++)
            {
                Put(I, Get(I + 1));
            }
        }
    }

    // Engine.pas:4624
    public void Exchange(int Index1, int Index2)
    {
        bool Item;
        if (Index1 < 0 || Index1 >= FCount)
        {
            Error("List index out of bounds (%d)", Index1);
        }

        if (Index2 < 0 || Index2 >= FCount)
        {
            Error("List index out of bounds (%d)", Index2);
        }

        Item = Get(Index1);
        Put(Index1, Get(Index2));
        Put(Index2, Item);
    }

    // Engine.pas:4635
    public TBitList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4641
    public bool First() => Get(0);

    // Engine.pas:4688
    public int IndexOf(bool Item)
    {
        int Result = 0;
        while (Result < FCount && Item != Get(Result))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:4695
    public void Insert(int Index, bool Item)
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
            FCount += 1;
            // { sic: the original copies the same bit FCount - Index - 1 times instead of moving the tail }
            for (int I = FCount - 2; I >= Index; I--)
            {
                Put(Index + 1, Get(Index));
            }
        }
        else
        {
            FCount += 1;
        }

        Put(Index, Item);
    }

    // Engine.pas:4712
    public bool Last() => Get(FCount - 1);

    // Engine.pas:4717
    public void Move(int CurIndex, int NewIndex)
    {
        bool Item;
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

    // Engine.pas:4730
    public int Remove(bool Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:4736
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

    // Engine.pas:4775
    public bool Compare(TBitList List) => List.FCount == FCount && CompareMem(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), List.FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), (int)(((uint)(FCount + 7)) >> 3));

    // Engine.pas:4782
    public void LoadFromStream(TStream Stream)
    {
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 1));
        Stream.Read(Bytes.Of(ref FCount, 4));
        SetCapacity(FCount);
        Stream.Read(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
    }

    // Engine.pas:4791
    public void SaveToStream(TStream Stream)
    {
        Stream.Write(Bytes.Of(ref FDefault, 1));
        Stream.Write(Bytes.Of(ref FCount, 4));
        Stream.Write(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public bool Default { get => FDefault; set => FDefault = value; }

    public bool this[int Index] { get => Get(Index); set => Put(Index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public int BitsSet { get => GetBitsSet(); }
    public int BitsCleared { get => GetBitsCleared(); }

    // Engine.pas:4613
    protected internal static void Error(string Msg, int Data)
    {
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

public partial class TEventList : TPersistent
{
    internal TNotifyEvent[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal TNotifyEvent FDefault;
    // Engine.pas:4929
    internal TNotifyEvent Get(int Index)
    {
        TNotifyEvent Result;
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

    // Engine.pas:4987
    internal void Put(int Index, TNotifyEvent Item)
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

    // Engine.pas:5011
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // Engine.pas:5022
    internal void SetCount(int NewCount)
    {
        int OldCount;
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            OldCount = FCount;
            FCount = NewCount;
            for (int I = OldCount, last = FCount - 1; I <= last; I++)
            {
                Put(I, FDefault);
            }
        }
        else
        {
            FCount = NewCount;
        }
    }

    // Engine.pas:4937
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

    // Engine.pas:4854
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:4859
    public override void Assign(TPersistent Source)
    {
        if (Source is TEventList)
        {
            var eventList = (TEventList)Source;
            this.FCount = eventList.FCount;
            this.SetCapacity(eventList.FCount);
            Array.Copy(eventList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = eventList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:4873
    public int Add(TNotifyEvent Item)
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

    // Engine.pas:4881
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:4887
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

    // Engine.pas:4907
    public void Exchange(int Index1, int Index2)
    {
        TNotifyEvent Item;
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

    // Engine.pas:4918
    public TEventList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4924
    public TNotifyEvent First() => Get(0);

    // Engine.pas:4947
    public int IndexOf(TNotifyEvent Item)
    {
        Delegate M;
        int Result = 0;
        M = Item;
        while (Result < FCount && (MethodTarget(FList[Result]) != MethodTarget(M) || MethodCode(FList[Result]) != MethodCode(M)))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:4958
    public void Insert(int Index, TNotifyEvent Item)
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

    // Engine.pas:4969
    public TNotifyEvent Last() => Get(FCount - 1);

    // Engine.pas:4974
    public void Move(int CurIndex, int NewIndex)
    {
        TNotifyEvent Item;
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

    // Engine.pas:4994
    public int Remove(TNotifyEvent Item)
    {
        int Result = default;
        Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:5000
    public void Pack()
    {
        Delegate M;
        M = FDefault;
        // { sic: the original deletes an item only when both halves differ from the default }
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (MethodTarget(FList[I]) != MethodTarget(M) && MethodCode(FList[I]) != MethodCode(M))
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5039
    public bool Compare(TEventList List) => List.FCount == FCount && FList.AsSpan(0, FCount).SequenceEqual(List.FList.AsSpan(0, FCount));

    // Engine.pas:5046
    public void TriggerNotifyEvents(TObject Sender)
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (FList[I] != null)
            {
                FList[I](Sender);
            }
        }
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public TNotifyEvent Default { get => FDefault; set => FDefault = value; }

    public TNotifyEvent this[int Index] { get => Get(Index); set => Put(Index, value); }

    public TNotifyEvent[] List { get => FList; }

    // Engine.pas:4896
    protected internal static void Error(string Msg, int Data)
    {
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TDoubleItem
{
    [FieldOffset(0)]
    public int A;
    [FieldOffset(4)]
    public int B;
}

public delegate int TDoubleItemSortCompare(in TDoubleItem Item1, in TDoubleItem Item2);
public partial class TDoubleItemList : TPersistent
{
    internal TDoubleItem[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal TDoubleItem FDefault;
    // Engine.pas:5140
    internal TDoubleItem Get(int Index)
    {
        TDoubleItem Result;
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
    internal void Put(int Index, TDoubleItem Item)
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
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
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
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            EngineUnit.FillLargeInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
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
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:5061
    public override void Assign(TPersistent Source)
    {
        if (Source is TDoubleItemList)
        {
            var doubleItemList = (TDoubleItemList)Source;
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
    public int Add(in TDoubleItem Item)
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
        TDoubleItem Item;
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
    public TDoubleItemList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:5135
    public TDoubleItem First() => Get(0);

    // Engine.pas:5158
    public int IndexOf(in TDoubleItem Item)
    {
        int Result = 0;
        while (Result < FCount && (double)Unsafe.As<TDoubleItem, long>(ref FList[Result]) != Unsafe.BitCast<TDoubleItem, long>(Item))
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
    public void Insert(int Index, in TDoubleItem Item)
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
    public TDoubleItem Last() => Get(FCount - 1);

    // Engine.pas:5195
    public void Move(int CurIndex, int NewIndex)
    {
        TDoubleItem Item;
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
    public int Remove(in TDoubleItem Item)
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
        TDoubleItem Item = default;
        for (int I = FCount - 1; I >= 0; I--)
        {
            Item = this[I];
            if ((double)Unsafe.As<TDoubleItem, long>(ref Item) == Unsafe.As<TDoubleItem, long>(ref FDefault))
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5281
    public void Sort(TDoubleItemSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            EngineUnit.QuickSortDoubleItem(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:5307
    public void SortAscendingItemA()
    {
        Sort(EngineUnit.CompareDoubleItemAAscending);
    }

    // Engine.pas:5312
    public void SortAscendingItemB()
    {
        Sort(EngineUnit.CompareDoubleItemBAscending);
    }

    // Engine.pas:5317
    public void SortDescendingItemA()
    {
        Sort(EngineUnit.CompareDoubleItemADescending);
    }

    // Engine.pas:5322
    public void SortDescendingItemB()
    {
        Sort(EngineUnit.CompareDoubleItemBDescending);
    }

    // Engine.pas:5327
    public bool Compare(TDoubleItemList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 8);

    // Engine.pas:5334
    public void LoadFromStream(TStream Stream)
    {
        TDoubleItem Item = default;
        int Run = default;
        int Count = default;
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 8));
        Stream.Read(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.Read(Bytes.Of(ref Item, 8));
            if ((double)Unsafe.As<TDoubleItem, long>(ref Item) == Unsafe.As<TDoubleItem, long>(ref FDefault))
            {
                Stream.Read(Bytes.Of(ref Run, 4));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:5356
    public void SaveToStream(TStream Stream)
    {
        int I;
        int Run = default;
        int DefaultRun()
        {
            int J;
            int Result = 0;
            J = I;
            while (J < FCount && (double)Unsafe.As<TDoubleItem, long>(ref FList[J]) == Unsafe.As<TDoubleItem, long>(ref FDefault))
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
    public TDoubleItem Default { get => FDefault; set => FDefault = value; }

    public TDoubleItem this[int Index] { get => Get(Index); set => Put(Index, value); }

    public TDoubleItem[] List { get => FList; }

    // Engine.pas:5107
    protected internal static void Error(string Msg, int Data)
    {
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct TQuadInteger
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

public delegate int TQuadItemSortCompare(in TQuadInteger Item1, in TQuadInteger Item2);
public partial class TQuadItemList : TPersistent
{
    internal TQuadInteger[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal TQuadInteger FDefault;
    // Engine.pas:5538
    internal TQuadInteger Get(int Index)
    {
        TQuadInteger Result;
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
    internal void Put(int Index, TQuadInteger Item)
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
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
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
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            EngineUnit.FillQuadInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
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
    public override void Destroy()
    {
        Clear();
    }

    // Engine.pas:5457
    public override void Assign(TPersistent Source)
    {
        if (Source is TQuadItemList)
        {
            var quadItemList = (TQuadItemList)Source;
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
    public int Add(in TQuadInteger Item)
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
        TQuadInteger Item;
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
    public TQuadItemList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:5533
    public TQuadInteger First() => Get(0);

    // Engine.pas:5556
    public int IndexOf(in TQuadInteger Item)
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
    public void Insert(int Index, in TQuadInteger Item)
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
    public TQuadInteger Last() => Get(FCount - 1);

    // Engine.pas:5608
    public void Move(int CurIndex, int NewIndex)
    {
        TQuadInteger Item;
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
    public int Remove(in TQuadInteger Item)
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
    public void Sort(TQuadItemSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            EngineUnit.QuickSortQuadItem(FList, 0, Count - 1, Compare);
        }
    }

    // Engine.pas:5737
    public void SortAscendingItemA()
    {
        Sort(EngineUnit.CompareQuadItemAAscending);
    }

    // Engine.pas:5742
    public void SortAscendingItemB()
    {
        Sort(EngineUnit.CompareQuadItemBAscending);
    }

    // Engine.pas:5747
    public void SortAscendingItemC()
    {
        Sort(EngineUnit.CompareQuadItemCAscending);
    }

    // Engine.pas:5752
    public void SortAscendingItemD()
    {
        Sort(EngineUnit.CompareQuadItemDAscending);
    }

    // Engine.pas:5757
    public void SortDescendingItemA()
    {
        Sort(EngineUnit.CompareQuadItemADescending);
    }

    // Engine.pas:5762
    public void SortDescendingItemB()
    {
        Sort(EngineUnit.CompareQuadItemBDescending);
    }

    // Engine.pas:5767
    public void SortDescendingItemC()
    {
        Sort(EngineUnit.CompareQuadItemCDescending);
    }

    // Engine.pas:5772
    public void SortDescendingItemD()
    {
        Sort(EngineUnit.CompareQuadItemDDescending);
    }

    // Engine.pas:5777
    public bool Compare(TQuadItemList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 16);

    // Engine.pas:5784
    public void LoadFromStream(TStream Stream)
    {
        int Run = default;
        int Count = default;
        TQuadInteger Item = default;
        Clear();
        Stream.Read(Bytes.Of(ref FDefault, 16));
        Stream.Read(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (FCount < Count)
        {
            Stream.Read(Bytes.Of(ref Item, 16));
            if (Item.A == FDefault.A && Item.B == FDefault.B && Item.C == FDefault.C && Item.D == FDefault.D)
            {
                Stream.Read(Bytes.Of(ref Run, 4));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:5807
    public void SaveToStream(TStream Stream)
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
    public TQuadInteger Default { get => FDefault; set => FDefault = value; }

    public TQuadInteger this[int Index] { get => Get(Index); set => Put(Index, value); }

    public TQuadInteger[] List { get => FList; }

    // Engine.pas:5505
    protected internal static void Error(string Msg, int Data)
    {
        throw EListError.CreateFmt(Msg, [Data]);
    }


}

// The record lists above with an object column. Engine.pas stored Integer(Object) in column A of a
// TDoubleItemList or TQuadItemList and cast it back on read. These lists keep the reference itself,
// so no id table holds the object and the owner of the list alone decides its lifetime. Every
// column that held a number stays an int. Sort runs the quicksort of EngineUnit.QuickSortDoubleItem,
// so a list sorted on its int columns keeps the original order.
public delegate int TRecordSortCompare<TRecord>(in TRecord Item1, in TRecord Item2);

public class TRecordList<TRecord> : TPersistent where TRecord : struct
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
        if (NewCapacity < FCount || NewCapacity > EngineUnit.MaxListSize)
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
        if (NewCount < 0 || NewCount > EngineUnit.MaxListSize)
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

    public override void Destroy()
    {
        Clear();
        base.Destroy();
    }

    public override void Assign(TPersistent Source)
    {
        if (Source is TRecordList<TRecord> list)
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

    public void Sort(TRecordSortCompare<TRecord> Compare)
    {
        if (FCount > 0)
        {
            QuickSort(FList, 0, FCount - 1, Compare);
        }
    }

    // Engine.pas:5254, the quicksort of QuickSortDoubleItem and QuickSortQuadItem.
    private static void QuickSort(Span<TRecord> SortList, int L, int R, TRecordSortCompare<TRecord> SCompare)
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
        throw EListError.CreateFmt(Msg, [Data]);
    }
}

public struct TDoubleItem<TA> where TA : class
{
    public TA A;
    public int B;
}

public class TDoubleItemList<TA> : TRecordList<TDoubleItem<TA>> where TA : class
{
    public int AddAB(TA A, int B) => Add(new TDoubleItem<TA> { A = A, B = B });

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

public struct TQuadItem<TA> where TA : class
{
    public TA A;
    public int B;
    public int C;
    public int D;
}

public class TQuadItemList<TA> : TRecordList<TQuadItem<TA>> where TA : class
{
    public int AddABCD(TA A, int B, int C, int D) => Add(new TQuadItem<TA> { A = A, B = B, C = C, D = D });

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

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TPropertyItem
{
    [FieldOffset(0)]
    public int ID;
    [FieldOffset(4)]
    public int Offset;
}

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct TPropertySettings
{
    [FieldOffset(0)]
    public int First;
    [FieldOffset(4)]
    public int NextIndex;
    [FieldOffset(8)]
    public int MaxID;
}

public partial class TPropertyTable : TObject
{
    internal TPropertyItem[] FItems = [];
    internal int FCapacity;
    internal int FCount;
    internal int FNextIndex;
    internal int FMaxID;
    internal TPropertySettings[] FStack = [];
    internal int FStackCount;
    internal int FStackCapacity;
    internal int FFirst;
    // Engine.pas:2260
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity > 0)
        {
            NewCapacity = (NewCapacity + 31) & ~31;
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FItems, NewCapacity);
            FCapacity = NewCapacity;
            if (FCapacity < FCount)
            {
                FCount = FCapacity;
            }
        }
    }

    // Engine.pas:2253
    public override void Destroy()
    {
        SetCapacity(0);
        Array.Resize(ref FStack, 0);
        base.Destroy();
    }

    // Engine.pas:2273
    public void Add(int ID, int Offset)
    {
        if (FCapacity == FCount)
        {
            SetCapacity(FCapacity + 1);
        }

        FItems[FCount].ID = ID;
        FItems[FCount].Offset = Offset;
        FCount += 1;
        if (ID > FMaxID)
        {
            FMaxID = ID;
        }
    }

    // Engine.pas:2284
    public void Clear()
    {
        FNextIndex = 0;
        FMaxID = 0;
        SetCapacity(0);
    }

    // Engine.pas:2291
    public int FindOffset(int ID)
    {
        int Result;
        int I;
        I = FNextIndex;
        if (I >= FCount || FItems[I].ID != ID)
        {
            I = FFirst;
            Result = -1;
            while (I < FCount && FItems[I].ID != ID)
            {
                I += 1;
            }

            if (I != FCount)
            {
                Result = FItems[I].Offset;
                FNextIndex = I + 1;
            }
        }
        else
        {
            Result = FItems[I].Offset;
            FNextIndex += 1;
        }

        return Result;
    }

    // Engine.pas:2315
    public bool PropertyExist(int ID)
    {
        bool Result;
        int I;
        if (ID <= FMaxID)
        {
            I = FFirst;
            while (I < FCount && FItems[I].ID != ID)
            {
                I += 1;
            }

            Result = I != FCount;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Engine.pas:2330
    public void BackupSettings()
    {
        if (FStackCount == FStackCapacity)
        {
            FStackCapacity += 16;
            Array.Resize(ref FStack, FStackCapacity);
        }

        ref var propertySettings = ref FStack[FStackCount];
        propertySettings.First = FFirst;
        propertySettings.NextIndex = FNextIndex;
        propertySettings.MaxID = FMaxID;

        FStackCount += 1;
        FNextIndex = FCount;
        FFirst = FCount;
        FMaxID = 0;
    }

    // Engine.pas:2349
    public void RestoreSettings()
    {
        FStackCount -= 1;
        FCount = FFirst;
        ref var propertySettings = ref FStack[FStackCount];
        FFirst = propertySettings.First;
        FNextIndex = propertySettings.NextIndex;
        FMaxID = propertySettings.MaxID;
    }

    // Engine.pas:2361
    public void LoadFromStream(TStream Stream)
    {
        int LongCount = default;
        int ID = default;
        int Offset = default;
        byte ShortCount = default;
        ushort Pair = default;
        Stream.Read(Bytes.Of(ref ShortCount, 1));
        if ((ShortCount & 128) != 0)
        {
            ShortCount = (byte)(ShortCount & 127);
            Stream.Read(Bytes.Of(ref LongCount, 4));
        }
        else
        {
            LongCount = 0;
        }

        if (ShortCount != 0)
        {
            for (int ordinal = 0, last = ShortCount - 1; ordinal <= last; ordinal++)
            {
                Stream.Read(Bytes.Of(ref Pair, 2));
                Add(Pair & 255, (int)(((uint)Pair) >> 8));
            }
        }

        if (LongCount != 0)
        {
            for (int ordinal = 0, last = LongCount - 1; ordinal <= last; ordinal++)
            {
                Stream.Read(Bytes.Of(ref ID, 4));
                Stream.Read(Bytes.Of(ref Offset, 4));
                Add(ID, Offset);
            }
        }
    }

    // Engine.pas:2391
    public void SaveToStream(TStream Stream)
    {
        int LongCount;
        byte ShortCount;
        byte Marker;
        int Pair = default;
        LongCount = 0;
        ShortCount = 0;
        for (int I = FFirst, last = FCount - 1; I <= last; I++)
        {
            if (FItems[I].ID <= 127 && FItems[I].Offset <= 255)
            {
                ShortCount = (byte)(ShortCount + 1);
            }
            else
            {
                LongCount += 1;
            }
        }

        if (LongCount != 0)
        {
            Marker = (byte)(ShortCount | 128);
            Stream.Write(Bytes.Of(ref Marker, 1));
            Stream.Write(Bytes.Of(ref LongCount, 4));
        }
        else
        {
            Stream.Write(Bytes.Of(ref ShortCount, 1));
        }

        if (ShortCount != 0)
        {
            for (int I = FFirst, last = FCount - 1; I <= last; I++)
            {
                if (FItems[I].ID <= 127 && FItems[I].Offset <= 255)
                {
                    Pair = FItems[I].ID + (FItems[I].Offset << 8);
                    Stream.Write(Bytes.Of(ref Pair, 2));
                }
            }
        }

        if (LongCount != 0)
        {
            for (int I = FFirst, last = FCount - 1; I <= last; I++)
            {
                if (FItems[I].ID > 127 || FItems[I].Offset > 255)
                {
                    Stream.Write(Bytes.Of(ref FItems[I].ID, 4));
                    Stream.Write(Bytes.Of(ref FItems[I].Offset, 4));
                }
            }
        }
    }

    public TPropertyItem[] Items { get => FItems; }
    public int Count { get => FCount; }
    public int First { get => FFirst; }
}

// FOwner
public partial class TEObject : TObject
{
    // Engine.pas:2461
    public const int ClassID = 80;

    private EClass eclass;
    // The engine class descriptor of the instance's exact type, read once and cached.
    public EClass EClass => eclass ??= EClass.Of(GetType());

    // The Delphi pre-inherited statements run after capture and before any inherited body.
    private protected virtual void InitializeBeforeInherited() { }

    public TEObject(TECustomNode owner) : this(null, owner) { }

    private protected TEObject(ConstructionContext context, TECustomNode owner)
    {
        context?.Capture(this);
        InitializeBeforeInherited();
        SetOwner(owner);
    }

    protected internal TECustomNode FOwner;
    // Engine.pas:2487
    internal string GetEObjectPath()
    {
        TEObject P;
        string S;
        S = ClassNameOf(this.GetType());
        P = FOwner;
        while (P != null)
        {
            S = ClassNameOf(P.GetType()) + "" + S;
            P = P.FOwner;
        }

        return S;
    }

    // Engine.pas:2442
    public virtual int ClassVersion() => 0;

    // Engine.pas:2563
    public virtual TSet<TEControlStyleElement> GetControlStyle() => TSet<TEControlStyleElement>.Of(EngineUnit.csStored);

    // Engine.pas:2568
    public virtual void SetOwner(TECustomNode AOwner)
    {
        if (AOwner != FOwner)
        {
            if (FOwner != null)
            {
                FOwner.RemoveChild(this);
            }

            FOwner = AOwner;
            if (FOwner != null)
            {
                FOwner.AddChild(this);
            }
        }
    }

    // Engine.pas:2558
    public virtual TEngine GetEngine() => FOwner.GetEngine();

    // Engine.pas:2502
    public virtual int GetLastMsg() => 2147483647;

    // Engine.pas:2507
    public virtual void SetLastMsg(int Value)
    {
    }

    // Engine.pas:2590
    public virtual void ReadWrite(TEStorageStream Stream)
    {
    }

    // Engine.pas:2594
    public virtual void MainReadWrite(TEStorageStream Stream)
    {
        if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
        {
            Clear();
        }

        ReadWrite(Stream);
    }

    // Engine.pas:2447
    public virtual int AddRef()
    {
        throw Exception.Create("Reference counting not supported");
    }

    // Engine.pas:2452
    public virtual int Release()
    {
        throw Exception.Create("Reference counting not supported");
    }

    // Engine.pas:2457
    public virtual void Clear()
    {
    }

    // Engine.pas:2511
    public virtual bool Compare(TEObject Obj, TEngine AEngine)
    {
        TMemoryStream S1;
        TMemoryStream S2;
        bool Result = false;
        if (EClass.ClassID != Obj.EClass.ClassID)
        {
            return Result;
        }

        S1 = new TMemoryStream();
        S2 = new TMemoryStream();
        try
        {
            AEngine.WriteEObject(S1, this, null);
            AEngine.WriteEObject(S2, Obj, null);
            bool condition = S1.Size == S2.Size;
            if (condition)
            {
                var size = S1.Size;
                condition = CompareMem(S1.Memory.AsSpan(0, size), S2.Memory.AsSpan(0, size), size);
            }

            Result = condition;
        }
        finally
        {
            S1.Free();
            S2.Free();
        }

        return Result;
    }

    // Engine.pas:2466
    public virtual EditorClass Editor()
    {
        TEngine E;
        EditorClass Result = null;
        E = GetEngine();
        if (E != null)
        {
            for (var C = EClass; C.Type != typeof(TEObject) && Result == null; C = C.Parent)
            {
                Result = E.FEditors.FindEditor(C.ClassID);
            }
        }

        if (Result == null)
        {
            Result = EditorClass.Of<TEObjectEditor>();
        }

        return Result;
    }

    // Engine.pas:2529
    public virtual void Assign(TEObject Source, TEngine AEngine)
    {
        TMemoryStream S;
        S = new TMemoryStream();
        try
        {
            AEngine.WriteEObject(S, Source, null);
            S.Position = 0;
            AEngine.ReadEObject(S, this, null);
        }
        finally
        {
            S.Destroy();
        }
    }

    // Engine.pas:2483
    public virtual void TimerEvent(int Tick, TEObject Sender)
    {
    }

    // Engine.pas:2585
    public virtual void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        if (Msg.MsgID == 1)
        {
            SetLastMsg(2147483647);
        }
    }

    // Engine.pas:2578
    public virtual void SetArrayOwner(TECustomNode AOwner, int Index)
    {
        if (FOwner != null)
        {
            FOwner.RemoveChild(this);
        }

        FOwner = AOwner;
        if (FOwner != null)
        {
            FOwner.SetChild(Index, this);
        }
    }

    // Engine.pas:2436
    public override void Destroy()
    {
        SetOwner(null);
        base.Destroy();
    }

    // Engine.pas:2543
    public TEObject Copy(TEngine AEngine)
    {
        TEObject Result = default;
        TMemoryStream S;
        S = new TMemoryStream();
        try
        {
            AEngine.WriteEObject(S, this, null);
            S.Position = 0;
            Result = EClass.Create(null);
            AEngine.ReadEObject(S, Result, null);
        }
        finally
        {
            S.Destroy();
        }

        return Result;
    }

    public TECustomNode Owner { get => FOwner; set => SetOwner(value); }
    public string EObjectPath { get => GetEObjectPath(); }
    public int LastMsg { get => GetLastMsg(); set => SetLastMsg(value); }


}

// children through an abstract list interface. Message ids carry flags: $10000000 deliver once
// per Seq, $20000000 owned children only, $40000000 do not forward to children
public partial class TECustomNode : TEObject
{

    // Engine.pas:2430
    public TECustomNode(TECustomNode owner) : this(null, owner) { }

    private protected TECustomNode(ConstructionContext context, TECustomNode owner) : base(context, owner)
    { }

    // Engine.pas:2666
    public override void MainReadWrite(TEStorageStream Stream)
    {
        if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
        {
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(EngineUnit.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of())
            {
                Clear();
            }
        }

        ReadWrite(Stream);
        ReadWriteChildren(Stream);
    }

    // Engine.pas:2626
    public override void Clear()
    {
        TEObject C;
        if (Count != 0)
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null && C.Owner == this)
                {
                    if (!C.GetControlStyle().Contains(EngineUnit.csShared))
                    {
                        C.Destroy();
                    }
                    else
                    {
                        C.Clear();
                    }
                }
            }

            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null)
                {
                    if (!(C.Owner == this && C.GetControlStyle().Contains(EngineUnit.csShared)))
                    {
                        RemoveChild(C);
                    }
                }
            }
        }
    }

    // Engine.pas:2648
    public override void Assign(TEObject Source, TEngine AEngine)
    {
        Clear();
        base.Assign(Source, AEngine);
    }

    // Engine.pas:2720
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (SendToChild(ref Msg))
        {
            SendMsg(ref Msg, Sender);
        }
    }

    // Engine.pas:593
    public virtual TEObject GetChild(int Index)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:594
    public virtual void SetChild(int Index, TEObject Child)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:595
    public virtual int GetCount()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:2661
    public virtual void ReadWriteChildren(TEStorageStream Stream)
    {
        Stream.rwChildren(this, 1);
    }

    // Engine.pas:2715
    public virtual bool SendToChild(ref TEMsg Msg) => (Msg.MsgID & 1073741824) == 0;

    // Engine.pas:2608
    public virtual void DestroyAllChildren()
    {
        TEObject C;
        for (int I = Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C != null && C.Owner == this)
            {
                C.Destroy();
            }
        }

        if (Count != 0)
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null)
                {
                    RemoveChild(C);
                }
            }
        }
    }

    // Engine.pas:2654
    public virtual void QuickClear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            RemoveChild(Children[I]);
        }
    }

    // Engine.pas:2674
    public virtual void SendMsg(ref TEMsg Msg, TEObject Sender)
    {
        TEObject C;
        if ((Msg.MsgID & 268435456) != 0)
        {
            if ((Msg.MsgID & 536870912) == 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.GetLastMsg() != Msg.Seq)
                    {
                        C.SetLastMsg(Msg.Seq);
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
            else
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.GetLastMsg() != Msg.Seq && C.Owner == this)
                    {
                        C.SetLastMsg(Msg.Seq);
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
        }
        else
        {
            if ((Msg.MsgID & 536870912) == 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null)
                    {
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
            else
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.Owner == this)
                    {
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
        }
    }

    // Engine.pas:601
    public virtual int RemoveChild(TEObject Child)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:602
    public virtual int AddChild(TEObject Child)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:2786
    // { The original tests Result before the child-count call. }
    public virtual int DestroyChildren(int AClassID)
    {
        TEObject C;
        int N;
        N = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C != null && C.EClass.ClassID == AClassID)
            {
                C.Destroy();
                N += 1;
            }
        }

        return N;
    }

    // Engine.pas:2768
    public virtual TEObject FindChildInTree(int AClassID)
    {
        int I;
        TEObject Result = FindChild(AClassID);
        if (Result == null)
        {
            I = 0;
            while (Result == null && I < Count)
            {
                if (Children[I] is TECustomNode)
                {
                    Result = ((TECustomNode)Children[I]).FindChildInTree(AClassID);
                }

                I += 1;
            }
        }

        return Result;
    }

    // Engine.pas:2726
    public virtual TEObject FindChild(int AClassID)
    {
        TEObject Result;
        int I;
        bool Found;
        TEObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < Count)
        {
            C = Children[I];
            Found = C != null && C.EClass.ClassID == AClassID;
            I += 1;
        }

        if (Found)
        {
            Result = C;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:2744
    public virtual TEObject FindOwnedChild(int AClassID)
    {
        TEObject Result;
        int I;
        bool Found;
        TEObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < Count)
        {
            C = Children[I];
            Found = C != null && C.Owner == this && AClassID == C.EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = C;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:2762
    public virtual int FindChildIndex(TEObject Child)
    {
        int Result = Count - 1;
        while (Result >= 0 && Child != Children[Result])
        {
            Result -= 1;
        }

        return Result;
    }

    // Engine.pas:2602
    public override void Destroy()
    {
        DestroyAllChildren();
        base.Destroy();
    }

    public int Count { get => GetCount(); }

    public sealed class ChildrenAccessor(TECustomNode target)
    {
        public TEObject this[int Index] { get => target.GetChild(Index); set => target.SetChild(Index, value); }
    }

    public ChildrenAccessor Children => new ChildrenAccessor(this);
}

// FMsgCount, FEClasses, FClassTable, FEditors, FControlNode, FComponent (the owning component),
// FDebugIDs, FDebugEvents, FStartupDirectory, FOptions. Every engine is in the Engines list
public partial class TEngine : TENode
{
    // Engine.pas:8756
    public new const int ClassID = 17;

    internal int FMsgCount;
    internal TEObjectClassList FEClasses;
    internal TIntegerHashTable256 FClassTable;
    internal TEObjectEditorList FEditors;
    internal TEControlStyleNode FControlNode;
    public TEComponent FComponent;
    internal TStringList FDebugIDs;
    internal TEventList FDebugEvents;
    internal string FStartupDirectory = "";
    public TSet<TEngineUserElement> FUser;
    internal TSet<TEngineModeElement> FOptions;
    // Engine.pas:8776
    internal int CreateUniqueDebugID()
    {
        int Result = 0;
        while (FDebugIDs.IndexOfObject(IntToObject(Result)) != -1)
        {
            Result += 1;
        }

        return Result;
    }

    // Engine.pas:8814
    public override TEngine GetEngine() => this;

    public TEngine(TECustomNode owner) : this(null, owner) { }

    private protected TEngine(ConstructionContext context, TECustomNode owner) : base(context, owner)
    {
        UpdateStartupDirectory();
        FOptions = TSet<TEngineModeElement>.Of(EngineUnit.eoVerifyClasses);
        FDebugEvents = new TEventList();
        FEClasses = new TEObjectClassList();
        FClassTable = new TIntegerHashTable256();
        FEditors = new TEObjectEditorList();
        FControlNode = new TEControlStyleNode(this);
        FControlNode.SetControlStyle(TSet<TEControlStyleElement>.Of(EngineUnit.csShared));
        FDebugIDs = new TStringList();
        FDebugIDs.AddObject(EngineUnit.UserStr, IntToObject(-1));
        RegisterEClasses([EClass.Of<TEControlStyleObject>(), EClass.Of<TEControlStyleNode>(), EClass.Of<TECustomOwner>(), EClass.Of<TEResourceEditSettings>()]);
        EngineUnit.Engines.Add(this);
    }

    // Engine.pas:8745
    public override void Destroy()
    {
        FEditors.Free();
        EngineUnit.Engines.Remove(this);
        FDebugEvents.Free();
        FDebugIDs.Free();
        base.Destroy();
        FClassTable.Free();
        FEClasses.Free();
    }

    // Engine.pas:8771
    public virtual void SetStartupDirectory(string Value)
    {
        FStartupDirectory = Value;
    }

    // Engine.pas:8766
    public virtual void UpdateStartupDirectory()
    {
        FStartupDirectory = ExtractFilePath(Application.ExeName);
    }

    // Engine.pas:8887
    public virtual TEObject CreateEObject(int AClassID, TECustomNode AOwner)
    {
        EClass C;
        C = GetEClass(AClassID);
        return C.Create(AOwner);
    }

    // Engine.pas:8860
    public virtual EClass GetEClass(int AClassID)
    {
        object P;
        P = FClassTable.Find(AClassID);
        EClass Result = (EClass)P;
        if (Result == null)
        {
            throw Exception.Create("Unregistered E-ClassID found (" + IntToStr(AClassID) + ")");
        }

        return Result;
    }

    // Engine.pas:8761
    public virtual string TranslateText(string Text) => Text;

    // Engine.pas:8782
    public int RegisterDebugID(string Name)
    {
        int Result = CreateUniqueDebugID();
        FDebugIDs.AddObject(Name, IntToObject(Result));
        return Result;
    }

    // Engine.pas:8788
    public void UnregisterDebugID(int ID)
    {
        int I;
        I = FDebugIDs.IndexOfObject(IntToObject(ID));
        if (I != -1)
        {
            FDebugIDs.Delete(I);
        }
    }

    // Engine.pas:8796
    public void OutputDebugStr(string S)
    {
        var notification = new EngineDebugNotification();
        ref var Info = ref notification.Info;
        Info.ID = -1;
        Info.Text = S;
        FDebugEvents.TriggerNotifyEvents(notification);
    }

    // Engine.pas:8805
    public void OutputDebugMessage(int ID, string S)
    {
        var notification = new EngineDebugNotification();
        ref var Info = ref notification.Info;
        Info.ID = ID;
        Info.Text = S;
        FDebugEvents.TriggerNotifyEvents(notification);
    }

    // Engine.pas:8819
    public void ResetMsgCount()
    {
        TEMsg Msg = default;
        EngineUnit.InitMsgEx(ref Msg, 1, this, FMsgCount + 1);
        MsgProc(ref Msg, this);
        FMsgCount = 0;
    }

    // Engine.pas:8828
    public int IncMsgCount()
    {
        FMsgCount += 1;
        if (FMsgCount > 268435455)
        {
            ResetMsgCount();
        }

        return FMsgCount;
    }

    // Engine.pas:8835
    public void RegisterEClasses(ReadOnlySpan<EClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            if (FOptions.Contains(EngineUnit.eoVerifyClasses))
            {
                VerifyEObjectClass(AClasses[I]);
            }

            var classID = AClasses[I].ClassID;
            if (FClassTable.Find(classID) != null)
            {
                throw Exception.Create("EObject Class ID double defined!");
            }

            FEClasses.Add(AClasses[I]);
            var classID2 = AClasses[I].ClassID;
            FClassTable.Add(AClasses[I], classID2);
        }
    }

    // Engine.pas:8849
    public void UnregisterEClasses(ReadOnlySpan<EClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            FEClasses.Remove(AClasses[I]);
            var classID = AClasses[I].ClassID;
            FClassTable.Delete(classID);
        }
    }

    // Engine.pas:8867
    public TEObjectClassList GetDescendantClasses(Type AClass)
    {
        TEObjectClassList L;
        L = new TEObjectClassList();
        for (int I = 0, last = FEClasses.Count - 1; I <= last; I++)
        {
            if (((EClass)FEClasses[I]).InheritsFrom(AClass))
            {
                L.Add(FEClasses[I]);
            }
        }

        return L;
    }

    // Engine.pas:8878
    public void ActivateEObject(TEObject Obj)
    {
        TEMsg Msg = default;
        EngineUnit.InitMsg(ref Msg, 536870920, this);
        Msg.Payload = this;
        Obj.MsgProc(ref Msg, this);
    }

    // Engine.pas:8895
    public void WriteEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
    {
        TEStorageStream S;
        S = new TEWriteStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S.Free();
        }
    }

    // Engine.pas:8908
    public void ReadEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
    {
        TEStorageStream S;
        S = new TEReadStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S.Free();
        }
    }

    // Engine.pas:8921
    public void WritePreviewEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
    {
        TEStorageStream S;
        S = new TEWriteStorageStream(Stream, this, true);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S.Free();
        }
    }

    // Engine.pas:8934
    public void ReadPreviewEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
    {
        TEStorageStream S;
        S = new TEReadStorageStream(Stream, this, true);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S.Free();
        }
    }

    // Engine.pas:8947
    public void StoreEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
    {
        TEStorageStream S;
        S = new TEWriteStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwCreateEObject(ref Obj, null, 0);
        }
        finally
        {
            S.Free();
        }
    }

    // Engine.pas:8960
    public TEObject RetrieveEObject(TStream Stream, TProgressEvent AProgress)
    {
        TEObject Result = default;
        TEStorageStream S;
        S = new TEReadStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwCreateEObject(ref Result, null, 0);
        }
        finally
        {
            S.Free();
        }

        return Result;
    }

    // Engine.pas:8973
    public TEObject RetrieveFromFile(string FileName, TProgressEvent AProgress)
    {
        TEObject Result = default;
        TFileStream F;
        F = new TFileStream(FileName, 0 | 32);
        try
        {
            Result = RetrieveEObject(F, AProgress);
        }
        finally
        {
            F.Free();
        }

        return Result;
    }

    // Engine.pas:8985
    public void StoreInFile(string FileName, TEObject Obj, TProgressEvent AProgress)
    {
        TFileStream F;
        F = new TFileStream(FileName, 65535);
        try
        {
            StoreEObject(F, Obj, AProgress);
        }
        finally
        {
            F.Free();
        }
    }

    // Engine.pas:8997
    public void WriteToFile(string FileName, TEObject Obj, TProgressEvent AProgress)
    {
        TMemoryStream M;
        TFileStream F = default;
        M = new TMemoryStream();
        try
        {
            WriteEObject(M, Obj, AProgress);
            F = new TFileStream(FileName, 65535);
            try
            {
                M.Position = 0;
                F.CopyFrom(M, M.Size);
            }
            finally
            {
                F.Free();
            }
        }
        finally
        {
            M.Free();
        }
    }

    // Engine.pas:9017
    public void ReadFromFile(string FileName, TEObject Obj, TProgressEvent AProgress)
    {
        TMemoryStream M;
        TFileStream F;
        M = new TMemoryStream();
        F = new TFileStream(FileName, 0 | 32);
        try
        {
            M.LoadFromStream(F);
        }
        finally
        {
            F.Free();
        }

        M.Position = 0;
        try
        {
            ReadEObject(M, Obj, AProgress);
        }
        finally
        {
            M.Free();
        }
    }

    // Engine.pas:9037
    public void WriteToFileCRC(string FileName, TEObject Obj, TProgressEvent AProgress)
    {
        int CRC1 = default;
        int CRC2 = default;
        TMemoryStream M;
        TFileStream F = default;
        M = new TMemoryStream();
        try
        {
            WriteEObject(M, Obj, AProgress);
            var size = M.Size;
            CRC1 = EngineUnit.GetCRC32(M.Memory, size);
            M.Write(Bytes.Of(ref CRC1, 4));
            var mSize = M.Size;
            CRC2 = EngineUnit.GetCRC32(M.Memory, mSize);
            F = new TFileStream(FileName, 65535);
            try
            {
                F.Write(Bytes.Of(ref CRC2, 4));
                var size2 = M.Size;
                F.Write(M.Memory.AsSpan(0, size2));
            }
            finally
            {
                F.Free();
            }
        }
        finally
        {
            M.Free();
        }
    }

    // Engine.pas:9061
    public bool ReadFromFileCRC(string FileName, TEObject Obj, TProgressEvent AProgress)
    {
        TMemoryStream M;
        TFileStream F;
        int CRC2;
        int CRC1;
        bool Result = false;
        M = new TMemoryStream();
        F = new TFileStream(FileName, 0 | 32);
        try
        {
            M.LoadFromStream(F);
            M.Position = 0;
            if (M.Size > 8)
            {
                CRC1 = MemoryMarshal.Read<int>(M.Memory.AsSpan(0, 4));
                var size = M.Size;
                CRC2 = MemoryMarshal.Read<int>(M.Memory.AsSpan(size - 4, 4));
                var mSize = M.Size;
                bool condition = CRC1 == EngineUnit.GetCRC32(M.Memory.AsSpan(4), mSize - 4);
                if (condition)
                {
                    var size2 = M.Size;
                    condition = CRC2 == EngineUnit.GetCRC32(M.Memory.AsSpan(4), size2 - 8);
                }

                if (condition)
                {
                    M.Position = 4;
                    ReadEObject(M, Obj, AProgress);
                    Result = true;
                }
            }
        }
        finally
        {
            F.Free();
            M.Free();
        }

        return Result;
    }

    // Engine.pas:9091
    public void ReadTextFromFile(string FileName, TEObject Obj)
    {
        TEStorageStream S;
        TFileStream F;
        F = new TFileStream(FileName, 0 | 32);
        try
        {
            S = new TEReadTextStorageStream(F, this, 0);
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S.Free();
            }
        }
        finally
        {
            F.Free();
        }
    }

    // Engine.pas:9109
    public void WriteTextToFile(string FileName, TEObject Obj)
    {
        TEStorageStream S;
        TFileStream F;
        F = new TFileStream(FileName, 65535);
        try
        {
            S = new TEWriteTextStorageStream(F, this);
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S.Free();
            }
        }
        finally
        {
            F.Free();
        }
    }

    public TEObjectClassList EClasses { get => FEClasses; }
    public TIntegerHashTable256 ClassTable { get => FClassTable; }
    public TEObjectEditorList Editors { get => FEditors; }
    public TEControlStyleNode ControlNode { get => FControlNode; }
    public TStringList DebugIDs { get => FDebugIDs; }
    public TEventList DebugEvents { get => FDebugEvents; }
    public string StartupDirectory { get => FStartupDirectory; set => SetStartupDirectory(value); }
    public TSet<TEngineModeElement> Options { get => FOptions; set => FOptions = value; }

    // Engine.pas:1126
    public bool VerifyEObjectClass(EClass type)
    {
        // The native allocator byte total has no corresponding managed heap measure.
        // Exercise the same constructor and destructor without comparing GC generations.
        type.Create(null).Free();
        return true;
    }
}

// the base of every storage stream. The rw* methods do nothing here. the subclasses read,
// write, compare or translate
public partial class TEStorageStream : TObject
{

    internal TEngine FEngine;
    internal TStream FStream;
    internal bool FErrorHandled;
    internal TPropertyTable FPropertyTable;
    internal TStorageStreamError FOnError;
    internal TProgressEvent FOnProgress;
    // Engine.pas:1184
    public virtual TSet<TStorageModeElement> StorageMode()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:9502
    public virtual TStream OpenPropertyStream(int ID) => null;

    // Engine.pas:1186
    public virtual void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:1187
    public virtual void rwEObject(TEObject Obj, int ID)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:1188
    public virtual void rwEObjectStream(TEObject Obj, int ID)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:1189
    public virtual void rwChildren(TECustomNode Node, int ID)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:9559
    public virtual void rwStreamProc(int ID, TStreamProc LoadProc, TStreamProc SaveProc)
    {
    }

    // Engine.pas:9507
    public virtual void rwData(Span<byte> Buffer, int Size, int ID)
    {
    }

    // Engine.pas:9511
    public virtual void rwString(ref string S, int ID)
    {
    }

    // Engine.pas:9515
    public virtual void rwStrings(TStrings Strings, int ID)
    {
    }

    // Engine.pas:9519
    public virtual void rwStringsEx(TStrings Strings, int ID)
    {
    }

    // Engine.pas:9523
    public virtual void rwInteger(ref int I, int ID)
    {
    }

    // Engine.pas:9527
    public virtual void rwByte(ref byte B, int ID)
    {
    }

    // Engine.pas:9535
    public virtual void rwWord(ref ushort W, int ID)
    {
    }

    // Engine.pas:9539
    public virtual void rwBool(ref bool B, int ID)
    {
    }

    // Engine.pas:9531
    public virtual void rwShortint(ref sbyte S, int ID)
    {
    }

    // Engine.pas:9543
    public virtual void rwIntegerEx(ref int I, int Default, int ID)
    {
    }

    // Engine.pas:9547
    public virtual void rwByteEx(ref byte B, byte Default, int ID)
    {
    }

    // Engine.pas:9551
    public virtual void rwWordEx(ref ushort W, ushort Default, int ID)
    {
    }

    // Engine.pas:9555
    public virtual void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
    }

    // Engine.pas:9489
    public TEStorageStream(TStream AStream, TEngine AEngine)
    {
        FEngine = AEngine;
        FStream = AStream;
        FErrorHandled = false;
    }

    // Engine.pas:9497
    public void ShowProgress(TEObject Sender, string Msg, int Percent)
    {
        if (FOnProgress != null)
        {
            FOnProgress(Sender, Msg, Percent);
        }
    }

    public TEngine Engine { get => FEngine; }
    public TStream Stream { get => FStream; }
    public TPropertyTable PropertyTable { get => FPropertyTable; }
    public TStorageStreamError OnError { get => FOnError; set => FOnError = value; }
    public TProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

    // Delphi var parameters alias a descendant reference through a base-class cast.
    public void rwCreateEObject<T>(ref T value, TECustomNode owner, int id) where T : class
    {
        TEObject result = (TEObject)(object)value;
        try { rwCreateEObject(ref result, owner, id); }
        finally { value = (T)(object)result; }
    }
}

// { Engine.TEObjectEditorList: the editor descriptors an engine registers, one per editor class id
//   (an EditorClass per editor class since 9.4-4, a list of editor metaclasses before) }
public partial class TEObjectEditorList : TObject
{
    private readonly List<EditorClass> FList = new();

    public int Count { get => FList.Count; }
    public EditorClass this[int Index] { get => FList[Index]; }

    // Engine.pas:8356
    public void RegisterClass(EditorClass AClass)
    {
        if (FindEditor(AClass.EditorClassID) == null)
        {
            FList.Add(AClass);
        }
        else
        {
            throw Exception.Create("EObjectEditor Double Defined!");
        }
    }

    // Engine.pas:8364
    public EditorClass FindEditor(int AClassID)
    {
        EditorClass Result;
        int I;
        I = 0;
        while (I != Count && AClassID != FList[I].EditorClassID)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = FList[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }
}

// FResourceList, FResourceID, FEditID (the edit id of the resource grid that placed it),
// FEditIndex (its cell in that grid). all three integers are persisted (ids 1.3)
public partial class TEResource : TEObject
{
    public new const int ClassID = 1024;

    public TEResource(TECustomNode owner) : base(owner)
    { }

    internal TECustomNode FResourceList;
    internal int FResourceID;
    internal int FEditID;
    internal int FEditIndex;
    // Engine.pas:8513
    public override void SetOwner(TECustomNode AOwner)
    {
        base.SetOwner(AOwner);
        FResourceList = AOwner;
    }

    // Engine.pas:8542
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FResourceID, 1);
        Stream.rwInteger(ref FEditID, 2);
        Stream.rwInteger(ref FEditIndex, 3);
    }

    // Engine.pas:8519
    public override void SetArrayOwner(TECustomNode AOwner, int Index)
    {
        base.SetArrayOwner(AOwner, Index);
        FResourceList = AOwner;
    }

    // Engine.pas:8533
    public virtual ImageLib.TLibraryImage GetCellImage() => null;

    // Engine.pas:8538
    public virtual void SetCellImage(ImageLib.TLibraryImage Value)
    {
    }

    // Engine.pas:8550
    public virtual int ResourceUserClassID() => 0;

    // Engine.pas:8555
    public virtual TEObject CreateResourceUser()
    {
        TEObject Result;
        int UserClassID;
        UserClassID = ResourceUserClassID();
        if (UserClassID != 0)
        {
            Result = GetEngine().CreateEObject(UserClassID, null);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8525
    public virtual void Draw(TCanvas Canvas, TRect Rect, ImageLib.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
    {
        TRect R = default;
        BltLibraryImage(Canvas, GetCellImage(), Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref R);
    }

    public TECustomNode ResourceList { get => FResourceList; set => FResourceList = value; }
    public int EditID { get => FEditID; set => FEditID = value; }
    public int EditIndex { get => FEditIndex; set => FEditIndex = value; }
    public int ResourceID { get => FResourceID; set => FResourceID = value; }
    public ImageLib.TLibraryImage CellImage { get => GetCellImage(); set => SetCellImage(value); }

    // Engine.pas:8508
    public virtual string ResourceEditName() => ClassNameOf(GetType());


}

public struct TEMsg
{
    public int MsgID;
    public int Seq;
    public TEObject Sender;
    public TEMsgParams Param;
    public MsgArg Payload;

    public FixedArray_int_4 Data;
    public int Result;
}

// The parameter slots of an engine message. The count is the arity of the message protocol
// (Engine.pas TEMsg.Param), not a count of content.
[InlineArray(Arity)]
public struct TEMsgParams
{
    public const int Arity = 3;

    private MsgArg element0;
}

public delegate void TStorageStreamError(TEStorageStream StorageStream, string Message, ref bool Handled);
public delegate void TProgressEvent(TEObject Sender, string Message, int Progression);
public partial class EDoublePrioriyID : Exception
{
    // SysUtils.pas:174
    public EDoublePrioriyID(string Msg)
    {
Message = Msg;
}
}

// the write stream's data buffer
public partial class TEBufferStream : TFastMemoryStream
{
    // System.pas:111
    public TEBufferStream()
    { }
}

public enum TCompressLevel : byte
{
    cpNone = 0,
    cpFastest = 1,
    cpDefault = 2,
    cpMax = 3
}

public partial class TCompressStream : TMemoryStream
{
    // Engine.pas:1715
    public new void LoadFromStream(TStream Stream)
    {
        bool Compressed = default;
        FixedArray_byte_4 Magic = default;
        TDecompressionStream D;
        byte[] Buffer;
        int Count;
        Stream.Read(((Span<byte>)Magic).Slice(0, 4));
        Stream.Read(Bytes.Of(ref Compressed, 1));
        if (!ArrayEquals<FixedArray_byte_4, byte>(Magic, EngineUnit.CompressMagic))
        {
            Compressed = false;
            Stream.Seek(0, 0);
        }

        if (Compressed)
        {
            Buffer = default;
            Count = -1;
            Array.Resize(ref Buffer, 8096);
            D = new TDecompressionStream(Stream);
            try
            {
                while (Count != 0)
                {
                    Count = D.Read(Buffer.AsSpan(0, 8096));
                    if (Count != 0)
                    {
                        this.WriteBuffer(Buffer.AsSpan(0, Count));
                    }
                }
            }
            finally
            {
                Array.Resize(ref Buffer, 0);
                D.Free();
            }
        }
        else
        {
            CopyFrom(Stream, Stream.Size - Stream.Position);
        }

        Position = 0;
    }

    // Engine.pas:1752
    public int SaveToStreamEx(TStream Stream, TCompressLevel Level)
    {
        TCompressionStream C;
        int StartSize;
        TCompressionLevel CompressionLevel;
        Position = 0;
        int Result = 100;
        StartSize = Stream.Size;
        Stream.Write(((ReadOnlySpan<byte>)EngineUnit.CompressMagic).Slice(0, 4));
        Stream.Write(Bytes.Of(ref Level, 1));
        if (Level != TCompressLevel.cpNone)
        {
            CompressionLevel = (TCompressionLevel)Level;
            C = new TCompressionStream(CompressionLevel, Stream);
            try
            {
                C.CopyFrom(this, 0);
                Result = Round((double)((double)1 - (double)(Stream.Size - StartSize) / Size) * 100);
            }
            finally
            {
                C.Free();
            }
        }
        else
        {
            Stream.CopyFrom(this, 0);
        }

        return Result;
    }

    // Engine.pas:1776
    public new void SaveToStream(TStream Stream)
    {
        SaveToStreamEx(Stream, TCompressLevel.cpDefault);
    }

    // Engine.pas:1781
    public int SaveToFileEx(string FileName, TCompressLevel Level)
    {
        int Result = default;
        TFileStream F;
        F = new TFileStream(FileName, 65535);
        try
        {
            Result = SaveToStreamEx(F, Level);
        }
        finally
        {
            F.Free();
        }

        return Result;
    }

    // Engine.pas:1793
    public new void SaveToFile(string FileName)
    {
        TFileStream F;
        F = new TFileStream(FileName, 65535);
        try
        {
            SaveToStreamEx(F, TCompressLevel.cpDefault);
        }
        finally
        {
            F.Free();
        }
    }

    // Engine.pas:1805
    public new void LoadFromFile(string FileName)
    {
        TFileStream F;
        F = new TFileStream(FileName, 0 | 32);
        try
        {
            LoadFromStream(F);
        }
        finally
        {
            F.Free();
        }
    }
}

// FRefCount, FEngines, FCounts. A plug-in registers itself in PlugIns on creation and counts
// the engines it is initialized for
public partial class TPlugIn : TObject
{
    internal int FRefCount;
    internal TList FEngines;
    internal TIntegerList FCounts;
    // Engine.pas:9144
    public TPlugIn()
    {
        FEngines = new TList();
        FCounts = new TIntegerList();
        EngineUnit.RegisterPlugIn(this);
        FRefCount = 1;
    }

    // Engine.pas:9153
    public override void Destroy()
    {
        while (FEngines.Count != 0)
        {
            Finalize((TEngine)FEngines[FEngines.Count - 1]);
        }

        FEngines.Free();
        FCounts.Free();
        EngineUnit.UnregisterPlugIn(this);
        base.Destroy();
    }

    // Engine.pas:9162
    public void AddRef()
    {
        FRefCount += 1;
    }

    // Engine.pas:9167
    public void Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0)
        {
            Destroy();
        }
    }

    // Engine.pas:9173
    public virtual string Name() => "Abstract PlugIn";

    // Engine.pas:9178
    public virtual bool Initialize(TEngine AEngine)
    {
        bool Result;
        int I;
        AddRef();
        I = FEngines.IndexOf(AEngine);
        if (I == -1)
        {
            FEngines.Add(AEngine);
            FCounts.Add(1);
            Result = true;
        }
        else
        {
            var item = FCounts[I];
            FCounts[I] = item + 1;
            Result = false;
        }

        return Result;
    }

    // Engine.pas:9197
    public virtual bool Finalize(TEngine AEngine)
    {
        bool Result;
        int I;
        I = FEngines.IndexOf(AEngine);
        if (I != -1)
        {
            if (FCounts[I] == 0)
            {
                throw Exception.Create("Invalid PlugIn Finalize Usage");
            }

            if (FCounts[I] == 1)
            {
                FEngines.Remove(AEngine);
                FCounts.Delete(I);
                Result = true;
            }
            else
            {
                var item = FCounts[I];
                FCounts[I] = item - 1;
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        Release();
        return Result;
    }

    public int RefCount { get => FRefCount; }

    // Engine.pas:550

}

public partial class EPlugInMissing : Exception
{
    // SysUtils.pas:174
    public EPlugInMissing(string Msg)
    {
Message = Msg;
}
}

// FEngine, FUsedPlugIns. the names of the plug-ins an engine uses, initialized and finalized as
// the list changes
public partial class TPlugInControl : TEObject
{
    internal TEngine FEngine;
    internal TStringList FUsedPlugIns;
    // Engine.pas:9279
    internal void SetUsedPlugIns(TStrings Value)
    {
        FUsedPlugIns.Assign(Value);
    }

    // Engine.pas:9284
    internal TStrings GetUsedPlugIns() => FUsedPlugIns;

    // Engine.pas:9289
    internal void OnChange(TObject Sender)
    {
        InitializePlugIns();
    }

    // Engine.pas:9294
    internal void OnChanging(TObject Sender)
    {
        FinalizePlugIns();
    }

    // Engine.pas:9299
    internal void InitializePlugIns()
    {
        int[] I__cell = new int[1];
        ref int I = ref I__cell[0];
        TPlugIn P;
        I = 0;
        int last = FUsedPlugIns.Count - 1;
        for (; I <= last; I += 1)
        {
            P = EngineUnit.FindPlugIn(FUsedPlugIns[I]);
            if (P != null)
            {
                P.Initialize(FEngine);
            }
            else
            {
                throw new EPlugInMissing("PlugIn Missing (" + FUsedPlugIns[I] + ")");
            }
        }
    }

    // Engine.pas:9319
    internal void FinalizePlugIns()
    {
        TPlugIn P;
        for (int I = 0, last = FUsedPlugIns.Count - 1; I <= last; I++)
        {
            P = EngineUnit.FindPlugIn(FUsedPlugIns[I]);
            if (P != null)
            {
                P.Finalize(FEngine);
            }
        }
    }

    // Engine.pas:9252
    public TPlugInControl(TECustomNode owner) : base(owner)
    {
        FUsedPlugIns = new TStringList();
        FUsedPlugIns.Sorted = true;
        FUsedPlugIns.OnChange = OnChange;
        FUsedPlugIns.OnChanging = OnChanging;
        FEngine = null;
    }

    // Engine.pas:9262
    public override void Destroy()
    {
        FUsedPlugIns.Clear();
        FUsedPlugIns.Free();
        base.Destroy();
    }

    // Engine.pas:9269
    public override TEngine GetEngine() => FEngine;

    // Engine.pas:9274
    public override TSet<TEControlStyleElement> GetControlStyle() => TSet<TEControlStyleElement>.Of(EngineUnit.csStored, EngineUnit.csShared);

    // Engine.pas:9331
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(TStorageModeElement.smTranslate))
        {
            Stream.rwStrings(FUsedPlugIns, 1);
        }
    }

    public TEngine Engine { get => FEngine; set => FEngine = value; }
    public TStrings UsedPlugIns { get => GetUsedPlugIns(); set => SetUsedPlugIns(value); }
}

// the child list of a TENode
public partial class TEObjectList : TList
{
    // Engine.pas:7136
    protected internal TEObject GetEObject(int Index) => (TEObject)this[Index];

    // Engine.pas:7141
    protected internal void SetEObject(int Index, TEObject Value)
    {
        this[Index] = Value;
    }

    // Engine.pas:7146
    public TEObject FindObject(int AClassID)
    {
        TEObject Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = List[I] != null && AClassID == ((TEObject)List[I]).EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = (TEObject)List[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:7161
    public int FindObjectIndex(int AClassID)
    {
        int Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = List[I] != null && AClassID == ((TEObject)List[I]).EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = I - 1;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:7176
    public void ClearList()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            // { the original tests List^[Count], one past the end, not List^[I] }
            if (List[Count] != null)
            {
                ((TEObject)List[I]).Destroy();
            }

            Delete(I);
        }
    }

    // Engine.pas:7188
    public bool AllHaveCID(int AClassID)
    {
        int I;
        bool OK;
        I = 0;
        OK = true;
        while (I < Count)
        {
            OK = List[I] == null || AClassID == ((TEObject)List[I]).EClass.ClassID;
            I += 1;
        }

        return OK && Count != 0;
    }

    public sealed class EObjectsAccessor(TEObjectList target)
    {
        public TEObject this[int Index] { get => target.GetEObject(Index); set => target.SetEObject(Index, value); }
    }

    public EObjectsAccessor EObjects => new EObjectsAccessor(this);
}

// FChildren
public partial class TENode : TECustomNode
{
    // Engine.pas:2861
    public new const int ClassID = 256;

    // Engine.pas:2807
    public TENode(TECustomNode owner) : this(null, owner) { }

    private protected TENode(ConstructionContext context, TECustomNode owner) : base(context, owner)
    {
        FChildren = new TEObjectList();
    }

    public TEObjectList FChildren;

    // Engine.pas:2833
    public override void Clear()
    {
        int I;
        TEObject C;
        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (TEObject)FChildren.List[I];
            if (C != null && C.Owner == this)
            {
                if (!C.GetControlStyle().Contains(EngineUnit.csShared))
                {
                    C.Destroy();
                }
                else
                {
                    C.Clear();
                }
            }
        }

        if (Count != 0)
        {
            I = Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (TEObject)FChildren.List[I];
                if (C != null)
                {
                    if (!(C.Owner == this && C.GetControlStyle().Contains(EngineUnit.csShared)))
                    {
                        RemoveChild(C);
                    }
                }
            }

            I = FChildren.Count - 1;
            while (I >= 0 && FChildren.List[I] == null)
            {
                FChildren.Delete(I);
                I -= 1;
            }
        }
    }

    // Engine.pas:2866
    public override TEObject GetChild(int Index) => (TEObject)FChildren.List[Index];

    // Engine.pas:2871
    public override void SetChild(int Index, TEObject Child)
    {
        while (Index >= Count)
        {
            FChildren.Add(null);
        }

        if (FChildren.List[Index] != null && ((TEObject)FChildren.List[Index]).Owner == this)
        {
            ((TEObject)FChildren.List[Index]).Destroy();
        }

        FChildren.List[Index] = Child;
    }

    // Engine.pas:2879
    public override int GetCount() => FChildren.Count;

    // Engine.pas:2819
    public override void DestroyAllChildren()
    {
        TEObject C;
        Clear();
        if (FChildren.Count != 0)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (TEObject)FChildren.List[I];
                if (C != null && C.Owner == this)
                {
                    C.Destroy();
                }
            }
        }
    }

    // Engine.pas:2884
    public override void SendMsg(ref TEMsg Msg, TEObject Sender)
    {
        var children = FChildren;
        if (GetControlStyle().Contains(EngineUnit.csKeepIndex))
        {
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children.List[I];
                            if (node.GetLastMsg() != Msg.Seq)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children[I];
                            if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children.List[I];
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children[I];
                            if (node.Owner == this)
                            {
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children.List[I];
                        if (node.GetLastMsg() != Msg.Seq)
                        {
                            node.SetLastMsg(Msg.Seq);
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children[I];
                        if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                        {
                            node.SetLastMsg(Msg.Seq);
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children.List[I];
                        node.MsgProc(ref Msg, this);
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children[I];
                        if (node.Owner == this)
                        {
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
        }
    }

    // Engine.pas:2981
    public override int RemoveChild(TEObject Child)
    {
        int Result;
        if (GetControlStyle().Contains(EngineUnit.csKeepIndex))
        {
            Result = FChildren.IndexOf(Child);
            if (Result != -1)
            {
                FChildren.List[Result] = null;
            }
        }
        else
        {
            Result = FChildren.Remove(Child);
        }

        return Result;
    }

    // Engine.pas:2992
    public override int AddChild(TEObject Child)
    {
        FChildren.Add(Child);
        return FChildren.Count - 1;
    }

    // Engine.pas:2998
    public override TEObject FindChild(int AClassID)
    {
        TEObject Result;
        int I;
        bool Found;
        TEObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < FChildren.Count)
        {
            C = (TEObject)FChildren.List[I];
            Found = C != null && AClassID == C.EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = C;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:3016
    public override int FindChildIndex(TEObject Child)
    {
        int Result = Count - 1;
        while (Result >= 0 && Child != (TEObject)FChildren.List[Result])
        {
            Result -= 1;
        }

        return Result;
    }

    // Engine.pas:2813
    public override void Destroy()
    {
        base.Destroy();
        FChildren.Free();
    }

    // Engine.pas:3022
    public void DestroyEObjects(int AClassID)
    {
        TEObject C;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (TEObject)FChildren.List[I];
            if (C != null && AClassID == C.EClass.ClassID && C.Owner == this)
            {
                C.Destroy();
            }
        }
    }

    // Engine.pas:631
}

// FTick, FCount, FTimerLists[1.10]. Children sit in one of ten lists and get TimerEvent every
// tick, every second tick. every tenth
public partial class TETimerNode : TECustomNode
{
    // Engine.pas:1836
    public new const int ClassID = 264;

    internal int FTick;
    internal int FCount;
    internal FixedArray_Aow_Rtl_TList_10 FTimerLists;
    // Engine.pas:1856
    internal int FindTimerList(TEObject Child)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I < 10)
        {
            I += 1;
            // { the original indexes the lists with Result (-1 on the first round), not with I }
            Result = FTimerLists[Result - 1].IndexOf(Child);
        }

        if (Result != -1)
        {
            Result = I;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:1819
    public TETimerNode(TECustomNode owner) : base(owner)
    {
        FCount = 0;
        for (int I = 1; I <= 10; I++)
        {
            var list = new TList();
            FTimerLists[I - 1] = list;
        }
    }

    // Engine.pas:1828
    public override void Destroy()
    {
        base.Destroy();
        for (int I = 1; I <= 10; I++)
        {
            FTimerLists[I - 1].Free();
        }
    }

    // Engine.pas:1841
    public override TSet<TEControlStyleElement> GetControlStyle() => TSet<TEControlStyleElement>.Of(EngineUnit.csStored, EngineUnit.csShared, EngineUnit.csReserved2);

    // Engine.pas:1846
    public override int GetCount() => FCount;

    // Engine.pas:1851
    public TList GetTimerList(sbyte Timer) => FTimerLists[Timer - 1];

    // Engine.pas:1871
    public override int RemoveChild(TEObject Child)
    {
        int Result;
        int L;
        L = FindTimerList(Child);
        if (L != -1)
        {
            Result = FTimerLists[L - 1].Remove(Child);
            if (Result != -1)
            {
                FCount -= 1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:1885
    public override int AddChild(TEObject Child)
    {
        int Result = FTimerLists[1 - 1].Add(Child);
        FCount += 1;
        return Result;
    }

    // Engine.pas:1891
    public override TEObject GetChild(int Index)
    {
        int I;
        int Total;
        TList L;
        I = 1;
        L = FTimerLists[1 - 1];
        Total = L.Count;
        while (Index >= Total && I < 10)
        {
            I += 1;
            L = FTimerLists[I - 1];
            Total += L.Count;
        }

        L = FTimerLists[I - 1];
        Total -= L.Count;
        return (TEObject)L.List[Index - Total];
    }

    // Engine.pas:1910
    public override void SetChild(int Index, TEObject Child)
    {
        throw Exception.Create("Set Child not supported (TETimerNode)");
    }

    // Engine.pas:1915
    public int AddTimerChild(TEObject Child, sbyte Timer)
    {
        int Result = FTimerLists[Timer - 1].Add(Child);
        FCount += 1;
        return Result;
    }

    // Engine.pas:1921
    public int RemoveTimerChild(TEObject Child, sbyte Timer)
    {
        int Result = FTimerLists[Timer - 1].Remove(Child);
        if (Result != -1)
        {
            FCount -= 1;
        }

        return Result;
    }

    // Engine.pas:1927
    public void TriggerTimerListEvent(int Timer)
    {
        for (int I = 0, last = FTimerLists[Timer - 1].Count - 1; I <= last; I++)
        {
            ((TEObject)FTimerLists[Timer - 1].List[I]).TimerEvent(FTick, this);
        }
    }

    // Engine.pas:1935
    public void TriggerTimerEvent()
    {
        if (FTick > 268435455)
        {
            FTick = FTick & 15;
        }

        FTick += 1;
        TriggerTimerListEvent(1);
        if ((FTick & 1) == 0)
        {
            TriggerTimerListEvent(2);
        }

        if (FTick % 3 == 0)
        {
            TriggerTimerListEvent(3);
        }

        if ((FTick & 3) == 2)
        {
            TriggerTimerListEvent(4);
        }

        if (FTick % 5 == 0)
        {
            TriggerTimerListEvent(5);
        }

        if (FTick % 6 == 0)
        {
            TriggerTimerListEvent(6);
        }

        if (FTick % 7 == 0)
        {
            TriggerTimerListEvent(7);
        }

        if ((FTick & 7) == 4)
        {
            TriggerTimerListEvent(8);
        }

        if (FTick % 9 == 0)
        {
            TriggerTimerListEvent(9);
        }

        if (FTick % 10 == 0)
        {
            TriggerTimerListEvent(10);
        }
    }

    public int Tick { get => FTick; }

}

// Engine.TEChangeNotifyNode (16 bytes): +8 FChildren, FUpdateCount. Changed on child changes
public partial class TEChangeNotifyNode : TECustomNode
{
    // Engine.pas:7246
    public new const int ClassID = 265;

    public TEChangeNotifyNode(TECustomNode owner) : base(owner)
    {
        FChildren = new TEObjectList();
        FUpdateCount = 0;
    }

    public TEObjectList FChildren;
    internal int FUpdateCount;

    // Engine.pas:7212
    public override void Destroy()
    {
        BeginUpdate();
        base.Destroy();
        FChildren.Free();
        FChildren = null;
    }

    // Engine.pas:7220
    public override void Clear()
    {
        int I;
        TEObject C;
        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (TEObject)FChildren.List[I];
            if (C != null && C.Owner == this)
            {
                if (!C.GetControlStyle().Contains(EngineUnit.csShared))
                {
                    C.Destroy();
                }
                else
                {
                    C.Clear();
                }
            }
        }

        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (TEObject)FChildren.List[I];
            if (C != null)
            {
                if (!(C.Owner == this && C.GetControlStyle().Contains(EngineUnit.csShared)))
                {
                    RemoveChild(C);
                }
            }
        }

        I = FChildren.Count - 1;
        while (I >= 0 && FChildren.List[I] == null)
        {
            FChildren.Delete(I);
            I -= 1;
        }

        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7251
    public override TEObject GetChild(int Index) => (TEObject)FChildren.List[Index];

    // Engine.pas:7256
    public override void SetChild(int Index, TEObject Child)
    {
        while (Index >= Count)
        {
            FChildren.Add(null);
        }

        FChildren[Index] = Child;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7263
    public override void Assign(TEObject Source, TEngine AEngine)
    {
        BeginUpdate();
        base.Assign(Source, AEngine);
        EndUpdate();
    }

    // Engine.pas:7270
    public void LockChanged()
    {
        FUpdateCount += 1;
    }

    // Engine.pas:7275
    public void UnlockChanged()
    {
        FUpdateCount -= 1;
    }

    // Engine.pas:7280
    public virtual void Changed()
    {
    }

    // Engine.pas:7284
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // Engine.pas:7957
    // { Original source line layout for EndUpdate.
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// }
    public void EndUpdate()
    {
        Assert(FUpdateCount > 0, "Invalid UpdateCount Usage", "Engine.pas", 7959);
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7964
    public void Sort(TListSortCompare Compare)
    {
        BeginUpdate();
        FChildren.Sort(Compare);
        EndUpdate();
    }

    // Engine.pas:7971
    public override void MainReadWrite(TEStorageStream Stream)
    {
        if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
        {
            BeginUpdate();
        }

        base.MainReadWrite(Stream);
        if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
        {
            EndUpdate();
        }
    }

    // Engine.pas:7978
    public override int GetCount() => FChildren.Count;

    // Engine.pas:7983
    public override void SendMsg(ref TEMsg Msg, TEObject Sender)
    {
        var children = FChildren;
        if (GetControlStyle().Contains(EngineUnit.csKeepIndex))
        {
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children.List[I];
                            if (node.GetLastMsg() != Msg.Seq)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children[I];
                            if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children.List[I];
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (TEObject)children[I];
                            if (node.Owner == this)
                            {
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children.List[I];
                        if (node.GetLastMsg() != Msg.Seq)
                        {
                            node.SetLastMsg(Msg.Seq);
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children[I];
                        if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                        {
                            node.SetLastMsg(Msg.Seq);
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children.List[I];
                        node.MsgProc(ref Msg, this);
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (TEObject)children[I];
                        if (node.Owner == this)
                        {
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
        }
    }

    // Engine.pas:8080
    public override int RemoveChild(TEObject Child)
    {
        int Result;
        if (GetControlStyle().Contains(EngineUnit.csKeepIndex))
        {
            Result = FChildren.IndexOf(Child);
            if (Result != -1)
            {
                FChildren.List[Result] = null;
            }
        }
        else
        {
            Result = FChildren.Remove(Child);
        }

        if (FUpdateCount == 0)
        {
            Changed();
        }

        return Result;
    }

    // Engine.pas:8092
    public override int AddChild(TEObject Child)
    {
        FChildren.Add(Child);
        if (FUpdateCount == 0)
        {
            Changed();
        }

        return FChildren.Count - 1;
    }

    // Engine.pas:8099
    public override TEObject FindChild(int AClassID)
    {
        TEObject Result;
        int I;
        bool Found;
        TEObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < FChildren.Count)
        {
            C = (TEObject)FChildren.List[I];
            Found = C != null && AClassID == C.EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = C;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8117
    public override int FindChildIndex(TEObject Child)
    {
        int Result = Count - 1;
        while (Result >= 0 && Child != FChildren.List[Result])
        {
            Result -= 1;
        }

        return Result;
    }

    public int UpdateCount { get => FUpdateCount; }

    // Engine.pas:680


}

// FEvents, notified by Changed
public partial class TEChangeNotifyEventList : TEChangeNotifyNode
{

    public TEChangeNotifyEventList(TECustomNode owner) : base(owner)
    {
        FEvents = new TEventList();
    }

    internal TEventList FEvents;

    // Engine.pas:8131
    public override void Destroy()
    {
        FEvents.Free();
        base.Destroy();
    }

    // Engine.pas:8137
    public override void Changed()
    {
        FEvents.TriggerNotifyEvents(this);
    }

    public TEventList Events { get => FEvents; }

    // Engine.pas:708

}

public enum TEditResult : byte
{
    erOK = 0,
    erCancel = 1
}

public delegate void TEditDoneEvent(TEditResult EditResult, TEObject EObject);
// editors are classes looked up by the class id of the object (TEObject.Editor). everything is
// called on the class. The base editor edits nothing: Edit reports a cancel at once
public partial class TEObjectEditor : TEObject, IEObjectEditorClass
{

    public TEObjectEditor(TECustomNode owner) : base(owner)
    { }

    // Engine.pas:3036
    // { TEObjectEditor: stubs }
    public static int EditorClassID => 0;

    // Engine.pas:3041
    public static void Edit(TEObject Obj, TEngine AEngine, TEditDoneEvent Done)
    {
        Done(TEditResult.erCancel, Obj);
    }


}

// FCount, FCapacity, FItems. The hash classes come from an include file of the original,
// D:\GEngine\Engine\inc\HashTab.PAS, whose name the Assert calls carry
public struct THashItem
{
    public IntOrRef Key;
    public object Item;
}

public partial class THashList : TObject
{
    internal int FCount;
    internal int FCapacity;
    internal THashItem[] FItems = [];
    // HashTab.PAS:18
    protected internal void SetCapacity(int NewCapacity)
    {
        Assert(NewCapacity >= FCount, "Capacity Error (THashList)", "HashTab.PAS", 20);
        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FItems, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // HashTab.PAS:28
    protected internal void Delete(int Index)
    {
        Assert(Index >= 0 && Index < FCount, "Delete Error (THashList)", "HashTab.PAS", 30);
        FCount -= 1;
        if (Index < FCount)
        {
            Array.Copy(FItems, Index + 1, FItems, Index, FCount - Index);
        }
    }

    // HashTab.PAS:36
    protected internal void Add(THashItem Item)
    {
        if (FCapacity == FCount)
        {
            SetCapacity(FCapacity + 16);
        }

        FItems[FCount] = Item;
        FCount += 1;
    }

    // HashTab.PAS:6
    // { The hash list and hash table classes of the Engine unit. The original keeps them in an include
//   file, D:\GEngine\Engine\inc\HashTab.PAS: the Assert calls carry that file name, and line numbers
//   counted from the start of Engine.pas (THashList.SetCapacity's Assert is at line 7127 there). The
//   declarations are in the Engine.pas interface. }
    public override void Destroy()
    {
        Clear();
        base.Destroy();
    }

    // HashTab.PAS:12
    public virtual void Clear()
    {
        FCount = 0;
        SetCapacity(0);
    }

    public int Count { get => FCount; }
}

// items keyed by an integer
public partial class TIntegerHashList : THashList
{
    // HashTab.PAS:43
    public void Add(object Item, int Key)
    {
        THashItem H = default;
        if (Find(Key) != null)
        {
            throw Exception.Create("ID already in HashTable");
        }

        Assert(Item != null, "Cannot add nil item to hash table", "HashTab.PAS", 48);
        H.Key = Key;
        H.Item = Item;
        base.Add(H);
    }

    // HashTab.PAS:54
    public new void Delete(int Key)
    {
        int I;
        I = FCount;
        if (I != 0)
        {
            do
            {
                I -= 1;
                if (Key == FItems[I].Key.Value)
                {
                    base.Delete(I);
                    return;
                }
            }
            while (!(I == 0));
        }
    }

    // HashTab.PAS:70
    public object Find(int Key)
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (Key == FItems[I].Key.Value)
            {
                return FItems[I].Item;
            }
        }

        return null;
    }
}

// the abstract table interface
public partial class TIntegerHashTable : TObject
{
    // Engine.pas:771
    public virtual void Add(object Item, int Key)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:772
    public virtual void Delete(int Key)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:773
    public virtual object Find(int Key)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:774
    public virtual void Clear()
    {
        throw EAbstractError.Create("Abstract Error");
    }
}

// 256 lists selected by the low key byte
public partial class TIntegerHashTable256 : TIntegerHashTable
{
    internal FixedArray_Aow_Units_Engine_TIntegerHashList_256 FLists;
    // HashTab.PAS:83
    public TIntegerHashTable256()
    {
        for (int I = 0; I <= 255; I++)
        {
            var integerHashList = new TIntegerHashList();
            FLists[I] = integerHashList;
        }
    }

    // HashTab.PAS:91
    public override void Destroy()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Free();
        }

        base.Destroy();
    }

    // HashTab.PAS:99
    public override void Clear()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Clear();
        }
    }

    // HashTab.PAS:106
    public override void Add(object Item, int Key)
    {
        FLists[Key & 255].Add(Item, Key);
    }

    // HashTab.PAS:111
    public override void Delete(int Key)
    {
        FLists[Key & 255].Delete(Key);
    }

    // HashTab.PAS:116
    public override object Find(int Key) => FLists[Key & 255].Find(Key);

    // HashTab.PAS:121
    public void DeleteSafe(object Item, int Key)
    {
        if (FLists[Key & 255].Find(Key) == Item)
        {
            FLists[Key & 255].Delete(Key);
        }
    }

    // HashTab.PAS:126
    public virtual int GetFreeID()
    {
        int Min;
        int MinIndex;
        int I;
        Min = 16777215;
        MinIndex = 0;
        I = 0;
        for (; I <= 255; I += 1)
        {
            if (Min > FLists[I].Count)
            {
                Min = FLists[I].Count;
                MinIndex = I;
            }
        }

        I = FLists[MinIndex].Count;
        while (FLists[MinIndex].Find((I << 8) + MinIndex) != null)
        {
            I += 1;
        }

        return (I << 8) + MinIndex;
    }
}

// THashList items whose key dword is a string
public partial class TStringHashList : THashList
{
    // HashTab.PAS:143
    public override void Clear()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            FItems[I].Key = "";
        }

        base.Clear();
    }

    // HashTab.PAS:151
    public object Find(string Key)
    {
        int I;
        object Result = null;
        I = FCount;
        if (I != 0)
        {
            do
            {
                I -= 1;
                if (AnsiCompareStr(FItems[I].Key.As<string>(), Key) == 0)
                {
                    return FItems[I].Item;
                }
            }
            while (!(I == 0));
        }

        return Result;
    }

    // HashTab.PAS:170
    public void Add(object Item, string Key)
    {
        THashItem H = default;
        if (Find(Key) != null)
        {
            throw Exception.Create("ID already in HashTable");
        }

        Assert(Item != null, "Cannot add nil item to hash table", "HashTab.PAS", 175);
        H.Key = 0;
        H.Item = Item;
        H.Key = Key;
        base.Add(H);
    }

    // HashTab.PAS:182
    public void Delete(string Key)
    {
        int I;
        I = FCount;
        if (I != 0)
        {
            do
            {
                I -= 1;
                if (AnsiCompareStr(FItems[I].Key.As<string>(), Key) == 0)
                {
                    FItems[I].Key = "";
                    base.Delete(I);
                    return;
                }
            }
            while (!(I == 0));
        }
    }
}

// the abstract table interface
public partial class TStringHashTable : TObject
{
    // Engine.pas:804
    public virtual void Add(object Item, string Key)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:805
    public virtual void Delete(string Key)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:806
    public virtual object Find(string Key)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // Engine.pas:807
    public virtual void Clear()
    {
        throw EAbstractError.Create("Abstract Error");
    }
}

// 256 lists selected by a hash of the first and the second-to-last character and the length
public partial class TStringHashTable256 : TStringHashTable
{
    internal FixedArray_Aow_Units_Engine_TStringHashList_256 FLists;
    // HashTab.PAS:199
    public TStringHashTable256()
    {
        for (int I = 0; I <= 255; I++)
        {
            var stringHashList = new TStringHashList();
            FLists[I] = stringHashList;
        }
    }

    // HashTab.PAS:207
    public override void Destroy()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Free();
        }

        base.Destroy();
    }

    // HashTab.PAS:215
    public override void Clear()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Clear();
        }
    }

    // HashTab.PAS:222
    public override void Add(object Item, string Key)
    {
        int H;
        H = Key.Length;
        if (H >= 2)
        {
            H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
        }
        else
        {
            if (H == 1)
            {
                H = Cp1252.Byte(Key[0]);
            }
        }

        FLists[H].Add(Item, Key);
    }

    // HashTab.PAS:234
    public override void Delete(string Key)
    {
        int H;
        H = Key.Length;
        if (H >= 2)
        {
            H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
        }
        else
        {
            if (H == 1)
            {
                H = Cp1252.Byte(Key[0]);
            }
        }

        FLists[H].Delete(Key);
    }

    // HashTab.PAS:246
    public override object Find(string Key)
    {
        int H;
        H = Key.Length;
        if (H >= 2)
        {
            H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
        }
        else
        {
            if (H == 1)
            {
                H = Cp1252.Byte(Key[0]);
            }
        }

        return FLists[H].Find(Key);
    }

    // Engine.pas:816

}

// FControlStyle
public partial class TEControlStyleObject : TEObject
{
    // Engine.pas:8228
    public new const int ClassID = 81;

    internal TSet<TEControlStyleElement> FControlStyle;
    // Engine.pas:8222
    public TEControlStyleObject(TECustomNode owner) : this(null, owner) { }

    // Engine.pas:8222
    private protected TEControlStyleObject(ConstructionContext context, TECustomNode owner) : base(context, owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    // Engine.pas:8233
    public override TSet<TEControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:8238
    public void SetControlStyle(TSet<TEControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:8243
    public override void ReadWrite(TEStorageStream Stream)
    {
        RwSet(ref FControlStyle, 1, 1, Stream.rwData, 0);
    }

}

// FControlStyle
public partial class TEControlStyleNode : TENode
{
    // Engine.pas:8199
    public new const int ClassID = 257;

    public TEControlStyleNode(TECustomNode owner) : base(owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    internal TSet<TEControlStyleElement> FControlStyle;

    // Engine.pas:8204
    public override TSet<TEControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:8209
    public void SetControlStyle(TSet<TEControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:8214
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
    }

}

// FEngine, FControlStyle. A node that knows its engine: message $20000008
// (TEngine.ActivateEObject) carries the engine in the message
public partial class TECustomOwner : TENode
{
    // Engine.pas:8257
    public new const int ClassID = 259;

    // Engine.pas:8250
    public TECustomOwner(TECustomNode owner) : this(null, owner) { }

    private protected TECustomOwner(ConstructionContext context, TECustomNode owner) : base(context, owner)
    {
        FOwner = owner;
        FControlStyle = base.GetControlStyle();
    }

    internal TEngine FEngine;
    internal TSet<TEControlStyleElement> FControlStyle;
    // Engine.pas:8262
    public override TEngine GetEngine() => FEngine;

    // Engine.pas:8267
    public void SetEngine(TEngine AEngine)
    {
        FEngine = AEngine;
    }

    // Engine.pas:8272
    public override TSet<TEControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:8277
    public void SetControlStyle(TSet<TEControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:8282
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
    }

    // Engine.pas:8288
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        if (Msg.MsgID == 536870920)
        {
            FEngine = Msg.Payload.As<TEngine>();
        }

        base.MsgProc(ref Msg, Sender);
    }

    // Engine.pas:855
}

// a TList of class references
public partial class TEObjectClassList : TList
{
    // Engine.pas:8296
    protected internal EClass GetEClass(int Index) => (EClass)this[Index];

    // Engine.pas:8301
    public EClass FindClass(int AClassID)
    {
        EClass Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = AClassID == ((EClass)this[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = (EClass)this[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8316
    public int FindClassIndex(int AClassID)
    {
        int Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = AClassID == ((EClass)this[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = I - 1;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:8331
    public EClass GetClass(int AClassID)
    {
        EClass Result = FindClass(AClassID);
        if (Result == null)
        {
            throw Exception.Create("Unregistered E-ClassID found (" + IntToStr(AClassID) + ")");
        }

        return Result;
    }

    // Engine.pas:8338
    public void RegisterClass(EClass AClass)
    {
        if (FindClass(AClass.ClassID) == null && AClass.ClassID != 0)
        {
            Add(AClass);
        }
        else
        {
            throw Exception.Create("EObject Class ID double defined!");
        }
    }

    // Engine.pas:8346
    public void UnregisterClass(EClass AClass)
    {
        int I;
        I = FindClassIndex(AClass.ClassID);
        if (I != -1)
        {
            Delete(I);
        }
    }

    public sealed class EClassesAccessor(TEObjectClassList target)
    {
        public EClass this[int Index] { get => target.GetEClass(Index); }
    }

    public EClassesAccessor EClasses => new EClassesAccessor(this);
}

// FEditors, editor names with ids
public partial class TEResourceEditSettings : TEControlStyleObject
{
    // Engine.pas:8662
    public new const int ClassID = 1026;

    internal TStringList FEditors;
    // Engine.pas:8677
    internal int GetUniqueEditID()
    {
        int I;
        bool Found;
        int Result = 1;
        I = 0;
        Found = FEditors.Count == 0;
        while (!Found)
        {
            if (Result != ObjectToInt(FEditors.Objects[I]))
            {
                I += 1;
                Found = I == FEditors.Count;
            }
            else
            {
                I = 0;
                Result += 1;
            }
        }

        return Result;
    }

    // Engine.pas:8650
    public TEResourceEditSettings(TECustomNode owner) : base(owner)
    {
        FEditors = new TStringList();
    }

    // Engine.pas:8656
    public override void Destroy()
    {
        FEditors.Free();
        base.Destroy();
    }

    // Engine.pas:8667
    public string GetUniqueEditorName(string Name)
    {
        string S;
        S = Name;
        while (FEditors.IndexOf(S) != -1)
        {
            S = Name + IntToStr(Random(268435455));
        }

        return S;
    }

    // Engine.pas:8700
    public int GetEditID(string Name)
    {
        int Result;
        int I;
        I = FEditors.IndexOf(Name);
        if (I != -1)
        {
            Result = ObjectToInt(FEditors.Objects[I]);
        }
        else
        {
            Result = GetUniqueEditID();
            FEditors.AddObject(Name, IntToObject(Result));
        }

        return Result;
    }

    // Engine.pas:8714
    public int RemoveEditor(string Name)
    {
        int Result = FEditors.IndexOf(Name);
        if (Result != -1)
        {
            FEditors.Delete(Result);
        }

        return Result;
    }

    // Engine.pas:8720
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineUnit.smText))
        {
            Stream.rwStringsEx(FEditors, 3);
        }
    }

    public TStringList Editors { get => FEditors; }

}

public struct TSortItem
{
    public int SortValue;
    public TEObject Child;
}

public partial class TESortNode : TECustomNode
{
    // Engine.pas:1478
    public new const int ClassID = 260;

    public TESortNode(TECustomNode owner) : base(owner)
    {
        FItems = [];
    }

    protected internal bool FSorted;
    protected internal int FCount;
    protected internal int FCapacity;
    protected internal TSortItem[] FItems = [];
    // Engine.pas:1513
    protected internal void Resize(int NewCapacity)
    {
        Array.Resize(ref FItems, NewCapacity);
        FCapacity = NewCapacity;
        if (FCapacity < FCount)
        {
            FCount = FCapacity;
        }
    }

    // Engine.pas:1498
    internal int GetSortValue(int Index) => FItems[Index].SortValue;

    // Engine.pas:1503
    internal void SetSortValue(int Index, int Value)
    {
        FItems[Index].SortValue = Value;
    }

    // Engine.pas:1472
    public override void Destroy()
    {
        base.Destroy();
        Resize(0);
    }

    // Engine.pas:1483
    public override void QuickClear()
    {
        Resize(0);
    }

    // Engine.pas:1488
    public override TEObject GetChild(int Index) => FItems[Index].Child;

    // Engine.pas:1493
    public override void SetChild(int Index, TEObject Child)
    {
        FItems[Index].Child = Child;
    }

    // Engine.pas:1508
    public override int GetCount() => FCount;

    // Engine.pas:1520
    public virtual void Grow()
    {
        Resize(FCapacity + 2);
    }

    // Engine.pas:1525
    public override int RemoveChild(TEObject Child)
    {
        int Result = FCount - 1;
        while (Result >= 0 && Child != FItems[Result].Child)
        {
            Result -= 1;
        }

        if (Result >= 0)
        {
            FCount -= 1;
            if (Result < FCount)
            {
                Array.Copy(FItems, Result + 1, FItems, Result, FCount - Result);
            }
        }

        return Result;
    }

    // Engine.pas:1537
    public override int AddChild(TEObject Child) => AddSortChild(Child, 0);

    // Engine.pas:1542
    public virtual int AddSortChild(TEObject Child, int SortValue)
    {
        int Result;
        int I;
        if (FCount >= FCapacity)
        {
            Grow();
        }

        if (FSorted)
        {
            I = FCount;
            while (I > 0 && SortValue > FItems[I - 1].SortValue)
            {
                I -= 1;
            }

            if (I != FCount)
            {
                Array.Copy(FItems, I, FItems, I + 1, FCount - I);
            }

            FItems[I].Child = Child;
            FItems[I].SortValue = SortValue;
            Result = I;
        }
        else
        {
            FItems[FCount].Child = Child;
            FItems[FCount].SortValue = SortValue;
            Result = FCount;
        }

        FCount += 1;
        return Result;
    }

    // Engine.pas:1565
    public override void ReadWrite(TEStorageStream Stream)
    {
        throw Exception.Create("Read/Write not supported (TESortNode)");
    }

    // Engine.pas:1596
    public virtual void Sort()
    {
        if (!FSorted && FCount > 0)
        {
            EngineUnit.SortItems(FItems, 0, FCount - 1);
        }

        FSorted = true;
    }

    public sealed class SortValueAccessor(TESortNode target)
    {
        public int this[int Index] { get => target.GetSortValue(Index); set => target.SetSortValue(Index, value); }
    }

    public SortValueAccessor SortValue => new SortValueAccessor(this);
    public bool Sorted { get => FSorted; }

    // Engine.pas:922


}

// { Engine.TEInverseSortNode: the same, sorted the other way }
public partial class TEInverseSortNode : TESortNode
{
    public new const int ClassID = 262;

    public TEInverseSortNode(TECustomNode owner) : base(owner)
    { }

    // Engine.pas:1635
    public override int AddSortChild(TEObject Child, int SortValue)
    {
        int Result;
        int I;
        if (FCount >= FCapacity)
        {
            Grow();
        }

        if (FSorted)
        {
            I = FCount;
            while (I > 0 && SortValue < FItems[I - 1].SortValue)
            {
                I -= 1;
            }

            if (I != FCount)
            {
                Array.Copy(FItems, I, FItems, I + 1, FCount - I);
            }

            FItems[I].Child = Child;
            FItems[I].SortValue = SortValue;
            Result = I;
        }
        else
        {
            FItems[FCount].Child = Child;
            FItems[FCount].SortValue = SortValue;
            Result = FCount;
        }

        FCount += 1;
        return Result;
    }

    // Engine.pas:1658
    public override void Sort()
    {
        if (!FSorted && FCount > 0)
        {
            EngineUnit.InverseSortItems(FItems, 0, FCount - 1);
        }

        FSorted = true;
    }

}

// FControlStyle
public partial class TEControlStyleSortNode : TESortNode
{
    // Engine.pas:1672
    public new const int ClassID = 261;

    internal TSet<TEControlStyleElement> FControlStyle;
    // Engine.pas:1666
    public TEControlStyleSortNode(TECustomNode owner) : base(owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    // Engine.pas:1677
    public override TSet<TEControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:1682
    public void SetControlStyle(TSet<TEControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

}

// FControlStyle
public partial class TEControlStyleInverseSortNode : TEInverseSortNode
{
    // Engine.pas:1695
    public new const int ClassID = 263;

    public TEControlStyleInverseSortNode(TECustomNode owner) : base(owner)
    {
        FControlStyle = base.GetControlStyle();
    }

    internal TSet<TEControlStyleElement> FControlStyle;

    // Engine.pas:1700
    public override TSet<TEControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:1705
    public void SetControlStyle(TSet<TEControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:963


}

// a TENode that holds references to objects owned elsewhere (AddRef on AddChild, Release on
// removal)
public partial class TEReferenceNode : TENode
{

    public TEReferenceNode(TECustomNode owner) : base(owner)
    { }

    // Engine.pas:8144
    public override void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((TEObject)FChildren.List[I]).Release();
        }

        FChildren.Clear();
    }

    // Engine.pas:8157
    public override void ReadWriteChildren(TEStorageStream Stream)
    {
        if (Stream.StorageMode().Contains(TStorageModeElement.smWrite))
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                if (((TEObject)FChildren.List[I]).FOwner == null)
                {
                    ((TEObject)FChildren.List[I]).FOwner = this;
                }
            }

            base.ReadWriteChildren(Stream);
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                if (((TEObject)FChildren.List[I]).FOwner == this)
                {
                    ((TEObject)FChildren.List[I]).FOwner = null;
                }
            }
        }
        else
        {
            base.ReadWriteChildren(Stream);
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
            {
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    ((TEObject)FChildren.List[I]).Release();
                }
            }
        }
    }

    // Engine.pas:8152
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // Engine.pas:8177
    public override int RemoveChild(TEObject Child)
    {
        int Result = FChildren.Remove(Child);
        if (Result != -1)
        {
            Child.Release();
        }

        return Result;
    }

    // Engine.pas:8183
    public override int AddChild(TEObject Child)
    {
        if (Child.FOwner == this)
        {
            Child.FOwner = null;
        }

        FChildren.Add(Child);
        if (Child.AddRef() == -1)
        {
            throw Exception.Create("EObject doesn't support ReferenceCounting");
        }

        return FChildren.Count - 1;
    }

    // Engine.pas:631

}

// FNode, the node the user is connected to
public partial class TEUser : TEObject
{
    // Engine.pas:2430
    public TEUser(TECustomNode owner) : base(owner)
    { }

    internal TECustomNode FNode;
    // Engine.pas:7112
    // { Original source line layout for the HashTab assertions.
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// }
    public override void Destroy()
    {
        Disconnect();
        base.Destroy();
    }

    // Engine.pas:7118
    public void Connect(TECustomNode Node)
    {
        Disconnect();
        FNode = Node;
        if (Node != null)
        {
            FNode.AddChild(this);
        }
    }

    // Engine.pas:7125
    public void Disconnect()
    {
        if (FNode != null)
        {
            FNode.RemoveChild(this);
            FNode = null;
        }
    }

    public TECustomNode Node { get => FNode; }
}

// resources with unique ids
public partial class TEResourceList : TECustomOwner
{
    public new const int ClassID = 1025;

    public TEResourceList(TECustomNode owner) : base(owner)
    { }

    // Engine.pas:8573
    protected internal TEResource GetResource(int Index) => (TEResource)FChildren.List[Index];

    // Engine.pas:8578
    public virtual void ResourceChanged()
    {
    }

    // Engine.pas:8582
    public TEResource FindResource(int ID)
    {
        TEResource Result;
        int I;
        bool Found;
        Found = false;
        I = 0;
        while (!Found && I != this.FChildren.Count)
        {
            Found = this.FChildren.List[I] != null && ID == ((TEResource)this.FChildren.List[I]).FResourceID;
            I += 1;
        }

        if (Found)
        {
            Result = (TEResource)this.FChildren.List[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8600
    public void ValidateResourceID(int Index)
    {
        int ID;
        int NewID;
        int Tries;
        bool Found;
        if (Index < FChildren.Count)
        {
            ID = GetResource(Index).FResourceID;
            var resource = GetResource(Index);
            resource.FResourceID = resource.FResourceID ^ 65535;

            if (FindResource(ID) != null)
            {
                Tries = 10;
                Found = false;
                NewID = 0;
                while (Tries > 0 && !Found)
                {
                    NewID = Random(268435455);
                    Found = FindResource(NewID) == null;
                    Tries -= 1;
                }

                while (!Found)
                {
                    NewID = Tries;
                    Found = FindResource(NewID) == null;
                    Tries += 1;
                }

                GetResource(Index).FResourceID = NewID;
            }
            else
            {
                GetResource(Index).FResourceID = ID;
            }
        }
    }

    public sealed class ResourcesAccessor(TEResourceList target)
    {
        public TEResource this[int Index] { get => target.GetResource(Index); }
    }

    public ResourcesAccessor Resources => new ResourcesAccessor(this);

}

// FOnRegisterClasses (called from Loaded), FOnUnRegisterClasses (called from Destroy), FEngine,
// the engine the component owns and frees. Descendants such as HSEngine's THexagonSpriteEngine
// publish the two events
public partial class TEComponent : TComponent
{
    internal TNotifyEvent FOnRegisterClasses;
    internal TNotifyEvent FOnUnRegisterClasses;
    internal TEngine FEngine;
    // Engine.pas:8642
    public override void Loaded()
    {
        base.Loaded();
        if (FOnRegisterClasses != null)
        {
            FOnRegisterClasses(this);
        }
    }

    protected internal TNotifyEvent OnRegisterClasses { get => FOnRegisterClasses; set => FOnRegisterClasses = value; }
    protected internal TNotifyEvent OnUnRegisterClasses { get => FOnUnRegisterClasses; set => FOnUnRegisterClasses = value; }

    // Engine.pas:8635
    public override void Destroy()
    {
        if (FOnUnRegisterClasses != null)
        {
            FOnUnRegisterClasses(this);
        }

        FEngine.Free();
        base.Destroy();
    }

    public TEngine Engine { get => FEngine; set => FEngine = value; }

    public TEComponent(TComponent owner) : base(owner)
        { }
}

public struct TCallBackEvent
{
    public TNotifyEvent Event;
    public int Data;
}

public partial class TCallBackEventList : TObject
{
    internal List<TCallBackEvent> FList = [];
    // Engine.pas:5864
    internal int GetCount() => FList.Count;

    // Engine.pas:5869
    internal ref TCallBackEvent GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

    // Engine.pas:5846
    public TCallBackEventList()
    {
        FList = new List<TCallBackEvent>();
    }

    // Engine.pas:5852
    public override void Destroy()
    {
        Clear();
        base.Destroy();
    }

    // Engine.pas:5859
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // Engine.pas:5874
    public void Add(in TCallBackEvent Event)
    {
        FList.Add(Event);
    }

    // Engine.pas:5883
    public void Delete(int Index)
    {
        if (Index < FList.Count)
        {
            FList.RemoveAt(Index);
        }
    }

    public int Count { get => GetCount(); }

    public ref TCallBackEvent this[int Index] => ref GetItem(Index);
}

// { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
public struct TEngineDebugInfo
{
    public int ID;
    private string text;
    public string Text { readonly get => text ?? ""; set => text = value; }
}

// FPath, FSize, FSections, FText, FValues. The text form of the storage: "[:path:]" lines open
// the section of an object (its path is the ids of its parents joined with '\'), "[id]=value"
// lines are its properties. the section is split into FValues with the property table pointing
// at them
public partial class TEReadTextStorageStream : TEStorageStream
{
    internal string FPath = "";
    internal int FSize;
    internal TStringList FSections;
    internal TStringList FText;
    internal TStringList FValues;
    // Engine.pas:10632
    internal void ReadStreamText()
    {
        string Line;
        TStringList Text;
        int P = 0;
        int Start;
        int L;
        Text = FText;
        FSections.Clear();
        Text.BeginUpdate();
        try
        {
            if (FSize < 0) throw new ERangeError("Negative string length");
            byte[] buffer = new byte[FSize];
            FStream.Read(buffer);
            Text.Clear();
            ReadOnlySpan<byte> text = buffer;
            if (!text.IsEmpty)
            {
                while ((P < text.Length ? text[P] : (byte)0) != 0)
                {
                    Start = P;
                    while (!TSet<byte>.WithBounds(0, 255).With(0).With(10).With(13).Contains(P < text.Length ? text[P] : (byte)0))
                    {
                        P += 1;
                    }

                    Line = Cp1252.Decode(text[Start..P].ToArray());
                    if (!IsEmpty(Line))
                    {
                        L = Line.Length;
                        if (L >= 4 && Copy(Line, 1, 2) == "[:" && Copy(Line, L - 1, 2) == ":]")
                        {
                            var count = Text.Count;
                            FSections.AddObject(Copy(Line, 3, L - 4), IntToObject(count));
                        }
                        else
                        {
                            Text.Add(Line);
                        }
                    }

                    if ((P < text.Length ? text[P] : (byte)0) == 13)
                    {
                        P += 1;
                    }

                    if ((P < text.Length ? text[P] : (byte)0) == 10)
                    {
                        P += 1;
                    }
                }
            }
        }
        finally
        {
            Text.EndUpdate();
        }
    }

    // Engine.pas:10694
    internal void ReadTextBuffer()
    {
        int Index;
        int First;
        int Last;
        int I;
        int ID;
        int NewID;
        string Value;
        FValues.Clear();
        Index = FSections.IndexOf(FPath);
        if (Index != -1)
        {
            First = ObjectToInt(FSections.Objects[Index]);
            if (Index != FSections.Count - 1)
            {
                Last = ObjectToInt(FSections.Objects[Index + 1]) - 1;
            }
            else
            {
                Last = FText.Count - 1;
            }

            I = First;
            ID = -1;
            Value = "";
            while (I <= Last)
            {
                NewID = EngineUnit.GetLineID(FText[I]);
                if (ID != -1)
                {
                    if (NewID != ID && NewID != -1)
                    {
                        var count = FValues.Count;
                        FPropertyTable.Add(ID, count);
                        FValues.Add(Value);
                        ID = NewID;
                        Value = EngineUnit.GetLineValue(FText[I]);
                    }
                    else
                    {
                        var item = FText[I];
                        Value = Value + "\r" + "\n" + item;
                    }
                }
                else
                {
                    if (NewID != -1)
                    {
                        if (ID == -1)
                        {
                            ID = NewID;
                        }

                        Value = EngineUnit.GetLineValue(FText[I]);
                    }
                }

                I += 1;
            }

            if (ID != -1)
            {
                var count = FValues.Count;
                FPropertyTable.Add(ID, count);
                FValues.Add(Value);
            }
        }
    }

    // Engine.pas:10611
    public TEReadTextStorageStream(TStream AStream, TEngine AEngine, int ASize) : base(AStream, AEngine)
    {
        FPath = "";
        if (ASize <= 0)
        {
            FSize = FStream.Size - FStream.Position;
        }
        else
        {
            FSize = ASize;
        }

        FValues = new TStringList();
        FPropertyTable = new TPropertyTable();
        FText = new TStringList();
        FSections = new TStringList();
        ReadStreamText();
        ReadTextBuffer();
    }

    // Engine.pas:10624
    public override void Destroy()
    {
        FValues.Free();
        FPropertyTable.Free();
        FSections.Free();
        FText.Free();
    }

    // Engine.pas:10669
    public override TSet<TStorageModeElement> StorageMode() => TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smText);

    // Engine.pas:10741
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        TEObject O;
        O = Obj;
        if (O != null)
        {
            rwEObject(O, ID);
        }
    }

    // Engine.pas:10749
    public override void rwEObject(TEObject Obj, int ID)
    {
        TStringList SavedValues;
        string SavedPath;
        bool Continue;
        SavedPath = FPath;
        if (!IsEmpty(FPath))
        {
            FPath = FPath + "\\" + IntToStr(ID);
        }
        else
        {
            FPath = IntToStr(ID);
        }

        SavedValues = FValues;
        FPropertyTable.BackupSettings();
        FValues = new TStringList();
        try
        {
            ReadTextBuffer();
            Obj.MainReadWrite(this);
            FValues.Free();
            FPropertyTable.RestoreSettings();
            FValues = SavedValues;
            FPath = SavedPath;
        }
        catch
        {
            FValues.Free();
            FPropertyTable.RestoreSettings();
            FValues = SavedValues;
            FPath = SavedPath;
            Continue = true;
            if (FOnError != null)
            {
                var classID = Obj.EClass.ClassID;
                FOnError(this, "Error reading object (cid:" + IntToStr(classID) + ")", ref Continue);
            }

            if (!Continue)
            {
                throw;
            }
        }
    }

    // Engine.pas:10779
    public override void rwEObjectStream(TEObject Obj, int ID)
    {
        rwEObject(Obj, ID);
    }

    // Engine.pas:10784
    public override void rwString(ref string S, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            S = FValues[Offset];
        }
        else
        {
            S = "";
        }
    }

    // Engine.pas:10792
    public override void rwStrings(TStrings Strings, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            Strings.Text = NullTerminatedText(FValues[Offset]);
        }
        else
        {
            Strings.Clear();
        }
    }

    // Engine.pas:10800
    public override void rwStringsEx(TStrings Strings, int ID)
    {
        rwStrings(Strings, ID);
    }

    // Engine.pas:10805
    public override void rwChildren(TECustomNode Node, int ID)
    {
        TEObject C;
        // { the original saves the path and never restores it }
        if (!IsEmpty(FPath))
        {
            FPath = FPath + "\\" + IntToStr(ID);
        }
        else
        {
            FPath = IntToStr(ID);
        }

        for (int I = 0, last = Node.Count - 1; I <= last; I++)
        {
            C = Node.Children[I];
            if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineUnit.csShared) && C.GetControlStyle().Contains(EngineUnit.csStored))
            {
                rwEObject(C, I);
            }
        }
    }

}

// FPath, FBuffer. Each object's properties are collected in FBuffer and written behind the
// "[:path:]" line
public partial class TEWriteTextStorageStream : TEStorageStream
{
    internal string FPath = "";
    protected internal TEBufferStream FBuffer;
    // Engine.pas:10355
    internal void WriteTextBuffer(TStream Stream)
    {
        string S;
        if (FPropertyTable.Count != 0 && FBuffer.Size != 0)
        {
            S = "[:" + FPath + ":]" + "\r" + "\n";
            if (!IsEmpty(FPath))
            {
                S = "\r\n" + S;
            }

            Stream.Write(Cp1252.Encode(S).AsSpan().Slice(1 - 1, S.Length));
            FBuffer.SaveToStream(Stream);
        }
    }

    // Engine.pas:10335
    public TEWriteTextStorageStream(TStream AStream, TEngine AEngine) : base(AStream, AEngine)
    {
        FPath = "";
        FBuffer = new TEBufferStream();
        FPropertyTable = new TPropertyTable();
    }

    // Engine.pas:10343
    public override void Destroy()
    {
        WriteTextBuffer(FStream);
        FBuffer.Free();
        FPropertyTable.Free();
    }

    // Engine.pas:10350
    public override TSet<TStorageModeElement> StorageMode() => TSet<TStorageModeElement>.Of(TStorageModeElement.smWrite, EngineUnit.smText);

    // Engine.pas:10368
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        TEObject O;
        O = Obj;
        if (O != null)
        {
            rwEObject(O, ID);
        }
    }

    // Engine.pas:10376
    public override void rwEObject(TEObject Obj, int ID)
    {
        TEBufferStream SavedBuffer;
        string SavedPath;
        bool Continue;
        if (!FPropertyTable.PropertyExist(ID))
        {
            SavedBuffer = FBuffer;
            SavedPath = FPath;
            FPropertyTable.BackupSettings();
            FBuffer = new TEBufferStream();
            if (!IsEmpty(FPath))
            {
                FPath = FPath + "\\" + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            try
            {
                Obj.MainReadWrite(this);
                WriteTextBuffer(FStream);
                FBuffer.Free();
                FPropertyTable.RestoreSettings();
                FBuffer = SavedBuffer;
                FPath = SavedPath;
                FPropertyTable.Add(ID, 0);
            }
            catch
            {
                FBuffer.Free();
                FBuffer = SavedBuffer;
                FPropertyTable.RestoreSettings();
                FPath = SavedPath;
                Continue = true;
                if (FOnError != null)
                {
                    var classID = Obj.EClass.ClassID;
                    FOnError(this, "Error writing object (cid:" + IntToStr(classID) + ")", ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found");
        }
    }

    // Engine.pas:10412
    public override void rwEObjectStream(TEObject Obj, int ID)
    {
        rwEObject(Obj, ID);
    }

    // Engine.pas:10417
    public override void rwString(ref string S, int ID)
    {
        string Line;
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (!IsEmpty(S))
            {
                Line = "[" + IntToStr(ID) + "]=" + S + "\r" + "\n";
                FBuffer.Write(Cp1252.Encode(Line).AsSpan().Slice(1 - 1, Line.Length));
                FPropertyTable.Add(ID, 0);
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found");
        }
    }

    // Engine.pas:10434
    public override void rwStrings(TStrings Strings, int ID)
    {
        string Line;
        string P;
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                P = Strings.Text;
                try
                {
                    Line = "[" + IntToStr(ID) + "]=";
                    Line += NullTerminatedText(P);

                    FBuffer.Write(Cp1252.Encode(Line).AsSpan().Slice(1 - 1, Line.Length));
                    FPropertyTable.Add(ID, 0);
                }
                finally
                {
                }
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found");
        }
    }

    // Engine.pas:10458
    public override void rwStringsEx(TStrings Strings, int ID)
    {
        rwStrings(Strings, ID);
    }

    // Engine.pas:10463
    public override void rwChildren(TECustomNode Node, int ID)
    {
        string SavedPath;
        TEObject C;
        if (!FPropertyTable.PropertyExist(ID))
        {
            SavedPath = FPath;
            if (!IsEmpty(FPath))
            {
                FPath = FPath + "\\" + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineUnit.csShared) && C.GetControlStyle().Contains(EngineUnit.csStored))
                {
                    rwEObject(C, I);
                }
            }

            FPath = SavedPath;
            FPropertyTable.Add(ID, 0);
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found");
        }
    }

}

// FPartial, FDataBase. The constructor reads the root property table. every property is found
// through the table and read at FDataBase + offset
public partial class TEReadStorageStream : TEStorageStream
{
    internal bool FPartial;
    internal int FDataBase;
    // Engine.pas:9968
    public TEReadStorageStream(TStream AStream, TEngine AEngine, bool APartial) : base(AStream, AEngine)
    {
        FPropertyTable = new TPropertyTable();
        FPropertyTable.LoadFromStream(AStream);
        FDataBase = FStream.Position;
        FPartial = APartial;
    }

    // Engine.pas:9977
    public override void Destroy()
    {
        FPropertyTable.Free();
        base.Destroy();
    }

    // Engine.pas:9983
    public override TSet<TStorageModeElement> StorageMode()
    {
        TSet<TStorageModeElement> Result;
        if (FPartial)
        {
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smPartial);
        }
        else
        {
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead);
        }

        return Result;
    }

    // Engine.pas:9991
    public override void rwInteger(ref int I, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref I, 4));
        }
        else
        {
            I = 0;
        }
    }

    // Engine.pas:10005
    public override void rwByte(ref byte B, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref B, 1));
        }
        else
        {
            B = 0;
        }
    }

    // Engine.pas:10019
    public override void rwShortint(ref sbyte S, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref S, 1));
        }
        else
        {
            S = 0;
        }
    }

    // Engine.pas:10033
    public override void rwWord(ref ushort W, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref W, 2));
        }
        else
        {
            W = 0;
        }
    }

    // Engine.pas:10047
    public override void rwIntegerEx(ref int I, int Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref I, 4));
        }
        else
        {
            I = Default;
        }
    }

    // Engine.pas:10061
    public override void rwByteEx(ref byte B, byte Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref B, 1));
        }
        else
        {
            B = Default;
        }
    }

    // Engine.pas:10075
    public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref S, 1));
        }
        else
        {
            S = Default;
        }
    }

    // Engine.pas:10089
    public override void rwWordEx(ref ushort W, ushort Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref W, 2));
        }
        else
        {
            W = Default;
        }
    }

    // Engine.pas:10103
    public override void rwString(ref string S, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            EngineUnit.LoadStringFromStream(ref S, FStream);
        }
        else
        {
            S = "";
        }
    }

    // Engine.pas:10117
    public override void rwStrings(TStrings Strings, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            EngineUnit.StringsFromStream(Strings, FStream);
        }
        else
        {
            Strings.Clear();
        }
    }

    // Engine.pas:10131
    public override void rwStringsEx(TStrings Strings, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            EngineUnit.StringsFromStreamEx(Strings, FStream);
        }
        else
        {
            Strings.Clear();
        }
    }

    // Engine.pas:10145
    public override void rwData(Span<byte> Buffer, int Size, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Buffer.Slice(0, Size));
        }
    }

    // Engine.pas:10157
    public override void rwStreamProc(int ID, TStreamProc LoadProc, TStreamProc SaveProc)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            try
            {
                LoadProc(FStream);
            }
            catch
            {
            }
        }
    }

    // Engine.pas:10172
    public override TStream OpenPropertyStream(int ID)
    {
        TStream Result;
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            Result = FStream;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:10186
    public override void rwBool(ref bool B, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        B = Offset != -1;
    }

    // Engine.pas:10191
    public bool HandleException(Exception E)
    {
        bool Continue;
        if (!FErrorHandled)
        {
            Continue = true;
            if (FOnError != null)
            {
                if (IsEmpty(E.Message))
                {
                    FOnError(this, EngineUnit.EStreamReadErrorStr, ref Continue);
                }
                else
                {
                    FOnError(this, E.Message, ref Continue);
                }
            }

            if (!Continue)
            {
                FErrorHandled = true;
            }
        }

        return !FErrorHandled;
    }

    // Engine.pas:10208
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        int Offset;
        int SavedBase;
        int AClassID = default;
        Obj = null;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref AClassID, 4));
            if (AClassID != 0)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                FPropertyTable.LoadFromStream(FStream);
                FDataBase = FStream.Position;
                try
                {
                    Obj = FEngine.CreateEObject(AClassID, null);
                    Obj.FOwner = AOwner;
                    Obj.MainReadWrite(this);
                    Obj.FOwner = null;
                    Obj.SetOwner(AOwner);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                }
                catch (System.Exception error)
                {
                    var E = Wrap(error);
                    Obj.Free();
                    Obj = null;
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
        }
    }

    // Engine.pas:10246
    public override void rwChildren(TECustomNode Node, int ID)
    {
        int Offset;
        int SavedBase;
        int ChildID;
        TEObject Child = default;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            SavedBase = FDataBase;
            FPropertyTable.BackupSettings();
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FPropertyTable.LoadFromStream(FStream);
            FDataBase = FStream.Position;
            if (Node.GetControlStyle().Contains(EngineUnit.csKeepIndex))
            {
                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    ChildID = FPropertyTable.Items[I].ID;
                    rwCreateEObject(ref Child, Node, ChildID);
                    if (Child != null)
                    {
                        Child.SetArrayOwner(Node, ChildID);
                    }
                }
            }
            else
            {
                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    ChildID = FPropertyTable.Items[I].ID;
                    rwCreateEObject(ref Child, Node, ChildID);
                }
            }

            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
        }
    }

    // Engine.pas:10277
    public override void rwEObject(TEObject Obj, int ID)
    {
        int Offset;
        int SavedBase;
        SavedBase = FDataBase;
        Offset = FPropertyTable.FindOffset(ID);
        FPropertyTable.BackupSettings();
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FPropertyTable.LoadFromStream(FStream);
            FDataBase = FStream.Position;
        }

        try
        {
            Obj.MainReadWrite(this);
            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
        }
        catch (System.Exception error)
        {
            var E = Wrap(error);
            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
            if (!HandleException(E))
            {
                throw;
            }
        }
    }

    // Engine.pas:10304
    public override void rwEObjectStream(TEObject Obj, int ID)
    {
        int SavedBase;
        int Start;
        int Size = default;
        Start = FStream.Position;
        FStream.Read(Bytes.Of(ref Size, 4));
        if (Size != 0)
        {
            SavedBase = FDataBase;
            FPropertyTable.BackupSettings();
            try
            {
                FPropertyTable.LoadFromStream(FStream);
                FDataBase = FStream.Position;
                Obj.MainReadWrite(this);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                FStream.Position = Start + Size + 4;
            }
            catch (System.Exception error)
            {
                var E = Wrap(error);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                FStream.Position = Start + Size + 4;
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }
    }

    public int DataBase { get => FDataBase; }

}

// FPartial, FBuffer. Values go into the buffer. the property table records (id, buffer
// position). A nested object gets its own buffer, appended to the outer one as [table][data].
// The destructor writes the root table and buffer
public partial class TEWriteStorageStream : TEStorageStream
{

    internal bool FPartial;
    internal TEBufferStream FBuffer;
    // Engine.pas:9592
    internal void PropertyExistError(int ID)
    {
        throw new EDoublePrioriyID("PropertyID already in use (" + IntToStr(ID) + " / " + ClassNameOf(this.GetType()) + ")");
    }

    // Engine.pas:9565
    // Engine.pas:10489 TDebugWriteStream borrows the supplied stream as its buffer.
    private protected TEWriteStorageStream(TStream stream, TEngine engine, TEBufferStream buffer) : base(stream, engine)
    {
        FBuffer = buffer;
        FPropertyTable = new TPropertyTable();
        FPartial = false;
    }

    public TEWriteStorageStream(TStream AStream, TEngine AEngine, bool APartial) : base(AStream, AEngine)
    {
        FEngine = AEngine;
        FStream = AStream;
        FBuffer = new TEBufferStream();
        FPropertyTable = new TPropertyTable();
        FPartial = APartial;
    }

    // Engine.pas:9575
    public override void Destroy()
    {
        FPropertyTable.SaveToStream(FStream);
        FBuffer.SaveToStream(FStream);
        FPropertyTable.Free();
        FBuffer.Free();
        base.Destroy();
    }

    // Engine.pas:9584
    public override TSet<TStorageModeElement> StorageMode()
    {
        TSet<TStorageModeElement> Result;
        if (FPartial)
        {
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smWrite, EngineUnit.smPartial);
        }
        else
        {
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smWrite);
        }

        return Result;
    }

    // Engine.pas:9597
    public override void rwByte(ref byte B, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (B != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref B, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9611
    public override void rwWord(ref ushort W, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (W != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref W, 2));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9625
    public override void rwShortint(ref sbyte S, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (S != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref S, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9639
    public override void rwInteger(ref int I, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (I != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref I, 4));
            }
        }
        else
        {
            throw new EDoublePrioriyID("PropertyID already in use");
        }
    }

    // Engine.pas:9653
    public override void rwByteEx(ref byte B, byte Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (B != Default)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref B, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9667
    public override void rwWordEx(ref ushort W, ushort Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (W != Default)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref W, 2));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9681
    public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (S != Default)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref S, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9695
    public override void rwIntegerEx(ref int I, int Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (I != Default)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Bytes.Of(ref I, 4));
            }
        }
        else
        {
            throw new EDoublePrioriyID("PropertyID already in use");
        }
    }

    // Engine.pas:9709
    public override void rwString(ref string S, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (!IsEmpty(S))
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                EngineUnit.SaveStringToStream(S, FBuffer);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9723
    public override void rwStrings(TStrings Strings, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                EngineUnit.StringsToStream(Strings, FBuffer);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9737
    public override void rwStringsEx(TStrings Strings, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                EngineUnit.StringsToStreamEx(Strings, FBuffer);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9751
    public override void rwData(Span<byte> Buffer, int Size, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Size != 0)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                FBuffer.Write(Buffer.Slice(0, Size));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9765
    public override void rwStreamProc(int ID, TStreamProc LoadProc, TStreamProc SaveProc)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.Add(ID, FBuffer.Position);
            try
            {
                SaveProc(FBuffer);
            }
            catch
            {
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9779
    public override TStream OpenPropertyStream(int ID)
    {
        TStream Result;
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.Add(ID, FBuffer.Position);
            Result = FBuffer;
        }
        else
        {
            PropertyExistError(ID);
            Result = null;
        }

        return Result;
    }

    // Engine.pas:9793
    public override void rwBool(ref bool B, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (B)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9803
    public bool HandleException(Exception E)
    {
        bool Continue;
        if (!FErrorHandled)
        {
            Continue = true;
            if (FOnError != null)
            {
                if (IsEmpty(E.Message))
                {
                    FOnError(this, EngineUnit.EStreamWriteErrorStr, ref Continue);
                }
                else
                {
                    FOnError(this, E.Message, ref Continue);
                }
            }

            if (!Continue)
            {
                FErrorHandled = true;
            }
        }

        return !FErrorHandled;
    }

    // Engine.pas:9820
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        TEBufferStream Outer;
        int Start;
        int AClassID;
        TEObject O;
        if (!FPropertyTable.PropertyExist(ID))
        {
            Start = FBuffer.Position;
            O = Obj;
            if (O != null)
            {
                AClassID = O.EClass.ClassID;
            }
            else
            {
                AClassID = 0;
            }

            if (AClassID != 0)
            {
                Outer = FBuffer;
                FBuffer = new TEBufferStream();
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    Outer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.SaveToStream(Outer);
                    FBuffer.SaveToStream(Outer);
                    FBuffer.Free();
                    FPropertyTable.RestoreSettings();
                    FBuffer = Outer;
                    FPropertyTable.Add(ID, Start);
                }
                catch (System.Exception error)
                {
                    var E = Wrap(error);
                    FPropertyTable.RestoreSettings();
                    FBuffer.Free();
                    FBuffer = Outer;
                    Outer.Position = Start;
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
            else
            {
                FBuffer.Write(Bytes.Of(ref AClassID, 4));
                FPropertyTable.Add(ID, Start);
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:9866
    public override void rwChildren(TECustomNode Node, int ID)
    {
        TEBufferStream Outer;
        int Start;
        TEObject C = default;
        if (!FPropertyTable.PropertyExist(ID))
        {
            Start = FBuffer.Position;
            Outer = FBuffer;
            FBuffer = new TEBufferStream();
            FPropertyTable.BackupSettings();
            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineUnit.csShared) && C.GetControlStyle().Contains(EngineUnit.csStored))
                {
                    rwCreateEObject(ref C, Node, I);
                }
            }

            FPropertyTable.SaveToStream(Outer);
            FBuffer.SaveToStream(Outer);
            FBuffer.Free();
            FPropertyTable.RestoreSettings();
            FBuffer = Outer;
            FPropertyTable.Add(ID, Start);
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:9897
    public override void rwEObject(TEObject Obj, int ID)
    {
        TEBufferStream Outer;
        int Start;
        if (!FPropertyTable.PropertyExist(ID))
        {
            Start = FBuffer.Position;
            Outer = FBuffer;
            FBuffer = new TEBufferStream();
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                FPropertyTable.RestoreSettings();
                FBuffer.Free();
                FBuffer = Outer;
                FPropertyTable.Add(ID, Start);
            }
            catch (System.Exception error)
            {
                var E = Wrap(error);
                FPropertyTable.RestoreSettings();
                FBuffer.Free();
                FBuffer = Outer;
                Outer.Position = Start;
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found (" + ClassNameOf(Obj.GetType()) + ":" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:9931
    public override void rwEObjectStream(TEObject Obj, int ID)
    {
        TEBufferStream Outer;
        int Start;
        int Len = default;
        Start = FBuffer.Position;
        Outer = FBuffer;
        FBuffer = new TEBufferStream();
        FPropertyTable.BackupSettings();
        try
        {
            Obj.MainReadWrite(this);
            Outer.Write(Bytes.Of(ref Len, 4));
            FPropertyTable.SaveToStream(Outer);
            FBuffer.SaveToStream(Outer);
            Len = Outer.Position - Start - 4;
            Outer.Position = Start;
            Outer.Write(Bytes.Of(ref Len, 4));
            Outer.Position = Start + Len + 4;
            FPropertyTable.RestoreSettings();
            FBuffer.Free();
            FBuffer = Outer;
        }
        catch (System.Exception error)
        {
            var E = Wrap(error);
            FBuffer.Free();
            FBuffer = Outer;
            FPropertyTable.RestoreSettings();
            Len = 0;
            Outer.Position = Start;
            Outer.Write(Bytes.Of(ref Len, 4));
            if (!HandleException(E))
            {
                throw;
            }
        }
    }

}

// a write storage stream that writes every object's data straight into the stream (FBuffer is
// the stream itself)
public partial class TDebugWriteStream : TEWriteStorageStream
{
    // Engine.pas:10489
    // Retained: The debug writer uses the supplied stream as FBuffer and skips the normal writer buffer allocation. Retire with a replacement-base storage protocol.
    public TDebugWriteStream(TStream AStream, TEngine AEngine) : base(AStream, AEngine, (TEBufferStream)AStream)
    { }

    // Engine.pas:10499
    public override void Destroy()
    {
        FPropertyTable.SaveToStream(FStream);
        FPropertyTable.Free();
    }

    // Engine.pas:10505
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        int AClassID;
        TEObject O;
        if (!FPropertyTable.PropertyExist(ID))
        {
            O = Obj;
            if (O != null)
            {
                AClassID = O.EClass.ClassID;
            }
            else
            {
                AClassID = 0;
            }

            if (AClassID != 0)
            {
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    FBuffer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.SaveToStream(FBuffer);
                    FBuffer.Free();
                    FPropertyTable.RestoreSettings();
                    FPropertyTable.Add(ID, FBuffer.Position);
                }
                catch (System.Exception error)
                {
                    var E = Wrap(error);
                    FPropertyTable.RestoreSettings();
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
            else
            {
                FBuffer.Write(Bytes.Of(ref AClassID, 4));
                FPropertyTable.Add(ID, FBuffer.Position);
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:10542
    public override void rwChildren(TECustomNode Node, int ID)
    {
        TEObject C = default;
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.BackupSettings();
            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineUnit.csShared) && C.GetControlStyle().Contains(EngineUnit.csStored))
                {
                    rwCreateEObject(ref C, Node, I);
                }
            }

            FPropertyTable.SaveToStream(FBuffer);
            FPropertyTable.RestoreSettings();
            FPropertyTable.Add(ID, FBuffer.Position);
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:10565
    public override void rwEObject(TEObject Obj, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                FPropertyTable.SaveToStream(FBuffer);
                FPropertyTable.RestoreSettings();
                FPropertyTable.Add(ID, FBuffer.Position);
            }
            catch (System.Exception error)
            {
                var E = Wrap(error);
                FPropertyTable.RestoreSettings();
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }
        else
        {
            throw new EDoublePrioriyID("Double priorityID found (" + ClassNameOf(Obj.GetType()) + ":" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:10587
    public override void rwEObjectStream(TEObject Obj, int ID)
    {
        int Size = default;
        FPropertyTable.BackupSettings();
        try
        {
            Obj.MainReadWrite(this);
            FBuffer.Write(Bytes.Of(ref Size, 4));
            FPropertyTable.SaveToStream(FBuffer);
            FBuffer.Write(Bytes.Of(ref Size, 4));
            FPropertyTable.RestoreSettings();
        }
        catch (System.Exception error)
        {
            var E = Wrap(error);
            FPropertyTable.RestoreSettings();
            Size = 0;
            FBuffer.Write(Bytes.Of(ref Size, 4));
            if (!HandleException(E))
            {
                throw;
            }
        }
    }
}

// visits every string of an object tree through TranslateString and TranslateStrings. nothing
// is read or written
public partial class TTranslateStorageStream : TEStorageStream
{
    // Engine.pas:10825
    public TTranslateStorageStream(TEngine AEngine) : base(null, AEngine)
    {
        FPropertyTable = new TPropertyTable();
    }

    // Engine.pas:10831
    public override void Destroy()
    {
        FPropertyTable.Free();
        base.Destroy();
    }

    // Engine.pas:10837
    public override TSet<TStorageModeElement> StorageMode() => TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, EngineUnit.smText, TStorageModeElement.smTranslate);

    // Engine.pas:10842
    public override void rwString(ref string S, int ID)
    {
        TranslateString(ref S);
    }

    // Engine.pas:10847
    public override void rwStrings(TStrings Strings, int ID)
    {
        TranslateStrings(ref Strings);
    }

    // Engine.pas:10852
    public override void rwStringsEx(TStrings Strings, int ID)
    {
        TranslateStrings(ref Strings);
    }

    // Engine.pas:10857
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        TEObject O;
        O = Obj;
        if (O != null)
        {
            rwEObject(O, ID);
        }
    }

    // Engine.pas:10865
    public override void rwEObject(TEObject Obj, int ID)
    {
        bool Continue = default;
        try
        {
            FPropertyTable.BackupSettings();
            Obj.MainReadWrite(this);
            FPropertyTable.RestoreSettings();
        }
        catch
        {
            FPropertyTable.RestoreSettings();
            // { the original leaves Continue uninitialized here }
            if (FOnError != null)
            {
                var classID = Obj.EClass.ClassID;
                FOnError(this, "Error reading object (cid:" + IntToStr(classID) + ")", ref Continue);
            }

            if (!Continue)
            {
                throw;
            }
        }
    }

    // Engine.pas:10882
    public override void rwEObjectStream(TEObject Obj, int ID)
    {
        rwEObject(Obj, ID);
    }

    // Engine.pas:10887
    public override void rwChildren(TECustomNode Node, int ID)
    {
        TEObject C;
        for (int I = 0, last = Node.Count - 1; I <= last; I++)
        {
            C = Node.Children[I];
            if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineUnit.csShared) && C.GetControlStyle().Contains(EngineUnit.csStored))
            {
                rwEObject(C, I);
            }
        }
    }

    // Engine.pas:10901
    public virtual void TranslateStrings(ref TStrings Strings)
    {
    }

    // Engine.pas:10905
    public virtual void TranslateString(ref string S)
    {
    }

}

// a read stream that announces a comparison to come (smSetupCompare)
public partial class TSetupCompareStorageStream : TEReadStorageStream
{
    public TSetupCompareStorageStream(TStream AStream, TEngine AEngine, bool APartial) : base(AStream, AEngine, APartial)
    { }

    // Engine.pas:10911
    public override TSet<TStorageModeElement> StorageMode() => TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, TStorageModeElement.smSetupCompare);
}

// reads a stored copy and calls Mismatch for every property that differs from the object.
// Mismatch raises and swallows its own exception, a place for a breakpoint
public partial class TCompareStorageStream : TEReadStorageStream
{
    // Engine.pas:11002
    internal void Mismatch(int ID)
    {
        try
        {
            throw Exception.Create("Property Mismatch (PropertyID:" + IntToStr(ID) + ")");
        }
        catch
        {
        }
    }

    // Engine.pas:10918
    public TCompareStorageStream(TStream AStream, TEngine AEngine) : base(AStream, AEngine, false)
    { }

    // Engine.pas:10923
    public override void Destroy()
    {
        base.Destroy();
    }

    // Engine.pas:10928
    public override TSet<TStorageModeElement> StorageMode() => TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, TStorageModeElement.smCompare);

    // Engine.pas:10933
    public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
    {
        int Offset;
        int SavedBase;
        int AClassID = default;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1 && Obj != null)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.Read(Bytes.Of(ref AClassID, 4));
            if (AClassID != 0)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                FPropertyTable.LoadFromStream(FStream);
                FDataBase = FStream.Position;
                // { The original handles read errors by destroying and clearing Obj. }
                try
                {
                    Obj.MainReadWrite(this);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                }
                catch (System.Exception error)
                {
                    var E = Wrap(error);
                    Obj.Free();
                    Obj = null;
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
        }
    }

    // Engine.pas:10958
    public override void rwChildren(TECustomNode Node, int ID)
    {
        int SavedBase;
        int Start;
        TEObject C = default;
        int Offset;
        int Index;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            SavedBase = FDataBase;
            FPropertyTable.BackupSettings();
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FPropertyTable.LoadFromStream(FStream);
            FDataBase = FStream.Position;
            if (Node.GetControlStyle().Contains(EngineUnit.csKeepIndex))
            {
                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    Index = FPropertyTable.Items[I].ID;
                    C = Node.Children[Index];
                    if (C != null)
                    {
                        rwCreateEObject(ref C, Node, Index);
                    }
                }
            }
            else
            {
                Start = 0;
                while (Start < Node.Count && Node.Children[Start] != null && (Node.Children[Start].GetControlStyle() & TSet<TEControlStyleElement>.Of(EngineUnit.csShared)) != TSet<TEControlStyleElement>.Of())
                {
                    Start += 1;
                }

                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    Index = FPropertyTable.Items[I].ID;
                    if (Index < Node.Count)
                    {
                        C = Node.Children[I - FPropertyTable.First + Start];
                        if (C != null)
                        {
                            rwCreateEObject(ref C, Node, Index);
                        }
                    }
                }
            }

            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
        }
    }

    // Engine.pas:11010
    public override void rwData(Span<byte> Buffer, int Size, int ID)
    {
        byte[] image = new byte[Size];
        base.rwData(image, Size, ID);
        if (!image.AsSpan().SequenceEqual(Buffer[..Size])) Mismatch(ID);
    }

    // Engine.pas:11022
    public override void rwString(ref string S, int ID)
    {
        string Value = "";
        base.rwString(ref Value, ID);
        if (Value != S)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11030
    public override void rwStrings(TStrings Strings, int ID)
    {
        TStringList L;
        // { the original never frees L }
        L = new TStringList();
        base.rwStrings(L, ID);
        var text = Strings.Text;
        if (CompareText(L.Text, text) != 0)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11040
    public override void rwStringsEx(TStrings Strings, int ID)
    {
        TStringList L;
        L = new TStringList();
        base.rwStrings(L, ID);
        var text = Strings.Text;
        if (CompareText(L.Text, text) != 0)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11049
    public override void rwInteger(ref int I, int ID)
    {
        int Value = default;
        base.rwInteger(ref Value, ID);
        if (I != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11057
    public override void rwByte(ref byte B, int ID)
    {
        byte Value = default;
        base.rwByte(ref Value, ID);
        if (B != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11073
    public override void rwWord(ref ushort W, int ID)
    {
        ushort Value = default;
        base.rwWord(ref Value, ID);
        if (W != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11081
    public override void rwBool(ref bool B, int ID)
    {
        bool Value = default;
        base.rwBool(ref Value, ID);
        if (B != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11065
    public override void rwShortint(ref sbyte S, int ID)
    {
        sbyte Value = default;
        base.rwShortint(ref Value, ID);
        if (S != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11089
    public override void rwIntegerEx(ref int I, int Default, int ID)
    {
        int Value = default;
        base.rwIntegerEx(ref Value, Default, ID);
        if (I != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11097
    public override void rwByteEx(ref byte B, byte Default, int ID)
    {
        byte Value = default;
        base.rwByteEx(ref Value, Default, ID);
        if (B != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11113
    public override void rwWordEx(ref ushort W, ushort Default, int ID)
    {
        ushort Value = default;
        base.rwWordEx(ref Value, Default, ID);
        if (W != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11105
    public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
        sbyte Value = default;
        base.rwShortintEx(ref Value, Default, ID);
        if (S != Value)
        {
            Mismatch(ID);
        }
    }

}

public delegate int TStringsSortCompare(string S1, string S2, TObject O1, TObject O2);
public struct TStringHashItem
{
    private string key;
    public string Key { readonly get => key ?? ""; set => key = value; }
    public object Item;
}

public delegate void TExchangeProc(TList Data, int I, int J);
public struct TSortStringsContext
{
    public TStrings Strings;
    public TStringsSortCompare Compare;
    public TExchangeProc ExchangeProc;
    public TList ExchangeData;
}

// { the layout of TList, to write its count field straight from the object }
public partial class TListFields : TObject
{
    public object FList;
    public int FCount;
}

public static partial class EngineUnit
{
    // The Engine unit of Enginep.dpl. The hash classes come from inc\HashTab.PAS as in the
    // original
    public const int MaxListSize = 134217727;
    // { Compatibility names used by the recovered dependent packages. }
    public const TEControlStyleElement csStored = 0;
    public const TEControlStyleElement csShared = (TEControlStyleElement)1;
    public const TEControlStyleElement csReserved2 = (TEControlStyleElement)2;
    public const TEControlStyleElement csKeepIndex = (TEControlStyleElement)3;
    public const TEngineModeElement eoVerifyClasses = 0;
    public const TStorageModeElement smText = (TStorageModeElement)2;
    public const TStorageModeElement smPartial = (TStorageModeElement)3;
    // Engine.pas:1953
    // { Strings }
    public static string ReplaceText(string S, string Old, string New)
    {
        int P;
        string Result = "";
        P = Pos(Old, S);
        while (P != 0)
        {
            Result = Result + Copy(S, 1, P - 1) + New;
            S = Copy(S, P + Old.Length, S.Length - P - Old.Length + 1);
            P = Pos(Old, S);
        }

        return Result + S;
    }

    // Engine.pas:2005
    public static void SortStrings(TStrings Strings, TStringsSortCompare Compare)
    {
        TSortStringsContext Ctx = default;
        if (Strings.Count != 0)
        {
            Ctx.Strings = Strings;
            Ctx.Compare = Compare;
            Ctx.ExchangeProc = null;
            var count = Strings.Count;
            QuickSortStrings(ref Ctx, 0, count - 1);
        }
    }

    // Engine.pas:2025
    public static void SortStringsEx(TStrings Strings, TStringsSortCompare Compare, ReadOnlySpan<TStrings> LinkedStrings)
    {
        TList Lists = default;
        TSortStringsContext Ctx = default;
        if (Strings.Count != 0)
        {
            Strings.BeginUpdate();
            Ctx.Strings = Strings;
            Ctx.Compare = Compare;
            // { Original unsigned test also takes an empty array. }
            if ((uint)(LinkedStrings.Length - 1) >= 0)
            {
                Lists = new TList();
                for (int I = 0, last = LinkedStrings.Length - 1; I <= last; I++)
                {
                    LinkedStrings[I].BeginUpdate();
                    Lists.Add(LinkedStrings[I]);
                }

                Ctx.ExchangeProc = ExchangeLinkedStrings;
                Ctx.ExchangeData = Lists;
            }
            else
            {
                Lists = null;
            }

            var count = Strings.Count;
            QuickSortStrings(ref Ctx, 0, count - 1);
            if (Lists != null)
            {
                for (int I = 0, last = LinkedStrings.Length - 1; I <= last; I++)
                {
                    LinkedStrings[I].EndUpdate();
                }

                Lists.Free();
            }

            Strings.EndUpdate();
        }
    }

    // Engine.pas:2065
    public static void SortStringsAscending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        SortStringsEx(Strings, CompareAscending, LinkedStrings);
    }

    // Engine.pas:2076
    public static void SortStringsDescending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        SortStringsEx(Strings, CompareDescending, LinkedStrings);
    }

    // Engine.pas:2090
    public static void SortStringsNumericalAscending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        SortStringsEx(Strings, CompareNumericalAscending, LinkedStrings);
    }

    // Engine.pas:2104
    public static void SortStringsNumericalDescending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        SortStringsEx(Strings, CompareNumericalDescending, LinkedStrings);
    }

    // Engine.pas:2109
    public static int HexToInt(string S)
    {
        int L;
        int D;
        byte C;
        int Result = 0;
        L = S.Length;
        if (L > 8)
        {
            L = 8;
        }

        for (int I = 0; I <= L - 1; I++)
        {
            C = Cp1252.Byte(S[L - I - 1]);
            if (TSet<byte>.WithBounds(0, 255).WithRange(48, 57).Contains(C))
            {
                D = C - 48;
            }
            else
            {
                if (TSet<byte>.WithBounds(0, 255).WithRange(65, 70).Contains(C))
                {
                    D = C - 65 + 10;
                }
                else
                {
                    if (TSet<byte>.WithBounds(0, 255).WithRange(97, 102).Contains(C))
                    {
                        D = C - 97 + 10;
                    }
                    else
                    {
                        D = 0;
                    }
                }
            }

            Result = Result | (D << (I * 4));
        }

        return Result;
    }

    // Engine.pas:9224
    // { Plug-in registry }
    public static void RegisterPlugIn(TPlugIn PlugIn)
    {
        int I;
        bool Found;
        Found = false;
        I = 0;
        while (!Found && I != PlugIns.Count)
        {
            Found = ((TPlugIn)PlugIns[I]).Name() == PlugIn.Name();
            I += 1;
        }

        if (!Found)
        {
            PlugIns.Add(PlugIn);
        }
        else
        {
            throw Exception.Create("PlugIn " + PlugIn.Name() + " already registered");
        }
    }

    // Engine.pas:9242
    public static void UnregisterPlugIn(TPlugIn PlugIn)
    {
        if (PlugIns.IndexOf(PlugIn) != -1)
        {
            PlugIns.Remove(PlugIn);
        }
        else
        {
            throw Exception.Create("PlugIn not found");
        }
    }

    // Engine.pas:9337
    public static void InitializePlugIns(TEngine AEngine)
    {
        for (int I = 0, last = PlugIns.Count - 1; I <= last; I++)
        {
            ((TPlugIn)PlugIns[I]).Initialize(AEngine);
        }
    }

    // Engine.pas:9344
    public static void FinalizePlugIns(TEngine AEngine)
    {
        for (int I = 0, last = PlugIns.Count - 1; I <= last; I++)
        {
            ((TPlugIn)PlugIns[I]).Finalize(AEngine);
        }
    }

    // Engine.pas:9351
    public static TPlugIn GetPlugIn(int Index)
    {
        TPlugIn Result;
        if (PlugIns != null && Index >= 0 && Index < PlugIns.Count)
        {
            Result = (TPlugIn)PlugIns[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:9359
    public static TPlugIn FindPlugIn(string Name)
    {
        int I;
        TPlugIn Result = null;
        if (PlugIns != null)
        {
            I = 0;
            while (I != PlugIns.Count && Result == null)
            {
                if (((TPlugIn)PlugIns[I]).Name() == Name)
                {
                    Result = (TPlugIn)PlugIns[I];
                }

                I += 1;
            }
        }

        return Result;
    }

    // Engine.pas:9375
    public static void LoadExternalPlugIns(string Directory)
    {
        string Dir;
        void LoadFrom(string Dir)
        {
            TSearchRec SR = default;
            TStringList Dirs;
            bool Found;
            int H = default;
            Found = FindFirst(Dir + "*.DPL", 63 - 16, ref SR) == 0;
            while (Found)
            {
                try
                {
                    /* TODO: Implement the native Delphi package loader. */
                    throw new NotImplementedException();
                    Packages.AddObject(Dir + SR.Name, IntToObject(H));
                }
                catch
                {
                    throw Exception.Create("Unable to load PlugIn: " + Dir + SR.Name);
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            Dirs = new TStringList();
            Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    Dirs.Add(Dir + SR.Name + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
            {
                LoadFrom(Dirs[I]);
            }

            Dirs.Free();
        }

        Dir = ExpandFileName(Directory);
        // { the original tests the character of Directory, not of Dir }
        if (!IsEmpty(Dir) && Directory[Dir.Length - 1] != 92)
        {
            Dir = Dir + "\\";
        }

        LoadFrom(Dir);
    }

    // Engine.pas:9419
    public static void UnloadExternalPlugIns()
    {
        string S;
        for (int I = Packages.Count - 1; I >= 0; I--)
        {
            try
            {
                S = Packages[I];
                // { the original hands the name, not the handle in Objects, to UnloadPackage }
                /* TODO: Implement the native Delphi package loader. */
                throw new NotImplementedException();
                Packages.Delete(I);
            }
            catch
            {
                S = "Could not release PlugIn: " + Packages[I];
                throw Exception.Create(S);
            }
        }

        Packages.Clear();
    }

    // Engine.pas:9441
    // { Messages and engines }
    public static void InitMsg(ref TEMsg Msg, int MsgID, TEObject Sender)
    {
        Msg = default;
        Msg.MsgID = MsgID;
        Msg.Sender = Sender;
        Msg.Result = -1;
    }

    // Engine.pas:9449
    public static void InitMsgEx(ref TEMsg Msg, int MsgID, TEObject Sender, int Seq)
    {
        Msg = default;
        Msg.MsgID = MsgID;
        Msg.Seq = Seq;
        Msg.Sender = Sender;
        Msg.Result = -1;
    }

    // Engine.pas:3086
    public static int GetCRC32(ReadOnlySpan<byte> buffer, int size)
    {
        if (CRC32Table.Length == 0) BuildCRC32Table();
        int crc = -1;
        foreach (byte value in buffer[..size])
            crc = CRC32Table[(value ^ crc) & 255] ^ (int)((uint)crc >> 8);
        return crc ^ -1;
    }

    // Engine.pas:3099
    public static int GetFileCRC32(string FileName)
    {
        int Result = default;
        TMemoryStream M;
        TFileStream F;
        M = new TMemoryStream();
        try
        {
            F = new TFileStream(FileName, 0);
            try
            {
                M.LoadFromStream(F);
                var size = M.Size;
                Result = GetCRC32(M.Memory, size);
            }
            finally
            {
                F.Free();
            }
        }
        finally
        {
            M.Free();
        }

        return Result;
    }

    // Engine.pas:3081
    public static void ReleaseCRC32Table()
    {
        CRC32Table = [];
    }

    // Engine.pas:9458
    public static TEngine GetEngine(int Index)
    {
        TEngine Result;
        object P;
        if (Engines != null && Index >= 0 && Index < Engines.Count)
        {
            P = Engines[Index];
            Result = (TEngine)P;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:9466
    public static TEngine FindEngine(int AClassID)
    {
        int I;
        bool Found;
        Found = false;
        TEngine Result = null;
        if (Engines != null)
        {
            I = 0;
            Found = false;
            while (!Found && I < Engines.Count)
            {
                Found = AClassID == ((TEngine)Engines[I]).EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = (TEngine)Engines[I - 1];
            }
        }

        if (!Found)
        {
            throw Exception.Create("Cannot access Engine");
        }

        return Result;
    }

    // Engine.pas:2132
    public static void StringToStream(string S, TStream Stream)
    {
        int Len;
        Len = S.Length;
        Stream.Write(Bytes.Of(ref Len, 4));
        Stream.Write(Cp1252.Encode(S).AsSpan().Slice(0, Len));
    }

    // Engine.pas:2141
    public static void StringFromStream(ref string S, TStream Stream)
    {
        int Len = default;
        Stream.Read(Bytes.Of(ref Len, 4));
        if (Len < 0) throw new ERangeError("Negative string length");
        byte[] buffer = new byte[Len];
        Cp1252.Encode(S)[..Math.Min(S.Length, Len)].CopyTo(buffer);
        // A failed read leaves the resized value in the destination.
        S = Cp1252.Decode(buffer);
        Stream.Read(buffer);
        S = Cp1252.Decode(buffer);
    }

    // Engine.pas:2185
    public static void StringsToStream(TStrings Strings, TStream Stream)
    {
        ReadOnlySpan<byte> P;
        int Len;
        P = Cp1252.Encode(Strings.Text);
        Len = CStringLength(P);
        Stream.Write(Bytes.Of(ref Len, 4));
        Stream.WriteBuffer(P[..Len]);
    }

    // Engine.pas:2197
    public static void StringsFromStream(TStrings Strings, TStream Stream)
    {
        int Len = default;
        Strings.BeginUpdate();
        try
        {
            Stream.Read(Bytes.Of(ref Len, 4));
            if (Len < 0) throw new ERangeError("Negative string length");
            byte[] buffer = new byte[Len];
            Stream.Read(buffer);
            Strings.Text = Cp1252.Decode(buffer);
        }
        finally
        {
            Strings.EndUpdate();
        }
    }

    public static bool VerifyEObjectEnabled;
    public static readonly string EStreamReadErrorStr = "Error Reading EObject";
    public static readonly string EStreamWriteErrorStr = "Error Writing EObject";
    public static readonly string UserStr = "User";
    public static TList Engines;
    public static TList PlugIns;
    public static TEngine CopyEngine;
    public static TStringList Packages;
    public static uint ClipboardFormat;
    // Engine.pas:1570
    public static void SortItems(Span<TSortItem> SortList, int L, int R)
    {
        int I;
        int J;
        int P;
        TSortItem T;
        do
        {
            I = L;
            J = R;
            P = SortList[(int)(((uint)(L + R)) >> 1)].SortValue;
            do
            {
                while (P > SortList[I].SortValue)
                {
                    I += 1;
                }

                while (P < SortList[J].SortValue)
                {
                    J -= 1;
                }

                if (I <= J)
                {
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                SortItems(SortList, L, J);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:1602
    public static void InverseSortItems(Span<TSortItem> SortList, int L, int R)
    {
        int I;
        int J;
        int P;
        TSortItem T;
        do
        {
            I = L;
            J = R;
            P = SortList[(int)(((uint)(L + R)) >> 1)].SortValue;
            do
            {
                while (P < SortList[I].SortValue)
                {
                    I += 1;
                }

                while (P > SortList[J].SortValue)
                {
                    J -= 1;
                }

                if (I <= J)
                {
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                InverseSortItems(SortList, L, J);
            }

            L = I;
        }
        while (!(I >= R));
    }

    public static FixedArray_byte_4 CompressMagic = FixedArray_byte_4.From([67, 70, 83, 0]);
    // Engine.pas:1977
    public static void QuickSortStrings(ref TSortStringsContext Ctx, int L, int R)
    {
        int I;
        int J;
        int M;
        TObject PO;
        string P;
        do
        {
            I = L;
            J = R;
            M = (int)(((uint)(L + R)) >> 1);
            P = Ctx.Strings[M];
            PO = Ctx.Strings.Objects[M];
            do
            {
                while (true)
                {
                    var item = Ctx.Strings.Objects[I];
                    var stringsItem = Ctx.Strings[I];
                    if (!(Ctx.Compare(stringsItem, P, item, PO) < 0))
                        break;
                    I += 1;
                }

                while (true)
                {
                    var item = Ctx.Strings.Objects[J];
                    var stringsItem = Ctx.Strings[J];
                    if (!(Ctx.Compare(stringsItem, P, item, PO) > 0))
                        break;
                    J -= 1;
                }

                if (I <= J)
                {
                    Ctx.Strings.Exchange(I, J);
                    if (Ctx.ExchangeProc != null)
                    {
                        Ctx.ExchangeProc(Ctx.ExchangeData, I, J);
                    }

                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortStrings(ref Ctx, L, J);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:2018
    public static void ExchangeLinkedStrings(TList Lists, int I, int J)
    {
        for (int K = Lists.Count - 1; K >= 0; K--)
        {
            ((TStrings)Lists.List[K]).Exchange(I, J);
        }
    }

    // Engine.pas:2059
    public static int CompareAscending(string S1, string S2, TObject O1, TObject O2)
    {
        int Result = CompareText(S1, S2);
        if (Result == 0)
        {
            Result = S1.Length - S2.Length;
        }

        return Result;
    }

    // Engine.pas:2070
    public static int CompareDescending(string S1, string S2, TObject O1, TObject O2)
    {
        int Result = CompareText(S2, S1);
        if (Result == 0)
        {
            Result = S2.Length - S1.Length;
        }

        return Result;
    }

    // Engine.pas:2081
    public static int CompareNumericalAscending(string S1, string S2, TObject O1, TObject O2)
    {
        int V1;
        int V2;
        V1 = StrToIntDef(S1, -1);
        V2 = StrToIntDef(S2, -1);
        return V1 - V2;
    }

    // Engine.pas:2095
    public static int CompareNumericalDescending(string S1, string S2, TObject O1, TObject O2)
    {
        int V1;
        int V2;
        V1 = StrToIntDef(S1, -1);
        V2 = StrToIntDef(S2, -1);
        return V2 - V1;
    }

    // Engine.pas:2150
    public static void SaveStringToStream(string S, TStream Stream)
    {
        int Len;
        byte B = default;
        Len = S.Length;
        if (Len < 255)
        {
            B = (byte)Len;
            Stream.Write(Bytes.Of(ref B, 1));
        }
        else
        {
            B = 255;
            Stream.Write(Bytes.Of(ref B, 1));
            Stream.Write(Bytes.Of(ref Len, 4));
        }

        Stream.Write(Cp1252.Encode(S).AsSpan().Slice(0, Len));
    }

    // Engine.pas:2170
    public static void LoadStringFromStream(ref string S, TStream Stream)
    {
        int Len = default;
        byte B = default;
        S = "";
        Stream.Read(Bytes.Of(ref B, 1));
        if (B == 255)
        {
            Stream.Read(Bytes.Of(ref Len, 4));
        }
        else
        {
            Len = B;
        }

        if (Len < 0) throw new ERangeError("Negative string length");
        byte[] buffer = new byte[Len];
        Cp1252.Encode(S)[..Math.Min(S.Length, Len)].CopyTo(buffer);
        // A failed read leaves the resized value in the destination.
        S = Cp1252.Decode(buffer);
        Stream.Read(buffer);
        S = Cp1252.Decode(buffer);
    }

    // Engine.pas:2213
    public static void StringsToStreamEx(TStrings Strings, TStream Stream)
    {
        ReadOnlySpan<byte> P;
        int Len;
        int Obj = default;
        P = Cp1252.Encode(Strings.Text);
        Len = CStringLength(P);
        Stream.Write(Bytes.Of(ref Len, 4));
        Stream.WriteBuffer(P[..Len]);
        for (int I = 0, last = Strings.Count - 1; I <= last; I++)
        {
            Obj = ObjectToInt(Strings.Objects[I]);
            Stream.Write(Bytes.Of(ref Obj, 4));
        }
    }

    // Engine.pas:2230
    public static void StringsFromStreamEx(TStrings Strings, TStream Stream)
    {
        int Len = default;
        int Obj = default;
        Strings.BeginUpdate();
        try
        {
            Stream.Read(Bytes.Of(ref Len, 4));
            if (Len < 0) throw new ERangeError("Negative string length");
            byte[] buffer = new byte[Len];
            Stream.Read(buffer);
            Strings.Text = Cp1252.Decode(buffer);
            for (int I = 0, last = Strings.Count - 1; I <= last; I++)
            {
                Stream.Read(Bytes.Of(ref Obj, 4));
                Strings.Objects[I] = IntToObject(Obj);
            }
        }
        finally
        {
            Strings.EndUpdate();
        }
    }

    // { CRC32 }
    public static int[] CRC32Table = [];
    // Engine.pas:3061
    public static void BuildCRC32Table()
    {
        int C;
        int Polynomial;
        if (CRC32Table.Length == 0)
        {
            CRC32Table = new int[256];
            Polynomial = -306674912;
            for (int I = 0; I <= 255; I++)
            {
                C = I;
                for (int J = 7; J >= 0; J--)
                {
                    if ((C & 1) != 0)
                    {
                        C = ((int)(((uint)C) >> 1)) ^ Polynomial;
                    }
                    else
                    {
                        C = (int)(((uint)C) >> 1);
                    }

                    CRC32Table[I] = C;
                }
            }
        }
    }

    // Engine.pas:3438
    public static void QuickSortInteger(Span<int> SortList, int L, int R, TIntegerSortCompare SCompare)
    {
        int I;
        int J;
        int P;
        int T = default;
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
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortInteger(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:3773
    public static void QuickSortByte(Span<byte> SortList, int L, int R, TByteSortCompare SCompare)
    {
        int I;
        int J;
        byte P;
        byte T = default;
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
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortByte(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:4120
    public static void QuickSortShortInt(Span<sbyte> SortList, int L, int R, TShortIntSortCompare SCompare)
    {
        int I;
        int J;
        sbyte P;
        sbyte T = default;
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
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortShortInt(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:4431
    public static void QuickSortWord(Span<ushort> SortList, int L, int R, TWordSortCompare SCompare)
    {
        int I;
        int J;
        ushort P;
        ushort T = default;
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
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortWord(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:5254
    public static void QuickSortDoubleItem(Span<TDoubleItem> SortList, int L, int R, TDoubleItemSortCompare SCompare)
    {
        int I;
        int J;
        TDoubleItem P;
        TDoubleItem T = default;
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
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortDoubleItem(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:5287
    public static int CompareDoubleItemAAscending(in TDoubleItem Item1, in TDoubleItem Item2) => Item1.A - Item2.A;

    // Engine.pas:5292
    public static int CompareDoubleItemBAscending(in TDoubleItem Item1, in TDoubleItem Item2) => Item1.B - Item2.B;

    // Engine.pas:5297
    public static int CompareDoubleItemADescending(in TDoubleItem Item1, in TDoubleItem Item2) => Item2.A - Item1.A;

    // Engine.pas:5302
    public static int CompareDoubleItemBDescending(in TDoubleItem Item1, in TDoubleItem Item2) => Item2.B - Item1.B;

    // Engine.pas:5664
    public static void QuickSortQuadItem(Span<TQuadInteger> SortList, int L, int R, TQuadItemSortCompare SCompare)
    {
        int I;
        int J;
        TQuadInteger P;
        TQuadInteger T = default;
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
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortQuadItem(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Engine.pas:5697
    public static int CompareQuadItemAAscending(in TQuadInteger Item1, in TQuadInteger Item2) => Item1.A - Item2.A;

    // Engine.pas:5702
    public static int CompareQuadItemBAscending(in TQuadInteger Item1, in TQuadInteger Item2) => Item1.B - Item2.B;

    // Engine.pas:5707
    public static int CompareQuadItemCAscending(in TQuadInteger Item1, in TQuadInteger Item2) => Item1.C - Item2.C;

    // Engine.pas:5712
    public static int CompareQuadItemDAscending(in TQuadInteger Item1, in TQuadInteger Item2) => Item1.D - Item2.D;

    // Engine.pas:5717
    public static int CompareQuadItemADescending(in TQuadInteger Item1, in TQuadInteger Item2) => Item2.A - Item1.A;

    // Engine.pas:5722
    public static int CompareQuadItemBDescending(in TQuadInteger Item1, in TQuadInteger Item2) => Item2.B - Item1.B;

    // Engine.pas:5727
    public static int CompareQuadItemCDescending(in TQuadInteger Item1, in TQuadInteger Item2) => Item2.C - Item1.C;

    // Engine.pas:5732
    public static int CompareQuadItemDDescending(in TQuadInteger Item1, in TQuadInteger Item2) => Item2.D - Item1.D;

    // Engine.pas:10675
    // { "[id]=" at the start of a text line, or -1 }
    public static int GetLineID(string S)
    {
        int Result;
        int I;
        I = 1;
        while (TSet<byte>.WithBounds(0, 255).With(91).WithRange(48, 57).Contains(Cp1252.Byte(S[I - 1])))
        {
            I += 1;
        }

        if (Copy(S, I, 2) == "]=")
        {
            Result = StrToInt(Copy(S, 2, I - 2));
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:10685
    // { the text behind "[id]=" }
    public static string GetLineValue(string S)
    {
        int I;
        I = 1;
        while (TSet<byte>.WithBounds(0, 255).With(91).With(93).WithRange(48, 57).Contains(Cp1252.Byte(S[I - 1])))
        {
            I += 1;
        }

        return Copy(S, I + 1, S.Length - I);
    }

    public static void Initialize()
    {
        CRC32Table = [];
        VerifyEObjectEnabled = true;
        Engines = new TList();
        PlugIns = new TList();
        Packages = new TStringList();
        ClipboardFormat = RegisterClipboardFormat("ECLIPBOARD FORMAT");
        RegisterClass(typeof(TECopyComponent));
    }

    public static void Finalize()
    {
        ReleaseCRC32Table();
        UnloadExternalPlugIns();
        Packages.Free();
        Engines.Free();
        PlugIns.Free();
    }
}
