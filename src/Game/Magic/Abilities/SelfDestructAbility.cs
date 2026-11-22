#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

namespace Aow.Game.Magic.Abilities;

public class SelfDestructAbility : TouchAbility
{
    // AoWAb.PAS:2136
    public SelfDestructAbility(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 65;
        AbilityType = AbilityType.Combat;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_selfDestructText);
        SelectionTypes = AbilitySelectionTypes.Unit;
    }

    // AoWAb.PAS:2211
    public override WallCombatFeatures GetWallCombatFeatures(AbilityOwner owner) => WallCombatFeatures.WallBreaching | WallCombatFeatures.Participate;

    // AoWAb.PAS:2222
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand? command)
    {
        CombatObject? target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        SelfDestructCombatAction action = CreateSelfDestructCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        _ = action.Release();
        return true;
    }

    // AoWAb.PAS:2145
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        if (CanTouchUnit(attacker, target: defender))
        {
            int hitPoints = defender.GetHitPoints();
            int defense = defender.GetDefense();
            int resistance = defender.GetResistance();
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            DamageTypes protectionTypes = defender.GetProtectionTypes();
            return AbilityRules.StatisticsToLimitedDamageValue(8, damage: 6, damageTypes: DamageTypes.Fire | DamageTypes.Wall, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes);
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:2154
    public override void GetDamageValues(Troop attacker, Troop? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, target: defender))
        {
            damageValues.DamageTypes = DamageTypes.Fire | DamageTypes.Wall;
            int hitPoints = defender.GetHitPoints();
            int defense = defender.GetDefense();
            int resistance = defender.GetResistance();
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            DamageTypes protectionTypes = defender.GetProtectionTypes();
            int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(8, damage: 6, damageTypes: DamageTypes.Fire | DamageTypes.Wall, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: defender.GetProtectionTypes(), immunityTypes: defender.GetImmunityTypes(), damage: 6, damageTypes: DamageTypes.Fire | DamageTypes.Wall);
            damageValues.SourceCost = statisticsToMaxDamage;
            int attackerHitPoints = attacker.GetHitPoints();
            damageValues.TargetDamage = attackerHitPoints * 100;
            int hitPoints2 = attacker.GetHitPoints();
            damageValues.TargetCost = hitPoints2;
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

    // AoWAb.PAS:2205
    public override void GetOffensiveStrength(AbilityOwner? owner, ref int strength, ref DamageTypes types)
    {
        strength = AbilityRules.LimitedDamage(8, defense: 6, damage: 1);
        types = DamageTypes.Fire | DamageTypes.Wall;
    }

    // AoWAb.PAS:2175
    public override void FastBattleGetDamageValues(CombatObject? attacker, CombatObject? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, target: defender))
        {
            damageValues.DamageTypes = DamageTypes.Fire | DamageTypes.Wall;
            int hitPoints = defender.HitPoints;
            int defense = defender.GetDefense();
            int resistance = defender.GetResistance();
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            DamageTypes protectionTypes = defender.GetProtectionTypes();
            int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(8, damage: 6, damageTypes: DamageTypes.Fire | DamageTypes.Wall, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(protectionTypes: defender.GetProtectionTypes(), immunityTypes: defender.GetImmunityTypes(), damage: 6, damageTypes: DamageTypes.Fire | DamageTypes.Wall);
            damageValues.SourceCost = statisticsToMaxDamage;
            int attackerHitPoints = attacker.HitPoints;
            damageValues.TargetDamage = attackerHitPoints * 100;
            int hitPoints2 = attacker.HitPoints;
            damageValues.TargetCost = hitPoints2;
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

    // AoWAb.PAS:2196
    public override void GetCombatInfo(AbilityOwner? owner, ref AbilityCombatInfo info)
    {
        info.Attack = 8;
        info.Damage = 6;
        info.DamageTypes = DamageTypes.Fire | DamageTypes.Wall;
        info.RepeatCount = -1;
        info.TargetTypes = CombatTargetTypes.Enemy;
    }

    // AoWAb.PAS:2235
    public override bool CanTouch(CombatObject? source, CombatObject? target)
    {
        return source.Side == CombatSideIndex.Defender
            ? !source.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || target.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing)
            : !target.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || source.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing);
    }

    // AoWAb.PAS:2257
    public override bool CanTouchUnit(Troop source, Troop? target) => true;

    // AoWAb.PAS:2216
    public SelfDestructCombatAction CreateSelfDestructCombatAction(CombatObject? source, CombatObject? target)
    {
        SelfDestructCombatAction result = new(null) { Holder = CreationHolder() };
        result.Generate(source, target, attack: 8, damage: 6, damageTypes: DamageTypes.Fire | DamageTypes.Wall);
        return result;
    }

    private static readonly string s_selfDestructText = "Self Destruct";
}
