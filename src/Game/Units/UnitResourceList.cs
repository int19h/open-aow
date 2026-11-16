#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class UnitResourceList : ResourceCollection, IChildList<UnitResource>
{
    // The unit resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a UnitResource, so the cast never fails.
    public new UnitResource this[int index] => GetChild<UnitResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is UnitResource;

    int IChildSink<UnitResource>.AddChild(UnitResource child) => AddChild(child);

    // UnitRes.PAS:206
    public UnitResourceList(CustomNode owner) : base(owner) => SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));

    // UnitRes.PAS:212
    public UnitResource GetUnitResource(int index)
    {
        UnitResource result = index >= 0 && index < Count ? this[index] : null;
        return result;
    }

    // intf_P2_05.inc:260

}
