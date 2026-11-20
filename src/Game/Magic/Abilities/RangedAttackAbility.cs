#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (range, damage, attack, packed damage types, repeats)
public partial class RangedAttackAbility : AdjustableAbility
{
    // AoWAb.PAS:4653
    public RangedAttackAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
        AbilityType = AbilityType.Combat;
    }

    public CombatRange RangeRangedAttack { get; set; }
    public sbyte DamageRangedAttack { get; set; }
    public sbyte AttackRangedAttack { get; set; }

    public Set<DamageType> DamageTypesRangedAttack { get; set; }

    public sbyte AttackRepeatRangedAttack { get; set; }

    // AoWAb.PAS:4694
    public override CombatMode GetCombatMode(AbilityOwner owner) => CombatRangeToCombatMode(GetRangeRangedAttack(owner));

    // AoWAb.PAS:4698
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner owner)
    {
        return GetDamageTypesRangedAttack(owner).Contains(DamageType.Wall)
            ? Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate)
            : Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.Participate);
    }

    // AoWAb.PAS:4659
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4832
    public override bool FastBattleValidRoundDistance(CombatObject source, CombatObject target)
    {
        int distance = source.Combat.GetRoundDistance();
        CombatRange range = GetRangeRangedAttack(source.GetAbilityOwner());
        if (range == CombatRange.Long)
        {
            distance += 4;
        }
        else
        {
            if (range == CombatRange.Medium)
            {
                distance += 2;
            }
            else
            {
                if (range == CombatRange.Short)
                {
                    distance += 1;
                }
            }
        }

        return distance >= target.FastBattleRoundDistance();
    }

    // AoWAb.PAS:4808
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        AbilityDamageValues values = default;
        for (int i = 0, last = commands.TargetSide.GetCount() - 1; i <= last; i++)
        {
            CombatObject target = commands.TargetSide.GetObjects(i);
            if (target.GetEnabled() && FastBattleValidRoundDistance(commands.Source, target))
            {
                FastBattleGetDamageValues(wallSituation: commands.Source.Combat.GetWallSituation(commands.Source, target), attacker: commands.Source, defender: target, damageValues: ref values);
                if (values.SourceDamage != 0)
                {
                    FastBattleCommand command = new(AbilityId)
                    {
                        Target = commands.TargetSide.GetObjects(i),
                        MoveCost = 1,
                        ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target)
                    };
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:4787
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        int penalty = source.Combat.GetWallSituation(source, target) == WallSituation.Target ? 2 : 0;

        if (target.GetEnabled())
        {
            for (int i = GetAttackRepeatRangedAttack(source.GetAbilityOwner()) - 1; i >= 0; i--)
            {
                RangedAttackCombatAction action = CreateRangedAttackCombatAction(source, target, penalty);
                source.Combat.ExecuteCombatAction(action);
                _ = action.Release();
            }

            source.State += Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.Vulnerable);
        }

        return true;
    }

    public override CombatAction CreateRangedCombatAction(CombatUnit source, CombatUnit target) =>
        CreateRangedAttackCombatAction(source, target: target, attackPenalty: 0);

    // AoWAb.PAS:4708
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        sbyte hitPoints = defender.GetHitPoints();
        sbyte defense = defender.GetDefense();
        sbyte resistance = defender.GetResistance();
        Set<DamageType> immunityTypes = defender.GetImmunityTypes();
        Set<DamageType> protectionTypes = defender.GetProtectionTypes();
        Set<DamageType> damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker);
        sbyte damageRangedAttack = GetDamageRangedAttack(attacker);
        int result = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker), damageRangedAttack, damageTypesRangedAttack, hitPoints, defense, resistance, immunityTypes, protectionTypes);
        int repeats = GetAttackRepeatRangedAttack(attacker);
        return result * repeats;
    }

    // AoWAb.PAS:4717
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        int defense = defender.GetDefense();
        if (wallSituation == WallSituation.Target)
        {
            defense += 2;
        }

        sbyte hitPoints = defender.GetHitPoints();
        sbyte resistance = defender.GetResistance();
        Set<DamageType> immunityTypes = defender.GetImmunityTypes();
        Set<DamageType> protectionTypes = defender.GetProtectionTypes();
        Set<DamageType> damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker);
        sbyte damageRangedAttack = GetDamageRangedAttack(attacker);
        int damage = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker), damageRangedAttack, damageTypesRangedAttack, hitPoints, defense, resistance, immunityTypes, protectionTypes);
        int repeats = GetAttackRepeatRangedAttack(attacker);
        damage *= repeats;
        damageValues.SourceDamage = damage;
        damageValues.TargetDamage = 0;
        Set<DamageType> damageTypesRangedAttack2 = GetDamageTypesRangedAttack(attacker);
        damageValues.DamageTypes = damageTypesRangedAttack2;
    }

    // AoWAb.PAS:4703
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        sbyte attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        sbyte damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), damageRangedAttack, attackRepeatRangedAttack);
        types = GetDamageTypesRangedAttack(owner);
    }

    // AoWAb.PAS:4732
    // { Original leaves both cost fields untouched. }
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        CombatRange range = GetRangeRangedAttack(attacker.GetAbilityOwner());
        if (wallSituation == WallSituation.None || Set<CombatRange>.Of(CombatRange.Medium, CombatRange.Long).Contains(range) || attacker.GetAbilityEnabled(1))
        {
            Set<DamageType> protectionTypes = defender.GetProtectionTypes();
            Set<DamageType> immunityTypes = defender.GetImmunityTypes();
            Set<DamageType> damageTypes = GetDamageTypesRangedAttack(attacker.GetAbilityOwner());
            int damage = GetDamageRangedAttack(attacker.GetAbilityOwner());
            int defense = defender.GetDefense();
            if (wallSituation == WallSituation.Target)
            {
                defense += 2;
            }

            Set<DamageType> damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker.GetAbilityOwner());
            damageValues.DamageTypes = damageTypesRangedAttack;
            sbyte hitPoints = defender.GetHitPoints();
            sbyte resistance = defender.GetResistance();
            int value = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker.GetAbilityOwner()), damage, damageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes);
            int repeats = GetAttackRepeatRangedAttack(attacker.GetAbilityOwner());
            value *= repeats;
            damageValues.SourceDamage = value;
            // { Original swaps protection and immunity for maximum damage. }
            value = AbilityRules.StatisticsToMaxDamage(damage, damageTypes, protectionTypes, immunityTypes);
            repeats = GetAttackRepeatRangedAttack(attacker.GetAbilityOwner());
            value *= repeats;
            damageValues.SourceCost = value;
            damageValues.TargetDamage = 0;
            damageValues.TargetCost = 0;
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

    // AoWAb.PAS:4769
    public override void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        sbyte attackRangedAttack = GetAttackRangedAttack(owner);
        info.Attack = attackRangedAttack;
        sbyte damageRangedAttack = GetDamageRangedAttack(owner);
        info.Damage = damageRangedAttack;
        Set<DamageType> damageTypesRangedAttack = GetDamageTypesRangedAttack(owner);
        info.DamageTypes = damageTypesRangedAttack;
        sbyte attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        info.RepeatCount = attackRepeatRangedAttack;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:4661
    public virtual CombatRange GetRangeRangedAttack(AbilityOwner owner) => RangeRangedAttack;

    // AoWAb.PAS:4665
    public virtual sbyte GetDamageRangedAttack(AbilityOwner owner)
    {
        sbyte result = DamageRangedAttack;
        if (owner is not null && owner.GetOwnAbilityEnabled(32))
        {
            switch (owner.GetOwnAbilityLevel(32))
            {
                case 2:
                    return (sbyte)(result + 1);
                case 3:
                    return (sbyte)(result + 1);
                case 4:
                    return (sbyte)(result + 2);
                default:
                    break;
            }
        }

        return result;
    }

    // AoWAb.PAS:4675
    public virtual sbyte GetAttackRangedAttack(AbilityOwner owner)
    {
        sbyte result = AttackRangedAttack;
        if (owner is not null && owner.GetOwnAbilityEnabled(32))
        {
            switch (owner.GetOwnAbilityLevel(32))
            {
                case 1:
                    return (sbyte)(result + 1);
                case 2:
                    return (sbyte)(result + 1);
                case 3:
                    return (sbyte)(result + 2);
                case 4:
                    return (sbyte)(result + 2);
                default:
                    break;
            }
        }

        return result;
    }

    // AoWAb.PAS:4686
    public virtual sbyte GetAttackRepeatRangedAttack(AbilityOwner owner) => AttackRepeatRangedAttack;

    // AoWAb.PAS:4690
    public virtual Set<DamageType> GetDamageTypesRangedAttack(AbilityOwner owner) => DamageTypesRangedAttack;

    // AoWAb.PAS:4777
    public RangedAttackCombatAction CreateRangedAttackCombatAction(CombatObject source, CombatObject target, int attackPenalty)
    {
        RangedAttackCombatAction action = new(null) { Holder = CreationHolder() };
        action.Generate(damage: GetDamageRangedAttack(source.GetAbilityOwner()), damageTypes: GetDamageTypesRangedAttack(source.GetAbilityOwner()), source: source, target: target, attack: (sbyte)(GetAttackRangedAttack(source.GetAbilityOwner()) - attackPenalty));
        action.AbilityId = AbilityId;
        return action;
    }

    private static CombatMode CombatRangeToCombatMode(CombatRange range)
    {
        return range switch
        {
            CombatRange.Long => CombatMode.LongRange,
            CombatRange.Medium => CombatMode.MediumRange,
            CombatRange.Short => CombatMode.ShortRange,
            CombatRange.Touch => CombatMode.Touch,
            _ => CombatMode.None,
        };
    }

    public enum CombatRange : byte
    {
        Touch = 0, Short = 1, Medium = 2, Long = 3
    }
}
