// Translated from AoWE.pas.

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// status, settings, cycles, army table, transporter table, multiple gaps, mode, search state,
// path settings
public partial class AIMoveControl : EngineObject
{
    // AIGroup.pas:3911
    public AIMoveControl(ECustomNode owner) : base(owner)
    {
        _pathSettings = new MoveControlSettings(null);
        Clear();
    }

    public AIMoveControlStatus Status { get; private set; }
    public AIMoveControlSettings Settings { get; private set; }

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
        _pathSettings.Release();
        base.Dispose();
    }

    // AIGroup.pas:3925
    public override void Clear()
    {
        AIMoveControlSettings settings;
        settings = Settings;
        if (settings != null)
        {
            settings.Release();
            Settings = null;
        }

        Status = AIMoveControlStatus.Idle;
        _processCycles = 0;
        _pathSettings.Reset();
        _pathSettings.Mode = _pathSettings.Mode + Set<MoveModeElement>.Of(MoveModeElement.AI);
    }

    // AIGroup.pas:3941
    public void ProcessingDone(AIMoveControlStatus status)
    {
        if (Settings != null)
        {
            Status = status;
            Settings.Status = status;
            Settings.RaiseDone(this);
        }
    }

    // AIGroup.pas:3951
    public void ProcessCanMove()
    {
        int points;
        switch (_state)
        {
            case 0:
                _pathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = StrategicMap.Current.MoveControl.CanMove(_pathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                Settings.MovePoints = points;
                if (points != -1)
                {
                    ProcessingDone(AIMoveControlStatus.Done);
                }
                else
                {
                    _state += 1;
                    if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 6)
                    {
                        ProcessCanMove();
                    }
                }

                break;
            case 1:
                _pathSettings.MovePointsMax = Settings.MovePointsMax;
                var canMove = StrategicMap.Current.MoveControl.CanMove(_pathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Exact));
                Settings.MovePoints = canMove;
                Settings.TransportPath.Assign(_pathSettings.Path);
                ProcessingDone(AIMoveControlStatus.Done);
                break;
        }
    }

    // AIGroup.pas:3978
    public void SelectCombinedTransporterArmyMpT()
    {
        sbyte points;
        for (int terrain = 0; terrain <= 15; terrain++)
        {
            for (int overlay = -1; overlay <= 11; overlay++)
            {
                points = _movePointTable[terrain][overlay - MovementTables.FirstOverlay];
                if (points == -1)
                {
                    points = _transportMovePointTable[terrain][overlay - MovementTables.FirstOverlay];
                    if (points != -1)
                    {
                        points = 8;
                    }
                }

                _pathSettings.MovePointTable[terrain][overlay - MovementTables.FirstOverlay] = points;
            }
        }
    }

    // AIGroup.pas:3993
    public bool CalculateTransportGaps(MovePath path, MovePointTable table)
    {
        bool result;
        int I;
        int firstGap;
        int lastGap;
        int gaps;
        bool inGap;
        MapField mapField;
        gaps = 0;
        firstGap = -1;
        lastGap = -1;
        I = path.Count - 1;
        inGap = false;
        while (I > 1)
        {
            I -= 1;
            var item = path[I];
            var pathItem = path[I];
            var item2 = path[I];
            mapField = StrategicMap.Current.MapContainer.GetField(item2.X, pathItem.Y, item.MapLevel);
            if (table[mapField.TerrainType][mapField.OverlayType - MovementTables.FirstOverlay] == -1)
            {
                if (!inGap)
                {
                    if (firstGap == -1)
                    {
                        firstGap = I;
                    }

                    gaps += 1;
                    inGap = true;
                }
            }
            else
            {
                if (inGap)
                {
                    lastGap = I + 1;
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
            var item = path[firstGap + 1];
            Settings.BoardingPoint = item;
            var pathItem = path[firstGap];
            Settings.TransportSource = pathItem;
            var item2 = path[lastGap - 1];
            Settings.ArrivalPoint = item2;
            var item3 = path[lastGap];
            Settings.TransportDestination = item3;
            result = true;
            _multipleTransportGaps = gaps > 1;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:4037
    public void ProcessTransporterCanMove()
    {
        int points;
        MapCoordinate Location = default;
        switch (_state)
        {
            case 0:
                _pathSettings.MovePointTable = _movePointTable;
                _pathSettings.MovePointsMax = Settings.QuickMovePointsMax;
                points = StrategicMap.Current.MoveControl.CanMove(_pathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                Settings.MovePoints = points;
                if (points == -1)
                {
                    _state += 1;
                    if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 6)
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
                _pathSettings.Mode = Settings.MoveMode + Set<MoveModeElement>.Of(MoveModeElement.AvoidArmies);
                StrategicMap.Current.MoveControl.ProcessMove(_pathSettings);
                if (_pathSettings.Path.Count != 0)
                {
                    var item = _pathSettings.Path[0];
                    var fPathItem = _pathSettings.Path[0];
                    var item2 = _pathSettings.Path[0];
                    var movePoints = _pathSettings.GetMovePoints(item2.X, fPathItem.Y, item.MapLevel);
                    Settings.MovePoints = movePoints;
                    Settings.TransportPath.Assign(_pathSettings.Path);
                    if (CalculateTransportGaps(_pathSettings.Path, _movePointTable))
                    {
                        _state = 2;
                        _pathSettings.MovePointTable = _transportMovePointTable;
                        Settings.Transporter.GetLocation(out Location.X, out Location.Y, out Location.MapLevel);
                        _pathSettings.SetSourcePosition(Location.X, Location.Y, Location.MapLevel);
                        ref var location = ref Settings.TransportSource;
                        _pathSettings.SetDestinationPosition(location.X, location.Y, location.MapLevel);

                        if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
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
                points = StrategicMap.Current.MoveControl.CanMove(_pathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast));
                if (points == -1)
                {
                    _state = 3;
                    if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 6)
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
                        ref var location = ref Settings.TransportSource;
                        _pathSettings.SetDestinationPosition(location.X, location.Y, location.MapLevel);

                        _multipleTransportGaps = false;
                        if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
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
                points = StrategicMap.Current.MoveControl.CanMove(_pathSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Exact));
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
                        ref var location = ref Settings.TransportDestination;
                        _pathSettings.SetDestinationPosition(location.X, location.Y, location.MapLevel);

                        _multipleTransportGaps = false;
                        _state = 2;
                        if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
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
        }
    }

    // AIGroup.pas:4128
    public void Initialize(AIMoveControlSettings settings)
    {
        AIMoveControlSettings oldSettings;
        Troop transporter;
        oldSettings = Settings;
        if (oldSettings != null)
        {
            oldSettings.Release();
        }

        Settings = settings;
        if (Settings != null)
        {
            Settings.AddRef();
            Settings.MovePoints = -1;
            Settings.TransportSource.X = -1;
            Settings.TransportDestination.X = -1;
            Settings.BoardingPoint.X = -1;
            Settings.ArrivalPoint.X = -1;
            Settings.Status = AIMoveControlStatus.Processing;
            _movePointTable = Settings.MovePointTable;
            transporter = Settings.Transporter;
            if (transporter != null)
            {
                var moveTypesToInt32 = MovementTables.MoveTypesToInt32(transporter.GetMoveTypes());
                _transportMovePointTable = MovementTables.MovePointTables[moveTypesToInt32];
            }
        }
    }

    // AIGroup.pas:4152
    public bool CanMove(AIMoveControlSettings settings)
    {
        bool result;
        Reset();
        Initialize(settings);
        Status = AIMoveControlStatus.Processing;
        _pathSettings.Reset();
        _pathSettings.SetSourcePosition(this.Settings.SourceX, this.Settings.SourceY, this.Settings.SourceL);

        _pathSettings.Player = Settings.Player;
        _pathSettings.SetDestinationPosition(Settings.DestinationX, Settings.DestinationY, Settings.DestinationL);
        _pathSettings.MovePointsMax = Settings.MovePointsMax;
        _pathSettings.ArmyHexSprite = Settings.ArmyHexSprite;
        _pathSettings.Selection = Settings.Selection;
        _pathSettings.Mode = Settings.MoveMode;
        if (Settings.Transporter == null)
        {
            _mode = AIMoveControlMode.CanMove;
            _pathSettings.MovePointTable = _movePointTable;
            if (Settings.SourceL == Settings.DestinationL)
            {
                _state = 0;
                if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
                {
                    ProcessCanMove();
                }
            }
            else
            {
                _state = 1;
            }

            result = true;
        }
        else
        {
            _mode = AIMoveControlMode.TransporterCanMove;
            var armyHexSprite = Settings.Transporter.Owner is HeroList ? null : Settings.Transporter.ArmyOwner.GetArmyHexSprite();
            _pathSettings.TransporterArmy = armyHexSprite;
            if (Settings.SourceL == Settings.DestinationL)
            {
                _state = 0;
                if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 1)
                {
                    ProcessTransporterCanMove();
                }
            }
            else
            {
                _state = 1;
            }

            result = true;
        }

        return result;
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
            }
        }

        return Status == AIMoveControlStatus.Processing;
    }

    // intf_P5_06.inc:180

}

public enum AIMoveControlMode : byte
{
    CanMove = 0, TransporterCanMove = 1
}
