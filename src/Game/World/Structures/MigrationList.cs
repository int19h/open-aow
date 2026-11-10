// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

public partial class MigrationList : ENode
{
    public MigrationList(ECustomNode owner) : base(owner)
    { }

    // City.pas:2711
    public MigrationItem GetItems(int Index) => (MigrationItem)FChildren.List[Index];

}
