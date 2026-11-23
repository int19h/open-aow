#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.Units;

// inherited List, Count, Default
public sealed class UnitIndexList : Int32List
{
    // UnitRes.PAS:219
    public UnitIndexList() => Default = -1;

    // UnitRes.PAS:225
    public int GetUnitIndex(int index) => index < 0 || index >= Count ? Default : List[index];

    // UnitRes.PAS:231
    public void VerifyUnits(bool showErrors)
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            int u = List[i];
            if (u != -1 && GamePack.Current.UnitResources.GetUnitResource(u) is null)
            {
                if (showErrors)
                {
                    ShowMessage($"Used unit missing (unit index:{u.ToString(CultureInfo.InvariantCulture)})");
                }

                List[i] = Default;
            }
        }
    }

    // UnitRes.PAS:245
    public void SetUnitIndex(int index, int value)
    {
        Assert(index >= 0, message: "List index out of bounds (TUnitIndexList)", file: "UnitRes.PAS", line: 250);
        if (index >= Count)
        {
            Count = index + 1;
        }

        List[index] = value;
    }

    // UnitRes.PAS:262
    public void SortUnits()
    {
        if (GamePack.Current is not null)
        {
            Pack();
            try
            {
                Sort(CompareUnitIndexes);
            }
            catch (Exception error)
            {
                HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
            }
        }
    }

    private static int CompareUnitIndexes(int item1, int item2)
    {
        // UnitRes.PAS:256 tests the first item twice.
        Assert(GamePack.Current.UnitResources.GetUnitResource(item1) is not null, message: "Undefined UnitResource Used!", file: "UnitRes.PAS", line: 256);
        UnitResource unitResource = GamePack.Current.UnitResources.GetUnitResource(item2);
        UnitResource unitResource2 = GamePack.Current.UnitResources.GetUnitResource(item1);
        return unitResource.Level - unitResource2.Level;
    }
}
