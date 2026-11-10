// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// meeting point
public partial class TransportMEC : AIMoveExecuterControl
{
    public TransportMEC(ECustomNode owner) : base(owner)
    { }

    public XYL FMeetingPoint;

    // AIGroup.pas:5741
    public override void CalculateMoves()
    {
        AIMoveExecuterArmyList Transporters = default;
        AIMoveExecuterArmyList Armies = default;
        AIMoveControlSettings Settings = default;
        int I = default;
        int TimeLeft = default;
        int Index = default;
        byte Selection = default;
        ArmyHS ArmyHS = default;
        AIMoveExecuterArmy Army = default;
        AIMoveExecuterMove Move = default;
        switch (FState)
        {
            case 0:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
                I = 0;
                TimeLeft = 5;
                while (I < Armies.Count && FMoveControl.FStatus != AIMoveControlStatus.Processing && TimeLeft > 2)
                {
                    if (Armies.GetArmies(I).FTransportStatus == AIMoveExecuterTransportStatus.Idle)
                    {
                        Settings = new AIMoveControlSettings(null);
                        Armies.GetArmies(I).RetrieveArmy(255, ref ArmyHS, ref Selection);
                        Settings.Setup(ArmyHS, Selection, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L);
                        Settings.FTransporter = FExecuter.FTransporter.FUnit;
                        Settings.FOnDone = CalculateTransportPathCallBack;
                        Settings.FMoveMode = Set<MoveModeElement>.Of(MoveModeElement.AI);
                        FMoveControl.Reset();
                        FMoveControl.CanMove(Settings);
                        if (FMoveControl.FStatus != AIMoveControlStatus.Processing)
                        {
                            TimeLeft = AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft();
                        }

                        Settings.Release();
                    }

                    I += 1;
                }

                Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Idle);
                if (Index == -1)
                {
                    if (!TransportDone())
                    {
                        FState += 1;
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
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
                Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Arriving);
                if (Index != -1)
                {
                    ref var location = ref Armies.GetArmies(Index).FArrivalPoint;
                    Move = CreateMove(Armies.GetArmies(Index), 255, location.X, location.Y, location.L, Set<CreateMoveModeElement>.Of());

                    Armies.GetArmies(Index).SetTransportStatus(255, AIMoveExecuterTransportStatus.Done);
                    // { The original executes queued moves only when creation returns nil. }
                    if (Move == null)
                    {
                        FExecuter.ExecuteMoves();
                    }
                }
                else
                {
                    FState += 1;
                }

                Armies?.Dispose();
                break;
            case 2:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
                Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Boarding);
                bool condition = Index != -1;
                if (condition)
                {
                    var armies = Armies.GetArmies(Index);
                    condition = Unsafe.As<XYL, int>(ref armies.FTransportSource) == Unsafe.As<XYL, int>(ref FExecuter.FTransporter.FLocation);
                }

                if (condition)
                {
                    I = 0;
                    while (I < Armies.Count && FMoveControl.FStatus != AIMoveControlStatus.Processing)
                    {
                        if (Armies.GetArmies(I).FTransportStatus == AIMoveExecuterTransportStatus.Dumping)
                        {
                            DumpTransportUnits(Armies.GetArmies(I));
                        }

                        I += 1;
                    }

                    Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Dumping);
                    if (Index == -1)
                    {
                        if (!TransportDone())
                        {
                            FState += 1;
                        }
                        else
                        {
                            Done();
                        }
                    }
                }
                else
                {
                    FState += 1;
                }

