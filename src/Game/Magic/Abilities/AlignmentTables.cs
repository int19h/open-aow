using System.Collections.Immutable;
using Aow.Game.Session;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

internal static class AlignmentTables
{
    internal static Status AlignmentToStatus(Alignment alignment, Alignment otherAlignment) => Race.StatusValueToStatus(AlignmentStatusValues[(int)alignment][(int)otherAlignment]);

    internal static sbyte AlignmentToIndependentRelation(Alignment alignment)
    {
        return alignment switch
        {
            Alignment.PureGood or Alignment.Good => -2,
            Alignment.Evil or Alignment.PureEvil => -4,
            Alignment.PureNeutral or Alignment.Neutral => -3,
            Alignment.Unaligned or _ => -1,
        };
    }

    internal static ImmutableArray<ImmutableArray<sbyte>> AlignmentStatusValues { get; } = [[100, 90, 30, 40, 10, 0, 50], [90, 90, 40, 50, 10, 10, 50], [30, 40, 70, 50, 40, 30, 50], [40, 50, 50, 50, 50, 40, 50], [10, 10, 40, 50, 90, 90, 50], [0, 10, 30, 40, 90, 100, 50], [50, 50, 50, 50, 50, 50, 50]];

    internal static ImmutableArray<ImmutableArray<sbyte>> AlignmentToStatusValue { get; } = [[100, 90, 30, 40, 10, 0, 50], [90, 90, 40, 50, 10, 10, 50], [30, 40, 70, 50, 40, 30, 50], [40, 50, 50, 50, 50, 40, 50], [10, 10, 40, 50, 90, 90, 50], [0, 10, 30, 40, 90, 100, 50], [50, 50, 50, 50, 50, 50, 50]];

    internal static DiplomaticRelation AlignmentToDiplomaticRelation(Alignment A, Alignment B)
    {
        return A is Alignment.PureGood or Alignment.Good
            ? B is Alignment.PureGood or Alignment.Good
                ? DiplomaticRelation.Peace
                : B is Alignment.PureNeutral or Alignment.Neutral or Alignment.Unaligned
                    ? DiplomaticRelation.Unknown
                    : DiplomaticRelation.War
            : A is Alignment.Evil or Alignment.PureEvil
                ? B is Alignment.Evil or Alignment.PureEvil
                    ? DiplomaticRelation.Peace
                    : B is Alignment.PureNeutral or Alignment.Neutral or Alignment.Unaligned
                        ? DiplomaticRelation.Unknown
                        : DiplomaticRelation.War
                : A == Alignment.PureNeutral && B == Alignment.PureNeutral ? DiplomaticRelation.Peace : DiplomaticRelation.Unknown;
    }

    internal static DiplomaticRelation StatusToDiplomaticRelation(Status status)
    {
        return status switch
        {
            Status.Polite or Status.Friendly => DiplomaticRelation.Peace,
            Status.Neutral => DiplomaticRelation.Unknown,
            Status.Hostile or Status.Wary => DiplomaticRelation.War,
            _ => DiplomaticRelation.Unknown,
        };
    }

    internal static Alignments PureGoodAlignments { get; } = Alignments.PureGood | Alignments.Good | Alignments.Unaligned;

    internal static Alignments GoodAlignments { get; } = Alignments.PureGood | Alignments.Good | Alignments.Neutral | Alignments.Unaligned;

    internal static Alignments NeutralAlignments { get; } = Alignments.Good | Alignments.Neutral | Alignments.Evil | Alignments.Unaligned;

    internal static Alignments EvilAlignments { get; } = Alignments.Neutral | Alignments.Evil | Alignments.PureEvil | Alignments.Unaligned;

    internal static Alignments PureEvilAlignments { get; } = Alignments.Evil | Alignments.PureEvil | Alignments.Unaligned;

}
