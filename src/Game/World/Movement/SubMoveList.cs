// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Movement;

public partial class SubMoveList : ENode
{
    public SubMoveList(ECustomNode owner) : base(owner)
    { }

    // MoveT.PAS:39
    public PathFindingStep GetSubMove(int Index) => (PathFindingStep)FChildren.List[Index];

    // MoveT.PAS:44
    public int DeleteXYL(int Location)
    {
        int Result = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            if (Location == ((PathFindingStep)FChildren.List[I]).FLocation.Packed)
            {
                ((PathFindingStep)FChildren.List[I])?.Dispose();
                Result += 1;
            }
        }

        return Result;
    }

    // MoveT.PAS:57
    public void Delete(int Index)
    {
        ((PathFindingStep)FChildren.List[Index])?.Dispose();
    }

    // MoveT.PAS:62
    public int IndexOfXYL(int Location)
    {
        int Result = FChildren.Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Location == ((PathFindingStep)FChildren.List[Result]).FLocation.Packed)
            {
                return Result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
