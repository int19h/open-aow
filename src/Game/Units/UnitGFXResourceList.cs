// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

public partial class UnitGFXResourceList : EResourceList
{
    // UnitHS.PAS:133
    public UnitGFXResourceList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, EngineServices.KeepIndex));
    }

    // UnitHS.PAS:138
    public UnitGraphicsResource GetUnitGraphicsResource(int index)
    {
        UnitGraphicsResource result;
        if (index >= 0 && index < _children.Count)
        {
            result = (UnitGraphicsResource)_children.List[index];
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
            if (_children.List[I] != null)
            {
                list.AddObject(((UnitGraphicsResource)Children[I]).Name, IntToObject(I));
            }
        }
    }

    // intf_P2_03.inc:39

}
