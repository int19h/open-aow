using System.Globalization;
using Aow.Engine;

namespace Aow.Game.AI.Groups;

public readonly record struct GroupId(int Value) : IOptionalOriginalId<GroupId>
{
    public const int MaxValue = 268_435_455;

    public int OriginalValue => Value;
    public static int NoneValue => 0;
    int IOriginalValue.NoneValue => NoneValue;

    public static GroupId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static GroupId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class GroupIdExtensions
{
    public static int ToOriginal(this GroupId? group) => group?.Value ?? GroupId.NoneValue;
}
