// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class PowerSourceList : ENode, IChildList<PowerSource>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PowerSource, so the cast never fails.
    public PowerSource this[int index] => (PowerSource)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is PowerSource;

    int IChildSink<PowerSource>.AddChild(PowerSource child) => AddChild(child);

    public PowerSourceList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1828
    public PowerSource GetItems(int index) => this[index];

    // Engine.pas:631

}
