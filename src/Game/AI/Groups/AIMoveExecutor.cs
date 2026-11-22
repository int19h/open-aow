#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.AI.Groups;

// destination, control, control queue, moves, units, transporter, move limit, mode, state,
// status, player, process cycles, completion event
public sealed class AIMoveExecutor : EngineObject
{
    // AIGroup.pas:5976
    public AIMoveExecutor(CustomNode? owner) : base(owner)
    {
        Units = new AIMoveExecutorUnitList(null) { Holder = CreationHolder() };
        Moves = new AIMoveExecutorMoveQueue(null) { Holder = CreationHolder() };
        ControlQueue = [];
        Clear();
    }

    public MapCoordinate Destination { get; private set; }

    public AIMoveExecutorControl? Control { get; private set; }
    public Int32List? ControlQueue { get; private set; }
    public AIMoveExecutorMoveQueue? Moves { get; private set; }
    public AIMoveExecutorUnitList? Units { get; private set; }
    public AIMoveExecutorUnit? Transporter { get; private set; }
    public int MovePointsMax { get; private set; }
    public AIMoveExecutorModes Mode { get; set; }
    public int State { get; private set; }
    public AIMoveExecutorStatus Status { get; set; }

    private sbyte _player;
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _processCycles;

    private Action<object>? _done;

    internal void SetDoneHandler(Action<object>? handler) => _done = handler;

    // AIGroup.pas:5985
    public override void Dispose()
    {
        Clear();
        Units?.Dispose();
        Units = null;
        Moves?.Dispose();
        Moves = null;
        ControlQueue?.Dispose();
        ControlQueue = null;
        base.Dispose();
    }

    // AIGroup.pas:6017
    public override void Clear()
    {
        base.Clear();
        DeactivateMoveExecutorControl();
        if (Moves.Count != 0)
        {
            Moves.Clear();
        }

        if (Transporter is not null)
        {
            Transporter?.Dispose();
            Transporter = null;
        }

        Units.Clear();
        Mode = AIMoveExecutorModes.Adjacent | AIMoveExecutorModes.Simultaneous;
    }

    // AIGroup.pas:5997
    public void ActivateMoveExecutorControl(Func<AIMoveExecutorControl>? createControl)
    {
        DeactivateMoveExecutorControl();
        if (createControl is not null)
        {
            Control = createControl();
            Control.Activate(this);
        }
    }

    // AIGroup.pas:6007
    public void DeactivateMoveExecutorControl()
    {
        if (Control is not null)
        {
            Control.Deactivate();
            Control.Dispose();
            Control = null;
        }
    }

    // AIGroup.pas:6031
    public AIMoveExecutorUnit AddUnit(Troop? troop)
    {
        if (_player == -1)
        {
            _player = troop.Player;
        }
        else
        {
            if (_player != troop.Player)
            {
                throw new InvalidOperationException("Player mismatch!");
            }
        }

        if (Units.IndexOfUnitId(troop.UnitId) != -1)
        {
            throw new InvalidOperationException("Unit already exists! (TAIMoveExecuter)");
        }

        MapCoordinate coordinate103 = troop.GetLocation();
        int locationX = coordinate103.X;
        int locationY = coordinate103.Y;
        int locationMapLevel = coordinate103.MapLevel;
        MapCoordinate location = new((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
        AIMoveExecutorUnit unitEntry = new(null) { Holder = CreationHolder() };
        unitEntry.Initialize(troop, location.X, location.Y, L: location.MapLevel);
        unitEntry.Owner = Units;
        return unitEntry;
    }

    // AIGroup.pas:6047
    public void AddUnits(UnitList units)
    {
        for (int i = units.Count - 1; i >= 0; i--)
        {
            _ = AddUnit(units.GetUnit(i));
        }
    }

    // AIGroup.pas:6053
    public void AddArmy(ArmyHexSprite army, int selection)
    {
        for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                _ = AddUnit(army.Army.Units.GetUnit(i));
            }

            selection >>>= 1;
        }
    }

