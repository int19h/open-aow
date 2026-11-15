// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class DiplomaticActionList : EReferenceNode, IChildList<DiplomaticAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a DiplomaticAction, so the cast never fails.
    public DiplomaticAction this[int index] => Actions[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is DiplomaticAction;

    int IChildSink<DiplomaticAction>.AddChild(DiplomaticAction child) => AddChild(child);

    public DiplomaticActionList(ECustomNode owner) : base(owner)
    { }

    /// <summary>The actions of this list, in their stored order.</summary>
    public DiplomaticActionCollection Actions => new(this);

    // Diplomacy.pas:1097
    public DiplomaticAction GetDiplomaticActions(int index) => Actions.GetDiplomaticActions(index);

    // Diplomacy.pas:1100
    public DiplomaticAction FindLastAction(byte kind, DiplomaticActionReaction reaction, sbyte source, sbyte target, sbyte other) =>
        Actions.FindLastAction(kind, reaction: reaction, source: source, target: target, other: other);

    // Engine.pas:631

}
