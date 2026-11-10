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

namespace Aow.Game.Ai.Groups;

// destination, control, control queue, moves, units, transporter, move limit, mode, state,
// status, player, process cycles, completion event
public partial class AIMoveExecuter : EObject
{
    // AIGroup.pas:5976
    public AIMoveExecuter(ECustomNode owner) : base(owner)
    {
        FUnits = new AIMoveExecuterUnitList(null);
        FMoves = new AIMoveExecuterMoveQueue(null);
        FControlQueue = new IntegerList();
        Clear();
    }

    public XYL FDestination;
    public AIMoveExecuterControl FControl;
    public IntegerList FControlQueue;
    public AIMoveExecuterMoveQueue FMoves;
    public AIMoveExecuterUnitList FUnits;
    public AIMoveExecuterUnit FTransporter;
    public int FMovePointsMax;
    public Set<AIMoveExecuterModeElement> FMode;
    public int FState;
    public Status FStatus;
    public sbyte FPlayer;
    public int FProcessCycles;
    public DoneEvent FOnDone;

    // AIGroup.pas:5985
    public override void Dispose()
    {
        Clear();
        FUnits?.Dispose();
        FUnits = null;
        FMoves?.Dispose();
        FMoves = null;
        FControlQueue?.Dispose();
        FControlQueue = null;
        base.Dispose();
    }

