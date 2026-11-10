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

namespace Aow.Game.Ai.Groups;

// mode, control, completion, units, remaining strengths, remaining maximum, requested maximum,
// damage types, requested strengths, superiority, move types, moves, path-strength factor
public partial class AIGroupRecruitControl : AIGroupControlPlugin
{
    // AIGroup.pas:2070
    public AIGroupRecruitControl(ECustomNode owner) : base(owner)
    {
        FUnits = new UnitUserList(null);
    }

    public Mode FMode;
    public AIGroupControl FControl;
    public bool FComplete;
    public UnitUserList FUnits;
    public FixedArray_int_10 FRemainingStrengths;
    public int FRemainingStrength;
    public int FRecruitStrength;
    public Set<DamageType> FDamageTypes;
    public FixedArray_int_10 FRecruitStrengths;
    public int FSuperiority;
    public Set<MoveType> FMoveTypes;
    public int FMovesMin;
    public float FPathStrengthFactor;

    // AIGroup.pas:2076
    public override void Dispose()
    {
        FUnits?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2097
    public override bool GetBusy() => FState != -1;

    // AIGroup.pas:2268
    public override bool Activate(AIGroupControl Control)
    {
        FControl = Control;
        FRemainingStrength = FRecruitStrength;
        UpdateCompletion();
        return true;
    }

    // AIGroup.pas:2276
    public override bool Process()
    {
        int I;
        switch (FState)
        {
            case 0:
                AoWHSMap.Current.FAIExecuter.UseProcessTime(500);
                I = 0;
                FControl.FRecruitmentGroups.Validate(FControl);
                while (!FComplete && I != FControl.FRecruitmentGroups.Count)
                {
                    if (ValidRecruitmentGroup(FControl.FRecruitmentGroups.GetRecruitmentGroups(I)))
                    {
                        RecruitFromGroup(FControl.FRecruitmentGroups.GetRecruitmentGroups(I));
                    }

                    I += 1;
                }

                if (Set<Mode>.Of(Mode.AutoRecruit, Mode.ForceRecruit).Contains(FMode) || (FMode == Mode.CompleteRecruitOnly && FComplete))
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
    public virtual bool Setup(AIGroupControl Control)
    {
        Reset();
        return true;
    }

    // AIGroup.pas:2082
    public int GetRecruitedStrength() => FRecruitStrength - FRemainingStrength;

    // AIGroup.pas:2087
    public int GetRecruitStrengthEx(DamageType DamageType) => FRecruitStrengths[(int)DamageType];

    // AIGroup.pas:2092
    public void SetRecruitStrengthEx(DamageType DamageType, int Value)
    {
        FRecruitStrengths[(int)DamageType] = Value;
    }

    // AIGroup.pas:2102
    public int GetRecruitMoveDays()
    {
        AIRecruitmentGroup Group;
        int Days;
        int Result = -1;
        for (int I = 0, last = FUnits.Count - 1; I <= last; I++)
        {
            var unit = FUnits.GetUnit(I);
            Group = FControl.FRecruitmentGroups.FindUnit(unit);
            if (Group != null && FUnits.GetUnit(I).GetMoves() != 0)
            {
                var moves = FUnits.GetUnit(I).GetMoves();
                Days = (Group.FMovePoints - 1) / moves;
                if (Days > Result)
                {
                    Result = Days;
                }
            }
        }

        return Result;
    }

    // AIGroup.pas:2119
    public bool UsefullRecruitUnit(AbstractUnit AUnit)
    {
        DamageType D;
        bool Result = AUnit.GetMoves() > FMovesMin && (AUnit.GetMoveTypes() & FMoveTypes) != Set<MoveType>.Of();
        if (Result)
        {
            Result = FRemainingStrength > 0 && AUnit.GetStrengthMax(FDamageTypes) > 0;
            if (!Result)
            {
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    Result = FRemainingStrengths[(int)D] > 0 && AUnit.GetStrength(D) > 0;
                    if (Result)
                    {
                        return Result;
                    }
                }

                if (!Result && FSuperiority != -1)
                {
                    Result = (FControl.FCombatPredictor.Usefullness(AUnit) & 8388608) != 0;
                }
            }
        }

        return Result;
    }

    // AIGroup.pas:2140
    public void UpdateCompletion()
    {
        DamageType D;
        FComplete = FRemainingStrength <= 0;
        if (FComplete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (FRemainingStrengths[(int)D] > 0)
                {
                    FComplete = false;
                    return;
                }
            }

            if (FSuperiority != -1)
            {
                FControl.FCombatPredictor.Execute();
                if (FControl.FCombatPredictor.GetSuperiority() < FSuperiority)
                {
                    FComplete = false;
                }
            }
        }
    }

    // AIGroup.pas:2162
    public void AddRecruitUnit(AbstractUnit AUnit)
    {
        DamageType D;
        FUnits.AddUnit(AUnit);
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = AUnit.GetStrength(D);
            FRemainingStrengths[(int)D] -= strength;
        }

        var strengthMax = AUnit.GetStrengthMax(FDamageTypes);
        FRemainingStrength -= strengthMax;
        if (FSuperiority != -1)
        {
            FControl.FCombatPredictor.AddUnit(AUnit);
        }

        UpdateCompletion();
    }

