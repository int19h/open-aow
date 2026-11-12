// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class QuadItemList : Persistent
{
    internal QuadInteger[] _list = [];
    internal int _count;
    internal int _capacity;
    internal QuadInteger _default;

    // Engine.pas:5538
    internal QuadInteger Get(int index)
    {
        QuadInteger result;
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

    // Engine.pas:5621
    internal void Put(int index, QuadInteger item)
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

    // Engine.pas:5643
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

    // Engine.pas:5654
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
            FillQuadInteger(_list.AsSpan(_count), newCount - _count, _default);
        }

        _count = newCount;
    }

    // Engine.pas:5546
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

    // Engine.pas:5452
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:5457
    public override void Assign(Persistent source)
    {
        if (source is QuadItemList)
        {
            var quadItemList = (QuadItemList)source;
            this._count = quadItemList._count;
            this.SetCapacity(quadItemList._count);
            Array.Copy(quadItemList._list, 0, this._list, 0, this._count);
            this._default = quadItemList._default;
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

        _list[result].First = A;
        _list[result].Second = B;
        _list[result].Third = C;
        _list[result].Fourth = D;
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
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(_list.AsSpan(index + 1, _count - index)), MemoryMarshal.AsBytes(_list.AsSpan(index, _count - index)), (_count - index) * 16);
        }
    }

    // Engine.pas:5516
    public void Exchange(int index1, int index2)
    {
        QuadInteger item;
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
        while (result < _count && (_list[result].First != item.First || _list[result].Second != item.Second || _list[result].Third != item.Third || _list[result].Fourth != item.Fourth))
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:5564
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

    // Engine.pas:5571
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

    // Engine.pas:5578
    public int IndexOfItemC(int C)
    {
        int result = 0;
        while (result < _count && _list[result].Third != C)
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:5585
    public int IndexOfItemD(int D)
    {
        int result = 0;
        while (result < _count && _list[result].Fourth != D)
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:5592
    public void Insert(int index, in QuadInteger item)
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
            RtlUnit.Move(MemoryMarshal.AsBytes(_list.AsSpan(index, _count - index)), MemoryMarshal.AsBytes(_list.AsSpan(index + 1, _count - index)), (_count - index) * 16);
        }

        _list[index] = item;
        _count += 1;
    }

    // Engine.pas:5603
    public QuadInteger Last() => Get(_count - 1);

    // Engine.pas:5608
    public void Move(int currentIndex, int newIndex)
    {
        QuadInteger item;
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
        for (int I = _count - 1; I >= 0; I--)
        {
            if (_list[I].First == _default.First && _list[I].Second == _default.Second && _list[I].Third == _default.Third && _list[I].Fourth == _default.Fourth)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5691
    public void Sort(QuadItemSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortQuadItem(_list, 0, Count - 1, compare);
        }
    }

    // Engine.pas:5737
    public void SortAscendingItemA()
    {
        Sort(CompareQuadItemAAscending);
    }

    // Engine.pas:5742
    public void SortAscendingItemB()
    {
        Sort(CompareQuadItemBAscending);
    }

    // Engine.pas:5747
    public void SortAscendingItemC()
    {
        Sort(CompareQuadItemCAscending);
    }

    // Engine.pas:5752
    public void SortAscendingItemD()
    {
        Sort(CompareQuadItemDAscending);
    }

    // Engine.pas:5757
    public void SortDescendingItemA()
    {
        Sort(CompareQuadItemADescending);
    }

    // Engine.pas:5762
    public void SortDescendingItemB()
    {
        Sort(CompareQuadItemBDescending);
    }

    // Engine.pas:5767
    public void SortDescendingItemC()
    {
        Sort(CompareQuadItemCDescending);
    }

    // Engine.pas:5772
    public void SortDescendingItemD()
    {
        Sort(CompareQuadItemDDescending);
    }

    // Engine.pas:5777
    public bool Compare(QuadItemList list) => list._count == _count && CompareMem(MemoryMarshal.AsBytes(_list.AsSpan(0, _count)), MemoryMarshal.AsBytes(list._list.AsSpan(0, _count)), _count * 16);

    // Engine.pas:5784
    public void LoadFromStream(Stream stream)
    {
        int run = default;
        int count = default;
        QuadInteger item = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default, 16));
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (_count < count)
        {
            stream.ReadExactly(Bytes.Of(ref item, 16));
            if (item.First == _default.First && item.Second == _default.Second && item.Third == _default.Third && item.Fourth == _default.Fourth)
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

    // Engine.pas:5807
    public void SaveToStream(Stream stream)
    {
        int I;
        int run = default;
        int DefaultRun()
        {
            int J;
            int result = 0;
            J = I;
            while (J < _count && (_list[J].First == _default.First && _list[J].Second == _default.Second && _list[J].Third == _default.Third && _list[J].Fourth == _default.Fourth))
            {
                result += 1;
                J += 1;
            }

            return result;
        }

        I = 0;
        stream.Write(Bytes.Of(ref _default, 16));
        stream.Write(Bytes.Of(ref _count, 4));
        while (_count != I)
        {
            run = DefaultRun();
            if (run > 0)
            {
                stream.Write(Bytes.Of(ref _default, 16));
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

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }
    public QuadInteger Default { get => _default; set => _default = value; }

    public QuadInteger this[int index] { get => Get(index); set => Put(index, value); }

    public QuadInteger[] List { get => _list; }

    // Engine.pas:5505
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortQuadItem(Span<QuadInteger> sortList, int L, int R, QuadItemSortCompare sCompare)
    {
        QuadInteger quadInteger = default(QuadInteger);
        int number;
        do
        {
            number = L;
            int number2 = R;
            QuadInteger item = sortList[L + R >>> 1];
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
                    quadInteger = sortList[number];
                    sortList[number] = sortList[number2];
                    sortList[number2] = quadInteger;
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
                QuickSortQuadItem(sortList, L, number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    private static int CompareQuadItemAAscending(in QuadInteger item1, in QuadInteger item2)
    {
        return item1.First - item2.First;
    }

    private static int CompareQuadItemBAscending(in QuadInteger item1, in QuadInteger item2)
    {
        return item1.Second - item2.Second;
    }

    private static int CompareQuadItemCAscending(in QuadInteger item1, in QuadInteger item2)
    {
        return item1.Third - item2.Third;
    }

    private static int CompareQuadItemDAscending(in QuadInteger item1, in QuadInteger item2)
    {
        return item1.Fourth - item2.Fourth;
    }

    private static int CompareQuadItemADescending(in QuadInteger item1, in QuadInteger item2)
    {
        return item2.First - item1.First;
    }

    private static int CompareQuadItemBDescending(in QuadInteger item1, in QuadInteger item2)
    {
        return item2.Second - item1.Second;
    }

    private static int CompareQuadItemCDescending(in QuadInteger item1, in QuadInteger item2)
    {
        return item2.Third - item1.Third;
    }

    private static int CompareQuadItemDDescending(in QuadInteger item1, in QuadInteger item2)
    {
        return item2.Fourth - item1.Fourth;
    }

    internal static void FillQuadInteger(Span<QuadInteger> buffer, int count, in QuadInteger value)
    {
        QuadInteger value2 = value;
        EngineServices.Fill(buffer, count, value2);
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct QuadInteger
    {
        [FieldOffset(0)]
        public int First;
        [FieldOffset(4)]
        public int Second;
        [FieldOffset(8)]
        public int Third;
        [FieldOffset(12)]
        public int Fourth;
    }

    public delegate int QuadItemSortCompare(in QuadInteger Item1, in QuadInteger Item2);

}
