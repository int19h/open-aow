// Translated from AoWE.pas.
namespace Aow.Game.Units;

/// <summary>A set of <see cref="ItemType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(ItemType))]
public enum ItemTypes : byte
{
    None = 0,
    Head = 1 << 0,
    Torso = 1 << 1,
    Attack = 1 << 2,
    Defense = 1 << 3,
    Ring = 1 << 4,
    Scroll = 1 << 5,
    Use = 1 << 6,
}

/// <summary>The conversions between a single <see cref="ItemType"/> and its flag in <see cref="ItemTypes"/>.</summary>
public static class ItemTypeConversions
{
    extension(ItemType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public ItemTypes ToFlag() => (int)value is >= 0 and <= 6 ? (ItemTypes)(1U << (int)value) : ItemTypes.None;
    }

    extension(ItemTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public ItemType ToItemType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (ItemType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
