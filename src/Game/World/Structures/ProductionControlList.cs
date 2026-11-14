// Translated from AoWE.pas.

namespace Aow.Game.World.Structures;

// City and place production controls in the order structures register them.
public partial class ProductionControlList : IDisposable
{
    private readonly List<ProductionControl> FItems = [];

    public int Count => FItems.Count;

    // Structur.PAS:929
    public ProductionControl GetPC(int index) => FItems[index];

    public int Add(ProductionControl Item)
    {
        FItems.Add(Item);
        return FItems.Count - 1;
    }

    public int IndexOf(ProductionControl Item) => FItems.IndexOf(Item);

    public int Remove(ProductionControl Item)
    {
        int index = IndexOf(Item);
        if (index >= 0)
        {
            FItems.RemoveAt(index);
        }

        return index;
    }

    public void Dispose() => FItems.Clear();
}
