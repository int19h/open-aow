// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

// production control
public partial class ProductionQueue : ENode
{
    public ProductionQueue(ECustomNode owner) : base(owner)
    { }

    public ProductionControl FProductionControl;

    // Structur.PAS:83
    public ProductionItem GetItem(int Index)
    {
        ProductionItem Result;
        if (Index >= 0 && Index < Count)
        {
            Result = (ProductionItem)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:89
    public int IndexOfID(int ID)
    {
        int Result = FChildren.Count;
        while (Result != 0)
        {
            Result -= 1;
            if (ID == ((ProductionItem)FChildren.List[Result]).Order.ToFields().Id)
            {
                return Result;
            }
        }

        return -1;
    }

    // Structur.PAS:100
    public int IndexOf(ProductionOrder Order)
    {
        ProductionItem Item;
        int Result = FChildren.Count;
        while (Result != 0)
        {
            Result -= 1;
            Item = (ProductionItem)FChildren.List[Result];
            if (Item.Order == Order)
            {
                return Result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
