using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Units;

public readonly record struct UnitTypeId(int Value) : IOptionalOriginalId<UnitTypeId>, IComparable<UnitTypeId>
{
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static UnitTypeId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static UnitTypeId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public int CompareTo(UnitTypeId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class UnitTypeIdExtensions
{
    public static int ToOriginal(this UnitTypeId? unitType) => unitType?.Value ?? UnitTypeId.NoneValue;
}
