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
    internal static TStatus dAlignmentToStatus(TAlignment A, TAlignment B) => Aow.Game.Session.TRace.StatusValueToStatus(dAlignmentToStatusValue[(int)A][(int)B]);

    internal static sbyte AlignmentToIR(TAlignment Alignment)
    {
        sbyte Result;
        switch (Alignment)
        {
            case TAlignment.alPureGood:
            case TAlignment.alGood:
                Result = -2;
                break;
            case TAlignment.alEvil:
            case TAlignment.alPureEvil:
                Result = -4;
                break;
            case TAlignment.alPureNeutral:
            case TAlignment.alNeutral:
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

    internal static TDiplomaticRelation dAlignmentToDiplomaticRelation(TAlignment A, TAlignment B)
    {
        TDiplomaticRelation Result;
        if (TSet<TAlignment>.Of(TAlignment.alPureGood, TAlignment.alGood).Contains(A))
        {
            if (TSet<TAlignment>.Of(TAlignment.alPureGood, TAlignment.alGood).Contains(B))
            {
                Result = TDiplomaticRelation.drPeace;
            }
            else
            {
                if (TSet<TAlignment>.Of(TAlignment.alPureNeutral, TAlignment.alNeutral, TAlignment.alNone).Contains(B))
                {
                    Result = TDiplomaticRelation.drUnknown;
                }
                else
                {
                    Result = TDiplomaticRelation.drWar;
                }
            }
        }
        else
        {
            if (TSet<TAlignment>.Of(TAlignment.alEvil, TAlignment.alPureEvil).Contains(A))
            {
                if (TSet<TAlignment>.Of(TAlignment.alEvil, TAlignment.alPureEvil).Contains(B))
                {
                    Result = TDiplomaticRelation.drPeace;
                }
                else
                {
                    if (TSet<TAlignment>.Of(TAlignment.alPureNeutral, TAlignment.alNeutral, TAlignment.alNone).Contains(B))
                    {
                        Result = TDiplomaticRelation.drUnknown;
                    }
                    else
                    {
                        Result = TDiplomaticRelation.drWar;
                    }
                }
            }
            else
            {
                if (A == TAlignment.alPureNeutral && B == TAlignment.alPureNeutral)
                {
                    Result = TDiplomaticRelation.drPeace;
                }
                else
                {
                    Result = TDiplomaticRelation.drUnknown;
                }
            }
        }

        return Result;
    }

    internal static TDiplomaticRelation StatusToDiplomaticRelation(TStatus Status)
    {
        TDiplomaticRelation Result;
        switch (Status)
        {
            case TStatus.sPolite:
            case TStatus.sFriendly:
                Result = TDiplomaticRelation.drPeace;
                break;
            case TStatus.sNeutral:
                Result = TDiplomaticRelation.drUnknown;
                break;
            case TStatus.sHostile:
            case TStatus.sWary:
                Result = TDiplomaticRelation.drWar;
                break;
            default:
                Result = TDiplomaticRelation.drUnknown;
                break;
        }

        return Result;
    }

    internal static TSet<TAlignment> alsPureGood = TSet<TAlignment>.FromMask(67UL);

    internal static TSet<TAlignment> alsGood = TSet<TAlignment>.FromMask(75UL);

    internal static TSet<TAlignment> alsNeutral = TSet<TAlignment>.FromMask(90UL);

    internal static TSet<TAlignment> alsEvil = TSet<TAlignment>.FromMask(120UL);

    internal static TSet<TAlignment> alsPureEvil = TSet<TAlignment>.FromMask(112UL);

    internal static string NoneRStr = "None";

}
