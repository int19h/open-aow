// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class WebAbility : TouchAbility
{
    // AoWAb.PAS:2806
    public WebAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 27;
        FAbilityName = AbilityRules.TranslateRStr(WebRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:2901
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        WebCA Action;
        StrikeCA Strike;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreateWebCA(Source, Target);
        Source.FCombat.ExecuteCombatAction(Action);
        if (!Action.FSuccessful)
        {
            Strike = CombatObject.CreateStrikeCA(Target, Source, true);
            if (Strike != null)
            {
                Source.FCombat.ExecuteCombatAction(Strike);
                Strike.Release();
            }
        }

        Action.Release();
        return true;
    }

    // AoWAb.PAS:2842
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        float Probability;
        if ((!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            Result = Round((double)((double)Defender.GetHitPoints() * Probability) * 100);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:2855
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        if (WallSituation == WallSituation.None && (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceDamage = Round((double)((double)hitPoints * Probability) * 100);
            var defenderHitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = defenderHitPoints;
            var p4UnitStrikeDV = AbstractUnit.P4UnitStrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)p4UnitStrikeDV * ((double)1 - Probability));
            var damage = Defender.GetDamage();
            DamageValues.TargetCost = damage;
            DamageValues.DamageTypes = Set<DamageType>.Of();
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

    // AoWAb.PAS:2833
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        if (Owner is AbstractUnit unit)
        {
            Strength = Round((double)(unit.GetAttack() * 4 * 4) * 0.1);
            Types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:2878
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            Probability = CombatTouchRoleProbability((CombatUnit)Attacker, Defender);
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceDamage = Round((double)((double)hitPoints * Probability) * 100);
            var defenderHitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = defenderHitPoints;
            var strikeDV = CombatObject.StrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)strikeDV * ((double)1 - Probability));
            var protectionTypes = Attacker.GetProtectionTypes();
            var immunityTypes = Attacker.GetImmunityTypes();
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(Defender);
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(Defender.GetDamage(), meleeDamageTypes, immunityTypes, protectionTypes);
            DamageValues.TargetCost = statisticsToMaxDamage;
            DamageValues.DamageTypes = Set<DamageType>.Of();
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

    // AoWAb.PAS:2814
    public override sbyte GetTouchAttack(int Level) => 4;

    // AoWAb.PAS:2825
    public override bool CanTouch(CombatObject Source, CombatObject Target) => base.CanTouch(Source, Target) && (Source is CombatUnit && Target is CombatUnit && !Target.GetAbilityEnabled(97));

    // AoWAb.PAS:2819
    public WebCA CreateWebCA(CombatObject Source, CombatObject Target)
    {
        WebCA Result = new WebCA(null);
        var combatTouchRole = CombatTouchRole((CombatUnit)Source, Target);
        Result.Initialize(Source.FID, Target.FID, combatTouchRole);
        return Result;
    }

    private static readonly string WebRStr = "Web";
}
