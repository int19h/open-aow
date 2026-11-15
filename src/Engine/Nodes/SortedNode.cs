// Translated from Engine.pas.
using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

public partial class SortedNode : CustomNode
{
    // Engine.pas:1478
    public new const int ClassId = 260;

    public SortedNode(CustomNode owner) : base(owner)
    {
        Items = [];
    }

    private bool _sorted;

    protected internal int StoredCount { get; set; }

    protected internal int Capacity { get; private set; }

    protected internal SortItem[] Items { get; protected set; } = [];

    // Engine.pas:1513
    protected internal void Resize(int newCapacity)
    {
        var items = Items;
        Array.Resize(ref items, newCapacity);
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
        var items = Items;
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
    public override void QuickClear()
    {
        Resize(0);
    }

    // Engine.pas:1488
    public override EngineObject GetChild(int index) => Items[index].Child;

    // Engine.pas:1493
    public override void SetChild(int index, EngineObject child)
    {
        CheckChild(child);
        var items = Items;
        items[index].Child = child;
        Items = items;
    }

    // Engine.pas:1508
    public override int GetCount() => StoredCount;

    // Engine.pas:1520
    public virtual void Grow()
    {
        Resize(Capacity + 2);
    }

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
                Array.Copy(Items, result + 1, Items, result, StoredCount - result);
            }
        }

        return result;
    }

    // Engine.pas:1537
    public override int AddChild(EngineObject child) => AddSortChild(child, 0);

    // Engine.pas:1542
    public virtual int AddSortChild(EngineObject child, int sortValue)
    {
        int result;
        int i;
        CheckChild(child);
        if (StoredCount >= Capacity)
        {
            Grow();
        }

        if (_sorted)
        {
            i = StoredCount;
            while (i > 0 && sortValue > Items[i - 1].SortValue)
            {
                i -= 1;
            }

            if (i != StoredCount)
            {
                Array.Copy(Items, i, Items, i + 1, StoredCount - i);
            }

            var items = Items;
            items[i].Child = child;
            Items = items;
            var items2 = Items;
            items2[i].SortValue = sortValue;
            Items = items2;
            result = i;
        }
        else
        {
            var items3 = Items;
            items3[StoredCount].Child = child;
            Items = items3;
            var items4 = Items;
            items4[StoredCount].SortValue = sortValue;
            Items = items4;
            result = StoredCount;
        }

        StoredCount += 1;
        return result;
    }

    // Engine.pas:1565
    public override void PersistFields(IPersister persister)
    {
        throw new NotSupportedException("Read/Write not supported (TESortNode)");
    }

    // Engine.pas:1596
    public virtual void Sort()
    {
        if (!_sorted && StoredCount > 0)
        {
            SortItems(Items, 0, StoredCount - 1);
        }

        _sorted = true;
    }

    public bool Sorted { get => _sorted; protected internal set => _sorted = value; }

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
                    SortItem sortItem = sortList[number];
                    sortList[number] = sortList[number2];
                    sortList[number2] = sortItem;
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
                SortItems(sortList, L, number2);
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

    // Engine.pas:922

    // Engine.pas:922

}
