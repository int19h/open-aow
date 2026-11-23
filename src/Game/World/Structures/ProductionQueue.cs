#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// production control
public class ProductionQueue(CustomNode? owner) : OwningNode(owner), IChildList<ProductionItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a ProductionItem, so the cast never fails.
    public ProductionItem this[int index] => GetChild<ProductionItem>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is ProductionItem;

    int IChildSink<ProductionItem>.AddChild(ProductionItem child) => AddChild(child);

    public ProductionControl ProductionControl { get; init; }

    // Structur.PAS:83
    public ProductionItem? GetItem(int index) => index >= 0 && index < Count ? this[index] : null;

    // Structur.PAS:89
    public int IndexOfId(int id) => this.FindLastIndex(item => id == item.Order.ToFields().Kind);

    // Structur.PAS:100
    public int IndexOf(ProductionOrder order) => this.FindLastIndex(item => item.Order == order);
}
