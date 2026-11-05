// Translated from AGCPlugins.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.City;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AGCPlugins;

public enum TAIGroupRazeControlModeElement : byte
{
    rcmCannotDefend = 0,
    rcmOnMigrate = 1,
    rcmAlways = 2
}

public enum TAIGroupDefendTargetSelectorModeElement : byte
{
    dtmCalculateMovepathBFS = 0
}

public enum TAIGroupWanderModeElement : byte
{
    wmTransportersAllowed = 0
}

// { ot=1 }
// fields control, mode, damage types, structure factor, city factor
public partial class TAIGroupRazeControl : TAIGroupControlPlugin
{
    public TAIGroupRazeControl(TECustomNode owner) : base(owner)
    { }

    public TAIGroupControl FControl;
    public TSet<TAIGroupRazeControlModeElement> FMode;
    public TSet<TDamageType> FDamageTypes;
    public float FStructureStrengthFactor;
    public float FCityStrengthFactor;
    // AGCPlugins.pas:101
    public override bool Activate(TAIGroupControl Control)
    {
        FState = 0;
        FControl = Control;
        return true;
    }

    // AGCPlugins.pas:133
    public override bool Process()
    {
        bool Result;
        TStructure Structure;
        TBattleFieldInfo Info = default;
        if (FState == 0)
        {
            Structure = (TStructure)AoWHSMap.MapContainer.GetFieldXYL(Unsafe.As<TXYL, int>(ref FControl.FLocation)).FindNoneTransparentHS(typeof(TStructure));
            if (Structure != null && Structure.GetRazeable())
            {
                SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnoreIndependents), FDamageTypes);
                AoWHSMap.RetrieveBattleFieldInfo(FControl.FLocation.X, FControl.FLocation.Y, FControl.FLocation.L, 15, ref Info);
                if (Structure is TCity)
                {
                    Result = ValidateRazeCity((TCity)Structure, Info);
                }
                else
                {
                    Result = ValidateRazeStructure(Structure, Info);
                }

                if (Result)
                {
                    ExecuteRaze(Structure);
                }
            }

            Done();
        }
        else
        {
            Done();
        }

        return false;
    }

    // AGCPlugins.pas:90
    public void Reset()
    {
        FDamageTypes = AGCPlugins.AllDamageTypes - TSet<TDamageType>.Of(TDamageType.dtWall);
        FMode = TSet<TAIGroupRazeControlModeElement>.Of(TAIGroupRazeControlModeElement.rcmCannotDefend);
        FCityStrengthFactor = 2;
        FStructureStrengthFactor = 2;
    }

    // AGCPlugins.pas:98
    public bool Setup(TAIGroupControl Control)
    {
        Reset();
        return true;
    }

    // AGCPlugins.pas:108
    public void ExecuteRaze(TStructure Structure)
    {
        Structure.Raze(FControl.FGroup.FPlayer);
    }

    // AGCPlugins.pas:111
    public bool ValidateRazeCity(TCity City, in TBattleFieldInfo Info)
    {
        bool Result;
        switch (AoWHSMap.FRaces.GetRace(City.FRace).GetPlayerRelation(FControl.FGroup.FPlayer))
        {
            case TStatus.sHostile:
            case TStatus.sWary:
                Result = FMode.Contains(TAIGroupRazeControlModeElement.rcmAlways);
                if (FMode.Contains(TAIGroupRazeControlModeElement.rcmOnMigrate))
                {
                    Result = true;
                }

                if (FMode.Contains(TAIGroupRazeControlModeElement.rcmCannotDefend))
                {
                    Result = Result || Round((double)Info.FriendlyStrength * FCityStrengthFactor) < Info.Strength;
                }

                break;
            default:
                Result = FMode.Contains(TAIGroupRazeControlModeElement.rcmAlways);
                break;
        }

        return Result;
    }

    // AGCPlugins.pas:125
    public bool ValidateRazeStructure(TStructure Structure, in TBattleFieldInfo Info)
    {
        bool Result = false;
        if (FMode.Contains(TAIGroupRazeControlModeElement.rcmAlways))
        {
            Result = true;
        }

        if (FMode.Contains(TAIGroupRazeControlModeElement.rcmCannotDefend) && Round((double)Info.FriendlyStrength * FStructureStrengthFactor) < Info.Strength)
        {
            Result = true;
        }

        return Result;
    }

    // intf_P5_07.inc:218

}

