// Translated from City.pas.
namespace Aow.Game.World.Structures;

/// <summary>A set of <see cref="CityEnchantmentId"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(CityEnchantmentId))]
public enum CityEnchantmentIds : byte
{
    None = 0,
    FountainOfLife = 1 << 0,
    GoldRush = 1 << 1,
    AntiMagicShell = 1 << 2,
    Warmonger = 1 << 3,
}

/// <summary>The conversions between a single <see cref="CityEnchantmentId"/> and its flag in <see cref="CityEnchantmentIds"/>.</summary>
public static class CityEnchantmentIdConversions
{
    extension(CityEnchantmentId value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public CityEnchantmentIds ToFlag() => (int)value is >= 0 and <= 3 ? (CityEnchantmentIds)(1U << (int)value) : CityEnchantmentIds.None;
    }

    extension(CityEnchantmentIds flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public CityEnchantmentId ToCityEnchantmentId() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (CityEnchantmentId)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
