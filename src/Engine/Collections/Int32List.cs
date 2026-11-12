// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class Int32List : Persistent
{
    private int[] _list = [];
    private int _count;
    private int _capacity;
    private int _default;

    // Engine.pas:3343
    public int Get(int index)
    {
        int result;
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

    // Engine.pas:3397
    // AoWE
    internal void Put(int index, int item)
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

    // Engine.pas:3417
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

    // Engine.pas:3428
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
            FillInteger(_list.AsSpan(_count), newCount - _count, _default);
        }

        _count = newCount;
    }

    // Engine.pas:3537
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

    // Engine.pas:3548
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

            Add(V);
            P = Pos(",", S);
        }
    }

    // Engine.pas:3351
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

    // Engine.pas:3268
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:3273
    public override void Assign(Persistent source)
    {
        if (source is Int32List)
        {
            var int32List = (Int32List)source;
            this._count = int32List._count;
            this.SetCapacity(int32List._count);
            Array.Copy(int32List._list, 0, this._list, 0, this._count);
            this._default = int32List._default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:3287
    public int Add(int item)
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

    // Engine.pas:3295
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3301
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

    // Engine.pas:3321
    public void Exchange(int index1, int index2)
    {
        int item;
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

    // Engine.pas:3332
    public Int32List Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:3338
    public int First() => Get(0);

    // Engine.pas:3361
    public int IndexOf(int item)
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

    // Engine.pas:3368
    public void Insert(int index, int item)
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

    // Engine.pas:3379
    public int Last() => Get(_count - 1);

    // Engine.pas:3384
    public void Move(int currentIndex, int newIndex)
    {
        int item;
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

    // Engine.pas:3404
    public int Remove(int item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:3410
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

    // Engine.pas:3465
    public void Sort(IntegerSortCompare compare)
    {
        if (_list.Length != 0 && Count > 0)
        {
            QuickSortInteger(_list, 0, Count - 1, compare);
        }
    }

    // Keeps the order of equal items. HeroGrid.SortHeroes is the only caller.
    public void SortStable(IntegerSortCompare compare)
    {
        for (int I = 1; I < Count; I++)
        {
            int item = _list[I];
            int J = I - 1;
            while (J >= 0 && compare(_list[J], item) > 0)
            {
                _list[J + 1] = _list[J];
                J -= 1;
            }

            _list[J + 1] = item;
        }
    }

    // Engine.pas:3471
    public bool Compare(Int32List list) => list._count == _count && CompareMem(MemoryMarshal.AsBytes(_list.AsSpan(0, _count)), MemoryMarshal.AsBytes(list._list.AsSpan(0, _count)), _count * 4);

    // Engine.pas:3478
    public void LoadFromStream(Stream stream)
    {
        int count = default;
        int item = default;
        int run = default;
        Clear();
        stream.ReadExactly(Bytes.Of(ref _default, 4));
        stream.ReadExactly(Bytes.Of(ref count, 4));
        SetCapacity(count);
        while (count != _count)
        {
            stream.ReadExactly(Bytes.Of(ref item, 4));
            if (item == _default)
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

    // Engine.pas:3499
    public void SaveToStream(Stream stream)
    {
        int I;
        int run = default;
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

    // Engine.pas:3573
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
    public int Default { get => _default; set => _default = value; }

    public int this[int index] { get => Get(index); set => Put(index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public int[] List { get => _list; }

    // Engine.pas:3310
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

    private static void QuickSortInteger(Span<int> sortList, int L, int R, IntegerSortCompare sCompare)
    {
        int I;
        int J;
        int P;
        int T = default;
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
                QuickSortInteger(sortList, L, J, sCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    internal static void FillInteger(Span<int> buffer, int count, int value) => EngineServices.Fill(buffer, count, value);

    internal static int FillWithRandomUnits(Int32List indexes, UnitList units, int level, int value, Set<UnitType> types)
    {
        Unit testUnit;
        int index;
        int tries;
        int I;
        int difference;
        int bestIndex;
        int delta;
        Unit unit;
        bool CanUse(Unit unit)
        {
            bool condition = unit.UnitResource != null;
            if (condition)
            {
                var unitType = unit.GetUnitType();
                condition = types.Contains(unitType);
            }

            return condition && unit.GetUnitLevel() <= level && units.CanAddUnit(unit);
        }

        testUnit = new Unit(null);
        int result = 0;
        tries = 50;
        units.BeginUpdate();
        while (result < value && tries != 0)
        {
            tries -= 1;
            index = indexes[Random(indexes.Count)];
            if (index != -1)
            {
                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                if (CanUse(testUnit))
                {
                    if (testUnit.GetObtainValue() + result > value)
                    {
                        bestIndex = index;
                        I = 20;
                        difference = Math.Abs(testUnit.GetObtainValue() - (value - result));
                        do
                        {
                            index = indexes[Random(indexes.Count)];
                            if (index != -1 && index != bestIndex)
                            {
                                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                                if (CanUse(testUnit))
                                {
                                    delta = Math.Abs(testUnit.GetObtainValue() - (value - result));
                                    if (delta < difference)
                                    {
                                        bestIndex = index;
                                        difference = delta;
                                    }
                                }
                            }

                            I -= 1;
                        }
                        while (!(I == 0));
                        index = bestIndex;
                    }

                    unit = new Unit(null);
                    unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                    units.AddUnit(unit);
                    result += unit.GetObtainValue();
                    tries = 50;
                    unit.Release();
                }
            }
        }

        units.EndUpdate();
        testUnit.Release();
        return result;
    }

    internal static int FillWithRandomUnitIndexes(Int32List indexes, Int32List units, int level, int value, Set<UnitType> types)
    {
        Unit testUnit;
        int index;
        int tries;
        int I;
        int difference;
        int bestIndex;
        int delta;
        bool CanUse(Unit unit)
        {
            bool condition = unit.UnitResource != null;
            if (condition)
            {
                var unitType = unit.GetUnitType();
                condition = types.Contains(unitType);
            }

            return condition && unit.GetUnitLevel() <= level;
        }

        testUnit = new Unit(null);
        int result = 0;
        tries = 50;
        while (result < value && tries != 0)
        {
            tries -= 1;
            index = indexes[Random(indexes.Count)];
            if (index != -1)
            {
                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                if (CanUse(testUnit))
                {
                    if (testUnit.GetObtainValue() + result > value)
                    {
                        bestIndex = index;
                        I = 20;
                        difference = Math.Abs(testUnit.GetObtainValue() - (value - result));
                        do
                        {
                            index = indexes[Random(indexes.Count)];
                            if (index != -1 && index != bestIndex)
                            {
                                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                                if (CanUse(testUnit))
                                {
                                    delta = Math.Abs(testUnit.GetObtainValue() - (value - result));
                                    if (delta < difference)
                                    {
                                        bestIndex = index;
                                        difference = delta;
                                    }
                                }
                            }

                            I -= 1;
                        }
                        while (!(I == 0));
                        index = bestIndex;
                    }

                    units.Add(index);
                    result += GamePack.Current.UnitResources.GetUnitResource(index).ObtainValue;
                    tries = 50;
                }
            }
        }

        testUnit.Release();
        return result;
    }

    internal static int LevelCost(Int32List costs, int level) => costs[level];

    public delegate int IntegerSortCompare(int Item1, int Item2);

}
