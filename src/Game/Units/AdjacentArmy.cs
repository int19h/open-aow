// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Units;

// location, moves, current move points, path
public class AdjacentArmy(CustomNode owner) : EngineObject(owner)
{

    public MapCoordinate Location { get; set; }

    public int Moves { get; set; }
    public int MovePoints { get; set; }
    public MovePath Path { get; } = new MovePath();

    // AIGroup.pas:5051
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:5057
    public float GetMovementDays()
    {
        if (Moves != 0)
        {
            int points = Path.GetMovePoints();
            return points > MovePoints
                ? (float)((double)((double)(points - MovePoints) / Moves) + 1)
                : points != 0 ? (float)((double)points / MovePoints) : 0;
        }
        else
        {
            return 99;
        }
    }
}
