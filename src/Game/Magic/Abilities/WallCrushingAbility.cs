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
        FAbilityID = 117;
        FAbilityType = AbilityType.Combat;
        FAbilityName = AbilityRules.TranslateRStr(WallCrushingRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem, AbilitySelectionType.UseItem);
    }

    // AoWAb.PAS:2076
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);

    // AoWAb.PAS:2000
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        var hitPoints = Defender.GetHitPoints();
        var defense = Defender.GetDefense();
        var resistance = Defender.GetResistance();
        var immunityTypes = Defender.GetImmunityTypes();
        var protectionTypes = Defender.GetProtectionTypes();
        return AbilityRules.StatisticsToLimitedDV(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
    }

    // AoWAb.PAS:2006
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None)
        {
            // { Original leaves DamageTypes untouched on this successful path. }
            var hitPoints = Defender.GetHitPoints();
            var defense = Defender.GetDefense();
            var resistance = Defender.GetResistance();
            var immunityTypes = Defender.GetImmunityTypes();
            var protectionTypes = Defender.GetProtectionTypes();
            var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            DamageValues.SourceDamage = statisticsToLimitedDV;
            var defenderProtectionTypes = Defender.GetProtectionTypes();
            var defenderImmunityTypes = Defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            DamageValues.SourceCost = statisticsToMaxDamage;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
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

    // AoWAb.PAS:2096
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        if (Owner is AbstractUnit unit)
        {
            Strength = AbilityRules.P3LimitedDamage(unit.GetAttack(), 6, 1);
            Types = Set<DamageType>.Of(DamageType.Wall);
        }
    }

    // AoWAb.PAS:2027
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (CanTouch(Attacker, Defender))
        {
            var hitPoints = Defender.GetHitPoints();
            var defense = Defender.GetDefense();
            var resistance = Defender.GetResistance();
            var immunityTypes = Defender.GetImmunityTypes();
            var protectionTypes = Defender.GetProtectionTypes();
            var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            DamageValues.SourceDamage = statisticsToLimitedDV;
            var defenderProtectionTypes = Defender.GetProtectionTypes();
            var defenderImmunityTypes = Defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            DamageValues.SourceCost = statisticsToMaxDamage;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
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

    // AoWAb.PAS:2047
    public override void tcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, ref AbilityDamageValues DamageValues)
    {
        if (CanTouch(Attacker, Defender))
        {
            var hitPoints = Defender.GetHitPoints();
            var defense = Defender.GetDefense();
            var resistance = Defender.GetResistance();
            var immunityTypes = Defender.GetImmunityTypes();
            var protectionTypes = Defender.GetProtectionTypes();
            var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(6, 6, Set<DamageType>.Of(DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            DamageValues.SourceDamage = statisticsToLimitedDV;
            var defenderProtectionTypes = Defender.GetProtectionTypes();
            var defenderImmunityTypes = Defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            DamageValues.SourceCost = statisticsToMaxDamage;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
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

    // AoWAb.PAS:2067
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        Info.Attack = 6;
        Info.Damage = 6;
        Info.DamageTypes = Set<DamageType>.Of(DamageType.Wall);
        Info.RepeatCount = 1;
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of();
    }

    // AoWAb.PAS:2105
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result = base.CanTouch(Source, Target);
        return Result && !(Target is CombatUnit);
    }

    private static readonly string WallCrushingRStr = "Wall Crushing";

    private static DamageCA CreateDamageCA(CombatObject Source, CombatObject Target, int Attack, int Damage, Set<DamageType> DamageTypes)
    {
        DamageCA tDamageCA = new DamageCA(null);
        tDamageCA.Generate(Source, Target, (sbyte)Attack, (sbyte)Damage, DamageTypes);
        return tDamageCA;
    }
}
