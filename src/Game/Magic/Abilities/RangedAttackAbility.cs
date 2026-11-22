#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (range, damage, attack, packed damage types, repeats)
public class RangedAttackAbility : AdjustableAbility
{
    // AoWAb.PAS:4653
    public RangedAttackAbility(CustomNode? owner) : base(owner)
    {
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.UseItem;
        AbilityType = AbilityType.Combat;
    }

    public CombatRange RangeRangedAttack { get; init; }
    public int DamageRangedAttack { get; init; }
    public int AttackRangedAttack { get; init; }

    public DamageTypes DamageTypesRangedAttack { get; init; }

    public int AttackRepeatRangedAttack { get; init; }

    // AoWAb.PAS:4694
    public override CombatMode GetCombatMode(AbilityOwner? owner) => CombatRangeToCombatMode(GetRangeRangedAttack(owner));

    // AoWAb.PAS:4698
    public override WallCombatFeatures GetWallCombatFeatures(AbilityOwner owner)
    {
        return GetDamageTypesRangedAttack(owner).Contains(DamageTypes.Wall)
            ? WallCombatFeatures.WallBreaching | WallCombatFeatures.Participate
            : WallCombatFeatures.Participate;
    }

    // AoWAb.PAS:4659
    public override AbilityControlTypes GetControlType(AbilityOwner? owner) => AbilityControlTypes.CombatAction;

