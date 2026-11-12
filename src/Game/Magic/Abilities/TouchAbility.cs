// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class TouchAbility : AdjustableAbility
{
    // AoWAb.PAS:1843
    public TouchAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = SelectionTypes + Set<AbilitySelectionType>.Of(AbilitySelectionType.UseItem);
        AbilityType = AbilityType.Combat;
    }

    // AoWAb.PAS:1875
    public override CombatMode GetCombatMode(AbilityOwner owner) => CombatMode.Touch;

    // AoWAb.PAS:1850
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:1962
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        CombatObject target;
        FastBattleCommand command;
        AbilityDamageValues values = default;
        for (int I = 0, last = commands.TargetSide.GetCount() - 1; I <= last; I++)
        {
            target = commands.TargetSide.GetObjects(I);
            if (target.GetEnabled() && CanTouch(commands.Source, target) && FastBattleValidRoundDistance(commands.Source, target))
            {
                FastBattleGetDamageValues(commands.Source, target, WallSituation.None, ref values);
                if (values.SourceDamage != 0)
                {
                    command = new FastBattleCommand(AbilityId);
                    command.Target = commands.TargetSide.GetObjects(I);
                    command.SourceDamageValue = values.SourceDamage;
                    command.TargetDamageValue = values.TargetDamage;
                    command.ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target);
                    if (values.TargetDamage == 0)
                    {
                        command.MoveCost = 1;
                    }
                    else
                    {
                        command.MoveCost = 2;
                    }

                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:1860
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        sbyte attack;
        int level;
        level = GetLevel(owner);
        attack = GetTouchAttack(level);
        info.Attack = attack;
        if (attack < 0)
        {
            info.Attack = 0;
        }

        info.Damage = -1;
        info.DamageTypes = Set<DamageType>.Of();
        info.RepeatCount = 1;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:1855
    public virtual sbyte GetTouchAttack(int level) => -10;

    // AoWAb.PAS:1892
    public virtual bool CombatTouchRole(CombatUnit source, CombatObject target)
    {
        int attack;
        bool result = HitChance.HitRole(source.GetAttack() - target.GetDefense());
        attack = GetTouchAttack(GetLevel(source.Troop));
        if (result && attack != -10)
        {
            result = result && HitChance.HitRole(attack - target.GetResistance());
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:1904
    public virtual float CombatTouchRoleProbability(CombatUnit source, CombatObject target)
    {
        int attack;
        float result = HitChance.HitRoleProbability(source.GetAttack() - target.GetDefense());
        attack = GetTouchAttack(GetLevel(source.Troop));
        if (attack != -10)
        {
            result = (float)((double)result * HitChance.HitRoleProbability(attack - target.GetResistance()));
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:1880
    public virtual float TouchRoleProbability(Troop source, Troop target)
    {
        int attack;
        float result = HitChance.HitRoleProbability(source.GetAttack() - target.GetDefense());
        attack = GetTouchAttack(GetLevel(source));
        if (attack != -10)
        {
            result = (float)((double)result * HitChance.HitRoleProbability(attack - target.GetResistance()));
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:1916
    public virtual bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result;
        if (source.GetSide() == CombatSideIndex.Defender)
        {
            if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position))
            {
                result = target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
            }
            else
            {
                result = true;
            }
        }
        else
        {
            if (target.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !source.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        if (source.GetSide() != target.GetSide())
        {
            result = result && (!target.GetAbilityEnabled(1) || source.GetAbilityEnabled(1));
        }

        return result;
    }

    // AoWAb.PAS:1937
    public virtual bool CanTouchUnit(Troop source, Troop target)
    {
        if (target.GetAbilityEnabled(1) && !source.GetAbilityEnabled(1))
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:1947
    public void FastBattleValidateMoveOutsideWall(CombatObject source, CombatObject target)
    {
        MoveWallCombatAction action;
        if (source.FastBattleBehindWall() && source.FastBattleWallInBetween(target.Position) && !target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
        {
            action = new MoveWallCombatAction(null);
            action.CombatObjectId = source.Id;
            action.InsideWall = 0;
            source.Combat.ExecuteCombatAction(action);
            action.Release();
        }
    }
}
