// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class DiplomaticActionList : EReferenceNode, IChildList<DiplomaticAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a DiplomaticAction, so the cast never fails.
    public DiplomaticAction this[int index] => (DiplomaticAction)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is DiplomaticAction;

    int IChildSink<DiplomaticAction>.AddChild(DiplomaticAction child) => AddChild(child);

    public DiplomaticActionList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:1097
    public DiplomaticAction GetDiplomaticActions(int index) => this[index];

    // Diplomacy.pas:1100
    public DiplomaticAction FindLastAction(byte kind, DiplomaticActionReaction reaction, sbyte source, sbyte target, sbyte other)
    {
        int I;
        DiplomaticAction candidate;
        DiplomaticAction result = null;
        I = Count;
        while (result == null && I != 0)
        {
            I -= 1;
            candidate = this[I];
            if (candidate.Proposal.ToFields().Kind == kind && candidate.Reaction == reaction && (source == candidate.Source || source == -1) && (candidate.Target == target || target == -1) && (candidate.OtherPlayer == other || other == -1))
            {
                result = candidate;
            }
        }

        return result;
    }

    // Engine.pas:631

}
