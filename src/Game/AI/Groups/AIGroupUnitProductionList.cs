// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupUnitProductionList : EReferenceNode
{
    public AIGroupUnitProductionList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:2435
    public AIGroupUnitProduction GetUnitProductions(int index) => (AIGroupUnitProduction)_children.EObjects[index];

    // Engine.pas:631

}
