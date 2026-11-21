#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

namespace Aow.Game.Magic.Abilities;

public class TouchAbility : AdjustableAbility
{
    // AoWAb.PAS:1843
    public TouchAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes |= AbilitySelectionTypes.UseItem;
        AbilityType = AbilityType.Combat;
    }

    // AoWAb.PAS:1875
    public override CombatMode GetCombatMode(AbilityOwner owner) => CombatMode.Touch;

    // AoWAb.PAS:1850
    public override AbilityControlTypes GetControlType(AbilityOwner owner) => AbilityControlTypes.CombatAction;

    // AoWAb.PAS:1962
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        AbilityDamageValues values = default;
        for (int i = 0, last = commands.TargetSide.GetCount() - 1; i <= last; i++)
        {
            CombatObject target = commands.TargetSide.GetObjects(i);
            if (target.GetEnabled() && CanTouch(commands.Source, target) && FastBattleValidRoundDistance(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, defender: target, wallSituation: WallSituation.None, damageValues: ref values);
                if (values.SourceDamage != 0)
                {
                    FastBattleCommand command = new(AbilityId)
                    {
                        Target = commands.TargetSide.GetObjects(i),
                        SourceDamageValue = values.SourceDamage,
                        TargetDamageValue = values.TargetDamage,
                        ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, targetDamageValue: values.TargetDamage, commands.Source, target),
                        MoveCost = values.TargetDamage == 0 ? 1 : 2
                    };

                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:1860
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        int level = GetLevel(owner);
        sbyte attack = GetTouchAttack(level);
        info.Attack = attack;
        if (attack < 0)
        {
            info.Attack = 0;
        }

        info.Damage = -1;
        info.DamageTypes = DamageTypes.None;
        info.RepeatCount = 1;
        info.TargetTypes = CombatTargetTypes.Enemy;
    }

    // AoWAb.PAS:1855
    public virtual sbyte GetTouchAttack(int level) => -10;

    // AoWAb.PAS:1892
    public virtual bool CombatTouchRole(CombatUnit source, CombatObject target)
    {
        bool result = HitChance.HitRole(source.GetAttack() - target.GetDefense());
        int attack = GetTouchAttack(GetLevel(source.Troop));
        return result && attack != -10 && result && HitChance.HitRole(attack - target.GetResistance());
    }

    // AoWAb.PAS:1904
    public virtual float CombatTouchRoleProbability(CombatUnit source, CombatObject target)
    {
        float result = HitChance.HitRoleProbability(source.GetAttack() - target.GetDefense());
        int attack = GetTouchAttack(GetLevel(source.Troop));
        return attack != -10 ? (float)((double)result * HitChance.HitRoleProbability(attack - target.GetResistance())) : 0;
    }

    // AoWAb.PAS:1880
    public virtual float TouchRoleProbability(Troop source, Troop target)
    {
        float result = HitChance.HitRoleProbability(source.GetAttack() - target.GetDefense());
        int attack = GetTouchAttack(GetLevel(source));
        return attack != -10 ? (float)((double)result * HitChance.HitRoleProbability(attack - target.GetResistance())) : 0;
    }

    // AoWAb.PAS:1916
    public virtual bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = source.Side == CombatSideIndex.Defender
            ? !source.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || target.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing)
            : !target.FastBattleBehindWall() || !source.FastBattleWallInBetween(target.Position) || source.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing);
        return source.Side != target.Side ? result && (!target.GetAbilityEnabled(1) || source.GetAbilityEnabled(1)) : result;
    }

    // AoWAb.PAS:1937
    public virtual bool CanTouchUnit(Troop source, Troop target) => !target.GetAbilityEnabled(1) || source.GetAbilityEnabled(1);

    // AoWAb.PAS:1947
    public void FastBattleValidateMoveOutsideWall(CombatObject source, CombatObject target)
    {
        if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !target.WallCombatFeatures().Contains(WallCombatFeatures.WallPassing))
        {
            MoveWallCombatAction action = new(null)
            {
                Holder = CreationHolder(),
                CombatObjectId = source.Id,
                InsideWall = 0
            };
            source.Combat.ExecuteCombatAction(action);
            _ = action.Release();
        }
    }
}
