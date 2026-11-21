// Translated from AoWE.pas.
namespace Aow.Game.Magic;

/// <summary>A set of <see cref="MagicSphere"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(MagicSphere))]
public enum MagicSpheres : ushort
{
    None = 0,
    Cosmos = 1 << 0,
    Life = 1 << 1,
    Death = 1 << 2,
    Earth = 1 << 3,
    Air = 1 << 4,
    Fire = 1 << 5,
    Water = 1 << 6,
    Secret = 1 << 7,
}

/// <summary>The conversions between a single <see cref="MagicSphere"/> and its flag in <see cref="MagicSpheres"/>.</summary>
public static class MagicSphereConversions
{
    extension(MagicSphere value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public MagicSpheres ToFlag() => (int)value is >= 0 and <= 8 ? (MagicSpheres)(1U << (int)value) : MagicSpheres.None;
    }

    extension(MagicSpheres flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public MagicSphere ToMagicSphere() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (MagicSphere)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
