// Translated from AoWE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// CollectionSize. Units
public partial class UnitIndexListCollection : IDisposable
{
    // Unit.PAS:2413
    public UnitIndexListCollection()
    {
        _units = new UnitIndexList();
        _collectionSize = 8;
    }

    private readonly int _collectionSize;
    private readonly UnitIndexList _units;

    /// <summary>The flat list the collection cuts into groups, which its stream stores.</summary>
    internal UnitIndexList Units => _units;

    // Unit.PAS:2419
    public virtual void Dispose()
    {
        _units?.Dispose();
    }

    // Unit.PAS:2432
    public int GetCollectionCount(int index)
    {
        int I;
        int last;
        int result = 0;
        I = index * _collectionSize;
        last = _collectionSize + I;
        if (last > _units.Count)
        {
            last = _units.Count;
        }

        while (I < last && _units[I] != -1)
        {
            I += 1;
            result += 1;
        }

        return result;
    }

    // Unit.PAS:2446
    public int GetCount() => (_units.Count + (_collectionSize - 1)) / _collectionSize;

    // Unit.PAS:2450
    public int GetU(int collection, int index) => _units[index + collection * _collectionSize];

    // Unit.PAS:2454
    public bool AddUnit(int collection, int unitIndex)
    {
        bool result;
        int N;
        N = GetCollectionCount(collection);
        if (N < _collectionSize)
        {
            _units[N + collection * _collectionSize] = unitIndex;
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
            offset = collection * _collectionSize;
            unitIndex = offset + index;
            _units.Delete(unitIndex);
            _units.Insert(offset + N - 1, -1);
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
