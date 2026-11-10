// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Ai.Groups;

// armies
public partial class AIMoveExecuterArmyList : ENode
{
    public AIMoveExecuterArmyList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4642
    public AIMoveExecuterArmy GetArmies(int Index) => (AIMoveExecuterArmy)FChildren.List[Index];

    // AIGroup.pas:4647
    public int IndexOf(ArmyHS Army, byte Selection)
    {
        byte FoundSelection = default;
        int I;
        AIMoveExecuterArmy Entry;
        ArmyHS FoundArmy = default;
        // { byte local precedes the pointer local }
        I = Count;
        while (I != 0)
        {
            I -= 1;
            Entry = (AIMoveExecuterArmy)FChildren.List[I];
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
        AIMoveExecuterArmy Army;
        I = Count;
        while (I != 0)
        {
            I -= 1;
            Army = (AIMoveExecuterArmy)FChildren.List[I];
            if (X == Army.FLocation.X && Army.FLocation.Y == Y && Army.FLocation.L == L)
            {
                return I;
            }
        }

        return -1;
    }

    // AIGroup.pas:4693
    public int IndexOfLoc(XYL Location)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Unsafe.As<XYL, int>(ref ((AIMoveExecuterArmy)FChildren.List[Result]).FLocation) == Unsafe.As<XYL, int>(ref Location))
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4704
    public int IndexOfTransportStatus(AIMoveExecuterTransportStatus Status)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((AIMoveExecuterArmy)FChildren.List[Result]).FTransportStatus == Status)
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
        return ((AIMoveExecuterArmy)Item1).MovePoints(255) - ((AIMoveExecuterArmy)Item2).MovePoints(255);
    }

    // Engine.pas:631

    // Engine.pas:631

}
