// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// completion, control, groups, mode, remaining strength, remaining strengths, path factor,
// target, strength, damage types, strengths, required superiority
public partial class AIGroupCooperationControl : AIGroupTask
{
    // AIGroup.pas:2839
    public AIGroupCooperationControl(ECustomNode owner) : base(owner)
    {
        Groups = new AIRecruitmentGroupList(null);
    }

    public bool Complete { get; private set; }

    private AIGroupControl _control;

    public AIRecruitmentGroupList Groups { get; }
    public CooperationMode Mode { get; set; }

    private int _remainingStrength;
    private readonly int[] _remainingStrengths = new int[10];
    private float _pathStrengthFactor;
    private AIGroupControlTarget _target;

    public int CooperateStrength { get; set; }

    private Set<DamageType> _damageTypes;
    private readonly int[] _cooperateStrengths = new int[10];

    public int Superiority { get; set; }

    // AIGroup.pas:2845
    public override void Dispose()
    {
        Groups?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2919
    public override bool GetBusy() => State != -1;

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
        int I;
        switch (State)
        {
            case 0:
                StrategicMap.Current.AIExecutor.UseProcessTime(500);
                _control.RecruitmentGroups.Validate(_control);
                I = 0;
                while (!Complete && I != _control.RecruitmentGroups.Count)
                {
                    if (ValidCooperationGroup(_control.RecruitmentGroups.GetRecruitmentGroups(I)))
                    {
                        AddCooperationGroup(_control.RecruitmentGroups.GetRecruitmentGroups(I));
                    }

                    I += 1;
                }

                if ((Groups.Count != 0 && Mode == CooperationMode.AutoCooperate) || (Mode == CooperationMode.CompleteCooperateOnly && Complete))
                {
                    ExecuteCooperation();
                }

                Done();
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
    public int GetCooperateMoveDays()
    {
        int moves;
        int days;
        int result = -1;
        // { Original bug: the first cooperation group is skipped. }
        for (int I = 1, last = Groups.Count - 1; I <= last; I++)
        {
            moves = Groups.GetRecruitmentGroups(I).Group.GetMovesMin();
            if (moves > 0)
            {
                days = (Groups.GetRecruitmentGroups(I).MovePoints - 1) / moves;
                if (result < days)
                {
                    result = days;
                }
            }
        }

        return result;
    }

    // AIGroup.pas:2868
    public int GetCooperatedStrength() => CooperateStrength - _remainingStrength;

    // AIGroup.pas:2873
    public int GetCooperateStrength(DamageType damageType) => _cooperateStrengths[(int)damageType];

    // AIGroup.pas:2878
    public void SetCooperateStrength(DamageType damageType, int value)
    {
        _cooperateStrengths[(int)damageType] = value;
    }

    // AIGroup.pas:2883
    public void UpdateCompletion()
    {
        DamageType D;
        Complete = _remainingStrength <= 0;
        if (Complete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (_remainingStrengths[(int)D] > 0)
                {
                    Complete = false;
                    return;
                }
            }

            if (Superiority != -1)
            {
                _control.CombatPredictor.Execute();
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
        _damageTypes = AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        Array.Clear(_cooperateStrengths);
        Superiority = 120;
        _pathStrengthFactor = (float)0.8;
        State = 0;
    }

    // AIGroup.pas:2940
    public void AddCooperationGroup(AIRecruitmentGroup group)
    {
        Troop troop;
        DamageType D;
        Groups.AddChild(group);
        for (int I = group.Group.Count - 1; I >= 0; I--)
        {
            troop = group.Group.GetUnit(I);
            var strengthMax = troop.GetStrengthMax(_damageTypes);
            _remainingStrength -= strengthMax;
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = troop.GetStrength(D);
                _remainingStrengths[(int)D] -= strength;
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
        bool result;
        int strength1;
        int strength2;
        int strength;
        int[] locationCell = new int[1];
        ref int location = ref locationCell[0];
        int advantage1;
        int advantage2;
        int moves;
        int otherMoves;
        int threshold;
        AIGroup group1;
        AIGroup group2;
        if (group.Group.TransporterId == -1 && ((Unsafe.As<MapCoordinate, int>(ref group.Group.TargetMapCoordinate) == Unsafe.As<MapCoordinate, int>(ref _target.Location) && group.Group.TargetType == AIGroupTargetStatus.Setup) || Set<AIGroupTargetStatus>.Of(AIGroupTargetStatus.None, AIGroupTargetStatus.Idle).Contains(group.Group.TargetType)) && group.Group.CooperateWithGroup(_control.Group, _target))
        {
            if (group.PathStrength != 0)
            {
                location = group.Group.GetLocation();
                strength1 = group.Group.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                group1 = group.Group;
                advantage1 = -GetBattleFieldStrengthAdvantage(location, group1.Player, group1.IndependentRelation, group1, 1, 10, AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));

                location = _control.Group.GetLocation();
                strength2 = _control.Group.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                group2 = _control.Group;
                advantage2 = -GetBattleFieldStrengthAdvantage(location, group2.Player, group2.IndependentRelation, group2, 1, 10, AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));

                if (strength1 < strength2)
                {
                    strength = strength1;
                }
                else
                {
                    strength = strength2;
                }

                // { Original bug: compares the first advantage with itself. }
                if (advantage1 > advantage1 + 0)
                {
                }
                else
                {
                    advantage1 = advantage2;
                }

                threshold = Round((double)group.PathStrength * _pathStrengthFactor);
                if (strength < threshold && strength >= advantage1)
                {
                    moves = group.Group.GetMovePointsMin();
                    otherMoves = _control.Group.GetMovePointsMin();
                    if (otherMoves < moves)
                    {
                        moves = otherMoves;
                    }

                    result = moves >= group.MovePoints;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:2998
    public void ExecuteCooperation()
    {
        AIGroup group;
        Troop leader;
        for (int I = Groups.Count - 1; I >= 0; I--)
        {
            group = Groups.GetRecruitmentGroups(I).Group;
            if (group.TargetType == AIGroupTargetStatus.Setup && _target.MovePoints > group.TargetMovePoints)
            {
                leader = group.Leader;
            }
            else
            {
                leader = _control.Group.Leader;
            }

            for (int J = group.Productions.Count - 1; J >= 0; J--)
            {
                group.Productions.ChangeGroup(J, _control.Group);
            }

            _control.Group.AddUnits(group);
            _control.Group.SetLeader(leader);
        }
    }

    private static int GetBattleFieldStrengthAdvantage(int location, sbyte player, sbyte relation, UnitList units, int friendlyRadius, int enemyRadius, Set<DamageType> damageTypes)
    {
        BattleFieldInfo info = default(BattleFieldInfo);
        BattleFieldInfo.SetupBattleFieldInfo(out info, player, relation, Set<BattleFieldInfoRequestsElement>.Of(Array.Empty<BattleFieldInfoRequestsElement>()), damageTypes);
        info.EnemyRelations = Set<byte>.WithBounds(0, 7);
        info.Units = units;
        info.Requests += Set<BattleFieldInfoRequestsElement>.Of(new BattleFieldInfoRequestsElement[1] { BattleFieldInfoRequestsElement.IgnorePassiveIndependents });
        StrategicMap.Current.RetrieveBattleFieldInfo(Unsafe.As<int, MapCoordinate>(ref location).X, Unsafe.As<int, MapCoordinate>(ref location).Y, Unsafe.As<int, MapCoordinate>(ref location).MapLevel, friendlyRadius, ref info);
        int friendlyStrength = info.FriendlyStrength;
        BattleFieldInfo.SetupBattleFieldInfo(out info, player, relation, Set<BattleFieldInfoRequestsElement>.Of(Array.Empty<BattleFieldInfoRequestsElement>()), damageTypes);
        info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
        info.Units = units;
        info.Requests += Set<BattleFieldInfoRequestsElement>.Of(new BattleFieldInfoRequestsElement[1] { BattleFieldInfoRequestsElement.IgnorePassiveIndependents });
        StrategicMap.Current.RetrieveBattleFieldInfo(Unsafe.As<int, MapCoordinate>(ref location).X, Unsafe.As<int, MapCoordinate>(ref location).Y, Unsafe.As<int, MapCoordinate>(ref location).MapLevel, enemyRadius, ref info);
        int strength = info.Strength;
        return friendlyStrength - strength;
    }

    public enum CooperationMode : byte
    {
        None = 0, AutoCooperate = 1, CompleteCooperateOnly = 2
    }

    // intf_P5_07.inc:431

    // intf_P5_07.inc:431

}
