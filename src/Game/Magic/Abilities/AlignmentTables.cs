
using Aow.Engine.Buffers;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class AlignmentTables
{
    internal static Status dAlignmentToStatus(Alignment A, Alignment B) => Race.StatusValueToStatus(dAlignmentToStatusValue[(int)A][(int)B]);

    internal static sbyte AlignmentToIR(Alignment Alignment)
    {
        sbyte Result;
        switch (Alignment)
        {
            case Alignment.PureGood:
            case Alignment.Good:
                Result = -2;
                break;
            case Alignment.Evil:
            case Alignment.PureEvil:
                Result = -4;
                break;
            case Alignment.PureNeutral:
            case Alignment.Neutral:
                Result = -3;
                break;
            default:
                Result = -1;
                break;
        }

        return Result;
    }

    internal static SByteArray7By7 dAlignmentToStatusValue = SByteArray7By7.From([SByteArray7.From([100, 90, 30, 40, 10, 0, 50]), SByteArray7.From([90, 90, 40, 50, 10, 10, 50]), SByteArray7.From([30, 40, 70, 50, 40, 30, 50]), SByteArray7.From([40, 50, 50, 50, 50, 40, 50]), SByteArray7.From([10, 10, 40, 50, 90, 90, 50]), SByteArray7.From([0, 10, 30, 40, 90, 100, 50]), SByteArray7.From([50, 50, 50, 50, 50, 50, 50])]);

    internal static SByteArray7By7 P2AlignmentToStatusValue = SByteArray7By7.From([SByteArray7.From([100, 90, 30, 40, 10, 0, 50]), SByteArray7.From([90, 90, 40, 50, 10, 10, 50]), SByteArray7.From([30, 40, 70, 50, 40, 30, 50]), SByteArray7.From([40, 50, 50, 50, 50, 40, 50]), SByteArray7.From([10, 10, 40, 50, 90, 90, 50]), SByteArray7.From([0, 10, 30, 40, 90, 100, 50]), SByteArray7.From([50, 50, 50, 50, 50, 50, 50])]);

    internal static DiplomaticRelation dAlignmentToDiplomaticRelation(Alignment A, Alignment B)
    {
        DiplomaticRelation Result;
        if (Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(A))
        {
            if (Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(B))
            {
                Result = DiplomaticRelation.Peace;
            }
            else
            {
                if (Set<Alignment>.Of(Alignment.PureNeutral, Alignment.Neutral, Alignment.None).Contains(B))
                {
                    Result = DiplomaticRelation.Unknown;
                }
                else
                {
                    Result = DiplomaticRelation.War;
                }
            }
        }
        else
        {
            if (Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(A))
            {
                if (Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(B))
                {
                    Result = DiplomaticRelation.Peace;
                }
                else
                {
                    if (Set<Alignment>.Of(Alignment.PureNeutral, Alignment.Neutral, Alignment.None).Contains(B))
                    {
                        Result = DiplomaticRelation.Unknown;
                    }
                    else
                    {
                        Result = DiplomaticRelation.War;
                    }
                }
            }
            else
            {
                if (A == Alignment.PureNeutral && B == Alignment.PureNeutral)
                {
                    Result = DiplomaticRelation.Peace;
                }
                else
                {
                    Result = DiplomaticRelation.Unknown;
                }
            }
        }

        return Result;
    }

    internal static DiplomaticRelation StatusToDiplomaticRelation(Status Status)
    {
        DiplomaticRelation Result;
        switch (Status)
        {
            case Status.Polite:
            case Status.Friendly:
                Result = DiplomaticRelation.Peace;
                break;
            case Status.Neutral:
                Result = DiplomaticRelation.Unknown;
                break;
            case Status.Hostile:
            case Status.Wary:
                Result = DiplomaticRelation.War;
                break;
            default:
                Result = DiplomaticRelation.Unknown;
                break;
        }

        return Result;
    }

    internal static Set<Alignment> alsPureGood = Set<Alignment>.FromMask(67UL);

    internal static Set<Alignment> alsGood = Set<Alignment>.FromMask(75UL);

    internal static Set<Alignment> alsNeutral = Set<Alignment>.FromMask(90UL);

    internal static Set<Alignment> alsEvil = Set<Alignment>.FromMask(120UL);

    internal static Set<Alignment> alsPureEvil = Set<Alignment>.FromMask(112UL);

    internal static string NoneRStr = "None";

}