// control, selector, targets, mode, path step, radius, damage, factor
public partial class TAIGroupDefendTargetSelector : TAIGroupControlPlugin
{
    public TAIGroupControl FControl;
    public TAIMoveTargetSelector FSelector;
    public TAIGroupControlTargetList FTargets;
    public TSet<TAIGroupDefendTargetSelectorModeElement> FMode;
    public int FPathStep;
    public int FBattleFieldRadius;
    public TSet<TDamageType> FDamageTypes;
    public float FMovePointsFactor;
    // AGCPlugins.pas:164
    public override void Destroy()
    {
        FSelector.Free();
        FTargets.Free();
        base.Destroy();
    }

    // AGCPlugins.pas:157
    public TAIGroupDefendTargetSelector(TECustomNode owner) : base(owner)
    {
        FSelector = new TAIMoveTargetSelector(null);
        FTargets = new TAIGroupControlTargetList(null);
    }

    // AGCPlugins.pas:245
    public override bool Activate(TAIGroupControl Control)
    {
        FControl = Control;
        FSelector.Search(Control);
        FState = 0;
        return true;
    }

    // AGCPlugins.pas:253
    public override bool Process()
    {
        bool Result;
        switch (FState)
        {
            case 0:
                if (FSelector.FStatus == TAIMoveTargetSelectorStatus.atsSearching)
                {
                    Result = FSelector.Process();
                }
                else
                {
                    Result = false;
                }

                if (!Result)
                {
                    FTargets.SortOnPriority();
                    FState += 1;
                }

                break;
            case 1:
                if (FTargets.Count != 0)
                {
                    FControl.FGroup.SetupTarget(null, TAIGroupTargetStatus.gtsIdle, 0);
                    FControl.SetupME();
                    var targetLocation = FTargets.GetTargets(0);
                    if (Unsafe.As<TXYL, int>(ref targetLocation.FLocation) == Unsafe.As<TXYL, int>(ref FControl.FLocation))
                    {
                        FControl.FMoveExecuter.FMode = FControl.FMoveExecuter.FMode - TSet<TAIMoveExecuterModeElement>.Of(TAIMoveExecuterModeElement.memAdjacent);
                    }

                    var target = FTargets.GetTargets(0);
                    var target2 = FTargets.GetTargets(0);
                    var target3 = FTargets.GetTargets(0);
                    FControl.ExecuteME(target3.FLocation.X, target2.FLocation.Y, target.FLocation.L);
                    FState = 2;
                }
                else
                {
                    FControl.SetupME();
                    var aiMoveExecuter = FControl.FMoveExecuter;
                    aiMoveExecuter.FMode = aiMoveExecuter.FMode - TSet<TAIMoveExecuterModeElement>.Of(TAIMoveExecuterModeElement.memAdjacent);

                    var aiGroupControl = FControl;
                    aiGroupControl.ExecuteME(aiGroupControl.FLocation.X, aiGroupControl.FLocation.Y, aiGroupControl.FLocation.L);

                    FControl.FGroup.SetupTarget(null, TAIGroupTargetStatus.gtsIdle, 0);
                    Done();
                }

                Result = false;
                break;
            case 2:
                Done();
                Result = false;
                break;
            default:
                Result = false;
                break;
        }

        return Result;
    }

    // AGCPlugins.pas:171
    public void Reset()
    {
        FSelector.Reset();
        this.FSelector.FOnTargetFound = TargetFoundEvent;
        this.FSelector.FMovePointsMax = 150;
        this.FSelector.FMovePointsMin = 0;
        this.FSelector.FTargetInfo.Mode = TAITargetMode.tmDefend;

        FPathStep = 7;
        FDamageTypes = AGCPlugins.AllDamageTypes - TSet<TDamageType>.Of(TDamageType.dtWall);
        FBattleFieldRadius = 10;
        FMovePointsFactor = -1;
        FMode = TSet<TAIGroupDefendTargetSelectorModeElement>.Of();
    }

