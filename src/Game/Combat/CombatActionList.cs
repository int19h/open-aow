// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat.Actions;

namespace Aow.Game.Combat;

public class CombatActionList(CustomNode owner) : ReferenceNode(owner), IChildList<CombatAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatAction, so the cast never fails.
    public CombatAction this[int index] => Actions[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatAction;

    int IChildSink<CombatAction>.AddChild(CombatAction child) => AddChild(child);

    /// <summary>The actions of this list, in their stored order.</summary>
    public CombatActionCollection Actions => new(this);

    // Combat2.pas:729
    public CombatAction GetCombatActions(int index) => Actions.GetCombatActions(index);
}
