// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class BreathAbility : RangedAttackAbility
{
    // AoWAb.PAS:4852
    public BreathAbility(ECustomNode owner) : base(owner)
    {
        RangeRangedAttack = CombatRange.Short;
        AttackRepeatRangedAttack = 1;
        AttackRangedAttack = 7;
        DamageRangedAttack = 5;
    }

    // AoWAb.PAS:4894
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        Combat.Battle combat;
        DelphiList targets;
        CombatObject target;
        CombatObject other;
        int count;
        RangedAttackCombatAction action;
        bool result = base.FastBattleExecuteCombatCommand(source, command);
        if (result)
        {
            combat = source.Combat;
            targets = new DelphiList();
            target = command.Target;
            for (int I = combat.Sides[(int)target.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                var side = target.GetSide();
                other = combat.Sides[(int)side].GetObjects(I);
                bool condition = other != target && other.GetEnabled() && (int)(((uint)other.Position) >> 4) == (int)(((uint)target.Position) >> 4) && other.FastBattleRoundDistance() <= combat.GetRoundDistance();
                if (condition)
                {
                    var immunityTypes = other.GetImmunityTypes();
                    condition = DamageTypesRangedAttack.Value - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
                {
                    targets.Add(other);
                }
            }

            count = AoWHSMap.Current.Random(2) + 1;
            while (count != 0 && targets.Count != 0)
            {
                other = (CombatObject)targets[AoWHSMap.Current.Random(targets.Count)];
                action = CreateRangedAttackCombatAction(source, other, 2);
                source.Combat.ExecuteCombatAction(action);
                action.Release();
                targets.Remove(other);
                count -= 1;
            }

            targets?.Dispose();
        }

        return result;
    }

    // AoWAb.PAS:4860
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        var attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        var damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), damageRangedAttack, attackRepeatRangedAttack);
        strength = strength * 2;
        types = GetDamageTypesRangedAttack(owner);
    }

    // AoWAb.PAS:4866
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        int count;
        Combat.Battle combat;
        CombatObject target;
        base.FastBattleGetDamageValues(attacker, defender, wallSituation, ref damageValues);
        if (damageValues.SourceDamage != 0)
        {
            combat = attacker.Combat;
            count = 0;
            for (int I = combat.Sides[(int)defender.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                var side = defender.GetSide();
                target = combat.Sides[(int)side].GetObjects(I);
                bool condition = target != defender && target.GetEnabled() && (int)(((uint)target.Position) >> 4) == (int)(((uint)defender.Position) >> 4) && target.FastBattleRoundDistance() <= combat.GetRoundDistance();
                if (condition)
                {
                    var immunityTypes = target.GetImmunityTypes();
                    condition = DamageTypesRangedAttack.Value - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
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
    }

    // AoWAb.PAS:4930
    public override void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues)
    {
        base.FastBattleGetDamageValues(attacker, defender, WallSituation.None, ref damageValues);
    }
}
