// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class SubMoveList : ENode, IChildList<PathFindingStep>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PathFindingStep, so the cast never fails.
    public PathFindingStep this[int index] => (PathFindingStep)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PathFindingStep;

    int IChildSink<PathFindingStep>.AddChild(PathFindingStep child) => AddChild(child);

    public SubMoveList(ECustomNode owner) : base(owner)
    { }

    // MoveT.PAS:39
    public PathFindingStep GetSubMove(int index) => this[index];

    // MoveT.PAS:44
    public int DeleteMapCoordinate(MapCoordinate location)
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (location == this[I].Location)
            {
                this[I]?.Dispose();
                result += 1;
            }
        }

        return result;
    }

    // MoveT.PAS:57
    public void Delete(int index)
    {
        this[index]?.Dispose();
    }

    // MoveT.PAS:62
    public int IndexOfMapCoordinate(MapCoordinate location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (location == this[result].Location)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
