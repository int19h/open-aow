#nullable disable warnings
// Translated from AoWE.pas.
using System.Collections.Immutable;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.World.Movement;

// map, status, player, mode, source, destination, map level, processed count, move limit, move
// table, processed fields, army, selection, move data, callback, field offsets, point offsets,
// gap endpoints, direct route, exploration mask, settings
public partial class MoveControl(GameMap owner) : IDisposable
{
    public GameMap Map { get; init; } = owner;

    private Statuses _status;

    public PlayerId? Player { get; private set; } = PlayerId.Independent;
    public MoveModes Mode { get; private set; }

    private MapCoordinate Source { get; set; }

    public MapCoordinate Destination { get; set; }
    public MapLevel? MapLevel { get; set; }

    private int _processedCount;

    public int MovePointsLimit { get; set; }

    public MovePointTable MovePointTable { get; private set; }

    private DelphiList<MapField>? _processedMapFields;

    public ArmyHexSprite? ArmyHexSprite { get; private set; }
    public int Selection { get; private set; }
    public ArmyHexSprite? TransporterArmy { get; private set; }

    private Func<MapField?, int, int> _movePoint;

    internal void SetMovePointHandler(Func<MapField?, int, int> handler) => _movePoint = handler;

    private readonly int[][] _fieldOffsets = [new int[8], new int[8]];
    private readonly int[][] _pointOffsets = [new int[8], new int[8]];

    public MapCoordinateList GapStarts { get; init; } = [];
    public MapCoordinateList GapEnds { get; init; } = [];

    private readonly MapCoordinateList _directRoute = [];
    private int _exploredMask;

    public MoveControlSettings? Settings { get; set; }

    // MoveT.PAS:266
    public virtual void Dispose()
    {
        GapStarts?.Dispose();
        GapEnds?.Dispose();
        _directRoute?.Dispose();
    }

