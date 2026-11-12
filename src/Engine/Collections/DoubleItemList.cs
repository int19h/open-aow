// Translated from Engine.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class DoubleItemList : Persistent
{
    internal DoubleItem[] _list = [];
    internal int _count;
    internal int _capacity;
    internal DoubleItem _default;

    // Engine.pas:5140
    internal DoubleItem Get(int index)
    {
        DoubleItem result;
        if (index < 0 || index >= _count)
        {
            result = _default;
        }
        else
        {
            result = _list[index];
        }

        return result;
    }

    // Engine.pas:5208
    internal void Put(int index, DoubleItem item)
    {
        if (index < 0)
        {
            Error("List index out of bounds (%d)", index);
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
            Error("List capacity out of bounds (%d)", newCapacity);
        }

        if (newCapacity != _capacity)
        {
            Array.Resize(ref _list, newCapacity);
            _capacity = newCapacity;
        }
    }

    // Engine.pas:5244
    internal void SetCount(int newCount)
    {
        if (newCount < 0 || newCount > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", newCount);
        }

        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            FillLargeInteger(_list.AsSpan(_count), newCount - _count, _default);
        }

        _count = newCount;
    }

    // Engine.pas:5148
    protected internal virtual void Grow()
    {
        int delta;
        if (_capacity > 64)
        {
            delta = _capacity / 4;
        }
        else
        {
            if (_capacity > 8)
            {
                delta = 16;
            }
            else
            {
                delta = 4;
            }
        }

        SetCapacity(_capacity + delta);
    }

    // Engine.pas:5056
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:5061
    public override void Assign(Persistent source)
    {
        if (source is DoubleItemList)
        {
            var doubleItemList = (DoubleItemList)source;
            this._count = doubleItemList._count;
            this.SetCapacity(doubleItemList._count);
            Array.Copy(doubleItemList._list, 0, this._list, 0, this._count);
            this._default = doubleItemList._default;
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
    public int AddAbility(int A, int B)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        _list[result].First = A;
        _list[result].Second = B;
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
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(_list.AsSpan(index + 1, _count - index)), MemoryMarshal.AsBytes(_list.AsSpan(index, _count - index)), (_count - index) * 8);
        }
    }

    // Engine.pas:5118
    public void Exchange(int index1, int index2)
    {
        DoubleItem item;
        if (index1 < 0 || index1 >= _count)
        {
            Error("List index out of bounds (%d)", index1);
        }

        if (index2 < 0 || index2 >= _count)
        {
            Error("List index out of bounds (%d)", index2);
        }

        item = _list[index1];
        _list[index1] = _list[index2];
        _list[index2] = item;
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

    // Engine.pas:5158
    public int IndexOf(in DoubleItem item)
    {
        int result = 0;
        while (result < _count && (double)Unsafe.As<DoubleItem, long>(ref _list[result]) != Unsafe.BitCast<DoubleItem, long>(item))
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:5165
    public int IndexOfItemA(int A)
    {
        int result = 0;
        while (result < _count && _list[result].First != A)
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:5172
    public int IndexOfItemB(int B)
    {
        int result = 0;
        while (result < _count && _list[result].Second != B)
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:5179
    public void Insert(int index, in DoubleItem item)
    {
        if (index < 0 || index > _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        if (_count == _capacity)
        {
            Grow();
        }

        if (index < _count)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(_list.AsSpan(index, _count - index)), MemoryMarshal.AsBytes(_list.AsSpan(index + 1, _count - index)), (_count - index) * 8);
        }

        _list[index] = item;
        _count += 1;
    }

    // Engine.pas:5190
    public DoubleItem Last() => Get(_count - 1);

    // Engine.pas:5195
    public void Move(int currentIndex, int newIndex)
    {
        DoubleItem item;
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", newIndex);
            }

            item = Get(currentIndex);
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
        DoubleItem item = default;
        for (int I = _count - 1; I >= 0; I--)
        {
            item = this[I];
            if ((double)Unsafe.As<DoubleItem, long>(ref item) == Unsafe.As<DoubleItem, long>(ref _default))
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5281
    public void Sort(DoubleItemSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortDoubleItem(_list, 0, Count - 1, compare);
        }
    }

    // Engine.pas:5307
    public void SortAscendingItemA()
    {
        Sort(CompareDoubleItemAAscending);
    }

    // Engine.pas:5312
    public void SortAscendingItemB()
    {
        Sort(CompareDoubleItemBAscending);
    }

    // Engine.pas:5317
    public void SortDescendingItemA()
    {
        Sort(CompareDoubleItemADescending);
    }

    // Engine.pas:5322
    public void SortDescendingItemB()
    {
        Sort(CompareDoubleItemBDescending);
    }

    // Engine.pas:5327
    public bool Compare(DoubleItemList list) => list._count == _count && CompareMem(MemoryMarshal.AsBytes(_list.AsSpan(0, _count)), MemoryMarshal.AsBytes(list._list.AsSpan(0, _count)), _count * 8);

    // Engine.pas:5334
    public void LoadFromStream(Stream stream)
    {
        DoubleItem item = default;
        int run = default;
        int count = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default, 8));
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (_count < count)
        {
            stream.ReadExactly(Bytes.Of(ref item, 8));
            if ((double)Unsafe.As<DoubleItem, long>(ref item) == Unsafe.As<DoubleItem, long>(ref _default))
            {
                stream.ReadExactly(Bytes.Of(ref run, 4));
                SetCount(_count + run);
            }
            else
            {
                Add(item);
            }
        }
    }

    // Engine.pas:5356
    public void SaveToStream(Stream stream)
    {
        int I;
        int run = default;
        int DefaultRun()
        {
            int J;
            int result = 0;
            J = I;
            while (J < _count && (double)Unsafe.As<DoubleItem, long>(ref _list[J]) == Unsafe.As<DoubleItem, long>(ref _default))
            {
                result += 1;
                J += 1;
            }

            return result;
        }

        I = 0;
        stream.Write(Bytes.Of(ref _default, 8));
        stream.Write(Bytes.Of(ref _count, 4));
        while (_count != I)
        {
            run = DefaultRun();
            if (run > 0)
            {
                stream.Write(Bytes.Of(ref _default, 8));
                stream.Write(Bytes.Of(ref run, 4));
                I += run;
            }
            else
            {
                stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:5394
    public int RandomChanceItemA(int bonus)
    {
        int result;
        int total;
        int chance;
        int R;
        if (_count != 0)
        {
            total = 0;
            for (int I = _count - 1; I >= 0; I--)
            {
                chance = _list[I].First + bonus;
                if (chance < 0)
                {
                    chance = 0;
                }

                total += chance;
            }

            R = Random(total);
            total = 0;
            result = 0;
            while (total < R)
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
            result = -1;
        }

        return result;
    }

    // Engine.pas:5422
    public int RandomChanceItemB(int bonus)
    {
        int result;
        int total;
        int chance;
        int R;
        if (_count != 0)
        {
            total = 0;
            for (int I = _count - 1; I >= 0; I--)
            {
                chance = _list[I].Second + bonus;
                if (chance > 0)
                {
                    total += chance;
                }
            }

            R = Random(total);
            result = 0;
            total = 0;
            chance = _list[0].Second + bonus;
            if (chance > 0)
            {
                total += chance;
            }

            while (total <= R)
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
            result = -1;
        }

        return result;
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }
    public DoubleItem Default { get => _default; set => _default = value; }

    public DoubleItem this[int index] { get => Get(index); set => Put(index, value); }

    public DoubleItem[] List { get => _list; }

    // Engine.pas:5107
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortDoubleItem(Span<DoubleItem> sortList, int L, int R, DoubleItemSortCompare sCompare)
    {
        DoubleItem doubleItem = default(DoubleItem);
        int number;
        do
        {
            number = L;
            int number2 = R;
            DoubleItem item = sortList[L + R >>> 1];
            while (true)
            {
                if (sCompare(in sortList[number], in item) < 0)
                {
                    number++;
                    continue;
                }
                while (sCompare(in sortList[number2], in item) > 0)
                {
                    number2--;
                }
                if (number <= number2)
                {
                    doubleItem = sortList[number];
                    sortList[number] = sortList[number2];
                    sortList[number2] = doubleItem;
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
                QuickSortDoubleItem(sortList, L, number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    private static int CompareDoubleItemAAscending(in DoubleItem item1, in DoubleItem item2)
    {
        return item1.First - item2.First;
    }

    private static int CompareDoubleItemBAscending(in DoubleItem item1, in DoubleItem item2)
    {
        return item1.Second - item2.Second;
    }

    private static int CompareDoubleItemADescending(in DoubleItem item1, in DoubleItem item2)
    {
        return item2.First - item1.First;
    }

    private static int CompareDoubleItemBDescending(in DoubleItem item1, in DoubleItem item2)
    {
        return item2.Second - item1.Second;
    }

    private static void FillLargeInteger(Span<DoubleItem> buffer, int count, DoubleItem value)
    {
        EngineServices.Fill(buffer, count, value);
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct DoubleItem
    {
        [FieldOffset(0)]
        public int First;
        [FieldOffset(4)]
        public int Second;
    }

    public delegate int DoubleItemSortCompare(in DoubleItem Item1, in DoubleItem Item2);

}
