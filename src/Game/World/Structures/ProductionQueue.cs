// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

// production control
public partial class ProductionQueue : ENode
{
    public ProductionQueue(ECustomNode owner) : base(owner)
    { }

    public ProductionControl ProductionControl { get; set; }

    // Structur.PAS:83
    public ProductionItem GetItem(int index)
    {
        ProductionItem result;
        if (index >= 0 && index < Count)
        {
            result = (ProductionItem)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Structur.PAS:89
    public int IndexOfId(int id)
    {
        int result = _children.Count;
        while (result != 0)
        {
            result -= 1;
            if (id == ((ProductionItem)_children.List[result]).Order.ToFields().Id)
            {
                return result;
            }
        }

        return -1;
    }

    // Structur.PAS:100
    public int IndexOf(ProductionOrder order)
    {
        ProductionItem item;
        int result = _children.Count;
        while (result != 0)
        {
            result -= 1;
            item = (ProductionItem)_children.List[result];
            if (item.Order == order)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
