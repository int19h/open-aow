// Translated from Engine.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.FMemStr;
using static Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.Engine
{
    public enum TEControlStyleElement : byte
    {
        csSaveLoad = 0,
        csFixedChild = 1,
        csVerifyFixedChildren = 2,
        csArrayMode = 3
    }

    public enum TEditModeElement : byte
    {
        emEdit = 0,
        emCut = 1,
        emCopy = 2,
        emPaste = 3,
        emDelete = 4
    }

    public enum TEngineUserElement : byte
    {
        euProgrammer = 0,
        euDesigner = 1,
        euUser = 2
    }

    public enum TEngineModeElement : byte
    {
        emVerifyEObjects = 0
    }

    public enum TStorageModeElement : byte
    {
        smRead = 0,
        smWrite = 1,
        smTextOnly = 2,
        smPreview = 3,
        smTranslate = 4,
        smSetupCompare = 5,
        smCompare = 6
    }

    public delegate int TIntegerSortCompare(int Item1, int Item2);
    public partial class TIntegerList : TPersistent
    {
        internal int[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal int FDefault;
        // Engine.pas:3343
        public int Get(int Index)
        {
            int Result = default;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Engine.FillInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:3537
        internal AnsiString GetItemString()
        {
            AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__1 = FCount - 1;
                for (; I <= end__1; I += 1)
                {
                    Result = Result + IntToStr(FList[I]) + AnsiString.FromBytes(44);
                }
            }

            if (Result != (AnsiString)"")
            {
                Result = Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:3548
        internal void SetItemString(AnsiString S)
        {
            AnsiString Item = default;
            int P = default;
            int V = default;
            Clear();
            P = Pos(AnsiString.FromBytes(44), S);
            while (S != (AnsiString)"")
            {
                if (P != 0)
                {
                    Item = Copy(S, 1, P - 1);
                    S = Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Item = S;
                    S = (AnsiString)"";
                }

                if (Item != (AnsiString)"")
                {
                    V = StrToInt(Item);
                }
                else
                {
                    V = 0;
                }

                Add(V);
                P = Pos(AnsiString.FromBytes(44), S);
            }
        }

        // Engine.pas:3351
        protected internal virtual void Grow()
        {
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:3273
        public override void Assign(TPersistent Source)
        {
            if (Source is TIntegerList)
            {
                var integerList = (TIntegerList)Source;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            int Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:3332
        public TIntegerList Expand()
        {
            TIntegerList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:3338
        public int First()
        {
            int Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:3361
        public int IndexOf(int Item)
        {
            int Result = default;
            Result = 0;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
        public int Last()
        {
            int Result = default;
            var call__3 = Get(FCount - 1);
            Result = call__3;
            return Result;
        }

        // Engine.pas:3384
        public void Move(int CurIndex, int NewIndex)
        {
            int Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:3404
        public int Remove(int Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:3410
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__4 = 0;
                for (; I >= end__4; I -= 1)
                {
                    var call__5 = this[I];
                    if (call__5 == FDefault)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:3465
        public void Sort(TIntegerSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Engine.QuickSortInteger(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:3471
        public bool Compare(TIntegerList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__6 = CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);
                Result = call__6;
            }

            return Result;
        }

        // Engine.pas:3478
        public void LoadFromStream(TStream Stream)
        {
            int Count = default;
            int Item = default;
            int Run = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 4));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (Count != FCount)
            {
                Stream.Read(Bytes.Of(ref Item, 4));
                if (Item == FDefault)
                {
                    Stream.Read(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:3499
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
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
            int Result = default;
            int I = default;
            int Total = default;
            int Chance = default;
            int R = default;
            if (FCount != 0)
            {
                Total = 0;
                {
                    I = FCount - 1;
                    byte end__7 = 0;
                    for (; I >= end__7; I -= 1)
                    {
                        Chance = FList[I] + Bonus;
                        if (Chance < 0)
                        {
                            Chance = 0;
                        }

                        Total += Chance;
                    }
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

        public AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }
        public int[] List { get => FList; }

        // Engine.pas:3310
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    public delegate int TByteSortCompare(byte Item1, byte Item2);
    public partial class TByteList : TPersistent
    {
        internal byte[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal byte FDefault;
        // Engine.pas:3678
        internal byte Get(int Index)
        {
            byte Result = default;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
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
        internal AnsiString GetItemString()
        {
            AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__8 = FCount - 1;
                for (; I <= end__8; I += 1)
                {
                    Result = Result + IntToStr(FList[I]) + AnsiString.FromBytes(44);
                }
            }

            if (Result != (AnsiString)"")
            {
                Result = Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:3894
        internal void SetItemString(AnsiString S)
        {
            AnsiString Item = default;
            int P = default;
            int V = default;
            Clear();
            P = Pos(AnsiString.FromBytes(44), S);
            while (S != (AnsiString)"")
            {
                if (P != 0)
                {
                    Item = Copy(S, 1, P - 1);
                    S = Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Item = S;
                    S = (AnsiString)"";
                }

                if (Item != (AnsiString)"")
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
                P = Pos(AnsiString.FromBytes(44), S);
            }
        }

        // Engine.pas:3686
        protected internal virtual void Grow()
        {
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:3608
        public override void Assign(TPersistent Source)
        {
            if (Source is TByteList)
            {
                var byteList = (TByteList)Source;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            byte Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:3667
        public TByteList Expand()
        {
            TByteList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:3673
        public byte First()
        {
            byte Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:3696
        public int IndexOf(byte Item)
        {
            int Result = default;
            Result = 0;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
        public byte Last()
        {
            byte Result = default;
            var call__10 = Get(FCount - 1);
            Result = call__10;
            return Result;
        }

        // Engine.pas:3719
        public void Move(int CurIndex, int NewIndex)
        {
            byte Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:3739
        public int Remove(byte Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:3745
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__11 = 0;
                for (; I >= end__11; I -= 1)
                {
                    var call__12 = this[I];
                    if (call__12 == FDefault)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:3800
        public void Sort(TByteSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Engine.QuickSortByte(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:3806
        public bool Compare(TByteList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__13 = CompareMem(FList.AsSpan(0, FCount * 1), List.FList.AsSpan(0, FCount * 1), FCount * 1);
                Result = call__13;
            }

            return Result;
        }

        // Engine.pas:3813
        public void LoadFromStream(TStream Stream)
        {
            byte Item = default;
            byte Run = default;
            int Count = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 1));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Bytes.Of(ref Item, 1));
                if (Item == FDefault)
                {
                    Stream.Read(Bytes.Of(ref Run, 1));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:3835
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            byte B = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
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
            int Result = default;
            int I = default;
            int Total = default;
            int Chance = default;
            int R = default;
            if (FCount != 0)
            {
                Total = 0;
                {
                    I = FCount - 1;
                    byte end__14 = 0;
                    for (; I >= end__14; I -= 1)
                    {
                        Chance = FList[I] + Bonus;
                        if (Chance < 0)
                        {
                            Chance = 0;
                        }

                        Total += Chance;
                    }
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

        public AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }
        public byte[] List { get => FList; }

        // Engine.pas:3645
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    public delegate int TShortIntSortCompare(sbyte Item1, sbyte Item2);
    public partial class TShortIntList : TPersistent
    {
        internal sbyte[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal sbyte FDefault;
        // Engine.pas:4025
        internal sbyte Get(int Index)
        {
            sbyte Result = default;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
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
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:3955
        public override void Assign(TPersistent Source)
        {
            if (Source is TShortIntList)
            {
                var shortIntList = (TShortIntList)Source;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            sbyte Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:4014
        public TShortIntList Expand()
        {
            TShortIntList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:4020
        public sbyte First()
        {
            sbyte Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:4043
        public int IndexOf(sbyte Item)
        {
            int Result = default;
            Result = 0;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
        public sbyte Last()
        {
            sbyte Result = default;
            var call__16 = Get(FCount - 1);
            Result = call__16;
            return Result;
        }

        // Engine.pas:4066
        public void Move(int CurIndex, int NewIndex)
        {
            sbyte Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:4086
        public int Remove(sbyte Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:4092
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__17 = 0;
                for (; I >= end__17; I -= 1)
                {
                    var call__18 = this[I];
                    if (call__18 == FDefault)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:4147
        public void Sort(TShortIntSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Engine.QuickSortShortInt(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:4153
        public bool Compare(TShortIntList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__19 = CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount * 1)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount * 1)), FCount * 1);
                Result = call__19;
            }

            return Result;
        }

        // Engine.pas:4160
        public void LoadFromStream(TStream Stream)
        {
            byte Run = default;
            sbyte Item = default;
            int Count = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 1));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Bytes.Of(ref Item, 1));
                if (Item == FDefault)
                {
                    Stream.Read(Bytes.Of(ref Run, 1));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:4183
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            byte B = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
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
            int Result = default;
            int I = default;
            int Total = default;
            int Chance = default;
            int R = default;
            if (FCount != 0)
            {
                Total = 0;
                {
                    I = FCount - 1;
                    byte end__20 = 0;
                    for (; I >= end__20; I -= 1)
                    {
                        Chance = FList[I] + Bonus;
                        if (Chance < 0)
                        {
                            Chance = 0;
                        }

                        Total += Chance;
                    }
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
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    public delegate int TWordSortCompare(ushort Item1, ushort Item2);
    public partial class TWordList : TPersistent
    {
        internal ushort[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal ushort FDefault;
        // Engine.pas:4336
        internal ushort Get(int Index)
        {
            ushort Result = default;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Engine.FillWord(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:4344
        protected internal virtual void Grow()
        {
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:4266
        public override void Assign(TPersistent Source)
        {
            if (Source is TWordList)
            {
                var wordList = (TWordList)Source;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            ushort Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:4325
        public TWordList Expand()
        {
            TWordList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:4331
        public ushort First()
        {
            ushort Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:4354
        public int IndexOf(ushort Item)
        {
            int Result = default;
            Result = 0;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
        public ushort Last()
        {
            ushort Result = default;
            var call__22 = Get(FCount - 1);
            Result = call__22;
            return Result;
        }

        // Engine.pas:4377
        public void Move(int CurIndex, int NewIndex)
        {
            ushort Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:4397
        public int Remove(ushort Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:4403
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__23 = 0;
                for (; I >= end__23; I -= 1)
                {
                    var call__24 = this[I];
                    if (call__24 == FDefault)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:4458
        public void Sort(TWordSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Engine.QuickSortWord(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:4464
        public bool Compare(TWordList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__25 = CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 2);
                Result = call__25;
            }

            return Result;
        }

        // Engine.pas:4471
        public void LoadFromStream(TStream Stream)
        {
            ushort Item = default;
            ushort Run = default;
            int Count = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 2));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Bytes.Of(ref Item, 2));
                if (Item == FDefault)
                {
                    Stream.Read(Bytes.Of(ref Run, 2));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:4493
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            ushort B = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
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
            int Result = default;
            int I = default;
            int Total = default;
            int Chance = default;
            int R = default;
            if (FCount != 0)
            {
                Total = 0;
                {
                    I = FCount - 1;
                    byte end__26 = 0;
                    for (; I >= end__26; I -= 1)
                    {
                        Chance = FList[I] + Bonus;
                        if (Chance < 0)
                        {
                            Chance = 0;
                        }

                        Total += Chance;
                    }
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
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    public partial class TBitList : TPersistent
    {
        internal Memory<byte> FList = Memory<byte>.Empty;
        internal int FCount;
        internal int FCapacity;
        internal bool FDefault;
        // Engine.pas:4743
        internal void SetCapacity(int NewCapacity)
        {
            int I = default;
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
                {
                    I = FCount;
                    int end__27 = NewCapacity - 1;
                    for (; I <= end__27; I += 1)
                    {
                        Put(I, FDefault);
                    }
                }
            }
        }

        // Engine.pas:4758
        internal void SetCount(int NewCount)
        {
            int I = default;
            int OldCount = default;
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                OldCount = FCount;
                FCount = NewCount;
                {
                    I = OldCount;
                    int end__28 = FCount - 1;
                    for (; I <= end__28; I += 1)
                    {
                        Put(I, FDefault);
                    }
                }
            }
            else
            {
                FCount = NewCount;
            }
        }

        // Engine.pas:4798
        internal AnsiString GetItemString()
        {
            AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__29 = FCount - 1;
                for (; I <= end__29; I += 1)
                {
                    var call__30 = Get(I);
                    if (call__30 != FDefault)
                    {
                        Result = Result + IntToStr(I) + AnsiString.FromBytes(44);
                    }
                }
            }

            if (Result != (AnsiString)"")
            {
                Result = Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:4810
        internal void SetItemString(AnsiString S)
        {
            AnsiString Item = default;
            int P = default;
            Clear();
            P = Pos(AnsiString.FromBytes(44), S);
            while (S != (AnsiString)"")
            {
                if (P != 0)
                {
                    Item = Copy(S, 1, P - 1);
                    S = Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Item = S;
                    S = (AnsiString)"";
                }

                if (Item != (AnsiString)"")
                {
                    Put(StrToInt(Item), !FDefault);
                }

                P = Pos(AnsiString.FromBytes(44), S);
            }
        }

        // Engine.pas:4834
        internal int GetBitsSet()
        {
            int Result = default;
            int I = default;
            Result = 0;
            {
                I = 0;
                int end__31 = FCount - 1;
                for (; I <= end__31; I += 1)
                {
                    if (Get(I))
                    {
                        Result += 1;
                    }
                }
            }

            return Result;
        }

        // Engine.pas:4843
        internal int GetBitsCleared()
        {
            int Result = default;
            int I = default;
            Result = 0;
            {
                I = 0;
                int end__32 = FCount - 1;
                for (; I <= end__32; I += 1)
                {
                    if (!Get(I))
                    {
                        Result += 1;
                    }
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:4576
        public override void Assign(TPersistent Source)
        {
            if (Source is TBitList)
            {
                var bitList = (TBitList)Source;
                this.FCount = bitList.FCount;
                this.SetCapacity(bitList.FCount);
                Rtl.Rtl.Move(bitList.FList.Span.Slice(0, (int)(((uint)(this.FCount + 31)) >> 5) * 4), this.FList.Span.Slice(0, (int)(((uint)(this.FCount + 31)) >> 5) * 4), (int)(((uint)(this.FCount + 31)) >> 5) * 4);
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
            int Result = default;
            Result = FCount;
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
            int I = default;
            if (Index < 0 || Index >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                {
                    I = Index;
                    int end__34 = FCount - 1;
                    for (; I <= end__34; I += 1)
                    {
                        var call__35 = Get(I + 1);
                        Put(I, call__35);
                    }
                }
            }
        }

        // Engine.pas:4624
        public void Exchange(int Index1, int Index2)
        {
            bool Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = Get(Index1);
            var call__36 = Get(Index2);
            Put(Index1, call__36);
            Put(Index2, Item);
        }

        // Engine.pas:4635
        public TBitList Expand()
        {
            TBitList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:4641
        public bool First()
        {
            bool Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:4688
        public int IndexOf(bool Item)
        {
            int Result = default;
            Result = 0;
            while (true)
            {
                bool bool__37 = Result < FCount;
                if (bool__37)
                {
                    var call__38 = Get(Result);
                    bool__37 = Item != call__38;
                }

                if (!bool__37)
                    break;
                {
                    Result += 1;
                }
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
            int I = default;
            if (Index < 0 || Index > FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                FCount += 1;
                // { sic: the original copies the same bit FCount - Index - 1 times instead of moving the tail }
                {
                    I = FCount - 2;
                    int end__39 = Index;
                    for (; I >= end__39; I -= 1)
                    {
                        var call__40 = Get(Index);
                        Put(Index + 1, call__40);
                    }
                }
            }
            else
            {
                FCount += 1;
            }

            Put(Index, Item);
        }

        // Engine.pas:4712
        public bool Last()
        {
            bool Result = default;
            var call__41 = Get(FCount - 1);
            Result = call__41;
            return Result;
        }

        // Engine.pas:4717
        public void Move(int CurIndex, int NewIndex)
        {
            bool Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:4730
        public int Remove(bool Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:4736
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__42 = 0;
                for (; I >= end__42; I -= 1)
                {
                    var call__43 = this[I];
                    if (call__43 == FDefault)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:4775
        public bool Compare(TBitList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__44 = CompareMem(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), List.FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)), (int)(((uint)(FCount + 7)) >> 3));
                Result = call__44;
            }

            return Result;
        }

        // Engine.pas:4782
        public void LoadFromStream(TStream Stream)
        {
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 1));
            Stream.Read(Bytes.Of(ref FCount, 4));
            SetCapacity(FCount);
            Stream.Read(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
        }

        // Engine.pas:4791
        public void SaveToStream(TStream Stream)
        {
            Stream.Write(Bytes.Of(ref FDefault, 1));
            Stream.Write(Bytes.Of(ref FCount, 4));
            Stream.Write(FList.Span.Slice(0, (int)(((uint)(FCount + 7)) >> 3)));
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public bool Default { get => FDefault; set => FDefault = value; }

        public bool this[int Index] { get => Get(Index); set => Put(Index, value); }

        public AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }
        public int BitsSet { get => GetBitsSet(); }
        public int BitsCleared { get => GetBitsCleared(); }

        // Engine.pas:4613
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    public partial class TEventList : TPersistent
    {
        internal TNotifyEvent[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal TNotifyEvent FDefault;
        // Engine.pas:4929
        internal TNotifyEvent Get(int Index)
        {
            TNotifyEvent Result = default;
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
        internal void Put(int Index, TNotifyEvent Item)
        {
            if (Index < 0)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            int I = default;
            int OldCount = default;
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                OldCount = FCount;
                FCount = NewCount;
                {
                    I = OldCount;
                    int end__45 = FCount - 1;
                    for (; I <= end__45; I += 1)
                    {
                        Put(I, FDefault);
                    }
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
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:4859
        public override void Assign(TPersistent Source)
        {
            if (Source is TEventList)
            {
                var eventList = (TEventList)Source;
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
        public int Add(TNotifyEvent Item)
        {
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            TNotifyEvent Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:4918
        public TEventList Expand()
        {
            TEventList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:4924
        public TNotifyEvent First()
        {
            TNotifyEvent Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:4947
        public int IndexOf(TNotifyEvent Item)
        {
            int Result = default;
            Delegate M = default;
            Result = 0;
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
        public void Insert(int Index, TNotifyEvent Item)
        {
            if (Index < 0 || Index > FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
        public TNotifyEvent Last()
        {
            TNotifyEvent Result = default;
            var call__47 = Get(FCount - 1);
            Result = call__47;
            return Result;
        }

        // Engine.pas:4974
        public void Move(int CurIndex, int NewIndex)
        {
            TNotifyEvent Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:4994
        public int Remove(TNotifyEvent Item)
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
            int I = default;
            Delegate M = default;
            M = FDefault;
            // { sic: the original deletes an item only when both halves differ from the default }
            {
                I = FCount - 1;
                byte end__48 = 0;
                for (; I >= end__48; I -= 1)
                {
                    if (MethodTarget(FList[I]) != MethodTarget(M) && MethodCode(FList[I]) != MethodCode(M))
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:5039
        public bool Compare(TEventList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__49 = FList.AsSpan(0, FCount).SequenceEqual(List.FList.AsSpan(0, FCount));
                Result = call__49;
            }

            return Result;
        }

        // Engine.pas:5046
        public void TriggerNotifyEvents(TObject Sender)
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__50 = 0;
                for (; I >= end__50; I -= 1)
                {
                    if (FList[I] != null)
                    {
                        FList[I](Sender);
                    }
                }
            }
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public TNotifyEvent Default { get => FDefault; set => FDefault = value; }

        public TNotifyEvent this[int Index] { get => Get(Index); set => Put(Index, value); }

        public TNotifyEvent[] List { get => FList; }

        // Engine.pas:4896
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct TDoubleItem
    {
        [FieldOffset(0)]
        public int A;
        [FieldOffset(4)]
        public int B;
    }

    public delegate int TDoubleItemSortCompare(in TDoubleItem Item1, in TDoubleItem Item2);
    public partial class TDoubleItemList : TPersistent
    {
        internal TDoubleItem[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal TDoubleItem FDefault;
        // Engine.pas:5140
        internal TDoubleItem Get(int Index)
        {
            TDoubleItem Result = default;
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
        internal void Put(int Index, TDoubleItem Item)
        {
            if (Index < 0)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Engine.FillLargeInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:5148
        protected internal virtual void Grow()
        {
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:5061
        public override void Assign(TPersistent Source)
        {
            if (Source is TDoubleItemList)
            {
                var doubleItemList = (TDoubleItemList)Source;
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
        public int Add(in TDoubleItem Item)
        {
            int Result = default;
            Result = FCount;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Rtl.Rtl.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 8);
            }
        }

        // Engine.pas:5118
        public void Exchange(int Index1, int Index2)
        {
            TDoubleItem Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:5129
        public TDoubleItemList Expand()
        {
            TDoubleItemList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:5135
        public TDoubleItem First()
        {
            TDoubleItem Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:5158
        public int IndexOf(in TDoubleItem Item)
        {
            int Result = default;
            Result = 0;
            while (Result < FCount && (double)Unsafe.As<TDoubleItem, long>(ref FList[Result]) != Unsafe.BitCast<TDoubleItem, long>(Item))
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
            int Result = default;
            Result = 0;
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
            int Result = default;
            Result = 0;
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
        public void Insert(int Index, in TDoubleItem Item)
        {
            if (Index < 0 || Index > FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Rtl.Rtl.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 8);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:5190
        public TDoubleItem Last()
        {
            TDoubleItem Result = default;
            var call__52 = Get(FCount - 1);
            Result = call__52;
            return Result;
        }

        // Engine.pas:5195
        public void Move(int CurIndex, int NewIndex)
        {
            TDoubleItem Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:5215
        public int Remove(in TDoubleItem Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:5221
        public void Pack()
        {
            int I = default;
            TDoubleItem Item = default;
            {
                I = FCount - 1;
                byte end__53 = 0;
                for (; I >= end__53; I -= 1)
                {
                    var call__54 = this[I];
                    Item = call__54;
                    if ((double)Unsafe.As<TDoubleItem, long>(ref Item) == Unsafe.As<TDoubleItem, long>(ref FDefault))
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:5281
        public void Sort(TDoubleItemSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Engine.QuickSortDoubleItem(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:5307
        public void SortAscendingItemA()
        {
            Sort(Engine.CompareDoubleItemAAscending);
        }

        // Engine.pas:5312
        public void SortAscendingItemB()
        {
            Sort(Engine.CompareDoubleItemBAscending);
        }

        // Engine.pas:5317
        public void SortDescendingItemA()
        {
            Sort(Engine.CompareDoubleItemADescending);
        }

        // Engine.pas:5322
        public void SortDescendingItemB()
        {
            Sort(Engine.CompareDoubleItemBDescending);
        }

        // Engine.pas:5327
        public bool Compare(TDoubleItemList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__55 = CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 8);
                Result = call__55;
            }

            return Result;
        }

        // Engine.pas:5334
        public void LoadFromStream(TStream Stream)
        {
            TDoubleItem Item = default;
            int Run = default;
            int Count = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 8));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Bytes.Of(ref Item, 8));
                if ((double)Unsafe.As<TDoubleItem, long>(ref Item) == Unsafe.As<TDoubleItem, long>(ref FDefault))
                {
                    Stream.Read(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:5356
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
                J = I;
                while (J < FCount && (double)Unsafe.As<TDoubleItem, long>(ref FList[J]) == Unsafe.As<TDoubleItem, long>(ref FDefault))
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
            int Result = default;
            int I = default;
            int Total = default;
            int Chance = default;
            int R = default;
            if (FCount != 0)
            {
                Total = 0;
                {
                    I = FCount - 1;
                    byte end__56 = 0;
                    for (; I >= end__56; I -= 1)
                    {
                        Chance = FList[I].A + Bonus;
                        if (Chance < 0)
                        {
                            Chance = 0;
                        }

                        Total += Chance;
                    }
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
            int Result = default;
            int I = default;
            int Total = default;
            int Chance = default;
            int R = default;
            if (FCount != 0)
            {
                Total = 0;
                {
                    I = FCount - 1;
                    byte end__57 = 0;
                    for (; I >= end__57; I -= 1)
                    {
                        Chance = FList[I].B + Bonus;
                        if (Chance > 0)
                        {
                            Total += Chance;
                        }
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
        public TDoubleItem Default { get => FDefault; set => FDefault = value; }

        public TDoubleItem this[int Index] { get => Get(Index); set => Put(Index, value); }

        public TDoubleItem[] List { get => FList; }

        // Engine.pas:5107
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct TQuadInteger
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

    public delegate int TQuadItemSortCompare(in TQuadInteger Item1, in TQuadInteger Item2);
    public partial class TQuadItemList : TPersistent
    {
        internal TQuadInteger[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal TQuadInteger FDefault;
        // Engine.pas:5538
        internal TQuadInteger Get(int Index)
        {
            TQuadInteger Result = default;
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
        internal void Put(int Index, TQuadInteger Item)
        {
            if (Index < 0)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > Engine.MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > Engine.MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Engine.FillQuadInteger(FList.AsSpan(FCount), NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:5546
        protected internal virtual void Grow()
        {
            int Delta = default;
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
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:5457
        public override void Assign(TPersistent Source)
        {
            if (Source is TQuadItemList)
            {
                var quadItemList = (TQuadItemList)Source;
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
        public int Add(in TQuadInteger Item)
        {
            int Result = default;
            Result = FCount;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Rtl.Rtl.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 16);
            }
        }

        // Engine.pas:5516
        public void Exchange(int Index1, int Index2)
        {
            TQuadInteger Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:5527
        public TQuadItemList Expand()
        {
            TQuadItemList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:5533
        public TQuadInteger First()
        {
            TQuadInteger Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:5556
        public int IndexOf(in TQuadInteger Item)
        {
            int Result = default;
            Result = 0;
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
            int Result = default;
            Result = 0;
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
            int Result = default;
            Result = 0;
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
            int Result = default;
            Result = 0;
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
            int Result = default;
            Result = 0;
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
        public void Insert(int Index, in TQuadInteger Item)
        {
            if (Index < 0 || Index > FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Rtl.Rtl.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 16);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:5603
        public TQuadInteger Last()
        {
            TQuadInteger Result = default;
            var call__59 = Get(FCount - 1);
            Result = call__59;
            return Result;
        }

        // Engine.pas:5608
        public void Move(int CurIndex, int NewIndex)
        {
            TQuadInteger Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:5628
        public int Remove(in TQuadInteger Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // Engine.pas:5634
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__60 = 0;
                for (; I >= end__60; I -= 1)
                {
                    if (FList[I].A == FDefault.A && FList[I].B == FDefault.B && FList[I].C == FDefault.C && FList[I].D == FDefault.D)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:5691
        public void Sort(TQuadItemSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Engine.QuickSortQuadItem(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:5737
        public void SortAscendingItemA()
        {
            Sort(Engine.CompareQuadItemAAscending);
        }

        // Engine.pas:5742
        public void SortAscendingItemB()
        {
            Sort(Engine.CompareQuadItemBAscending);
        }

        // Engine.pas:5747
        public void SortAscendingItemC()
        {
            Sort(Engine.CompareQuadItemCAscending);
        }

        // Engine.pas:5752
        public void SortAscendingItemD()
        {
            Sort(Engine.CompareQuadItemDAscending);
        }

        // Engine.pas:5757
        public void SortDescendingItemA()
        {
            Sort(Engine.CompareQuadItemADescending);
        }

        // Engine.pas:5762
        public void SortDescendingItemB()
        {
            Sort(Engine.CompareQuadItemBDescending);
        }

        // Engine.pas:5767
        public void SortDescendingItemC()
        {
            Sort(Engine.CompareQuadItemCDescending);
        }

        // Engine.pas:5772
        public void SortDescendingItemD()
        {
            Sort(Engine.CompareQuadItemDDescending);
        }

        // Engine.pas:5777
        public bool Compare(TQuadItemList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__61 = CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 16);
                Result = call__61;
            }

            return Result;
        }

        // Engine.pas:5784
        public void LoadFromStream(TStream Stream)
        {
            int Run = default;
            int Count = default;
            TQuadInteger Item = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 16));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Bytes.Of(ref Item, 16));
                if (Item.A == FDefault.A && Item.B == FDefault.B && Item.C == FDefault.C && Item.D == FDefault.D)
                {
                    Stream.Read(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:5807
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
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
        public TQuadInteger Default { get => FDefault; set => FDefault = value; }

        public TQuadInteger this[int Index] { get => Get(Index); set => Put(Index, value); }

        public TQuadInteger[] List { get => FList; }

        // Engine.pas:5505
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
        }


    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct TPropertyItem
    {
        [FieldOffset(0)]
        public int ID;
        [FieldOffset(4)]
        public int Offset;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct TPropertySettings
    {
        [FieldOffset(0)]
        public int First;
        [FieldOffset(4)]
        public int NextIndex;
        [FieldOffset(8)]
        public int MaxID;
    }

    public partial class TPropertyTable : TObject
    {
        internal TPropertyItem[] FItems = [];
        internal int FCapacity;
        internal int FCount;
        internal int FNextIndex;
        internal int FMaxID;
        internal TPropertySettings[] FStack = [];
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
        public override void Destroy()
        {
            SetCapacity(0);
            Array.Resize(ref FStack, 0);
            base.Destroy();
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
            int Result = default;
            int I = default;
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
            bool Result = default;
            int I = default;
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

            ref var propertySettings = ref FStack[FStackCount];
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
            ref var propertySettings = ref FStack[FStackCount];
            FFirst = propertySettings.First;
            FNextIndex = propertySettings.NextIndex;
            FMaxID = propertySettings.MaxID;
        }

        // Engine.pas:2361
        public void LoadFromStream(TStream Stream)
        {
            int LongCount = default;
            int ID = default;
            int Offset = default;
            byte ShortCount = default;
            ushort Pair = default;
            ushort I = default;
            Stream.Read(Bytes.Of(ref ShortCount, 1));
            if ((ShortCount & 128) != 0)
            {
                ShortCount = (byte)(ShortCount & 127);
                Stream.Read(Bytes.Of(ref LongCount, 4));
            }
            else
            {
                LongCount = 0;
            }

            if (ShortCount != 0)
            {
                {
                    int first__66 = 0;
                    int end__64 = ShortCount - 1;
                    for (int ordinal__65 = first__66; ordinal__65 <= end__64; ordinal__65++)
                    {
                        I = (ushort)ordinal__65;
                        Stream.Read(Bytes.Of(ref Pair, 2));
                        Add(Pair & 255, (int)(((uint)Pair) >> 8));
                    }
                }
            }

            if (LongCount != 0)
            {
                {
                    int first__69 = 0;
                    int end__67 = LongCount - 1;
                    for (int ordinal__68 = first__69; ordinal__68 <= end__67; ordinal__68++)
                    {
                        I = (ushort)ordinal__68;
                        Stream.Read(Bytes.Of(ref ID, 4));
                        Stream.Read(Bytes.Of(ref Offset, 4));
                        Add(ID, Offset);
                    }
                }
            }
        }

        // Engine.pas:2391
        public void SaveToStream(TStream Stream)
        {
            int LongCount = default;
            int I = default;
            byte ShortCount = default;
            byte Marker = default;
            int Pair = default;
            LongCount = 0;
            ShortCount = 0;
            {
                I = FFirst;
                int end__70 = FCount - 1;
                for (; I <= end__70; I += 1)
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
                {
                    I = FFirst;
                    int end__71 = FCount - 1;
                    for (; I <= end__71; I += 1)
                    {
                        if (FItems[I].ID <= 127 && FItems[I].Offset <= 255)
                        {
                            Pair = FItems[I].ID + (FItems[I].Offset << 8);
                            Stream.Write(Bytes.Of(ref Pair, 2));
                        }
                    }
                }
            }

            if (LongCount != 0)
            {
                {
                    I = FFirst;
                    int end__72 = FCount - 1;
                    for (; I <= end__72; I += 1)
                    {
                        if (FItems[I].ID > 127 || FItems[I].Offset > 255)
                        {
                            Stream.Write(Bytes.Of(ref FItems[I].ID, 4));
                            Stream.Write(Bytes.Of(ref FItems[I].Offset, 4));
                        }
                    }
                }
            }
        }

        public TPropertyItem[] Items { get => FItems; }
        public int Count { get => FCount; }
        public int First { get => FFirst; }
    }

    // FOwner
    public partial class TEObject : TObject
    {
        // Engine.pas:2461
        public const int ClassID = 80;

        private EClass eclass;
        // The engine class descriptor of the instance's exact type, read once and cached.
        public EClass EClass => eclass ??= EClass.Of(GetType());

        // The Delphi pre-inherited statements run after capture and before any inherited body.
        private protected virtual void InitializeBeforeInherited() { }

        public TEObject(TECustomNode owner) : this(null, owner) { }

        private protected TEObject(ConstructionContext context, TECustomNode owner)
        {
            context?.Capture(this);
            InitializeBeforeInherited();
            SetOwner(owner);
        }

        protected internal TECustomNode FOwner;
        // Engine.pas:2487
        internal AnsiString GetEObjectPath()
        {
            AnsiString Result = default;
            TEObject P = default;
            AnsiString S = default;
            S = (AnsiString)ClassNameOf(this.GetType());
            P = FOwner;
            while (P != null)
            {
                S = (AnsiString)ClassNameOf(P.GetType()) + (AnsiString)"" + S;
                P = P.FOwner;
            }

            Result = S;
            return Result;
        }

        // Engine.pas:2442
        public virtual int ClassVersion()
        {
            int Result = default;
            Result = 0;
            return Result;
        }

        // Engine.pas:2563
        public virtual TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = TSet<TEControlStyleElement>.Of(Engine.csStored);
            return Result;
        }

        // Engine.pas:2568
        public virtual void SetOwner(TECustomNode AOwner)
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
        public virtual TEngine GetEngine()
        {
            TEngine Result = default;
            var call__73 = FOwner.GetEngine();
            Result = call__73;
            return Result;
        }

        // Engine.pas:2502
        public virtual int GetLastMsg()
        {
            int Result = default;
            Result = 2147483647;
            return Result;
        }

        // Engine.pas:2507
        public virtual void SetLastMsg(int Value)
        {
        }

        // Engine.pas:2590
        public virtual void ReadWrite(TEStorageStream Stream)
        {
        }

        // Engine.pas:2594
        public virtual void MainReadWrite(TEStorageStream Stream)
        {
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Engine.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
            {
                Clear();
            }

            ReadWrite(Stream);
        }

        // Engine.pas:2447
        public virtual int AddRef()
        {
            int Result = default;
            throw Exception.Create((AnsiString)"Reference counting not supported");
            return Result;
        }

        // Engine.pas:2452
        public virtual int Release()
        {
            int Result = default;
            throw Exception.Create((AnsiString)"Reference counting not supported");
            return Result;
        }

        // Engine.pas:2457
        public virtual void Clear()
        {
        }

        // Engine.pas:2511
        public virtual bool Compare(TEObject Obj, TEngine AEngine)
        {
            bool Result = default;
            TMemoryStream S1 = default;
            TMemoryStream S2 = default;
            Result = false;
            var call__74 = EClass.ClassID;
            var call__75 = Obj.EClass.ClassID;
            if (call__74 != call__75)
            {
                return Result;
            }

            S1 = new TMemoryStream();
            S2 = new TMemoryStream();
            try
            {
                AEngine.WriteEObject(S1, this, null);
                AEngine.WriteEObject(S2, Obj, null);
                var call__76 = S1.Size;
                var call__77 = S2.Size;
                bool bool__78 = call__76 == call__77;
                if (bool__78)
                {
                    var call__79 = S1.Size;
                    var call__80 = CompareMem(S1.Memory.AsSpan(0, call__79), S2.Memory.AsSpan(0, call__79), call__79);
                    bool__78 = call__80;
                }

                Result = bool__78;
            }
            finally
            {
                S1.Free();
                S2.Free();
            }

            return Result;
        }

        // Engine.pas:2466
        public virtual EditorClass Editor()
        {
            EditorClass Result = default;
            TEngine E = default;
            Result = null;
            E = GetEngine();
            if (E != null)
            {
                for (var C = EClass; C.Type != typeof(TEObject) && Result == null; C = C.Parent)
                {
                    Result = E.FEditors.FindEditor(C.ClassID);
                }
            }

            if (Result == null)
            {
                Result = EditorClass.Of<TEObjectEditor>();
            }

            return Result;
        }

        // Engine.pas:2529
        public virtual void Assign(TEObject Source, TEngine AEngine)
        {
            TMemoryStream S = default;
            S = new TMemoryStream();
            try
            {
                AEngine.WriteEObject(S, Source, null);
                S.Position = 0;
                AEngine.ReadEObject(S, this, null);
            }
            finally
            {
                S.Destroy();
            }
        }

        // Engine.pas:2483
        public virtual void TimerEvent(int Tick, TEObject Sender)
        {
        }

        // Engine.pas:2585
        public virtual void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            if (Msg.MsgID == 1)
            {
                SetLastMsg(2147483647);
            }
        }

        // Engine.pas:2578
        public virtual void SetArrayOwner(TECustomNode AOwner, int Index)
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
        public override void Destroy()
        {
            SetOwner(null);
            base.Destroy();
        }

        // Engine.pas:2543
        public TEObject Copy(TEngine AEngine)
        {
            TEObject Result = default;
            TMemoryStream S = default;
            S = new TMemoryStream();
            try
            {
                AEngine.WriteEObject(S, this, null);
                S.Position = 0;
                Result = EClass.Create(null);
                AEngine.ReadEObject(S, Result, null);
            }
            finally
            {
                S.Destroy();
            }

            return Result;
        }

        public TECustomNode Owner { get => FOwner; set => SetOwner(value); }
        public AnsiString EObjectPath { get => GetEObjectPath(); }
        public int LastMsg { get => GetLastMsg(); set => SetLastMsg(value); }


    }

    // children through an abstract list interface. Message ids carry flags: $10000000 deliver once
    // per Seq, $20000000 owned children only, $40000000 do not forward to children
    public partial class TECustomNode : TEObject
    {

        // Engine.pas:2430
        public TECustomNode(TECustomNode owner) : this(null, owner) { }

        private protected TECustomNode(ConstructionContext context, TECustomNode owner) : base(context, owner)
        { }

        // Engine.pas:2666
        public override void MainReadWrite(TEStorageStream Stream)
        {
            if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
            {
                if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(Engine.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of())
                {
                    Clear();
                }
            }

            ReadWrite(Stream);
            ReadWriteChildren(Stream);
        }

        // Engine.pas:2626
        public override void Clear()
        {
            int I = default;
            TEObject C = default;
            var call__83 = Count;
            if (call__83 != 0)
            {
                {
                    I = Count - 1;
                    byte end__84 = 0;
                    for (; I >= end__84; I -= 1)
                    {
                        var call__85 = Children[I];
                        C = call__85;
                        if (C != null && C.Owner == this)
                        {
                            if (!C.GetControlStyle().Contains(Engine.csShared))
                            {
                                C.Destroy();
                            }
                            else
                            {
                                C.Clear();
                            }
                        }
                    }
                }

                {
                    I = Count - 1;
                    byte end__86 = 0;
                    for (; I >= end__86; I -= 1)
                    {
                        var call__87 = Children[I];
                        C = call__87;
                        if (C != null)
                        {
                            bool bool__88 = C.Owner == this;
                            if (bool__88)
                            {
                                var call__89 = C.GetControlStyle();
                                bool__88 = call__89.Contains(Engine.csShared);
                            }

                            if (!bool__88)
                            {
                                RemoveChild(C);
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:2648
        public override void Assign(TEObject Source, TEngine AEngine)
        {
            Clear();
            base.Assign(Source, AEngine);
        }

        // Engine.pas:2720
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (SendToChild(ref Msg))
            {
                SendMsg(ref Msg, Sender);
            }
        }

        // Engine.pas:593
        public virtual TEObject GetChild(int Index)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:594
        public virtual void SetChild(int Index, TEObject Child)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:595
        public virtual int GetCount()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:2661
        public virtual void ReadWriteChildren(TEStorageStream Stream)
        {
            Stream.rwChildren(this, 1);
        }

        // Engine.pas:2715
        public virtual bool SendToChild(ref TEMsg Msg)
        {
            bool Result = default;
            Result = (Msg.MsgID & 1073741824) == 0;
            return Result;
        }

        // Engine.pas:2608
        public virtual void DestroyAllChildren()
        {
            TEObject C = default;
            int I = default;
            {
                I = Count - 1;
                byte end__90 = 0;
                for (; I >= end__90; I -= 1)
                {
                    var call__91 = Children[I];
                    C = call__91;
                    if (C != null && C.Owner == this)
                    {
                        C.Destroy();
                    }
                }
            }

            var call__92 = Count;
            if (call__92 != 0)
            {
                {
                    I = Count - 1;
                    byte end__93 = 0;
                    for (; I >= end__93; I -= 1)
                    {
                        var call__94 = Children[I];
                        C = call__94;
                        if (C != null)
                        {
                            RemoveChild(C);
                        }
                    }
                }
            }
        }

        // Engine.pas:2654
        public virtual void QuickClear()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__95 = 0;
                for (; I >= end__95; I -= 1)
                {
                    var call__96 = Children[I];
                    RemoveChild(call__96);
                }
            }
        }

        // Engine.pas:2674
        public virtual void SendMsg(ref TEMsg Msg, TEObject Sender)
        {
            int I = default;
            TEObject C = default;
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    {
                        I = Count - 1;
                        byte end__97 = 0;
                        for (; I >= end__97; I -= 1)
                        {
                            var call__98 = Children[I];
                            C = call__98;
                            bool bool__99 = C != null;
                            if (bool__99)
                            {
                                var call__100 = C.GetLastMsg();
                                bool__99 = call__100 != Msg.Seq;
                            }

                            if (bool__99)
                            {
                                C.SetLastMsg(Msg.Seq);
                                C.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
                else
                {
                    {
                        I = Count - 1;
                        byte end__101 = 0;
                        for (; I >= end__101; I -= 1)
                        {
                            var call__102 = Children[I];
                            C = call__102;
                            bool bool__103 = C != null;
                            if (bool__103)
                            {
                                var call__104 = C.GetLastMsg();
                                bool__103 = call__104 != Msg.Seq;
                            }

                            if (bool__103 && C.Owner == this)
                            {
                                C.SetLastMsg(Msg.Seq);
                                C.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    {
                        I = Count - 1;
                        byte end__105 = 0;
                        for (; I >= end__105; I -= 1)
                        {
                            var call__106 = Children[I];
                            C = call__106;
                            if (C != null)
                            {
                                C.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
                else
                {
                    {
                        I = Count - 1;
                        byte end__107 = 0;
                        for (; I >= end__107; I -= 1)
                        {
                            var call__108 = Children[I];
                            C = call__108;
                            if (C != null && C.Owner == this)
                            {
                                C.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:601
        public virtual int RemoveChild(TEObject Child)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:602
        public virtual int AddChild(TEObject Child)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:2786
        // { The original tests Result before the child-count call. }
        public virtual int DestroyChildren(int AClassID)
        {
            int Result = default;
            int I = default;
            TEObject C = default;
            int N = default;
            N = 0;
            {
                I = Count - 1;
                byte end__109 = 0;
                for (; I >= end__109; I -= 1)
                {
                    var call__110 = Children[I];
                    C = call__110;
                    if (C != null && C.EClass.ClassID == AClassID)
                    {
                        C.Destroy();
                        N += 1;
                    }
                }
            }

            Result = N;
            return Result;
        }

        // Engine.pas:2768
        public virtual TEObject FindChildInTree(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            Result = FindChild(AClassID);
            if (Result == null)
            {
                I = 0;
                while (true)
                {
                    bool bool__111 = Result == null;
                    if (bool__111)
                    {
                        var call__112 = Count;
                        bool__111 = I < call__112;
                    }

                    if (!bool__111)
                        break;
                    {
                        var call__113 = Children[I];
                        if (call__113 is TECustomNode)
                        {
                            var call__114 = Children[I];
                            var call__115 = ((TECustomNode)call__114).FindChildInTree(AClassID);
                            Result = call__115;
                        }

                        I += 1;
                    }
                }
            }

            return Result;
        }

        // Engine.pas:2726
        public virtual TEObject FindChild(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            bool Found = default;
            TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (true)
            {
                bool bool__116 = !Found;
                if (bool__116)
                {
                    var call__117 = Count;
                    bool__116 = I < call__117;
                }

                if (!bool__116)
                    break;
                {
                    var call__118 = Children[I];
                    C = call__118;
                    Found = C != null && C.EClass.ClassID == AClassID;
                    I += 1;
                }
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
        public virtual TEObject FindOwnedChild(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            bool Found = default;
            TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (true)
            {
                bool bool__119 = !Found;
                if (bool__119)
                {
                    var call__120 = Count;
                    bool__119 = I < call__120;
                }

                if (!bool__119)
                    break;
                {
                    var call__121 = Children[I];
                    C = call__121;
                    bool bool__122 = C != null && C.Owner == this;
                    if (bool__122)
                    {
                        var call__123 = C.EClass.ClassID;
                        bool__122 = AClassID == call__123;
                    }

                    Found = bool__122;
                    I += 1;
                }
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
        public virtual int FindChildIndex(TEObject Child)
        {
            int Result = default;
            var call__124 = Count;
            Result = call__124 - 1;
            while (true)
            {
                bool bool__125 = Result >= 0;
                if (bool__125)
                {
                    var call__126 = Children[Result];
                    bool__125 = Child != call__126;
                }

                if (!bool__125)
                    break;
                {
                    Result -= 1;
                }
            }

            return Result;
        }

        // Engine.pas:2602
        public override void Destroy()
        {
            DestroyAllChildren();
            base.Destroy();
        }

        public int Count { get => GetCount(); }

        public sealed class ChildrenAccessor(TECustomNode target)
        {
            public TEObject this[int Index] { get => target.GetChild(Index); set => target.SetChild(Index, value); }
        }

        public ChildrenAccessor Children => new ChildrenAccessor(this);
    }

    // FMsgCount, FEClasses, FClassTable, FEditors, FControlNode, FComponent (the owning component),
    // FDebugIDs, FDebugEvents, FStartupDirectory, FOptions. Every engine is in the Engines list
    public partial class TEngine : TENode
    {
        // Engine.pas:8756
        public new const int ClassID = 17;

        internal int FMsgCount;
        internal TEObjectClassList FEClasses;
        internal TIntegerHashTable256 FClassTable;
        internal TEObjectEditorList FEditors;
        internal TEControlStyleNode FControlNode;
        public TEComponent FComponent;
        internal TStringList FDebugIDs;
        internal TEventList FDebugEvents;
        internal AnsiString FStartupDirectory;
        public TSet<TEngineUserElement> FUser;
        internal TSet<TEngineModeElement> FOptions;
        // Engine.pas:8776
        internal int CreateUniqueDebugID()
        {
            int Result = default;
            Result = 0;
            while (true)
            {
                var call__127 = FDebugIDs.IndexOfObject(IntToObject(Result));
                if (!(call__127 != -1))
                    break;
                {
                    Result += 1;
                }
            }

            return Result;
        }

        // Engine.pas:8814
        public override TEngine GetEngine()
        {
            TEngine Result = default;
            Result = this;
            return Result;
        }

        public TEngine(TECustomNode owner) : this(null, owner) { }

        private protected TEngine(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            UpdateStartupDirectory();
            FOptions = TSet<TEngineModeElement>.Of(Engine.eoVerifyClasses);
            var call__128 = new TEventList();
            FDebugEvents = call__128;
            var call__129 = new TEObjectClassList();
            FEClasses = call__129;
            var call__130 = new TIntegerHashTable256();
            FClassTable = call__130;
            var call__131 = new TEObjectEditorList();
            FEditors = call__131;
            var call__132 = new TEControlStyleNode(this);
            FControlNode = call__132;
            FControlNode.SetControlStyle(TSet<TEControlStyleElement>.Of(Engine.csShared));
            var call__133 = new TStringList();
            FDebugIDs = call__133;
            FDebugIDs.AddObject(Engine.UserStr, IntToObject(-1));
            RegisterEClasses([EClass.Of<TEControlStyleObject>(), EClass.Of<TEControlStyleNode>(), EClass.Of<TECustomOwner>(), EClass.Of<TEResourceEditSettings>()]);
            Engine.Engines.Add(this);
        }

        // Engine.pas:8745
        public override void Destroy()
        {
            FEditors.Free();
            Engine.Engines.Remove(this);
            FDebugEvents.Free();
            FDebugIDs.Free();
            base.Destroy();
            FClassTable.Free();
            FEClasses.Free();
        }

        // Engine.pas:8771
        public virtual void SetStartupDirectory(AnsiString Value)
        {
            FStartupDirectory = Value;
        }

        // Engine.pas:8766
        public virtual void UpdateStartupDirectory()
        {
            var call__134 = Application.ExeName;
            var call__135 = ExtractFilePath(call__134);
            FStartupDirectory = call__135;
        }

        // Engine.pas:8887
        public virtual TEObject CreateEObject(int AClassID, TECustomNode AOwner)
        {
            TEObject Result = default;
            EClass C = default;
            C = GetEClass(AClassID);
            Result = C.Create(AOwner);
            return Result;
        }

        // Engine.pas:8860
        public virtual EClass GetEClass(int AClassID)
        {
            EClass Result = default;
            object P = default;
            var call__136 = FClassTable.Find(AClassID);
            P = call__136;
            Result = (EClass)P;
            if (Result == null)
            {
                throw Exception.Create((AnsiString)"Unregistered E-ClassID found (" + IntToStr(AClassID) + AnsiString.FromBytes(41));
            }

            return Result;
        }

        // Engine.pas:8761
        public virtual AnsiString TranslateText(AnsiString Text)
        {
            AnsiString Result = default;
            Result = Text;
            return Result;
        }

        // Engine.pas:8782
        public int RegisterDebugID(AnsiString Name)
        {
            int Result = default;
            Result = CreateUniqueDebugID();
            FDebugIDs.AddObject(Name, IntToObject(Result));
            return Result;
        }

        // Engine.pas:8788
        public void UnregisterDebugID(int ID)
        {
            int I = default;
            var call__137 = FDebugIDs.IndexOfObject(IntToObject(ID));
            I = call__137;
            if (I != -1)
            {
                FDebugIDs.Delete(I);
            }
        }

        // Engine.pas:8796
        public void OutputDebugStr(AnsiString S)
        {
            var notification = new EngineDebugNotification();
            ref var Info = ref notification.Info;
            Info.ID = -1;
            Info.Text = S;
            FDebugEvents.TriggerNotifyEvents(notification);
        }

        // Engine.pas:8805
        public void OutputDebugMessage(int ID, AnsiString S)
        {
            var notification = new EngineDebugNotification();
            ref var Info = ref notification.Info;
            Info.ID = ID;
            Info.Text = S;
            FDebugEvents.TriggerNotifyEvents(notification);
        }

        // Engine.pas:8819
        public void ResetMsgCount()
        {
            TEMsg Msg = default;
            Engine.InitMsgEx(ref Msg, 1, this, FMsgCount + 1);
            MsgProc(ref Msg, this);
            FMsgCount = 0;
        }

        // Engine.pas:8828
        public int IncMsgCount()
        {
            int Result = default;
            FMsgCount += 1;
            if (FMsgCount > 268435455)
            {
                ResetMsgCount();
            }

            Result = FMsgCount;
            return Result;
        }

        // Engine.pas:8835
        public void RegisterEClasses(ReadOnlySpan<EClass> AClasses)
        {
            int I = default;
            {
                I = 0;
                int end__138 = AClasses.Length - 1;
                for (; I <= end__138; I += 1)
                {
                    if (FOptions.Contains(Engine.eoVerifyClasses))
                    {
                        VerifyEObjectClass(AClasses[I]);
                    }

                    var call__139 = AClasses[I].ClassID;
                    var call__140 = FClassTable.Find(call__139);
                    if (call__140 != null)
                    {
                        throw Exception.Create((AnsiString)"EObject Class ID double defined!");
                    }

                    FEClasses.Add(AClasses[I]);
                    var call__141 = AClasses[I].ClassID;
                    FClassTable.Add(AClasses[I], call__141);
                }
            }
        }

        // Engine.pas:8849
        public void UnregisterEClasses(ReadOnlySpan<EClass> AClasses)
        {
            int I = default;
            {
                I = 0;
                int end__142 = AClasses.Length - 1;
                for (; I <= end__142; I += 1)
                {
                    FEClasses.Remove(AClasses[I]);
                    var call__143 = AClasses[I].ClassID;
                    FClassTable.Delete(call__143);
                }
            }
        }

        // Engine.pas:8867
        public TEObjectClassList GetDescendantClasses(Type AClass)
        {
            TEObjectClassList Result = default;
            int I = default;
            TEObjectClassList L = default;
            L = new TEObjectClassList();
            {
                I = 0;
                int end__144 = FEClasses.Count - 1;
                for (; I <= end__144; I += 1)
                {
                    var call__145 = FEClasses[I];
                    var call__146 = ((EClass)call__145).InheritsFrom(AClass);
                    if (call__146)
                    {
                        var call__147 = FEClasses[I];
                        L.Add(call__147);
                    }
                }
            }

            Result = L;
            return Result;
        }

        // Engine.pas:8878
        public void ActivateEObject(TEObject Obj)
        {
            TEMsg Msg = default;
            Engine.InitMsg(ref Msg, 536870920, this);
            Msg.Engine = this;
            Obj.MsgProc(ref Msg, this);
        }

        // Engine.pas:8895
        public void WriteEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
        {
            TEStorageStream S = default;
            S = new TEWriteStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S.Free();
            }
        }

        // Engine.pas:8908
        public void ReadEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
        {
            TEStorageStream S = default;
            S = new TEReadStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S.Free();
            }
        }

        // Engine.pas:8921
        public void WritePreviewEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
        {
            TEStorageStream S = default;
            S = new TEWriteStorageStream(Stream, this, true);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S.Free();
            }
        }

        // Engine.pas:8934
        public void ReadPreviewEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
        {
            TEStorageStream S = default;
            S = new TEReadStorageStream(Stream, this, true);
            S.OnProgress = AProgress;
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S.Free();
            }
        }

        // Engine.pas:8947
        public void StoreEObject(TStream Stream, TEObject Obj, TProgressEvent AProgress)
        {
            TEStorageStream S = default;
            S = new TEWriteStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwCreateEObject(ref Obj, null, 0);
            }
            finally
            {
                S.Free();
            }
        }

        // Engine.pas:8960
        public TEObject RetrieveEObject(TStream Stream, TProgressEvent AProgress)
        {
            TEObject Result = default;
            TEStorageStream S = default;
            S = new TEReadStorageStream(Stream, this, false);
            S.OnProgress = AProgress;
            try
            {
                S.rwCreateEObject(ref Result, null, 0);
            }
            finally
            {
                S.Free();
            }

            return Result;
        }

        // Engine.pas:8973
        public TEObject RetrieveFromFile(AnsiString FileName, TProgressEvent AProgress)
        {
            TEObject Result = default;
            TFileStream F = default;
            F = new TFileStream(FileName, 0 | 32);
            try
            {
                Result = RetrieveEObject(F, AProgress);
            }
            finally
            {
                F.Free();
            }

            return Result;
        }

        // Engine.pas:8985
        public void StoreInFile(AnsiString FileName, TEObject Obj, TProgressEvent AProgress)
        {
            TFileStream F = default;
            F = new TFileStream(FileName, 65535);
            try
            {
                StoreEObject(F, Obj, AProgress);
            }
            finally
            {
                F.Free();
            }
        }

        // Engine.pas:8997
        public void WriteToFile(AnsiString FileName, TEObject Obj, TProgressEvent AProgress)
        {
            TMemoryStream M = default;
            TFileStream F = default;
            M = new TMemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                F = new TFileStream(FileName, 65535);
                try
                {
                    M.Position = 0;
                    var call__148 = M.Size;
                    F.CopyFrom(M, call__148);
                }
                finally
                {
                    F.Free();
                }
            }
            finally
            {
                M.Free();
            }
        }

        // Engine.pas:9017
        public void ReadFromFile(AnsiString FileName, TEObject Obj, TProgressEvent AProgress)
        {
            TMemoryStream M = default;
            TFileStream F = default;
            M = new TMemoryStream();
            F = new TFileStream(FileName, 0 | 32);
            try
            {
                M.LoadFromStream(F);
            }
            finally
            {
                F.Free();
            }

            M.Position = 0;
            try
            {
                ReadEObject(M, Obj, AProgress);
            }
            finally
            {
                M.Free();
            }
        }

        // Engine.pas:9037
        public void WriteToFileCRC(AnsiString FileName, TEObject Obj, TProgressEvent AProgress)
        {
            int CRC1 = default;
            int CRC2 = default;
            TMemoryStream M = default;
            TFileStream F = default;
            M = new TMemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                var call__149 = M.Size;
                var call__150 = Engine.GetCRC32(M.Memory, call__149);
                CRC1 = call__150;
                M.Write(Bytes.Of(ref CRC1, 4));
                var call__151 = M.Size;
                var call__152 = Engine.GetCRC32(M.Memory, call__151);
                CRC2 = call__152;
                F = new TFileStream(FileName, 65535);
                try
                {
                    F.Write(Bytes.Of(ref CRC2, 4));
                    var call__153 = M.Size;
                    F.Write(M.Memory.AsSpan(0, call__153));
                }
                finally
                {
                    F.Free();
                }
            }
            finally
            {
                M.Free();
            }
        }

        // Engine.pas:9061
        public bool ReadFromFileCRC(AnsiString FileName, TEObject Obj, TProgressEvent AProgress)
        {
            bool Result = default;
            TMemoryStream M = default;
            TFileStream F = default;
            int CRC2 = default;
            int CRC1 = default;
            Result = false;
            M = new TMemoryStream();
            F = new TFileStream(FileName, 0 | 32);
            try
            {
                M.LoadFromStream(F);
                M.Position = 0;
                var call__154 = M.Size;
                if (call__154 > 8)
                {
                    CRC1 = MemoryMarshal.Read<int>(M.Memory.AsSpan(0, 4));
                    var call__155 = M.Size;
                    CRC2 = MemoryMarshal.Read<int>(M.Memory.AsSpan(call__155 - 4, 4));
                    var call__156 = M.Size;
                    var call__157 = Engine.GetCRC32(M.Memory.AsSpan(4), call__156 - 4);
                    bool bool__158 = CRC1 == call__157;
                    if (bool__158)
                    {
                        var call__159 = M.Size;
                        var call__160 = Engine.GetCRC32(M.Memory.AsSpan(4), call__159 - 8);
                        bool__158 = CRC2 == call__160;
                    }

                    if (bool__158)
                    {
                        M.Position = 4;
                        ReadEObject(M, Obj, AProgress);
                        Result = true;
                    }
                }
            }
            finally
            {
                F.Free();
                M.Free();
            }

            return Result;
        }

        // Engine.pas:9091
        public void ReadTextFromFile(AnsiString FileName, TEObject Obj)
        {
            TEStorageStream S = default;
            TFileStream F = default;
            F = new TFileStream(FileName, 0 | 32);
            try
            {
                S = new TEReadTextStorageStream(F, this, 0);
                try
                {
                    S.rwEObject(Obj, 0);
                }
                finally
                {
                    S.Free();
                }
            }
            finally
            {
                F.Free();
            }
        }

        // Engine.pas:9109
        public void WriteTextToFile(AnsiString FileName, TEObject Obj)
        {
            TEStorageStream S = default;
            TFileStream F = default;
            F = new TFileStream(FileName, 65535);
            try
            {
                S = new TEWriteTextStorageStream(F, this);
                try
                {
                    S.rwEObject(Obj, 0);
                }
                finally
                {
                    S.Free();
                }
            }
            finally
            {
                F.Free();
            }
        }

        public TEObjectClassList EClasses { get => FEClasses; }
        public TIntegerHashTable256 ClassTable { get => FClassTable; }
        public TEObjectEditorList Editors { get => FEditors; }
        public TEControlStyleNode ControlNode { get => FControlNode; }
        public TStringList DebugIDs { get => FDebugIDs; }
        public TEventList DebugEvents { get => FDebugEvents; }
        public AnsiString StartupDirectory { get => FStartupDirectory; set => SetStartupDirectory(value); }
        public TSet<TEngineModeElement> Options { get => FOptions; set => FOptions = value; }

        // Engine.pas:1126
    }

    // the base of every storage stream. The rw* methods do nothing here. the subclasses read,
    // write, compare or translate
    public partial class TEStorageStream : TObject
    {

        internal TEngine FEngine;
        internal TStream FStream;
        internal bool FErrorHandled;
        internal TPropertyTable FPropertyTable;
        internal TStorageStreamError FOnError;
        internal TProgressEvent FOnProgress;
        // Engine.pas:1184
        public virtual TSet<TStorageModeElement> StorageMode()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:9502
        public virtual TStream OpenPropertyStream(int ID)
        {
            TStream Result = default;
            Result = null;
            return Result;
        }

        // Engine.pas:1186
        public virtual void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:1187
        public virtual void rwEObject(TEObject Obj, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:1188
        public virtual void rwEObjectStream(TEObject Obj, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:1189
        public virtual void rwChildren(TECustomNode Node, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:9559
        public virtual void rwStreamProc(int ID, TStreamProc LoadProc, TStreamProc SaveProc)
        {
        }

        // Engine.pas:9507
        public virtual void rwData(Span<byte> Buffer, int Size, int ID)
        {
        }

        // Engine.pas:9511
        public virtual void rwString(ref AnsiString S, int ID)
        {
        }

        // Engine.pas:9515
        public virtual void rwStrings(TStrings Strings, int ID)
        {
        }

        // Engine.pas:9519
        public virtual void rwStringsEx(TStrings Strings, int ID)
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
        public TEStorageStream(TStream AStream, TEngine AEngine)
        {
            FEngine = AEngine;
            FStream = AStream;
            FErrorHandled = false;
        }

        // Engine.pas:9497
        public void ShowProgress(TEObject Sender, AnsiString Msg, int Percent)
        {
            if (FOnProgress != null)
            {
                FOnProgress(Sender, Msg, Percent);
            }
        }

        public TEngine Engine { get => FEngine; }
        public TStream Stream { get => FStream; }
        public TPropertyTable PropertyTable { get => FPropertyTable; }
        public TStorageStreamError OnError { get => FOnError; set => FOnError = value; }
        public TProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

    }

    // { Engine.TEObjectEditorList: the editor descriptors an engine registers, one per editor class id
    //   (an EditorClass per editor class since 9.4-4, a list of editor metaclasses before) }
    public partial class TEObjectEditorList : TObject
    {
        private readonly List<EditorClass> FList = new();

        public int Count { get => FList.Count; }
        public EditorClass this[int Index] { get => FList[Index]; }

        // Engine.pas:8356
        public void RegisterClass(EditorClass AClass)
        {
            var call__161 = AClass.EditorClassID;
            var call__162 = FindEditor(call__161);
            if (call__162 == null)
            {
                FList.Add(AClass);
            }
            else
            {
                throw Exception.Create((AnsiString)"EObjectEditor Double Defined!");
            }
        }

        // Engine.pas:8364
        public EditorClass FindEditor(int AClassID)
        {
            EditorClass Result = default;
            int I = default;
            I = 0;
            while (true)
            {
                bool bool__163 = I != Count;
                if (bool__163)
                {
                    var call__164 = FList[I].EditorClassID;
                    bool__163 = AClassID != call__164;
                }
                if (!bool__163)
                    break;
                {
                    I += 1;
                }
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
    }

    // FResourceList, FResourceID, FEditID (the edit id of the resource grid that placed it),
    // FEditIndex (its cell in that grid). all three integers are persisted (ids 1.3)
    public partial class TEResource : TEObject
    {
        public new const int ClassID = 1024;

        public TEResource(TECustomNode owner) : base(owner)
        { }

        internal TECustomNode FResourceList;
        internal int FResourceID;
        internal int FEditID;
        internal int FEditIndex;
        // Engine.pas:8513
        public override void SetOwner(TECustomNode AOwner)
        {
            base.SetOwner(AOwner);
            FResourceList = AOwner;
        }

        // Engine.pas:8542
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FResourceID, 1);
            Stream.rwInteger(ref FEditID, 2);
            Stream.rwInteger(ref FEditIndex, 3);
        }

        // Engine.pas:8519
        public override void SetArrayOwner(TECustomNode AOwner, int Index)
        {
            base.SetArrayOwner(AOwner, Index);
            FResourceList = AOwner;
        }

        // Engine.pas:8533
        public virtual ImageLib.TLibraryImage GetCellImage()
        {
            ImageLib.TLibraryImage Result = default;
            Result = null;
            return Result;
        }

        // Engine.pas:8538
        public virtual void SetCellImage(ImageLib.TLibraryImage Value)
        {
        }

        // Engine.pas:8550
        public virtual int ResourceUserClassID()
        {
            int Result = default;
            Result = 0;
            return Result;
        }

        // Engine.pas:8555
        public virtual TEObject CreateResourceUser()
        {
            TEObject Result = default;
            int UserClassID = default;
            UserClassID = ResourceUserClassID();
            if (UserClassID != 0)
            {
                var call__165 = GetEngine();
                var call__166 = call__165.CreateEObject(UserClassID, null);
                Result = call__166;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:8525
        public virtual void Draw(TCanvas Canvas, TRect Rect, ImageLib.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
        {
            TRect R = default;
            var call__167 = GetCellImage();
            BltLibraryImage(Canvas, call__167, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref R);
        }

        public TECustomNode ResourceList { get => FResourceList; set => FResourceList = value; }
        public int EditID { get => FEditID; set => FEditID = value; }
        public int EditIndex { get => FEditIndex; set => FEditIndex = value; }
        public int ResourceID { get => FResourceID; set => FResourceID = value; }
        public ImageLib.TLibraryImage CellImage { get => GetCellImage(); set => SetCellImage(value); }

        // Engine.pas:8508
        public virtual AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)ClassNameOf(GetType());
            return Result;
        }


    }

    public struct TEMsg
    {
        public int MsgID;
        public int Seq;
        public TEObject Sender;
        public FixedArray_IntOrRef_3 Param;
        public IntOrRef Payload;
        public TEngine Engine { get => Payload.As<TEngine>(); set => Payload = value; }

        public FixedArray_int_4 Data;
        public int Result;
        public TEObject Obj { get => Payload.As<TEObject>(); set => Payload = value; }
        public int Value { get => Payload.Value; set => Payload = value; }
    }

    public delegate void TStorageStreamError(TEStorageStream StorageStream, AnsiString Message, ref bool Handled);
    public delegate void TProgressEvent(TEObject Sender, AnsiString Message, int Progression);
    public partial class EDoublePrioriyID : Exception
    {
        // SysUtils.pas:174
        public EDoublePrioriyID(AnsiString Msg)
        {
    Message = Msg;
}
    }

    // the write stream's data buffer
    public partial class TEBufferStream : TFastMemoryStream
    {
        // System.pas:111
        public TEBufferStream()
        { }
    }

    public enum TCompressLevel : byte
    {
        cpNone = 0,
        cpFastest = 1,
        cpDefault = 2,
        cpMax = 3
    }

    public partial class TCompressStream : TMemoryStream
    {
        // Engine.pas:1715
        public new void LoadFromStream(TStream Stream)
        {
            bool Compressed = default;
            FixedArray_byte_4 Magic = default;
            TDecompressionStream D = default;
            byte[] Buffer = [];
            int Count = default;
            Stream.Read(((Span<byte>)Magic).Slice(0, 4));
            Stream.Read(Bytes.Of(ref Compressed, 1));
            if (!ArrayEquals<FixedArray_byte_4, byte>(Magic, Engine.CompressMagic))
            {
                Compressed = false;
                Stream.Seek(0, 0);
            }

            if (Compressed)
            {
                Buffer = default;
                Count = -1;
                Array.Resize(ref Buffer, 8096);
                D = new TDecompressionStream(Stream);
                try
                {
                    while (Count != 0)
                    {
                        var call__168 = D.Read(Buffer.AsSpan(0, 8096));
                        Count = call__168;
                        if (Count != 0)
                        {
                            this.WriteBuffer(Buffer.AsSpan(0, Count));
                        }
                    }
                }
                finally
                {
                    Array.Resize(ref Buffer, 0);
                    D.Free();
                }
            }
            else
            {
                var call__169 = Stream.Size;
                var call__170 = Stream.Position;
                CopyFrom(Stream, call__169 - call__170);
            }

            Position = 0;
        }

        // Engine.pas:1752
        public int SaveToStreamEx(TStream Stream, TCompressLevel Level)
        {
            int Result = default;
            TCompressionStream C = default;
            int StartSize = default;
            TCompressionLevel CompressionLevel = default;
            Position = 0;
            Result = 100;
            var call__171 = Stream.Size;
            StartSize = call__171;
            Stream.Write(((ReadOnlySpan<byte>)Engine.CompressMagic).Slice(0, 4));
            Stream.Write(Bytes.Of(ref Level, 1));
            if (Level != TCompressLevel.cpNone)
            {
                CompressionLevel = (TCompressionLevel)Level;
                C = new TCompressionStream(CompressionLevel, Stream);
                try
                {
                    C.CopyFrom(this, 0);
                    var call__172 = Stream.Size;
                    var call__173 = Size;
                    Result = Round((double)((double)1 - (double)(call__172 - StartSize) / call__173) * 100);
                }
                finally
                {
                    C.Free();
                }
            }
            else
            {
                Stream.CopyFrom(this, 0);
            }

            return Result;
        }

        // Engine.pas:1776
        public new void SaveToStream(TStream Stream)
        {
            SaveToStreamEx(Stream, TCompressLevel.cpDefault);
        }

        // Engine.pas:1781
        public int SaveToFileEx(AnsiString FileName, TCompressLevel Level)
        {
            int Result = default;
            TFileStream F = default;
            F = new TFileStream(FileName, 65535);
            try
            {
                Result = SaveToStreamEx(F, Level);
            }
            finally
            {
                F.Free();
            }

            return Result;
        }

        // Engine.pas:1793
        public new void SaveToFile(AnsiString FileName)
        {
            TFileStream F = default;
            F = new TFileStream(FileName, 65535);
            try
            {
                SaveToStreamEx(F, TCompressLevel.cpDefault);
            }
            finally
            {
                F.Free();
            }
        }

        // Engine.pas:1805
        public new void LoadFromFile(AnsiString FileName)
        {
            TFileStream F = default;
            F = new TFileStream(FileName, 0 | 32);
            try
            {
                LoadFromStream(F);
            }
            finally
            {
                F.Free();
            }
        }
    }

    // FRefCount, FEngines, FCounts. A plug-in registers itself in PlugIns on creation and counts
    // the engines it is initialized for
    public partial class TPlugIn : TObject
    {
        internal int FRefCount;
        internal TList FEngines;
        internal TIntegerList FCounts;
        // Engine.pas:9144
        public TPlugIn()
        {
            ;
            var call__174 = new TList();
            FEngines = call__174;
            var call__175 = new TIntegerList();
            FCounts = call__175;
            Engine.RegisterPlugIn(this);
            FRefCount = 1;
        }

        // Engine.pas:9153
        public override void Destroy()
        {
            while (FEngines.Count != 0)
            {
                var call__176 = FEngines[FEngines.Count - 1];
                Finalize((TEngine)call__176);
            }

            FEngines.Free();
            FCounts.Free();
            Engine.UnregisterPlugIn(this);
            base.Destroy();
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
                Destroy();
            }
        }

        // Engine.pas:9173
        public virtual AnsiString Name()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Abstract PlugIn";
            return Result;
        }

        // Engine.pas:9178
        public virtual bool Initialize(TEngine AEngine)
        {
            bool Result = default;
            int I = default;
            AddRef();
            var call__177 = FEngines.IndexOf(AEngine);
            I = call__177;
            if (I == -1)
            {
                FEngines.Add(AEngine);
                FCounts.Add(1);
                Result = true;
            }
            else
            {
                var call__178 = FCounts[I];
                FCounts[I] = call__178 + 1;
                Result = false;
            }

            return Result;
        }

        // Engine.pas:9197
        public virtual bool Finalize(TEngine AEngine)
        {
            bool Result = default;
            int I = default;
            var call__179 = FEngines.IndexOf(AEngine);
            I = call__179;
            if (I != -1)
            {
                var call__180 = FCounts[I];
                if (call__180 == 0)
                {
                    throw Exception.Create((AnsiString)"Invalid PlugIn Finalize Usage");
                }

                var call__181 = FCounts[I];
                if (call__181 == 1)
                {
                    FEngines.Remove(AEngine);
                    FCounts.Delete(I);
                    Result = true;
                }
                else
                {
                    var call__182 = FCounts[I];
                    FCounts[I] = call__182 - 1;
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

        // Engine.pas:550

    }

    public partial class EPlugInMissing : Exception
    {
        // SysUtils.pas:174
        public EPlugInMissing(AnsiString Msg)
        {
    Message = Msg;
}
    }

    // FEngine, FUsedPlugIns. the names of the plug-ins an engine uses, initialized and finalized as
    // the list changes
    public partial class TPlugInControl : TEObject
    {
        internal TEngine FEngine;
        internal TStringList FUsedPlugIns;
        // Engine.pas:9279
        internal void SetUsedPlugIns(TStrings Value)
        {
            FUsedPlugIns.Assign(Value);
        }

        // Engine.pas:9284
        internal TStrings GetUsedPlugIns()
        {
            TStrings Result = default;
            Result = FUsedPlugIns;
            return Result;
        }

        // Engine.pas:9289
        internal void OnChange(TObject Sender)
        {
            InitializePlugIns();
        }

        // Engine.pas:9294
        internal void OnChanging(TObject Sender)
        {
            FinalizePlugIns();
        }

        // Engine.pas:9299
        internal void InitializePlugIns()
        {
            int[] I__cell = new int[1];
            ref int I = ref I__cell[0];
            object IndexAddress = default;
            TPlugIn P = default;
            ;
            {
                I = 0;
                int end__183 = FUsedPlugIns.Count - 1;
                for (; I <= end__183; I += 1)
                {
                    var call__184 = FUsedPlugIns[I];
                    var call__185 = Units.Engine.Engine.FindPlugIn(call__184);
                    P = call__185;
                    if (P != null)
                    {
                        P.Initialize(FEngine);
                    }
                    else
                    {
                        var call__186 = FUsedPlugIns[I];
                        var call__187 = new EPlugInMissing((AnsiString)"PlugIn Missing (" + call__186 + AnsiString.FromBytes(41));
                        throw call__187;
                    }
                }
            }
        }

        // Engine.pas:9319
        internal void FinalizePlugIns()
        {
            int I = default;
            TPlugIn P = default;
            {
                I = 0;
                int end__188 = FUsedPlugIns.Count - 1;
                for (; I <= end__188; I += 1)
                {
                    var call__189 = FUsedPlugIns[I];
                    var call__190 = Units.Engine.Engine.FindPlugIn(call__189);
                    P = call__190;
                    if (P != null)
                    {
                        P.Finalize(FEngine);
                    }
                }
            }
        }

        // Engine.pas:9252
        public TPlugInControl(TECustomNode owner) : base(owner)
        {
            var call__191 = new TStringList();
            FUsedPlugIns = call__191;
            FUsedPlugIns.Sorted = true;
            FUsedPlugIns.OnChange = OnChange;
            FUsedPlugIns.OnChanging = OnChanging;
            FEngine = null;
        }

        // Engine.pas:9262
        public override void Destroy()
        {
            FUsedPlugIns.Clear();
            FUsedPlugIns.Free();
            base.Destroy();
        }

        // Engine.pas:9269
        public override TEngine GetEngine()
        {
            TEngine Result = default;
            Result = FEngine;
            return Result;
        }

        // Engine.pas:9274
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = TSet<TEControlStyleElement>.Of(Units.Engine.Engine.csStored, Units.Engine.Engine.csShared);
            return Result;
        }

        // Engine.pas:9331
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(TStorageModeElement.smTranslate))
            {
                Stream.rwStrings(FUsedPlugIns, 1);
            }
        }

        public TEngine Engine { get => FEngine; set => FEngine = value; }
        public TStrings UsedPlugIns { get => GetUsedPlugIns(); set => SetUsedPlugIns(value); }
    }

    // the child list of a TENode
    public partial class TEObjectList : TList
    {
        // Engine.pas:7136
        protected internal TEObject GetEObject(int Index)
        {
            TEObject Result = default;
            var call__192 = this[Index];
            Result = (TEObject)call__192;
            return Result;
        }

        // Engine.pas:7141
        protected internal void SetEObject(int Index, TEObject Value)
        {
            this[Index] = Value;
        }

        // Engine.pas:7146
        public TEObject FindObject(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            bool Found = default;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                bool bool__193 = List[I] != null;
                if (bool__193)
                {
                    var call__194 = ((TEObject)List[I]).EClass.ClassID;
                    bool__193 = AClassID == call__194;
                }

                Found = bool__193;
                I += 1;
            }

            if (Found)
            {
                Result = (TEObject)List[I - 1];
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
            int Result = default;
            int I = default;
            bool Found = default;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                bool bool__195 = List[I] != null;
                if (bool__195)
                {
                    var call__196 = ((TEObject)List[I]).EClass.ClassID;
                    bool__195 = AClassID == call__196;
                }

                Found = bool__195;
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
            int I = default;
            {
                I = Count - 1;
                byte end__197 = 0;
                for (; I >= end__197; I -= 1)
                {
                    // { the original tests List^[Count], one past the end, not List^[I] }
                    if (List[Count] != null)
                    {
                        ((TEObject)List[I]).Destroy();
                    }

                    Delete(I);
                }
            }
        }

        // Engine.pas:7188
        public bool AllHaveCID(int AClassID)
        {
            bool Result = default;
            int I = default;
            bool OK = default;
            I = 0;
            OK = true;
            while (I < Count)
            {
                bool bool__198 = List[I] == null;
                if (!bool__198)
                {
                    var call__199 = ((TEObject)List[I]).EClass.ClassID;
                    bool__198 = AClassID == call__199;
                }

                OK = bool__198;
                I += 1;
            }

            Result = OK && Count != 0;
            return Result;
        }

        public sealed class EObjectsAccessor(TEObjectList target)
        {
            public TEObject this[int Index] { get => target.GetEObject(Index); set => target.SetEObject(Index, value); }
        }

        public EObjectsAccessor EObjects => new EObjectsAccessor(this);
    }

    // FChildren
    public partial class TENode : TECustomNode
    {
        // Engine.pas:2861
        public new const int ClassID = 256;

        // Engine.pas:2807
        public TENode(TECustomNode owner) : this(null, owner) { }

        private protected TENode(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            var call__200 = new TEObjectList();
            FChildren = call__200;
        }

        public TEObjectList FChildren;

        // Engine.pas:2833
        public override void Clear()
        {
            int I = default;
            TEObject C = default;
            {
                I = Count - 1;
                byte end__201 = 0;
                for (; I >= end__201; I -= 1)
                {
                    C = (TEObject)FChildren.List[I];
                    if (C != null && C.Owner == this)
                    {
                        if (!C.GetControlStyle().Contains(Engine.csShared))
                        {
                            C.Destroy();
                        }
                        else
                        {
                            C.Clear();
                        }
                    }
                }
            }

            var call__202 = Count;
            if (call__202 != 0)
            {
                {
                    I = Count - 1;
                    byte end__203 = 0;
                    for (; I >= end__203; I -= 1)
                    {
                        C = (TEObject)FChildren.List[I];
                        if (C != null)
                        {
                            bool bool__204 = C.Owner == this;
                            if (bool__204)
                            {
                                var call__205 = C.GetControlStyle();
                                bool__204 = call__205.Contains(Engine.csShared);
                            }

                            if (!bool__204)
                            {
                                RemoveChild(C);
                            }
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
        public override TEObject GetChild(int Index)
        {
            TEObject Result = default;
            Result = (TEObject)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:2871
        public override void SetChild(int Index, TEObject Child)
        {
            while (true)
            {
                var call__206 = Count;
                if (!(Index >= call__206))
                    break;
                {
                    FChildren.Add(null);
                }
            }

            if (FChildren.List[Index] != null && ((TEObject)FChildren.List[Index]).Owner == this)
            {
                ((TEObject)FChildren.List[Index]).Destroy();
            }

            FChildren.List[Index] = Child;
        }

        // Engine.pas:2879
        public override int GetCount()
        {
            int Result = default;
            Result = FChildren.Count;
            return Result;
        }

        // Engine.pas:2819
        public override void DestroyAllChildren()
        {
            int I = default;
            TEObject C = default;
            Clear();
            if (FChildren.Count != 0)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__207 = 0;
                    for (; I >= end__207; I -= 1)
                    {
                        C = (TEObject)FChildren.List[I];
                        if (C != null && C.Owner == this)
                        {
                            C.Destroy();
                        }
                    }
                }
            }
        }

        // Engine.pas:2884
        public override void SendMsg(ref TEMsg Msg, TEObject Sender)
        {
            int I = default;
            var children = FChildren;
            if (GetControlStyle().Contains(Engine.csKeepIndex))
            {
                if ((Msg.MsgID & 268435456) != 0)
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        {
                            I = children.Count - 1;
                            byte end__209 = 0;
                            for (; I >= end__209; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node = (TEObject)children.List[I];
                                    var call__211 = node.GetLastMsg();
                                    if (call__211 != Msg.Seq)
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
                        {
                            I = children.Count - 1;
                            byte end__212 = 0;
                            for (; I >= end__212; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node2 = (TEObject)children[I];
                                    var call__214 = node2.GetLastMsg();
                                    if (call__214 != Msg.Seq && node2.Owner == this)
                                    {
                                        node2.SetLastMsg(Msg.Seq);
                                        node2.MsgProc(ref Msg, this);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        {
                            I = children.Count - 1;
                            byte end__215 = 0;
                            for (; I >= end__215; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node3 = (TEObject)children.List[I];
                                    node3.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                    else
                    {
                        {
                            I = children.Count - 1;
                            byte end__217 = 0;
                            for (; I >= end__217; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node4 = (TEObject)children[I];
                                    if (node4.Owner == this)
                                    {
                                        node4.MsgProc(ref Msg, this);
                                    }
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
                        {
                            I = children.Count - 1;
                            byte end__219 = 0;
                            for (; I >= end__219; I -= 1)
                            {
                                var node5 = (TEObject)children.List[I];
                                var call__221 = node5.GetLastMsg();
                                if (call__221 != Msg.Seq)
                                {
                                    node5.SetLastMsg(Msg.Seq);
                                    node5.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                    else
                    {
                        {
                            I = children.Count - 1;
                            byte end__222 = 0;
                            for (; I >= end__222; I -= 1)
                            {
                                var node6 = (TEObject)children[I];
                                var call__224 = node6.GetLastMsg();
                                if (call__224 != Msg.Seq && node6.Owner == this)
                                {
                                    node6.SetLastMsg(Msg.Seq);
                                    node6.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        {
                            I = children.Count - 1;
                            byte end__225 = 0;
                            for (; I >= end__225; I -= 1)
                            {
                                var node7 = (TEObject)children.List[I];
                                node7.MsgProc(ref Msg, this);
                            }
                        }
                    }
                    else
                    {
                        {
                            I = children.Count - 1;
                            byte end__227 = 0;
                            for (; I >= end__227; I -= 1)
                            {
                                var node8 = (TEObject)children[I];
                                if (node8.Owner == this)
                                {
                                    node8.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:2981
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            if (GetControlStyle().Contains(Engine.csKeepIndex))
            {
                var call__229 = FChildren.IndexOf(Child);
                Result = call__229;
                if (Result != -1)
                {
                    FChildren.List[Result] = null;
                }
            }
            else
            {
                var call__230 = FChildren.Remove(Child);
                Result = call__230;
            }

            return Result;
        }

        // Engine.pas:2992
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            FChildren.Add(Child);
            Result = FChildren.Count - 1;
            return Result;
        }

        // Engine.pas:2998
        public override TEObject FindChild(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            bool Found = default;
            TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (!Found && I < FChildren.Count)
            {
                C = (TEObject)FChildren.List[I];
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
        public override int FindChildIndex(TEObject Child)
        {
            int Result = default;
            var call__231 = Count;
            Result = call__231 - 1;
            while (Result >= 0 && Child != (TEObject)FChildren.List[Result])
            {
                Result -= 1;
            }

            return Result;
        }

        // Engine.pas:2813
        public override void Destroy()
        {
            base.Destroy();
            FChildren.Free();
        }

        // Engine.pas:3022
        public void DestroyEObjects(int AClassID)
        {
            int I = default;
            TEObject C = default;
            {
                I = FChildren.Count - 1;
                byte end__232 = 0;
                for (; I >= end__232; I -= 1)
                {
                    C = (TEObject)FChildren.List[I];
                    bool bool__233 = C != null;
                    if (bool__233)
                    {
                        var call__234 = C.EClass.ClassID;
                        bool__233 = AClassID == call__234;
                    }

                    if (bool__233 && C.Owner == this)
                    {
                        C.Destroy();
                    }
                }
            }
        }

        // Engine.pas:631
    }

    // FTick, FCount, FTimerLists[1.10]. Children sit in one of ten lists and get TimerEvent every
    // tick, every second tick. every tenth
    public partial class TETimerNode : TECustomNode
    {
        // Engine.pas:1836
        public new const int ClassID = 264;

        internal int FTick;
        internal int FCount;
        internal FixedArray_Aow_Rtl_TList_10 FTimerLists;
        // Engine.pas:1856
        internal int FindTimerList(TEObject Child)
        {
            int Result = default;
            int I = default;
            Result = -1;
            I = 0;
            while (Result == -1 && I < 10)
            {
                I += 1;
                // { the original indexes the lists with Result (-1 on the first round), not with I }
                var call__235 = FTimerLists[Result - 1].IndexOf(Child);
                Result = call__235;
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
        public TETimerNode(TECustomNode owner) : base(owner)
        {
            int I = default;
            FCount = 0;
            {
                I = 1;
                byte end__236 = 10;
                for (; I <= end__236; I += 1)
                {
                    var call__237 = new TList();
                    FTimerLists[I - 1] = call__237;
                }
            }
        }

        // Engine.pas:1828
        public override void Destroy()
        {
            int I = default;
            base.Destroy();
            {
                I = 1;
                byte end__238 = 10;
                for (; I <= end__238; I += 1)
                {
                    FTimerLists[I - 1].Free();
                }
            }
        }

        // Engine.pas:1841
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = TSet<TEControlStyleElement>.Of(Engine.csStored, Engine.csShared, Engine.csReserved2);
            return Result;
        }

        // Engine.pas:1846
        public override int GetCount()
        {
            int Result = default;
            Result = FCount;
            return Result;
        }

        // Engine.pas:1851
        public TList GetTimerList(sbyte Timer)
        {
            TList Result = default;
            Result = FTimerLists[Timer - 1];
            return Result;
        }

        // Engine.pas:1871
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            int L = default;
            L = FindTimerList(Child);
            if (L != -1)
            {
                var call__239 = FTimerLists[L - 1].Remove(Child);
                Result = call__239;
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
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            var call__240 = FTimerLists[1 - 1].Add(Child);
            Result = call__240;
            FCount += 1;
            return Result;
        }

        // Engine.pas:1891
        public override TEObject GetChild(int Index)
        {
            TEObject Result = default;
            int I = default;
            int Total = default;
            TList L = default;
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
            Result = (TEObject)L.List[Index - Total];
            return Result;
        }

        // Engine.pas:1910
        public override void SetChild(int Index, TEObject Child)
        {
            throw Exception.Create((AnsiString)"Set Child not supported (TETimerNode)");
        }

        // Engine.pas:1915
        public int AddTimerChild(TEObject Child, sbyte Timer)
        {
            int Result = default;
            var call__241 = FTimerLists[Timer - 1].Add(Child);
            Result = call__241;
            FCount += 1;
            return Result;
        }

        // Engine.pas:1921
        public int RemoveTimerChild(TEObject Child, sbyte Timer)
        {
            int Result = default;
            var call__242 = FTimerLists[Timer - 1].Remove(Child);
            Result = call__242;
            if (Result != -1)
            {
                FCount -= 1;
            }

            return Result;
        }

        // Engine.pas:1927
        public void TriggerTimerListEvent(int Timer)
        {
            int I = default;
            {
                I = 0;
                int end__243 = FTimerLists[Timer - 1].Count - 1;
                for (; I <= end__243; I += 1)
                {
                    ((TEObject)FTimerLists[Timer - 1].List[I]).TimerEvent(FTick, this);
                }
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
    public partial class TEChangeNotifyNode : TECustomNode
    {
        // Engine.pas:7246
        public new const int ClassID = 265;

        public TEChangeNotifyNode(TECustomNode owner) : base(owner)
        {
            var call__244 = new TEObjectList();
            FChildren = call__244;
            FUpdateCount = 0;
        }

        public TEObjectList FChildren;
        internal int FUpdateCount;

        // Engine.pas:7212
        public override void Destroy()
        {
            BeginUpdate();
            base.Destroy();
            FChildren.Free();
            FChildren = null;
        }

        // Engine.pas:7220
        public override void Clear()
        {
            int I = default;
            TEObject C = default;
            {
                I = Count - 1;
                byte end__245 = 0;
                for (; I >= end__245; I -= 1)
                {
                    C = (TEObject)FChildren.List[I];
                    if (C != null && C.Owner == this)
                    {
                        if (!C.GetControlStyle().Contains(Engine.csShared))
                        {
                            C.Destroy();
                        }
                        else
                        {
                            C.Clear();
                        }
                    }
                }
            }

            {
                I = Count - 1;
                byte end__246 = 0;
                for (; I >= end__246; I -= 1)
                {
                    C = (TEObject)FChildren.List[I];
                    if (C != null)
                    {
                        bool bool__247 = C.Owner == this;
                        if (bool__247)
                        {
                            var call__248 = C.GetControlStyle();
                            bool__247 = call__248.Contains(Engine.csShared);
                        }

                        if (!bool__247)
                        {
                            RemoveChild(C);
                        }
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
        public override TEObject GetChild(int Index)
        {
            TEObject Result = default;
            Result = (TEObject)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:7256
        public override void SetChild(int Index, TEObject Child)
        {
            while (true)
            {
                var call__249 = Count;
                if (!(Index >= call__249))
                    break;
                {
                    FChildren.Add(null);
                }
            }

            FChildren[Index] = Child;
            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // Engine.pas:7263
        public override void Assign(TEObject Source, TEngine AEngine)
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
            Assert(FUpdateCount > 0, (AnsiString)"Invalid UpdateCount Usage", "Engine.pas", 7959);
            FUpdateCount -= 1;
            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // Engine.pas:7964
        public void Sort(TListSortCompare Compare)
        {
            BeginUpdate();
            FChildren.Sort(Compare);
            EndUpdate();
        }

        // Engine.pas:7971
        public override void MainReadWrite(TEStorageStream Stream)
        {
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Engine.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
            {
                BeginUpdate();
            }

            base.MainReadWrite(Stream);
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Engine.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
            {
                EndUpdate();
            }
        }

        // Engine.pas:7978
        public override int GetCount()
        {
            int Result = default;
            Result = FChildren.Count;
            return Result;
        }

        // Engine.pas:7983
        public override void SendMsg(ref TEMsg Msg, TEObject Sender)
        {
            int I = default;
            var children = FChildren;
            if (GetControlStyle().Contains(Engine.csKeepIndex))
            {
                if ((Msg.MsgID & 268435456) != 0)
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        {
                            I = children.Count - 1;
                            byte end__251 = 0;
                            for (; I >= end__251; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node = (TEObject)children.List[I];
                                    var call__253 = node.GetLastMsg();
                                    if (call__253 != Msg.Seq)
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
                        {
                            I = children.Count - 1;
                            byte end__254 = 0;
                            for (; I >= end__254; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node2 = (TEObject)children[I];
                                    var call__256 = node2.GetLastMsg();
                                    if (call__256 != Msg.Seq && node2.Owner == this)
                                    {
                                        node2.SetLastMsg(Msg.Seq);
                                        node2.MsgProc(ref Msg, this);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        {
                            I = children.Count - 1;
                            byte end__257 = 0;
                            for (; I >= end__257; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node3 = (TEObject)children.List[I];
                                    node3.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                    else
                    {
                        {
                            I = children.Count - 1;
                            byte end__259 = 0;
                            for (; I >= end__259; I -= 1)
                            {
                                if (children.List[I] != null)
                                {
                                    var node4 = (TEObject)children[I];
                                    if (node4.Owner == this)
                                    {
                                        node4.MsgProc(ref Msg, this);
                                    }
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
                        {
                            I = children.Count - 1;
                            byte end__261 = 0;
                            for (; I >= end__261; I -= 1)
                            {
                                var node5 = (TEObject)children.List[I];
                                var call__263 = node5.GetLastMsg();
                                if (call__263 != Msg.Seq)
                                {
                                    node5.SetLastMsg(Msg.Seq);
                                    node5.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                    else
                    {
                        {
                            I = children.Count - 1;
                            byte end__264 = 0;
                            for (; I >= end__264; I -= 1)
                            {
                                var node6 = (TEObject)children[I];
                                var call__266 = node6.GetLastMsg();
                                if (call__266 != Msg.Seq && node6.Owner == this)
                                {
                                    node6.SetLastMsg(Msg.Seq);
                                    node6.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((Msg.MsgID & 536870912) == 0)
                    {
                        {
                            I = children.Count - 1;
                            byte end__267 = 0;
                            for (; I >= end__267; I -= 1)
                            {
                                var node7 = (TEObject)children.List[I];
                                node7.MsgProc(ref Msg, this);
                            }
                        }
                    }
                    else
                    {
                        {
                            I = children.Count - 1;
                            byte end__269 = 0;
                            for (; I >= end__269; I -= 1)
                            {
                                var node8 = (TEObject)children[I];
                                if (node8.Owner == this)
                                {
                                    node8.MsgProc(ref Msg, this);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:8080
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            if (GetControlStyle().Contains(Engine.csKeepIndex))
            {
                var call__271 = FChildren.IndexOf(Child);
                Result = call__271;
                if (Result != -1)
                {
                    FChildren.List[Result] = null;
                }
            }
            else
            {
                var call__272 = FChildren.Remove(Child);
                Result = call__272;
            }

            if (FUpdateCount == 0)
            {
                Changed();
            }

            return Result;
        }

        // Engine.pas:8092
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            FChildren.Add(Child);
            if (FUpdateCount == 0)
            {
                Changed();
            }

            Result = FChildren.Count - 1;
            return Result;
        }

        // Engine.pas:8099
        public override TEObject FindChild(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            bool Found = default;
            TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (!Found && I < FChildren.Count)
            {
                C = (TEObject)FChildren.List[I];
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
        public override int FindChildIndex(TEObject Child)
        {
            int Result = default;
            var call__273 = Count;
            Result = call__273 - 1;
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
    public partial class TEChangeNotifyEventList : TEChangeNotifyNode
    {

        public TEChangeNotifyEventList(TECustomNode owner) : base(owner)
        {
            var call__274 = new TEventList();
            FEvents = call__274;
        }

        internal TEventList FEvents;

        // Engine.pas:8131
        public override void Destroy()
        {
            FEvents.Free();
            base.Destroy();
        }

        // Engine.pas:8137
        public override void Changed()
        {
            FEvents.TriggerNotifyEvents(this);
        }

        public TEventList Events { get => FEvents; }

        // Engine.pas:708

    }

    public enum TEditResult : byte
    {
        erOK = 0,
        erCancel = 1
    }

    public delegate void TEditDoneEvent(TEditResult EditResult, TEObject EObject);
    // editors are classes looked up by the class id of the object (TEObject.Editor). everything is
    // called on the class. The base editor edits nothing: Edit reports a cancel at once
    public partial class TEObjectEditor : TEObject, IEObjectEditorClass
    {

        public TEObjectEditor(TECustomNode owner) : base(owner)
        { }

        // Engine.pas:3036
        // { TEObjectEditor: stubs }
        public static int EditorClassID => 0;

        // Engine.pas:3041
        public static void Edit(TEObject Obj, TEngine AEngine, TEditDoneEvent Done)
        {
            Done(TEditResult.erCancel, Obj);
        }


    }

    // FCount, FCapacity, FItems. The hash classes come from an include file of the original,
    // D:\GEngine\Engine\inc\HashTab.PAS, whose name the Assert calls carry
    public struct THashItem
    {
        public IntOrRef Key;
        public object Item;
    }

    public partial class THashList : TObject
    {
        internal int FCount;
        internal int FCapacity;
        internal THashItem[] FItems = [];
        // HashTab.PAS:18
        protected internal void SetCapacity(int NewCapacity)
        {
            Assert(NewCapacity >= FCount, (AnsiString)"Capacity Error (THashList)", "HashTab.PAS", 20);
            if (NewCapacity != FCapacity)
            {
                Array.Resize(ref FItems, NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // HashTab.PAS:28
        protected internal void Delete(int Index)
        {
            Assert(Index >= 0 && Index < FCount, (AnsiString)"Delete Error (THashList)", "HashTab.PAS", 30);
            FCount -= 1;
            if (Index < FCount)
            {
                Array.Copy(FItems, Index + 1, FItems, Index, FCount - Index);
            }
        }

        // HashTab.PAS:36
        protected internal void Add(THashItem Item)
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
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // HashTab.PAS:12
        public virtual void Clear()
        {
            FCount = 0;
            SetCapacity(0);
        }

        public int Count { get => FCount; }
    }

    // items keyed by an integer
    public partial class TIntegerHashList : THashList
    {
        // HashTab.PAS:43
        public void Add(object Item, int Key)
        {
            THashItem H = default;
            if (Find(Key) != null)
            {
                throw Exception.Create((AnsiString)"ID already in HashTable");
            }

            Assert(Item != null, (AnsiString)"Cannot add nil item to hash table", "HashTab.PAS", 48);
            H.Key = Key;
            H.Item = Item;
            base.Add(H);
        }

        // HashTab.PAS:54
        public new void Delete(int Key)
        {
            int I = default;
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
            object Result = default;
            int I = default;
            {
                I = FCount - 1;
                byte end__275 = 0;
                for (; I >= end__275; I -= 1)
                {
                    if (Key == FItems[I].Key.Value)
                    {
                        Result = FItems[I].Item;
                        return Result;
                    }
                }
            }

            Result = null;
            return Result;
        }
    }

    // the abstract table interface
    public partial class TIntegerHashTable : TObject
    {
        // Engine.pas:771
        public virtual void Add(object Item, int Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:772
        public virtual void Delete(int Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:773
        public virtual object Find(int Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:774
        public virtual void Clear()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    // 256 lists selected by the low key byte
    public partial class TIntegerHashTable256 : TIntegerHashTable
    {
        internal FixedArray_Aow_Units_Engine_TIntegerHashList_256 FLists;
        // HashTab.PAS:83
        public TIntegerHashTable256()
        {
            int I = default;
            ;
            {
                I = 0;
                byte end__276 = 255;
                for (; I <= end__276; I += 1)
                {
                    var call__277 = new TIntegerHashList();
                    FLists[I] = call__277;
                }
            }
        }

        // HashTab.PAS:91
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                byte end__278 = 255;
                for (; I <= end__278; I += 1)
                {
                    FLists[I].Free();
                }
            }

            base.Destroy();
        }

        // HashTab.PAS:99
        public override void Clear()
        {
            int I = default;
            {
                I = 0;
                byte end__279 = 255;
                for (; I <= end__279; I += 1)
                {
                    FLists[I].Clear();
                }
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
        public override object Find(int Key)
        {
            object Result = default;
            var call__280 = FLists[Key & 255].Find(Key);
            Result = call__280;
            return Result;
        }

        // HashTab.PAS:121
        public void DeleteSafe(object Item, int Key)
        {
            var call__281 = FLists[Key & 255].Find(Key);
            if (call__281 == Item)
            {
                FLists[Key & 255].Delete(Key);
            }
        }

        // HashTab.PAS:126
        public virtual int GetFreeID()
        {
            int Result = default;
            int Min = default;
            int MinIndex = default;
            int I = default;
            Min = 16777215;
            MinIndex = 0;
            {
                I = 0;
                byte end__282 = 255;
                for (; I <= end__282; I += 1)
                {
                    if (Min > FLists[I].Count)
                    {
                        Min = FLists[I].Count;
                        MinIndex = I;
                    }
                }
            }

            I = FLists[MinIndex].Count;
            while (true)
            {
                var call__283 = FLists[MinIndex].Find((I << 8) + MinIndex);
                if (!(call__283 != null))
                    break;
                {
                    I += 1;
                }
            }

            Result = (I << 8) + MinIndex;
            return Result;
        }
    }

    // THashList items whose key dword is a string
    public partial class TStringHashList : THashList
    {
        // HashTab.PAS:143
        public override void Clear()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__284 = 0;
                for (; I >= end__284; I -= 1)
                {
                    FItems[I].Key = (AnsiString)"";
                }
            }

            base.Clear();
        }

        // HashTab.PAS:151
        public object Find(AnsiString Key)
        {
            object Result = default;
            int I = default;
            Result = null;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (AnsiCompareStr(FItems[I].Key.As<AnsiString>(), Key) == 0)
                    {
                        Result = FItems[I].Item;
                        return Result;
                    }
                }
                while (!(I == 0));
            }

            return Result;
        }

        // HashTab.PAS:170
        public void Add(object Item, AnsiString Key)
        {
            THashItem H = default;
            if (Find(Key) != null)
            {
                throw Exception.Create((AnsiString)"ID already in HashTable");
            }

            Assert(Item != null, (AnsiString)"Cannot add nil item to hash table", "HashTab.PAS", 175);
            H.Key = 0;
            H.Item = Item;
            H.Key = Key;
            base.Add(H);
        }

        // HashTab.PAS:182
        public void Delete(AnsiString Key)
        {
            int I = default;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (AnsiCompareStr(FItems[I].Key.As<AnsiString>(), Key) == 0)
                    {
                        FItems[I].Key = (AnsiString)"";
                        base.Delete(I);
                        return;
                    }
                }
                while (!(I == 0));
            }
        }
    }

    // the abstract table interface
    public partial class TStringHashTable : TObject
    {
        // Engine.pas:804
        public virtual void Add(object Item, AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:805
        public virtual void Delete(AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:806
        public virtual object Find(AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:807
        public virtual void Clear()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    // 256 lists selected by a hash of the first and the second-to-last character and the length
    public partial class TStringHashTable256 : TStringHashTable
    {
        internal FixedArray_Aow_Units_Engine_TStringHashList_256 FLists;
        // HashTab.PAS:199
        public TStringHashTable256()
        {
            int I = default;
            ;
            {
                I = 0;
                byte end__285 = 255;
                for (; I <= end__285; I += 1)
                {
                    var call__286 = new TStringHashList();
                    FLists[I] = call__286;
                }
            }
        }

        // HashTab.PAS:207
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                byte end__287 = 255;
                for (; I <= end__287; I += 1)
                {
                    FLists[I].Free();
                }
            }

            base.Destroy();
        }

        // HashTab.PAS:215
        public override void Clear()
        {
            int I = default;
            {
                I = 0;
                byte end__288 = 255;
                for (; I <= end__288; I += 1)
                {
                    FLists[I].Clear();
                }
            }
        }

        // HashTab.PAS:222
        public override void Add(object Item, AnsiString Key)
        {
            int H = default;
            H = Key.Length;
            if (H >= 2)
            {
                H = (Key[1] + Key[H - 1] + (H << 4)) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = Key[1];
                }
            }

            FLists[H].Add(Item, Key);
        }

        // HashTab.PAS:234
        public override void Delete(AnsiString Key)
        {
            int H = default;
            H = Key.Length;
            if (H >= 2)
            {
                H = (Key[1] + Key[H - 1] + (H << 4)) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = Key[1];
                }
            }

            FLists[H].Delete(Key);
        }

        // HashTab.PAS:246
        public override object Find(AnsiString Key)
        {
            object Result = default;
            int H = default;
            H = Key.Length;
            if (H >= 2)
            {
                H = (Key[1] + Key[H - 1] + (H << 4)) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = Key[1];
                }
            }

            var call__289 = FLists[H].Find(Key);
            Result = call__289;
            return Result;
        }

        // Engine.pas:816

    }

    // FControlStyle
    public partial class TEControlStyleObject : TEObject
    {
        // Engine.pas:8228
        public new const int ClassID = 81;

        internal TSet<TEControlStyleElement> FControlStyle;
        // Engine.pas:8222
        public TEControlStyleObject(TECustomNode owner) : this(null, owner) { }

        // Engine.pas:8222
        private protected TEControlStyleObject(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:8233
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:8238
        public void SetControlStyle(TSet<TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8243
        public override void ReadWrite(TEStorageStream Stream)
        {
            RwSet(ref FControlStyle, 1, 1, Stream.rwData, 0);
        }

    }

    // FControlStyle
    public partial class TEControlStyleNode : TENode
    {
        // Engine.pas:8199
        public new const int ClassID = 257;

        public TEControlStyleNode(TECustomNode owner) : base(owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        internal TSet<TEControlStyleElement> FControlStyle;

        // Engine.pas:8204
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:8209
        public void SetControlStyle(TSet<TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8214
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
        }

    }

    // FEngine, FControlStyle. A node that knows its engine: message $20000008
    // (TEngine.ActivateEObject) carries the engine in the message
    public partial class TECustomOwner : TENode
    {
        // Engine.pas:8257
        public new const int ClassID = 259;

        // Engine.pas:8250
        public TECustomOwner(TECustomNode owner) : this(null, owner) { }

        private protected TECustomOwner(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            FOwner = owner;
            FControlStyle = base.GetControlStyle();
        }

        internal TEngine FEngine;
        internal TSet<TEControlStyleElement> FControlStyle;
        // Engine.pas:8262
        public override TEngine GetEngine()
        {
            TEngine Result = default;
            Result = FEngine;
            return Result;
        }

        // Engine.pas:8267
        public void SetEngine(TEngine AEngine)
        {
            FEngine = AEngine;
        }

        // Engine.pas:8272
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:8277
        public void SetControlStyle(TSet<TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8282
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
        }

        // Engine.pas:8288
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = Msg.Engine;
            }

            base.MsgProc(ref Msg, Sender);
        }

        // Engine.pas:855
    }

    // a TList of class references
    public partial class TEObjectClassList : TList
    {
        // Engine.pas:8296
        protected internal EClass GetEClass(int Index)
        {
            EClass Result = default;
            var call__290 = this[Index];
            Result = (EClass)call__290;
            return Result;
        }

        // Engine.pas:8301
        public EClass FindClass(int AClassID)
        {
            EClass Result = default;
            int I = default;
            bool Found = default;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                var call__291 = this[I];
                var call__292 = ((EClass)call__291).ClassID;
                Found = AClassID == call__292;
                I += 1;
            }

            if (Found)
            {
                var call__293 = this[I - 1];
                Result = (EClass)call__293;
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
            int Result = default;
            int I = default;
            bool Found = default;
            I = 0;
            Found = false;
            while (!Found && I < Count)
            {
                var call__294 = this[I];
                var call__295 = ((EClass)call__294).ClassID;
                Found = AClassID == call__295;
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
            EClass Result = default;
            Result = FindClass(AClassID);
            if (Result == null)
            {
                throw Exception.Create((AnsiString)"Unregistered E-ClassID found (" + IntToStr(AClassID) + AnsiString.FromBytes(41));
            }

            return Result;
        }

        // Engine.pas:8338
        public void RegisterClass(EClass AClass)
        {
            var call__296 = AClass.ClassID;
            var call__297 = FindClass(call__296);
            bool bool__298 = call__297 == null;
            if (bool__298)
            {
                var call__299 = AClass.ClassID;
                bool__298 = call__299 != 0;
            }

            if (bool__298)
            {
                Add(AClass);
            }
            else
            {
                throw Exception.Create((AnsiString)"EObject Class ID double defined!");
            }
        }

        // Engine.pas:8346
        public void UnregisterClass(EClass AClass)
        {
            int I = default;
            var call__300 = AClass.ClassID;
            var call__301 = FindClassIndex(call__300);
            I = call__301;
            if (I != -1)
            {
                Delete(I);
            }
        }

        public sealed class EClassesAccessor(TEObjectClassList target)
        {
            public EClass this[int Index] { get => target.GetEClass(Index); }
        }

        public EClassesAccessor EClasses => new EClassesAccessor(this);
    }

    // FEditors, editor names with ids
    public partial class TEResourceEditSettings : TEControlStyleObject
    {
        // Engine.pas:8662
        public new const int ClassID = 1026;

        internal TStringList FEditors;
        // Engine.pas:8677
        internal int GetUniqueEditID()
        {
            int Result = default;
            int I = default;
            bool Found = default;
            Result = 1;
            I = 0;
            var call__302 = FEditors.Count;
            Found = call__302 == 0;
            while (!Found)
            {
                var call__303 = FEditors.Objects[I];
                if (Result != ObjectToInt(call__303))
                {
                    I += 1;
                    var call__304 = FEditors.Count;
                    Found = I == call__304;
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
        public TEResourceEditSettings(TECustomNode owner) : base(owner)
        {
            var call__305 = new TStringList();
            FEditors = call__305;
        }

        // Engine.pas:8656
        public override void Destroy()
        {
            FEditors.Free();
            base.Destroy();
        }

        // Engine.pas:8667
        public AnsiString GetUniqueEditorName(AnsiString Name)
        {
            AnsiString Result = default;
            AnsiString S = default;
            S = Name;
            while (true)
            {
                var call__306 = FEditors.IndexOf(S);
                if (!(call__306 != -1))
                    break;
                {
                    S = Name + IntToStr(Random(268435455));
                }
            }

            Result = S;
            return Result;
        }

        // Engine.pas:8700
        public int GetEditID(AnsiString Name)
        {
            int Result = default;
            int I = default;
            var call__307 = FEditors.IndexOf(Name);
            I = call__307;
            if (I != -1)
            {
                var call__308 = FEditors.Objects[I];
                Result = ObjectToInt(call__308);
            }
            else
            {
                Result = GetUniqueEditID();
                FEditors.AddObject(Name, IntToObject(Result));
            }

            return Result;
        }

        // Engine.pas:8714
        public int RemoveEditor(AnsiString Name)
        {
            int Result = default;
            var call__309 = FEditors.IndexOf(Name);
            Result = call__309;
            if (Result != -1)
            {
                FEditors.Delete(Result);
            }

            return Result;
        }

        // Engine.pas:8720
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Engine.smText))
            {
                Stream.rwStringsEx(FEditors, 3);
            }
        }

        public TStringList Editors { get => FEditors; }

    }

    public struct TSortItem
    {
        public int SortValue;
        public TEObject Child;
    }

    public partial class TESortNode : TECustomNode
    {
        // Engine.pas:1478
        public new const int ClassID = 260;

        public TESortNode(TECustomNode owner) : base(owner)
        {
            FItems = [];
        }

        protected internal bool FSorted;
        protected internal int FCount;
        protected internal int FCapacity;
        protected internal TSortItem[] FItems = [];
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
        internal int GetSortValue(int Index)
        {
            int Result = default;
            Result = FItems[Index].SortValue;
            return Result;
        }

        // Engine.pas:1503
        internal void SetSortValue(int Index, int Value)
        {
            FItems[Index].SortValue = Value;
        }

        // Engine.pas:1472
        public override void Destroy()
        {
            base.Destroy();
            Resize(0);
        }

        // Engine.pas:1483
        public override void QuickClear()
        {
            Resize(0);
        }

        // Engine.pas:1488
        public override TEObject GetChild(int Index)
        {
            TEObject Result = default;
            Result = FItems[Index].Child;
            return Result;
        }

        // Engine.pas:1493
        public override void SetChild(int Index, TEObject Child)
        {
            FItems[Index].Child = Child;
        }

        // Engine.pas:1508
        public override int GetCount()
        {
            int Result = default;
            Result = FCount;
            return Result;
        }

        // Engine.pas:1520
        public virtual void Grow()
        {
            Resize(FCapacity + 2);
        }

        // Engine.pas:1525
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            Result = FCount - 1;
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
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            Result = AddSortChild(Child, 0);
            return Result;
        }

        // Engine.pas:1542
        public virtual int AddSortChild(TEObject Child, int SortValue)
        {
            int Result = default;
            int I = default;
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
        public override void ReadWrite(TEStorageStream Stream)
        {
            throw Exception.Create((AnsiString)"Read/Write not supported (TESortNode)");
        }

        // Engine.pas:1596
        public virtual void Sort()
        {
            if (!FSorted && FCount > 0)
            {
                Engine.SortItems(FItems, 0, FCount - 1);
            }

            FSorted = true;
        }

        public sealed class SortValueAccessor(TESortNode target)
        {
            public int this[int Index] { get => target.GetSortValue(Index); set => target.SetSortValue(Index, value); }
        }

        public SortValueAccessor SortValue => new SortValueAccessor(this);
        public bool Sorted { get => FSorted; }

        // Engine.pas:922


    }

    // { Engine.TEInverseSortNode: the same, sorted the other way }
    public partial class TEInverseSortNode : TESortNode
    {
        public new const int ClassID = 262;

        public TEInverseSortNode(TECustomNode owner) : base(owner)
        { }

        // Engine.pas:1635
        public override int AddSortChild(TEObject Child, int SortValue)
        {
            int Result = default;
            int I = default;
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
                Engine.InverseSortItems(FItems, 0, FCount - 1);
            }

            FSorted = true;
        }

    }

    // FControlStyle
    public partial class TEControlStyleSortNode : TESortNode
    {
        // Engine.pas:1672
        public new const int ClassID = 261;

        internal TSet<TEControlStyleElement> FControlStyle;
        // Engine.pas:1666
        public TEControlStyleSortNode(TECustomNode owner) : base(owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:1677
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:1682
        public void SetControlStyle(TSet<TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

    }

    // FControlStyle
    public partial class TEControlStyleInverseSortNode : TEInverseSortNode
    {
        // Engine.pas:1695
        public new const int ClassID = 263;

        public TEControlStyleInverseSortNode(TECustomNode owner) : base(owner)
        {
            FControlStyle = base.GetControlStyle();
        }

        internal TSet<TEControlStyleElement> FControlStyle;

        // Engine.pas:1700
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:1705
        public void SetControlStyle(TSet<TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:963


    }

    // a TENode that holds references to objects owned elsewhere (AddRef on AddChild, Release on
    // removal)
    public partial class TEReferenceNode : TENode
    {

        public TEReferenceNode(TECustomNode owner) : base(owner)
        { }

        // Engine.pas:8144
        public override void Clear()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__310 = 0;
                for (; I >= end__310; I -= 1)
                {
                    ((TEObject)FChildren.List[I]).Release();
                }
            }

            FChildren.Clear();
        }

        // Engine.pas:8157
        public override void ReadWriteChildren(TEStorageStream Stream)
        {
            int I = default;
            if (Stream.StorageMode().Contains(TStorageModeElement.smWrite))
            {
                {
                    I = 0;
                    int end__311 = FChildren.Count - 1;
                    for (; I <= end__311; I += 1)
                    {
                        if (((TEObject)FChildren.List[I]).FOwner == null)
                        {
                            ((TEObject)FChildren.List[I]).FOwner = this;
                        }
                    }
                }

                base.ReadWriteChildren(Stream);
                {
                    I = 0;
                    int end__312 = FChildren.Count - 1;
                    for (; I <= end__312; I += 1)
                    {
                        if (((TEObject)FChildren.List[I]).FOwner == this)
                        {
                            ((TEObject)FChildren.List[I]).FOwner = null;
                        }
                    }
                }
            }
            else
            {
                base.ReadWriteChildren(Stream);
                if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Engine.smText, TStorageModeElement.smCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
                {
                    {
                        I = 0;
                        int end__313 = FChildren.Count - 1;
                        for (; I <= end__313; I += 1)
                        {
                            ((TEObject)FChildren.List[I]).Release();
                        }
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
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            var call__314 = FChildren.Remove(Child);
            Result = call__314;
            if (Result != -1)
            {
                Child.Release();
            }

            return Result;
        }

        // Engine.pas:8183
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            if (Child.FOwner == this)
            {
                Child.FOwner = null;
            }

            FChildren.Add(Child);
            if (Child.AddRef() == -1)
            {
                throw Exception.Create((AnsiString)"EObject doesn't support ReferenceCounting");
            }

            Result = FChildren.Count - 1;
            return Result;
        }

        // Engine.pas:631

    }

    // FNode, the node the user is connected to
    public partial class TEUser : TEObject
    {
        // Engine.pas:2430
        public TEUser(TECustomNode owner) : base(owner)
        { }

        internal TECustomNode FNode;
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
        public override void Destroy()
        {
            Disconnect();
            base.Destroy();
        }

        // Engine.pas:7118
        public void Connect(TECustomNode Node)
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

        public TECustomNode Node { get => FNode; }
    }

    // resources with unique ids
    public partial class TEResourceList : TECustomOwner
    {
        public new const int ClassID = 1025;

        public TEResourceList(TECustomNode owner) : base(owner)
        { }

        // Engine.pas:8573
        protected internal TEResource GetResource(int Index)
        {
            TEResource Result = default;
            Result = (TEResource)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:8578
        public virtual void ResourceChanged()
        {
        }

        // Engine.pas:8582
        public TEResource FindResource(int ID)
        {
            TEResource Result = default;
            int I = default;
            bool Found = default;
            Found = false;
            I = 0;
            while (!Found && I != this.FChildren.Count)
            {
                Found = this.FChildren.List[I] != null && ID == ((TEResource)this.FChildren.List[I]).FResourceID;
                I += 1;
            }

            if (Found)
            {
                Result = (TEResource)this.FChildren.List[I - 1];
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
            int ID = default;
            int NewID = default;
            int Tries = default;
            bool Found = default;
            if (Index < FChildren.Count)
            {
                var call__316 = GetResource(Index);
                ID = call__316.FResourceID;
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

                    var call__318 = GetResource(Index);
                    call__318.FResourceID = NewID;
                }
                else
                {
                    var call__319 = GetResource(Index);
                    call__319.FResourceID = ID;
                }
            }
        }

        public sealed class ResourcesAccessor(TEResourceList target)
        {
            public TEResource this[int Index] { get => target.GetResource(Index); }
        }

        public ResourcesAccessor Resources => new ResourcesAccessor(this);

    }

    // FOnRegisterClasses (called from Loaded), FOnUnRegisterClasses (called from Destroy), FEngine,
    // the engine the component owns and frees. Descendants such as HSEngine's THexagonSpriteEngine
    // publish the two events
    public partial class TEComponent : TComponent
    {
        internal TNotifyEvent FOnRegisterClasses;
        internal TNotifyEvent FOnUnRegisterClasses;
        internal TEngine FEngine;
        // Engine.pas:8642
        public override void Loaded()
        {
            base.Loaded();
            if (FOnRegisterClasses != null)
            {
                FOnRegisterClasses(this);
            }
        }

        protected internal TNotifyEvent OnRegisterClasses { get => FOnRegisterClasses; set => FOnRegisterClasses = value; }
        protected internal TNotifyEvent OnUnRegisterClasses { get => FOnUnRegisterClasses; set => FOnUnRegisterClasses = value; }

        // Engine.pas:8635
        public override void Destroy()
        {
            if (FOnUnRegisterClasses != null)
            {
                FOnUnRegisterClasses(this);
            }

            FEngine.Free();
            base.Destroy();
        }

        public TEngine Engine { get => FEngine; set => FEngine = value; }
    }

    public struct TCallBackEvent
    {
        public TNotifyEvent Event;
        public int Data;
    }

    public partial class TCallBackEventList : TObject
    {
        internal List<TCallBackEvent> FList = [];
        // Engine.pas:5864
        internal int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

        // Engine.pas:5869
        internal ref TCallBackEvent GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

        // Engine.pas:5846
        public TCallBackEventList()
        {
            var call__320 = new List<TCallBackEvent>();
            FList = call__320;
        }

        // Engine.pas:5852
        public override void Destroy()
        {
            Clear();
            base.Destroy();
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
        public void Add(in TCallBackEvent Event)
        {
            FList.Add(Event);
        }

        // Engine.pas:5883
        public void Delete(int Index)
        {
            if (Index < FList.Count)
            {
                ;
                FList.RemoveAt(Index);
            }
        }

        public int Count { get => GetCount(); }

        public ref TCallBackEvent this[int Index] => ref GetItem(Index);
    }

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct TEngineDebugInfo
    {
        public int ID;
        public AnsiString Text;
    }

    // FPath, FSize, FSections, FText, FValues. The text form of the storage: "[:path:]" lines open
    // the section of an object (its path is the ids of its parents joined with '\'), "[id]=value"
    // lines are its properties. the section is split into FValues with the property table pointing
    // at them
    public partial class TEReadTextStorageStream : TEStorageStream
    {
        internal AnsiString FPath;
        internal int FSize;
        internal TStringList FSections;
        internal TStringList FText;
        internal TStringList FValues;
        // Engine.pas:10632
        internal void ReadStreamText()
        {
            AnsiString Line = default;
            TStringList Text = default;
            int P = 0;
            int Start = 0;
            int L = default;
            Text = FText;
            FSections.Clear();
            Text.BeginUpdate();
            try
            {
                if (FSize < 0) throw new ERangeError("Negative string length");
                byte[] buffer = new byte[FSize];
                FStream.Read(buffer);
                Text.Clear();
                ReadOnlySpan<byte> text = buffer;
                if (!text.IsEmpty)
                {
                    while ((P < text.Length ? text[P] : (byte)0) != 0)
                    {
                        Start = P;
                        while (!TSet<byte>.WithBounds(0, 255).With(0).With(10).With(13).Contains(P < text.Length ? text[P] : (byte)0))
                        {
                            P += 1;
                        }

                        Line = AnsiString.FromBytes(text[Start..P].ToArray());
                        if (Line != (AnsiString)"")
                        {
                            L = Line.Length;
                            if (L >= 4 && Copy(Line, 1, 2) == (AnsiString)"[:" && Copy(Line, L - 1, 2) == (AnsiString)":]")
                            {
                                var call__321 = Text.Count;
                                FSections.AddObject(Copy(Line, 3, L - 4), IntToObject(call__321));
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
            int Index = default;
            int First = default;
            int Last = default;
            int I = default;
            int ID = default;
            int NewID = default;
            AnsiString Value = default;
            FValues.Clear();
            var call__322 = FSections.IndexOf(FPath);
            Index = call__322;
            if (Index != -1)
            {
                var call__323 = FSections.Objects[Index];
                First = ObjectToInt(call__323);
                var call__324 = FSections.Count;
                if (Index != call__324 - 1)
                {
                    var call__325 = FSections.Objects[Index + 1];
                    Last = ObjectToInt(call__325) - 1;
                }
                else
                {
                    var call__326 = FText.Count;
                    Last = call__326 - 1;
                }

                I = First;
                ID = -1;
                Value = (AnsiString)"";
                while (I <= Last)
                {
                    var call__327 = FText[I];
                    var call__328 = Units.Engine.Engine.GetLineID(call__327);
                    NewID = call__328;
                    if (ID != -1)
                    {
                        if (NewID != ID && NewID != -1)
                        {
                            var call__329 = FValues.Count;
                            FPropertyTable.Add(ID, call__329);
                            FValues.Add(Value);
                            ID = NewID;
                            var call__330 = FText[I];
                            var call__331 = Units.Engine.Engine.GetLineValue(call__330);
                            Value = call__331;
                        }
                        else
                        {
                            var call__332 = FText[I];
                            Value = Value + AnsiString.FromBytes(13) + AnsiString.FromBytes(10) + call__332;
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

                            var call__333 = FText[I];
                            var call__334 = Units.Engine.Engine.GetLineValue(call__333);
                            Value = call__334;
                        }
                    }

                    I += 1;
                }

                if (ID != -1)
                {
                    var call__335 = FValues.Count;
                    FPropertyTable.Add(ID, call__335);
                    FValues.Add(Value);
                }
            }
        }

        // Engine.pas:10611
        public TEReadTextStorageStream(TStream AStream, TEngine AEngine, int ASize) : base(AStream, AEngine)
        {
            FPath = (AnsiString)"";
            if (ASize <= 0)
            {
                var call__336 = FStream.Size;
                var call__337 = FStream.Position;
                FSize = call__336 - call__337;
            }
            else
            {
                FSize = ASize;
            }

            var call__338 = new TStringList();
            FValues = call__338;
            var call__339 = new TPropertyTable();
            FPropertyTable = call__339;
            var call__340 = new TStringList();
            FText = call__340;
            var call__341 = new TStringList();
            FSections = call__341;
            ReadStreamText();
            ReadTextBuffer();
        }

        // Engine.pas:10624
        public override void Destroy()
        {
            FValues.Free();
            FPropertyTable.Free();
            FSections.Free();
            FText.Free();
        }

        // Engine.pas:10669
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Units.Engine.Engine.smText);
            return Result;
        }

        // Engine.pas:10741
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            TEObject O = default;
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10749
        public override void rwEObject(TEObject Obj, int ID)
        {
            TStringList SavedValues = default;
            AnsiString SavedPath = default;
            bool Continue = default;
            SavedPath = FPath;
            if (FPath != (AnsiString)"")
            {
                FPath = FPath + AnsiString.FromBytes(92) + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            SavedValues = FValues;
            FPropertyTable.BackupSettings();
            var call__342 = new TStringList();
            FValues = call__342;
            try
            {
                ReadTextBuffer();
                Obj.MainReadWrite(this);
                FValues.Free();
                FPropertyTable.RestoreSettings();
                FValues = SavedValues;
                FPath = SavedPath;
            }
            catch (System.Exception)
            {
                FValues.Free();
                FPropertyTable.RestoreSettings();
                FValues = SavedValues;
                FPath = SavedPath;
                Continue = true;
                if (FOnError != null)
                {
                    var call__343 = Obj.EClass.ClassID;
                    FOnError(this, (AnsiString)"Error reading object (cid:" + IntToStr(call__343) + AnsiString.FromBytes(41), ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }

        // Engine.pas:10779
        public override void rwEObjectStream(TEObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10784
        public override void rwString(ref AnsiString S, int ID)
        {
            int Offset = default;
            var call__344 = FPropertyTable.FindOffset(ID);
            Offset = call__344;
            if (Offset != -1)
            {
                var call__345 = FValues[Offset];
                S = call__345;
            }
            else
            {
                S = (AnsiString)"";
            }
        }

        // Engine.pas:10792
        public override void rwStrings(TStrings Strings, int ID)
        {
            int Offset = default;
            var call__346 = FPropertyTable.FindOffset(ID);
            Offset = call__346;
            if (Offset != -1)
            {
                var call__347 = FValues[Offset];
                Strings.Text = NullTerminatedText(call__347);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10800
        public override void rwStringsEx(TStrings Strings, int ID)
        {
            rwStrings(Strings, ID);
        }

        // Engine.pas:10805
        public override void rwChildren(TECustomNode Node, int ID)
        {
            AnsiString SavedPath = default;
            int I = default;
            TEObject C = default;
            // { the original saves the path and never restores it }
            SavedPath = FPath;
            if (FPath != (AnsiString)"")
            {
                FPath = FPath + AnsiString.FromBytes(92) + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            {
                I = 0;
                int end__348 = Node.Count - 1;
                for (; I <= end__348; I += 1)
                {
                    var call__349 = Node.Children[I];
                    C = call__349;
                    bool bool__350 = C != null && C.Owner == Node;
                    if (bool__350)
                    {
                        var call__351 = C.GetControlStyle();
                        bool__350 = !call__351.Contains(Units.Engine.Engine.csShared);
                    }

                    bool bool__352 = bool__350;
                    if (bool__352)
                    {
                        var call__353 = C.GetControlStyle();
                        bool__352 = call__353.Contains(Units.Engine.Engine.csStored);
                    }

                    if (bool__352)
                    {
                        rwEObject(C, I);
                    }
                }
            }
        }

    }

    // FPath, FBuffer. Each object's properties are collected in FBuffer and written behind the
    // "[:path:]" line
    public partial class TEWriteTextStorageStream : TEStorageStream
    {
        internal AnsiString FPath;
        protected internal TEBufferStream FBuffer;
        // Engine.pas:10355
        internal void WriteTextBuffer(TStream Stream)
        {
            AnsiString S = default;
            bool bool__354 = FPropertyTable.Count != 0;
            if (bool__354)
            {
                var call__355 = FBuffer.Size;
                bool__354 = call__355 != 0;
            }

            if (bool__354)
            {
                S = (AnsiString)"[:" + FPath + (AnsiString)":]" + AnsiString.FromBytes(13) + AnsiString.FromBytes(10);
                if (FPath != (AnsiString)"")
                {
                    S = AnsiString.FromBytes(13, 10) + S;
                }

                Stream.Write(StringBytes(S).Slice(1 - 1, S.Length));
                FBuffer.SaveToStream(Stream);
            }
        }

        // Engine.pas:10335
        public TEWriteTextStorageStream(TStream AStream, TEngine AEngine) : base(AStream, AEngine)
        {
            FPath = (AnsiString)"";
            var call__356 = new TEBufferStream();
            FBuffer = call__356;
            var call__357 = new TPropertyTable();
            FPropertyTable = call__357;
        }

        // Engine.pas:10343
        public override void Destroy()
        {
            WriteTextBuffer(FStream);
            FBuffer.Free();
            FPropertyTable.Free();
        }

        // Engine.pas:10350
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smWrite, Units.Engine.Engine.smText);
            return Result;
        }

        // Engine.pas:10368
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            TEObject O = default;
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10376
        public override void rwEObject(TEObject Obj, int ID)
        {
            TEBufferStream SavedBuffer = default;
            AnsiString SavedPath = default;
            bool Continue = default;
            var call__358 = FPropertyTable.PropertyExist(ID);
            if (!call__358)
            {
                SavedBuffer = FBuffer;
                SavedPath = FPath;
                FPropertyTable.BackupSettings();
                var call__359 = new TEBufferStream();
                FBuffer = call__359;
                if (FPath != (AnsiString)"")
                {
                    FPath = FPath + AnsiString.FromBytes(92) + IntToStr(ID);
                }
                else
                {
                    FPath = IntToStr(ID);
                }

                try
                {
                    Obj.MainReadWrite(this);
                    WriteTextBuffer(FStream);
                    FBuffer.Free();
                    FPropertyTable.RestoreSettings();
                    FBuffer = SavedBuffer;
                    FPath = SavedPath;
                    FPropertyTable.Add(ID, 0);
                }
                catch (System.Exception)
                {
                    FBuffer.Free();
                    FBuffer = SavedBuffer;
                    FPropertyTable.RestoreSettings();
                    FPath = SavedPath;
                    Continue = true;
                    if (FOnError != null)
                    {
                        var call__360 = Obj.EClass.ClassID;
                        FOnError(this, (AnsiString)"Error writing object (cid:" + IntToStr(call__360) + AnsiString.FromBytes(41), ref Continue);
                    }

                    if (!Continue)
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:10412
        public override void rwEObjectStream(TEObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10417
        public override void rwString(ref AnsiString S, int ID)
        {
            AnsiString Line = default;
            var call__361 = FPropertyTable.PropertyExist(ID);
            if (!call__361)
            {
                if (S != (AnsiString)"")
                {
                    Line = AnsiString.FromBytes(91) + IntToStr(ID) + (AnsiString)"]=" + S + AnsiString.FromBytes(13) + AnsiString.FromBytes(10);
                    FBuffer.Write(StringBytes(Line).Slice(1 - 1, Line.Length));
                    FPropertyTable.Add(ID, 0);
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:10434
        public override void rwStrings(TStrings Strings, int ID)
        {
            AnsiString Line = default;
            AnsiString P = default;
            var call__362 = FPropertyTable.PropertyExist(ID);
            if (!call__362)
            {
                var call__363 = Strings.Count;
                if (call__363 != 0)
                {
                    P = Strings.Text;
                    try
                    {
                        Line = AnsiString.FromBytes(91) + IntToStr(ID) + (AnsiString)"]=";
                        Line += NullTerminatedText(P);

                        FBuffer.Write(StringBytes(Line).Slice(1 - 1, Line.Length));
                        FPropertyTable.Add(ID, 0);
                    }
                    finally
                    {
                        P = default;
                    }
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:10458
        public override void rwStringsEx(TStrings Strings, int ID)
        {
            rwStrings(Strings, ID);
        }

        // Engine.pas:10463
        public override void rwChildren(TECustomNode Node, int ID)
        {
            AnsiString SavedPath = default;
            int I = default;
            TEObject C = default;
            var call__364 = FPropertyTable.PropertyExist(ID);
            if (!call__364)
            {
                SavedPath = FPath;
                if (FPath != (AnsiString)"")
                {
                    FPath = FPath + AnsiString.FromBytes(92) + IntToStr(ID);
                }
                else
                {
                    FPath = IntToStr(ID);
                }

                {
                    I = 0;
                    int end__365 = Node.Count - 1;
                    for (; I <= end__365; I += 1)
                    {
                        var call__366 = Node.Children[I];
                        C = call__366;
                        bool bool__367 = C != null && C.Owner == Node;
                        if (bool__367)
                        {
                            var call__368 = C.GetControlStyle();
                            bool__367 = !call__368.Contains(Units.Engine.Engine.csShared);
                        }

                        bool bool__369 = bool__367;
                        if (bool__369)
                        {
                            var call__370 = C.GetControlStyle();
                            bool__369 = call__370.Contains(Units.Engine.Engine.csStored);
                        }

                        if (bool__369)
                        {
                            rwEObject(C, I);
                        }
                    }
                }

                FPath = SavedPath;
                FPropertyTable.Add(ID, 0);
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found");
            }
        }

    }

    // FPartial, FDataBase. The constructor reads the root property table. every property is found
    // through the table and read at FDataBase + offset
    public partial class TEReadStorageStream : TEStorageStream
    {
        internal bool FPartial;
        internal int FDataBase;
        // Engine.pas:9968
        public TEReadStorageStream(TStream AStream, TEngine AEngine, bool APartial) : base(AStream, AEngine)
        {
            var call__371 = new TPropertyTable();
            FPropertyTable = call__371;
            FPropertyTable.LoadFromStream(AStream);
            var call__372 = FStream.Position;
            FDataBase = call__372;
            FPartial = APartial;
        }

        // Engine.pas:9977
        public override void Destroy()
        {
            FPropertyTable.Free();
            base.Destroy();
        }

        // Engine.pas:9983
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            if (FPartial)
            {
                Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Units.Engine.Engine.smPartial);
            }
            else
            {
                Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead);
            }

            return Result;
        }

        // Engine.pas:9991
        public override void rwInteger(ref int I, int ID)
        {
            int Offset = default;
            var call__373 = FPropertyTable.FindOffset(ID);
            Offset = call__373;
            if (Offset != -1)
            {
                var call__374 = FStream.Position;
                if (call__374 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref I, 4));
            }
            else
            {
                I = 0;
            }
        }

        // Engine.pas:10005
        public override void rwByte(ref byte B, int ID)
        {
            int Offset = default;
            var call__375 = FPropertyTable.FindOffset(ID);
            Offset = call__375;
            if (Offset != -1)
            {
                var call__376 = FStream.Position;
                if (call__376 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref B, 1));
            }
            else
            {
                B = 0;
            }
        }

        // Engine.pas:10019
        public override void rwShortint(ref sbyte S, int ID)
        {
            int Offset = default;
            var call__377 = FPropertyTable.FindOffset(ID);
            Offset = call__377;
            if (Offset != -1)
            {
                var call__378 = FStream.Position;
                if (call__378 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref S, 1));
            }
            else
            {
                S = 0;
            }
        }

        // Engine.pas:10033
        public override void rwWord(ref ushort W, int ID)
        {
            int Offset = default;
            var call__379 = FPropertyTable.FindOffset(ID);
            Offset = call__379;
            if (Offset != -1)
            {
                var call__380 = FStream.Position;
                if (call__380 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref W, 2));
            }
            else
            {
                W = 0;
            }
        }

        // Engine.pas:10047
        public override void rwIntegerEx(ref int I, int Default, int ID)
        {
            int Offset = default;
            var call__381 = FPropertyTable.FindOffset(ID);
            Offset = call__381;
            if (Offset != -1)
            {
                var call__382 = FStream.Position;
                if (call__382 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref I, 4));
            }
            else
            {
                I = Default;
            }
        }

        // Engine.pas:10061
        public override void rwByteEx(ref byte B, byte Default, int ID)
        {
            int Offset = default;
            var call__383 = FPropertyTable.FindOffset(ID);
            Offset = call__383;
            if (Offset != -1)
            {
                var call__384 = FStream.Position;
                if (call__384 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref B, 1));
            }
            else
            {
                B = Default;
            }
        }

        // Engine.pas:10075
        public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
        {
            int Offset = default;
            var call__385 = FPropertyTable.FindOffset(ID);
            Offset = call__385;
            if (Offset != -1)
            {
                var call__386 = FStream.Position;
                if (call__386 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref S, 1));
            }
            else
            {
                S = Default;
            }
        }

        // Engine.pas:10089
        public override void rwWordEx(ref ushort W, ushort Default, int ID)
        {
            int Offset = default;
            var call__387 = FPropertyTable.FindOffset(ID);
            Offset = call__387;
            if (Offset != -1)
            {
                var call__388 = FStream.Position;
                if (call__388 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref W, 2));
            }
            else
            {
                W = Default;
            }
        }

        // Engine.pas:10103
        public override void rwString(ref AnsiString S, int ID)
        {
            int Offset = default;
            var call__389 = FPropertyTable.FindOffset(ID);
            Offset = call__389;
            if (Offset != -1)
            {
                var call__390 = FStream.Position;
                if (call__390 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                Units.Engine.Engine.LoadStringFromStream(ref S, FStream);
            }
            else
            {
                S = (AnsiString)"";
            }
        }

        // Engine.pas:10117
        public override void rwStrings(TStrings Strings, int ID)
        {
            int Offset = default;
            var call__391 = FPropertyTable.FindOffset(ID);
            Offset = call__391;
            if (Offset != -1)
            {
                var call__392 = FStream.Position;
                if (call__392 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                Units.Engine.Engine.StringsFromStream(Strings, FStream);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10131
        public override void rwStringsEx(TStrings Strings, int ID)
        {
            int Offset = default;
            var call__393 = FPropertyTable.FindOffset(ID);
            Offset = call__393;
            if (Offset != -1)
            {
                var call__394 = FStream.Position;
                if (call__394 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                Units.Engine.Engine.StringsFromStreamEx(Strings, FStream);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10145
        public override void rwData(Span<byte> Buffer, int Size, int ID)
        {
            int Offset = default;
            var call__395 = FPropertyTable.FindOffset(ID);
            Offset = call__395;
            if (Offset != -1)
            {
                var call__396 = FStream.Position;
                if (call__396 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Buffer.Slice(0, Size));
            }
        }

        // Engine.pas:10157
        public override void rwStreamProc(int ID, TStreamProc LoadProc, TStreamProc SaveProc)
        {
            int Offset = default;
            var call__397 = FPropertyTable.FindOffset(ID);
            Offset = call__397;
            if (Offset != -1)
            {
                var call__398 = FStream.Position;
                if (call__398 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                try
                {
                    LoadProc(FStream);
                }
                catch (System.Exception)
                {
                }
            }
        }

        // Engine.pas:10172
        public override TStream OpenPropertyStream(int ID)
        {
            TStream Result = default;
            int Offset = default;
            var call__399 = FPropertyTable.FindOffset(ID);
            Offset = call__399;
            if (Offset != -1)
            {
                var call__400 = FStream.Position;
                if (call__400 != FDataBase + Offset)
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
            int Offset = default;
            var call__401 = FPropertyTable.FindOffset(ID);
            Offset = call__401;
            B = Offset != -1;
        }

        // Engine.pas:10191
        public bool HandleException(Exception E)
        {
            bool Result = default;
            bool Continue = default;
            if (!FErrorHandled)
            {
                Continue = true;
                if (FOnError != null)
                {
                    if (E.Message == (AnsiString)"")
                    {
                        FOnError(this, Units.Engine.Engine.EStreamReadErrorStr, ref Continue);
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

            Result = !FErrorHandled;
            return Result;
        }

        // Engine.pas:10208
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            int AClassID = default;
            Obj = null;
            var call__402 = FPropertyTable.FindOffset(ID);
            Offset = call__402;
            if (Offset != -1)
            {
                var call__403 = FStream.Position;
                if (call__403 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref AClassID, 4));
                if (AClassID != 0)
                {
                    SavedBase = FDataBase;
                    FPropertyTable.BackupSettings();
                    FPropertyTable.LoadFromStream(FStream);
                    var call__404 = FStream.Position;
                    FDataBase = call__404;
                    try
                    {
                        var call__406 = FEngine.CreateEObject(AClassID, null);
                        Obj = call__406;
                        Obj.FOwner = AOwner;
                        Obj.MainReadWrite(this);
                        Obj.FOwner = null;
                        Obj.SetOwner(AOwner);
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                    }
                    catch (System.Exception caught__405)
                    {
                        var E = Wrap(caught__405);
                        Obj.Free();
                        Obj = null;
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                        if (!HandleException(E))
                        {
                            throw;
                        }
                    }
                }
            }
        }

        // Engine.pas:10246
        public override void rwChildren(TECustomNode Node, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            int I = default;
            int ChildID = default;
            TEObject Child = default;
            var call__407 = FPropertyTable.FindOffset(ID);
            Offset = call__407;
            if (Offset != -1)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                var call__408 = FStream.Position;
                if (call__408 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                var call__409 = FStream.Position;
                FDataBase = call__409;
                if (Node.GetControlStyle().Contains(Units.Engine.Engine.csKeepIndex))
                {
                    {
                        I = FPropertyTable.First;
                        int end__410 = FPropertyTable.Count - 1;
                        for (; I <= end__410; I += 1)
                        {
                            ChildID = FPropertyTable.Items[I].ID;
                            rwCreateEObject(ref Child, Node, ChildID);
                            if (Child != null)
                            {
                                Child.SetArrayOwner(Node, ChildID);
                            }
                        }
                    }
                }
                else
                {
                    {
                        I = FPropertyTable.First;
                        int end__411 = FPropertyTable.Count - 1;
                        for (; I <= end__411; I += 1)
                        {
                            ChildID = FPropertyTable.Items[I].ID;
                            rwCreateEObject(ref Child, Node, ChildID);
                        }
                    }
                }

                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
            }
        }

        // Engine.pas:10277
        public override void rwEObject(TEObject Obj, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            SavedBase = FDataBase;
            var call__412 = FPropertyTable.FindOffset(ID);
            Offset = call__412;
            FPropertyTable.BackupSettings();
            if (Offset != -1)
            {
                var call__413 = FStream.Position;
                if (call__413 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                var call__414 = FStream.Position;
                FDataBase = call__414;
            }

            try
            {
                Obj.MainReadWrite(this);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
            }
            catch (System.Exception caught__415)
            {
                var E = Wrap(caught__415);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }

        // Engine.pas:10304
        public override void rwEObjectStream(TEObject Obj, int ID)
        {
            int SavedBase = default;
            int Start = default;
            int Size = default;
            var call__416 = FStream.Position;
            Start = call__416;
            FStream.Read(Bytes.Of(ref Size, 4));
            if (Size != 0)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                try
                {
                    FPropertyTable.LoadFromStream(FStream);
                    var call__418 = FStream.Position;
                    FDataBase = call__418;
                    Obj.MainReadWrite(this);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    FStream.Position = Start + Size + 4;
                }
                catch (System.Exception caught__417)
                {
                    var E = Wrap(caught__417);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    FStream.Position = Start + Size + 4;
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
        }

        public int DataBase { get => FDataBase; }

    }

    // FPartial, FBuffer. Values go into the buffer. the property table records (id, buffer
    // position). A nested object gets its own buffer, appended to the outer one as [table][data].
    // The destructor writes the root table and buffer
    public partial class TEWriteStorageStream : TEStorageStream
    {

        internal bool FPartial;
        internal TEBufferStream FBuffer;
        // Engine.pas:9592
        internal void PropertyExistError(int ID)
        {
            throw new EDoublePrioriyID((AnsiString)"PropertyID already in use (" + IntToStr(ID) + (AnsiString)" / " + (AnsiString)ClassNameOf(this.GetType()) + AnsiString.FromBytes(41));
        }

        // Engine.pas:9565
        // Engine.pas:10489 TDebugWriteStream borrows the supplied stream as its buffer.
        private protected TEWriteStorageStream(TStream stream, TEngine engine, TEBufferStream buffer) : base(stream, engine)
        {
            FBuffer = buffer;
            FPropertyTable = new TPropertyTable();
            FPartial = false;
        }

        public TEWriteStorageStream(TStream AStream, TEngine AEngine, bool APartial) : base(AStream, AEngine)
        {
            FEngine = AEngine;
            FStream = AStream;
            var call__419 = new TEBufferStream();
            FBuffer = call__419;
            var call__420 = new TPropertyTable();
            FPropertyTable = call__420;
            FPartial = APartial;
        }

        // Engine.pas:9575
        public override void Destroy()
        {
            FPropertyTable.SaveToStream(FStream);
            FBuffer.SaveToStream(FStream);
            FPropertyTable.Free();
            FBuffer.Free();
            base.Destroy();
        }

        // Engine.pas:9584
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            if (FPartial)
            {
                Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smWrite, Units.Engine.Engine.smPartial);
            }
            else
            {
                Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smWrite);
            }

            return Result;
        }

        // Engine.pas:9597
        public override void rwByte(ref byte B, int ID)
        {
            var call__421 = FPropertyTable.PropertyExist(ID);
            if (!call__421)
            {
                if (B != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__422 = FPropertyTable.PropertyExist(ID);
            if (!call__422)
            {
                if (W != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__423 = FPropertyTable.PropertyExist(ID);
            if (!call__423)
            {
                if (S != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__424 = FPropertyTable.PropertyExist(ID);
            if (!call__424)
            {
                if (I != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    FBuffer.Write(Bytes.Of(ref I, 4));
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"PropertyID already in use");
            }
        }

        // Engine.pas:9653
        public override void rwByteEx(ref byte B, byte Default, int ID)
        {
            var call__425 = FPropertyTable.PropertyExist(ID);
            if (!call__425)
            {
                if (B != Default)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__426 = FPropertyTable.PropertyExist(ID);
            if (!call__426)
            {
                if (W != Default)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__427 = FPropertyTable.PropertyExist(ID);
            if (!call__427)
            {
                if (S != Default)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__428 = FPropertyTable.PropertyExist(ID);
            if (!call__428)
            {
                if (I != Default)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    FBuffer.Write(Bytes.Of(ref I, 4));
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"PropertyID already in use");
            }
        }

        // Engine.pas:9709
        public override void rwString(ref AnsiString S, int ID)
        {
            var call__429 = FPropertyTable.PropertyExist(ID);
            if (!call__429)
            {
                if (S != (AnsiString)"")
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    Units.Engine.Engine.SaveStringToStream(S, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9723
        public override void rwStrings(TStrings Strings, int ID)
        {
            var call__430 = FPropertyTable.PropertyExist(ID);
            if (!call__430)
            {
                var call__431 = Strings.Count;
                if (call__431 != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    Units.Engine.Engine.StringsToStream(Strings, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9737
        public override void rwStringsEx(TStrings Strings, int ID)
        {
            var call__432 = FPropertyTable.PropertyExist(ID);
            if (!call__432)
            {
                var call__433 = Strings.Count;
                if (call__433 != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    Units.Engine.Engine.StringsToStreamEx(Strings, FBuffer);
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
            var call__434 = FPropertyTable.PropertyExist(ID);
            if (!call__434)
            {
                if (Size != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    FBuffer.Write(Buffer.Slice(0, Size));
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9765
        public override void rwStreamProc(int ID, TStreamProc LoadProc, TStreamProc SaveProc)
        {
            var call__435 = FPropertyTable.PropertyExist(ID);
            if (!call__435)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
                try
                {
                    SaveProc(FBuffer);
                }
                catch (System.Exception)
                {
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9779
        public override TStream OpenPropertyStream(int ID)
        {
            TStream Result = default;
            var call__436 = FPropertyTable.PropertyExist(ID);
            if (!call__436)
            {
                FPropertyTable.Add(ID, FBuffer.Position);
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
            var call__437 = FPropertyTable.PropertyExist(ID);
            if (!call__437)
            {
                if (B)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
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
            bool Result = default;
            bool Continue = default;
            if (!FErrorHandled)
            {
                Continue = true;
                if (FOnError != null)
                {
                    if (E.Message == (AnsiString)"")
                    {
                        FOnError(this, Units.Engine.Engine.EStreamWriteErrorStr, ref Continue);
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

            Result = !FErrorHandled;
            return Result;
        }

        // Engine.pas:9820
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            TEBufferStream Outer = default;
            int Start = default;
            int AClassID = default;
            TEObject O = default;
            var call__438 = FPropertyTable.PropertyExist(ID);
            if (!call__438)
            {
                Start = FBuffer.Position;
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
                    var call__439 = new TEBufferStream();
                    FBuffer = call__439;
                    FPropertyTable.BackupSettings();
                    try
                    {
                        Obj.MainReadWrite(this);
                        Outer.Write(Bytes.Of(ref AClassID, 4));
                        FPropertyTable.SaveToStream(Outer);
                        FBuffer.SaveToStream(Outer);
                        FBuffer.Free();
                        FPropertyTable.RestoreSettings();
                        FBuffer = Outer;
                        FPropertyTable.Add(ID, Start);
                    }
                    catch (System.Exception caught__440)
                    {
                        var E = Wrap(caught__440);
                        FPropertyTable.RestoreSettings();
                        FBuffer.Free();
                        FBuffer = Outer;
                        Outer.Position = Start;
                        if (!HandleException(E))
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
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found (" + IntToStr(ID) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:9866
        public override void rwChildren(TECustomNode Node, int ID)
        {
            TEBufferStream Outer = default;
            int Start = default;
            TEObject C = default;
            int I = default;
            var call__441 = FPropertyTable.PropertyExist(ID);
            if (!call__441)
            {
                Start = FBuffer.Position;
                Outer = FBuffer;
                var call__442 = new TEBufferStream();
                FBuffer = call__442;
                FPropertyTable.BackupSettings();
                {
                    I = 0;
                    int end__443 = Node.Count - 1;
                    for (; I <= end__443; I += 1)
                    {
                        var call__444 = Node.Children[I];
                        C = call__444;
                        bool bool__445 = C != null && C.Owner == Node;
                        if (bool__445)
                        {
                            var call__446 = C.GetControlStyle();
                            bool__445 = !call__446.Contains(Units.Engine.Engine.csShared);
                        }

                        bool bool__447 = bool__445;
                        if (bool__447)
                        {
                            var call__448 = C.GetControlStyle();
                            bool__447 = call__448.Contains(Units.Engine.Engine.csStored);
                        }

                        if (bool__447)
                        {
                            rwCreateEObject(ref C, Node, I);
                        }
                    }
                }

                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                FBuffer.Free();
                FPropertyTable.RestoreSettings();
                FBuffer = Outer;
                FPropertyTable.Add(ID, Start);
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found (" + IntToStr(ID) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:9897
        public override void rwEObject(TEObject Obj, int ID)
        {
            TEBufferStream Outer = default;
            int Start = default;
            var call__449 = FPropertyTable.PropertyExist(ID);
            if (!call__449)
            {
                Start = FBuffer.Position;
                Outer = FBuffer;
                var call__450 = new TEBufferStream();
                FBuffer = call__450;
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    FPropertyTable.SaveToStream(Outer);
                    FBuffer.SaveToStream(Outer);
                    FPropertyTable.RestoreSettings();
                    FBuffer.Free();
                    FBuffer = Outer;
                    FPropertyTable.Add(ID, Start);
                }
                catch (System.Exception caught__451)
                {
                    var E = Wrap(caught__451);
                    FPropertyTable.RestoreSettings();
                    FBuffer.Free();
                    FBuffer = Outer;
                    Outer.Position = Start;
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found (" + (AnsiString)ClassNameOf(Obj.GetType()) + AnsiString.FromBytes(58) + IntToStr(ID) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:9931
        public override void rwEObjectStream(TEObject Obj, int ID)
        {
            TEBufferStream Outer = default;
            int Start = default;
            int Len = default;
            Start = FBuffer.Position;
            Outer = FBuffer;
            var call__452 = new TEBufferStream();
            FBuffer = call__452;
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                Outer.Write(Bytes.Of(ref Len, 4));
                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                Len = Outer.Position - Start - 4;
                Outer.Position = Start;
                Outer.Write(Bytes.Of(ref Len, 4));
                Outer.Position = Start + Len + 4;
                FPropertyTable.RestoreSettings();
                FBuffer.Free();
                FBuffer = Outer;
            }
            catch (System.Exception caught__453)
            {
                var E = Wrap(caught__453);
                FBuffer.Free();
                FBuffer = Outer;
                FPropertyTable.RestoreSettings();
                Len = 0;
                Outer.Position = Start;
                Outer.Write(Bytes.Of(ref Len, 4));
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }

    }

    // a write storage stream that writes every object's data straight into the stream (FBuffer is
    // the stream itself)
    public partial class TDebugWriteStream : TEWriteStorageStream
    {
        // Engine.pas:10489
        // Retained: The debug writer uses the supplied stream as FBuffer and skips the normal writer buffer allocation. Retire with a replacement-base storage protocol.
        public TDebugWriteStream(TStream AStream, TEngine AEngine) : base(AStream, AEngine, (TEBufferStream)AStream)
        { }

        // Engine.pas:10499
        public override void Destroy()
        {
            FPropertyTable.SaveToStream(FStream);
            FPropertyTable.Free();
        }

        // Engine.pas:10505
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            int AClassID = default;
            TEObject O = default;
            var call__455 = FPropertyTable.PropertyExist(ID);
            if (!call__455)
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
                        FBuffer.Free();
                        FPropertyTable.RestoreSettings();
                        FPropertyTable.Add(ID, FBuffer.Position);
                    }
                    catch (System.Exception caught__456)
                    {
                        var E = Wrap(caught__456);
                        FPropertyTable.RestoreSettings();
                        if (!HandleException(E))
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    FBuffer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.Add(ID, FBuffer.Position);
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found (" + IntToStr(ID) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:10542
        public override void rwChildren(TECustomNode Node, int ID)
        {
            TEObject C = default;
            int I = default;
            var call__457 = FPropertyTable.PropertyExist(ID);
            if (!call__457)
            {
                FPropertyTable.BackupSettings();
                {
                    I = 0;
                    int end__458 = Node.Count - 1;
                    for (; I <= end__458; I += 1)
                    {
                        var call__459 = Node.Children[I];
                        C = call__459;
                        bool bool__460 = C != null && C.Owner == Node;
                        if (bool__460)
                        {
                            var call__461 = C.GetControlStyle();
                            bool__460 = !call__461.Contains(Units.Engine.Engine.csShared);
                        }

                        bool bool__462 = bool__460;
                        if (bool__462)
                        {
                            var call__463 = C.GetControlStyle();
                            bool__462 = call__463.Contains(Units.Engine.Engine.csStored);
                        }

                        if (bool__462)
                        {
                            rwCreateEObject(ref C, Node, I);
                        }
                    }
                }

                FPropertyTable.SaveToStream(FBuffer);
                FPropertyTable.RestoreSettings();
                FPropertyTable.Add(ID, FBuffer.Position);
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found (" + IntToStr(ID) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:10565
        public override void rwEObject(TEObject Obj, int ID)
        {
            var call__464 = FPropertyTable.PropertyExist(ID);
            if (!call__464)
            {
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    FPropertyTable.SaveToStream(FBuffer);
                    FPropertyTable.RestoreSettings();
                    FPropertyTable.Add(ID, FBuffer.Position);
                }
                catch (System.Exception caught__465)
                {
                    var E = Wrap(caught__465);
                    FPropertyTable.RestoreSettings();
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new EDoublePrioriyID((AnsiString)"Double priorityID found (" + (AnsiString)ClassNameOf(Obj.GetType()) + AnsiString.FromBytes(58) + IntToStr(ID) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:10587
        public override void rwEObjectStream(TEObject Obj, int ID)
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
            catch (System.Exception caught__466)
            {
                var E = Wrap(caught__466);
                FPropertyTable.RestoreSettings();
                Size = 0;
                FBuffer.Write(Bytes.Of(ref Size, 4));
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }
    }

    // visits every string of an object tree through TranslateString and TranslateStrings. nothing
    // is read or written
    public partial class TTranslateStorageStream : TEStorageStream
    {
        // Engine.pas:10825
        public TTranslateStorageStream(TEngine AEngine) : base(null, AEngine)
        {
            var call__467 = new TPropertyTable();
            FPropertyTable = call__467;
        }

        // Engine.pas:10831
        public override void Destroy()
        {
            FPropertyTable.Free();
            base.Destroy();
        }

        // Engine.pas:10837
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Units.Engine.Engine.smText, TStorageModeElement.smTranslate);
            return Result;
        }

        // Engine.pas:10842
        public override void rwString(ref AnsiString S, int ID)
        {
            TranslateString(ref S);
        }

        // Engine.pas:10847
        public override void rwStrings(TStrings Strings, int ID)
        {
            TranslateStrings(ref Strings);
        }

        // Engine.pas:10852
        public override void rwStringsEx(TStrings Strings, int ID)
        {
            TranslateStrings(ref Strings);
        }

        // Engine.pas:10857
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            TEObject O = default;
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10865
        public override void rwEObject(TEObject Obj, int ID)
        {
            bool Continue = default;
            try
            {
                FPropertyTable.BackupSettings();
                Obj.MainReadWrite(this);
                FPropertyTable.RestoreSettings();
            }
            catch (System.Exception)
            {
                FPropertyTable.RestoreSettings();
                // { the original leaves Continue uninitialized here }
                if (FOnError != null)
                {
                    var call__468 = Obj.EClass.ClassID;
                    FOnError(this, (AnsiString)"Error reading object (cid:" + IntToStr(call__468) + AnsiString.FromBytes(41), ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }

        // Engine.pas:10882
        public override void rwEObjectStream(TEObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10887
        public override void rwChildren(TECustomNode Node, int ID)
        {
            int I = default;
            TEObject C = default;
            {
                I = 0;
                int end__469 = Node.Count - 1;
                for (; I <= end__469; I += 1)
                {
                    var call__470 = Node.Children[I];
                    C = call__470;
                    bool bool__471 = C != null && C.Owner == Node;
                    if (bool__471)
                    {
                        var call__472 = C.GetControlStyle();
                        bool__471 = !call__472.Contains(Units.Engine.Engine.csShared);
                    }

                    bool bool__473 = bool__471;
                    if (bool__473)
                    {
                        var call__474 = C.GetControlStyle();
                        bool__473 = call__474.Contains(Units.Engine.Engine.csStored);
                    }

                    if (bool__473)
                    {
                        rwEObject(C, I);
                    }
                }
            }
        }

        // Engine.pas:10901
        public virtual void TranslateStrings(ref TStrings Strings)
        {
        }

        // Engine.pas:10905
        public virtual void TranslateString(ref AnsiString S)
        {
        }

    }

    // a read stream that announces a comparison to come (smSetupCompare)
    public partial class TSetupCompareStorageStream : TEReadStorageStream
    {
        public TSetupCompareStorageStream(TStream AStream, TEngine AEngine, bool APartial) : base(AStream, AEngine, APartial)
        { }

        // Engine.pas:10911
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, TStorageModeElement.smSetupCompare);
            return Result;
        }
    }

    // reads a stored copy and calls Mismatch for every property that differs from the object.
    // Mismatch raises and swallows its own exception, a place for a breakpoint
    public partial class TCompareStorageStream : TEReadStorageStream
    {
        // Engine.pas:11002
        internal void Mismatch(int ID)
        {
            try
            {
                throw Exception.Create((AnsiString)"Property Mismatch (PropertyID:" + IntToStr(ID) + AnsiString.FromBytes(41));
            }
            catch (System.Exception)
            {
            }
        }

        // Engine.pas:10918
        public TCompareStorageStream(TStream AStream, TEngine AEngine) : base(AStream, AEngine, false)
        { }

        // Engine.pas:10923
        public override void Destroy()
        {
            base.Destroy();
        }

        // Engine.pas:10928
        public override TSet<TStorageModeElement> StorageMode()
        {
            TSet<TStorageModeElement> Result = default;
            Result = TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, TStorageModeElement.smCompare);
            return Result;
        }

        // Engine.pas:10933
        public override void rwCreateEObject(ref TEObject Obj, TECustomNode AOwner, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            int AClassID = default;
            var call__475 = FPropertyTable.FindOffset(ID);
            Offset = call__475;
            if (Offset != -1 && Obj != null)
            {
                var call__476 = FStream.Position;
                if (call__476 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Bytes.Of(ref AClassID, 4));
                if (AClassID != 0)
                {
                    SavedBase = FDataBase;
                    FPropertyTable.BackupSettings();
                    FPropertyTable.LoadFromStream(FStream);
                    var call__477 = FStream.Position;
                    FDataBase = call__477;
                    // { The original handles read errors by destroying and clearing Obj. }
                    try
                    {
                        Obj.MainReadWrite(this);
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                    }
                    catch (System.Exception caught__478)
                    {
                        var E = Wrap(caught__478);
                        Obj.Free();
                        Obj = null;
                        FPropertyTable.RestoreSettings();
                        FDataBase = SavedBase;
                        if (!HandleException(E))
                        {
                            throw;
                        }
                    }
                }
            }
        }

        // Engine.pas:10958
        public override void rwChildren(TECustomNode Node, int ID)
        {
            int SavedBase = default;
            int I = default;
            int Start = default;
            TEObject C = default;
            int Offset = default;
            int Index = default;
            var call__479 = FPropertyTable.FindOffset(ID);
            Offset = call__479;
            if (Offset != -1)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                var call__480 = FStream.Position;
                if (call__480 != FDataBase + Offset)
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                var call__481 = FStream.Position;
                FDataBase = call__481;
                if (Node.GetControlStyle().Contains(Units.Engine.Engine.csKeepIndex))
                {
                    {
                        I = FPropertyTable.First;
                        int end__482 = FPropertyTable.Count - 1;
                        for (; I <= end__482; I += 1)
                        {
                            Index = FPropertyTable.Items[I].ID;
                            var call__483 = Node.Children[Index];
                            C = call__483;
                            if (C != null)
                            {
                                rwCreateEObject(ref C, Node, Index);
                            }
                        }
                    }
                }
                else
                {
                    Start = 0;
                    while (true)
                    {
                        var call__484 = Node.Count;
                        bool bool__485 = Start < call__484;
                        if (bool__485)
                        {
                            var call__486 = Node.Children[Start];
                            bool__485 = call__486 != null;
                        }

                        bool bool__487 = bool__485;
                        if (bool__487)
                        {
                            var call__488 = Node.Children[Start];
                            var call__489 = call__488.GetControlStyle();
                            bool__487 = (call__489 & TSet<TEControlStyleElement>.Of(Units.Engine.Engine.csShared)) != TSet<TEControlStyleElement>.Of();
                        }

                        if (!bool__487)
                            break;
                        {
                            Start += 1;
                        }
                    }

                    {
                        I = FPropertyTable.First;
                        int end__490 = FPropertyTable.Count - 1;
                        for (; I <= end__490; I += 1)
                        {
                            Index = FPropertyTable.Items[I].ID;
                            var call__491 = Node.Count;
                            if (Index < call__491)
                            {
                                var call__492 = Node.Children[I - FPropertyTable.First + Start];
                                C = call__492;
                                if (C != null)
                                {
                                    rwCreateEObject(ref C, Node, Index);
                                }
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
        public override void rwString(ref AnsiString S, int ID)
        {
            AnsiString Value = default;
            base.rwString(ref Value, ID);
            if (Value != S)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11030
        public override void rwStrings(TStrings Strings, int ID)
        {
            TStringList L = default;
            // { the original never frees L }
            L = new TStringList();
            base.rwStrings(L, ID);
            var call__493 = Strings.Text;
            var call__494 = L.Text;
            if (CompareText(call__494, call__493) != 0)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11040
        public override void rwStringsEx(TStrings Strings, int ID)
        {
            TStringList L = default;
            L = new TStringList();
            base.rwStrings(L, ID);
            var call__495 = Strings.Text;
            var call__496 = L.Text;
            if (CompareText(call__496, call__495) != 0)
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

    public delegate int TStringsSortCompare(AnsiString S1, AnsiString S2, TObject O1, TObject O2);
    public struct TStringHashItem
    {
        public AnsiString Key;
        public object Item;
    }

    public delegate void TExchangeProc(TList Data, int I, int J);
    public struct TSortStringsContext
    {
        public TStrings Strings;
        public TStringsSortCompare Compare;
        public TExchangeProc ExchangeProc;
        public TList ExchangeData;
    }

    // { the layout of TList, to write its count field straight from the object }
    public partial class TListFields : TObject
    {
        public object FList;
        public int FCount;
    }

    public static partial class Engine
    {
        // The Engine unit of Enginep.dpl. The hash classes come from inc\HashTab.PAS as in the
        // original
        public const int MaxListSize = 134217727;
        // { Compatibility names used by the recovered dependent packages. }
        public const TEControlStyleElement csStored = 0;
        public const TEControlStyleElement csShared = (TEControlStyleElement)1;
        public const TEControlStyleElement csReserved2 = (TEControlStyleElement)2;
        public const TEControlStyleElement csKeepIndex = (TEControlStyleElement)3;
        public const TEngineModeElement eoVerifyClasses = 0;
        public const TStorageModeElement smText = (TStorageModeElement)2;
        public const TStorageModeElement smPartial = (TStorageModeElement)3;
        // Engine.pas:1953
        // { Strings }
        public static AnsiString ReplaceText(AnsiString S, AnsiString Old, AnsiString New)
        {
            AnsiString Result = default;
            int P = default;
            Result = (AnsiString)"";
            P = Pos(Old, S);
            while (P != 0)
            {
                Result = Result + Copy(S, 1, P - 1) + New;
                S = Copy(S, P + Old.Length, S.Length - P - Old.Length + 1);
                P = Pos(Old, S);
            }

            Result = Result + S;
            return Result;
        }

        // Engine.pas:2005
        public static void SortStrings(TStrings Strings, TStringsSortCompare Compare)
        {
            TSortStringsContext Ctx = default;
            var call__497 = Strings.Count;
            if (call__497 != 0)
            {
                Ctx.Strings = Strings;
                Ctx.Compare = Compare;
                Ctx.ExchangeProc = null;
                var call__498 = Strings.Count;
                QuickSortStrings(ref Ctx, 0, call__498 - 1);
            }
        }

        // Engine.pas:2025
        public static void SortStringsEx(TStrings Strings, TStringsSortCompare Compare, ReadOnlySpan<TStrings> LinkedStrings)
        {
            TList Lists = default;
            int I = default;
            TSortStringsContext Ctx = default;
            var call__499 = Strings.Count;
            if (call__499 != 0)
            {
                Strings.BeginUpdate();
                Ctx.Strings = Strings;
                Ctx.Compare = Compare;
                // { Original unsigned test also takes an empty array. }
                if ((uint)(LinkedStrings.Length - 1) >= 0)
                {
                    Lists = new TList();
                    {
                        I = 0;
                        int end__500 = LinkedStrings.Length - 1;
                        for (; I <= end__500; I += 1)
                        {
                            LinkedStrings[I].BeginUpdate();
                            Lists.Add(LinkedStrings[I]);
                        }
                    }

                    Ctx.ExchangeProc = ExchangeLinkedStrings;
                    Ctx.ExchangeData = Lists;
                }
                else
                {
                    Lists = null;
                }

                var call__501 = Strings.Count;
                QuickSortStrings(ref Ctx, 0, call__501 - 1);
                if (Lists != null)
                {
                    {
                        I = 0;
                        int end__502 = LinkedStrings.Length - 1;
                        for (; I <= end__502; I += 1)
                        {
                            LinkedStrings[I].EndUpdate();
                        }
                    }

                    Lists.Free();
                }

                Strings.EndUpdate();
            }
        }

        // Engine.pas:2065
        public static void SortStringsAscending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
        {
            SortStringsEx(Strings, CompareAscending, LinkedStrings);
        }

        // Engine.pas:2076
        public static void SortStringsDescending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
        {
            SortStringsEx(Strings, CompareDescending, LinkedStrings);
        }

        // Engine.pas:2090
        public static void SortStringsNumericalAscending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
        {
            SortStringsEx(Strings, CompareNumericalAscending, LinkedStrings);
        }

        // Engine.pas:2104
        public static void SortStringsNumericalDescending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
        {
            SortStringsEx(Strings, CompareNumericalDescending, LinkedStrings);
        }

        // Engine.pas:2109
        public static int HexToInt(AnsiString S)
        {
            int Result = default;
            int I = default;
            int L = default;
            int D = default;
            byte C = default;
            Result = 0;
            L = S.Length;
            if (L > 8)
            {
                L = 8;
            }

            {
                I = 0;
                int end__503 = L - 1;
                for (; I <= end__503; I += 1)
                {
                    C = S[L - I];
                    if (TSet<byte>.WithBounds(0, 255).WithRange(48, 57).Contains(C))
                    {
                        D = C - 48;
                    }
                    else
                    {
                        if (TSet<byte>.WithBounds(0, 255).WithRange(65, 70).Contains(C))
                        {
                            D = C - 65 + 10;
                        }
                        else
                        {
                            if (TSet<byte>.WithBounds(0, 255).WithRange(97, 102).Contains(C))
                            {
                                D = C - 97 + 10;
                            }
                            else
                            {
                                D = 0;
                            }
                        }
                    }

                    Result = Result | (D << (I * 4));
                }
            }

            return Result;
        }

        // Engine.pas:9224
        // { Plug-in registry }
        public static void RegisterPlugIn(TPlugIn PlugIn)
        {
            int I = default;
            bool Found = default;
            Found = false;
            I = 0;
            while (!Found && I != PlugIns.Count)
            {
                var call__504 = PlugIns[I];
                var call__505 = ((TPlugIn)call__504).Name();
                var call__506 = PlugIn.Name();
                Found = call__505 == call__506;
                I += 1;
            }

            if (!Found)
            {
                PlugIns.Add(PlugIn);
            }
            else
            {
                var call__507 = PlugIn.Name();
                var call__508 = Exception.Create((AnsiString)"PlugIn " + call__507 + (AnsiString)" already registered");
                throw call__508;
            }
        }

        // Engine.pas:9242
        public static void UnregisterPlugIn(TPlugIn PlugIn)
        {
            var call__509 = PlugIns.IndexOf(PlugIn);
            if (call__509 != -1)
            {
                PlugIns.Remove(PlugIn);
            }
            else
            {
                throw Exception.Create((AnsiString)"PlugIn not found");
            }
        }

        // Engine.pas:9337
        public static void InitializePlugIns(TEngine AEngine)
        {
            int I = default;
            {
                I = 0;
                int end__510 = PlugIns.Count - 1;
                for (; I <= end__510; I += 1)
                {
                    var call__511 = PlugIns[I];
                    ((TPlugIn)call__511).Initialize(AEngine);
                }
            }
        }

        // Engine.pas:9344
        public static void FinalizePlugIns(TEngine AEngine)
        {
            int I = default;
            {
                I = 0;
                int end__512 = PlugIns.Count - 1;
                for (; I <= end__512; I += 1)
                {
                    var call__513 = PlugIns[I];
                    ((TPlugIn)call__513).Finalize(AEngine);
                }
            }
        }

        // Engine.pas:9351
        public static TPlugIn GetPlugIn(int Index)
        {
            TPlugIn Result = default;
            if (PlugIns != null && Index >= 0 && Index < PlugIns.Count)
            {
                var call__514 = PlugIns[Index];
                Result = (TPlugIn)call__514;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:9359
        public static TPlugIn FindPlugIn(AnsiString Name)
        {
            TPlugIn Result = default;
            int I = default;
            Result = null;
            if (PlugIns != null)
            {
                I = 0;
                while (I != PlugIns.Count && Result == null)
                {
                    var call__515 = PlugIns[I];
                    var call__516 = ((TPlugIn)call__515).Name();
                    if (call__516 == Name)
                    {
                        var call__517 = PlugIns[I];
                        Result = (TPlugIn)call__517;
                    }

                    I += 1;
                }
            }

            return Result;
        }

        // Engine.pas:9375
        public static void LoadExternalPlugIns(AnsiString Directory)
        {
            AnsiString Dir = default;
            void LoadFrom(AnsiString Dir)
            {
                TSearchRec SR = default;
                TStringList Dirs = default;
                int I = default;
                bool Found = default;
                int H = default;
                Found = FindFirst(Dir + (AnsiString)"*.DPL", 63 - 16, ref SR) == 0;
                while (Found)
                {
                    try
                    {
                        /* TODO: Implement the native Delphi package loader. */
                        throw new NotImplementedException();
                        Packages.AddObject(Dir + SR.Name, IntToObject(H));
                    }
                    catch (System.Exception)
                    {
                        var call__518 = Exception.Create((AnsiString)"Unable to load PlugIn: " + Dir + SR.Name);
                        throw call__518;
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                Dirs = new TStringList();
                Found = FindFirst(Dir + (AnsiString)"*.*", 16, ref SR) == 0;
                while (Found)
                {
                    if ((SR.Attr & 16) != 0 && SR.Name != AnsiString.FromBytes(46) && SR.Name != (AnsiString)"..")
                    {
                        Dirs.Add(Dir + SR.Name + AnsiString.FromBytes(92));
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                {
                    I = 0;
                    int end__520 = Dirs.Count - 1;
                    for (; I <= end__520; I += 1)
                    {
                        var call__521 = Dirs[I];
                        LoadFrom(call__521);
                    }
                }

                Dirs.Free();
            }

            Dir = ExpandFileName(Directory);
            // { the original tests the character of Directory, not of Dir }
            if (Dir != (AnsiString)"" && Directory[Dir.Length] != 92)
            {
                Dir = Dir + AnsiString.FromBytes(92);
            }

            LoadFrom(Dir);
        }

        // Engine.pas:9419
        public static void UnloadExternalPlugIns()
        {
            int I = default;
            AnsiString S = default;
            {
                I = Packages.Count - 1;
                byte end__522 = 0;
                for (; I >= end__522; I -= 1)
                {
                    try
                    {
                        var call__524 = Packages[I];
                        S = call__524;
                        // { the original hands the name, not the handle in Objects, to UnloadPackage }
                        /* TODO: Implement the native Delphi package loader. */
                        throw new NotImplementedException();
                        Packages.Delete(I);
                    }
                    catch (System.Exception)
                    {
                        var call__523 = Packages[I];
                        S = (AnsiString)"Could not release PlugIn: " + call__523;
                        throw Exception.Create(S);
                    }
                }
            }

            Packages.Clear();
        }

        // Engine.pas:9441
        // { Messages and engines }
        public static void InitMsg(ref TEMsg Msg, int MsgID, TEObject Sender)
        {
            Msg = default;
            Msg.MsgID = MsgID;
            Msg.Sender = Sender;
            Msg.Result = -1;
        }

        // Engine.pas:9449
        public static void InitMsgEx(ref TEMsg Msg, int MsgID, TEObject Sender, int Seq)
        {
            Msg = default;
            Msg.MsgID = MsgID;
            Msg.Seq = Seq;
            Msg.Sender = Sender;
            Msg.Result = -1;
        }

        // Engine.pas:3086
        public static int GetCRC32(ReadOnlySpan<byte> buffer, int size)
        {
            if (CRC32Table.Length == 0) BuildCRC32Table();
            int crc = -1;
            foreach (byte value in buffer[..size])
                crc = CRC32Table[(value ^ crc) & 255] ^ (int)((uint)crc >> 8);
            return crc ^ -1;
        }

        // Engine.pas:3099
        public static int GetFileCRC32(AnsiString FileName)
        {
            int Result = default;
            TMemoryStream M = default;
            TFileStream F = default;
            M = new TMemoryStream();
            try
            {
                F = new TFileStream(FileName, 0);
                try
                {
                    M.LoadFromStream(F);
                    var call__525 = M.Size;
                    var call__526 = GetCRC32(M.Memory, call__525);
                    Result = call__526;
                }
                finally
                {
                    F.Free();
                }
            }
            finally
            {
                M.Free();
            }

            return Result;
        }

        // Engine.pas:3081
        public static void ReleaseCRC32Table()
        {
            CRC32Table = [];
        }

        // Engine.pas:9458
        public static TEngine GetEngine(int Index)
        {
            TEngine Result = default;
            object P = default;
            if (Engines != null && Index >= 0 && Index < Engines.Count)
            {
                var call__527 = Engines[Index];
                P = call__527;
                Result = (TEngine)P;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:9466
        public static TEngine FindEngine(int AClassID)
        {
            TEngine Result = default;
            int I = default;
            bool Found = default;
            Found = false;
            Result = null;
            if (Engines != null)
            {
                I = 0;
                Found = false;
                while (!Found && I < Engines.Count)
                {
                    var call__528 = Engines[I];
                    var call__529 = ((TEngine)call__528).EClass.ClassID;
                    Found = AClassID == call__529;
                    I += 1;
                }

                if (Found)
                {
                    var call__530 = Engines[I - 1];
                    Result = (TEngine)call__530;
                }
            }

            if (!Found)
            {
                throw Exception.Create((AnsiString)"Cannot access Engine");
            }

            return Result;
        }

        // Engine.pas:2132
        public static void StringToStream(AnsiString S, TStream Stream)
        {
            int Len = default;
            Len = S.Length;
            Stream.Write(Bytes.Of(ref Len, 4));
            Stream.Write(StringBytes(S).Slice(0, Len));
        }

        // Engine.pas:2141
        public static void StringFromStream(ref AnsiString S, TStream Stream)
        {
            int Len = default;
            Stream.Read(Bytes.Of(ref Len, 4));
            if (Len < 0) throw new ERangeError("Negative string length");
            byte[] buffer = new byte[Len];
            StringBytes(S)[..Math.Min(S.Length, Len)].CopyTo(buffer);
            // A failed read leaves the resized value in the destination.
            S = AnsiString.FromBytes(buffer);
            Stream.Read(buffer);
            S = AnsiString.FromBytes(buffer);
        }

        // Engine.pas:2185
        public static void StringsToStream(TStrings Strings, TStream Stream)
        {
            ReadOnlySpan<byte> P = default;
            int Len = default;
            P = StringBytes(Strings.Text);
            Len = CStringLength(P);
            Stream.Write(Bytes.Of(ref Len, 4));
            Stream.WriteBuffer(P[..Len]);
            P = default;
        }

        // Engine.pas:2197
        public static void StringsFromStream(TStrings Strings, TStream Stream)
        {
            int Len = default;
            Strings.BeginUpdate();
            try
            {
                Stream.Read(Bytes.Of(ref Len, 4));
                if (Len < 0) throw new ERangeError("Negative string length");
                byte[] buffer = new byte[Len];
                Stream.Read(buffer);
                Strings.Text = AnsiString.FromBytes(buffer);
            }
            finally
            {
                Strings.EndUpdate();
            }
        }

        public static bool VerifyEObjectEnabled;
        public static readonly AnsiString EStreamReadErrorStr = (AnsiString)"Error Reading EObject";
        public static readonly AnsiString EStreamWriteErrorStr = (AnsiString)"Error Writing EObject";
        public static readonly AnsiString UserStr = (AnsiString)"User";
        public static TList Engines;
        public static TList PlugIns;
        public static TEngine CopyEngine;
        public static TStringList Packages;
        public static uint ClipboardFormat;
        // Engine.pas:1570
        public static void SortItems(Span<TSortItem> SortList, int L, int R)
        {
            int I = default;
            int J = default;
            int P = default;
            TSortItem T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)].SortValue;
                do
                {
                    while (P > SortList[I].SortValue)
                    {
                        I += 1;
                    }

                    while (P < SortList[J].SortValue)
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
                    SortItems(SortList, L, J);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:1602
        public static void InverseSortItems(Span<TSortItem> SortList, int L, int R)
        {
            int I = default;
            int J = default;
            int P = default;
            TSortItem T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)].SortValue;
                do
                {
                    while (P < SortList[I].SortValue)
                    {
                        I += 1;
                    }

                    while (P > SortList[J].SortValue)
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
                    InverseSortItems(SortList, L, J);
                }

                L = I;
            }
            while (!(I >= R));
        }

        public static FixedArray_byte_4 CompressMagic = FixedArray_byte_4.From([67, 70, 83, 0]);
        // Engine.pas:1977
        public static void QuickSortStrings(ref TSortStringsContext Ctx, int L, int R)
        {
            int I = default;
            int J = default;
            int M = default;
            TObject PO = default;
            AnsiString P = default;
            do
            {
                I = L;
                J = R;
                M = (int)(((uint)(L + R)) >> 1);
                var call__531 = Ctx.Strings[M];
                P = call__531;
                var call__532 = Ctx.Strings.Objects[M];
                PO = call__532;
                do
                {
                    while (true)
                    {
                        var call__533 = Ctx.Strings.Objects[I];
                        var call__534 = Ctx.Strings[I];
                        var call__535 = Ctx.Compare(call__534, P, call__533, PO);
                        if (!(call__535 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__536 = Ctx.Strings.Objects[J];
                        var call__537 = Ctx.Strings[J];
                        var call__538 = Ctx.Compare(call__537, P, call__536, PO);
                        if (!(call__538 > 0))
                            break;
                        {
                            J -= 1;
                        }
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

        // Engine.pas:2018
        public static void ExchangeLinkedStrings(TList Lists, int I, int J)
        {
            int K = default;
            {
                K = Lists.Count - 1;
                byte end__539 = 0;
                for (; K >= end__539; K -= 1)
                {
                    ((TStrings)Lists.List[K]).Exchange(I, J);
                }
            }
        }

        // Engine.pas:2059
        public static int CompareAscending(AnsiString S1, AnsiString S2, TObject O1, TObject O2)
        {
            int Result = default;
            Result = CompareText(S1, S2);
            if (Result == 0)
            {
                Result = S1.Length - S2.Length;
            }

            return Result;
        }

        // Engine.pas:2070
        public static int CompareDescending(AnsiString S1, AnsiString S2, TObject O1, TObject O2)
        {
            int Result = default;
            Result = CompareText(S2, S1);
            if (Result == 0)
            {
                Result = S2.Length - S1.Length;
            }

            return Result;
        }

        // Engine.pas:2081
        public static int CompareNumericalAscending(AnsiString S1, AnsiString S2, TObject O1, TObject O2)
        {
            int Result = default;
            int V1 = default;
            int V2 = default;
            V1 = StrToIntDef(S1, -1);
            V2 = StrToIntDef(S2, -1);
            Result = V1 - V2;
            return Result;
        }

        // Engine.pas:2095
        public static int CompareNumericalDescending(AnsiString S1, AnsiString S2, TObject O1, TObject O2)
        {
            int Result = default;
            int V1 = default;
            int V2 = default;
            V1 = StrToIntDef(S1, -1);
            V2 = StrToIntDef(S2, -1);
            Result = V2 - V1;
            return Result;
        }

        // Engine.pas:2150
        public static void SaveStringToStream(AnsiString S, TStream Stream)
        {
            int Len = default;
            byte B = default;
            Len = S.Length;
            if (Len < 255)
            {
                B = (byte)Len;
                Stream.Write(Bytes.Of(ref B, 1));
            }
            else
            {
                B = 255;
                Stream.Write(Bytes.Of(ref B, 1));
                Stream.Write(Bytes.Of(ref Len, 4));
            }

            Stream.Write(StringBytes(S).Slice(0, Len));
        }

        // Engine.pas:2170
        public static void LoadStringFromStream(ref AnsiString S, TStream Stream)
        {
            int Len = default;
            byte B = default;
            S = (AnsiString)"";
            Stream.Read(Bytes.Of(ref B, 1));
            if (B == 255)
            {
                Stream.Read(Bytes.Of(ref Len, 4));
            }
            else
            {
                Len = B;
            }

            if (Len < 0) throw new ERangeError("Negative string length");
            byte[] buffer = new byte[Len];
            StringBytes(S)[..Math.Min(S.Length, Len)].CopyTo(buffer);
            // A failed read leaves the resized value in the destination.
            S = AnsiString.FromBytes(buffer);
            Stream.Read(buffer);
            S = AnsiString.FromBytes(buffer);
        }

        // Engine.pas:2213
        public static void StringsToStreamEx(TStrings Strings, TStream Stream)
        {
            ReadOnlySpan<byte> P = default;
            int Len = default;
            int I = default;
            int Obj = default;
            P = StringBytes(Strings.Text);
            Len = CStringLength(P);
            Stream.Write(Bytes.Of(ref Len, 4));
            Stream.WriteBuffer(P[..Len]);
            P = default;
            {
                I = 0;
                int end__540 = Strings.Count - 1;
                for (; I <= end__540; I += 1)
                {
                    var call__541 = Strings.Objects[I];
                    Obj = ObjectToInt(call__541);
                    Stream.Write(Bytes.Of(ref Obj, 4));
                }
            }
        }

        // Engine.pas:2230
        public static void StringsFromStreamEx(TStrings Strings, TStream Stream)
        {
            int Len = default;
            int I = default;
            int Obj = default;
            Strings.BeginUpdate();
            try
            {
                Stream.Read(Bytes.Of(ref Len, 4));
                if (Len < 0) throw new ERangeError("Negative string length");
                byte[] buffer = new byte[Len];
                Stream.Read(buffer);
                Strings.Text = AnsiString.FromBytes(buffer);
                {
                    I = 0;
                    int end__542 = Strings.Count - 1;
                    for (; I <= end__542; I += 1)
                    {
                        Stream.Read(Bytes.Of(ref Obj, 4));
                        Strings.Objects[I] = IntToObject(Obj);
                    }
                }
            }
            finally
            {
                Strings.EndUpdate();
            }
        }

        // { CRC32 }
        public static int[] CRC32Table = [];
        // Engine.pas:3061
        public static void BuildCRC32Table()
        {
            int I = default;
            int J = default;
            int C = default;
            int Polynomial = default;
            if (CRC32Table.Length == 0)
            {
                CRC32Table = new int[256];
                Polynomial = -306674912;
                {
                    I = 0;
                    byte end__543 = 255;
                    for (; I <= end__543; I += 1)
                    {
                        C = I;
                        {
                            J = 7;
                            byte end__544 = 0;
                            for (; J >= end__544; J -= 1)
                            {
                                if ((C & 1) != 0)
                                {
                                    C = ((int)(((uint)C) >> 1)) ^ Polynomial;
                                }
                                else
                                {
                                    C = (int)(((uint)C) >> 1);
                                }

                                CRC32Table[I] = C;
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:3438
        public static void QuickSortInteger(Span<int> SortList, int L, int R, TIntegerSortCompare SCompare)
        {
            int I = default;
            int J = default;
            int P = default;
            int T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__545 = SCompare(SortList[I], P);
                        if (!(call__545 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__546 = SCompare(SortList[J], P);
                        if (!(call__546 > 0))
                            break;
                        {
                            J -= 1;
                        }
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

        // Engine.pas:3773
        public static void QuickSortByte(Span<byte> SortList, int L, int R, TByteSortCompare SCompare)
        {
            int I = default;
            int J = default;
            byte P = default;
            byte T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__547 = SCompare(SortList[I], P);
                        if (!(call__547 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__548 = SCompare(SortList[J], P);
                        if (!(call__548 > 0))
                            break;
                        {
                            J -= 1;
                        }
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
                    QuickSortByte(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:4120
        public static void QuickSortShortInt(Span<sbyte> SortList, int L, int R, TShortIntSortCompare SCompare)
        {
            int I = default;
            int J = default;
            sbyte P = default;
            sbyte T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__549 = SCompare(SortList[I], P);
                        if (!(call__549 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__550 = SCompare(SortList[J], P);
                        if (!(call__550 > 0))
                            break;
                        {
                            J -= 1;
                        }
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
                    QuickSortShortInt(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:4431
        public static void QuickSortWord(Span<ushort> SortList, int L, int R, TWordSortCompare SCompare)
        {
            int I = default;
            int J = default;
            ushort P = default;
            ushort T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__551 = SCompare(SortList[I], P);
                        if (!(call__551 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__552 = SCompare(SortList[J], P);
                        if (!(call__552 > 0))
                            break;
                        {
                            J -= 1;
                        }
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
                    QuickSortWord(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:5254
        public static void QuickSortDoubleItem(Span<TDoubleItem> SortList, int L, int R, TDoubleItemSortCompare SCompare)
        {
            int I = default;
            int J = default;
            TDoubleItem P = default;
            TDoubleItem T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__553 = SCompare(SortList[I], P);
                        if (!(call__553 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__554 = SCompare(SortList[J], P);
                        if (!(call__554 > 0))
                            break;
                        {
                            J -= 1;
                        }
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
                    QuickSortDoubleItem(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:5287
        public static int CompareDoubleItemAAscending(in TDoubleItem Item1, in TDoubleItem Item2)
        {
            int Result = default;
            Result = Item1.A - Item2.A;
            return Result;
        }

        // Engine.pas:5292
        public static int CompareDoubleItemBAscending(in TDoubleItem Item1, in TDoubleItem Item2)
        {
            int Result = default;
            Result = Item1.B - Item2.B;
            return Result;
        }

        // Engine.pas:5297
        public static int CompareDoubleItemADescending(in TDoubleItem Item1, in TDoubleItem Item2)
        {
            int Result = default;
            Result = Item2.A - Item1.A;
            return Result;
        }

        // Engine.pas:5302
        public static int CompareDoubleItemBDescending(in TDoubleItem Item1, in TDoubleItem Item2)
        {
            int Result = default;
            Result = Item2.B - Item1.B;
            return Result;
        }

        // Engine.pas:5664
        public static void QuickSortQuadItem(Span<TQuadInteger> SortList, int L, int R, TQuadItemSortCompare SCompare)
        {
            int I = default;
            int J = default;
            TQuadInteger P = default;
            TQuadInteger T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__555 = SCompare(SortList[I], P);
                        if (!(call__555 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__556 = SCompare(SortList[J], P);
                        if (!(call__556 > 0))
                            break;
                        {
                            J -= 1;
                        }
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
                    QuickSortQuadItem(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:5697
        public static int CompareQuadItemAAscending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.A - Item2.A;
            return Result;
        }

        // Engine.pas:5702
        public static int CompareQuadItemBAscending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.B - Item2.B;
            return Result;
        }

        // Engine.pas:5707
        public static int CompareQuadItemCAscending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.C - Item2.C;
            return Result;
        }

        // Engine.pas:5712
        public static int CompareQuadItemDAscending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.D - Item2.D;
            return Result;
        }

        // Engine.pas:5717
        public static int CompareQuadItemADescending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.A - Item1.A;
            return Result;
        }

        // Engine.pas:5722
        public static int CompareQuadItemBDescending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.B - Item1.B;
            return Result;
        }

        // Engine.pas:5727
        public static int CompareQuadItemCDescending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.C - Item1.C;
            return Result;
        }

        // Engine.pas:5732
        public static int CompareQuadItemDDescending(in TQuadInteger Item1, in TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.D - Item1.D;
            return Result;
        }

        // Engine.pas:10675
        // { "[id]=" at the start of a text line, or -1 }
        public static int GetLineID(AnsiString S)
        {
            int Result = default;
            int I = default;
            I = 1;
            while (TSet<byte>.WithBounds(0, 255).With(91).WithRange(48, 57).Contains(S[I]))
            {
                I += 1;
            }

            if (Copy(S, I, 2) == (AnsiString)"]=")
            {
                Result = StrToInt(Copy(S, 2, I - 2));
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:10685
        // { the text behind "[id]=" }
        public static AnsiString GetLineValue(AnsiString S)
        {
            AnsiString Result = default;
            int I = default;
            I = 1;
            while (TSet<byte>.WithBounds(0, 255).With(91).With(93).WithRange(48, 57).Contains(S[I]))
            {
                I += 1;
            }

            Result = Copy(S, I + 1, S.Length - I);
            return Result;
        }

        public static void Initialize()
        {
            CRC32Table = [];
            VerifyEObjectEnabled = true;
            var call__557 = new TList();
            Engines = call__557;
            var call__558 = new TList();
            PlugIns = call__558;
            var call__559 = new TStringList();
            Packages = call__559;
            var call__560 = RegisterClipboardFormat((AnsiString)"ECLIPBOARD FORMAT");
            ClipboardFormat = call__560;
            RegisterClass(typeof(TECopyComponent));
        }

        public static void Finalize()
        {
            ReleaseCRC32Table();
            UnloadExternalPlugIns();
            Packages.Free();
            Engines.Free();
            PlugIns.Free();
        }
    }
}
