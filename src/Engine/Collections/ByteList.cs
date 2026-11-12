// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class ByteList : Persistent
{
    private byte[] _list = [];
    private int _count;
    private int _capacity;
    private byte _default;

    // Engine.pas:3678
    internal byte Get(int index)
    {
        byte result;
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

    // Engine.pas:3732
    internal void Put(int index, byte item)
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

    // Engine.pas:3752
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

    // Engine.pas:3763
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
            FillBytes(_list.AsSpan(_count, newCount - _count), _default);
        }

        _count = newCount;
    }

    // Engine.pas:3883
    internal string GetItemString()
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

    // Engine.pas:3894
    internal void SetItemString(string S)
    {
        string item;
        int P;
        int V;
        Clear();
        P = Pos(",", S);
        while (!IsEmpty(S))
        {
            if (P != 0)
            {
                item = Copy(S, 1, P - 1);
                S = RtlUnit.Delete(S, 1, P);
            }
            else
            {
                item = S;
                S = "";
            }

            if (!IsEmpty(item))
            {
                V = StrToInt(item);
            }
            else
            {
                V = 0;
            }

            if (V > 255)
            {
                V = 0;
            }

            Add((byte)V);
            P = Pos(",", S);
        }
    }

    // Engine.pas:3686
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

    // Engine.pas:3603
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:3608
    public override void Assign(Persistent source)
    {
        if (source is ByteList)
        {
            var byteList = (ByteList)source;
            this._count = byteList._count;
            this.SetCapacity(byteList._count);
            Array.Copy(byteList._list, 0, this._list, 0, this._count * 1);
            this._default = byteList._default;
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
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            Array.Copy(_list, index + 1, _list, index, (_count - index) * 1);
        }
    }

    // Engine.pas:3656
    public void Exchange(int index1, int index2)
    {
        byte item;
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

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:3703
    public void Insert(int index, byte item)
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

    // Engine.pas:3714
    public byte Last() => Get(_count - 1);

    // Engine.pas:3719
    public void Move(int currentIndex, int newIndex)
    {
        byte item;
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
        for (int I = _count - 1; I >= 0; I--)
        {
            if (this[I] == _default)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:3800
    public void Sort(ByteSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortByte(_list, 0, Count - 1, compare);
        }
    }

    // Engine.pas:3806
    public bool Compare(ByteList list) => list._count == _count && CompareMem(_list.AsSpan(0, _count * 1), list._list.AsSpan(0, _count * 1), _count * 1);

    // Engine.pas:3813
    public void LoadFromStream(Stream stream)
    {
        byte item = default;
        byte run = default;
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

    // Engine.pas:3835
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
                stream.Write(_list.AsSpan(I, 1));
                I += 1;
            }
        }
    }

    // Engine.pas:3920
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
    public byte Default { get => _default; set => _default = value; }

    public byte this[int index] { get => Get(index); set => Put(index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public byte[] List { get => _list; }

    // Engine.pas:3645
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortByte(Span<byte> sortList, int L, int R, ByteSortCompare sCompare)
    {
        byte b = 0;
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
                QuickSortByte(sortList, L, number2, sCompare);
            }
            L = number;
        }
        while (number < R);
    }

    public delegate int ByteSortCompare(byte Item1, byte Item2);

}
