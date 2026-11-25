#nullable disable warnings
// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Engine.Collections;

public class DoubleItemList : Persistent, IReadOnlyList<DoubleItemList.DoubleItem>
{
    private DoubleItem[] _list = [];
    private int _count;
    private int _capacity;

    // Engine.pas:5140
    internal DoubleItem Get(int index) => index < 0 || index >= _count ? Default : _list[index];

    // Engine.pas:5208
    internal void Put(int index, DoubleItem item)
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

    // Engine.pas:5233
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

    // Engine.pas:5244
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
            FillLargeInteger(_list.AsSpan(_count), count: newCount - _count, value: Default);
        }

        _count = newCount;
    }

    // Engine.pas:5148
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // Engine.pas:5056
    public override void Dispose() => Clear();

    // Engine.pas:5061
    public override void Assign(Persistent? source)
    {
        if (source is DoubleItemList doubleItemList)
        {
            _count = doubleItemList._count;
            SetCapacity(doubleItemList._count);
            Array.Copy(doubleItemList._list, sourceIndex: 0, destinationArray: _list, destinationIndex: 0, length: _count);
            Default = doubleItemList.Default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:5075
    public int Add(in DoubleItem item)
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

    // Engine.pas:5083
    public int AddAbility(int a, int b)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        _list[result] = new DoubleItem(First: a, Second: b);
        _count += 1;
        return result;
    }

    // Engine.pas:5092
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:5098
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

    // Engine.pas:5118
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

    // Engine.pas:5129
    public DoubleItemList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:5135
    public DoubleItem First() => Get(0);

    // Engine.pas:5158. The Delphi compares the items as Comp, which is exact. In range the
    // indexer returns _list[i], and the record equality compares the same fields.
    public int IndexOf(in DoubleItem item)
    {
        // A lambda cannot capture the in parameter.
        DoubleItem sought = item;
        return this.FindIndex(entry => entry == sought);
    }

    // Engine.pas:5165
    public int IndexOfItemA(int A) => this.FindIndex(entry => entry.First == A);

    // Engine.pas:5172
    public int IndexOfItemB(int B) => this.FindIndex(entry => entry.Second == B);

    // Engine.pas:5179
    public void Insert(int index, in DoubleItem item)
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

    // Engine.pas:5190
    public DoubleItem Last() => Get(_count - 1);

    // Engine.pas:5195
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", data: newIndex);
            }

            DoubleItem item = Get(currentIndex);
            Delete(currentIndex);
            Insert(newIndex, item);
        }
    }

    // Engine.pas:5215
    public int Remove(in DoubleItem item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:5221
    public void Pack()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            if (this[i] == Default)
            {
                Delete(i);
            }
        }
    }

    // Engine.pas:5281
    public void Sort(Comparison<DoubleItem> compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortDoubleItem(_list, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    // Engine.pas:5307
    public void SortAscendingItemA() => Sort(CompareDoubleItemAAscending);

    // Engine.pas:5312
    public void SortAscendingItemB() => Sort(CompareDoubleItemBAscending);

    // Engine.pas:5317
    public void SortDescendingItemA() => Sort(CompareDoubleItemADescending);

    // Engine.pas:5322
    public void SortDescendingItemB() => Sort(CompareDoubleItemBDescending);

    // Engine.pas:5327
    public bool Compare(DoubleItemList list) => list._count == _count && _list.AsSpan(0, length: _count).SequenceEqual(list._list.AsSpan(0, length: _count));

    // Engine.pas:5394
    public int RandomChanceItemA(int bonus)
    {
        int result;
        if (_count != 0)
        {
            // The draw below stays a loop. The weights are a checked sum (decision 1).
            int total = this.Sum(entry => Math.Max(entry.First + bonus, val2: 0));
            int chance;

            int r = Random(total);
            total = 0;
            result = 0;
            while (total < r)
            {
                chance = _list[result].First + bonus;
                if (chance < 0)
                {
                    chance = 0;
                }

                total += chance;
                result += 1;
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    // Engine.pas:5422
    public int RandomChanceItemB(int bonus)
    {
        int result;
        if (_count != 0)
        {
            // The draw below stays a loop. A non-positive weight adds nothing, as the loop did.
            int total = this.Sum(entry => Math.Max(entry.Second + bonus, val2: 0));
            int chance;

            int r = Random(total);
            result = 0;
            total = 0;
            chance = _list[0].Second + bonus;
            if (chance > 0)
            {
                total += chance;
            }

            while (total <= r)
            {
                result += 1;
                chance = _list[result].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }
    public DoubleItem Default { get; set; }

    public DoubleItem this[int index] { get => Get(index); set => Put(index, item: value); }

    public DoubleItem[] List => _list;

    // Engine.pas:5107
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

    private static void QuickSortDoubleItem(Span<DoubleItem> sortList, int L, int R, Comparison<DoubleItem> sCompare)
    {
        int number;
        do
        {
            number = L;
            int number2 = R;
            DoubleItem item = sortList[L + R >>> 1];
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
                QuickSortDoubleItem(sortList, L, R: number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    private static int CompareDoubleItemAAscending(DoubleItem item1, DoubleItem item2) => item1.First - item2.First;

    private static int CompareDoubleItemBAscending(DoubleItem item1, DoubleItem item2) => item1.Second - item2.Second;

    private static int CompareDoubleItemADescending(DoubleItem item1, DoubleItem item2) => item2.First - item1.First;

    private static int CompareDoubleItemBDescending(DoubleItem item1, DoubleItem item2) => item2.Second - item1.Second;

    private static void FillLargeInteger(Span<DoubleItem> buffer, int count, DoubleItem value) => buffer[..count].Fill(value);

    // Delphi TDoubleItem, two integers. Equality compares both, as the Delphi Comp compare of the
    // eight bytes does.
    public readonly record struct DoubleItem(int First, int Second);

    // The items from the first to the last in index order, read through the indexer. Each step
    // reads the count again, as the index loop over a live count does.
    public IEnumerator<DoubleItem> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
