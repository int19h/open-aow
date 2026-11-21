#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// meeting point
public class TransportMoveExecutorControl(CustomNode? owner) : AIMoveExecutorControl(owner)
{

    // A path step with its cost, copied to the units' arrival points (see AIMoveExecutorUnit).
    private PathStep _meetingPoint;

    // AIGroup.pas:5741
    public override void CalculateMoves()
    {
        AIMoveExecutorArmyList aiMoveExecutorArmyList;
        int i;
        int timeLeft;
        int index;
        byte selection;
        AIMoveExecutorArmy army;
        AIMoveExecutorMove? move;
        switch (State)
        {
            case 0:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                i = 0;
                timeLeft = 5;
                while (i < aiMoveExecutorArmyList.Count && MoveControl.Status != AIMoveControlStatus.Processing && timeLeft > 2)
                {
                    if (aiMoveExecutorArmyList.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Idle)
                    {
                        AIMoveControlSettings settings = new(null, done: CalculateTransportPathCallback) { Holder = CreationHolder() };
                        aiMoveExecutorArmyList.GetArmies(i).RetrieveArmy(255, army: out ArmyHexSprite armyHexSprite, armySelection: out selection);
                        settings.Setup(armyHexSprite, selection, Executor.Destination.X, Executor.Destination.Y, L: Executor.Destination.MapLevel);
                        settings.Transporter = Executor.Transporter.Troop;
                        settings.MoveMode = MoveModes.AI;
                        MoveControl.Reset();
                        _ = MoveControl.CanMove(settings);
                        if (MoveControl.Status != AIMoveControlStatus.Processing)
                        {
                            timeLeft = StrategicMap.Current.AIExecutor.ProcessTimeLeft;
                        }

                        _ = settings.Release();
                    }

                    i += 1;
                }

                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Idle);
                if (index == -1)
                {
                    if (!TransportDone())
                    {
                        State += 1;
                        UpdateTransportMeetingPoint();
                    }
                    else
                    {
                        Done();
                    }
                }

                aiMoveExecutorArmyList?.Dispose();
                break;
            case 1:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Arriving);
                if (index != -1)
                {
                    PathStep location = aiMoveExecutorArmyList.GetArmies(index).ArrivalPoint;
                    (move, _) = CreateMove(aiMoveExecutorArmyList.GetArmies(index), selection: 255, location.Location.X, location.Location.Y, location.Location.MapLevel, mode: CreateMoveModes.None);
                    aiMoveExecutorArmyList.GetArmies(index).SetTransportStatus(255, status: AIMoveExecutorTransportStatus.Done);
                    // { The original executes queued moves only when creation returns nil. }
                    if (move is null)
                    {
                        Executor.ExecuteMoves();
                    }
                }
                else
                {
                    State += 1;
                }

                aiMoveExecutorArmyList?.Dispose();
                break;
            case 2:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                bool condition = index != -1;
                if (condition)
                {
                    AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(index);
                    condition = armies.TransportSource == Executor.Transporter.Location;
                }

