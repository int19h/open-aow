// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Session;

public partial class RaceResourceList : EResourceList
{
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
            result = (RaceResource)_children.List[index];
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
        while (result > 0 && _children.List[result] == null)
        {
            result -= 1;
        }

        return result;
    }

    // intf_P1_09.inc:44

}