    // AIGroup.pas:6017
    public override void Clear()
    {
        base.Clear();
        DeactivateMEC();
        if (FMoves.Count != 0)
        {
            FMoves.Clear();
        }

        if (FTransporter != null)
        {
            FTransporter?.Dispose();
            FTransporter = null;
        }

        FUnits.Clear();
        FMode = Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent, AIMoveExecuterModeElement.Simultaneous);
    }

    // AIGroup.pas:5997
    public void ActivateMEC(Func<AIMoveExecuterControl> createControl)
    {
        DeactivateMEC();
        if (createControl != null)
        {
            FControl = createControl();
            FControl.Activate(this);
        }
    }

    // AIGroup.pas:6007
    public void DeactivateMEC()
    {
        if (FControl != null)
        {
            FControl.Deactivate();
            FControl.Dispose();
            FControl = null;
        }
    }

    // AIGroup.pas:6031
    public AIMoveExecuterUnit AddUnit(AbstractUnit AUnit)
    {
        XYL Location = default;
        AIMoveExecuterUnit UnitEntry;
        if (FPlayer == -1)
        {
            FPlayer = AUnit.FPlayer;
        }
        else
        {
            if (FPlayer != AUnit.FPlayer)
            {
                throw new InvalidOperationException("Player mismatch!");
            }
        }

        if (FUnits.IndexOfUnitID(AUnit.FUnitID) != -1)
        {
            throw new InvalidOperationException("Unit already exists! (TAIMoveExecuter)");
        }

        AUnit.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
        UnitEntry = new AIMoveExecuterUnit(null);
        UnitEntry.Initialize(AUnit, Location.X, Location.Y, Location.L);
        UnitEntry.Owner = FUnits;
        return UnitEntry;
    }

    // AIGroup.pas:6047
    public void AddUnits(UnitList Units)
    {
        for (int I = Units.Count - 1; I >= 0; I--)
        {
            AddUnit(Units.GetUnit(I));
        }
    }

    // AIGroup.pas:6053
    public void AddArmy(ArmyHS Army, byte Selection)
    {
        for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                AddUnit(Army.FArmy.GetUnit(I));
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // AIGroup.pas:6063
    public void AddTransporter(AbstractUnit AUnit)
    {
        XYL Location = default;
        int Index;
        if (FTransporter != null)
        {
            throw new InvalidOperationException("Transporter already exists! (TAIMoveExecuter)");
        }

        Index = FUnits.IndexOfUnitID(AUnit.FUnitID);
        if (Index != -1)
        {
            FUnits.GetUnits(Index)?.Dispose();
        }

        AUnit.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
        FTransporter = new AIMoveExecuterUnit(null);
        FTransporter.Initialize(AUnit, Location.X, Location.Y, Location.L);
    }

    // AIGroup.pas:6077
    public void InvalidateMove(ArmyHS Army, byte Selection)
    {
        FStatus = Status.Invalidated;
        UpdateMoveResult();
        if (FOnDone != null)
        {
            FOnDone(this);
        }
    }

    // AIGroup.pas:6084
    public void Reset()
    {
        Clear();
        FStatus = Status.Idle;
        FState = 0;
        FOnDone = null;
        FControlQueue.Clear();
        FPlayer = -1;
        FMovePointsMax = 200;
    }

    // AIGroup.pas:6095
    public bool Validate()
    {
        bool Valid;
        Valid = true;
        if (FTransporter != null)
        {
            Valid = Valid && FTransporter.Validate();
        }

        for (int I = FUnits.Count - 1; I >= 0; I--)
        {
            Valid = Valid && FUnits.GetUnits(I).Validate();
        }

        return Valid;
    }

    // AIGroup.pas:6106
    public AIMoveExecuterArmyList CreateArmies(Set<AIMoveExecuterUnitStatus> Status)
    {
        IntegerList Used;
        AIMoveExecuterArmyList Armies;
        AIMoveExecuterUnit UnitEntry;
        AIMoveExecuterArmy Army;
        Used = new IntegerList();
        Armies = new AIMoveExecuterArmyList(null);
        for (int I = 0, last = FUnits.Count - 1; I <= last; I++)
        {
            if (Used.IndexOf(I) == -1)
            {
                UnitEntry = FUnits.GetUnits(I);
                if (Status.Contains(UnitEntry.FStatus))
                {
                    Used.Add(I);
                    Army = new AIMoveExecuterArmy(null);
                    Army.Setup(UnitEntry);
                    for (int J = 0, lastJ = FUnits.Count - 1; J <= lastJ; J++)
                    {
                        if (Used.IndexOf(J) == -1)
                        {
                            UnitEntry = FUnits.GetUnits(J);
                            if (Status.Contains(UnitEntry.FStatus) && Army.CanAdd(UnitEntry))
                            {
                                Army.AddChild(UnitEntry);
                                Used.Add(J);
                            }
                        }
                    }

                    Army.Owner = Armies;
                }
            }
        }

        Used?.Dispose();
        return Armies;
    }

    // AIGroup.pas:6142
    public void FinishMoves()
    {
        FStatus = Status.Done;
    }

    // AIGroup.pas:6147
    public void ExecuteMoves()
    {
        if (FMoves.Count != 0)
        {
            FState = 1;
        }
    }

    // AIGroup.pas:6152
    public void UnitsMoveDone()
    {
        for (int I = FUnits.Count - 1; I >= 0; I--)
        {
            if (FUnits.GetUnits(I).FStatus != AIMoveExecuterUnitStatus.Lost)
            {
                var units = FUnits.GetUnits(I);
                FUnits.GetUnits(I).FUnit.FUnitStatus = units.FUnit.FUnitStatus + Set<UnitStatusElement>.Of(UnitStatusElement.MoveDone);
            }
        }
    }

    // AIGroup.pas:6161
    public bool GetTransporting()
    {
        bool Transporting;
        int I;
        Transporting = false;
        I = FUnits.Count;
        while (!Transporting && I != 0)
        {
            I -= 1;
            var units = FUnits.GetUnits(I);
            Transporting = Set<AIMoveExecuterTransportStatus>.Of(AIMoveExecuterTransportStatus.Boarding, AIMoveExecuterTransportStatus.Transporting).Contains(units.FTransportStatus);
        }

        return Transporting;
    }

    // AIGroup.pas:6176
    public void UpdateMoveResult()
    {
        int I;
        if (FStatus == Status.Done)
        {
            I = 0;
            while (true)
            {
                bool condition = I != FUnits.Count;
                if (condition)
                {
                    var units = FUnits.GetUnits(I);
                    condition = Unsafe.As<XYL, int>(ref units.FLocation) == Unsafe.As<XYL, int>(ref FDestination) || FUnits.GetUnits(I).FMoveResult == 2;
                }

                if (!condition)
                    break;
                I += 1;
            }

            if (I != FUnits.Count)
            {
                FStatus = Status.OutOfMovePoints;
            }
        }
    }

    // AIGroup.pas:6189
    public void ProcessExecuteMoves()
    {
        MultiMoveTE MultiMove;
        byte Selection = default;
        ArmyHS Army = default;
        ArmyDefaultMoveTE Move;
        if (FMoves.Count != 0)
        {
            if (FMoves.Count > 1 && FMode.Contains(AIMoveExecuterModeElement.Simultaneous))
            {
                MultiMove = new MultiMoveTE(null);
                MultiMove.FSimultaneous = false;
                for (int I = FMoves.Count - 1; I >= 0; I--)
                {
                    if (FMoves.GetMoves(I).RetrieveArmy(ref Army, ref Selection))
                    {
                        Move = new ArmyDefaultMoveTE(null);
                        var moves = FMoves.GetMoves(I);
                        if (!Move.Setup(Army, Selection, moves.FPath, Set<ArmyMoveModeElement>.Of()))
                        {
                            throw new InvalidOperationException("FAILURE: TAIMoveExecuter.ExecuteMoves");
                        }

                        FMoves.GetMoves(I).SetUnitStatus(AIMoveExecuterUnitStatus.Moving);
                        MultiMove.FEvents.Add(Move);
                        Move.Release();
                    }
                }

                AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(MultiMove);
                MultiMove.Release();
                FMoves.Clear();
            }
            else
            {
                if (FMoves.GetMoves(0).RetrieveArmy(ref Army, ref Selection))
                {
                    var moves = FMoves.GetMoves(0);
                    if (!ArmyHS.MoveArmy(Army, Selection, moves.FPath))
                    {
                        FMoves.GetMoves(0).SetUnitStatus(AIMoveExecuterUnitStatus.Failed);
                        FStatus = Status.Failed;
                    }
                    else
                    {
                        FMoves.GetMoves(0).SetUnitStatus(AIMoveExecuterUnitStatus.Moving);
                    }
                }

                FMoves.GetMoves(0).Dispose();
            }
        }
        else
        {
            FState = 0;
        }
    }

    // AIGroup.pas:6232
    public void ProcessMECS()
    {
        int TimeLeft = default;
        TimeLeft = 5;
        while (TimeLeft > 0 && (FControl != null || FControlQueue.Count != 0) && FStatus == Status.Busy && FState == 0)
        {
            if (FControl != null)
            {
                if (!FControl.Process())
                {
                    DeactivateMEC();
                }

                TimeLeft = AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft();
            }

            if (FControl == null && FControlQueue.Count != 0 && FStatus == Status.Busy)
            {
                switch (FControlQueue[FControlQueue.Count - 1])
                {
                    case 0:
                        ActivateMEC(static () => new DefaultMEC(null));
                        break;
                    case 2:
                        ActivateMEC(static () => new AdjacentMEC(null));
                        break;
                    case 1:
                        ActivateMEC(static () => new TransportMEC(null));
                        break;
                }

                FControlQueue.Delete(FControlQueue.Count - 1);
            }
        }

        if (FControl == null && FStatus == Status.Busy && FState == 0)
        {
            FStatus = Status.Done;
        }
    }

    // AIGroup.pas:6257
    public bool Process()
    {
        bool Busy;
        if (Validate())
        {
            try
            {
                if (FStatus == Status.Busy)
                {
                    FProcessCycles += 1;
                    if (FState == 1)
                    {
                        ProcessExecuteMoves();
                    }

                    if (FState == 0)
                    {
                        ProcessMECS();
                    }
                }
            }
            catch
            {
                FStatus = Status.Invalidated;
            }
        }
        else
        {
            FStatus = Status.Invalidated;
        }

        Busy = FStatus == Status.Busy;
        if (!Busy)
        {
            UpdateMoveResult();
            try
            {
                if (FOnDone != null)
                {
                    FOnDone(this);
                }
            }
            catch
            {
                ShowMessage("MoveExecute.OnDone failed");
            }
        }

        return Busy;
    }

    // AIGroup.pas:6285
    public void SetupTransporter()
    {
        int I;
        int Capacity;
        ArmyHS Army;
        AbstractUnit AUnit;
        AIMoveExecuterUnit UnitEntry;
        FControlQueue.Add(1);
        Capacity = FTransporter.FUnit.GetTransportCapacity();
        if (Capacity > 4)
        {
            Capacity = Capacity - 4;
        }
        else
        {
            Capacity = 1;
        }

        Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(this.FTransporter.FLocation.X, this.FTransporter.FLocation.Y, this.FTransporter.FLocation.L)).GetArmyHS();

        I = 0;
        while (Capacity > 0 && I != Army.FArmy.Count)
        {
            AUnit = Army.FArmy.GetUnit(I);
            if (AUnit != FTransporter.FUnit)
            {
                if (FUnits.IndexOfUnitID(AUnit.FUnitID) == -1 && !AUnit.GetTransporting())
                {
                    UnitEntry = AddUnit(AUnit);
                    UnitEntry.FTransportStatus = AIMoveExecuterTransportStatus.Guarding;
                    Capacity -= 1;
                }
            }

            I += 1;
        }

        I = 0;
        while (I != Army.FArmy.Count)
        {
            AUnit = Army.FArmy.GetUnit(I);
            if (AUnit != FTransporter.FUnit)
            {
                if (FUnits.IndexOfUnitID(AUnit.FUnitID) == -1)
                {
                    UnitEntry = AddUnit(AUnit);
                    if (Capacity > 0)
                    {
                        UnitEntry.FTransportStatus = AIMoveExecuterTransportStatus.Guarding;
                        Capacity -= 1;
                    }
                    else
                    {
                        UnitEntry.FTransportStatus = AIMoveExecuterTransportStatus.Dumping;
                    }

                    // { The original revisits the index after adding a unit to the executer. }
                    I -= 1;
                }
            }

            I += 1;
        }

        FTransporter.FStatus = AIMoveExecuterUnitStatus.Ready;
    }

    // AIGroup.pas:6332
    public bool ExecuteMove(sbyte X, sbyte Y, sbyte L)
    {
        AIMoveExecuterArmyList Armies;
        bool Started;
        if (FStatus != Status.Busy)
        {
            FState = 0;
            FProcessCycles = 0;
            FDestination.X = X;
            FDestination.Y = Y;
            FDestination.L = L;
            Armies = CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Idle));
            for (int I = 0, last = Armies.Count - 1; I <= last; I++)
            {
                Armies.GetArmies(I).SetStatus(255, AIMoveExecuterUnitStatus.Ready);
            }

            if (Armies.Count != 0)
            {
                bool condition = Armies.Count != 1;
                if (!condition)
                {
                    var armies = Armies.GetArmies(0);
                    condition = Unsafe.As<XYL, int>(ref armies.FLocation) != Unsafe.As<XYL, int>(ref FDestination);
                }

                if (condition)
                {
                    FStatus = Status.Busy;
                    if (FMode.Contains(AIMoveExecuterModeElement.Adjacent) && Armies.Count > 1)
                    {
                        FControlQueue.Add(2);
                    }
                    else
                    {
                        FControlQueue.Add(0);
                    }

                    if (FTransporter != null)
                    {
                        SetupTransporter();
                    }

                    Process();
                }
                else
                {
                    FStatus = Status.Done;
                    Process();
                }

                Started = true;
            }
            else
            {
                FStatus = Status.Failed;
                Started = false;
            }

            Armies?.Dispose();
        }
        else
        {
            Started = false;
        }

        return Started;
    }

    public enum Status : byte
    {
        Idle = 0, Busy = 1, Failed = 2, Done = 3, OutOfMovePoints = 4, Invalidated = 5
    }

    public delegate void DoneEvent(object Sender);

    // intf_P5_06.inc:459

}
