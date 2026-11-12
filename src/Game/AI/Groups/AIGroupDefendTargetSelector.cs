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

namespace Aow.Game.AI.Groups;

// control, selector, targets, mode, path step, radius, damage, factor
public partial class AIGroupDefendTargetSelector : AIGroupTask
{
    public AIGroupControl Control;
    public AIMoveTargetSelector Selector;
    public AIGroupControlTargetList Targets;
    public Set<ModeElement> Mode;
    public int PathStep;
    public int BattleFieldRadius;
    public Set<DamageType> DamageTypes;
    public float MovePointsFactor;

    // AGCPlugins.pas:164
    public override void Dispose()
    {
        Selector?.Dispose();
        Targets?.Dispose();
        base.Dispose();
    }

    // AGCPlugins.pas:157
    public AIGroupDefendTargetSelector(ECustomNode owner) : base(owner)
    {
        Selector = new AIMoveTargetSelector(null);
        Targets = new AIGroupControlTargetList(null);
    }

    // AGCPlugins.pas:245
    public override bool Activate(AIGroupControl control)
    {
        Control = control;
        Selector.Search(control);
        State = 0;
        return true;
    }

    // AGCPlugins.pas:253
    public override bool Process()
    {
        bool result;
        switch (State)
        {
            case 0:
                if (Selector.Status == AIMoveTargetSelectorStatus.Searching)
                {
                    result = Selector.Process();
                }
                else
                {
                    result = false;
                }

                if (!result)
                {
                    Targets.SortOnPriority();
                    State += 1;
                }

                break;
            case 1:
                if (Targets.Count != 0)
                {
                    Control.Group.SetupTarget(null, AIGroupTargetStatus.Idle, 0);
                    Control.SetupME();
                    var targetLocation = Targets.GetTargets(0);
                    if (Unsafe.As<MapCoordinate, int>(ref targetLocation.Location) == Unsafe.As<MapCoordinate, int>(ref Control.Location))
                    {
                        Control.MoveExecutor.Mode = Control.MoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);
                    }

                    var target = Targets.GetTargets(0);
                    var target2 = Targets.GetTargets(0);
                    var target3 = Targets.GetTargets(0);
                    Control.ExecuteME(target3.Location.X, target2.Location.Y, target.Location.MapLevel);
                    State = 2;
                }
                else
                {
                    Control.SetupME();
                    var aiMoveExecutor = Control.MoveExecutor;
                    aiMoveExecutor.Mode = aiMoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);

                    var aiGroupControl = Control;
                    aiGroupControl.ExecuteME(aiGroupControl.Location.X, aiGroupControl.Location.Y, aiGroupControl.Location.MapLevel);

                    Control.Group.SetupTarget(null, AIGroupTargetStatus.Idle, 0);
                    Done();
                }

                result = false;
                break;
            case 2:
                Done();
                result = false;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    // AGCPlugins.pas:171
    public void Reset()
    {
        Selector.Reset();
        this.Selector.OnTargetFound = TargetFoundEvent;
        this.Selector.MovePointsMax = 150;
        this.Selector.MovePointsMin = 0;
        this.Selector.TargetInfo.Mode = AITargetMode.Defend;

        PathStep = 7;
        DamageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        BattleFieldRadius = 10;
        MovePointsFactor = -1;
        Mode = Set<ModeElement>.Of();
    }

    // AGCPlugins.pas:188
    public int GetMovePathBattleFieldStrength(MovePath path)
    {
        int result;
        int I;
        BattleFieldInfo info = default;
        if (path != null && PathStep < path.Count)
        {
            result = 0;
            I = PathStep;
            BattleFieldInfo.SetupBattleFieldInfo(ref info, Control.Group.Player, Control.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), DamageTypes);
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            while (I < path.Count)
            {
                info.Strength = 0;
                var pathL = path[I];
                var pathY = path[I];
                var pathX = path[I];
                AoWHSMap.Current.RetrieveBattleFieldInfo(pathX.X, pathY.Y, pathL.MapLevel, BattleFieldRadius, ref info);
                if (result < info.Strength)
                {
                    result = info.Strength;
                }

                I += PathStep;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AGCPlugins.pas:213
    public void TargetFoundEvent(MoveTarget target, ref bool searchDone)
    {
        AIGroupControlTarget newTarget;
        newTarget = new AIGroupControlTarget(null);
        newTarget.Info = this.Selector.TargetInfo;

        newTarget.Data = target.Data;
        newTarget.Location = target.Location;
        newTarget.MovePoints = target.MovePoints;
        newTarget.Data.Priority += Round((double)target.MovePoints * MovePointsFactor);
        if (Mode.Contains(ModeElement.CalculateMovePathBfs))
        {
            newTarget.PathStrength = GetMovePathBattleFieldStrength(target.MovePath);
        }

        if (target.TransporterId != -1)
        {
            newTarget.TransporterId = target.TransporterId;
        }

        if (newTarget.PathStrength < Control.StrengthMax)
        {
            Targets.Add(newTarget);
        }

        newTarget.Release();
    }

    // AGCPlugins.pas:229
    public bool Setup(AIGroupControl control)
    {
        Reset();
        Selector.SourceLocation = control.Location;
        Selector.MovePointTable = control.MovePointTable;
        Selector.TargetInfo.Player = control.Group.Player;
        Selector.TargetInfo.IndependentRelation = control.Group.IndependentRelation;
        switch (AoWHSMap.Current.Players.GetPlayers(control.Group.Player).PlayerType)
        {
            case PlayerType.Cpu2:
                Mode = Mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
            case PlayerType.Cpu3:
                Mode = Mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
            case PlayerType.Cpu4:
                Mode = Mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
            case PlayerType.Cpu5:
                Mode = Mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
        }

        return true;
    }

    public enum ModeElement : byte
    {
        CalculateMovePathBfs = 0
    }

}
