
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Hash;

public partial class StringHashList : HashList
{
    // HashTabU.pas:226
    public override void Clear()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            FItems[I].Key = "";
        }

        base.Clear();
    }

    // HashTabU.pas:234
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

    // HashTabU.pas:251
    public void Add(object Item, string Key)
    {
        HashItem H = default;
        if (Find(Key) != null)
        {
            throw new InvalidOperationException("ID already in HashTable");
        }

        H.Key = 0;
        H.Item = Item;
        H.Key = Key;
        base.Add(H);
    }

    // HashTabU.pas:262
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
