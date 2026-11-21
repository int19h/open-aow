// Translated from AoWE.pas.
namespace Aow.Game.Session;

/// <summary>A set of <see cref="DiplomaticRelation"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(DiplomaticRelation))]
public enum DiplomaticRelations : byte
{
    None = 0,
    Peace = 1 << 0,
    War = 1 << 1,
    Alliance = 1 << 2,
    Unknown = 1 << 3,
}

/// <summary>The conversions between a single <see cref="DiplomaticRelation"/> and its flag in <see cref="DiplomaticRelations"/>.</summary>
public static class DiplomaticRelationConversions
{
    extension(DiplomaticRelation value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public DiplomaticRelations ToFlag() => (int)value is >= 0 and <= 3 ? (DiplomaticRelations)(1U << (int)value) : DiplomaticRelations.None;
    }

    extension(DiplomaticRelations flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public DiplomaticRelation ToDiplomaticRelation() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (DiplomaticRelation)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
