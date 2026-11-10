using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class AlignmentTables
{
    internal static Status dAlignmentToStatus(Alignment A, Alignment B) => Aow.Game.Session.Race.StatusValueToStatus(dAlignmentToStatusValue[(int)A][(int)B]);

    internal static sbyte AlignmentToIR(Alignment Alignment)
    {
        sbyte Result;
        switch (Alignment)
        {
            case Engine.Alignment.PureGood:
            case Engine.Alignment.Good:
                Result = -2;
                break;
            case Engine.Alignment.Evil:
            case Engine.Alignment.PureEvil:
                Result = -4;
                break;
            case Engine.Alignment.PureNeutral:
            case Engine.Alignment.Neutral:
                Result = -3;
                break;
            default:
                Result = -1;
                break;
        }

        return Result;
    }

    internal static FixedArray_Aow_Units_FixedArray_sbyte_7_7 dAlignmentToStatusValue = FixedArray_Aow_Units_FixedArray_sbyte_7_7.From([FixedArray_sbyte_7.From([100, 90, 30, 40, 10, 0, 50]), FixedArray_sbyte_7.From([90, 90, 40, 50, 10, 10, 50]), FixedArray_sbyte_7.From([30, 40, 70, 50, 40, 30, 50]), FixedArray_sbyte_7.From([40, 50, 50, 50, 50, 40, 50]), FixedArray_sbyte_7.From([10, 10, 40, 50, 90, 90, 50]), FixedArray_sbyte_7.From([0, 10, 30, 40, 90, 100, 50]), FixedArray_sbyte_7.From([50, 50, 50, 50, 50, 50, 50])]);

    internal static FixedArray_Aow_Units_FixedArray_sbyte_7_7 P2AlignmentToStatusValue = FixedArray_Aow_Units_FixedArray_sbyte_7_7.From([FixedArray_sbyte_7.From([100, 90, 30, 40, 10, 0, 50]), FixedArray_sbyte_7.From([90, 90, 40, 50, 10, 10, 50]), FixedArray_sbyte_7.From([30, 40, 70, 50, 40, 30, 50]), FixedArray_sbyte_7.From([40, 50, 50, 50, 50, 40, 50]), FixedArray_sbyte_7.From([10, 10, 40, 50, 90, 90, 50]), FixedArray_sbyte_7.From([0, 10, 30, 40, 90, 100, 50]), FixedArray_sbyte_7.From([50, 50, 50, 50, 50, 50, 50])]);

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
            case Engine.Status.Polite:
            case Engine.Status.Friendly:
                Result = DiplomaticRelation.Peace;
                break;
            case Engine.Status.Neutral:
                Result = DiplomaticRelation.Unknown;
                break;
            case Engine.Status.Hostile:
            case Engine.Status.Wary:
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
