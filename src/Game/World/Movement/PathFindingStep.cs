// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// { Interface declaration run owned by P5. }
// location, origin, move point lines
public class PathFindingStep(CustomNode owner) : EngineObject(owner)
{

    public MapCoordinate Location { get; internal set; } = MapCoordinate.None;
    public MapCoordinate Origin { get; internal set; } = MapCoordinate.None;

    public DoubleItemList MovePointLines { get; } = new DoubleItemList();

    // MoveT.PAS:28
    public override void Dispose()
    {
        MovePointLines?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:34
    public void AddMovePointLine(int direction, int count) => MovePointLines.AddAbility(direction, count);
}
