// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// destination, control, control queue, moves, units, transporter, move limit, mode, state,
// status, player, process cycles, completion event
public partial class AIMoveExecutor : EngineObject
{
    // AIGroup.pas:5976
    public AIMoveExecutor(ECustomNode owner) : base(owner)
    {
        Units = new AIMoveExecutorUnitList(null);
        Moves = new AIMoveExecutorMoveQueue(null);
        ControlQueue = new Int32List();
        Clear();
    }

    public MapCoordinate Destination;

    public AIMoveExecutorControl Control { get; private set; }
    public Int32List ControlQueue { get; private set; }
    public AIMoveExecutorMoveQueue Moves { get; private set; }
    public AIMoveExecutorUnitList Units { get; private set; }
    public AIMoveExecutorUnit Transporter { get; private set; }
    public int MovePointsMax { get; private set; }
    public Set<AIMoveExecutorModeElement> Mode { get; set; }
    public int State { get; private set; }
    public AIMoveExecutorStatus Status { get; set; }

    private sbyte _player;
    private int _processCycles;

    public DoneEvent OnDone { get; set; }

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

        if (Transporter != null)
        {
            Transporter?.Dispose();
            Transporter = null;
        }

        Units.Clear();
        Mode = Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent, AIMoveExecutorModeElement.Simultaneous);
    }

    // AIGroup.pas:5997
    public void ActivateMoveExecutorControl(Func<AIMoveExecutorControl> createControl)
    {
        DeactivateMoveExecutorControl();
        if (createControl != null)
        {
            Control = createControl();
            Control.Activate(this);
        }
    }

    // AIGroup.pas:6007
    public void DeactivateMoveExecutorControl()
    {
        if (Control != null)
        {
            Control.Deactivate();
            Control.Dispose();
            Control = null;
        }
    }

    // AIGroup.pas:6031
    public AIMoveExecutorUnit AddUnit(Troop troop)
    {
        MapCoordinate location = default;
        AIMoveExecutorUnit unitEntry;
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

        troop.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
        unitEntry = new AIMoveExecutorUnit(null);
        unitEntry.Initialize(troop, location.X, location.Y, location.MapLevel);
        unitEntry.Owner = Units;
        return unitEntry;
    }

    // AIGroup.pas:6047
    public void AddUnits(UnitList units)
    {
        for (int I = units.Count - 1; I >= 0; I--)
        {
            AddUnit(units.GetUnit(I));
        }
    }

    // AIGroup.pas:6053
    public void AddArmy(ArmyHexSprite army, byte selection)
    {
        for (int I = 0, last = army.Army.Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                AddUnit(army.Army.GetUnit(I));
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // AIGroup.pas:6063
    public void AddTransporter(Troop troop)
    {
        MapCoordinate location = default;
        int index;
        if (Transporter != null)
        {
            throw new InvalidOperationException("Transporter already exists! (TAIMoveExecuter)");
        }

        index = Units.IndexOfUnitId(troop.UnitId);
        if (index != -1)
        {
            Units.GetUnits(index)?.Dispose();
        }

        troop.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
        Transporter = new AIMoveExecutorUnit(null);
        Transporter.Initialize(troop, location.X, location.Y, location.MapLevel);
    }

    // AIGroup.pas:6077
    public void InvalidateMove(ArmyHexSprite army, byte selection)
    {
        Status = AIMoveExecutorStatus.Invalidated;
        UpdateMoveResult();
        if (OnDone != null)
        {
            OnDone(this);
        }
    }

    // AIGroup.pas:6084
    public void Reset()
    {
        Clear();
        Status = AIMoveExecutorStatus.Idle;
        State = 0;
        OnDone = null;
        ControlQueue.Clear();
        _player = -1;
        MovePointsMax = 200;
    }

    // AIGroup.pas:6095
    public bool Validate()
    {
        bool valid;
        valid = true;
        if (Transporter != null)
        {
            valid = valid && Transporter.Validate();
        }

        for (int I = Units.Count - 1; I >= 0; I--)
        {
            valid = valid && Units.GetUnits(I).Validate();
        }

        return valid;
    }

    // AIGroup.pas:6106
    public AIMoveExecutorArmyList CreateArmies(Set<AIMoveExecutorUnitStatus> status)
    {
        Int32List used;
        AIMoveExecutorArmyList armies;
        AIMoveExecutorUnit unitEntry;
        AIMoveExecutorArmy army;
        used = new Int32List();
        armies = new AIMoveExecutorArmyList(null);
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            if (used.IndexOf(I) == -1)
            {
                unitEntry = Units.GetUnits(I);
                if (status.Contains(unitEntry.Status))
                {
                    used.Add(I);
                    army = new AIMoveExecutorArmy(null);
                    army.Setup(unitEntry);
                    for (int J = 0, lastJ = Units.Count - 1; J <= lastJ; J++)
                    {
                        if (used.IndexOf(J) == -1)
                        {
                            unitEntry = Units.GetUnits(J);
                            if (status.Contains(unitEntry.Status) && army.CanAdd(unitEntry))
                            {
                                army.AddChild(unitEntry);
                                used.Add(J);
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
    public void FinishMoves()
    {
        Status = AIMoveExecutorStatus.Done;
    }

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
        for (int I = Units.Count - 1; I >= 0; I--)
        {
            if (Units.GetUnits(I).Status != AIMoveExecutorUnitStatus.Lost)
            {
                var units = Units.GetUnits(I);
                Units.GetUnits(I).Troop.UnitStatus = units.Troop.UnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.MoveDone);
            }
        }
    }

    // AIGroup.pas:6161
    public bool GetTransporting()
    {
        bool transporting;
        int I;
        transporting = false;
        I = Units.Count;
        while (!transporting && I != 0)
        {
            I -= 1;
            var units = Units.GetUnits(I);
            transporting = Set<AIMoveExecutorTransportStatus>.Of(AIMoveExecutorTransportStatus.Boarding, AIMoveExecutorTransportStatus.Transporting).Contains(units.TransportStatus);
        }

        return transporting;
    }

    // AIGroup.pas:6176
    public void UpdateMoveResult()
    {
        int I;
        if (Status == AIMoveExecutorStatus.Done)
        {
            I = 0;
            while (true)
            {
                bool condition = I != Units.Count;
                if (condition)
                {
                    var units = Units.GetUnits(I);
                    condition = Unsafe.As<MapCoordinate, int>(ref units.Location) == Unsafe.As<MapCoordinate, int>(ref Destination) || Units.GetUnits(I).MoveResult == 2;
                }

                if (!condition)
                    break;
                I += 1;
            }

            if (I != Units.Count)
            {
                Status = AIMoveExecutorStatus.OutOfMovePoints;
            }
        }
    }

    // AIGroup.pas:6189
    public void ProcessExecuteMoves()
    {
        MultiMoveTokenEvent multiMove;
        byte selection = default;
        ArmyHexSprite army = default;
        ArmyDefaultMoveTokenEvent move;
        if (Moves.Count != 0)
        {
            if (Moves.Count > 1 && Mode.Contains(AIMoveExecutorModeElement.Simultaneous))
            {
                multiMove = new MultiMoveTokenEvent(null);
                multiMove.Simultaneous = false;
                for (int I = Moves.Count - 1; I >= 0; I--)
                {
                    if (Moves.GetMoves(I).RetrieveArmy(ref army, ref selection))
                    {
                        move = new ArmyDefaultMoveTokenEvent(null);
                        var moves = Moves.GetMoves(I);
                        if (!move.Setup(army, selection, moves.Path, Set<ArmyMoveModeElement>.Of()))
                        {
                            throw new InvalidOperationException("FAILURE: TAIMoveExecuter.ExecuteMoves");
                        }

                        Moves.GetMoves(I).SetUnitStatus(AIMoveExecutorUnitStatus.Moving);
                        multiMove.Events.Add(move);
                        move.Release();
                    }
                }

                StrategicMap.Current.TokenManager.GetTokenControl(_player).AddEvent(multiMove);
                multiMove.Release();
                Moves.Clear();
            }
            else
            {
                if (Moves.GetMoves(0).RetrieveArmy(ref army, ref selection))
                {
                    var moves = Moves.GetMoves(0);
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
        int timeLeft = default;
        timeLeft = 5;
        while (timeLeft > 0 && (Control != null || ControlQueue.Count != 0) && Status == AIMoveExecutorStatus.Busy && State == 0)
        {
            if (Control != null)
            {
                if (!Control.Process())
                {
                    DeactivateMoveExecutorControl();
                }

                timeLeft = StrategicMap.Current.AIExecutor.GetProcessTimeLeft();
            }

            if (Control == null && ControlQueue.Count != 0 && Status == AIMoveExecutorStatus.Busy)
            {
                switch (ControlQueue[ControlQueue.Count - 1])
                {
                    case 0:
                        ActivateMoveExecutorControl(static () => new DefaultMoveExecutorControl(null));
                        break;
                    case 2:
                        ActivateMoveExecutorControl(static () => new AdjacentMoveExecutorControl(null));
                        break;
                    case 1:
                        ActivateMoveExecutorControl(static () => new TransportMoveExecutorControl(null));
                        break;
                }

                ControlQueue.Delete(ControlQueue.Count - 1);
            }
        }

        if (Control == null && Status == AIMoveExecutorStatus.Busy && State == 0)
        {
            Status = AIMoveExecutorStatus.Done;
        }
    }

    // AIGroup.pas:6257
    public bool Process()
    {
        bool busy;
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

        busy = Status == AIMoveExecutorStatus.Busy;
        if (!busy)
        {
            UpdateMoveResult();
            try
            {
                if (OnDone != null)
                {
                    OnDone(this);
                }
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
        int I;
        int capacity;
        ArmyHexSprite army;
        Troop troop;
        AIMoveExecutorUnit unitEntry;
        ControlQueue.Add(1);
        capacity = Transporter.Troop.GetTransportCapacity();
        if (capacity > 4)
        {
            capacity = capacity - 4;
        }
        else
        {
            capacity = 1;
        }

        army = StrategicMap.Current.MapContainer.GetField(this.Transporter.Location.X, this.Transporter.Location.Y, this.Transporter.Location.MapLevel).GetArmyHexSprite();

        I = 0;
        while (capacity > 0 && I != army.Army.Count)
        {
            troop = army.Army.GetUnit(I);
            if (troop != Transporter.Troop)
            {
                if (Units.IndexOfUnitId(troop.UnitId) == -1 && !troop.GetTransporting())
                {
                    unitEntry = AddUnit(troop);
                    unitEntry.TransportStatus = AIMoveExecutorTransportStatus.Guarding;
                    capacity -= 1;
                }
            }

            I += 1;
        }

        I = 0;
        while (I != army.Army.Count)
        {
            troop = army.Army.GetUnit(I);
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
                    I -= 1;
                }
            }

            I += 1;
        }

        Transporter.Status = AIMoveExecutorUnitStatus.Ready;
    }

    // AIGroup.pas:6332
    public bool ExecuteMove(sbyte X, sbyte Y, sbyte L)
    {
        AIMoveExecutorArmyList Armies;
        bool started;
        if (Status != AIMoveExecutorStatus.Busy)
        {
            State = 0;
            _processCycles = 0;
            Destination.X = X;
            Destination.Y = Y;
            Destination.MapLevel = L;
            Armies = CreateArmies(Set<AIMoveExecutorUnitStatus>.Of(AIMoveExecutorUnitStatus.Idle));
            for (int I = 0, last = Armies.Count - 1; I <= last; I++)
            {
                Armies.GetArmies(I).SetStatus(255, AIMoveExecutorUnitStatus.Ready);
            }

            if (Armies.Count != 0)
            {
                bool condition = Armies.Count != 1;
                if (!condition)
                {
                    var armies = Armies.GetArmies(0);
                    condition = Unsafe.As<MapCoordinate, int>(ref armies.Location) != Unsafe.As<MapCoordinate, int>(ref Destination);
                }

                if (condition)
                {
                    Status = AIMoveExecutorStatus.Busy;
                    if (Mode.Contains(AIMoveExecutorModeElement.Adjacent) && Armies.Count > 1)
                    {
                        ControlQueue.Add(2);
                    }
                    else
                    {
                        ControlQueue.Add(0);
                    }

                    if (Transporter != null)
                    {
                        SetupTransporter();
                    }

                    Process();
                }
                else
                {
                    Status = AIMoveExecutorStatus.Done;
                    Process();
                }

                started = true;
            }
            else
            {
                Status = AIMoveExecutorStatus.Failed;
                started = false;
            }

            Armies?.Dispose();
        }
        else
        {
            started = false;
        }

        return started;
    }

    public delegate void DoneEvent(object Sender);

    // intf_P5_06.inc:459

}

public enum AIMoveExecutorStatus : byte
{
    Idle = 0, Busy = 1, Failed = 2, Done = 3, OutOfMovePoints = 4, Invalidated = 5
}
