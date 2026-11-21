#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public sealed class AICastSpellActionList(CustomNode? owner) : OwningNode(owner), IChildList<AICastSpellAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AICastSpellAction.
    public AICastSpellAction this[int index] => GetChild<AICastSpellAction>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AICastSpellAction;

    int IChildSink<AICastSpellAction>.AddChild(AICastSpellAction child) => AddChild(child);

    // Spells.PAS:1535
    // inherited children
    public AICastSpellAction GetCastSpellActions(int index) => this[index];

    // Spells.PAS:1539
    public void SortOnPriority() => SortChildren<AICastSpellAction>(CompareCastSpellPriority);

    private static int CompareCastSpellPriority(AICastSpellAction item1, AICastSpellAction item2) => item2.Priority - item1.Priority;
}
