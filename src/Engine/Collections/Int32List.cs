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
    internal int[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal int FDefault;

    // Engine.pas:3343
    public int Get(int Index)
    {
        int Result;
        if (Index < 0 || Index >= FCount)
        {
            Result = FDefault;
        }
        else
        {
            Result = FList[Index];
        }

        return Result;
    }

    // Engine.pas:3397
    // AoWE
    internal void Put(int Index, int Item)
    {
        if (Index < 0)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        FList[Index] = Item;
    }

    // Engine.pas:3417
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // Engine.pas:3428
    internal void SetCount(int NewCount)
    {
        if (NewCount < 0 || NewCount > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            FillInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
        }

        FCount = NewCount;
    }

    // Engine.pas:3537
    internal string GetItemString()
    {
        string Result = "";
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            Result = Result + IntToStr(FList[I]) + ",";
        }

        if (!IsEmpty(Result))
        {
            Result = Copy(Result, 1, Result.Length - 1);
        }

        return Result;
    }

    // Engine.pas:3548
    internal void SetItemString(string S)
    {
        string Item;
        int P;
        int V;
        Clear();
        P = Pos(",", S);
        while (!IsEmpty(S))
        {
            if (P != 0)
            {
                Item = Copy(S, 1, P - 1);
                S = RtlUnit.Delete(S, 1, P);
            }
            else
            {
                Item = S;
                S = "";
            }

            if (!IsEmpty(Item))
            {
                V = StrToInt(Item);
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
        int Delta;
        if (FCapacity > 64)
        {
            Delta = FCapacity / 4;
        }
        else
        {
            if (FCapacity > 8)
            {
                Delta = 16;
            }
            else
            {
                Delta = 4;
            }
        }

        SetCapacity(FCapacity + Delta);
    }

    // Engine.pas:3268
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:3273
    public override void Assign(Persistent Source)
    {
        if (Source is Int32List)
        {
            var integerList = (Int32List)Source;
            this.FCount = integerList.FCount;
            this.SetCapacity(integerList.FCount);
            Array.Copy(integerList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = integerList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:3287
    public int Add(int Item)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result] = Item;
        FCount += 1;
        return Result;
    }

    // Engine.pas:3295
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:3301
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            Array.Copy(FList, Index + 1, FList, Index, FCount - Index);
        }
    }

    // Engine.pas:3321
    public void Exchange(int Index1, int Index2)
    {
        int Item;
        if (Index1 < 0 || Index1 >= FCount)
        {
            Error("List index out of bounds (%d)", Index1);
        }

        if (Index2 < 0 || Index2 >= FCount)
        {
            Error("List index out of bounds (%d)", Index2);
        }

        Item = FList[Index1];
        FList[Index1] = FList[Index2];
        FList[Index2] = Item;
    }

    // Engine.pas:3332
    public Int32List Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:3338
    public int First() => Get(0);

    // Engine.pas:3361
    public int IndexOf(int Item)
    {
        int Result = 0;
        while (Result < FCount && FList[Result] != Item)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:3368
    public void Insert(int Index, int Item)
    {
        if (Index < 0 || Index > FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (FCount == FCapacity)
        {
            Grow();
        }

        if (Index < FCount)
        {
            Array.Copy(FList, Index, FList, Index + 1, FCount - Index);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:3379
    public int Last() => Get(FCount - 1);

    // Engine.pas:3384
    public void Move(int CurIndex, int NewIndex)
    {
        int Item;
        if (CurIndex != NewIndex)
        {
            if (NewIndex < 0 || NewIndex >= FCount)
            {
                Error("List index out of bounds (%d)", NewIndex);
            }

            Item = Get(CurIndex);
            Delete(CurIndex);
            Insert(NewIndex, Item);
        }
    }

    // Engine.pas:3404
    public int Remove(int Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:3410
    public void Pack()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (this[I] == FDefault)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:3465
    public void Sort(IntegerSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortInteger(FList, 0, Count - 1, Compare);
        }
    }

    // Keeps the order of equal items. HeroGrid.SortHeroes is the only caller.
    public void SortStable(IntegerSortCompare Compare)
    {
        for (int I = 1; I < Count; I++)
        {
            int Item = FList[I];
            int J = I - 1;
            while (J >= 0 && Compare(FList[J], Item) > 0)
            {
                FList[J + 1] = FList[J];
                J -= 1;
            }

            FList[J + 1] = Item;
        }
    }

    // Engine.pas:3471
    public bool Compare(Int32List List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

    // Engine.pas:3478
    public void LoadFromStream(Stream Stream)
    {
        int Count = default;
        int Item = default;
        int Run = default;
        Clear();
        Stream.ReadExactly(Bytes.Of(ref FDefault, 4));
        Stream.ReadExactly(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (Count != FCount)
        {
            Stream.ReadExactly(Bytes.Of(ref Item, 4));
            if (Item == FDefault)
            {
                Stream.ReadExactly(Bytes.Of(ref Run, 4));
                SetCount(FCount + Run);
            }
            else
            {
                Add(Item);
            }
        }
    }

    // Engine.pas:3499
    public void SaveToStream(Stream Stream)
    {
        int I;
        int Run = default;
        int DefaultRun()
        {
            int J;
            int Result = 0;
            J = I;
            while (J < FCount && FList[J] == FDefault)
            {
                Result += 1;
                J += 1;
            }

            return Result;
        }

        I = 0;
        Stream.Write(Bytes.Of(ref FDefault, 4));
        Stream.Write(Bytes.Of(ref FCount, 4));
        while (FCount != I)
        {
            Run = DefaultRun();
            if (Run > 0)
            {
                Stream.Write(Bytes.Of(ref FDefault, 4));
                Stream.Write(Bytes.Of(ref Run, 4));
                I += Run;
            }
            else
            {
                Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                I += 1;
            }
        }
    }

    // Engine.pas:3573
    public int RandomChanceItem(int Bonus)
    {
        int Result;
        int Total;
        int Chance;
        int R;
        if (FCount != 0)
        {
            Total = 0;
            for (int I = FCount - 1; I >= 0; I--)
            {
                Chance = FList[I] + Bonus;
                if (Chance < 0)
                {
                    Chance = 0;
                }

                Total += Chance;
            }

            R = Random(Total);
            Total = 0;
            Result = 0;
            while (Total < R)
            {
                Chance = FList[Result] + Bonus;
                if (Chance < 0)
                {
                    Chance = 0;
                }

                Total += Chance;
                Result += 1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public int Default { get => FDefault; set => FDefault = value; }

    public int this[int Index] { get => Get(Index); set => Put(Index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public int[] List { get => FList; }

    // Engine.pas:3310
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortInteger(Span<int> SortList, int L, int R, IntegerSortCompare SCompare)
    {
        int I;
        int J;
        int P;
        int T = default;
        do
        {
            I = L;
            J = R;
            P = SortList[(int)(((uint)(L + R)) >> 1)];
            do
            {
                while (SCompare(SortList[I], P) < 0)
                {
                    I += 1;
                }

                while (SCompare(SortList[J], P) > 0)
                {
                    J -= 1;
                }

                if (I <= J)
                {
                    T = SortList[I];
                    SortList[I] = SortList[J];
                    SortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortInteger(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    internal static void FillInteger(Span<int> buffer, int count, int value) => EngineServices.Fill(buffer, count, value);

    internal static int FillWithRandomUnits(Int32List Indexes, UnitList Units, int Level, int Value, Set<UnitType> Types)
    {
        Unit TestUnit;
        int Index;
        int Tries;
        int I;
        int Difference;
        int BestIndex;
        int Delta;
        Unit Unit_;
        bool CanUse(Unit Unit_)
        {
            bool condition = Unit_.FUnitResource != null;
            if (condition)
            {
                var unitType = Unit_.GetUnitType();
                condition = Types.Contains(unitType);
            }

            return condition && Unit_.GetUnitLevel() <= Level && Units.CanAddUnit(Unit_);
        }

        TestUnit = new Unit(null);
        int Result = 0;
        Tries = 50;
        Units.BeginUpdate();
        while (Result < Value && Tries != 0)
        {
            Tries -= 1;
            Index = Indexes[Random(Indexes.Count)];
            if (Index != -1)
            {
                TestUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(Index));
                if (CanUse(TestUnit))
                {
                    if (TestUnit.GetObtainValue() + Result > Value)
                    {
                        BestIndex = Index;
                        I = 20;
                        Difference = Math.Abs(TestUnit.GetObtainValue() - (Value - Result));
                        do
                        {
                            Index = Indexes[Random(Indexes.Count)];
                            if (Index != -1 && Index != BestIndex)
                            {
                                TestUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(Index));
                                if (CanUse(TestUnit))
                                {
                                    Delta = Math.Abs(TestUnit.GetObtainValue() - (Value - Result));
                                    if (Delta < Difference)
                                    {
                                        BestIndex = Index;
                                        Difference = Delta;
                                    }
                                }
                            }

                            I -= 1;
                        }
                        while (!(I == 0));
                        Index = BestIndex;
                    }

                    Unit_ = new Unit(null);
                    Unit_.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(Index));
                    Units.AddUnit(Unit_);
                    Result += Unit_.GetObtainValue();
                    Tries = 50;
                    Unit_.Release();
                }
            }
        }

        Units.EndUpdate();
        TestUnit.Release();
        return Result;
    }

    internal static int FillWithRandomUnitIndexes(Int32List Indexes, Int32List Units, int Level, int Value, Set<UnitType> Types)
    {
        Unit TestUnit;
        int Index;
        int Tries;
        int I;
        int Difference;
        int BestIndex;
        int Delta;
        bool CanUse(Unit Unit_)
        {
            bool condition = Unit_.FUnitResource != null;
            if (condition)
            {
                var unitType = Unit_.GetUnitType();
                condition = Types.Contains(unitType);
            }

            return condition && Unit_.GetUnitLevel() <= Level;
        }

        TestUnit = new Unit(null);
        int Result = 0;
        Tries = 50;
        while (Result < Value && Tries != 0)
        {
            Tries -= 1;
            Index = Indexes[Random(Indexes.Count)];
            if (Index != -1)
            {
                TestUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(Index));
                if (CanUse(TestUnit))
                {
                    if (TestUnit.GetObtainValue() + Result > Value)
                    {
                        BestIndex = Index;
                        I = 20;
                        Difference = Math.Abs(TestUnit.GetObtainValue() - (Value - Result));
                        do
                        {
                            Index = Indexes[Random(Indexes.Count)];
                            if (Index != -1 && Index != BestIndex)
                            {
                                TestUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(Index));
                                if (CanUse(TestUnit))
                                {
                                    Delta = Math.Abs(TestUnit.GetObtainValue() - (Value - Result));
                                    if (Delta < Difference)
                                    {
                                        BestIndex = Index;
                                        Difference = Delta;
                                    }
                                }
                            }

                            I -= 1;
                        }
                        while (!(I == 0));
                        Index = BestIndex;
                    }

                    Units.Add(Index);
                    Result += AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FObtainValue;
                    Tries = 50;
                }
            }
        }

        TestUnit.Release();
        return Result;
    }

    internal static int P4LevelCost(Int32List Costs, int Level) => Costs[Level];

    public delegate int IntegerSortCompare(int Item1, int Item2);

}
