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
        FPath = new MovePath();
    }

    public MapCoordinate FLocation;
    public int FMoves;
    public int FMovePoints;
    public MovePath FPath;

    // AIGroup.pas:5051
    public override void Dispose()
    {
        FPath?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:5057
    public float GetMovementDays()
    {
        float Result;
        int Points;
        if (FMoves != 0)
        {
            Points = FPath.GetMovePoints();
            if (Points > FMovePoints)
            {
                Result = (float)((double)((double)(Points - FMovePoints) / FMoves) + 1);
            }
            else
            {
                if (Points != 0)
                {
                    Result = (float)((double)Points / FMovePoints);
                }
                else
                {
                    Result = 0;
                }
            }
        }
        else
        {
            Result = 99;
        }

        return Result;
    }

    // intf_P5_06.inc:394

}
