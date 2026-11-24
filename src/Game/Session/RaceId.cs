using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Session;

public readonly record struct RaceId(sbyte Value) : IOptionalOriginalId<RaceId>, IComparable<RaceId>
{
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static RaceId FromInt32(int value) => new(unchecked((sbyte)value));
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static RaceId? FromOriginal(int value) => value is -1 or 255 ? null : FromInt32(value);
    public int CompareTo(RaceId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class RaceIdExtensions
{
    public static int ToOriginal(this RaceId? race) => race?.Value ?? RaceId.NoneValue;
}
