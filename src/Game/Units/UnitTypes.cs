// Translated from AoWE.pas.
namespace Aow.Game.Units;

/// <summary>A set of <see cref="UnitType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(UnitType))]
public enum UnitTypes : byte
{
    None = 0,
    Humanoid = 1 << 0,
    Creature = 1 << 1,
    Machine = 1 << 2,
}

/// <summary>The conversions between a single <see cref="UnitType"/> and its flag in <see cref="UnitTypes"/>.</summary>
public static class UnitTypeConversions
{
    extension(UnitType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public UnitTypes ToFlag() => (int)value is >= 0 and <= 2 ? (UnitTypes)(1U << (int)value) : UnitTypes.None;
    }

    extension(UnitTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public UnitType ToUnitType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (UnitType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
