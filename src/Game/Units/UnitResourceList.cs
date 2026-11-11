// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Units;

public partial class UnitResourceList : EResourceList
{
    // UnitRes.PAS:206
    public UnitResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // UnitRes.PAS:212
    public UnitResource GetUnitResource(int Index)
    {
        UnitResource Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (UnitResource)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // intf_P2_05.inc:260

}
