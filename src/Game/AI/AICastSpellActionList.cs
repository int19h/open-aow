// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public partial class AICastSpellActionList : ENode, IChildList<AICastSpellAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AICastSpellAction, so the cast never fails.
    public AICastSpellAction this[int index] => (AICastSpellAction)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AICastSpellAction;

    int IChildSink<AICastSpellAction>.AddChild(AICastSpellAction child) => AddChild(child);

    public AICastSpellActionList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1535
    // inherited children
    public AICastSpellAction GetCastSpellActions(int index) => this[index];

    // Spells.PAS:1539
    public void SortOnPriority()
    {
        SortChildren(CompareCastSpellPriority);
    }

    private static int CompareCastSpellPriority(object item1, object item2)
    {
        return ((AICastSpellAction)item2).Priority - ((AICastSpellAction)item1).Priority;
    }

    // Engine.pas:631

    // Engine.pas:631

}
