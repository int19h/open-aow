// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat.Actions;

namespace Aow.Game.Combat;

public partial class CombatActionList : EReferenceNode, IChildList<CombatAction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatAction, so the cast never fails.
    public CombatAction this[int index] => (CombatAction)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatAction;

    int IChildSink<CombatAction>.AddChild(CombatAction child) => AddChild(child);

    public CombatActionList(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:729
    public CombatAction GetCombatActions(int index) => this[index];

    // Engine.pas:631

}
