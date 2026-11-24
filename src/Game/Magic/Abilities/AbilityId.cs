using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Magic.Abilities;

public readonly partial record struct AbilityId(int Value) : IOptionalOriginalId<AbilityId>, IComparable<AbilityId>
{
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static AbilityId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static AbilityId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public int CompareTo(AbilityId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class AbilityIdExtensions
{
    public static int ToOriginal(this AbilityId? ability) => ability?.Value ?? AbilityId.NoneValue;
}
