#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class WallCrushingAbility : TouchAbility
{
    // AoWAb.PAS:1991
    public WallCrushingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 117;
        AbilityType = AbilityType.Combat;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_wallCrushingText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.UseItem);
    }

    // AoWAb.PAS:2076
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);

    // AoWAb.PAS:2000
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        sbyte hitPoints = defender.GetHitPoints();
        sbyte defense = defender.GetDefense();
        sbyte resistance = defender.GetResistance();
        Set<DamageType> immunityTypes = defender.GetImmunityTypes();
        Set<DamageType> protectionTypes = defender.GetProtectionTypes();
        return AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
    }

    // AoWAb.PAS:2006
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None)
        {
            // { Original leaves DamageTypes untouched on this successful path. }
            sbyte hitPoints = defender.GetHitPoints();
            sbyte defense = defender.GetDefense();
            sbyte resistance = defender.GetResistance();
            Set<DamageType> immunityTypes = defender.GetImmunityTypes();
            Set<DamageType> protectionTypes = defender.GetProtectionTypes();
            int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: defender.GetProtectionTypes(), immunityTypes: defender.GetImmunityTypes(), damage: 6, damageTypes: Set<DamageType>.Of(DamageType.Wall));
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
            sbyte hitPoints = defender.GetHitPoints();
            sbyte defense = defender.GetDefense();
            sbyte resistance = defender.GetResistance();
            Set<DamageType> immunityTypes = defender.GetImmunityTypes();
            Set<DamageType> protectionTypes = defender.GetProtectionTypes();
            int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: defender.GetProtectionTypes(), immunityTypes: defender.GetImmunityTypes(), damage: 6, damageTypes: Set<DamageType>.Of(DamageType.Wall));
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
            sbyte hitPoints = defender.GetHitPoints();
            sbyte defense = defender.GetDefense();
            sbyte resistance = defender.GetResistance();
            Set<DamageType> immunityTypes = defender.GetImmunityTypes();
            Set<DamageType> protectionTypes = defender.GetProtectionTypes();
            int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: defender.GetProtectionTypes(), immunityTypes: defender.GetImmunityTypes(), damage: 6, damageTypes: Set<DamageType>.Of(DamageType.Wall));
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
        DamageCombatAction damageCombatAction = new(null) { Holder = source };
        damageCombatAction.Generate(source, target, (sbyte)attack, (sbyte)damage, damageTypes);
        return damageCombatAction;
    }
}
