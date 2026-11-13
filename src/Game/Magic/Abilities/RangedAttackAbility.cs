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
    public RangedAttackAbility(ECustomNode owner) : base(owner)
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
        Set<WallCombatFeaturesElement> result;
        if (GetDamageTypesRangedAttack(owner).Contains(DamageType.Wall))
        {
            result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);
        }
        else
        {
            result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.Participate);
        }

        return result;
    }

    // AoWAb.PAS:4659
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4832
    public override bool FastBattleValidRoundDistance(CombatObject source, CombatObject target)
    {
        int distance;
        CombatRange range;
        distance = source.Combat.GetRoundDistance();
        range = GetRangeRangedAttack(source.GetAbilityOwner());
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
        CombatObject target;
        AbilityDamageValues values = default;
        FastBattleCommand command;
        for (int I = 0, last = commands.TargetSide.GetCount() - 1; I <= last; I++)
        {
            target = commands.TargetSide.GetObjects(I);
            if (target.GetEnabled() && FastBattleValidRoundDistance(commands.Source, target))
            {
                var wallSituation = commands.Source.Combat.GetWallSituation(commands.Source, target);
                FastBattleGetDamageValues(commands.Source, target, wallSituation, ref values);
                if (values.SourceDamage != 0)
                {
                    command = new FastBattleCommand(AbilityId);
                    command.Target = commands.TargetSide.GetObjects(I);
                    command.MoveCost = 1;
                    command.ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target);
                    commands.Add(command);
                }
            }
        }
    }

    // AoWAb.PAS:4787
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        CombatObject target;
        int penalty;
        RangedAttackCombatAction action;
        target = command.Target;
        if (source.Combat.GetWallSituation(source, target) == WallSituation.Target)
        {
            penalty = 2;
        }
        else
        {
            penalty = 0;
        }

        if (target.GetEnabled())
        {
            for (int I = GetAttackRepeatRangedAttack(source.GetAbilityOwner()) - 1; I >= 0; I--)
            {
                action = CreateRangedAttackCombatAction(source, target, penalty);
                source.Combat.ExecuteCombatAction(action);
                action.Release();
            }

            ((FastBattleUnit)source).State = ((FastBattleUnit)source).State + Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.Vulnerable);
        }

        return true;
    }

    // AoWAb.PAS:4708
    public override int GetDamageValue(Troop attacker, Troop defender)
    {
        int repeats;
        var hitPoints = defender.GetHitPoints();
        var defense = defender.GetDefense();
        var resistance = defender.GetResistance();
        var immunityTypes = defender.GetImmunityTypes();
        var protectionTypes = defender.GetProtectionTypes();
        var damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker);
        var damageRangedAttack = GetDamageRangedAttack(attacker);
        int result = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker), damageRangedAttack, damageTypesRangedAttack, hitPoints, defense, resistance, immunityTypes, protectionTypes);
        repeats = GetAttackRepeatRangedAttack(attacker);
        return result * repeats;
    }

    // AoWAb.PAS:4717
    public override void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        int defense;
        int damage;
        int repeats;
        defense = defender.GetDefense();
        if (wallSituation == WallSituation.Target)
        {
            defense += 2;
        }

        var hitPoints = defender.GetHitPoints();
        var resistance = defender.GetResistance();
        var immunityTypes = defender.GetImmunityTypes();
        var protectionTypes = defender.GetProtectionTypes();
        var damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker);
        var damageRangedAttack = GetDamageRangedAttack(attacker);
        damage = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker), damageRangedAttack, damageTypesRangedAttack, hitPoints, defense, resistance, immunityTypes, protectionTypes);
        repeats = GetAttackRepeatRangedAttack(attacker);
        damage = damage * repeats;
        damageValues.SourceDamage = damage;
        damageValues.TargetDamage = 0;
        var damageTypesRangedAttack2 = GetDamageTypesRangedAttack(attacker);
        damageValues.DamageTypes = damageTypesRangedAttack2;
    }

    // AoWAb.PAS:4703
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        var attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        var damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), damageRangedAttack, attackRepeatRangedAttack);
        types = GetDamageTypesRangedAttack(owner);
    }

    // AoWAb.PAS:4732
    // { Original leaves both cost fields untouched. }
    public override void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        Set<DamageType> damageTypes;
        Set<DamageType> protectionTypes;
        Set<DamageType> immunityTypes;
        int damage;
        int defense;
        int value;
        int repeats;
        CombatRange range;
        range = GetRangeRangedAttack(attacker.GetAbilityOwner());
        if (wallSituation == WallSituation.None || Set<CombatRange>.Of(CombatRange.Medium, CombatRange.Long).Contains(range) || attacker.GetAbilityEnabled(1))
        {
            protectionTypes = defender.GetProtectionTypes();
            immunityTypes = defender.GetImmunityTypes();
            damageTypes = GetDamageTypesRangedAttack(attacker.GetAbilityOwner());
            damage = GetDamageRangedAttack(attacker.GetAbilityOwner());
            defense = defender.GetDefense();
            if (wallSituation == WallSituation.Target)
            {
                defense += 2;
            }

            var damageTypesRangedAttack = GetDamageTypesRangedAttack(attacker.GetAbilityOwner());
            damageValues.DamageTypes = damageTypesRangedAttack;
            var hitPoints = defender.GetHitPoints();
            var resistance = defender.GetResistance();
            value = AbilityRules.StatisticsToLimitedDamageValue(GetAttackRangedAttack(attacker.GetAbilityOwner()), damage, damageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes);
            repeats = GetAttackRepeatRangedAttack(attacker.GetAbilityOwner());
            value = value * repeats;
            damageValues.SourceDamage = value;
            // { Original swaps protection and immunity for maximum damage. }
            value = AbilityRules.StatisticsToMaxDamage(damage, damageTypes, protectionTypes, immunityTypes);
            repeats = GetAttackRepeatRangedAttack(attacker.GetAbilityOwner());
            value = value * repeats;
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
        var attackRangedAttack = GetAttackRangedAttack(owner);
        info.Attack = attackRangedAttack;
        var damageRangedAttack = GetDamageRangedAttack(owner);
        info.Damage = damageRangedAttack;
        var damageTypesRangedAttack = GetDamageTypesRangedAttack(owner);
        info.DamageTypes = damageTypesRangedAttack;
        var attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        info.RepeatCount = attackRepeatRangedAttack;
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:4661
    public virtual CombatRange GetRangeRangedAttack(AbilityOwner owner) => RangeRangedAttack;

    // AoWAb.PAS:4665
    public virtual sbyte GetDamageRangedAttack(AbilityOwner owner)
    {
        sbyte result = DamageRangedAttack;
        if (owner != null && owner.GetAbEnabled(32))
        {
            switch (owner.GetAbLevel(32))
            {
                case 2:
                    result = (sbyte)(result + 1);
                    break;
                case 3:
                    result = (sbyte)(result + 1);
                    break;
                case 4:
                    result = (sbyte)(result + 2);
                    break;
            }
        }

        return result;
    }

    // AoWAb.PAS:4675
    public virtual sbyte GetAttackRangedAttack(AbilityOwner owner)
    {
        sbyte result = AttackRangedAttack;
        if (owner != null && owner.GetAbEnabled(32))
        {
            switch (owner.GetAbLevel(32))
            {
                case 1:
                    result = (sbyte)(result + 1);
                    break;
                case 2:
                    result = (sbyte)(result + 1);
                    break;
                case 3:
                    result = (sbyte)(result + 2);
                    break;
                case 4:
                    result = (sbyte)(result + 2);
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
        RangedAttackCombatAction action;
        action = new RangedAttackCombatAction(null) { Holder = CreationHolder() };
        var damageRangedAttack = GetDamageRangedAttack(source.GetAbilityOwner());
        var damageTypesRangedAttack = GetDamageTypesRangedAttack(source.GetAbilityOwner());
        action.Generate(source, target, (sbyte)(GetAttackRangedAttack(source.GetAbilityOwner()) - attackPenalty), damageRangedAttack, damageTypesRangedAttack);
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
