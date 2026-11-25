#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// meeting point, adjacent armies
public class AdjacentMoveExecutorControl : AIMoveExecutorControl
{
    // AIGroup.pas:5117
    public AdjacentMoveExecutorControl(CustomNode? owner) : base(owner) => _adjacentArmies = new AdjacentArmyList(null) { Holder = CreationHolder() };

    // A path step with its cost: the meeting point is a step of a move path, and the comparison
    // with the destination covers the cost as the Delphi four-byte comparison did.
    private PathStep _meetingPoint;
    private readonly AdjacentArmyList _adjacentArmies;

    // AIGroup.pas:5123
    public override void Dispose()
    {
        _adjacentArmies?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:5239
    public override void CalculateMoves()
    {
        int i;
        AIMoveExecutorArmyList aiMoveExecutorArmyList;
        AIMoveExecutorMove? move;
        byte moveResult;
        switch (State)
        {
            case 0:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
                i = 0;
                while (true)
                {
                    bool condition = aiMoveExecutorArmyList.Count != i;
                    if (condition)
                    {
                        AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(i);
                        condition = _adjacentArmies.IndexOfLocation(armies.Location) != -1;
                    }

                    if (!condition)
                    {
                        break;
                    }

                    i += 1;
                }

                if (i < aiMoveExecutorArmyList.Count)
                {
                    MoveControlSettings settings = new(null) { Holder = CreationHolder() };
                    int armySelection = default;
                    ArmyHexSprite? armyHexSprite = default;
                    try
                    {
                        aiMoveExecutorArmyList.GetArmies(i).RetrieveArmy(255, army: out armyHexSprite, out armySelection);
                    }
                    catch (Exception error)
                    {
                        HostLog.DebugFallback(RtlHost.Logger, error: error, location: "AdjacentMoveExecutorControl.CalculateMoves");
                    }

                    // { Original suppresses a RetrieveArmy exception. }
                    // { Original uses transporter IDs as visited markers during adjacent moves. }
                    foreach (AIMoveExecutorUnit unit in aiMoveExecutorArmyList.GetArmies(i).Reverse())
                    {
                        unit.TransporterId = 0;
                    }

                    settings.Reset();
                    settings.MovePointsMax = Executor.MovePointsMax;
                    settings.SetupArmy(armyHexSprite, selection: armySelection);
                    settings.SetDestinationPosition(Executor.Destination);
                    StrategicMap.Current.MoveControl.ProcessMove(settings);
                    AdjacentArmy adjacent = new(_adjacentArmies);
                    adjacent.Path.Assign(settings.Path);
                    adjacent.Moves = armyHexSprite.Army.Moves(armySelection);
                    adjacent.MovePoints = armyHexSprite.Army.MovePoints(armySelection);
                    adjacent.Location = aiMoveExecutorArmyList.GetArmies(i).Location;
                    _ = settings.Release();
                }
                else
                {
                    // Path.Count is a field. FindLastIndex stops at the first empty path from the end.
                    i = _adjacentArmies.FindLastIndex(static army => army.Path.Count == 0);
                    if (i < 0)
                    {
                        State += 1;
                    }
                    else
                    {
                        State = 4;
                    }
                }

                aiMoveExecutorArmyList?.Dispose();
                break;
            case 1:
                if (_adjacentArmies.GetAdjacent())
                {
                    AdjacentArmy? slowestArmy = _adjacentArmies.GetSlowestArmy();
                    if (slowestArmy != null && (double)slowestArmy.GetMovementDays() <= 1.25 && Executor.Mode.Contains(AIMoveExecutorModes.Attack))
                    {
                        if (ExecuteSurroundMove())
                        {
                            State = 5;
                        }
                        else
                        {
                            _meetingPoint = new PathStep(Location: Executor.Destination, MoveCost: 0);
                            State = 2;
                        }
                    }
                    else
                    {
                        _meetingPoint = new PathStep(Location: Executor.Destination, MoveCost: 0);
                        State = 2;
                    }
                }
                else
                {
                    AdjacentArmy? slowestArmy = _adjacentArmies.GetSlowestArmy();
                    if (slowestArmy != null && (double)slowestArmy.GetMovementDays() <= 1 && Executor.Mode.Contains(AIMoveExecutorModes.Attack))
                    {
                        State = ExecuteSurroundMove() ? 5 : 3;
                    }
                    else
                    {
                        _ = ExecuteMeetingPointMove();
                        State = 3;
                    }
                }

                break;
            case 2:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
                aiMoveExecutorArmyList.SortOnMovePoints();
                if (aiMoveExecutorArmyList.Count != 0)
                {
                    CreateMoveModes mode;
                    if (Executor.Moves.Count == 0)
                    {
                        // GetArmies(index) is the enumerated army. TransporterId is a field.
                        // FindIndex returns -1 where the loop left the count.
                        int index = aiMoveExecutorArmyList.FindIndex(static army => army.GetUnits(0).TransporterId != 1);
                        if (index >= 0)
                        {
                            foreach (AIMoveExecutorUnit unit in aiMoveExecutorArmyList.GetArmies(index).Reverse())
                            {
                                unit.TransporterId = 1;
                            }

                            mode = Executor.Mode.Contains(AIMoveExecutorModes.Attack)
                                ? CreateMoveModes.DoNotQueue | CreateMoveModes.Surround
                                : CreateMoveModes.DoNotQueue;

                            (move, moveResult) = CreateMove(aiMoveExecutorArmyList.GetArmies(index), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode);
                            if (move is null)
                            {
                                if (moveResult == 1)
                                {
                                    State = 3;
                                }
                                else
                                {
                                    if (moveResult == 3)
                                    {
                                        Executor.Status = AIMoveExecutorStatus.Failed;
                                        State = 3;
                                    }
                                    else
                                    {
                                        if (moveResult != 2)
                                        {
                                            State = 3;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                move.SetOwner(Executor.Moves);
                                move.SetUnitStatus(AIMoveExecutorUnitStatus.Queued);
                                _meetingPoint = moveResult == 0 ? move.Path[0] : new PathStep(Location: Executor.Destination, MoveCost: 0);
                            }
                        }
                        else
                        {
                            _ = Executor.ControlQueue.Add(0);
                            State = 3;
                        }
                    }
                    else
                    {
                        mode = _meetingPoint == new PathStep(Location: Executor.Destination, MoveCost: 0) && Executor.Mode.Contains(AIMoveExecutorModes.Attack)
                            ? CreateMoveModes.DoNotQueue | CreateMoveModes.Surround
                            : CreateMoveModes.DoNotQueue;

                        (move, moveResult) = CreateMove(aiMoveExecutorArmyList.GetArmies(0), selection: 255, _meetingPoint.Location.X, _meetingPoint.Location.Y, _meetingPoint.Location.MapLevel, mode);
                        if (move is not null)
                        {
                            if (moveResult == 0)
                            {
                                if (Executor.Moves.Count != 0)
                                {
                                    Executor.Moves.Reset();
                                }

                                move?.Dispose();
                            }
                            else
                            {
                                move.SetOwner(Executor.Moves);
                                move.SetUnitStatus(AIMoveExecutorUnitStatus.Queued);
                            }
                        }
                        else
                        {
                            if (moveResult is 1 or 2)
                            {
                                aiMoveExecutorArmyList.GetArmies(0).SetStatus(255, status: AIMoveExecutorUnitStatus.Done);
                            }
                            else
                            {
                                if (moveResult == 3)
                                {
                                    Executor.Status = AIMoveExecutorStatus.Failed;
                                    Executor.Moves.Reset();
                                    State = 3;
                                }
                                else
                                {
                                    Executor.Moves.Reset();
                                    State = 3;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Executor.ExecuteMoves();
                    State = 3;
                }

                aiMoveExecutorArmyList?.Dispose();
                break;
            case 3:
                Done();
                break;
            case 4:
                if (Executor.Mode.Contains(AIMoveExecutorModes.Attack))
                {
                    // Path.Count is a field. FindLastIndex stops at the first path of length 2 from the end.
                    i = _adjacentArmies.FindLastIndex(static army => army.Path.Count == 2);
                    if (i >= 0)
                    {
                        State = 5;
                    }
                    else
                    {
                        Executor.Status = AIMoveExecutorStatus.Failed;
                        State = 3;
                    }
                }
                else
                {
                    Executor.Status = AIMoveExecutorStatus.Failed;
                    State = 3;
                }

                break;
            case 5:
                i = 0;
                int ready = 0;
                int failed = 0;
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                while (aiMoveExecutorArmyList.Count != i)
                {
                    (move, moveResult) = CreateMove(aiMoveExecutorArmyList.GetArmies(i), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: CreateMoveModes.DoNotQueue | CreateMoveModes.Surround);
                    bool condition = moveResult != 0;
                    if (!condition)
                    {
                        AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(i);
                        AIMoveExecutorArmy armiesArmies = aiMoveExecutorArmyList.GetArmies(i);
                        condition = HexGeometry.DeltaHexToRadius(Executor.Destination.X, Executor.Destination.Y, armiesArmies.Location.X, armies.Location.Y) == 1 && aiMoveExecutorArmyList.GetArmies(i).Location.MapLevel == Executor.Destination.MapLevel;
                    }

                    bool canMove = condition;
                    if (canMove)
                    {
                        ready += 1;
                    }
                    else
                    {
                        failed += 1;
                    }

                    move?.Dispose();

                    i += 1;
                }

                aiMoveExecutorArmyList?.Dispose();
                if (ready > 4 || failed == 0)
                {
                    i = 0;
                    move = null;
                    aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                    while (move is null && aiMoveExecutorArmyList.Count != i)
                    {
                        (move, _) = CreateMove(aiMoveExecutorArmyList.GetArmies(i), selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: CreateMoveModes.None);
                        if (move is not null)
                        {
                            Executor.ExecuteMoves();
                        }

                        i += 1;
                    }

                    aiMoveExecutorArmyList?.Dispose();
                }

                Done();
                break;
            default:
                break;
        }
    }

    // AIGroup.pas:5129
    public bool ExecuteSurroundMove()
    {
        bool result = true;
        AIMoveExecutorArmyList armies = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
        DoubleItemList<AdjacentArmy> sorted = [];
        int i = 0;
        int last = _adjacentArmies.Count - 1;
        for (; i <= last; i += 1)
        {
            float movementDays = _adjacentArmies.GetAdjacentArmies(i).GetMovementDays();
            _ = sorted.AddAbility(_adjacentArmies.GetAdjacentArmies(i), Round((double)movementDays * 100));
        }

        sorted.SortDescendingItemB();
        _ = sorted[0].First.Path[1];
        i = 0;
        while (result && i < sorted.Count)
        {
            int index = armies.IndexOfLocation(sorted[i].First.Location);
            if (index != -1)
            {
                AIMoveExecutorArmy army = armies.GetArmies(index);
                if (army.Location.MapLevel == Executor.Destination.MapLevel && HexGeometry.DeltaHexToRadius(army.Location.X, army.Location.Y, Executor.Destination.X, Executor.Destination.Y) > 1)
                {
                    int moveResult = CreateMove(army, selection: 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode: CreateMoveModes.SurroundNearest).MoveResult;
                    result = moveResult is not 0 and not 3;
                }
            }

            i += 1;
        }

        if (Executor.Moves.Count != 0)
        {
            if (result)
            {
                Executor.ExecuteMoves();
            }
            else
            {
                Executor.Moves.Reset();
            }
        }
        armies?.Dispose();
        return result;
    }

    // AIGroup.pas:5170
    public MapCoordinate CalculateMeetingPoint()
    {
        AIMoveExecutorArmyList armies = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
        MapCoordinate result = MapCoordinate.None;
        AdjacentArmy? i = _adjacentArmies.GetSlowestArmy();
        if (i != null)
        {
            if ((double)i.GetMovementDays() > 1 || !Executor.Mode.Contains(AIMoveExecutorModes.Attack))
            {
                int points = i.MovePoints;
                if (Executor.Mode.Contains(AIMoveExecutorModes.Attack))
                {
                    int reserve = Round(points * 0.75);
                    if (reserve > i.Path.GetMovePoints() - points)
                    {
                        points = i.Path.GetMovePoints() - reserve;
                    }
                }

                AdjacentArmy adjacent = i;
                int index = armies.IndexOfLocation(i.Location);
                if (index != -1)
                {
                    AIMoveExecutorArmy army = armies.GetArmies(index);
                    int position = adjacent.Path.MovePointsToPosition(points);
                    while (true)
                    {
                        bool condition = position > 1;
                        if (condition)
                        {
                            PathStep item = adjacent.Path[position];
                            PathStep fPathItem = adjacent.Path[position];
                            condition = CanMoveTo(army, selection: 255, adjacent.Path[position].Location.X, item.Location.Y, fPathItem.Location.MapLevel) != 255;
                        }

                        if (!condition)
                        {
                            break;
                        }

                        position -= 1;
                    }

                    result = i.Path[position].Location;
                }
            }
        }

        armies?.Dispose();
        return result;
    }

    // AIGroup.pas:5206
    public bool ExecuteMeetingPointMove()
    {
        bool result = true;
        MapCoordinate mapCoordinate = CalculateMeetingPoint();
        if (mapCoordinate == MapCoordinate.None)
        {
            MapCoordinate location = Executor.Destination;
            mapCoordinate = new MapCoordinate(location.X, location.Y, location.MapLevel);
        }

        AIMoveExecutorArmyList armies = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
        DoubleItemList<AdjacentArmy> sorted = [];
        for (int i = 0, last = _adjacentArmies.Count - 1; i <= last; i++)
        {
            float movementDays = _adjacentArmies.GetAdjacentArmies(i).GetMovementDays();
            _ = sorted.AddAbility(_adjacentArmies.GetAdjacentArmies(i), Round((double)movementDays * 100));
        }

        sorted.SortDescendingItemB();
        for (int i = 0, last = sorted.Count - 1; i <= last; i++)
        {
            int index = armies.IndexOfLocation(sorted[i].First.Location);
            if (index != -1)
            {
                int moveResult = CreateMove(armies.GetArmies(index), selection: 255, mapCoordinate.X, mapCoordinate.Y, mapCoordinate.MapLevel, mode: CreateMoveModes.None).MoveResult;
                if (moveResult == 0)
                {
                    result = false;
                }
            }
        }

        if (Executor.Moves.Count != 0)
        {
            Executor.ExecuteMoves();
        }
        armies?.Dispose();
        return result;
    }
}