    // AIGroup.pas:6063
    public void AddTransporter(Troop troop)
    {
        if (Transporter is not null)
        {
            throw new InvalidOperationException("Transporter already exists! (TAIMoveExecuter)");
        }

        int index = Units.IndexOfUnitId(troop.UnitId);
        if (index != -1)
        {
            Units.GetUnits(index)?.Dispose();
        }

        MapCoordinate coordinate104 = troop.GetLocation();
        int locationX = coordinate104.X;
        int locationY = coordinate104.Y;
        int locationMapLevel = coordinate104.MapLevel;
        MapCoordinate location = new((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
        Transporter = new AIMoveExecutorUnit(null) { Holder = CreationHolder() };
        Transporter.Initialize(troop, location.X, location.Y, L: location.MapLevel);
    }

    // AIGroup.pas:6077
    public void InvalidateMove(ArmyHexSprite? army, int selection)
    {
        Status = AIMoveExecutorStatus.Invalidated;
        UpdateMoveResult();
        _done?.Invoke(this);
    }

    // AIGroup.pas:6084
    public void Reset()
    {
        Clear();
        Status = AIMoveExecutorStatus.Idle;
        State = 0;
        _done = null;
        ControlQueue.Clear();
        _player = -1;
        MovePointsMax = 200;
    }

    // AIGroup.pas:6095
    public bool Validate()
    {
        bool valid = true;
        if (Transporter is not null)
        {
            valid = valid && Transporter.Validate();
        }

        for (int i = Units.Count - 1; i >= 0; i--)
        {
            valid = valid && Units.GetUnits(i).Validate();
        }

        return valid;
    }

    // AIGroup.pas:6106
    public AIMoveExecutorArmyList CreateArmies(AIMoveExecutorUnitStatuses status)
    {
        Int32List used = [];
        AIMoveExecutorArmyList armies = new(null) { Holder = CreationHolder() };
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if (used.IndexOf(i) == -1)
            {
                AIMoveExecutorUnit unitEntry = Units.GetUnits(i);
                if (status.Contains(unitEntry.Status.ToFlag()))
                {
                    _ = used.Add(i);
                    AIMoveExecutorArmy army = new(null) { Holder = CreationHolder() };
                    army.Setup(unitEntry);
                    for (int j = 0, lastJ = Units.Count - 1; j <= lastJ; j++)
                    {
                        if (used.IndexOf(j) == -1)
                        {
                            unitEntry = Units.GetUnits(j);
                            if (status.Contains(unitEntry.Status.ToFlag()) && army.CanAdd(unitEntry))
                            {
                                _ = army.AddChild(unitEntry);
                                _ = used.Add(j);
                            }
                        }
                    }

                    army.Owner = armies;
                }
            }
        }

        used?.Dispose();
        return armies;
    }

    // AIGroup.pas:6142
    public void FinishMoves() => Status = AIMoveExecutorStatus.Done;

    // AIGroup.pas:6147
    public void ExecuteMoves()
    {
        if (Moves.Count != 0)
        {
            State = 1;
        }
    }

    // AIGroup.pas:6152
    public void UnitsMoveDone()
    {
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            if (Units.GetUnits(i).Status != AIMoveExecutorUnitStatus.Lost)
            {
                AIMoveExecutorUnit units = Units.GetUnits(i);
                Units.GetUnits(i).Troop.UnitStatus = units.Troop.UnitStatus | UnitStatuses.MoveDone;
            }
        }
    }

    // AIGroup.pas:6161
    public bool GetTransporting()
    {
        bool transporting = false;
        int i = Units.Count;
        while (!transporting && i != 0)
        {
            i -= 1;
            AIMoveExecutorUnit units = Units.GetUnits(i);
            transporting = units.TransportStatus is AIMoveExecutorTransportStatus.Boarding or AIMoveExecutorTransportStatus.Transporting;
        }

        return transporting;
    }

    // AIGroup.pas:6176
    public void UpdateMoveResult()
    {
        if (Status == AIMoveExecutorStatus.Done)
        {
            int i = 0;
            while (true)
            {
                bool condition = i != Units.Count;
                if (condition)
                {
                    AIMoveExecutorUnit units = Units.GetUnits(i);
                    condition = units.Location == Destination || Units.GetUnits(i).MoveResult == 2;
                }

                if (!condition)
                {
                    break;
                }

                i += 1;
            }

            if (i != Units.Count)
            {
                Status = AIMoveExecutorStatus.OutOfMovePoints;
            }
        }
    }

