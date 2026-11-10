// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// count, capacity, strikes, combat objects, cached damage values, units, strike counts,
// attacker/defender damage types
public partial class MeleeRound : IDisposable
{

    public int FCount;
    public int FCapacity;
    public MeleeStrike[] FStrikes = [];
    public CombatObject FAttacker;
    public CombatObject FDefender;
    public int FAttackerDV;
    public int FDefenderDV;
    public AbstractUnit FAttackerUnit;
    public AbstractUnit FDefenderUnit;
    public int FAttackerStrikes;
    public int FDefenderStrikes;
    public Set<DamageType> FAttackerDamageTypes;
    public Set<DamageType> FDefenderDamageTypes;

    // AoWAb.PAS:1492
    public virtual void Dispose()
    {
        SetCapacity(0);
    }

    // AoWAb.PAS:1498
    public void SetCapacity(int NewCapacity)
    {
        FCapacity = NewCapacity;
        if (FCapacity < FCount)
        {
            FCount = FCapacity;
        }

        Array.Resize(ref FStrikes, NewCapacity);
    }

    // AoWAb.PAS:1505
    public void SetCount(int NewCount)
    {
        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            FStrikes.AsSpan(FCount, NewCount - FCount).Clear();
        }

        FCount = NewCount;
    }

    // AoWAb.PAS:1513
    public void GetMeleeStrike(int Index, ref MeleeStrike Strike)
    {
        Strike = FStrikes[Index];
    }

    // AoWAb.PAS:1518
    public void Reset()
    {
        SetCount(0);
        FAttacker = null;
        FDefender = null;
        FAttackerUnit = null;
        FDefenderUnit = null;
        FAttackerDV = -1;
        FDefenderDV = -1;
        FAttackerStrikes = 0;
        FDefenderStrikes = 0;
    }

    // AoWAb.PAS:1531
    public void CalculateUnitStrikes(CombatSideIndex FirstSide)
    {
        int AttackerCount;
        int DefenderCount;
        CombatSideIndex Side;
        AbstractUnit Source;
        AbstractUnit Target;
        Side = FirstSide;
        AttackerCount = 0;
        DefenderCount = 0;
        SetCount(FAttackerStrikes + FDefenderStrikes);
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (Side == CombatSideIndex.Attacker)
            {
                Source = FAttackerUnit;
                Target = FDefenderUnit;
            }
            else
            {
                Source = FDefenderUnit;
                Target = FAttackerUnit;
            }

            ref MeleeStrike Strike = ref FStrikes[I];
            Strike.Side = Side;
            if (I == 0 || (I == 1 && Side != FirstSide))
            {
                if (Side == CombatSideIndex.Attacker && I == 0 && Source.GetAbilityEnabled(111))
                {
                    Strike.DamageBonus += 2;
                }

                if (Target.GetAbilityEnabled(113))
                {
                    Strike.AttackBonus -= 2;
                }
            }

            bool condition = Source.GetAbilityEnabled(146) || Source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = Target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                Strike.DamageBonus += 2;
                Strike.AttackBonus += 2;
            }

            bool test = Source.GetAbilityEnabled(147) || Source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = Target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                Strike.DamageBonus += 2;
                Strike.AttackBonus += 2;
            }

            if (Source.GetAbilityEnabled(112) && Target.GetAbilityEnabled(63))
            {
                Strike.AttackBonus += 3;
                Strike.DamageBonus += 3;
            }

            // { Original uses the attacker's damage types for both sides. }
            var defense = Target.GetDefense();
            var resistance = Target.GetResistance();
            var immunityTypes = Target.GetImmunityTypes();
            var protectionTypes = Target.GetProtectionTypes();
            var damage = Source.GetDamage();
            var statisticsToDV = StatisticsToDV(Source.GetAttack() + Strike.AttackBonus, damage + Strike.DamageBonus, FAttackerDamageTypes, defense, resistance, immunityTypes, protectionTypes);
            Strike.DamageValue = statisticsToDV;
            switch (Side)
            {
                case CombatSideIndex.Attacker:
                    AttackerCount += 1;
                    if (FDefenderStrikes != DefenderCount)
                    {
                        Side = CombatSideIndex.Defender;
                    }

                    break;
                case CombatSideIndex.Defender:
                    DefenderCount += 1;
                    if (FAttackerStrikes != AttackerCount)
                    {
                        Side = CombatSideIndex.Attacker;
                    }

                    break;
            }
        }
    }

    // AoWAb.PAS:1600
    public void CalculateFreeUnitSwing(AbstractUnit Attacker, AbstractUnit Defender)
    {
        Reset();
        FAttackerUnit = Attacker;
        FDefenderUnit = Defender;
        FAttackerDamageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Attacker);
        FDefenderDamageTypes = Set<DamageType>.Of();
        if (FAttackerDamageTypes != Set<DamageType>.Of())
        {
            FAttackerStrikes = 1;
            CalculateUnitStrikes(CombatSideIndex.Attacker);
        }
    }

    // AoWAb.PAS:1614
    public void CalculateUnit(AbstractUnit Attacker, AbstractUnit Defender)
    {
        CombatSideIndex Side;
        Reset();
        FAttackerUnit = Attacker;
        FDefenderUnit = Defender;
        FAttackerDamageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Attacker);
        if (FAttackerDamageTypes != Set<DamageType>.Of())
        {
            FAttackerStrikes = 2;
            if (FAttackerUnit.GetAbilityEnabled(115))
            {
                FAttackerStrikes += 1;
            }
        }
        else
        {
            FAttackerStrikes = 0;
        }

        FDefenderDamageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Defender);
        if (FDefenderDamageTypes != Set<DamageType>.Of())
        {
            FDefenderStrikes = 2;
        }
        else
        {
            FDefenderStrikes = 0;
        }

        if (FDefenderUnit.GetAbilityEnabled(116) && !FAttackerUnit.GetAbilityEnabled(116) && FDefenderStrikes != 0)
        {
            Side = CombatSideIndex.Defender;
        }
        else
        {
            Side = CombatSideIndex.Attacker;
        }

        CalculateUnitStrikes(Side);
    }

    // AoWAb.PAS:1642
    public void CalculateStrikes(CombatSideIndex FirstSide)
    {
        int AttackerCount;
        int DefenderCount;
        CombatSideIndex Side;
        CombatObject Source;
        CombatObject Target;
        Side = FirstSide;
        AttackerCount = 0;
        DefenderCount = 0;
        SetCount(FAttackerStrikes + FDefenderStrikes);
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (Side == CombatSideIndex.Attacker)
            {
                Source = FAttacker;
                Target = FDefender;
            }
            else
            {
                Source = FDefender;
                Target = FAttacker;
            }

            ref MeleeStrike Strike = ref FStrikes[I];
            Strike.Side = Side;
            if (I == 0 || (I == 1 && Side != FirstSide))
            {
                if (Side == CombatSideIndex.Attacker && I == 0 && Source.GetAbilityEnabled(111))
                {
                    Strike.DamageBonus += 2;
                }

                if (Target.GetAbilityEnabled(113))
                {
                    Strike.AttackBonus -= 2;
                }
            }

            bool condition = Source.GetAbilityEnabled(146) || Source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = Target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                Strike.AttackBonus += 2;
                Strike.DamageBonus += 2;
            }

            bool test = Source.GetAbilityEnabled(147) || Source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = Target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                Strike.AttackBonus += 2;
                Strike.DamageBonus += 2;
            }

            if (Source.GetAbilityEnabled(112) && Target.GetAbilityEnabled(63))
            {
                Strike.AttackBonus += 3;
                Strike.DamageBonus += 3;
            }

            // { Original uses the attacker's damage types for both sides. }
            var defense = Target.GetDefense();
            var resistance = Target.GetResistance();
            var immunityTypes = Target.GetImmunityTypes();
            var protectionTypes = Target.GetProtectionTypes();
            var damage = Source.GetDamage();
            var statisticsToDV = StatisticsToDV(Source.GetAttack() + Strike.AttackBonus, damage + Strike.DamageBonus, FAttackerDamageTypes, defense, resistance, immunityTypes, protectionTypes);
            Strike.DamageValue = statisticsToDV;
            switch (Side)
            {
                case CombatSideIndex.Attacker:
                    AttackerCount += 1;
                    if (FDefenderStrikes != DefenderCount)
                    {
                        Side = CombatSideIndex.Defender;
                    }

                    break;
                case CombatSideIndex.Defender:
                    DefenderCount += 1;
                    if (FAttackerStrikes != AttackerCount)
                    {
                        Side = CombatSideIndex.Attacker;
                    }

                    break;
            }
        }
    }

    // AoWAb.PAS:1711
    public void Calculate(CombatObject Attacker, CombatObject Defender)
    {
        CombatSideIndex Side;
        Reset();
        FAttacker = Attacker;
        FDefender = Defender;
        FAttackerDamageTypes = CombatObject.MeleeDamageTypes(Attacker);
        if (FAttackerDamageTypes != Set<DamageType>.Of() && !Attacker.GetLocked())
        {
            FAttackerStrikes = 2;
            if (FAttacker.GetAbilityEnabled(115))
            {
                FAttackerStrikes += 1;
            }
        }
        else
        {
            FAttackerStrikes = 0;
        }

        FDefenderDamageTypes = CombatObject.MeleeDamageTypes(Defender);
        if (FDefenderDamageTypes != Set<DamageType>.Of() && !Defender.GetLocked())
        {
            FDefenderStrikes = 2;
        }
        else
        {
            FDefenderStrikes = 0;
        }

        if (Defender.GetAbilityEnabled(116) && !Attacker.GetAbilityEnabled(116) && FDefenderStrikes != 0)
        {
            Side = CombatSideIndex.Defender;
        }
        else
        {
            Side = CombatSideIndex.Attacker;
        }

        CalculateStrikes(Side);
    }

    // AoWAb.PAS:1739
    public void CalculateSingleSide(CombatObject Attacker, CombatObject Defender)
    {
        Reset();
        FAttacker = Attacker;
        FDefender = Defender;
        FAttackerDamageTypes = CombatObject.MeleeDamageTypes(Attacker);
        FDefenderDamageTypes = Set<DamageType>.Of();
        if (FAttackerDamageTypes != Set<DamageType>.Of() && !FAttacker.GetLocked())
        {
            FAttackerStrikes = 2;
            if (FAttacker.GetAbilityEnabled(115))
            {
                FAttackerStrikes += 1;
            }
        }

        CalculateStrikes(CombatSideIndex.Attacker);
    }

    // AoWAb.PAS:1754
    public StrikeCA CreateStrikeCA(int Index)
    {
        StrikeCA Result = null;
        if (Index < FCount)
        {
            ref MeleeStrike Strike = ref FStrikes[Index];
            switch (Strike.Side)
            {
                case CombatSideIndex.Attacker:
                    if (!FAttacker.GetLocked())
                    {
                        Result = new StrikeCA(null);
                        var damage = FAttacker.GetDamage();
                        var attack = FAttacker.GetAttack();
                        Result.Generate(FAttacker, FDefender, (sbyte)(attack + Strike.AttackBonus), (sbyte)(damage + Strike.DamageBonus), FAttackerDamageTypes);
                        if (FAttacker.GetAbilityEnabled(118))
                        {
                            Result.FStrikeMode = Result.FStrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.LifeStealing);
                        }
                    }

                    break;
                case CombatSideIndex.Defender:
                    if (!FDefender.GetLocked())
                    {
                        Result = new StrikeCA(null);
                        Result.SetDefensive(true);
                        var damage = FDefender.GetDamage();
                        var attack = FDefender.GetAttack();
                        Result.Generate(FDefender, FAttacker, (sbyte)(attack + Strike.AttackBonus), (sbyte)(damage + Strike.DamageBonus), FDefenderDamageTypes);
                    }

                    break;
            }
        }

        return Result;
    }

    // AoWAb.PAS:1786
    public void CalculateDV()
    {
        int I;
        int LifeStealingDV;
        bool LifeStealing;
        CombatObject Source;
        FAttackerDV = 0;
        FDefenderDV = 0;
        I = 0;
        LifeStealingDV = 0;
        Source = FAttacker;
        if (Source != null)
        {
            LifeStealing = Source.GetAbilityEnabled(118);
            if (LifeStealing && FDefender is CombatUnit && ((CombatUnit)FDefender).FUnit.GetUnitType() == UnitType.Machine)
            {
                LifeStealing = false;
            }
        }
        else
        {
            LifeStealing = FAttackerUnit.GetAbilityEnabled(118);
            if (LifeStealing && FDefenderUnit.GetUnitType() == UnitType.Machine)
            {
                LifeStealing = false;
            }
        }

        while (I != FCount)
        {
            if (FStrikes[I].Side == CombatSideIndex.Attacker)
            {
                FAttackerDV += FStrikes[I].DamageValue;
                if (FStrikes[I].DamageValue > 100)
                {
                    LifeStealingDV += 100;
                }
                else
                {
                    LifeStealingDV += FStrikes[I].DamageValue;
                }
            }
            else
            {
                FDefenderDV += FStrikes[I].DamageValue;
            }

            I += 1;
        }

        if (LifeStealing && FDefenderDV > 0)
        {
            FDefenderDV -= LifeStealingDV;
            if (FDefenderDV <= 0)
            {
                FDefenderDV = 1;
            }
        }
    }

    // AoWAb.PAS:1831
    public int GetAttackerDV()
    {
        if (FAttackerDV == -1)
        {
            CalculateDV();
        }

        return FAttackerDV;
    }

    // AoWAb.PAS:1837
    public int GetDefenderDV()
    {
        if (FDefenderDV == -1)
        {
            CalculateDV();
        }

        return FDefenderDV;
    }

    internal static int StatisticsToDV(int Attack, int Damage, Set<DamageType> DamageTypes, int Defense, int Resistance, Set<DamageType> ImmunityTypes, Set<DamageType> ProtectionTypes)
    {
        DamageTypes -= ImmunityTypes;
        if (DamageTypes != Set<DamageType>.Of(Array.Empty<DamageType>()))
        {
            Attack -= Defense;
            if (DamageTypes - ProtectionTypes == Set<DamageType>.Of(Array.Empty<DamageType>()))
            {
                Damage = Damage + 1 >>> 1;
            }
            return AbilityRules.DMDCtoDV(Damage, Attack);
        }
        return 0;
    }

    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct MeleeStrike
    {
        [FieldOffset(0)]
        public int AttackBonus;
        [FieldOffset(4)]
        public int DamageBonus;
        [FieldOffset(8)]
        public int DamageValue;
        [FieldOffset(12)]
        public CombatSideIndex Side;
        [FieldOffset(13)]
        public FixedArray_byte_7 F_0D;
    }

    // intf_P4_02.inc:123

    // intf_P4_02.inc:123

}
