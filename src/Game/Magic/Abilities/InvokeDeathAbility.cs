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
        AbilityId = 35;
        AbilityName = AbilityRules.TranslateResourceString(s_invokeDeathText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
    }

    // AoWAb.PAS:3532
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        InvokeDeathCombatAction action;
        StrikeCombatAction strike;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreateCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (target.GetEnabled())
        {
            strike = CombatObject.CreateStrikeCombatAction(target, source, true);
            if (strike != null)
            {
                source.Combat.ExecuteCombatAction(strike);
                strike.Release();
            }
        }

        action.Release();
        return true;
    }

    // AoWAb.PAS:3464
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        float probability;
        if ((!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            result = Round((double)((double)defender.GetHitPoints() * probability) * 100);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:3476
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        int hitPoints;
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            var unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)unitStrikeDamageValue * ((double)1 - probability));
            var protectionTypes = attacker.GetProtectionTypes();
            var damageTypes = ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).GetDamageTypes(defender);
            var immunityTypes = attacker.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(defender.GetDamage(), damageTypes, immunityTypes, protectionTypes);
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:3456
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        if (owner is Troop troop)
        {
            strength = Round((double)(troop.GetAttack() * 6 * 5) * 0.1);
            types = Set<DamageType>.Of(DamageType.Death);
        }
    }

    // AoWAb.PAS:3502
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        int hitPoints;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            probability = CombatTouchRoleProbability((CombatUnit)attacker, defender);
            hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            var strikeDamageValue = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)strikeDamageValue * ((double)1 - probability));
            var protectionTypes = attacker.GetProtectionTypes();
            var immunityTypes = attacker.GetImmunityTypes();
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(defender);
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(defender.GetDamage(), meleeDamageTypes, immunityTypes, protectionTypes);
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:3444
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        var touchAttack = GetTouchAttack(1);
        info.Attack = touchAttack;
        info.Damage = -1;
        info.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        info.RepeatCount = 1;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:3452
    public override sbyte GetTouchAttack(int level) => 6;

    // AoWAb.PAS:3561
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        if (result)
        {
            bool condition = target is CombatUnit;
            if (condition)
            {
                var unitType = ((CombatUnit)target).Troop.GetUnitType();
                condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
            }

            result = condition && (ushort)((CombatUnit)target).Troop.GetRace() != 11 && !((CombatUnit)target).Troop.GetAbilityEnabled(129);
        }

        return result;
    }

    // AoWAb.PAS:3554
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        if (result)
        {
            var unitType = target.GetUnitType();
            result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && (ushort)target.GetRace() != 11 && !target.GetAbilityEnabled(129);
        }

        return result;
    }

    // AoWAb.PAS:3527
    public InvokeDeathCombatAction CreateCombatAction(CombatObject source, CombatObject target)
    {
        InvokeDeathCombatAction result = new InvokeDeathCombatAction(null);
        var combatTouchRole = CombatTouchRole((CombatUnit)source, target);
        result.Initialize(source.Id, target.Id, combatTouchRole);
        return result;
    }

    private static readonly string s_invokeDeathText = "Invoke Death";
}
