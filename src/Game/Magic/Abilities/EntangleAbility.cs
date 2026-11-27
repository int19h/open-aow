#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public class EntangleAbility : TouchAbility
{
    // AoWAb.PAS:2968
    public EntangleAbility(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityId = 40;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.EntangleText);
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.UseItem;
    }

    // AoWAb.PAS:3058
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand? command)
    {
        CombatObject? target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        EntangleCombatAction action = CreateEntangleCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (!action.Successful)
        {
            StrikeCombatAction? strike = CombatObject.CreateStrikeCombatAction(target, target: source, defensive: true);
            if (strike is not null)
            {
                source.Combat.ExecuteCombatAction(strike);
                _ = strike.Release();
            }
        }

        _ = action.Release();
        return true;
    }

    // AoWAb.PAS:2999
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        if (CanTouchUnit(attacker, target: defender) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
        {
            float probability = TouchRoleProbability(attacker, target: defender);
            return Round((double)((double)defender.GetHitPoints() * probability) * 100);
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:3012
    public override void GetDamageValues(Troop attacker, Troop? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, target: defender) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
        {
            float probability = TouchRoleProbability(attacker, target: defender);
            int hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            int defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints;
            int unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, target: attacker);
            damageValues.TargetDamage = Round(unitStrikeDamageValue * ((double)1 - probability));
            int damage = defender.GetDamage();
            damageValues.TargetCost = damage;
            damageValues.DamageTypes = DamageTypes.None;
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = DamageTypes.None;
        }
    }

    // AoWAb.PAS:2991
    public override void GetOffensiveStrength(AbilityOwner? owner, ref int strength, ref DamageTypes types)
    {
        if (owner is Troop troop)
        {
            strength = Round(troop.GetAttack() * 7 * 5 * 0.1);
            types = DamageTypes.NonSpecific;
        }
    }

    // AoWAb.PAS:3035
    public override void FastBattleGetDamageValues(CombatObject? attacker, CombatObject? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, target: defender) && attacker is CombatUnit attackerUnit)
        {
            float probability = CombatTouchRoleProbability(attackerUnit, target: defender);
            int hitPoints = defender.HitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            int defenderHitPoints = defender.HitPoints;
            damageValues.SourceCost = defenderHitPoints;
            int strikeDamageValue = CombatObject.StrikeDamageValue(defender, target: attacker);
            damageValues.TargetDamage = Round(strikeDamageValue * ((double)1 - probability));
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: attacker.GetProtectionTypes(), immunityTypes: attacker.GetImmunityTypes(), damageTypes: CombatObject.MeleeDamageTypes(defender), damage: defender.GetDamage());
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = DamageTypes.None;
        }
        else
        {
            damageValues.SourceDamage = 0;
            damageValues.SourceCost = 0;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
            damageValues.DamageTypes = DamageTypes.None;
        }
    }

    // AoWAb.PAS:2975
    public override int GetTouchAttack(int level) => 7;

    // AoWAb.PAS:2984
    public override bool CanTouch(CombatObject? source, CombatObject? target) => base.CanTouch(source, target) && source is CombatUnit && target is CombatUnit && !target.GetAbilityEnabled(94);

    // AoWAb.PAS:2979. Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit? target)
    {
        RandSeed = GetEngine().StrategicMap.Random(16777215);
        return CreateEntangleCombatAction(source, target: target);
    }

    public EntangleCombatAction CreateEntangleCombatAction(CombatObject source, CombatObject? target)
    {
        EntangleCombatAction result = new(null) { Holder = CreationHolder() };
        result.Initialize(successful: source is CombatUnit sourceUnit && CombatTouchRole(sourceUnit, target), sourceId: source.Id, targetId: target.Id);
        return result;
    }
}
