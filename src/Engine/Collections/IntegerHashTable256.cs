// Translated from Engine.pas.
using Aow.Engine.Buffers;

namespace Aow.Engine.Collections;

// 256 lists selected by the low key byte
public partial class IntegerHashTable256 : IntegerHashTable
, IDisposable
{
    internal FixedArray_Aow_Units_Engine_TIntegerHashList_256 FLists;

    // HashTab.PAS:83
    public IntegerHashTable256()
    {
        for (int I = 0; I <= 255; I++)
        {
            var integerHashList = new IntegerHashList();
            FLists[I] = integerHashList;
        }
    }

    // HashTab.PAS:91
    public virtual void Dispose()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I]?.Dispose();
        }
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
