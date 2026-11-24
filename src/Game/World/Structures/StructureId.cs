using System.Globalization;
using Aow.Engine;

namespace Aow.Game.World.Structures;

public readonly record struct StructureId(int Value) : IOptionalOriginalId<StructureId>
{
    public const int MaxValue = 268_435_455;

    public int OriginalValue => Value;
    public static int NoneValue => 0;
    int IOriginalValue.NoneValue => NoneValue;

    public static StructureId FromInt32(int value) => new(value);
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static StructureId? FromOriginal(int value) => value == NoneValue ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class StructureIdExtensions
{
    public static int ToOriginal(this StructureId? structure) => structure?.Value ?? StructureId.NoneValue;
}