    // AoWAb.PAS:4832
    public override bool FastBattleValidRoundDistance(CombatObject source, CombatObject target)
    {
        int distance = source.Combat.RoundDistance;
        CombatRange range = GetRangeRangedAttack(source.AbilityOwner);
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
                        ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, targetDamageValue: values.TargetDamage, commands.Source, target)
                    };
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:4787
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand? command)
    {
        CombatObject? target = command.Target;
        int penalty = source.Combat.GetWallSituation(source, target) == WallSituation.Target ? 2 : 0;

        if (target.GetEnabled())
        {
            for (int i = GetAttackRepeatRangedAttack(source.AbilityOwner) - 1; i >= 0; i--)
            {
                RangedAttackCombatAction action = CreateRangedAttackCombatAction(source, target, attackPenalty: penalty);
                source.Combat.ExecuteCombatAction(action);
                _ = action.Release();
            }

            source.State |= FastBattleUnitStates.Vulnerable;
        }

        return true;
    }

    public override CombatAction CreateRangedCombatAction(CombatUnit source, CombatUnit target)
        => CreateRangedAttackCombatAction(source, target: target, attackPenalty: 0);

    // AoWAb.PAS:4708
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int hitPoints = defender.GetHitPoints();
        int defense = defender.GetDefense();
        int resistance = defender.GetResistance();
        DamageTypes immunityTypes = defender.GetImmunityTypes();
        DamageTypes protectionTypes = defender.GetProtectionTypes();
        DamageTypes damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker);
        int damageRangedAttack = GetDamageRangedAttack(attacker);
        int result = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker), damage: damageRangedAttack, damageTypes: damageTypesRangedAttack, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes);
        int repeats = GetAttackRepeatRangedAttack(attacker);
        return result * repeats;
    }

    // AoWAb.PAS:4717
    public override void GetDamageValues(Troop attacker, Troop? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        int defense = defender.GetDefense();
        if (wallSituation == WallSituation.Target)
        {
            defense += 2;
        }

        int hitPoints = defender.GetHitPoints();
        int resistance = defender.GetResistance();
        DamageTypes immunityTypes = defender.GetImmunityTypes();
        DamageTypes protectionTypes = defender.GetProtectionTypes();
        DamageTypes damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker);
        int damageRangedAttack = GetDamageRangedAttack(attacker);
        int damage = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker), damage: damageRangedAttack, damageTypes: damageTypesRangedAttack, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes);
        int repeats = GetAttackRepeatRangedAttack(attacker);
        damage *= repeats;
        damageValues.SourceDamage = damage;
        damageValues.TargetDamage = 0;
        DamageTypes damageTypesRangedAttack2 = GetDamageTypesRangedAttack(attacker);
        damageValues.DamageTypes = damageTypesRangedAttack2;
    }

    // AoWAb.PAS:4703
    public override void GetOffensiveStrength(AbilityOwner? owner, ref int strength, ref DamageTypes types)
    {
        int attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        int damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), defense: damageRangedAttack, damage: attackRepeatRangedAttack);
        types = GetDamageTypesRangedAttack(owner);
    }

    // AoWAb.PAS:4732
    // { Original leaves both cost fields untouched. }
    public override void FastBattleGetDamageValues(CombatObject? attacker, CombatObject? defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        CombatRange range = GetRangeRangedAttack(attacker.AbilityOwner);
        if (wallSituation == WallSituation.None || range is CombatRange.Medium or CombatRange.Long || attacker.GetAbilityEnabled(1))
        {
            DamageTypes protectionTypes = defender.GetProtectionTypes();
            DamageTypes immunityTypes = defender.GetImmunityTypes();
            DamageTypes damageTypes = GetDamageTypesRangedAttack(attacker.AbilityOwner);
            int damage = GetDamageRangedAttack(attacker.AbilityOwner);
            int defense = defender.GetDefense();
            if (wallSituation == WallSituation.Target)
            {
                defense += 2;
            }

            DamageTypes damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker.AbilityOwner);
            damageValues.DamageTypes = damageTypesRangedAttack;
            int hitPoints = defender.HitPoints;
            int resistance = defender.GetResistance();
            int value = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker.AbilityOwner), damage, damageTypes, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes);
            int repeats = GetAttackRepeatRangedAttack(attacker.AbilityOwner);
            value *= repeats;
            damageValues.SourceDamage = value;
            // { Original swaps protection and immunity for maximum damage. }
            value = AbilityRules.StatisticsToMaxDamage(damage, damageTypes, immunityTypes: protectionTypes, protectionTypes: immunityTypes);
            repeats = GetAttackRepeatRangedAttack(attacker.AbilityOwner);
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
            damageValues.DamageTypes = DamageTypes.None;
        }
    }

    // AoWAb.PAS:4769
    public override void GetCombatInfo(AbilityOwner? owner, ref AbilityCombatInfo info)
    {
        int attackRangedAttack = GetAttackRangedAttack(owner);
        info.Attack = (sbyte)attackRangedAttack;
        int damageRangedAttack = GetDamageRangedAttack(owner);
        info.Damage = (sbyte)damageRangedAttack;
        DamageTypes damageTypesRangedAttack = GetDamageTypesRangedAttack(owner);
        info.DamageTypes = damageTypesRangedAttack;
        int attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        info.RepeatCount = (sbyte)attackRepeatRangedAttack;
        info.TargetTypes = CombatTargetTypes.Enemy;
    }

    // AoWAb.PAS:4661
    public virtual CombatRange GetRangeRangedAttack(AbilityOwner? owner) => RangeRangedAttack;

    // AoWAb.PAS:4665
    public virtual int GetDamageRangedAttack(AbilityOwner? owner)
    {
        int result = DamageRangedAttack;
        if (owner is not null && owner.GetOwnAbilityEnabled(32))
        {
            switch (owner.GetOwnAbilityLevel(32))
            {
                case 2:
                    return (result + 1).TruncateToSByte();
                case 3:
                    return (result + 1).TruncateToSByte();
                case 4:
                    return (result + 2).TruncateToSByte();
                default:
                    break;
            }
        }

        return result;
    }

    // AoWAb.PAS:4675
    public virtual int GetAttackRangedAttack(AbilityOwner? owner)
    {
        int result = AttackRangedAttack;
        if (owner is not null && owner.GetOwnAbilityEnabled(32))
        {
            switch (owner.GetOwnAbilityLevel(32))
            {
                case 1:
                    return (result + 1).TruncateToSByte();
                case 2:
                    return (result + 1).TruncateToSByte();
                case 3:
                    return (result + 2).TruncateToSByte();
                case 4:
                    return (result + 2).TruncateToSByte();
                default:
                    break;
            }
        }

        return result;
    }

    // AoWAb.PAS:4686
    public virtual int GetAttackRepeatRangedAttack(AbilityOwner? owner) => AttackRepeatRangedAttack;

    // AoWAb.PAS:4690
    public virtual DamageTypes GetDamageTypesRangedAttack(AbilityOwner? owner) => DamageTypesRangedAttack;

    // AoWAb.PAS:4777
    public RangedAttackCombatAction CreateRangedAttackCombatAction(CombatObject? source, CombatObject? target, int attackPenalty)
    {
        RangedAttackCombatAction action = new(null) { Holder = CreationHolder() };
        action.Generate(damage: GetDamageRangedAttack(source.AbilityOwner), damageTypes: GetDamageTypesRangedAttack(source.AbilityOwner), source: source, target: target, attack: (GetAttackRangedAttack(source.AbilityOwner) - attackPenalty).TruncateToSByte());
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
