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
    public ItemGraphics GetItemGraphics(int index)
    {
        ItemGraphics result;
        if (index >= 0 && index < _children.Count)
        {
            result = (ItemGraphics)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:5189
    public int FindItemTypeGraphics(ItemType itemType)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I != _children.Count)
        {
            if (_children.List[I] != null && ((ItemGraphics)_children.List[I]).ItemType == itemType)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

    // intf_P2_04.inc:37

}
