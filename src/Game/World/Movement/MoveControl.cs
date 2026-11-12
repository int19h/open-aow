// Translated from AoWE.pas.
using System.Collections.Immutable;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

// map, status, player, mode, source, destination, map level, processed count, move limit, move
// table, processed fields, army, selection, move data, callback, field offsets, point offsets,
// gap endpoints, direct route, exploration mask, settings
public partial class MoveControl : IDisposable
{

    // MoveT.PAS:257
    public MoveControl(GameMap owner)
    {
        Map = owner;
        GapStarts = new MapCoordinateList();
        GapEnds = new MapCoordinateList();
        _directRoute = new MapCoordinateList();
    }

    public GameMap Map { get; init; }

    private Set<StatusElement> _status;

    public sbyte Player { get; private set; }
    public Set<MoveModeElement> Mode { get; private set; }

    private int _sourceX;
    private int _sourceY;
    private int _sourceL;

    public int DestinationX { get; set; }
    public int DestinationY { get; set; }
    public int DestinationL { get; private set; }
    public MapLevel MapLevel { get; set; }

    private int _processedCount;

    public int MovePointsLimit { get; set; }

    public MovePointTable MovePointTable;
    private DelphiList<MapField> _processedMapFields;

    public ArmyHexSprite ArmyHexSprite { get; private set; }
    public int Selection { get; private set; }
    public ArmyHexSprite TransporterArmy { get; private set; }

    public event MoveOnMovePointProc OnMovePoint;

    internal void SetMovePointHandler(MoveOnMovePointProc handler) => OnMovePoint = handler;

    private readonly int[][] _fieldOffsets = [new int[8], new int[8]];
    private readonly int[][] _pointOffsets = [new int[8], new int[8]];

    public MapCoordinateList GapStarts { get; init; }
    public MapCoordinateList GapEnds { get; init; }

    private readonly MapCoordinateList _directRoute;
    private int _exploredMask;

    public MoveControlSettings Settings { get; set; }

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
        MapField field;
        PathFindingStep pathFindingStep;
        _processedCount = 0;
        Settings.ClearMovePoints();
        Settings.SubMoves.Clear();
        Settings.SubMoveIndex = 0;
        Settings.Path.Clear();
        if (Mode.Contains(MoveModeElement.LogProcessedMapFields))
        {
            Settings.ProcessedMapFields.Count = 0;
            Settings.ProcessedMapFields.Capacity = 4096;
        }

