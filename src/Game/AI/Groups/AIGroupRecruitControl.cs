// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// mode, control, completion, units, remaining strengths, remaining maximum, requested maximum,
// damage types, requested strengths, superiority, move types, moves, path-strength factor
public partial class AIGroupRecruitControl : AIGroupTask
{
    // AIGroup.pas:2070
    public AIGroupRecruitControl(ECustomNode owner) : base(owner)
    {
        Units = new UnitUserList(null);
    }

    public AIGroupRecruitControlMode Mode { get; set; }

    private AIGroupControl _control;

    public bool Complete { get; private set; }
    public UnitUserList Units { get; }

    private Int32Array10 _remainingStrengths;
    private int _remainingStrength;
    public int RecruitStrength;
    private Set<DamageType> _damageTypes;
    private Int32Array10 _recruitStrengths;

    public int Superiority { get; set; }

    private Set<MoveType> _moveTypes;
    private int _movesMin;
    private float _pathStrengthFactor;

    // AIGroup.pas:2076
    public override void Dispose()
    {
        Units?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2097
    public override bool GetBusy() => State != -1;

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
        int I;
        switch (State)
        {
            case 0:
                AoWHSMap.Current.AIExecutor.UseProcessTime(500);
                I = 0;
                _control.RecruitmentGroups.Validate(_control);
                while (!Complete && I != _control.RecruitmentGroups.Count)
                {
                    if (ValidRecruitmentGroup(_control.RecruitmentGroups.GetRecruitmentGroups(I)))
                    {
                        RecruitFromGroup(_control.RecruitmentGroups.GetRecruitmentGroups(I));
                    }

                    I += 1;
                }

                if (Set<AIGroupRecruitControlMode>.Of(AIGroupRecruitControlMode.AutoRecruit, AIGroupRecruitControlMode.ForceRecruit).Contains(Mode) || (Mode == AIGroupRecruitControlMode.CompleteRecruitOnly && Complete))
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
    public int GetRecruitedStrength() => RecruitStrength - _remainingStrength;

    // AIGroup.pas:2087
    public int GetRecruitStrength(DamageType damageType) => _recruitStrengths[(int)damageType];

    // AIGroup.pas:2092
    public void SetRecruitStrength(DamageType damageType, int value)
    {
        _recruitStrengths[(int)damageType] = value;
    }

    // AIGroup.pas:2102
    public int GetRecruitMoveDays()
    {
        AIRecruitmentGroup group;
        int days;
        int result = -1;
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            var troop = Units.GetUnit(I);
            group = _control.RecruitmentGroups.FindUnit(troop);
            if (group != null && Units.GetUnit(I).GetMoves() != 0)
            {
                var moves = Units.GetUnit(I).GetMoves();
                days = (group.MovePoints - 1) / moves;
                if (days > result)
                {
                    result = days;
                }
            }
        }

        return result;
    }

    // AIGroup.pas:2119
    public bool UsefulRecruitUnit(Troop troop)
    {
        DamageType D;
        bool result = troop.GetMoves() > _movesMin && (troop.GetMoveTypes() & _moveTypes) != Set<MoveType>.Of();
        if (result)
        {
            result = _remainingStrength > 0 && troop.GetStrengthMax(_damageTypes) > 0;
            if (!result)
            {
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    result = _remainingStrengths[(int)D] > 0 && troop.GetStrength(D) > 0;
                    if (result)
                    {
                        return result;
                    }
                }

                if (!result && Superiority != -1)
                {
                    result = (_control.CombatPredictor.Usefullness(troop) & 8388608) != 0;
                }
            }
        }

        return result;
    }

    // AIGroup.pas:2140
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

    // AIGroup.pas:2162
    public void AddRecruitUnit(Troop troop)
    {
        DamageType D;
        Units.AddUnit(troop);
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = troop.GetStrength(D);
            _remainingStrengths[(int)D] -= strength;
        }

        var strengthMax = troop.GetStrengthMax(_damageTypes);
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
        DamageType D;
        if (Units.IndexOf(troop) != -1)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = troop.GetStrength(D);
                _remainingStrengths[(int)D] += strength;
            }

            var strengthMax = troop.GetStrengthMax(_damageTypes);
            _remainingStrength += strengthMax;
            if (Superiority != -1)
            {
                _control.CombatPredictor.DeleteUnit(troop);
            }

            Units.DeleteUnit(troop);
            UpdateCompletion();
        }
    }

    // AIGroup.pas:2191
    public void ExecuteRecruitment()
    {
        _control.Group.AddUnits(Units);
    }

    // AIGroup.pas:2196
    public bool ValidRecruitmentGroup(AIRecruitmentGroup group)
    {
        bool result;
        if (group.PathStrength != 0)
        {
            result = group.Group.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall)) >= Round((double)group.PathStrength * _pathStrengthFactor);
            if (!result)
            {
                if (Mode != AIGroupRecruitControlMode.ForceRecruit)
                {
                    result = group.Group.GetMovePointsMin() >= group.MovePoints;
                }
                else
                {
                    result = group.Group.GetMovePointsMin() + 20 >= group.MovePoints;
                }
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AIGroup.pas:2212
    public bool RecruitFromGroup(AIRecruitmentGroup group)
    {
        UnitUserList units;
        UnitUserList recruitable;
        int I;
        Troop troop;
        bool result = false;
        units = new UnitUserList(null);
        I = group.Group.Count - 1;
        for (; I >= 0; I -= 1)
        {
            if (UsefulRecruitUnit(group.Group.GetUnit(I)))
            {
                units.AddUnit(group.Group.GetUnit(I));
            }
        }

        if (units.Count != 0)
        {
            recruitable = new UnitUserList(null);
            if (Mode != AIGroupRecruitControlMode.ForceRecruit)
            {
                group.Group.GetRecruitableUnits(_control.Group, units, recruitable);
            }
            else
            {
                recruitable.AddUnits(units);
            }

            I = 0;
            while (I < recruitable.Count && !Complete)
            {
                troop = recruitable.GetUnit(I);
                if (UsefulRecruitUnit(troop))
                {
                    AddRecruitUnit(troop);
                    result = true;
                }

                I += 1;
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
        _damageTypes = AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        // { Original bug: clears only the maximum strength, not the per-type array. }
        FillBytes(Bytes.Of(ref RecruitStrength, 4), 0);
        _moveTypes = Set<MoveType>.Of(MoveType.Walking, MoveType.Flying);
        _pathStrengthFactor = (float)0.8;
        Complete = false;
        Mode = AIGroupRecruitControlMode.AutoRecruit;
    }

    // intf_P5_07.inc:390

}

public enum AIGroupRecruitControlMode : byte
{
    None = 0, AutoRecruit = 1, CompleteRecruitOnly = 2, ForceRecruit = 3
}
