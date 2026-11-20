using System.Collections.Immutable;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Rtl;

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
            Alignment.None or _ => -1,
        };
    }

    internal static ImmutableArray<ImmutableArray<sbyte>> AlignmentStatusValues { get; } = [[100, 90, 30, 40, 10, 0, 50], [90, 90, 40, 50, 10, 10, 50], [30, 40, 70, 50, 40, 30, 50], [40, 50, 50, 50, 50, 40, 50], [10, 10, 40, 50, 90, 90, 50], [0, 10, 30, 40, 90, 100, 50], [50, 50, 50, 50, 50, 50, 50]];

    internal static ImmutableArray<ImmutableArray<sbyte>> AlignmentToStatusValue { get; } = [[100, 90, 30, 40, 10, 0, 50], [90, 90, 40, 50, 10, 10, 50], [30, 40, 70, 50, 40, 30, 50], [40, 50, 50, 50, 50, 40, 50], [10, 10, 40, 50, 90, 90, 50], [0, 10, 30, 40, 90, 100, 50], [50, 50, 50, 50, 50, 50, 50]];

    internal static DiplomaticRelation AlignmentToDiplomaticRelation(Alignment A, Alignment B)
    {
        return Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(A)
            ? Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(B)
                ? DiplomaticRelation.Peace
                : Set<Alignment>.Of(Alignment.PureNeutral, Alignment.Neutral, Alignment.None).Contains(B)
                    ? DiplomaticRelation.Unknown
                    : DiplomaticRelation.War
            : Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(A)
                ? Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(B)
                    ? DiplomaticRelation.Peace
                    : Set<Alignment>.Of(Alignment.PureNeutral, Alignment.Neutral, Alignment.None).Contains(B)
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

    internal static Set<Alignment> PureGoodAlignments { get; } = Set<Alignment>.FromMask(67UL);

    internal static Set<Alignment> GoodAlignments { get; } = Set<Alignment>.FromMask(75UL);

    internal static Set<Alignment> NeutralAlignments { get; } = Set<Alignment>.FromMask(90UL);

    internal static Set<Alignment> EvilAlignments { get; } = Set<Alignment>.FromMask(120UL);

    internal static Set<Alignment> PureEvilAlignments { get; } = Set<Alignment>.FromMask(112UL);

}
