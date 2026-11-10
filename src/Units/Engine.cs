// Translated from Engine.pas.
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.GlobalTarget;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Nodes
{

    public enum EControlStyleElement : byte
    {
        SaveLoad = 0,
        FixedChild = 1,
        VerifyFixedChildren = 2,
        ArrayMode = 3
    }
}

namespace Aow.Game.Engine
{

    public enum EngineUserElement : byte
    {
        Programmer = 0,
        Designer = 1,
        User = 2
    }

    public enum EngineModeElement : byte
    {
        VerifyEObjects = 0
    }
}

namespace Aow.Game.Engine.Storage
{

    public enum StorageModeElement : byte
    {
        Read = 0,
        Write = 1,
        TextOnly = 2,
        Preview = 3,
        Translate = 4,
        SetupCompare = 5,
        Compare = 6
    }
}

namespace Aow.Game.Engine.Collections
{

    public partial class IntegerList : Persistent
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
            if (Source is IntegerList)
            {
                var integerList = (IntegerList)Source;
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
        public IntegerList Expand()
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
        public bool Compare(IntegerList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

        // Engine.pas:3478
        public void LoadFromStream(Stream Stream)
        {
            int Count = default;
            int Item = default;
            int Run = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 4));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (Count != FCount)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 4));
                if (Item == FDefault)
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 4));
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

        internal static int FillWithRandomUnits(IntegerList Indexes, UnitList Units, int Level, int Value, Set<UnitType> Types)
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

        internal static int FillWithRandomUnitIndexes(IntegerList Indexes, IntegerList Units, int Level, int Value, Set<UnitType> Types)
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

        internal static int P4LevelCost(IntegerList Costs, int Level) => Costs[Level];

        public delegate int IntegerSortCompare(int Item1, int Item2);

    }

    public partial class ByteList : Persistent
    {
        internal byte[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal byte FDefault;

        // Engine.pas:3678
        internal byte Get(int Index)
        {
            byte Result;
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

        // Engine.pas:3732
        internal void Put(int Index, byte Item)
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

        // Engine.pas:3752
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
            {
                Error("List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                Array.Resize(ref FList, NewCapacity * 1);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:3763
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
                FillBytes(FList.AsSpan(FCount, NewCount - FCount), FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:3883
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

        // Engine.pas:3894
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

        // Engine.pas:3603
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:3608
        public override void Assign(Persistent Source)
        {
            if (Source is ByteList)
            {
                var byteList = (ByteList)Source;
                this.FCount = byteList.FCount;
                this.SetCapacity(byteList.FCount);
                Array.Copy(byteList.FList, 0, this.FList, 0, this.FCount * 1);
                this.FDefault = byteList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:3622
        public int Add(byte Item)
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

        // Engine.pas:3630
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:3636
        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Array.Copy(FList, Index + 1, FList, Index, (FCount - Index) * 1);
            }
        }

        // Engine.pas:3656
        public void Exchange(int Index1, int Index2)
        {
            byte Item;
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

        // Engine.pas:3667
        public ByteList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:3673
        public byte First() => Get(0);

        // Engine.pas:3696
        public int IndexOf(byte Item)
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

        // Engine.pas:3703
        public void Insert(int Index, byte Item)
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
                Array.Copy(FList, Index, FList, Index + 1, (FCount - Index) * 1);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:3714
        public byte Last() => Get(FCount - 1);

        // Engine.pas:3719
        public void Move(int CurIndex, int NewIndex)
        {
            byte Item;
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

        // Engine.pas:3739
        public int Remove(byte Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:3745
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

        // Engine.pas:3800
        public void Sort(ByteSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                QuickSortByte(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:3806
        public bool Compare(ByteList List) => List.FCount == FCount && CompareMem(FList.AsSpan(0, FCount * 1), List.FList.AsSpan(0, FCount * 1), FCount * 1);

        // Engine.pas:3813
        public void LoadFromStream(Stream Stream)
        {
            byte Item = default;
            byte Run = default;
            int Count = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 1));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 1));
                if (Item == FDefault)
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 1));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:3835
        public void SaveToStream(Stream Stream)
        {
            int I;
            int Run;
            byte B = default;
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
            Stream.Write(Bytes.Of(ref FDefault, 1));
            Stream.Write(Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    while (Run > 255)
                    {
                        B = 255;
                        Stream.Write(Bytes.Of(ref FDefault, 1));
                        Stream.Write(Bytes.Of(ref B, 1));
                        Run -= 255;
                        I += 255;
                    }

                    B = (byte)Run;
                    Stream.Write(Bytes.Of(ref FDefault, 1));
                    Stream.Write(Bytes.Of(ref B, 1));
                    I += Run;
                }
                else
                {
                    Stream.Write(FList.AsSpan(I, 1));
                    I += 1;
                }
            }
        }

        // Engine.pas:3920
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
        public byte Default { get => FDefault; set => FDefault = value; }

        public byte this[int Index] { get => Get(Index); set => Put(Index, value); }

        public string ItemString { get => GetItemString(); set => SetItemString(value); }
        public byte[] List { get => FList; }

        // Engine.pas:3645
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        private static void QuickSortByte(Span<byte> SortList, int L, int R, ByteSortCompare SCompare)
        {
            byte b = 0;
            int num;
            do
            {
                num = L;
                int num2 = R;
                byte item = SortList[L + R >>> 1];
                while (true)
                {
                    if (SCompare(SortList[num], item) < 0)
                    {
                        num++;
                        continue;
                    }
                    while (SCompare(SortList[num2], item) > 0)
                    {
                        num2--;
                    }
                    if (num <= num2)
                    {
                        b = SortList[num];
                        SortList[num] = SortList[num2];
                        SortList[num2] = b;
                        num++;
                        num2--;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                }
                if (L < num2)
                {
                    QuickSortByte(SortList, L, num2, SCompare);
                }
                L = num;
            }
            while (num < R);
        }

        public delegate int ByteSortCompare(byte Item1, byte Item2);

    }

    public partial class ShortIntList : Persistent
    {
        internal sbyte[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal sbyte FDefault;

        // Engine.pas:4025
        internal sbyte Get(int Index)
        {
            sbyte Result;
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

        // Engine.pas:4079
        internal void Put(int Index, sbyte Item)
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

        // Engine.pas:4099
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
            {
                Error("List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                Array.Resize(ref FList, NewCapacity * 1);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:4110
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
                FillBytes(MemoryMarshal.AsBytes(FList.AsSpan(FCount, NewCount - FCount)), FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:4033
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

        // Engine.pas:3950
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:3955
        public override void Assign(Persistent Source)
        {
            if (Source is ShortIntList)
            {
                var shortIntList = (ShortIntList)Source;
                this.FCount = shortIntList.FCount;
                this.SetCapacity(shortIntList.FCount);
                Array.Copy(shortIntList.FList, 0, this.FList, 0, this.FCount * 1);
                this.FDefault = shortIntList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:3969
        public int Add(sbyte Item)
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

        // Engine.pas:3977
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:3983
        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Array.Copy(FList, Index + 1, FList, Index, (FCount - Index) * 1);
            }
        }

        // Engine.pas:4003
        public void Exchange(int Index1, int Index2)
        {
            sbyte Item;
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

        // Engine.pas:4014
        public ShortIntList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:4020
        public sbyte First() => Get(0);

        // Engine.pas:4043
        public int IndexOf(sbyte Item)
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

        // Engine.pas:4050
        public void Insert(int Index, sbyte Item)
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
                Array.Copy(FList, Index, FList, Index + 1, (FCount - Index) * 1);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:4061
        public sbyte Last() => Get(FCount - 1);

        // Engine.pas:4066
        public void Move(int CurIndex, int NewIndex)
        {
            sbyte Item;
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

        // Engine.pas:4086
        public int Remove(sbyte Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:4092
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

        // Engine.pas:4147
        public void Sort(ShortIntSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                QuickSortShortInt(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:4153
        public bool Compare(ShortIntList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount * 1)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount * 1)), FCount * 1);

        // Engine.pas:4160
        public void LoadFromStream(Stream Stream)
        {
            byte Run = default;
            sbyte Item = default;
            int Count = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 1));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 1));
                if (Item == FDefault)
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 1));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:4183
        public void SaveToStream(Stream Stream)
        {
            int I;
            int Run;
            byte B = default;
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
            Stream.Write(Bytes.Of(ref FDefault, 1));
            Stream.Write(Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    while (Run > 255)
                    {
                        B = 255;
                        Stream.Write(Bytes.Of(ref FDefault, 1));
                        Stream.Write(Bytes.Of(ref B, 1));
                        Run -= 255;
                        I += 255;
                    }

                    B = (byte)Run;
                    Stream.Write(Bytes.Of(ref FDefault, 1));
                    Stream.Write(Bytes.Of(ref B, 1));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                    I += 1;
                }
            }
        }

        // Engine.pas:4231
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
        public sbyte Default { get => FDefault; set => FDefault = value; }

        public sbyte this[int Index] { get => Get(Index); set => Put(Index, value); }

        public sbyte[] List { get => FList; }

        // Engine.pas:3992
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        private static void QuickSortShortInt(Span<sbyte> SortList, int L, int R, ShortIntSortCompare SCompare)
        {
            sbyte b = 0;
            int num;
            do
            {
                num = L;
                int num2 = R;
                sbyte item = SortList[L + R >>> 1];
                while (true)
                {
                    if (SCompare(SortList[num], item) < 0)
                    {
                        num++;
                        continue;
                    }
                    while (SCompare(SortList[num2], item) > 0)
                    {
                        num2--;
                    }
                    if (num <= num2)
                    {
                        b = SortList[num];
                        SortList[num] = SortList[num2];
                        SortList[num2] = b;
                        num++;
                        num2--;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                }
                if (L < num2)
                {
                    QuickSortShortInt(SortList, L, num2, SCompare);
                }
                L = num;
            }
            while (num < R);
        }

        public delegate int ShortIntSortCompare(sbyte Item1, sbyte Item2);

    }

    public partial class WordList : Persistent
    {
        internal ushort[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal ushort FDefault;

        // Engine.pas:4336
        internal ushort Get(int Index)
        {
            ushort Result;
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

        // Engine.pas:4390
        internal void Put(int Index, ushort Item)
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

        // Engine.pas:4410
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

        // Engine.pas:4421
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
                FillWord(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:4344
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

        // Engine.pas:4261
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:4266
        public override void Assign(Persistent Source)
        {
            if (Source is WordList)
            {
                var wordList = (WordList)Source;
                this.FCount = wordList.FCount;
                this.SetCapacity(wordList.FCount);
                Array.Copy(wordList.FList, 0, this.FList, 0, this.FCount);
                this.FDefault = wordList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:4280
        public int Add(ushort Item)
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

        // Engine.pas:4288
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:4294
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

        // Engine.pas:4314
        public void Exchange(int Index1, int Index2)
        {
            ushort Item;
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

        // Engine.pas:4325
        public WordList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:4331
        public ushort First() => Get(0);

        // Engine.pas:4354
        public int IndexOf(ushort Item)
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

        // Engine.pas:4361
        public void Insert(int Index, ushort Item)
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

        // Engine.pas:4372
        public ushort Last() => Get(FCount - 1);

        // Engine.pas:4377
        public void Move(int CurIndex, int NewIndex)
        {
            ushort Item;
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

        // Engine.pas:4397
        public int Remove(ushort Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:4403
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

        // Engine.pas:4458
        public void Sort(WordSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                QuickSortWord(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:4464
        public bool Compare(WordList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 2);

        // Engine.pas:4471
        public void LoadFromStream(Stream Stream)
        {
            ushort Item = default;
            ushort Run = default;
            int Count = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 2));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 2));
                if (Item == FDefault)
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 2));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:4493
        public void SaveToStream(Stream Stream)
        {
            int I;
            int Run;
            ushort B = default;
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
            Stream.Write(Bytes.Of(ref FDefault, 2));
            Stream.Write(Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    while (Run > 65535)
                    {
                        B = 65535;
                        Stream.Write(Bytes.Of(ref FDefault, 2));
                        Stream.Write(Bytes.Of(ref B, 2));
                        // { sic: the original subtracts $FF here but advances I by $FFFF }
                        Run -= 255;
                        I += 65535;
                    }

                    B = (ushort)Run;
                    Stream.Write(Bytes.Of(ref FDefault, 2));
                    Stream.Write(Bytes.Of(ref B, 2));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                    I += 1;
                }
            }
        }

        // Engine.pas:4541
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
        public ushort Default { get => FDefault; set => FDefault = value; }

        public ushort this[int Index] { get => Get(Index); set => Put(Index, value); }

        public ushort[] List { get => FList; }

        // Engine.pas:4303
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        private static void QuickSortWord(Span<ushort> SortList, int L, int R, WordSortCompare SCompare)
        {
            ushort num = 0;
            int num2;
            do
            {
                num2 = L;
                int num3 = R;
                ushort item = SortList[L + R >>> 1];
                while (true)
                {
                    if (SCompare(SortList[num2], item) < 0)
                    {
                        num2++;
                        continue;
                    }
                    while (SCompare(SortList[num3], item) > 0)
                    {
                        num3--;
                    }
                    if (num2 <= num3)
                    {
                        num = SortList[num2];
                        SortList[num2] = SortList[num3];
                        SortList[num3] = num;
                        num2++;
                        num3--;
                    }
                    if (num2 > num3)
                    {
                        break;
                    }
                }
                if (L < num3)
                {
                    QuickSortWord(SortList, L, num3, SCompare);
                }
                L = num2;
            }
            while (num2 < R);
        }

        internal static void FillWord(Span<ushort> buffer, int count, ushort value)
        {
            EngineServices.Fill(buffer, count, value);
        }

        public delegate int WordSortCompare(ushort Item1, ushort Item2);

    }

    public partial class BitList : Persistent
    {
        internal Memory<byte> FList = Memory<byte>.Empty;
        internal int FCount;
        internal int FCapacity;
        internal bool FDefault;

        // Engine.pas:4743
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
            {
                Error("List capacity out of bounds (%d)", NewCapacity);
            }

            NewCapacity = (NewCapacity + 31) & ~31;
            if (NewCapacity != FCapacity)
            {
                if (NewCapacity == 0) FList = Memory<byte>.Empty;
                else
                {
                    var resized = new byte[(int)((uint)NewCapacity >> 3)];
                    FList.Span[..Math.Min(FList.Length, resized.Length)].CopyTo(resized);
                    FList = resized;
                }
                FCapacity = NewCapacity;
                for (int I = FCount; I <= NewCapacity - 1; I++)
                {
                    Put(I, FDefault);
                }
            }
        }

        // Engine.pas:4758
        internal void SetCount(int NewCount)
        {
            int OldCount;
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
                OldCount = FCount;
                FCount = NewCount;
                for (int I = OldCount, last = FCount - 1; I <= last; I++)
                {
                    Put(I, FDefault);
                }
            }
            else
            {
                FCount = NewCount;
            }
        }

        // Engine.pas:4798
        internal string GetItemString()
        {
            string Result = "";
            for (int I = 0, last = FCount - 1; I <= last; I++)
            {
                if (Get(I) != FDefault)
                {
                    Result = Result + IntToStr(I) + ",";
                }
            }

            if (!IsEmpty(Result))
            {
                Result = Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:4810
        internal void SetItemString(string S)
        {
            string Item;
            int P;
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
                    Put(StrToInt(Item), !FDefault);
                }

                P = Pos(",", S);
            }
        }

        // Engine.pas:4834
        internal int GetBitsSet()
        {
            int Result = 0;
            for (int I = 0, last = FCount - 1; I <= last; I++)
            {
                if (Get(I))
                {
                    Result += 1;
                }
            }

            return Result;
        }

        // Engine.pas:4843
        internal int GetBitsCleared()
        {
            int Result = 0;
            for (int I = 0, last = FCount - 1; I <= last; I++)
            {
                if (!Get(I))
                {
                    Result += 1;
                }
            }

            return Result;
        }

        // Engine.pas:4683
        // {Item}
        // {Item}
        protected internal virtual void Grow()
        {
            SetCapacity(FCapacity + 32);
        }

        // Engine.pas:4571
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:4576
        public override void Assign(Persistent Source)
        {
            if (Source is BitList)
            {
                var bitList = (BitList)Source;
                this.FCount = bitList.FCount;
                this.SetCapacity(bitList.FCount);
                RtlUnit.Move(bitList.FList.Span.Slice(0, (int)(((uint)(this.FCount + 31)) >> 5) * 4), this.FList.Span.Slice(0, (int)(((uint)(this.FCount + 31)) >> 5) * 4), (int)(((uint)(this.FCount + 31)) >> 5) * 4);
                this.FDefault = bitList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:4590
        public int Add(bool Item)
        {
            int Result = FCount;
            if (Result == FCapacity)
            {
                Grow();
            }

            Put(FCount, Item);
            return Result;
        }

        // Engine.pas:4597
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:4603
        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                for (int I = Index, last = FCount - 1; I <= last; I++)
                {
                    Put(I, Get(I + 1));
                }
            }
        }

        // Engine.pas:4624
        public void Exchange(int Index1, int Index2)
        {
            bool Item;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error("List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error("List index out of bounds (%d)", Index2);
            }

            Item = Get(Index1);
            Put(Index1, Get(Index2));
            Put(Index2, Item);
        }

        // Engine.pas:4635
        public BitList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:4641
        public bool First() => Get(0);

        // Engine.pas:4688
        public int IndexOf(bool Item)
        {
            int Result = 0;
            while (Result < FCount && Item != Get(Result))
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:4695
        public void Insert(int Index, bool Item)
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
                FCount += 1;
                // { sic: the original copies the same bit FCount - Index - 1 times instead of moving the tail }
                for (int I = FCount - 2; I >= Index; I--)
                {
                    Put(Index + 1, Get(Index));
                }
            }
            else
            {
                FCount += 1;
            }

            Put(Index, Item);
        }

        // Engine.pas:4712
        public bool Last() => Get(FCount - 1);

        // Engine.pas:4717
        public void Move(int CurIndex, int NewIndex)
        {
            bool Item;
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

        // Engine.pas:4730
        public int Remove(bool Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:4736
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

        // Engine.pas:4775
        public bool Compare(BitList List) => List.FCount == FCount && CompareMem(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), List.FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), (int)(((uint)(FCount + 7)) >> 3));

        // Engine.pas:4782
        public void LoadFromStream(Stream Stream)
        {
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 1));
            Stream.ReadBytes(Bytes.Of(ref FCount, 4));
            SetCapacity(FCount);
            Stream.ReadBytes(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
        }

        // Engine.pas:4791
        public void SaveToStream(Stream Stream)
        {
            Stream.Write(Bytes.Of(ref FDefault, 1));
            Stream.Write(Bytes.Of(ref FCount, 4));
            Stream.Write(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public bool Default { get => FDefault; set => FDefault = value; }

        public bool this[int Index] { get => Get(Index); set => Put(Index, value); }

        public string ItemString { get => GetItemString(); set => SetItemString(value); }
        public int BitsSet { get => GetBitsSet(); }
        public int BitsCleared { get => GetBitsCleared(); }

        // Engine.pas:4613
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

    }

    public partial class EventList : Persistent
    {
        internal NotifyEvent<object>[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal NotifyEvent<object> FDefault;

        // Engine.pas:4929
        internal NotifyEvent<object> Get(int Index)
        {
            NotifyEvent<object> Result;
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

        // Engine.pas:4987
        internal void Put(int Index, NotifyEvent<object> Item)
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

        // Engine.pas:5011
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

        // Engine.pas:5022
        internal void SetCount(int NewCount)
        {
            int OldCount;
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
                OldCount = FCount;
                FCount = NewCount;
                for (int I = OldCount, last = FCount - 1; I <= last; I++)
                {
                    Put(I, FDefault);
                }
            }
            else
            {
                FCount = NewCount;
            }
        }

        // Engine.pas:4937
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

        // Engine.pas:4854
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:4859
        public override void Assign(Persistent Source)
        {
            if (Source is EventList)
            {
                var eventList = (EventList)Source;
                this.FCount = eventList.FCount;
                this.SetCapacity(eventList.FCount);
                Array.Copy(eventList.FList, 0, this.FList, 0, this.FCount);
                this.FDefault = eventList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:4873
        public int Add(NotifyEvent<object> Item)
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

        // Engine.pas:4881
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:4887
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

        // Engine.pas:4907
        public void Exchange(int Index1, int Index2)
        {
            NotifyEvent<object> Item;
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

        // Engine.pas:4918
        public EventList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:4924
        public NotifyEvent<object> First() => Get(0);

        // Engine.pas:4947
        public int IndexOf(NotifyEvent<object> Item)
        {
            Delegate M;
            int Result = 0;
            M = Item;
            while (Result < FCount && (MethodTarget(FList[Result]) != MethodTarget(M) || MethodCode(FList[Result]) != MethodCode(M)))
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:4958
        public void Insert(int Index, NotifyEvent<object> Item)
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

        // Engine.pas:4969
        public NotifyEvent<object> Last() => Get(FCount - 1);

        // Engine.pas:4974
        public void Move(int CurIndex, int NewIndex)
        {
            NotifyEvent<object> Item;
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

        // Engine.pas:4994
        public int Remove(NotifyEvent<object> Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:5000
        public void Pack()
        {
            Delegate M;
            M = FDefault;
            // { sic: the original deletes an item only when both halves differ from the default }
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (MethodTarget(FList[I]) != MethodTarget(M) && MethodCode(FList[I]) != MethodCode(M))
                {
                    Delete(I);
                }
            }
        }

        // Engine.pas:5039
        public bool Compare(EventList List) => List.FCount == FCount && FList.AsSpan(0, FCount).SequenceEqual(List.FList.AsSpan(0, FCount));

        // Engine.pas:5046
        public void TriggerNotifyEvents(object Sender)
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (FList[I] != null)
                {
                    FList[I](Sender);
                }
            }
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public NotifyEvent<object> Default { get => FDefault; set => FDefault = value; }

        public NotifyEvent<object> this[int Index] { get => Get(Index); set => Put(Index, value); }

        public NotifyEvent<object>[] List { get => FList; }

        // Engine.pas:4896
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

    }

    public partial class DoubleItemList : Persistent
    {
        internal DoubleItem[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal DoubleItem FDefault;

        // Engine.pas:5140
        internal DoubleItem Get(int Index)
        {
            DoubleItem Result;
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

        // Engine.pas:5208
        internal void Put(int Index, DoubleItem Item)
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

        // Engine.pas:5233
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

        // Engine.pas:5244
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
                FillLargeInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:5148
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

        // Engine.pas:5056
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:5061
        public override void Assign(Persistent Source)
        {
            if (Source is DoubleItemList)
            {
                var doubleItemList = (DoubleItemList)Source;
                this.FCount = doubleItemList.FCount;
                this.SetCapacity(doubleItemList.FCount);
                Array.Copy(doubleItemList.FList, 0, this.FList, 0, this.FCount);
                this.FDefault = doubleItemList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:5075
        public int Add(in DoubleItem Item)
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

        // Engine.pas:5083
        public int AddAB(int A, int B)
        {
            int Result = FCount;
            if (Result == FCapacity)
            {
                Grow();
            }

            FList[Result].A = A;
            FList[Result].B = B;
            FCount += 1;
            return Result;
        }

        // Engine.pas:5092
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:5098
        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 8);
            }
        }

        // Engine.pas:5118
        public void Exchange(int Index1, int Index2)
        {
            DoubleItem Item;
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

        // Engine.pas:5129
        public DoubleItemList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:5135
        public DoubleItem First() => Get(0);

        // Engine.pas:5158
        public int IndexOf(in DoubleItem Item)
        {
            int Result = 0;
            while (Result < FCount && (double)Unsafe.As<DoubleItem, long>(ref FList[Result]) != Unsafe.BitCast<DoubleItem, long>(Item))
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5165
        public int IndexOfItemA(int A)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].A != A)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5172
        public int IndexOfItemB(int B)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].B != B)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5179
        public void Insert(int Index, in DoubleItem Item)
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
                RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 8);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:5190
        public DoubleItem Last() => Get(FCount - 1);

        // Engine.pas:5195
        public void Move(int CurIndex, int NewIndex)
        {
            DoubleItem Item;
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

        // Engine.pas:5215
        public int Remove(in DoubleItem Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:5221
        public void Pack()
        {
            DoubleItem Item = default;
            for (int I = FCount - 1; I >= 0; I--)
            {
                Item = this[I];
                if ((double)Unsafe.As<DoubleItem, long>(ref Item) == Unsafe.As<DoubleItem, long>(ref FDefault))
                {
                    Delete(I);
                }
            }
        }

        // Engine.pas:5281
        public void Sort(DoubleItemSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                QuickSortDoubleItem(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:5307
        public void SortAscendingItemA()
        {
            Sort(CompareDoubleItemAAscending);
        }

        // Engine.pas:5312
        public void SortAscendingItemB()
        {
            Sort(CompareDoubleItemBAscending);
        }

        // Engine.pas:5317
        public void SortDescendingItemA()
        {
            Sort(CompareDoubleItemADescending);
        }

        // Engine.pas:5322
        public void SortDescendingItemB()
        {
            Sort(CompareDoubleItemBDescending);
        }

        // Engine.pas:5327
        public bool Compare(DoubleItemList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 8);

        // Engine.pas:5334
        public void LoadFromStream(Stream Stream)
        {
            DoubleItem Item = default;
            int Run = default;
            int Count = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 8));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 8));
                if ((double)Unsafe.As<DoubleItem, long>(ref Item) == Unsafe.As<DoubleItem, long>(ref FDefault))
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:5356
        public void SaveToStream(Stream Stream)
        {
            int I;
            int Run = default;
            int DefaultRun()
            {
                int J;
                int Result = 0;
                J = I;
                while (J < FCount && (double)Unsafe.As<DoubleItem, long>(ref FList[J]) == Unsafe.As<DoubleItem, long>(ref FDefault))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Bytes.Of(ref FDefault, 8));
            Stream.Write(Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    Stream.Write(Bytes.Of(ref FDefault, 8));
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

        // Engine.pas:5394
        public int RandomChanceItemA(int Bonus)
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
                    Chance = FList[I].A + Bonus;
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
                    Chance = FList[Result].A + Bonus;
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

        // Engine.pas:5422
        public int RandomChanceItemB(int Bonus)
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
                    Chance = FList[I].B + Bonus;
                    if (Chance > 0)
                    {
                        Total += Chance;
                    }
                }

                R = Random(Total);
                Result = 0;
                Total = 0;
                Chance = FList[0].B + Bonus;
                if (Chance > 0)
                {
                    Total += Chance;
                }

                while (Total <= R)
                {
                    Result += 1;
                    Chance = FList[Result].B + Bonus;
                    if (Chance > 0)
                    {
                        Total += Chance;
                    }
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
        public DoubleItem Default { get => FDefault; set => FDefault = value; }

        public DoubleItem this[int Index] { get => Get(Index); set => Put(Index, value); }

        public DoubleItem[] List { get => FList; }

        // Engine.pas:5107
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        private static void QuickSortDoubleItem(Span<DoubleItem> SortList, int L, int R, DoubleItemSortCompare SCompare)
        {
            DoubleItem tDoubleItem = default(DoubleItem);
            int num;
            do
            {
                num = L;
                int num2 = R;
                DoubleItem Item = SortList[L + R >>> 1];
                while (true)
                {
                    if (SCompare(in SortList[num], in Item) < 0)
                    {
                        num++;
                        continue;
                    }
                    while (SCompare(in SortList[num2], in Item) > 0)
                    {
                        num2--;
                    }
                    if (num <= num2)
                    {
                        tDoubleItem = SortList[num];
                        SortList[num] = SortList[num2];
                        SortList[num2] = tDoubleItem;
                        num++;
                        num2--;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                }
                if (L < num2)
                {
                    QuickSortDoubleItem(SortList, L, num2, SCompare);
                }
                L = num;
            }
            while (num < R);
        }

        private static int CompareDoubleItemAAscending(in DoubleItem Item1, in DoubleItem Item2)
        {
            return Item1.A - Item2.A;
        }

        private static int CompareDoubleItemBAscending(in DoubleItem Item1, in DoubleItem Item2)
        {
            return Item1.B - Item2.B;
        }

        private static int CompareDoubleItemADescending(in DoubleItem Item1, in DoubleItem Item2)
        {
            return Item2.A - Item1.A;
        }

        private static int CompareDoubleItemBDescending(in DoubleItem Item1, in DoubleItem Item2)
        {
            return Item2.B - Item1.B;
        }

        private static void FillLargeInteger(Span<DoubleItem> buffer, int count, DoubleItem value)
        {
            EngineServices.Fill(buffer, count, value);
        }

        [StructLayout(LayoutKind.Explicit, Size = 8)]
        public struct DoubleItem
        {
            [FieldOffset(0)]
            public int A;
            [FieldOffset(4)]
            public int B;
        }

        public delegate int DoubleItemSortCompare(in DoubleItem Item1, in DoubleItem Item2);

    }

    public partial class QuadItemList : Persistent
    {
        internal QuadInteger[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal QuadInteger FDefault;

        // Engine.pas:5538
        internal QuadInteger Get(int Index)
        {
            QuadInteger Result;
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

        // Engine.pas:5621
        internal void Put(int Index, QuadInteger Item)
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

        // Engine.pas:5643
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

        // Engine.pas:5654
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
                FillQuadInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:5546
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

        // Engine.pas:5452
        public override void Dispose()
        {
            Clear();
        }

        // Engine.pas:5457
        public override void Assign(Persistent Source)
        {
            if (Source is QuadItemList)
            {
                var quadItemList = (QuadItemList)Source;
                this.FCount = quadItemList.FCount;
                this.SetCapacity(quadItemList.FCount);
                Array.Copy(quadItemList.FList, 0, this.FList, 0, this.FCount);
                this.FDefault = quadItemList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:5471
        public int Add(in QuadInteger Item)
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

        // Engine.pas:5479
        public int AddABCD(int A, int B, int C, int D)
        {
            int Result = FCount;
            if (Result == FCapacity)
            {
                Grow();
            }

            FList[Result].A = A;
            FList[Result].B = B;
            FList[Result].C = C;
            FList[Result].D = D;
            FCount += 1;
            return Result;
        }

        // Engine.pas:5490
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // Engine.pas:5496
        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 16);
            }
        }

        // Engine.pas:5516
        public void Exchange(int Index1, int Index2)
        {
            QuadInteger Item;
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

        // Engine.pas:5527
        public QuadItemList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // Engine.pas:5533
        public QuadInteger First() => Get(0);

        // Engine.pas:5556
        public int IndexOf(in QuadInteger Item)
        {
            int Result = 0;
            while (Result < FCount && (FList[Result].A != Item.A || FList[Result].B != Item.B || FList[Result].C != Item.C || FList[Result].D != Item.D))
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5564
        public int IndexOfItemA(int A)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].A != A)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5571
        public int IndexOfItemB(int B)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].B != B)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5578
        public int IndexOfItemC(int C)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].C != C)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5585
        public int IndexOfItemD(int D)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].D != D)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:5592
        public void Insert(int Index, in QuadInteger Item)
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
                RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 16);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:5603
        public QuadInteger Last() => Get(FCount - 1);

        // Engine.pas:5608
        public void Move(int CurIndex, int NewIndex)
        {
            QuadInteger Item;
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

        // Engine.pas:5628
        public int Remove(in QuadInteger Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:5634
        public void Pack()
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (FList[I].A == FDefault.A && FList[I].B == FDefault.B && FList[I].C == FDefault.C && FList[I].D == FDefault.D)
                {
                    Delete(I);
                }
            }
        }

        // Engine.pas:5691
        public void Sort(QuadItemSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                QuickSortQuadItem(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:5737
        public void SortAscendingItemA()
        {
            Sort(CompareQuadItemAAscending);
        }

        // Engine.pas:5742
        public void SortAscendingItemB()
        {
            Sort(CompareQuadItemBAscending);
        }

        // Engine.pas:5747
        public void SortAscendingItemC()
        {
            Sort(CompareQuadItemCAscending);
        }

        // Engine.pas:5752
        public void SortAscendingItemD()
        {
            Sort(CompareQuadItemDAscending);
        }

        // Engine.pas:5757
        public void SortDescendingItemA()
        {
            Sort(CompareQuadItemADescending);
        }

        // Engine.pas:5762
        public void SortDescendingItemB()
        {
            Sort(CompareQuadItemBDescending);
        }

        // Engine.pas:5767
        public void SortDescendingItemC()
        {
            Sort(CompareQuadItemCDescending);
        }

        // Engine.pas:5772
        public void SortDescendingItemD()
        {
            Sort(CompareQuadItemDDescending);
        }

        // Engine.pas:5777
        public bool Compare(QuadItemList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 16);

        // Engine.pas:5784
        public void LoadFromStream(Stream Stream)
        {
            int Run = default;
            int Count = default;
            QuadInteger Item = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 16));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 16));
                if (Item.A == FDefault.A && Item.B == FDefault.B && Item.C == FDefault.C && Item.D == FDefault.D)
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:5807
        public void SaveToStream(Stream Stream)
        {
            int I;
            int Run = default;
            int DefaultRun()
            {
                int J;
                int Result = 0;
                J = I;
                while (J < FCount && (FList[J].A == FDefault.A && FList[J].B == FDefault.B && FList[J].C == FDefault.C && FList[J].D == FDefault.D))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Bytes.Of(ref FDefault, 16));
            Stream.Write(Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    Stream.Write(Bytes.Of(ref FDefault, 16));
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

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public QuadInteger Default { get => FDefault; set => FDefault = value; }

        public QuadInteger this[int Index] { get => Get(Index); set => Put(Index, value); }

        public QuadInteger[] List { get => FList; }

        // Engine.pas:5505
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        private static void QuickSortQuadItem(Span<QuadInteger> SortList, int L, int R, QuadItemSortCompare SCompare)
        {
            QuadInteger tQuadInteger = default(QuadInteger);
            int num;
            do
            {
                num = L;
                int num2 = R;
                QuadInteger Item = SortList[L + R >>> 1];
                while (true)
                {
                    if (SCompare(in SortList[num], in Item) < 0)
                    {
                        num++;
                        continue;
                    }
                    while (SCompare(in SortList[num2], in Item) > 0)
                    {
                        num2--;
                    }
                    if (num <= num2)
                    {
                        tQuadInteger = SortList[num];
                        SortList[num] = SortList[num2];
                        SortList[num2] = tQuadInteger;
                        num++;
                        num2--;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                }
                if (L < num2)
                {
                    QuickSortQuadItem(SortList, L, num2, SCompare);
                }
                L = num;
            }
            while (num < R);
        }

        private static int CompareQuadItemAAscending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item1.A - Item2.A;
        }

        private static int CompareQuadItemBAscending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item1.B - Item2.B;
        }

        private static int CompareQuadItemCAscending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item1.C - Item2.C;
        }

        private static int CompareQuadItemDAscending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item1.D - Item2.D;
        }

        private static int CompareQuadItemADescending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item2.A - Item1.A;
        }

        private static int CompareQuadItemBDescending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item2.B - Item1.B;
        }

        private static int CompareQuadItemCDescending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item2.C - Item1.C;
        }

        private static int CompareQuadItemDDescending(in QuadInteger Item1, in QuadInteger Item2)
        {
            return Item2.D - Item1.D;
        }

        internal static void FillQuadInteger(Span<QuadInteger> buffer, int count, in QuadInteger value)
        {
            QuadInteger value2 = value;
            EngineServices.Fill(buffer, count, value2);
        }

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct QuadInteger
        {
            [FieldOffset(0)]
            public int A;
            [FieldOffset(4)]
            public int B;
            [FieldOffset(8)]
            public int C;
            [FieldOffset(12)]
            public int D;
        }

        public delegate int QuadItemSortCompare(in QuadInteger Item1, in QuadInteger Item2);

    }

    public class RecordList<TRecord> : Persistent where TRecord : struct
    {
        internal TRecord[] FList = [];
        internal int FCount;
        internal int FCapacity;

        internal TRecord Get(int Index) => Index < 0 || Index >= FCount ? default : FList[Index];

        internal void Put(int Index, TRecord Item)
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
                Array.Clear(FList, FCount, NewCount - FCount);
            }
            else
            {
                Array.Clear(FList, NewCount, FCount - NewCount);
            }

            FCount = NewCount;
        }

        protected internal virtual void Grow()
        {
            int Delta;
            if (FCapacity > 64)
            {
                Delta = FCapacity / 4;
            }
            else if (FCapacity > 8)
            {
                Delta = 16;
            }
            else
            {
                Delta = 4;
            }

            SetCapacity(FCapacity + Delta);
        }

        public override void Dispose()
        {
            Clear();
            base.Dispose();
        }

        public override void Assign(Persistent Source)
        {
            if (Source is RecordList<TRecord> list)
            {
                Clear();
                SetCapacity(list.FCount);
                Array.Copy(list.FList, FList, list.FCount);
                FCount = list.FCount;
            }
            else
            {
                base.Assign(Source);
            }
        }

        public int Add(in TRecord Item)
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

        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            Array.Copy(FList, Index + 1, FList, Index, FCount - Index);
            FList[FCount] = default;
        }

        public void Exchange(int Index1, int Index2)
        {
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error("List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error("List index out of bounds (%d)", Index2);
            }

            (FList[Index1], FList[Index2]) = (FList[Index2], FList[Index1]);
        }

        public TRecord First() => Get(0);

        public void Insert(int Index, in TRecord Item)
        {
            if (Index < 0 || Index > FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            Array.Copy(FList, Index, FList, Index + 1, FCount - Index);
            FList[Index] = Item;
            FCount += 1;
        }

        public TRecord Last() => Get(FCount - 1);

        public void Sort(RecordSortCompare<TRecord> Compare)
        {
            if (FCount > 0)
            {
                QuickSort(FList, 0, FCount - 1, Compare);
            }
        }

        // Engine.pas:5254, the quicksort of QuickSortDoubleItem and QuickSortQuadItem.
        private static void QuickSort(Span<TRecord> SortList, int L, int R, RecordSortCompare<TRecord> SCompare)
        {
            int I;
            int J;
            TRecord P;
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
                        (SortList[I], SortList[J]) = (SortList[J], SortList[I]);
                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    QuickSort(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }

        public TRecord this[int Index] { get => Get(Index); set => Put(Index, value); }

        public TRecord[] List { get => FList; }

        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        // The record lists above with an object column. Engine.pas stored Integer(Object) in column A of a
        // TDoubleItemList or TQuadItemList and cast it back on read. These lists keep the reference itself,
        // so no id table holds the object and the owner of the list alone decides its lifetime. Every
        // column that held a number stays an int. Sort runs the quicksort of EngineUnit.QuickSortDoubleItem,
        // so a list sorted on its int columns keeps the original order.
        public delegate int RecordSortCompare<TItem>(in TItem Item1, in TItem Item2);
    }

    public struct DoubleItem<TA> where TA : class
    {
        public TA A;
        public int B;
    }

    public class DoubleItemList<TA> : RecordList<DoubleItem<TA>> where TA : class
    {
        public int AddAB(TA A, int B) => Add(new DoubleItem<TA> { A = A, B = B });

        public int IndexOfItemA(TA A)
        {
            int Result = 0;
            while (Result < FCount && !ReferenceEquals(FList[Result].A, A))
            {
                Result += 1;
            }

            return Result == FCount ? -1 : Result;
        }

        public int IndexOfItemB(int B)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].B != B)
            {
                Result += 1;
            }

            return Result == FCount ? -1 : Result;
        }

        public void SortAscendingItemB() => Sort(static (in Item1, in Item2) => Item1.B - Item2.B);

        public void SortDescendingItemB() => Sort(static (in Item1, in Item2) => Item2.B - Item1.B);

        // Engine.pas:5422
        public int RandomChanceItemB(int Bonus)
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
                    Chance = FList[I].B + Bonus;
                    if (Chance > 0)
                    {
                        Total += Chance;
                    }
                }

                R = Random(Total);
                Result = 0;
                Total = 0;
                Chance = FList[0].B + Bonus;
                if (Chance > 0)
                {
                    Total += Chance;
                }

                while (Total <= R)
                {
                    Result += 1;
                    Chance = FList[Result].B + Bonus;
                    if (Chance > 0)
                    {
                        Total += Chance;
                    }
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }
    }

    public struct QuadItem<TA> where TA : class
    {
        public TA A;
        public int B;
        public int C;
        public int D;

        internal static int P3CompareRoundTargets(in QuadItem<CombatPredictorUnit> A, in QuadItem<CombatPredictorUnit> B) => A.D - B.D;

    }

    public class QuadItemList<TA> : RecordList<QuadItem<TA>> where TA : class
    {
        public int AddABCD(TA A, int B, int C, int D) => Add(new QuadItem<TA> { A = A, B = B, C = C, D = D });

        public int IndexOfItemA(TA A)
        {
            int Result = 0;
            while (Result < FCount && !ReferenceEquals(FList[Result].A, A))
            {
                Result += 1;
            }

            return Result == FCount ? -1 : Result;
        }

        public int IndexOfItemB(int B)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].B != B)
            {
                Result += 1;
            }

            return Result == FCount ? -1 : Result;
        }

        public int IndexOfItemC(int C)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].C != C)
            {
                Result += 1;
            }

            return Result == FCount ? -1 : Result;
        }

        public int IndexOfItemD(int D)
        {
            int Result = 0;
            while (Result < FCount && FList[Result].D != D)
            {
                Result += 1;
            }

            return Result == FCount ? -1 : Result;
        }

        public void SortAscendingItemB() => Sort(static (in Item1, in Item2) => Item1.B - Item2.B);

        public void SortAscendingItemC() => Sort(static (in Item1, in Item2) => Item1.C - Item2.C);

        public void SortAscendingItemD() => Sort(static (in Item1, in Item2) => Item1.D - Item2.D);

        public void SortDescendingItemB() => Sort(static (in Item1, in Item2) => Item2.B - Item1.B);

        public void SortDescendingItemC() => Sort(static (in Item1, in Item2) => Item2.C - Item1.C);

        public void SortDescendingItemD() => Sort(static (in Item1, in Item2) => Item2.D - Item1.D);
    }
}

