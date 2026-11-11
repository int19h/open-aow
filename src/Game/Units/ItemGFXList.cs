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
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
    }

    // Army.PAS:5182
    public ItemGraphics GetItemGFX(int Index)
    {
        ItemGraphics Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (ItemGraphics)FChildren.List[Index];
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
            if (FChildren.List[I] != null && ((ItemGraphics)FChildren.List[I]).FItemType == ItemType)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

    // intf_P2_04.inc:37

}
