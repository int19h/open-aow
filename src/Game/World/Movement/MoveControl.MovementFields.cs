#nullable disable warnings

using Aow.Game.AI;
using Aow.Game.Host;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class MoveControl
{
    public void CalculatePath(MovePath path, int x, int y, int level)
    {
        DelphiAssert.Assert(_status.Contains(Statuses.Activated), message: "Assertion failure", file: @"D:\AoWDev\AoWE\inc\MoveT.PAS", line: 0x7e4f);
        path.Clear();
        if (level >= Map.MapContainer.LevelCount)
        {
            return;
        }

        SetMapL(Map.MapContainer.GetMapLevel(level));
        MapField? field = MapLevel.GetField(x, y);
        int points = Settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
        bool failed = points == 0xffffff;
        MapField? neighbor = null;
        while (field is not null && (field.HexX != Source.X || field.HexY != Source.Y || field.HexLevel != Source.MapLevel) && !failed)
        {
            _ = path.AddMapCoordinate(field.HexX, field.HexY, field.HexLevel);
            int cost = _movePoint(field, 1);
            bool found = false;
            for (int direction = 1; !found && direction <= 6; direction++)
            {
                neighbor = NeighborField(field, direction);
                if (Settings.GetMovePoints(neighbor.HexX, neighbor.HexY, neighbor.HexLevel) != points - cost)
                {
                    continue;
                }

                bool source = neighbor.HexX == Source.X && neighbor.HexY == Source.Y && neighbor.HexLevel == Source.MapLevel;
                int over = source || (neighbor.FieldFlags & MapField.MoveOverFlag) == 0 ? 1 : neighbor.CanMoveOver(this);
                found = over != 0 && ((field.FieldFlags & MapField.MoveOnFlag) == 0 || field.CanMoveOn(this, defaultCode: cost, moveOverCode: over) != -1);
            }
            MapField? previous;
            if (found)
            {
                points = Settings.GetMovePoints(neighbor.HexX, neighbor.HexY, neighbor.HexLevel);
                previous = neighbor;
            }
            else
            {
                previous = FindSubMoveOrigin(field);
                if (previous is not null && path.IndexOfMapCoordinate(previous.HexX, previous.HexY, L: previous.HexLevel) == -1)
                {
                    points = Settings.GetMovePoints(previous.HexX, previous.HexY, previous.HexLevel);
                    SetMapL(previous.Owner);
                    found = true;
                }
            }
            if (previous is not null && found)
            {
                path.List[path.Count - 1] = path.List[path.Count - 1] with { MoveCost = (sbyte)(Settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) - points) };
            }

            field = previous;
            failed = !found;
        }
        if (failed)
        {
            path.Clear();
        }
        else if (field is not null && points == 0)
        {
            _ = path.AddMapCoordinate(field.HexX, field.HexY, field.HexLevel);
        }

        StrategicMap strategicMap = Map.GetEngine().StrategicMap;
        if (strategicMap.AIExecutor.Status.Contains(AIExecutorStatuses.Executing))
        {
            strategicMap.AIExecutor.UseProcessTime(path.Count);
        }
    }

    private MapField NeighborField(MapField field, int direction)
    {
        XYTableEntry delta = HexGeometry.HexNumberToXYTable[field.HexX & 1][direction];
        return field.Owner.GetField(field.HexX + delta.X, field.HexY + delta.Y);
    }

    public int CalculateMovePointLine(MapField origin, int direction, int moveOverCode)
    {
        int initialOverCode = moveOverCode;
        int count = 0, points = Settings.GetMovePoints(origin.HexX, origin.HexY, origin.HexLevel);
        MapField field = origin;
        bool continueMove = true;
        while (continueMove)
        {
            field = NeighborField(field, direction);
            int cost = _movePoint(field, moveOverCode);
            if (cost < 0)
            {
                break;
            }

            points += cost;
            int old = Settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
            if (old <= points || points > MovePointsLimit)
            {
                break;
            }

            if (Mode.Contains(MoveModes.LogProcessedMapFields) && old == 0xff_ff_ff)
            {
                _ = _processedMapFields.Add(field);
            }

            if (field.HexX == Destination.X && field.HexY == Destination.Y && field.HexLevel == Destination.MapLevel)
            {
                MovePointsLimit = points;
            }

            Settings.SetMovePoints(field.HexX, field.HexY, field.HexLevel, value: points);
            moveOverCode = (field.FieldFlags & MapField.MoveOverFlag) == 0 ? 1 : field.CanMoveOver(this);

            continueMove = moveOverCode != 0;
            if (continueMove)
            {
                count++;
            }
        }
        HeadlessHooks.MoveLine?.Invoke(origin.MapCoordinate, direction, initialOverCode, count);
        return count;
    }

    public void ProcessHex(MapField field, int direction, int count)
    {
        _processedCount++;
        int over = (field.FieldFlags & MapField.MoveOverFlag) == 0 ? 1 : field.CanMoveOver(this);
        int left = s_sideDirections[direction][0], right = s_sideDirections[direction][1];
        int leftCount = CalculateMovePointLine(field, direction: left, moveOverCode: over);
        int rightCount = CalculateMovePointLine(field, direction: right, moveOverCode: over);
        if ((count & 0xFF) != 0)
        {
            ProcessHex(NeighborField(field, direction), direction, count: over == 2 ? count + 0xff : count - 1);
        }

        if (leftCount != 0)
        {
            ProcessHex(NeighborField(field, direction: left), direction: left, count: over == 2 ? leftCount + 0xff : leftCount - 1);
        }

        if (rightCount != 0)
        {
            ProcessHex(NeighborField(field, direction: right), direction: right, count: over == 2 ? rightCount + 0xff : rightCount - 1);
        }

        if ((count & 0b0001_0000_0000) != 0)
        {
            int opposite = OppositeDirections[direction];
            left = s_sideDirections[opposite][0];
            right = s_sideDirections[opposite][1];
            leftCount = CalculateMovePointLine(field, direction: left, moveOverCode: over);
            rightCount = CalculateMovePointLine(field, direction: right, moveOverCode: over);
            if (leftCount != 0)
            {
                ProcessHex(NeighborField(field, direction: left), direction: left, count: leftCount - 1);
            }

            if (rightCount != 0)
            {
                ProcessHex(NeighborField(field, direction: right), direction: right, count: rightCount - 1);
            }
        }
        if ((field.FieldFlags & MapField.MoveExclusiveFlag) != 0)
        {
            field.MoveExclusive(this);
        }
    }
}
