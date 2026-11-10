// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class UnitProductionList : ENode
{
    public UnitProductionList(ECustomNode owner) : base(owner)
    { }

    // Structur.PAS:14
    public UnitProduction GetItem(int Index)
    {
        UnitProduction Result;
        if (Index >= 0 && Index < Count)
        {
            Result = (UnitProduction)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:20
    public void Add(int UnitIndex, bool Researched)
    {
        var unitProduction = new UnitProduction(this);
        unitProduction.FUnitIndex = UnitIndex;
        unitProduction.FResearched = Researched;
    }

    // Structur.PAS:29
    public int IndexOf(int Index)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Index == GetItem(Result).FUnitIndex)
            {
                return Result;
            }
        }

        return -1;
    }

    // Structur.PAS:55
    public void SortOnLevel()
    {
        FChildren.Sort(CompareUnitProductionLevel);
    }

    private static int CompareUnitProductionLevel(object Item1, object Item2)
    {
        UnitProduction tUnitProduction = (UnitProduction)Item1;
        int num = ((AoWHSSet.Current.FUnitResources.GetUnitResource(tUnitProduction.FUnitIndex) != null) ? (AoWHSSet.Current.FUnitResources.GetUnitResource(tUnitProduction.FUnitIndex).FLevel * 2) : 0);
        int num2 = ((AoWHSSet.Current.FUnitResources.GetUnitResource(((UnitProduction)Item2).FUnitIndex) != null) ? (AoWHSSet.Current.FUnitResources.GetUnitResource(((UnitProduction)Item2).FUnitIndex).FLevel * 2) : 0);
        int num3 = num - num2;
        if (num3 == 0)
        {
            num3 = ((UnitProduction)Item1).FUnitIndex - ((UnitProduction)Item2).FUnitIndex;
        }
        return num3;
    }

    // Engine.pas:631

    // Engine.pas:631

}
