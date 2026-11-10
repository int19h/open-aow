
namespace Aow.Engine.Hash;

// items keyed by an integer
public partial class IntegerHashList : HashList
{
    // HashTabU.pas:145
    public void Add(object Item, int Key)
    {
        HashItem H = default;
        if (Find(Key) != null)
        {
            throw new InvalidOperationException("ID already in HashTable");
        }

        H.Key = Key;
        H.Item = Item;
        base.Add(H);
    }

    // HashTabU.pas:155
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

    // HashTabU.pas:171
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
