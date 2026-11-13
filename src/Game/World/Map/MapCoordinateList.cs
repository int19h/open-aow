// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// Engine's value list (Classes.TList adapted to a value type, with a default item for new and
// out-of-range items and a run-length encoded stream form) for map locations
public partial class MapCoordinateList : Persistent
{
    private MapCoordinate[] _list = [];
    private int _count;
    private int _capacity;
    internal MapCoordinate _default;

    // HSEngine.pas:1673
    internal MapCoordinate Get(int index)
    {
        MapCoordinate result;
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

    // HSEngine.pas:1746
    internal void Put(int index, MapCoordinate item)
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
            _list.AsSpan(_count, newCount - _count).Fill(_default);
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

    // HSEngine.pas:1578
    public MapCoordinateList()
    {
        _default.X = -1;
        _default.Y = -1;
        _default.MapLevel = -1;
        _default.Tag = 0;
    }

    // HSEngine.pas:1587
    public override void Dispose()
    {
        Clear();
    }

    // HSEngine.pas:1592
    public override void Assign(Persistent source)
    {
        if (source is MapCoordinateList)
        {
            var mapCoordinateList = (MapCoordinateList)source;
            this._count = mapCoordinateList._count;
            this.SetCapacity(mapCoordinateList._capacity);
            Array.Copy(mapCoordinateList._list, 0, this._list, 0, this._count);
            this._default = mapCoordinateList._default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // HSEngine.pas:1606
    public int Add(MapCoordinate item)
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

    // HSEngine.pas:1614
    public int AddMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        _list[result].X = X;
        _list[result].Y = Y;
        _list[result].MapLevel = L;
        _list[result].Tag = 0;
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
        MapCoordinate item;
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

    // HSEngine.pas:1662
    public MapCoordinateList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // HSEngine.pas:1668
    public MapCoordinate First() => Get(0);

    // HSEngine.pas:1691
    public int IndexOf(MapCoordinate item)
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

    // HSEngine.pas:1698
    public int IndexOfXY(sbyte X, sbyte Y)
    {
        int result = 0;
        while (result < _count && (_list[result].X != X || _list[result].Y != Y))
        {
            result += 1;
        }

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:1709
    public int IndexOfMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        int result = 0;
        while (result < _count && (_list[result].X != X || _list[result].Y != Y || _list[result].MapLevel != L))
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
    public void Insert(int index, MapCoordinate item)
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
    public MapCoordinate Last() => Get(_count - 1);

    // HSEngine.pas:1733
    public void Move(int currentIndex, int newIndex)
    {
        MapCoordinate item;
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

    // HSEngine.pas:1753
    public int Remove(MapCoordinate item)
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
            if (this[I] == _default)
            {
                Delete(I);
            }
        }
    }

    // HSEngine.pas:1815
    public void Sort(XYLSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortMapCoordinate(_list, 0, Count - 1, compare);
        }
    }

    // HSEngine.pas:1821
    public bool Compare(MapCoordinateList list) => list._count == _count && _list.AsSpan(0, _count).SequenceEqual(list._list.AsSpan(0, _count));

    // HSEngine.pas:1828
    public void LoadFromStream(Stream stream)
    {
        int count = default;
        int run = default;
        MapCoordinate item = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default, 4));
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (count != _count)
        {
            stream.ReadExactly(Bytes.Of(ref item, 4));
            if (Unsafe.As<MapCoordinate, int>(ref item) == Unsafe.As<MapCoordinate, int>(ref _default))
            {
                stream.ReadExactly(Bytes.Of(ref run, 4));
                SetCount(_count + run);
            }
            else
            {
                Add(item);
            }
        }
    }

    // HSEngine.pas:1850
    public void SaveToStream(Stream stream)
    {
        int I;
        int run = default;
        int DefaultRun()
        {
            int J;
            int result = 0;
            J = I;
            while (J < _count && Unsafe.As<MapCoordinate, int>(ref _list[J]) == Unsafe.As<MapCoordinate, int>(ref _default))
            {
                result += 1;
                J += 1;
            }

            return result;
        }

        I = 0;
        stream.Write(Bytes.Of(ref _default, 4));
        stream.Write(Bytes.Of(ref _count, 4));
        while (_count != I)
        {
            run = DefaultRun();
            if (run > 0)
            {
                stream.Write(Bytes.Of(ref _default, 4));
                stream.Write(Bytes.Of(ref run, 4));
                I += run;
            }
            else
            {
                stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; set => SetCount(value); }
    public MapCoordinate Default { get => _default; set => _default = value; }

    public MapCoordinate this[int index] { get => Get(index); set => Put(index, value); }

    public MapCoordinate[] List { get => _list; }

    // HSEngine.pas:1640
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortMapCoordinate(Span<MapCoordinate> sortList, int L, int R, XYLSortCompare sCompare)
    {
        int I;
        int J;
        MapCoordinate P;
        MapCoordinate T = default;
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
                    T = sortList[I];
                    sortList[I] = sortList[J];
                    sortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortMapCoordinate(sortList, L, J, sCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    internal static bool LocationsConnected(MapCoordinateList list)
    {
        bool result = default;
        MapCoordinateList locations;
        MapCoordinate mapCoordinate = default;
        void Connect(MapCoordinate mapCoordinate)
        {
            HexNumberWalkData walk = default;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, mapCoordinate.X, mapCoordinate.Y, 1);
            while (walk.Radius <= 1)
            {
                for (int I = 0, last = locations.Count - 1; I <= last; I++)
                {
                    if (locations[I].Tag == 0)
                    {
                        var item = locations[I];
                        var packMapCoordinate = HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, mapCoordinate.MapLevel);
                        if (item == packMapCoordinate)
                        {
                            locations._list[I].Tag = 1;
                            Connect(locations[I]);
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        locations = new MapCoordinateList();
        for (int I = 0, last = list.Count - 1; I <= last; I++)
        {
            mapCoordinate = list[I];
            mapCoordinate.Tag = 0;
            if (locations.IndexOf(mapCoordinate) == -1)
            {
                locations.Add(mapCoordinate);
            }
        }

        if (locations.Count > 0)
        {
            if (locations.Count > 1)
            {
                locations._list[0].Tag = 1;
                Connect(locations[0]);
                result = true;
                for (int I = 0, last = locations.Count - 1; I <= last; I++)
                {
                    if (locations[I].Tag != 1)
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        locations?.Dispose();
        return result;
    }

    public delegate int XYLSortCompare(MapCoordinate Item1, MapCoordinate Item2);

}