namespace Aow.Game.Engine
{

    [LegacyObject]
    public partial class PropertyTable : IDisposable
    {
        internal PropertyItem[] FItems = [];
        internal int FCapacity;
        internal int FCount;
        internal int FNextIndex;
        internal int FMaxID;
        internal PropertySettings[] FStack = [];
        internal int FStackCount;
        internal int FStackCapacity;
        internal int FFirst;

        // Engine.pas:2260
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity > 0)
            {
                NewCapacity = (NewCapacity + 31) & ~31;
            }

            if (NewCapacity != FCapacity)
            {
                Array.Resize(ref FItems, NewCapacity);
                FCapacity = NewCapacity;
                if (FCapacity < FCount)
                {
                    FCount = FCapacity;
                }
            }
        }

        // Engine.pas:2253
        public virtual void Dispose()
        {
            SetCapacity(0);
            Array.Resize(ref FStack, 0);
        }

        // Engine.pas:2273
        public void Add(int ID, int Offset)
        {
            if (FCapacity == FCount)
            {
                SetCapacity(FCapacity + 1);
            }

            FItems[FCount].ID = ID;
            FItems[FCount].Offset = Offset;
            FCount += 1;
            if (ID > FMaxID)
            {
                FMaxID = ID;
            }
        }

        // Engine.pas:2284
        public void Clear()
        {
            FNextIndex = 0;
            FMaxID = 0;
            SetCapacity(0);
        }

        // Engine.pas:2291
        public int FindOffset(int ID)
        {
            int Result;
            int I;
            I = FNextIndex;
            if (I >= FCount || FItems[I].ID != ID)
            {
                I = FFirst;
                Result = -1;
                while (I < FCount && FItems[I].ID != ID)
                {
                    I += 1;
                }

                if (I != FCount)
                {
                    Result = FItems[I].Offset;
                    FNextIndex = I + 1;
                }
            }
            else
            {
                Result = FItems[I].Offset;
                FNextIndex += 1;
            }

            return Result;
        }

        // Engine.pas:2315
        public bool PropertyExist(int ID)
        {
            bool Result;
            int I;
            if (ID <= FMaxID)
            {
                I = FFirst;
                while (I < FCount && FItems[I].ID != ID)
                {
                    I += 1;
                }

                Result = I != FCount;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Engine.pas:2330
        public void BackupSettings()
        {
            if (FStackCount == FStackCapacity)
            {
                FStackCapacity += 16;
                Array.Resize(ref FStack, FStackCapacity);
            }

            ref PropertySettings propertySettings = ref FStack[FStackCount];
            propertySettings.First = FFirst;
            propertySettings.NextIndex = FNextIndex;
            propertySettings.MaxID = FMaxID;

            FStackCount += 1;
            FNextIndex = FCount;
            FFirst = FCount;
            FMaxID = 0;
        }

        // Engine.pas:2349
        public void RestoreSettings()
        {
            FStackCount -= 1;
            FCount = FFirst;
            ref PropertySettings propertySettings = ref FStack[FStackCount];
            FFirst = propertySettings.First;
            FNextIndex = propertySettings.NextIndex;
            FMaxID = propertySettings.MaxID;
        }

        // Engine.pas:2361
        public void LoadFromStream(Stream Stream)
        {
            int LongCount = default;
            int ID = default;
            int Offset = default;
            byte ShortCount = default;
            ushort Pair = default;
            Stream.ReadBytes(Bytes.Of(ref ShortCount, 1));
            if ((ShortCount & 128) != 0)
            {
                ShortCount = (byte)(ShortCount & 127);
                Stream.ReadBytes(Bytes.Of(ref LongCount, 4));
            }
            else
            {
                LongCount = 0;
            }

            if (ShortCount != 0)
            {
                for (int ordinal = 0, last = ShortCount - 1; ordinal <= last; ordinal++)
                {
                    Stream.ReadBytes(Bytes.Of(ref Pair, 2));
                    Add(Pair & 255, (int)(((uint)Pair) >> 8));
                }
            }

            if (LongCount != 0)
            {
                for (int ordinal = 0, last = LongCount - 1; ordinal <= last; ordinal++)
                {
                    Stream.ReadBytes(Bytes.Of(ref ID, 4));
                    Stream.ReadBytes(Bytes.Of(ref Offset, 4));
                    Add(ID, Offset);
                }
            }
        }

        // Engine.pas:2391
        public void SaveToStream(Stream Stream)
        {
            int LongCount;
            byte ShortCount;
            byte Marker;
            int Pair = default;
            LongCount = 0;
            ShortCount = 0;
            for (int I = FFirst, last = FCount - 1; I <= last; I++)
            {
                if (FItems[I].ID <= 127 && FItems[I].Offset <= 255)
                {
                    ShortCount = (byte)(ShortCount + 1);
                }
                else
                {
                    LongCount += 1;
                }
            }

            if (LongCount != 0)
            {
                Marker = (byte)(ShortCount | 128);
                Stream.Write(Bytes.Of(ref Marker, 1));
                Stream.Write(Bytes.Of(ref LongCount, 4));
            }
            else
            {
                Stream.Write(Bytes.Of(ref ShortCount, 1));
            }

            if (ShortCount != 0)
            {
                for (int I = FFirst, last = FCount - 1; I <= last; I++)
                {
                    if (FItems[I].ID <= 127 && FItems[I].Offset <= 255)
                    {
                        Pair = FItems[I].ID + (FItems[I].Offset << 8);
                        Stream.Write(Bytes.Of(ref Pair, 2));
                    }
                }
            }

            if (LongCount != 0)
            {
                for (int I = FFirst, last = FCount - 1; I <= last; I++)
                {
                    if (FItems[I].ID > 127 || FItems[I].Offset > 255)
                    {
                        Stream.Write(Bytes.Of(ref FItems[I].ID, 4));
                        Stream.Write(Bytes.Of(ref FItems[I].Offset, 4));
                    }
                }
            }
        }

        public PropertyItem[] Items { get => FItems; }
        public int Count { get => FCount; }
        public int First { get => FFirst; }

        public PropertyTable()
        {
            CaptureAddress.Assign(this);
        }

        [StructLayout(LayoutKind.Explicit, Size = 8)]
        public struct PropertyItem
        {
            [FieldOffset(0)]
            public int ID;
            [FieldOffset(4)]
            public int Offset;
        }

        [StructLayout(LayoutKind.Explicit, Size = 12)]
        public struct PropertySettings
        {
            [FieldOffset(0)]
            public int First;
            [FieldOffset(4)]
            public int NextIndex;
            [FieldOffset(8)]
            public int MaxID;
        }
    }
}

