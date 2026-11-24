using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Combat;

public readonly record struct CpuId(int Value) : IOptionalOriginalId<CpuId>
{
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static CpuId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static CpuId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class CpuIdExtensions
{
    public static int ToOriginal(this CpuId? cpu) => cpu?.Value ?? CpuId.NoneValue;
}
