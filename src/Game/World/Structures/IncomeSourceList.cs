// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class IncomeSourceList : ENode, IChildList<IncomeSource>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an IncomeSource, so the cast never fails.
    public IncomeSource this[int index] => (IncomeSource)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is IncomeSource;

    int IChildSink<IncomeSource>.AddChild(IncomeSource child) => AddChild(child);

    public IncomeSourceList(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:231
    public IncomeSource GetItems(int index) => this[index];

    // Engine.pas:631

}
