// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class RoundAttackAbility : TouchAbility
{
    // AoWAb.PAS:2262
    public RoundAttackAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 114;
        AbilityType = AbilityType.Combat;
        AbilityName = AbilityRules.TranslateResourceString(s_roundAttackText);
    }

    // AoWAb.PAS:2404
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        Set<DamageType> damageTypes;
        int count;
        StrikeCombatAction action;
        Combat.Battle combat;
        DelphiList<CombatObject> targets;
        CombatObject target;
        CombatObject other;
        MoveWallCombatAction moveAction;
        combat = source.Combat;
        targets = new DelphiList<CombatObject>();
        damageTypes = CombatObject.MeleeDamageTypes(source);
        target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        if (source is FastBattleUnit)
        {
            ((FastBattleUnit)source).RoundDistance = 0;
            if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                moveAction = new MoveWallCombatAction(null);
                moveAction.CombatObjectId = source.Id;
                moveAction.InsideWall = 0;
                source.Combat.ExecuteCombatAction(moveAction);
                moveAction.Release();
            }
        }

        action = CombatObject.CreateStrikeCombatAction(source, target, false);
        if (action != null)
        {
            combat.ExecuteCombatAction(action);
            action.Release();
        }

        for (int I = combat.Sides[(int)target.GetSide()].GetCount() - 1; I >= 0; I--)
        {
            var side = target.GetSide();
            other = combat.Sides[(int)side].GetObjects(I);
            if (other != target && other.GetEnabled() && (int)(((uint)other.Position) >> 4) == (int)(((uint)target.Position) >> 4) && (!other.GetAbilityEnabled(1) || source.GetAbilityEnabled(1)) && !source.FastBattleWallInBetween(other.Position) && other.FastBattleRoundDistance() <= combat.GetRound() - 1 && damageTypes - other.GetImmunityTypes() != Set<DamageType>.Of())
            {
                targets.Add(other);
            }
        }

        count = StrategicMap.Current.Random(2) + 1;
        while (count != 0 && targets.Count != 0)
        {
            other = targets[StrategicMap.Current.Random(targets.Count)];
            action = CombatObject.CreateStrikeCombatAction(source, other, false);
            if (action != null)
            {
                combat.ExecuteCombatAction(action);
                action.Release();
            }

            targets.Remove(other);
            count -= 1;
        }

        targets?.Dispose();
        return true;
    }

    // AoWAb.PAS:2286
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int result;
        if (CanTouchUnit(attacker, defender))
        {
            result = Troop.UnitStrikeDamageValue(attacker, defender) * 2;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:2318
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, defender))
        {
            var unitStrikeDamageValue = Troop.UnitStrikeDamageValue(attacker, defender);
            damageValues.SourceDamage = unitStrikeDamageValue * 2;
            damageValues.SourceCost = 0;
            var unitStrikeDamageValueLocal = Troop.UnitStrikeDamageValue(attacker, defender);
            damageValues.TargetDamage = unitStrikeDamageValueLocal;
            damageValues.TargetCost = 0;
            var damageTypes = ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).GetDamageTypes(attacker);
            var immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = damageTypes - immunityTypes;
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

    // AoWAb.PAS:2270
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        int attack;
        int damage;
        if (owner is Troop troop)
        {
            attack = troop.GetAttack();
            damage = troop.GetDamage();

            strength = AbilityRules.LimitedDamage(attack, damage, 1) * 3;
            types = ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).GetDamageTypes(owner);
        }
    }

    // AoWAb.PAS:2338
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        int count;
        Set<DamageType> damageTypes;
        Combat.Battle combat;
        CombatObject target;
        if (wallSituation == WallSituation.None && CanTouch(attacker, defender))
        {
            damageTypes = CombatObject.MeleeDamageTypes(attacker);
            var strikeDamageValue = CombatObject.StrikeDamageValue(attacker, defender);
            damageValues.SourceDamage = strikeDamageValue;
            var damage = attacker.GetDamage();
            damageValues.SourceCost = damage;
            var strikeDamageValueLocal = CombatObject.StrikeDamageValue(defender, attacker);
            damageValues.TargetDamage = strikeDamageValueLocal;
            var defenderDamage = defender.GetDamage();
            damageValues.TargetCost = defenderDamage;
            var immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = damageTypes - immunityTypes;
            combat = attacker.Combat;
            count = 0;
            for (int I = combat.Sides[(int)defender.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                // { Preserve the original saved registers. }
                var side = defender.GetSide();
                target = combat.Sides[(int)side].GetObjects(I);
                // { Original tests Defender's flying ability here, rather than Target's. }
                if (target != defender && target.GetEnabled() && (int)(((uint)target.Position) >> 4) == (int)(((uint)defender.Position) >> 4) && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !attacker.FastBattleWallInBetween(target.Position) && target.FastBattleRoundDistance() <= combat.GetRoundDistance() && damageTypes - target.GetImmunityTypes() != Set<DamageType>.Of())
                {
                    count += 1;
                }
            }

            if (count != 0)
            {
                if (count > 3)
                {
                    count = 3;
                }

                damageValues.SourceDamage += Round((double)(damageValues.SourceDamage * count) / 3);
            }
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

    // AoWAb.PAS:2383
    public override void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues)
    {
        if (CanTouch(attacker, defender))
        {
            var strikeDamageValue = CombatObject.StrikeDamageValue(attacker, defender);
            damageValues.SourceDamage = strikeDamageValue * 2;
            var damage = attacker.GetDamage();
            damageValues.SourceCost = damage * 2;
            // { Original repeats the forward strike instead of reversing the arguments. }
            var strikeDamageValueLocal = CombatObject.StrikeDamageValue(attacker, defender);
            damageValues.TargetDamage = strikeDamageValueLocal;
            var defenderDamage = defender.GetDamage();
            damageValues.TargetCost = defenderDamage;
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(attacker);
            var immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = meleeDamageTypes - immunityTypes;
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

    // AoWAb.PAS:2294
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        Troop source;
        if (owner is Troop)
        {
            source = (Troop)owner;
            var attack = source.GetAttack();
            info.Attack = attack;
            var damage = source.GetDamage();
            info.Damage = damage;
            var damageTypes = ((StrikeAbility)GamePack.Current.AbilityControl.GetAbility(21)).GetDamageTypes(source);
            info.DamageTypes = damageTypes;
            info.RepeatCount = 2;
            info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
            return;
        }
        else
        {
            info.Attack = -1;
            info.Damage = -1;
            info.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            info.RepeatCount = 2;
            info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
        }
    }

    private static readonly string s_roundAttackText = "Round Attack";
}
