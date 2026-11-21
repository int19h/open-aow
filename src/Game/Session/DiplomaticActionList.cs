#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public class DiplomaticActionList(CustomNode owner) : ReferenceNode(owner), IChildList<DiplomaticAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a DiplomaticAction, so the cast never fails.
    public DiplomaticAction this[int index] => Actions[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is DiplomaticAction;

    int IChildSink<DiplomaticAction>.AddChild(DiplomaticAction child) => AddChild(child);

    /// <summary>The actions of this list, in their stored order.</summary>
    public DiplomaticActionCollection Actions => new(this);

    // Diplomacy.pas:1097
    public DiplomaticAction GetDiplomaticActions(int index) => Actions.GetDiplomaticActions(index);

    // Diplomacy.pas:1100
    public DiplomaticAction FindLastAction(byte kind, DiplomaticActionReaction reaction, sbyte source, sbyte target, sbyte other)
        => Actions.FindLastAction(kind, reaction: reaction, source: source, target: target, other: other);
}
