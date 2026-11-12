// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class PowerSourceList : ENode
{
    public PowerSourceList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1828
    public PowerSource GetItems(int index) => (PowerSource)_children.List[index];

    // Engine.pas:631

}
