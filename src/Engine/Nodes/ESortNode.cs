// Translated from Engine.pas.
using Aow.Engine.Storage;

namespace Aow.Engine.Nodes;

public partial class ESortNode : ECustomNode
{
    // Engine.pas:1478
    public new const int ClassId = 260;

    public ESortNode(ECustomNode owner) : base(owner)
    {
        Items = [];
    }

    private bool _sorted;
    protected internal int _count;

    protected internal int Capacity { get; private set; }

    protected internal SortItem[] Items = [];

    // Engine.pas:1513
    protected internal void Resize(int newCapacity)
    {
        Array.Resize(ref Items, newCapacity);
        Capacity = newCapacity;
        if (Capacity < _count)
        {
            _count = Capacity;
        }
    }

    // Engine.pas:1498
    internal int GetSortValue(int index) => Items[index].SortValue;

    // Engine.pas:1503
    internal void SetSortValue(int index, int value)
    {
        Items[index].SortValue = value;
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
        Items[index].Child = child;
    }

    // Engine.pas:1508
    public override int GetCount() => _count;

    // Engine.pas:1520
    public virtual void Grow()
    {
        Resize(Capacity + 2);
    }

    // Engine.pas:1525
    public override int RemoveChild(EngineObject child)
    {
        int result = _count - 1;
        while (result >= 0 && child != Items[result].Child)
        {
            result -= 1;
        }

        if (result >= 0)
        {
            _count -= 1;
            if (result < _count)
            {
                Array.Copy(Items, result + 1, Items, result, _count - result);
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
        int I;
        if (_count >= Capacity)
        {
            Grow();
        }

        if (_sorted)
        {
            I = _count;
            while (I > 0 && sortValue > Items[I - 1].SortValue)
            {
                I -= 1;
            }

            if (I != _count)
            {
                Array.Copy(Items, I, Items, I + 1, _count - I);
            }

            Items[I].Child = child;
            Items[I].SortValue = sortValue;
            result = I;
        }
        else
        {
            Items[_count].Child = child;
            Items[_count].SortValue = sortValue;
            result = _count;
        }

        _count += 1;
        return result;
    }

    // Engine.pas:1565
    public override void ReadWrite(EStorageStream stream)
    {
        throw new NotSupportedException("Read/Write not supported (TESortNode)");
    }

    // Engine.pas:1596
    public virtual void Sort()
    {
        if (!_sorted && _count > 0)
        {
            SortItems(Items, 0, _count - 1);
        }

        _sorted = true;
    }

    public sealed class SortValueAccessor(ESortNode target)
    {
        public int this[int index] { get => target.GetSortValue(index); set => target.SetSortValue(index, value); }
    }

    public SortValueAccessor SortValue => new SortValueAccessor(this);
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
