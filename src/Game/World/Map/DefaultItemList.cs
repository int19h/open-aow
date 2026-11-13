// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// Engine's value list (Classes.TList adapted to a value type, with a default item for new and
// out-of-range items), the list code of TXYLList. MapCoordinateList holds locations, PathStepList
// the steps of a path and UnitPositionControl the unit positions of a combat map.
public class DefaultItemList<TItem> : Persistent where TItem : struct, IEquatable<TItem>
{
    private TItem[] _list = [];
    private int _count;
    private int _capacity;

    // HSEngine.pas:1673
    internal TItem Get(int index) => index < 0 || index >= _count ? Default : _list[index];

    // HSEngine.pas:1746
    internal void Put(int index, TItem item)
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

    // HSEngine.pas:1767
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

    // HSEngine.pas:1778
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
            _list.AsSpan(_count, newCount - _count).Fill(Default);
        }

        _count = newCount;
    }

    // HSEngine.pas:1681
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

    // HSEngine.pas:1587
    public override void Dispose()
    {
        Clear();
    }

    // HSEngine.pas:1592
    public override void Assign(Persistent source)
    {
        if (source is DefaultItemList<TItem> list)
        {
            _count = list._count;
            SetCapacity(list._capacity);
            Array.Copy(list._list, 0, _list, 0, _count);
            Default = list.Default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // HSEngine.pas:1606
    public int Add(TItem item)
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

    // HSEngine.pas:1625
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // HSEngine.pas:1631
    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            _list.AsSpan(index + 1, _count - index).CopyTo(_list.AsSpan(index, _count - index));
        }
    }

    // HSEngine.pas:1651
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

    // HSEngine.pas:1668
    public TItem First() => Get(0);

    // HSEngine.pas:1691
    public int IndexOf(TItem item)
    {
        int result = 0;
        while (result < _count && !_list[result].Equals(item))
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:1717
    public void Insert(int index, TItem item)
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
            _list.AsSpan(index, _count - index).CopyTo(_list.AsSpan(index + 1, _count - index));
        }

        _list[index] = item;
        _count += 1;
    }

    // HSEngine.pas:1728
    public TItem Last() => Get(_count - 1);

    // HSEngine.pas:1733
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", newIndex);
            }

            var item = Get(currentIndex);
            Delete(currentIndex);
            Insert(newIndex, item);
        }
    }

    // HSEngine.pas:1753
    public int Remove(TItem item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // HSEngine.pas:1759
    public void Pack()
    {
        for (int I = _count - 1; I >= 0; I--)
        {
            if (this[I].Equals(Default))
            {
                Delete(I);
            }
        }
    }

    // HSEngine.pas:1821
    public bool Compare(DefaultItemList<TItem> list) => list._count == _count && _list.AsSpan(0, _count).SequenceEqual(list._list.AsSpan(0, _count));

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }

    // The item that new and out-of-range positions read.
    public TItem Default { get; set; }

    public TItem this[int index] { get => Get(index); set => Put(index, value); }

    public TItem[] List { get => _list; }

    // HSEngine.pas:1640
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }
}
