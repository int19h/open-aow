#nullable disable
// Translated from AGCPlugins.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// control, selector, targets, mode, path step, radius, damage, factor
public class AIGroupDefendTargetSelector : AIGroupTask
{
    private AIGroupControl _control;
    private readonly AIMoveTargetSelector _selector;
    private readonly AIGroupControlTargetList _targets;
    private Modes _mode;
    private int _pathStep;
    private int _battleFieldRadius;
    private DamageTypes _damageTypes;
    private float _movePointsFactor;

    // AGCPlugins.pas:164
    public override void Dispose()
    {
        _selector?.Dispose();
        _targets?.Dispose();
        base.Dispose();
    }

    // AGCPlugins.pas:157
    public AIGroupDefendTargetSelector(CustomNode owner) : base(owner)
    {
        _selector = new AIMoveTargetSelector(null) { Holder = CreationHolder() };
        _targets = new AIGroupControlTargetList(null) { Holder = CreationHolder() };
    }

    // AGCPlugins.pas:245
    public override bool Activate(AIGroupControl control)
    {
        _control = control;
        _ = _selector.Search(control);
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
                result = _selector.Status == AIMoveTargetSelectorStatus.Searching && _selector.Process();

                if (!result)
                {
                    _targets.SortOnPriority();
                    State += 1;
                }

                break;
            case 1:
                if (_targets.Count != 0)
                {
                    _ = _control.Group.SetupTarget(null, targetType: AIGroupTargetStatus.Idle, mode: 0);
                    _control.SetupME();
                    AIGroupControlTarget targetLocation = _targets.GetTargets(0);
                    if (targetLocation.Location == _control.Location)
                    {
                        _control.MoveExecutor.Mode &= ~AIMoveExecutorModes.Adjacent;
                    }

                    AIGroupControlTarget target = _targets.GetTargets(0);
                    AIGroupControlTarget target2 = _targets.GetTargets(0);
                    AIGroupControlTarget target3 = _targets.GetTargets(0);
                    _control.ExecuteME(target3.Location.X, target2.Location.Y, L: target.Location.MapLevel);
                    State = 2;
                }
                else
                {
                    _control.SetupME();
                    AIMoveExecutor aiMoveExecutor = _control.MoveExecutor;
                    aiMoveExecutor.Mode &= ~AIMoveExecutorModes.Adjacent;

                    AIGroupControl aiGroupControl = _control;
                    aiGroupControl.ExecuteME(aiGroupControl.Location.X, aiGroupControl.Location.Y, L: aiGroupControl.Location.MapLevel);

                    _ = _control.Group.SetupTarget(null, targetType: AIGroupTargetStatus.Idle, mode: 0);
                    Done();
                }

                return false;
            case 2:
                Done();
                return false;
            default:
                return false;
        }

        return result;
    }

    // AGCPlugins.pas:171
    public void Reset()
    {
        _selector.Reset();
        _selector.SetTargetFoundHandler(TargetFoundEvent);
        _selector.MovePointsMax = 150;
        _selector.MovePointsMin = 0;
        AITargetInfo targetInfo = _selector.TargetInfo;
        targetInfo.Mode = AITargetMode.Defend;
        _selector.TargetInfo = targetInfo;

        _pathStep = 7;
        _damageTypes = AbilityRules.AllDamageTypes & ~DamageTypes.Wall;
        _battleFieldRadius = 10;
        _movePointsFactor = -1;
        _mode = Modes.None;
    }

    // AGCPlugins.pas:188
    public int GetMovePathBattleFieldStrength(MovePath path)
    {
        int result;
        if (path is not null && _pathStep < path.Count)
        {
            result = 0;
            int i = _pathStep;
            BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, _control.Group.Player, relation: _control.Group.IndependentRelation, requests: BattleFieldInfoRequests.None, _damageTypes);
            info.FriendlyRelations = DiplomaticRelations.None;
            info.Requests |= BattleFieldInfoRequests.IgnorePassiveIndependents;
            while (i < path.Count)
            {
                info.Strength = 0;
                PathStep pathL = path[i];
                PathStep pathY = path[i];
                PathStep pathX = path[i];
                StrategicMap.Current.RetrieveBattleFieldInfo(pathX.Location.X, pathY.Location.Y, pathL.Location.MapLevel, radius: _battleFieldRadius, ref info);
                if (result < info.Strength)
                {
                    result = info.Strength;
                }

                i += _pathStep;
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // AGCPlugins.pas:213
    public void TargetFoundEvent(MoveTarget target, ref bool searchDone)
    {
        AIGroupControlTarget newTarget = new(null)
        {
            Holder = CreationHolder(),
            Info = _selector.TargetInfo,

            Data = target.Data,
            Location = target.Location,
            MovePoints = target.MovePoints
        };
        AITargetData data = newTarget.Data;
        data.Priority += Round((double)target.MovePoints * _movePointsFactor);
        newTarget.Data = data;
        if (_mode.Contains(Modes.CalculateMovePathBfs))
        {
            newTarget.PathStrength = GetMovePathBattleFieldStrength(target.MovePath);
        }

        if (target.TransporterId != -1)
        {
            newTarget.TransporterId = target.TransporterId;
        }

        if (newTarget.PathStrength < _control.StrengthMax)
        {
            _targets.Add(newTarget);
        }

        _ = newTarget.Release();
    }

    // AGCPlugins.pas:229
    public bool Setup(AIGroupControl control)
    {
        Reset();
        _selector.SourceLocation = control.Location;
        _selector.MovePointTable = control.MovePointTable;
        AITargetInfo targetInfo = _selector.TargetInfo;
        targetInfo.Player = control.Group.Player;
        _selector.TargetInfo = targetInfo;
        AITargetInfo targetInfo2 = _selector.TargetInfo;
        targetInfo2.IndependentRelation = control.Group.IndependentRelation;
        _selector.TargetInfo = targetInfo2;
        switch (StrategicMap.Current.Players.GetPlayers(control.Group.Player).PlayerType)
        {
            case PlayerType.Cpu2:
                _mode |= Modes.CalculateMovePathBfs;
                break;
            case PlayerType.Cpu3:
                _mode |= Modes.CalculateMovePathBfs;
                break;
            case PlayerType.Cpu4:
                _mode |= Modes.CalculateMovePathBfs;
                break;
            case PlayerType.Cpu5:
                _mode |= Modes.CalculateMovePathBfs;
                break;
            case PlayerType.Human:
            case PlayerType.Cpu1:
            case PlayerType.Independent:
            case PlayerType.Free:
            default:
                break;
        }

        return true;
    }

    [Flags]
    public enum Modes : byte
    {
        None = 0,
        CalculateMovePathBfs = 1 << 0,
    }
}
