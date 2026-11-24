using System.Globalization;
using Aow.Engine;

namespace Aow.Game.Combat;

public readonly record struct CombatObjectId(byte Value) : IOptionalOriginalId<CombatObjectId>
{
    public int Index => Value;
    public int OriginalValue => Value;
    public static int NoneValue => -1;
    int IOriginalValue.NoneValue => NoneValue;

    public static CombatObjectId FromInt32(int value) => new(unchecked((byte)value));
    /// <summary>Reads the integer message form. Stored forms go through <see cref="PersistSupport"/>.</summary>
    public static CombatObjectId? FromOriginal(int value) => value == -1 ? null : FromInt32(value);
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public static class CombatObjectIdExtensions
{
    public static int ToOriginal(this CombatObjectId? objectId) => objectId?.Value ?? CombatObjectId.NoneValue;
}
