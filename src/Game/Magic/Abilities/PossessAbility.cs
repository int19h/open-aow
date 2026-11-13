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
    public PossessAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 106;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_possessText);
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }

    // AoWAb.PAS:2737
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        PossessCombatAction action;
        StrikeCombatAction strike;
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        action = CreatePossessCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (!action.Successful)
        {
            strike = CombatObject.CreateStrikeCombatAction(target, source, true);
            if (strike != null)
            {
                source.Combat.ExecuteCombatAction(strike);
                strike.Release();
            }
        }

        action.Release();
        return true;
    }

    // AoWAb.PAS:2673
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        float probability;
        if (CanTouchUnit(attacker, defender) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            result = Round((double)((double)defender.GetHitPoints() * probability) * 200);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:2686
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, defender) && !defender.GetImmunityTypes().Contains(DamageType.None))
        {
            probability = TouchRoleProbability(attacker, defender);
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 200);
            if (attacker.GetAbilityEnabled(106))
            {
                damageValues.TargetDamage = 0;
            }
            else
            {
                var attackerHitPoints = attacker.GetHitPoints();
                damageValues.TargetDamage = Round((double)((double)attackerHitPoints * probability) * 100);
            }

            // { Original leaves both cost fields untouched on success. }
            damageValues.DamageTypes = Set<DamageType>.Of();
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

    // AoWAb.PAS:2664
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        if (owner is Troop troop)
        {
            strength = Round((double)(troop.GetAttack() * 25) * 0.1);
            types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:2711
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        float probability;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            probability = CombatTouchRoleProbability((CombatUnit)attacker, defender);
            var hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 200);
            var defenderHitPoints = defender.GetHitPoints();
            damageValues.SourceCost = defenderHitPoints * 2;
            if (attacker.GetAbilityEnabled(106))
            {
                damageValues.TargetDamage = 0;
            }
            else
            {
                var attackerHitPoints = attacker.GetHitPoints();
                damageValues.TargetDamage = Round((double)((double)attackerHitPoints * probability) * 100);
            }

            var damage = defender.GetDamage();
            damageValues.TargetCost = damage;
            damageValues.DamageTypes = Set<DamageType>.Of();
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

    // AoWAb.PAS:2635
    public override sbyte GetTouchAttack(int level) => 5;

    // AoWAb.PAS:2646
    public override bool CanTouch(CombatObject source, CombatObject target) => base.CanTouch(source, target) && (source is CombatUnit && target is CombatUnit && !target.GetAbilityEnabled(106) && ((CombatUnit)target).Troop.GetUnitType() != UnitType.Machine && !(((CombatUnit)target).Troop is Leader));

    // AoWAb.PAS:2656
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        // { Original excludes humanoids here, but machines in CanTouch. }
        return result && !target.GetAbilityEnabled(106) && target.GetUnitType() != UnitType.Humanoid && !(target is Leader);
    }

    // AoWAb.PAS:2640
    public PossessCombatAction CreatePossessCombatAction(CombatObject source, CombatObject target)
    {
        PossessCombatAction result = new PossessCombatAction(null) { Holder = CreationHolder() };
        var combatTouchRole = CombatTouchRole((CombatUnit)source, target);
        result.Initialize(source.Id, target.Id, combatTouchRole);
        return result;
    }

    private static readonly string s_possessText = "Possess";
}
