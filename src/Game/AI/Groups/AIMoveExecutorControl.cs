#nullable disable
// Translated from AoWE.pas.
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
    public AIMoveExecutorControl(CustomNode owner) : base(owner) => MoveControl = new AIMoveControl(null) { Holder = CreationHolder() };

    public AIMoveControl MoveControl { get; }

    // A free hexagon beside the destination and the move points that reach it. AIGroup.pas:4738
    // kept the pair in a TDoubleItemList, the location packed into column A. The list sorts with
    // the same quicksort on the points, so the order of equal points is the original's.
    private readonly record struct SurroundCandidate(MapCoordinate Location, int MovePoints);

    public AIMoveExecutor Executor { get; private set; }
    public int State { get; protected set; }

    // AIGroup.pas:4732
    public override void Dispose()
    {
        MoveControl?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:4738
    public virtual AIMoveExecutorMove CreateMove(AIMoveExecutorArmy army, byte selection, sbyte x, sbyte y, sbyte mapLevel, Set<CreateMoveModeElement> mode, out byte moveResult)
    {
        AIMoveExecutorMove move = null;
        army.RetrieveArmy(selection, army: out ArmyHexSprite armyHexSprite, out byte armySelection);
        MoveControlSettings settings = new(null) { Holder = CreationHolder() };
        int points;
        if (mode.Contains(CreateMoveModeElement.SurroundNearest))
        {
            settings.MovePointsMax = army.MovePoints(255);
            settings.SetupArmy(armyHexSprite, selection: armySelection);
            StrategicMap.Current.MoveControl.ProcessMove(settings);
            MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(mapLevel);
            RecordList<SurroundCandidate> candidates = new();
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 1);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    points = settings.GetMovePoints(walk.X, walk.Y, mapLevel);
                    MapField mapField = StrategicMap.Current.MapContainer.GetField(walk.X, walk.Y, mapLevel);
                    if (points != 16777215 && (mapField.GetPlayer() == -1 || mapField.GetPlayer() == army.Player))
                    {
                        if (CanMoveTo(army, selection, (sbyte)walk.X, (sbyte)walk.Y, mapLevel) == selection)
                        {
                            _ = candidates.Add(new SurroundCandidate(Location: HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, mapLevel), MovePoints: points));
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            if (candidates.Count != 0)
            {
                candidates.Sort(static (in first, in second) => first.MovePoints - second.MovePoints);
                MapCoordinate location = candidates[0].Location;
                x = location.X;
                y = location.Y;
                mapLevel = location.MapLevel;
                StrategicMap.Current.MoveControl.CalculatePath(settings, settings.Path, x, y, L: mapLevel);
            }

            candidates?.Dispose();
        }
        else
        {
            settings.MovePointsMax = Executor.MovePointsMax;
            settings.SetupArmy(armyHexSprite, selection: armySelection);
            settings.SetDestinationPosition(x, y, mapLevel);
            StrategicMap.Current.MoveControl.ProcessMove(settings);
        }

        if (settings.Path.Count != 0)
        {
            points = armyHexSprite.Army.MovePoints(armySelection);
            int position = settings.Path.MovePointsToPosition(points);
            if (!mode.Contains(CreateMoveModeElement.Surround) && settings.Path[position].Location.X == Executor.Destination.X && settings.Path[position].Location.Y == Executor.Destination.Y && settings.Path[position].Location.MapLevel == Executor.Destination.MapLevel)
            {
                army.SetMoveResult(selection, moveResult: 2);
            }

            moveResult = position == 0 ? (byte)2 : (byte)0;

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
                        PathStep item = settings.Path[position];
                        PathStep fPathItem = settings.Path[position];
                        condition = CanMoveTo(army, selection, settings.Path[position].Location.X, item.Location.Y, fPathItem.Location.MapLevel) != selection;
                    }

                    if (!condition)
                    {
                        break;
                    }

                    position += 1;
                    if (moveResult == 2)
                    {
                        moveResult = 1;
                    }
                }

                if (position < settings.Path.Count - 1)
                {
                    move = new AIMoveExecutorMove(null) { Holder = CreationHolder() };
                    move.AddUnits(army, selection);
                    for (int i = position, last = settings.Path.Count - 1; i <= last; i++)
                    {
                        PathStep item = settings.Path[i];
                        _ = move.Path.Add(item);
                    }

                    move.MovePoints = points;
                    move.Moves = armyHexSprite.Army.Moves(armySelection);
                    move.Source = new MapCoordinate(armyHexSprite.HexX, armyHexSprite.HexY, armyHexSprite.HexLevel);
                    move.Destination = new MapCoordinate(x, y, mapLevel);
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
            moveResult = army.Location.X == x && army.Location.Y == y && army.Location.MapLevel == mapLevel ? (byte)2 : (byte)3;

            if (Executor.Destination.X == settings.Source.X && Executor.Destination.Y == settings.Source.Y && Executor.Destination.MapLevel == settings.Source.MapLevel)
            {
                army.SetMoveResult(selection, moveResult: 2);
                army.SetStatus(selection, status: AIMoveExecutorUnitStatus.Done);
            }
        }

        _ = settings.Release();
        return move;
    }

    // AIGroup.pas:4854
    public virtual byte CanMoveTo(AIMoveExecutorArmy army, byte selection, sbyte x, sbyte y, sbyte mapLevel)
    {
        int unitCount = 0;
        int transporterCount = 0;
        int capacity = 8;
        bool allowed = true;
        MapField mapField = StrategicMap.Current.MapContainer.GetField(x, y, mapLevel);
        ArmyHexSprite armyHexSprite = mapField.GetArmyHexSprite();
        byte result = 0;
        int i;
        if (armyHexSprite is not null && armyHexSprite.Army.Player == army.Player)
        {
            Army existingArmy = armyHexSprite.Army;
            i = 0;
            int last = existingArmy.Units.Count - 1;
            for (; i <= last; i += 1)
            {
                Troop troop = armyHexSprite.Army.Units.GetUnit(i);
                int index = Executor.Units.IndexOfUnitId(troop.UnitId);
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
            if ((Executor.Destination.X != x || Executor.Destination.Y != y || Executor.Destination.MapLevel != mapLevel) && mapField.GetPlayer() != -1 && mapField.GetPlayer() != army.Player)
            {
                allowed = false;
            }
        }

        if (allowed)
        {
            i = 0;
            int last = Executor.Moves.Count - 1;
            AIMoveExecutorUnit transporter;
            for (; i <= last; i += 1)
            {
                AIMoveExecutorMove move = Executor.Moves.GetMoves(i);
                if (move.Path[0].Location.X == x && move.Path[0].Location.Y == y && move.Path[0].Location.MapLevel == mapLevel)
                {
                    unitCount += move.Count;
                    transporter = move.Transporter(255);
                    if (transporter is not null)
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
            if (transporter is null || transporterCount == 0)
            {
                if (transporter is not null)
                {
                    if (transporter.Troop.GetTransportCapacity() + 1 < capacity)
                    {
                        capacity = transporter.Troop.GetTransportCapacity() + 1;
                    }
                }

                int excess = army.SelectionCount(selection) + unitCount - capacity;
                if (excess > 0)
                {
                    if (transporter is null)
                    {
                        i = 8;
                        selection = army.ValidateSelection(selection);
                        result = selection;
                        while (excess != 0 && result != 0)
                        {
                            i -= 1;
                            if ((selection & 0b1000_0000) != 0)
                            {
                                result = (byte)(result & ~(1 << i));
                                excess -= 1;
                            }

                            selection = (byte)((selection & 0b0111_1111) << 1);
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

        return !allowed ? (byte)0 : result;
    }

    // AIGroup.pas:4952
    public virtual void CalculateMoves() => Done();

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
    public virtual void Done() => State = -1;

    // AIGroup.pas:4962
    public virtual bool Process()
    {
        int timeLeft = 5;
        if (MoveControl.Status == AIMoveControlStatus.Processing)
        {
            _ = MoveControl.Process();
        }

        while (MoveControl.Status != AIMoveControlStatus.Processing && timeLeft > 0 && Executor.Status == AIMoveExecutorStatus.Busy && Executor.State == 0 && State != -1)
        {
            CalculateMoves();
            timeLeft = StrategicMap.Current.AIExecutor.ProcessTimeLeft;
        }

        return State != -1;
    }
}
