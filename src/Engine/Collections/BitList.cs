// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class BitList : Persistent
{
    internal Memory<byte> FList = Memory<byte>.Empty;
    internal int FCount;
    internal int FCapacity;
    internal bool FDefault;

    // Engine.pas:4743
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
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
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:4576
    public override void Assign(Persistent Source)
    {
        if (Source is BitList)
        {
            var bitList = (BitList)Source;
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
    public BitList Expand()
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
    public bool Compare(BitList List) => List.FCount == FCount && CompareMem(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), List.FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), (int)(((uint)(FCount + 7)) >> 3));

    // Engine.pas:4782
    public void LoadFromStream(Stream Stream)
    {
        Clear();
        Stream.ReadBytes(Bytes.Of(ref FDefault, 1));
        Stream.ReadBytes(Bytes.Of(ref FCount, 4));
        SetCapacity(FCount);
        Stream.ReadBytes(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
    }

    // Engine.pas:4791
    public void SaveToStream(Stream Stream)
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
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

}
