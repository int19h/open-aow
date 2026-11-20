#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Units;

public sealed class UnitGFXResourceList : ResourceCollection, IChildList<UnitGraphicsResource>
{
    // The unit graphics in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a UnitGraphicsResource, so the cast never fails.
    public new UnitGraphicsResource this[int index] => GetChild<UnitGraphicsResource>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is UnitGraphicsResource;

    int IChildSink<UnitGraphicsResource>.AddChild(UnitGraphicsResource child) => AddChild(child);

    // UnitHS.PAS:133
    public UnitGFXResourceList(CustomNode owner) : base(owner) => SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, EngineServices.KeepIndex));

    // UnitHS.PAS:138
    public UnitGraphicsResource GetUnitGraphicsResource(int index) => index >= 0 && index < Count ? this[index] : null;

    // UnitHS.PAS:144
    public void CreateUnitGraphicsSelectionList(DelphiStringCollection list)
    {
        list.Clear();
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null)
            {
                _ = list.AddObject(this[i].Name, item: IntToObject(i));
            }
        }
    }
}
