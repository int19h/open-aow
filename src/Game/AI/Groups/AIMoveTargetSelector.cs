// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// state, targets, settings, buffer, transporter movement table, transporter, control
public partial class AIMoveTargetSelector : EngineObject
{
    // AIGroup.pas:6375
    public AIMoveTargetSelector(ECustomNode owner) : base(owner)
    {
        _settings = new MoveControlSettings(null) { Holder = CreationHolder() };
        _targets = new MoveTargetList(null) { Holder = CreationHolder() };
        _bufferedTargets = new MoveTargetList(null) { Holder = CreationHolder() };
        Units = new UnitUserList(null) { Holder = CreationHolder() };
    }

    private int _state;
    private readonly MoveTargetList _targets;

    public AIMoveTargetSelectorStatus Status { get; private set; }

    private readonly MoveControlSettings _settings;

    public Set<AIMoveTargetSelectorModeElement> Mode { get; set; }

    private readonly MoveTargetList _bufferedTargets;

    public event MoveTargetFoundEvent OnTargetFound;

    internal void SetTargetFoundHandler(MoveTargetFoundEvent handler) => OnTargetFound = handler;

    private MovePointTable _transportMovePointTable;
    public MapCoordinate SourceLocation;
    public AITargetInfo TargetInfo;
    public MovePointTable MovePointTable;

    public int MovePointsMax { get; set; }
    public int MovePointsMin { get; set; }

    private Troop _transporter;

    public UnitUserList Units { get; }

    private AIGroupControl _control;

    // AIGroup.pas:6384
    public override void Dispose()
    {
        _settings.Release();
        _targets?.Dispose();
        _bufferedTargets?.Dispose();
        Units?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:6393
    public void SetTransporter(Troop transporter)
    {
        if (transporter != _transporter)
        {
            if (_transporter != null)
            {
                _transporter.Release();
            }

            _transporter = transporter;
            if (_transporter != null)
            {
                _transporter.AddRef();
            }
        }
    }

    // AIGroup.pas:6403
    public void Reset()
    {
        _state = 0;
        Status = AIMoveTargetSelectorStatus.Idle;
        OnTargetFound = null;
        MovePointTable.Fill(-1);
        TargetInfo = default;
        TargetInfo.Player = -1;
        MovePointsMax = 250;
        MovePointsMin = 100;
        SetTransporter(null);
        SourceLocation = MapCoordinate.None;
        _targets.Clear();
        Mode = Set<AIMoveTargetSelectorModeElement>.Of();
        Units.Clear();
    }

    // AIGroup.pas:6421
    public bool SetupArmy(ArmyHexSprite army, byte selection)
    {
        Reset();
        var mapCoordinate = army.GetMapCoordinate();
        SourceLocation = mapCoordinate;
        Units.AddUnits(army.Army, selection);
        TargetInfo.Player = army.Army.Player;
        if (TargetInfo.Player == 0)
        {
            TargetInfo.IndependentRelation = army.Army.IndependentRelation;
        }

        army.Army.CreateMovePointTable(selection, out MovePointTable);
        return true;
    }

    // AIGroup.pas:6432
    public void SetupTransport(Troop transporter, bool resetSettings)
    {
        if (resetSettings)
        {
            Reset();
        }

        SetTransporter(transporter);
        MovePointsMax = 0;
    }

    // AIGroup.pas:6439
    public void Done()
    {
        _state = -1;
        Status = AIMoveTargetSelectorStatus.Done;
    }

    // AIGroup.pas:6445
    public void TriggerTargetFound(MoveTarget target)
    {
        bool searchDone;
        if (OnTargetFound != null)
        {
            searchDone = false;
            OnTargetFound(target, ref searchDone);
            if (searchDone)
            {
                Done();
            }
        }
    }

    // AIGroup.pas:6456
    public bool ProcessBufferedTargets()
    {
        if (_bufferedTargets.Count != 0)
        {
            TriggerTargetFound(_bufferedTargets.GetTarget(0));
            var target = _bufferedTargets.GetTarget(0);
            _targets.AddChild(target);
            var fBufferedTargetsTarget = _bufferedTargets.GetTarget(0);
            _bufferedTargets.RemoveChild(fBufferedTargetsTarget);
        }

        return _bufferedTargets.Count != 0 && _state != -1;
    }

    // AIGroup.pas:6467
    public bool ProcessBufferedTransportTargets()
    {
        MapCoordinate location = default;
        Int32List gaps;
        MoveTarget target;
        bool valid;
        int I;
        if (_bufferedTargets.Count != 0)
        {
            gaps = new Int32List();
            target = _bufferedTargets.GetTarget(0);
            _transporter.GetLocation(out location.X, out location.Y, out location.MapLevel);
            if (ListMovementGaps(StrategicMap.Current, target.MovePath, MovePointTable, gaps, null) != 0)
            {
                _settings.Reset();
                this._settings.Player = TargetInfo.Player;
                this._settings.Mode = this._settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
                this._settings.SetSourcePosition(location.X, location.Y, location.MapLevel);

                _settings.MovePointsMax = MovePointsMax;
                _settings.MovePointTable = _transportMovePointTable;
                I = gaps.Count;
                valid = true;
                while (I != 0 && valid)
                {
                    I -= 1;
                    _settings.ResetState();
                    var item = gaps[I];
                    var fMovePathItem = target.MovePath[item];
                    var gapsItem = gaps[I];
                    var item2 = target.MovePath[gapsItem];
                    var item3 = gaps[I];
                    var item4 = target.MovePath[item3];
                    _settings.SetDestinationPosition(item4.X, item2.Y, fMovePathItem.MapLevel);
                    valid = StrategicMap.Current.MoveControl.CanMove(_settings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast)) != -1;
                }
            }
            else
            {
                valid = true;
            }

            if (valid)
            {
                TriggerTargetFound(target);
                _targets.AddChild(target);
            }

            _bufferedTargets.RemoveChild(target);
            gaps?.Dispose();
        }

        return _bufferedTargets.Count != 0 && _state != -1;
    }

