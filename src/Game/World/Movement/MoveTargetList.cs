// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// reference children
public partial class MoveTargetList : EReferenceNode
{
    public MoveTargetList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4240
    public MoveTarget GetTarget(int index) => (MoveTarget)_children.List[index];

    // AIGroup.pas:4245
    public int IndexOfMapCoordinate(int location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (Unsafe.As<MapCoordinate, int>(ref ((MoveTarget)_children.List[result]).Location) == location)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4256
    public void AddTarget(int location, int movePoints, int transporterId, in AITargetData data)
    {
        MoveTarget target;
        target = new MoveTarget(null);
        Unsafe.As<MapCoordinate, int>(ref target.Location) = location;
        target.MovePoints = movePoints;
        target.TransporterId = transporterId;
        target.Data = data;
        AddChild(target);
        target.Release();
    }

    // AIGroup.pas:4275
    public void SortOnMovePoints()
    {
        _children.Sort(CompareMoveTargets);
    }

    private static int CompareMoveTargets(object item1, object item2)
    {
        return ((MoveTarget)item1).MovePoints - ((MoveTarget)item2).MovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
