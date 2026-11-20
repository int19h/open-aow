#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public partial class BreathAbility : RangedAttackAbility
{
    // AoWAb.PAS:4852
    public BreathAbility(CustomNode owner) : base(owner)
    {
        RangeRangedAttack = CombatRange.Short;
        AttackRepeatRangedAttack = 1;
        AttackRangedAttack = 7;
        DamageRangedAttack = 5;
    }

    // AoWAb.PAS:4894
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        bool result = base.FastBattleExecuteCombatCommand(source, command);
        if (result)
        {
            Battle combat = source.Combat;
            DelphiList<CombatObject> targets = new();
            CombatObject target = command.Target;
            CombatObject other;
            for (int i = combat.Sides[(int)target.GetSide()].GetCount() - 1; i >= 0; i--)
            {
                CombatSideIndex side = target.GetSide();
                other = combat.Sides[(int)side].GetObjects(i);
                bool condition = other != target && other.GetEnabled() && (int)(((uint)other.Position) >> 4) == (int)(((uint)target.Position) >> 4) && other.FastBattleRoundDistance() <= combat.GetRoundDistance();
                if (condition)
                {
                    Set<DamageType> immunityTypes = other.GetImmunityTypes();
                    condition = DamageTypesRangedAttack - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
                {
                    _ = targets.Add(other);
                }
            }

            int count = StrategicMap.Current.Random(2) + 1;
            while (count != 0 && targets.Count != 0)
            {
                other = targets[StrategicMap.Current.Random(targets.Count)];
                RangedAttackCombatAction action = CreateRangedAttackCombatAction(source, other, 2);
                source.Combat.ExecuteCombatAction(action);
                _ = action.Release();
                _ = targets.Remove(other);
                count -= 1;
            }

            targets?.Dispose();
        }

        return result;
    }

    // AoWAb.PAS:4860
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        sbyte attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        sbyte damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), damageRangedAttack, attackRepeatRangedAttack);
        strength *= 2;
        types = GetDamageTypesRangedAttack(owner);
    }

    // AoWAb.PAS:4866
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        base.FastBattleGetDamageValues(attacker, defender, wallSituation, ref damageValues);
        if (damageValues.SourceDamage != 0)
        {
            Battle combat = attacker.Combat;
            int count = 0;
            for (int i = combat.Sides[(int)defender.GetSide()].GetCount() - 1; i >= 0; i--)
            {
                CombatSideIndex side = defender.GetSide();
                CombatObject target = combat.Sides[(int)side].GetObjects(i);
                bool condition = target != defender && target.GetEnabled() && (int)(((uint)target.Position) >> 4) == (int)(((uint)defender.Position) >> 4) && target.FastBattleRoundDistance() <= combat.GetRoundDistance();
                if (condition)
                {
                    Set<DamageType> immunityTypes = target.GetImmunityTypes();
                    condition = DamageTypesRangedAttack - immunityTypes != Set<DamageType>.Of();
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
    public override void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues) => base.FastBattleGetDamageValues(attacker, defender, WallSituation.None, ref damageValues);
}
