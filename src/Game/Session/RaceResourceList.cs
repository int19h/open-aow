// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Session;

public partial class RaceResourceList : EResourceList, IChildList<RaceResource>
{
    // The race resource in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a RaceResource, so the cast never fails.
    public new RaceResource this[int index] => (RaceResource)GetChild(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is RaceResource;

    int IChildSink<RaceResource>.AddChild(RaceResource child) => AddChild(child);

    // PlayerC.PAS:1890
    public RaceResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // PlayerC.PAS:1896
    public RaceResource GetRaceResource(int index)
    {
        RaceResource result;
        if (index < GetCount() && index >= 0)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // PlayerC.PAS:1903
    public int GetMaxRaceIndex()
    {
        int result = Count - 1;
        while (result > 0 && GetChild(result) == null)
        {
            result -= 1;
        }

        return result;
    }

    // intf_P1_09.inc:44

}
