// Translated from AoWE.pas.
namespace Aow.Game.AI.Groups;

/// <summary>A set of <see cref="AIMoveExecutorUnitStatus"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(AIMoveExecutorUnitStatus))]
public enum AIMoveExecutorUnitStatuses : byte
{
    None = 0,
    Idle = 1 << 0,
    Ready = 1 << 1,
    Queued = 1 << 2,
    Moving = 1 << 3,
    Done = 1 << 4,
    Lost = 1 << 5,
    Failed = 1 << 6,
}

/// <summary>The conversions between a single <see cref="AIMoveExecutorUnitStatus"/> and its flag in <see cref="AIMoveExecutorUnitStatuses"/>.</summary>
public static class AIMoveExecutorUnitStatusConversions
{
    extension(AIMoveExecutorUnitStatus value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public AIMoveExecutorUnitStatuses ToFlag() => (int)value is >= 0 and <= 6 ? (AIMoveExecutorUnitStatuses)(1U << (int)value) : AIMoveExecutorUnitStatuses.None;
    }

    extension(AIMoveExecutorUnitStatuses flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public AIMoveExecutorUnitStatus ToAIMoveExecutorUnitStatus() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (AIMoveExecutorUnitStatus)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