    // MoveT.PAS:474
    public virtual bool SetupMove()
    {
        _processedCount = 0;
        Settings.ClearMovePoints();
        Settings.SubMoves.Clear();
        Settings.SubMoveIndex = 0;
        Settings.Path.Clear();
        if (Mode.Contains(MoveModes.LogProcessedMapFields))
        {
            Settings.ProcessedMapFields.Count = 0;
            Settings.ProcessedMapFields.Capacity = 4_096;
        }

        // { Original bug: the Y bound also uses the map width. }
        if (!(MapLevel is not null && Source.X >= 0 && Source.X < MapLevel.Width && Source.Y >= 0 && Source.Y < MapLevel.Width))
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, message: "Invalid settings (TMoveControl)", file: "D:\\AoWDev\\AoWE\\inc\\MoveT.PAS", line: 32187);
        }

        if ((Settings.Source.X != Settings.Destination.X || Settings.Source.Y != Settings.Destination.Y || Settings.Source.MapLevel != Settings.Destination.MapLevel) && (Destination.X == -1 || ValidLocation(Destination.X, Destination.Y, L: Destination.MapLevel)))
        {
            Settings.SetupLevel(Settings.Source.MapLevel);
            if (Settings.Destination.MapLevel != -1)
            {
                Settings.SetupLevel(Settings.Destination.MapLevel);
            }

            if (Source.MapLevel == Destination.MapLevel && MovePointsLimit > 40 && Destination.X != -1)
            {
                MovePointsLimit = GetMovePointsCostFast(Source.X, Source.Y, Destination.X, Destination.Y);
                if (MovePointsLimit == -1 || MovePointsLimit > Settings.MovePointsMax)
                {
                    MovePointsLimit = Settings.MovePointsMax;
                }

                StrategicMap.Current.AIExecutor.UseProcessTime(MovePointsLimit);
            }

            Settings.SetMovePoints(Source.X, Source.Y, Source.MapLevel, value: 0);
            MapField field = MapLevel.GetField(Source.X, Source.Y);
            if (Mode.Contains(MoveModes.LogProcessedMapFields))
            {
                _ = _processedMapFields.Add(field);
            }

            PathFindingStep pathFindingStep = SetupSubMove(field, origin: null);
            if ((field.FieldFlags & MapField.MoveExclusiveFlag) != 0)
            {
                field.MoveExclusive(this);
            }

            pathFindingStep.Owner = Settings.SubMoves;
            StrategicMap.Current.AIExecutor.UseProcessTime(100);
        }
        else
        {
            Settings.SetupLevel(Settings.Source.MapLevel);
            Settings.SetMovePoints(Source.X, Source.Y, Source.MapLevel, value: 0);
            Settings.Status = MoveControlSettingsStatus.Processed;
        }

        return true;
    }

    // MoveT.PAS:1515
    public virtual void ProcessMove(MoveControlSettings settings)
    {
        if (settings.Source.X != settings.Destination.X || settings.Source.Y != settings.Destination.Y || settings.Source.MapLevel != settings.Destination.MapLevel)
        {
            if (Activate(settings))
            {
                Assert(settings.GetEngine().Demo || settings.Status != MoveControlSettingsStatus.Processed, message: "", file: "MoveT.PAS", line: 1541);
                do
                {
                }
                while (!!Process());
                if (Destination.X != -1)
                {
                    CalculatePath(settings.Path, Destination.X, Destination.Y, Destination.MapLevel);
                    if (settings.Path.Count == 0)
                    {
                        settings.Status = MoveControlSettingsStatus.Failed;
                    }
                }

                Deactivate();
            }
        }
        else
        {
            settings.Path.Clear();
            _ = settings.Path.AddMapCoordinate(settings.Source.X, settings.Source.Y, settings.Source.MapLevel);
            settings.Status = MoveControlSettingsStatus.Processed;
        }
    }

    // MoveT.PAS:274
    public void Clear()
    {
        Deactivate();
        Player = null;
        MapLevel = null;
        Source = new MapCoordinate(-1, -1, -1);
        Destination = new MapCoordinate(-1, -1, -1);
        _processedCount = 0;
        _movePoint = DefaultMoveOnMovePointProc;
        _processedMapFields = null;
    }

    // MoveT.PAS:299
    public int DefaultMoveOnMovePointProc(MapField? mapField, int moveOverCode)
    {
        int overlay = mapField.OverlayType.ToOriginal();
        int points = MovePointTable[mapField.TerrainNumber][overlay - MovementTables.FirstOverlay];
        if ((mapField.FieldFlags & MapField.MoveOnFlag) != 0)
        {
            points = mapField.CanMoveOn(this, defaultCode: points, moveOverCode);
        }

        return points;
    }

    // MoveT.PAS:308
    public MapField? FindSubMoveOrigin(MapField mapField)
    {
        int best = 16_777_215;
        MapField? result = null;
        int i = 0;
        MoveControlSettings? settings = Settings;
        while (i < settings.SubMoves.Count)
        {
            PathFindingStep pathFindingStep = settings.SubMoves.GetSubMove(i);
            if (mapField.HexX == pathFindingStep.Location.X && mapField.HexY == pathFindingStep.Location.Y && mapField.HexLevel == pathFindingStep.Location.MapLevel)
            {
                int points = settings.GetMovePoints(pathFindingStep.Origin.X, pathFindingStep.Origin.Y, pathFindingStep.Origin.MapLevel);
                if (points <= best)
                {
                    // { After pushing L, the original stack slot holds Self, not Result. }
                    result = Map.MapContainer.GetField(pathFindingStep.Origin.X, pathFindingStep.Origin.Y, pathFindingStep.Origin.MapLevel);
                    best = points;
                }
            }

            i += 1;
        }

        return result;
    }

    // MoveT.PAS:430
    public bool ValidLocation(int X, int Y, int L)
    {
        MapField field = Map.MapContainer.GetField(X, Y, L);
        return _movePoint(field, 1) >= 0;
    }

    // MoveT.PAS:437
    public void ExecuteExternalMove(MapField source, MapField? target, int movePoints)
    {
        int canMoveOver = source.CanMoveOver(this);
        int points = _movePoint(target, canMoveOver);
        if (points >= 0)
        {
            Settings.SetupLevel(target.HexLevel);
            if (movePoints == -1)
            {
                points += Settings.GetMovePoints(source.HexX, source.HexY, source.HexLevel);
            }
            else
            {
                points = Settings.GetMovePoints(source.HexX, source.HexY, source.HexLevel) + movePoints;
            }

            int previous = Settings.GetMovePoints(target.HexX, target.HexY, target.HexLevel);
            if (points < previous && points <= MovePointsLimit)
            {
                MapCoordinate mapCoordinate = target.MapCoordinate;
                int index = Settings.SubMoves.IndexOfMapCoordinate(mapCoordinate);
                if (index > Settings.SubMoveIndex)
                {
                    Settings.SubMoves.Delete(index);
                }

                if (Mode.Contains(MoveModes.LogProcessedMapFields) && previous == 16_777_215)
                {
                    _ = _processedMapFields.Add(target);
                }

                Settings.SetMovePoints(target.HexX, target.HexY, target.HexLevel, value: points);
                if (target.HexX == Destination.X && target.HexY == Destination.Y && target.HexLevel == Destination.MapLevel)
                {
                    MovePointsLimit = points;
                }

                PathFindingStep pathFindingStep;
                if ((target.FieldFlags & MapField.MoveOverFlag) == 0 || target.CanMoveOver(this) != 0)
                {
                    pathFindingStep = SetupSubMove(target, origin: source);
                    if ((target.FieldFlags & MapField.MoveExclusiveFlag) != 0)
                    {
                        target.MoveExclusive(this);
                    }

                    pathFindingStep.Owner = Settings.SubMoves;
                }
                else
                {
                    pathFindingStep = new PathFindingStep(null) { Holder = Map };
                    MapCoordinate targetMapCoordinate = target.MapCoordinate;
                    pathFindingStep.Location = targetMapCoordinate;
                    MapCoordinate sourceMapCoordinate = source.MapCoordinate;
                    pathFindingStep.Origin = sourceMapCoordinate;
                    pathFindingStep.Owner = Settings.SubMoves;
                }
            }
        }
    }

    // MoveT.PAS:1257
    public PathFindingStep SetupSubMove(MapField mapField, MapField? origin)
    {
        PathFindingStep pathFindingStep = new(null) { Holder = Map };
        MapCoordinate mapCoordinate = mapField.MapCoordinate;
        pathFindingStep.Location = mapCoordinate;
        int overCode;
        if (origin is not null)
        {
            MapCoordinate originMapCoordinate = origin.MapCoordinate;
            pathFindingStep.Origin = originMapCoordinate;
            overCode = (mapField.FieldFlags & MapField.MoveOverFlag) == 0 ? 1 : mapField.CanMoveOver(this);
        }
        else
        {
            overCode = 1;
        }

        if (overCode != 0)
        {
            int direction;
            if (Settings.Destination.X != -1)
            {
                direction = HexGeometry.DeltaHexToHexDirection(mapField.HexX, mapField.HexY, Settings.Destination.X, Settings.Destination.Y);
                if (direction == 0)
                {
                    direction = 1;
                }
            }
            else
            {
                direction = 1;
            }

            int count;
            int hexDirection;
            for (int i = 0; i <= 2; i++)
            {
                hexDirection = s_oddDirections[direction - 1][i];
                count = CalculateMovePointLine(mapField, direction: hexDirection, moveOverCode: overCode);
                if (count != 0)
                {
                    pathFindingStep.AddMovePointLine(hexDirection, count);
                }
            }

            for (int i = 0; i <= 2; i++)
            {
                hexDirection = s_evenDirections[direction - 1][i];
                count = CalculateMovePointLine(mapField, direction: hexDirection, moveOverCode: overCode);
                if (count != 0)
                {
                    pathFindingStep.AddMovePointLine(hexDirection, count);
                }
            }
        }

        return pathFindingStep;
    }

    // MoveT.PAS:1295
    public bool ProcessSubMove(PathFindingStep pathFindingStep)
    {
        if (pathFindingStep.MovePointLines.Count != 0)
        {
            Settings.SetupLevel(pathFindingStep.Location.MapLevel);
            SetMapL(Map.MapContainer.GetMapLevel(pathFindingStep.Location.MapLevel));
            int direction = pathFindingStep.MovePointLines[0].First;
            int count = pathFindingStep.MovePointLines[0].Second;
            int x = default;
            int y = default;
            HexGeometry.CenterHexNumberToHex(pathFindingStep.Location.X, pathFindingStep.Location.Y, hexNumber: direction, ref x, ref y);
            pathFindingStep.MovePointLines.Delete(0);
            int processed = _processedCount;
            ProcessHex(MapLevel.GetField(x, y), direction, count: count - 1);
            processed = _processedCount - processed;
            StrategicMap.Current.AIExecutor.UseProcessTime(Round((double)((double)processed / 430) * 1_000));
        }

        return pathFindingStep.MovePointLines.Count == 0;
    }

    // MoveT.PAS:1315
    public bool Process()
    {
        Assert(_status.Contains(Statuses.Activated), message: "", file: "MoveT.PAS", line: 1339);
        switch (Settings.Status)
        {
            case MoveControlSettingsStatus.Idle:
                _ = SetupMove();
                Settings.Status = MoveControlSettingsStatus.Processing;
                break;
            case MoveControlSettingsStatus.Processing:
                int count = Settings.SubMoves.Count;
                if (Settings.SubMoveIndex < count)
                {
                    if (ProcessSubMove(Settings.SubMoves.GetSubMove(Settings.SubMoveIndex)))
                    {
                        Settings.SubMoveIndex += 1;
                    }
                }

                int fSubMovesCount = Settings.SubMoves.Count;
                if (Settings.SubMoveIndex >= fSubMovesCount)
                {
                    Settings.Status = MoveControlSettingsStatus.Processed;
                }

                break;
            case MoveControlSettingsStatus.Processed:
            case MoveControlSettingsStatus.Failed:
            default:
                break;
        }

        return Settings.Status is not MoveControlSettingsStatus.Processed and not MoveControlSettingsStatus.Failed;
    }

    // MoveT.PAS:1436
    public void CalculatePath(MoveControlSettings settings, MovePath path, int X, int Y, int L)
    {
        _ = Activate(settings);
        CalculatePath(path, X, Y, L);
        Deactivate();
    }

    // MoveT.PAS:1443
    public int ExploredOnlyMoveOnMovePointProc(MapField? mapField, int moveOverCode)
    {
        int points;
        if ((mapField.FieldFlags & _exploredMask) != 0)
        {
            points = MovePointTable[mapField.TerrainNumber][mapField.OverlayType.ToOriginal() - MovementTables.FirstOverlay];
            if ((mapField.FieldFlags & MapField.MoveOnFlag) != 0)
            {
                points = mapField.CanMoveOn(this, defaultCode: points, moveOverCode);
            }
        }
        else
        {
            points = -1;
        }

        return points;
    }

    // MoveT.PAS:1504
    public void Deactivate()
    {
        if (_status.Contains(Statuses.Activated))
        {
            _status &= ~Statuses.Activated;
            if (Settings.Status == MoveControlSettingsStatus.Processing)
            {
                Settings.MovePointsLimit = MovePointsLimit;
            }

            _ = Settings.Release();
            Settings = null;
        }
    }

    // MoveT.PAS:1559
    public int CanMove(MoveControlSettings settings, CanMoveModes mode)
    {
        int points;
        if (settings.Source.X != settings.Destination.X || settings.Source.Y != settings.Destination.Y || settings.Source.MapLevel != settings.Destination.MapLevel)
        {
            settings.ResetState();
            if (Activate(settings))
            {
                points = -1;
                if (Destination.X == -1 || ValidLocation(Destination.X, Destination.Y, L: Destination.MapLevel))
                {
                    if (Source.MapLevel == Destination.MapLevel && mode.Contains(CanMoveModes.Fast))
                    {
                        points = GetMovePointsCostFast(Source.X, Source.Y, Destination.X, Destination.Y);
                    }

                    if (points == -1 && mode.Contains(CanMoveModes.Exact))
                    {
                        do
                        {
                        }
                        while (!!Process());
                        points = Settings.GetMovePoints(Destination.X, Destination.Y, Destination.MapLevel);
                        if (points == 16777215)
                        {
                            points = -1;
                        }
                    }
                }

                Deactivate();
            }
            else
            {
                points = -1;
            }
        }
        else
        {
            points = 0;
        }

        return points;
    }

    // MoveT.PAS:1589
    public bool ValidPath(MoveControlSettings settings, MovePath path)
    {
        bool valid = true;
        int i = path.Count - 2;
        int points = 0;
        settings.MovePointsMax = path.PositionToMovePoints(0);
        ProcessMove(settings);
        while (i > 0 && valid)
        {
            points += path[i].MoveCost;
            PathStep item = path[i];
            PathStep pathItem = path[i];
            valid = points == settings.GetMovePoints(path[i].Location.X, pathItem.Location.Y, item.Location.MapLevel);
            i -= 1;
        }

        return valid;
    }

    // MoveT.PAS:1608
    public void CalculateGaps(int sourceX, int sourceY, int destinationX, int destinationY)
    {
        bool clear = true;
        GapStarts.Count = 0;
        GapEnds.Count = 0;
        _directRoute.Count = 0;
        int overCode = 1;
        _ = _directRoute.AddMapCoordinate(sourceX.TruncateToSByte(), sourceY.TruncateToSByte(), 0);
        int x = 0;
        int y = 0;
        int direction = 0;
        while (sourceX != destinationX || sourceY != destinationY)
        {
            if (sourceX == destinationX)
            {
                if (sourceY > destinationY)
                {
                    direction = 1;
                    x = sourceX;
                    y = sourceY - 1;
                }
                else
                {
                    if (sourceY < destinationY)
                    {
                        direction = 4;
                        x = sourceX;
                        y = sourceY + 1;
                    }
                }
            }
            else
            {
                if ((sourceX & 1) == 0)
                {
                    if (sourceX > destinationX)
                    {
                        if (sourceY > destinationY)
                        {
                            direction = 6;
                            x = sourceX - 1;
                            y = sourceY - 1;
                        }
                        else
                        {
                            direction = 5;
                            x = sourceX - 1;
                            y = sourceY;
                        }
                    }
                    else
                    {
                        if (sourceY > destinationY)
                        {
                            direction = 2;
                            x = sourceX + 1;
                            y = sourceY - 1;
                        }
                        else
                        {
                            direction = 3;
                            x = sourceX + 1;
                            y = sourceY;
                        }
                    }
                }
                else
                {
                    if (sourceX > destinationX)
                    {
                        if (sourceY < destinationY)
                        {
                            direction = 5;
                            x = sourceX - 1;
                            y = sourceY + 1;
                        }
                        else
                        {
                            direction = 6;
                            x = sourceX - 1;
                            y = sourceY;
                        }
                    }
                    else
                    {
                        if (sourceY < destinationY)
                        {
                            direction = 3;
                            x = sourceX + 1;
                            y = sourceY + 1;
                        }
                        else
                        {
                            direction = 2;
                            x = sourceX + 1;
                            y = sourceY;
                        }
                    }
                }
            }

            MapField field = MapLevel.GetField(x, y);
            int points = _movePoint(field, overCode);
            if (points >= 0)
            {
                if ((field.FieldFlags & MapField.MoveOverFlag) != 0 && (x != destinationX || y != destinationY))
                {
                    overCode = field.CanMoveOver(this);
                    if (overCode == 0)
                    {
                        points = -1;
                        overCode = 1;
                    }
                }
                else
                {
                    overCode = 1;
                }
            }

            if (points < 0)
            {
                if (clear)
                {
                    _ = GapStarts.AddMapCoordinate(sourceX.TruncateToSByte(), sourceY.TruncateToSByte(), direction.TruncateToSByte());
                    clear = false;
                    overCode = 1;
                }
            }
            else
            {
                if (!clear)
                {
                    _ = GapEnds.AddMapCoordinate(x.TruncateToSByte(), y.TruncateToSByte(), -1);
                    clear = true;
                }
            }

            sourceX = x;
            sourceY = y;
            _ = _directRoute.AddMapCoordinate(sourceX.TruncateToSByte(), sourceY.TruncateToSByte(), points.TruncateToSByte());
        }

        if (!clear)
        {
            _ = GapEnds.AddMapCoordinate(destinationX.TruncateToSByte(), destinationY.TruncateToSByte(), -1);
        }
    }

    // MoveT.PAS:1815
    public int GetMovePointsCostFast(int sourceX, int sourceY, int destinationX, int destinationY)
    {
        int i = 1;
        int points = 0;
        CalculateGaps(sourceX, sourceY, destinationX, destinationY);
        while (i < _directRoute.Count && points != -1)
        {
            int cost = _directRoute[i].MapLevel;
            if (cost < 0)
            {
                MapCoordinate item = _directRoute[i - 1];
                MapCoordinate fDirectRouteItem = _directRoute[i - 1];
                int gapIndex = GapStarts.IndexOfXY(fDirectRouteItem.X, item.Y);
                cost = CalculateTurnAroundMovePoints(ref gapIndex);
                if (cost != -1)
                {
                    points += cost;
                    MapCoordinate fGapEndsItem = GapEnds[gapIndex];
                    MapCoordinate item2 = GapEnds[gapIndex];
                    i = _directRoute.IndexOfXY(item2.X, fGapEndsItem.Y);
                }
                else
                {
                    points = -1;
                }
            }
            else
            {
                points += cost;
            }

            i += 1;
        }

        return points;
    }

    private static ReadOnlySpan<int> OppositeDirections => [0, 4, 5, 6, 1, 2, 3];

    private static readonly ImmutableArray<ImmutableArray<int>> s_sideDirections = [[-1, -1], [6, 2], [1, 3], [2, 4], [3, 5], [4, 6], [5, 1]];
    private static readonly ImmutableArray<ImmutableArray<byte>> s_oddDirections = [[1, 3, 5], [3, 1, 5], [3, 5, 1], [5, 3, 1], [5, 1, 3], [1, 5, 3]];
    private static readonly ImmutableArray<ImmutableArray<byte>> s_evenDirections = [[2, 4, 6], [4, 2, 6], [4, 6, 2], [6, 4, 2], [6, 2, 4], [2, 6, 4]];

    [Flags]
    public enum Statuses : byte
    {
        None = 0,
        Activated = 1 << 0,
    }
}
