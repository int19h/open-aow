#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public class WebAbility : TouchAbility
{
    // AoWAb.PAS:2806
    public WebAbility(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 27;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_webText);
        SelectionTypes = AbilitySelectionTypes.Unit;
    }

    // AoWAb.PAS:2901
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand? command)
    {
        CombatObject? target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        WebCombatAction action = CreateWebCombatAction(source, target);
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

    // AoWAb.PAS:2842
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

    // AoWAb.PAS:2855
    public override void GetDamageValues(Troop attacker, Troop? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
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

    // AoWAb.PAS:2833
    public override void GetOffensiveStrength(AbilityOwner? owner, ref int strength, ref DamageTypes types)
    {
        if (owner is Troop troop)
        {
            strength = Round(troop.GetAttack() * 4 * 4 * 0.1);
            types = DamageTypes.NonSpecific;
        }
    }

    // AoWAb.PAS:2878
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

    // AoWAb.PAS:2814
    public override int GetTouchAttack(int level) => 4;

    // AoWAb.PAS:2825
    public override bool CanTouch(CombatObject? source, CombatObject? target) => base.CanTouch(source, target) && source is CombatUnit && target is CombatUnit && !target.GetAbilityEnabled(97);

    // AoWAb.PAS:2819. Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit? target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreateWebCombatAction(source, target: target);
    }

    public WebCombatAction CreateWebCombatAction(CombatObject source, CombatObject? target)
    {
        WebCombatAction result = new(null) { Holder = CreationHolder() };
        result.Initialize(successful: source is CombatUnit sourceUnit && CombatTouchRole(sourceUnit, target), sourceId: source.Id, targetId: target.Id);
        return result;
    }

    private static readonly string s_webText = "Web";
}