    // AIGroup.pas:6514
    public bool ProcessStates()
    {
        bool yield;
        bool processing;
        bool found;
        int I;
        MoveControlSettings settings;
        MapField field;
        MoveTarget target;
        AITargetData data = default;
        yield = false;
        switch (_state)
        {
            case 0:
                if (MovePointsMax != 0)
                {
                    _bufferedTargets.Clear();
                    _settings.Reset();
                    this._settings.Player = TargetInfo.Player;
                    this._settings.Mode = this._settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
                    this._settings.SetSourcePosition(SourceLocation.X, SourceLocation.Y, SourceLocation.MapLevel);

                    _settings.MovePointsMax = MovePointsMax;
                    _settings.MovePointTable = MovePointTable;
                    _settings.Units.AddUnits(Units);
                    _state = 1;
                }
                else
                {
                    _state = 10;
                }

                yield = false;
                break;
            case 1:
                StrategicMap.Current.MoveControl.Activate(_settings);
                processing = true;
                while (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0 && processing)
                {
                    processing = StrategicMap.Current.MoveControl.Process();
                }

                yield = processing;
                StrategicMap.Current.MoveControl.Deactivate();
                if (_settings.Status != MoveControlSettingsStatus.Processing)
                {
                    _state = 2;
                }

                break;
            case 2:
                settings = _settings;
                found = false;
                I = settings.ProcessedMapFields.Count;
                while (I > 0 && !found)
                {
                    I -= 1;
                    field = settings.ProcessedMapFields.List[I];
                    if ((field.FieldFlags & MapField.AITargetFlag) != 0)
                    {
                        var mapCoordinate = field.GetMapCoordinate();
                        if (_targets.IndexOfMapCoordinate(mapCoordinate) == -1 && field.GetAITarget(_control, TargetInfo, out data))
                        {
                            target = new MoveTarget(null) { Holder = CreationHolder() };
                            var fieldMapCoordinate = field.GetMapCoordinate();
                            target.Location = fieldMapCoordinate;
                            target.MovePoints = settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
                            target.TransporterId = -1;
                            target.Data = data;
                            if (Mode.Contains(AIMoveTargetSelectorModeElement.CreateMovePaths))
                            {
                                target.MovePath = new MovePath();
                                StrategicMap.Current.MoveControl.Activate(_settings);
                                StrategicMap.Current.MoveControl.CalculatePath(target.MovePath, field.HexX, field.HexY, field.HexLevel);
                                if (target.MovePath.Count != 0)
                                {
                                    _bufferedTargets.AddChild(target);
                                }

                                StrategicMap.Current.MoveControl.Deactivate();
                            }
                            else
                            {
                                _bufferedTargets.AddChild(target);
                            }

                            target.Release();
                            found = true;
                        }
                    }

                    settings.ProcessedMapFields.Delete(I);
                }

                if (_settings.ProcessedMapFields.Count == 0)
                {
                    _bufferedTargets.SortOnMovePoints();
                    _state = 3;
                }

                yield = false;
                break;
            case 3:
                if (!ProcessBufferedTargets() && _state == 3)
                {
                    _state = 10;
                }

                yield = false;
                break;
            case 10:
                if (MovePointsMin > 0 && _transporter != null)
                {
                    _bufferedTargets.Clear();
                    _settings.Reset();
                    this._settings.Player = TargetInfo.Player;
                    this._settings.Mode = this._settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI, MoveModeElement.AvoidArmies);
                    this._settings.SetSourcePosition(SourceLocation.X, SourceLocation.Y, SourceLocation.MapLevel);

                    _settings.MovePointsMax = MovePointsMin;
                    _settings.MovePointTable = MovePointTable;
                    _settings.Units.AddUnits(Units);
                    _transporter.CreateMovePointTable(out _transportMovePointTable);
                    for (int terrain = 0; terrain <= 15; terrain++)
                    {
                        for (int overlay = -1; overlay <= 11; overlay++)
                        {
                            if (_settings.MovePointTable[terrain][overlay - MovementTables.FirstOverlay] == -1 && _transportMovePointTable[terrain][overlay - MovementTables.FirstOverlay] != -1)
                            {
                                _settings.MovePointTable[terrain][overlay - MovementTables.FirstOverlay] = 8;
                            }
                        }
                    }

                    _state = 11;
                }
                else
                {
                    Done();
                }

                yield = false;
                break;
            case 11:
                StrategicMap.Current.MoveControl.Activate(_settings);
                while (StrategicMap.Current.MoveControl.Process() && StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0)
                {
                }

                StrategicMap.Current.MoveControl.Deactivate();
                if (_settings.Status != MoveControlSettingsStatus.Processing)
                {
                    _state = 12;
                }

                break;
            case 12:
                settings = _settings;
                found = false;
                I = settings.ProcessedMapFields.Count;
                while (I > 0 && !found)
                {
                    I -= 1;
                    field = settings.ProcessedMapFields.List[I];
                    if ((field.FieldFlags & MapField.AITargetFlag) != 0)
                    {
                        var mapCoordinate = field.GetMapCoordinate();
                        if (_targets.IndexOfMapCoordinate(mapCoordinate) == -1 && field.GetAITarget(_control, TargetInfo, out data) && MovePointTable[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay] >= 0)
                        {
                            target = new MoveTarget(null) { Holder = CreationHolder() };
                            var fieldMapCoordinate = field.GetMapCoordinate();
                            target.Location = fieldMapCoordinate;
                            target.MovePoints = settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
                            target.TransporterId = _transporter.UnitId;
                            target.Data = data;
                            target.MovePath = new MovePath();
                            StrategicMap.Current.MoveControl.Activate(_settings);
                            StrategicMap.Current.MoveControl.CalculatePath(target.MovePath, field.HexX, field.HexY, field.HexLevel);
                            StrategicMap.Current.MoveControl.Deactivate();
                            _bufferedTargets.AddChild(target);
                            target.Release();
                            found = true;
                        }
                    }

                    settings.ProcessedMapFields.Delete(I);
                }

                if (_settings.ProcessedMapFields.Count == 0)
                {
                    _bufferedTargets.SortOnMovePoints();
                    _state = 13;
                }

                yield = false;
                break;
            case 13:
                if (!ProcessBufferedTransportTargets() && _state == 13)
                {
                    Done();
                }

                break;
        }

