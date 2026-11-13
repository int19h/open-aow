// Translated from AoWE.pas.
namespace Aow.Game.World.Movement;

// inherited steps and count. each step carries the move points spent entering its location
public partial class MovePath : PathStepList
{
    // Structur.PAS:3981
    // inherited locations and count
    public int MovePointsToPosition(int movePoints)
    {
        int result;
        if (movePoints >= 0)
        {
            result = Count;
            while (result > 0 && movePoints >= 0)
            {
                result -= 1;
                movePoints -= List[result].MoveCost;
            }

            if (movePoints < 0)
            {
                result += 1;
            }
        }
        else
        {
            result = Count - 1;
        }

        return result;
    }

    // Structur.PAS:3996
    public int PositionToMovePoints(int position)
    {
        int result = 0;
        for (int I = Count - 1; I >= position; I--)
        {
            result += List[I].MoveCost;
        }

        return result;
    }

    // Structur.PAS:4004
    public int GetMovePoints() => PositionToMovePoints(0);

    // HSEngine.pas:1279

}
