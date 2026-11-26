// Translated from HSEngine.pas (the terrain list and the hexagon sprite answers).
namespace Aow.Game.World.Map;

/// <summary>
/// What a map object's footprint says about one hexagon, for the terrain or for the overlay of the field
/// there. The hexagon is outside the footprint (<see cref="Outside"/>, stored -2), covered without a value
/// of its own, which leaves the field's value as it is (<see cref="Keep"/>, stored -1), or covered with a
/// value. The stored numbers convert at the terrain list, the boundary of the stored footprint.
/// </summary>
/// <remarks>
/// A struct, not a closed hierarchy: every field update asks each sprite of the field for its answer, and
/// the answer is a value. The constructor is private because a value outside the footprint has no value.
/// </remarks>
public readonly record struct FootprintCell<T> where T : struct, Enum
{
    private FootprintCell(bool isCovered, T? value)
    {
        IsCovered = isCovered;
        Value = value;
    }

    /// <summary>The hexagon is outside the footprint. The default value is this one.</summary>
    public static FootprintCell<T> Outside => default;

    /// <summary>The hexagon is covered and leaves the field's value as it is.</summary>
    public static FootprintCell<T> Keep => new(isCovered: true, value: null);

    /// <summary>Whether the hexagon is inside the footprint, with or without a value of its own.</summary>
    public bool IsCovered { get; }

    /// <summary>The value of a covered hexagon. Null outside the footprint and where the hexagon keeps the field's value.</summary>
    public T? Value { get; }

    /// <summary>A covered hexagon with a value.</summary>
    public static FootprintCell<T> Of(T value) => new(isCovered: true, value: value);

    /// <summary>
    /// A covered hexagon with a value, as <see cref="Of"/>. <c>default(T)</c> converts to a covered hexagon
    /// with the member of 0 (Water, Mountain), not to <see cref="Outside"/>: the default answer is
    /// <c>default(FootprintCell&lt;T&gt;)</c>.
    /// </summary>
    public static implicit operator FootprintCell<T>(T value) => Of(value);
}

/// <summary>The conversions between a terrain footprint answer and the original stored number: -2 outside, -1 keep, else the terrain.</summary>
public static class TerrainCellConversions
{
    extension(FootprintCell<TerrainType>)
    {
        /// <summary>
        /// The answer of an original terrain number: -2 outside, -1 keep, 0 to 15 a member. A number below -2
        /// answers outside, as the original tests of -1 and up read it. A number above 15 names no terrain and throws.
        /// </summary>
        public static FootprintCell<TerrainType> FromOriginal(int value) => value switch
        {
            -1 => FootprintCell<TerrainType>.Keep,
            >= 0 and <= 15 => (TerrainType)value,
            > 15 => throw new ArgumentOutOfRangeException(paramName: nameof(value), actualValue: value, message: "A terrain number above -1 is 0 to 15."),
            _ => FootprintCell<TerrainType>.Outside,
        };
    }

    extension(FootprintCell<TerrainType> cell)
    {
        /// <summary>The original terrain number of the answer.</summary>
        public sbyte ToOriginal() => cell.Value is { } terrain ? (sbyte)terrain : cell.IsCovered ? (sbyte)-1 : (sbyte)-2;
    }
}

/// <summary>The conversions between an overlay footprint answer and the original stored number: -2 outside, -1 keep, else the overlay.</summary>
public static class OverlayCellConversions
{
    extension(FootprintCell<OverlayType>)
    {
        /// <summary>
        /// The answer of an original overlay number: -2 outside, -1 keep, 0 to 11 a member. A number below -2
        /// answers outside, as the original tests of -1 and up read it. A number above 11 names no overlay and throws.
        /// </summary>
        public static FootprintCell<OverlayType> FromOriginal(int value) => value switch
        {
            -1 => FootprintCell<OverlayType>.Keep,
            >= 0 and <= 11 => (OverlayType)value,
            > 11 => throw new ArgumentOutOfRangeException(paramName: nameof(value), actualValue: value, message: "An overlay number above -1 is 0 to 11."),
            _ => FootprintCell<OverlayType>.Outside,
        };
    }

    extension(FootprintCell<OverlayType> cell)
    {
        /// <summary>The original overlay number of the answer.</summary>
        public sbyte ToOriginal() => cell.Value is { } overlay ? (sbyte)overlay : cell.IsCovered ? (sbyte)-1 : (sbyte)-2;
    }
}
