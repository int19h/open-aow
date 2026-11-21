// Translated from AoWE.pas.
namespace Aow.Game.AI;

/// <summary>A set of <see cref="AIPlayerProcessPhase"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(AIPlayerProcessPhase))]
public enum AIPlayerProcessPhases : byte
{
    None = 0,
    Main = 1 << 1,
    Loop = 1 << 2,
}

/// <summary>The conversions between a single <see cref="AIPlayerProcessPhase"/> and its flag in <see cref="AIPlayerProcessPhases"/>.</summary>
public static class AIPlayerProcessPhaseConversions
{
    extension(AIPlayerProcessPhase value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public AIPlayerProcessPhases ToFlag() => (int)value is >= 0 and <= 2 ? (AIPlayerProcessPhases)(1U << (int)value) : AIPlayerProcessPhases.None;
    }

    extension(AIPlayerProcessPhases flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public AIPlayerProcessPhase ToAIPlayerProcessPhase() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (AIPlayerProcessPhase)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
