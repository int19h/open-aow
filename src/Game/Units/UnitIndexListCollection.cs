// Translated from AoWE.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// FCollectionSize. FUnits
public partial class UnitIndexListCollection : IDisposable
{
    // Unit.PAS:2413
    public UnitIndexListCollection()
    {
        FUnits = new UnitIndexList();
        FCollectionSize = 8;
    }

    public int FCollectionSize;
    public UnitIndexList FUnits;

    // Unit.PAS:2419
    public virtual void Dispose()
    {
        FUnits?.Dispose();
    }

    // Unit.PAS:2424
    public void LoadFromStream(Stream Stream)
    {
        FUnits.LoadFromStream(Stream);
    }

    // Unit.PAS:2428
    public void SaveToStream(Stream Stream)
    {
        FUnits.SaveToStream(Stream);
    }

    // Unit.PAS:2432
    public int GetCollectionCount(int Index)
    {
        int I;
        int Last;
        int Result = 0;
        I = Index * FCollectionSize;
        Last = FCollectionSize + I;
        if (Last > FUnits.Count)
        {
            Last = FUnits.Count;
        }

        while (I < Last && FUnits[I] != -1)
        {
            I += 1;
            Result += 1;
        }

        return Result;
    }

    // Unit.PAS:2446
    public int GetCount() => (FUnits.Count + (FCollectionSize - 1)) / FCollectionSize;

    // Unit.PAS:2450
    public int GetU(int Collection, int Index) => FUnits[Index + Collection * FCollectionSize];

    // Unit.PAS:2454
    public bool AddUnit(int Collection, int UnitIndex)
    {
        bool Result;
        int N;
        N = GetCollectionCount(Collection);
        if (N < FCollectionSize)
        {
            FUnits[N + Collection * FCollectionSize] = UnitIndex;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Unit.PAS:2466
    public void DeleteUnit(int Collection, int Index)
    {
        int N;
        int Offset;
        int UnitIndex;
        N = GetCollectionCount(Collection);
        if (Index < N)
        {
            Offset = Collection * FCollectionSize;
            UnitIndex = Offset + Index;
            FUnits.Delete(UnitIndex);
            FUnits.Insert(Offset + N - 1, -1);
        }
    }

    // Unit.PAS:2477
    public int GetRndCollection()
    {
        int Index;
        int Tries;
        Tries = 10;
        do
        {
            Index = Random(GetCount());
            Tries -= 1;
        }
        while (!(GetCollectionCount(Index) != 0 || Tries == 0));
        if (GetCollectionCount(Index) == 0)
        {
            Index = 0;
            while (Index < GetCount() && GetCollectionCount(Index) == 0)
            {
                Index += 1;
            }

            if (GetCollectionCount(Index) == 0)
            {
                Index = -1;
            }
        }

        return Index;
    }

    // intf_P2_05.inc:377

}
