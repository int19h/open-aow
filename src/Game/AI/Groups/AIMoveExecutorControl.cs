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
        MoveControl = new AIMoveControl(null);
    }

    public AIMoveControl MoveControl { get; }
    public AIMoveExecutor Executor { get; private set; }
    public int State { get; protected set; }

    // AIGroup.pas:4732
    public override void Dispose()
    {
        MoveControl?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:4738
    public virtual AIMoveExecutorMove CreateMove(AIMoveExecutorArmy army, byte selection, sbyte X, sbyte Y, sbyte L, Set<CreateMoveModeElement> mode, ref byte moveResult)
    {
        MoveControlSettings settings;
        AIMoveExecutorMove move;
        DoubleItemList candidates;
        byte armySelection = default;
        int position;
        int points;
        AoWMapField mapField;
        ArmyHexSprite armyHexSprite = default;
        MapLevel level;
        HexNumberWalkData walk = default;
        move = null;
        army.RetrieveArmy(selection, ref armyHexSprite, ref armySelection);
        settings = new MoveControlSettings(null);
        if (mode.Contains(CreateMoveModeElement.SurroundNearest))
        {
            settings.MovePointsMax = army.MovePoints(255);
            settings.SetupArmy(armyHexSprite, armySelection);
            AoWHSMap.Current.MoveControl.ProcessMove(settings);
            level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            candidates = new DoubleItemList();
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 1);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    points = settings.GetMovePoints(walk.X, walk.Y, L);
                    mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(walk.X, walk.Y, L);
                    if (points != 16777215 && (mapField.GetPlayer() == -1 || mapField.GetPlayer() == army.Player))
                    {
                        if (CanMoveTo(army, selection, (sbyte)walk.X, (sbyte)walk.Y, L) == selection)
                        {
                            candidates.AddAbility(Unsafe.BitCast<MapCoordinate, int>(HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, L)), points);
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            if (candidates.Count != 0)
            {
                candidates.SortAscendingItemB();
                var item = candidates[0];
                X = Unsafe.As<int, MapCoordinate>(ref item.First).X;
                var candidatesItem = candidates[0];
                Y = Unsafe.As<int, MapCoordinate>(ref candidatesItem.First).Y;
                var item2 = candidates[0];
                L = Unsafe.As<int, MapCoordinate>(ref item2.First).MapLevel;
                AoWHSMap.Current.MoveControl.CalculatePath(settings, settings.Path, X, Y, L);
            }

            candidates?.Dispose();
        }
        else
        {
            settings.MovePointsMax = Executor.MovePointsMax;
            settings.SetupArmy(armyHexSprite, armySelection);
            settings.SetDestinationPosition(X, Y, L);
            AoWHSMap.Current.MoveControl.ProcessMove(settings);
        }

        if (settings.Path.Count != 0)
        {
            points = armyHexSprite.Army.MovePoints(armySelection);
            position = settings.Path.MovePointsToPosition(points);
            if (!mode.Contains(CreateMoveModeElement.Surround) && settings.Path[position].X == Executor.Destination.X && settings.Path[position].Y == Executor.Destination.Y && settings.Path[position].MapLevel == Executor.Destination.MapLevel)
            {
                army.SetMoveResult(selection, 2);
            }

            if (position == 0)
            {
                moveResult = 2;
            }
            else
            {
                moveResult = 0;
            }

            if (mode.Contains(CreateMoveModeElement.Surround) && position == 0)
            {
                position += 1;
            }

            if (position < settings.Path.Count - 1)
            {
                while (true)
                {
                    bool condition = position != settings.Path.Count - 1;
                    if (condition)
                    {
                        var item = settings.Path[position];
                        var fPathItem = settings.Path[position];
                        condition = CanMoveTo(army, selection, settings.Path[position].X, item.Y, fPathItem.MapLevel) != selection;
                    }

                    if (!condition)
                        break;
                    position += 1;
                    if (moveResult == 2)
                    {
                        moveResult = 1;
                    }
                }

                if (position < settings.Path.Count - 1)
                {
                    move = new AIMoveExecutorMove(null);
                    move.AddUnits(army, selection);
                    for (int I = position, last = settings.Path.Count - 1; I <= last; I++)
                    {
                        var item = settings.Path[I];
                        move.Path.Add(item);
                    }

                    move.MovePoints = points;
                    move.Moves = armyHexSprite.Army.Moves(armySelection);
                    move.SourceX = armyHexSprite.GetHexX();
                    move.SourceY = armyHexSprite.GetHexY();
                    move.SourceL = armyHexSprite.GetHexLevel();
                    move.DestinationX = X;
                    move.DestinationY = Y;
                    move.DestinationL = L;
                    if (!mode.Contains(CreateMoveModeElement.DoNotQueue))
                    {
                        move.SetOwner(Executor.Moves);
                        move.SetUnitStatus(AIMoveExecutorUnitStatus.Queued);
                    }
                }
            }
        }
        else
        {
            if (army.Location.X == X && army.Location.Y == Y && army.Location.MapLevel == L)
            {
                moveResult = 2;
            }
            else
            {
                moveResult = 3;
            }

            if (Executor.Destination.X == settings.SourceX && Executor.Destination.Y == settings.SourceY && Executor.Destination.MapLevel == settings.SourceL)
            {
                army.SetMoveResult(selection, 2);
                army.SetStatus(selection, AIMoveExecutorUnitStatus.Done);
            }
        }

        settings.Release();
        return move;
    }

    // AIGroup.pas:4854
    public virtual byte CanMoveTo(AIMoveExecutorArmy army, byte selection, sbyte X, sbyte Y, sbyte L)
    {
        int transporterCount;
        int unitCount;
        int capacity;
        int I;
        int index;
        int excess;
        Troop troop;
        bool allowed;
        ArmyHexSprite armyHexSprite;
        AoWMapField mapField;
        AIMoveExecutorMove move;
        Army existingArmy;
        AIMoveExecutorUnit transporter;
        unitCount = 0;
        transporterCount = 0;
        capacity = 8;
        allowed = true;
        mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        armyHexSprite = mapField.GetArmyHexSprite();
        byte result = 0;
        if (armyHexSprite != null && armyHexSprite.Army.Player == army.Player)
        {
            existingArmy = armyHexSprite.Army;
            I = 0;
            int last = existingArmy.Count - 1;
            for (; I <= last; I += 1)
            {
                troop = armyHexSprite.Army.GetUnit(I);
                index = Executor.Units.IndexOfUnitId(troop.UnitId);
                if (index == -1 || Executor.Units.GetUnits(index).Status != AIMoveExecutorUnitStatus.Queued)
                {
                    unitCount += 1;
                    if (troop.GetTransporter())
                    {
                        transporterCount += 1;
                        if (troop.GetTransportCapacity() + 1 < capacity)
                        {
                            capacity = troop.GetTransportCapacity() + 1;
                        }
                    }
                }
            }
        }
        else
        {
            if ((Executor.Destination.X != X || Executor.Destination.Y != Y || Executor.Destination.MapLevel != L) && mapField.GetPlayer() != -1 && mapField.GetPlayer() != army.Player)
            {
                allowed = false;
            }
        }

        if (allowed)
        {
            I = 0;
            int last = Executor.Moves.Count - 1;
            for (; I <= last; I += 1)
            {
                move = Executor.Moves.GetMoves(I);
                if (move.Path[0].X == X && move.Path[0].Y == Y && move.Path[0].MapLevel == L)
                {
                    unitCount += move.Count;
                    transporter = move.Transporter(255);
                    if (transporter != null)
                    {
                        transporterCount += 1;
                        if (transporter.Troop.GetTransportCapacity() + 1 < capacity)
                        {
                            capacity = transporter.Troop.GetTransportCapacity() + 1;
                        }
                    }
                }
            }

            transporter = army.Transporter(selection);
            if (transporter == null || transporterCount == 0)
            {
                if (transporter != null)
                {
                    if (transporter.Troop.GetTransportCapacity() + 1 < capacity)
                    {
                        capacity = transporter.Troop.GetTransportCapacity() + 1;
                    }
                }

                excess = army.SelectionCount(selection) + unitCount - capacity;
                if (excess > 0)
                {
                    if (transporter == null)
                    {
                        I = 8;
                        selection = army.ValidateSelection(selection);
                        result = selection;
                        while (excess != 0 && result != 0)
                        {
                            I -= 1;
                            if ((selection & 128) != 0)
                            {
                                result = (byte)(result & ~(1 << I));
                                excess -= 1;
                            }

                            selection = (byte)((selection & 127) << 1);
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = selection;
                }
            }
            else
            {
                result = 0;
            }
        }

        if (!allowed)
        {
            result = 0;
        }

        return result;
    }

    // AIGroup.pas:4952
    public virtual void CalculateMoves()
    {
        Done();
    }

    // AIGroup.pas:4942
    public virtual void Activate(AIMoveExecutor executor)
    {
        Executor = executor;
        State = 0;
    }

    // AIGroup.pas:4948
    public virtual void Deactivate()
    {
    }

    // AIGroup.pas:4957
    public virtual void Done()
    {
        State = -1;
    }

    // AIGroup.pas:4962
    public virtual bool Process()
    {
        int timeLeft;
        timeLeft = 5;
        if (MoveControl.Status == AIMoveControlStatus.Processing)
        {
            MoveControl.Process();
        }

        while (MoveControl.Status != AIMoveControlStatus.Processing && timeLeft > 0 && Executor.Status == AIMoveExecutorStatus.Busy && Executor.State == 0 && State != -1)
        {
            CalculateMoves();
            timeLeft = AoWHSMap.Current.AIExecutor.GetProcessTimeLeft();
        }

        return State != -1;
    }

}
