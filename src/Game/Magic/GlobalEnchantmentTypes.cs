// Translated from AoWE.pas.
namespace Aow.Game.Magic;

/// <summary>A set of <see cref="GlobalEnchantmentType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(GlobalEnchantmentType))]
public enum GlobalEnchantmentTypes : byte
{
    None = 0,
    Hatred = 1 << 1,
    Harmony = 1 << 2,
    PowerLeak = 1 << 3,
    Flood = 1 << 4,
    FireMastery = 1 << 5,
    SpellWard = 1 << 6,
}

/// <summary>The conversions between a single <see cref="GlobalEnchantmentType"/> and its flag in <see cref="GlobalEnchantmentTypes"/>.</summary>
public static class GlobalEnchantmentTypeConversions
{
    extension(GlobalEnchantmentType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public GlobalEnchantmentTypes ToFlag() => (int)value is >= 0 and <= 6 ? (GlobalEnchantmentTypes)(1U << (int)value) : GlobalEnchantmentTypes.None;
    }

    extension(GlobalEnchantmentTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public GlobalEnchantmentType ToGlobalEnchantmentType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (GlobalEnchantmentType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
