#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class MoveControl
{
    // MoveT.PAS:1687. Resolve native field offsets through hex neighbors.
    public int CalculateTurnAroundMovePoints(ref int gapIndex)
    {
        MapCoordinate start = GapStarts[gapIndex];
        MapField rightField = MapLevel.GetField(start.X, start.Y);
        MapField leftField = rightField;
        int rightDirection = start.MapLevel, leftDirection = start.MapLevel;
        int rightPoints = 0, leftPoints = 0;
        int rightOver = rightField.CanMoveOver(this), leftOver = rightOver;
        bool rightActive = true, leftActive = true, rightFound = false, leftFound = false, done = false;
        int index = gapIndex;
        while (!done)
        {
            if (rightActive)
            {
                Step(true, field: ref rightField, direction: ref rightDirection, total: ref rightPoints, over: ref rightOver, active: ref rightActive, found: ref rightFound);
            }

            if (leftActive)
            {
                Step(false, field: ref leftField, direction: ref leftDirection, total: ref leftPoints, over: ref leftOver, active: ref leftActive, found: ref leftFound);
            }
        }
        gapIndex = index;
        return leftFound ? leftPoints : rightFound ? rightPoints : -1;

        void Step(bool right, ref MapField field, ref int direction, ref int total,
            ref int over, ref bool active, ref bool found)
        {
            int attempts = 0, points, nextOver;
            MapField next;
            do
            {
                next = NeighborField(field, direction);
                points = _movePoint(next, over);
                nextOver = 1;
                if (points >= 0 && (next.FieldFlags & MapField.MoveOverFlag) != 0 &&
                    (next.HexX != Destination.X || next.HexY != Destination.Y))
                {
                    nextOver = next.CanMoveOver(this);
                    if (nextOver == 0)
                    {
                        points = -1;
                    }
                }
                direction += right ? 1 : -1;
                if (direction > 6)
                {
                    direction = 1;
                }

                if (direction == 0)
                {
                    direction = 6;
                }

                attempts++;
            } while (points < 0 && attempts <= 6);
            direction += (right ? -1 : 1) * (over == 2 ? 3 : 2);
            if (direction <= 0)
            {
                direction += 6;
            }

            if (direction > 6)
            {
                direction -= 6;
            }

            over = nextOver == 2 ? 2 : 1;
            total += points;
            field = next;
            if ((right ? total <= MovePointsLimit : total < MovePointsLimit) && points >= 0)
            {
                int end = GapEnds.IndexOfXY(field.HexX, field.HexY);
                if (end != -1 && end >= index)
                {
                    index = end;
                    found = true;
                    done = true;
                    active = false;
                }
                if (field.HexX == start.X && field.HexY == start.Y)
                {
                    active = false;
                    if (right ? !leftActive : !rightActive)
                    {
                        done = true;
                    }
                }
            }
            else
            {
                active = false;
                if (right ? !leftActive : !rightActive)
                {
                    done = true;
                }
            }
        }
    }
}
