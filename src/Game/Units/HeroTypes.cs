// Translated from AoWE.pas.
namespace Aow.Game.Units;

/// <summary>A set of <see cref="HeroType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(HeroType))]
public enum HeroTypes : byte
{
    None = 0,
    Hero = 1 << 0,
    Leader = 1 << 1,
    HeroLeader = 1 << 2,
    Merchant = 1 << 3,
}

/// <summary>The conversions between a single <see cref="HeroType"/> and its flag in <see cref="HeroTypes"/>.</summary>
public static class HeroTypeConversions
{
    extension(HeroType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public HeroTypes ToFlag() => (int)value is >= 0 and <= 3 ? (HeroTypes)(1U << (int)value) : HeroTypes.None;
    }

    extension(HeroTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public HeroType ToHeroType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (HeroType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
