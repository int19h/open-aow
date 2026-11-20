// Translated from Engine.pas.
using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

public class SortedNode(CustomNode owner) : CustomNode(owner)
{

    // Engine.pas:1478
    public new const int ClassId = 260;

    protected internal int StoredCount { get; protected set; }

    protected internal int Capacity { get; private set; }

    protected internal SortItem[] Items { get; protected set; } = [];

    // Engine.pas:1513
    protected internal void Resize(int newCapacity)
    {
        SortItem[] items = Items;
        Array.Resize(ref items, newSize: newCapacity);
        Items = items;
        Capacity = newCapacity;
        if (Capacity < StoredCount)
        {
            StoredCount = Capacity;
        }
    }

    // Engine.pas:1498
    internal int GetSortValue(int index) => Items[index].SortValue;

    // Engine.pas:1503
    internal void SetSortValue(int index, int value)
    {
        SortItem[] items = Items;
        items[index].SortValue = value;
        Items = items;
    }

    // Engine.pas:1472
    public override void Dispose()
    {
        base.Dispose();
        Resize(0);
    }

    // Engine.pas:1483
    public override void QuickClear() => Resize(0);

    // Engine.pas:1488
    public override EngineObject GetChild(int index) => Items[index].Child;

    // Engine.pas:1493
    public override void SetChild(int index, EngineObject child)
    {
        CheckChild(child);
        SortItem[] items = Items;
        items[index].Child = child;
        Items = items;
    }

    // Engine.pas:1508
    public override int GetCount() => StoredCount;

    // Engine.pas:1520
    public virtual void Grow() => Resize(Capacity + 2);

    // Engine.pas:1525
    public override int RemoveChild(EngineObject child)
    {
        int result = StoredCount - 1;
        while (result >= 0 && child != Items[result].Child)
        {
            result -= 1;
        }

        if (result >= 0)
        {
            StoredCount -= 1;
            if (result < StoredCount)
            {
                Array.Copy(Items, sourceIndex: result + 1, destinationArray: Items, destinationIndex: result, length: StoredCount - result);
            }
        }

        return result;
    }

    // Engine.pas:1537
    public override int AddChild(EngineObject child) => AddSortChild(child, sortValue: 0);

    // Engine.pas:1542
    public virtual int AddSortChild(EngineObject child, int sortValue)
    {
        CheckChild(child);
        if (StoredCount >= Capacity)
        {
            Grow();
        }

        int result;
        if (Sorted)
        {
            int i = StoredCount;
            while (i > 0 && sortValue > Items[i - 1].SortValue)
            {
                i -= 1;
            }

            if (i != StoredCount)
            {
                Array.Copy(Items, sourceIndex: i, destinationArray: Items, destinationIndex: i + 1, length: StoredCount - i);
            }

            SortItem[] items = Items;
            items[i].Child = child;
            Items = items;
            SortItem[] items2 = Items;
            items2[i].SortValue = sortValue;
            Items = items2;
            result = i;
        }
        else
        {
            SortItem[] items3 = Items;
            items3[StoredCount].Child = child;
            Items = items3;
            SortItem[] items4 = Items;
            items4[StoredCount].SortValue = sortValue;
            Items = items4;
            result = StoredCount;
        }

        StoredCount += 1;
        return result;
    }

    // Engine.pas:1565
    public override void PersistFields(IPersister persister) => throw new NotSupportedException("Read/Write not supported (TESortNode)");

    // Engine.pas:1596
    public virtual void Sort()
    {
        if (!Sorted && StoredCount > 0)
        {
            SortItems(Items, L: 0, R: StoredCount - 1);
        }

        Sorted = true;
    }

    public bool Sorted { get; protected internal set; }

    private static void SortItems(Span<SortItem> sortList, int L, int R)
    {
        int number;
        do
        {
            number = L;
            int number2 = R;
            int sortValue = sortList[L + R >>> 1].SortValue;
            while (true)
            {
                if (sortValue > sortList[number].SortValue)
                {
                    number++;
                    continue;
                }
                while (sortValue < sortList[number2].SortValue)
                {
                    number2--;
                }
                if (number <= number2)
                {
                    (sortList[number2], sortList[number]) = (sortList[number], sortList[number2]);
                    number++;
                    number2--;
                }
                if (number > number2)
                {
                    break;
                }
            }
            if (L < number2)
            {
                SortItems(sortList, L, R: number2);
            }
            L = number;
        }
        while (number < R);
    }

    public struct SortItem
    {
        public int SortValue { get; set; }
        public EngineObject Child { get; set; }
    }
}
