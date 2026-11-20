// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public sealed class AIBudgetList(CustomNode owner) : OwningNode(owner), IChildList<AIBudget>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIBudget.
    public AIBudget this[int index] => GetChild<AIBudget>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIBudget;

    int IChildSink<AIBudget>.AddChild(AIBudget child) => AddChild(child);

    // AI.pas:1707
    public AIBudget GetBudgets(int index) => this[index];
}
