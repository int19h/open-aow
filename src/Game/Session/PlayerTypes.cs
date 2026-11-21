// Translated from AoWE.pas.
namespace Aow.Game.Session;

/// <summary>A set of <see cref="PlayerType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(PlayerType))]
public enum PlayerTypes : byte
{
    None = 0,
    Human = 1 << 0,
    Cpu1 = 1 << 1,
    Cpu2 = 1 << 2,
    Cpu3 = 1 << 3,
    Independent = 1 << 4,
    Free = 1 << 5,
    Cpu4 = 1 << 6,
    Cpu5 = 1 << 7,
}

/// <summary>The conversions between a single <see cref="PlayerType"/> and its flag in <see cref="PlayerTypes"/>.</summary>
public static class PlayerTypeConversions
{
    extension(PlayerType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public PlayerTypes ToFlag() => (int)value is >= 0 and <= 7 ? (PlayerTypes)(1U << (int)value) : PlayerTypes.None;
    }

    extension(PlayerTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public PlayerType ToPlayerType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (PlayerType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
