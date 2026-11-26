#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Units;

// inherited children, engine, control style
public sealed class ItemGFXList : ResourceCollection, IChildList<ItemGraphics>
{
    // The item graphics in slot index, read as the original reads the list, without a range check.
    // AcceptsResource admits only a ItemGraphics, so the cast never fails.
    public new ItemGraphics this[int index] => GetChild<ItemGraphics>(index);

    protected sealed override bool AcceptsResource(EngineResource resource) => resource is ItemGraphics;

    int IChildSink<ItemGraphics>.AddChild(ItemGraphics child) => AddChild(child);

    // Army.PAS:5176
    public ItemGFXList(CustomNode owner) : base(owner) => SetControlStyle(ControlStyles.SaveLoad | ControlStyles.FixedChild | ControlStyles.ArrayMode);

    // Army.PAS:5182
    public ItemGraphics? GetItemGraphics(int index) => index >= 0 && index < Count ? this[index] : null;

    // Army.PAS:5189
    public int FindItemTypeGraphics(ItemType itemType)
        => ((IReadOnlyList<ItemGraphics>)this).FindIndex(itemType, match: static (sought, graphics) => graphics is not null && graphics.ItemType == sought);

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable<ItemGraphics>)this).GetEnumerator();
}
