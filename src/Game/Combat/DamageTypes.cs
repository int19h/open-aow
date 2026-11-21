// Translated from AoWE.pas.
namespace Aow.Game.Combat;

/// <summary>A set of <see cref="DamageType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(DamageType))]
public enum DamageTypes : ushort
{
    None = 0,
    Fire = 1 << 0,
    Cold = 1 << 1,
    Lightning = 1 << 2,
    Magic = 1 << 3,
    Poison = 1 << 4,
    Death = 1 << 5,
    Holy = 1 << 6,
    Physical = 1 << 7,
    Wall = 1 << 8,
    NonSpecific = 1 << 9,
}

/// <summary>The conversions between a single <see cref="DamageType"/> and its flag in <see cref="DamageTypes"/>.</summary>
public static class DamageTypeConversions
{
    extension(DamageType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public DamageTypes ToFlag() => (int)value is >= 0 and <= 9 ? (DamageTypes)(1U << (int)value) : DamageTypes.None;
    }

    extension(DamageTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public DamageType ToDamageType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (DamageType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
