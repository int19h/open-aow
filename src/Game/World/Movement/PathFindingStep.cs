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
        FMovePointLines = new DoubleItemList();
        FOrigin.Packed = 16777215;
        FLocation.Packed = 16777215;
    }

    public MapCoordinate FLocation;
    public MapCoordinate FOrigin;
    public DoubleItemList FMovePointLines;

    // MoveT.PAS:28
    public override void Dispose()
    {
        FMovePointLines?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:34
    public void AddMovePointLine(int Direction, int Count)
    {
        FMovePointLines.AddAB(Direction, Count);
    }

    // intf_P5_01.inc:9

}
