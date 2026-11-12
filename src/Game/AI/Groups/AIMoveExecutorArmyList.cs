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
    public AIMoveExecutorArmy GetArmies(int index) => (AIMoveExecutorArmy)_children.List[index];

    // AIGroup.pas:4647
    public int IndexOf(ArmyHexSprite army, byte selection)
    {
        byte foundSelection = default;
        int I;
        AIMoveExecutorArmy entry;
        ArmyHexSprite foundArmy = default;
        // { byte local precedes the pointer local }
        I = Count;
        while (I != 0)
        {
            I -= 1;
            entry = (AIMoveExecutorArmy)_children.List[I];
            if (army.GetHexX() == entry.Location.X && army.GetHexY() == entry.Location.Y && army.GetHexLevel() == entry.Location.MapLevel)
            {
                entry.RetrieveArmy(255, ref foundArmy, ref foundSelection);
                if (army == foundArmy && foundSelection == selection)
                {
                    return I;
                }
            }
        }

        return -1;
    }

    // AIGroup.pas:4673
    public int IndexOfMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        int I;
        AIMoveExecutorArmy army;
        I = Count;
        while (I != 0)
        {
            I -= 1;
            army = (AIMoveExecutorArmy)_children.List[I];
            if (X == army.Location.X && army.Location.Y == Y && army.Location.MapLevel == L)
            {
                return I;
            }
        }

        return -1;
    }

    // AIGroup.pas:4693
    public int IndexOfLocation(MapCoordinate location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (Unsafe.As<MapCoordinate, int>(ref ((AIMoveExecutorArmy)_children.List[result]).Location) == Unsafe.As<MapCoordinate, int>(ref location))
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4704
    public int IndexOfTransportStatus(AIMoveExecutorTransportStatus status)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (((AIMoveExecutorArmy)_children.List[result]).TransportStatus == status)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4721
    public void SortOnMovePoints()
    {
        _children.Sort(CompareExecutorArmies);
    }

    private static int CompareExecutorArmies(object item1, object item2)
    {
        return ((AIMoveExecutorArmy)item1).MovePoints(255) - ((AIMoveExecutorArmy)item2).MovePoints(255);
    }

    // Engine.pas:631

    // Engine.pas:631

}
