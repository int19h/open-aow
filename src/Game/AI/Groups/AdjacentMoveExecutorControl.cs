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
    public AdjacentMoveExecutorControl(ECustomNode owner) : base(owner)
    {
        _adjacentArmies = new AdjacentArmyList(null) { Holder = CreationHolder() };
    }

    private MapCoordinate _meetingPoint;
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
        int I;
        AIMoveExecutorArmyList Armies;
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
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                I = 0;
                while (true)
                {
                    bool condition = Armies.Count != I;
                    if (condition)
                    {
                        var armies = Armies.GetArmies(I);
                        condition = _adjacentArmies.IndexOfLocation(armies.Location) != -1;
                    }

                    if (!condition)
                        break;
                    I += 1;
                }

                if (I < Armies.Count)
                {
                    settings = new MoveControlSettings(null) { Holder = CreationHolder() };
                    try
                    {
                        Armies.GetArmies(I).RetrieveArmy(255, out armyHexSprite, out armySelection);
                    }
                    catch
                    {
                    }

                    // { Original suppresses a RetrieveArmy exception. }
                    // { Original uses transporter IDs as visited markers during adjacent moves. }
                    for (int J = Armies.GetArmies(I).Count - 1; J >= 0; J--)
                    {
                        Armies.GetArmies(I).GetUnits(J).TransporterId = 0;
                    }

                    settings.Reset();
                    settings.MovePointsMax = Executor.MovePointsMax;
                    settings.SetupArmy(armyHexSprite, armySelection);
                    settings.SetDestinationPosition(Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel);
                    StrategicMap.Current.MoveControl.ProcessMove(settings);
                    adjacent = new AdjacentArmy(_adjacentArmies);
                    adjacent.Path.Assign(settings.Path);
                    adjacent.Moves = armyHexSprite.Army.Moves(armySelection);
                    adjacent.MovePoints = armyHexSprite.Army.MovePoints(armySelection);
                    adjacent.Location = Armies.GetArmies(I).Location;
                    settings.Release();
                }
                else
                {
                    I = _adjacentArmies.Count - 1;
                    while (I >= 0 && _adjacentArmies.GetAdjacentArmies(I).Path.Count > 0)
                    {
                        I -= 1;
                    }

                    if (I == -1)
                    {
                        State += 1;
                    }
                    else
                    {
                        State = 4;
                    }
                }

                Armies?.Dispose();
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
                            _meetingPoint = Executor.Destination;
                            State = 2;
                        }
                    }
                    else
                    {
                        _meetingPoint = Executor.Destination;
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
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                Armies.SortOnMovePoints();
                if (Armies.Count != 0)
                {
                    if (Executor.Moves.Count == 0)
                    {
                        index = 0;
                        while (index < Armies.Count && Armies.GetArmies(index).GetUnits(0).TransporterId == 1)
                        {
                            index += 1;
                        }

                        if (index < Armies.Count)
                        {
                            for (int J = Armies.GetArmies(index).Count - 1; J >= 0; J--)
                            {
                                Armies.GetArmies(index).GetUnits(J).TransporterId = 1;
                            }

                            if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                            {
                                mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround);
                            }
                            else
                            {
                                mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue);
                            }

                            move = CreateMove(Armies.GetArmies(index), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, mode, out moveResult);
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
                                    _meetingPoint = Executor.Destination;
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
                        if (_meetingPoint == Executor.Destination && Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                        {
                            mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround);
                        }
                        else
                        {
                            mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue);
                        }

                        move = CreateMove(Armies.GetArmies(0), 255, _meetingPoint.X, _meetingPoint.Y, _meetingPoint.MapLevel, mode, out moveResult);
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
                                Armies.GetArmies(0).SetStatus(255, AIMoveExecutorUnitStatus.Done);
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

                Armies?.Dispose();
                break;
            case 3:
                Done();
                break;
            case 4:
                if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                {
                    I = _adjacentArmies.Count - 1;
                    while (I >= 0 && _adjacentArmies.GetAdjacentArmies(I).Path.Count != 2)
                    {
                        I -= 1;
                    }

                    if (I != -1)
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
                I = 0;
                ready = 0;
                failed = 0;
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                while (Armies.Count != I)
                {
                    move = CreateMove(Armies.GetArmies(I), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DoNotQueue, CreateMoveModeElement.Surround), out moveResult);
                    bool condition = moveResult != 0;
                    if (!condition)
                    {
                        var armies = Armies.GetArmies(I);
                        var armiesArmies = Armies.GetArmies(I);
                        condition = HexGeometry.DeltaHexToRadius(Executor.Destination.X, Executor.Destination.Y, armiesArmies.Location.X, armies.Location.Y) == 1 && Armies.GetArmies(I).Location.MapLevel == Executor.Destination.MapLevel;
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

                    I += 1;
                }

                Armies?.Dispose();
                if (ready > 4 || failed == 0)
                {
                    I = 0;
                    move = null;
                    Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                    while (move == null && Armies.Count != I)
                    {
                        move = CreateMove(Armies.GetArmies(I), 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(), out moveResult);
                        if (move != null)
                        {
                            Executor.ExecuteMoves();
                        }

                        I += 1;
                    }

                    Armies?.Dispose();
                }

                Done();
                break;
        }
    }

    // AIGroup.pas:5129
    public bool ExecuteSurroundMove()
    {
        int I;
        int index;
        AIMoveExecutorArmyList armies;
        DoubleItemList<AdjacentArmy> sorted;
        AIMoveExecutorArmy army;
        byte moveResult = default;
        bool result = true;
        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
        sorted = new DoubleItemList<AdjacentArmy>();
        I = 0;
        int last = _adjacentArmies.Count - 1;
        for (; I <= last; I += 1)
        {
            var movementDays = _adjacentArmies.GetAdjacentArmies(I).GetMovementDays();
            sorted.AddAbility(_adjacentArmies.GetAdjacentArmies(I), Round((double)movementDays * 100));
        }

        sorted.SortDescendingItemB();
        _ = sorted[0].First.Path[1];
        I = 0;
        while (result && I < sorted.Count)
        {
            index = armies.IndexOfLocation(sorted[I].First.Location);
            if (index != -1)
            {
                army = armies.GetArmies(index);
                if (army.Location.MapLevel == Executor.Destination.MapLevel && HexGeometry.DeltaHexToRadius(army.Location.X, army.Location.Y, Executor.Destination.X, Executor.Destination.Y) > 1)
                {
                    CreateMove(army, 255, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.SurroundNearest), out moveResult);
                    result = moveResult != 0 && moveResult != 3;
                }
            }

            I += 1;
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
        int I;
        int points;
        int reserve;
        int index;
        int position;
        AIMoveExecutorArmyList armies;
        AdjacentArmy adjacent;
        AIMoveExecutorArmy army;
        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
        MapCoordinate result = MapCoordinate.None;
        I = _adjacentArmies.GetSlowestArmy();
        if (I != -1)
        {
            if ((double)_adjacentArmies.GetAdjacentArmies(I).GetMovementDays() > 1 || !Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
            {
                points = _adjacentArmies.GetAdjacentArmies(I).MovePoints;
                if (Executor.Mode.Contains(AIMoveExecutorModeElement.Attack))
                {
                    reserve = Round((double)points * 0.75);
                    if (reserve > _adjacentArmies.GetAdjacentArmies(I).Path.GetMovePoints() - points)
                    {
                        points = _adjacentArmies.GetAdjacentArmies(I).Path.GetMovePoints() - reserve;
                    }
                }

                adjacent = _adjacentArmies.GetAdjacentArmies(I);
                index = armies.IndexOfLocation(_adjacentArmies.GetAdjacentArmies(I).Location);
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
                            condition = CanMoveTo(army, 255, adjacent.Path[position].X, item.Y, fPathItem.MapLevel) != 255;
                        }

                        if (!condition)
                            break;
                        position -= 1;
                    }

                    result = _adjacentArmies.GetAdjacentArmies(I).Path[position];
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
        MapCoordinate Location = default;
        bool result = true;
        Location = CalculateMeetingPoint();
        if (Location == MapCoordinate.None)
        {
            ref var location = ref Executor.Destination;
            Location = HexGeometry.PackMapCoordinate(location.X, location.Y, location.MapLevel);
        }

        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
        sorted = new DoubleItemList<AdjacentArmy>();
        for (int I = 0, last = _adjacentArmies.Count - 1; I <= last; I++)
        {
            var movementDays = _adjacentArmies.GetAdjacentArmies(I).GetMovementDays();
            sorted.AddAbility(_adjacentArmies.GetAdjacentArmies(I), Round((double)movementDays * 100));
        }

        sorted.SortDescendingItemB();
        for (int I = 0, last = sorted.Count - 1; I <= last; I++)
        {
            index = armies.IndexOfLocation(sorted[I].First.Location);
            if (index != -1)
            {
                CreateMove(armies.GetArmies(index), 255, Location.X, Location.Y, Location.MapLevel, Set<CreateMoveModeElement>.Of(), out moveResult);
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
