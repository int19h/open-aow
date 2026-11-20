// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public sealed class PowerSourceList(CustomNode owner) : OwningNode(owner), IChildList<PowerSource>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PowerSource, so the cast never fails.
    public PowerSource this[int index] => GetChild<PowerSource>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PowerSource;

    int IChildSink<PowerSource>.AddChild(PowerSource child) => AddChild(child);

    // Spells.PAS:1828
    public PowerSource GetItems(int index) => this[index];
}
