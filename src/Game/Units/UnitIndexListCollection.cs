// Translated from AoWE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// CollectionSize. Units
public partial class UnitIndexListCollection : IDisposable
{
    // Unit.PAS:2413
    public UnitIndexListCollection()
    {
        Units = new UnitIndexList();
        CollectionSize = 8;
    }

    public int CollectionSize;
    public UnitIndexList Units;

    // Unit.PAS:2419
    public virtual void Dispose()
    {
        Units?.Dispose();
    }

    // Unit.PAS:2424
    public void LoadFromStream(Stream stream)
    {
        Units.LoadFromStream(stream);
    }

    // Unit.PAS:2428
    public void SaveToStream(Stream stream)
    {
        Units.SaveToStream(stream);
    }

    // Unit.PAS:2432
    public int GetCollectionCount(int index)
    {
        int I;
        int last;
        int result = 0;
        I = index * CollectionSize;
        last = CollectionSize + I;
        if (last > Units.Count)
        {
            last = Units.Count;
        }

        while (I < last && Units[I] != -1)
        {
            I += 1;
            result += 1;
        }

        return result;
    }

    // Unit.PAS:2446
    public int GetCount() => (Units.Count + (CollectionSize - 1)) / CollectionSize;

    // Unit.PAS:2450
    public int GetU(int collection, int index) => Units[index + collection * CollectionSize];

    // Unit.PAS:2454
    public bool AddUnit(int collection, int unitIndex)
    {
        bool result;
        int N;
        N = GetCollectionCount(collection);
        if (N < CollectionSize)
        {
            Units[N + collection * CollectionSize] = unitIndex;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Unit.PAS:2466
    public void DeleteUnit(int collection, int index)
    {
        int N;
        int offset;
        int unitIndex;
        N = GetCollectionCount(collection);
        if (index < N)
        {
            offset = collection * CollectionSize;
            unitIndex = offset + index;
            Units.Delete(unitIndex);
            Units.Insert(offset + N - 1, -1);
        }
    }

    // Unit.PAS:2477
    public int GetRandomCollection()
    {
        int index;
        int tries;
        tries = 10;
        do
        {
            index = Random(GetCount());
            tries -= 1;
        }
        while (!(GetCollectionCount(index) != 0 || tries == 0));
        if (GetCollectionCount(index) == 0)
        {
            index = 0;
            while (index < GetCount() && GetCollectionCount(index) == 0)
            {
                index += 1;
            }

            if (GetCollectionCount(index) == 0)
            {
                index = -1;
            }
        }

        return index;
    }

    // intf_P2_05.inc:377

}