    // AGCPlugins.pas:188
    public int GetMovepathBattleFieldStrength(TMovepath Path)
    {
        int Result;
        int I;
        TBattleFieldInfo Info = default;
        if (Path != null && FPathStep < Path.Count)
        {
            Result = 0;
            I = FPathStep;
            SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(), FDamageTypes);
            Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
            Info.Requests = Info.Requests + TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents);
            while (I < Path.Count)
            {
                Info.Strength = 0;
                var pathL = Path[I];
                var pathY = Path[I];
                var pathX = Path[I];
                AoWHSMap.RetrieveBattleFieldInfo(pathX.X, pathY.Y, pathL.L, FBattleFieldRadius, ref Info);
                if (Result < Info.Strength)
                {
                    Result = Info.Strength;
                }

                I += FPathStep;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AGCPlugins.pas:213
    public void TargetFoundEvent(TMoveTarget Target, ref bool SearchDone)
    {
        TAIGroupControlTarget NewTarget;
        NewTarget = new TAIGroupControlTarget(null);
        NewTarget.FInfo = this.FSelector.FTargetInfo;

        NewTarget.FData = Target.FData;
        NewTarget.FLocation = Target.FLocation;
        NewTarget.FMovePoints = Target.FMovePoints;
        NewTarget.FData.Priority += Round((double)Target.FMovePoints * FMovePointsFactor);
        if (FMode.Contains(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS))
        {
            NewTarget.FPathStrength = GetMovepathBattleFieldStrength(Target.FMovepath);
        }

        if (Target.FTransporterID != -1)
        {
            NewTarget.FTransporterID = Target.FTransporterID;
        }

        if (NewTarget.FPathStrength < FControl.FStrengthMax)
        {
            FTargets.Add(NewTarget);
        }

        NewTarget.Release();
    }

    // AGCPlugins.pas:229
    public bool Setup(TAIGroupControl Control)
    {
        Reset();
        FSelector.FSourceLocation = Control.FLocation;
        FSelector.FMovePointTable = Control.FMovePointTable;
        FSelector.FTargetInfo.Player = Control.FGroup.FPlayer;
        FSelector.FTargetInfo.IndependentRelation = Control.FGroup.FIndependentRelation;
        switch (AoWHSMap.FPlayers.GetPlayers(Control.FGroup.FPlayer).FPlayerType)
        {
            case TPlayerType.ptCPU2:
                FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                break;
            case TPlayerType.ptCPU3:
                FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                break;
            case TPlayerType.ptCPU4:
                FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                break;
            case TPlayerType.ptCPU5:
                FMode = FMode + TSet<TAIGroupDefendTargetSelectorModeElement>.Of(TAIGroupDefendTargetSelectorModeElement.dtmCalculateMovepathBFS);
                break;
        }

        return true;
    }

}

// mode, move limit, move settings, control
public partial class TAIGroupWanderControl : TAIGroupControlPlugin
{
    public TSet<TAIGroupWanderModeElement> FMode;
    public int FMovePointsMax;
    public TMoveControlSettings FMoveSettings;
    public TAIGroupControl FControl;
    // AGCPlugins.pas:303
    public override void Destroy()
    {
        FMoveSettings.Release();
        base.Destroy();
    }

    // AGCPlugins.pas:297
    public TAIGroupWanderControl(TECustomNode owner) : base(owner)
    {
        FMoveSettings = new TMoveControlSettings(null);
    }

    // AGCPlugins.pas:319
    public override bool Activate(TAIGroupControl Control)
    {
        FControl = Control;
        FState = 0;
        return true;
    }

    // AGCPlugins.pas:329
    public override bool Process()
    {
        bool Result = default;
        TAIGroupControl Control = default;
        TMoveControlSettings Settings = default;
        TDoubleItemList Fields = default;
        TAoWMapField Field = default;
        int I = default;
        int TargetWeight = default;
        TXYL Location = default;
        switch (FState)
        {
            case 0:
                Control = FControl;
                FMoveSettings.Reset();
            {
                this.FMoveSettings.FPlayer = Control.FGroup.FPlayer;
                this.FMoveSettings.FMode = this.FMoveSettings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmAI);
                this.FMoveSettings.SetSourcePos(Control.FLocation.X, Control.FLocation.Y, Control.FLocation.L);
            }

            {
                var moveControlSettings = FMoveSettings;
                moveControlSettings.FMovePointsMax = this.FMovePointsMax;
                moveControlSettings.FArmyHS = Control.FArmyHS;
                moveControlSettings.FSelection = Control.FSelection;
                moveControlSettings.FMovePointTable = Control.FMovePointTable;
                moveControlSettings.FMode = moveControlSettings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmLogProcessedMapFields);
                AoWHSMap.FMoveControl.ProcessMove(FMoveSettings);
            }

