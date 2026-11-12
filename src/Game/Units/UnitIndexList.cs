// Translated from AoWE.pas.
using Aow.Engine.Collections;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// inherited List, Count, Default
public partial class UnitIndexList : Int32List
{
    // UnitRes.PAS:219
    public UnitIndexList()
    {
        Default = -1;
    }

    // UnitRes.PAS:225
    public int GetUnitIndex(int index)
    {
        int result;
        if (index < 0 || index >= Count)
        {
            result = Default;
        }
        else
        {
            result = List[index];
        }

        return result;
    }

    // UnitRes.PAS:231
    public void VerifyUnits(bool showErrors)
    {
        int U;
        for (int I = Count - 1; I >= 0; I--)
        {
            U = List[I];
            if (U != -1 && GamePack.Current.UnitResources.GetUnitResource(U) == null)
            {
                if (showErrors)
                {
                    ShowMessage("Used unit missing (unit index:" + IntToStr(U) + ")");
                }

                List[I] = Default;
            }
        }
    }

    // UnitRes.PAS:245
    public void SetUnitIndex(int index, int value)
    {
        Assert(index >= 0, "List index out of bounds (TUnitIndexList)", "UnitRes.PAS", 250);
        if (index >= Count)
        {
            Count = index + 1;
        }

        List[index] = value;
    }

    // UnitRes.PAS:262
    public void SortUnits()
    {
        if (GamePack.Current != null)
        {
            Pack();
            try
            {
                Sort(CompareUnitIndexes);
            }
            catch
            {
            }
        }
    }

    private static int CompareUnitIndexes(int item1, int item2)
    {
        RtlUnit.Assert(GamePack.Current.UnitResources.GetUnitResource(item1) != null && GamePack.Current.UnitResources.GetUnitResource(item1) != null, "Undefined UnitResource Used!", "UnitRes.PAS", 256);
        UnitResource unitResource = GamePack.Current.UnitResources.GetUnitResource(item2);
        UnitResource unitResource2 = GamePack.Current.UnitResources.GetUnitResource(item1);
        return unitResource.Level - unitResource2.Level;
    }

    // intf_P2_05.inc:364

    // intf_P2_05.inc:364

}
