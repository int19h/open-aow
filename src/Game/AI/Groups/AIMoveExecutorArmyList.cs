// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// armies
public partial class AIMoveExecutorArmyList : ENode
{
    public AIMoveExecutorArmyList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4642
    public AIMoveExecutorArmy GetArmies(int Index) => (AIMoveExecutorArmy)FChildren.List[Index];

    // AIGroup.pas:4647
    public int IndexOf(ArmyHexSprite Army, byte Selection)
    {
        byte FoundSelection = default;
        int I;
        AIMoveExecutorArmy Entry;
        ArmyHexSprite FoundArmy = default;
        // { byte local precedes the pointer local }
        I = Count;
        while (I != 0)
        {
            I -= 1;
            Entry = (AIMoveExecutorArmy)FChildren.List[I];
            if (Army.GetXhx() == Entry.FLocation.X && Army.GetYhx() == Entry.FLocation.Y && Army.GetLhx() == Entry.FLocation.L)
            {
                Entry.RetrieveArmy(255, ref FoundArmy, ref FoundSelection);
                if (Army == FoundArmy && FoundSelection == Selection)
                {
                    return I;
                }
            }
        }

        return -1;
    }

    // AIGroup.pas:4673
    public int IndexOfXYL(sbyte X, sbyte Y, sbyte L)
    {
        int I;
        AIMoveExecutorArmy Army;
        I = Count;
        while (I != 0)
        {
            I -= 1;
            Army = (AIMoveExecutorArmy)FChildren.List[I];
            if (X == Army.FLocation.X && Army.FLocation.Y == Y && Army.FLocation.L == L)
            {
                return I;
            }
        }

        return -1;
    }

    // AIGroup.pas:4693
    public int IndexOfLoc(MapCoordinate Location)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Unsafe.As<MapCoordinate, int>(ref ((AIMoveExecutorArmy)FChildren.List[Result]).FLocation) == Unsafe.As<MapCoordinate, int>(ref Location))
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4704
    public int IndexOfTransportStatus(AIMoveExecutorTransportStatus Status)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecutorArmy)FChildren.List[Result]).FTransportStatus == Status)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4721
    public void SortOnMovePoints()
    {
        FChildren.Sort(CompareExecuterArmies);
    }

    private static int CompareExecuterArmies(object Item1, object Item2)
    {
        return ((AIMoveExecutorArmy)Item1).MovePoints(255) - ((AIMoveExecutorArmy)Item2).MovePoints(255);
    }

    // Engine.pas:631

    // Engine.pas:631

}
