// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

// meeting point, adjacent armies
public partial class AdjacentMEC : AIMoveExecuterControl
{
    // AIGroup.pas:5117
    public AdjacentMEC(ECustomNode owner) : base(owner)
    {
        FAdjacentArmies = new AdjacentArmyList(null);
    }

    public XYL FMeetingPoint;
    public AdjacentArmyList FAdjacentArmies;

    // AIGroup.pas:5123
    public override void Dispose()
    {
        FAdjacentArmies?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:5239
    public override void CalculateMoves()
    {
        int Index;
        int Ready;
        int Failed;
        int I;
        AIMoveExecuterArmyList Armies;
        byte MoveResult = default;
        MoveControlSettings Settings;
        byte ArmySelection = default;
        ArmyHS ArmyHS = default;
        AdjacentArmy Adjacent;
        AIMoveExecuterMove Move;
        Set<CreateMoveModeElement> Mode;
        bool CanMove;
        switch (FState)
        {
            case 0:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
                I = 0;
                while (true)
                {
                    bool condition = Armies.Count != I;
                    if (condition)
                    {
                        var armies = Armies.GetArmies(I);
                        condition = FAdjacentArmies.IndexOfLoc(armies.FLocation) != -1;
                    }

                    if (!condition)
                        break;
                    I += 1;
                }

                if (I < Armies.Count)
                {
                    Settings = new MoveControlSettings(null);
                    try
                    {
                        Armies.GetArmies(I).RetrieveArmy(255, ref ArmyHS, ref ArmySelection);
                    }
                    catch
                    {
                    }

                    // { Original suppresses a RetrieveArmy exception. }
                    // { Original uses transporter IDs as visited markers during adjacent moves. }
                    for (int J = Armies.GetArmies(I).Count - 1; J >= 0; J--)
                    {
                        Armies.GetArmies(I).GetUnits(J).FTransporterID = 0;
                    }

                    Settings.Reset();
                    Settings.FMovePointsMax = FExecuter.FMovePointsMax;
                    Settings.SetupArmy(ArmyHS, ArmySelection);
                    Settings.SetDestinationPos(FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L);
                    AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
                    Adjacent = new AdjacentArmy(FAdjacentArmies);
                    Adjacent.FPath.Assign(Settings.FPath);
                    Adjacent.FMoves = ArmyHS.FArmy.Moves(ArmySelection);
                    Adjacent.FMovePoints = ArmyHS.FArmy.MovePoints(ArmySelection);
                    Adjacent.FLocation = Armies.GetArmies(I).FLocation;
                    Settings.Release();
                }
                else
                {
                    I = FAdjacentArmies.Count - 1;
                    while (I >= 0 && FAdjacentArmies.GetAdjacentArmies(I).FPath.Count > 0)
                    {
                        I -= 1;
                    }

                    if (I == -1)
                    {
                        FState += 1;
                    }
                    else
                    {
                        FState = 4;
                    }
                }

                Armies?.Dispose();
                break;
            case 1:
                if (FAdjacentArmies.GetAdjacent())
                {
                    var slowestArmy = FAdjacentArmies.GetSlowestArmy();
                    if ((double)FAdjacentArmies.GetAdjacentArmies(slowestArmy).GetMovementDays() <= 1.25 && FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
                    {
                        if (ExecuteSurroundMove())
                        {
                            FState = 5;
                        }
                        else
                        {
                            FMeetingPoint = FExecuter.FDestination;
                            FState = 2;
                        }
                    }
                    else
                    {
                        FMeetingPoint = FExecuter.FDestination;
                        FState = 2;
                    }
                }
                else
                {
                    var slowestArmy = FAdjacentArmies.GetSlowestArmy();
                    if ((double)FAdjacentArmies.GetAdjacentArmies(slowestArmy).GetMovementDays() <= 1 && FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
                    {
                        if (ExecuteSurroundMove())
                        {
                            FState = 5;
                        }
                        else
                        {
                            FState = 3;
                        }
                    }
                    else
                    {
                        ExecuteMeetingPointMove();
                        FState = 3;
                    }
                }

                break;
            case 2:
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
                Armies.SortOnMovePoints();
                if (Armies.Count != 0)
                {
                    if (FExecuter.FMoves.Count == 0)
                    {
                        Index = 0;
                        while (Index < Armies.Count && Armies.GetArmies(Index).GetUnits(0).FTransporterID == 1)
                        {
                            Index += 1;
                        }

                        if (Index < Armies.Count)
                        {
                            for (int J = Armies.GetArmies(Index).Count - 1; J >= 0; J--)
                            {
                                Armies.GetArmies(Index).GetUnits(J).FTransporterID = 1;
                            }

                            if (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
                            {
                                Mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DontQueue, CreateMoveModeElement.Surround);
                            }
                            else
                            {
                                Mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DontQueue);
                            }

                            Move = CreateMove(Armies.GetArmies(Index), 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Mode, ref MoveResult);
                            if (Move == null)
                            {
                                if (MoveResult == 1)
                                {
                                    FState = 3;
                                }
                                else
                                {
                                    if (MoveResult == 3)
                                    {
                                        FExecuter.FStatus = AIMoveExecuter.Status.Failed;
                                        FState = 3;
                                    }
                                    else
                                    {
                                        if (MoveResult != 2)
                                        {
                                            FState = 3;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Move.SetOwner(FExecuter.FMoves);
                                Move.SetUnitStatus(AIMoveExecuterUnitStatus.Queued);
                                if (MoveResult == 0)
                                {
                                    FMeetingPoint = Move.FPath[0];
                                }
                                else
                                {
                                    FMeetingPoint = FExecuter.FDestination;
                                }
                            }
                        }
                        else
                        {
                            FExecuter.FControlQueue.Add(0);
                            FState = 3;
                        }
                    }
                    else
                    {
                        if (Unsafe.As<XYL, int>(ref FMeetingPoint) == Unsafe.As<XYL, int>(ref FExecuter.FDestination) && FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
                        {
                            Mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DontQueue, CreateMoveModeElement.Surround);
                        }
                        else
                        {
                            Mode = Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DontQueue);
                        }

                        Move = CreateMove(Armies.GetArmies(0), 255, FMeetingPoint.X, FMeetingPoint.Y, FMeetingPoint.L, Mode, ref MoveResult);
                        if (Move != null)
                        {
                            if (MoveResult == 0)
                            {
                                if (FExecuter.FMoves.Count != 0)
                                {
                                    FExecuter.FMoves.Reset();
                                }

                                Move?.Dispose();
                            }
                            else
                            {
                                Move.SetOwner(FExecuter.FMoves);
                                Move.SetUnitStatus(AIMoveExecuterUnitStatus.Queued);
                            }
                        }
                        else
                        {
                            if (MoveResult == 1 || MoveResult == 2)
                            {
                                Armies.GetArmies(0).SetStatus(255, AIMoveExecuterUnitStatus.Done);
                            }
                            else
                            {
                                if (MoveResult == 3)
                                {
                                    FExecuter.FStatus = AIMoveExecuter.Status.Failed;
                                    FExecuter.FMoves.Reset();
                                    FState = 3;
                                }
                                else
                                {
                                    FExecuter.FMoves.Reset();
                                    FState = 3;
                                }
                            }
                        }
                    }
                }
                else
                {
                    FExecuter.ExecuteMoves();
                    FState = 3;
                }

                Armies?.Dispose();
                break;
            case 3:
                Done();
                break;
            case 4:
                if (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
                {
                    I = FAdjacentArmies.Count - 1;
                    while (I >= 0 && FAdjacentArmies.GetAdjacentArmies(I).FPath.Count != 2)
                    {
                        I -= 1;
                    }

                    if (I != -1)
                    {
                        FState = 5;
                    }
                    else
                    {
                        FExecuter.FStatus = AIMoveExecuter.Status.Failed;
                        FState = 3;
                    }
                }
                else
                {
                    FExecuter.FStatus = AIMoveExecuter.Status.Failed;
                    FState = 3;
                }

                break;
            case 5:
                I = 0;
                Ready = 0;
                Failed = 0;
                Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
                while (Armies.Count != I)
                {
                    Move = CreateMove(Armies.GetArmies(I), 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.DontQueue, CreateMoveModeElement.Surround), ref MoveResult);
                    bool condition = MoveResult != 0;
                    if (!condition)
                    {
                        var armies = Armies.GetArmies(I);
                        var armiesArmies = Armies.GetArmies(I);
                        condition = HexGeometry.dHXtoRad(FExecuter.FDestination.X, FExecuter.FDestination.Y, armiesArmies.FLocation.X, armies.FLocation.Y) == 1 && Armies.GetArmies(I).FLocation.L == FExecuter.FDestination.L;
                    }

                    CanMove = condition;
                    if (CanMove)
                    {
                        Ready += 1;
                    }
                    else
                    {
                        Failed += 1;
                    }

                    if (Move != null)
                    {
                        Move?.Dispose();
                    }

                    I += 1;
                }

                Armies?.Dispose();
                if (Ready > 4 || Failed == 0)
                {
                    I = 0;
                    Move = null;
                    Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
                    while (Move == null && Armies.Count != I)
                    {
                        Move = CreateMove(Armies.GetArmies(I), 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Set<CreateMoveModeElement>.Of(), ref MoveResult);
                        if (Move != null)
                        {
                            FExecuter.ExecuteMoves();
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
        int Index;
        AIMoveExecuterArmyList Armies;
        DoubleItemList<AdjacentArmy> Sorted;
        AIMoveExecuterArmy Army;
        byte MoveResult = default;
        bool Result = true;
        Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
        Sorted = new DoubleItemList<AdjacentArmy>();
        I = 0;
        int last = FAdjacentArmies.Count - 1;
        for (; I <= last; I += 1)
        {
            var movementDays = FAdjacentArmies.GetAdjacentArmies(I).GetMovementDays();
            Sorted.AddAB(FAdjacentArmies.GetAdjacentArmies(I), Round((double)movementDays * 100));
        }

        Sorted.SortDescendingItemB();
        _ = Sorted[0].A.FPath[1];
        I = 0;
        while (Result && I < Sorted.Count)
        {
            Index = Armies.IndexOfLoc(Sorted[I].A.FLocation);
            if (Index != -1)
            {
                Army = Armies.GetArmies(Index);
                if (Army.FLocation.L == FExecuter.FDestination.L && HexGeometry.dHXtoRad(Army.FLocation.X, Army.FLocation.Y, FExecuter.FDestination.X, FExecuter.FDestination.Y) > 1)
                {
                    CreateMove(Army, 255, FExecuter.FDestination.X, FExecuter.FDestination.Y, FExecuter.FDestination.L, Set<CreateMoveModeElement>.Of(CreateMoveModeElement.SurroundEx), ref MoveResult);
                    Result = MoveResult != 0 && MoveResult != 3;
                }
            }

            I += 1;
        }

        if (FExecuter.FMoves.Count != 0)
        {
            if (Result)
            {
                FExecuter.ExecuteMoves();
            }
            else
            {
                FExecuter.FMoves.Reset();
            }
        }

        Sorted?.Dispose();
        Armies?.Dispose();
        return Result;
    }

    // AIGroup.pas:5170
    public int CalculateMeetingPoint()
    {
        int I;
        int Points;
        int Reserve;
        int Index;
        int Position;
        AIMoveExecuterArmyList Armies;
        AdjacentArmy Adjacent;
        AIMoveExecuterArmy Army;
        Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready, AIMoveExecuterUnitStatus.Done));
        int Result = 16777215;
        I = FAdjacentArmies.GetSlowestArmy();
        if (I != -1)
        {
            if ((double)FAdjacentArmies.GetAdjacentArmies(I).GetMovementDays() > 1 || !FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
            {
                Points = FAdjacentArmies.GetAdjacentArmies(I).FMovePoints;
                if (FExecuter.FMode.Contains(AIMoveExecuterModeElement.Attack))
                {
                    Reserve = Round((double)Points * 0.75);
                    if (Reserve > FAdjacentArmies.GetAdjacentArmies(I).FPath.GetMovePoints() - Points)
                    {
                        Points = FAdjacentArmies.GetAdjacentArmies(I).FPath.GetMovePoints() - Reserve;
                    }
                }

                Adjacent = FAdjacentArmies.GetAdjacentArmies(I);
                Index = Armies.IndexOfLoc(FAdjacentArmies.GetAdjacentArmies(I).FLocation);
                if (Index != -1)
                {
                    Army = Armies.GetArmies(Index);
                    Position = Adjacent.FPath.MovePointsToPos(Points);
                    while (true)
                    {
                        bool condition = Position > 1;
                        if (condition)
                        {
                            var item = Adjacent.FPath[Position];
                            var fPathItem = Adjacent.FPath[Position];
                            condition = CanMoveTo(Army, 255, Adjacent.FPath[Position].X, item.Y, fPathItem.L) != 255;
                        }

                        if (!condition)
                            break;
                        Position -= 1;
                    }

                    Result = Unsafe.BitCast<XYL, int>(FAdjacentArmies.GetAdjacentArmies(I).FPath[Position]);
                }
            }
        }

        Armies?.Dispose();
        return Result;
    }

    // AIGroup.pas:5206
    public bool ExecuteMeetingPointMove()
    {
        int Index;
        AIMoveExecuterArmyList Armies;
        DoubleItemList<AdjacentArmy> Sorted;
        byte MoveResult = default;
        XYL Location = default;
        bool Result = true;
        Unsafe.As<XYL, int>(ref Location) = CalculateMeetingPoint();
        if (Unsafe.As<XYL, int>(ref Location) == 16777215)
        {
            ref var location = ref FExecuter.FDestination;
            Location = HexGeometry.PackXYL(location.X, location.Y, location.L);
        }

        Armies = FExecuter.CreateArmies(Set<AIMoveExecuterUnitStatus>.Of(AIMoveExecuterUnitStatus.Ready));
        Sorted = new DoubleItemList<AdjacentArmy>();
        for (int I = 0, last = FAdjacentArmies.Count - 1; I <= last; I++)
        {
            var movementDays = FAdjacentArmies.GetAdjacentArmies(I).GetMovementDays();
            Sorted.AddAB(FAdjacentArmies.GetAdjacentArmies(I), Round((double)movementDays * 100));
        }

        Sorted.SortDescendingItemB();
        for (int I = 0, last = Sorted.Count - 1; I <= last; I++)
        {
            Index = Armies.IndexOfLoc(Sorted[I].A.FLocation);
            if (Index != -1)
            {
                CreateMove(Armies.GetArmies(Index), 255, Location.X, Location.Y, Location.L, Set<CreateMoveModeElement>.Of(), ref MoveResult);
                if (MoveResult == 0)
                {
                    Result = false;
                }
            }
        }

        if (FExecuter.FMoves.Count != 0)
        {
            FExecuter.ExecuteMoves();
        }

        Sorted?.Dispose();
        Armies?.Dispose();
        return Result;
    }

    // intf_P5_06.inc:413

}
