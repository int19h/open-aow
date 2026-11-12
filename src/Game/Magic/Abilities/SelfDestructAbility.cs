// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class SelfDestructAbility : TouchAbility
{
    // AoWAb.PAS:2136
    public SelfDestructAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 65;
        AbilityType = AbilityType.Combat;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_selfDestructText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:2211
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);

    // AoWAb.PAS:2222
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        SelfDestructCombatAction action;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreateSelfDestructCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        action.Release();
        return true;
    }

    // AoWAb.PAS:2145
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        if (CanTouchUnit(attacker, defender))
        {
            var hitPoints = defender.GetHitPoints();
            var defense = defender.GetDefense();
            var resistance = defender.GetResistance();
            var immunityTypes = defender.GetImmunityTypes();
            var protectionTypes = defender.GetProtectionTypes();
            result = AbilityRules.StatisticsToLimitedDamageValue(8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:2154
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, defender))
        {
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
            var hitPoints = defender.GetHitPoints();
            var defense = defender.GetDefense();
            var resistance = defender.GetResistance();
            var immunityTypes = defender.GetImmunityTypes();
            var protectionTypes = defender.GetProtectionTypes();
            var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            var defenderProtectionTypes = defender.GetProtectionTypes();
            var defenderImmunityTypes = defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            damageValues.SourceCost = statisticsToMaxDamage;
            var attackerHitPoints = attacker.GetHitPoints();
            damageValues.TargetDamage = attackerHitPoints * 100;
            var hitPoints2 = attacker.GetHitPoints();
            damageValues.TargetCost = hitPoints2;
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

    // AoWAb.PAS:2205
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        strength = AbilityRules.LimitedDamage(8, 6, 1);
        types = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
    }

    // AoWAb.PAS:2175
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            damageValues.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
            var hitPoints = defender.GetHitPoints();
            var defense = defender.GetDefense();
            var resistance = defender.GetResistance();
            var immunityTypes = defender.GetImmunityTypes();
            var protectionTypes = defender.GetProtectionTypes();
            var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            damageValues.SourceDamage = statisticsToLimitedDamageValue;
            var defenderProtectionTypes = defender.GetProtectionTypes();
            var defenderImmunityTypes = defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            damageValues.SourceCost = statisticsToMaxDamage;
            var attackerHitPoints = attacker.GetHitPoints();
            damageValues.TargetDamage = attackerHitPoints * 100;
            var hitPoints2 = attacker.GetHitPoints();
            damageValues.TargetCost = hitPoints2;
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

    // AoWAb.PAS:2196
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        info.Attack = 8;
        info.Damage = 6;
        info.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
        info.RepeatCount = -1;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:2235
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result;
        if (source.GetSide() == CombatSideIndex.Defender)
        {
            if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position))
            {
                result = target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (target.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !source.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // AoWAb.PAS:2257
    public override bool CanTouchUnit(Troop source, Troop target) => true;

    // AoWAb.PAS:2216
    public SelfDestructCombatAction CreateSelfDestructCombatAction(CombatObject source, CombatObject target)
    {
        SelfDestructCombatAction result = new SelfDestructCombatAction(null) { Holder = CreationHolder() };
        result.Generate(source, target, 8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall));
        return result;
    }

    private static readonly string s_selfDestructText = "Self Destruct";
}