                if (condition)
                {
                    i = 0;
                    while (i < aiMoveExecutorArmyList.Count && MoveControl.Status != AIMoveControlStatus.Processing)
                    {
                        if (aiMoveExecutorArmyList.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Dumping)
                        {
                            DumpTransportUnits(aiMoveExecutorArmyList.GetArmies(i));
                        }

                        i += 1;
                    }

                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Dumping);
                    if (index == -1)
                    {
                        if (!TransportDone())
                        {
                            State += 1;
                        }
                        else
                        {
                            Done();
                        }
                    }
                }
                else
                {
                    State += 1;
                }

                aiMoveExecutorArmyList?.Dispose();
                break;
            case 3:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Transporting);
                bool test = index != -1;
                if (test)
                {
                    AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(index);
                    AIMoveExecutorArmy armiesArmies = aiMoveExecutorArmyList.GetArmies(index);
                    test = armies.TransportDestination == armiesArmies.Location;
                }

                if (test)
                {
                    AIMoveExecutorArmyList transporters = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                    i = 0;
                    while (i != transporters.Count)
                    {
                        bool passed = transporters.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Done;
                        if (passed)
                        {
                            AIMoveExecutorArmy armies = transporters.GetArmies(i);
                            AIMoveExecutorArmy transportersArmies = transporters.GetArmies(i);
                            AIMoveExecutorArmy armies2 = transporters.GetArmies(i);
                            passed = CanMoveTo(aiMoveExecutorArmyList.GetArmies(index), selection: 255, armies2.Location.X, armies.Location.Y, transportersArmies.Location.MapLevel) == 255;
                        }

                        if (passed)
                        {
                            break;
                        }

                        i += 1;
                    }

                    if (i != transporters.Count)
                    {
                        AIMoveExecutorArmy aiMoveExecutorArmy = transporters.GetArmies(i);
                        AIMoveExecutorArmy armies = transporters.GetArmies(i);
                        AIMoveExecutorArmy transportersArmies = transporters.GetArmies(i);
                        AIMoveExecutorArmy armies2 = transporters.GetArmies(i);
                        (move, _) = CreateMove(aiMoveExecutorArmyList.GetArmies(index), selection: 255, armies2.Location.X, armies.Location.Y, transportersArmies.Location.MapLevel, mode: CreateMoveModes.None);
                    }
                    else
                    {
                        PathStep location = aiMoveExecutorArmyList.GetArmies(index).ArrivalPoint;
                        (move, _) = CreateMove(aiMoveExecutorArmyList.GetArmies(index), selection: 255, location.Location.X, location.Location.Y, location.Location.MapLevel, mode: CreateMoveModes.None);
                    }

                    if (move is null)
                    {
                        aiMoveExecutorArmyList.GetArmies(index).SetStatus(255, status: AIMoveExecutorUnitStatus.Done);
                        Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                    }
                    else
                    {
                        aiMoveExecutorArmyList.GetArmies(index).SetTransportStatus(255, status: AIMoveExecutorTransportStatus.Done);
                        Executor.ExecuteMoves();
                    }

                    transporters?.Dispose();
                }

                aiMoveExecutorArmyList?.Dispose();
                State += 1;
                break;
            case 4:
                if (Executor.Transporter.Status == AIMoveExecutorUnitStatus.Ready)
                {
                    aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Done);
                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Transporting);
                    if (index != -1)
                    {
                        MapCoordinate location = aiMoveExecutorArmyList.GetArmies(index).TransportDestination;
                        move = CreateTransporterMove(location.X, location.Y, L: location.MapLevel, queue: true);

                        if (move is null)
                        {
                            aiMoveExecutorArmyList.GetArmies(index).SetStatus(255, status: AIMoveExecutorUnitStatus.Done);
                            Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                        }
                    }

                    aiMoveExecutorArmyList?.Dispose();
                }

                State += 1;
                break;
            case 5:
                aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
                i = 0;
                timeLeft = 5;
                while (timeLeft > 2 && i != aiMoveExecutorArmyList.Count)
                {
                    if (aiMoveExecutorArmyList.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Boarding)
                    {
                        army = aiMoveExecutorArmyList.GetArmies(i);
                        if (army.BoardingPoint.X != -1 && army.Location != army.BoardingPoint)
                        {
                            (move, _) = CreateMove(aiMoveExecutorArmyList.GetArmies(i), selection: 255, army.BoardingPoint.X, army.BoardingPoint.Y, army.BoardingPoint.MapLevel, mode: CreateMoveModes.None);
                            if (move is null)
                            {
                                army.SetStatus(255, status: AIMoveExecutorUnitStatus.Done);
                            }

                            timeLeft = StrategicMap.Current.AIExecutor.ProcessTimeLeft;
                        }
                    }

                    i += 1;
                }

                if (i == aiMoveExecutorArmyList.Count)
                {
                    State += 1;
                }

                aiMoveExecutorArmyList?.Dispose();
                break;
            case 6:
                if (Executor.Transporter.Status == AIMoveExecutorUnitStatus.Ready)
                {
                    aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Queued | AIMoveExecutorUnitStatuses.Done);
                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                    if (index != -1)
                    {
                        AIMoveExecutorArmy aiMoveExecutorArmy = aiMoveExecutorArmyList.GetArmies(index);
                        if (Executor.Transporter.Location != aiMoveExecutorArmy.TransportSource)
                        {
                            move = CreateTransporterMove(aiMoveExecutorArmy.TransportSource.X, aiMoveExecutorArmy.TransportSource.Y, L: aiMoveExecutorArmy.TransportSource.MapLevel, queue: true);
                            if (move is null)
                            {
                                Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                            }
                        }
                    }

                    aiMoveExecutorArmyList?.Dispose();
                }

                State += 1;
                break;
            case 7:
                if (Executor.Transporter.Status == AIMoveExecutorUnitStatus.Ready)
                {
                    aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready | AIMoveExecutorUnitStatuses.Queued | AIMoveExecutorUnitStatuses.Done);
                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                    if (index != -1)
                    {
                        army = aiMoveExecutorArmyList.GetArmies(index);
                        if (army.WaitingForTransport() && Executor.Transporter.Location == army.TransportSource)
                        {
                            selection = CanMoveTo(aiMoveExecutorArmyList.GetArmies(index), selection: 255, army.TransportSource.X, army.TransportSource.Y, army.TransportSource.MapLevel);
                            if (selection != 0)
                            {
                                (move, _) = CreateMove(aiMoveExecutorArmyList.GetArmies(index), selection, army.TransportSource.X, army.TransportSource.Y, army.TransportSource.MapLevel, mode: CreateMoveModes.None);
                                if (move is null)
                                {
                                    Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                                }
                                else
                                {
                                    aiMoveExecutorArmyList.GetArmies(index).SetTransportStatus(selection, status: AIMoveExecutorTransportStatus.Transporting);
                                }
                            }
                            else
                            {
                                Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                            }
                        }
                    }

                    aiMoveExecutorArmyList?.Dispose();
                }

                State += 1;
                break;
            case 8:
                if (Executor.Moves.Count == 0)
                {
                    Done();
                }
                else
                {
                    State = 1;
                    Executor.ExecuteMoves();
                }

                break;
            default:
                break;
        }
    }

    // AIGroup.pas:5543
    public override void Done()
    {
        if (!TransportDone())
        {
            Executor.Status = AIMoveExecutorStatus.OutOfMovePoints;
        }

        for (int i = Executor.Units.Count - 1; i >= 0; i--)
        {
            AIMoveExecutorUnit units = Executor.Units.GetUnits(i);
            if (units.TransportStatus is AIMoveExecutorTransportStatus.Guarding or AIMoveExecutorTransportStatus.Dumped)
            {
                Executor.Units.GetUnits(i)?.Dispose();
            }
        }

        State = -1;
    }

    // AIGroup.pas:5465
    public AIMoveExecutorMove? CreateTransporterMove(sbyte X, sbyte Y, sbyte L, bool queue)
    {
        AIMoveExecutorMove? move = null;
        AIMoveExecutorArmy army = new(null) { Holder = CreationHolder() };
        army.Setup(Executor.Transporter);
        for (int i = 0, last = Executor.Units.Count - 1; i <= last; i++)
        {
            AIMoveExecutorUnit unitEntry = Executor.Units.GetUnits(i);
            if (unitEntry.Location == Executor.Transporter.Location)
            {
                _ = army.AddChild(unitEntry);
            }
        }

        AIMoveExecutorUnit? aiMoveExecutorUnit = Executor.Transporter;
        ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(aiMoveExecutorUnit.Location.X, aiMoveExecutorUnit.Location.Y, aiMoveExecutorUnit.Location.MapLevel).GetArmyHexSprite();

        MoveControlSettings settings = new(null) { Holder = CreationHolder() };
        settings.SetupArmy(armyHexSprite, selection: 255);
        settings.SetDestinationPosition(X, Y, L);
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        if (settings.Path.Count != 0)
        {
            int movePoints = armyHexSprite.Army.MovePoints(255);
            int position = settings.Path.MovePointsToPosition(movePoints);
            if (position < settings.Path.Count - 1)
            {
                while (true)
                {
                    bool condition = position != settings.Path.Count - 1;
                    if (condition)
                    {
                        PathStep item = settings.Path[position];
                        PathStep fPathItem = settings.Path[position];
                        condition = CanMoveTo(army, selection: 255, settings.Path[position].Location.X, item.Location.Y, fPathItem.Location.MapLevel) != 255;
                    }

                    if (!condition)
                    {
                        break;
                    }

                    position += 1;
                }

                if (position < settings.Path.Count - 1)
                {
                    move = new AIMoveExecutorMove(null) { Holder = CreationHolder() };
                    move.AddUnits(army, selection: 255);
                    for (int i = position, last = settings.Path.Count - 1; i <= last; i++)
                    {
                        PathStep item = settings.Path[i];
                        _ = move.Path.Add(item);
                    }

                    move.MovePoints = movePoints;
                    move.Moves = armyHexSprite.Army.Moves(255);
                    move.Source = new MapCoordinate(armyHexSprite.HexX, armyHexSprite.HexY, armyHexSprite.HexLevel);
                    move.Destination = new MapCoordinate(X, Y, L);
                    if (queue)
                    {
                        move.Owner = Executor.Moves;
                        move.SetUnitStatus(AIMoveExecutorUnitStatus.Queued);
                    }
                }
            }
        }

        army?.Dispose();
        _ = settings.Release();
        return move;
    }

    // AIGroup.pas:5525
    public bool TransportDone()
    {
        AIMoveExecutor executor = Executor;
        int i = executor.Units.Count;
        bool done = true;
        while (i != 0 && done)
        {
            i -= 1;
            AIMoveExecutorUnit units = executor.Units.GetUnits(i);
            if (units.TransportStatus is not (>= AIMoveExecutorTransportStatus.Arriving and <= AIMoveExecutorTransportStatus.Done))
            {
                done = false;
            }
        }

        return done;
    }

    // AIGroup.pas:5553
    public void DumpTransportUnits(AIMoveExecutorArmy army)
    {
        byte transporting = 0;
        byte others = 0;
        int i = 0;
        int last = army.Count - 1;
        for (; i <= last; i += 1)
        {
            if (army.GetUnits(i).Troop.GetTransporting())
            {
                transporting = (byte)(transporting | (1 << i));
            }
            else
            {
                others = (byte)(others | (1 << i));
            }
        }

        byte selection;
        ArmyHexSprite armyHexSprite;
        MoveControlSettings settings;
        MapField? field;
        bool found;
        if (transporting != 0)
        {
            settings = new MoveControlSettings(null) { Holder = CreationHolder() };
            army.RetrieveArmy(transporting, army: out armyHexSprite, armySelection: out selection);
            settings.SetupArmy(armyHexSprite, selection);
            settings.MovePointsMax = armyHexSprite.Army.Moves(selection);
            settings.Mode |= MoveModes.LogProcessedMapFields;
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            DelphiList<MapField> fields = settings.ProcessedMapFields;
            found = false;
            i = 0;
            field = null;
            while (!found && i != fields.Count)
            {
                field = fields.List[i];
                if (army.Location.X != field.HexX || army.Location.Y != field.HexY || army.Location.MapLevel != field.HexLevel)
                {
                    found = CanMoveTo(army, selection: transporting, field.HexX, field.HexY, field.HexLevel) == transporting;
                }

                if (!found)
                {
                    i += 1;
                }
            }

            if (found)
            {
                (AIMoveExecutorMove move, _) = CreateMove(army, selection: transporting, field.HexX, field.HexY, field.HexLevel, mode: CreateMoveModes.None);
                if (move is not null)
                {
                    Executor.ExecuteMoves();
                    army.SetStatus(transporting, status: AIMoveExecutorUnitStatus.Done);
                    army.SetTransportStatus(transporting, status: AIMoveExecutorTransportStatus.Dumped);
                }
                else
                {
                    Executor.Status = AIMoveExecutorStatus.OutOfMovePoints;
                }
            }
            else
            {
                army.SetTransportStatus(transporting, status: AIMoveExecutorTransportStatus.Guarding);
            }

            _ = settings.Release();
        }

        if (transporting == 0 && others != 0)
        {
            settings = new MoveControlSettings(null) { Holder = CreationHolder() };
            army.RetrieveArmy(others, army: out armyHexSprite, armySelection: out selection);
            settings.SetupArmy(armyHexSprite, selection);
            settings.MovePointsMax = armyHexSprite.Army.Moves(selection);
            settings.Mode |= MoveModes.LogProcessedMapFields;
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            field = null;
            DelphiList<MapField> otherFields = settings.ProcessedMapFields;
            found = false;
            i = 0;
            while (!found && i != otherFields.Count)
            {
                field = otherFields.List[i];
                if (army.Location.X != field.HexX || army.Location.Y != field.HexY || army.Location.MapLevel != field.HexLevel)
                {
                    found = CanMoveTo(army, selection: others, field.HexX, field.HexY, field.HexLevel) == others;
                }

                if (!found)
                {
                    i += 1;
                }
            }

            if (found)
            {
                (AIMoveExecutorMove move, _) = CreateMove(army, selection: others, field.HexX, field.HexY, field.HexLevel, mode: CreateMoveModes.None);
                if (move is not null)
                {
                    Executor.ExecuteMoves();
                    army.SetStatus(others, status: AIMoveExecutorUnitStatus.Done);
                    army.SetTransportStatus(others, status: AIMoveExecutorTransportStatus.Dumped);
                }
                else
                {
                    Executor.Status = AIMoveExecutorStatus.OutOfMovePoints;
                }
            }
            else
            {
                army.SetTransportStatus(others, status: AIMoveExecutorTransportStatus.Guarding);
            }

            _ = settings.Release();
        }
    }

    // AIGroup.pas:5638
    public void UpdateTransportMeetingPoint()
    {
        // { Addressability survives elimination of this assignment. }
        bool found = false;
        if ((Executor.Mode & (AIMoveExecutorModes.Adjacent | AIMoveExecutorModes.Attack)) != AIMoveExecutorModes.None)
        {
            AIMoveExecutorArmyList aiMoveExecutorArmyList = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
            int i = aiMoveExecutorArmyList.Count;
            while (!found && i != 0)
            {
                i -= 1;
                AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(i);
                found = armies.TransportStatus is AIMoveExecutorTransportStatus.Boarding or AIMoveExecutorTransportStatus.Transporting;
                if (found)
                {
                    _meetingPoint = aiMoveExecutorArmyList.GetArmies(i).ArrivalPoint;
                }
            }

            if (!found)
            {
                i = aiMoveExecutorArmyList.Count;
            }

            while (!found && i != 0)
            {
                i -= 1;
                AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(i);
                found = armies.TransportStatus is AIMoveExecutorTransportStatus.Arriving or AIMoveExecutorTransportStatus.Done;
                if (found)
                {
                    _meetingPoint = new PathStep(Location: aiMoveExecutorArmyList.GetArmies(i).Location, MoveCost: 0);
                }
            }

            if (found)
            {
                MoveControlSettings settings = new(null) { Holder = CreationHolder() };
                aiMoveExecutorArmyList.GetArmies(i).RetrieveArmy(255, army: out ArmyHexSprite armyHexSprite, armySelection: out byte selection);
                settings.SetupArmy(armyHexSprite, selection);
                settings.SetSourcePosition(_meetingPoint.Location);
                settings.SetDestinationPosition(Executor.Destination);
                StrategicMap.Current.MoveControl.ProcessMove(settings);
                if (settings.Path.Count > 4)
                {
                    _meetingPoint = settings.Path[^2];
                }

                i = 0;
                int limit = aiMoveExecutorArmyList.Count - 1;
                for (; i <= limit; i += 1)
                {
                    AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(i);
                    if (armies.TransportStatus is (>= AIMoveExecutorTransportStatus.Boarding and <= AIMoveExecutorTransportStatus.Arriving) or AIMoveExecutorTransportStatus.Done)
                    {
                        for (int j = 0, last = aiMoveExecutorArmyList.GetArmies(i).Count - 1; j <= last; j++)
                        {
                            aiMoveExecutorArmyList.GetArmies(i).GetUnits(j).ArrivalPoint = _meetingPoint;
                        }
                    }
                }

                _ = settings.Release();
            }

            aiMoveExecutorArmyList?.Dispose();
        }

        if (!found)
        {
            // The location is reset and the cost stays, as the Delphi component writes left the flag byte.
            _meetingPoint = _meetingPoint with { Location = MapCoordinate.None };
        }
    }

    // AIGroup.pas:5691
    public void CalculateTransportPathCallback(object sender)
    {
        AIMoveExecutorArmyList armies = Executor.CreateArmies(AIMoveExecutorUnitStatuses.Ready);
        int index = armies.IndexOf(MoveControl.Settings.ArmyHexSprite, selection: (byte)MoveControl.Settings.Selection);
        AIMoveExecutorArmy? army = index != -1 ? armies.GetArmies(index) : null;

        if (army is not null && MoveControl.Settings.MovePoints != -1)
        {
            if (MoveControl.Settings.BoardingPoint.X != -1)
            {
                AIMoveControlSettings aiMoveControlSettings = MoveControl.Settings;
                index = aiMoveControlSettings.TransportPath.IndexOfMapCoordinate(aiMoveControlSettings.BoardingPoint.X, aiMoveControlSettings.BoardingPoint.Y, L: aiMoveControlSettings.BoardingPoint.MapLevel);
                while (true)
                {
                    bool condition = index != aiMoveControlSettings.TransportPath.Count - 1;
                    if (condition)
                    {
                        PathStep item = aiMoveControlSettings.TransportPath[index];
                        PathStep fTransportPathItem = aiMoveControlSettings.TransportPath[index];
                        condition = CanMoveTo(army, selection: 255, aiMoveControlSettings.TransportPath[index].Location.X, item.Location.Y, fTransportPathItem.Location.MapLevel) != 255;
                    }

                    if (!condition)
                    {
                        break;
                    }

                    index += 1;
                }

                aiMoveControlSettings.BoardingPoint = aiMoveControlSettings.TransportPath[index].Location;
                index = aiMoveControlSettings.TransportPath.IndexOfMapCoordinate(aiMoveControlSettings.ArrivalPoint.X, aiMoveControlSettings.ArrivalPoint.Y, L: aiMoveControlSettings.ArrivalPoint.MapLevel);
                while (true)
                {
                    bool condition = index != 0;
                    if (condition)
                    {
                        PathStep item = aiMoveControlSettings.TransportPath[index];
                        PathStep fTransportPathItem = aiMoveControlSettings.TransportPath[index];
                        condition = CanMoveTo(army, selection: 255, aiMoveControlSettings.TransportPath[index].Location.X, item.Location.Y, fTransportPathItem.Location.MapLevel) != 255;
                    }

                    if (!condition)
                    {
                        break;
                    }

                    index -= 1;
                }

                aiMoveControlSettings.ArrivalPoint = aiMoveControlSettings.TransportPath[index].Location;
                for (int i = 0, last = army.Count - 1; i <= last; i++)
                {
                    AIMoveExecutorUnit unitEntry = army.GetUnits(i);
                    AIMoveExecutorUnit units = army.GetUnits(i);
                    unitEntry.TransportStatus = units.Location == Executor.Transporter.Location
                        ? AIMoveExecutorTransportStatus.Transporting
                        : AIMoveExecutorTransportStatus.Boarding;

                    unitEntry.BoardingPoint = MoveControl.Settings.BoardingPoint;
                    unitEntry.ArrivalPoint = new PathStep(Location: MoveControl.Settings.ArrivalPoint, MoveCost: 0);
                    unitEntry.TransportSource = MoveControl.Settings.TransportSource;
                    unitEntry.TransportDestination = MoveControl.Settings.TransportDestination;
                }
            }
            else
            {
                for (int i = 0, last = army.Count - 1; i <= last; i++)
                {
                    army.GetUnits(i).TransportStatus = AIMoveExecutorTransportStatus.Arriving;
                }
            }
        }
        else
        {
            Executor.Status = AIMoveExecutorStatus.Failed;
        }

        armies?.Dispose();
    }
}
