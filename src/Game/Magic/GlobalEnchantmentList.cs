// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic;

public partial class GlobalEnchantmentList : ENode
{
    public GlobalEnchantmentList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:2671
    public GlobalEnchantment GetEnchantments(int Index) => (GlobalEnchantment)FChildren.List[Index];

    // Engine.pas:631

}
