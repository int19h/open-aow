// Translated from AoWE.pas.
namespace Aow.Game.Units;

/// <summary>A set of <see cref="Alignment"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(Alignment))]
public enum Alignments : byte
{
    None = 0,
    PureGood = 1 << 0,
    Good = 1 << 1,
    PureNeutral = 1 << 2,
    Neutral = 1 << 3,
    Evil = 1 << 4,
    PureEvil = 1 << 5,
    Unaligned = 1 << 6,
}

/// <summary>The conversions between a single <see cref="Alignment"/> and its flag in <see cref="Alignments"/>.</summary>
public static class AlignmentConversions
{
    extension(Alignment value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public Alignments ToFlag() => (int)value is >= 0 and <= 6 ? (Alignments)(1U << (int)value) : Alignments.None;
    }

    extension(Alignments flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public Alignment ToAlignment() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (Alignment)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