    // AIGroup.pas:6189
    public void ProcessExecuteMoves()
    {
        if (Moves.Count != 0)
        {
            int selection;
            ArmyHexSprite army;
            if (Moves.Count > 1 && Mode.Contains(AIMoveExecutorModes.Simultaneous))
            {
                MultiMoveTokenEvent multiMove = new(null)
                {
                    Holder = CreationHolder(),
                    Simultaneous = false
                };
                for (int i = Moves.Count - 1; i >= 0; i--)
                {
                    if (Moves.GetMoves(i).RetrieveArmy(out army, out selection))
                    {
                        ArmyDefaultMoveTokenEvent move = new(null) { Holder = CreationHolder() };
                        AIMoveExecutorMove moves = Moves.GetMoves(i);
                        if (!move.Setup(army, selection, moves.Path, mode: ArmyMoveModes.None))
                        {
                            throw new InvalidOperationException("FAILURE: TAIMoveExecuter.ExecuteMoves");
                        }

                        Moves.GetMoves(i).SetUnitStatus(AIMoveExecutorUnitStatus.Moving);
                        multiMove.Events.Add(move);
                        _ = move.Release();
                    }
                }

                StrategicMap.Current.TokenManager.GetTokenControl(_player).AddEvent(multiMove);
                _ = multiMove.Release();
                Moves.Clear();
            }
            else
            {
                if (Moves.GetMoves(0).RetrieveArmy(out army, out selection))
                {
                    AIMoveExecutorMove moves = Moves.GetMoves(0);
                    if (!ArmyHexSprite.MoveArmy(army, selection, moves.Path))
                    {
                        Moves.GetMoves(0).SetUnitStatus(AIMoveExecutorUnitStatus.Failed);
                        Status = AIMoveExecutorStatus.Failed;
                    }
                    else
                    {
                        Moves.GetMoves(0).SetUnitStatus(AIMoveExecutorUnitStatus.Moving);
                    }
                }

                Moves.GetMoves(0).Dispose();
            }
        }
        else
        {
            State = 0;
        }
    }

    // AIGroup.pas:6232
    public void ProcessMoveExecutorControls()
    {
        int timeLeft = 5;
        while (timeLeft > 0 && (Control is not null || ControlQueue.Count != 0) && Status == AIMoveExecutorStatus.Busy && State == 0)
        {
            if (Control is not null)
            {
                if (!Control.Process())
                {
                    DeactivateMoveExecutorControl();
                }

                timeLeft = StrategicMap.Current.AIExecutor.ProcessTimeLeft;
            }

            if (Control is null && ControlQueue.Count != 0 && Status == AIMoveExecutorStatus.Busy)
            {
                switch (ControlQueue[^1])
                {
                    case 0:
                        ActivateMoveExecutorControl(() => new DefaultMoveExecutorControl(null) { Holder = CreationHolder() });
                        break;
                    case 2:
                        ActivateMoveExecutorControl(() => new AdjacentMoveExecutorControl(null) { Holder = CreationHolder() });
                        break;
                    case 1:
                        ActivateMoveExecutorControl(() => new TransportMoveExecutorControl(null) { Holder = CreationHolder() });
                        break;
                    default:
                        break;
                }

                ControlQueue.Delete(ControlQueue.Count - 1);
            }
        }

        if (Control is null && Status == AIMoveExecutorStatus.Busy && State == 0)
        {
            Status = AIMoveExecutorStatus.Done;
        }
    }

