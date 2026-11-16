// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// inherited List, Count, Default
public partial class UnitIndexList : Int32List
{
    // UnitRes.PAS:219
    public UnitIndexList() => Default = -1;

    // UnitRes.PAS:225
    public int GetUnitIndex(int index)
    {
        int result = index < 0 || index >= Count ? Default : List[index];
        return result;
    }

    // UnitRes.PAS:231
    public void VerifyUnits(bool showErrors)
    {
        int u;
        for (int i = Count - 1; i >= 0; i--)
        {
            u = List[i];
            if (u != -1 && GamePack.Current.UnitResources.GetUnitResource(u) == null)
            {
                if (showErrors)
                {
                    ShowMessage("Used unit missing (unit index:" + IntToStr(u) + ")");
                }

                List[i] = Default;
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
        Assert(GamePack.Current.UnitResources.GetUnitResource(item1) != null && GamePack.Current.UnitResources.GetUnitResource(item1) != null, "Undefined UnitResource Used!", "UnitRes.PAS", 256);
        UnitResource unitResource = GamePack.Current.UnitResources.GetUnitResource(item2);
        UnitResource unitResource2 = GamePack.Current.UnitResources.GetUnitResource(item1);
        return unitResource.Level - unitResource2.Level;
    }

    // intf_P2_05.inc:364

    // intf_P2_05.inc:364

}
