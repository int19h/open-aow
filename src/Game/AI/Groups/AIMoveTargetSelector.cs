// Translated from AoWE.pas.
using Aow.Engine.Buffers;
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
        Settings = new MoveControlSettings(null);
        Targets = new MoveTargetList(null);
        BufferedTargets = new MoveTargetList(null);
        Units = new UnitUserList(null);
    }

    public int State;
    public MoveTargetList Targets;
    public AIMoveTargetSelectorStatus Status;
    public MoveControlSettings Settings;
    public Set<AIMoveTargetSelectorModeElement> Mode;
    public MoveTargetList BufferedTargets;
    public MoveTargetFoundEvent OnTargetFound;
    public int ProcessCycles;
    public SByteArray16By16 TransportMovePointTable;
    public MapCoordinate SourceLocation;
    public AITargetInfo TargetInfo;
    public SByteArray16By16 MovePointTable;
    public int MovePointsMax;
    public int MovePointsMin;
    public Troop Transporter;
    public UnitUserList Units;
    public AIGroupControl Control;

    // AIGroup.pas:6384
    public override void Dispose()
    {
        Settings.Release();
        Targets?.Dispose();
        BufferedTargets?.Dispose();
        Units?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:6393
    public void SetTransporter(Troop transporter)
    {
        if (transporter != Transporter)
        {
            if (Transporter != null)
            {
                Transporter.Release();
            }

            Transporter = transporter;
            if (Transporter != null)
            {
                Transporter.AddRef();
            }
        }
    }

    // AIGroup.pas:6403
    public void Reset()
    {
        State = 0;
        Status = AIMoveTargetSelectorStatus.Idle;
        OnTargetFound = null;
        ProcessCycles = 0;
        FillBytes(Bytes.Of(ref MovePointTable, 256), -1);
        TargetInfo = default;
        TargetInfo.Player = -1;
        MovePointsMax = 250;
        MovePointsMin = 100;
        SetTransporter(null);
        SourceLocation.Packed = 16777215;
        Targets.Clear();
        Mode = Set<AIMoveTargetSelectorModeElement>.Of();
        Units.Clear();
    }

    // AIGroup.pas:6421
    public bool SetupArmy(ArmyHexSprite army, byte selection)
    {
        Reset();
        var mapCoordinate = army.GetMapCoordinate();
        SourceLocation.Packed = mapCoordinate;
        Units.AddUnits(army.Army, selection);
        TargetInfo.Player = army.Army.Player;
        if (TargetInfo.Player == 0)
        {
            TargetInfo.IndependentRelation = army.Army.IndependentRelation;
        }

        army.Army.CreateMovePointTable(selection, Bytes.Of(ref MovePointTable));
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
        State = -1;
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
        if (BufferedTargets.Count != 0)
        {
            TriggerTargetFound(BufferedTargets.GetTarget(0));
            var target = BufferedTargets.GetTarget(0);
            Targets.AddChild(target);
            var fBufferedTargetsTarget = BufferedTargets.GetTarget(0);
            BufferedTargets.RemoveChild(fBufferedTargetsTarget);
        }

        return BufferedTargets.Count != 0 && State != -1;
    }

    // AIGroup.pas:6467
    public bool ProcessBufferedTransportTargets()
    {
        MapCoordinate location = default;
        Int32List gaps;
        MoveTarget target;
        bool valid;
        int I;
        if (BufferedTargets.Count != 0)
        {
            gaps = new Int32List();
            target = BufferedTargets.GetTarget(0);
            Transporter.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
            if (ListMovementGaps(AoWHSMap.Current, target.MovePath, MovePointTable, gaps, null) != 0)
            {
                Settings.Reset();
                this.Settings.Player = TargetInfo.Player;
                this.Settings.Mode = this.Settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
                this.Settings.SetSourcePosition(location.X, location.Y, location.MapLevel);

                Settings.MovePointsMax = MovePointsMax;
                Settings.MovePointTable = TransportMovePointTable;
                I = gaps.Count;
                valid = true;
                while (I != 0 && valid)
                {
                    I -= 1;
                    Settings.ResetState();
                    var item = gaps[I];
                    var fMovePathItem = target.MovePath[item];
                    var gapsItem = gaps[I];
                    var item2 = target.MovePath[gapsItem];
                    var item3 = gaps[I];
                    var item4 = target.MovePath[item3];
                    Settings.SetDestinationPosition(item4.X, item2.Y, fMovePathItem.MapLevel);
                    valid = AoWHSMap.Current.MoveControl.CanMove(Settings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast)) != -1;
                }
            }
            else
            {
                valid = true;
            }

            if (valid)
            {
                TriggerTargetFound(target);
                Targets.AddChild(target);
            }

            BufferedTargets.RemoveChild(target);
            gaps?.Dispose();
        }

        return BufferedTargets.Count != 0 && State != -1;
    }

    // AIGroup.pas:6514
    public bool ProcessStates()
    {
        bool yield;
        bool processing;
        bool found;
        int I;
        MoveControlSettings settings;
        AoWMapField field;
        MoveTarget target;
        AITargetData data = default;
        yield = false;
        switch (State)
        {
            case 0:
                if (MovePointsMax != 0)
                {
                    BufferedTargets.Clear();
                    Settings.Reset();
                    this.Settings.Player = TargetInfo.Player;
                    this.Settings.Mode = this.Settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
                    this.Settings.SetSourcePosition(SourceLocation.X, SourceLocation.Y, SourceLocation.MapLevel);

                    Settings.MovePointsMax = MovePointsMax;
                    Settings.MovePointTable = MovePointTable;
                    Settings.Units.AddUnits(Units);
                    State = 1;
                }
                else
                {
                    State = 10;
                }

                yield = false;
                break;
            case 1:
                AoWHSMap.Current.MoveControl.Activate(Settings);
                processing = true;
                while (AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0 && processing)
                {
                    processing = AoWHSMap.Current.MoveControl.Process();
                }

                yield = processing;
                AoWHSMap.Current.MoveControl.Deactivate();
                if (Settings.Status != MoveControlSettingsStatus.Processing)
                {
                    State = 2;
                }

                break;
            case 2:
                settings = Settings;
                found = false;
                I = settings.ProcessedMapFields.Count;
                while (I > 0 && !found)
                {
                    I -= 1;
                    field = (AoWMapField)settings.ProcessedMapFields.List[I];
                    if ((field.FieldFlags & 134217728) != 0)
                    {
                        var mapCoordinate = field.GetMapCoordinate();
                        if (Targets.IndexOfMapCoordinate(mapCoordinate) == -1 && field.GetAITarget(Control, TargetInfo, ref data))
                        {
                            target = new MoveTarget(null);
                            var fieldMapCoordinate = field.GetMapCoordinate();
                            target.Location.Packed = fieldMapCoordinate;
                            target.MovePoints = settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
                            target.TransporterId = -1;
                            target.Data = data;
                            if (Mode.Contains(AIMoveTargetSelectorModeElement.CreateMovePaths))
                            {
                                target.MovePath = new MovePath();
                                AoWHSMap.Current.MoveControl.Activate(Settings);
                                AoWHSMap.Current.MoveControl.CalculatePath(target.MovePath, field.HexX, field.HexY, field.HexLevel);
                                if (target.MovePath.Count != 0)
                                {
                                    BufferedTargets.AddChild(target);
                                }

                                AoWHSMap.Current.MoveControl.Deactivate();
                            }
                            else
                            {
                                BufferedTargets.AddChild(target);
                            }

                            target.Release();
                            found = true;
                        }
                    }

                    settings.ProcessedMapFields.Delete(I);
                }

                if (Settings.ProcessedMapFields.Count == 0)
                {
                    BufferedTargets.SortOnMovePoints();
                    State = 3;
                }

                yield = false;
                break;
            case 3:
                if (!ProcessBufferedTargets() && State == 3)
                {
                    State = 10;
                }

                yield = false;
                break;
            case 10:
                if (MovePointsMin > 0 && Transporter != null)
                {
                    BufferedTargets.Clear();
                    Settings.Reset();
                    this.Settings.Player = TargetInfo.Player;
                    this.Settings.Mode = this.Settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI, MoveModeElement.AvoidArmies);
                    this.Settings.SetSourcePosition(SourceLocation.X, SourceLocation.Y, SourceLocation.MapLevel);

                    Settings.MovePointsMax = MovePointsMin;
                    Settings.MovePointTable = MovePointTable;
                    Settings.Units.AddUnits(Units);
                    Transporter.CreateMovePointTable(ref TransportMovePointTable);
                    for (int terrain = 0; terrain <= 15; terrain++)
                    {
                        for (int overlay = -1; overlay <= 11; overlay++)
                        {
                            if (Settings.MovePointTable[terrain][overlay - -1] == -1 && TransportMovePointTable[terrain][overlay - -1] != -1)
                            {
                                Settings.MovePointTable[terrain][overlay - -1] = 8;
                            }
                        }
                    }

                    State = 11;
                }
                else
                {
                    Done();
                }

                yield = false;
                break;
            case 11:
                AoWHSMap.Current.MoveControl.Activate(Settings);
                while (AoWHSMap.Current.MoveControl.Process() && AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0)
                {
                }

                AoWHSMap.Current.MoveControl.Deactivate();
                if (Settings.Status != MoveControlSettingsStatus.Processing)
                {
                    State = 12;
                }

                break;
            case 12:
                settings = Settings;
                found = false;
                I = settings.ProcessedMapFields.Count;
                while (I > 0 && !found)
                {
                    I -= 1;
                    field = (AoWMapField)settings.ProcessedMapFields.List[I];
                    if ((field.FieldFlags & 134217728) != 0)
                    {
                        var mapCoordinate = field.GetMapCoordinate();
                        if (Targets.IndexOfMapCoordinate(mapCoordinate) == -1 && field.GetAITarget(Control, TargetInfo, ref data) && MovePointTable[field.TerrainType][field.OverlayType - -1] >= 0)
                        {
                            target = new MoveTarget(null);
                            var fieldMapCoordinate = field.GetMapCoordinate();
                            target.Location.Packed = fieldMapCoordinate;
                            target.MovePoints = settings.GetMovePoints(field.HexX, field.HexY, field.HexLevel);
                            target.TransporterId = Transporter.UnitId;
                            target.Data = data;
                            target.MovePath = new MovePath();
                            AoWHSMap.Current.MoveControl.Activate(Settings);
                            AoWHSMap.Current.MoveControl.CalculatePath(target.MovePath, field.HexX, field.HexY, field.HexLevel);
                            AoWHSMap.Current.MoveControl.Deactivate();
                            BufferedTargets.AddChild(target);
                            target.Release();
                            found = true;
                        }
                    }

                    settings.ProcessedMapFields.Delete(I);
                }

                if (Settings.ProcessedMapFields.Count == 0)
                {
                    BufferedTargets.SortOnMovePoints();
                    State = 13;
                }

                yield = false;
                break;
            case 13:
                if (!ProcessBufferedTransportTargets() && State == 13)
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
            while (!yield && State != -1 && AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0)
            {
                yield = ProcessStates();
            }
        }
        catch
        {
            State = -1;
        }

        return State != -1;
    }

    // AIGroup.pas:6690
    public bool Search(AIGroupControl control)
    {
        bool result;
        if (TargetInfo.Player != -1 && OnTargetFound != null && SourceLocation.Packed != 16777215 && (MovePointsMax > 0 || (MovePointsMin > 0 && Transporter != null)))
        {
            if (MovePointsMax > 0)
            {
                State = 0;
            }
            else
            {
                State = 10;
            }

            ProcessCycles = 0;
            Control = control;
            Status = AIMoveTargetSelectorStatus.Searching;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static int ListMovementGaps(AoWHSMap map, MovePath path, SByteArray16By16 table, Int32List starts, Int32List ends)
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
            if (table[(int)field.TerrainType][field.OverlayType - -1] == -1)
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
