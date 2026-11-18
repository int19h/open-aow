// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Units;

// location, moves, current move points, path
public partial class AdjacentArmy(CustomNode owner) : EngineObject(owner)
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
        float result;
        int points;
        if (Moves != 0)
        {
            points = Path.GetMovePoints();
            result = points > MovePoints
                ? (float)((double)((double)(points - MovePoints) / Moves) + 1)
                : points != 0 ? (float)((double)points / MovePoints) : 0;
        }
        else
        {
            result = 99;
        }

        return result;
    }

    // intf_P5_06.inc:394

}
