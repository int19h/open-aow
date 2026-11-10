// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class IncomeSourceList : ENode
{
    public IncomeSourceList(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:231
    public IncomeSource GetItems(int Index) => (IncomeSource)FChildren.List[Index];

    // Engine.pas:631

}
