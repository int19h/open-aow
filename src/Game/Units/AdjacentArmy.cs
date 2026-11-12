// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Units;

// location, moves, current move points, path
public partial class AdjacentArmy : EngineObject
{
    // AIGroup.pas:5045
    public AdjacentArmy(ECustomNode owner) : base(owner)
    {
        Path = new MovePath();
    }

    public MapCoordinate Location;
    public int Moves;
    public int MovePoints;
    public MovePath Path;

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
            if (points > MovePoints)
            {
                result = (float)((double)((double)(points - MovePoints) / Moves) + 1);
            }
            else
            {
                if (points != 0)
                {
                    result = (float)((double)points / MovePoints);
                }
                else
                {
                    result = 0;
                }
            }
        }
        else
        {
            result = 99;
        }

        return result;
    }

    // intf_P5_06.inc:394

}
