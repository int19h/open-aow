// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

// children
public partial class AIPlayerActionList : ENode
{
    public AIPlayerActionList(ECustomNode owner) : base(owner)
    { }

    // AI.pas:26
    public AIPlayerAction GetActions(int Index) => (AIPlayerAction)FChildren.List[Index];

    // AI.pas:31
    public int IndexOf(AIPlayerAction Action) => FChildren.IndexOf(Action);

    // Engine.pas:631

}
