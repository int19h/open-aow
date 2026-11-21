#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public class PossessAbility : TouchAbility
{
    // AoWAb.PAS:2627
    public PossessAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 106;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_possessText);
        SelectionTypes = AbilitySelectionTypes.Unit;
    }

    // AoWAb.PAS:2737
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        PossessCombatAction action = CreatePossessCombatAction(source, target);
        source.Combat.ExecuteCombatAction(action);
        if (!action.Successful)
        {
            StrikeCombatAction strike = CombatObject.CreateStrikeCombatAction(target, target: source, defensive: true);
            if (strike is not null)
            {
                source.Combat.ExecuteCombatAction(strike);
                _ = strike.Release();
            }
        }

        _ = action.Release();
        return true;
    }

    // AoWAb.PAS:2673
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        if (CanTouchUnit(attacker, target: defender) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
        {
            float probability = TouchRoleProbability(attacker, target: defender);
            return Round((double)((double)defender.GetHitPoints() * probability) * 200);
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:2686
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, target: defender) && !defender.GetImmunityTypes().Contains(DamageTypes.NonSpecific))
        {
            float probability = TouchRoleProbability(attacker, target: defender);
            sbyte hitPoints = defender.GetHitPoints();
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 200);
            if (attacker.GetAbilityEnabled(106))
            {
                damageValues.TargetDamage = 0;
            }
            else
            {
                sbyte attackerHitPoints = attacker.GetHitPoints();
                damageValues.TargetDamage = Round((double)((double)attackerHitPoints * probability) * 100);
            }

            // { Original leaves both cost fields untouched on success. }
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

    // AoWAb.PAS:2664
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref DamageTypes types)
    {
        if (owner is Troop troop)
        {
            strength = Round(troop.GetAttack() * 25 * 0.1);
            types = DamageTypes.NonSpecific;
        }
    }

    // AoWAb.PAS:2711
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, target: defender) && attacker is CombatUnit attackerUnit)
        {
            float probability = CombatTouchRoleProbability(attackerUnit, target: defender);
            sbyte hitPoints = defender.HitPoints;
            damageValues.SourceDamage = Round((double)((double)hitPoints * probability) * 200);
            sbyte defenderHitPoints = defender.HitPoints;
            damageValues.SourceCost = defenderHitPoints * 2;
            if (attacker.GetAbilityEnabled(106))
            {
                damageValues.TargetDamage = 0;
            }
            else
            {
                sbyte attackerHitPoints = attacker.HitPoints;
                damageValues.TargetDamage = Round((double)((double)attackerHitPoints * probability) * 100);
            }

            sbyte damage = defender.GetDamage();
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

    // AoWAb.PAS:2635
    public override sbyte GetTouchAttack(int level) => 5;

    // AoWAb.PAS:2646
    public override bool CanTouch(CombatObject source, CombatObject target) => base.CanTouch(source, target) && source is CombatUnit && target is CombatUnit targetUnit && !target.GetAbilityEnabled(106) && targetUnit.Troop.GetUnitType() != UnitType.Machine && targetUnit.Troop is not Leader;

    // AoWAb.PAS:2656
    public override bool CanTouchUnit(Troop source, Troop target)
    {
        bool result = base.CanTouchUnit(source, target);
        // { Original excludes humanoids here, but machines in CanTouch. }
        return result && !target.GetAbilityEnabled(106) && target.GetUnitType() != UnitType.Humanoid && target is not Leader;
    }

    // AoWAb.PAS:2640. Draws the random value the touch event used to draw, then builds the action.
    public override CombatAction CreateTouchMoveCombatAction(CombatUnit source, CombatUnit target)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        return CreatePossessCombatAction(source, target: target);
    }

    public PossessCombatAction CreatePossessCombatAction(CombatObject source, CombatObject target)
    {
        PossessCombatAction result = new(null) { Holder = CreationHolder() };
        result.Initialize(successful: source is CombatUnit sourceUnit && CombatTouchRole(sourceUnit, target), sourceId: source.Id, targetId: target.Id);
        return result;
    }

    private static readonly string s_possessText = "Possess";
}
