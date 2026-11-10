// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class SelfDestructAbility : TouchAbility
{
    // AoWAb.PAS:2136
    public SelfDestructAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 65;
        FAbilityType = AbilityType.Combat;
        FAbilityName = AbilityRules.TranslateRStr(SelfDestructRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:2211
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner) => Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);

    // AoWAb.PAS:2222
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        SelfDestructCA Action;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreateSelfDestructCA(Source, Target);
        Source.FCombat.ExecuteCombatAction(Action);
        Action.Release();
        return true;
    }

    // AoWAb.PAS:2145
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        if (CanTouchUnit(Attacker, Defender))
        {
            var hitPoints = Defender.GetHitPoints();
            var defense = Defender.GetDefense();
            var resistance = Defender.GetResistance();
            var immunityTypes = Defender.GetImmunityTypes();
            var protectionTypes = Defender.GetProtectionTypes();
            Result = AbilityRules.StatisticsToLimitedDV(8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:2154
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && CanTouchUnit(Attacker, Defender))
        {
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
            var hitPoints = Defender.GetHitPoints();
            var defense = Defender.GetDefense();
            var resistance = Defender.GetResistance();
            var immunityTypes = Defender.GetImmunityTypes();
            var protectionTypes = Defender.GetProtectionTypes();
            var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            DamageValues.SourceDamage = statisticsToLimitedDV;
            var defenderProtectionTypes = Defender.GetProtectionTypes();
            var defenderImmunityTypes = Defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            DamageValues.SourceCost = statisticsToMaxDamage;
            var attackerHitPoints = Attacker.GetHitPoints();
            DamageValues.TargetDamage = attackerHitPoints * 100;
            var hitPoints2 = Attacker.GetHitPoints();
            DamageValues.TargetCost = hitPoints2;
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

    // AoWAb.PAS:2205
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        Strength = AbilityRules.P3LimitedDamage(8, 6, 1);
        Types = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
    }

    // AoWAb.PAS:2175
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
            var hitPoints = Defender.GetHitPoints();
            var defense = Defender.GetDefense();
            var resistance = Defender.GetResistance();
            var immunityTypes = Defender.GetImmunityTypes();
            var protectionTypes = Defender.GetProtectionTypes();
            var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), hitPoints, defense, resistance, immunityTypes, protectionTypes);
            DamageValues.SourceDamage = statisticsToLimitedDV;
            var defenderProtectionTypes = Defender.GetProtectionTypes();
            var defenderImmunityTypes = Defender.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall), defenderImmunityTypes, defenderProtectionTypes);
            DamageValues.SourceCost = statisticsToMaxDamage;
            var attackerHitPoints = Attacker.GetHitPoints();
            DamageValues.TargetDamage = attackerHitPoints * 100;
            var hitPoints2 = Attacker.GetHitPoints();
            DamageValues.TargetCost = hitPoints2;
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

    // AoWAb.PAS:2196
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        Info.Attack = 8;
        Info.Damage = 6;
        Info.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
        Info.RepeatCount = -1;
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:2235
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result;
        if (Source.GetSide() == CombatSideIndex.Defender)
        {
            if (Source.fcBehindWall() && Source.fcWallInBetween(Target.FPosition))
            {
                Result = Target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (Target.fcBehindWall() && Source.fcWallInBetween(Target.FPosition) && !Source.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // AoWAb.PAS:2257
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target) => true;

    // AoWAb.PAS:2216
    public SelfDestructCA CreateSelfDestructCA(CombatObject Source, CombatObject Target)
    {
        SelfDestructCA Result = new SelfDestructCA(null);
        Result.Generate(Source, Target, 8, 6, Set<DamageType>.Of(DamageType.Fire, DamageType.Wall));
        return Result;
    }

    private static readonly string SelfDestructRStr = "Self Destruct";
}
