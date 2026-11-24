using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Magic.Spells;

public readonly partial record struct SpellId(int Value) : IOptionalOriginalId<SpellId>, IComparable<SpellId>
{
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => 0;
    int IOriginalValue.NoneValue => NoneValue;

    public static SpellId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static SpellId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public int CompareTo(SpellId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class SpellIdExtensions
{
    public static int ToOriginal(this SpellId? spell) => spell?.Value ?? SpellId.NoneValue;
}
