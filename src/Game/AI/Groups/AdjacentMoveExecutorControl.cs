#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// meeting point, adjacent armies
public partial class AdjacentMoveExecutorControl : AIMoveExecutorControl
{
    // AIGroup.pas:5117
    public AdjacentMoveExecutorControl(CustomNode owner) : base(owner)
    {
        _adjacentArmies = new AdjacentArmyList(null) { Holder = CreationHolder() };
    }

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
        int index;
        int ready;
        int failed;
        int i;
        AIMoveExecutorArmyList aiMoveExecutorArmyList;
        byte moveResult = default;
        MoveControlSettings settings;
        byte armySelection = default;
        ArmyHexSprite armyHexSprite = default;
        AdjacentArmy adjacent;
        AIMoveExecutorMove move;
        Set<CreateMoveModeElement> mode;
        bool canMove;
        switch (State)
        {
            case 0:
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                i = 0;
                while (true)
                {
                    bool condition = aiMoveExecutorArmyList.Count != i;
                    if (condition)
                    {
                        var armies = aiMoveExecutorArmyList.GetArmies(i);
                        condition = _adjacentArmies.IndexOfLocation(armies.Location) != -1;
                    }

                    if (!condition)
                        break;
                    i += 1;
                }

                if (i < aiMoveExecutorArmyList.Count)
                {
                    settings = new MoveControlSettings(null) { Holder = CreationHolder() };
                    try
                    {
                        aiMoveExecutorArmyList.GetArmies(i).RetrieveArmy(255, out armyHexSprite, out armySelection);
                    }
                    catch
                    {
                    }

                    // { Original suppresses a RetrieveArmy exception. }
                    // { Original uses transporter IDs as visited markers during adjacent moves. }
                    for (int j = aiMoveExecutorArmyList.GetArmies(i).Count - 1; j >= 0; j--)
                    {
                        aiMoveExecutorArmyList.GetArmies(i).GetUnits(j).TransporterId = 0;
                    }

                    settings.Reset();
                    settings.MovePointsMax = Executor.MovePointsMax;
                    settings.SetupArmy(armyHexSprite, armySelection);
                    settings.SetDestinationPosition(Executor.Destination);
                    StrategicMap.Current.MoveControl.ProcessMove(settings);
                    adjacent = new AdjacentArmy(_adjacentArmies);
                    adjacent.Path.Assign(settings.Path);
                    adjacent.Moves = armyHexSprite.Army.Moves(armySelection);
                    adjacent.MovePoints = armyHexSprite.Army.MovePoints(armySelection);
                    adjacent.Location = aiMoveExecutorArmyList.GetArmies(i).Location;
                    settings.Release();
                }
                else
                {
                    i = _adjacentArmies.Count - 1;
                    while (i >= 0 && _adjacentArmies.GetAdjacentArmies(i).Path.Count > 0)
                    {
                        i -= 1;
                    }

                    if (i == -1)
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
                    var slowestArmy = _adjacentArmies.GetSlowestArmy();
                    if ((double)_adjacentArmies.GetAdjacentArmies(slowestArmy).GetMovementDays() <= 1.25 && Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
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
                    var slowestArmy = _adjacentArmies.GetSlowestArmy();
                    if ((double)_adjacentArmies.GetAdjacentArmies(slowestArmy).GetMovementDays() <= 1 && Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                    {
                        if (ExecuteSurroundMove())
                        {
                            State = 5;
                        }
                        else
                        {
                            State = 3;
                        }
                    }
                    else
                    {
                        ExecuteMeetingPointMove();
                        State = 3;
                    }
                }

                break;
            case 2:
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                aiMoveExecutorArmyList.SortOnMovePoints();
                if (aiMoveExecutorArmyList.Count != 0)
                {
                    if (Executor.Moves.Count == 0)
                    {
                        index = 0;
                        while (index < aiMoveExecutorArmyList.Count && aiMoveExecutorArmyList.GetArmies(index).GetUnits(0).TransporterId == 1)
                        {
                            index += 1;
                        }

                        if (index < aiMoveExecutorArmyList.Count)
                        {
                            for (int j = aiMoveExecutorArmyList.GetArmies(index).Count - 1; j >= 0; j--)
                            {
                                aiMoveExecutorArmyList.GetArmies(index).GetUnits(j).TransporterId = 1;
                            }

                            if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                            {
                                mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround);
                            }
                            else
                            {
                                mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue);
                            }

                            move = CreateMove(aiMoveExecutorArmyList.GetArmies(index), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode, out moveResult);
                            if (move == null)
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
                                if (moveResult == 0)
                                {
                                    _meetingPoint = move.Path[0];
                                }
                                else
                                {
                                    _meetingPoint = new PathStep(Location: Executor.Destination, MoveCost: 0);
                                }
                            }
                        }
                        else
                        {
                            Executor.ControlQueue.Add(0);
                            State = 3;
                        }
                    }
                    else
                    {
                        if (_meetingPoint == new PathStep(Location: Executor.Destination, MoveCost: 0) && Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                        {
                            mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround);
                        }
                        else
                        {
                            mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue);
                        }

                        move = CreateMove(aiMoveExecutorArmyList.GetArmies(0), 255, _meetingPoint.Location.X, _meetingPoint.Location.Y, _meetingPoint.Location.MapLevel, mode, out moveResult);
                        if (move != null)
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
                            if (moveResult == 1 || moveResult == 2)
                            {
                                aiMoveExecutorArmyList.GetArmies(0).SetStatus(255, AIMoveExecutorUnitStatus.Done);
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
                if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                {
                    i = _adjacentArmies.Count - 1;
                    while (i >= 0 && _adjacentArmies.GetAdjacentArmies(i).Path.Count != 2)
                    {
                        i -= 1;
                    }

                    if (i != -1)
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
                ready = 0;
                failed = 0;
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                while (aiMoveExecutorArmyList.Count != i)
                {
                    move = CreateMove(aiMoveExecutorArmyList.GetArmies(i), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround), out moveResult);
                    bool condition = moveResult != 0;
                    if (!condition)
                    {
                        var armies = aiMoveExecutorArmyList.GetArmies(i);
                        var armiesArmies = aiMoveExecutorArmyList.GetArmies(i);
                        condition = HexGeometry.DeltaHexToRadius(Executor.Destination.X, Executor.Destination.Y, armiesArmies.Location.X, armies.Location.Y) == 1 && aiMoveExecutorArmyList.GetArmies(i).Location.MapLevel == Executor.Destination.MapLevel;
                    }

                    canMove = condition;
                    if (canMove)
                    {
                        ready += 1;
                    }
                    else
                    {
                        failed += 1;
                    }

                    if (move != null)
                    {
                        move?.Dispose();
                    }

                    i += 1;
                }

                aiMoveExecutorArmyList?.Dispose();
                if (ready > 4 || failed == 0)
                {
                    i = 0;
                    move = null;
                    aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                    while (move == null && aiMoveExecutorArmyList.Count != i)
                    {
                        move = CreateMove(aiMoveExecutorArmyList.GetArmies(i), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(), out moveResult);
                        if (move != null)
                        {
                            Executor.ExecuteMoves();
                        }

                        i += 1;
                    }

                    aiMoveExecutorArmyList?.Dispose();
                }

                Done();
                break;
        }
    }

