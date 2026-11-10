// Translated from AGCPlugins.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

public enum AIGroupRazeControlModeElement : byte
{
    CannotDefend = 0,
    OnMigrate = 1,
    Always = 2
}

// { ot=1 }
// fields control, mode, damage types, structure factor, city factor
public partial class AIGroupRazeControl : AIGroupControlPlugin
{
    public AIGroupRazeControl(ECustomNode owner) : base(owner)
    { }

    public AIGroupControl FControl;
    public Set<AIGroupRazeControlModeElement> FMode;
    public Set<DamageType> FDamageTypes;
    public float FStructureStrengthFactor;
    public float FCityStrengthFactor;

    // AGCPlugins.pas:101
    public override bool Activate(AIGroupControl Control)
    {
        FState = 0;
        FControl = Control;
        return true;
    }

    // AGCPlugins.pas:133
    public override bool Process()
    {
        bool Result;
        Structure Structure;
        BattleFieldInfo Info = default;
        if (FState == 0)
        {
            Structure = (Structure)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref FControl.FLocation)).FindNoneTransparentHS(typeof(Structure));
            if (Structure != null && Structure.GetRazeable())
            {
                BattleFieldInfo.SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnoreIndependents), FDamageTypes);
                AoWHSMap.Current.RetrieveBattleFieldInfo(FControl.FLocation.X, FControl.FLocation.Y, FControl.FLocation.L, 15, ref Info);
                if (Structure is City)
                {
                    Result = ValidateRazeCity((City)Structure, Info);
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
        FDamageTypes = Magic.Abilities.AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        FMode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.CannotDefend);
        FCityStrengthFactor = 2;
        FStructureStrengthFactor = 2;
    }

    // AGCPlugins.pas:98
    public bool Setup(AIGroupControl Control)
    {
        Reset();
        return true;
    }

    // AGCPlugins.pas:108
    public void ExecuteRaze(Structure Structure)
    {
        Structure.Raze(FControl.FGroup.FPlayer);
    }

    // AGCPlugins.pas:111
    public bool ValidateRazeCity(City City, in BattleFieldInfo Info)
    {
        bool Result;
        switch (AoWHSMap.Current.FRaces.GetRace(City.FRace).GetPlayerRelation(FControl.FGroup.FPlayer))
        {
            case Status.Hostile:
            case Status.Wary:
                Result = FMode.Contains(AIGroupRazeControlModeElement.Always);
                if (FMode.Contains(AIGroupRazeControlModeElement.OnMigrate))
                {
                    Result = true;
                }

                if (FMode.Contains(AIGroupRazeControlModeElement.CannotDefend))
                {
                    Result = Result || Round((double)Info.FriendlyStrength * FCityStrengthFactor) < Info.Strength;
                }

                break;
            default:
                Result = FMode.Contains(AIGroupRazeControlModeElement.Always);
                break;
        }

        return Result;
    }

    // AGCPlugins.pas:125
    public bool ValidateRazeStructure(Structure Structure, in BattleFieldInfo Info)
    {
        bool Result = false;
        if (FMode.Contains(AIGroupRazeControlModeElement.Always))
        {
            Result = true;
        }

        if (FMode.Contains(AIGroupRazeControlModeElement.CannotDefend) && Round((double)Info.FriendlyStrength * FStructureStrengthFactor) < Info.Strength)
        {
            Result = true;
        }

        return Result;
    }

    // intf_P5_07.inc:218

}

// control, selector, targets, mode, path step, radius, damage, factor
public partial class AIGroupDefendTargetSelector : AIGroupControlPlugin
{
    public AIGroupControl FControl;
    public AIMoveTargetSelector FSelector;
    public AIGroupControlTargetList FTargets;
    public Set<ModeElement> FMode;
    public int FPathStep;
    public int FBattleFieldRadius;
    public Set<DamageType> FDamageTypes;
    public float FMovePointsFactor;

    // AGCPlugins.pas:164
    public override void Dispose()
    {
        FSelector?.Dispose();
        FTargets?.Dispose();
        base.Dispose();
    }

    // AGCPlugins.pas:157
    public AIGroupDefendTargetSelector(ECustomNode owner) : base(owner)
    {
        FSelector = new AIMoveTargetSelector(null);
        FTargets = new AIGroupControlTargetList(null);
    }

