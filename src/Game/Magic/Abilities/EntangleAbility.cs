// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class EntangleAbility : TouchAbility
{
    // AoWAb.PAS:2968
    public EntangleAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 40;
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_entangleText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
    }

    // AoWAb.PAS:3058
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        EntangleCombatAction action;
        StrikeCombatAction strike;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreateEntangleCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (!action.Successful)
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

    // AoWAb.PAS:2999
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        float probability;
        if (CanTouchUnit(attacker, defender) && !defender.GetImmunityTypes().Contains(DamageType.None))
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

    // AoWAb.PAS:3012
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, defender) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            var defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints;
            var unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)unitStrikeDamageValue * ((double)1 - probability));
            var damage = defender.GetDamage();
            damageValues.TargetCost = damage;
            damageValues.DamageTypes = Set<DamageType>.Of();
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

    // AoWAb.PAS:2991
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        if (owner is Troop troop)
        {
            strength = Round((double)(troop.GetAttack() * 7 * 5) * 0.1);
            types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:3035
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            probability = CombatTouchRoleProbability((CombatUnit)attacker, defender);
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            var defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints;
            var strikeDamageValue = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = Round((double)strikeDamageValue * ((double)1 - probability));
            var protectionTypes = attacker.GetProtectionTypes();
            var immunityTypes = attacker.GetImmunityTypes();
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(defender);
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(defender.GetDamage(), meleeDamageTypes, immunityTypes, protectionTypes);
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = Set<DamageType>.Of();
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

    // AoWAb.PAS:2975
    public override sbyte GetTouchAttack(int level) => 7;

    // AoWAb.PAS:2984
    public override bool CanTouch(CombatObject source, CombatObject target) => base.CanTouch(source, target) && (source is CombatUnit && target is CombatUnit && !target.GetAbilityEnabled(94));

    // AoWAb.PAS:2979
    public EntangleCombatAction CreateEntangleCombatAction(CombatObject source, CombatObject target)
    {
        EntangleCombatAction result = new EntangleCombatAction(null);
        var combatTouchRole = CombatTouchRole((CombatUnit)source, target);
        result.Initialize(source.Id, target.Id, combatTouchRole);
        return result;
    }
}