                Armies?.Dispose();
                break;
            case 3:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
                Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Transporting);
                bool test = Index != -1;
                if (test)
                {
                    var armies = Armies.GetArmies(Index);
                    var armiesArmies = Armies.GetArmies(Index);
                    test = Unsafe.As<XYL, int>(ref armies.FTransportDestination) == Unsafe.As<XYL, int>(ref armiesArmies.FLocation);
                }

                if (test)
                {
                    Transporters = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
                    I = 0;
                    while (I != Transporters.Count)
                    {
                        bool passed = Transporters.GetArmies(I).FTransportStatus == AIMoveExecuterTransportStatus.Done;
                        if (passed)
                        {
                            var armies = Transporters.GetArmies(I);
                            var transportersArmies = Transporters.GetArmies(I);
                            var armies2 = Transporters.GetArmies(I);
                            passed = CanMoveTo(Armies.GetArmies(Index), 255, armies2.FLocation.X, armies.FLocation.Y, transportersArmies.FLocation.L) == 255;
                        }

                        if (passed)
                        {
                            break;
                        }

                        I += 1;
                    }

                    if (I != Transporters.Count)
                    {
                        var aiMoveExecuterArmy = Transporters.GetArmies(I);
                        var armies = Transporters.GetArmies(I);
                        var transportersArmies = Transporters.GetArmies(I);
                        var armies2 = Transporters.GetArmies(I);
                        Move = CreateMove(Armies.GetArmies(Index), 255, armies2.FLocation.X, armies.FLocation.Y, transportersArmies.FLocation.L, Set<CreateMoveModeElement>.Of());
                    }
                    else
                    {
                        ref var location = ref Armies.GetArmies(Index).FArrivalPoint;
                        Move = CreateMove(Armies.GetArmies(Index), 255, location.X, location.Y, location.L, Set<CreateMoveModeElement>.Of());
                    }

                    if (Move == null)
                    {
                        Armies.GetArmies(Index).SetStatus(255, AIMoveExecuterUnitStatus.Done);
                        FExecuter.FTransporter.FStatus = AIMoveExecuterUnitStatus.Done;
                    }
                    else
                    {
                        Armies.GetArmies(Index).SetTransportStatus(255, AIMoveExecuterTransportStatus.Done);
                        FExecuter.ExecuteMoves();
                    }

                    Transporters?.Dispose();
                }

                Armies?.Dispose();
                FState += 1;
                break;
            case 4:
                if (FExecuter.FTransporter.FStatus == AIMoveExecuterUnitStatus.Ready)
                {
                    Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
                    Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Transporting);
                    if (Index != -1)
                    {
                        ref var location = ref Armies.GetArmies(Index).FTransportDestination;
                        Move = CreateTransporterMove(location.X, location.Y, location.L, true);

                        if (Move == null)
                        {
                            Armies.GetArmies(Index).SetStatus(255, AIMoveExecuterUnitStatus.Done);
                            FExecuter.FTransporter.FStatus = AIMoveExecuterUnitStatus.Done;
                        }
                    }

                    Armies?.Dispose();
                }

                FState += 1;
                break;
            case 5:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
                I = 0;
                TimeLeft = 5;
                while (TimeLeft > 2 && I != Armies.Count)
                {
                    if (Armies.GetArmies(I).FTransportStatus == AIMoveExecuterTransportStatus.Boarding)
                    {
                        Army = Armies.GetArmies(I);
                        if (Army.FBoardingPoint.X != -1 && Unsafe.As<XYL, int>(ref Army.FLocation) != Unsafe.As<XYL, int>(ref Army.FBoardingPoint))
                        {
                            Move = CreateMove(Armies.GetArmies(I), 255, Army.FBoardingPoint.X, Army.FBoardingPoint.Y, Army.FBoardingPoint.L, Set<CreateMoveModeElement>.Of());
                            if (Move == null)
                            {
                                Army.SetStatus(255, AIMoveExecuterUnitStatus.Done);
                            }

                            TimeLeft = AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft();
                        }
                    }

                    I += 1;
                }

                if (I == Armies.Count)
                {
                    FState += 1;
                }

                Armies?.Dispose();
                break;
            case 6:
                if (FExecuter.FTransporter.FStatus == AIMoveExecuterUnitStatus.Ready)
                {
                    Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Queued, AIMoveExecuterUnitStatus.Done));
                    Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Boarding);
                    if (Index != -1)
                    {
                        var aiMoveExecuterArmy = Armies.GetArmies(Index);
                        if (Unsafe.As<XYL, int>(ref FExecuter.FTransporter.FLocation) != Unsafe.As<XYL, int>(ref aiMoveExecuterArmy.FTransportSource))
                        {
                            Move = CreateTransporterMove(aiMoveExecuterArmy.FTransportSource.X, aiMoveExecuterArmy.FTransportSource.Y, aiMoveExecuterArmy.FTransportSource.L, true);
                            if (Move == null)
                            {
                                FExecuter.FTransporter.FStatus = AIMoveExecuterUnitStatus.Done;
                            }
                        }
                    }

                    Armies?.Dispose();
                }

                FState += 1;
                break;
            case 7:
                if (FExecuter.FTransporter.FStatus == AIMoveExecuterUnitStatus.Ready)
                {
                    Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Queued, AIMoveExecuterUnitStatus.Done));
                    Index = Armies.IndexOfTransportStatus(AIMoveExecuterTransportStatus.Boarding);
                    if (Index != -1)
                    {
                        Army = Armies.GetArmies(Index);
                        if (Army.WaitingForTransport() && Unsafe.As<XYL, int>(ref FExecuter.FTransporter.FLocation) == Unsafe.As<XYL, int>(ref Army.FTransportSource))
                        {
                            Selection = CanMoveTo(Armies.GetArmies(Index), 255, Army.FTransportSource.X, Army.FTransportSource.Y, Army.FTransportSource.L);
                            if (Selection != 0)
                            {
                                Move = CreateMove(Armies.GetArmies(Index), Selection, Army.FTransportSource.X, Army.FTransportSource.Y, Army.FTransportSource.L, Set<CreateMoveModeElement>.Of());
                                if (Move == null)
                                {
                                    FExecuter.FTransporter.FStatus = AIMoveExecuterUnitStatus.Done;
                                }
                                else
                                {
                                    Armies.GetArmies(Index).SetTransportStatus(Selection, AIMoveExecuterTransportStatus.Transporting);
                                }
                            }
                            else
                            {
                                FExecuter.FTransporter.FStatus = AIMoveExecuterUnitStatus.Done;
                            }
                        }
                    }

                    Armies?.Dispose();
                }

                FState += 1;
                break;
            case 8:
                if (FExecuter.FMoves.Count == 0)
                {
                    Done();
                }
                else
                {
                    FState = 1;
                    FExecuter.ExecuteMoves();
                }

                break;
        }
    }

    // AIGroup.pas:5543
    public override void Done()
    {
        if (!TransportDone())
        {
            FExecuter.FStatus = AIMoveExecuter.Status.OutOfMovePoints;
        }

        for (int I = FExecuter.FUnits.Count - 1; I >= 0; I--)
        {
            var units = FExecuter.FUnits.GetUnits(I);
            if (Set<AIMoveExecuterTransportStatus>.Of(AIMoveExecuterTransportStatus.Guarding, AIMoveExecuterTransportStatus.Dumped).Contains(units.FTransportStatus))
            {
                FExecuter.FUnits.GetUnits(I)?.Dispose();
            }
        }

        FState = -1;
    }

    // AIGroup.pas:5465
    public AIMoveExecuterMove CreateTransporterMove(sbyte X, sbyte Y, sbyte L, bool Queue)
    {
        AIMoveExecuterMove Move;
        int MovePoints;
        int Pos;
        AIMoveExecuterArmy Army;
        AIMoveExecuterUnit UnitEntry;
        ArmyHS ArmyHS;
        MoveControlSettings Settings;
        Move = null;
        Army = new AIMoveExecuterArmy(null);
        Army.Setup(FExecuter.FTransporter);
        for (int I = 0, last = FExecuter.FUnits.Count - 1; I <= last; I++)
        {
            UnitEntry = FExecuter.FUnits.GetUnits(I);
            if (Unsafe.As<XYL, int>(ref UnitEntry.FLocation) == Unsafe.As<XYL, int>(ref FExecuter.FTransporter.FLocation))
            {
                Army.AddChild(UnitEntry);
            }
        }

        var aiMoveExecuterUnit = FExecuter.FTransporter;
        ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(aiMoveExecuterUnit.FLocation.X, aiMoveExecuterUnit.FLocation.Y, aiMoveExecuterUnit.FLocation.L)).GetArmyHS();

        Settings = new MoveControlSettings(null);
        Settings.SetupArmy(ArmyHS, 255);
        Settings.SetDestinationPos(X, Y, L);
        AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
        if (Settings.FPath.Count != 0)
        {
            MovePoints = ArmyHS.FArmy.MovePoints(255);
            Pos = Settings.FPath.MovePointsToPos(MovePoints);
            if (Pos < Settings.FPath.Count - 1)
            {
                while (true)
                {
                    bool condition = Pos != Settings.FPath.Count - 1;
                    if (condition)
                    {
                        var item = Settings.FPath[Pos];
                        var fPathItem = Settings.FPath[Pos];
                        condition = CanMoveTo(Army, 255, Settings.FPath[Pos].X, item.Y, fPathItem.L) != 255;
                    }

                    if (!condition)
                        break;
                    Pos += 1;
                }

                if (Pos < Settings.FPath.Count - 1)
                {
                    Move = new AIMoveExecuterMove(null);
                    Move.AddUnits(Army, 255);
                    for (int I = Pos, last = Settings.FPath.Count - 1; I <= last; I++)
                    {
                        var item = Settings.FPath[I];
                        Move.FPath.Add(item);
                    }

                    Move.FMovePoints = MovePoints;
                    Move.FMoves = ArmyHS.FArmy.Moves(255);
                    Move.FSourceX = ArmyHS.Xhx;
                    Move.FSourceY = ArmyHS.Yhx;
                    Move.FSourceL = ArmyHS.Lhx;
                    Move.FDestinationX = X;
                    Move.FDestinationY = Y;
                    Move.FDestinationL = L;
                    if (Queue)
                    {
                        Move.Owner = FExecuter.FMoves;
                        Move.SetUnitStatus(AIMoveExecuterUnitStatus.Queued);
                    }
                }
            }
        }

        Army?.Dispose();
        Settings.Release();
        return Move;
    }

    // AIGroup.pas:5525
    public bool TransportDone()
    {
        AIMoveExecuter Executer;
        int I;
        bool Done;
        Executer = FExecuter;
        I = Executer.FUnits.Count;
        Done = true;
        while (I != 0 && Done)
        {
            I -= 1;
            var units = Executer.FUnits.GetUnits(I);
            if (!Set<AIMoveExecuterTransportStatus>.Of(AIMoveExecuterTransportStatus.Arriving, AIMoveExecuterTransportStatus.Guarding, AIMoveExecuterTransportStatus.Dumping, AIMoveExecuterTransportStatus.Dumped, AIMoveExecuterTransportStatus.Done).Contains(units.FTransportStatus))
            {
                Done = false;
            }
        }

        return Done;
    }

    // AIGroup.pas:5553
    public void DumpTransportUnits(AIMoveExecuterArmy Army)
    {
        byte Transporting;
        byte Others;
        MoveControlSettings Settings;
        byte Selection = default;
        ArmyHS ArmyHS = default;
        DelphiList Fields;
        DelphiList OtherFields;
        int I;
        AoWMapField Field;
        bool Found;
        Transporting = 0;
        Others = 0;
        I = 0;
        int last = Army.Count - 1;
        for (; I <= last; I += 1)
        {
            if (Army.GetUnits(I).FUnit.GetTransporting())
            {
                Transporting = (byte)(Transporting | (1 << I));
            }
            else
            {
                Others = (byte)(Others | (1 << I));
            }
        }

        if (Transporting != 0)
        {
            Settings = new MoveControlSettings(null);
            Army.RetrieveArmy(Transporting, ref ArmyHS, ref Selection);
            Settings.SetupArmy(ArmyHS, Selection);
            Settings.FMovePointsMax = ArmyHS.FArmy.Moves(Selection);
            Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
            AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
            Fields = Settings.FProcessedMapFields;
            Found = false;
            I = 0;
            Field = null;
            while (!Found && I != Fields.Count)
            {
                Field = (AoWMapField)Fields.List[I];
                if (Army.FLocation.X != Field.Xhx || Army.FLocation.Y != Field.Yhx || Army.FLocation.L != Field.Lhx)
                {
                    Found = CanMoveTo(Army, Transporting, Field.Xhx, Field.Yhx, Field.Lhx) == Transporting;
                }

                if (!Found)
                {
                    I += 1;
                }
            }

            if (Found)
            {
                if (CreateMove(Army, Transporting, Field.Xhx, Field.Yhx, Field.Lhx, Set<CreateMoveModeElement>.Of()) != null)
                {
                    FExecuter.ExecuteMoves();
                    Army.SetStatus(Transporting, AIMoveExecuterUnitStatus.Done);
                    Army.SetTransportStatus(Transporting, AIMoveExecuterTransportStatus.Dumped);
                }
                else
                {
                    FExecuter.FStatus = AIMoveExecuter.Status.OutOfMovePoints;
                }
            }
            else
            {
                Army.SetTransportStatus(Transporting, AIMoveExecuterTransportStatus.Guarding);
            }

            Settings.Release();
        }

        if (Transporting == 0 && Others != 0)
        {
            Settings = new MoveControlSettings(null);
            Army.RetrieveArmy(Others, ref ArmyHS, ref Selection);
            Settings.SetupArmy(ArmyHS, Selection);
            Settings.FMovePointsMax = ArmyHS.FArmy.Moves(Selection);
            Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
            AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
            Field = null;
            OtherFields = Settings.FProcessedMapFields;
            Found = false;
            I = 0;
            while (!Found && I != OtherFields.Count)
            {
                Field = (AoWMapField)OtherFields.List[I];
                if (Army.FLocation.X != Field.Xhx || Army.FLocation.Y != Field.Yhx || Army.FLocation.L != Field.Lhx)
                {
                    Found = CanMoveTo(Army, Others, Field.Xhx, Field.Yhx, Field.Lhx) == Others;
                }

                if (!Found)
                {
                    I += 1;
                }
            }

            if (Found)
            {
                if (CreateMove(Army, Others, Field.Xhx, Field.Yhx, Field.Lhx, Set<CreateMoveModeElement>.Of()) != null)
                {
                    FExecuter.ExecuteMoves();
                    Army.SetStatus(Others, AIMoveExecuterUnitStatus.Done);
                    Army.SetTransportStatus(Others, AIMoveExecuterTransportStatus.Dumped);
                }
                else
                {
                    FExecuter.FStatus = AIMoveExecuter.Status.OutOfMovePoints;
                }
            }
            else
            {
                Army.SetTransportStatus(Others, AIMoveExecuterTransportStatus.Guarding);
            }

            Settings.Release();
        }
    }

    // AIGroup.pas:5638
    public void UpdateTransportMeetingPoint()
    {
        AIMoveExecuterArmyList Armies;
        MoveControlSettings Settings;
        byte Selection = default;
        int I;
        bool Found;
        ArmyHS ArmyHS = default;
        // { Addressability survives elimination of this assignment. }
        Found = false;
        if ((FExecuter.FMode & Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent, AIMoveExecuterModeElement.Attack)) != Set<AIMoveExecuterModeElement>.Of())
        {
            Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
            I = Armies.Count;
            while (!Found && I != 0)
            {
                I -= 1;
                var armies = Armies.GetArmies(I);
                Found = Set<AIMoveExecuterTransportStatus>.Of(AIMoveExecuterTransportStatus.Boarding, AIMoveExecuterTransportStatus.Transporting).Contains(armies.FTransportStatus);
                if (Found)
                {
                    FMeetingPoint = Armies.GetArmies(I).FArrivalPoint;
                }
            }

            if (!Found)
            {
                I = Armies.Count;
            }

            while (!Found && I != 0)
            {
                I -= 1;
                var armies = Armies.GetArmies(I);
                Found = Set<AIMoveExecuterTransportStatus>.Of(AIMoveExecuterTransportStatus.Arriving, AIMoveExecuterTransportStatus.Done).Contains(armies.FTransportStatus);
                if (Found)
                {
                    FMeetingPoint = Armies.GetArmies(I).FLocation;
                }
            }

            if (Found)
            {
                Settings = new MoveControlSettings(null);
                Armies.GetArmies(I).RetrieveArmy(255, ref ArmyHS, ref Selection);
                Settings.SetupArmy(ArmyHS, Selection);
                Settings.SetSourcePos(FMeetingPoint.X, FMeetingPoint.Y, FMeetingPoint.L);
                Settings.SetDestinationPos(FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L);
                AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
                if (Settings.FPath.Count > 4)
                {
                    FMeetingPoint = Settings.FPath[Settings.FPath.Count - 2];
                }

                I = 0;
                int limit = Armies.Count - 1;
                for (; I <= limit; I += 1)
                {
                    var armies = Armies.GetArmies(I);
                    if (Set<AIMoveExecuterTransportStatus>.Of(AIMoveExecuterTransportStatus.Boarding, AIMoveExecuterTransportStatus.Transporting, AIMoveExecuterTransportStatus.Arriving, AIMoveExecuterTransportStatus.Done).Contains(armies.FTransportStatus))
                    {
                        for (int J = 0, last = Armies.GetArmies(I).Count - 1; J <= last; J++)
                        {
                            Armies.GetArmies(I).GetUnits(J).FArrivalPoint = FMeetingPoint;
                        }
                    }
                }

                Settings.Release();
            }

            Armies?.Dispose();
        }

        if (!Found)
        {
            FMeetingPoint.X = -1;
            FMeetingPoint.Y = -1;
            FMeetingPoint.L = -1;
        }
    }

    // AIGroup.pas:5691
    public void CalculateTransportPathCallBack(object Sender)
    {
        AIMoveExecuterArmyList Armies;
        int Index;
        AIMoveExecuterArmy Army;
        AIMoveExecuterUnit UnitEntry;
        Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
        Index = Armies.IndexOf(FMoveControl.FSettings.FArmyHS, (byte)FMoveControl.FSettings.FSelection);
        if (Index != -1)
        {
            Army = Armies.GetArmies(Index);
        }
        else
        {
            Army = null;
        }

        if (Army != null && FMoveControl.FSettings.FMovePoints != -1)
        {
            if (FMoveControl.FSettings.FBoardingPoint.X != -1)
            {
                var aiMoveControlSettings = FMoveControl.FSettings;
                Index = aiMoveControlSettings.FTransportPath.IndexOfXYL(aiMoveControlSettings.FBoardingPoint.X, aiMoveControlSettings.FBoardingPoint.Y, aiMoveControlSettings.FBoardingPoint.L);
                while (true)
                {
                    bool condition = Index != aiMoveControlSettings.FTransportPath.Count - 1;
                    if (condition)
                    {
                        var item = aiMoveControlSettings.FTransportPath[Index];
                        var fTransportPathItem = aiMoveControlSettings.FTransportPath[Index];
                        condition = CanMoveTo(Army, 255, aiMoveControlSettings.FTransportPath[Index].X, item.Y, fTransportPathItem.L) != 255;
                    }

                    if (!condition)
                        break;
                    Index += 1;
                }

                aiMoveControlSettings.FBoardingPoint = aiMoveControlSettings.FTransportPath[Index];
                Index = aiMoveControlSettings.FTransportPath.IndexOfXYL(aiMoveControlSettings.FArrivalPoint.X, aiMoveControlSettings.FArrivalPoint.Y, aiMoveControlSettings.FArrivalPoint.L);
                while (true)
                {
                    bool condition = Index != 0;
                    if (condition)
                    {
                        var item = aiMoveControlSettings.FTransportPath[Index];
                        var fTransportPathItem = aiMoveControlSettings.FTransportPath[Index];
                        condition = CanMoveTo(Army, 255, aiMoveControlSettings.FTransportPath[Index].X, item.Y, fTransportPathItem.L) != 255;
                    }

                    if (!condition)
                        break;
                    Index -= 1;
                }

                aiMoveControlSettings.FArrivalPoint = aiMoveControlSettings.FTransportPath[Index];
                for (int I = 0, last = Army.Count - 1; I <= last; I++)
                {
                    UnitEntry = Army.GetUnits(I);
                    var units = Army.GetUnits(I);
                    if (Unsafe.As<XYL, int>(ref units.FLocation) == Unsafe.As<XYL, int>(ref FExecuter.FTransporter.FLocation))
                    {
                        UnitEntry.FTransportStatus = AIMoveExecuterTransportStatus.Transporting;
                    }
                    else
                    {
                        UnitEntry.FTransportStatus = AIMoveExecuterTransportStatus.Boarding;
                    }

                    UnitEntry.FBoardingPoint = FMoveControl.FSettings.FBoardingPoint;
                    UnitEntry.FBoardingPoint.Flag = 0;
                    UnitEntry.FArrivalPoint = FMoveControl.FSettings.FArrivalPoint;
                    UnitEntry.FArrivalPoint.Flag = 0;
                    UnitEntry.FTransportSource = FMoveControl.FSettings.FTransportSource;
                    UnitEntry.FTransportSource.Flag = 0;
                    UnitEntry.FTransportDestination = FMoveControl.FSettings.FTransportDestination;
                    UnitEntry.FTransportDestination.Flag = 0;
                }
            }
            else
            {
                for (int I = 0, last = Army.Count - 1; I <= last; I++)
                {
                    Army.GetUnits(I).FTransportStatus = AIMoveExecuterTransportStatus.Arriving;
                }
            }
        }
        else
        {
            FExecuter.FStatus = AIMoveExecuter.Status.Failed;
        }

        Armies?.Dispose();
    }
}
