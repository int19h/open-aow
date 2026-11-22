#nullable disable warnings
// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Engine.Collections;

public class UInt16List : Persistent, IReadOnlyList<ushort>
{
    private ushort[] _list = [];
    private int _count;
    private int _capacity;
    private ushort _default;

    // Engine.pas:4336
    internal int Get(int index) => index < 0 || index >= _count ? _default : _list[index];

    // Engine.pas:4390
    internal void Put(int index, int item)
    {
        if (index < 0)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        if (index >= _count)
        {
            SetCount(index + 1);
        }

        _list[index] = (ushort)item;
    }

    // Engine.pas:4410
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

    // Engine.pas:4421
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
            FillWord(_list.AsSpan(_count), count: newCount - _count, value: _default);
        }

        _count = newCount;
    }

    // Engine.pas:4344
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // Engine.pas:4261
    public override void Dispose() => Clear();

    // Engine.pas:4266
    public override void Assign(Persistent? source)
    {
        if (source is UInt16List uInt16List)
        {
            _count = uInt16List._count;
            SetCapacity(uInt16List._count);
            Array.Copy(uInt16List._list, sourceIndex: 0, destinationArray: _list, destinationIndex: 0, length: _count);
            _default = uInt16List._default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:4280
    public int Add(int item)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        _list[result] = (ushort)item;
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
            Error("List index out of bounds (%d)", data: index);
        }

        _count -= 1;
        if (index < _count)
        {
            Array.Copy(_list, sourceIndex: index + 1, destinationArray: _list, destinationIndex: index, length: _count - index);
        }
    }

    // Engine.pas:4314
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
    public int First() => Get(0);

    // Engine.pas:4354
    public int IndexOf(ushort item)
    {
        int result = 0;
        while (result < _count && _list[result] != item)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:4361
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
            Array.Copy(_list, sourceIndex: index, destinationArray: _list, destinationIndex: index + 1, length: _count - index);
        }

        _list[index] = (ushort)item;
        _count += 1;
    }

    // Engine.pas:4372
    public int Last() => Get(_count - 1);

    // Engine.pas:4377
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

    // Engine.pas:4397
    public int Remove(int item)
    {
        int result = IndexOf((ushort)item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:4403
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

    // Engine.pas:4458
    public void Sort(Comparison<ushort> compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortWord(_list, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    // Engine.pas:4464
    public bool Compare(UInt16List list) => list._count == _count && _list.AsSpan(0, length: _count).SequenceEqual(list._list.AsSpan(0, length: _count));

    // Engine.pas:4471
    public void LoadFromStream(Stream stream)
    {
        ushort item = default;
        ushort run = default;
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

    // Engine.pas:4493
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
                ushort b;
                while (run > 65_535)
                {
                    b = 65_535;
                    stream.Write(Bytes.Of(ref _default));
                    stream.Write(Bytes.Of(ref b));
                    // { sic: the original subtracts $FF here but advances I by $FFFF }
                    run -= 255;
                    i += 65_535;
                }

                b = (ushort)run;
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

    // Engine.pas:4541
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
    public int Count { get => _count; set => SetCount(value); }
    public int Default { get => _default; set => _default = (ushort)value; }

    public ushort this[int index] { get => (ushort)Get(index); set => Put(index, item: value); }

    public ushort[] List => _list;

    // Engine.pas:4303
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

    private static void QuickSortWord(Span<ushort> sortList, int L, int R, Comparison<ushort> sCompare)
    {
        int number2;
        do
        {
            number2 = L;
            int number3 = R;
            int item = sortList[L + R >>> 1];
            while (true)
            {
                if (sCompare(sortList[number2], (ushort)item) < 0)
                {
                    number2++;
                    continue;
                }
                while (sCompare(sortList[number3], (ushort)item) > 0)
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
                QuickSortWord(sortList, L, R: number3, sCompare);
            }
            L = number2;
        }
        while (number2 < R);
    }

    internal static void FillWord(Span<ushort> buffer, int count, int value) => buffer[..count].Fill((ushort)value);

    // The values from the first to the last in index order, read through the indexer. Each step
    // reads the count again, as the index loop over a live count does.
    public IEnumerator<ushort> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
