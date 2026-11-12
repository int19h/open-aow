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
    public UnitResource GetUnitResource(int index)
    {
        UnitResource result;
        if (index >= 0 && index < _children.Count)
        {
            result = (UnitResource)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // intf_P2_05.inc:260

}
