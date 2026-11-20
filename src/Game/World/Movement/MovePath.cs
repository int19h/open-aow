// Translated from AoWE.pas.
namespace Aow.Game.World.Movement;

// inherited steps and count. each step carries the move points spent entering its location
public sealed class MovePath : PathStepList
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
            return Count - 1;
        }

        return result;
    }

    // Structur.PAS:3996
    public int PositionToMovePoints(int position)
    {
        int result = 0;
        for (int i = Count - 1; i >= position; i--)
        {
            result += List[i].MoveCost;
        }

        return result;
    }

    // Structur.PAS:4004
    public int GetMovePoints() => PositionToMovePoints(0);
}
