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

    public int Count;
    public int Capacity;
    public Strike[] Strikes = [];
    public CombatObject Attacker;
    public CombatObject Defender;
    public int AttackerDamageValue;
    public int DefenderDamageValue;
    public Troop AttackerUnit;
    public Troop DefenderUnit;
    public int AttackerStrikes;
    public int DefenderStrikes;
    public Set<DamageType> AttackerDamageTypes;
    public Set<DamageType> DefenderDamageTypes;

    // AoWAb.PAS:1492
    public virtual void Dispose()
    {
        SetCapacity(0);
    }

    // AoWAb.PAS:1498
    public void SetCapacity(int newCapacity)
    {
        Capacity = newCapacity;
        if (Capacity < Count)
        {
            Count = Capacity;
        }

        Array.Resize(ref Strikes, newCapacity);
    }

    // AoWAb.PAS:1505
    public void SetCount(int newCount)
    {
        if (newCount > Capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > Count)
        {
            Strikes.AsSpan(Count, newCount - Count).Clear();
        }

        Count = newCount;
    }

    // AoWAb.PAS:1513
    public void GetMeleeStrike(int index, ref Strike strike)
    {
        strike = Strikes[index];
    }

    // AoWAb.PAS:1518
    public void Reset()
    {
        SetCount(0);
        Attacker = null;
        Defender = null;
        AttackerUnit = null;
        DefenderUnit = null;
        AttackerDamageValue = -1;
        DefenderDamageValue = -1;
        AttackerStrikes = 0;
        DefenderStrikes = 0;
    }

    // AoWAb.PAS:1531
    public void CalculateUnitStrikes(CombatSideIndex firstSide)
    {
        int attackerCount;
        int defenderCount;
        CombatSideIndex side;
        Troop source;
        Troop target;
        side = firstSide;
        attackerCount = 0;
        defenderCount = 0;
        SetCount(AttackerStrikes + DefenderStrikes);
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (side == CombatSideIndex.Attacker)
            {
                source = AttackerUnit;
                target = DefenderUnit;
            }
            else
            {
                source = DefenderUnit;
                target = AttackerUnit;
            }

            ref Strike strike = ref Strikes[I];
            strike.Side = side;
            if (I == 0 || (I == 1 && side != firstSide))
            {
                if (side == CombatSideIndex.Attacker && I == 0 && source.GetAbilityEnabled(111))
                {
                    strike.DamageBonus += 2;
                }

                if (target.GetAbilityEnabled(113))
                {
                    strike.AttackBonus -= 2;
                }
            }

            bool condition = source.GetAbilityEnabled(146) || source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                strike.DamageBonus += 2;
                strike.AttackBonus += 2;
            }

            bool test = source.GetAbilityEnabled(147) || source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                strike.DamageBonus += 2;
                strike.AttackBonus += 2;
            }

            if (source.GetAbilityEnabled(112) && target.GetAbilityEnabled(63))
            {
                strike.AttackBonus += 3;
                strike.DamageBonus += 3;
            }

            // { Original uses the attacker's damage types for both sides. }
            var defense = target.GetDefense();
            var resistance = target.GetResistance();
            var immunityTypes = target.GetImmunityTypes();
            var protectionTypes = target.GetProtectionTypes();
            var damage = source.GetDamage();
            var statisticsToDamageValue = StatisticsToDamageValue(source.GetAttack() + strike.AttackBonus, damage + strike.DamageBonus, AttackerDamageTypes, defense, resistance, immunityTypes, protectionTypes);
            strike.DamageValue = statisticsToDamageValue;
            switch (side)
            {
                case CombatSideIndex.Attacker:
                    attackerCount += 1;
                    if (DefenderStrikes != defenderCount)
                    {
                        side = CombatSideIndex.Defender;
                    }

                    break;
                case CombatSideIndex.Defender:
                    defenderCount += 1;
                    if (AttackerStrikes != attackerCount)
                    {
                        side = CombatSideIndex.Attacker;
                    }

                    break;
            }
        }
    }

    // AoWAb.PAS:1600
    public void CalculateFreeUnitSwing(Troop attacker, Troop defender)
    {
        Reset();
        AttackerUnit = attacker;
        DefenderUnit = defender;
        AttackerDamageTypes = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(attacker);
        DefenderDamageTypes = Set<DamageType>.Of();
        if (AttackerDamageTypes != Set<DamageType>.Of())
        {
            AttackerStrikes = 1;
            CalculateUnitStrikes(CombatSideIndex.Attacker);
        }
    }

    // AoWAb.PAS:1614
    public void CalculateUnit(Troop attacker, Troop defender)
    {
        CombatSideIndex side;
        Reset();
        AttackerUnit = attacker;
        DefenderUnit = defender;
        AttackerDamageTypes = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(attacker);
        if (AttackerDamageTypes != Set<DamageType>.Of())
        {
            AttackerStrikes = 2;
            if (AttackerUnit.GetAbilityEnabled(115))
            {
                AttackerStrikes += 1;
            }
        }
        else
        {
            AttackerStrikes = 0;
        }

        DefenderDamageTypes = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(defender);
        if (DefenderDamageTypes != Set<DamageType>.Of())
        {
            DefenderStrikes = 2;
        }
        else
        {
            DefenderStrikes = 0;
        }

        if (DefenderUnit.GetAbilityEnabled(116) && !AttackerUnit.GetAbilityEnabled(116) && DefenderStrikes != 0)
        {
            side = CombatSideIndex.Defender;
        }
        else
        {
            side = CombatSideIndex.Attacker;
        }

        CalculateUnitStrikes(side);
    }

    // AoWAb.PAS:1642
    public void CalculateStrikes(CombatSideIndex firstSide)
    {
        int attackerCount;
        int defenderCount;
        CombatSideIndex side;
        CombatObject source;
        CombatObject target;
        side = firstSide;
        attackerCount = 0;
        defenderCount = 0;
        SetCount(AttackerStrikes + DefenderStrikes);
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (side == CombatSideIndex.Attacker)
            {
                source = Attacker;
                target = Defender;
            }
            else
            {
                source = Defender;
                target = Attacker;
            }

            ref Strike strike = ref Strikes[I];
            strike.Side = side;
            if (I == 0 || (I == 1 && side != firstSide))
            {
                if (side == CombatSideIndex.Attacker && I == 0 && source.GetAbilityEnabled(111))
                {
                    strike.DamageBonus += 2;
                }

                if (target.GetAbilityEnabled(113))
                {
                    strike.AttackBonus -= 2;
                }
            }

            bool condition = source.GetAbilityEnabled(146) || source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                strike.AttackBonus += 2;
                strike.DamageBonus += 2;
            }

            bool test = source.GetAbilityEnabled(147) || source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                strike.AttackBonus += 2;
                strike.DamageBonus += 2;
            }

            if (source.GetAbilityEnabled(112) && target.GetAbilityEnabled(63))
            {
                strike.AttackBonus += 3;
                strike.DamageBonus += 3;
            }

            // { Original uses the attacker's damage types for both sides. }
            var defense = target.GetDefense();
            var resistance = target.GetResistance();
            var immunityTypes = target.GetImmunityTypes();
            var protectionTypes = target.GetProtectionTypes();
            var damage = source.GetDamage();
            var statisticsToDamageValue = StatisticsToDamageValue(source.GetAttack() + strike.AttackBonus, damage + strike.DamageBonus, AttackerDamageTypes, defense, resistance, immunityTypes, protectionTypes);
            strike.DamageValue = statisticsToDamageValue;
            switch (side)
            {
                case CombatSideIndex.Attacker:
                    attackerCount += 1;
                    if (DefenderStrikes != defenderCount)
                    {
                        side = CombatSideIndex.Defender;
                    }

                    break;
                case CombatSideIndex.Defender:
                    defenderCount += 1;
                    if (AttackerStrikes != attackerCount)
                    {
                        side = CombatSideIndex.Attacker;
                    }

                    break;
            }
        }
    }

    // AoWAb.PAS:1711
    public void Calculate(CombatObject attacker, CombatObject defender)
    {
        CombatSideIndex side;
        Reset();
        Attacker = attacker;
        Defender = defender;
        AttackerDamageTypes = CombatObject.MeleeDamageTypes(attacker);
        if (AttackerDamageTypes != Set<DamageType>.Of() && !attacker.GetLocked())
        {
            AttackerStrikes = 2;
            if (Attacker.GetAbilityEnabled(115))
            {
                AttackerStrikes += 1;
            }
        }
        else
        {
            AttackerStrikes = 0;
        }

        DefenderDamageTypes = CombatObject.MeleeDamageTypes(defender);
        if (DefenderDamageTypes != Set<DamageType>.Of() && !defender.GetLocked())
        {
            DefenderStrikes = 2;
        }
        else
        {
            DefenderStrikes = 0;
        }

        if (defender.GetAbilityEnabled(116) && !attacker.GetAbilityEnabled(116) && DefenderStrikes != 0)
        {
            side = CombatSideIndex.Defender;
        }
        else
        {
            side = CombatSideIndex.Attacker;
        }

        CalculateStrikes(side);
    }

    // AoWAb.PAS:1739
    public void CalculateSingleSide(CombatObject attacker, CombatObject defender)
    {
        Reset();
        Attacker = attacker;
        Defender = defender;
        AttackerDamageTypes = CombatObject.MeleeDamageTypes(attacker);
        DefenderDamageTypes = Set<DamageType>.Of();
        if (AttackerDamageTypes != Set<DamageType>.Of() && !Attacker.GetLocked())
        {
            AttackerStrikes = 2;
            if (Attacker.GetAbilityEnabled(115))
            {
                AttackerStrikes += 1;
            }
        }

        CalculateStrikes(CombatSideIndex.Attacker);
    }

    // AoWAb.PAS:1754
    public StrikeCombatAction CreateStrikeCombatAction(int index)
    {
        StrikeCombatAction result = null;
        if (index < Count)
        {
            ref Strike strike = ref Strikes[index];
            switch (strike.Side)
            {
                case CombatSideIndex.Attacker:
                    if (!Attacker.GetLocked())
                    {
                        result = new StrikeCombatAction(null);
                        var damage = Attacker.GetDamage();
                        var attack = Attacker.GetAttack();
                        result.Generate(Attacker, Defender, (sbyte)(attack + strike.AttackBonus), (sbyte)(damage + strike.DamageBonus), AttackerDamageTypes);
                        if (Attacker.GetAbilityEnabled(118))
                        {
                            result.StrikeMode = result.StrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.LifeStealing);
                        }
                    }

                    break;
                case CombatSideIndex.Defender:
                    if (!Defender.GetLocked())
                    {
                        result = new StrikeCombatAction(null);
                        result.SetDefensive(true);
                        var damage = Defender.GetDamage();
                        var attack = Defender.GetAttack();
                        result.Generate(Defender, Attacker, (sbyte)(attack + strike.AttackBonus), (sbyte)(damage + strike.DamageBonus), DefenderDamageTypes);
                    }

                    break;
            }
        }

        return result;
    }

    // AoWAb.PAS:1786
    public void CalculateDamageValue()
    {
        int I;
        int lifeStealingDamageValue;
        bool lifeStealing;
        CombatObject source;
        AttackerDamageValue = 0;
        DefenderDamageValue = 0;
        I = 0;
        lifeStealingDamageValue = 0;
        source = Attacker;
        if (source != null)
        {
            lifeStealing = source.GetAbilityEnabled(118);
            if (lifeStealing && Defender is CombatUnit && ((CombatUnit)Defender).Troop.GetUnitType() == UnitType.Machine)
            {
                lifeStealing = false;
            }
        }
        else
        {
            lifeStealing = AttackerUnit.GetAbilityEnabled(118);
            if (lifeStealing && DefenderUnit.GetUnitType() == UnitType.Machine)
            {
                lifeStealing = false;
            }
        }

        while (I != Count)
        {
            if (Strikes[I].Side == CombatSideIndex.Attacker)
            {
                AttackerDamageValue += Strikes[I].DamageValue;
                if (Strikes[I].DamageValue > 100)
                {
                    lifeStealingDamageValue += 100;
                }
                else
                {
                    lifeStealingDamageValue += Strikes[I].DamageValue;
                }
            }
            else
            {
                DefenderDamageValue += Strikes[I].DamageValue;
            }

            I += 1;
        }

        if (lifeStealing && DefenderDamageValue > 0)
        {
            DefenderDamageValue -= lifeStealingDamageValue;
            if (DefenderDamageValue <= 0)
            {
                DefenderDamageValue = 1;
            }
        }
    }

    // AoWAb.PAS:1831
    public int GetAttackerDamageValue()
    {
        if (AttackerDamageValue == -1)
        {
            CalculateDamageValue();
        }

        return AttackerDamageValue;
    }

    // AoWAb.PAS:1837
    public int GetDefenderDamageValue()
    {
        if (DefenderDamageValue == -1)
        {
            CalculateDamageValue();
        }

        return DefenderDamageValue;
    }

    internal static int StatisticsToDamageValue(int attack, int damage, Set<DamageType> damageTypes, int defense, int resistance, Set<DamageType> immunityTypes, Set<DamageType> protectionTypes)
    {
        damageTypes -= immunityTypes;
        if (damageTypes != Set<DamageType>.Of(Array.Empty<DamageType>()))
        {
            attack -= defense;
            if (damageTypes - protectionTypes == Set<DamageType>.Of(Array.Empty<DamageType>()))
            {
                damage = damage + 1 >>> 1;
            }
            return AbilityRules.ExpectedDamage(damage, attack);
        }
        return 0;
    }

    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct Strike
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
        public ByteArray7 Reserved0D;
    }

    // intf_P4_02.inc:123

    // intf_P4_02.inc:123

}
