// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class UnitProductionList : ENode, IChildList<UnitProduction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an UnitProduction, so the cast never fails.
    public UnitProduction this[int index] => (UnitProduction)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is UnitProduction;

    int IChildSink<UnitProduction>.AddChild(UnitProduction child) => AddChild(child);

    public UnitProductionList(ECustomNode owner) : base(owner)
    { }

    // Structur.PAS:14
    public UnitProduction GetItem(int index)
    {
        UnitProduction result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Structur.PAS:20
    public void Add(int unitIndex, bool researched)
    {
        var unitProduction = new UnitProduction(this);
        unitProduction.UnitIndex = unitIndex;
        unitProduction.Researched = researched;
    }

    // Structur.PAS:29
    public int IndexOf(int index)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (index == GetItem(result).UnitIndex)
            {
                return result;
            }
        }

        return -1;
    }

    // Structur.PAS:55
    public void SortOnLevel()
    {
        SortChildren(CompareUnitProductionLevel);
    }

    private static int CompareUnitProductionLevel(object item1, object item2)
    {
        UnitProduction unitProduction = (UnitProduction)item1;
        int number = ((AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex) != null) ? (AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level * 2) : 0);
        int number2 = ((AoWHSSet.Current.UnitResources.GetUnitResource(((UnitProduction)item2).UnitIndex) != null) ? (AoWHSSet.Current.UnitResources.GetUnitResource(((UnitProduction)item2).UnitIndex).Level * 2) : 0);
        int number3 = number - number2;
        if (number3 == 0)
        {
            number3 = ((UnitProduction)item1).UnitIndex - ((UnitProduction)item2).UnitIndex;
        }
        return number3;
    }

    // Engine.pas:631

    // Engine.pas:631

}
