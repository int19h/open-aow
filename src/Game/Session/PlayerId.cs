using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Session;

public readonly record struct PlayerId(sbyte Value) : IOptionalOriginalId<PlayerId>, IComparable<PlayerId>
{
    public static PlayerId Independent { get; } = new(0);
    public static PlayerId Player1 { get; } = new(1);
    public static PlayerId Player2 { get; } = new(2);
    public static PlayerId Player3 { get; } = new(3);
    public static PlayerId Player4 { get; } = new(4);
    public static PlayerId Player5 { get; } = new(5);
    public static PlayerId Player6 { get; } = new(6);
    public static PlayerId Player7 { get; } = new(7);
    public static PlayerId Player8 { get; } = new(8);
    public static PlayerId Player9 { get; } = new(9);
    public static PlayerId Player10 { get; } = new(10);
    public static PlayerId Player11 { get; } = new(11);
    public static PlayerId Player12 { get; } = new(12);

    public static IReadOnlyList<PlayerId> Ids { get; } = [Independent, Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8, Player9, Player10, Player11, Player12];
    public static IReadOnlyList<PlayerId> GameIds { get; } = [Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8, Player9, Player10, Player11, Player12];

    public bool IsIndependent => Value == 0;
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static PlayerId FromInt32(int value) => new(unchecked((sbyte)value));
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static PlayerId? FromOriginal(int value) => value is -1 or 255 ? null : FromInt32(value);
    public int CompareTo(PlayerId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class PlayerIdExtensions
{
    public static int ToOriginal(this PlayerId? player) => player?.Value ?? PlayerId.NoneValue;
}
