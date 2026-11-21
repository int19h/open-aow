#nullable disable warnings
// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Engine.Collections;

public class SByteList : Persistent
{
    private sbyte[] _list = [];
    private int _count;
    private int _capacity;
    private sbyte _default;

    // Engine.pas:4025
    internal sbyte Get(int index) => index < 0 || index >= _count ? _default : _list[index];

    // Engine.pas:4079
    internal void Put(int index, sbyte item)
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

    // Engine.pas:4099
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity < _count || newCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", data: newCapacity);
        }

        if (newCapacity != _capacity)
        {
            Array.Resize(ref _list, newSize: newCapacity * 1);
            _capacity = newCapacity;
        }
    }

    // Engine.pas:4110
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
            _list.AsSpan(_count, length: newCount - _count).Fill(_default);
        }

        _count = newCount;
    }

    // Engine.pas:4033
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // Engine.pas:3950
    public override void Dispose() => Clear();

    // Engine.pas:3955
    public override void Assign(Persistent? source)
    {
        if (source is SByteList sByteList)
        {
            _count = sByteList._count;
            SetCapacity(sByteList._count);
            Array.Copy(sByteList._list, sourceIndex: 0, destinationArray: _list, destinationIndex: 0, length: _count * 1);
            _default = sByteList._default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:3969
    public int Add(sbyte item)
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

    // Engine.pas:3977
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3983
    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        _count -= 1;
        if (index < _count)
        {
            Array.Copy(_list, sourceIndex: index + 1, destinationArray: _list, destinationIndex: index, length: (_count - index) * 1);
        }
    }

    // Engine.pas:4003
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

    // Engine.pas:4014
    public SByteList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4020
    public sbyte First() => Get(0);

    // Engine.pas:4043
    public int IndexOf(sbyte item)
    {
        int result = 0;
        while (result < _count && _list[result] != item)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:4050
    public void Insert(int index, sbyte item)
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
            Array.Copy(_list, sourceIndex: index, destinationArray: _list, destinationIndex: index + 1, length: (_count - index) * 1);
        }

        _list[index] = item;
        _count += 1;
    }

    // Engine.pas:4061
    public sbyte Last() => Get(_count - 1);

    // Engine.pas:4066
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", data: newIndex);
            }

            sbyte item = Get(currentIndex);
            Delete(currentIndex);
            Insert(newIndex, item);
        }
    }

    // Engine.pas:4086
    public int Remove(sbyte item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:4092
    public void Pack()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            if (this[i] == _default)
            {
                Delete(i);
            }
        }
    }

    // Engine.pas:4147
    public void Sort(Comparison<sbyte> compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortShortInteger(_list, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    // Engine.pas:4153
    public bool Compare(SByteList list) => list._count == _count && _list.AsSpan(0, length: _count).SequenceEqual(list._list.AsSpan(0, length: _count));

    // Engine.pas:4160
    public void LoadFromStream(Stream stream)
    {
        byte run = default;
        sbyte item = default;
        int count = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default));
        stream.ReadExactly(Bytes.Of(ref count));
        SetCapacity(count);
        while (_count < count)
        {
            stream.ReadExactly(Bytes.Of(ref item));
            if (item == _default)
            {
                stream.ReadExactly(Bytes.Of(ref run));
                SetCount(_count + run);
            }
            else
            {
                _ = Add(item);
            }
        }
    }

    // Engine.pas:4183
    public void SaveToStream(Stream stream)
    {
        int i;
        int DefaultRun()
        {
            int result = 0;
            int j = i;
            while (j < _count && _list[j] == _default)
            {
                result += 1;
                j += 1;
            }

            return result;
        }

        i = 0;
        stream.Write(Bytes.Of(ref _default));
        stream.Write(Bytes.Of(ref _count));
        while (_count != i)
        {
            int run = DefaultRun();
            if (run > 0)
            {
                byte b;
                while (run > 255)
                {
                    b = 255;
                    stream.Write(Bytes.Of(ref _default));
                    stream.Write(Bytes.Of(ref b));
                    run -= 255;
                    i += 255;
                }

                b = (byte)run;
                stream.Write(Bytes.Of(ref _default));
                stream.Write(Bytes.Of(ref b));
                i += run;
            }
            else
            {
                stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(i, length: 1)));
                i += 1;
            }
        }
    }

    // Engine.pas:4231
    public int RandomChanceItem(int bonus)
    {
        int result;
        if (_count != 0)
        {
            int total = 0;
            int chance;
            for (int i = _count - 1; i >= 0; i--)
            {
                chance = _list[i] + bonus;
                if (chance < 0)
                {
                    chance = 0;
                }

                total += chance;
            }

            int r = Random(total);
            total = 0;
            result = 0;
            while (total < r)
            {
                chance = _list[result] + bonus;
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

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; internal set => SetCount(value); }
    public sbyte Default { get => _default; init => _default = value; }

    public sbyte this[int index] { get => Get(index); set => Put(index, item: value); }

    public sbyte[] List => _list;

    // Engine.pas:3992
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

    private static void QuickSortShortInteger(Span<sbyte> sortList, int L, int R, Comparison<sbyte> sCompare)
    {
        int number;
        do
        {
            number = L;
            int number2 = R;
            sbyte item = sortList[L + R >>> 1];
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
                QuickSortShortInteger(sortList, L, R: number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }
}
