// Translated from ImageLib.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class FrameTableList : Persistent
{
    private int[] _list;
    internal int _count;
    private int _capacity;
    private int _defaultValue;

    // ImageLib.pas:9434
    protected internal int Get(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        return _list[index];
    }

    // ImageLib.pas:9440
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

    // ImageLib.pas:9486
    // { Unlike TList.Put, an index past the end grows the list }
    protected internal void Put(int index, int item)
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

    // ImageLib.pas:9506
    protected internal void SetCapacity(int newCapacity)
    {
        if (newCapacity < _count || newCapacity > 134217727)
        {
            Error("List capacity out of bounds (%d)", newCapacity);
        }

        if (newCapacity != _capacity)
        {
            _list = ReallocArray(_list, newCapacity);
            _capacity = newCapacity;
        }
    }

    // ImageLib.pas:9517
    protected internal void SetCount(int newCount)
    {
        if (newCount < 0 || newCount > 134217727)
        {
            Error("List count out of bounds (%d)", newCount);
        }

        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            FillBytes(MemoryMarshal.AsBytes(_list.AsSpan(_count, newCount - _count)), _defaultValue);
        }

        _count = newCount;
    }

    // ImageLib.pas:9360
    public override void Dispose()
    {
        Clear();
    }

    // ImageLib.pas:9365
    public override void Assign(Persistent source)
    {
        if (source is FrameTableList)
        {
            var frameTableList = (FrameTableList)source;
            this.SetCapacity(frameTableList._count);
            this._count = frameTableList._count;
            MoveElements(frameTableList._list, 0, this._list, 0, this._count);
            this._defaultValue = frameTableList._defaultValue;
        }
        else
        {
            base.Assign(source);
        }
    }

    // ImageLib.pas:9379
    public int Add(int item)
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

    // ImageLib.pas:9387
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // ImageLib.pas:9393
    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            MoveElements(_list, index + 1, _list, index, _count - index);
        }
    }

    // ImageLib.pas:9412
    public void Exchange(int index1, int index2)
    {
        int item;
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

    // ImageLib.pas:9423
    public FrameTableList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // ImageLib.pas:9429
    public int First() => Get(0);

    // ImageLib.pas:9450
    public int IndexOf(int item)
    {
        int result = 0;
        while (result < _count && _list[result] != item)
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:9457
    public void Insert(int index, int item)
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
            MoveElements(_list, index, _list, index + 1, _count - index);
        }

        _list[index] = item;
        _count += 1;
    }

    // ImageLib.pas:9467
    public int Last() => Get(_count - 1);

    // ImageLib.pas:9472
    public void Move(int currentIndex, int newIndex)
    {
        int item;
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

    // ImageLib.pas:9493
    public int Remove(int item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // ImageLib.pas:9499
    public void Pack()
    {
        for (int I = _count - 1; I >= 0; I--)
        {
            if (this[I] == 0)
            {
                Delete(I);
            }
        }
    }

    // ImageLib.pas:9554
    public void Sort(FrameTableSortCompare compare)
    {
        if (_list != null && Count > 0)
        {
            FrameTableQuickSort(_list, 0, Count - 1, compare);
        }
    }

    // ImageLib.pas:9560
    public bool Compare(FrameTableList list) => list._count == _count && CompareMem(MemoryMarshal.AsBytes(_list.AsSpan(0, _count)), MemoryMarshal.AsBytes(list._list.AsSpan(0, _count)), _count * 4);

    // ImageLib.pas:9566
    public void LoadFromStream(Stream stream)
    {
        int N = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _defaultValue, 4));
        stream.ReadExactly(Bytes.Of(ref N, 4));
        SetCapacity(N);
        stream.ReadExactly(MemoryMarshal.AsBytes(_list.AsSpan(0, N)));
        _count = N;
    }

    /// <summary><see cref="LoadFromStream"/> from a stored default and stored items.</summary>
    internal void LoadItems(int defaultValue, IReadOnlyList<int> items)
    {
        Clear();
        _defaultValue = defaultValue;
        SetCapacity(items.Count);
        for (int I = 0; I < items.Count; I++)
        {
            _list[I] = items[I];
        }

        _count = items.Count;
    }

    // ImageLib.pas:9578
    public void SaveToStream(Stream stream)
    {
        stream.Write(Bytes.Of(ref _defaultValue, 4));
        stream.Write(Bytes.Of(ref _count, 4));
        stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(0, _count)));
    }

    // ImageLib.pas:9586
    // { The items separated by commas }
    public string GetItemString()
    {
        string result = "";
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            result = result + IntToStr(_list[I]) + ",";
        }

        if (!IsEmpty(result))
        {
            result = Copy(result, 1, result.Length - 1);
        }

        return result;
    }

    // ImageLib.pas:9597
    // { Reads items separated by commas; other characters than digits are dropped, an empty item
    //   is 0 }
    public void SetItemString(string value)
    {
        int I;
        int N;
        string S;
        I = value.Length;
        for (; I >= 1; I -= 1)
        {
            if (!Set<byte>.WithBounds(0, 255).With(44).WithRange(48, 57).Contains(Cp1252.Byte(value[I - 1])))
            {
                value = RtlUnit.Delete(value, I, 1);
            }
        }

        Clear();
        I = Pos(",", value);
        while (!IsEmpty(value))
        {
            if (I != 0)
            {
                S = Copy(value, 1, I - 1);
                value = RtlUnit.Delete(value, 1, I);
            }
            else
            {
                S = value;
                value = "";
            }

            if (!IsEmpty(S))
            {
                N = StrToInt(S);
            }
            else
            {
                N = 0;
            }

            Add(N);
            I = Pos(",", value);
        }
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }

    public int this[int index] { get => Get(index); set => Put(index, value); }

    public int[] List { get => _list; }
    public int DefaultValue { get => _defaultValue; set => _defaultValue = value; }
    public string ItemString { get => GetItemString(); set => SetItemString(value); }

    // ImageLib.pas:9401
    public static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void FrameTableQuickSort(int[] sortList, int L, int R, FrameTableSortCompare sCompare)
    {
        int number = 0;
        int number2;
        do
        {
            number2 = L;
            int number3 = R;
            int item = sortList[L + R >>> 1];
            while (true)
            {
                if (sCompare(sortList[number2], item) < 0)
                {
                    number2++;
                    continue;
                }
                while (sCompare(sortList[number3], item) > 0)
                {
                    number3--;
                }
                if (number2 <= number3)
                {
                    number = sortList[number2];
                    sortList[number2] = sortList[number3];
                    sortList[number3] = number;
                    number2++;
                    number3--;
                }
                if (number2 > number3)
                {
                    break;
                }
            }
            if (L < number3)
            {
                FrameTableQuickSort(sortList, L, number3, sCompare);
            }
            L = number2;
        }
        while (number2 < R);
    }

    public delegate int FrameTableSortCompare(int Item1, int Item2);

}
