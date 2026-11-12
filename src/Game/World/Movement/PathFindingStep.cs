// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// { Interface declaration run owned by P5. }
// location, origin, move point lines
public partial class PathFindingStep : EngineObject
{
    // MoveT.PAS:20
    public PathFindingStep(ECustomNode owner) : base(owner)
    {
        MovePointLines = new DoubleItemList();
        Origin.Packed = 16777215;
        Location.Packed = 16777215;
    }

    public MapCoordinate Location;
    public MapCoordinate Origin;
    public DoubleItemList MovePointLines;

    // MoveT.PAS:28
    public override void Dispose()
    {
        MovePointLines?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:34
    public void AddMovePointLine(int direction, int count)
    {
        MovePointLines.AddAbility(direction, count);
    }

    // intf_P5_01.inc:9

}
