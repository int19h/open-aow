// Translated from AoWE.pas.

namespace Aow.Game.World.Structures;

// City and place production controls in the order structures register them.
public partial class ProductionControlList : IDisposable
{
    private readonly List<ProductionControl> FItems = [];

    public int Count => FItems.Count;

    // Structur.PAS:929
    public ProductionControl GetPC(int Index) => FItems[Index];

    public int Add(ProductionControl Item)
    {
        FItems.Add(Item);
        return FItems.Count - 1;
    }

    public int IndexOf(ProductionControl Item) => FItems.IndexOf(Item);

    public int Remove(ProductionControl Item)
    {
        int Index = IndexOf(Item);
        if (Index >= 0)
        {
            FItems.RemoveAt(Index);
        }

        return Index;
    }

    public void Dispose() => FItems.Clear();
}
