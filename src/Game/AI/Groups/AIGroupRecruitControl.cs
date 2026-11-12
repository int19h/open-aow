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

    public AIGroupRecruitControlMode Mode;
    public AIGroupControl Control;
    public bool Complete;
    public UnitUserList Units;
    public Int32Array10 RemainingStrengths;
    public int RemainingStrength;
    public int RecruitStrength;
    public Set<DamageType> DamageTypes;
    public Int32Array10 RecruitStrengths;
    public int Superiority;
    public Set<MoveType> MoveTypes;
    public int MovesMin;
    public float PathStrengthFactor;

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
        Control = control;
        RemainingStrength = RecruitStrength;
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
                Control.RecruitmentGroups.Validate(Control);
                while (!Complete && I != Control.RecruitmentGroups.Count)
                {
                    if (ValidRecruitmentGroup(Control.RecruitmentGroups.GetRecruitmentGroups(I)))
                    {
                        RecruitFromGroup(Control.RecruitmentGroups.GetRecruitmentGroups(I));
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
    public int GetRecruitedStrength() => RecruitStrength - RemainingStrength;

    // AIGroup.pas:2087
    public int GetRecruitStrength(DamageType damageType) => RecruitStrengths[(int)damageType];

    // AIGroup.pas:2092
    public void SetRecruitStrength(DamageType damageType, int value)
    {
        RecruitStrengths[(int)damageType] = value;
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
            group = Control.RecruitmentGroups.FindUnit(troop);
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
        bool result = troop.GetMoves() > MovesMin && (troop.GetMoveTypes() & MoveTypes) != Set<MoveType>.Of();
        if (result)
        {
            result = RemainingStrength > 0 && troop.GetStrengthMax(DamageTypes) > 0;
            if (!result)
            {
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    result = RemainingStrengths[(int)D] > 0 && troop.GetStrength(D) > 0;
                    if (result)
                    {
                        return result;
                    }
                }

                if (!result && Superiority != -1)
                {
                    result = (Control.CombatPredictor.Usefullness(troop) & 8388608) != 0;
                }
            }
        }

        return result;
    }

    // AIGroup.pas:2140
    public void UpdateCompletion()
    {
        DamageType D;
        Complete = RemainingStrength <= 0;
        if (Complete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (RemainingStrengths[(int)D] > 0)
                {
                    Complete = false;
                    return;
                }
            }

            if (Superiority != -1)
            {
                Control.CombatPredictor.Execute();
                if (Control.CombatPredictor.GetSuperiority() < Superiority)
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
            RemainingStrengths[(int)D] -= strength;
        }

        var strengthMax = troop.GetStrengthMax(DamageTypes);
        RemainingStrength -= strengthMax;
        if (Superiority != -1)
        {
            Control.CombatPredictor.AddUnit(troop);
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
                RemainingStrengths[(int)D] += strength;
            }

            var strengthMax = troop.GetStrengthMax(DamageTypes);
            RemainingStrength += strengthMax;
            if (Superiority != -1)
            {
                Control.CombatPredictor.DeleteUnit(troop);
            }

            Units.DeleteUnit(troop);
            UpdateCompletion();
        }
    }

    // AIGroup.pas:2191
    public void ExecuteRecruitment()
    {
        Control.Group.AddUnits(Units);
    }

    // AIGroup.pas:2196
    public bool ValidRecruitmentGroup(AIRecruitmentGroup group)
    {
        bool result;
        if (group.PathStrength != 0)
        {
            result = group.Group.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall)) >= Round((double)group.PathStrength * PathStrengthFactor);
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
                group.Group.GetRecruitableUnits(Control.Group, units, recruitable);
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
        MovesMin = 0;
        Superiority = -1;
        RecruitStrength = 0;
        DamageTypes = AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        // { Original bug: clears only the maximum strength, not the per-type array. }
        FillBytes(Bytes.Of(ref RecruitStrength, 4), 0);
        MoveTypes = Set<MoveType>.Of(MoveType.Walking, MoveType.Flying);
        PathStrengthFactor = (float)0.8;
        Complete = false;
        Mode = AIGroupRecruitControlMode.AutoRecruit;
    }

    // intf_P5_07.inc:390

}

public enum AIGroupRecruitControlMode : byte
{
    None = 0, AutoRecruit = 1, CompleteRecruitOnly = 2, ForceRecruit = 3
}
