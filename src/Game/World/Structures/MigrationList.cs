// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

public partial class MigrationList : ENode, IChildList<MigrationItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a MigrationItem, so the cast never fails.
    public MigrationItem this[int index] => (MigrationItem)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is MigrationItem;

    int IChildSink<MigrationItem>.AddChild(MigrationItem child) => AddChild(child);

    public MigrationList(ECustomNode owner) : base(owner)
    { }

    // City.pas:2711
    public MigrationItem GetItems(int index) => this[index];

}
