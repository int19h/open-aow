using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Units;

public readonly record struct LibraryId(int Value) : IOptionalOriginalId<LibraryId>
{
    public const int MaxValue = 268_435_455;

    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static LibraryId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static LibraryId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class LibraryIdExtensions
{
    public static int ToOriginal(this LibraryId? library) => library?.Value ?? LibraryId.NoneValue;
}
