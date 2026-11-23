#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Units;

public sealed class UnitResourceList : ResourceCollection, IChildList<UnitResource>
{
    // The unit resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a UnitResource, so the cast never fails.
    public new UnitResource this[int index] => GetChild<UnitResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is UnitResource;

    int IChildSink<UnitResource>.AddChild(UnitResource child) => AddChild(child);

    // UnitRes.PAS:206
    public UnitResourceList(CustomNode? owner) : base(owner) => SetControlStyle(ControlStyles.SaveLoad | ControlStyles.FixedChild | ControlStyles.ArrayMode);

    // UnitRes.PAS:212
    public UnitResource? GetUnitResource(int index) => index >= 0 && index < Count ? this[index] : null;

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((System.Collections.Generic.IEnumerable<UnitResource>)this).GetEnumerator();
}
