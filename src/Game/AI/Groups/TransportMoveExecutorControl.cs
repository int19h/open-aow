// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// meeting point
public partial class TransportMoveExecutorControl : AIMoveExecutorControl
{
    public TransportMoveExecutorControl(CustomNode owner) : base(owner)
    { }

    // A path step with its cost, copied to the units' arrival points (see AIMoveExecutorUnit).
    private PathStep _meetingPoint;

    // AIGroup.pas:5741
    public override void CalculateMoves()
    {
        AIMoveExecutorArmyList transporters = default;
        AIMoveExecutorArmyList aiMoveExecutorArmyList = default;
        AIMoveControlSettings settings = default;
        int i = default;
        int timeLeft = default;
        int index = default;
        byte selection = default;
        ArmyHexSprite armyHexSprite = default;
        AIMoveExecutorArmy army = default;
        AIMoveExecutorMove move = default;
        switch (State)
        {
            case 0:
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                i = 0;
                timeLeft = 5;
                while (i < aiMoveExecutorArmyList.Count && MoveControl.Status != AIMoveControlStatus.Processing && timeLeft > 2)
                {
                    if (aiMoveExecutorArmyList.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Idle)
                    {
                        settings = new AIMoveControlSettings(null) { Holder = CreationHolder() };
                        aiMoveExecutorArmyList.GetArmies(i).RetrieveArmy(255, out armyHexSprite, out selection);
                        settings.Setup(armyHexSprite, selection, Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel);
                        settings.Transporter = Executor.Transporter.Troop;
                        settings.OnDone += CalculateTransportPathCallback;
                        settings.MoveMode = Set<MoveModeElement>.Of(MoveModeElement.AI);
                        MoveControl.Reset();
                        MoveControl.CanMove(settings);
                        if (MoveControl.Status != AIMoveControlStatus.Processing)
                        {
                            timeLeft = StrategicMap.Current.AIExecutor.GetProcessTimeLeft();
                        }

                        settings.Release();
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
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Arriving);
                if (index != -1)
                {
                    ref var location = ref aiMoveExecutorArmyList.GetArmies(index).ArrivalPoint;
                    move = CreateMove(aiMoveExecutorArmyList.GetArmies(index), 255, location.Location.X, location.Location.Y, location.Location.MapLevel, Set<CreateMoveModeElement>.Of());

                    aiMoveExecutorArmyList.GetArmies(index).SetTransportStatus(255, AIMoveExecutorTransportStatus.Done);
                    // { The original executes queued moves only when creation returns nil. }
                    if (move == null)
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
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                bool condition = index != -1;
                if (condition)
                {
                    var armies = aiMoveExecutorArmyList.GetArmies(index);
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
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Transporting);
                bool test = index != -1;
                if (test)
                {
                    var armies = aiMoveExecutorArmyList.GetArmies(index);
                    var armiesArmies = aiMoveExecutorArmyList.GetArmies(index);
                    test = armies.TransportDestination == armiesArmies.Location;
                }

                if (test)
                {
                    transporters = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                    i = 0;
                    while (i != transporters.Count)
                    {
                        bool passed = transporters.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Done;
                        if (passed)
                        {
                            var armies = transporters.GetArmies(i);
                            var transportersArmies = transporters.GetArmies(i);
                            var armies2 = transporters.GetArmies(i);
                            passed = CanMoveTo(aiMoveExecutorArmyList.GetArmies(index), 255, armies2.Location.X, armies.Location.Y, transportersArmies.Location.MapLevel) == 255;
                        }

                        if (passed)
                        {
                            break;
                        }

                        i += 1;
                    }

                    if (i != transporters.Count)
                    {
                        var aiMoveExecutorArmy = transporters.GetArmies(i);
                        var armies = transporters.GetArmies(i);
                        var transportersArmies = transporters.GetArmies(i);
                        var armies2 = transporters.GetArmies(i);
                        move = CreateMove(aiMoveExecutorArmyList.GetArmies(index), 255, armies2.Location.X, armies.Location.Y, transportersArmies.Location.MapLevel, Set<CreateMoveModeElement>.Of());
                    }
                    else
                    {
                        ref var location = ref aiMoveExecutorArmyList.GetArmies(index).ArrivalPoint;
                        move = CreateMove(aiMoveExecutorArmyList.GetArmies(index), 255, location.Location.X, location.Location.Y, location.Location.MapLevel, Set<CreateMoveModeElement>.Of());
                    }

                    if (move == null)
                    {
                        aiMoveExecutorArmyList.GetArmies(index).SetStatus(255, AIMoveExecutorUnitStatus.Done);
                        Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                    }
                    else
                    {
                        aiMoveExecutorArmyList.GetArmies(index).SetTransportStatus(255, AIMoveExecutorTransportStatus.Done);
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
                    aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Transporting);
                    if (index != -1)
                    {
                        ref var location = ref aiMoveExecutorArmyList.GetArmies(index).TransportDestination;
                        move = CreateTransporterMove(location.X, location.Y, location.MapLevel, true);

                        if (move == null)
                        {
                            aiMoveExecutorArmyList.GetArmies(index).SetStatus(255, AIMoveExecutorUnitStatus.Done);
                            Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                        }
                    }

                    aiMoveExecutorArmyList?.Dispose();
                }

                State += 1;
                break;
            case 5:
                aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                i = 0;
                timeLeft = 5;
                while (timeLeft > 2 && i != aiMoveExecutorArmyList.Count)
                {
                    if (aiMoveExecutorArmyList.GetArmies(i).TransportStatus == AIMoveExecutorTransportStatus.Boarding)
                    {
                        army = aiMoveExecutorArmyList.GetArmies(i);
                        if (army.BoardingPoint.X != -1 && army.Location != army.BoardingPoint)
                        {
                            move = CreateMove(aiMoveExecutorArmyList.GetArmies(i), 255, army.BoardingPoint.X, army.BoardingPoint.Y, army.BoardingPoint.MapLevel, Set<CreateMoveModeElement>.Of());
                            if (move == null)
                            {
                                army.SetStatus(255, AIMoveExecutorUnitStatus.Done);
                            }

                            timeLeft = StrategicMap.Current.AIExecutor.GetProcessTimeLeft();
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
                    aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Queued, AIMoveExecutorUnitStatus.Done));
                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                    if (index != -1)
                    {
                        var aiMoveExecutorArmy = aiMoveExecutorArmyList.GetArmies(index);
                        if (Executor.Transporter.Location != aiMoveExecutorArmy.TransportSource)
                        {
                            move = CreateTransporterMove(aiMoveExecutorArmy.TransportSource.X, aiMoveExecutorArmy.TransportSource.Y, aiMoveExecutorArmy.TransportSource.MapLevel, true);
                            if (move == null)
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
                    aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Queued, AIMoveExecutorUnitStatus.Done));
                    index = aiMoveExecutorArmyList.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                    if (index != -1)
                    {
                        army = aiMoveExecutorArmyList.GetArmies(index);
                        if (army.WaitingForTransport() && Executor.Transporter.Location == army.TransportSource)
                        {
                            selection = CanMoveTo(aiMoveExecutorArmyList.GetArmies(index), 255, army.TransportSource.X, army.TransportSource.Y, army.TransportSource.MapLevel);
                            if (selection != 0)
                            {
                                move = CreateMove(aiMoveExecutorArmyList.GetArmies(index), selection, army.TransportSource.X, army.TransportSource.Y, army.TransportSource.MapLevel, Set<CreateMoveModeElement>.Of());
                                if (move == null)
                                {
                                    Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                                }
                                else
                                {
                                    aiMoveExecutorArmyList.GetArmies(index).SetTransportStatus(selection, AIMoveExecutorTransportStatus.Transporting);
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
            var units = Executor.Units.GetUnits(i);
            if (Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Guarding, AIMoveExecutorTransportStatus.Dumped).Contains(units.TransportStatus))
            {
                Executor.Units.GetUnits(i)?.Dispose();
            }
        }

        State = -1;
    }

    // AIGroup.pas:5465
    public AIMoveExecutorMove CreateTransporterMove(sbyte X, sbyte Y, sbyte L, bool queue)
    {
        AIMoveExecutorMove move;
        int movePoints;
        int position;
        AIMoveExecutorArmy army;
        AIMoveExecutorUnit unitEntry;
        ArmyHexSprite armyHexSprite;
        MoveControlSettings settings;
        move = null;
        army = new AIMoveExecutorArmy(null) { Holder = CreationHolder() };
        army.Setup(Executor.Transporter);
        for (int i = 0, last = Executor.Units.Count - 1; i <= last; i++)
        {
            unitEntry = Executor.Units.GetUnits(i);
            if (unitEntry.Location == Executor.Transporter.Location)
            {
                army.AddChild(unitEntry);
            }
        }

        var aiMoveExecutorUnit = Executor.Transporter;
        armyHexSprite = StrategicMap.Current.MapContainer.GetField(aiMoveExecutorUnit.Location.X, aiMoveExecutorUnit.Location.Y, aiMoveExecutorUnit.Location.MapLevel).GetArmyHexSprite();

        settings = new MoveControlSettings(null) { Holder = CreationHolder() };
        settings.SetupArmy(armyHexSprite, 255);
        settings.SetDestinationPosition(X, Y, L);
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        if (settings.Path.Count != 0)
        {
            movePoints = armyHexSprite.Army.MovePoints(255);
            position = settings.Path.MovePointsToPosition(movePoints);
            if (position < settings.Path.Count - 1)
            {
                while (true)
                {
                    bool condition = position != settings.Path.Count - 1;
                    if (condition)
                    {
                        var item = settings.Path[position];
                        var fPathItem = settings.Path[position];
                        condition = CanMoveTo(army, 255, settings.Path[position].Location.X, item.Location.Y, fPathItem.Location.MapLevel) != 255;
                    }

                    if (!condition)
                        break;
                    position += 1;
                }

                if (position < settings.Path.Count - 1)
                {
                    move = new AIMoveExecutorMove(null) { Holder = CreationHolder() };
                    move.AddUnits(army, 255);
                    for (int i = position, last = settings.Path.Count - 1; i <= last; i++)
                    {
                        var item = settings.Path[i];
                        move.Path.Add(item);
                    }

                    move.MovePoints = movePoints;
                    move.Moves = armyHexSprite.Army.Moves(255);
                    move.SourceX = armyHexSprite.HexX;
                    move.SourceY = armyHexSprite.HexY;
                    move.SourceL = armyHexSprite.HexLevel;
                    move.DestinationX = X;
                    move.DestinationY = Y;
                    move.DestinationL = L;
                    if (queue)
                    {
                        move.Owner = Executor.Moves;
                        move.SetUnitStatus(AIMoveExecutorUnitStatus.Queued);
                    }
                }
            }
        }

        army?.Dispose();
        settings.Release();
        return move;
    }

    // AIGroup.pas:5525
    public bool TransportDone()
    {
        AIMoveExecutor executor;
        int i;
        bool done;
        executor = Executor;
        i = executor.Units.Count;
        done = true;
        while (i != 0 && done)
        {
            i -= 1;
            var units = executor.Units.GetUnits(i);
            if (!Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Arriving, AIMoveExecutorTransportStatus.Guarding, AIMoveExecutorTransportStatus.Dumping, AIMoveExecutorTransportStatus.Dumped, AIMoveExecutorTransportStatus.Done).Contains(units.TransportStatus))
            {
                done = false;
            }
        }

        return done;
    }

    // AIGroup.pas:5553
    public void DumpTransportUnits(AIMoveExecutorArmy army)
    {
        byte transporting;
        byte others;
        MoveControlSettings settings;
        byte selection = default;
        ArmyHexSprite armyHexSprite = default;
        DelphiList<MapField> fields;
        DelphiList<MapField> otherFields;
        int i;
        MapField field;
        bool found;
        transporting = 0;
        others = 0;
        i = 0;
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

        if (transporting != 0)
        {
            settings = new MoveControlSettings(null) { Holder = CreationHolder() };
            army.RetrieveArmy(transporting, out armyHexSprite, out selection);
            settings.SetupArmy(armyHexSprite, selection);
            settings.MovePointsMax = armyHexSprite.Army.Moves(selection);
            settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            fields = settings.ProcessedMapFields;
            found = false;
            i = 0;
            field = null;
            while (!found && i != fields.Count)
            {
                field = fields.List[i];
                if (army.Location.X != field.HexX || army.Location.Y != field.HexY || army.Location.MapLevel != field.HexLevel)
                {
                    found = CanMoveTo(army, transporting, field.HexX, field.HexY, field.HexLevel) == transporting;
                }

                if (!found)
                {
                    i += 1;
                }
            }

            if (found)
            {
                if (CreateMove(army, transporting, field.HexX, field.HexY, field.HexLevel, Set<CreateMoveModeElement>.Of()) != null)
                {
                    Executor.ExecuteMoves();
                    army.SetStatus(transporting, AIMoveExecutorUnitStatus.Done);
                    army.SetTransportStatus(transporting, AIMoveExecutorTransportStatus.Dumped);
                }
                else
                {
                    Executor.Status = AIMoveExecutorStatus.OutOfMovePoints;
                }
            }
            else
            {
                army.SetTransportStatus(transporting, AIMoveExecutorTransportStatus.Guarding);
            }

            settings.Release();
        }

        if (transporting == 0 && others != 0)
        {
            settings = new MoveControlSettings(null) { Holder = CreationHolder() };
            army.RetrieveArmy(others, out armyHexSprite, out selection);
            settings.SetupArmy(armyHexSprite, selection);
            settings.MovePointsMax = armyHexSprite.Army.Moves(selection);
            settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            field = null;
            otherFields = settings.ProcessedMapFields;
            found = false;
            i = 0;
            while (!found && i != otherFields.Count)
            {
                field = otherFields.List[i];
                if (army.Location.X != field.HexX || army.Location.Y != field.HexY || army.Location.MapLevel != field.HexLevel)
                {
                    found = CanMoveTo(army, others, field.HexX, field.HexY, field.HexLevel) == others;
                }

                if (!found)
                {
                    i += 1;
                }
            }

            if (found)
            {
                if (CreateMove(army, others, field.HexX, field.HexY, field.HexLevel, Set<CreateMoveModeElement>.Of()) != null)
                {
                    Executor.ExecuteMoves();
                    army.SetStatus(others, AIMoveExecutorUnitStatus.Done);
                    army.SetTransportStatus(others, AIMoveExecutorTransportStatus.Dumped);
                }
                else
                {
                    Executor.Status = AIMoveExecutorStatus.OutOfMovePoints;
                }
            }
            else
            {
                army.SetTransportStatus(others, AIMoveExecutorTransportStatus.Guarding);
            }

            settings.Release();
        }
    }

    // AIGroup.pas:5638
    public void UpdateTransportMeetingPoint()
    {
        AIMoveExecutorArmyList aiMoveExecutorArmyList;
        MoveControlSettings settings;
        byte selection = default;
        int i;
        bool found;
        ArmyHexSprite armyHexSprite = default;
        // { Addressability survives elimination of this assignment. }
        found = false;
        if ((Executor.Mode & Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent, AIMoveExecutorModeElement.Attack)) != Set<AIMoveExecutorModeElement>.Of())
        {
            aiMoveExecutorArmyList = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
            i = aiMoveExecutorArmyList.Count;
            while (!found && i != 0)
            {
                i -= 1;
                var armies = aiMoveExecutorArmyList.GetArmies(i);
                found = Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Boarding, AIMoveExecutorTransportStatus.Transporting).Contains(armies.TransportStatus);
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
                var armies = aiMoveExecutorArmyList.GetArmies(i);
                found = Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Arriving, AIMoveExecutorTransportStatus.Done).Contains(armies.TransportStatus);
                if (found)
                {
                    _meetingPoint = new PathStep(Location: aiMoveExecutorArmyList.GetArmies(i).Location, MoveCost: 0);
                }
            }

            if (found)
            {
                settings = new MoveControlSettings(null) { Holder = CreationHolder() };
                aiMoveExecutorArmyList.GetArmies(i).RetrieveArmy(255, out armyHexSprite, out selection);
                settings.SetupArmy(armyHexSprite, selection);
                settings.SetSourcePosition(_meetingPoint.Location.X, _meetingPoint.Location.Y, _meetingPoint.Location.MapLevel);
                settings.SetDestinationPosition(Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel);
                StrategicMap.Current.MoveControl.ProcessMove(settings);
                if (settings.Path.Count > 4)
                {
                    _meetingPoint = settings.Path[settings.Path.Count - 2];
                }

                i = 0;
                int limit = aiMoveExecutorArmyList.Count - 1;
                for (; i <= limit; i += 1)
                {
                    var armies = aiMoveExecutorArmyList.GetArmies(i);
                    if (Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Boarding, AIMoveExecutorTransportStatus.Transporting, AIMoveExecutorTransportStatus.Arriving, AIMoveExecutorTransportStatus.Done).Contains(armies.TransportStatus))
                    {
                        for (int j = 0, last = aiMoveExecutorArmyList.GetArmies(i).Count - 1; j <= last; j++)
                        {
                            aiMoveExecutorArmyList.GetArmies(i).GetUnits(j).ArrivalPoint = _meetingPoint;
                        }
                    }
                }

                settings.Release();
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
        AIMoveExecutorArmyList armies;
        int index;
        AIMoveExecutorArmy army;
        AIMoveExecutorUnit unitEntry;
        armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
        index = armies.IndexOf(MoveControl.Settings.ArmyHexSprite, (byte)MoveControl.Settings.Selection);
        if (index != -1)
        {
            army = armies.GetArmies(index);
        }
        else
        {
            army = null;
        }

        if (army != null && MoveControl.Settings.MovePoints != -1)
        {
            if (MoveControl.Settings.BoardingPoint.X != -1)
            {
                var aiMoveControlSettings = MoveControl.Settings;
                index = aiMoveControlSettings.TransportPath.IndexOfMapCoordinate(aiMoveControlSettings.BoardingPoint.X, aiMoveControlSettings.BoardingPoint.Y, aiMoveControlSettings.BoardingPoint.MapLevel);
                while (true)
                {
                    bool condition = index != aiMoveControlSettings.TransportPath.Count - 1;
                    if (condition)
                    {
                        var item = aiMoveControlSettings.TransportPath[index];
                        var fTransportPathItem = aiMoveControlSettings.TransportPath[index];
                        condition = CanMoveTo(army, 255, aiMoveControlSettings.TransportPath[index].Location.X, item.Location.Y, fTransportPathItem.Location.MapLevel) != 255;
                    }

                    if (!condition)
                        break;
                    index += 1;
                }

                aiMoveControlSettings.BoardingPoint = aiMoveControlSettings.TransportPath[index].Location;
                index = aiMoveControlSettings.TransportPath.IndexOfMapCoordinate(aiMoveControlSettings.ArrivalPoint.X, aiMoveControlSettings.ArrivalPoint.Y, aiMoveControlSettings.ArrivalPoint.MapLevel);
                while (true)
                {
                    bool condition = index != 0;
                    if (condition)
                    {
                        var item = aiMoveControlSettings.TransportPath[index];
                        var fTransportPathItem = aiMoveControlSettings.TransportPath[index];
                        condition = CanMoveTo(army, 255, aiMoveControlSettings.TransportPath[index].Location.X, item.Location.Y, fTransportPathItem.Location.MapLevel) != 255;
                    }

                    if (!condition)
                        break;
                    index -= 1;
                }

                aiMoveControlSettings.ArrivalPoint = aiMoveControlSettings.TransportPath[index].Location;
                for (int i = 0, last = army.Count - 1; i <= last; i++)
                {
                    unitEntry = army.GetUnits(i);
                    var units = army.GetUnits(i);
                    if (units.Location == Executor.Transporter.Location)
                    {
                        unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Transporting;
                    }
                    else
                    {
                        unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Boarding;
                    }

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
