using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Units;

public readonly record struct ItemId(int Value) : IOriginalId<ItemId>
{
    public const int MaxValue = 268_435_455;

    public int OriginalValue => Value;
    static int IOriginalId<ItemId>.NoneValue => 0;
    int IOriginalValue.NoneValue => 0;

    public static ItemId FromInt32(int value) => new(value);
    /// <summary>Reads a required integer message form. Zero is a present item id.</summary>
    public static ItemId? FromOriginal(int value) => FromInt32(value);
    public static ItemId TagOfHero(UnitId hero) => new(hero.Value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}
