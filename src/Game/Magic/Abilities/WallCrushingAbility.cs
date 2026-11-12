// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class WallCrushingAbility : TouchAbility
{
    // AoWAb.PAS:1991
    public WallCrushingAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 117;
        AbilityType = AbilityType.Combat;
        AbilityName = AbilityRules.TranslateResourceString(s_wallCrushingText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.UseItem);
    }

    // AoWAb.PAS:2076
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);

    // AoWAb.PAS:2000
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        var hitPoints = defender.GetHitPoints();
        var defense = defender.GetDefense();
        var resistance = defender.GetResistance();
        var immunityTypes = defender.GetImmunityTypes();
        var protectionTypes = defender.GetProtectionTypes();
        return AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
    }

    // AoWAb.PAS:2006
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None)
        {
            // { Original leaves DamageTypes untouched on this successful path. }
            var hitPoints = defender.GetHitPoints();
            var defense = defender.GetDefense();
            var resistance = defender.GetResistance();
            var immunityTypes = defender.GetImmunityTypes();
            var protectionTypes = defender.GetProtectionTypes();
            var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            var defenderProtectionTypes = defender.GetProtectionTypes();
            var defenderImmunityTypes = defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            damageValues.SourceCost = statisticsToMaxDamage;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
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

    // AoWAb.PAS:2096
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        if (owner is Troop troop)
        {
            strength = AbilityRules.LimitedDamage(troop.GetAttack(), 6, 1);
            types = Set<DamageType>.Of(DamageType.Wall);
        }
    }

    // AoWAb.PAS:2027
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (CanTouch(attacker, defender))
        {
            var hitPoints = defender.GetHitPoints();
            var defense = defender.GetDefense();
            var resistance = defender.GetResistance();
            var immunityTypes = defender.GetImmunityTypes();
            var protectionTypes = defender.GetProtectionTypes();
            var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            var defenderProtectionTypes = defender.GetProtectionTypes();
            var defenderImmunityTypes = defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            damageValues.SourceCost = statisticsToMaxDamage;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
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

    // AoWAb.PAS:2047
    public override void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues)
    {
        if (CanTouch(attacker, defender))
        {
            var hitPoints = defender.GetHitPoints();
            var defense = defender.GetDefense();
            var resistance = defender.GetResistance();
            var immunityTypes = defender.GetImmunityTypes();
            var protectionTypes = defender.GetProtectionTypes();
            var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            var defenderProtectionTypes = defender.GetProtectionTypes();
            var defenderImmunityTypes = defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            damageValues.SourceCost = statisticsToMaxDamage;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
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

    // AoWAb.PAS:2067
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        info.Attack = 6;
        info.Damage = 6;
        info.DamageTypes = Set<DamageType>.Of(DamageType.Wall);
        info.RepeatCount = 1;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of();
    }

    // AoWAb.PAS:2105
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        return result && !(target is CombatUnit);
    }

    private static readonly string s_wallCrushingText = "Wall Crushing";

    private static DamageCombatAction CreateDamageCombatAction(CombatObject source, CombatObject target, int attack, int damage, Set<DamageType> damageTypes)
    {
        DamageCombatAction damageCombatAction = new DamageCombatAction(null);
        damageCombatAction.Generate(source, target, (sbyte)attack, (sbyte)damage, damageTypes);
        return damageCombatAction;
    }
}
