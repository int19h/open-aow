#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic;

public class EnchantmentList(CustomNode owner) : OwningNode(owner), IChildList<Enchantment>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an Enchantment, so the read never fails.
    public Enchantment this[int index] => GetChild<Enchantment>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Enchantment;

    int IChildSink<Enchantment>.AddChild(Enchantment child) => AddChild(child);

    // Spells.PAS:1762
    // inherited children
    public Enchantment GetEnchantments(int index) => this[index];

    // Spells.PAS:1764
    public int IndexOfId(int id)
    {
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            if (this[result].EnchantmentId == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Spells.PAS:1774
    public Enchantment FindId(int id)
    {
        int index = IndexOfId(id);
        return index != -1 ? this[index] : null;
    }
}
