// Translated from AoWE.pas.
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Units;

// CollectionSize. Units
public partial class UnitIndexListCollection : IDisposable
{
    // Unit.PAS:2413
    public UnitIndexListCollection()
    {
        Units = new UnitIndexList();
        _collectionSize = 8;
    }

    private readonly int _collectionSize;

    /// <summary>The flat list the collection cuts into groups, which its stream stores.</summary>
    internal UnitIndexList Units { get; }

    // Unit.PAS:2419
    public virtual void Dispose() => Units?.Dispose();

    // Unit.PAS:2432
    public int GetCollectionCount(int index)
    {
        int result = 0;
        int i = index * _collectionSize;
        int last = _collectionSize + i;
        if (last > Units.Count)
        {
            last = Units.Count;
        }

        while (i < last && Units[i] != -1)
        {
            i += 1;
            result += 1;
        }

        return result;
    }

    // Unit.PAS:2446
    public int GetCount() => (Units.Count + (_collectionSize - 1)) / _collectionSize;

    // Unit.PAS:2450
    public int GetU(int collection, int index) => Units[index + (collection * _collectionSize)];

    // Unit.PAS:2454
    public bool AddUnit(int collection, int unitIndex)
    {
        int n = GetCollectionCount(collection);
        bool result;
        if (n < _collectionSize)
        {
            Units[n + (collection * _collectionSize)] = unitIndex;
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
        int n = GetCollectionCount(collection);
        if (index < n)
        {
            int offset = collection * _collectionSize;
            int unitIndex = offset + index;
            Units.Delete(unitIndex);
            Units.Insert(offset + n - 1, -1);
        }
    }

    // Unit.PAS:2477
    public int GetRandomCollection()
    {
        int tries = 10;
        int index;
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
