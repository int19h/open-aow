using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Combat;

public readonly record struct CombatPlayerId(byte Value) : IOptionalOriginalId<CombatPlayerId>, IComparable<CombatPlayerId>
{
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => 255;
    int IOriginalValue.NoneValue => NoneValue;

    public static CombatPlayerId FromInt32(int value) => new(unchecked((byte)value));
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static CombatPlayerId? FromOriginal(int value) => value is -1 or 255 ? null : FromInt32(value);
    public int CompareTo(CombatPlayerId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class CombatPlayerIdExtensions
{
    public static int ToOriginal(this CombatPlayerId? player) => player?.Value ?? CombatPlayerId.NoneValue;
}
