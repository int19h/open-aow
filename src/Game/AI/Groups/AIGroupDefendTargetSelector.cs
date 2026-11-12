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
    private AIGroupControl _control;
    private readonly AIMoveTargetSelector _selector;
    private readonly AIGroupControlTargetList _targets;
    private Set<ModeElement> _mode;
    private int _pathStep;
    private int _battleFieldRadius;
    private Set<DamageType> _damageTypes;
    private float _movePointsFactor;

    // AGCPlugins.pas:164
    public override void Dispose()
    {
        _selector?.Dispose();
        _targets?.Dispose();
        base.Dispose();
    }

    // AGCPlugins.pas:157
    public AIGroupDefendTargetSelector(ECustomNode owner) : base(owner)
    {
        _selector = new AIMoveTargetSelector(null);
        _targets = new AIGroupControlTargetList(null);
    }

    // AGCPlugins.pas:245
    public override bool Activate(AIGroupControl control)
    {
        _control = control;
        _selector.Search(control);
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
                if (_selector.Status == AIMoveTargetSelectorStatus.Searching)
                {
                    result = _selector.Process();
                }
                else
                {
                    result = false;
                }

                if (!result)
                {
                    _targets.SortOnPriority();
                    State += 1;
                }

                break;
            case 1:
                if (_targets.Count != 0)
                {
                    _control.Group.SetupTarget(null, AIGroupTargetStatus.Idle, 0);
                    _control.SetupME();
                    var targetLocation = _targets.GetTargets(0);
                    if (Unsafe.As<MapCoordinate, int>(ref targetLocation.Location) == Unsafe.As<MapCoordinate, int>(ref _control.Location))
                    {
                        _control.MoveExecutor.Mode = _control.MoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);
                    }

                    var target = _targets.GetTargets(0);
                    var target2 = _targets.GetTargets(0);
                    var target3 = _targets.GetTargets(0);
                    _control.ExecuteME(target3.Location.X, target2.Location.Y, target.Location.MapLevel);
                    State = 2;
                }
                else
                {
                    _control.SetupME();
                    var aiMoveExecutor = _control.MoveExecutor;
                    aiMoveExecutor.Mode = aiMoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);

                    var aiGroupControl = _control;
                    aiGroupControl.ExecuteME(aiGroupControl.Location.X, aiGroupControl.Location.Y, aiGroupControl.Location.MapLevel);

                    _control.Group.SetupTarget(null, AIGroupTargetStatus.Idle, 0);
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
        _selector.Reset();
        this._selector.SetTargetFoundHandler(TargetFoundEvent);
        this._selector.MovePointsMax = 150;
        this._selector.MovePointsMin = 0;
        this._selector.TargetInfo.Mode = AITargetMode.Defend;

        _pathStep = 7;
        _damageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        _battleFieldRadius = 10;
        _movePointsFactor = -1;
        _mode = Set<ModeElement>.Of();
    }

    // AGCPlugins.pas:188
    public int GetMovePathBattleFieldStrength(MovePath path)
    {
        int result;
        int I;
        BattleFieldInfo info = default;
        if (path != null && _pathStep < path.Count)
        {
            result = 0;
            I = _pathStep;
            BattleFieldInfo.SetupBattleFieldInfo(out info, _control.Group.Player, _control.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), _damageTypes);
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            while (I < path.Count)
            {
                info.Strength = 0;
                var pathL = path[I];
                var pathY = path[I];
                var pathX = path[I];
                StrategicMap.Current.RetrieveBattleFieldInfo(pathX.X, pathY.Y, pathL.MapLevel, _battleFieldRadius, ref info);
                if (result < info.Strength)
                {
                    result = info.Strength;
                }

                I += _pathStep;
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
        newTarget.Info = this._selector.TargetInfo;

        newTarget.Data = target.Data;
        newTarget.Location = target.Location;
        newTarget.MovePoints = target.MovePoints;
        newTarget.Data.Priority += Round((double)target.MovePoints * _movePointsFactor);
        if (_mode.Contains(ModeElement.CalculateMovePathBfs))
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

        newTarget.Release();
    }

    // AGCPlugins.pas:229
    public bool Setup(AIGroupControl control)
    {
        Reset();
        _selector.SourceLocation = control.Location;
        _selector.MovePointTable = control.MovePointTable;
        _selector.TargetInfo.Player = control.Group.Player;
        _selector.TargetInfo.IndependentRelation = control.Group.IndependentRelation;
        switch (StrategicMap.Current.Players.GetPlayers(control.Group.Player).PlayerType)
        {
            case PlayerType.Cpu2:
                _mode = _mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
            case PlayerType.Cpu3:
                _mode = _mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
            case PlayerType.Cpu4:
                _mode = _mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
            case PlayerType.Cpu5:
                _mode = _mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
                break;
        }

        return true;
    }

    public enum ModeElement : byte
    {
        CalculateMovePathBfs = 0
    }

}
