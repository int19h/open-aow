// Translated from AoWE.pas.
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// inherited locations and count. the signed Flag byte of each location is its movement cost
public partial class MovePath : MapCoordinateList
{
    // Structur.PAS:3981
    // inherited locations and count
    public int MovePointsToPos(int MovePoints)
    {
        int Result;
        if (MovePoints >= 0)
        {
            Result = Count;
            while (Result > 0 && MovePoints >= 0)
            {
                Result -= 1;
                MovePoints -= List[Result].Flag;
            }

            if (MovePoints < 0)
            {
                Result += 1;
            }
        }
        else
        {
            Result = Count - 1;
        }

        return Result;
    }

    // Structur.PAS:3996
    public int PosToMovePoints(int Pos)
    {
        int Result = 0;
        for (int I = Count - 1; I >= Pos; I--)
        {
            Result += List[I].Flag;
        }

        return Result;
    }

    // Structur.PAS:4004
    public int GetMovePoints() => PosToMovePoints(0);

    // HSEngine.pas:1279

}
