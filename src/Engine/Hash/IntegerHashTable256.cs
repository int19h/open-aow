using Aow.Engine.Buffers;

namespace Aow.Engine.Hash;

// 256 lists selected by the low key byte
public partial class IntegerHashTable256 : IntegerHashTable
, IDisposable
{
    internal FixedArray_Aow_Units_HashTabU_TIntegerHashList_256 FLists;

    // HashTabU.pas:186
    public IntegerHashTable256()
    {
        for (int I = 0; I <= 255; I++)
        {
            var integerHashList = new IntegerHashList();
            FLists[I] = integerHashList;
        }
    }

    // HashTabU.pas:194
    public virtual void Dispose()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I]?.Dispose();
        }
    }

    // HashTabU.pas:202
    public override void Clear()
    {
        for (int I = 0; I <= 255; I++)
        {
            FLists[I].Clear();
        }
    }

    // HashTabU.pas:209
    public override void Add(object Item, int Key)
    {
        FLists[Key & 255].Add(Item, Key);
    }

    // HashTabU.pas:214
    public override void Delete(int Key)
    {
        FLists[Key & 255].Delete(Key);
    }

    // HashTabU.pas:219
    public override object Find(int Key) => FLists[Key & 255].Find(Key);

}
