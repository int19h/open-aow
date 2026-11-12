// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

public partial class UnitGFXResourceList : EResourceList, IChildList<UnitGraphicsResource>
{
    // The unit graphics in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a UnitGraphicsResource, so the cast never fails.
    public new UnitGraphicsResource this[int index] => (UnitGraphicsResource)GetChild(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is UnitGraphicsResource;

    int IChildSink<UnitGraphicsResource>.AddChild(UnitGraphicsResource child) => AddChild(child);

    // UnitHS.PAS:133
    public UnitGFXResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, EngineServices.KeepIndex));
    }

    // UnitHS.PAS:138
    public UnitGraphicsResource GetUnitGraphicsResource(int index)
    {
        UnitGraphicsResource result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // UnitHS.PAS:144
    public void CreateUnitGraphicsSelectionList(StringCollection list)
    {
        list.Clear();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (GetChild(I) != null)
            {
                list.AddObject(this[I].Name, IntToObject(I));
            }
        }
    }

    // intf_P2_03.inc:39

}
