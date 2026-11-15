// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupUnitProductionList : EReferenceNode, IChildList<AIGroupUnitProduction>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIGroupUnitProduction, so the cast never fails.
    public AIGroupUnitProduction this[int index] => Productions[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIGroupUnitProduction;

    int IChildSink<AIGroupUnitProduction>.AddChild(AIGroupUnitProduction child) => AddChild(child);

    public AIGroupUnitProductionList(ECustomNode owner) : base(owner)
    { }

    /// <summary>The productions of this list, in their stored order.</summary>
    public AIGroupUnitProductionCollection Productions => new(this);

    // AIGroup.pas:2435
    public AIGroupUnitProduction GetUnitProductions(int index) => Productions.GetUnitProductions(index);

    // Engine.pas:631

}
