// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public partial class AIBudgetList : ENode, IChildList<AIBudget>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIBudget, so the cast never fails.
    public AIBudget this[int index] => (AIBudget)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIBudget;

    int IChildSink<AIBudget>.AddChild(AIBudget child) => AddChild(child);

    public AIBudgetList(ECustomNode owner) : base(owner)
    { }

    // AI.pas:1707
    public AIBudget GetBudgets(int index) => this[index];

    // Engine.pas:631

}
