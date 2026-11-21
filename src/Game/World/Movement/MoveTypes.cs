// Translated from AoWE.pas.
namespace Aow.Game.World.Movement;

/// <summary>A set of <see cref="MoveType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(MoveType))]
public enum MoveTypes : byte
{
    None = 0,
    Walking = 1 << 0,
    Flying = 1 << 1,
    Swimming = 1 << 2,
    Forestry = 1 << 3,
    CaveCrawling = 1 << 4,
    Mountaineering = 1 << 5,
    LavaCrossing = 1 << 6,
    Tunneling = 1 << 7,
}

/// <summary>The conversions between a single <see cref="MoveType"/> and its flag in <see cref="MoveTypes"/>.</summary>
public static class MoveTypeConversions
{
    extension(MoveType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public MoveTypes ToFlag() => (int)value is >= 0 and <= 7 ? (MoveTypes)(1U << (int)value) : MoveTypes.None;
    }

    extension(MoveTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public MoveType ToMoveType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (MoveType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
