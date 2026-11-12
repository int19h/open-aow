
using Aow.Game.AI;
using Aow.Game.Host;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class MoveControl
{
    public void CalculatePath(MovePath path, sbyte x, sbyte y, sbyte level)
    {
        RtlUnit.Assert(_status.Contains(StatusElement.Activated), "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7e4f);
        path.Clear();
        if (level >= Map.MapContainer.LevelCount) return;
        SetMapL(Map.MapContainer.GetMapLevel(level));
        var field = (AoWMapField)MapLevel.GetField(x, y);
        int points = Settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
        bool failed = points == 0xffffff;
        AoWMapField neighbor = null;
        while (field != null && (field.HexX != _sourceX || field.HexY != _sourceY || field.HexLevel != _sourceL) && !failed)
        {
            path.AddMapCoordinate(field.HexX, field.HexY, field.HexLevel);
            int cost = OnMovePoint(field, 1);
            bool found = false;
            for (int direction = 1; !found && direction <= 6; direction++)
            {
                neighbor = NeighborField(field, direction);
                if (Settings.GetMovePoints(neighbor.HexX, neighbor.HexY, neighbor.HexLevel) != points - cost) continue;
                bool source = neighbor.HexX == _sourceX && neighbor.HexY == _sourceY && neighbor.HexLevel == _sourceL;
                int over = source || (neighbor.FieldFlags & 0x2000) == 0 ? 1 : neighbor.CanMoveOver(this);
                found = over != 0 && ((field.FieldFlags & 0x4000) == 0 || field.CanMoveOn(this, cost, over) != -1);
            }
            AoWMapField previous;
            if (found)
            {
                points = Settings.GetMovePoints(neighbor.HexX, neighbor.HexY, neighbor.HexLevel);
                previous = neighbor;
            }
            else
            {
                previous = FindSubMoveOrigin(field);
                if (previous != null && path.IndexOfMapCoordinate(previous.HexX, previous.HexY, previous.HexLevel) == -1)
                {
                    points = Settings.GetMovePoints(previous.HexX, previous.HexY, previous.HexLevel);
                    SetMapL(previous.Owner);
                    found = true;
                }
            }
            if (previous != null && found)
                path.List[path.Count - 1].Tag = (sbyte)(Settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel) - points);
            field = previous;
            failed = !found;
        }
        if (failed) path.Clear();
        else if (field != null && points == 0) path.AddMapCoordinate(field.HexX, field.HexY, field.HexLevel);
        if (AoWHSMap.Current.AIExecutor.Status.Contains(AIExecutorStatusElement.Executing)) AoWHSMap.Current.AIExecutor.UseProcessTime(path.Count);
    }

    private AoWMapField NeighborField(AoWMapField field, int direction)
    {
        var delta = HexGeometry.HexNumberToXYTable[field.HexX & 1][direction];
        return (AoWMapField)field.Owner.GetField(field.HexX + delta.X, field.HexY + delta.Y);
    }

    public int CalculateMovePointLine(AoWMapField origin, int direction, int moveOverCode)
    {
        int initialOverCode = moveOverCode;
        int count = 0, points = Settings.GetMovePoints(origin.HexX, origin.HexY, origin.HexLevel);
        var field = origin;
        bool continueMove = true;
        while (continueMove)
        {
            field = NeighborField(field, direction);
            int cost = OnMovePoint(field, moveOverCode);
            if (cost < 0) break;
            points += cost;
            int old = Settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
            if (old <= points || points > MovePointsLimit) break;
            if (Mode.Contains(MoveModeElement.LogProcessedMapFields) && old == 0xffffff) _processedMapFields.Add(field);
            if (field.HexX == DestinationX && field.HexY == DestinationY && field.HexLevel == DestinationL) MovePointsLimit = points;
            Settings.SetMovePoints(field.HexX, field.HexY, field.HexLevel, points);
            if ((field.FieldFlags & 0x2000) == 0) moveOverCode = 1;
            else moveOverCode = field.CanMoveOver(this);
            continueMove = moveOverCode != 0;
            if (continueMove) count++;
        }
        HeadlessHooks.MoveLine?.Invoke(origin.GetMapCoordinate(), direction, initialOverCode, count);
        return count;
    }

    public void ProcessHex(AoWMapField field, int direction, int count)
    {
        _processedCount++;
        int over = (field.FieldFlags & 0x2000) == 0 ? 1 : field.CanMoveOver(this);
        int left = s_sideDirections[direction][0], right = s_sideDirections[direction][1];
        int leftCount = CalculateMovePointLine(field, left, over);
        int rightCount = CalculateMovePointLine(field, right, over);
        if ((count & 0xff) != 0) ProcessHex(NeighborField(field, direction), direction, over == 2 ? count + 0xff : count - 1);
        if (leftCount != 0) ProcessHex(NeighborField(field, left), left, over == 2 ? leftCount + 0xff : leftCount - 1);
        if (rightCount != 0) ProcessHex(NeighborField(field, right), right, over == 2 ? rightCount + 0xff : rightCount - 1);
        if ((count & 0x100) != 0)
        {
            int opposite = OppositeDirections[direction];
            left = s_sideDirections[opposite][0];
            right = s_sideDirections[opposite][1];
            leftCount = CalculateMovePointLine(field, left, over);
            rightCount = CalculateMovePointLine(field, right, over);
            if (leftCount != 0) ProcessHex(NeighborField(field, left), left, leftCount - 1);
            if (rightCount != 0) ProcessHex(NeighborField(field, right), right, rightCount - 1);
        }
        if ((field.FieldFlags & 0x8000) != 0) field.MoveExclusive(this);
    }
}