    // AIGroup.pas:5129
    public bool ExecuteSurroundMove()
    {
        int i;
        int index;
        AIMoveExecutorArmyList armies;
        DoubleItemList<AdjacentArmy> sorted;
        AIMoveExecutorArmy army;
        byte moveResult = default;
        bool result = true;
        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
        sorted = new DoubleItemList<AdjacentArmy>();
        i = 0;
        int last = _adjacentArmies.Count - 1;
        for (; i <= last; i += 1)
        {
            var movementDays = _adjacentArmies.GetAdjacentArmies(i).GetMovementDays();
            sorted.AddAbility(_adjacentArmies.GetAdjacentArmies(i), Round((double)movementDays * 100));
        }

        sorted.SortDescendingItemB();
        _ = sorted[0].First.Path[1];
        i = 0;
        while (result && i < sorted.Count)
        {
            index = armies.IndexOfLocation(sorted[i].First.Location);
            if (index != -1)
            {
                army = armies.GetArmies(index);
                if (army.Location.MapLevel == Executor.Destination.MapLevel && HexGeometry.DeltaHexToRadius(army.Location.X, army.Location.Y, Executor.Destination.X, Executor.Destination.Y) > 1)
                {
                    CreateMove(army, 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.SurroundNearest), out moveResult);
                    result = moveResult != 0 && moveResult != 3;
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

        sorted?.Dispose();
        armies?.Dispose();
        return result;
    }

    // AIGroup.pas:5170
    public MapCoordinate CalculateMeetingPoint()
    {
        int i;
        int points;
        int reserve;
        int index;
        int position;
        AIMoveExecutorArmyList armies;
        AdjacentArmy adjacent;
        AIMoveExecutorArmy army;
        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
        MapCoordinate result = MapCoordinate.None;
        i = _adjacentArmies.GetSlowestArmy();
        if (i != -1)
        {
            if ((double)_adjacentArmies.GetAdjacentArmies(i).GetMovementDays() > 1 || !Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
            {
                points = _adjacentArmies.GetAdjacentArmies(i).MovePoints;
                if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                {
                    reserve = Round((double)points * 0.75);
                    if (reserve > _adjacentArmies.GetAdjacentArmies(i).Path.GetMovePoints() - points)
                    {
                        points = _adjacentArmies.GetAdjacentArmies(i).Path.GetMovePoints() - reserve;
                    }
                }

                adjacent = _adjacentArmies.GetAdjacentArmies(i);
                index = armies.IndexOfLocation(_adjacentArmies.GetAdjacentArmies(i).Location);
                if (index != -1)
                {
                    army = armies.GetArmies(index);
                    position = adjacent.Path.MovePointsToPosition(points);
                    while (true)
                    {
                        bool condition = position > 1;
                        if (condition)
                        {
                            var item = adjacent.Path[position];
                            var fPathItem = adjacent.Path[position];
                            condition = CanMoveTo(army, 255, adjacent.Path[position].Location.X, item.Location.Y, fPathItem.Location.MapLevel) != 255;
                        }

                        if (!condition)
                            break;
                        position -= 1;
                    }

                    result = _adjacentArmies.GetAdjacentArmies(i).Path[position].Location;
                }
            }
        }

        armies?.Dispose();
        return result;
    }

    // AIGroup.pas:5206
    public bool ExecuteMeetingPointMove()
    {
        int index;
        AIMoveExecutorArmyList armies;
        DoubleItemList<AdjacentArmy> sorted;
        byte moveResult = default;
        MapCoordinate mapCoordinate = default;
        bool result = true;
        mapCoordinate = CalculateMeetingPoint();
        if (mapCoordinate == MapCoordinate.None)
        {
            var location = Executor.Destination;
            mapCoordinate = HexGeometry.PackMapCoordinate(location.X, location.Y, location.MapLevel);
        }

        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
        sorted = new DoubleItemList<AdjacentArmy>();
        for (int i = 0, last = _adjacentArmies.Count - 1; i <= last; i++)
        {
            var movementDays = _adjacentArmies.GetAdjacentArmies(i).GetMovementDays();
            sorted.AddAbility(_adjacentArmies.GetAdjacentArmies(i), Round((double)movementDays * 100));
        }

        sorted.SortDescendingItemB();
        for (int i = 0, last = sorted.Count - 1; i <= last; i++)
        {
            index = armies.IndexOfLocation(sorted[i].First.Location);
            if (index != -1)
            {
                CreateMove(armies.GetArmies(index), 255, mapCoordinate.X, mapCoordinate.Y, mapCoordinate.MapLevel, Set<CreateMoveModeElement>.Of(), out moveResult);
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

        sorted?.Dispose();
        armies?.Dispose();
        return result;
    }

    // intf_P5_06.inc:413

}
