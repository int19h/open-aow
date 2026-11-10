// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited children, engine, control style
public partial class ItemGFXList : EResourceList
{
    // Army.PAS:5176
    public ItemGFXList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<EControlStyleElement>.Of(EControlStyleElement.SaveLoad, EControlStyleElement.FixedChild, EControlStyleElement.ArrayMode));
    }

    // Army.PAS:5182
    public ItemGFX GetItemGFX(int Index)
    {
        ItemGFX Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (ItemGFX)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:5189
    public int FindItemTypeGFX(ItemType ItemType)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I != FChildren.Count)
        {
            if (FChildren.List[I] != null && ((ItemGFX)FChildren.List[I]).FItemType == ItemType)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

    // intf_P2_04.inc:37

}
