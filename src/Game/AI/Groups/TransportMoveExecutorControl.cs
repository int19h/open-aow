// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// meeting point
public partial class TransportMoveExecutorControl : AIMoveExecutorControl
{
    public TransportMoveExecutorControl(ECustomNode owner) : base(owner)
    { }

    private MapCoordinate _meetingPoint;

    // AIGroup.pas:5741
    public override void CalculateMoves()
    {
        AIMoveExecutorArmyList transporters = default;
        AIMoveExecutorArmyList Armies = default;
        AIMoveControlSettings settings = default;
        int I = default;
        int timeLeft = default;
        int index = default;
        byte selection = default;
        ArmyHexSprite armyHexSprite = default;
        AIMoveExecutorArmy army = default;
        AIMoveExecutorMove move = default;
        switch (State)
        {
            case 0:
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                I = 0;
                timeLeft = 5;
                while (I < Armies.Count && MoveControl.Status != AIMoveControlStatus.Processing && timeLeft > 2)
                {
                    if (Armies.GetArmies(I).TransportStatus == AIMoveExecutorTransportStatus.Idle)
                    {
                        settings = new AIMoveControlSettings(null);
                        Armies.GetArmies(I).RetrieveArmy(255, out armyHexSprite, out selection);
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

                    I += 1;
                }

                index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Idle);
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

                Armies?.Dispose();
                break;
            case 1:
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Arriving);
                if (index != -1)
                {
                    ref var location = ref Armies.GetArmies(index).ArrivalPoint;
                    move = CreateMove(Armies.GetArmies(index), 255, location.X, location.Y, location.MapLevel, Set<CreateMoveModeElement>.Of());

                    Armies.GetArmies(index).SetTransportStatus(255, AIMoveExecutorTransportStatus.Done);
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

                Armies?.Dispose();
                break;
            case 2:
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                bool condition = index != -1;
                if (condition)
                {
                    var armies = Armies.GetArmies(index);
                    condition = Unsafe.As<MapCoordinate, int>(ref armies.TransportSource) == Unsafe.As<MapCoordinate, int>(ref Executor.Transporter.Location);
                }

                if (condition)
                {
                    I = 0;
                    while (I < Armies.Count && MoveControl.Status != AIMoveControlStatus.Processing)
                    {
                        if (Armies.GetArmies(I).TransportStatus == AIMoveExecutorTransportStatus.Dumping)
                        {
                            DumpTransportUnits(Armies.GetArmies(I));
                        }

                        I += 1;
                    }

                    index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Dumping);
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

                Armies?.Dispose();
                break;
            case 3:
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Transporting);
                bool test = index != -1;
                if (test)
                {
                    var armies = Armies.GetArmies(index);
                    var armiesArmies = Armies.GetArmies(index);
                    test = Unsafe.As<MapCoordinate, int>(ref armies.TransportDestination) == Unsafe.As<MapCoordinate, int>(ref armiesArmies.Location);
                }

                if (test)
                {
                    transporters = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                    I = 0;
                    while (I != transporters.Count)
                    {
                        bool passed = transporters.GetArmies(I).TransportStatus == AIMoveExecutorTransportStatus.Done;
                        if (passed)
                        {
                            var armies = transporters.GetArmies(I);
                            var transportersArmies = transporters.GetArmies(I);
                            var armies2 = transporters.GetArmies(I);
                            passed = CanMoveTo(Armies.GetArmies(index), 255, armies2.Location.X, armies.Location.Y, transportersArmies.Location.MapLevel) == 255;
                        }

                        if (passed)
                        {
                            break;
                        }

                        I += 1;
                    }

                    if (I != transporters.Count)
                    {
                        var aiMoveExecutorArmy = transporters.GetArmies(I);
                        var armies = transporters.GetArmies(I);
                        var transportersArmies = transporters.GetArmies(I);
                        var armies2 = transporters.GetArmies(I);
                        move = CreateMove(Armies.GetArmies(index), 255, armies2.Location.X, armies.Location.Y, transportersArmies.Location.MapLevel, Set<CreateMoveModeElement>.Of());
                    }
                    else
                    {
                        ref var location = ref Armies.GetArmies(index).ArrivalPoint;
                        move = CreateMove(Armies.GetArmies(index), 255, location.X, location.Y, location.MapLevel, Set<CreateMoveModeElement>.Of());
                    }

                    if (move == null)
                    {
                        Armies.GetArmies(index).SetStatus(255, AIMoveExecutorUnitStatus.Done);
                        Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                    }
                    else
                    {
                        Armies.GetArmies(index).SetTransportStatus(255, AIMoveExecutorTransportStatus.Done);
                        Executor.ExecuteMoves();
                    }

                    transporters?.Dispose();
                }

                Armies?.Dispose();
                State += 1;
                break;
            case 4:
                if (Executor.Transporter.Status == AIMoveExecutorUnitStatus.Ready)
                {
                    Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Done));
                    index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Transporting);
                    if (index != -1)
                    {
                        ref var location = ref Armies.GetArmies(index).TransportDestination;
                        move = CreateTransporterMove(location.X, location.Y, location.MapLevel, true);

                        if (move == null)
                        {
                            Armies.GetArmies(index).SetStatus(255, AIMoveExecutorUnitStatus.Done);
                            Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                        }
                    }

                    Armies?.Dispose();
                }

                State += 1;
                break;
            case 5:
                Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
                I = 0;
                timeLeft = 5;
                while (timeLeft > 2 && I != Armies.Count)
                {
                    if (Armies.GetArmies(I).TransportStatus == AIMoveExecutorTransportStatus.Boarding)
                    {
                        army = Armies.GetArmies(I);
                        if (army.BoardingPoint.X != -1 && Unsafe.As<MapCoordinate, int>(ref army.Location) != Unsafe.As<MapCoordinate, int>(ref army.BoardingPoint))
                        {
                            move = CreateMove(Armies.GetArmies(I), 255, army.BoardingPoint.X, army.BoardingPoint.Y, army.BoardingPoint.MapLevel, Set<CreateMoveModeElement>.Of());
                            if (move == null)
                            {
                                army.SetStatus(255, AIMoveExecutorUnitStatus.Done);
                            }

                            timeLeft = StrategicMap.Current.AIExecutor.GetProcessTimeLeft();
                        }
                    }

                    I += 1;
                }

                if (I == Armies.Count)
                {
                    State += 1;
                }

                Armies?.Dispose();
                break;
            case 6:
                if (Executor.Transporter.Status == AIMoveExecutorUnitStatus.Ready)
                {
                    Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Queued, AIMoveExecutorUnitStatus.Done));
                    index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                    if (index != -1)
                    {
                        var aiMoveExecutorArmy = Armies.GetArmies(index);
                        if (Unsafe.As<MapCoordinate, int>(ref Executor.Transporter.Location) != Unsafe.As<MapCoordinate, int>(ref aiMoveExecutorArmy.TransportSource))
                        {
                            move = CreateTransporterMove(aiMoveExecutorArmy.TransportSource.X, aiMoveExecutorArmy.TransportSource.Y, aiMoveExecutorArmy.TransportSource.MapLevel, true);
                            if (move == null)
                            {
                                Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                            }
                        }
                    }

                    Armies?.Dispose();
                }

                State += 1;
                break;
            case 7:
                if (Executor.Transporter.Status == AIMoveExecutorUnitStatus.Ready)
                {
                    Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready, AIMoveExecutorUnitStatus.Queued, AIMoveExecutorUnitStatus.Done));
                    index = Armies.IndexOfTransportStatus(AIMoveExecutorTransportStatus.Boarding);
                    if (index != -1)
                    {
                        army = Armies.GetArmies(index);
                        if (army.WaitingForTransport() && Unsafe.As<MapCoordinate, int>(ref Executor.Transporter.Location) == Unsafe.As<MapCoordinate, int>(ref army.TransportSource))
                        {
                            selection = CanMoveTo(Armies.GetArmies(index), 255, army.TransportSource.X, army.TransportSource.Y, army.TransportSource.MapLevel);
                            if (selection != 0)
                            {
                                move = CreateMove(Armies.GetArmies(index), selection, army.TransportSource.X, army.TransportSource.Y, army.TransportSource.MapLevel, Set<CreateMoveModeElement>.Of());
                                if (move == null)
                                {
                                    Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                                }
                                else
                                {
                                    Armies.GetArmies(index).SetTransportStatus(selection, AIMoveExecutorTransportStatus.Transporting);
                                }
                            }
                            else
                            {
                                Executor.Transporter.Status = AIMoveExecutorUnitStatus.Done;
                            }
                        }
                    }

                    Armies?.Dispose();
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

        for (int I = Executor.Units.Count - 1; I >= 0; I--)
        {
            var units = Executor.Units.GetUnits(I);
            if (Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Guarding, AIMoveExecutorTransportStatus.Dumped).Contains(units.TransportStatus))
            {
                Executor.Units.GetUnits(I)?.Dispose();
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
        army = new AIMoveExecutorArmy(null);
        army.Setup(Executor.Transporter);
        for (int I = 0, last = Executor.Units.Count - 1; I <= last; I++)
        {
            unitEntry = Executor.Units.GetUnits(I);
            if (Unsafe.As<MapCoordinate, int>(ref unitEntry.Location) == Unsafe.As<MapCoordinate, int>(ref Executor.Transporter.Location))
            {
                army.AddChild(unitEntry);
            }
        }

        var aiMoveExecutorUnit = Executor.Transporter;
        armyHexSprite = StrategicMap.Current.MapContainer.GetField(aiMoveExecutorUnit.Location.X, aiMoveExecutorUnit.Location.Y, aiMoveExecutorUnit.Location.MapLevel).GetArmyHexSprite();

        settings = new MoveControlSettings(null);
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
                        condition = CanMoveTo(army, 255, settings.Path[position].X, item.Y, fPathItem.MapLevel) != 255;
                    }

                    if (!condition)
                        break;
                    position += 1;
                }

                if (position < settings.Path.Count - 1)
                {
                    move = new AIMoveExecutorMove(null);
                    move.AddUnits(army, 255);
                    for (int I = position, last = settings.Path.Count - 1; I <= last; I++)
                    {
                        var item = settings.Path[I];
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
        int I;
        bool done;
        executor = Executor;
        I = executor.Units.Count;
        done = true;
        while (I != 0 && done)
        {
            I -= 1;
            var units = executor.Units.GetUnits(I);
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
        int I;
        MapField field;
        bool found;
        transporting = 0;
        others = 0;
        I = 0;
        int last = army.Count - 1;
        for (; I <= last; I += 1)
        {
            if (army.GetUnits(I).Troop.GetTransporting())
            {
                transporting = (byte)(transporting | (1 << I));
            }
            else
            {
                others = (byte)(others | (1 << I));
            }
        }

        if (transporting != 0)
        {
            settings = new MoveControlSettings(null);
            army.RetrieveArmy(transporting, out armyHexSprite, out selection);
            settings.SetupArmy(armyHexSprite, selection);
            settings.MovePointsMax = armyHexSprite.Army.Moves(selection);
            settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            fields = settings.ProcessedMapFields;
            found = false;
            I = 0;
            field = null;
            while (!found && I != fields.Count)
            {
                field = fields.List[I];
                if (army.Location.X != field.HexX || army.Location.Y != field.HexY || army.Location.MapLevel != field.HexLevel)
                {
                    found = CanMoveTo(army, transporting, field.HexX, field.HexY, field.HexLevel) == transporting;
                }

                if (!found)
                {
                    I += 1;
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
            settings = new MoveControlSettings(null);
            army.RetrieveArmy(others, out armyHexSprite, out selection);
            settings.SetupArmy(armyHexSprite, selection);
            settings.MovePointsMax = armyHexSprite.Army.Moves(selection);
            settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            field = null;
            otherFields = settings.ProcessedMapFields;
            found = false;
            I = 0;
            while (!found && I != otherFields.Count)
            {
                field = otherFields.List[I];
                if (army.Location.X != field.HexX || army.Location.Y != field.HexY || army.Location.MapLevel != field.HexLevel)
                {
                    found = CanMoveTo(army, others, field.HexX, field.HexY, field.HexLevel) == others;
                }

                if (!found)
                {
                    I += 1;
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
        AIMoveExecutorArmyList Armies;
        MoveControlSettings settings;
        byte selection = default;
        int I;
        bool found;
        ArmyHexSprite armyHexSprite = default;
        // { Addressability survives elimination of this assignment. }
        found = false;
        if ((Executor.Mode & Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent, AIMoveExecutorModeElement.Attack)) != Set<AIMoveExecutorModeElement>.Of())
        {
            Armies = Executor.CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Ready));
            I = Armies.Count;
            while (!found && I != 0)
            {
                I -= 1;
                var armies = Armies.GetArmies(I);
                found = Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Boarding, AIMoveExecutorTransportStatus.Transporting).Contains(armies.TransportStatus);
                if (found)
                {
                    _meetingPoint = Armies.GetArmies(I).ArrivalPoint;
                }
            }

            if (!found)
            {
                I = Armies.Count;
            }

            while (!found && I != 0)
            {
                I -= 1;
                var armies = Armies.GetArmies(I);
                found = Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Arriving, AIMoveExecutorTransportStatus.Done).Contains(armies.TransportStatus);
                if (found)
                {
                    _meetingPoint = Armies.GetArmies(I).Location;
                }
            }

            if (found)
            {
                settings = new MoveControlSettings(null);
                Armies.GetArmies(I).RetrieveArmy(255, out armyHexSprite, out selection);
                settings.SetupArmy(armyHexSprite, selection);
                settings.SetSourcePosition(_meetingPoint.X, _meetingPoint.Y, _meetingPoint.MapLevel);
                settings.SetDestinationPosition(Executor.Destination.X, Executor.Destination.Y, Executor.Destination.MapLevel);
                StrategicMap.Current.MoveControl.ProcessMove(settings);
                if (settings.Path.Count > 4)
                {
                    _meetingPoint = settings.Path[settings.Path.Count - 2];
                }

                I = 0;
                int limit = Armies.Count - 1;
                for (; I <= limit; I += 1)
                {
                    var armies = Armies.GetArmies(I);
                    if (Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Boarding, AIMoveExecutorTransportStatus.Transporting, AIMoveExecutorTransportStatus.Arriving, AIMoveExecutorTransportStatus.Done).Contains(armies.TransportStatus))
                    {
                        for (int J = 0, last = Armies.GetArmies(I).Count - 1; J <= last; J++)
                        {
                            Armies.GetArmies(I).GetUnits(J).ArrivalPoint = _meetingPoint;
                        }
                    }
                }

                settings.Release();
            }

            Armies?.Dispose();
        }

        if (!found)
        {
            _meetingPoint.X = -1;
            _meetingPoint.Y = -1;
            _meetingPoint.MapLevel = -1;
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
                        condition = CanMoveTo(army, 255, aiMoveControlSettings.TransportPath[index].X, item.Y, fTransportPathItem.MapLevel) != 255;
                    }

                    if (!condition)
                        break;
                    index += 1;
                }

                aiMoveControlSettings.BoardingPoint = aiMoveControlSettings.TransportPath[index];
                index = aiMoveControlSettings.TransportPath.IndexOfMapCoordinate(aiMoveControlSettings.ArrivalPoint.X, aiMoveControlSettings.ArrivalPoint.Y, aiMoveControlSettings.ArrivalPoint.MapLevel);
                while (true)
                {
                    bool condition = index != 0;
                    if (condition)
                    {
                        var item = aiMoveControlSettings.TransportPath[index];
                        var fTransportPathItem = aiMoveControlSettings.TransportPath[index];
                        condition = CanMoveTo(army, 255, aiMoveControlSettings.TransportPath[index].X, item.Y, fTransportPathItem.MapLevel) != 255;
                    }

                    if (!condition)
                        break;
                    index -= 1;
                }

                aiMoveControlSettings.ArrivalPoint = aiMoveControlSettings.TransportPath[index];
                for (int I = 0, last = army.Count - 1; I <= last; I++)
                {
                    unitEntry = army.GetUnits(I);
                    var units = army.GetUnits(I);
                    if (Unsafe.As<MapCoordinate, int>(ref units.Location) == Unsafe.As<MapCoordinate, int>(ref Executor.Transporter.Location))
                    {
                        unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Transporting;
                    }
                    else
                    {
                        unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Boarding;
                    }

                    unitEntry.BoardingPoint = MoveControl.Settings.BoardingPoint;
                    unitEntry.BoardingPoint.Tag = 0;
                    unitEntry.ArrivalPoint = MoveControl.Settings.ArrivalPoint;
                    unitEntry.ArrivalPoint.Tag = 0;
                    unitEntry.TransportSource = MoveControl.Settings.TransportSource;
                    unitEntry.TransportSource.Tag = 0;
                    unitEntry.TransportDestination = MoveControl.Settings.TransportDestination;
                    unitEntry.TransportDestination.Tag = 0;
                }
            }
            else
            {
                for (int I = 0, last = army.Count - 1; I <= last; I++)
                {
                    army.GetUnits(I).TransportStatus = AIMoveExecutorTransportStatus.Arriving;
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
