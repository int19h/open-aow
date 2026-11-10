// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai;

public partial class AIBudgetList : ENode
{
    public AIBudgetList(ECustomNode owner) : base(owner)
    { }

    // AI.pas:1707
    public AIBudget GetBudgets(int Index) => (AIBudget)FChildren.List[Index];

    // Engine.pas:631

}
