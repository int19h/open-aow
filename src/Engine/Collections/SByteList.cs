// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class SByteList : Persistent
{
    private sbyte[] _list = [];
    private int _count;
    private int _capacity;
    private sbyte _default;

    // Engine.pas:4025
    internal sbyte Get(int index)
    {
        sbyte result;
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

    // Engine.pas:4079
    internal void Put(int index, sbyte item)
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

    // Engine.pas:4099
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity < _count || newCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", newCapacity);
        }

        if (newCapacity != _capacity)
        {
            Array.Resize(ref _list, newCapacity * 1);
            _capacity = newCapacity;
        }
    }

    // Engine.pas:4110
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
            _list.AsSpan(_count, newCount - _count).Fill(_default);
        }

        _count = newCount;
    }

    // Engine.pas:4033
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

    // Engine.pas:3950
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:3955
    public override void Assign(Persistent source)
    {
        if (source is SByteList)
        {
            var sByteList = (SByteList)source;
            this._count = sByteList._count;
            this.SetCapacity(sByteList._count);
            Array.Copy(sByteList._list, 0, this._list, 0, this._count * 1);
            this._default = sByteList._default;
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
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            Array.Copy(_list, index + 1, _list, index, (_count - index) * 1);
        }
    }

    // Engine.pas:4003
    public void Exchange(int index1, int index2)
    {
        sbyte item;
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

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:4050
    public void Insert(int index, sbyte item)
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
            Array.Copy(_list, index, _list, index + 1, (_count - index) * 1);
        }

        _list[index] = item;
        _count += 1;
    }

    // Engine.pas:4061
    public sbyte Last() => Get(_count - 1);

    // Engine.pas:4066
    public void Move(int currentIndex, int newIndex)
    {
        sbyte item;
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
        for (int I = _count - 1; I >= 0; I--)
        {
            if (this[I] == _default)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:4147
    public void Sort(ShortIntSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortShortInteger(_list, 0, Count - 1, compare);
        }
    }

    // Engine.pas:4153
    public bool Compare(SByteList list) => list._count == _count && _list.AsSpan(0, _count).SequenceEqual(list._list.AsSpan(0, _count));

    // Engine.pas:4160
    public void LoadFromStream(Stream stream)
    {
        byte run = default;
        sbyte item = default;
        int count = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default, 1));
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (_count < count)
        {
            stream.ReadExactly(Bytes.Of(ref item, 1));
            if (item == _default)
            {
                stream.ReadExactly(Bytes.Of(ref run, 1));
                SetCount(_count + run);
            }
            else
            {
                Add(item);
            }
        }
    }

    // Engine.pas:4183
    public void SaveToStream(Stream stream)
    {
        int I;
        int run;
        byte B = default;
        int DefaultRun()
        {
            int J;
            int result = 0;
            J = I;
            while (J < _count && _list[J] == _default)
            {
                result += 1;
                J += 1;
            }

            return result;
        }

        I = 0;
        stream.Write(Bytes.Of(ref _default, 1));
        stream.Write(Bytes.Of(ref _count, 4));
        while (_count != I)
        {
            run = DefaultRun();
            if (run > 0)
            {
                while (run > 255)
                {
                    B = 255;
                    stream.Write(Bytes.Of(ref _default, 1));
                    stream.Write(Bytes.Of(ref B, 1));
                    run -= 255;
                    I += 255;
                }

                B = (byte)run;
                stream.Write(Bytes.Of(ref _default, 1));
                stream.Write(Bytes.Of(ref B, 1));
                I += run;
            }
            else
            {
                stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:4231
    public int RandomChanceItem(int bonus)
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
                chance = _list[I] + bonus;
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
            result = -1;
        }

        return result;
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }
    public sbyte Default { get => _default; set => _default = value; }

    public sbyte this[int index] { get => Get(index); set => Put(index, value); }

    public sbyte[] List { get => _list; }

    // Engine.pas:3992
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortShortInteger(Span<sbyte> sortList, int L, int R, ShortIntSortCompare sCompare)
    {
        sbyte b = 0;
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
                    b = sortList[number];
                    sortList[number] = sortList[number2];
                    sortList[number2] = b;
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
                QuickSortShortInteger(sortList, L, number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    public delegate int ShortIntSortCompare(sbyte Item1, sbyte Item2);

}
