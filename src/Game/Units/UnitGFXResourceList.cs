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
        SetControlStyle(Set<EControlStyleElement>.Of(EControlStyleElement.SaveLoad, EControlStyleElement.FixedChild, EngineServices.csKeepIndex));
    }

    // UnitHS.PAS:138
    public UnitGFXResource GetUnitGFXResource(int Index)
    {
        UnitGFXResource Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (UnitGFXResource)FChildren.List[Index];
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
                List.AddObject(((UnitGFXResource)Children[I]).FName, IntToObject(I));
            }
        }
    }

    // intf_P2_03.inc:39

}
