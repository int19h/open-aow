// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

// items keyed by an integer
public partial class IntegerHashList : HashList
{
    // HashTab.PAS:43
    public void Add(object Item, int Key)
    {
        HashItem H = default;
        if (Find(Key) != null)
        {
            throw new InvalidOperationException("ID already in HashTable");
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