namespace Aow.Game.Engine.Nodes
{

    [LegacyObject]
    // FOwner
    public partial class EObject : IDisposable, IPersistable
    {
        // Engine.pas:2461
        public const int ClassID = 80;

        private EClass eclass;

        // The engine class descriptor of the instance's exact type, read once and cached.
        public EClass EClass => eclass ??= EClass.Of(GetType());

        // The Delphi pre-inherited statements run after capture and before any inherited body.
        private protected virtual void InitializeBeforeInherited() { }

        public EObject(ECustomNode owner) : this(null, owner) { }

        private protected EObject(ConstructionContext context, ECustomNode owner)
        {
            CaptureAddress.Assign(this);
            context?.Capture(this);
            InitializeBeforeInherited();
            SetOwner(owner);
        }

        protected internal ECustomNode FOwner;

        // Engine.pas:2487
        internal string GetEObjectPath()
        {
            EObject P;
            string S;
            S = this.GetType().Name;
            P = FOwner;
            while (P != null)
            {
                S = P.GetType().Name + "" + S;
                P = P.FOwner;
            }

            return S;
        }

        // Engine.pas:2442
        public virtual int ClassVersion() => 0;

        // Engine.pas:2563
        public virtual Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EngineServices.csStored);

        // Engine.pas:2568
        public virtual void SetOwner(ECustomNode AOwner)
        {
            if (AOwner != FOwner)
            {
                if (FOwner != null)
                {
                    FOwner.RemoveChild(this);
                }

                FOwner = AOwner;
                if (FOwner != null)
                {
                    FOwner.AddChild(this);
                }
            }
        }

        // Engine.pas:2558
        public virtual Engine GetEngine() => FOwner.GetEngine();

        // Engine.pas:2502
        public virtual int GetLastMsg() => 2147483647;

        // Engine.pas:2507
        public virtual void SetLastMsg(int Value)
        {
        }

        // Engine.pas:2590
        public virtual void ReadWrite(EStorageStream Stream)
        {
        }

        // Tree form of ReadWrite. MainPersist is the stored image.
        public virtual void PersistFields(IPersister persister)
        {
        }

        void IPersistable.Persist(IPersister persister)
        {
            if (TreePersistenceRefusal(GetType()) is string reason)
                throw new InvalidOperationException(reason);
            MainPersist(persister);
        }

        /// <summary>
        /// A class that stores through MainReadWrite or ReadWriteChildren needs the same MainPersist.
        /// A class that stores through ReadWrite needs its own PersistFields.
        /// The name is the class that is missing it. Null means the closure can use the tree.
        /// </summary>
        public static string TreePersistenceRefusal(Type type)
        {
            const BindingFlags declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            for (var current = type; current is not null && current != typeof(object); current = current.BaseType)
            {
                bool image = current.GetMethod(nameof(MainReadWrite), declared) is not null
                    || current.GetMethod("ReadWriteChildren", declared) is not null;
                bool tree = current.GetMethod(nameof(MainPersist), declared) is not null;
                if (image && !tree)
                    return "Class " + current.Name + " overrides MainReadWrite or ReadWriteChildren and has no MainPersist.";
                bool fields = current.GetMethod(nameof(ReadWrite), declared) is not null;
                bool persisted = current.GetMethod(nameof(PersistFields), declared) is not null;
                if (fields && !persisted)
                    return "Class " + current.Name + " overrides ReadWrite and has no PersistFields.";
            }
            return null;
        }

        public virtual void MainPersist(IPersister persister)
        {
            if (persister.Loading)
            {
                Clear();
            }

            PersistFields(persister);
        }

        // Engine.pas:2594
        public virtual void MainReadWrite(EStorageStream Stream)
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                Clear();
            }

            ReadWrite(Stream);
        }

        // Engine.pas:2447
        public virtual int AddRef()
        {
            throw new NotSupportedException("Reference counting not supported");
        }

        // Engine.pas:2452
        public virtual int Release()
        {
            throw new NotSupportedException("Reference counting not supported");
        }

        // Engine.pas:2457
        public virtual void Clear()
        {
        }

        // Engine.pas:2511
        public virtual bool Compare(EObject Obj, Engine AEngine)
        {
            MemoryStream S1;
            MemoryStream S2;
            bool Result = false;
            if (EClass.ClassID != Obj.EClass.ClassID)
            {
                return Result;
            }

            S1 = new MemoryStream();
            S2 = new MemoryStream();
            try
            {
                AEngine.WriteEObject(S1, this, null);
                AEngine.WriteEObject(S2, Obj, null);
                bool condition = S1.Length == S2.Length;
                if (condition)
                {
                    var size = (int)S1.Length;
                    condition = CompareMem(S1.GetBuffer().AsSpan(0, size), S2.GetBuffer().AsSpan(0, size), size);
                }

                Result = condition;
            }
            finally
            {
                S1?.Dispose();
                S2?.Dispose();
            }

            return Result;
        }

        // Engine.pas:2466
        public virtual EditorClass Editor()
        {
            Engine E;
            EditorClass Result = null;
            E = GetEngine();
            if (E != null)
            {
                for (var C = EClass; C.Type != typeof(EObject) && Result == null; C = C.Parent)
                {
                    Result = E.FEditors.FindEditor(C.ClassID);
                }
            }

            if (Result == null)
            {
                Result = EditorClass.Of<EObjectEditor>();
            }

            return Result;
        }

        // Engine.pas:2529
        public virtual void Assign(EObject Source, Engine AEngine)
        {
            MemoryStream S;
            S = new MemoryStream();
            try
            {
                AEngine.WriteEObject(S, Source, null);
                S.Position = 0;
                AEngine.ReadEObject(S, this, null);
            }
            finally
            {
                S.Dispose();
            }
        }

        // Engine.pas:2483
        public virtual void TimerEvent(int Tick, EObject Sender)
        {
        }

        // Engine.pas:2585
        public virtual void MsgProc(ref EMsg Msg, EObject Sender)
        {
            if (Msg.MsgID == 1)
            {
                SetLastMsg(2147483647);
            }
        }

        // Engine.pas:2578
        public virtual void SetArrayOwner(ECustomNode AOwner, int Index)
        {
            if (FOwner != null)
            {
                FOwner.RemoveChild(this);
            }

            FOwner = AOwner;
            if (FOwner != null)
            {
                FOwner.SetChild(Index, this);
            }
        }

        // Engine.pas:2436
        public virtual void Dispose()
        {
            SetOwner(null);
        }

        // Engine.pas:2543
        public EObject Copy(Engine AEngine)
        {
            EObject Result = default;
            MemoryStream S;
            S = new MemoryStream();
            try
            {
                AEngine.WriteEObject(S, this, null);
                S.Position = 0;
                Result = EClass.Create(null);
                AEngine.ReadEObject(S, Result, null);
            }
            finally
            {
                S.Dispose();
            }

            return Result;
        }

        public ECustomNode Owner { get => FOwner; set => SetOwner(value); }
        public string EObjectPath { get => GetEObjectPath(); }
        public int LastMsg { get => GetLastMsg(); set => SetLastMsg(value); }

    }

    // children through an abstract list interface. Message ids carry flags: $10000000 deliver once
    // per Seq, $20000000 owned children only, $40000000 do not forward to children
    public partial class ECustomNode : EObject
    {

        // Engine.pas:2430
        public ECustomNode(ECustomNode owner) : this(null, owner) { }

        private protected ECustomNode(ConstructionContext context, ECustomNode owner) : base(context, owner)
        { }

        // Engine.pas:2666
        public override void MainReadWrite(EStorageStream Stream)
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                if ((Stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
                {
                    Clear();
                }
            }

            ReadWrite(Stream);
            ReadWriteChildren(Stream);
        }

        public override void MainPersist(IPersister persister)
        {
            if (persister.Loading)
            {
                Clear();
            }

            PersistFields(persister);
            persister.Children("Children", new NodeChildren(this));
        }

        // Engine.pas:2626
        public override void Clear()
        {
            EObject C;
            if (Count != 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.Owner == this)
                    {
                        if (!C.GetControlStyle().Contains(EngineServices.csShared))
                        {
                            C.Dispose();
                        }
                        else
                        {
                            C.Clear();
                        }
                    }
                }

                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null)
                    {
                        if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.csShared)))
                        {
                            RemoveChild(C);
                        }
                    }
                }
            }
        }

        // Engine.pas:2648
        public override void Assign(EObject Source, Engine AEngine)
        {
            Clear();
            base.Assign(Source, AEngine);
        }

        // Engine.pas:2720
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (SendToChild(ref Msg))
            {
                SendMsg(ref Msg, Sender);
            }
        }

        // Engine.pas:593
        public virtual EObject GetChild(int Index)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:594
        public virtual void SetChild(int Index, EObject Child)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:595
        public virtual int GetCount()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:2661
        public virtual void ReadWriteChildren(EStorageStream Stream)
        {
            Stream.rwChildren(this, 1);
        }

        // Engine.pas:2715
        public virtual bool SendToChild(ref EMsg Msg) => (Msg.MsgID & 1073741824) == 0;

        // Engine.pas:2608
        public virtual void DestroyAllChildren()
        {
            EObject C;
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null && C.Owner == this)
                {
                    C.Dispose();
                }
            }

            if (Count != 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null)
                    {
                        RemoveChild(C);
                    }
                }
            }
        }

        // Engine.pas:2654
        public virtual void QuickClear()
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                RemoveChild(Children[I]);
            }
        }

        // Engine.pas:2674
        public virtual void SendMsg(ref EMsg Msg, EObject Sender)
        {
            EObject C;
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = Count - 1; I >= 0; I--)
                    {
                        C = Children[I];
                        if (C != null && C.GetLastMsg() != Msg.Seq)
                        {
                            C.SetLastMsg(Msg.Seq);
                            C.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = Count - 1; I >= 0; I--)
                    {
                        C = Children[I];
                        if (C != null && C.GetLastMsg() != Msg.Seq && C.Owner == this)
                        {
                            C.SetLastMsg(Msg.Seq);
                            C.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = Count - 1; I >= 0; I--)
                    {
                        C = Children[I];
                        if (C != null)
                        {
                            C.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = Count - 1; I >= 0; I--)
                    {
                        C = Children[I];
                        if (C != null && C.Owner == this)
                        {
                            C.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
        }

        // Engine.pas:601
        public virtual int RemoveChild(EObject Child)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:602
        public virtual int AddChild(EObject Child)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:2786
        // { The original tests Result before the child-count call. }
        public virtual int DestroyChildren(int AClassID)
        {
            EObject C;
            int N;
            N = 0;
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null && C.EClass.ClassID == AClassID)
                {
                    C.Dispose();
                    N += 1;
                }
            }

            return N;
        }

        // Engine.pas:2768
        public virtual EObject FindChildInTree(int AClassID)
        {
            int I;
            EObject Result = FindChild(AClassID);
            if (Result == null)
            {
                I = 0;
                while (Result == null && I < Count)
                {
                    if (Children[I] is ECustomNode)
                    {
                        Result = ((ECustomNode)Children[I]).FindChildInTree(AClassID);
                    }

                    I += 1;
                }
            }

            return Result;
        }

        // Engine.pas:2726
        public virtual EObject FindChild(int AClassID)
        {
            EObject Result;
            int I;
            bool Found;
            EObject C;
            I = 0;
            Found = false;
            C = null;
            while (!Found && I < Count)
            {
                C = Children[I];
                Found = C != null && C.EClass.ClassID == AClassID;
                I += 1;
            }

            if (Found)
            {
                Result = C;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:2744
        public virtual EObject FindOwnedChild(int AClassID)
        {
            EObject Result;
            int I;
            bool Found;
            EObject C;
            I = 0;
            Found = false;
            C = null;
            while (!Found && I < Count)
            {
                C = Children[I];
                Found = C != null && C.Owner == this && AClassID == C.EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = C;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:2762
        public virtual int FindChildIndex(EObject Child)
        {
            int Result = Count - 1;
            while (Result >= 0 && Child != Children[Result])
            {
                Result -= 1;
            }

            return Result;
        }

        // Engine.pas:2602
        public override void Dispose()
        {
            DestroyAllChildren();
            base.Dispose();
        }

        public int Count { get => GetCount(); }

        public sealed class ChildrenAccessor(ECustomNode target)
        {
            public EObject this[int Index] { get => target.GetChild(Index); set => target.SetChild(Index, value); }
        }

        public ChildrenAccessor Children => new ChildrenAccessor(this);
    }
}

namespace Aow.Game.Engine
{

    // FMsgCount, FEClasses, FClassTable, FEditors, FControlNode, FComponent (the owning component),
    // FDebugIDs, FDebugEvents, FStartupDirectory, FOptions. Every engine is in the Engines list
    public partial class Engine : ENode
    {
        // Engine.pas:8756
        public new const int ClassID = 17;

        internal int FMsgCount;
        internal EObjectClassList FEClasses;
        internal IntegerHashTable256 FClassTable;
        internal EObjectEditorList FEditors;
        internal EControlStyleNode FControlNode;
        public EComponent FComponent;
        internal StringList FDebugIDs;

        internal EventStore<EngineDebugNotification> FDebugEventsStore;

        internal event NotifyEvent<EngineDebugNotification> FDebugEvents
        {
            add => FDebugEventsStore.Add(value);
            remove => FDebugEventsStore.Remove(value);
        }

        internal string FStartupDirectory = "";
        public Set<EngineUserElement> FUser;
        internal Set<EngineModeElement> FOptions;

        // Engine.pas:8776
        internal int CreateUniqueDebugID()
        {
            int Result = 0;
            while (FDebugIDs.IndexOfObject(IntToObject(Result)) != -1)
            {
                Result += 1;
            }

            return Result;
        }

        // Engine.pas:8814
        public override Engine GetEngine() => this;

        public Engine(ECustomNode owner) : this(null, owner) { }

        private protected Engine(ConstructionContext context, ECustomNode owner) : base(context, owner)
        {
            UpdateStartupDirectory();
            FOptions = Set<EngineModeElement>.Of(eoVerifyClasses);
            FDebugEventsStore = new();
            FEClasses = new EObjectClassList();
            FClassTable = new IntegerHashTable256();
            FEditors = new EObjectEditorList();
            FControlNode = new EControlStyleNode(this);
            FControlNode.SetControlStyle(Set<EControlStyleElement>.Of(EngineServices.csShared));
            FDebugIDs = new StringList();
            FDebugIDs.AddObject(UserStr, IntToObject(-1));
            RegisterEClasses([EClass.Of<EControlStyleObject>(), EClass.Of<EControlStyleNode>(), EClass.Of<ECustomOwner>(), EClass.Of<EResourceEditSettings>()]);
            EngineHost.Engines.Add(this);
        }

        // Engine.pas:8745
        public override void Dispose()
        {
            EngineHost.Engines.Remove(this);
            FDebugEventsStore.Clear();
            FDebugIDs?.Dispose();
            base.Dispose();
            FClassTable?.Dispose();
            FEClasses?.Dispose();
        }

        // Engine.pas:8771
        public virtual void SetStartupDirectory(string Value)
        {
            FStartupDirectory = Value;
        }

        // Engine.pas:8766
        public virtual void UpdateStartupDirectory()
        {
            FStartupDirectory = ExtractFilePath(RtlUnit.Application.ExeName);
        }

        // Engine.pas:8887
        public virtual EObject CreateEObject(int AClassID, ECustomNode AOwner)
        {
            EClass C;
            C = GetEClass(AClassID);
            return C.Create(AOwner);
        }

        // Engine.pas:8860
        public virtual EClass GetEClass(int AClassID)
        {
            object P;
            P = FClassTable.Find(AClassID);
            EClass Result = (EClass)P;
            if (Result == null)
            {
                throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(AClassID) + ")");
            }

            return Result;
        }

        // Engine.pas:8761
        public virtual string TranslateText(string Text) => Text;

        // Engine.pas:8782
        public int RegisterDebugID(string Name)
        {
            int Result = CreateUniqueDebugID();
            FDebugIDs.AddObject(Name, IntToObject(Result));
            return Result;
        }

        // Engine.pas:8788
        public void UnregisterDebugID(int ID)
        {
            int I;
            I = FDebugIDs.IndexOfObject(IntToObject(ID));
            if (I != -1)
            {
                FDebugIDs.Delete(I);
            }
        }

        // Engine.pas:8796
        public void OutputDebugStr(string S)
        {
            var notification = new EngineDebugNotification();
            ref EngineDebugNotification.EngineDebugInfo Info = ref notification.Info;
            Info.ID = -1;
            Info.Text = S;
            FDebugEventsStore.Raise(notification);
        }

        // Engine.pas:8805
        public void OutputDebugMessage(int ID, string S)
        {
            var notification = new EngineDebugNotification();
            ref EngineDebugNotification.EngineDebugInfo Info = ref notification.Info;
            Info.ID = ID;
            Info.Text = S;
            FDebugEventsStore.Raise(notification);
        }

        // Engine.pas:8819
        public void ResetMsgCount()
        {
            EMsg Msg = default;
            InitMsgEx(ref Msg, 1, this, FMsgCount + 1);
            MsgProc(ref Msg, this);
            FMsgCount = 0;
        }

        // Engine.pas:8828
        public int IncMsgCount()
        {
            FMsgCount += 1;
            if (FMsgCount > 268435455)
            {
                ResetMsgCount();
            }

            return FMsgCount;
        }

        // Engine.pas:8835
        public void RegisterEClasses(ReadOnlySpan<EClass> AClasses)
        {
            for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
            {
                if (FOptions.Contains(eoVerifyClasses))
                {
                    VerifyEObjectClass(AClasses[I]);
                }

                var classID = AClasses[I].ClassID;
                if (FClassTable.Find(classID) != null)
                {
                    throw new InvalidOperationException("EObject Class ID double defined!");
                }

                FEClasses.Add(AClasses[I]);
                var classID2 = AClasses[I].ClassID;
                FClassTable.Add(AClasses[I], classID2);
            }
        }

        // Engine.pas:8849
        public void UnregisterEClasses(ReadOnlySpan<EClass> AClasses)
        {
            for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
            {
                FEClasses.Remove(AClasses[I]);
                var classID = AClasses[I].ClassID;
                FClassTable.Delete(classID);
            }
        }

        // Engine.pas:8867
        public EObjectClassList GetDescendantClasses(Type AClass)
        {
            EObjectClassList L;
            L = new EObjectClassList();
            for (int I = 0, last = FEClasses.Count - 1; I <= last; I++)
            {
                if (((EClass)FEClasses[I]).InheritsFrom(AClass))
                {
                    L.Add(FEClasses[I]);
                }
            }

            return L;
        }

        // Engine.pas:8878
        public void ActivateEObject(EObject Obj)
        {
            EMsg Msg = default;
            EMsg.InitMsg(ref Msg, 536870920, this);
            Msg.Payload = this;
            Obj.MsgProc(ref Msg, this);
        }

        // Engine.pas:8895
        public void WriteEObject(Stream Stream, EObject Obj, ProgressEvent AProgress)
        {
            EStorageStream S;
            S = new EWriteStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }

        // Engine.pas:8908
        public void ReadEObject(Stream Stream, EObject Obj, ProgressEvent AProgress)
        {
            EStorageStream S;
            S = new EReadStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }

        // Engine.pas:8921
        public void WritePreviewEObject(Stream Stream, EObject Obj, ProgressEvent AProgress)
        {
            EStorageStream S;
            S = new EWriteStorageStream(Stream, this, true);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }

        // Engine.pas:8934
        public void ReadPreviewEObject(Stream Stream, EObject Obj, ProgressEvent AProgress)
        {
            EStorageStream S;
            S = new EReadStorageStream(Stream, this, true);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }

        // Engine.pas:8947
        public void StoreEObject(Stream Stream, EObject Obj, ProgressEvent AProgress)
        {
            EStorageStream S;
            S = new EWriteStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwCreateEObject(ref Obj, null, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }

        // Engine.pas:8960
        public EObject RetrieveEObject(Stream Stream, ProgressEvent AProgress)
        {
            EObject Result = default;
            EStorageStream S;
            S = new EReadStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwCreateEObject(ref Result, null, 0);
            }
            finally
            {
                S?.Dispose();
            }

            return Result;
        }

        // Engine.pas:8973
        // The file goes through the game's file boundary with the caller's profile. A refused
        // preflight raises EGameFileError before an object is created (own-formats.md 4.1).
        public EObject RetrieveFromFile(string FileName, ProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            return RetrieveFromImage(FileBoundary.LoadOrRaise(FileName, Profile), AProgress);
        }

        // Retrieves the object of an engine image whose preflight has passed.
        public EObject RetrieveFromImage(byte[] Image, ProgressEvent AProgress)
        {
            EObject Result = default;
            MemoryStream M;
            M = ImageStream(Image);
            try
            {
                Result = RetrieveEObject(M, AProgress);
            }
            finally
            {
                M?.Dispose();
            }

            return Result;
        }

        // Engine.pas:8985
        // The engine writes the image in memory. The boundary converts, validates and replaces the
        // file only after the whole document is serialized.
        public void StoreInFile(string FileName, EObject Obj, ProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            MemoryStream M;
            M = new MemoryStream();
            try
            {
                StoreEObject(M, Obj, AProgress);
                FileBoundary.Save(FileName, Profile, M.GetBuffer().AsSpan(0, (int)M.Length).ToArray());
            }
            finally
            {
                M?.Dispose();
            }
        }

        // An engine image in a memory stream at position 0, for the routines that read it.
        public static MemoryStream ImageStream(byte[] Image)
        {
            MemoryStream M = new MemoryStream();
            M.WriteBuffer(Image);
            M.Position = 0;
            return M;
        }

        // The in-scope form of WriteToFile for the hero and item lists (own-formats.md 4.1). The
        // three-argument form below stays the original byte path of the excluded PFS callers.
        public void WriteToFile(string FileName, EObject Obj, ProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            MemoryStream M;
            M = new MemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                FileBoundary.Save(FileName, Profile, M.GetBuffer().AsSpan(0, (int)M.Length).ToArray());
            }
            finally
            {
                M?.Dispose();
            }
        }

        // The in-scope form of ReadFromFile. A refused preflight raises before Obj is read.
        public void ReadFromFile(string FileName, EObject Obj, ProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            MemoryStream M;
            M = ImageStream(FileBoundary.LoadOrRaise(FileName, Profile));
            try
            {
                ReadEObject(M, Obj, AProgress);
            }
            finally
            {
                M?.Dispose();
            }
        }

        // Engine.pas:8997
        public void WriteToFile(string FileName, EObject Obj, ProgressEvent AProgress)
        {
            MemoryStream M;
            Stream F = default;
            M = new MemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                F = OpenFile(FileName, 65535);
                try
                {
                    M.Position = 0;
                    F.CopyFrom(M, (int)M.Length);
                }
                finally
                {
                    F?.Dispose();
                }
            }
            finally
            {
                M?.Dispose();
            }
        }

        // Engine.pas:9017
        public void ReadFromFile(string FileName, EObject Obj, ProgressEvent AProgress)
        {
            MemoryStream M;
            Stream F;
            M = new MemoryStream();
            F = OpenFile(FileName, 0 | 32);
            try
            {
                M.LoadFromStream(F);
            }
            finally
            {
                F?.Dispose();
            }

            M.Position = 0;
            try
            {
                ReadEObject(M, Obj, AProgress);
            }
            finally
            {
                M?.Dispose();
            }
        }

        // Engine.pas:9037
        public void WriteToFileCRC(string FileName, EObject Obj, ProgressEvent AProgress)
        {
            int CRC1 = default;
            int CRC2 = default;
            MemoryStream M;
            Stream F = default;
            M = new MemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                var size = (int)M.Length;
                CRC1 = EngineServices.GetCRC32(M.GetBuffer(), size);
                M.Write(Bytes.Of(ref CRC1, 4));
                var mSize = (int)M.Length;
                CRC2 = EngineServices.GetCRC32(M.GetBuffer(), mSize);
                F = OpenFile(FileName, 65535);
                try
                {
                    F.Write(Bytes.Of(ref CRC2, 4));
                    var size2 = (int)M.Length;
                    F.Write(M.GetBuffer().AsSpan(0, size2));
                }
                finally
                {
                    F?.Dispose();
                }
            }
            finally
            {
                M?.Dispose();
            }
        }

        // Engine.pas:9061
        public bool ReadFromFileCRC(string FileName, EObject Obj, ProgressEvent AProgress)
        {
            MemoryStream M;
            Stream F;
            int CRC2;
            int CRC1;
            bool Result = false;
            M = new MemoryStream();
            F = OpenFile(FileName, 0 | 32);
            try
            {
                M.LoadFromStream(F);
                M.Position = 0;
                if (M.Length > 8)
                {
                    CRC1 = MemoryMarshal.Read<int>(M.GetBuffer().AsSpan(0, 4));
                    var size = (int)M.Length;
                    CRC2 = MemoryMarshal.Read<int>(M.GetBuffer().AsSpan(size - 4, 4));
                    var mSize = (int)M.Length;
                    bool condition = CRC1 == EngineServices.GetCRC32(M.GetBuffer().AsSpan(4), mSize - 4);
                    if (condition)
                    {
                        var size2 = (int)M.Length;
                        condition = CRC2 == EngineServices.GetCRC32(M.GetBuffer().AsSpan(4), size2 - 8);
                    }

                    if (condition)
                    {
                        M.Position = 4;
                        ReadEObject(M, Obj, AProgress);
                        Result = true;
                    }
                }
            }
            finally
            {
                F?.Dispose();
                M?.Dispose();
            }

            return Result;
        }

        // Engine.pas:9091
        public void ReadTextFromFile(string FileName, EObject Obj)
        {
            EStorageStream S;
            Stream F;
            F = OpenFile(FileName, 0 | 32);
            try
            {
                S = new EReadTextStorageStream(F, this, 0);
                try
                {
                    S.rwEObject(Obj, 0);
                }
                finally
                {
                    S?.Dispose();
                }
            }
            finally
            {
                F?.Dispose();
            }
        }

        // Engine.pas:9109
        public void WriteTextToFile(string FileName, EObject Obj)
        {
            EStorageStream S;
            Stream F;
            F = OpenFile(FileName, 65535);
            try
            {
                S = new EWriteTextStorageStream(F, this);
                try
                {
                    S.rwEObject(Obj, 0);
                }
                finally
                {
                    S?.Dispose();
                }
            }
            finally
            {
                F?.Dispose();
            }
        }

        public EObjectClassList EClasses { get => FEClasses; }
        public IntegerHashTable256 ClassTable { get => FClassTable; }
        public EObjectEditorList Editors { get => FEditors; }
        public EControlStyleNode ControlNode { get => FControlNode; }
        public StringList DebugIDs { get => FDebugIDs; }

        public event NotifyEvent<EngineDebugNotification> DebugEvents
        {
            add => FDebugEventsStore.Add(value);
            remove => FDebugEventsStore.Remove(value);
        }

        public string StartupDirectory { get => FStartupDirectory; set => SetStartupDirectory(value); }
        public Set<EngineModeElement> Options { get => FOptions; set => FOptions = value; }

        // Engine.pas:1126
        public bool VerifyEObjectClass(EClass type)
        {
            // The native allocator byte total has no corresponding managed heap measure.
            // Exercise the same constructor and destructor without comparing GC generations.
            type.Create(null)?.Dispose();
            return true;
        }

        internal const EngineModeElement eoVerifyClasses = 0;

        private static void InitMsgEx(ref EMsg Msg, int MsgID, EObject Sender, int Seq)
        {
            Msg = default;
            Msg.MsgID = MsgID;
            Msg.Seq = Seq;
            Msg.Sender = Sender;
            Msg.Result = -1;
        }

        private static readonly string UserStr = "User";

        internal static void UnregisterSoundEngineClasses(Engine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<SFXSequence>(), EClass.Of<SFXLibrary>(), EClass.Of<SFX>()]);
        }

        internal static void UnRegisterStorms(Engine AEngine)
        {
            SpellControl Spells;
            Spells = ((AoWEngine)AEngine).FGameSet.FSpellControl;
            Spells.UnregisterSpellID(3);
            Spells.UnregisterSpellID(5);
            Spells.UnregisterSpellID(6);
            Spells.UnregisterSpellID(7);
            Spells.UnregisterSpellID(4);
            Spells.UnregisterSpellID(9);
        }

        internal static void InitializePlugIns(Engine AEngine)
        {
            for (int I = 0, last = EngineHost.PlugIns.Count - 1; I <= last; I++)
            {
                ((PlugIn)EngineHost.PlugIns[I]).Initialize(AEngine);
            }
        }

        internal static void FinalizePlugIns(Engine AEngine)
        {
            for (int I = 0, last = EngineHost.PlugIns.Count - 1; I <= last; I++)
            {
                ((PlugIn)EngineHost.PlugIns[I]).Finalize(AEngine);
            }
        }

        internal static Engine CopyEngine;

        internal static void UnRegisterMountain(Engine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<MountainMO>(), EClass.Of<MountainMOR>(), EClass.Of<RaiseTerrainTE>(), EClass.Of<RaisedMountain>(), EClass.Of<RaisedMountain.Resource>()]);
            // { Original unregisters spell 12, although Raise Terrain registers as spell 19. }
            ((AoWEngine)AEngine).FGameSet.FSpellControl.UnregisterSpellID(12);
        }

        internal static void RegisterBorderHexagon(Engine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<BorderHexagon>(), EClass.Of<BorderHexagon.Resource>()]);
        }

        internal static void RegisterHexagon(Engine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<Hexagon>(), EClass.Of<HexagonResource>()]);
        }

        internal static void RegisterIsometricHexagon(Engine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<IsometricHexagon>(), EClass.Of<IsometricHexagonResource>(), EClass.Of<UpperIsometricHexagon>(), EClass.Of<UpperIsometricHexagon.Resource>(), EClass.Of<LowerIsometricHexagon>(), EClass.Of<LowerIsometricHexagon.Resource>()]);
        }

        internal static void RegisterRoad(Engine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<Road>(), EClass.Of<Road.Resource>()]);
        }

        internal static void RegisterTerrain(Engine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TerrainMO>(), EClass.Of<TerrainMOR>()]);
        }

        internal static void RegisterHill(Engine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<HillMO>()]);
        }

        internal static void RegisterAltar(Engine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<Altar>(), EClass.Of<AltarResource>()]);
        }

        internal static void RegisterRandomNode(Engine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<RandomNode>(), EClass.Of<RandomNode.Resource>()]);
        }

        internal static void UnRegisterHolyGround(Engine Engine)
        {
            Engine.UnregisterEClasses([EClass.Of<HolyGround>(), EClass.Of<HolyGround.Resource>(), EClass.Of<HolyGroundSpellTE>()]);
            ((AoWEngine)Engine).FGameSet.FSpellControl.UnregisterSpellID(13);
        }

        internal static void UnRegisterOverlayObject(Engine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<OverlayObject>(), EClass.Of<OverlayObject.Resource>()]);
        }

    }
}

