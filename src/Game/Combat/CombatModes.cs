// Translated from AoWE.pas.
namespace Aow.Game.Combat;

/// <summary>A set of <see cref="CombatMode"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(CombatMode))]
public enum CombatModes : byte
{
    None = 0,
    Melee = 1 << 1,
    Touch = 1 << 2,
    ShortRange = 1 << 3,
    MediumRange = 1 << 4,
    LongRange = 1 << 5,
}

/// <summary>The conversions between a single <see cref="CombatMode"/> and its flag in <see cref="CombatModes"/>.</summary>
public static class CombatModeConversions
{
    extension(CombatMode value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public CombatModes ToFlag() => (int)value is >= 0 and <= 5 ? (CombatModes)(1U << (int)value) : CombatModes.None;
    }

    extension(CombatModes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public CombatMode ToCombatMode() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (CombatMode)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
