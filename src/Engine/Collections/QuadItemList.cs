#nullable disable
// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.DelphiFormat;

namespace Aow.Engine.Collections;

public class QuadItemList : Persistent
{
    private QuadInteger[] _list = [];
    private int _count;
    private int _capacity;

    // Engine.pas:5538
    internal QuadInteger Get(int index) => index < 0 || index >= _count ? Default : _list[index];

    // Engine.pas:5621
    internal void Put(int index, QuadInteger item)
    {
        if (index < 0)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        if (index >= _count)
        {
            SetCount(index + 1);
        }

        _list[index] = item;
    }

    // Engine.pas:5643
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity < _count || newCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", data: newCapacity);
        }

        if (newCapacity != _capacity)
        {
            Array.Resize(ref _list, newSize: newCapacity);
            _capacity = newCapacity;
        }
    }

    // Engine.pas:5654
    internal void SetCount(int newCount)
    {
        if (newCount is < 0 or > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", data: newCount);
        }

        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            FillQuadInteger(_list.AsSpan(_count), count: newCount - _count, value: Default);
        }

        _count = newCount;
    }

    // Engine.pas:5546
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // Engine.pas:5452
    public override void Dispose() => Clear();

    // Engine.pas:5457
    public override void Assign(Persistent source)
    {
        if (source is QuadItemList quadItemList)
        {
            _count = quadItemList._count;
            SetCapacity(quadItemList._count);
            Array.Copy(quadItemList._list, sourceIndex: 0, destinationArray: _list, destinationIndex: 0, length: _count);
            Default = quadItemList.Default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:5471
    public int Add(in QuadInteger item)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        _list[result] = item;
        _count += 1;
        return result;
    }

    // Engine.pas:5479
    public int AddABCD(int A, int B, int C, int D)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        _list[result] = new QuadInteger(First: A, Second: B, Third: C, Fourth: D);
        _count += 1;
        return result;
    }

    // Engine.pas:5490
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:5496
    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        _count -= 1;
        if (index < _count)
        {
            _list.AsSpan(index + 1, length: _count - index).CopyTo(_list.AsSpan(index, length: _count - index));
        }
    }

    // Engine.pas:5516
    public void Exchange(int index1, int index2)
    {
        if (index1 < 0 || index1 >= _count)
        {
            Error("List index out of bounds (%d)", data: index1);
        }

        if (index2 < 0 || index2 >= _count)
        {
            Error("List index out of bounds (%d)", data: index2);
        }

        (_list[index2], _list[index1]) = (_list[index1], _list[index2]);
    }

    // Engine.pas:5527
    public QuadItemList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:5533
    public QuadInteger First() => Get(0);

    // Engine.pas:5556
    public int IndexOf(in QuadInteger item)
    {
        int result = 0;
        while (result < _count && _list[result] != item)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:5564
    public int IndexOfItemA(int A)
    {
        int result = 0;
        while (result < _count && _list[result].First != A)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:5571
    public int IndexOfItemB(int B)
    {
        int result = 0;
        while (result < _count && _list[result].Second != B)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:5578
    public int IndexOfItemC(int C)
    {
        int result = 0;
        while (result < _count && _list[result].Third != C)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:5585
    public int IndexOfItemD(int D)
    {
        int result = 0;
        while (result < _count && _list[result].Fourth != D)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:5592
    public void Insert(int index, in QuadInteger item)
    {
        if (index < 0 || index > _count)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        if (_count == _capacity)
        {
            Grow();
        }

        if (index < _count)
        {
            _list.AsSpan(index, length: _count - index).CopyTo(_list.AsSpan(index + 1, length: _count - index));
        }

        _list[index] = item;
        _count += 1;
    }

    // Engine.pas:5603
    public QuadInteger Last() => Get(_count - 1);

    // Engine.pas:5608
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", data: newIndex);
            }

            QuadInteger item = Get(currentIndex);
            Delete(currentIndex);
            Insert(newIndex, item);
        }
    }

    // Engine.pas:5628
    public int Remove(in QuadInteger item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:5634
    public void Pack()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            if (_list[i] == Default)
            {
                Delete(i);
            }
        }
    }

    // Engine.pas:5691
    public void Sort(Comparison<QuadInteger> compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortQuadItem(_list, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    // Engine.pas:5737
    public void SortAscendingItemA() => Sort(CompareQuadItemAAscending);

    // Engine.pas:5742
    public void SortAscendingItemB() => Sort(CompareQuadItemBAscending);

    // Engine.pas:5747
    public void SortAscendingItemC() => Sort(CompareQuadItemCAscending);

    // Engine.pas:5752
    public void SortAscendingItemD() => Sort(CompareQuadItemDAscending);

    // Engine.pas:5757
    public void SortDescendingItemA() => Sort(CompareQuadItemADescending);

    // Engine.pas:5762
    public void SortDescendingItemB() => Sort(CompareQuadItemBDescending);

    // Engine.pas:5767
    public void SortDescendingItemC() => Sort(CompareQuadItemCDescending);

    // Engine.pas:5772
    public void SortDescendingItemD() => Sort(CompareQuadItemDDescending);

    // Engine.pas:5777
    public bool Compare(QuadItemList list) => list._count == _count && _list.AsSpan(0, length: _count).SequenceEqual(list._list.AsSpan(0, length: _count));

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }
    public QuadInteger Default { get; private set; }

    public QuadInteger this[int index] { get => Get(index); set => Put(index, item: value); }

    public QuadInteger[] List => _list;

    // Engine.pas:5505
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

    private static void QuickSortQuadItem(Span<QuadInteger> sortList, int L, int R, Comparison<QuadInteger> sCompare)
    {
        int number;
        do
        {
            number = L;
            int number2 = R;
            QuadInteger item = sortList[L + R >>> 1];
            while (true)
            {
                if (sCompare(sortList[number], item) < 0)
                {
                    number++;
                    continue;
                }
                while (sCompare(sortList[number2], item) > 0)
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
                QuickSortQuadItem(sortList, L, R: number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    private static int CompareQuadItemAAscending(QuadInteger item1, QuadInteger item2) => item1.First - item2.First;

    private static int CompareQuadItemBAscending(QuadInteger item1, QuadInteger item2) => item1.Second - item2.Second;

    private static int CompareQuadItemCAscending(QuadInteger item1, QuadInteger item2) => item1.Third - item2.Third;

    private static int CompareQuadItemDAscending(QuadInteger item1, QuadInteger item2) => item1.Fourth - item2.Fourth;

    private static int CompareQuadItemADescending(QuadInteger item1, QuadInteger item2) => item2.First - item1.First;

    private static int CompareQuadItemBDescending(QuadInteger item1, QuadInteger item2) => item2.Second - item1.Second;

    private static int CompareQuadItemCDescending(QuadInteger item1, QuadInteger item2) => item2.Third - item1.Third;

    private static int CompareQuadItemDDescending(QuadInteger item1, QuadInteger item2) => item2.Fourth - item1.Fourth;

    internal static void FillQuadInteger(Span<QuadInteger> buffer, int count, in QuadInteger value) => EngineServices.Fill(value: value, buffer: buffer, count: count);

    // Delphi TQuadInteger, four integers. Equality compares all four.
    public readonly record struct QuadInteger(int First, int Second, int Third, int Fourth);
}