namespace Aow.Game.Engine.Storage
{

    [LegacyObject]
    // the base of every storage stream. The rw* methods do nothing here. the subclasses read,
    // write, compare or translate
    public partial class EStorageStream : IDisposable
    {

        internal Engine FEngine;
        internal Stream FStream;
        internal bool FErrorHandled;
        internal PropertyTable FPropertyTable;
        internal StorageStreamError FOnError;
        internal ProgressEvent FOnProgress;

        // Engine.pas:1184
        public virtual Set<StorageModeElement> StorageMode()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:9502
        public virtual Stream OpenPropertyStream(int ID) => null;

        // Engine.pas:1186
        public virtual void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:1187
        public virtual void rwEObject(EObject Obj, int ID)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:1188
        public virtual void rwEObjectStream(EObject Obj, int ID)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:1189
        public virtual void rwChildren(ECustomNode Node, int ID)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:9559
        public virtual void rwStreamProc(int ID, StreamProc LoadProc, StreamProc SaveProc)
        {
        }

        // Engine.pas:9507
        public virtual void rwData(Span<byte> Buffer, int Size, int ID)
        {
        }

        // Engine.pas:9511
        public virtual void rwString(ref string S, int ID)
        {
        }

        // Engine.pas:9515
        public virtual void rwStrings(StringCollection Strings, int ID)
        {
        }

        // Engine.pas:9519
        public virtual void rwStringsEx(StringCollection Strings, int ID)
        {
        }

        // Engine.pas:9523
        public virtual void rwInteger(ref int I, int ID)
        {
        }

        // Engine.pas:9527
        public virtual void rwByte(ref byte B, int ID)
        {
        }

        // Engine.pas:9535
        public virtual void rwWord(ref ushort W, int ID)
        {
        }

        // Engine.pas:9539
        public virtual void rwBool(ref bool B, int ID)
        {
        }

        // Engine.pas:9531
        public virtual void rwShortint(ref sbyte S, int ID)
        {
        }

        // Engine.pas:9543
        public virtual void rwIntegerEx(ref int I, int Default, int ID)
        {
        }

        // Engine.pas:9547
        public virtual void rwByteEx(ref byte B, byte Default, int ID)
        {
        }

        // Engine.pas:9551
        public virtual void rwWordEx(ref ushort W, ushort Default, int ID)
        {
        }

        // Engine.pas:9555
        public virtual void rwShortintEx(ref sbyte S, sbyte Default, int ID)
        {
        }

        // Engine.pas:9489
        public EStorageStream(Stream AStream, Engine AEngine)
        {
            CaptureAddress.Assign(this);
            FEngine = AEngine;
            FStream = AStream;
            FErrorHandled = false;
        }

        // Engine.pas:9497
        public void ShowProgress(EObject Sender, string Msg, int Percent)
        {
            if (FOnProgress != null)
            {
                FOnProgress(Sender, Msg, Percent);
            }
        }

        public Engine Engine { get => FEngine; }
        public Stream Stream { get => FStream; }
        public PropertyTable PropertyTable { get => FPropertyTable; }
        public StorageStreamError OnError { get => FOnError; set => FOnError = value; }
        public ProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

        // Delphi var parameters alias a descendant reference through a base-class cast.
        public void rwCreateEObject<T>(ref T value, ECustomNode owner, int id) where T : class
        {
            EObject result = (EObject)(object)value;
            try { rwCreateEObject(ref result, owner, id); }
            finally { value = (T)(object)result; }
        }

        public virtual void Dispose()
        {
        }

        public delegate void StorageStreamError(EStorageStream StorageStream, string Message, ref bool Handled);
    }
}

namespace Aow.Game.Engine.Nodes
{

    [LegacyObject]
    // { Engine.TEObjectEditorList: the editor descriptors an engine registers, one per editor class id
    //   (an EditorClass per editor class since 9.4-4, a list of editor metaclasses before) }
    public partial class EObjectEditorList
    {
        private readonly List<EditorClass> FList = new();

        public int Count { get => FList.Count; }
        public EditorClass this[int Index] { get => FList[Index]; }

        // Engine.pas:8356
        public void RegisterClass(EditorClass AClass)
        {
            if (FindEditor(AClass.EditorClassID) == null)
            {
                FList.Add(AClass);
            }
            else
            {
                throw new InvalidOperationException("EObjectEditor Double Defined!");
            }
        }

        // Engine.pas:8364
        public EditorClass FindEditor(int AClassID)
        {
            EditorClass Result;
            int I;
            I = 0;
            while (I != Count && AClassID != FList[I].EditorClassID)
            {
                I += 1;
            }

            if (I != Count)
            {
                Result = FList[I];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        public EObjectEditorList()
        {
            CaptureAddress.Assign(this);
        }
    }

    // FResourceList, FResourceID, FEditID (the edit id of the resource grid that placed it),
    // FEditIndex (its cell in that grid). all three integers are persisted (ids 1.3)
    public partial class EResource : EObject
    {
        public new const int ClassID = 1024;

        public EResource(ECustomNode owner) : base(owner)
        { }

        internal ECustomNode FResourceList;
        internal int FResourceID;
        internal int FEditID;
        internal int FEditIndex;

        // Engine.pas:8513
        public override void SetOwner(ECustomNode AOwner)
        {
            base.SetOwner(AOwner);
            FResourceList = AOwner;
        }

        // Engine.pas:8542
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FResourceID, 1);
            Stream.rwInteger(ref FEditID, 2);
            Stream.rwInteger(ref FEditIndex, 3);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Int("ResourceID", ref FResourceID);
            persister.Int("EditID", ref FEditID);
            persister.Int("EditIndex", ref FEditIndex);
        }

        // Engine.pas:8519
        public override void SetArrayOwner(ECustomNode AOwner, int Index)
        {
            base.SetArrayOwner(AOwner, Index);
            FResourceList = AOwner;
        }

        // Engine.pas:8533
        public virtual LibraryImage GetCellImage() => null;

        // Engine.pas:8538
        public virtual void SetCellImage(LibraryImage Value)
        {
        }

        // Engine.pas:8550
        public virtual int ResourceUserClassID() => 0;

        // Engine.pas:8555
        public virtual EObject CreateResourceUser()
        {
            EObject Result;
            int UserClassID;
            UserClassID = ResourceUserClassID();
            if (UserClassID != 0)
            {
                Result = GetEngine().CreateEObject(UserClassID, null);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:8525
        public virtual void Draw(Canvas Canvas, Rect Rect, Draw.Images.BltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
        {
            Rect R = default;
            LibraryDrawing.BltLibraryImage(Canvas, GetCellImage(), Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref R);
        }

        public ECustomNode ResourceList { get => FResourceList; set => FResourceList = value; }
        public int EditID { get => FEditID; set => FEditID = value; }
        public int EditIndex { get => FEditIndex; set => FEditIndex = value; }
        public int ResourceID { get => FResourceID; set => FResourceID = value; }
        public LibraryImage CellImage { get => GetCellImage(); set => SetCellImage(value); }

        // Engine.pas:8508
        public virtual string ResourceEditName() => GetType().Name;

    }

    public struct EMsg
    {
        public int MsgID;
        public int Seq;
        public EObject Sender;
        public EMsgParams Param;
        public MsgArg Payload;

        public FixedArray_int_4 Data;
        public int Result;

        internal static void InitMsg(ref EMsg Msg, int MsgID, EObject Sender)
        {
            Msg = default;
            Msg.MsgID = MsgID;
            Msg.Sender = Sender;
            Msg.Result = -1;
        }

    }

    // The parameter slots of an engine message. The count is the arity of the message protocol
    // (Engine.pas TEMsg.Param), not a count of content.
    [InlineArray(Arity)]
    public struct EMsgParams
    {
        public const int Arity = 3;

        private MsgArg element0;
    }
}

namespace Aow.Game.Engine
{
    public delegate void ProgressEvent(EObject Sender, string Message, int Progression);

    public partial class DoublePrioriyIDException : System.Exception
    {
        // SysUtils.pas:174
        public DoublePrioriyIDException(string Msg) : base(Msg)
        {
        }
    }
}

namespace Aow.Game.Engine.Storage
{

    // the write stream's data buffer
    public partial class EBufferStream : FastMemoryStream
    {
        // System.pas:111
        public EBufferStream()
        { }
    }

    [LegacyObject]
    public partial class CompressStream : MemoryStream
    {
        public CompressStream()
        {
            CaptureAddress.Assign(this);
        }

        // Engine.pas:1715
        public void LoadFromStream(Stream Stream)
        {
            bool Compressed = default;
            FixedArray_byte_4 Magic = default;
            ZLibStream D;
            byte[] Buffer;
            int Count;
            Stream.ReadBytes(((Span<byte>)Magic).Slice(0, 4));
            Stream.ReadBytes(Bytes.Of(ref Compressed, 1));
            if (!ArrayEquals<FixedArray_byte_4, byte>(Magic, CompressMagic))
            {
                Compressed = false;
                Stream.Seek(0, SeekOrigin.Begin);
            }

            if (Compressed)
            {
                Buffer = default;
                Count = -1;
                Array.Resize(ref Buffer, 8096);
                D = new ZLibStream(Stream, CompressionMode.Decompress, true);
                try
                {
                    while (Count != 0)
                    {
                        Count = D.Read(Buffer.AsSpan(0, 8096));
                        if (Count != 0)
                        {
                            this.WriteBuffer(Buffer.AsSpan(0, Count));
                        }
                    }
                }
                finally
                {
                    Array.Resize(ref Buffer, 0);
                    D?.Dispose();
                }
            }
            else
            {
                this.CopyFrom(Stream, (int)(Stream.Length - Stream.Position));
            }

            Position = 0;
        }

        // Engine.pas:1752
        public int SaveToStreamEx(Stream Stream, CompressLevel Level)
        {
            ZLibStream C;
            int StartSize;
            Position = 0;
            int Result = 100;
            StartSize = (int)Stream.Length;
            Stream.Write(((ReadOnlySpan<byte>)CompressMagic).Slice(0, 4));
            Stream.Write(Bytes.Of(ref Level, 1));
            if (Level != CompressLevel.None)
            {
                C = new ZLibStream(Stream, ZLibLevels.ForByte((byte)Level), true);
                try
                {
                    C.CopyFrom(this, 0);
                    Result = Round((double)((double)1 - (double)(Stream.Length - StartSize) / Length) * 100);
                }
                finally
                {
                    C?.Dispose();
                }
            }
            else
            {
                Stream.CopyFrom(this, 0);
            }

            return Result;
        }

        // Engine.pas:1776
        public void SaveToStream(Stream Stream)
        {
            SaveToStreamEx(Stream, CompressLevel.Default);
        }

        // Engine.pas:1781
        public int SaveToFileEx(string FileName, CompressLevel Level)
        {
            int Result = default;
            Stream F;
            F = OpenFile(FileName, 65535);
            try
            {
                Result = SaveToStreamEx(F, Level);
            }
            finally
            {
                F?.Dispose();
            }

            return Result;
        }

        // Engine.pas:1793
        public void SaveToFile(string FileName)
        {
            Stream F;
            F = OpenFile(FileName, 65535);
            try
            {
                SaveToStreamEx(F, CompressLevel.Default);
            }
            finally
            {
                F?.Dispose();
            }
        }

        // An engine image the file boundary prepared: the inflated payload, at position 0.
        public void LoadImage(byte[] Image)
        {
            this.WriteBuffer(Image);
            Position = 0;
        }

        // Engine.pas:1805
        public void LoadFromFile(string FileName)
        {
            Stream F;
            F = OpenFile(FileName, 0 | 32);
            try
            {
                LoadFromStream(F);
            }
            finally
            {
                F?.Dispose();
            }
        }

        private static FixedArray_byte_4 CompressMagic = FixedArray_byte_4.From("CFS\0"u8);

        public enum CompressLevel : byte
        {
            None = 0, Fastest = 1, Default = 2, Max = 3
        }
    }
}

namespace Aow.Game.Engine
{

    [LegacyObject]
    // FRefCount, FEngines, FCounts. A plug-in registers itself in PlugIns on creation and counts
    // the engines it is initialized for
    public partial class PlugIn : IDisposable
    {
        internal int FRefCount;
        internal DelphiList FEngines;
        internal IntegerList FCounts;

        // Engine.pas:9144
        public PlugIn()
        {
            CaptureAddress.Assign(this);
            FEngines = new DelphiList();
            FCounts = new IntegerList();
            RegisterPlugIn(this);
            FRefCount = 1;
        }

        // Engine.pas:9153
        public virtual void Dispose()
        {
            while (FEngines.Count != 0)
            {
                Finalize((Engine)FEngines[FEngines.Count - 1]);
            }

            FEngines?.Dispose();
            FCounts?.Dispose();
            UnregisterPlugIn(this);
        }

        // Engine.pas:9162
        public void AddRef()
        {
            FRefCount += 1;
        }

        // Engine.pas:9167
        public void Release()
        {
            FRefCount -= 1;
            if (FRefCount == 0)
            {
                Dispose();
            }
        }

        // Engine.pas:9173
        public virtual string Name() => "Abstract PlugIn";

        // Engine.pas:9178
        public virtual bool Initialize(Engine AEngine)
        {
            bool Result;
            int I;
            AddRef();
            I = FEngines.IndexOf(AEngine);
            if (I == -1)
            {
                FEngines.Add(AEngine);
                FCounts.Add(1);
                Result = true;
            }
            else
            {
                var item = FCounts[I];
                FCounts[I] = item + 1;
                Result = false;
            }

            return Result;
        }

        // Engine.pas:9197
        public virtual bool Finalize(Engine AEngine)
        {
            bool Result;
            int I;
            I = FEngines.IndexOf(AEngine);
            if (I != -1)
            {
                if (FCounts[I] == 0)
                {
                    throw new ArgumentException("Invalid PlugIn Finalize Usage");
                }

                if (FCounts[I] == 1)
                {
                    FEngines.Remove(AEngine);
                    FCounts.Delete(I);
                    Result = true;
                }
                else
                {
                    var item = FCounts[I];
                    FCounts[I] = item - 1;
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }

            Release();
            return Result;
        }

        public int RefCount { get => FRefCount; }

        private static void RegisterPlugIn(PlugIn PlugIn)
        {
            bool flag = false;
            int num = 0;
            while (!flag && num != EngineHost.PlugIns.Count)
            {
                flag = ((PlugIn)EngineHost.PlugIns[num]).Name() == PlugIn.Name();
                num++;
            }
            if (!flag)
            {
                EngineHost.PlugIns.Add((object)PlugIn);
                return;
            }
            throw new InvalidOperationException("PlugIn " + PlugIn.Name() + " already registered");
        }

        private static void UnregisterPlugIn(PlugIn PlugIn)
        {
            if (EngineHost.PlugIns.IndexOf((object)PlugIn) != -1)
            {
                EngineHost.PlugIns.Remove((object)PlugIn);
                return;
            }
            throw new KeyNotFoundException("PlugIn not found");
        }

        // Engine.pas:550

        // Engine.pas:550

    }

    public partial class PlugInMissingException : System.Exception
    {
        // SysUtils.pas:174
        public PlugInMissingException(string Msg) : base(Msg)
        {
        }
    }

    // FEngine, FUsedPlugIns. the names of the plug-ins an engine uses, initialized and finalized as
    // the list changes
    public partial class PlugInControl : EObject
    {
        internal Engine FEngine;
        internal StringList FUsedPlugIns;

        // Engine.pas:9279
        internal void SetUsedPlugIns(StringCollection Value)
        {
            FUsedPlugIns.Assign(Value);
        }

        // Engine.pas:9284
        internal StringCollection GetUsedPlugIns() => FUsedPlugIns;

        // Engine.pas:9289
        internal void OnChange(object Sender)
        {
            InitializePlugIns();
        }

        // Engine.pas:9294
        internal void OnChanging(object Sender)
        {
            FinalizePlugIns();
        }

        // Engine.pas:9299
        internal void InitializePlugIns()
        {
            int[] I__cell = new int[1];
            ref int I = ref I__cell[0];
            PlugIn P;
            I = 0;
            int last = FUsedPlugIns.Count - 1;
            for (; I <= last; I += 1)
            {
                P = FindPlugIn(FUsedPlugIns[I]);
                if (P != null)
                {
                    P.Initialize(FEngine);
                }
                else
                {
                    throw new PlugInMissingException("PlugIn Missing (" + FUsedPlugIns[I] + ")");
                }
            }
        }

        // Engine.pas:9319
        internal void FinalizePlugIns()
        {
            PlugIn P;
            for (int I = 0, last = FUsedPlugIns.Count - 1; I <= last; I++)
            {
                P = FindPlugIn(FUsedPlugIns[I]);
                if (P != null)
                {
                    P.Finalize(FEngine);
                }
            }
        }

        // Engine.pas:9252
        public PlugInControl(ECustomNode owner) : base(owner)
        {
            FUsedPlugIns = new StringList();
            FUsedPlugIns.Sorted = true;
            FUsedPlugIns.OnChange = OnChange;
            FUsedPlugIns.OnChanging = OnChanging;
            FEngine = null;
        }

        // Engine.pas:9262
        public override void Dispose()
        {
            FUsedPlugIns.Clear();
            FUsedPlugIns?.Dispose();
            base.Dispose();
        }

        // Engine.pas:9269
        public override Engine GetEngine() => FEngine;

        // Engine.pas:9274
        public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared);

        // Engine.pas:9331
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(StorageModeElement.Translate))
            {
                Stream.rwStrings(FUsedPlugIns, 1);
            }
        }

        public Engine Engine { get => FEngine; set => FEngine = value; }
        public StringCollection UsedPlugIns { get => GetUsedPlugIns(); set => SetUsedPlugIns(value); }

        private static PlugIn FindPlugIn(string Name)
        {
            PlugIn tPlugIn = null;
            if (EngineHost.PlugIns != null)
            {
                for (int i = 0; i != EngineHost.PlugIns.Count; i++)
                {
                    if (tPlugIn != null)
                    {
                        break;
                    }
                    if (((PlugIn)EngineHost.PlugIns[i]).Name() == Name)
                    {
                        tPlugIn = (PlugIn)EngineHost.PlugIns[i];
                    }
                }
            }
            return tPlugIn;
        }
    }
}

namespace Aow.Game.Engine.Nodes
{