    // AIGroup.pas:6257
    public bool Process()
    {
        if (Validate())
        {
            try
            {
                if (Status == AIMoveExecutorStatus.Busy)
                {
                    _processCycles += 1;
                    if (State == 1)
                    {
                        ProcessExecuteMoves();
                    }

                    if (State == 0)
                    {
                        ProcessMoveExecutorControls();
                    }
                }
            }
            catch
            {
                Status = AIMoveExecutorStatus.Invalidated;
            }
        }
        else
        {
            Status = AIMoveExecutorStatus.Invalidated;
        }

        bool busy = Status == AIMoveExecutorStatus.Busy;
        if (!busy)
        {
            UpdateMoveResult();
            try
            {
                _done?.Invoke(this);
            }
            catch
            {
                ShowMessage("MoveExecute.OnDone failed");
            }
        }

        return busy;
    }

    // AIGroup.pas:6285
    public void SetupTransporter()
    {
        _ = ControlQueue.Add(1);
        int capacity = Transporter.Troop.GetTransportCapacity();
        capacity = capacity > 4 ? capacity - 4 : 1;

        ArmyHexSprite army = StrategicMap.Current.MapContainer.GetField(Transporter.Location.X, Transporter.Location.Y, Transporter.Location.MapLevel).GetArmyHexSprite();

        int i = 0;
        Troop? troop;
        AIMoveExecutorUnit unitEntry;
        while (capacity > 0 && i != army.Army.Units.Count)
        {
            troop = army.Army.Units.GetUnit(i);
            if (troop != Transporter.Troop)
            {
                if (Units.IndexOfUnitId(troop.UnitId) == -1 && !troop.GetTransporting())
                {
                    unitEntry = AddUnit(troop);
                    unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Guarding;
                    capacity -= 1;
                }
            }

            i += 1;
        }

        i = 0;
        while (i != army.Army.Units.Count)
        {
            troop = army.Army.Units.GetUnit(i);
            if (troop != Transporter.Troop)
            {
                if (Units.IndexOfUnitId(troop.UnitId) == -1)
                {
                    unitEntry = AddUnit(troop);
                    if (capacity > 0)
                    {
                        unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Guarding;
                        capacity -= 1;
                    }
                    else
                    {
                        unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Dumping;
                    }

                    // { The original revisits the index after adding a unit to the executer. }
                    i -= 1;
                }
            }

            i += 1;
        }

        Transporter.Status = AIMoveExecutorUnitStatus.Ready;
    }

    // AIGroup.pas:6332
    public bool ExecuteMove(int X, int Y, int L)
    {
        bool started;
        if (Status != AIMoveExecutorStatus.Busy)
        {
            State = 0;
            _processCycles = 0;
            Destination = new MapCoordinate((sbyte)X, (sbyte)Y, (sbyte)L);
            AIMoveExecutorArmyList aiMoveExecutorArmyList = CreateArmies(AIMoveExecutorUnitStatuses.Idle);
            for (int i = 0, last = aiMoveExecutorArmyList.Count - 1; i <= last; i++)
            {
                aiMoveExecutorArmyList.GetArmies(i).SetStatus(255, status: AIMoveExecutorUnitStatus.Ready);
            }

            if (aiMoveExecutorArmyList.Count != 0)
            {
                bool condition = aiMoveExecutorArmyList.Count != 1;
                if (!condition)
                {
                    AIMoveExecutorArmy armies = aiMoveExecutorArmyList.GetArmies(0);
                    condition = armies.Location != Destination;
                }

                if (condition)
                {
                    Status = AIMoveExecutorStatus.Busy;
                    _ = Mode.Contains(AIMoveExecutorModes.Adjacent) && aiMoveExecutorArmyList.Count > 1
                        ? ControlQueue.Add(2)
                        : ControlQueue.Add(0);

                    if (Transporter is not null)
                    {
                        SetupTransporter();
                    }

                    _ = Process();
                }
                else
                {
                    Status = AIMoveExecutorStatus.Done;
                    _ = Process();
                }

                started = true;
            }
            else
            {
                Status = AIMoveExecutorStatus.Failed;
                started = false;
            }

            aiMoveExecutorArmyList?.Dispose();
        }
        else
        {
            started = false;
        }

        return started;
    }
}

public enum AIMoveExecutorStatus : byte
{
    Idle = 0, Busy = 1, Failed = 2, Done = 3, OutOfMovePoints = 4, Invalidated = 5
}
