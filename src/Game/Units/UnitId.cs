using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Units;

public readonly record struct UnitId(int Value) : IOptionalOriginalId<UnitId>
{
    public const int MaxValue = 268_435_455;

    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static UnitId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static UnitId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class UnitIdExtensions
{
    public static int ToOriginal(this UnitId? unit) => unit?.Value ?? UnitId.NoneValue;
}