        return yield;
    }

    // AIGroup.pas:6677
    public bool Process()
    {
        bool yield;
        try
        {
            yield = false;
            while (!yield && _state != -1 && StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0)
            {
                yield = ProcessStates();
            }
        }
        catch
        {
            _state = -1;
        }

        return _state != -1;
    }

    // AIGroup.pas:6690
    public bool Search(AIGroupControl control)
    {
        bool result;
        if (TargetInfo.Player != -1 && OnTargetFound != null && SourceLocation != MapCoordinate.None && (MovePointsMax > 0 || (MovePointsMin > 0 && _transporter != null)))
        {
            if (MovePointsMax > 0)
            {
                _state = 0;
            }
            else
            {
                _state = 10;
            }
            _control = control;
            Status = AIMoveTargetSelectorStatus.Searching;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static int ListMovementGaps(StrategicMap map, MovePath path, MovePointTable table, Int32List starts, Int32List ends)
    {
        int number = path.Count - 1;
        int number2 = 0;
        bool flag = false;
        while (number > 1)
        {
            number--;
            MapCoordinate mapCoordinate = path[number];
            MapCoordinate mapCoordinate2 = path[number];
            MapCoordinate mapCoordinate3 = path[number];
            MapField field = map.MapContainer.GetField(mapCoordinate3.X, mapCoordinate2.Y, mapCoordinate.MapLevel);
            if (table[(int)field.TerrainType][field.OverlayType - MovementTables.FirstOverlay] == -1)
            {
                if (!flag)
                {
                    number2++;
                    starts?.Add(number);
                    flag = true;
                }
            }
            else if (flag)
            {
                ends?.Add(number);
                flag = false;
            }
        }
        if (flag)
        {
            ends?.Add(1);
        }
        return number2;
    }

    public delegate void MoveTargetFoundEvent(MoveTarget Target, ref bool SearchDone);

    // intf_P5_06.inc:242

    // intf_P5_06.inc:242

}

public enum AIMoveTargetSelectorStatus : byte
{
    Idle = 0, Searching = 1, Done = 2, Failed = 3, Invalidated = 4
}
