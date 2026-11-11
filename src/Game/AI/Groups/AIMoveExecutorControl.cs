// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// move control, executer, state
public partial class AIMoveExecutorControl : EngineObject
{
    // AIGroup.pas:4726
    public AIMoveExecutorControl(ECustomNode owner) : base(owner)
    {
        FMoveControl = new AIMoveControl(null);
    }

    public AIMoveControl FMoveControl;
    public AIMoveExecutor FExecuter;
    public int FState;

    // AIGroup.pas:4732
    public override void Dispose()
    {
        FMoveControl?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:4738
    public virtual AIMoveExecutorMove CreateMove(AIMoveExecutorArmy Army, byte Selection, sbyte X, sbyte Y, sbyte L, Set<CreateMoveModeElement> Mode, ref byte MoveResult)
    {
        MoveControlSettings Settings;
        AIMoveExecutorMove Move;
        DoubleItemList Candidates;
        byte ArmySelection = default;
        int Position;
        int Points;
        AoWMapField MapF;
        ArmyHexSprite ArmyHS = default;
        MapLevel Level;
        HexNumberWalkData Walk = default;
        Move = null;
        Army.RetrieveArmy(Selection, ref ArmyHS, ref ArmySelection);
        Settings = new MoveControlSettings(null);
        if (Mode.Contains(CreateMoveModeElement.SurroundEx))
        {
            Settings.FMovePointsMax = Army.MovePoints(255);
            Settings.SetupArmy(ArmyHS, ArmySelection);
            AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
            Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            Candidates = new DoubleItemList();
            HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 1);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Points = Settings.GetMovePoints(Walk.X, Walk.Y, L);
                    MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(Walk.X, Walk.Y, L);
                    if (Points != 16777215 && (MapF.GetPlayer() == -1 || MapF.GetPlayer() == Army.FPlayer))
                    {
                        if (CanMoveTo(Army, Selection, (sbyte)Walk.X, (sbyte)Walk.Y, L) == Selection)
                        {
                            Candidates.AddAB(Unsafe.BitCast<MapCoordinate, int>(HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, L)), Points);
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            if (Candidates.Count != 0)
            {
                Candidates.SortAscendingItemB();
                var item = Candidates[0];
                X = Unsafe.As<int, MapCoordinate>(ref item.A).X;
                var candidatesItem = Candidates[0];
                Y = Unsafe.As<int, MapCoordinate>(ref candidatesItem.A).Y;
                var item2 = Candidates[0];
                L = Unsafe.As<int, MapCoordinate>(ref item2.A).L;
                AoWHSMap.Current.FMoveControl.CalculatePath(Settings, Settings.FPath, X, Y, L);
            }

            Candidates?.Dispose();
        }
        else
        {
            Settings.FMovePointsMax = FExecuter.FMovePointsMax;
            Settings.SetupArmy(ArmyHS, ArmySelection);
            Settings.SetDestinationPos(X, Y, L);
            AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
        }

        if (Settings.FPath.Count != 0)
        {
            Points = ArmyHS.FArmy.MovePoints(ArmySelection);
            Position = Settings.FPath.MovePointsToPos(Points);
            if (!Mode.Contains(CreateMoveModeElement.Surround) && Settings.FPath[Position].X == FExecuter.FDestination.X && Settings.FPath[Position].Y == FExecuter.FDestination.Y && Settings.FPath[Position].L == FExecuter.FDestination.L)
            {
                Army.SetMoveResult(Selection, 2);
            }

            if (Position == 0)
            {
                MoveResult = 2;
            }
            else
            {
                MoveResult = 0;
            }

            if (Mode.Contains(CreateMoveModeElement.Surround) && Position == 0)
            {
                Position += 1;
            }

            if (Position < Settings.FPath.Count - 1)
            {
                while (true)
                {
                    bool condition = Position != Settings.FPath.Count - 1;
                    if (condition)
                    {
                        var item = Settings.FPath[Position];
                        var fPathItem = Settings.FPath[Position];
                        condition = CanMoveTo(Army, Selection, Settings.FPath[Position].X, item.Y, fPathItem.L) != Selection;
                    }

                    if (!condition)
                        break;
                    Position += 1;
                    if (MoveResult == 2)
                    {
                        MoveResult = 1;
                    }
                }

                if (Position < Settings.FPath.Count - 1)
                {
                    Move = new AIMoveExecutorMove(null);
                    Move.AddUnits(Army, Selection);
                    for (int I = Position, last = Settings.FPath.Count - 1; I <= last; I++)
                    {
                        var item = Settings.FPath[I];
                        Move.FPath.Add(item);
                    }

                    Move.FMovePoints = Points;
                    Move.FMoves = ArmyHS.FArmy.Moves(ArmySelection);
                    Move.FSourceX = ArmyHS.GetXhx();
                    Move.FSourceY = ArmyHS.GetYhx();
                    Move.FSourceL = ArmyHS.GetLhx();
                    Move.FDestinationX = X;
                    Move.FDestinationY = Y;
                    Move.FDestinationL = L;
                    if (!Mode.Contains(CreateMoveModeElement.DontQueue))
                    {
                        Move.SetOwner(FExecuter.FMoves);
                        Move.SetUnitStatus(AIMoveExecutorUnitStatus.Queued);
                    }
                }
            }
        }
        else
        {
            if (Army.FLocation.X == X && Army.FLocation.Y == Y && Army.FLocation.L == L)
            {
                MoveResult = 2;
            }
            else
            {
                MoveResult = 3;
            }

            if (FExecuter.FDestination.X == Settings.FSourceX && FExecuter.FDestination.Y == Settings.FSourceY && FExecuter.FDestination.L == Settings.FSourceL)
            {
                Army.SetMoveResult(Selection, 2);
                Army.SetStatus(Selection, AIMoveExecutorUnitStatus.Done);
            }
        }

        Settings.Release();
        return Move;
    }

    // AIGroup.pas:4854
    public virtual byte CanMoveTo(AIMoveExecutorArmy Army, byte Selection, sbyte X, sbyte Y, sbyte L)
    {
        int TransporterCount;
        int UnitCount;
        int Capacity;
        int I;
        int Index;
        int Excess;
        Troop AUnit;
        bool Allowed;
        ArmyHexSprite ArmyHS;
        AoWMapField MapF;
        AIMoveExecutorMove Move;
        Army ExistingArmy;
        AIMoveExecutorUnit Transporter;
        UnitCount = 0;
        TransporterCount = 0;
        Capacity = 8;
        Allowed = true;
        MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        ArmyHS = MapF.GetArmyHS();
        byte Result = 0;
        if (ArmyHS != null && ArmyHS.FArmy.FPlayer == Army.FPlayer)
        {
            ExistingArmy = ArmyHS.FArmy;
            I = 0;
            int last = ExistingArmy.Count - 1;
            for (; I <= last; I += 1)
            {
                AUnit = ArmyHS.FArmy.GetUnit(I);
                Index = FExecuter.FUnits.IndexOfUnitID(AUnit.FUnitID);
                if (Index == -1 || FExecuter.FUnits.GetUnits(Index).FStatus != AIMoveExecutorUnitStatus.Queued)
                {
                    UnitCount += 1;
                    if (AUnit.GetTransporter())
                    {
                        TransporterCount += 1;
                        if (AUnit.GetTransportCapacity() + 1 < Capacity)
                        {
                            Capacity = AUnit.GetTransportCapacity() + 1;
                        }
                    }
                }
            }
        }
        else
        {
            if ((FExecuter.FDestination.X != X || FExecuter.FDestination.Y != Y || FExecuter.FDestination.L != L) && MapF.GetPlayer() != -1 && MapF.GetPlayer() != Army.FPlayer)
            {
                Allowed = false;
            }
        }

        if (Allowed)
        {
            I = 0;
            int last = FExecuter.FMoves.Count - 1;
            for (; I <= last; I += 1)
            {
                Move = FExecuter.FMoves.GetMoves(I);
                if (Move.FPath[0].X == X && Move.FPath[0].Y == Y && Move.FPath[0].L == L)
                {
                    UnitCount += Move.Count;
                    Transporter = Move.Transporter(255);
                    if (Transporter != null)
                    {
                        TransporterCount += 1;
                        if (Transporter.FUnit.GetTransportCapacity() + 1 < Capacity)
                        {
                            Capacity = Transporter.FUnit.GetTransportCapacity() + 1;
                        }
                    }
                }
            }

            Transporter = Army.Transporter(Selection);
            if (Transporter == null || TransporterCount == 0)
            {
                if (Transporter != null)
                {
                    if (Transporter.FUnit.GetTransportCapacity() + 1 < Capacity)
                    {
                        Capacity = Transporter.FUnit.GetTransportCapacity() + 1;
                    }
                }

                Excess = Army.SelectionCount(Selection) + UnitCount - Capacity;
                if (Excess > 0)
                {
                    if (Transporter == null)
                    {
                        I = 8;
                        Selection = Army.ValidateSelection(Selection);
                        Result = Selection;
                        while (Excess != 0 && Result != 0)
                        {
                            I -= 1;
                            if ((Selection & 128) != 0)
                            {
                                Result = (byte)(Result & ~(1 << I));
                                Excess -= 1;
                            }

                            Selection = (byte)((Selection & 127) << 1);
                        }
                    }
                    else
                    {
                        Result = 0;
                    }
                }
                else
                {
                    Result = Selection;
                }
            }
            else
            {
                Result = 0;
            }
        }

        if (!Allowed)
        {
            Result = 0;
        }

        return Result;
    }

    // AIGroup.pas:4952
    public virtual void CalculateMoves()
    {
        Done();
    }

    // AIGroup.pas:4942
    public virtual void Activate(AIMoveExecutor Executer)
    {
        FExecuter = Executer;
        FState = 0;
    }

    // AIGroup.pas:4948
    public virtual void Deactivate()
    {
    }

    // AIGroup.pas:4957
    public virtual void Done()
    {
        FState = -1;
    }

    // AIGroup.pas:4962
    public virtual bool Process()
    {
        int TimeLeft;
        TimeLeft = 5;
        if (FMoveControl.FStatus == AIMoveControlStatus.Processing)
        {
            FMoveControl.Process();
        }

        while (FMoveControl.FStatus != AIMoveControlStatus.Processing && TimeLeft > 0 && FExecuter.FStatus == AIMoveExecutor.Status.Busy && FExecuter.FState == 0 && FState != -1)
        {
            CalculateMoves();
            TimeLeft = AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft();
        }

        return FState != -1;
    }

}
