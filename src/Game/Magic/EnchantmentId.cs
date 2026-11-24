using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Magic;

public readonly record struct EnchantmentId(int Value) : IOptionalOriginalId<EnchantmentId>
{
    public const int MaxValue = 268_435_455;

    public int OriginalValue => Value;
    public static int NoneValue => 0;
    int IOriginalValue.NoneValue => NoneValue;

    public static EnchantmentId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static EnchantmentId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class EnchantmentIdExtensions
{
    public static int ToOriginal(this EnchantmentId? enchantment) => enchantment?.Value ?? EnchantmentId.NoneValue;
}
