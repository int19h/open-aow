#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Abilities;

public class BreathAbility : RangedAttackAbility
{
    // AoWAb.PAS:4852
    public BreathAbility(CustomNode? owner) : base(owner)
    {
        RangeRangedAttack = CombatRange.Short;
        AttackRepeatRangedAttack = 1;
        AttackRangedAttack = 7;
        DamageRangedAttack = 5;
    }

    // AoWAb.PAS:4894
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand? command)
    {
        bool result = base.FastBattleExecuteCombatCommand(source, command);
        if (result)
        {
            Battle combat = source.Combat;
            DelphiList<CombatObject> targets = [];
            CombatObject? target = command.Target;
            CombatObject? other;
            for (int i = combat.Sides[(int)target.Side].GetCount() - 1; i >= 0; i--)
            {
                CombatSideIndex side = target.Side;
                other = combat.Sides[(int)side].GetObjects(i);
                bool condition = other != target && other.GetEnabled() && other.Position >>> 4 == target.Position >>> 4 && other.FastBattleRoundDistance() <= combat.RoundDistance;
                if (condition)
                {
                    DamageTypes immunityTypes = other.GetImmunityTypes();
                    condition = (DamageTypesRangedAttack & ~immunityTypes) != DamageTypes.None;
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
                RangedAttackCombatAction action = CreateRangedAttackCombatAction(source, target: other, attackPenalty: 2);
                source.Combat.ExecuteCombatAction(action);
                _ = action.Release();
                _ = targets.Remove(other);
                count -= 1;
            }
        }

        return result;
    }

    // AoWAb.PAS:4860
    public override void GetOffensiveStrength(AbilityOwner? owner, ref int strength, ref DamageTypes types)
    {
        int attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        int damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), defense: damageRangedAttack, damage: attackRepeatRangedAttack);
        strength *= 2;
        types = GetDamageTypesRangedAttack(owner);
    }

    // AoWAb.PAS:4866
    public override void FastBattleGetDamageValues(CombatObject? attacker, CombatObject? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        base.FastBattleGetDamageValues(attacker, defender, wallSituation, ref damageValues);
        if (damageValues.SourceDamage != 0)
        {
            Battle combat = attacker.Combat;
            int count = 0;
            for (int i = combat.Sides[(int)defender.Side].GetCount() - 1; i >= 0; i--)
            {
                CombatSideIndex side = defender.Side;
                CombatObject target = combat.Sides[(int)side].GetObjects(i);
                bool condition = target != defender && target.GetEnabled() && target.Position >>> 4 == defender.Position >>> 4 && target.FastBattleRoundDistance() <= combat.RoundDistance;
                if (condition)
                {
                    DamageTypes immunityTypes = target.GetImmunityTypes();
                    condition = (DamageTypesRangedAttack & ~immunityTypes) != DamageTypes.None;
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
    public override void TacticalBattleGetDamageValues(CombatObject? attacker, CombatObject? defender, ref AbilityDamageValues damageValues) => base.FastBattleGetDamageValues(attacker, defender, wallSituation: WallSituation.None, ref damageValues);
}
