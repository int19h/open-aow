using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Session;

public readonly record struct ScenarioId(int Value) : IOptionalOriginalId<ScenarioId>
{
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static ScenarioId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static ScenarioId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class ScenarioIdExtensions
{
    public static int ToOriginal(this ScenarioId? scenario) => scenario?.Value ?? ScenarioId.NoneValue;
}
