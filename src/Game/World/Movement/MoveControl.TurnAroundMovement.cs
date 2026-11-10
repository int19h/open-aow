using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class MoveControl
{
    // MoveT.PAS:1687. Resolve native field offsets through hex neighbors.
    public int CalculateTurnAroundMovePoints(ref int gapIndex)
    {
        var start = FGapStarts[gapIndex];
        var rightField = (AoWMapField)FMapLevel.GetField(start.X, start.Y);
        var leftField = rightField;
        int rightDirection = start.L, leftDirection = start.L;
        int rightPoints = 0, leftPoints = 0;
        int rightOver = rightField.CanMoveOver(this), leftOver = rightOver;
        bool rightActive = true, leftActive = true, rightFound = false, leftFound = false, done = false;
        int index = gapIndex;
        while (!done)
        {
            if (rightActive) Step(true, ref rightField, ref rightDirection, ref rightPoints, ref rightOver, ref rightActive, ref rightFound);
            if (leftActive) Step(false, ref leftField, ref leftDirection, ref leftPoints, ref leftOver, ref leftActive, ref leftFound);
        }
        gapIndex = index;
        return leftFound ? leftPoints : rightFound ? rightPoints : -1;

        void Step(bool right, ref AoWMapField field, ref int direction, ref int total,
            ref int over, ref bool active, ref bool found)
        {
            int attempts = 0, points, nextOver;
            AoWMapField next;
            do
            {
                next = NeighborField(field, direction);
                points = FOnMovePoint(next, over);
                nextOver = 1;
                if (points >= 0 && (next.FFieldFlags & 0x2000) != 0 &&
                    (next.Xhx != FDestinationX || next.Yhx != FDestinationY))
                {
                    nextOver = next.CanMoveOver(this);
                    if (nextOver == 0) points = -1;
                }
                direction += right ? 1 : -1;
                if (direction > 6) direction = 1;
                if (direction == 0) direction = 6;
                attempts++;
            } while (points < 0 && attempts <= 6);
            direction += (right ? -1 : 1) * (over == 2 ? 3 : 2);
            if (direction <= 0) direction += 6;
            if (direction > 6) direction -= 6;
            over = nextOver == 2 ? 2 : 1;
            total += points;
            field = next;
            if ((right ? total <= FMovePointsLimit : total < FMovePointsLimit) && points >= 0)
            {
                int end = FGapEnds.IndexOfXY(field.Xhx, field.Yhx);
                if (end != -1 && end >= index)
                {
                    index = end;
                    found = true;
                    done = true;
                    active = false;
                }
                if (field.Xhx == start.X && field.Yhx == start.Y)
                {
                    active = false;
                    if (right ? !leftActive : !rightActive) done = true;
                }
            }
            else
            {
                active = false;
                if (right ? !leftActive : !rightActive) done = true;
            }
        }
    }
}
