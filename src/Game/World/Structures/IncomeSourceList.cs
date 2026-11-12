// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class IncomeSourceList : ENode
{
    public IncomeSourceList(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:231
    public IncomeSource GetItems(int index) => (IncomeSource)_children.List[index];

    // Engine.pas:631

}
