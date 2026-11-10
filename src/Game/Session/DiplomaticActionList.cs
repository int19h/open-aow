// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class DiplomaticActionList : EReferenceNode
{
    public DiplomaticActionList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:1097
    public DiplomaticAction GetDiplomaticActions(int Index) => (DiplomaticAction)FChildren.List[Index];

    // Diplomacy.pas:1100
    public DiplomaticAction FindLastAction(byte Kind, DiplomaticActionReaction Reaction, sbyte Source, sbyte Target, sbyte Other)
    {
        int I;
        DiplomaticAction Candidate;
        DiplomaticAction Result = null;
        I = Count;
        while (Result == null && I != 0)
        {
            I -= 1;
            Candidate = (DiplomaticAction)FChildren.List[I];
            if (Candidate.Proposal.ToFields().Kind == Kind && Candidate.FReaction == Reaction && (Source == Candidate.FSource || Source == -1) && (Candidate.FTarget == Target || Target == -1) && (Candidate.FOtherPlayer == Other || Other == -1))
            {
                Result = Candidate;
            }
        }

        return Result;
    }

    // Engine.pas:631

}