    // the child list of a TENode
    public partial class EObjectList : DelphiList
    {
        // Engine.pas:7136
        protected internal EObject GetEObject(int Index) => (EObject)this[Index];

        // Engine.pas:7141
        protected internal void SetEObject(int Index, EObject Value)
        {
            this[Index] = Value;
        }

        // Engine.pas:7146
        public EObject FindObject(int AClassID)
        {
            EObject Result;
            int I;
            bool Found;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                Found = List[I] != null && AClassID == ((EObject)List[I]).EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = (EObject)List[I - 1];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:7161
        public int FindObjectIndex(int AClassID)
        {
            int Result;
            int I;
            bool Found;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                Found = List[I] != null && AClassID == ((EObject)List[I]).EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = I - 1;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:7176
        public void ClearList()
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                // { the original tests List^[Count], one past the end, not List^[I] }
                if (List[Count] != null)
                {
                    ((List[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                }

                Delete(I);
            }
        }

        // Engine.pas:7188
        public bool AllHaveCID(int AClassID)
        {
            int I;
            bool OK;
            I = 0;
            OK = true;
            while (I < Count)
            {
                OK = List[I] == null || AClassID == ((EObject)List[I]).EClass.ClassID;
                I += 1;
            }

            return OK && Count != 0;
        }

        public sealed class EObjectsAccessor(EObjectList target)
        {
            public EObject this[int Index] { get => target.GetEObject(Index); set => target.SetEObject(Index, value); }
        }

        public EObjectsAccessor EObjects => new EObjectsAccessor(this);
    }

    // FChildren
    public partial class ENode : ECustomNode
    {
        // Engine.pas:2861
        public new const int ClassID = 256;

        // Engine.pas:2807
        public ENode(ECustomNode owner) : this(null, owner) { }

        private protected ENode(ConstructionContext context, ECustomNode owner) : base(context, owner)
        {
            FChildren = new EObjectList();
        }

        public EObjectList FChildren;

        // Engine.pas:2833
        public override void Clear()
        {
            int I;
            EObject C;
            I = Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (EObject)FChildren.List[I];
                if (C != null && C.Owner == this)
                {
                    if (!C.GetControlStyle().Contains(EngineServices.csShared))
                    {
                        C.Dispose();
                    }
                    else
                    {
                        C.Clear();
                    }
                }
            }

            if (Count != 0)
            {
                I = Count - 1;
                for (; I >= 0; I -= 1)
                {
                    C = (EObject)FChildren.List[I];
                    if (C != null)
                    {
                        if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.csShared)))
                        {
                            RemoveChild(C);
                        }
                    }
                }

                I = FChildren.Count - 1;
                while (I >= 0 && FChildren.List[I] == null)
                {
                    FChildren.Delete(I);
                    I -= 1;
                }
            }
        }

        // Engine.pas:2866
        public override EObject GetChild(int Index) => (EObject)FChildren.List[Index];

        // Engine.pas:2871
        public override void SetChild(int Index, EObject Child)
        {
            while (Index >= Count)
            {
                FChildren.Add(null);
            }

            if (FChildren.List[Index] != null && ((EObject)FChildren.List[Index]).Owner == this)
            {
                ((FChildren.List[Index] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            }

            FChildren.List[Index] = Child;
        }

        // Engine.pas:2879
        public override int GetCount() => FChildren.Count;

        // Engine.pas:2819
        public override void DestroyAllChildren()
        {
            EObject C;
            Clear();
            if (FChildren.Count != 0)
            {
                for (int I = FChildren.Count - 1; I >= 0; I--)
                {
                    C = (EObject)FChildren.List[I];
                    if (C != null && C.Owner == this)
                    {
                        C.Dispose();
                    }
                }
            }
        }

        // Engine.pas:2884
        public override void SendMsg(ref EMsg Msg, EObject Sender)
        {
            var children = FChildren;
            if (GetControlStyle().Contains(EngineServices.csKeepIndex))
            {
                if ((Msg.MsgID & 268435456) != 0)
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children.List[I];
                                if (node.GetLastMsg() != Msg.Seq)
                                {
                                    node.SetLastMsg(Msg.Seq);
                                    node.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children[I];
                                if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                                {
                                    node.SetLastMsg(Msg.Seq);
                                    node.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children.List[I];
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children[I];
                                if (node.Owner == this)
                                {
                                    node.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 268435456) != 0)
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children.List[I];
                            if (node.GetLastMsg() != Msg.Seq)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children[I];
                            if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children.List[I];
                            node.MsgProc(ref Msg, this);
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children[I];
                            if (node.Owner == this)
                            {
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:2981
        public override int RemoveChild(EObject Child)
        {
            int Result;
            if (GetControlStyle().Contains(EngineServices.csKeepIndex))
            {
                Result = FChildren.IndexOf(Child);
                if (Result != -1)
                {
                    FChildren.List[Result] = null;
                }
            }
            else
            {
                Result = FChildren.Remove(Child);
            }

            return Result;
        }

        // Engine.pas:2992
        public override int AddChild(EObject Child)
        {
            FChildren.Add(Child);
            return FChildren.Count - 1;
        }

        // Engine.pas:2998
        public override EObject FindChild(int AClassID)
        {
            EObject Result;
            int I;
            bool Found;
            EObject C;
            I = 0;
            Found = false;
            C = null;
            while (!Found && I < FChildren.Count)
            {
                C = (EObject)FChildren.List[I];
                Found = C != null && AClassID == C.EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = C;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:3016
        public override int FindChildIndex(EObject Child)
        {
            int Result = Count - 1;
            while (Result >= 0 && Child != (EObject)FChildren.List[Result])
            {
                Result -= 1;
            }

            return Result;
        }

        // Engine.pas:2813
        public override void Dispose()
        {
            base.Dispose();
            FChildren?.Dispose();
        }

        // Engine.pas:3022
        public void DestroyEObjects(int AClassID)
        {
            EObject C;
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (EObject)FChildren.List[I];
                if (C != null && AClassID == C.EClass.ClassID && C.Owner == this)
                {
                    C.Dispose();
                }
            }
        }

        // Engine.pas:631
    }

    // FTick, FCount, FTimerLists[1.10]. Children sit in one of ten lists and get TimerEvent every
    // tick, every second tick. every tenth
    public partial class ETimerNode : ECustomNode
    {
        // Engine.pas:1836
        public new const int ClassID = 264;

        internal int FTick;
        internal int FCount;
        internal FixedArray_Aow_Rtl_TList_10 FTimerLists;

        // Engine.pas:1856
        internal int FindTimerList(EObject Child)
        {
            int I;
            int Result = -1;
            I = 0;
            while (Result == -1 && I < 10)
            {
                I += 1;
                // { the original indexes the lists with Result (-1 on the first round), not with I }
                Result = FTimerLists[Result - 1].IndexOf(Child);
            }

            if (Result != -1)
            {
                Result = I;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:1819
        public ETimerNode(ECustomNode owner) : base(owner)
        {
            FCount = 0;
            for (int I = 1; I <= 10; I++)
            {
                var list = new DelphiList();
                FTimerLists[I - 1] = list;
            }
        }

        // Engine.pas:1828
        public override void Dispose()
        {
            base.Dispose();
            for (int I = 1; I <= 10; I++)
            {
                FTimerLists[I - 1]?.Dispose();
            }
        }

        // Engine.pas:1841
        public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared, EngineServices.csReserved2);

        // Engine.pas:1846
        public override int GetCount() => FCount;

        // Engine.pas:1851
        public DelphiList GetTimerList(sbyte Timer) => FTimerLists[Timer - 1];

        // Engine.pas:1871
        public override int RemoveChild(EObject Child)
        {
            int Result;
            int L;
            L = FindTimerList(Child);
            if (L != -1)
            {
                Result = FTimerLists[L - 1].Remove(Child);
                if (Result != -1)
                {
                    FCount -= 1;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:1885
        public override int AddChild(EObject Child)
        {
            int Result = FTimerLists[1 - 1].Add(Child);
            FCount += 1;
            return Result;
        }

        // Engine.pas:1891
        public override EObject GetChild(int Index)
        {
            int I;
            int Total;
            DelphiList L;
            I = 1;
            L = FTimerLists[1 - 1];
            Total = L.Count;
            while (Index >= Total && I < 10)
            {
                I += 1;
                L = FTimerLists[I - 1];
                Total += L.Count;
            }

            L = FTimerLists[I - 1];
            Total -= L.Count;
            return (EObject)L.List[Index - Total];
        }

        // Engine.pas:1910
        public override void SetChild(int Index, EObject Child)
        {
            throw new NotSupportedException("Set Child not supported (TETimerNode)");
        }

        // Engine.pas:1915
        public int AddTimerChild(EObject Child, sbyte Timer)
        {
            int Result = FTimerLists[Timer - 1].Add(Child);
            FCount += 1;
            return Result;
        }

        // Engine.pas:1921
        public int RemoveTimerChild(EObject Child, sbyte Timer)
        {
            int Result = FTimerLists[Timer - 1].Remove(Child);
            if (Result != -1)
            {
                FCount -= 1;
            }

            return Result;
        }

        // Engine.pas:1927
        public void TriggerTimerListEvent(int Timer)
        {
            for (int I = 0, last = FTimerLists[Timer - 1].Count - 1; I <= last; I++)
            {
                ((EObject)FTimerLists[Timer - 1].List[I]).TimerEvent(FTick, this);
            }
        }

        // Engine.pas:1935
        public void TriggerTimerEvent()
        {
            if (FTick > 268435455)
            {
                FTick = FTick & 15;
            }

            FTick += 1;
            TriggerTimerListEvent(1);
            if ((FTick & 1) == 0)
            {
                TriggerTimerListEvent(2);
            }

            if (FTick % 3 == 0)
            {
                TriggerTimerListEvent(3);
            }

            if ((FTick & 3) == 2)
            {
                TriggerTimerListEvent(4);
            }

            if (FTick % 5 == 0)
            {
                TriggerTimerListEvent(5);
            }

            if (FTick % 6 == 0)
            {
                TriggerTimerListEvent(6);
            }

            if (FTick % 7 == 0)
            {
                TriggerTimerListEvent(7);
            }

            if ((FTick & 7) == 4)
            {
                TriggerTimerListEvent(8);
            }

            if (FTick % 9 == 0)
            {
                TriggerTimerListEvent(9);
            }

            if (FTick % 10 == 0)
            {
                TriggerTimerListEvent(10);
            }
        }

        public int Tick { get => FTick; }

    }

    // Engine.TEChangeNotifyNode (16 bytes): +8 FChildren, FUpdateCount. Changed on child changes
    public partial class EChangeNotifyNode : ECustomNode
    {
        // Engine.pas:7246
        public new const int ClassID = 265;

        public EChangeNotifyNode(ECustomNode owner) : base(owner)
        {
            FChildren = new EObjectList();
            FUpdateCount = 0;
        }

        public EObjectList FChildren;
        internal int FUpdateCount;

        // Engine.pas:7212
        public override void Dispose()
        {
            BeginUpdate();
            base.Dispose();
            FChildren?.Dispose();
            FChildren = null;
        }

        // Engine.pas:7220
        public override void Clear()
        {
            int I;
            EObject C;
            I = Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (EObject)FChildren.List[I];
                if (C != null && C.Owner == this)
                {
                    if (!C.GetControlStyle().Contains(EngineServices.csShared))
                    {
                        C.Dispose();
                    }
                    else
                    {
                        C.Clear();
                    }
                }
            }

            I = Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (EObject)FChildren.List[I];
                if (C != null)
                {
                    if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.csShared)))
                    {
                        RemoveChild(C);
                    }
                }
            }

            I = FChildren.Count - 1;
            while (I >= 0 && FChildren.List[I] == null)
            {
                FChildren.Delete(I);
                I -= 1;
            }

            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // Engine.pas:7251
        public override EObject GetChild(int Index) => (EObject)FChildren.List[Index];

        // Engine.pas:7256
        public override void SetChild(int Index, EObject Child)
        {
            while (Index >= Count)
            {
                FChildren.Add(null);
            }

            FChildren[Index] = Child;
            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // Engine.pas:7263
        public override void Assign(EObject Source, Engine AEngine)
        {
            BeginUpdate();
            base.Assign(Source, AEngine);
            EndUpdate();
        }

        // Engine.pas:7270
        public void LockChanged()
        {
            FUpdateCount += 1;
        }

        // Engine.pas:7275
        public void UnlockChanged()
        {
            FUpdateCount -= 1;
        }

        // Engine.pas:7280
        public virtual void Changed()
        {
        }

        // Engine.pas:7284
        public void BeginUpdate()
        {
            FUpdateCount += 1;
        }

        // Engine.pas:7957
        // { Original source line layout for EndUpdate.
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        // }
        public void EndUpdate()
        {
            Assert(FUpdateCount > 0, "Invalid UpdateCount Usage", "Engine.pas", 7959);
            FUpdateCount -= 1;
            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // Engine.pas:7964
        public void Sort(ListSortCompare Compare)
        {
            BeginUpdate();
            FChildren.Sort(Compare);
            EndUpdate();
        }

        // Keeps the order of equal items. Reflecting pool ordering is the only caller.
        public void SortStable(ListSortCompare Compare)
        {
            BeginUpdate();
            FChildren.SortStable(Compare);
            EndUpdate();
        }

        // Engine.pas:7971
        public override void MainReadWrite(EStorageStream Stream)
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                BeginUpdate();
            }

            base.MainReadWrite(Stream);
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                EndUpdate();
            }
        }

        public override void MainPersist(IPersister persister)
        {
            if (persister.Loading)
            {
                BeginUpdate();
            }

            base.MainPersist(persister);
            if (persister.Loading)
            {
                EndUpdate();
            }
        }

        // Engine.pas:7978
        public override int GetCount() => FChildren.Count;

        // Engine.pas:7983
        public override void SendMsg(ref EMsg Msg, EObject Sender)
        {
            var children = FChildren;
            if (GetControlStyle().Contains(EngineServices.csKeepIndex))
            {
                if ((Msg.MsgID & 268435456) != 0)
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children.List[I];
                                if (node.GetLastMsg() != Msg.Seq)
                                {
                                    node.SetLastMsg(Msg.Seq);
                                    node.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children[I];
                                if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                                {
                                    node.SetLastMsg(Msg.Seq);
                                    node.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children.List[I];
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            if (children.List[I] != null)
                            {
                                var node = (EObject)children[I];
                                if (node.Owner == this)
                                {
                                    node.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 268435456) != 0)
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children.List[I];
                            if (node.GetLastMsg() != Msg.Seq)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children[I];
                            if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children.List[I];
                            node.MsgProc(ref Msg, this);
                        }
                    }
                    else
                    {
                        for (int I = children.Count - 1; I >= 0; I--)
                        {
                            var node = (EObject)children[I];
                            if (node.Owner == this)
                            {
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:8080
        public override int RemoveChild(EObject Child)
        {
            int Result;
            if (GetControlStyle().Contains(EngineServices.csKeepIndex))
            {
                Result = FChildren.IndexOf(Child);
                if (Result != -1)
                {
                    FChildren.List[Result] = null;
                }
            }
            else
            {
                Result = FChildren.Remove(Child);
            }

            if (FUpdateCount == 0)
            {
                Changed();
            }

            return Result;
        }

        // Engine.pas:8092
        public override int AddChild(EObject Child)
        {
            FChildren.Add(Child);
            if (FUpdateCount == 0)
            {
                Changed();
            }

            return FChildren.Count - 1;
        }

        // Engine.pas:8099
        public override EObject FindChild(int AClassID)
        {
            EObject Result;
            int I;
            bool Found;
            EObject C;
            I = 0;
            Found = false;
            C = null;
            while (!Found && I < FChildren.Count)
            {
                C = (EObject)FChildren.List[I];
                Found = C != null && AClassID == C.EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = C;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:8117
        public override int FindChildIndex(EObject Child)
        {
            int Result = Count - 1;
            while (Result >= 0 && Child != FChildren.List[Result])
            {
                Result -= 1;
            }

            return Result;
        }

        public int UpdateCount { get => FUpdateCount; }

        // Engine.pas:680

    }

    // FEvents, notified by Changed
    public partial class EChangeNotifyEventList : EChangeNotifyNode
    {

        public EChangeNotifyEventList(ECustomNode owner) : base(owner)
        {
            FEventsStore = new();
        }

        internal EventStore<EChangeNotifyEventList> FEventsStore;

        internal event NotifyEvent<EChangeNotifyEventList> FEvents
        {
            add => FEventsStore.Add(value);
            remove => FEventsStore.Remove(value);
        }

        // Engine.pas:8131
        public override void Dispose()
        {
            FEventsStore.Clear();
            base.Dispose();
        }

        // Engine.pas:8137
        public override void Changed()
        {
            FEventsStore.Raise(this);
        }

        public event NotifyEvent<EChangeNotifyEventList> Events
        {
            add => FEventsStore.Add(value);
            remove => FEventsStore.Remove(value);
        }
    }

    public enum EditResult : byte
    {
        OK = 0,
        Cancel = 1
    }

    public delegate void EditDoneEvent(EditResult EditResult, EObject EObject);

    // editors are classes looked up by the class id of the object (TEObject.Editor). everything is
    // called on the class. The base editor edits nothing: Edit reports a cancel at once
    public partial class EObjectEditor : EObject, IEObjectEditorClass
    {

        public EObjectEditor(ECustomNode owner) : base(owner)
        { }

        // Engine.pas:3036
        // { TEObjectEditor: stubs }
        public static int EditorClassID => 0;

        // Engine.pas:3041
        public static void Edit(EObject Obj, Engine AEngine, EditDoneEvent Done)
        {
            Done(EditResult.Cancel, Obj);
        }

        public enum EditModeElement : byte
        {
            Edit = 0, Cut = 1, Copy = 2, Paste = 3, Delete = 4
        }

    }
}

namespace Aow.Game.Engine.Collections
{

    [LegacyObject]
    public partial class HashList : IDisposable
    {
        internal int FCount;
        internal int FCapacity;
        internal HashItem[] FItems = [];

        // HashTab.PAS:18
        protected internal void SetCapacity(int NewCapacity)
        {
            Assert(NewCapacity >= FCount, "Capacity Error (THashList)", "HashTab.PAS", 20);
            if (NewCapacity != FCapacity)
            {
                Array.Resize(ref FItems, NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // HashTab.PAS:28
        protected internal void Delete(int Index)
        {
            Assert(Index >= 0 && Index < FCount, "Delete Error (THashList)", "HashTab.PAS", 30);
            FCount -= 1;
            if (Index < FCount)
            {
                Array.Copy(FItems, Index + 1, FItems, Index, FCount - Index);
            }
        }

        // HashTab.PAS:36
        protected internal void Add(HashItem Item)
        {
            if (FCapacity == FCount)
            {
                SetCapacity(FCapacity + 16);
            }

            FItems[FCount] = Item;
            FCount += 1;
        }

        // HashTab.PAS:6
        // { The hash list and hash table classes of the Engine unit. The original keeps them in an include
        //   file, D:\GEngine\Engine\inc\HashTab.PAS: the Assert calls carry that file name, and line numbers
        //   counted from the start of Engine.pas (THashList.SetCapacity's Assert is at line 7127 there). The
        //   declarations are in the Engine.pas interface. }
        public virtual void Dispose()
        {
            Clear();
        }

        // HashTab.PAS:12
        public virtual void Clear()
        {
            FCount = 0;
            SetCapacity(0);
        }

        public int Count { get => FCount; }

        public HashList()
        {
            CaptureAddress.Assign(this);
        }

        // FCount, FCapacity, FItems. The hash classes come from an include file of the original,
        // D:\GEngine\Engine\inc\HashTab.PAS, whose name the Assert calls carry
        public struct HashItem
        {
            public IntOrRef Key;
            public object Item;
        }
    }

    // items keyed by an integer
    public partial class IntegerHashList : HashList
    {
        // HashTab.PAS:43
        public void Add(object Item, int Key)
        {
            HashItem H = default;
            if (Find(Key) != null)
            {
                throw new InvalidOperationException("ID already in HashTable");
            }

            Assert(Item != null, "Cannot add nil item to hash table", "HashTab.PAS", 48);
            H.Key = Key;
            H.Item = Item;
            base.Add(H);
        }

        // HashTab.PAS:54
        public new void Delete(int Key)
        {
            int I;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (Key == FItems[I].Key.Value)
                    {
                        base.Delete(I);
                        return;
                    }
                }
                while (!(I == 0));
            }
        }

        // HashTab.PAS:70
        public object Find(int Key)
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (Key == FItems[I].Key.Value)
                {
                    return FItems[I].Item;
                }
            }

            return null;
        }
    }

    [LegacyObject]
    // the abstract table interface
    public partial class IntegerHashTable
    {
        // Engine.pas:771
        public virtual void Add(object Item, int Key)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:772
        public virtual void Delete(int Key)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:773
        public virtual object Find(int Key)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:774
        public virtual void Clear()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        public IntegerHashTable()
        {
            CaptureAddress.Assign(this);
        }
    }

    // 256 lists selected by the low key byte
    public partial class IntegerHashTable256 : IntegerHashTable
    , IDisposable
    {
        internal FixedArray_Aow_Units_Engine_TIntegerHashList_256 FLists;

        // HashTab.PAS:83
        public IntegerHashTable256()
        {
            for (int I = 0; I <= 255; I++)
            {
                var integerHashList = new IntegerHashList();
                FLists[I] = integerHashList;
            }
        }

        // HashTab.PAS:91
        public virtual void Dispose()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I]?.Dispose();
            }
        }

        // HashTab.PAS:99
        public override void Clear()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I].Clear();
            }
        }

        // HashTab.PAS:106
        public override void Add(object Item, int Key)
        {
            FLists[Key & 255].Add(Item, Key);
        }

        // HashTab.PAS:111
        public override void Delete(int Key)
        {
            FLists[Key & 255].Delete(Key);
        }

        // HashTab.PAS:116
        public override object Find(int Key) => FLists[Key & 255].Find(Key);

        // HashTab.PAS:121
        public void DeleteSafe(object Item, int Key)
        {
            if (FLists[Key & 255].Find(Key) == Item)
            {
                FLists[Key & 255].Delete(Key);
            }
        }

        // HashTab.PAS:126
        public virtual int GetFreeID()
        {
            int Min;
            int MinIndex;
            int I;
            Min = 16777215;
            MinIndex = 0;
            I = 0;
            for (; I <= 255; I += 1)
            {
                if (Min > FLists[I].Count)
                {
                    Min = FLists[I].Count;
                    MinIndex = I;
                }
            }

            I = FLists[MinIndex].Count;
            while (FLists[MinIndex].Find((I << 8) + MinIndex) != null)
            {
                I += 1;
            }

            return (I << 8) + MinIndex;
        }
    }

    // THashList items whose key dword is a string
    public partial class StringHashList : HashList
    {
        // HashTab.PAS:143
        public override void Clear()
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                FItems[I].Key = "";
            }

            base.Clear();
        }

        // HashTab.PAS:151
        public object Find(string Key)
        {
            int I;
            object Result = null;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (AnsiCompareStr(FItems[I].Key.As<string>(), Key) == 0)
                    {
                        return FItems[I].Item;
                    }
                }
                while (!(I == 0));
            }

            return Result;
        }

        // HashTab.PAS:170
        public void Add(object Item, string Key)
        {
            HashItem H = default;
            if (Find(Key) != null)
            {
                throw new InvalidOperationException("ID already in HashTable");
            }

            Assert(Item != null, "Cannot add nil item to hash table", "HashTab.PAS", 175);
            H.Key = 0;
            H.Item = Item;
            H.Key = Key;
            base.Add(H);
        }

        // HashTab.PAS:182
        public void Delete(string Key)
        {
            int I;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (AnsiCompareStr(FItems[I].Key.As<string>(), Key) == 0)
                    {
                        FItems[I].Key = "";
                        base.Delete(I);
                        return;
                    }
                }
                while (!(I == 0));
            }
        }
    }

