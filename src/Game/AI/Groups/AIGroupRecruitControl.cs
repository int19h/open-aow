#nullable disable warnings
// Translated from AoWE.pas.

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// mode, control, completion, units, remaining strengths, remaining maximum, requested maximum,
// damage types, requested strengths, superiority, move types, moves, path-strength factor
public class AIGroupRecruitControl : AIGroupTask
{
    // AIGroup.pas:2070
    public AIGroupRecruitControl(CustomNode? owner) : base(owner) => Units = new UnitUserList(null) { Holder = CreationHolder() };

    public AIGroupRecruitControlMode Mode { get; set; }

    private AIGroupControl _control;

    public bool Complete { get; private set; }
    public UnitUserList Units { get; }

    private readonly int[] _remainingStrengths = new int[10];
    private int _remainingStrength;

    public int RecruitStrength { get; internal set; }

    private DamageTypes _damageTypes;
    private readonly int[] _recruitStrengths = new int[10];

    public int Superiority { get; set; }

    private MoveTypes _moveTypes;
    private int _movesMin;
    private float _pathStrengthFactor;

    // AIGroup.pas:2076
    public override void Dispose()
    {
        Units?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2097
    public override bool Busy => State != -1;

    // AIGroup.pas:2268
    public override bool Activate(AIGroupControl control)
    {
        _control = control;
        _remainingStrength = RecruitStrength;
        UpdateCompletion();
        return true;
    }

    // AIGroup.pas:2276
    public override bool Process()
    {
        switch (State)
        {
            case 0:
                StrategicMap.Current.AIExecutor.UseProcessTime(500);
                int i = 0;
                _ = _control.RecruitmentGroups.Validate(_control);
                while (!Complete && i != _control.RecruitmentGroups.Count)
                {
                    if (ValidRecruitmentGroup(_control.RecruitmentGroups.GetRecruitmentGroups(i)))
                    {
                        _ = RecruitFromGroup(_control.RecruitmentGroups.GetRecruitmentGroups(i));
                    }

                    i += 1;
                }

                if (Mode is AIGroupRecruitControlMode.AutoRecruit or AIGroupRecruitControlMode.ForceRecruit || (Mode == AIGroupRecruitControlMode.CompleteRecruitOnly && Complete))
                {
                    ExecuteRecruitment();
                }

                Done();
                break;
            default:
                Done();
                break;
        }

        return false;
    }

    // AIGroup.pas:2262
    public virtual bool Setup(AIGroupControl control)
    {
        Reset();
        return true;
    }

    // AIGroup.pas:2082
    public int RecruitedStrength => RecruitStrength - _remainingStrength;

    // AIGroup.pas:2087
    public int GetRecruitStrength(DamageType damageType) => _recruitStrengths[(int)damageType];

    // AIGroup.pas:2092
    public void SetRecruitStrength(DamageType damageType, int value) => _recruitStrengths[(int)damageType] = value;

    // AIGroup.pas:2102
    // GetUnit(i) is the enumerated troop. FindUnit and Moves only read. A non-positive day stays -1.
    public int GetRecruitMoveDays() => Units.Select(troop =>
    {
        AIRecruitmentGroup group = _control.RecruitmentGroups.FindUnit(troop);
        return group is not null && troop.Moves != 0 ? Math.Max((group.MovePoints - 1) / troop.Moves, val2: -1) : -1;
    }).DefaultIfEmpty(-1).Max();

    // AIGroup.pas:2119
    public bool UsefulRecruitUnit(Troop? troop)
    {
        bool result = troop.Moves > _movesMin && (troop.GetMoveTypes() & _moveTypes) != MoveTypes.None;
        if (result)
        {
            result = _remainingStrength > 0 && troop.GetStrengthMax(_damageTypes) > 0;
            if (!result)
            {
                for (int dOrdinal = (int)DamageType.Fire; dOrdinal <= (int)DamageType.NonSpecific; dOrdinal++)
                {
                    DamageType d = (DamageType)dOrdinal;
                    result = _remainingStrengths[(int)d] > 0 && troop.GetStrength(d) > 0;
                    if (result)
                    {
                        return result;
                    }
                }

                if (!result && Superiority != -1)
                {
                    return (_control.CombatPredictor.Usefullness(troop) & 0b1000_0000_0000_0000_0000_0000) != 0;
                }
            }
        }

        return result;
    }

    // AIGroup.pas:2140
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

    // AIGroup.pas:2162
    public void AddRecruitUnit(Troop? troop)
    {
        _ = Units.AddUnit(troop);
        for (int dOrdinal = (int)DamageType.Fire; dOrdinal <= (int)DamageType.NonSpecific; dOrdinal++)
        {
            DamageType d = (DamageType)dOrdinal;
            int strength = troop.GetStrength(d);
            _remainingStrengths[(int)d] -= strength;
        }

        int strengthMax = troop.GetStrengthMax(_damageTypes);
        _remainingStrength -= strengthMax;
        if (Superiority != -1)
        {
            _control.CombatPredictor.AddUnit(troop);
        }

        UpdateCompletion();
    }

    // AIGroup.pas:2175
    public void DeleteRecruitUnit(Troop troop)
    {
        if (Units.IndexOf(troop) != -1)
        {
            for (int dOrdinal = (int)DamageType.Fire; dOrdinal <= (int)DamageType.NonSpecific; dOrdinal++)
            {
                DamageType d = (DamageType)dOrdinal;
                int strength = troop.GetStrength(d);
                _remainingStrengths[(int)d] += strength;
            }

            int strengthMax = troop.GetStrengthMax(_damageTypes);
            _remainingStrength += strengthMax;
            if (Superiority != -1)
            {
                _control.CombatPredictor.DeleteUnit(troop);
            }

            _ = Units.DeleteUnit(troop);
            UpdateCompletion();
        }
    }

    // AIGroup.pas:2191
    public void ExecuteRecruitment() => _control.Group.AddUnits(Units.Units);

    // AIGroup.pas:2196
    public bool ValidRecruitmentGroup(AIRecruitmentGroup group)
    {
        bool result;
        if (group.PathStrength != 0)
        {
            result = group.Group.Units.GetStrengthMax(AbilityRules.AllDamageTypes & ~DamageTypes.Wall) >= Round((double)group.PathStrength * _pathStrengthFactor);
            if (!result)
            {
                return Mode != AIGroupRecruitControlMode.ForceRecruit
                    ? group.Group.GetMovePointsMin() >= group.MovePoints
                    : group.Group.GetMovePointsMin() + 20 >= group.MovePoints;
            }
        }
        else
        {
            return true;
        }

        return result;
    }

    // AIGroup.pas:2212
    public bool RecruitFromGroup(AIRecruitmentGroup group)
    {
        bool result = false;
        UnitUserList units = new(null) { Holder = CreationHolder() };
        int i = group.Group.Units.Count - 1;
        for (; i >= 0; i -= 1)
        {
            if (UsefulRecruitUnit(group.Group.Units.GetUnit(i)))
            {
                _ = units.AddUnit(group.Group.Units.GetUnit(i));
            }
        }

        if (units.Count != 0)
        {
            UnitUserList recruitable = new(null) { Holder = CreationHolder() };
            _ = Mode != AIGroupRecruitControlMode.ForceRecruit
                ? group.Group.GetRecruitableUnits(_control.Group, units, recruitable)
                : recruitable.AddUnits(units.Units);

            i = 0;
            while (i < recruitable.Count && !Complete)
            {
                Troop? troop = recruitable.GetUnit(i);
                if (UsefulRecruitUnit(troop))
                {
                    AddRecruitUnit(troop);
                    result = true;
                }

                i += 1;
            }

            recruitable?.Dispose();
        }

        units?.Dispose();
        return result;
    }

    // AIGroup.pas:2246
    public void Reset()
    {
        Units.Clear();
        State = 0;
        _movesMin = 0;
        Superiority = -1;
        RecruitStrength = 0;
        _damageTypes = AbilityRules.AllDamageTypes & ~DamageTypes.Wall;
        // { Original bug: a second FillChar clears only the maximum strength again, not the per-type
        //   array. }
        _moveTypes = MoveTypes.Walking | MoveTypes.Flying;
        _pathStrengthFactor = (float)0.8;
        Complete = false;
        Mode = AIGroupRecruitControlMode.AutoRecruit;
    }
}

public enum AIGroupRecruitControlMode : byte
{
    None = 0, AutoRecruit = 1, CompleteRecruitOnly = 2, ForceRecruit = 3
}
