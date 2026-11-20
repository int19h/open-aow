// Translated from AoWE.pas.

namespace Aow.Game.World.Structures;

// City and place production controls in the order structures register them.
public sealed class ProductionControlList : IDisposable
{
    private readonly List<ProductionControl> _items = [];

    public int Count => _items.Count;

    // Structur.PAS:929
    public ProductionControl this[int index] => _items[index];

    public int Add(ProductionControl Item)
    {
        _items.Add(Item);
        return _items.Count - 1;
    }

    public int IndexOf(ProductionControl Item) => _items.IndexOf(Item);

    public int Remove(ProductionControl Item)
    {
        int index = IndexOf(Item);
        if (index >= 0)
        {
            _items.RemoveAt(index);
        }

        return index;
    }

    public void Dispose() => _items.Clear();
}
