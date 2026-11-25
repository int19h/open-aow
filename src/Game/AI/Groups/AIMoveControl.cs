#nullable disable warnings
// Translated from AoWE.pas.

using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.AI.Groups;

// status, settings, cycles, army table, transporter table, multiple gaps, mode, search state,
// path settings
public sealed class AIMoveControl : EngineObject
{
    // AIGroup.pas:3911
    public AIMoveControl(CustomNode? owner) : base(owner)
    {
        _pathSettings = new MoveControlSettings(null) { Holder = CreationHolder() };
        Clear();
    }

    public AIMoveControlStatus Status { get; private set; }
    public AIMoveControlSettings? Settings { get; private set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _processCycles;
    private MovePointTable _movePointTable;
    private MovePointTable _transportMovePointTable;
    private bool _multipleTransportGaps;
    private AIMoveControlMode _mode;
    private int _state;
    private readonly MoveControlSettings _pathSettings;

    // AIGroup.pas:3918
    public override void Dispose()
    {
        Clear();
        _ = _pathSettings.Release();
        base.Dispose();
    }

    // AIGroup.pas:3925
    public override void Clear()
    {
        AIMoveControlSettings? settings = Settings;
        if (settings is not null)
        {
            _ = settings.Release();
            Settings = null;
        }

        Status = AIMoveControlStatus.Idle;
        _processCycles = 0;
        _pathSettings.Reset();
        _pathSettings.Mode |= MoveModes.AI;
    }

    // AIGroup.pas:3941
    public void ProcessingDone(AIMoveControlStatus status)
    {
        if (Settings is not null)
        {
            Status = status;
            Settings.Status = status;
            Settings.Complete(this);
        }
    }

    // AIGroup.pas:3951
    public void ProcessCanMove()
    {
        switch (_state)
        {
            case 0:
                _pathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                int points = GetEngine().StrategicMap.MoveControl.CanMove(_pathSettings, mode: CanMoveModes.Fast);
                Settings.MovePoints = points;
                if (points != -1)
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    _state += 1;
                    if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 6)
                    {
                        ProcessCanMove();
                    }
                }

                break;
            case 1:
                _pathSettings.MovePointsMax = Settings.MovePointsMax;
                int canMove = GetEngine().StrategicMap.MoveControl.CanMove(_pathSettings, mode: CanMoveModes.Exact);
                Settings.MovePoints = canMove;
                Settings.TransportPath.Assign(_pathSettings.Path);
                ProcessingDone(AIMoveControlStatus.Done);
                break;
            default:
                break;
        }
    }

    // AIGroup.pas:3978
    public void SelectCombinedTransporterArmyMpT()
    {
        for (int terrain = 0; terrain <= 15; terrain++)
        {
            for (int overlay = -1; overlay <= 11; overlay++)
            {
                int points = _movePointTable[terrain][overlay - MovementTables.FirstOverlay];
                if (points == -1)
                {
                    points = _transportMovePointTable[terrain][overlay - MovementTables.FirstOverlay];
                    if (points != -1)
                    {
                        points = 8;
                    }
                }

                MovePointTable movePointTable = _pathSettings.MovePointTable;
                movePointTable[terrain][overlay - MovementTables.FirstOverlay] = (sbyte)points;
                _pathSettings.MovePointTable = movePointTable;
            }
        }
    }

    // AIGroup.pas:3993
    public bool CalculateTransportGaps(MovePath path, MovePointTable table)
    {
        int gaps = 0;
        int firstGap = -1;
        int lastGap = -1;
        int i = path.Count - 1;
        bool inGap = false;
        while (i > 1)
        {
            i -= 1;
            PathStep item = path[i];
            PathStep pathItem = path[i];
            PathStep item2 = path[i];
            MapField mapField = GetEngine().StrategicMap.MapContainer.GetField(item2.Location.X, pathItem.Location.Y, item.Location.MapLevel);
            if (table[mapField.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay] == -1)
            {
                if (!inGap)
                {
                    if (firstGap == -1)
                    {
                        firstGap = i;
                    }

                    gaps += 1;
                    inGap = true;
                }
            }
            else
            {
                if (inGap)
                {
                    lastGap = i + 1;
                    inGap = false;
                }
            }
        }

        if (inGap)
        {
            lastGap = 1;
        }
        if (gaps != 0)
        {
            PathStep item = path[firstGap + 1];
            Settings.BoardingPoint = item.Location;
            PathStep pathItem = path[firstGap];
            Settings.TransportSource = pathItem.Location;
            PathStep item2 = path[lastGap - 1];
            Settings.ArrivalPoint = item2.Location;
            PathStep item3 = path[lastGap];
            Settings.TransportDestination = item3.Location;
            _multipleTransportGaps = gaps > 1;
        }
        else
        {
            return false;
        }

        return true;
    }

    // AIGroup.pas:4037
    public void ProcessTransporterCanMove()
    {
        int points;
        switch (_state)
        {
            case 0:
                _pathSettings.MovePointTable = _movePointTable;
                _pathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = GetEngine().StrategicMap.MoveControl.CanMove(_pathSettings, mode: CanMoveModes.Fast);
                Settings.MovePoints = points;
                if (points == -1)
                {
                    _state += 1;
                    if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 6)
                    {
                        ProcessTransporterCanMove();
                    }
                }
                else
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }

                break;
            case 1:
                SelectCombinedTransporterArmyMpT();
                _pathSettings.MovePointsMax = Settings.MovePointsMax;
                _pathSettings.Mode = Settings.MoveMode | MoveModes.AvoidArmies;
                GetEngine().StrategicMap.MoveControl.ProcessMove(_pathSettings);
                if (_pathSettings.Path.Count != 0)
                {
                    PathStep item = _pathSettings.Path[0];
                    PathStep fPathItem = _pathSettings.Path[0];
                    PathStep item2 = _pathSettings.Path[0];
                    int movePoints = _pathSettings.GetMovePoints(item2.Location.X, fPathItem.Location.Y, item.Location.MapLevel);
                    Settings.MovePoints = movePoints;
                    Settings.TransportPath.Assign(_pathSettings.Path);
                    if (CalculateTransportGaps(_pathSettings.Path, table: _movePointTable))
                    {
                        _state = 2;
                        _pathSettings.MovePointTable = _transportMovePointTable;
                        MapCoordinate coordinate102 = Settings.Transporter.GetLocation();
                        int locationX = coordinate102.X;
                        int locationY = coordinate102.Y;
                        int locationMapLevel = coordinate102.MapLevel;
                        MapCoordinate mapCoordinate = new((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
                        _pathSettings.SetSourcePosition(mapCoordinate);
                        MapCoordinate location = Settings.TransportSource;
                        _pathSettings.SetDestinationPosition(location);

                        if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 1)
                        {
                            ProcessTransporterCanMove();
                        }
                    }
                    else
                    {
                        ProcessingDone(AIMoveControlStatus.Done);
                    }
                }
                else
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }

                break;
            case 2:
                _pathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = GetEngine().StrategicMap.MoveControl.CanMove(_pathSettings, mode: CanMoveModes.Fast);
                if (points == -1)
                {
                    _state = 3;
                    if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 6)
                    {
                        ProcessTransporterCanMove();
                    }
                }
                else
                {
                    Settings.MovePoints += points;
                    if (_multipleTransportGaps)
                    {
                        // { Original repeats the first gap source before continuing. }
                        MapCoordinate location = Settings.TransportSource;
                        _pathSettings.SetDestinationPosition(location);

                        _multipleTransportGaps = false;
                        if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 1)
                        {
                            ProcessTransporterCanMove();
                        }
                    }
                    else
                    {
                        ProcessingDone(AIMoveControlStatus.Done);
                    }
                }

                break;
            case 3:
                _pathSettings.MovePointTable = _transportMovePointTable;
                _pathSettings.MovePointsMax = Settings.MovePointsMax;
                points = GetEngine().StrategicMap.MoveControl.CanMove(_pathSettings, mode: CanMoveModes.Exact);
                if (points == -1)
                {
                    Settings.MovePoints = -1;
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    Settings.MovePoints += points;
                    if (_multipleTransportGaps)
                    {
                        MapCoordinate location = Settings.TransportDestination;
                        _pathSettings.SetDestinationPosition(location);

                        _multipleTransportGaps = false;
                        _state = 2;
                        if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 1)
                        {
                            ProcessTransporterCanMove();
                        }
                    }
                    else
                    {
                        ProcessingDone(AIMoveControlStatus.Done);
                    }
                }

                break;
            default:
                break;
        }
    }

    // AIGroup.pas:4128
    public void Initialize(AIMoveControlSettings settings)
    {
        AIMoveControlSettings? oldSettings = Settings;
        _ = (oldSettings?.Release());

        Settings = settings;
        if (Settings is not null)
        {
            _ = Settings.AddRef();
            Settings.MovePoints = -1;
            Settings.TransportSource = Settings.TransportSource with { X = -1 };
            Settings.TransportDestination = Settings.TransportDestination with { X = -1 };
            Settings.BoardingPoint = Settings.BoardingPoint with { X = -1 };
            Settings.ArrivalPoint = Settings.ArrivalPoint with { X = -1 };
            Settings.Status = AIMoveControlStatus.Processing;
            _movePointTable = Settings.MovePointTable;
            Troop? transporter = Settings.Transporter;
            if (transporter is not null)
            {
                int moveTypesToInt32 = (byte)transporter.GetMoveTypes();
                _transportMovePointTable = MovementTables.MovePointTables[moveTypesToInt32];
            }
        }
    }

    // AIGroup.pas:4152
    public bool CanMove(AIMoveControlSettings settings)
    {
        Reset();
        Initialize(settings);
        Status = AIMoveControlStatus.Processing;
        _pathSettings.Reset();
        _pathSettings.SetSourcePosition(Settings.Source);

        _pathSettings.Player = Settings.Player;
        _pathSettings.SetDestinationPosition(Settings.Destination);
        _pathSettings.MovePointsMax = Settings.MovePointsMax;
        _pathSettings.ArmyHexSprite = Settings.ArmyHexSprite;
        _pathSettings.Selection = Settings.Selection;
        _pathSettings.Mode = Settings.MoveMode;
        if (Settings.Transporter is null)
        {
            _mode = AIMoveControlMode.CanMove;
            _pathSettings.MovePointTable = _movePointTable;
            if (Settings.Source.MapLevel == Settings.Destination.MapLevel)
            {
                _state = 0;
                if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 1)
                {
                    ProcessCanMove();
                }
            }
            else
            {
                _state = 1;
            }

            return true;
        }
        else
        {
            _mode = AIMoveControlMode.TransporterCanMove;
            ArmyHexSprite? armyHexSprite = Settings.Transporter.ArmyOwner?.GetArmyHexSprite();
            _pathSettings.TransporterArmy = armyHexSprite;
            if (Settings.Source.MapLevel == Settings.Destination.MapLevel)
            {
                _state = 0;
                if (GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 1)
                {
                    ProcessTransporterCanMove();
                }
            }
            else
            {
                _state = 1;
            }

            return true;
        }
    }

    // AIGroup.pas:4191
    public void Cancel()
    {
        if (Status == AIMoveControlStatus.Processing)
        {
            ProcessingDone(AIMoveControlStatus.Canceled);
        }
    }

    // AIGroup.pas:4196
    public void Reset()
    {
        Cancel();
        Clear();
    }

    // AIGroup.pas:4202
    public bool Process()
    {
        if (Status == AIMoveControlStatus.Processing)
        {
            _processCycles += 1;
            switch (_mode)
            {
                case AIMoveControlMode.CanMove:
                    ProcessCanMove();
                    break;
                case AIMoveControlMode.TransporterCanMove:
                    ProcessTransporterCanMove();
                    break;
                default:
                    break;
            }
        }

        return Status == AIMoveControlStatus.Processing;
    }
}

public enum AIMoveControlMode : byte
{
    CanMove = 0, TransporterCanMove = 1
}