    // AIGroup.pas:2175
    public void DeleteRecruitUnit(AbstractUnit AUnit)
    {
        DamageType D;
        if (FUnits.IndexOf(AUnit) != -1)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = AUnit.GetStrength(D);
                FRemainingStrengths[(int)D] += strength;
            }

            var strengthMax = AUnit.GetStrengthMax(FDamageTypes);
            FRemainingStrength += strengthMax;
            if (FSuperiority != -1)
            {
                FControl.FCombatPredictor.DeleteUnit(AUnit);
            }

            FUnits.DeleteUnit(AUnit);
            UpdateCompletion();
        }
    }

    // AIGroup.pas:2191
    public void ExecuteRecruitment()
    {
        FControl.FGroup.AddUnits(FUnits);
    }

    // AIGroup.pas:2196
    public bool ValidRecruitmentGroup(AIRecruitmentGroup Group)
    {
        bool Result;
        if (Group.FPathStrength != 0)
        {
            Result = Group.FGroup.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall)) >= Round((double)Group.FPathStrength * FPathStrengthFactor);
            if (!Result)
            {
                if (FMode != Mode.ForceRecruit)
                {
                    Result = Group.FGroup.GetMovePointsMin() >= Group.FMovePoints;
                }
                else
                {
                    Result = Group.FGroup.GetMovePointsMin() + 20 >= Group.FMovePoints;
                }
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AIGroup.pas:2212
    public bool RecruitFromGroup(AIRecruitmentGroup Group)
    {
        UnitUserList Units;
        UnitUserList Recruitable;
        int I;
        AbstractUnit AUnit;
        bool Result = false;
        Units = new UnitUserList(null);
        I = Group.FGroup.Count - 1;
        for (; I >= 0; I -= 1)
        {
            if (UsefullRecruitUnit(Group.FGroup.GetUnit(I)))
            {
                Units.AddUnit(Group.FGroup.GetUnit(I));
            }
        }

        if (Units.Count != 0)
        {
            Recruitable = new UnitUserList(null);
            if (FMode != Mode.ForceRecruit)
            {
                Group.FGroup.GetRecruitableUnits(FControl.FGroup, Units, Recruitable);
            }
            else
            {
                Recruitable.AddUnits(Units);
            }

            I = 0;
            while (I < Recruitable.Count && !FComplete)
            {
                AUnit = Recruitable.GetUnit(I);
                if (UsefullRecruitUnit(AUnit))
                {
                    AddRecruitUnit(AUnit);
                    Result = true;
                }

                I += 1;
            }

            Recruitable?.Dispose();
        }

        Units?.Dispose();
        return Result;
    }

    // AIGroup.pas:2246
    public void Reset()
    {
        FUnits.Clear();
        FState = 0;
        FMovesMin = 0;
        FSuperiority = -1;
        FRecruitStrength = 0;
        FDamageTypes = AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall);
        // { Original bug: clears only the maximum strength, not the per-type array. }
        FillBytes(Bytes.Of(ref FRecruitStrength, 4), 0);
        FMoveTypes = Set<MoveType>.Of(MoveType.Walking, MoveType.Flying);
        FPathStrengthFactor = (float)0.8;
        FComplete = false;
        FMode = Mode.AutoRecruit;
    }

    public enum Mode : byte
    {
        None = 0, AutoRecruit = 1, CompleteRecruitOnly = 2, ForceRecruit = 3
    }

    // intf_P5_07.inc:390

}
