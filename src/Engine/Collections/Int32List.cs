#nullable disable
// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Game;
using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Engine.Collections;

public class Int32List : Persistent
{
    private int[] _list = [];
    private int _count;
    private int _capacity;
    private int _default;

    // Engine.pas:3343
    public int Get(int index) => index < 0 || index >= _count ? _default : _list[index];

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
        if (newCount is < 0 or > EngineServices.MaxListSize)
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
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            result = $"{result}{IntToStr(_list[i])},";
        }

        return !IsEmpty(result) ? Copy(result, 1, result.Length - 1) : result;
    }

    // Engine.pas:3548
    internal void SetItemString(string S)
    {
        Clear();
        int p = Pos(",", S);
        while (!IsEmpty(S))
        {
            string item;
            if (p != 0)
            {
                item = Copy(S, 1, p - 1);
                S = DelphiStrings.Delete(S, 1, p);
            }
            else
            {
                item = S;
                S = "";
            }

            int v = !IsEmpty(item) ? StrToInt(item) : 0;

            _ = Add(v);
            p = Pos(",", S);
        }
    }

    // Engine.pas:3351
    protected internal virtual void Grow()
    {
        int delta = _capacity > 64 ? _capacity / 4 : _capacity > 8 ? 16 : 4;
        SetCapacity(_capacity + delta);
    }

    // Engine.pas:3268
    public override void Dispose() => Clear();

    // Engine.pas:3273
    public override void Assign(Persistent source)
    {
        if (source is Int32List int32List)
        {
            _count = int32List._count;
            SetCapacity(int32List._count);
            Array.Copy(int32List._list, 0, _list, 0, _count);
            _default = int32List._default;
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
        if (index1 < 0 || index1 >= _count)
        {
            Error("List index out of bounds (%d)", index1);
        }

        if (index2 < 0 || index2 >= _count)
        {
            Error("List index out of bounds (%d)", index2);
        }

        (_list[index2], _list[index1]) = (_list[index1], _list[index2]);
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

        return result == _count ? -1 : result;
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
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", newIndex);
            }

            int item = Get(currentIndex);
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
        for (int i = _count - 1; i >= 0; i--)
        {
            if (this[i] == _default)
            {
                Delete(i);
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
        for (int i = 1; i < Count; i++)
        {
            int item = _list[i];
            int j = i - 1;
            while (j >= 0 && compare(_list[j], item) > 0)
            {
                _list[j + 1] = _list[j];
                j -= 1;
            }

            _list[j + 1] = item;
        }
    }

    // Engine.pas:3471
    public bool Compare(Int32List list) => list._count == _count && _list.AsSpan(0, _count).SequenceEqual(list._list.AsSpan(0, _count));

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
                _ = Add(item);
            }
        }
    }

    // Engine.pas:3499
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
        stream.Write(Bytes.Of(ref _default, 4));
        stream.Write(Bytes.Of(ref _count, 4));
        while (_count != i)
        {
            int run = DefaultRun();
            if (run > 0)
            {
                stream.Write(Bytes.Of(ref _default, 4));
                stream.Write(Bytes.Of(ref run, 4));
                i += run;
            }
            else
            {
                stream.Write(MemoryMarshal.AsBytes(_list.AsSpan(i, 1)));
                i += 1;
            }
        }
    }

    // Engine.pas:3573
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
    public int Default { get => _default; set => _default = value; }

    public int this[int index] { get => Get(index); set => Put(index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public int[] List => _list;

    // Engine.pas:3310
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, Format(message, [data]));

    private static void QuickSortInteger(Span<int> sortList, int L, int R, IntegerSortCompare sCompare)
    {
        int i;
        do
        {
            i = L;
            int j = R;
            int p = sortList[(L + R) >>> 1];
            do
            {
                while (sCompare(sortList[i], p) < 0)
                {
                    i += 1;
                }

                while (sCompare(sortList[j], p) > 0)
                {
                    j -= 1;
                }

                if (i <= j)
                {
                    (sortList[j], sortList[i]) = (sortList[i], sortList[j]);
                    i += 1;
                    j -= 1;
                }
            }
            while (!(i > j));
            if (L < j)
            {
                QuickSortInteger(sortList, L, j, sCompare);
            }

            L = i;
        }
        while (!(i >= R));
    }

    internal static void FillInteger(Span<int> buffer, int count, int value) => EngineServices.Fill(buffer, count, value);

    internal static int FillWithRandomUnits(Int32List indexes, UnitCollection units, int level, int value, Set<UnitType> types)
    {
        bool CanUse(Unit unit)
        {
            bool condition = unit.UnitResource is not null;
            if (condition)
            {
                condition = types.Contains(unit.GetUnitType());
            }

            return condition && unit.UnitLevel <= level && units.CanAddUnit(unit);
        }

        Unit testUnit = new(null) { Holder = units.Holder };
        int result = 0;
        using (units.Holder.BeginUpdate())
        {
            int tries = 50;
            while (result < value && tries != 0)
            {
                tries -= 1;
                int index = indexes[Random(indexes.Count)];
                if (index != -1)
                {
                    testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                    if (CanUse(testUnit))
                    {
                        if (testUnit.GetObtainValue() + result > value)
                        {
                            int bestIndex = index;
                            int i = 20;
                            int difference = Math.Abs(testUnit.GetObtainValue() - (value - result));
                            do
                            {
                                index = indexes[Random(indexes.Count)];
                                if (index != -1 && index != bestIndex)
                                {
                                    testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                                    if (CanUse(testUnit))
                                    {
                                        int delta = Math.Abs(testUnit.GetObtainValue() - (value - result));
                                        if (delta < difference)
                                        {
                                            bestIndex = index;
                                            difference = delta;
                                        }
                                    }
                                }

                                i -= 1;
                            }
                            while (!(i == 0));
                            index = bestIndex;
                        }

                        Unit unit = new(null) { Holder = units.Holder };
                        unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                        _ = units.AddUnit(unit);
                        result += unit.GetObtainValue();
                        tries = 50;
                        _ = unit.Release();
                    }
                }
            }
        }
        _ = testUnit.Release();
        return result;
    }

    internal static int FillWithRandomUnitIndexes(Int32List indexes, Int32List units, int level, int value, Set<UnitType> types, GameEngine engine)
    {
        bool CanUse(Unit unit)
        {
            bool condition = unit.UnitResource is not null;
            if (condition)
            {
                condition = types.Contains(unit.GetUnitType());
            }

            return condition && unit.UnitLevel <= level;
        }

        Unit testUnit = new(null) { Holder = engine };
        int result = 0;
        int tries = 50;
        while (result < value && tries != 0)
        {
            tries -= 1;
            int index = indexes[Random(indexes.Count)];
            if (index != -1)
            {
                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                if (CanUse(testUnit))
                {
                    if (testUnit.GetObtainValue() + result > value)
                    {
                        int bestIndex = index;
                        int i = 20;
                        int difference = Math.Abs(testUnit.GetObtainValue() - (value - result));
                        do
                        {
                            index = indexes[Random(indexes.Count)];
                            if (index != -1 && index != bestIndex)
                            {
                                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(index));
                                if (CanUse(testUnit))
                                {
                                    int delta = Math.Abs(testUnit.GetObtainValue() - (value - result));
                                    if (delta < difference)
                                    {
                                        bestIndex = index;
                                        difference = delta;
                                    }
                                }
                            }

                            i -= 1;
                        }
                        while (!(i == 0));
                        index = bestIndex;
                    }

                    _ = units.Add(index);
                    result += GamePack.Current.UnitResources.GetUnitResource(index).ObtainValue;
                    tries = 50;
                }
            }
        }

        _ = testUnit.Release();
        return result;
    }

    internal static int LevelCost(Int32List costs, int level) => costs[level];

    public delegate int IntegerSortCompare(int Item1, int Item2);

}