    // AGCPlugins.pas:245
    public override bool Activate(AIGroupControl Control)
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
                if (FSelector.FStatus == AIMoveTargetSelector.Status.Searching)
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
                    FControl.FGroup.SetupTarget(null, AIGroupTargetStatus.Idle, 0);
                    FControl.SetupME();
                    var targetLocation = FTargets.GetTargets(0);
                    if (Unsafe.As<XYL, int>(ref targetLocation.FLocation) == Unsafe.As<XYL, int>(ref FControl.FLocation))
                    {
                        FControl.FMoveExecuter.FMode = FControl.FMoveExecuter.FMode - Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent);
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
                    aiMoveExecuter.FMode = aiMoveExecuter.FMode - Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent);

                    var aiGroupControl = FControl;
                    aiGroupControl.ExecuteME(aiGroupControl.FLocation.X, aiGroupControl.FLocation.Y, aiGroupControl.FLocation.L);

                    FControl.FGroup.SetupTarget(null, AIGroupTargetStatus.Idle, 0);
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
        this.FSelector.FTargetInfo.Mode = AITargetMode.Defend;

        FPathStep = 7;
        FDamageTypes = Magic.Abilities.AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        FBattleFieldRadius = 10;
        FMovePointsFactor = -1;
        FMode = Set<ModeElement>.Of();
    }

    // AGCPlugins.pas:188
    public int GetMovepathBattleFieldStrength(Movepath Path)
    {
        int Result;
        int I;
        BattleFieldInfo Info = default;
        if (Path != null && FPathStep < Path.Count)
        {
            Result = 0;
            I = FPathStep;
            BattleFieldInfo.SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), FDamageTypes);
            Info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            Info.Requests = Info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            while (I < Path.Count)
            {
                Info.Strength = 0;
                var pathL = Path[I];
                var pathY = Path[I];
                var pathX = Path[I];
                AoWHSMap.Current.RetrieveBattleFieldInfo(pathX.X, pathY.Y, pathL.L, FBattleFieldRadius, ref Info);
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
    public void TargetFoundEvent(MoveTarget Target, ref bool SearchDone)
    {
        AIGroupControlTarget NewTarget;
        NewTarget = new AIGroupControlTarget(null);
        NewTarget.FInfo = this.FSelector.FTargetInfo;

        NewTarget.FData = Target.FData;
        NewTarget.FLocation = Target.FLocation;
        NewTarget.FMovePoints = Target.FMovePoints;
        NewTarget.FData.Priority += Round((double)Target.FMovePoints * FMovePointsFactor);
        if (FMode.Contains(ModeElement.CalculateMovepathBFS))
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
    public bool Setup(AIGroupControl Control)
    {
        Reset();
        FSelector.FSourceLocation = Control.FLocation;
        FSelector.FMovePointTable = Control.FMovePointTable;
        FSelector.FTargetInfo.Player = Control.FGroup.FPlayer;
        FSelector.FTargetInfo.IndependentRelation = Control.FGroup.FIndependentRelation;
        switch (AoWHSMap.Current.FPlayers.GetPlayers(Control.FGroup.FPlayer).FPlayerType)
        {
            case PlayerType.CPU2:
                FMode = FMode + Set<ModeElement>.Of(ModeElement.CalculateMovepathBFS);
                break;
            case PlayerType.CPU3:
                FMode = FMode + Set<ModeElement>.Of(ModeElement.CalculateMovepathBFS);
                break;
            case PlayerType.CPU4:
                FMode = FMode + Set<ModeElement>.Of(ModeElement.CalculateMovepathBFS);
                break;
            case PlayerType.CPU5:
                FMode = FMode + Set<ModeElement>.Of(ModeElement.CalculateMovepathBFS);
                break;
        }

        return true;
    }

    public enum ModeElement : byte
    {
        CalculateMovepathBFS = 0
    }

}

// mode, move limit, move settings, control
public partial class AIGroupWanderControl : AIGroupControlPlugin
{
    public Set<AIGroupWanderModeElement> FMode;
    public int FMovePointsMax;
    public MoveControlSettings FMoveSettings;
    public AIGroupControl FControl;

    // AGCPlugins.pas:303
    public override void Dispose()
    {
        FMoveSettings.Release();
        base.Dispose();
    }

    // AGCPlugins.pas:297
    public AIGroupWanderControl(ECustomNode owner) : base(owner)
    {
        FMoveSettings = new MoveControlSettings(null);
    }

    // AGCPlugins.pas:319
    public override bool Activate(AIGroupControl Control)
    {
        FControl = Control;
        FState = 0;
        return true;
    }

    // AGCPlugins.pas:329
    public override bool Process()
    {
        bool Result = default;
        AIGroupControl Control = default;
        MoveControlSettings Settings = default;
        DoubleItemList<AoWMapField> Fields = default;
        AoWMapField Field = default;
        int I = default;
        int TargetWeight = default;
        XYL Location = default;
        switch (FState)
        {
            case 0:
                Control = FControl;
                FMoveSettings.Reset();
                {
                    this.FMoveSettings.FPlayer = Control.FGroup.FPlayer;
                    this.FMoveSettings.FMode = this.FMoveSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.AI);
                    this.FMoveSettings.SetSourcePos(Control.FLocation.X, Control.FLocation.Y, Control.FLocation.L);
                }

                {
                    var moveControlSettings = FMoveSettings;
                    moveControlSettings.FMovePointsMax = this.FMovePointsMax;
                    moveControlSettings.FArmyHS = Control.FArmyHS;
                    moveControlSettings.FSelection = Control.FSelection;
                    moveControlSettings.FMovePointTable = Control.FMovePointTable;
                    moveControlSettings.FMode = moveControlSettings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields);
                    AoWHSMap.Current.FMoveControl.ProcessMove(FMoveSettings);
                }

                {
                    var group = Control.FGroup;
                    // { Original reloads the current control. }
                    if (group.FTargetMode == 1 && this.FControl.FGroup.FTargetXYL.X > 0 && FMoveSettings.GetMovePoints(group.FTargetXYL.X, group.FTargetXYL.Y, group.FTargetXYL.L) != 16777215 && Unsafe.As<XYL, int>(ref this.FControl.FGroup.FTargetXYL) != Unsafe.As<XYL, int>(ref Control.FLocation))
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
                Fields = new DoubleItemList<AoWMapField>();
                if (Settings.FProcessedMapFields.Count > 40)
                {
                    I = Settings.FProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if ((I & 3) == 3)
                        {
                            Field = (AoWMapField)Settings.FProcessedMapFields[I];
                            if (FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx) != 16777215)
                            {
                                var movePoints = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                                Fields.AddAB(Field, movePoints);
                            }
                        }
                    }
                }
                else
                {
                    I = Settings.FProcessedMapFields.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        Field = (AoWMapField)Settings.FProcessedMapFields[I];
                        if (FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx) != 16777215)
                        {
                            var movePoints = FMoveSettings.GetMovePoints(Field.Xhx, Field.Yhx, Field.Lhx);
                            Fields.AddAB(Field, movePoints);
                        }
                    }
                }

                if (Fields.Count > 0)
                {
                    Fields.Sort(CompareWanderFields);
                    Field = null;
                    I = 0;
                    while (Fields.Count != 0 && Field == null)
                    {
                        I = AoWHSMap.Current.Random(Round((double)Fields.Count * 0.1) + 1);
                        Field = Fields[I].A;
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
                        Location.Packed = Field.GetXYL();
                        FControl.FGroup.SetupTargetEx(Location, TargetWeight, AIGroupTargetStatus.Execute, 1, -1, 0, 0);
                        FState += 1;
                    }
                    else
                    {
                        Done();
                    }

                    Fields?.Dispose();
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
        FMode = Set<AIGroupWanderModeElement>.Of();
        FMovePointsMax = 75;
        FMoveSettings.Reset();
    }

    // AGCPlugins.pas:316
    public bool Setup(AIGroupControl Control)
    {
        Reset();
        return true;
    }

    private static int CompareWanderFields(in DoubleItem<AoWMapField> Item1, in DoubleItem<AoWMapField> Item2)
    {
        return Item2.B - Item1.B;
    }

    public enum AIGroupWanderModeElement : byte
    {
        TransportersAllowed = 0
    }

}
