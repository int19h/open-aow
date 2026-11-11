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
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
        FAbilityType = AbilityType.Combat;
    }

    public CombatRange FRangeRA;
    public sbyte FDamageRA;
    public sbyte FAttackRA;
    public PackedDamageTypes FDamageTypesRA;
    public sbyte FAttackRepeatRA;

    // AoWAb.PAS:4694
    public override CombatMode GetCombatMode(AbilityOwner Owner) => CombatRangeToCombatMode(GetRangeRA(Owner));

    // AoWAb.PAS:4698
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner)
    {
        Set<WallCombatFeaturesElement> Result;
        if (GetDamageTypesRA(Owner).Contains(DamageType.Wall))
        {
            Result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.Participate);
        }
        else
        {
            Result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.Participate);
        }

        return Result;
    }

    // AoWAb.PAS:4659
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4832
    public override bool fcValidRoundDistance(CombatObject Source, CombatObject Target)
    {
        int Distance;
        CombatRange Range;
        Distance = Source.FCombat.GetRoundDistance();
        Range = GetRangeRA(Source.GetAbilityOwner());
        if (Range == CombatRange.Long)
        {
            Distance += 4;
        }
        else
        {
            if (Range == CombatRange.Medium)
            {
                Distance += 2;
            }
            else
            {
                if (Range == CombatRange.Short)
                {
                    Distance += 1;
                }
            }
        }

        return Distance >= Target.fcRoundDistance();
    }

    // AoWAb.PAS:4808
    public override void fcPrefetchCombatCommands(FastBattleCommandList Commands)
    {
        CombatObject Target;
        AbilityDamageValues Values = default;
        FastBattleCommand Command;
        for (int I = 0, last = Commands.FTargetSide.GetCount() - 1; I <= last; I++)
        {
            Target = Commands.FTargetSide.GetObjects(I);
            if (Target.GetEnabled() && fcValidRoundDistance(Commands.FSource, Target))
            {
                var wallSituation = Commands.FSource.FCombat.GetWallSituation(Commands.FSource, Target);
                fcGetDamageValueEx(Commands.FSource, Target, wallSituation, ref Values);
                if (Values.SourceDamage != 0)
                {
                    Command = new FastBattleCommand(FAbilityID);
                    Command.FTarget = Commands.FTargetSide.GetObjects(I);
                    Command.FMoveCost = 1;
                    Command.FExpectedDV = AbilityRules.DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                    Commands.Add(Command);
                }
            }
        }
    }

    // AoWAb.PAS:4787
    public override bool fcExecuteCombatCommand(CombatObject Source, FastBattleCommand Command)
    {
        CombatObject Target;
        int Penalty;
        RangedAttackCombatAction Action;
        Target = Command.FTarget;
        if (Source.FCombat.GetWallSituation(Source, Target) == WallSituation.Target)
        {
            Penalty = 2;
        }
        else
        {
            Penalty = 0;
        }

        if (Target.GetEnabled())
        {
            for (int I = GetAttackRepeatRA(Source.GetAbilityOwner()) - 1; I >= 0; I--)
            {
                Action = CreateRangedAttackCA(Source, Target, Penalty);
                Source.FCombat.ExecuteCombatAction(Action);
                Action.Release();
            }

            ((FastBattleUnit)Source).FState = ((FastBattleUnit)Source).FState + Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.Vulnerable);
        }

        return true;
    }

    // AoWAb.PAS:4708
    public override int GetDamageValue(Troop Attacker, Troop Defender)
    {
        int Repeats;
        var hitPoints = Defender.GetHitPoints();
        var defense = Defender.GetDefense();
        var resistance = Defender.GetResistance();
        var immunityTypes = Defender.GetImmunityTypes();
        var protectionTypes = Defender.GetProtectionTypes();
        var damageTypesRA = GetDamageTypesRA(Attacker);
        var damageRA = GetDamageRA(Attacker);
        int Result = AbilityRules.StatisticsToLimitedDV(GetAttackRA(Attacker), damageRA, damageTypesRA, hitPoints, defense, resistance, immunityTypes, protectionTypes);
        Repeats = GetAttackRepeatRA(Attacker);
        return Result * Repeats;
    }

    // AoWAb.PAS:4717
    public override void GetDamageValueEx(Troop Attacker, Troop Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        int Defense;
        int Damage;
        int Repeats;
        Defense = Defender.GetDefense();
        if (WallSituation == WallSituation.Target)
        {
            Defense += 2;
        }

        var hitPoints = Defender.GetHitPoints();
        var resistance = Defender.GetResistance();
        var immunityTypes = Defender.GetImmunityTypes();
        var protectionTypes = Defender.GetProtectionTypes();
        var damageTypesRA = GetDamageTypesRA(Attacker);
        var damageRA = GetDamageRA(Attacker);
        Damage = AbilityRules.StatisticsToLimitedDV(GetAttackRA(Attacker), damageRA, damageTypesRA, hitPoints, Defense, resistance, immunityTypes, protectionTypes);
        Repeats = GetAttackRepeatRA(Attacker);
        Damage = Damage * Repeats;
        DamageValues.SourceDamage = Damage;
        DamageValues.TargetDamage = 0;
        var damageTypesRA2 = GetDamageTypesRA(Attacker);
        DamageValues.DamageTypes = damageTypesRA2;
    }

    // AoWAb.PAS:4703
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        var attackRepeatRA = GetAttackRepeatRA(Owner);
        var damageRA = GetDamageRA(Owner);
        Strength = AbilityRules.P3LimitedDamage(GetAttackRA(Owner), damageRA, attackRepeatRA);
        Types = GetDamageTypesRA(Owner);
    }

    // AoWAb.PAS:4732
    // { Original leaves both cost fields untouched. }
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        Set<DamageType> DamageTypes;
        Set<DamageType> ProtectionTypes;
        Set<DamageType> ImmunityTypes;
        int Damage;
        int Defense;
        int Value;
        int Repeats;
        CombatRange Range;
        Range = GetRangeRA(Attacker.GetAbilityOwner());
        if (WallSituation == WallSituation.None || Set<CombatRange>.Of(CombatRange.Medium, CombatRange.Long).Contains(Range) || Attacker.GetAbilityEnabled(1))
        {
            ProtectionTypes = Defender.GetProtectionTypes();
            ImmunityTypes = Defender.GetImmunityTypes();
            DamageTypes = GetDamageTypesRA(Attacker.GetAbilityOwner());
            Damage = GetDamageRA(Attacker.GetAbilityOwner());
            Defense = Defender.GetDefense();
            if (WallSituation == WallSituation.Target)
            {
                Defense += 2;
            }

            var damageTypesRA = GetDamageTypesRA(Attacker.GetAbilityOwner());
            DamageValues.DamageTypes = damageTypesRA;
            var hitPoints = Defender.GetHitPoints();
            var resistance = Defender.GetResistance();
            Value = AbilityRules.StatisticsToLimitedDV(GetAttackRA(Attacker.GetAbilityOwner()), Damage, DamageTypes, hitPoints, Defense, resistance, ImmunityTypes, ProtectionTypes);
            Repeats = GetAttackRepeatRA(Attacker.GetAbilityOwner());
            Value = Value * Repeats;
            DamageValues.SourceDamage = Value;
            // { Original swaps protection and immunity for maximum damage. }
            Value = AbilityRules.StatisticsToMaxDamage(Damage, DamageTypes, ProtectionTypes, ImmunityTypes);
            Repeats = GetAttackRepeatRA(Attacker.GetAbilityOwner());
            Value = Value * Repeats;
            DamageValues.SourceCost = Value;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
        }
        else
        {
            DamageValues.SourceDamage = 0;
            DamageValues.SourceCost = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:4769
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        var attackRA = GetAttackRA(Owner);
        Info.Attack = attackRA;
        var damageRA = GetDamageRA(Owner);
        Info.Damage = damageRA;
        var damageTypesRA = GetDamageTypesRA(Owner);
        Info.DamageTypes = damageTypesRA;
        var attackRepeatRA = GetAttackRepeatRA(Owner);
        Info.RepeatCount = attackRepeatRA;
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:4661
    public virtual CombatRange GetRangeRA(AbilityOwner Owner) => FRangeRA;

    // AoWAb.PAS:4665
    public virtual sbyte GetDamageRA(AbilityOwner Owner)
    {
        sbyte Result = FDamageRA;
        if (Owner != null && Owner.GetAbEnabled(32))
        {
            switch (Owner.GetAbLevel(32))
            {
                case 2:
                    Result = (sbyte)(Result + 1);
                    break;
                case 3:
                    Result = (sbyte)(Result + 1);
                    break;
                case 4:
                    Result = (sbyte)(Result + 2);
                    break;
            }
        }

        return Result;
    }

    // AoWAb.PAS:4675
    public virtual sbyte GetAttackRA(AbilityOwner Owner)
    {
        sbyte Result = FAttackRA;
        if (Owner != null && Owner.GetAbEnabled(32))
        {
            switch (Owner.GetAbLevel(32))
            {
                case 1:
                    Result = (sbyte)(Result + 1);
                    break;
                case 2:
                    Result = (sbyte)(Result + 1);
                    break;
                case 3:
                    Result = (sbyte)(Result + 2);
                    break;
                case 4:
                    Result = (sbyte)(Result + 2);
                    break;
            }
        }

        return Result;
    }

    // AoWAb.PAS:4686
    public virtual sbyte GetAttackRepeatRA(AbilityOwner Owner) => FAttackRepeatRA;

    // AoWAb.PAS:4690
    public virtual Set<DamageType> GetDamageTypesRA(AbilityOwner Owner) => FDamageTypesRA.Value;

    // AoWAb.PAS:4777
    public RangedAttackCombatAction CreateRangedAttackCA(CombatObject Source, CombatObject Target, int AttackPenalty)
    {
        RangedAttackCombatAction Action;
        Action = new RangedAttackCombatAction(null);
        var damageRA = GetDamageRA(Source.GetAbilityOwner());
        var damageTypesRA = GetDamageTypesRA(Source.GetAbilityOwner());
        Action.Generate(Source, Target, (sbyte)(GetAttackRA(Source.GetAbilityOwner()) - AttackPenalty), damageRA, damageTypesRA);
        Action.FAbilityID = FAbilityID;
        return Action;
    }

    private static CombatMode CombatRangeToCombatMode(CombatRange Range)
    {
        return Range switch
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
