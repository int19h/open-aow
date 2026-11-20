// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public class SubMoveList(CustomNode owner) : OwningNode(owner), IChildList<PathFindingStep>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PathFindingStep, so the cast never fails.
    public PathFindingStep this[int index] => GetChild<PathFindingStep>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PathFindingStep;

    int IChildSink<PathFindingStep>.AddChild(PathFindingStep child) => AddChild(child);

    // MoveT.PAS:39
    public PathFindingStep GetSubMove(int index) => this[index];

    // MoveT.PAS:44
    public int DeleteMapCoordinate(MapCoordinate location)
    {
        int result = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            if (location == this[i].Location)
            {
                this[i]?.Dispose();
                result += 1;
            }
        }

        return result;
    }

    // MoveT.PAS:57
    public void Delete(int index) => this[index]?.Dispose();

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
}
