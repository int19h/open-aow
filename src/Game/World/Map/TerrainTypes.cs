// Translated from AoWE.pas.
namespace Aow.Game.World.Map;

/// <summary>A set of <see cref="TerrainType"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(TerrainType))]
public enum TerrainTypes : ushort
{
    None = 0,
    Water = 1 << 0,
    Grass = 1 << 1,
    Desert = 1 << 2,
    Snow = 1 << 3,
    Steppe = 1 << 4,
    Wasteland = 1 << 5,
    Ice = 1 << 6,
    Earth = 1 << 7,
    Rock = 1 << 8,
    Lava = 1 << 9,
    UndergroundWater = 1 << 10,
    Reserved = 1 << 11,
    Dirt = 1 << 12,
    UndergroundIce = 1 << 13,
    Coast = 1 << 14,
    Border = 1 << 15,
}

/// <summary>The conversions between a single <see cref="TerrainType"/> and its flag in <see cref="TerrainTypes"/>.</summary>
public static class TerrainTypeConversions
{
    extension(TerrainType value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public TerrainTypes ToFlag() => (int)value is >= 0 and <= 15 ? (TerrainTypes)(1U << (int)value) : TerrainTypes.None;

        /// <summary>The original terrain number of a terrain.</summary>
        public sbyte ToOriginal() => (sbyte)value;
    }

    extension(TerrainTypes flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public TerrainType ToTerrainType() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (TerrainType)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }

    extension(TerrainType)
    {
        /// <summary>The terrain of an original terrain number: -1 is no terrain, 0 to 15 a member. Any other number throws.</summary>
        public static TerrainType? FromOriginal(int value) => value switch
        {
            -1 => null,
            >= 0 and <= 15 => (TerrainType)value,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(value), actualValue: value, message: "A terrain number is -1 or 0 to 15."),
        };
    }

    extension(TerrainType? value)
    {
        /// <summary>The original terrain number: -1 for no terrain.</summary>
        public sbyte ToOriginal() => value is { } terrain ? (sbyte)terrain : (sbyte)-1;
    }
}
