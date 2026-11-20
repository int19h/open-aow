// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic;

public class GlobalEnchantmentList(CustomNode owner) : OwningNode(owner), IChildList<GlobalEnchantment>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a GlobalEnchantment, so the read never fails.
    public GlobalEnchantment this[int index] => GetChild<GlobalEnchantment>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is GlobalEnchantment;

    int IChildSink<GlobalEnchantment>.AddChild(GlobalEnchantment child) => AddChild(child);

    // Spells.PAS:2671
    public GlobalEnchantment GetEnchantments(int index) => this[index];
}
