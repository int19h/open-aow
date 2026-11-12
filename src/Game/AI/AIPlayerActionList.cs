// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

// children
public partial class AIPlayerActionList : ENode, IChildList<AIPlayerAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIPlayerAction, so the cast never fails.
    public AIPlayerAction this[int index] => (AIPlayerAction)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIPlayerAction;

    int IChildSink<AIPlayerAction>.AddChild(AIPlayerAction child) => AddChild(child);

    public AIPlayerActionList(ECustomNode owner) : base(owner)
    { }

    // AI.pas:26
    public AIPlayerAction GetActions(int index) => this[index];

    // AI.pas:31
    public int IndexOf(AIPlayerAction action) => IndexOfChild(action);

    // Engine.pas:631

}
