#nullable disable warnings
// Translated from Engine.pas.
using System.Globalization;
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Engine.Collections;

public class ByteList : Persistent, IReadOnlyList<byte>
{
    private byte[] _list = [];
    private int _count;
    private int _capacity;

    // Engine.pas:3678
    internal byte Get(int index) => index < 0 || index >= _count ? Default : _list[index];

    // Engine.pas:3732
    internal void Put(int index, byte item)
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

    // Engine.pas:3752
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

    // Engine.pas:3763
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
            _list.AsSpan(_count, length: newCount - _count).Fill(unchecked((byte)(int)Default));
        }

        _count = newCount;
    }

    // Engine.pas:3883
    internal string GetItemString()
    {
        string result = "";
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            result = $"{result}{((int)_list[i]).ToString(CultureInfo.InvariantCulture)},";
        }

        return !IsEmpty(result) ? Copy(result, index: 1, count: result.Length - 1) : result;
    }

    // Engine.pas:3894
    internal void SetItemString(string S)
    {
        Clear();
        int p = Pos(",", haystack: S);
        while (!IsEmpty(S))
        {
            string item;
            if (p != 0)
            {
                item = Copy(S, index: 1, count: p - 1);
                S = DelphiStrings.Delete(S, index: 1, count: p);
            }
            else
            {
                item = S;
                S = "";
            }

            int v = !IsEmpty(item) ? StrToInt(item) : 0;

            if (v > 255)
            {
                v = 0;
            }

            _ = Add((byte)v);
            p = Pos(",", haystack: S);
        }
    }

    // Engine.pas:3686
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // Engine.pas:3603
    public override void Dispose() => Clear();

    // Engine.pas:3608
    public override void Assign(Persistent? source)
    {
        if (source is ByteList byteList)
        {
            _count = byteList._count;
            SetCapacity(byteList._count);
            Array.Copy(byteList._list, sourceIndex: 0, destinationArray: _list, destinationIndex: 0, length: _count * 1);
            Default = byteList.Default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:3622
    public int Add(byte item)
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

    // Engine.pas:3630
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3636
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

    // Engine.pas:3656
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

    // Engine.pas:3667
    public ByteList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:3673
    public byte First() => Get(0);

    // Engine.pas:3696
    public int IndexOf(byte item)
    {
        int result = 0;
        while (result < _count && _list[result] != item)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:3703
    public void Insert(int index, byte item)
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

    // Engine.pas:3714
    public byte Last() => Get(_count - 1);

    // Engine.pas:3719
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", data: newIndex);
            }

            byte item = Get(currentIndex);
            Delete(currentIndex);
            Insert(newIndex, item);
        }
    }

    // Engine.pas:3739
    public int Remove(byte item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:3745
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

    // Engine.pas:3800
    public void Sort(Comparison<byte> compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortByte(_list, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    // Engine.pas:3806
    public bool Compare(ByteList list) => list._count == _count && _list.AsSpan(0, length: _count * 1).SequenceEqual(list._list.AsSpan(0, length: _count * 1));

    // Engine.pas:3920
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
    public byte Default { get; set; }

    public byte this[int index] { get => Get(index); set => Put(index, item: value); }

    public string ItemString => GetItemString();
    public byte[] List => _list;

    // Engine.pas:3645
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

    private static void QuickSortByte(Span<byte> sortList, int L, int R, Comparison<byte> sCompare)
    {
        int number;
        do
        {
            number = L;
            int number2 = R;
            byte item = sortList[L + R >>> 1];
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
                QuickSortByte(sortList, L, R: number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    // The values from the first to the last in index order, read through the indexer. Each step
    // reads the count again, as the index loop over a live count does.
    public IEnumerator<byte> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
