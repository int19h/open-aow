// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// children
public partial class AIGroupUnitProductionList : EReferenceNode
{
    public AIGroupUnitProductionList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:2435
    public AIGroupUnitProduction GetUnitProductions(int Index) => (AIGroupUnitProduction)FChildren.EObjects[Index];

    // Engine.pas:631

}
