using System.Runtime.InteropServices;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

internal static class ItemColumns
{
    internal static readonly string GridUsedRStr = "Used";

    internal static int ItemControlCompareName(int A, int B)
    {
        var itemControl = AoWHSMap.Current.FItemControl.GetItem(B);
        return CompareText(AoWHSMap.Current.FItemControl.GetItem(A).FOwnerName, itemControl.FOwnerName);
    }

    internal static int ItemControlCompareType(int A, int B)
    {
        var itemControl = AoWHSMap.Current.FItemControl.GetItem(A);
        var itemControlItemType = AoWHSMap.Current.FItemControl.GetItem(B);
        return itemControl.FItemType - itemControlItemType.FItemType;
    }

}
