// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

// children
public partial class AIPlayerActionList : ENode
{
    public AIPlayerActionList(ECustomNode owner) : base(owner)
    { }

    // AI.pas:26
    public AIPlayerAction GetActions(int index) => (AIPlayerAction)_children.List[index];

    // AI.pas:31
    public int IndexOf(AIPlayerAction action) => _children.IndexOf(action);

    // Engine.pas:631

}
