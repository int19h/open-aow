// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

// production control
public partial class ProductionQueue : ENode, IChildList<ProductionItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a ProductionItem, so the cast never fails.
    public ProductionItem this[int index] => (ProductionItem)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is ProductionItem;

    int IChildSink<ProductionItem>.AddChild(ProductionItem child) => AddChild(child);

    public ProductionQueue(ECustomNode owner) : base(owner)
    { }

    public ProductionControl ProductionControl { get; set; }

    // Structur.PAS:83
    public ProductionItem GetItem(int index)
    {
        ProductionItem result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
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
            if (id == this[result].Order.ToFields().Id)
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
            item = this[result];
            if (item.Order == order)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
