// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class PossessAbility : TouchAbility
{
    // AoWAb.PAS:2627
    public PossessAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 106;
        FAbilityName = AbilityRules.TranslateRStr(PossessRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:2737
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        PossessCA Action;
        StrikeCA Strike;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreatePossessCA(Source, Target);
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

    // AoWAb.PAS:2673
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        float Probability;
        if (CanTouchUnit(Attacker, Defender) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            Result = Round((double)((double)Defender.GetHitPoints() * Probability) * 200);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:2686
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        if (WallSituation == WallSituation.None && CanTouchUnit(Attacker, Defender) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceDamage = Round((double)((double)hitPoints * Probability) * 200);
            if (Attacker.GetAbilityEnabled(106))
            {
                DamageValues.TargetDamage = 0;
            }
            else
            {
                var attackerHitPoints = Attacker.GetHitPoints();
                DamageValues.TargetDamage = Round((double)((double)attackerHitPoints * Probability) * 100);
            }

            // { Original leaves both cost fields untouched on success. }
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

    // AoWAb.PAS:2664
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        if (Owner is AbstractUnit unit)
        {
            Strength = Round((double)(unit.GetAttack() * 25) * 0.1);
            Types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:2711
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            Probability = CombatTouchRoleProbability((CombatUnit)Attacker, Defender);
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceDamage = Round((double)((double)hitPoints * Probability) * 200);
            var defenderHitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = defenderHitPoints * 2;
            if (Attacker.GetAbilityEnabled(106))
            {
                DamageValues.TargetDamage = 0;
            }
            else
            {
                var attackerHitPoints = Attacker.GetHitPoints();
                DamageValues.TargetDamage = Round((double)((double)attackerHitPoints * Probability) * 100);
            }

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

    // AoWAb.PAS:2635
    public override sbyte GetTouchAttack(int Level) => 5;

    // AoWAb.PAS:2646
    public override bool CanTouch(CombatObject Source, CombatObject Target) => base.CanTouch(Source, Target) && (Source is CombatUnit && Target is CombatUnit && !Target.GetAbilityEnabled(106) && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine && !(((CombatUnit)Target).FUnit is Leader));

    // AoWAb.PAS:2656
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.CanTouchUnit(Source, Target);
        // { Original excludes humanoids here, but machines in CanTouch. }
        return Result && !Target.GetAbilityEnabled(106) && Target.GetUnitType() != UnitType.Humanoid && !(Target is Leader);
    }

    // AoWAb.PAS:2640
    public PossessCA CreatePossessCA(CombatObject Source, CombatObject Target)
    {
        PossessCA Result = new PossessCA(null);
        var combatTouchRole = CombatTouchRole((CombatUnit)Source, Target);
        Result.Initialize(Source.FID, Target.FID, combatTouchRole);
        return Result;
    }

    private static readonly string PossessRStr = "Possess";
}
