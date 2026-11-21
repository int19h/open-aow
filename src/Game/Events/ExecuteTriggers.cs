// Translated from EventLog.pas.
namespace Aow.Game.Events;

/// <summary>A set of <see cref="ExecuteTrigger"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(ExecuteTrigger))]
public enum ExecuteTriggers : byte
{
    None = 0,
    Event = 1 << 0,
    User = 1 << 1,
    NewTurn = 1 << 2,
}

/// <summary>The conversions between a single <see cref="ExecuteTrigger"/> and its flag in <see cref="ExecuteTriggers"/>.</summary>
public static class ExecuteTriggerConversions
{
    extension(ExecuteTrigger value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public ExecuteTriggers ToFlag() => (int)value is >= 0 and <= 2 ? (ExecuteTriggers)(1U << (int)value) : ExecuteTriggers.None;
    }

    extension(ExecuteTriggers flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public ExecuteTrigger ToExecuteTrigger() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (ExecuteTrigger)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
