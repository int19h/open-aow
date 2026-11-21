#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public class InvokeDeathAbility : TouchAbility
{
    // AoWAb.PAS:3437
    public InvokeDeathAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 35;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_invokeDeathText);
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.UseItem;
    }

    // AoWAb.PAS:3532
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        InvokeDeathCombatAction action = CreateCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (target.GetEnabled())
        {
            StrikeCombatAction strike = CombatObject.CreateStrikeCombatAction(target, target: source, defensive: true);
            if (strike is not null)
            {
                source.Combat.ExecuteCombatAction(strike);
                _ = strike.Release();
            }
        }

        _ = action.Release();
        return true;
    }

    // AoWAb.PAS:3464
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        if ((!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
        {
            float probability = TouchRoleProbability(attacker, target: defender);
            return Round((double)((double)defender.GetHitPoints() * probability) * 100);
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:3476
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
        {
            float probability = TouchRoleProbability(attacker, target: defender);
            int hitPoints = defender.GetHitPoints();
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            int unitStrikeDamageValue = Troop.UnitStrikeDamageValue(defender, target: attacker);
            damageValues.TargetDamage = Round(unitStrikeDamageValue * ((double)1 - probability));
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: attacker.GetProtectionTypes(), damageTypes: GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).GetDamageTypes(defender), immunityTypes: attacker.GetImmunityTypes(), damage: defender.GetDamage());
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = DamageTypes.Death;
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

    // AoWAb.PAS:3456
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref DamageTypes types)
    {
        if (owner is Troop troop)
        {
            strength = Round(troop.GetAttack() * 6 * 5 * 0.1);
            types = DamageTypes.Death;
        }
    }

    // AoWAb.PAS:3502
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, target: defender) && attacker is CombatUnit attackerUnit)
        {
            float probability = CombatTouchRoleProbability(attackerUnit, target: defender);
            int hitPoints = defender.HitPoints;
            damageValues.SourceCost = hitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 100);
            int strikeDamageValue = CombatObject.StrikeDamageValue(defender, target: attacker);
            damageValues.TargetDamage = Round(strikeDamageValue * ((double)1 - probability));
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: attacker.GetProtectionTypes(), immunityTypes: attacker.GetImmunityTypes(), damageTypes: CombatObject.MeleeDamageTypes(defender), damage: defender.GetDamage());
            damageValues.TargetCost = statisticsToMaxDamage;
            damageValues.DamageTypes = DamageTypes.Death;
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

    // AoWAb.PAS:3444
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        sbyte touchAttack = GetTouchAttack(1);
        info.Attack = touchAttack;
        info.Damage = -1;
        info.DamageTypes = DamageTypes.Death;
        info.RepeatCount = 1;
        info.TargetTypes = CombatTargetTypes.Enemy;
    }

    // AoWAb.PAS:3452
    public override sbyte GetTouchAttack(int level) => 6;

    // AoWAb.PAS:3561
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        return result
            ? target is CombatUnit targetUnit && targetUnit.Troop.GetUnitType() is UnitType.Humanoid or UnitType.Creature && (ushort)targetUnit.Troop.GetRace() != 11 && !targetUnit.Troop.GetAbilityEnabled(129)
            : result;
    }

    // AoWAb.PAS:3554
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        if (result)
        {
            UnitType unitType = target.GetUnitType();
            return unitType is UnitType.Humanoid or UnitType.Creature && (ushort)target.GetRace() != 11 && !target.GetAbilityEnabled(129);
        }

        return result;
    }

    // AoWAb.PAS:3527. Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreateCombatAction(source, target: target);
    }

    public InvokeDeathCombatAction CreateCombatAction(CombatObject source, CombatObject target)
    {
        InvokeDeathCombatAction result = new(null) { Holder = CreationHolder() };
        result.Initialize(successful: source is CombatUnit sourceUnit && CombatTouchRole(sourceUnit, target), sourceId: source.Id, targetId: target.Id);
        return result;
    }

    private static readonly string s_invokeDeathText = "Invoke Death";
}
