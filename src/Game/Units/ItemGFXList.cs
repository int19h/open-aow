#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited children, engine, control style
public partial class ItemGFXList : ResourceCollection, IChildList<ItemGraphics>
{
    // The item graphics in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a ItemGraphics, so the cast never fails.
    public new ItemGraphics this[int index] => GetChild<ItemGraphics>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is ItemGraphics;

    int IChildSink<ItemGraphics>.AddChild(ItemGraphics child) => AddChild(child);

    // Army.PAS:5176
    public ItemGFXList(CustomNode owner) : base(owner) => SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));

    // Army.PAS:5182
    public ItemGraphics GetItemGraphics(int index) => index >= 0 && index < Count ? this[index] : null;

    // Army.PAS:5189
    public int FindItemTypeGraphics(ItemType itemType)
    {
        int result = -1;
        int i = 0;
        while (result == -1 && i != Count)
        {
            if (GetChild(i) is not null && this[i].ItemType == itemType)
            {
                result = i;
            }

            i += 1;
        }

        return result;
    }

    // intf_P2_04.inc:37

}
