#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public class RoundAttackAbility : TouchAbility
{
    // AoWAb.PAS:2262
    public RoundAttackAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 114;
        AbilityType = AbilityType.Combat;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_roundAttackText);
    }

    // AoWAb.PAS:2404
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        Battle combat = source.Combat;
        DelphiList<CombatObject> targets = new();
        DamageTypes damageTypes = CombatObject.MeleeDamageTypes(source);
        CombatObject target = command.Target;
        FastBattleValidateMoveOutsideWall(source, target);
        if (source is FastBattleUnit fastUnit)
        {
            fastUnit.RoundDistance = 0;
            if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !target.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing))
            {
                MoveWallCombatAction moveAction = new(null)
                {
                    Holder = CreationHolder(),
                    CombatObjectId = source.Id,
                    InsideWall = 0
                };
                source.Combat.ExecuteCombatAction(moveAction);
                _ = moveAction.Release();
            }
        }

        StrikeCombatAction action = CombatObject.CreateStrikeCombatAction(source, target, defensive: false);
        if (action is not null)
        {
            combat.ExecuteCombatAction(action);
            _ = action.Release();
        }

        CombatObject other;
        for (int i = combat.Sides[(int)target.Side].GetCount() - 1; i >= 0; i--)
        {
            CombatSideIndex side = target.Side;
            other = combat.Sides[(int)side].GetObjects(i);
            if (other != target && other.GetEnabled() && other.Position >>> 4 == target.Position >>> 4 && (!other.GetAbilityEnabled(1) || source.GetAbilityEnabled(1)) && !source.FastBattleWallInBetween(other.Position) && other.FastBattleRoundDistance() <= combat.Round - 1 && (damageTypes & ~other.GetImmunityTypes()) != DamageTypes.None)
            {
                _ = targets.Add(other);
            }
        }

        int count = StrategicMap.Current.Random(2) + 1;
        while (count != 0 && targets.Count != 0)
        {
            other = targets[StrategicMap.Current.Random(targets.Count)];
            action = CombatObject.CreateStrikeCombatAction(source, target: other, defensive: false);
            if (action is not null)
            {
                combat.ExecuteCombatAction(action);
                _ = action.Release();
            }

            _ = targets.Remove(other);
            count -= 1;
        }

        targets?.Dispose();
        return true;
    }

    // AoWAb.PAS:2286
    public override int GetDamageValue(Troop attacker, Troop defender) => CanTouchUnit(attacker, target: defender) ? Troop.UnitStrikeDamageValue(attacker, target: defender) * 2 : 0;

    // AoWAb.PAS:2318
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouchUnit(attacker, target: defender))
        {
            int unitStrikeDamageValue = Troop.UnitStrikeDamageValue(attacker, target: defender);
            damageValues.SourceDamage = unitStrikeDamageValue * 2;
            damageValues.SourceCost = 0;
            int unitStrikeDamageValueLocal = Troop.UnitStrikeDamageValue(attacker, target: defender);
            damageValues.TargetDamage = unitStrikeDamageValueLocal;
            damageValues.TargetCost = 0;
            DamageTypes damageTypes = GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).GetDamageTypes(attacker);
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = damageTypes & ~immunityTypes;
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

    // AoWAb.PAS:2270
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref DamageTypes types)
    {
        if (owner is Troop troop)
        {
            int attack = troop.GetAttack();
            int damage = troop.GetDamage();

            strength = AbilityRules.LimitedDamage(attack, defense: damage, damage: 1) * 3;
            types = GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).GetDamageTypes(owner);
        }
    }

    // AoWAb.PAS:2338
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        if (wallSituation == WallSituation.None && CanTouch(attacker, target: defender))
        {
            DamageTypes damageTypes = CombatObject.MeleeDamageTypes(attacker);
            int strikeDamageValue = CombatObject.StrikeDamageValue(attacker, target: defender);
            damageValues.SourceDamage = strikeDamageValue;
            sbyte damage = attacker.GetDamage();
            damageValues.SourceCost = damage;
            int strikeDamageValueLocal = CombatObject.StrikeDamageValue(defender, target: attacker);
            damageValues.TargetDamage = strikeDamageValueLocal;
            sbyte defenderDamage = defender.GetDamage();
            damageValues.TargetCost = defenderDamage;
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = damageTypes & ~immunityTypes;
            Battle combat = attacker.Combat;
            int count = 0;
            for (int i = combat.Sides[(int)defender.Side].GetCount() - 1; i >= 0; i--)
            {
                // { Preserve the original saved registers. }
                CombatSideIndex side = defender.Side;
                CombatObject target = combat.Sides[(int)side].GetObjects(i);
                // { Original tests Defender's flying ability here, rather than Target's. }
                if (target != defender && target.GetEnabled() && target.Position >>> 4 == defender.Position >>> 4 && (!defender.GetAbilityEnabled(1) || attacker.GetAbilityEnabled(1)) && !attacker.FastBattleWallInBetween(target.Position) && target.FastBattleRoundDistance() <= combat.RoundDistance && (damageTypes & ~target.GetImmunityTypes()) != DamageTypes.None)
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
            damageValues.DamageTypes = DamageTypes.None;
        }
    }

    // AoWAb.PAS:2383
    public override void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues)
    {
        if (CanTouch(attacker, target: defender))
        {
            int strikeDamageValue = CombatObject.StrikeDamageValue(attacker, target: defender);
            damageValues.SourceDamage = strikeDamageValue * 2;
            sbyte damage = attacker.GetDamage();
            damageValues.SourceCost = damage * 2;
            // { Original repeats the forward strike instead of reversing the arguments. }
            int strikeDamageValueLocal = CombatObject.StrikeDamageValue(attacker, target: defender);
            damageValues.TargetDamage = strikeDamageValueLocal;
            sbyte defenderDamage = defender.GetDamage();
            damageValues.TargetCost = defenderDamage;
            DamageTypes meleeDamageTypes = CombatObject.MeleeDamageTypes(attacker);
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            damageValues.DamageTypes = meleeDamageTypes & ~immunityTypes;
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

    // AoWAb.PAS:2294
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        if (owner is Troop source)
        {
            sbyte attack = source.GetAttack();
            info.Attack = attack;
            sbyte damage = source.GetDamage();
            info.Damage = damage;
            DamageTypes damageTypes = GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).GetDamageTypes(source);
            info.DamageTypes = damageTypes;
            info.RepeatCount = 2;
            info.TargetTypes = CombatTargetTypes.Enemy;
            return;
        }
        else
        {
            info.Attack = -1;
            info.Damage = -1;
            info.DamageTypes = DamageTypes.Physical;
            info.RepeatCount = 2;
            info.TargetTypes = CombatTargetTypes.Enemy;
        }
    }

    private static readonly string s_roundAttackText = "Round Attack";
}
