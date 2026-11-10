// Translated from AoWE.pas.
using Aow.Engine.Collections;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// inherited List, Count, Default
public partial class UnitIndexList : IntegerList
{
    // UnitRes.PAS:219
    public UnitIndexList()
    {
        Default = -1;
    }

    // UnitRes.PAS:225
    public int GetUnitIndex(int Index)
    {
        int Result;
        if (Index < 0 || Index >= Count)
        {
            Result = Default;
        }
        else
        {
            Result = List[Index];
        }

        return Result;
    }

    // UnitRes.PAS:231
    public void VerifyUnits(bool ShowErrors)
    {
        int U;
        for (int I = Count - 1; I >= 0; I--)
        {
            U = List[I];
            if (U != -1 && AoWHSSet.Current.FUnitResources.GetUnitResource(U) == null)
            {
                if (ShowErrors)
                {
                    ShowMessage("Used unit missing (unit index:" + IntToStr(U) + ")");
                }

                List[I] = Default;
            }
        }
    }

    // UnitRes.PAS:245
    public void SetUnitIndex(int Index, int Value)
    {
        Assert(Index >= 0, "List index out of bounds (TUnitIndexList)", "UnitRes.PAS", 250);
        if (Index >= Count)
        {
            Count = Index + 1;
        }

        List[Index] = Value;
    }

    // UnitRes.PAS:262
    public void SortUnits()
    {
        if (AoWHSSet.Current != null)
        {
            Pack();
            try
            {
                Sort(P2CompareUnitIndexes);
            }
            catch
            {
            }
        }
    }

    private static int P2CompareUnitIndexes(int Item1, int Item2)
    {
        RtlUnit.Assert(AoWHSSet.Current.FUnitResources.GetUnitResource(Item1) != null && AoWHSSet.Current.FUnitResources.GetUnitResource(Item1) != null, "Undefined UnitResource Used!", "UnitRes.PAS", 256);
        UnitResource unitResource = AoWHSSet.Current.FUnitResources.GetUnitResource(Item2);
        UnitResource unitResource2 = AoWHSSet.Current.FUnitResources.GetUnitResource(Item1);
        return unitResource.FLevel - unitResource2.FLevel;
    }

    // intf_P2_05.inc:364

    // intf_P2_05.inc:364

}
