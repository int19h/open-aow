// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Engine.Collections;

public class RecordList<TRecord> : Persistent where TRecord : struct
{
    internal TRecord[] _list = [];
    private int _count;
    private int _capacity;

    internal TRecord Get(int index) => index < 0 || index >= _count ? default : _list[index];

    internal void Put(int index, TRecord item)
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
            Array.Clear(_list, _count, newCount - _count);
        }
        else
        {
            Array.Clear(_list, newCount, _count - newCount);
        }

        _count = newCount;
    }

    protected internal virtual void Grow()
    {
        int delta;
        if (_capacity > 64)
        {
            delta = _capacity / 4;
        }
        else if (_capacity > 8)
        {
            delta = 16;
        }
        else
        {
            delta = 4;
        }

        SetCapacity(_capacity + delta);
    }

    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    public override void Assign(Persistent source)
    {
        if (source is RecordList<TRecord> list)
        {
            Clear();
            SetCapacity(list._count);
            Array.Copy(list._list, _list, list._count);
            _count = list._count;
        }
        else
        {
            base.Assign(source);
        }
    }

    public int Add(in TRecord item)
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

    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        Array.Copy(_list, index + 1, _list, index, _count - index);
        _list[_count] = default;
    }

    public void Exchange(int index1, int index2)
    {
        if (index1 < 0 || index1 >= _count)
        {
            Error("List index out of bounds (%d)", index1);
        }

        if (index2 < 0 || index2 >= _count)
        {
            Error("List index out of bounds (%d)", index2);
        }

        (_list[index1], _list[index2]) = (_list[index2], _list[index1]);
    }

    public TRecord First() => Get(0);

    public void Insert(int index, in TRecord item)
    {
        if (index < 0 || index > _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        if (_count == _capacity)
        {
            Grow();
        }

        Array.Copy(_list, index, _list, index + 1, _count - index);
        _list[index] = item;
        _count += 1;
    }

    public TRecord Last() => Get(_count - 1);

    public void Sort(RecordSortCompare<TRecord> compare)
    {
        if (_count > 0)
        {
            QuickSort(_list, 0, _count - 1, compare);
        }
    }

    // Engine.pas:5254, the quicksort of QuickSortDoubleItem and QuickSortQuadItem.
    private static void QuickSort(Span<TRecord> sortList, int L, int R, RecordSortCompare<TRecord> sCompare)
    {
        int I;
        int J;
        TRecord P;
        do
        {
            I = L;
            J = R;
            P = sortList[(int)(((uint)(L + R)) >> 1)];
            do
            {
                while (sCompare(sortList[I], P) < 0)
                {
                    I += 1;
                }

                while (sCompare(sortList[J], P) > 0)
                {
                    J -= 1;
                }

                if (I <= J)
                {
                    (sortList[I], sortList[J]) = (sortList[J], sortList[I]);
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSort(sortList, L, J, sCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }

    public TRecord this[int index] { get => Get(index); set => Put(index, value); }

    public TRecord[] List { get => _list; }

    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    // The record lists above with an object column. Engine.pas stored Integer(Object) in column A of a
    // TDoubleItemList or TQuadItemList and cast it back on read. These lists keep the reference itself,
    // so no id table holds the object and the owner of the list alone decides its lifetime. Every
    // column that held a number stays an int. Sort runs the quicksort of EngineUnit.QuickSortDoubleItem,
    // so a list sorted on its int columns keeps the original order.
    public delegate int RecordSortCompare<TItem>(in TItem Item1, in TItem Item2);
}
