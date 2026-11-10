// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class InvokeDeathAbility : TouchAbility
{
    // AoWAb.PAS:3437
    public InvokeDeathAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 35;
        FAbilityName = AbilityRules.TranslateRStr(InvokeDeathRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
    }

    // AoWAb.PAS:3532
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        InvokeDeathCA Action;
        StrikeCA Strike;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreateCA(Source, Target);
        Source.FCombat.ExecuteCombatAction(Action);
        if (Target.GetEnabled())
        {
            Strike = CombatObject.CreateStrikeCA(Target, Source, true);
            if (Strike != null)
            {
                Source.FCombat.ExecuteCombatAction(Strike);
                Strike.Release();
            }
        }

        Action.Release();
        return true;
    }

    // AoWAb.PAS:3464
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        float Probability;
        if ((!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            Result = Round((double)((double)Defender.GetHitPoints() * Probability) * 100);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:3476
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        int HitPoints;
        if (WallSituation == WallSituation.None && (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            HitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = HitPoints;
            DamageValues.SourceDamage = Round((double)((double)HitPoints * Probability) * 100);
            var p4UnitStrikeDV = AbstractUnit.P4UnitStrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)p4UnitStrikeDV * ((double)1 - Probability));
            var protectionTypes = Attacker.GetProtectionTypes();
            var damageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Defender);
            var immunityTypes = Attacker.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(Defender.GetDamage(), damageTypes, immunityTypes, protectionTypes);
            DamageValues.TargetCost = statisticsToMaxDamage;
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            DamageValues.SourceDamage = 0;
            DamageValues.SourceCost = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:3456
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        if (Owner is AbstractUnit unit)
        {
            Strength = Round((double)(unit.GetAttack() * 6 * 5) * 0.1);
            Types = Set<DamageType>.Of(DamageType.Death);
        }
    }

    // AoWAb.PAS:3502
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        int HitPoints;
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            Probability = CombatTouchRoleProbability((CombatUnit)Attacker, Defender);
            HitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = HitPoints;
            DamageValues.SourceDamage = Round((double)((double)HitPoints * Probability) * 100);
            var strikeDV = CombatObject.StrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)strikeDV * ((double)1 - Probability));
            var protectionTypes = Attacker.GetProtectionTypes();
            var immunityTypes = Attacker.GetImmunityTypes();
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(Defender);
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(Defender.GetDamage(), meleeDamageTypes, immunityTypes, protectionTypes);
            DamageValues.TargetCost = statisticsToMaxDamage;
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            DamageValues.SourceDamage = 0;
            DamageValues.SourceCost = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:3444
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        var touchAttack = GetTouchAttack(1);
        Info.Attack = touchAttack;
        Info.Damage = -1;
        Info.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        Info.RepeatCount = 1;
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:3452
    public override sbyte GetTouchAttack(int Level) => 6;

    // AoWAb.PAS:3561
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result = base.CanTouch(Source, Target);
        if (Result)
        {
            bool condition = Target is CombatUnit;
            if (condition)
            {
                var unitType = ((CombatUnit)Target).FUnit.GetUnitType();
                condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
            }

            Result = condition && (ushort)((CombatUnit)Target).FUnit.GetRace() != 11 && !((CombatUnit)Target).FUnit.GetAbilityEnabled(129);
        }

        return Result;
    }

    // AoWAb.PAS:3554
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.CanTouchUnit(Source, Target);
        if (Result)
        {
            var unitType = Target.GetUnitType();
            Result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && (ushort)Target.GetRace() != 11 && !Target.GetAbilityEnabled(129);
        }

        return Result;
    }

    // AoWAb.PAS:3527
    public InvokeDeathCA CreateCA(CombatObject Source, CombatObject Target)
    {
        InvokeDeathCA Result = new InvokeDeathCA(null);
        var combatTouchRole = CombatTouchRole((CombatUnit)Source, Target);
        Result.Initialize(Source.FID, Target.FID, combatTouchRole);
        return Result;
    }

    private static readonly string InvokeDeathRStr = "Invoke Death";
}
