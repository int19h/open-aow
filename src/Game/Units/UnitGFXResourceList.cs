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
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, EngineServices.csKeepIndex));
    }

    // UnitHS.PAS:138
    public UnitGraphicsResource GetUnitGFXResource(int Index)
    {
        UnitGraphicsResource Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (UnitGraphicsResource)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // UnitHS.PAS:144
    public void CreateUnitGFXSelectionList(StringCollection List)
    {
        List.Clear();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null)
            {
                List.AddObject(((UnitGraphicsResource)Children[I]).FName, IntToObject(I));
            }
        }
    }

    // intf_P2_03.inc:39

}
