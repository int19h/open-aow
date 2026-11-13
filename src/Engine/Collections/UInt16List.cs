// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class UInt16List : Persistent
{
    private ushort[] _list = [];
    private int _count;
    private int _capacity;
    private ushort _default;

    // Engine.pas:4336
    internal ushort Get(int index)
    {
        ushort result;
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

    // Engine.pas:4390
    internal void Put(int index, ushort item)
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

    // Engine.pas:4410
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

    // Engine.pas:4421
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
            FillWord(_list.AsSpan(_count), newCount - _count, _default);
        }

        _count = newCount;
    }

    // Engine.pas:4344
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

    // Engine.pas:4261
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:4266
    public override void Assign(Persistent source)
    {
        if (source is UInt16List)
        {
            var uInt16List = (UInt16List)source;
            this._count = uInt16List._count;
            this.SetCapacity(uInt16List._count);
            Array.Copy(uInt16List._list, 0, this._list, 0, this._count);
            this._default = uInt16List._default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:4280
    public int Add(ushort item)
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

    // Engine.pas:4288
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:4294
    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            Array.Copy(_list, index + 1, _list, index, _count - index);
        }
    }

    // Engine.pas:4314
    public void Exchange(int index1, int index2)
    {
        ushort item;
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

    // Engine.pas:4325
    public UInt16List Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4331
    public ushort First() => Get(0);

    // Engine.pas:4354
    public int IndexOf(ushort item)
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

    // Engine.pas:4361
    public void Insert(int index, ushort item)
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
            Array.Copy(_list, index, _list, index + 1, _count - index);
        }

        _list[index] = item;
        _count += 1;
    }

    // Engine.pas:4372
    public ushort Last() => Get(_count - 1);

    // Engine.pas:4377
    public void Move(int currentIndex, int newIndex)
    {
        ushort item;
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

    // Engine.pas:4397
    public int Remove(ushort item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:4403
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

    // Engine.pas:4458
    public void Sort(WordSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortWord(_list, 0, Count - 1, compare);
        }
    }

    // Engine.pas:4464
    public bool Compare(UInt16List list) => list._count == _count && _list.AsSpan(0, _count).SequenceEqual(list._list.AsSpan(0, _count));

    // Engine.pas:4471
    public void LoadFromStream(Stream stream)
    {
        ushort item = default;
        ushort run = default;
        int count = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default, 2));
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (_count < count)
        {
            stream.ReadExactly(Bytes.Of(ref item, 2));
            if (item == _default)
            {
                stream.ReadExactly(Bytes.Of(ref run, 2));
                SetCount(_count + run);
            }
            else
            {
                Add(item);
            }
        }
    }

    // Engine.pas:4493
    public void SaveToStream(Stream stream)
    {
        int I;
        int run;
        ushort B = default;
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
        stream.Write(Bytes.Of(ref _default, 2));
        stream.Write(Bytes.Of(ref _count, 4));
        while (_count != I)
        {
            run = DefaultRun();
            if (run > 0)
            {
                while (run > 65535)
                {
                    B = 65535;
                    stream.Write(Bytes.Of(ref _default, 2));
                    stream.Write(Bytes.Of(ref B, 2));
                    // { sic: the original subtracts $FF here but advances I by $FFFF }
                    run -= 255;
                    I += 65535;
                }

                B = (ushort)run;
                stream.Write(Bytes.Of(ref _default, 2));
                stream.Write(Bytes.Of(ref B, 2));
                I += run;
            }
            else
            {
                stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:4541
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
    public ushort Default { get => _default; set => _default = value; }

    public ushort this[int index] { get => Get(index); set => Put(index, value); }

    public ushort[] List { get => _list; }

    // Engine.pas:4303
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortWord(Span<ushort> sortList, int L, int R, WordSortCompare sCompare)
    {
        ushort number = 0;
        int number2;
        do
        {
            number2 = L;
            int number3 = R;
            ushort item = sortList[L + R >>> 1];
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
                QuickSortWord(sortList, L, number3, sCompare);
            }
            L = number2;
        }
        while (number2 < R);
    }

    internal static void FillWord(Span<ushort> buffer, int count, ushort value)
    {
        EngineServices.Fill(buffer, count, value);
    }

    public delegate int WordSortCompare(ushort Item1, ushort Item2);

}
