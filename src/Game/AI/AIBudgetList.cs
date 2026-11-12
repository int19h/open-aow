// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public partial class AIBudgetList : ENode
{
    public AIBudgetList(ECustomNode owner) : base(owner)
    { }

    // AI.pas:1707
    public AIBudget GetBudgets(int index) => (AIBudget)_children.List[index];

    // Engine.pas:631

}
