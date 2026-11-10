using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class TMoveControlSettings
{
    public int GetMovePoints(int x, int y, int level) => FLevelData[level] is null ? 0xffffff : FLevelData[level][y * FWidth + x];
    public void SetMovePoints(int x, int y, int level, int value) => FLevelData[level][y * FWidth + x] = value;

    public int GetMovePointsXYL(int packed)
    {
        var location = new TXYL { XYL = packed };
        return GetMovePoints(location.X, location.Y, location.L);
    }

    public void SetMovePointsXYL(int packed, int value)
    {
        var location = new TXYL { XYL = packed };
        SetMovePoints(location.X, location.Y, location.L, value);
    }
}

public partial class TMoveControl
{
    public void CalculatePathEx(TMovepath path, sbyte x, sbyte y, sbyte level)
    {
        RtlUnit.Assert(FStatus.Contains(TMoveControlStatusElement.mcsActivated), "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7e4f);
        path.Clear();
        if (level >= FMap.MapContainer.LevelCount) return;
        SetMapL(FMap.MapContainer.GetMapLevel(level));
        var field = (TAoWMapField)FMapLevel.GetField(x, y);
        int points = FSettings.GetMovePoints(field.Xhx, field.Yhx, field.Lhx);
        bool failed = points == 0xffffff;
        TAoWMapField neighbor = null;
        while (field != null && (field.Xhx != FSourceX || field.Yhx != FSourceY || field.Lhx != FSourceL) && !failed)
        {
            path.AddXYL(field.Xhx, field.Yhx, field.Lhx);
            int cost = FOnMovePoint(field, 1);
            bool found = false;
            for (int direction = 1; !found && direction <= 6; direction++)
            {
                neighbor = NeighborField(field, direction);
                if (FSettings.GetMovePoints(neighbor.Xhx, neighbor.Yhx, neighbor.Lhx) != points - cost) continue;
                bool source = neighbor.Xhx == FSourceX && neighbor.Yhx == FSourceY && neighbor.Lhx == FSourceL;
                int over = source || (neighbor.FFieldFlags & 0x2000) == 0 ? 1 : neighbor.CanMoveOver(this);
                found = over != 0 && ((field.FFieldFlags & 0x4000) == 0 || field.CanMoveOn(this, cost, over) != -1);
            }
            TAoWMapField previous;
            if (found)
            {
                points = FSettings.GetMovePoints(neighbor.Xhx, neighbor.Yhx, neighbor.Lhx);
                previous = neighbor;
            }
            else
            {
                previous = FindSubMoveOrigin(field);
                if (previous != null && path.IndexOfXYL(previous.Xhx, previous.Yhx, previous.Lhx) == -1)
                {
                    points = FSettings.GetMovePoints(previous.Xhx, previous.Yhx, previous.Lhx);
                    SetMapL((TMapLevel)previous.Owner);
                    found = true;
                }
            }
            if (previous != null && found)
                path.List[path.Count - 1].Flag = (sbyte)(FSettings.GetMovePoints(field.Xhx, field.Yhx, field.Lhx) - points);
            field = previous;
            failed = !found;
        }
        if (failed) path.Clear();
        else if (field != null && points == 0) path.AddXYL(field.Xhx, field.Yhx, field.Lhx);
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIExecuter.FStatus.Contains(TAIExecuterStatusElement.aesExecuting)) Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIExecuter.UseProcessTime(path.Count);
    }

    private TAoWMapField NeighborField(TAoWMapField field, int direction)
    {
        var delta = Aow.Game.World.Map.HexGeometry.HNtoXYTable[field.Xhx & 1][direction];
        return (TAoWMapField)((TMapLevel)field.Owner).GetField(field.Xhx + delta.X, field.Yhx + delta.Y);
    }

    public int CalculateMovePointLine(TAoWMapField origin, int direction, int moveOverCode)
    {
        int initialOverCode = moveOverCode;
        int count = 0, points = FSettings.GetMovePoints(origin.Xhx, origin.Yhx, origin.Lhx);
        var field = origin;
        bool continueMove = true;
        while (continueMove)
        {
            field = NeighborField(field, direction);
            int cost = FOnMovePoint(field, moveOverCode);
            if (cost < 0) break;
            points += cost;
            int old = FSettings.GetMovePoints(field.Xhx, field.Yhx, field.Lhx);
            if (old <= points || points > FMovePointsLimit) break;
            if (FMode.Contains(TMoveModeElement.mmLogProcessedMapFields) && old == 0xffffff) FProcessedMapFields.Add(field);
            if (field.Xhx == FDestinationX && field.Yhx == FDestinationY && field.Lhx == FDestinationL) FMovePointsLimit = points;
            FSettings.SetMovePoints(field.Xhx, field.Yhx, field.Lhx, points);
            if ((field.FFieldFlags & 0x2000) == 0) moveOverCode = 1;
            else moveOverCode = field.CanMoveOver(this);
            continueMove = moveOverCode != 0;
            if (continueMove) count++;
        }
        HeadlessHooks.MoveLine?.Invoke(origin.GetXYL(), direction, initialOverCode, count);
        return count;
    }

    public void ProcessHexagon(TAoWMapField field, int direction, int count)
    {
        FProcessedCount++;
        int over = (field.FFieldFlags & 0x2000) == 0 ? 1 : field.CanMoveOver(this);
        int left = Aow.Game.World.Movement.TMoveControl.P5SideDirections[direction][0], right = Aow.Game.World.Movement.TMoveControl.P5SideDirections[direction][1];
        int leftCount = CalculateMovePointLine(field, left, over);
        int rightCount = CalculateMovePointLine(field, right, over);
        if ((count & 0xff) != 0) ProcessHexagon(NeighborField(field, direction), direction, over == 2 ? count + 0xff : count - 1);
        if (leftCount != 0) ProcessHexagon(NeighborField(field, left), left, over == 2 ? leftCount + 0xff : leftCount - 1);
        if (rightCount != 0) ProcessHexagon(NeighborField(field, right), right, over == 2 ? rightCount + 0xff : rightCount - 1);
        if ((count & 0x100) != 0)
        {
            int opposite = Aow.Game.World.Movement.TMoveControl.P5OppositeDirections[direction];
            left = Aow.Game.World.Movement.TMoveControl.P5SideDirections[opposite][0];
            right = Aow.Game.World.Movement.TMoveControl.P5SideDirections[opposite][1];
            leftCount = CalculateMovePointLine(field, left, over);
            rightCount = CalculateMovePointLine(field, right, over);
            if (leftCount != 0) ProcessHexagon(NeighborField(field, left), left, leftCount - 1);
            if (rightCount != 0) ProcessHexagon(NeighborField(field, right), right, rightCount - 1);
        }
        if ((field.FFieldFlags & 0x8000) != 0) field.MoveExclusive(this);
    }
}
