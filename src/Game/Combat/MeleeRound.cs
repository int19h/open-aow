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

    public int Count { get; private set; }

    private int _capacity;
    private Strike[] _strikes = [];
    private CombatObject _attacker;
    private CombatObject _defender;
    private int _attackerDamageValue;
    private int _defenderDamageValue;
    private Troop _attackerUnit;
    private Troop _defenderUnit;
    private int _attackerStrikes;
    private int _defenderStrikes;

    public Set<DamageType> AttackerDamageTypes { get; private set; }

    private Set<DamageType> _defenderDamageTypes;

    // AoWAb.PAS:1492
    public virtual void Dispose()
    {
        SetCapacity(0);
    }

    // AoWAb.PAS:1498
    public void SetCapacity(int newCapacity)
    {
        _capacity = newCapacity;
        if (_capacity < Count)
        {
            Count = _capacity;
        }

        Array.Resize(ref _strikes, newCapacity);
    }

    // AoWAb.PAS:1505
    public void SetCount(int newCount)
    {
        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > Count)
        {
            _strikes.AsSpan(Count, newCount - Count).Clear();
        }

        Count = newCount;
    }

    // AoWAb.PAS:1513
    public void GetMeleeStrike(int index, ref Strike strike)
    {
        strike = _strikes[index];
    }

    // AoWAb.PAS:1518
    public void Reset()
    {
        SetCount(0);
        _attacker = null;
        _defender = null;
        _attackerUnit = null;
        _defenderUnit = null;
        _attackerDamageValue = -1;
        _defenderDamageValue = -1;
        _attackerStrikes = 0;
        _defenderStrikes = 0;
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
        SetCount(_attackerStrikes + _defenderStrikes);
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (side == CombatSideIndex.Attacker)
            {
                source = _attackerUnit;
                target = _defenderUnit;
            }
            else
            {
                source = _defenderUnit;
                target = _attackerUnit;
            }

            ref Strike strike = ref _strikes[I];
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
                    if (_defenderStrikes != defenderCount)
                    {
                        side = CombatSideIndex.Defender;
                    }

                    break;
                case CombatSideIndex.Defender:
                    defenderCount += 1;
                    if (_attackerStrikes != attackerCount)
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
        _attackerUnit = attacker;
        _defenderUnit = defender;
        AttackerDamageTypes = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(attacker);
        _defenderDamageTypes = Set<DamageType>.Of();
        if (AttackerDamageTypes != Set<DamageType>.Of())
        {
            _attackerStrikes = 1;
            CalculateUnitStrikes(CombatSideIndex.Attacker);
        }
    }

    // AoWAb.PAS:1614
    public void CalculateUnit(Troop attacker, Troop defender)
    {
        CombatSideIndex side;
        Reset();
        _attackerUnit = attacker;
        _defenderUnit = defender;
        AttackerDamageTypes = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(attacker);
        if (AttackerDamageTypes != Set<DamageType>.Of())
        {
            _attackerStrikes = 2;
            if (_attackerUnit.GetAbilityEnabled(115))
            {
                _attackerStrikes += 1;
            }
        }
        else
        {
            _attackerStrikes = 0;
        }

        _defenderDamageTypes = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(defender);
        if (_defenderDamageTypes != Set<DamageType>.Of())
        {
            _defenderStrikes = 2;
        }
        else
        {
            _defenderStrikes = 0;
        }

        if (_defenderUnit.GetAbilityEnabled(116) && !_attackerUnit.GetAbilityEnabled(116) && _defenderStrikes != 0)
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
        SetCount(_attackerStrikes + _defenderStrikes);
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (side == CombatSideIndex.Attacker)
            {
                source = _attacker;
                target = _defender;
            }
            else
            {
                source = _defender;
                target = _attacker;
            }

            ref Strike strike = ref _strikes[I];
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
                    if (_defenderStrikes != defenderCount)
                    {
                        side = CombatSideIndex.Defender;
                    }

                    break;
                case CombatSideIndex.Defender:
                    defenderCount += 1;
                    if (_attackerStrikes != attackerCount)
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
        _attacker = attacker;
        _defender = defender;
        AttackerDamageTypes = CombatObject.MeleeDamageTypes(attacker);
        if (AttackerDamageTypes != Set<DamageType>.Of() && !attacker.GetLocked())
        {
            _attackerStrikes = 2;
            if (_attacker.GetAbilityEnabled(115))
            {
                _attackerStrikes += 1;
            }
        }
        else
        {
            _attackerStrikes = 0;
        }

        _defenderDamageTypes = CombatObject.MeleeDamageTypes(defender);
        if (_defenderDamageTypes != Set<DamageType>.Of() && !defender.GetLocked())
        {
            _defenderStrikes = 2;
        }
        else
        {
            _defenderStrikes = 0;
        }

        if (defender.GetAbilityEnabled(116) && !attacker.GetAbilityEnabled(116) && _defenderStrikes != 0)
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
        _attacker = attacker;
        _defender = defender;
        AttackerDamageTypes = CombatObject.MeleeDamageTypes(attacker);
        _defenderDamageTypes = Set<DamageType>.Of();
        if (AttackerDamageTypes != Set<DamageType>.Of() && !_attacker.GetLocked())
        {
            _attackerStrikes = 2;
            if (_attacker.GetAbilityEnabled(115))
            {
                _attackerStrikes += 1;
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
            ref Strike strike = ref _strikes[index];
            switch (strike.Side)
            {
                case CombatSideIndex.Attacker:
                    if (!_attacker.GetLocked())
                    {
                        result = new StrikeCombatAction(null);
                        var damage = _attacker.GetDamage();
                        var attack = _attacker.GetAttack();
                        result.Generate(_attacker, _defender, (sbyte)(attack + strike.AttackBonus), (sbyte)(damage + strike.DamageBonus), AttackerDamageTypes);
                        if (_attacker.GetAbilityEnabled(118))
                        {
                            result.StrikeMode = result.StrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.LifeStealing);
                        }
                    }

                    break;
                case CombatSideIndex.Defender:
                    if (!_defender.GetLocked())
                    {
                        result = new StrikeCombatAction(null);
                        result.SetDefensive(true);
                        var damage = _defender.GetDamage();
                        var attack = _defender.GetAttack();
                        result.Generate(_defender, _attacker, (sbyte)(attack + strike.AttackBonus), (sbyte)(damage + strike.DamageBonus), _defenderDamageTypes);
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
        _attackerDamageValue = 0;
        _defenderDamageValue = 0;
        I = 0;
        lifeStealingDamageValue = 0;
        source = _attacker;
        if (source != null)
        {
            lifeStealing = source.GetAbilityEnabled(118);
            if (lifeStealing && _defender is CombatUnit && ((CombatUnit)_defender).Troop.GetUnitType() == UnitType.Machine)
            {
                lifeStealing = false;
            }
        }
        else
        {
            lifeStealing = _attackerUnit.GetAbilityEnabled(118);
            if (lifeStealing && _defenderUnit.GetUnitType() == UnitType.Machine)
            {
                lifeStealing = false;
            }
        }

        while (I != Count)
        {
            if (_strikes[I].Side == CombatSideIndex.Attacker)
            {
                _attackerDamageValue += _strikes[I].DamageValue;
                if (_strikes[I].DamageValue > 100)
                {
                    lifeStealingDamageValue += 100;
                }
                else
                {
                    lifeStealingDamageValue += _strikes[I].DamageValue;
                }
            }
            else
            {
                _defenderDamageValue += _strikes[I].DamageValue;
            }

            I += 1;
        }

        if (lifeStealing && _defenderDamageValue > 0)
        {
            _defenderDamageValue -= lifeStealingDamageValue;
            if (_defenderDamageValue <= 0)
            {
                _defenderDamageValue = 1;
            }
        }
    }

    // AoWAb.PAS:1831
    public int GetAttackerDamageValue()
    {
        if (_attackerDamageValue == -1)
        {
            CalculateDamageValue();
        }

        return _attackerDamageValue;
    }

    // AoWAb.PAS:1837
    public int GetDefenderDamageValue()
    {
        if (_defenderDamageValue == -1)
        {
            CalculateDamageValue();
        }

        return _defenderDamageValue;
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
        public Array7<byte> Reserved0D;
    }

    // intf_P4_02.inc:123

    // intf_P4_02.inc:123

}
