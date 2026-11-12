
using Aow.Engine.Buffers;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class AlignmentTables
{
    internal static Status AlignmentToStatus(Alignment A, Alignment B) => Race.StatusValueToStatus(s_alignmentStatusValues[(int)A][(int)B]);

    internal static sbyte AlignmentToIndependentRelation(Alignment alignment)
    {
        sbyte result;
        switch (alignment)
        {
            case Alignment.PureGood:
            case Alignment.Good:
                result = -2;
                break;
            case Alignment.Evil:
            case Alignment.PureEvil:
                result = -4;
                break;
            case Alignment.PureNeutral:
            case Alignment.Neutral:
                result = -3;
                break;
            default:
                result = -1;
                break;
        }

        return result;
    }

    internal static SByteArray7By7 s_alignmentStatusValues = SByteArray7By7.From([SByteArray7.From([100, 90, 30, 40, 10, 0, 50]), SByteArray7.From([90, 90, 40, 50, 10, 10, 50]), SByteArray7.From([30, 40, 70, 50, 40, 30, 50]), SByteArray7.From([40, 50, 50, 50, 50, 40, 50]), SByteArray7.From([10, 10, 40, 50, 90, 90, 50]), SByteArray7.From([0, 10, 30, 40, 90, 100, 50]), SByteArray7.From([50, 50, 50, 50, 50, 50, 50])]);

    internal static SByteArray7By7 s_alignmentToStatusValue = SByteArray7By7.From([SByteArray7.From([100, 90, 30, 40, 10, 0, 50]), SByteArray7.From([90, 90, 40, 50, 10, 10, 50]), SByteArray7.From([30, 40, 70, 50, 40, 30, 50]), SByteArray7.From([40, 50, 50, 50, 50, 40, 50]), SByteArray7.From([10, 10, 40, 50, 90, 90, 50]), SByteArray7.From([0, 10, 30, 40, 90, 100, 50]), SByteArray7.From([50, 50, 50, 50, 50, 50, 50])]);

    internal static DiplomaticRelation AlignmentToDiplomaticRelation(Alignment A, Alignment B)
    {
        DiplomaticRelation result;
        if (Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(A))
        {
            if (Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(B))
            {
                result = DiplomaticRelation.Peace;
            }
            else
            {
                if (Set<Alignment>.Of(Alignment.PureNeutral, Alignment.Neutral, Alignment.None).Contains(B))
                {
                    result = DiplomaticRelation.Unknown;
                }
                else
                {
                    result = DiplomaticRelation.War;
                }
            }
        }
        else
        {
            if (Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(A))
            {
                if (Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(B))
                {
                    result = DiplomaticRelation.Peace;
                }
                else
                {
                    if (Set<Alignment>.Of(Alignment.PureNeutral, Alignment.Neutral, Alignment.None).Contains(B))
                    {
                        result = DiplomaticRelation.Unknown;
                    }
                    else
                    {
                        result = DiplomaticRelation.War;
                    }
                }
            }
            else
            {
                if (A == Alignment.PureNeutral && B == Alignment.PureNeutral)
                {
                    result = DiplomaticRelation.Peace;
                }
                else
                {
                    result = DiplomaticRelation.Unknown;
                }
            }
        }

        return result;
    }

    internal static DiplomaticRelation StatusToDiplomaticRelation(Status status)
    {
        DiplomaticRelation result;
        switch (status)
        {
            case Status.Polite:
            case Status.Friendly:
                result = DiplomaticRelation.Peace;
                break;
            case Status.Neutral:
                result = DiplomaticRelation.Unknown;
                break;
            case Status.Hostile:
            case Status.Wary:
                result = DiplomaticRelation.War;
                break;
            default:
                result = DiplomaticRelation.Unknown;
                break;
        }

        return result;
    }

    internal static Set<Alignment> PureGoodAlignments { get; } = Set<Alignment>.FromMask(67UL);

    internal static Set<Alignment> GoodAlignments { get; } = Set<Alignment>.FromMask(75UL);

    internal static Set<Alignment> NeutralAlignments { get; } = Set<Alignment>.FromMask(90UL);

    internal static Set<Alignment> EvilAlignments { get; } = Set<Alignment>.FromMask(120UL);

    internal static Set<Alignment> PureEvilAlignments { get; } = Set<Alignment>.FromMask(112UL);

    internal static string NoneText { get; } = "None";

}
