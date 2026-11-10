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

namespace Aow.Game.Ai.Groups;

// state, targets, settings, buffer, transporter movement table, transporter, control
public partial class AIMoveTargetSelector : EObject
{
    // AIGroup.pas:6375
    public AIMoveTargetSelector(ECustomNode owner) : base(owner)
    {
        FSettings = new MoveControlSettings(null);
        FTargets = new MoveTargetList(null);
        FBufferedTargets = new MoveTargetList(null);
        FUnits = new UnitUserList(null);
    }

    public int FState;
    public MoveTargetList FTargets;
    public Status FStatus;
    public MoveControlSettings FSettings;
    public Set<AIMoveTargetSelectorModeElement> FMode;
    public MoveTargetList FBufferedTargets;
    public MoveTargetFoundEvent FOnTargetFound;
    public int FProcessCycles;
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 FTransportMovePointTable;
    public XYL FSourceLocation;
    public AITargetInfo FTargetInfo;
    public FixedArray_Aow_Units_FixedArray_sbyte_16_16 FMovePointTable;
    public int FMovePointsMax;
    public int FMovePointsMin;
    public AbstractUnit FTransporter;
    public UnitUserList FUnits;
    public AIGroupControl FControl;

    // AIGroup.pas:6384
    public override void Dispose()
    {
        FSettings.Release();
        FTargets?.Dispose();
        FBufferedTargets?.Dispose();
        FUnits?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:6393
    public void SetTransporter(AbstractUnit Transporter)
    {
        if (Transporter != FTransporter)
        {
            if (FTransporter != null)
            {
                FTransporter.Release();
            }

            FTransporter = Transporter;
            if (FTransporter != null)
            {
                FTransporter.AddRef();
            }
        }
    }

    // AIGroup.pas:6403
    public void Reset()
    {
        FState = 0;
        FStatus = Status.Idle;
        FOnTargetFound = null;
        FProcessCycles = 0;
        FillBytes(Bytes.Of(ref FMovePointTable, 256), -1);
        FTargetInfo = default;
        FTargetInfo.Player = -1;
        FMovePointsMax = 250;
        FMovePointsMin = 100;
        SetTransporter(null);
        FSourceLocation.Packed = 16777215;
        FTargets.Clear();
        FMode = Set<AIMoveTargetSelectorModeElement>.Of();
        FUnits.Clear();
    }

    // AIGroup.pas:6421
    public bool SetupArmy(ArmyHS Army, byte Selection)
    {
        Reset();
        var xyl = Army.GetXYL();
        FSourceLocation.Packed = xyl;
        FUnits.AddUnitsEx(Army.FArmy, Selection);
        FTargetInfo.Player = Army.FArmy.FPlayer;
        if (FTargetInfo.Player == 0)
        {
            FTargetInfo.IndependentRelation = Army.FArmy.FIndependentRelation;
        }

        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref FMovePointTable));
        return true;
    }

    // AIGroup.pas:6432
    public void SetupTransport(AbstractUnit Transporter, bool ResetSettings)
    {
        if (ResetSettings)
        {
            Reset();
        }

        SetTransporter(Transporter);
        FMovePointsMax = 0;
    }

    // AIGroup.pas:6439
    public void Done()
    {
        FState = -1;
        FStatus = Status.Done;
    }

    // AIGroup.pas:6445
    public void TriggerTargetFound(MoveTarget Target)
    {
        bool SearchDone;
        if (FOnTargetFound != null)
        {
            SearchDone = false;
            FOnTargetFound(Target, ref SearchDone);
            if (SearchDone)
            {
                Done();
            }
        }
    }

    // AIGroup.pas:6456
    public bool ProcessBufferedTargets()
    {
        if (FBufferedTargets.Count != 0)
        {
            TriggerTargetFound(FBufferedTargets.GetTarget(0));
            var target = FBufferedTargets.GetTarget(0);
            FTargets.AddChild(target);
            var fBufferedTargetsTarget = FBufferedTargets.GetTarget(0);
            FBufferedTargets.RemoveChild(fBufferedTargetsTarget);
        }

        return FBufferedTargets.Count != 0 && FState != -1;
    }

    // AIGroup.pas:6467
    public bool ProcessBufferedTransportTargets()
    {
        XYL Location = default;
        IntegerList Gaps;
        MoveTarget Target;
        bool Valid;
        int I;
        if (FBufferedTargets.Count != 0)
        {
            Gaps = new IntegerList();
            Target = FBufferedTargets.GetTarget(0);
            FTransporter.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
            if (ListMovementGaps(AoWHSMap.Current, Target.FMovepath, FMovePointTable, Gaps, null) != 0)
            {
                FSettings.Reset();
                this.FSettings.FPlayer = FTargetInfo.Player;
                this.FSettings.FMode = this.FSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
                this.FSettings.SetSourcePos(Location.X, Location.Y, Location.L);

                FSettings.FMovePointsMax = FMovePointsMax;
                FSettings.FMovePointTable = FTransportMovePointTable;
                I = Gaps.Count;
                Valid = true;
                while (I != 0 && Valid)
                {
                    I -= 1;
                    FSettings.ResetState();
                    var item = Gaps[I];
                    var fMovepathItem = Target.FMovepath[item];
                    var gapsItem = Gaps[I];
                    var item2 = Target.FMovepath[gapsItem];
                    var item3 = Gaps[I];
                    var item4 = Target.FMovepath[item3];
                    FSettings.SetDestinationPos(item4.X, item2.Y, fMovepathItem.L);
                    Valid = AoWHSMap.Current.FMoveControl.CanMove(FSettings, Set<CanMoveModeElement>.Of(CanMoveModeElement.Fast)) != -1;
                }
            }
            else
            {
                Valid = true;
            }

            if (Valid)
            {
                TriggerTargetFound(Target);
                FTargets.AddChild(Target);
            }

            FBufferedTargets.RemoveChild(Target);
            Gaps?.Dispose();
        }

        return FBufferedTargets.Count != 0 && FState != -1;
    }

    // AIGroup.pas:6514
    public bool ProcessStates()
    {
        bool Yield;
        bool Processing;
        bool Found;
        int I;
        MoveControlSettings Settings;
        AoWMapField Field;
        MoveTarget Target;
        AITargetData Data = default;
        Yield = false;
        switch (FState)
        {
            case 0:
                if (FMovePointsMax != 0)
                {
                    FBufferedTargets.Clear();
                    FSettings.Reset();
                    this.FSettings.FPlayer = FTargetInfo.Player;
                    this.FSettings.FMode = this.FSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
                    this.FSettings.SetSourcePos(FSourceLocation.X, FSourceLocation.Y, FSourceLocation.L);

                    FSettings.FMovePointsMax = FMovePointsMax;
                    FSettings.FMovePointTable = FMovePointTable;
                    FSettings.FUnits.AddUnits(FUnits);
                    FState = 1;
                }
                else
                {
                    FState = 10;
                }

                Yield = false;
                break;
            case 1:
                AoWHSMap.Current.FMoveControl.Activate(FSettings);
                Processing = true;
                while (AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0 && Processing)
                {
                    Processing = AoWHSMap.Current.FMoveControl.Process();
                }

                Yield = Processing;
                AoWHSMap.Current.FMoveControl.Deactivate();
                if (FSettings.FStatus != MoveControlSettings.Status.Processing)
                {
                    FState = 2;
                }

                break;
            case 2:
                Settings = FSettings;
                Found = false;
                I = Settings.FProcessedMapFields.Count;
                while (I > 0 && !Found)
                {
                    I -= 1;
                    Field = (AoWMapField)Settings.FProcessedMapFields.List[I];
                    if ((Field.FFieldFlags & 134217728) != 0)
                    {
                        var xyl = Field.GetXYL();
                        if (FTargets.IndexOfXYL(xyl) == -1 && Field.GetAITarget(FControl, FTargetInfo, ref Data))
                        {
                            Target = new MoveTarget(null);
                            var fieldXYL = Field.GetXYL();
                            Target.FLocation.Packed = fieldXYL;
                            Target.FMovePoints = Settings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                            Target.FTransporterID = -1;
                            Target.FData = Data;
                            if (FMode.Contains(AIMoveTargetSelectorModeElement.CreateMovepaths))
                            {
                                Target.FMovepath = new Movepath();
                                AoWHSMap.Current.FMoveControl.Activate(FSettings);
                                AoWHSMap.Current.FMoveControl.CalculatePathEx(Target.FMovepath, Field.Xhx, Field.Yhx, Field.Lhx);
                                if (Target.FMovepath.Count != 0)
                                {
                                    FBufferedTargets.AddChild(Target);
                                }

                                AoWHSMap.Current.FMoveControl.Deactivate();
                            }
                            else
                            {
                                FBufferedTargets.AddChild(Target);
                            }

                            Target.Release();
                            Found = true;
                        }
                    }

                    Settings.FProcessedMapFields.Delete(I);
                }

                if (FSettings.FProcessedMapFields.Count == 0)
                {
                    FBufferedTargets.SortOnMovePoints();
                    FState = 3;
                }

                Yield = false;
                break;
            case 3:
                if (!ProcessBufferedTargets() && FState == 3)
                {
                    FState = 10;
                }

                Yield = false;
                break;
            case 10:
                if (FMovePointsMin > 0 && FTransporter != null)
                {
                    FBufferedTargets.Clear();
                    FSettings.Reset();
                    this.FSettings.FPlayer = FTargetInfo.Player;
                    this.FSettings.FMode = this.FSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI, MoveModeElement.AvoidArmies);
                    this.FSettings.SetSourcePos(FSourceLocation.X, FSourceLocation.Y, FSourceLocation.L);

                    FSettings.FMovePointsMax = FMovePointsMin;
                    FSettings.FMovePointTable = FMovePointTable;
                    FSettings.FUnits.AddUnits(FUnits);
                    FTransporter.CreateMovePointTable(ref FTransportMovePointTable);
                    for (int Terrain = 0; Terrain <= 15; Terrain++)
                    {
                        for (int Overlay = -1; Overlay <= 11; Overlay++)
                        {
                            if (FSettings.FMovePointTable[Terrain][Overlay - -1] == -1 && FTransportMovePointTable[Terrain][Overlay - -1] != -1)
                            {
                                FSettings.FMovePointTable[Terrain][Overlay - -1] = 8;
                            }
                        }
                    }

                    FState = 11;
                }
                else
                {
                    Done();
                }

                Yield = false;
                break;
            case 11:
                AoWHSMap.Current.FMoveControl.Activate(FSettings);
                while (AoWHSMap.Current.FMoveControl.Process() && AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0)
                {
                }

                AoWHSMap.Current.FMoveControl.Deactivate();
                if (FSettings.FStatus != MoveControlSettings.Status.Processing)
                {
                    FState = 12;
                }

                break;
            case 12:
                Settings = FSettings;
                Found = false;
                I = Settings.FProcessedMapFields.Count;
                while (I > 0 && !Found)
                {
                    I -= 1;
                    Field = (AoWMapField)Settings.FProcessedMapFields.List[I];
                    if ((Field.FFieldFlags & 134217728) != 0)
                    {
                        var xyl = Field.GetXYL();
                        if (FTargets.IndexOfXYL(xyl) == -1 && Field.GetAITarget(FControl, FTargetInfo, ref Data) && FMovePointTable[Field.TerrainType][Field.OverlayType - -1] >= 0)
                        {
                            Target = new MoveTarget(null);
                            var fieldXYL = Field.GetXYL();
                            Target.FLocation.Packed = fieldXYL;
                            Target.FMovePoints = Settings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                            Target.FTransporterID = FTransporter.FUnitID;
                            Target.FData = Data;
                            Target.FMovepath = new Movepath();
                            AoWHSMap.Current.FMoveControl.Activate(FSettings);
                            AoWHSMap.Current.FMoveControl.CalculatePathEx(Target.FMovepath, Field.Xhx, Field.Yhx, Field.Lhx);
                            AoWHSMap.Current.FMoveControl.Deactivate();
                            FBufferedTargets.AddChild(Target);
                            Target.Release();
                            Found = true;
                        }
                    }

                    Settings.FProcessedMapFields.Delete(I);
                }

                if (FSettings.FProcessedMapFields.Count == 0)
                {
                    FBufferedTargets.SortOnMovePoints();
                    FState = 13;
                }

                Yield = false;
                break;
            case 13:
                if (!ProcessBufferedTransportTargets() && FState == 13)
                {
                    Done();
                }

                break;
        }

        return Yield;
    }

    // AIGroup.pas:6677
    public bool Process()
    {
        bool Yield;
        try
        {
            Yield = false;
            while (!Yield && FState != -1 && AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0)
            {
                Yield = ProcessStates();
            }
        }
        catch
        {
            FState = -1;
        }

        return FState != -1;
    }

    // AIGroup.pas:6690
    public bool Search(AIGroupControl Control)
    {
        bool Result;
        if (FTargetInfo.Player != -1 && FOnTargetFound != null && FSourceLocation.Packed != 16777215 && (FMovePointsMax > 0 || (FMovePointsMin > 0 && FTransporter != null)))
        {
            if (FMovePointsMax > 0)
            {
                FState = 0;
            }
            else
            {
                FState = 10;
            }

            FProcessCycles = 0;
            FControl = Control;
            FStatus = Status.Searching;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static int ListMovementGaps(AoWHSMap Map, Movepath Path, FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table, IntegerList Starts, IntegerList Ends)
    {
        int num = Path.Count - 1;
        int num2 = 0;
        bool flag = false;
        while (num > 1)
        {
            num--;
            XYL tXYL = Path[num];
            XYL tXYL2 = Path[num];
            XYL tXYL3 = Path[num];
            MapField field = Map.MapContainer.GetField(tXYL3.X, tXYL2.Y, tXYL.L);
            if (Table[(int)field.TerrainType][field.OverlayType - -1] == -1)
            {
                if (!flag)
                {
                    num2++;
                    Starts?.Add(num);
                    flag = true;
                }
            }
            else if (flag)
            {
                Ends?.Add(num);
                flag = false;
            }
        }
        if (flag)
        {
            Ends?.Add(1);
        }
        return num2;
    }

    public delegate void MoveTargetFoundEvent(MoveTarget Target, ref bool SearchDone);

    public enum Status : byte
    {
        Idle = 0, Searching = 1, Done = 2, Failed = 3, Invalidated = 4
    }

    // intf_P5_06.inc:242

    // intf_P5_06.inc:242

}
