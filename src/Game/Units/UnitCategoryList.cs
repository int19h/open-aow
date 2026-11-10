// Translated from AoWE.pas.
using Aow.Engine.Collections;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class UnitCategoryList : DelphiList
{
    // Ability.PAS:723
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Ability.PAS:729
    public UnitIndexList GetCategory(int Index)
    {
        UnitIndexList Result;
        if (Index >= 0 && Index < Count)
        {
            Result = (UnitIndexList)this[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Ability.PAS:736
    public new void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            if (List[I] != null)
            {
                ((UnitIndexList)List[I])?.Dispose();
            }
        }

        base.Clear();
    }

    // Ability.PAS:745
    public void SortUnits()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            if (List[I] != null)
            {
                ((UnitIndexList)List[I]).SortUnits();
            }
        }
    }

    // Ability.PAS:753
    public void AddUnit(int Category, int UnitIndex)
    {
        while (Category >= Count)
        {
            Add(null);
        }

        // { Original does not reject negative category indexes. }
        if (List[Category] == null)
        {
            var unitIndexList = new UnitIndexList();
            List[Category] = unitIndexList;
        }

        if (((IntegerList)List[Category]).IndexOf(UnitIndex) == -1)
        {
            ((IntegerList)List[Category]).Add(UnitIndex);
        }
    }

    // System.pas:111

}
