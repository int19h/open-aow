#nullable disable
// Translated from ImageLib.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Draw.Images;

public class FrameTableList : Persistent
{
    private int _count;
    private int _capacity;
    private int _defaultValue;

    // ImageLib.pas:9434
    protected internal int Get(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        return List[index];
    }

    // ImageLib.pas:9440
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // ImageLib.pas:9486
    // { Unlike TList.Put, an index past the end grows the list }
    protected internal void Put(int index, int item)
    {
        if (index < 0)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        if (index >= _count)
        {
            SetCount(index + 1);
        }

        List[index] = item;
    }

    // ImageLib.pas:9506
    protected internal void SetCapacity(int newCapacity)
    {
        if (newCapacity < _count || newCapacity > 134_217_727)
        {
            Error("List capacity out of bounds (%d)", data: newCapacity);
        }

        if (newCapacity != _capacity)
        {
            List = ReallocArray(List, count: newCapacity);
            _capacity = newCapacity;
        }
    }

    // ImageLib.pas:9517
    protected internal void SetCount(int newCount)
    {
        if (newCount is < 0 or > 134_217_727)
        {
            Error("List count out of bounds (%d)", data: newCount);
        }

        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            // FillChar: each byte of a new item is the low byte of the default.
            List.AsSpan(_count, length: newCount - _count).Fill(unchecked((byte)_defaultValue * 0x0101_0101));
        }

        _count = newCount;
    }

    // ImageLib.pas:9360
    public override void Dispose() => Clear();

    // ImageLib.pas:9365
    public override void Assign(Persistent source)
    {
        if (source is FrameTableList frameTableList)
        {
            SetCapacity(frameTableList._count);
            _count = frameTableList._count;
            MoveElements(frameTableList.List, sourceIndex: 0, destination: List, destinationIndex: 0, _count);
            _defaultValue = frameTableList._defaultValue;
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

        List[result] = item;
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
            Error("List index out of bounds (%d)", data: index);
        }

        _count -= 1;
        if (index < _count)
        {
            MoveElements(List, sourceIndex: index + 1, destination: List, destinationIndex: index, count: _count - index);
        }
    }

    // ImageLib.pas:9412
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

        (List[index2], List[index1]) = (List[index1], List[index2]);
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
        while (result < _count && List[result] != item)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // ImageLib.pas:9457
    public void Insert(int index, int item)
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
            MoveElements(List, sourceIndex: index, destination: List, destinationIndex: index + 1, count: _count - index);
        }

        List[index] = item;
        _count += 1;
    }

    // ImageLib.pas:9467
    public int Last() => Get(_count - 1);

    // ImageLib.pas:9472
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", data: newIndex);
            }

            int item = Get(currentIndex);
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
        for (int i = _count - 1; i >= 0; i--)
        {
            if (this[i] == 0)
            {
                Delete(i);
            }
        }
    }

    // ImageLib.pas:9554
    public void Sort(FrameTableSortCompare compare)
    {
        if (List is not null && Count > 0)
        {
            FrameTableQuickSort(List, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    // ImageLib.pas:9560
    public bool Compare(FrameTableList list) => list._count == _count && List.AsSpan(0, length: _count).SequenceEqual(list.List.AsSpan(0, length: _count));

    // ImageLib.pas:9566
    public void LoadFromStream(Stream stream)
    {
        int n = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _defaultValue));
        stream.ReadExactly(Bytes.Of(ref n));
        SetCapacity(n);
        stream.ReadExactly(MemoryMarshal.AsBytes(List.AsSpan(0, length: n)));
        _count = n;
    }

    /// <summary><see cref="LoadFromStream"/> from a stored default and stored items.</summary>
    internal void LoadItems(int defaultValue, IReadOnlyList<int> items)
    {
        Clear();
        _defaultValue = defaultValue;
        SetCapacity(items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            List[i] = items[i];
        }

        _count = items.Count;
    }

    // ImageLib.pas:9578
    public void SaveToStream(Stream stream)
    {
        stream.Write(Bytes.Of(ref _defaultValue));
        stream.Write(Bytes.Of(ref _count));
        stream.Write(MemoryMarshal.AsBytes(List.AsSpan(0, length: _count)));
    }

    // ImageLib.pas:9586
    // { The items separated by commas }
    public string GetItemString()
    {
        string result = "";
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            result = $"{result}{IntToStr(List[i])},";
        }

        return !IsEmpty(result) ? Copy(result, index: 1, count: result.Length - 1) : result;
    }

    // ImageLib.pas:9597
    // { Reads items separated by commas; other characters than digits are dropped, an empty item
    //   is 0 }
    public void SetItemString(string value)
    {
        int i = value.Length;
        for (; i >= 1; i -= 1)
        {
            if (!Set<byte>.WithBounds(0, 255).With(44).WithRange(48, 57).Contains(Cp1252.Byte(value[i - 1])))
            {
                value = DelphiStrings.Delete(value, index: i, count: 1);
            }
        }

        Clear();
        i = Pos(",", haystack: value);
        while (!IsEmpty(value))
        {
            string s;
            if (i != 0)
            {
                s = Copy(value, index: 1, count: i - 1);
                value = DelphiStrings.Delete(value, index: 1, count: i);
            }
            else
            {
                s = value;
                value = "";
            }

            int n = !IsEmpty(s) ? StrToInt(s) : 0;

            _ = Add(n);
            i = Pos(",", haystack: value);
        }
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; internal set => SetCount(value); }

    public int this[int index] { get => Get(index); set => Put(index, item: value); }

    public int[] List { get; private set; }
    public int DefaultValue { get => _defaultValue; init => _defaultValue = value; }
    public string ItemString => GetItemString();

    // ImageLib.pas:9401
    public static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

    private static void FrameTableQuickSort(int[] sortList, int L, int R, FrameTableSortCompare sCompare)
    {
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
                    (sortList[number3], sortList[number2]) = (sortList[number2], sortList[number3]);
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
                FrameTableQuickSort(sortList, L, R: number3, sCompare);
            }
            L = number2;
        }
        while (number2 < R);
    }

    public delegate int FrameTableSortCompare(int Item1, int Item2);

}
