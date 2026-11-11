// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Engine;

public partial class PropertyTable : IDisposable
{
    internal Item[] FItems = [];
    internal int FCapacity;
    internal int FCount;
    internal int FNextIndex;
    internal int FMaxID;
    internal Settings[] FStack = [];
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
    public virtual void Dispose()
    {
        SetCapacity(0);
        Array.Resize(ref FStack, 0);
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

        ref Settings propertySettings = ref FStack[FStackCount];
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
        ref Settings propertySettings = ref FStack[FStackCount];
        FFirst = propertySettings.First;
        FNextIndex = propertySettings.NextIndex;
        FMaxID = propertySettings.MaxID;
    }

    // Engine.pas:2361
    public void LoadFromStream(Stream Stream)
    {
        int LongCount = default;
        int ID = default;
        int Offset = default;
        byte ShortCount = default;
        ushort Pair = default;
        Stream.ReadExactly(Bytes.Of(ref ShortCount, 1));
        if ((ShortCount & 128) != 0)
        {
            ShortCount = (byte)(ShortCount & 127);
            Stream.ReadExactly(Bytes.Of(ref LongCount, 4));
        }
        else
        {
            LongCount = 0;
        }

        if (ShortCount != 0)
        {
            for (int ordinal = 0, last = ShortCount - 1; ordinal <= last; ordinal++)
            {
                Stream.ReadExactly(Bytes.Of(ref Pair, 2));
                Add(Pair & 255, (int)(((uint)Pair) >> 8));
            }
        }

        if (LongCount != 0)
        {
            for (int ordinal = 0, last = LongCount - 1; ordinal <= last; ordinal++)
            {
                Stream.ReadExactly(Bytes.Of(ref ID, 4));
                Stream.ReadExactly(Bytes.Of(ref Offset, 4));
                Add(ID, Offset);
            }
        }
    }

    // Engine.pas:2391
    public void SaveToStream(Stream Stream)
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

    public Item[] Items { get => FItems; }
    public int Count { get => FCount; }
    public int First { get => FFirst; }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Item
    {
        [FieldOffset(0)]
        public int ID;
        [FieldOffset(4)]
        public int Offset;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct Settings
    {
        [FieldOffset(0)]
        public int First;
        [FieldOffset(4)]
        public int NextIndex;
        [FieldOffset(8)]
        public int MaxID;
    }
}
