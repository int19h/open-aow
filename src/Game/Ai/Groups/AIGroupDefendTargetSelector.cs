// Translated from AGCPlugins.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

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
        FDamageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
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
