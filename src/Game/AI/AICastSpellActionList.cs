// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public partial class AICastSpellActionList : ENode
{
    public AICastSpellActionList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1535
    // inherited children
    public AICastSpellAction GetCastSpellActions(int index) => (AICastSpellAction)_children.List[index];

    // Spells.PAS:1539
    public void SortOnPriority()
    {
        _children.Sort(CompareCastSpellPriority);
    }

    private static int CompareCastSpellPriority(object item1, object item2)
    {
        return ((AICastSpellAction)item2).Priority - ((AICastSpellAction)item1).Priority;
    }

    // Engine.pas:631

    // Engine.pas:631

}
