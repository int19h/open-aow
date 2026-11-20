// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public class IncomeSourceList(CustomNode owner) : OwningNode(owner), IChildList<IncomeSource>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an IncomeSource, so the cast never fails.
    public IncomeSource this[int index] => GetChild<IncomeSource>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is IncomeSource;

    int IChildSink<IncomeSource>.AddChild(IncomeSource child) => AddChild(child);

    // Player.PAS:231
    public IncomeSource GetItems(int index) => this[index];
}
