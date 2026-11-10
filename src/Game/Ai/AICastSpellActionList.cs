// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai;

public partial class AICastSpellActionList : ENode
{
    public AICastSpellActionList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1535
    // inherited children
    public AICastSpellAction GetCastSpellActions(int Index) => (AICastSpellAction)FChildren.List[Index];

    // Spells.PAS:1539
    public void SortOnPriority()
    {
        FChildren.Sort(P4CompareCastSpellPriority);
    }

    private static int P4CompareCastSpellPriority(object Item1, object Item2)
    {
        return ((AICastSpellAction)Item2).FPriority - ((AICastSpellAction)Item1).FPriority;
    }

    // Engine.pas:631

    // Engine.pas:631

}