        // { Original bug: the Y bound also uses the map width. }
        if (!(MapLevel != null && _sourceX >= 0 && _sourceX < MapLevel.Width && _sourceY >= 0 && _sourceY < MapLevel.Width))
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Invalid settings (TMoveControl)", "D:\\AoWDev\\AoWE\\inc\\MoveT.PAS", 32187);
        }

        if ((Settings.SourceX != Settings.DestinationX || Settings.SourceY != Settings.DestinationY || Settings.SourceL != Settings.DestinationL) && (DestinationX == -1 || ValidLocation(DestinationX, DestinationY, DestinationL)))
        {
            Settings.SetupLevel(Settings.SourceL);
            if (Settings.DestinationL != -1)
            {
                Settings.SetupLevel(Settings.DestinationL);
            }

            if (_sourceL == DestinationL && MovePointsLimit > 40 && DestinationX != -1)
            {
                MovePointsLimit = GetMovePointsCostFast(_sourceX, _sourceY, DestinationX, DestinationY);
                if (MovePointsLimit == -1 || MovePointsLimit > Settings.MovePointsMax)
                {
                    MovePointsLimit = Settings.MovePointsMax;
                }

                StrategicMap.Current.AIExecutor.UseProcessTime(MovePointsLimit);
            }

            Settings.SetMovePoints(_sourceX, _sourceY, _sourceL, 0);
            field = MapLevel.GetField(_sourceX, _sourceY);
            if (Mode.Contains(MoveModeElement.LogProcessedMapFields))
            {
                _processedMapFields.Add(field);
            }

            pathFindingStep = SetupSubMove(field, null);
            if ((field.FieldFlags & 32768) != 0)
            {
                field.MoveExclusive(this);
            }

            pathFindingStep.Owner = Settings.SubMoves;
            StrategicMap.Current.AIExecutor.UseProcessTime(100);
        }
        else
        {
            Settings.SetupLevel(Settings.SourceL);
            Settings.SetMovePoints(_sourceX, _sourceY, _sourceL, 0);
            Settings.Status = MoveControlSettingsStatus.Processed;
        }

        return true;
    }

    // MoveT.PAS:1515
    public virtual void ProcessMove(MoveControlSettings settings)
    {
        if (settings.SourceX != settings.DestinationX || settings.SourceY != settings.DestinationY || settings.SourceL != settings.DestinationL)
        {
            if (Activate(settings))
            {
                Assert(settings.GetEngine().Demo || settings.Status != MoveControlSettingsStatus.Processed, "", "MoveT.PAS", 1541);
                do
                {
                }
                while (!!Process());
                if (DestinationX != -1)
                {
                    CalculatePath(settings.Path, (sbyte)DestinationX, (sbyte)DestinationY, (sbyte)DestinationL);
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
            settings.Path.AddMapCoordinate((sbyte)settings.SourceX, (sbyte)settings.SourceY, (sbyte)settings.SourceL);
            settings.Status = MoveControlSettingsStatus.Processed;
        }
    }

    // MoveT.PAS:274
    public void Clear()
    {
        Deactivate();
        Player = -1;
        MapLevel = null;
        _sourceX = -1;
        _sourceY = -1;
        _sourceL = -1;
        DestinationX = -1;
        DestinationY = -1;
        DestinationL = -1;
        _processedCount = 0;
        OnMovePoint = DefaultMoveOnMovePointProc;
        _processedMapFields = null;
    }

    // MoveT.PAS:299
    public int DefaultMoveOnMovePointProc(MapField mapField, int moveOverCode)
    {
        int overlay;
        int points;
        overlay = mapField.OverlayType;
        points = MovePointTable[mapField.TerrainType][overlay - MovementTables.FirstOverlay];
        if ((mapField.FieldFlags & 16384) != 0)
        {
            points = mapField.CanMoveOn(this, points, moveOverCode);
        }

        return points;
    }

    // MoveT.PAS:308
    public MapField FindSubMoveOrigin(MapField mapField)
    {
        int best;
        int points;
        int I;
        PathFindingStep pathFindingStep;
        MoveControlSettings settings;
        best = 16777215;
        MapField result = null;
        I = 0;
        settings = Settings;
        while (I < settings.SubMoves.Count)
        {
            pathFindingStep = settings.SubMoves.GetSubMove(I);
            if (mapField.HexX == pathFindingStep.Location.X && mapField.HexY == pathFindingStep.Location.Y && mapField.HexLevel == pathFindingStep.Location.MapLevel)
            {
                points = settings.GetMovePoints(pathFindingStep.Origin.X, pathFindingStep.Origin.Y, pathFindingStep.Origin.MapLevel);
                if (points <= best)
                {
                    // { After pushing L, the original stack slot holds Self, not Result. }
                    result = Map.MapContainer.GetField(pathFindingStep.Origin.X, pathFindingStep.Origin.Y, pathFindingStep.Origin.MapLevel);
                    best = points;
                }
            }

            I += 1;
        }

        return result;
    }

    // MoveT.PAS:430
    public bool ValidLocation(int X, int Y, int L)
    {
        MapField field;
        field = Map.MapContainer.GetField(X, Y, L);
        return OnMovePoint(field, 1) >= 0;
    }

    // MoveT.PAS:437
    public void ExecuteExternalMove(MapField source, MapField target, int movePoints)
    {
        int points = default;
        int previous = default;
        int index = default;
        PathFindingStep pathFindingStep = default;
        var canMoveOver = source.CanMoveOver(this);
        points = OnMovePoint(target, canMoveOver);
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

            previous = Settings.GetMovePoints(target.HexX, target.HexY, target.HexLevel);
            if (points < previous && points <= MovePointsLimit)
            {
                var mapCoordinate = target.GetMapCoordinate();
                index = Settings.SubMoves.IndexOfMapCoordinate(mapCoordinate);
                if (index > Settings.SubMoveIndex)
                {
                    Settings.SubMoves.Delete(index);
                }

                if (Mode.Contains(MoveModeElement.LogProcessedMapFields) && previous == 16777215)
                {
                    _processedMapFields.Add(target);
                }

                Settings.SetMovePoints(target.HexX, target.HexY, target.HexLevel, points);
                if (target.HexX == DestinationX && target.HexY == DestinationY && target.HexLevel == DestinationL)
                {
                    MovePointsLimit = points;
                }

                if ((target.FieldFlags & 8192) == 0 || target.CanMoveOver(this) != 0)
                {
                    pathFindingStep = SetupSubMove(target, source);
                    if ((target.FieldFlags & 32768) != 0)
                    {
                        target.MoveExclusive(this);
                    }

                    pathFindingStep.Owner = Settings.SubMoves;
                }
                else
                {
                    pathFindingStep = new PathFindingStep(null) { Holder = Map };
                    var targetMapCoordinate = target.GetMapCoordinate();
                    pathFindingStep.Location.Packed = targetMapCoordinate;
                    var sourceMapCoordinate = source.GetMapCoordinate();
                    pathFindingStep.Origin.Packed = sourceMapCoordinate;
                    pathFindingStep.Owner = Settings.SubMoves;
                }
            }
        }
    }

    // MoveT.PAS:1257
    public PathFindingStep SetupSubMove(MapField mapField, MapField origin)
    {
        PathFindingStep pathFindingStep;
        int overCode;
        int direction;
        int count;
        int hexDirection;
        pathFindingStep = new PathFindingStep(null) { Holder = Map };
        var mapCoordinate = mapField.GetMapCoordinate();
        pathFindingStep.Location.Packed = mapCoordinate;
        if (origin != null)
        {
            var originMapCoordinate = origin.GetMapCoordinate();
            pathFindingStep.Origin.Packed = originMapCoordinate;
            if ((mapField.FieldFlags & 8192) == 0)
            {
                overCode = 1;
            }
            else
            {
                overCode = mapField.CanMoveOver(this);
            }
        }
        else
        {
            overCode = 1;
        }

        if (overCode != 0)
        {
            if (Settings.DestinationX != -1)
            {
                direction = HexGeometry.DeltaHexToHexDirection(mapField.HexX, mapField.HexY, Settings.DestinationX, Settings.DestinationY);
                if (direction == 0)
                {
                    direction = 1;
                }
            }
            else
            {
                direction = 1;
            }

            for (int I = 0; I <= 2; I++)
            {
                hexDirection = s_oddDirections[direction - 1][I];
                count = CalculateMovePointLine(mapField, hexDirection, overCode);
                if (count != 0)
                {
                    pathFindingStep.AddMovePointLine(hexDirection, count);
                }
            }

            for (int I = 0; I <= 2; I++)
            {
                hexDirection = s_evenDirections[direction - 1][I];
                count = CalculateMovePointLine(mapField, hexDirection, overCode);
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
        int X = default;
        int Y = default;
        int count;
        int direction;
        int processed;
        if (pathFindingStep.MovePointLines.Count != 0)
        {
            Settings.SetupLevel(pathFindingStep.Location.MapLevel);
            SetMapL(Map.MapContainer.GetMapLevel(pathFindingStep.Location.MapLevel));
            direction = pathFindingStep.MovePointLines[0].First;
            count = pathFindingStep.MovePointLines[0].Second;
            HexGeometry.CenterHexNumberToHex(pathFindingStep.Location.X, pathFindingStep.Location.Y, direction, ref X, ref Y);
            pathFindingStep.MovePointLines.Delete(0);
            processed = _processedCount;
            ProcessHex(MapLevel.GetField(X, Y), direction, count - 1);
            processed = _processedCount - processed;
            StrategicMap.Current.AIExecutor.UseProcessTime(Round((double)((double)processed / 430) * 1000));
        }

        return pathFindingStep.MovePointLines.Count == 0;
    }

    // MoveT.PAS:1315
    public bool Process()
    {
        Assert(_status.Contains(StatusElement.Activated), "", "MoveT.PAS", 1339);
        switch (Settings.Status)
        {
            case MoveControlSettingsStatus.Idle:
                SetupMove();
                Settings.Status = MoveControlSettingsStatus.Processing;
                break;
            case MoveControlSettingsStatus.Processing:
                var count = Settings.SubMoves.Count;
                if (Settings.SubMoveIndex < count)
                {
                    if (ProcessSubMove(Settings.SubMoves.GetSubMove(Settings.SubMoveIndex)))
                    {
                        Settings.SubMoveIndex += 1;
                    }
                }

                var fSubMovesCount = Settings.SubMoves.Count;
                if (Settings.SubMoveIndex >= fSubMovesCount)
                {
                    Settings.Status = MoveControlSettingsStatus.Processed;
                }

                break;
        }

        return Settings.Status != MoveControlSettingsStatus.Processed && Settings.Status != MoveControlSettingsStatus.Failed;
    }

    // MoveT.PAS:1436
    public void CalculatePath(MoveControlSettings settings, MovePath path, sbyte X, sbyte Y, sbyte L)
    {
        Activate(settings);
        CalculatePath(path, X, Y, L);
        Deactivate();
    }

    // MoveT.PAS:1443
    public int ExploredOnlyMoveOnMovePointProc(MapField mapField, int moveOverCode)
    {
        int points;
        if ((mapField.FieldFlags & _exploredMask) != 0)
        {
            points = MovePointTable[mapField.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay];
            if ((mapField.FieldFlags & 16384) != 0)
            {
                points = mapField.CanMoveOn(this, points, moveOverCode);
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
        if (_status.Contains(StatusElement.Activated))
        {
            _status = _status - Set<StatusElement>.Of(StatusElement.Activated);
            if (Settings.Status == MoveControlSettingsStatus.Processing)
            {
                Settings.MovePointsLimit = MovePointsLimit;
            }

            Settings.Release();
            Settings = null;
        }
    }

    // MoveT.PAS:1559
    public int CanMove(MoveControlSettings settings, Set<CanMoveModeElement> mode)
    {
        int points;
        if (settings.SourceX != settings.DestinationX || settings.SourceY != settings.DestinationY || settings.SourceL != settings.DestinationL)
        {
            settings.ResetState();
            if (Activate(settings))
            {
                points = -1;
                if (DestinationX == -1 || ValidLocation(DestinationX, DestinationY, DestinationL))
                {
                    if (_sourceL == DestinationL && mode.Contains(CanMoveModeElement.Fast))
                    {
                        points = GetMovePointsCostFast(_sourceX, _sourceY, DestinationX, DestinationY);
                    }

                    if (points == -1 && mode.Contains(CanMoveModeElement.Exact))
                    {
                        do
                        {
                        }
                        while (!!Process());
                        points = Settings.GetMovePoints(DestinationX, DestinationY, DestinationL);
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
        bool valid;
        int I;
        int points;
        valid = true;
        I = path.Count - 2;
        points = 0;
        settings.MovePointsMax = path.PositionToMovePoints(0);
        ProcessMove(settings);
        while (I > 0 && valid)
        {
            points += path[I].Tag;
            var item = path[I];
            var pathItem = path[I];
            valid = points == settings.GetMovePoints(path[I].X, pathItem.Y, item.MapLevel);
            I -= 1;
        }

        return valid;
    }

    // MoveT.PAS:1608
    public void CalculateGaps(int sourceX, int sourceY, int destinationX, int destinationY)
    {
        int X;
        int Y;
        int direction;
        int overCode;
        MapField field;
        bool clear;
        int points = default;
        clear = true;
        GapStarts.Count = 0;
        GapEnds.Count = 0;
        _directRoute.Count = 0;
        overCode = 1;
        _directRoute.AddMapCoordinate((sbyte)sourceX, (sbyte)sourceY, 0);
        X = 0;
        Y = 0;
        direction = 0;
        while (sourceX != destinationX || sourceY != destinationY)
        {
            if (sourceX == destinationX)
            {
                if (sourceY > destinationY)
                {
                    direction = 1;
                    X = sourceX;
                    Y = sourceY - 1;
                }
                else
                {
                    if (sourceY < destinationY)
                    {
                        direction = 4;
                        X = sourceX;
                        Y = sourceY + 1;
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
                            X = sourceX - 1;
                            Y = sourceY - 1;
                        }
                        else
                        {
                            direction = 5;
                            X = sourceX - 1;
                            Y = sourceY;
                        }
                    }
                    else
                    {
                        if (sourceY > destinationY)
                        {
                            direction = 2;
                            X = sourceX + 1;
                            Y = sourceY - 1;
                        }
                        else
                        {
                            direction = 3;
                            X = sourceX + 1;
                            Y = sourceY;
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
                            X = sourceX - 1;
                            Y = sourceY + 1;
                        }
                        else
                        {
                            direction = 6;
                            X = sourceX - 1;
                            Y = sourceY;
                        }
                    }
                    else
                    {
                        if (sourceY < destinationY)
                        {
                            direction = 3;
                            X = sourceX + 1;
                            Y = sourceY + 1;
                        }
                        else
                        {
                            direction = 2;
                            X = sourceX + 1;
                            Y = sourceY;
                        }
                    }
                }
            }

            field = MapLevel.GetField(X, Y);
            points = OnMovePoint(field, overCode);
            if (points >= 0)
            {
                if ((field.FieldFlags & 8192) != 0 && (X != destinationX || Y != destinationY))
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
                    GapStarts.AddMapCoordinate((sbyte)sourceX, (sbyte)sourceY, (sbyte)direction);
                    clear = false;
                    overCode = 1;
                }
            }
            else
            {
                if (!clear)
                {
                    GapEnds.AddMapCoordinate((sbyte)X, (sbyte)Y, -1);
                    clear = true;
                }
            }

            sourceX = X;
            sourceY = Y;
            _directRoute.AddMapCoordinate((sbyte)sourceX, (sbyte)sourceY, (sbyte)points);
        }

        if (!clear)
        {
            GapEnds.AddMapCoordinate((sbyte)destinationX, (sbyte)destinationY, -1);
        }
    }

    // MoveT.PAS:1815
    public int GetMovePointsCostFast(int sourceX, int sourceY, int destinationX, int destinationY)
    {
        int I;
        int points;
        int gapIndex;
        int cost;
        I = 1;
        gapIndex = 0;
        points = 0;
        CalculateGaps(sourceX, sourceY, destinationX, destinationY);
        while (I < _directRoute.Count && points != -1)
        {
            cost = _directRoute[I].MapLevel;
            if (cost < 0)
            {
                var item = _directRoute[I - 1];
                var fDirectRouteItem = _directRoute[I - 1];
                gapIndex = GapStarts.IndexOfXY(fDirectRouteItem.X, item.Y);
                cost = CalculateTurnAroundMovePoints(ref gapIndex);
                if (cost != -1)
                {
                    points += cost;
                    var fGapEndsItem = GapEnds[gapIndex];
                    var item2 = GapEnds[gapIndex];
                    I = _directRoute.IndexOfXY(item2.X, fGapEndsItem.Y);
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

            I += 1;
        }

        return points;
    }

    private static ReadOnlySpan<int> OppositeDirections => [0, 4, 5, 6, 1, 2, 3];

    private static readonly ImmutableArray<ImmutableArray<int>> s_sideDirections = [[-1, -1], [6, 2], [1, 3], [2, 4], [3, 5], [4, 6], [5, 1]];
    private static readonly ImmutableArray<ImmutableArray<byte>> s_oddDirections = [[1, 3, 5], [3, 1, 5], [3, 5, 1], [5, 3, 1], [5, 1, 3], [1, 5, 3]];
    private static readonly ImmutableArray<ImmutableArray<byte>> s_evenDirections = [[2, 4, 6], [4, 2, 6], [4, 6, 2], [6, 4, 2], [6, 2, 4], [2, 6, 4]];

    public enum StatusElement : byte
    {
        Activated = 0
    }

    public delegate int MoveOnMovePointProc(MapField MapF, int MoveOverCode);

    // intf_P5_01.inc:120

    // intf_P5_01.inc:120

}
