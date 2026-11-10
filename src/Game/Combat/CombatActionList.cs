// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat.Actions;

namespace Aow.Game.Combat;

public partial class CombatActionList : EReferenceNode
{
    public CombatActionList(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:729
    public CombatAction GetCombatActions(int Index) => (CombatAction)FChildren.List[Index];

    // Engine.pas:631

}
