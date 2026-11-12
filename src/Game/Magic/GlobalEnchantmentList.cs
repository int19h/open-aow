// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic;

public partial class GlobalEnchantmentList : ENode, IChildList<GlobalEnchantment>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a GlobalEnchantment, so the cast never fails.
    public GlobalEnchantment this[int index] => (GlobalEnchantment)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is GlobalEnchantment;

    int IChildSink<GlobalEnchantment>.AddChild(GlobalEnchantment child) => AddChild(child);

    public GlobalEnchantmentList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:2671
    public GlobalEnchantment GetEnchantments(int index) => this[index];

    // Engine.pas:631

}
