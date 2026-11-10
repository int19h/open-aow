// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

// THashList items whose key dword is a string
public partial class StringHashList : HashList
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
        HashItem H = default;
        if (Find(Key) != null)
        {
            throw new InvalidOperationException("ID already in HashTable");
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
