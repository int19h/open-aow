// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Movement;

public partial class SubMoveList : ENode
{
    public SubMoveList(ECustomNode owner) : base(owner)
    { }

    // MoveT.PAS:39
    public PathFindingStep GetSubMove(int index) => (PathFindingStep)_children.List[index];

    // MoveT.PAS:44
    public int DeleteMapCoordinate(int location)
    {
        int result = 0;
        for (int I = _children.Count - 1; I >= 0; I--)
        {
            if (location == ((PathFindingStep)_children.List[I]).Location.Packed)
            {
                ((PathFindingStep)_children.List[I])?.Dispose();
                result += 1;
            }
        }

        return result;
    }

    // MoveT.PAS:57
    public void Delete(int index)
    {
        ((PathFindingStep)_children.List[index])?.Dispose();
    }

    // MoveT.PAS:62
    public int IndexOfMapCoordinate(int location)
    {
        int result = _children.Count;
        while (result != 0)
        {
            result -= 1;
            if (location == ((PathFindingStep)_children.List[result]).Location.Packed)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