    [LegacyObject]
    // the abstract table interface
    public partial class StringHashTable
    {
        // Engine.pas:804
        public virtual void Add(object Item, string Key)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:805
        public virtual void Delete(string Key)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:806
        public virtual object Find(string Key)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Engine.pas:807
        public virtual void Clear()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        public StringHashTable()
        {
            CaptureAddress.Assign(this);
        }
    }

    // 256 lists selected by a hash of the first and the second-to-last character and the length
    public partial class StringHashTable256 : StringHashTable
    , IDisposable
    {
        internal FixedArray_Aow_Units_Engine_TStringHashList_256 FLists;

        // HashTab.PAS:199
        public StringHashTable256()
        {
            for (int I = 0; I <= 255; I++)
            {
                var stringHashList = new StringHashList();
                FLists[I] = stringHashList;
            }
        }

        // HashTab.PAS:207
        public virtual void Dispose()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I]?.Dispose();
            }
        }

        // HashTab.PAS:215
        public override void Clear()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I].Clear();
            }
        }

        // HashTab.PAS:222
        public override void Add(object Item, string Key)
        {
            int H;
            H = Key.Length;
            if (H >= 2)
            {
                H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = Cp1252.Byte(Key[0]);
                }
            }

            FLists[H].Add(Item, Key);
        }

        // HashTab.PAS:234
        public override void Delete(string Key)
        {
            int H;
            H = Key.Length;
            if (H >= 2)
            {
                H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = Cp1252.Byte(Key[0]);
                }
            }

            FLists[H].Delete(Key);
        }

        // HashTab.PAS:246
        public override object Find(string Key)
        {
            int H;
            H = Key.Length;
            if (H >= 2)
            {
                H = Cp1252.Byte(Key[0]) + Cp1252.Byte(Key[H - 2]) + (H << 4) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = Cp1252.Byte(Key[0]);
                }
            }

            return FLists[H].Find(Key);
        }

        // Engine.pas:816

    }
}

namespace Aow.Game.Engine.Nodes
{

    // FControlStyle
    public partial class EControlStyleObject : EObject
    {
        // Engine.pas:8228
        public new const int ClassID = 81;

        internal Set<EControlStyleElement> FControlStyle;

        // Engine.pas:8222
        public EControlStyleObject(ECustomNode owner) : this(null, owner) { }

        // Engine.pas:8222
        private protected EControlStyleObject(ConstructionContext context, ECustomNode owner) : base(context, owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:8233
        public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

        // Engine.pas:8238
        public void SetControlStyle(Set<EControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8243
        public override void ReadWrite(EStorageStream Stream)
        {
            RwSet(ref FControlStyle, 1, 1, Stream.rwData, 0);
        }

        public override void PersistFields(IPersister persister)
        {
            persister.Set("ControlStyle", ref FControlStyle);
        }

    }

    // FControlStyle
    public partial class EControlStyleNode : ENode
    {
        // Engine.pas:8199
        public new const int ClassID = 257;

        public EControlStyleNode(ECustomNode owner) : base(owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        internal Set<EControlStyleElement> FControlStyle;

        // Engine.pas:8204
        public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

        // Engine.pas:8209
        public void SetControlStyle(Set<EControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8214
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
        }

    }

    // FEngine, FControlStyle. A node that knows its engine: message $20000008
    // (TEngine.ActivateEObject) carries the engine in the message
    public partial class ECustomOwner : ENode
    {
        // Engine.pas:8257
        public new const int ClassID = 259;

        // Engine.pas:8250
        public ECustomOwner(ECustomNode owner) : this(null, owner) { }

        private protected ECustomOwner(ConstructionContext context, ECustomNode owner) : base(context, owner)
        {
            FOwner = owner;
            FControlStyle = base.GetControlStyle();
        }

        internal Engine FEngine;
        internal Set<EControlStyleElement> FControlStyle;

        // Engine.pas:8262
        public override Engine GetEngine() => FEngine;

        // Engine.pas:8267
        public void SetEngine(Engine AEngine)
        {
            FEngine = AEngine;
        }

        // Engine.pas:8272
        public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

        // Engine.pas:8277
        public void SetControlStyle(Set<EControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8282
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
        }

        // Engine.pas:8288
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = Msg.Payload.As<Engine>();
            }

            base.MsgProc(ref Msg, Sender);
        }

        // Engine.pas:855
    }
}

namespace Aow.Game.Engine
{

    // a TList of class references
    public partial class EObjectClassList : DelphiList
    {
        // Engine.pas:8296
        protected internal EClass GetEClass(int Index) => (EClass)this[Index];

        // Engine.pas:8301
        public EClass FindClass(int AClassID)
        {
            EClass Result;
            int I;
            bool Found;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                Found = AClassID == ((EClass)this[I]).ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = (EClass)this[I - 1];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:8316
        public int FindClassIndex(int AClassID)
        {
            int Result;
            int I;
            bool Found;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                Found = AClassID == ((EClass)this[I]).ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = I - 1;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:8331
        public EClass GetClass(int AClassID)
        {
            EClass Result = FindClass(AClassID);
            if (Result == null)
            {
                throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(AClassID) + ")");
            }

            return Result;
        }

        // Engine.pas:8338
        public void RegisterClass(EClass AClass)
        {
            if (FindClass(AClass.ClassID) == null && AClass.ClassID != 0)
            {
                Add(AClass);
            }
            else
            {
                throw new InvalidOperationException("EObject Class ID double defined!");
            }
        }

        // Engine.pas:8346
        public void UnregisterClass(EClass AClass)
        {
            int I;
            I = FindClassIndex(AClass.ClassID);
            if (I != -1)
            {
                Delete(I);
            }
        }

        public sealed class EClassesAccessor(EObjectClassList target)
        {
            public EClass this[int Index] { get => target.GetEClass(Index); }
        }

        public EClassesAccessor EClasses => new EClassesAccessor(this);
    }
}

namespace Aow.Game.Engine.Nodes
{

    // FEditors, editor names with ids
    public partial class EResourceEditSettings : EControlStyleObject
    {
        // Engine.pas:8662
        public new const int ClassID = 1026;

        internal StringList FEditors;

        // Engine.pas:8677
        internal int GetUniqueEditID()
        {
            int I;
            bool Found;
            int Result = 1;
            I = 0;
            Found = FEditors.Count == 0;
            while (!Found)
            {
                if (Result != ObjectToInt(FEditors.Objects[I]))
                {
                    I += 1;
                    Found = I == FEditors.Count;
                }
                else
                {
                    I = 0;
                    Result += 1;
                }
            }

            return Result;
        }

        // Engine.pas:8650
        public EResourceEditSettings(ECustomNode owner) : base(owner)
        {
            FEditors = new StringList();
        }

        // Engine.pas:8656
        public override void Dispose()
        {
            FEditors?.Dispose();
            base.Dispose();
        }

        // Engine.pas:8667
        public string GetUniqueEditorName(string Name)
        {
            string S;
            S = Name;
            while (FEditors.IndexOf(S) != -1)
            {
                S = Name + IntToStr(Random(268435455));
            }

            return S;
        }

        // Engine.pas:8700
        public int GetEditID(string Name)
        {
            int Result;
            int I;
            I = FEditors.IndexOf(Name);
            if (I != -1)
            {
                Result = ObjectToInt(FEditors.Objects[I]);
            }
            else
            {
                Result = GetUniqueEditID();
                FEditors.AddObject(Name, IntToObject(Result));
            }

            return Result;
        }

        // Engine.pas:8714
        public int RemoveEditor(string Name)
        {
            int Result = FEditors.IndexOf(Name);
            if (Result != -1)
            {
                FEditors.Delete(Result);
            }

            return Result;
        }

        // Engine.pas:8720
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(EngineServices.smText))
            {
                Stream.rwStringsEx(FEditors, 3);
            }
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            PersistSupport.LinesEx(persister, "Editors", FEditors);
        }

        public StringList Editors { get => FEditors; }

    }

    public partial class ESortNode : ECustomNode
    {
        // Engine.pas:1478
        public new const int ClassID = 260;

        public ESortNode(ECustomNode owner) : base(owner)
        {
            FItems = [];
        }

        protected internal bool FSorted;
        protected internal int FCount;
        protected internal int FCapacity;
        protected internal SortItem[] FItems = [];

        // Engine.pas:1513
        protected internal void Resize(int NewCapacity)
        {
            Array.Resize(ref FItems, NewCapacity);
            FCapacity = NewCapacity;
            if (FCapacity < FCount)
            {
                FCount = FCapacity;
            }
        }

        // Engine.pas:1498
        internal int GetSortValue(int Index) => FItems[Index].SortValue;

        // Engine.pas:1503
        internal void SetSortValue(int Index, int Value)
        {
            FItems[Index].SortValue = Value;
        }

        // Engine.pas:1472
        public override void Dispose()
        {
            base.Dispose();
            Resize(0);
        }

        // Engine.pas:1483
        public override void QuickClear()
        {
            Resize(0);
        }

        // Engine.pas:1488
        public override EObject GetChild(int Index) => FItems[Index].Child;

        // Engine.pas:1493
        public override void SetChild(int Index, EObject Child)
        {
            FItems[Index].Child = Child;
        }

        // Engine.pas:1508
        public override int GetCount() => FCount;

        // Engine.pas:1520
        public virtual void Grow()
        {
            Resize(FCapacity + 2);
        }

        // Engine.pas:1525
        public override int RemoveChild(EObject Child)
        {
            int Result = FCount - 1;
            while (Result >= 0 && Child != FItems[Result].Child)
            {
                Result -= 1;
            }

            if (Result >= 0)
            {
                FCount -= 1;
                if (Result < FCount)
                {
                    Array.Copy(FItems, Result + 1, FItems, Result, FCount - Result);
                }
            }

            return Result;
        }

        // Engine.pas:1537
        public override int AddChild(EObject Child) => AddSortChild(Child, 0);

        // Engine.pas:1542
        public virtual int AddSortChild(EObject Child, int SortValue)
        {
            int Result;
            int I;
            if (FCount >= FCapacity)
            {
                Grow();
            }

            if (FSorted)
            {
                I = FCount;
                while (I > 0 && SortValue > FItems[I - 1].SortValue)
                {
                    I -= 1;
                }

                if (I != FCount)
                {
                    Array.Copy(FItems, I, FItems, I + 1, FCount - I);
                }

                FItems[I].Child = Child;
                FItems[I].SortValue = SortValue;
                Result = I;
            }
            else
            {
                FItems[FCount].Child = Child;
                FItems[FCount].SortValue = SortValue;
                Result = FCount;
            }

            FCount += 1;
            return Result;
        }

        // Engine.pas:1565
        public override void ReadWrite(EStorageStream Stream)
        {
            throw new NotSupportedException("Read/Write not supported (TESortNode)");
        }

        // Engine.pas:1596
        public virtual void Sort()
        {
            if (!FSorted && FCount > 0)
            {
                SortItems(FItems, 0, FCount - 1);
            }

            FSorted = true;
        }

        public sealed class SortValueAccessor(ESortNode target)
        {
            public int this[int Index] { get => target.GetSortValue(Index); set => target.SetSortValue(Index, value); }
        }

        public SortValueAccessor SortValue => new SortValueAccessor(this);
        public bool Sorted { get => FSorted; }

        private static void SortItems(Span<SortItem> SortList, int L, int R)
        {
            int num;
            do
            {
                num = L;
                int num2 = R;
                int sortValue = SortList[L + R >>> 1].SortValue;
                while (true)
                {
                    if (sortValue > SortList[num].SortValue)
                    {
                        num++;
                        continue;
                    }
                    while (sortValue < SortList[num2].SortValue)
                    {
                        num2--;
                    }
                    if (num <= num2)
                    {
                        SortItem tSortItem = SortList[num];
                        SortList[num] = SortList[num2];
                        SortList[num2] = tSortItem;
                        num++;
                        num2--;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                }
                if (L < num2)
                {
                    SortItems(SortList, L, num2);
                }
                L = num;
            }
            while (num < R);
        }

        public struct SortItem
        {
            public int SortValue;
            public EObject Child;
        }

        // Engine.pas:922

        // Engine.pas:922

    }

    // { Engine.TEInverseSortNode: the same, sorted the other way }
    public partial class EInverseSortNode : ESortNode
    {
        public new const int ClassID = 262;

        public EInverseSortNode(ECustomNode owner) : base(owner)
        { }

        // Engine.pas:1635
        public override int AddSortChild(EObject Child, int SortValue)
        {
            int Result;
            int I;
            if (FCount >= FCapacity)
            {
                Grow();
            }

            if (FSorted)
            {
                I = FCount;
                while (I > 0 && SortValue < FItems[I - 1].SortValue)
                {
                    I -= 1;
                }

                if (I != FCount)
                {
                    Array.Copy(FItems, I, FItems, I + 1, FCount - I);
                }

                FItems[I].Child = Child;
                FItems[I].SortValue = SortValue;
                Result = I;
            }
            else
            {
                FItems[FCount].Child = Child;
                FItems[FCount].SortValue = SortValue;
                Result = FCount;
            }

            FCount += 1;
            return Result;
        }

        // Engine.pas:1658
        public override void Sort()
        {
            if (!FSorted && FCount > 0)
            {
                InverseSortItems(FItems, 0, FCount - 1);
            }

            FSorted = true;
        }

        private static void InverseSortItems(Span<SortItem> SortList, int L, int R)
        {
            int num;
            do
            {
                num = L;
                int num2 = R;
                int sortValue = SortList[L + R >>> 1].SortValue;
                while (true)
                {
                    if (sortValue < SortList[num].SortValue)
                    {
                        num++;
                        continue;
                    }
                    while (sortValue > SortList[num2].SortValue)
                    {
                        num2--;
                    }
                    if (num <= num2)
                    {
                        SortItem tSortItem = SortList[num];
                        SortList[num] = SortList[num2];
                        SortList[num2] = tSortItem;
                        num++;
                        num2--;
                    }
                    if (num > num2)
                    {
                        break;
                    }
                }
                if (L < num2)
                {
                    InverseSortItems(SortList, L, num2);
                }
                L = num;
            }
            while (num < R);
        }

    }

    // FControlStyle
    public partial class EControlStyleSortNode : ESortNode
    {
        // Engine.pas:1672
        public new const int ClassID = 261;

        internal Set<EControlStyleElement> FControlStyle;

        // Engine.pas:1666
        public EControlStyleSortNode(ECustomNode owner) : base(owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:1677
        public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

        // Engine.pas:1682
        public void SetControlStyle(Set<EControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

    }

    // FControlStyle
    public partial class EControlStyleInverseSortNode : EInverseSortNode
    {
        // Engine.pas:1695
        public new const int ClassID = 263;

        public EControlStyleInverseSortNode(ECustomNode owner) : base(owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        internal Set<EControlStyleElement> FControlStyle;

        // Engine.pas:1700
        public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

        // Engine.pas:1705
        public void SetControlStyle(Set<EControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:963

    }

    // a TENode that holds references to objects owned elsewhere (AddRef on AddChild, Release on
    // removal)
    public partial class EReferenceNode : ENode
    {

        public EReferenceNode(ECustomNode owner) : base(owner)
        { }

        // Engine.pas:8144
        public override void Clear()
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                ((EObject)FChildren.List[I]).Release();
            }

            FChildren.Clear();
        }

        // Engine.pas:8157
        public override void ReadWriteChildren(EStorageStream Stream)
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Write))
            {
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    if (((EObject)FChildren.List[I]).FOwner == null)
                    {
                        ((EObject)FChildren.List[I]).FOwner = this;
                    }
                }