            {
                var group = Control.FGroup;
                // { Original reloads the current control. }
                if (group.FTargetMode == 1 && this.FControl.FGroup.FTargetXYL.X > 0 && FMoveSettings.GetMovePoints(group.FTargetXYL.X, group.FTargetXYL.Y, group.FTargetXYL.L) != 16777215 && Unsafe.As<TXYL, int>(ref this.FControl.FGroup.FTargetXYL) != Unsafe.As<TXYL, int>(ref Control.FLocation))
                {
                    FState = 2;
                }
                else
                {
                    FState = 1;
                }
            }

                Result = false;
                break;
            case 1:
                Settings = FMoveSettings;
                Fields = new TDoubleItemList();
                if (Settings.FProcessedMapFields.Count > 40)
                {
                    I = Settings.FProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if ((I & 3) == 3)
                        {
                            Field = (TAoWMapField)Settings.FProcessedMapFields[I];
                            if (FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx) != 16777215)
                            {
                                var movePoints = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                Fields.AddAB(ObjectToInt(Field), movePoints);
                            }
                        }
                    }
                }
                else
                {
                    I = Settings.FProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        Field = (TAoWMapField)Settings.FProcessedMapFields[I];
                        if (FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx) != 16777215)
                        {
                            var movePoints = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                            Fields.AddAB(ObjectToInt(Field), movePoints);
                        }
                    }
                }

                if (Fields.Count > 0)
                {
                    Fields.Sort(AGCPlugins.CompareWanderFields);
                    Field = null;
                    I = 0;
                    while (Fields.Count != 0 && Field == null)
                    {
                        I = AoWHSMap.Random(Round((double)Fields.Count * 0.1) + 1);
                        Field = ReferenceFromId<TAoWMapField>(Fields[I].A);
                        if (Field.GetPlayer() != -1)
                        {
                            Fields.Delete(I);
                            Field = null;
                        }
                    }

                    if (Field != null)
                    {
                        // { Original reads the list item before GetXYL. }
                        TargetWeight = Fields[I].B;
                        Location.XYL = Field.GetXYL();
                        FControl.FGroup.SetupTargetEx(Location, TargetWeight, TAIGroupTargetStatus.gtsExecute, 1, -1, 0, 0);
                        FState += 1;
                    }
                    else
                    {
                        Done();
                    }

                    Fields.Free();
                }
                else
                {
                    // { Original leaks Fields when no reachable field was collected. }
                    Done();
                }

                Result = false;
                break;
            case 2:
            {
                var aiGroupControl = FControl;
                aiGroupControl.SetupME();
                aiGroupControl.ExecuteME(aiGroupControl.FGroup.FTargetXYL.X, aiGroupControl.FGroup.FTargetXYL.Y, aiGroupControl.FGroup.FTargetXYL.L);
            }

                FState += 1;
                Result = false;
                break;
            case 3:
                Done();
                Result = false;
                break;
            default:
                Result = false;
                break;
        }

        return Result;
    }

    // AGCPlugins.pas:309
    public void Reset()
    {
        FMode = TSet<TAIGroupWanderModeElement>.Of();
        FMovePointsMax = 75;
        FMoveSettings.Reset();
    }

    // AGCPlugins.pas:316
    public bool Setup(TAIGroupControl Control)
    {
        Reset();
        return true;
    }

}

public static partial class AGCPlugins
{
    public static TSet<TDamageType> AllDamageTypes = TSet<TDamageType>.FromMask(1023UL);
    // AGCPlugins.pas:326
    public static int CompareWanderFields(in TDoubleItem Item1, in TDoubleItem Item2) => Item2.B - Item1.B;

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
