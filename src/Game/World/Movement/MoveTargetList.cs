// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Ai;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// reference children
public partial class MoveTargetList : EReferenceNode
{
    public MoveTargetList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4240
    public MoveTarget GetTarget(int Index) => (MoveTarget)FChildren.List[Index];

    // AIGroup.pas:4245
    public int IndexOfXYL(int Location)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Unsafe.As<XYL, int>(ref ((MoveTarget)FChildren.List[Result]).FLocation) == Location)
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4256
    public void AddTargetEx(int Location, int MovePoints, int TransporterID, in AITargetData Data)
    {
        MoveTarget Target;
        Target = new MoveTarget(null);
        Unsafe.As<XYL, int>(ref Target.FLocation) = Location;
        Target.FMovePoints = MovePoints;
        Target.FTransporterID = TransporterID;
        Target.FData = Data;
        AddChild(Target);
        Target.Release();
    }

    // AIGroup.pas:4275
    public void SortOnMovePoints()
    {
        FChildren.Sort(CompareMoveTargets);
    }

    private static int CompareMoveTargets(object Item1, object Item2)
    {
        return ((MoveTarget)Item1).FMovePoints - ((MoveTarget)Item2).FMovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
