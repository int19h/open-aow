// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic;

public partial class EnchantmentList : ENode
{
    public EnchantmentList(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1762
    // inherited children
    public Enchantment GetEnchantments(int Index) => (Enchantment)FChildren.List[Index];

    // Spells.PAS:1764
    public int IndexOfID(int ID)
    {
        int Result = GetCount();
        while (Result != 0)
        {
            Result -= 1;
            if (((Enchantment)FChildren.List[Result]).FEnchantmentID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Spells.PAS:1774
    public Enchantment FindID(int ID)
    {
        Enchantment Result;
        int Index;
        Index = IndexOfID(ID);
        if (Index != -1)
        {
            Result = (Enchantment)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:631

}
