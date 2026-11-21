// Translated from AoWE.pas.
namespace Aow.Game.Magic.Spells;

/// <summary>A set of <see cref="SpellType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(SpellType))]
public enum SpellTypes : byte
{
    None = 0,
    Combat = 1 << 0,
    Unit = 1 << 1,
    Global = 1 << 2,
}

/// <summary>The conversions between a single <see cref="SpellType"/> and its flag in <see cref="SpellTypes"/>.</summary>
public static class SpellTypeConversions
{
    extension(SpellType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public SpellTypes ToFlag() => (int)value is >= 0 and <= 2 ? (SpellTypes)(1U << (int)value) : SpellTypes.None;
    }

    extension(SpellTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public SpellType ToSpellType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (SpellType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