                base.ReadWriteChildren(Stream);
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    if (((EObject)FChildren.List[I]).FOwner == this)
                    {
                        ((EObject)FChildren.List[I]).FOwner = null;
                    }
                }
            }
            else
            {
                base.ReadWriteChildren(Stream);
                if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
                {
                    for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                    {
                        ((EObject)FChildren.List[I]).Release();
                    }
                }
            }
        }

        // Engine.pas:8152
        public override void DestroyAllChildren()
        {
            Clear();
        }

        // Engine.pas:8177
        public override int RemoveChild(EObject Child)
        {
            int Result = FChildren.Remove(Child);
            if (Result != -1)
            {
                Child.Release();
            }

            return Result;
        }

        // Engine.pas:8183
        public override int AddChild(EObject Child)
        {
            if (Child.FOwner == this)
            {
                Child.FOwner = null;
            }

            FChildren.Add(Child);
            if (Child.AddRef() == -1)
            {
                throw new NotSupportedException("EObject doesn't support ReferenceCounting");
            }

            return FChildren.Count - 1;
        }

        // Engine.pas:631

    }

    // FNode, the node the user is connected to
    public partial class EUser : EObject
    {
        // Engine.pas:2430
        public EUser(ECustomNode owner) : base(owner)
        { }

        internal ECustomNode FNode;

        // Engine.pas:7112
        // { Original source line layout for the HashTab assertions.
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        // }
        public override void Dispose()
        {
            Disconnect();
            base.Dispose();
        }

        // Engine.pas:7118
        public void Connect(ECustomNode Node)
        {
            Disconnect();
            FNode = Node;
            if (Node != null)
            {
                FNode.AddChild(this);
            }
        }

        // Engine.pas:7125
        public void Disconnect()
        {
            if (FNode != null)
            {
                FNode.RemoveChild(this);
                FNode = null;
            }
        }

        public ECustomNode Node { get => FNode; }
    }

    // resources with unique ids
    public partial class EResourceList : ECustomOwner
    {
        public new const int ClassID = 1025;

        public EResourceList(ECustomNode owner) : base(owner)
        { }

        // Engine.pas:8573
        protected internal EResource GetResource(int Index) => (EResource)FChildren.List[Index];

        // Engine.pas:8578
        public virtual void ResourceChanged()
        {
        }

        // Engine.pas:8582
        public EResource FindResource(int ID)
        {
            EResource Result;
            int I;
            bool Found;
            Found = false;
            I = 0;
            while (!Found && I != this.FChildren.Count)
            {
                Found = this.FChildren.List[I] != null && ID == ((EResource)this.FChildren.List[I]).FResourceID;
                I += 1;
            }

            if (Found)
            {
                Result = (EResource)this.FChildren.List[I - 1];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:8600
        public void ValidateResourceID(int Index)
        {
            int ID;
            int NewID;
            int Tries;
            bool Found;
            if (Index < FChildren.Count)
            {
                ID = GetResource(Index).FResourceID;
                var resource = GetResource(Index);
                resource.FResourceID = resource.FResourceID ^ 65535;

                if (FindResource(ID) != null)
                {
                    Tries = 10;
                    Found = false;
                    NewID = 0;
                    while (Tries > 0 && !Found)
                    {
                        NewID = Random(268435455);
                        Found = FindResource(NewID) == null;
                        Tries -= 1;
                    }

                    while (!Found)
                    {
                        NewID = Tries;
                        Found = FindResource(NewID) == null;
                        Tries += 1;
                    }

                    GetResource(Index).FResourceID = NewID;
                }
                else
                {
                    GetResource(Index).FResourceID = ID;
                }
            }
        }

        public sealed class ResourcesAccessor(EResourceList target)
        {
            public EResource this[int Index] { get => target.GetResource(Index); }
        }

        public ResourcesAccessor Resources => new ResourcesAccessor(this);

    }

    // FOnRegisterClasses (called from Loaded), FOnUnRegisterClasses (called from Destroy), FEngine,
    // the engine the component owns and frees. Descendants such as HSEngine's THexagonSpriteEngine
    // publish the two events
    public partial class EComponent : Component
    {
        internal NotifyEvent<object> FOnRegisterClasses;
        internal NotifyEvent<object> FOnUnRegisterClasses;
        internal Engine FEngine;

        // Engine.pas:8642
        public override void Loaded()
        {
            base.Loaded();
            if (FOnRegisterClasses != null)
            {
                FOnRegisterClasses(this);
            }
        }

        protected internal NotifyEvent<object> OnRegisterClasses { get => FOnRegisterClasses; set => FOnRegisterClasses = value; }
        protected internal NotifyEvent<object> OnUnRegisterClasses { get => FOnUnRegisterClasses; set => FOnUnRegisterClasses = value; }

        // Engine.pas:8635
        public override void Dispose()
        {
            if (FOnUnRegisterClasses != null)
            {
                FOnUnRegisterClasses(this);
            }

            FEngine?.Dispose();
            base.Dispose();
        }

        public Engine Engine { get => FEngine; set => FEngine = value; }

        public EComponent(Component owner) : base(owner)
        { }
    }
}

namespace Aow.Game.Engine.Collections
{

    [LegacyObject]
    public partial class CallBackEventList : IDisposable
    {
        internal List<CallBackEvent> FList = [];

        // Engine.pas:5864
        internal int GetCount() => FList.Count;

        // Engine.pas:5869
        internal ref CallBackEvent GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

        // Engine.pas:5846
        public CallBackEventList()
        {
            CaptureAddress.Assign(this);
            FList = new List<CallBackEvent>();
        }

        // Engine.pas:5852
        public virtual void Dispose()
        {
            Clear();
        }

        // Engine.pas:5859
        public void Clear()
        {
            while (FList.Count != 0)
            {
                Delete(FList.Count - 1);
            }
        }

        // Engine.pas:5874
        public void Add(in CallBackEvent Event)
        {
            FList.Add(Event);
        }

        // Engine.pas:5883
        public void Delete(int Index)
        {
            if (Index < FList.Count)
            {
                FList.RemoveAt(Index);
            }
        }

        public int Count { get => GetCount(); }

        public ref CallBackEvent this[int Index] => ref GetItem(Index);

        public struct CallBackEvent
        {
            public NotifyEvent<object> Event;
            public int Data;
        }
    }
}

namespace Aow.Game.Engine.Storage
{

    // FPath, FSize, FSections, FText, FValues. The text form of the storage: "[:path:]" lines open
    // the section of an object (its path is the ids of its parents joined with '\'), "[id]=value"
    // lines are its properties. the section is split into FValues with the property table pointing
    // at them
    public partial class EReadTextStorageStream : EStorageStream
    {
        internal string FPath = "";
        internal int FSize;
        internal StringList FSections;
        internal StringList FText;
        internal StringList FValues;

        // Engine.pas:10632
        internal void ReadStreamText()
        {
            string Line;
            StringList Text;
            int P = 0;
            int Start;
            int L;
            Text = FText;
            FSections.Clear();
            Text.BeginUpdate();
            try
            {
                if (FSize < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative string length");
                byte[] buffer = new byte[FSize];
                FStream.ReadBytes(buffer);
                Text.Clear();
                ReadOnlySpan<byte> text = buffer;
                if (!text.IsEmpty)
                {
                    while ((P < text.Length ? text[P] : (byte)0) != 0)
                    {
                        Start = P;
                        while (!Set<byte>.WithBounds(0, 255).With(0).With(10).With(13).Contains(P < text.Length ? text[P] : (byte)0))
                        {
                            P += 1;
                        }

                        Line = Cp1252.Decode(text[Start..P].ToArray());
                        if (!IsEmpty(Line))
                        {
                            L = Line.Length;
                            if (L >= 4 && Copy(Line, 1, 2) == "[:" && Copy(Line, L - 1, 2) == ":]")
                            {
                                var count = Text.Count;
                                FSections.AddObject(Copy(Line, 3, L - 4), IntToObject(count));
                            }
                            else
                            {
                                Text.Add(Line);
                            }
                        }

                        if ((P < text.Length ? text[P] : (byte)0) == 13)
                        {
                            P += 1;
                        }

                        if ((P < text.Length ? text[P] : (byte)0) == 10)
                        {
                            P += 1;
                        }
                    }
                }
            }
            finally
            {
                Text.EndUpdate();
            }
        }

        // Engine.pas:10694
        internal void ReadTextBuffer()
        {
            int Index;
            int First;
            int Last;
            int I;
            int ID;
            int NewID;
            string Value;
            FValues.Clear();
            Index = FSections.IndexOf(FPath);
            if (Index != -1)
            {
                First = ObjectToInt(FSections.Objects[Index]);
                if (Index != FSections.Count - 1)
                {
                    Last = ObjectToInt(FSections.Objects[Index + 1]) - 1;
                }
                else
                {
                    Last = FText.Count - 1;
                }

                I = First;
                ID = -1;
                Value = "";
                while (I <= Last)
                {
                    NewID = GetLineID(FText[I]);
                    if (ID != -1)
                    {
                        if (NewID != ID && NewID != -1)
                        {
                            var count = FValues.Count;
                            FPropertyTable.Add(ID, count);
                            FValues.Add(Value);
                            ID = NewID;
                            Value = GetLineValue(FText[I]);
                        }
                        else
                        {
                            var item = FText[I];
                            Value = Value + "\r" + "\n" + item;
                        }
                    }
                    else
                    {
                        if (NewID != -1)
                        {
                            if (ID == -1)
                            {
                                ID = NewID;
                            }

                            Value = GetLineValue(FText[I]);
                        }
                    }

                    I += 1;
                }

                if (ID != -1)
                {
                    var count = FValues.Count;
                    FPropertyTable.Add(ID, count);
                    FValues.Add(Value);
                }
            }
        }

        // Engine.pas:10611
        public EReadTextStorageStream(Stream AStream, Engine AEngine, int ASize) : base(AStream, AEngine)
        {
            FPath = "";
            if (ASize <= 0)
            {
                FSize = (int)(FStream.Length - FStream.Position);
            }
            else
            {
                FSize = ASize;
            }

            FValues = new StringList();
            FPropertyTable = new PropertyTable();
            FText = new StringList();
            FSections = new StringList();
            ReadStreamText();
            ReadTextBuffer();
        }

        // Engine.pas:10624
        public override void Dispose()
        {
            FValues?.Dispose();
            FPropertyTable?.Dispose();
            FSections?.Dispose();
            FText?.Dispose();
        }

        // Engine.pas:10669
        public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText);

        // Engine.pas:10741
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            EObject O;
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10749
        public override void rwEObject(EObject Obj, int ID)
        {
            StringList SavedValues;
            string SavedPath;
            bool Continue;
            SavedPath = FPath;
            if (!IsEmpty(FPath))
            {
                FPath = FPath + "\\" + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            SavedValues = FValues;
            FPropertyTable.BackupSettings();
            FValues = new StringList();
            try
            {
                ReadTextBuffer();
                Obj.MainReadWrite(this);
                FValues?.Dispose();
                FPropertyTable.RestoreSettings();
                FValues = SavedValues;
                FPath = SavedPath;
            }
            catch
            {
                FValues?.Dispose();
                FPropertyTable.RestoreSettings();
                FValues = SavedValues;
                FPath = SavedPath;
                Continue = true;
                if (FOnError != null)
                {
                    var classID = Obj.EClass.ClassID;
                    FOnError(this, "Error reading object (cid:" + IntToStr(classID) + ")", ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }

        // Engine.pas:10779
        public override void rwEObjectStream(EObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10784
        public override void rwString(ref string S, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                S = FValues[Offset];
            }
            else
            {
                S = "";
            }
        }

        // Engine.pas:10792
        public override void rwStrings(StringCollection Strings, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                Strings.Text = NullTerminatedText(FValues[Offset]);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10800
        public override void rwStringsEx(StringCollection Strings, int ID)
        {
            rwStrings(Strings, ID);
        }

        // Engine.pas:10805
        public override void rwChildren(ECustomNode Node, int ID)
        {
            EObject C;
            // { the original saves the path and never restores it }
            if (!IsEmpty(FPath))
            {
                FPath = FPath + "\\" + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                {
                    rwEObject(C, I);
                }
            }
        }

        private static int GetLineID(string S)
        {
            int i;
            for (i = 1; Set<byte>.WithBounds(0, 255).With((byte)91).WithRange((byte)48, (byte)57)
                .Contains(Cp1252.Byte(S[i - 1])); i++)
            {
            }
            if (RtlUnit.Copy(S, i, 2) == "]=")
            {
                return RtlUnit.StrToInt(RtlUnit.Copy(S, 2, i - 2));
            }
            return -1;
        }

        private static string GetLineValue(string S)
        {
            int i;
            for (i = 1; Set<byte>.WithBounds(0, 255).With((byte)91).With((byte)93)
                .WithRange((byte)48, (byte)57)
                .Contains(Cp1252.Byte(S[i - 1])); i++)
            {
            }
            return RtlUnit.Copy(S, i + 1, S.Length - i);
        }

    }

    // FPath, FBuffer. Each object's properties are collected in FBuffer and written behind the
    // "[:path:]" line
    public partial class EWriteTextStorageStream : EStorageStream
    {
        internal string FPath = "";
        protected internal EBufferStream FBuffer;

        // Engine.pas:10355
        internal void WriteTextBuffer(Stream Stream)
        {
            string S;
            if (FPropertyTable.Count != 0 && FBuffer.Length != 0)
            {
                S = "[:" + FPath + ":]" + "\r" + "\n";
                if (!IsEmpty(FPath))
                {
                    S = "\r\n" + S;
                }

                Stream.Write(Cp1252.Encode(S).AsSpan().Slice(1 - 1, S.Length));
                FBuffer.SaveToStream(Stream);
            }
        }

        // Engine.pas:10335
        public EWriteTextStorageStream(Stream AStream, Engine AEngine) : base(AStream, AEngine)
        {
            FPath = "";
            FBuffer = new EBufferStream();
            FPropertyTable = new PropertyTable();
        }

        // Engine.pas:10343
        public override void Dispose()
        {
            WriteTextBuffer(FStream);
            FBuffer?.Dispose();
            FPropertyTable?.Dispose();
        }

        // Engine.pas:10350
        public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Write, EngineServices.smText);

        // Engine.pas:10368
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            EObject O;
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10376
        public override void rwEObject(EObject Obj, int ID)
        {
            EBufferStream SavedBuffer;
            string SavedPath;
            bool Continue;
            if (!FPropertyTable.PropertyExist(ID))
            {
                SavedBuffer = FBuffer;
                SavedPath = FPath;
                FPropertyTable.BackupSettings();
                FBuffer = new EBufferStream();
                if (!IsEmpty(FPath))
                {
                    FPath = FPath + "\\" + IntToStr(ID);
                }
                else
                {
                    FPath = IntToStr(ID);
                }

                try
                {
                    Obj.MainReadWrite(this);
                    WriteTextBuffer(FStream);
                    FBuffer?.Dispose();
                    FPropertyTable.RestoreSettings();
                    FBuffer = SavedBuffer;
                    FPath = SavedPath;
                    FPropertyTable.Add(ID, 0);
                }
                catch
                {
                    FBuffer?.Dispose();
                    FBuffer = SavedBuffer;
                    FPropertyTable.RestoreSettings();
                    FPath = SavedPath;
                    Continue = true;
                    if (FOnError != null)
                    {
                        var classID = Obj.EClass.ClassID;
                        FOnError(this, "Error writing object (cid:" + IntToStr(classID) + ")", ref Continue);
                    }

                    if (!Continue)
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found");
            }
        }

        // Engine.pas:10412
        public override void rwEObjectStream(EObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10417
        public override void rwString(ref string S, int ID)
        {
            string Line;
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (!IsEmpty(S))
                {
                    Line = "[" + IntToStr(ID) + "]=" + S + "\r" + "\n";
                    FBuffer.Write(Cp1252.Encode(Line).AsSpan().Slice(1 - 1, Line.Length));
                    FPropertyTable.Add(ID, 0);
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found");
            }
        }

        // Engine.pas:10434
        public override void rwStrings(StringCollection Strings, int ID)
        {
            string Line;
            string P;
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (Strings.Count != 0)
                {
                    P = Strings.Text;
                    {
                        Line = "[" + IntToStr(ID) + "]=";
                        Line += NullTerminatedText(P);

                        FBuffer.Write(Cp1252.Encode(Line).AsSpan().Slice(1 - 1, Line.Length));
                        FPropertyTable.Add(ID, 0);
                    }
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found");
            }
        }

        // Engine.pas:10458
        public override void rwStringsEx(StringCollection Strings, int ID)
        {
            rwStrings(Strings, ID);
        }

        // Engine.pas:10463
        public override void rwChildren(ECustomNode Node, int ID)
        {
            string SavedPath;
            EObject C;
            if (!FPropertyTable.PropertyExist(ID))
            {
                SavedPath = FPath;
                if (!IsEmpty(FPath))
                {
                    FPath = FPath + "\\" + IntToStr(ID);
                }
                else
                {
                    FPath = IntToStr(ID);
                }

                for (int I = 0, last = Node.Count - 1; I <= last; I++)
                {
                    C = Node.Children[I];
                    if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                    {
                        rwEObject(C, I);
                    }
                }

                FPath = SavedPath;
                FPropertyTable.Add(ID, 0);
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found");
            }
        }

    }

    // FPartial, FDataBase. The constructor reads the root property table. every property is found
    // through the table and read at FDataBase + offset
    public partial class EReadStorageStream : EStorageStream
    {
        internal bool FPartial;
        internal int FDataBase;

        // Engine.pas:9968
        public EReadStorageStream(Stream AStream, Engine AEngine, bool APartial) : base(AStream, AEngine)
        {
            FPropertyTable = new PropertyTable();
            FPropertyTable.LoadFromStream(AStream);
            FDataBase = (int)(FStream.Position);
            FPartial = APartial;
        }

        // Engine.pas:9977
        public override void Dispose()
        {
            FPropertyTable?.Dispose();
            base.Dispose();
        }

        // Engine.pas:9983
        public override Set<StorageModeElement> StorageMode()
        {
            Set<StorageModeElement> Result;
            if (FPartial)
            {
                Result = Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smPartial);
            }
            else
            {
                Result = Set<StorageModeElement>.Of(StorageModeElement.Read);
            }

            return Result;
        }

        // Engine.pas:9991
        public override void rwInteger(ref int I, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref I, 4));
            }
            else
            {
                I = 0;
            }
        }

        // Engine.pas:10005
        public override void rwByte(ref byte B, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref B, 1));
            }
            else
            {
                B = 0;
            }
        }

        // Engine.pas:10019
        public override void rwShortint(ref sbyte S, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref S, 1));
            }
            else
            {
                S = 0;
            }
        }

        // Engine.pas:10033
        public override void rwWord(ref ushort W, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref W, 2));
            }
            else
            {
                W = 0;
            }
        }

        // Engine.pas:10047
        public override void rwIntegerEx(ref int I, int Default, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref I, 4));
            }
            else
            {
                I = Default;
            }
        }

        // Engine.pas:10061
        public override void rwByteEx(ref byte B, byte Default, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref B, 1));
            }
            else
            {
                B = Default;
            }
        }

        // Engine.pas:10075
        public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref S, 1));
            }
            else
            {
                S = Default;
            }
        }

        // Engine.pas:10089
        public override void rwWordEx(ref ushort W, ushort Default, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref W, 2));
            }
            else
            {
                W = Default;
            }
        }

        // Engine.pas:10103
        public override void rwString(ref string S, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                LoadStringFromStream(ref S, FStream);
            }
            else
            {
                S = "";
            }
        }

        // Engine.pas:10117
        public override void rwStrings(StringCollection Strings, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                StringsFromStream(Strings, FStream);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10131
        public override void rwStringsEx(StringCollection Strings, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                StringsFromStreamEx(Strings, FStream);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10145
        public override void rwData(Span<byte> Buffer, int Size, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Buffer.Slice(0, Size));
            }
        }

        // Engine.pas:10157
        public override void rwStreamProc(int ID, StreamProc LoadProc, StreamProc SaveProc)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                try
                {
                    LoadProc(FStream);
                }
                catch
                {
                }
            }
        }

        // Engine.pas:10172
        public override Stream OpenPropertyStream(int ID)
        {
            Stream Result;
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                Result = FStream;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:10186
        public override void rwBool(ref bool B, int ID)
        {
            int Offset;
            Offset = FPropertyTable.FindOffset(ID);
            B = Offset != -1;
        }

        // Engine.pas:10191
        public bool HandleException(Exception E)
        {
            bool Continue;
            if (!FErrorHandled)
            {
                Continue = true;
                if (FOnError != null)
                {
                    if (IsEmpty(E.Message))
                    {
                        FOnError(this, EStreamReadErrorStr, ref Continue);
                    }
                    else
                    {
                        FOnError(this, E.Message, ref Continue);
                    }
                }

                if (!Continue)
                {
                    FErrorHandled = true;
                }
            }

            return !FErrorHandled;
        }

        // Engine.pas:10208
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            int Offset;
            int SavedBase;
            int AClassID = default;
            Obj = null;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref AClassID, 4));
                if (AClassID != 0)
                {
                    SavedBase = FDataBase;
                    FPropertyTable.BackupSettings();
                    FPropertyTable.LoadFromStream(FStream);
                    FDataBase = (int)(FStream.Position);
                    try
                    {
                        Obj = FEngine.CreateEObject(AClassID, null);
                        Obj.FOwner = AOwner;
                        Obj.MainReadWrite(this);
                        Obj.FOwner = null;
                        Obj.SetOwner(AOwner);
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                    }
                    catch (System.Exception error)
                    {
                        Obj?.Dispose();
                        Obj = null;
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                        if (!HandleException(error))
                        {
                            throw;
                        }
                    }
                }
            }
        }

        // Engine.pas:10246
        public override void rwChildren(ECustomNode Node, int ID)
        {
            int Offset;
            int SavedBase;
            int ChildID;
            EObject Child = default;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                FDataBase = (int)(FStream.Position);
                if (Node.GetControlStyle().Contains(EngineServices.csKeepIndex))
                {
                    for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                    {
                        ChildID = FPropertyTable.Items[I].ID;
                        rwCreateEObject(ref Child, Node, ChildID);
                        if (Child != null)
                        {
                            Child.SetArrayOwner(Node, ChildID);
                        }
                    }
                }
                else
                {
                    for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                    {
                        ChildID = FPropertyTable.Items[I].ID;
                        rwCreateEObject(ref Child, Node, ChildID);
                    }
                }

                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
            }
        }

        // Engine.pas:10277
        public override void rwEObject(EObject Obj, int ID)
        {
            int Offset;
            int SavedBase;
            SavedBase = FDataBase;
            Offset = FPropertyTable.FindOffset(ID);
            FPropertyTable.BackupSettings();
            if (Offset != -1)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                FDataBase = (int)(FStream.Position);
            }

            try
            {
                Obj.MainReadWrite(this);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
            }
            catch (System.Exception error)
            {
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }

        // Engine.pas:10304
        public override void rwEObjectStream(EObject Obj, int ID)
        {
            int SavedBase;
            int Start;
            int Size = default;
            Start = (int)(FStream.Position);
            FStream.ReadBytes(Bytes.Of(ref Size, 4));
            if (Size != 0)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                try
                {
                    FPropertyTable.LoadFromStream(FStream);
                    FDataBase = (int)(FStream.Position);
                    Obj.MainReadWrite(this);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    FStream.Position = Start + Size + 4;
                }
                catch (System.Exception error)
                {
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    FStream.Position = Start + Size + 4;
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
        }

        public int DataBase { get => FDataBase; }

        private static readonly string EStreamReadErrorStr = "Error Reading EObject";

        internal static void StringsFromStream(StringCollection Strings, Stream Stream)
        {
            int num = 0;
            Strings.BeginUpdate();
            try
            {
                StreamExtensions.ReadBytes(Stream, Bytes.Of<int>(ref num, 4));
                if (num < 0)
                {
                    throw new ArgumentOutOfRangeException((string)null, "Negative string length");
                }
                byte[] array = new byte[num];
                StreamExtensions.ReadBytes(Stream, (Span<byte>)(array));
                Strings.Text = Cp1252.Decode((ReadOnlySpan<byte>)(array));
            }
            finally
            {
                Strings.EndUpdate();
            }
        }

        internal static void LoadStringFromStream(ref string S, Stream Stream)
        {
            int num = 0;
            byte b = 0;
            S = "";
            StreamExtensions.ReadBytes(Stream, Bytes.Of<byte>(ref b, 1));
            if (b == 255)
            {
                StreamExtensions.ReadBytes(Stream, Bytes.Of<int>(ref num, 4));
            }
            else
            {
                num = b;
            }
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException((string)null, "Negative string length");
            }
            byte[] array = new byte[num];
            Cp1252.Encode(S)[..Math.Min(S.Length, num)].CopyTo(array);
            S = Cp1252.Decode((ReadOnlySpan<byte>)(array));
            StreamExtensions.ReadBytes(Stream, (Span<byte>)(array));
            S = Cp1252.Decode((ReadOnlySpan<byte>)(array));
        }

        internal static void StringsFromStreamEx(StringCollection Strings, Stream Stream)
        {
            int num = 0;
            int num2 = 0;
            Strings.BeginUpdate();
            try
            {
                StreamExtensions.ReadBytes(Stream, Bytes.Of<int>(ref num, 4));
                if (num < 0)
                {
                    throw new ArgumentOutOfRangeException((string)null, "Negative string length");
                }
                byte[] array = new byte[num];
                StreamExtensions.ReadBytes(Stream, (Span<byte>)(array));
                Strings.Text = Cp1252.Decode((ReadOnlySpan<byte>)(array));
                int i = 0;
                for (int num3 = Strings.Count - 1; i <= num3; i++)
                {
                    StreamExtensions.ReadBytes(Stream, Bytes.Of<int>(ref num2, 4));
                    Strings.Objects[i] = RtlUnit.IntToObject(num2);
                }
            }
            finally
            {
                Strings.EndUpdate();
            }
        }

    }

    // FPartial, FBuffer. Values go into the buffer. the property table records (id, buffer
    // position). A nested object gets its own buffer, appended to the outer one as [table][data].
    // The destructor writes the root table and buffer
    public partial class EWriteStorageStream : EStorageStream
    {

        internal bool FPartial;
        internal EBufferStream FBuffer;

        // Engine.pas:9592
        internal void PropertyExistError(int ID)
        {
            throw new DoublePrioriyIDException("PropertyID already in use (" + IntToStr(ID) + " / " + this.GetType().Name + ")");
        }

        // Engine.pas:9565
        // Engine.pas:10489 TDebugWriteStream borrows the supplied stream as its buffer.
        private protected EWriteStorageStream(Stream stream, Engine engine, EBufferStream buffer) : base(stream, engine)
        {
            FBuffer = buffer;
            FPropertyTable = new PropertyTable();
            FPartial = false;
        }

        public EWriteStorageStream(Stream AStream, Engine AEngine, bool APartial) : base(AStream, AEngine)
        {
            FEngine = AEngine;
            FStream = AStream;
            FBuffer = new EBufferStream();
            FPropertyTable = new PropertyTable();
            FPartial = APartial;
        }

        // Engine.pas:9575
        public override void Dispose()
        {
            FPropertyTable.SaveToStream(FStream);
            FBuffer.SaveToStream(FStream);
            FPropertyTable?.Dispose();
            FBuffer?.Dispose();
            base.Dispose();
        }

        // Engine.pas:9584
        public override Set<StorageModeElement> StorageMode()
        {
            Set<StorageModeElement> Result;
            if (FPartial)
            {
                Result = Set<StorageModeElement>.Of(StorageModeElement.Write, EngineServices.smPartial);
            }
            else
            {
                Result = Set<StorageModeElement>.Of(StorageModeElement.Write);
            }

            return Result;
        }

        // Engine.pas:9597
        public override void rwByte(ref byte B, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (B != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref B, 1));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9611
        public override void rwWord(ref ushort W, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (W != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref W, 2));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9625
        public override void rwShortint(ref sbyte S, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (S != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref S, 1));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9639
        public override void rwInteger(ref int I, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (I != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref I, 4));
                }
            }
            else
            {
                throw new DoublePrioriyIDException("PropertyID already in use");
            }
        }

        // Engine.pas:9653
        public override void rwByteEx(ref byte B, byte Default, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (B != Default)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref B, 1));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9667
        public override void rwWordEx(ref ushort W, ushort Default, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (W != Default)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref W, 2));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9681
        public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (S != Default)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref S, 1));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9695
        public override void rwIntegerEx(ref int I, int Default, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (I != Default)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Bytes.Of(ref I, 4));
                }
            }
            else
            {
                throw new DoublePrioriyIDException("PropertyID already in use");
            }
        }

        // Engine.pas:9709
        public override void rwString(ref string S, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (!IsEmpty(S))
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    SaveStringToStream(S, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9723
        public override void rwStrings(StringCollection Strings, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (Strings.Count != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    StringsToStream(Strings, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9737
        public override void rwStringsEx(StringCollection Strings, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (Strings.Count != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    StringsToStreamEx(Strings, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9751
        public override void rwData(Span<byte> Buffer, int Size, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (Size != 0)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    FBuffer.Write(Buffer.Slice(0, Size));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9765
        public override void rwStreamProc(int ID, StreamProc LoadProc, StreamProc SaveProc)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                try
                {
                    SaveProc(FBuffer);
                }
                catch
                {
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9779
        public override Stream OpenPropertyStream(int ID)
        {
            Stream Result;
            if (!FPropertyTable.PropertyExist(ID))
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                Result = FBuffer;
            }
            else
            {
                PropertyExistError(ID);
                Result = null;
            }

            return Result;
        }

        // Engine.pas:9793
        public override void rwBool(ref bool B, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                if (B)
                {
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9803
        public bool HandleException(Exception E)
        {
            bool Continue;
            if (!FErrorHandled)
            {
                Continue = true;
                if (FOnError != null)
                {
                    if (IsEmpty(E.Message))
                    {
                        FOnError(this, EStreamWriteErrorStr, ref Continue);
                    }
                    else
                    {
                        FOnError(this, E.Message, ref Continue);
                    }
                }

                if (!Continue)
                {
                    FErrorHandled = true;
                }
            }

            return !FErrorHandled;
        }

        // Engine.pas:9820
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            EBufferStream Outer;
            int Start;
            int AClassID;
            EObject O;
            if (!FPropertyTable.PropertyExist(ID))
            {
                Start = (int)(FBuffer.Position);
                O = Obj;
                if (O != null)
                {
                    AClassID = O.EClass.ClassID;
                }
                else
                {
                    AClassID = 0;
                }

                if (AClassID != 0)
                {
                    Outer = FBuffer;
                    FBuffer = new EBufferStream();
                    FPropertyTable.BackupSettings();
                    try
                    {
                        Obj.MainReadWrite(this);
                        Outer.Write(Bytes.Of(ref AClassID, 4));
                        FPropertyTable.SaveToStream(Outer);
                        FBuffer.SaveToStream(Outer);
                        FBuffer?.Dispose();
                        FPropertyTable.RestoreSettings();
                        FBuffer = Outer;
                        FPropertyTable.Add(ID, Start);
                    }
                    catch (System.Exception error)
                    {
                        FPropertyTable.RestoreSettings();
                        FBuffer?.Dispose();
                        FBuffer = Outer;
                        Outer.Position = Start;
                        if (!HandleException(error))
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    FBuffer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.Add(ID, Start);
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found (" + IntToStr(ID) + ")");
            }
        }

        // Engine.pas:9866
        public override void rwChildren(ECustomNode Node, int ID)
        {
            EBufferStream Outer;
            int Start;
            EObject C = default;
            if (!FPropertyTable.PropertyExist(ID))
            {
                Start = (int)(FBuffer.Position);
                Outer = FBuffer;
                FBuffer = new EBufferStream();
                FPropertyTable.BackupSettings();
                for (int I = 0, last = Node.Count - 1; I <= last; I++)
                {
                    C = Node.Children[I];
                    if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                    {
                        rwCreateEObject(ref C, Node, I);
                    }
                }

                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                FBuffer?.Dispose();
                FPropertyTable.RestoreSettings();
                FBuffer = Outer;
                FPropertyTable.Add(ID, Start);
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found (" + IntToStr(ID) + ")");
            }
        }

        // Engine.pas:9897
        public override void rwEObject(EObject Obj, int ID)
        {
            EBufferStream Outer;
            int Start;
            if (!FPropertyTable.PropertyExist(ID))
            {
                Start = (int)(FBuffer.Position);
                Outer = FBuffer;
                FBuffer = new EBufferStream();
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    FPropertyTable.SaveToStream(Outer);
                    FBuffer.SaveToStream(Outer);
                    FPropertyTable.RestoreSettings();
                    FBuffer?.Dispose();
                    FBuffer = Outer;
                    FPropertyTable.Add(ID, Start);
                }
                catch (System.Exception error)
                {
                    FPropertyTable.RestoreSettings();
                    FBuffer?.Dispose();
                    FBuffer = Outer;
                    Outer.Position = Start;
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found (" + Obj.GetType().Name + ":" + IntToStr(ID) + ")");
            }
        }

        // Engine.pas:9931
        public override void rwEObjectStream(EObject Obj, int ID)
        {
            EBufferStream Outer;
            int Start;
            int Len = default;
            Start = (int)(FBuffer.Position);
            Outer = FBuffer;
            FBuffer = new EBufferStream();
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                Outer.Write(Bytes.Of(ref Len, 4));
                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                Len = (int)(Outer.Position) - Start - 4;
                Outer.Position = Start;
                Outer.Write(Bytes.Of(ref Len, 4));
                Outer.Position = Start + Len + 4;
                FPropertyTable.RestoreSettings();
                FBuffer?.Dispose();
                FBuffer = Outer;
            }
            catch (System.Exception error)
            {
                FBuffer?.Dispose();
                FBuffer = Outer;
                FPropertyTable.RestoreSettings();
                Len = 0;
                Outer.Position = Start;
                Outer.Write(Bytes.Of(ref Len, 4));
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }

        private static readonly string EStreamWriteErrorStr = "Error Writing EObject";

        internal static void StringsToStream(StringCollection Strings, Stream Stream)
        {
            ReadOnlySpan<byte> text = (ReadOnlySpan<byte>)(Cp1252.Encode(Strings.Text));
            int num = CStringLength(text);
            Stream.Write((Span<byte>)(Bytes.Of<int>(ref num, 4)));
            StreamExtensions.WriteBuffer(Stream, text.Slice(0, num));
        }

        private static void SaveStringToStream(string S, Stream Stream)
        {
            byte b = 0;
            int length = S.Length;
            if (length < 255)
            {
                b = (byte)length;
                Stream.Write((Span<byte>)(Bytes.Of<byte>(ref b, 1)));
            }
            else
            {
                b = 255;
                Stream.Write((Span<byte>)(Bytes.Of<byte>(ref b, 1)));
                Stream.Write((Span<byte>)(Bytes.Of<int>(ref length, 4)));
            }
            Stream.Write((Span<byte>)(MemoryExtensions.AsSpan<byte>(Cp1252.Encode(S)).Slice(0, length)));
        }

        internal static void StringsToStreamEx(StringCollection Strings, Stream Stream)
        {
            int num = 0;
            ReadOnlySpan<byte> text = (ReadOnlySpan<byte>)(Cp1252.Encode(Strings.Text));
            int num2 = CStringLength(text);
            Stream.Write((Span<byte>)(Bytes.Of<int>(ref num2, 4)));
            StreamExtensions.WriteBuffer(Stream, text.Slice(0, num2));
            int i = 0;
            for (int num3 = Strings.Count - 1; i <= num3; i++)
            {
                num = RtlUnit.ObjectToInt(Strings.Objects[i]);
                Stream.Write((Span<byte>)(Bytes.Of<int>(ref num, 4)));
            }
        }

        private static int CStringLength(ReadOnlySpan<byte> text)
        {
            int num = MemoryExtensions.IndexOf<byte>(text, (byte)0);
            return (num < 0) ? text.Length : num;
        }

    }

    // a write storage stream that writes every object's data straight into the stream (FBuffer is
    // the stream itself)
    public partial class DebugWriteStream : EWriteStorageStream
    {
        // Engine.pas:10489
        // Retained: The debug writer uses the supplied stream as FBuffer and skips the normal writer buffer allocation. Retire with a replacement-base storage protocol.
        public DebugWriteStream(Stream AStream, Engine AEngine) : base(AStream, AEngine, (EBufferStream)AStream)
        { }

        // Engine.pas:10499
        public override void Dispose()
        {
            FPropertyTable.SaveToStream(FStream);
            FPropertyTable?.Dispose();
        }

        // Engine.pas:10505
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            int AClassID;
            EObject O;
            if (!FPropertyTable.PropertyExist(ID))
            {
                O = Obj;
                if (O != null)
                {
                    AClassID = O.EClass.ClassID;
                }
                else
                {
                    AClassID = 0;
                }

                if (AClassID != 0)
                {
                    FPropertyTable.BackupSettings();
                    try
                    {
                        Obj.MainReadWrite(this);
                        FBuffer.Write(Bytes.Of(ref AClassID, 4));
                        FPropertyTable.SaveToStream(FBuffer);
                        FBuffer?.Dispose();
                        FPropertyTable.RestoreSettings();
                        FPropertyTable.Add(ID, (int)(FBuffer.Position));
                    }
                    catch (System.Exception error)
                    {
                        FPropertyTable.RestoreSettings();
                        if (!HandleException(error))
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    FBuffer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found (" + IntToStr(ID) + ")");
            }
        }

        // Engine.pas:10542
        public override void rwChildren(ECustomNode Node, int ID)
        {
            EObject C = default;
            if (!FPropertyTable.PropertyExist(ID))
            {
                FPropertyTable.BackupSettings();
                for (int I = 0, last = Node.Count - 1; I <= last; I++)
                {
                    C = Node.Children[I];
                    if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                    {
                        rwCreateEObject(ref C, Node, I);
                    }
                }

                FPropertyTable.SaveToStream(FBuffer);
                FPropertyTable.RestoreSettings();
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found (" + IntToStr(ID) + ")");
            }
        }

        // Engine.pas:10565
        public override void rwEObject(EObject Obj, int ID)
        {
            if (!FPropertyTable.PropertyExist(ID))
            {
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    FPropertyTable.SaveToStream(FBuffer);
                    FPropertyTable.RestoreSettings();
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                }
                catch (System.Exception error)
                {
                    FPropertyTable.RestoreSettings();
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new DoublePrioriyIDException("Double priorityID found (" + Obj.GetType().Name + ":" + IntToStr(ID) + ")");
            }
        }

        // Engine.pas:10587
        public override void rwEObjectStream(EObject Obj, int ID)
        {
            int Size = default;
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                FBuffer.Write(Bytes.Of(ref Size, 4));
                FPropertyTable.SaveToStream(FBuffer);
                FBuffer.Write(Bytes.Of(ref Size, 4));
                FPropertyTable.RestoreSettings();
            }
            catch (System.Exception error)
            {
                FPropertyTable.RestoreSettings();
                Size = 0;
                FBuffer.Write(Bytes.Of(ref Size, 4));
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }
    }

    // visits every string of an object tree through TranslateString and TranslateStrings. nothing
    // is read or written
    public partial class TranslateStorageStream : EStorageStream
    {
        // Engine.pas:10825
        public TranslateStorageStream(Engine AEngine) : base(null, AEngine)
        {
            FPropertyTable = new PropertyTable();
        }

        // Engine.pas:10831
        public override void Dispose()
        {
            FPropertyTable?.Dispose();
            base.Dispose();
        }

        // Engine.pas:10837
        public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Translate);

        // Engine.pas:10842
        public override void rwString(ref string S, int ID)
        {
            TranslateString(ref S);
        }

        // Engine.pas:10847
        public override void rwStrings(StringCollection Strings, int ID)
        {
            TranslateStrings(ref Strings);
        }

        // Engine.pas:10852
        public override void rwStringsEx(StringCollection Strings, int ID)
        {
            TranslateStrings(ref Strings);
        }

        // Engine.pas:10857
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            EObject O;
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10865
        public override void rwEObject(EObject Obj, int ID)
        {
            bool Continue = default;
            try
            {
                FPropertyTable.BackupSettings();
                Obj.MainReadWrite(this);
                FPropertyTable.RestoreSettings();
            }
            catch
            {
                FPropertyTable.RestoreSettings();
                // { the original leaves Continue uninitialized here }
                if (FOnError != null)
                {
                    var classID = Obj.EClass.ClassID;
                    FOnError(this, "Error reading object (cid:" + IntToStr(classID) + ")", ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }

        // Engine.pas:10882
        public override void rwEObjectStream(EObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10887
        public override void rwChildren(ECustomNode Node, int ID)
        {
            EObject C;
            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                {
                    rwEObject(C, I);
                }
            }
        }

        // Engine.pas:10901
        public virtual void TranslateStrings(ref StringCollection Strings)
        {
        }

        // Engine.pas:10905
        public virtual void TranslateString(ref string S)
        {
        }

    }

    // a read stream that announces a comparison to come (smSetupCompare)
    public partial class SetupCompareStorageStream : EReadStorageStream
    {
        public SetupCompareStorageStream(Stream AStream, Engine AEngine, bool APartial) : base(AStream, AEngine, APartial)
        { }

        // Engine.pas:10911
        public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.SetupCompare);
    }

    // reads a stored copy and calls Mismatch for every property that differs from the object.
    // Mismatch raises and swallows its own exception, a place for a breakpoint
    public partial class CompareStorageStream : EReadStorageStream
    {
        // Engine.pas:11002
        internal void Mismatch(int ID)
        {
            try
            {
                throw new InvalidOperationException("Property Mismatch (PropertyID:" + IntToStr(ID) + ")");
            }
            catch
            {
            }
        }

        // Engine.pas:10918
        public CompareStorageStream(Stream AStream, Engine AEngine) : base(AStream, AEngine, false)
        { }

        // Engine.pas:10928
        public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Compare);

        // Engine.pas:10933
        public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
        {
            int Offset;
            int SavedBase;
            int AClassID = default;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1 && Obj != null)
            {
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.ReadBytes(Bytes.Of(ref AClassID, 4));
                if (AClassID != 0)
                {
                    SavedBase = FDataBase;
                    FPropertyTable.BackupSettings();
                    FPropertyTable.LoadFromStream(FStream);
                    FDataBase = (int)(FStream.Position);
                    // { The original handles read errors by destroying and clearing Obj. }
                    try
                    {
                        Obj.MainReadWrite(this);
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                    }
                    catch (System.Exception error)
                    {
                        Obj?.Dispose();
                        Obj = null;
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                        if (!HandleException(error))
                        {
                            throw;
                        }
                    }
                }
            }
        }

        // Engine.pas:10958
        public override void rwChildren(ECustomNode Node, int ID)
        {
            int SavedBase;
            int Start;
            EObject C = default;
            int Offset;
            int Index;
            Offset = FPropertyTable.FindOffset(ID);
            if (Offset != -1)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                if (FStream.Position != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                FDataBase = (int)(FStream.Position);
                if (Node.GetControlStyle().Contains(EngineServices.csKeepIndex))
                {
                    for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                    {
                        Index = FPropertyTable.Items[I].ID;
                        C = Node.Children[Index];
                        if (C != null)
                        {
                            rwCreateEObject(ref C, Node, Index);
                        }
                    }
                }
                else
                {
                    Start = 0;
                    while (Start < Node.Count && Node.Children[Start] != null && (Node.Children[Start].GetControlStyle() & Set<EControlStyleElement>.Of(EngineServices.csShared)) != Set<EControlStyleElement>.Of())
                    {
                        Start += 1;
                    }

                    for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                    {
                        Index = FPropertyTable.Items[I].ID;
                        if (Index < Node.Count)
                        {
                            C = Node.Children[I - FPropertyTable.First + Start];
                            if (C != null)
                            {
                                rwCreateEObject(ref C, Node, Index);
                            }
                        }
                    }
                }

                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
            }
        }

        // Engine.pas:11010
        public override void rwData(Span<byte> Buffer, int Size, int ID)
        {
            byte[] image = new byte[Size];
            base.rwData(image, Size, ID);
            if (!image.AsSpan().SequenceEqual(Buffer[..Size])) Mismatch(ID);
        }

        // Engine.pas:11022
        public override void rwString(ref string S, int ID)
        {
            string Value = "";
            base.rwString(ref Value, ID);
            if (Value != S)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11030
        public override void rwStrings(StringCollection Strings, int ID)
        {
            StringList L;
            // { the original never frees L }
            L = new StringList();
            base.rwStrings(L, ID);
            var text = Strings.Text;
            if (CompareText(L.Text, text) != 0)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11040
        public override void rwStringsEx(StringCollection Strings, int ID)
        {
            StringList L;
            L = new StringList();
            base.rwStrings(L, ID);
            var text = Strings.Text;
            if (CompareText(L.Text, text) != 0)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11049
        public override void rwInteger(ref int I, int ID)
        {
            int Value = default;
            base.rwInteger(ref Value, ID);
            if (I != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11057
        public override void rwByte(ref byte B, int ID)
        {
            byte Value = default;
            base.rwByte(ref Value, ID);
            if (B != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11073
        public override void rwWord(ref ushort W, int ID)
        {
            ushort Value = default;
            base.rwWord(ref Value, ID);
            if (W != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11081
        public override void rwBool(ref bool B, int ID)
        {
            bool Value = default;
            base.rwBool(ref Value, ID);
            if (B != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11065
        public override void rwShortint(ref sbyte S, int ID)
        {
            sbyte Value = default;
            base.rwShortint(ref Value, ID);
            if (S != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11089
        public override void rwIntegerEx(ref int I, int Default, int ID)
        {
            int Value = default;
            base.rwIntegerEx(ref Value, Default, ID);
            if (I != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11097
        public override void rwByteEx(ref byte B, byte Default, int ID)
        {
            byte Value = default;
            base.rwByteEx(ref Value, Default, ID);
            if (B != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11113
        public override void rwWordEx(ref ushort W, ushort Default, int ID)
        {
            ushort Value = default;
            base.rwWordEx(ref Value, Default, ID);
            if (W != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11105
        public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
        {
            sbyte Value = default;
            base.rwShortintEx(ref Value, Default, ID);
            if (S != Value)
            {
                Mismatch(ID);
            }
        }

    }
}

namespace Aow.Game.Engine.Collections
{

    public delegate int StringsSortCompare(string S1, string S2, object O1, object O2);

    public struct StringHashItem
    {
        private string key;

        public string Key { readonly get => key ?? ""; set => key = value; }

        public object Item;
    }

    public delegate void ExchangeProc(DelphiList Data, int I, int J);

    public struct SortStringsContext
    {
        public StringCollection Strings;
        public StringsSortCompare Compare;
        public ExchangeProc ExchangeProc;
        public DelphiList ExchangeData;

        internal static void QuickSortStrings(ref SortStringsContext Ctx, int L, int R)
        {
            int I;
            int J;
            int M;
            object PO;
            string P;
            do
            {
                I = L;
                J = R;
                M = (int)(((uint)(L + R)) >> 1);
                P = Ctx.Strings[M];
                PO = Ctx.Strings.Objects[M];
                do
                {
                    while (true)
                    {
                        var item = Ctx.Strings.Objects[I];
                        var stringsItem = Ctx.Strings[I];
                        if (!(Ctx.Compare(stringsItem, P, item, PO) < 0))
                            break;
                        I += 1;
                    }

                    while (true)
                    {
                        var item = Ctx.Strings.Objects[J];
                        var stringsItem = Ctx.Strings[J];
                        if (!(Ctx.Compare(stringsItem, P, item, PO) > 0))
                            break;
                        J -= 1;
                    }

                    if (I <= J)
                    {
                        Ctx.Strings.Exchange(I, J);
                        if (Ctx.ExchangeProc != null)
                        {
                            Ctx.ExchangeProc(Ctx.ExchangeData, I, J);
                        }

                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    QuickSortStrings(ref Ctx, L, J);
                }

                L = I;
            }
            while (!(I >= R));
        }

    }

    [LegacyObject]
    // { the layout of TList, to write its count field straight from the object }
    public partial class ListFields
    {
        public object FList;
        public int FCount;

        public ListFields()
        {
            CaptureAddress.Assign(this);
        }
    }
}
