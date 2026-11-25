#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// completion, control, groups, mode, remaining strength, remaining strengths, path factor,
// target, strength, damage types, strengths, required superiority
public class AIGroupCooperationControl : AIGroupTask
{
    // AIGroup.pas:2839
    public AIGroupCooperationControl(CustomNode? owner) : base(owner) => Groups = new AIRecruitmentGroupList(null) { Holder = CreationHolder() };

    public bool Complete { get; private set; }

    private AIGroupControl _control;

    public AIRecruitmentGroupList Groups { get; }
    public CooperationMode Mode { get; set; }

    private int _remainingStrength;
    private readonly int[] _remainingStrengths = new int[10];
    private float _pathStrengthFactor;
    private AIGroupControlTarget? _target;

    public int CooperateStrength { get; set; }

    private DamageTypes _damageTypes;
    private readonly int[] _cooperateStrengths = new int[10];

    public int Superiority { get; set; }

    // AIGroup.pas:2845
    public override void Dispose()
    {
        Groups?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2919
    public override bool Busy => State != -1;

    // AIGroup.pas:2932
    public override bool Activate(AIGroupControl control)
    {
        _control = control;
        _remainingStrength = CooperateStrength;
        UpdateCompletion();
        return true;
    }

    // AIGroup.pas:3017
    public override bool Process()
    {
        switch (State)
        {
            case 0:
                StrategicMap.Current.AIExecutor.UseProcessTime(500);
                _ = _control.RecruitmentGroups.Validate(_control);
                int i = 0;
                while (!Complete && i != _control.RecruitmentGroups.Count)
                {
                    if (ValidCooperationGroup(_control.RecruitmentGroups.GetRecruitmentGroups(i)))
                    {
                        AddCooperationGroup(_control.RecruitmentGroups.GetRecruitmentGroups(i));
                    }

                    i += 1;
                }

                if ((Groups.Count != 0 && Mode == CooperationMode.AutoCooperate) || (Mode == CooperationMode.CompleteCooperateOnly && Complete))
                {
                    ExecuteCooperation();
                }

                Done();
                break;
            default:
                break;
        }

        return false;
    }

    // AIGroup.pas:2924
    public virtual bool Setup(AIGroupControl control, AIGroupControlTarget target)
    {
        Reset();
        _target = target;
        return true;
    }

    // AIGroup.pas:2851
    // The first group is skipped, as the original loop starts at slot 1. GetMovesMin and MovePoints
    // only read. A non-positive day stays at the seed, which is -1.
    public int GetCooperateMoveDays() => Math.Max(Groups.Skip(1).Select(static recruit =>
    {
        int moves = recruit.Group.GetMovesMin();
        return moves > 0 ? (recruit.MovePoints - 1) / moves : -1;
    }).DefaultIfEmpty(-1).Max(), val2: -1);

    // AIGroup.pas:2868
    public int CooperatedStrength => CooperateStrength - _remainingStrength;

    // AIGroup.pas:2873
    public int GetCooperateStrength(DamageType damageType) => _cooperateStrengths[(int)damageType];

    // AIGroup.pas:2878
    public void SetCooperateStrength(DamageType damageType, int value) => _cooperateStrengths[(int)damageType] = value;

    // AIGroup.pas:2883
    public void UpdateCompletion()
    {
        Complete = _remainingStrength <= 0;
        if (Complete)
        {
            for (int dOrdinal = (int)DamageType.Fire; dOrdinal <= (int)DamageType.NonSpecific; dOrdinal++)
            {
                DamageType d = (DamageType)dOrdinal;
                if (_remainingStrengths[(int)d] > 0)
                {
                    Complete = false;
                    return;
                }
            }

            if (Superiority != -1)
            {
                _ = _control.CombatPredictor.Execute();
                if (_control.CombatPredictor.GetSuperiority() < Superiority)
                {
                    Complete = false;
                }
            }
        }
    }

    // AIGroup.pas:2905
    public void Reset()
    {
        Groups.Clear();
        Complete = false;
        Mode = CooperationMode.None;
        _target = null;
        CooperateStrength = 0;
        _damageTypes = AbilityRules.AllDamageTypes & ~DamageTypes.Wall;
        Array.Clear(_cooperateStrengths);
        Superiority = 120;
        _pathStrengthFactor = (float)0.8;
        State = 0;
    }

    // AIGroup.pas:2940
    public void AddCooperationGroup(AIRecruitmentGroup group)
    {
        _ = Groups.AddChild(group);
        for (int i = group.Group.Units.Count - 1; i >= 0; i--)
        {
            Troop? troop = group.Group.Units.GetUnit(i);
            int strengthMax = troop.GetStrengthMax(_damageTypes);
            _remainingStrength -= strengthMax;
            for (int dOrdinal = (int)DamageType.Fire; dOrdinal <= (int)DamageType.NonSpecific; dOrdinal++)
            {
                DamageType d = (DamageType)dOrdinal;
                int strength = troop.GetStrength(d);
                _remainingStrengths[(int)d] -= strength;
            }

            if (Superiority != -1)
            {
                _control.CombatPredictor.AddUnit(troop);
            }
        }

        UpdateCompletion();
    }

    // AIGroup.pas:2958
    public bool ValidCooperationGroup(AIRecruitmentGroup group)
    {
        MapCoordinate[] locationCell = new MapCoordinate[1];
        ref MapCoordinate location = ref locationCell[0];
        if (group.Group.TransporterId == -1 && ((group.Group.TargetLocation == _target.Location && group.Group.TargetType == AIGroupTargetStatus.Setup) || group.Group.TargetType is AIGroupTargetStatus.None or AIGroupTargetStatus.Idle) && group.Group.CooperateWithGroup(_control.Group, _target))
        {
            if (group.PathStrength != 0)
            {
                location = group.Group.GetLocation();
                int strength1 = group.Group.Units.GetStrengthMax(AbilityRules.AllDamageTypes & ~DamageTypes.Wall);
                AIGroup group1 = group.Group;
                int advantage1 = -GetBattleFieldStrengthAdvantage(location, group1.Player, relation: group1.IndependentRelation, group1.Units, friendlyRadius: 1, enemyRadius: 10, damageTypes: AbilityRules.AllDamageTypes & ~DamageTypes.Wall);

                location = _control.Group.GetLocation();
                int strength2 = _control.Group.Units.GetStrengthMax(AbilityRules.AllDamageTypes & ~DamageTypes.Wall);
                AIGroup group2 = _control.Group;
                int advantage2 = -GetBattleFieldStrengthAdvantage(location, group2.Player, relation: group2.IndependentRelation, group2.Units, friendlyRadius: 1, enemyRadius: 10, damageTypes: AbilityRules.AllDamageTypes & ~DamageTypes.Wall);

                int strength = strength1 < strength2 ? strength1 : strength2;

                // { Original bug: compares the first advantage with itself. }
                if (advantage1 > advantage1 + 0)
                {
                }
                else
                {
                    advantage1 = advantage2;
                }

                int threshold = Round((double)group.PathStrength * _pathStrengthFactor);
                if (strength < threshold && strength >= advantage1)
                {
                    int moves = group.Group.GetMovePointsMin();
                    int otherMoves = _control.Group.GetMovePointsMin();
                    if (otherMoves < moves)
                    {
                        moves = otherMoves;
                    }

                    return moves >= group.MovePoints;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    // AIGroup.pas:2998
    public void ExecuteCooperation()
    {
        for (int i = Groups.Count - 1; i >= 0; i--)
        {
            AIGroup? group = Groups.GetRecruitmentGroups(i).Group;
            Troop? leader = group.TargetType == AIGroupTargetStatus.Setup && _target.MovePoints > group.TargetMovePoints
                ? group.Leader
                : _control.Group.Leader;

            for (int j = group.Productions.Count - 1; j >= 0; j--)
            {
                group.Productions.ChangeGroup(j, _control.Group);
            }

            _ = _control.Group.AddUnits(group.Units);
            _control.Group.SetLeader(leader);
        }
    }

    private static int GetBattleFieldStrengthAdvantage(MapCoordinate location, PlayerId? player, int relation, UnitCollection units, int friendlyRadius, int enemyRadius, DamageTypes damageTypes)
    {
        BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, player, relation, requests: BattleFieldInfoRequests.None, damageTypes);
        info.EnemyRelations = DiplomaticRelations.None;
        info.Units = units;
        info.Requests |= BattleFieldInfoRequests.IgnorePassiveIndependents;
        StrategicMap.Current.RetrieveBattleFieldInfo(location.X, location.Y, location.MapLevel, radius: friendlyRadius, ref info);
        int friendlyStrength = info.FriendlyStrength;
        BattleFieldInfo.SetupBattleFieldInfo(out info, player, relation, requests: BattleFieldInfoRequests.None, damageTypes);
        info.FriendlyRelations = DiplomaticRelations.None;
        info.Units = units;
        info.Requests |= BattleFieldInfoRequests.IgnorePassiveIndependents;
        StrategicMap.Current.RetrieveBattleFieldInfo(location.X, location.Y, location.MapLevel, radius: enemyRadius, ref info);
        int strength = info.Strength;
        return friendlyStrength - strength;
    }

    public enum CooperationMode : byte
    {
        None = 0, AutoCooperate = 1, CompleteCooperateOnly = 2
    }
}
