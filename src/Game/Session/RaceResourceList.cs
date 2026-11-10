// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Session;

public partial class RaceResourceList : EResourceList
{
    // PlayerC.PAS:1890
    public RaceResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<EControlStyleElement>.Of(EControlStyleElement.SaveLoad, EControlStyleElement.FixedChild, EControlStyleElement.ArrayMode));
    }

    // PlayerC.PAS:1896
    public RaceResource GetRaceResource(int Index)
    {
        RaceResource Result;
        if (Index < GetCount() && Index >= 0)
        {
            Result = (RaceResource)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // PlayerC.PAS:1903
    public int GetMaxRaceIndex()
    {
        int Result = Count - 1;
        while (Result > 0 && FChildren.List[Result] == null)
        {
            Result -= 1;
        }

        return Result;
    }

    // intf_P1_09.inc:44

}
