// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic;

public partial class EnchantmentList : ENode
{
    public EnchantmentList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1762
    // inherited children
    public Enchantment GetEnchantments(int index) => (Enchantment)_children.List[index];

    // Spells.PAS:1764
    public int IndexOfId(int id)
    {
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            if (((Enchantment)_children.List[result]).EnchantmentId == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Spells.PAS:1774
    public Enchantment FindId(int id)
    {
        Enchantment result;
        int index;
        index = IndexOfId(id);
        if (index != -1)
        {
            result = (Enchantment)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:631

}
