using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Events;

public readonly record struct EventLogId(int Value) : IOptionalOriginalId<EventLogId>
{
    public const int MaxValue = 16_777_215;

    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static EventLogId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static EventLogId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class EventLogIdExtensions
{
    public static int ToOriginal(this EventLogId? log) => log?.Value ?? EventLogId.NoneValue;
}
