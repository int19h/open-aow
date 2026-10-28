// Translated from Engine.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.FMemStr;
using static Aow.Units.FMemStr.FMemStr;
using Aow.Units.ImageLib;
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
    public partial class TIntegerList : Aow.Rtl.TPersistent
    {
        internal Ptr<int> FList;
        internal int FCount;
        internal int FCapacity;
        internal int FDefault;
        // Engine.pas:3343
        public int Get(int Index)
        {
            int Result = default;
            if ((Index < 0) || (Index >= FCount))
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
        // { AoWE $557652FC, $55765367 }
        internal void Put(int Index, int Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:3428
        internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Units.Engine.Engine.FillInteger(FList + FCount, NewCount - FCount, FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:3537
        internal Aow.Rtl.AnsiString GetItemString()
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__1 = FCount - 1;
                for (; I <= end__1; I += 1)
                {
                    Result = (Result + Aow.Rtl.Rtl.IntToStr(FList[I])) + AnsiString.FromBytes(44);
                }
            }

            if (Result != ((AnsiString)""))
            {
                Result = Aow.Rtl.Rtl.Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:3548
        internal void SetItemString(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.AnsiString Item = default;
            int P = default;
            int V = default;
            Clear();
            P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
            while (S != ((AnsiString)""))
            {
                if (P != 0)
                {
                    Item = Aow.Rtl.Rtl.Copy(S, 1, P - 1);
                    S = Aow.Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Item = S;
                    S = (AnsiString)"";
                }

                if (Item != ((AnsiString)""))
                {
                    V = Aow.Rtl.Rtl.StrToInt(Item);
                }
                else
                {
                    V = 0;
                }

                Add(V);
                P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
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
        // { TIntegerList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:3273
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TIntegerList)
            {
                {
                    var w__2 = (Aow.Units.Engine.TIntegerList)Source;
                    this.FCount = w__2.FCount;
                    this.SetCapacity(w__2.FCount);
                    Ptr<int>.Copy(w__2.FList, this.FList, this.FCount);
                    this.FDefault = w__2.FDefault;
                }
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<int>.Copy(FList + (Index + 1), FList + Index, (FCount - Index));
            }
        }

        // Engine.pas:3321
        public void Exchange(int Index1, int Index2)
        {
            int Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:3332
        public Aow.Units.Engine.TIntegerList Expand()
        {
            Aow.Units.Engine.TIntegerList Result = default;
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
            while ((Result < FCount) && (FList[Result] != Item))
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
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Ptr<int>.Copy(FList + Index, FList + (Index + 1), (FCount - Index));
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
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
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
        public void Sort(Aow.Units.Engine.TIntegerSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.Engine.Engine.QuickSortInteger(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:3471
        public bool Compare(Aow.Units.Engine.TIntegerList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__6 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount)), MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index, FCount)), FCount * 4);
                Result = call__6;
            }

            return Result;
        }

        // Engine.pas:3478
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int Count = default;
            int Item = default;
            int Run = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (Count != FCount)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Item, 4));
                if (Item == FDefault)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:3499
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
                J = I;
                while ((J < FCount) && (FList[J] == FDefault))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 4));
                    Stream.Write(Aow.Rtl.Bytes.Of(ref Run, 4));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes((FList + I).Array.AsSpan((FList + I).Index, 1)));
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

                R = Aow.Rtl.Rtl.Random(Total);
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

        public Aow.Rtl.AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }
        public Ptr<int> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TIntegerList Create()
        {
            return new Aow.Units.Engine.TIntegerList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:3310
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TIntegerList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    public delegate int TByteSortCompare(byte Item1, byte Item2);
    public partial class TByteList : Aow.Rtl.TPersistent
    {
        internal Ptr<byte> FList;
        internal int FCount;
        internal int FCapacity;
        internal byte FDefault;
        // Engine.pas:3678
        internal byte Get(int Index)
        {
            byte Result = default;
            if ((Index < 0) || (Index >= FCount))
            {
                Result = unchecked((byte)FDefault);
            }
            else
            {
                Result = unchecked((byte)FList[Index]);
            }

            return Result;
        }

        // Engine.pas:3732
        internal void Put(int Index, byte Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            FList[Index] = unchecked((byte)Item);
        }

        // Engine.pas:3752
        internal void SetCapacity(int NewCapacity)
        {
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity * 1);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:3763
        internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Rtl.Rtl.FillBytes((FList + FCount).Array.AsSpan((FList + FCount).Index, NewCount - FCount), FDefault);
            }

            FCount = NewCount;
        }

        // Engine.pas:3883
        internal Aow.Rtl.AnsiString GetItemString()
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__8 = FCount - 1;
                for (; I <= end__8; I += 1)
                {
                    Result = (Result + Aow.Rtl.Rtl.IntToStr(FList[I])) + AnsiString.FromBytes(44);
                }
            }

            if (Result != ((AnsiString)""))
            {
                Result = Aow.Rtl.Rtl.Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:3894
        internal void SetItemString(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.AnsiString Item = default;
            int P = default;
            int V = default;
            Clear();
            P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
            while (S != ((AnsiString)""))
            {
                if (P != 0)
                {
                    Item = Aow.Rtl.Rtl.Copy(S, 1, P - 1);
                    S = Aow.Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Item = S;
                    S = (AnsiString)"";
                }

                if (Item != ((AnsiString)""))
                {
                    V = Aow.Rtl.Rtl.StrToInt(Item);
                }
                else
                {
                    V = 0;
                }

                if (V > 255)
                {
                    V = 0;
                }

                Add(unchecked((byte)V));
                P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
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
        // { TByteList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:3608
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TByteList)
            {
                {
                    var w__9 = (Aow.Units.Engine.TByteList)Source;
                    this.FCount = w__9.FCount;
                    this.SetCapacity(w__9.FCount);
                    Ptr<byte>.Copy(w__9.FList, this.FList, this.FCount * 1);
                    this.FDefault = unchecked((byte)w__9.FDefault);
                }
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

            FList[Result] = unchecked((byte)Item);
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<byte>.Copy(FList + (Index + 1), FList + Index, (FCount - Index) * 1);
            }
        }

        // Engine.pas:3656
        public void Exchange(int Index1, int Index2)
        {
            byte Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = unchecked((byte)FList[Index1]);
            FList[Index1] = unchecked((byte)FList[Index2]);
            FList[Index2] = unchecked((byte)Item);
        }

        // Engine.pas:3667
        public Aow.Units.Engine.TByteList Expand()
        {
            Aow.Units.Engine.TByteList Result = default;
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
            Result = unchecked((byte)Get(0));
            return Result;
        }

        // Engine.pas:3696
        public int IndexOf(byte Item)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && (FList[Result] != Item))
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
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Ptr<byte>.Copy(FList + Index, FList + (Index + 1), (FCount - Index) * 1);
            }

            FList[Index] = unchecked((byte)Item);
            FCount += 1;
        }

        // Engine.pas:3714
        public byte Last()
        {
            byte Result = default;
            var call__10 = Get(FCount - 1);
            Result = unchecked((byte)call__10);
            return Result;
        }

        // Engine.pas:3719
        public void Move(int CurIndex, int NewIndex)
        {
            byte Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = unchecked((byte)Get(CurIndex));
                Delete(CurIndex);
                Insert(NewIndex, unchecked((byte)Item));
            }
        }

        // Engine.pas:3739
        public int Remove(byte Item)
        {
            int Result = default;
            Result = IndexOf(unchecked((byte)Item));
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
        public void Sort(Aow.Units.Engine.TByteSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.Engine.Engine.QuickSortByte(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:3806
        public bool Compare(Aow.Units.Engine.TByteList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__13 = Aow.Rtl.Rtl.CompareMem(FList.Array.AsSpan(FList.Index, FCount * 1), List.FList.Array.AsSpan(List.FList.Index, FCount * 1), FCount * 1);
                Result = call__13;
            }

            return Result;
        }

        // Engine.pas:3813
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            byte Item = default;
            byte Run = default;
            int Count = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Item, 1));
                if (Item == FDefault)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Run, 1));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(unchecked((byte)Item));
                }
            }
        }

        // Engine.pas:3835
        public void SaveToStream(Aow.Rtl.TStream Stream)
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
                while ((J < FCount) && (FList[J] == FDefault))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    while (Run > 255)
                    {
                        B = 255;
                        Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
                        Stream.Write(Aow.Rtl.Bytes.Of(ref B, 1));
                        Run -= 255;
                        I += 255;
                    }

                    B = unchecked((byte)Run);
                    Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
                    Stream.Write(Aow.Rtl.Bytes.Of(ref B, 1));
                    I += Run;
                }
                else
                {
                    Stream.Write((FList + I).Array.AsSpan((FList + I).Index, 1));
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

                R = Aow.Rtl.Rtl.Random(Total);
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

        public Aow.Rtl.AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }
        public Ptr<byte> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TByteList Create()
        {
            return new Aow.Units.Engine.TByteList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:3645
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TByteList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    public delegate int TShortIntSortCompare(sbyte Item1, sbyte Item2);
    public partial class TShortIntList : Aow.Rtl.TPersistent
    {
        internal Ptr<sbyte> FList;
        internal int FCount;
        internal int FCapacity;
        internal sbyte FDefault;
        // Engine.pas:4025
        internal sbyte Get(int Index)
        {
            sbyte Result = default;
            if ((Index < 0) || (Index >= FCount))
            {
                Result = unchecked((sbyte)FDefault);
            }
            else
            {
                Result = unchecked((sbyte)FList[Index]);
            }

            return Result;
        }

        // Engine.pas:4079
        internal void Put(int Index, sbyte Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            FList[Index] = unchecked((sbyte)Item);
        }

        // Engine.pas:4099
        internal void SetCapacity(int NewCapacity)
        {
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity * 1);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:4110
        internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Rtl.Rtl.FillBytes(MemoryMarshal.AsBytes((FList + FCount).Array.AsSpan((FList + FCount).Index, NewCount - FCount)), FDefault);
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
        // { TShortIntList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:3955
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TShortIntList)
            {
                {
                    var w__15 = (Aow.Units.Engine.TShortIntList)Source;
                    this.FCount = w__15.FCount;
                    this.SetCapacity(w__15.FCount);
                    Ptr<sbyte>.Copy(w__15.FList, this.FList, this.FCount * 1);
                    this.FDefault = unchecked((sbyte)w__15.FDefault);
                }
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

            FList[Result] = unchecked((sbyte)Item);
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<sbyte>.Copy(FList + (Index + 1), FList + Index, (FCount - Index) * 1);
            }
        }

        // Engine.pas:4003
        public void Exchange(int Index1, int Index2)
        {
            sbyte Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = unchecked((sbyte)FList[Index1]);
            FList[Index1] = unchecked((sbyte)FList[Index2]);
            FList[Index2] = unchecked((sbyte)Item);
        }

        // Engine.pas:4014
        public Aow.Units.Engine.TShortIntList Expand()
        {
            Aow.Units.Engine.TShortIntList Result = default;
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
            Result = unchecked((sbyte)Get(0));
            return Result;
        }

        // Engine.pas:4043
        public int IndexOf(sbyte Item)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && (FList[Result] != Item))
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
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Ptr<sbyte>.Copy(FList + Index, FList + (Index + 1), (FCount - Index) * 1);
            }

            FList[Index] = unchecked((sbyte)Item);
            FCount += 1;
        }

        // Engine.pas:4061
        public sbyte Last()
        {
            sbyte Result = default;
            var call__16 = Get(FCount - 1);
            Result = unchecked((sbyte)call__16);
            return Result;
        }

        // Engine.pas:4066
        public void Move(int CurIndex, int NewIndex)
        {
            sbyte Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = unchecked((sbyte)Get(CurIndex));
                Delete(CurIndex);
                Insert(NewIndex, unchecked((sbyte)Item));
            }
        }

        // Engine.pas:4086
        public int Remove(sbyte Item)
        {
            int Result = default;
            Result = IndexOf(unchecked((sbyte)Item));
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
        public void Sort(Aow.Units.Engine.TShortIntSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.Engine.Engine.QuickSortShortInt(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:4153
        public bool Compare(Aow.Units.Engine.TShortIntList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__19 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount * 1)), MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index, FCount * 1)), FCount * 1);
                Result = call__19;
            }

            return Result;
        }

        // Engine.pas:4160
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            byte Run = default;
            sbyte Item = default;
            int Count = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Item, 1));
                if (Item == FDefault)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Run, 1));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(unchecked((sbyte)Item));
                }
            }
        }

        // Engine.pas:4183
        public void SaveToStream(Aow.Rtl.TStream Stream)
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
                while ((J < FCount) && (FList[J] == FDefault))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    while (Run > 255)
                    {
                        B = 255;
                        Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
                        Stream.Write(Aow.Rtl.Bytes.Of(ref B, 1));
                        Run -= 255;
                        I += 255;
                    }

                    B = unchecked((byte)Run);
                    Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
                    Stream.Write(Aow.Rtl.Bytes.Of(ref B, 1));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes((FList + I).Array.AsSpan((FList + I).Index, 1)));
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

                R = Aow.Rtl.Rtl.Random(Total);
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

        public Ptr<sbyte> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TShortIntList Create()
        {
            return new Aow.Units.Engine.TShortIntList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:3992
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TShortIntList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    public delegate int TWordSortCompare(ushort Item1, ushort Item2);
    public partial class TWordList : Aow.Rtl.TPersistent
    {
        internal Ptr<ushort> FList;
        internal int FCount;
        internal int FCapacity;
        internal ushort FDefault;
        // Engine.pas:4336
        internal ushort Get(int Index)
        {
            ushort Result = default;
            if ((Index < 0) || (Index >= FCount))
            {
                Result = unchecked((ushort)FDefault);
            }
            else
            {
                Result = unchecked((ushort)FList[Index]);
            }

            return Result;
        }

        // Engine.pas:4390
        internal void Put(int Index, ushort Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            FList[Index] = unchecked((ushort)Item);
        }

        // Engine.pas:4410
        internal void SetCapacity(int NewCapacity)
        {
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:4421
        internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Units.Engine.Engine.FillWord(FList + FCount, NewCount - FCount, FDefault);
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
        // { TWordList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:4266
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TWordList)
            {
                {
                    var w__21 = (Aow.Units.Engine.TWordList)Source;
                    this.FCount = w__21.FCount;
                    this.SetCapacity(w__21.FCount);
                    Ptr<ushort>.Copy(w__21.FList, this.FList, this.FCount);
                    this.FDefault = unchecked((ushort)w__21.FDefault);
                }
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

            FList[Result] = unchecked((ushort)Item);
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<ushort>.Copy(FList + (Index + 1), FList + Index, (FCount - Index));
            }
        }

        // Engine.pas:4314
        public void Exchange(int Index1, int Index2)
        {
            ushort Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = unchecked((ushort)FList[Index1]);
            FList[Index1] = unchecked((ushort)FList[Index2]);
            FList[Index2] = unchecked((ushort)Item);
        }

        // Engine.pas:4325
        public Aow.Units.Engine.TWordList Expand()
        {
            Aow.Units.Engine.TWordList Result = default;
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
            Result = unchecked((ushort)Get(0));
            return Result;
        }

        // Engine.pas:4354
        public int IndexOf(ushort Item)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && (FList[Result] != Item))
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
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Ptr<ushort>.Copy(FList + Index, FList + (Index + 1), (FCount - Index));
            }

            FList[Index] = unchecked((ushort)Item);
            FCount += 1;
        }

        // Engine.pas:4372
        public ushort Last()
        {
            ushort Result = default;
            var call__22 = Get(FCount - 1);
            Result = unchecked((ushort)call__22);
            return Result;
        }

        // Engine.pas:4377
        public void Move(int CurIndex, int NewIndex)
        {
            ushort Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = unchecked((ushort)Get(CurIndex));
                Delete(CurIndex);
                Insert(NewIndex, unchecked((ushort)Item));
            }
        }

        // Engine.pas:4397
        public int Remove(ushort Item)
        {
            int Result = default;
            Result = IndexOf(unchecked((ushort)Item));
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
        public void Sort(Aow.Units.Engine.TWordSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.Engine.Engine.QuickSortWord(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:4464
        public bool Compare(Aow.Units.Engine.TWordList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__25 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount)), MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index, FCount)), FCount * 2);
                Result = call__25;
            }

            return Result;
        }

        // Engine.pas:4471
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            ushort Item = default;
            ushort Run = default;
            int Count = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 2));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Item, 2));
                if (Item == FDefault)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Run, 2));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(unchecked((ushort)Item));
                }
            }
        }

        // Engine.pas:4493
        public void SaveToStream(Aow.Rtl.TStream Stream)
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
                while ((J < FCount) && (FList[J] == FDefault))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 2));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    while (Run > 65535)
                    {
                        B = 65535;
                        Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 2));
                        Stream.Write(Aow.Rtl.Bytes.Of(ref B, 2));
                        // { sic: the original subtracts $FF here but advances I by $FFFF }
                        Run -= 255;
                        I += 65535;
                    }

                    B = unchecked((ushort)Run);
                    Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 2));
                    Stream.Write(Aow.Rtl.Bytes.Of(ref B, 2));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes((FList + I).Array.AsSpan((FList + I).Index, 1)));
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

                R = Aow.Rtl.Rtl.Random(Total);
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

        public Ptr<ushort> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TWordList Create()
        {
            return new Aow.Units.Engine.TWordList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:4303
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TWordList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    public partial class TBitList : Aow.Rtl.TPersistent
    {
        internal Ptr<byte> FList;
        internal int FCount;
        internal int FCapacity;
        internal bool FDefault;
        // Engine.pas:4743
        internal void SetCapacity(int NewCapacity)
        {
            int I = default;
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            NewCapacity = (NewCapacity + 31) & ~31;
            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc((int)(((uint)NewCapacity) >> 3));
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
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
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
        internal Aow.Rtl.AnsiString GetItemString()
        {
            Aow.Rtl.AnsiString Result = default;
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
                        Result = (Result + Aow.Rtl.Rtl.IntToStr(I)) + AnsiString.FromBytes(44);
                    }
                }
            }

            if (Result != ((AnsiString)""))
            {
                Result = Aow.Rtl.Rtl.Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // Engine.pas:4810
        internal void SetItemString(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.AnsiString Item = default;
            int P = default;
            Clear();
            P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
            while (S != ((AnsiString)""))
            {
                if (P != 0)
                {
                    Item = Aow.Rtl.Rtl.Copy(S, 1, P - 1);
                    S = Aow.Rtl.Rtl.Delete(S, 1, P);
                }
                else
                {
                    Item = S;
                    S = (AnsiString)"";
                }

                if (Item != ((AnsiString)""))
                {
                    Put(Aow.Rtl.Rtl.StrToInt(Item), !FDefault);
                }

                P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), S);
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
        // { TBitList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:4576
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TBitList)
            {
                {
                    var w__33 = (Aow.Units.Engine.TBitList)Source;
                    this.FCount = w__33.FCount;
                    this.SetCapacity(w__33.FCount);
                    Aow.Rtl.Rtl.Move(w__33.FList.AsSpan(((int)(((uint)(this.FCount + 31)) >> 5)) * 4), this.FList.AsSpan(((int)(((uint)(this.FCount + 31)) >> 5)) * 4), ((int)(((uint)(this.FCount + 31)) >> 5)) * 4);
                    this.FDefault = w__33.FDefault;
                }
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = Get(Index1);
            var call__36 = Get(Index2);
            Put(Index1, call__36);
            Put(Index2, Item);
        }

        // Engine.pas:4635
        public Aow.Units.Engine.TBitList Expand()
        {
            Aow.Units.Engine.TBitList Result = default;
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
                bool bool__37 = (Result < FCount);
                if (bool__37)
                {
                    var call__38 = Get(Result);
                    bool__37 = (Item != call__38);
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
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
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
        public bool Compare(Aow.Units.Engine.TBitList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__44 = Aow.Rtl.Rtl.CompareMem(FList.AsSpan((int)(((uint)(FCount + 7)) >> 3)), List.FList.AsSpan((int)(((uint)(FCount + 7)) >> 3)), (int)(((uint)(FCount + 7)) >> 3));
                Result = call__44;
            }

            return Result;
        }

        // Engine.pas:4782
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FCount, 4));
            SetCapacity(FCount);
            Stream.Read(FList.AsSpan((int)(((uint)(FCount + 7)) >> 3)));
        }

        // Engine.pas:4791
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            Stream.Write(FList.AsSpan((int)(((uint)(FCount + 7)) >> 3)));
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public bool Default { get => FDefault; set => FDefault = value; }

        public bool this[int Index] { get => Get(Index); set => Put(Index, value); }

        public Aow.Rtl.AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }
        public int BitsSet { get => GetBitsSet(); }
        public int BitsCleared { get => GetBitsCleared(); }

        // System.pas:111
        public static Aow.Units.Engine.TBitList Create()
        {
            return new Aow.Units.Engine.TBitList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:4613
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TBitList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    public partial class TEventList : Aow.Rtl.TPersistent
    {
        internal Ptr<Aow.Rtl.TNotifyEvent> FList;
        internal int FCount;
        internal int FCapacity;
        internal Aow.Rtl.TNotifyEvent FDefault;
        // Engine.pas:4929
        internal Aow.Rtl.TNotifyEvent Get(int Index)
        {
            Aow.Rtl.TNotifyEvent Result = default;
            if ((Index < 0) || (Index >= FCount))
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
        internal void Put(int Index, Aow.Rtl.TNotifyEvent Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:5022
        internal void SetCount(int NewCount)
        {
            int I = default;
            int OldCount = default;
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
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
        // { TEventList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:4859
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TEventList)
            {
                {
                    var w__46 = (Aow.Units.Engine.TEventList)Source;
                    this.FCount = w__46.FCount;
                    this.SetCapacity(w__46.FCount);
                    Ptr<Aow.Rtl.TNotifyEvent>.Copy(w__46.FList, this.FList, this.FCount);
                    this.FDefault = w__46.FDefault;
                }
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:4873
        public int Add(Aow.Rtl.TNotifyEvent Item)
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<Aow.Rtl.TNotifyEvent>.Copy(FList + (Index + 1), FList + Index, (FCount - Index));
            }
        }

        // Engine.pas:4907
        public void Exchange(int Index1, int Index2)
        {
            Aow.Rtl.TNotifyEvent Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:4918
        public Aow.Units.Engine.TEventList Expand()
        {
            Aow.Units.Engine.TEventList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:4924
        public Aow.Rtl.TNotifyEvent First()
        {
            Aow.Rtl.TNotifyEvent Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:4947
        public int IndexOf(Aow.Rtl.TNotifyEvent Item)
        {
            int Result = default;
            System.Delegate M = default;
            Result = 0;
            M = Item;
            while ((Result < FCount) && ((Aow.Rtl.Rtl.MethodTarget(FList[Result]) != Aow.Rtl.Rtl.MethodTarget(M)) || (Aow.Rtl.Rtl.MethodCode(FList[Result]) != Aow.Rtl.Rtl.MethodCode(M))))
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
        public void Insert(int Index, Aow.Rtl.TNotifyEvent Item)
        {
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Ptr<Aow.Rtl.TNotifyEvent>.Copy(FList + Index, FList + (Index + 1), (FCount - Index));
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:4969
        public Aow.Rtl.TNotifyEvent Last()
        {
            Aow.Rtl.TNotifyEvent Result = default;
            var call__47 = Get(FCount - 1);
            Result = call__47;
            return Result;
        }

        // Engine.pas:4974
        public void Move(int CurIndex, int NewIndex)
        {
            Aow.Rtl.TNotifyEvent Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:4994
        public int Remove(Aow.Rtl.TNotifyEvent Item)
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
            System.Delegate M = default;
            M = FDefault;
            // { sic: the original deletes an item only when both halves differ from the default }
            {
                I = FCount - 1;
                byte end__48 = 0;
                for (; I >= end__48; I -= 1)
                {
                    if ((Aow.Rtl.Rtl.MethodTarget(FList[I]) != Aow.Rtl.Rtl.MethodTarget(M)) && (Aow.Rtl.Rtl.MethodCode(FList[I]) != Aow.Rtl.Rtl.MethodCode(M)))
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:5039
        public bool Compare(Aow.Units.Engine.TEventList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__49 = Aow.Rtl.Rtl.PointerSequenceEqual(FList, List.FList, FCount);
                Result = call__49;
            }

            return Result;
        }

        // Engine.pas:5046
        public void TriggerNotifyEvents(Aow.Rtl.TObject Sender)
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
        public Aow.Rtl.TNotifyEvent Default { get => FDefault; set => FDefault = value; }

        public Aow.Rtl.TNotifyEvent this[int Index] { get => Get(Index); set => Put(Index, value); }

        public Ptr<Aow.Rtl.TNotifyEvent> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TEventList Create()
        {
            return new Aow.Units.Engine.TEventList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:4896
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEventList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct TDoubleItem
    {
        [FieldOffset(0)]
        public int A;
        [FieldOffset(4)]
        public int B;
    }

    public delegate int TDoubleItemSortCompare(in Aow.Units.Engine.TDoubleItem Item1, in Aow.Units.Engine.TDoubleItem Item2);
    public partial class TDoubleItemList : Aow.Rtl.TPersistent
    {
        internal Ptr<Aow.Units.Engine.TDoubleItem> FList;
        internal int FCount;
        internal int FCapacity;
        internal Aow.Units.Engine.TDoubleItem FDefault;
        // Engine.pas:5140
        internal Aow.Units.Engine.TDoubleItem Get(int Index)
        {
            Aow.Units.Engine.TDoubleItem Result = default;
            if ((Index < 0) || (Index >= FCount))
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
        internal void Put(int Index, Aow.Units.Engine.TDoubleItem Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:5244
        internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Units.Engine.Engine.FillLargeInteger(FList + FCount, NewCount - FCount, FDefault);
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
        // { TDoubleItemList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:5061
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TDoubleItemList)
            {
                {
                    var w__51 = (Aow.Units.Engine.TDoubleItemList)Source;
                    this.FCount = w__51.FCount;
                    this.SetCapacity(w__51.FCount);
                    Ptr<Aow.Units.Engine.TDoubleItem>.Copy(w__51.FList, this.FList, this.FCount);
                    this.FDefault = w__51.FDefault;
                }
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:5075
        public int Add(in Aow.Units.Engine.TDoubleItem Item)
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Aow.Rtl.Rtl.Move(MemoryMarshal.AsBytes((FList + (Index + 1)).Array.AsSpan((FList + (Index + 1)).Index, (FCount - Index))), MemoryMarshal.AsBytes((FList + Index).Array.AsSpan((FList + Index).Index, (FCount - Index))), (FCount - Index) * 8);
            }
        }

        // Engine.pas:5118
        public void Exchange(int Index1, int Index2)
        {
            Aow.Units.Engine.TDoubleItem Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:5129
        public Aow.Units.Engine.TDoubleItemList Expand()
        {
            Aow.Units.Engine.TDoubleItemList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:5135
        public Aow.Units.Engine.TDoubleItem First()
        {
            Aow.Units.Engine.TDoubleItem Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:5158
        public int IndexOf(in Aow.Units.Engine.TDoubleItem Item)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && (((double)Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref FList[Result])) != Unsafe.BitCast<Aow.Units.Engine.TDoubleItem, long>(Item)))
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
            while ((Result < FCount) && (FList[Result].A != A))
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
            while ((Result < FCount) && (FList[Result].B != B))
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
        public void Insert(int Index, in Aow.Units.Engine.TDoubleItem Item)
        {
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Aow.Rtl.Rtl.Move(MemoryMarshal.AsBytes((FList + Index).Array.AsSpan((FList + Index).Index, (FCount - Index))), MemoryMarshal.AsBytes((FList + (Index + 1)).Array.AsSpan((FList + (Index + 1)).Index, (FCount - Index))), (FCount - Index) * 8);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:5190
        public Aow.Units.Engine.TDoubleItem Last()
        {
            Aow.Units.Engine.TDoubleItem Result = default;
            var call__52 = Get(FCount - 1);
            Result = call__52;
            return Result;
        }

        // Engine.pas:5195
        public void Move(int CurIndex, int NewIndex)
        {
            Aow.Units.Engine.TDoubleItem Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:5215
        public int Remove(in Aow.Units.Engine.TDoubleItem Item)
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
            Aow.Units.Engine.TDoubleItem Item = default;
            {
                I = FCount - 1;
                byte end__53 = 0;
                for (; I >= end__53; I -= 1)
                {
                    var call__54 = this[I];
                    Item = call__54;
                    if (((double)Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref Item)) == Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref FDefault))
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:5281
        public void Sort(Aow.Units.Engine.TDoubleItemSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.Engine.Engine.QuickSortDoubleItem(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:5307
        public void SortAscendingItemA()
        {
            Sort(Aow.Units.Engine.Engine.CompareDoubleItemAAscending);
        }

        // Engine.pas:5312
        public void SortAscendingItemB()
        {
            Sort(Aow.Units.Engine.Engine.CompareDoubleItemBAscending);
        }

        // Engine.pas:5317
        public void SortDescendingItemA()
        {
            Sort(Aow.Units.Engine.Engine.CompareDoubleItemADescending);
        }

        // Engine.pas:5322
        public void SortDescendingItemB()
        {
            Sort(Aow.Units.Engine.Engine.CompareDoubleItemBDescending);
        }

        // Engine.pas:5327
        public bool Compare(Aow.Units.Engine.TDoubleItemList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__55 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount)), MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index, FCount)), FCount * 8);
                Result = call__55;
            }

            return Result;
        }

        // Engine.pas:5334
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            Aow.Units.Engine.TDoubleItem Item = default;
            int Run = default;
            int Count = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 8));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Item, 8));
                if (((double)Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref Item)) == Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref FDefault))
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:5356
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
                J = I;
                while ((J < FCount) && (((double)Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref FList[J])) == Unsafe.As<Aow.Units.Engine.TDoubleItem, long>(ref FDefault)))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 8));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 8));
                    Stream.Write(Aow.Rtl.Bytes.Of(ref Run, 4));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes((FList + I).Array.AsSpan((FList + I).Index, 1)));
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

                R = Aow.Rtl.Rtl.Random(Total);
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

                R = Aow.Rtl.Rtl.Random(Total);
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
        public Aow.Units.Engine.TDoubleItem Default { get => FDefault; set => FDefault = value; }

        public Aow.Units.Engine.TDoubleItem this[int Index] { get => Get(Index); set => Put(Index, value); }

        public Ptr<Aow.Units.Engine.TDoubleItem> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TDoubleItemList Create()
        {
            return new Aow.Units.Engine.TDoubleItemList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:5107
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TDoubleItemList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
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

    public delegate int TQuadItemSortCompare(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2);
    public partial class TQuadItemList : Aow.Rtl.TPersistent
    {
        internal Ptr<Aow.Units.Engine.TQuadInteger> FList;
        internal int FCount;
        internal int FCapacity;
        internal Aow.Units.Engine.TQuadInteger FDefault;
        // Engine.pas:5538
        internal Aow.Units.Engine.TQuadInteger Get(int Index)
        {
            Aow.Units.Engine.TQuadInteger Result = default;
            if ((Index < 0) || (Index >= FCount))
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
        internal void Put(int Index, Aow.Units.Engine.TQuadInteger Item)
        {
            if (Index < 0)
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if ((NewCapacity < FCount) || (NewCapacity > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // Engine.pas:5654
        internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > Aow.Units.Engine.Engine.MaxListSize))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Units.Engine.Engine.FillQuadInteger(FList + FCount, NewCount - FCount, FDefault);
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
        // { TQuadItemList }
        public override void Destroy()
        {
            Clear();
        }

        // Engine.pas:5457
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.Engine.TQuadItemList)
            {
                {
                    var w__58 = (Aow.Units.Engine.TQuadItemList)Source;
                    this.FCount = w__58.FCount;
                    this.SetCapacity(w__58.FCount);
                    Ptr<Aow.Units.Engine.TQuadInteger>.Copy(w__58.FList, this.FList, this.FCount);
                    this.FDefault = w__58.FDefault;
                }
            }
            else
            {
                base.Assign(Source);
            }
        }

        // Engine.pas:5471
        public int Add(in Aow.Units.Engine.TQuadInteger Item)
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Aow.Rtl.Rtl.Move(MemoryMarshal.AsBytes((FList + (Index + 1)).Array.AsSpan((FList + (Index + 1)).Index, (FCount - Index))), MemoryMarshal.AsBytes((FList + Index).Array.AsSpan((FList + Index).Index, (FCount - Index))), (FCount - Index) * 16);
            }
        }

        // Engine.pas:5516
        public void Exchange(int Index1, int Index2)
        {
            Aow.Units.Engine.TQuadInteger Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // Engine.pas:5527
        public Aow.Units.Engine.TQuadItemList Expand()
        {
            Aow.Units.Engine.TQuadItemList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // Engine.pas:5533
        public Aow.Units.Engine.TQuadInteger First()
        {
            Aow.Units.Engine.TQuadInteger Result = default;
            Result = Get(0);
            return Result;
        }

        // Engine.pas:5556
        public int IndexOf(in Aow.Units.Engine.TQuadInteger Item)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && ((((FList[Result].A != Item.A) || (FList[Result].B != Item.B)) || (FList[Result].C != Item.C)) || (FList[Result].D != Item.D)))
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
            while ((Result < FCount) && (FList[Result].A != A))
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
            while ((Result < FCount) && (FList[Result].B != B))
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
            while ((Result < FCount) && (FList[Result].C != C))
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
            while ((Result < FCount) && (FList[Result].D != D))
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
        public void Insert(int Index, in Aow.Units.Engine.TQuadInteger Item)
        {
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Aow.Rtl.Rtl.Move(MemoryMarshal.AsBytes((FList + Index).Array.AsSpan((FList + Index).Index, (FCount - Index))), MemoryMarshal.AsBytes((FList + (Index + 1)).Array.AsSpan((FList + (Index + 1)).Index, (FCount - Index))), (FCount - Index) * 16);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // Engine.pas:5603
        public Aow.Units.Engine.TQuadInteger Last()
        {
            Aow.Units.Engine.TQuadInteger Result = default;
            var call__59 = Get(FCount - 1);
            Result = call__59;
            return Result;
        }

        // Engine.pas:5608
        public void Move(int CurIndex, int NewIndex)
        {
            Aow.Units.Engine.TQuadInteger Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // Engine.pas:5628
        public int Remove(in Aow.Units.Engine.TQuadInteger Item)
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
                    if ((((FList[I].A == FDefault.A) && (FList[I].B == FDefault.B)) && (FList[I].C == FDefault.C)) && (FList[I].D == FDefault.D))
                    {
                        Delete(I);
                    }
                }
            }
        }

        // Engine.pas:5691
        public void Sort(Aow.Units.Engine.TQuadItemSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.Engine.Engine.QuickSortQuadItem(FList, 0, Count - 1, Compare);
            }
        }

        // Engine.pas:5737
        public void SortAscendingItemA()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemAAscending);
        }

        // Engine.pas:5742
        public void SortAscendingItemB()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemBAscending);
        }

        // Engine.pas:5747
        public void SortAscendingItemC()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemCAscending);
        }

        // Engine.pas:5752
        public void SortAscendingItemD()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemDAscending);
        }

        // Engine.pas:5757
        public void SortDescendingItemA()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemADescending);
        }

        // Engine.pas:5762
        public void SortDescendingItemB()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemBDescending);
        }

        // Engine.pas:5767
        public void SortDescendingItemC()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemCDescending);
        }

        // Engine.pas:5772
        public void SortDescendingItemD()
        {
            Sort(Aow.Units.Engine.Engine.CompareQuadItemDDescending);
        }

        // Engine.pas:5777
        public bool Compare(Aow.Units.Engine.TQuadItemList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__61 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount)), MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index, FCount)), FCount * 16);
                Result = call__61;
            }

            return Result;
        }

        // Engine.pas:5784
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int Run = default;
            int Count = default;
            Aow.Units.Engine.TQuadInteger Item = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefault, 16));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (FCount < Count)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Item, 16));
                if ((((Item.A == FDefault.A) && (Item.B == FDefault.B)) && (Item.C == FDefault.C)) && (Item.D == FDefault.D))
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // Engine.pas:5807
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
                J = I;
                while ((J < FCount) && ((((FList[J].A == FDefault.A) && (FList[J].B == FDefault.B)) && (FList[J].C == FDefault.C)) && (FList[J].D == FDefault.D)))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 16));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    Stream.Write(Aow.Rtl.Bytes.Of(ref FDefault, 16));
                    Stream.Write(Aow.Rtl.Bytes.Of(ref Run, 4));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes((FList + I).Array.AsSpan((FList + I).Index, 1)));
                    I += 1;
                }
            }
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public Aow.Units.Engine.TQuadInteger Default { get => FDefault; set => FDefault = value; }

        public Aow.Units.Engine.TQuadInteger this[int Index] { get => Get(Index); set => Put(Index, value); }

        public Ptr<Aow.Units.Engine.TQuadInteger> List { get => FList; }

        // System.pas:111
        public static Aow.Units.Engine.TQuadItemList Create()
        {
            return new Aow.Units.Engine.TQuadItemList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:5505
            protected internal virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TQuadItemList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
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

    public partial class TPropertyTable : Aow.Rtl.TObject
    {
        internal Ptr<Aow.Units.Engine.TPropertyItem> FItems;
        internal int FCapacity;
        internal int FCount;
        internal int FNextIndex;
        internal int FMaxID;
        internal Ptr<Aow.Units.Engine.TPropertySettings> FStack;
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
                FItems = FItems.Realloc(NewCapacity);
                FCapacity = NewCapacity;
                if (FCapacity < FCount)
                {
                    FCount = FCapacity;
                }
            }
        }

        // Engine.pas:2253
        // { TPropertyTable }
        public override void Destroy()
        {
            SetCapacity(0);
            FStack = FStack.Realloc(0);
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
            if ((I >= FCount) || (FItems[I].ID != ID))
            {
                I = FFirst;
                Result = -1;
                while ((I < FCount) && (FItems[I].ID != ID))
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
                while ((I < FCount) && (FItems[I].ID != ID))
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
                FStack = FStack.Realloc(FStackCapacity);
            }

            {
                ref var w__62 = ref FStack[FStackCount];
                w__62.First = FFirst;
                w__62.NextIndex = FNextIndex;
                w__62.MaxID = FMaxID;
            }

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
            {
                ref var w__63 = ref FStack[FStackCount];
                FFirst = w__63.First;
                FNextIndex = w__63.NextIndex;
                FMaxID = w__63.MaxID;
            }
        }

        // Engine.pas:2361
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int LongCount = default;
            int ID = default;
            int Offset = default;
            byte ShortCount = default;
            ushort Pair = default;
            ushort I = default;
            Stream.Read(Aow.Rtl.Bytes.Of(ref ShortCount, 1));
            if ((ShortCount & 128) != 0)
            {
                ShortCount = unchecked((byte)(ShortCount & 127));
                Stream.Read(Aow.Rtl.Bytes.Of(ref LongCount, 4));
            }
            else
            {
                LongCount = 0;
            }

            if (ShortCount != 0)
            {
                {
                    int first__66 = (int)0;
                    int end__64 = (int)(ShortCount - 1);
                    for (int ordinal__65 = first__66; ordinal__65 <= end__64; ordinal__65++)
                    {
                        I = (ushort)ordinal__65;
                        Stream.Read(Aow.Rtl.Bytes.Of(ref Pair, 2));
                        Add(Pair & 255, (int)(((uint)Pair) >> 8));
                    }
                }
            }

            if (LongCount != 0)
            {
                {
                    int first__69 = (int)0;
                    int end__67 = (int)(LongCount - 1);
                    for (int ordinal__68 = first__69; ordinal__68 <= end__67; ordinal__68++)
                    {
                        I = (ushort)ordinal__68;
                        Stream.Read(Aow.Rtl.Bytes.Of(ref ID, 4));
                        Stream.Read(Aow.Rtl.Bytes.Of(ref Offset, 4));
                        Add(ID, Offset);
                    }
                }
            }
        }

        // Engine.pas:2391
        public void SaveToStream(Aow.Rtl.TStream Stream)
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
                    if ((FItems[I].ID <= 127) && (FItems[I].Offset <= 255))
                    {
                        ShortCount = (byte)(((int)ShortCount) + 1);
                    }
                    else
                    {
                        LongCount += 1;
                    }
                }
            }

            if (LongCount != 0)
            {
                Marker = unchecked((byte)(ShortCount | 128));
                Stream.Write(Aow.Rtl.Bytes.Of(ref Marker, 1));
                Stream.Write(Aow.Rtl.Bytes.Of(ref LongCount, 4));
            }
            else
            {
                Stream.Write(Aow.Rtl.Bytes.Of(ref ShortCount, 1));
            }

            if (ShortCount != 0)
            {
                {
                    I = FFirst;
                    int end__71 = FCount - 1;
                    for (; I <= end__71; I += 1)
                    {
                        if ((FItems[I].ID <= 127) && (FItems[I].Offset <= 255))
                        {
                            Pair = FItems[I].ID + (FItems[I].Offset << 8);
                            Stream.Write(Aow.Rtl.Bytes.Of(ref Pair, 2));
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
                        if ((FItems[I].ID > 127) || (FItems[I].Offset > 255))
                        {
                            Stream.Write(Aow.Rtl.Bytes.Of(ref FItems[I].ID, 4));
                            Stream.Write(Aow.Rtl.Bytes.Of(ref FItems[I].Offset, 4));
                        }
                    }
                }
            }
        }

        public Ptr<Aow.Units.Engine.TPropertyItem> Items { get => FItems; }
        public int Count { get => FCount; }
        public int First { get => FFirst; }

        // System.pas:111
        public static Aow.Units.Engine.TPropertyTable Create()
        {
            return new Aow.Units.Engine.TPropertyTable();
        }
    }

    // { Engine.TEObject (instance size 8): +4 FOwner. }
    public partial class TEObject : Aow.Rtl.TObject
    {
        protected internal Aow.Units.Engine.TECustomNode FOwner;
        // Engine.pas:2487
        internal Aow.Rtl.AnsiString GetEObjectPath()
        {
            Aow.Rtl.AnsiString Result = default;
            Aow.Units.Engine.TEObject P = default;
            Aow.Rtl.AnsiString S = default;
            S = (AnsiString)TObject.ClassNameOf(this.GetType());
            P = FOwner;
            while (P != null)
            {
                S = (((AnsiString)TObject.ClassNameOf(P.GetType())) + ((AnsiString)"")) + S;
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
        public virtual TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = TSet<Aow.Units.Engine.TEControlStyleElement>.Of(Aow.Units.Engine.Engine.csStored);
            return Result;
        }

        // Engine.pas:2568
        public virtual void SetOwner(Aow.Units.Engine.TECustomNode AOwner)
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
        public virtual Aow.Units.Engine.TEngine GetEngine()
        {
            Aow.Units.Engine.TEngine Result = default;
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
        public virtual void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
        }

        // Engine.pas:2594
        public virtual void MainReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            if ((Stream.StorageMode() & TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smText, Aow.Units.Engine.TStorageModeElement.smCompare)) == TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead))
            {
                Clear();
            }

            ReadWrite(Stream);
        }

        // Engine.pas:2430
        // { TEObject }
        public virtual void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            ;
            SetOwner(AOwner);
        }

        // Engine.pas:2447
        public virtual int AddRef()
        {
            int Result = default;
            throw Aow.Rtl.Exception.Create((AnsiString)"Reference counting not supported");
            return Result;
        }

        // Engine.pas:2452
        public virtual int Release()
        {
            int Result = default;
            throw Aow.Rtl.Exception.Create((AnsiString)"Reference counting not supported");
            return Result;
        }

        // Engine.pas:2457
        public virtual void Clear()
        {
        }

        // Engine.pas:2511
        public virtual bool Compare(Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TEngine AEngine)
        {
            bool Result = default;
            Aow.Rtl.TMemoryStream S1 = default;
            Aow.Rtl.TMemoryStream S2 = default;
            Result = false;
            var call__74 = ClassType.ClassID();
            var call__75 = Obj.ClassType.ClassID();
            if (call__74 != call__75)
            {
                return Result;
            }

            S1 = new Aow.Rtl.TMemoryStream();
            S2 = new Aow.Rtl.TMemoryStream();
            try
            {
                AEngine.WriteEObject(S1, this, null);
                AEngine.WriteEObject(S2, Obj, null);
                var call__76 = S1.Size;
                var call__77 = S2.Size;
                bool bool__78 = (call__76 == call__77);
                if (bool__78)
                {
                    var call__79 = S1.Size;
                    var call__80 = Aow.Rtl.Rtl.CompareMem(S1.Memory.AsSpan(0, call__79), S2.Memory.AsSpan(0, call__79), call__79);
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
        public virtual Aow.Units.Engine.TEObjectEditor.Meta Editor()
        {
            Aow.Units.Engine.TEObjectEditor.Meta Result = default;
            Aow.Rtl.TObject.Meta C = default;
            Aow.Units.Engine.TEngine E = default;
            Result = null;
            C = this.ClassType;
            E = GetEngine();
            if (E != null)
            {
                while ((C.Type != typeof(Aow.Units.Engine.TEObject)) && (Result == null))
                {
                    var call__81 = ((Aow.Units.Engine.TEObject.Meta)C).ClassID();
                    var call__82 = E.FEditors.FindEditor(call__81);
                    Result = call__82;
                    if (Result == null)
                    {
                        C = C.ClassParent;
                    }
                }
            }

            if (Result == null)
            {
                Result = Aow.Units.Engine.TEObjectEditor.Meta.Of<Aow.Units.Engine.TEObjectEditor>();
            }

            return Result;
        }

        // Engine.pas:2529
        public virtual void Assign(Aow.Units.Engine.TEObject Source, Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Rtl.TMemoryStream S = default;
            S = new Aow.Rtl.TMemoryStream();
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
        public virtual void TimerEvent(int Tick, Aow.Units.Engine.TEObject Sender)
        {
        }

        // Engine.pas:2585
        public virtual void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            if (Msg.MsgID == 1)
            {
                SetLastMsg(2147483647);
            }
        }

        // Engine.pas:2578
        public virtual void SetArrayOwner(Aow.Units.Engine.TECustomNode AOwner, int Index)
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
        public Aow.Units.Engine.TEObject Copy(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.Engine.TEObject Result = default;
            Aow.Rtl.TMemoryStream S = default;
            S = new Aow.Rtl.TMemoryStream();
            try
            {
                AEngine.WriteEObject(S, this, null);
                S.Position = 0;
                Result = ((Aow.Units.Engine.TEObject.Meta)this.ClassType).Create(null);
                AEngine.ReadEObject(S, Result, null);
            }
            finally
            {
                S.Destroy();
            }

            return Result;
        }

        public Aow.Units.Engine.TECustomNode Owner { get => FOwner; set => SetOwner(value); }
        public Aow.Rtl.AnsiString EObjectPath { get => GetEObjectPath(); }
        public int LastMsg { get => GetLastMsg(); set => SetLastMsg(value); }

        // Engine.pas:524
        public static Aow.Units.Engine.TEObject Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEObject o = new Aow.Units.Engine.TEObject();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // Engine.pas:2461
            public virtual int ClassID()
            {
                int Result = default;
                Result = 80;
                return Result;
            }

            // Engine.pas:524
            public Aow.Units.Engine.TEObject Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEObject o = (Aow.Units.Engine.TEObject)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEObject => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { Engine.TECustomNode (instance size 8): children through an abstract list interface. Message ids
//     carry flags: $10000000 deliver once per Seq, $20000000 owned children only, $40000000 do not
//     forward to children. }
    public partial class TECustomNode : Aow.Units.Engine.TEObject
    {
        // Engine.pas:2666
        public override void MainReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            if (Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smRead))
            {
                if ((Stream.StorageMode() & TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.Engine.smText, Aow.Units.Engine.TStorageModeElement.smCompare)) == TSet<Aow.Units.Engine.TStorageModeElement>.Of())
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
            Aow.Units.Engine.TEObject C = default;
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
                        if ((C != null) && (C.Owner == this))
                        {
                            if (!C.GetControlStyle().Contains(Aow.Units.Engine.Engine.csShared))
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
                            bool bool__88 = (C.Owner == this);
                            if (bool__88)
                            {
                                var call__89 = C.GetControlStyle();
                                bool__88 = call__89.Contains(Aow.Units.Engine.Engine.csShared);
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
        public override void Assign(Aow.Units.Engine.TEObject Source, Aow.Units.Engine.TEngine AEngine)
        {
            Clear();
            base.Assign(Source, AEngine);
        }

        // Engine.pas:2720
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (SendToChild(ref Msg))
            {
                SendMsg(ref Msg, Sender);
            }
        }

        // Engine.pas:593
        public virtual Aow.Units.Engine.TEObject GetChild(int Index)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:594
        public virtual void SetChild(int Index, Aow.Units.Engine.TEObject Child)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:595
        public virtual int GetCount()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:2661
        public virtual void ReadWriteChildren(Aow.Units.Engine.TEStorageStream Stream)
        {
            Stream.rwChildren(this, 1);
        }

        // Engine.pas:2715
        public virtual bool SendToChild(ref Aow.Units.Engine.TEMsg Msg)
        {
            bool Result = default;
            Result = (Msg.MsgID & 1073741824) == 0;
            return Result;
        }

        // Engine.pas:2608
        public virtual void DestroyAllChildren()
        {
            Aow.Units.Engine.TEObject C = default;
            int I = default;
            {
                I = Count - 1;
                byte end__90 = 0;
                for (; I >= end__90; I -= 1)
                {
                    var call__91 = Children[I];
                    C = call__91;
                    if ((C != null) && (C.Owner == this))
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
        public virtual void SendMsg(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            int I = default;
            Aow.Units.Engine.TEObject C = default;
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
                            bool bool__99 = (C != null);
                            if (bool__99)
                            {
                                var call__100 = C.GetLastMsg();
                                bool__99 = (call__100 != Msg.Seq);
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
                            bool bool__103 = (C != null);
                            if (bool__103)
                            {
                                var call__104 = C.GetLastMsg();
                                bool__103 = (call__104 != Msg.Seq);
                            }

                            if (bool__103 && (C.Owner == this))
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
                            if ((C != null) && (C.Owner == this))
                            {
                                C.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:601
        public virtual int RemoveChild(Aow.Units.Engine.TEObject Child)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:602
        public virtual int AddChild(Aow.Units.Engine.TEObject Child)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:2786
        // { The original tests Result before the child-count call. }
        public virtual int DestroyChildren(int AClassID)
        {
            int Result = default;
            int I = default;
            Aow.Units.Engine.TEObject C = default;
            int N = default;
            N = 0;
            {
                I = Count - 1;
                byte end__109 = 0;
                for (; I >= end__109; I -= 1)
                {
                    var call__110 = Children[I];
                    C = call__110;
                    if ((C != null) && (C.ClassType.ClassID() == AClassID))
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
        public virtual Aow.Units.Engine.TEObject FindChildInTree(int AClassID)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            Result = FindChild(AClassID);
            if (Result == null)
            {
                I = 0;
                while (true)
                {
                    bool bool__111 = (Result == null);
                    if (bool__111)
                    {
                        var call__112 = Count;
                        bool__111 = (I < call__112);
                    }

                    if (!bool__111)
                        break;
                    {
                        var call__113 = Children[I];
                        if (call__113 is Aow.Units.Engine.TECustomNode)
                        {
                            var call__114 = Children[I];
                            var call__115 = ((Aow.Units.Engine.TECustomNode)call__114).FindChildInTree(AClassID);
                            Result = call__115;
                        }

                        I += 1;
                    }
                }
            }

            return Result;
        }

        // Engine.pas:2726
        public virtual Aow.Units.Engine.TEObject FindChild(int AClassID)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            bool Found = default;
            Aow.Units.Engine.TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (true)
            {
                bool bool__116 = !Found;
                if (bool__116)
                {
                    var call__117 = Count;
                    bool__116 = (I < call__117);
                }

                if (!bool__116)
                    break;
                {
                    var call__118 = Children[I];
                    C = call__118;
                    Found = (C != null) && (C.ClassType.ClassID() == AClassID);
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
        public virtual Aow.Units.Engine.TEObject FindOwnedChild(int AClassID)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            bool Found = default;
            Aow.Units.Engine.TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (true)
            {
                bool bool__119 = !Found;
                if (bool__119)
                {
                    var call__120 = Count;
                    bool__119 = (I < call__120);
                }

                if (!bool__119)
                    break;
                {
                    var call__121 = Children[I];
                    C = call__121;
                    bool bool__122 = (C != null) && (C.Owner == this);
                    if (bool__122)
                    {
                        var call__123 = C.ClassType.ClassID();
                        bool__122 = (AClassID == call__123);
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
        public virtual int FindChildIndex(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            var call__124 = Count;
            Result = call__124 - 1;
            while (true)
            {
                bool bool__125 = (Result >= 0);
                if (bool__125)
                {
                    var call__126 = Children[Result];
                    bool__125 = (Child != call__126);
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
        // { TECustomNode }
        public override void Destroy()
        {
            DestroyAllChildren();
            base.Destroy();
        }

        public int Count { get => GetCount(); }

        public sealed class ChildrenAccessor(Aow.Units.Engine.TECustomNode target)
        {
            public Aow.Units.Engine.TEObject this[int Index] { get => target.GetChild(Index); set => target.SetChild(Index, value); }
        }

        public ChildrenAccessor Children => new ChildrenAccessor(this);
    }

    // { Engine.TEngine (instance size 52): +$C FMsgCount, +$10 FEClasses, +$14 FClassTable,
//     +$18 FEditors, +$1C FControlNode, +$20 FComponent (the owning component), +$24 FDebugIDs,
//     +$28 FDebugEvents, +$2C FStartupDirectory, +$31 FOptions. Every engine is in the Engines list. }
    public partial class TEngine : Aow.Units.Engine.TENode
    {
        internal int FMsgCount;
        internal Aow.Units.Engine.TEObjectClassList FEClasses;
        internal Aow.Units.Engine.TIntegerHashTable256 FClassTable;
        internal Aow.Units.Engine.TEObjectEditorList FEditors;
        internal Aow.Units.Engine.TEControlStyleNode FControlNode;
        public Aow.Units.Engine.TEComponent FComponent;
        // { $20; AoWEd 00404501 }
        internal Aow.Rtl.TStringList FDebugIDs;
        internal Aow.Units.Engine.TEventList FDebugEvents;
        internal Aow.Rtl.AnsiString FStartupDirectory;
        public TSet<Aow.Units.Engine.TEngineUserElement> FUser;
        // { $30; 5578A15C }
        internal TSet<Aow.Units.Engine.TEngineModeElement> FOptions;
        // Engine.pas:8776
        internal int CreateUniqueDebugID()
        {
            int Result = default;
            Result = 0;
            while (true)
            {
                var call__127 = FDebugIDs.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)Result));
                if (!(call__127 != -1))
                    break;
                {
                    Result += 1;
                }
            }

            return Result;
        }

        // Engine.pas:8814
        public override Aow.Units.Engine.TEngine GetEngine()
        {
            Aow.Units.Engine.TEngine Result = default;
            Result = this;
            return Result;
        }

        // Engine.pas:8728
        // { TEngine }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            UpdateStartupDirectory();
            FOptions = TSet<Aow.Units.Engine.TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
            var call__128 = Aow.Units.Engine.TEventList.Create();
            FDebugEvents = call__128;
            var call__129 = Aow.Units.Engine.TEObjectClassList.Create();
            FEClasses = call__129;
            var call__130 = Aow.Units.Engine.TIntegerHashTable256.Create();
            FClassTable = call__130;
            var call__131 = Aow.Units.Engine.TEObjectEditorList.Create();
            FEditors = call__131;
            var call__132 = Aow.Units.Engine.TEControlStyleNode.Create(this);
            FControlNode = call__132;
            FControlNode.SetControlStyle(TSet<Aow.Units.Engine.TEControlStyleElement>.Of(Aow.Units.Engine.Engine.csShared));
            var call__133 = new Aow.Rtl.TStringList();
            FDebugIDs = call__133;
            FDebugIDs.AddObject(Aow.Units.Engine.Engine.UserStr, Aow.Rtl.Rtl.IntToObject((int)-1));
            RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Engine.TEControlStyleObject>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Engine.TEControlStyleNode>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Engine.TECustomOwner>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Engine.TEResourceEditSettings>()]);
            Aow.Units.Engine.Engine.Engines.Add(this);
        }

        // Engine.pas:8745
        public override void Destroy()
        {
            FEditors.Free();
            Aow.Units.Engine.Engine.Engines.Remove(this);
            FDebugEvents.Free();
            FDebugIDs.Free();
            base.Destroy();
            FClassTable.Free();
            FEClasses.Free();
        }

        // Engine.pas:8771
        public virtual void SetStartupDirectory(Aow.Rtl.AnsiString Value)
        {
            FStartupDirectory = Value;
        }

        // Engine.pas:8766
        public virtual void UpdateStartupDirectory()
        {
            var call__134 = Aow.Rtl.Rtl.Application.ExeName;
            var call__135 = Aow.Rtl.Rtl.ExtractFilePath(call__134);
            FStartupDirectory = call__135;
        }

        // Engine.pas:8887
        public virtual Aow.Units.Engine.TEObject CreateEObject(int AClassID, Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEObject Result = default;
            Aow.Units.Engine.TEObject.Meta C = default;
            C = GetEClass(AClassID);
            Result = C.Create(AOwner);
            return Result;
        }

        // Engine.pas:8860
        public virtual Aow.Units.Engine.TEObject.Meta GetEClass(int AClassID)
        {
            Aow.Units.Engine.TEObject.Meta Result = default;
            object P = default;
            var call__136 = FClassTable.Find(AClassID);
            P = call__136;
            Result = (Aow.Units.Engine.TEObject.Meta)P;
            if (Result == null)
            {
                throw Aow.Rtl.Exception.Create((((AnsiString)"Unregistered E-ClassID found (") + Aow.Rtl.Rtl.IntToStr(AClassID)) + AnsiString.FromBytes(41));
            }

            return Result;
        }

        // Engine.pas:8761
        public virtual Aow.Rtl.AnsiString TranslateText(Aow.Rtl.AnsiString Text)
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Text;
            return Result;
        }

        // Engine.pas:8782
        public int RegisterDebugID(Aow.Rtl.AnsiString Name)
        {
            int Result = default;
            Result = CreateUniqueDebugID();
            FDebugIDs.AddObject(Name, Aow.Rtl.Rtl.IntToObject((int)Result));
            return Result;
        }

        // Engine.pas:8788
        public void UnregisterDebugID(int ID)
        {
            int I = default;
            var call__137 = FDebugIDs.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)ID));
            I = call__137;
            if (I != -1)
            {
                FDebugIDs.Delete(I);
            }
        }

        // Engine.pas:8796
        public void OutputDebugStr(Aow.Rtl.AnsiString S)
        {
            Aow.Units.Engine.TEngineDebugInfo[] Info__cell = new Aow.Units.Engine.TEngineDebugInfo[1];
            ref Aow.Units.Engine.TEngineDebugInfo Info = ref Info__cell[0];
            Info.ID = -1;
            Info.Text = S;
            FDebugEvents.TriggerNotifyEvents(Ptr<Aow.Units.Engine.TEngineDebugInfo>.To(Info__cell, 0).Box());
        }

        // Engine.pas:8805
        public void OutputDebugMessage(int ID, Aow.Rtl.AnsiString S)
        {
            Aow.Units.Engine.TEngineDebugInfo[] Info__cell = new Aow.Units.Engine.TEngineDebugInfo[1];
            ref Aow.Units.Engine.TEngineDebugInfo Info = ref Info__cell[0];
            Info.ID = ID;
            Info.Text = S;
            FDebugEvents.TriggerNotifyEvents(Ptr<Aow.Units.Engine.TEngineDebugInfo>.To(Info__cell, 0).Box());
        }

        // Engine.pas:8819
        public void ResetMsgCount()
        {
            Aow.Units.Engine.TEMsg Msg = default;
            Aow.Units.Engine.Engine.InitMsgEx(ref Msg, 1, this, FMsgCount + 1);
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
        public void RegisterEClasses(ReadOnlySpan<Aow.Units.Engine.TEObject.Meta> AClasses)
        {
            int I = default;
            {
                I = 0;
                int end__138 = AClasses.Length - 1;
                for (; I <= end__138; I += 1)
                {
                    if (FOptions.Contains(Aow.Units.Engine.Engine.eoVerifyClasses))
                    {
                        VerifyEObjectClass(AClasses[I]);
                    }

                    var call__139 = AClasses[I].ClassID();
                    var call__140 = FClassTable.Find(call__139);
                    if (call__140 != null)
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"EObject Class ID double defined!");
                    }

                    FEClasses.Add(AClasses[I]);
                    var call__141 = AClasses[I].ClassID();
                    FClassTable.Add(AClasses[I], call__141);
                }
            }
        }

        // Engine.pas:8849
        public void UnregisterEClasses(ReadOnlySpan<Aow.Units.Engine.TEObject.Meta> AClasses)
        {
            int I = default;
            {
                I = 0;
                int end__142 = AClasses.Length - 1;
                for (; I <= end__142; I += 1)
                {
                    FEClasses.Remove(AClasses[I]);
                    var call__143 = AClasses[I].ClassID();
                    FClassTable.Delete(call__143);
                }
            }
        }

        // Engine.pas:8867
        public Aow.Units.Engine.TEObjectClassList GetDescendantClasses(System.Type AClass)
        {
            Aow.Units.Engine.TEObjectClassList Result = default;
            int I = default;
            Aow.Units.Engine.TEObjectClassList L = default;
            L = Aow.Units.Engine.TEObjectClassList.Create();
            {
                I = 0;
                int end__144 = FEClasses.Count - 1;
                for (; I <= end__144; I += 1)
                {
                    var call__145 = FEClasses[I];
                    var call__146 = ((Aow.Units.Engine.TEObject.Meta)call__145).InheritsFrom(AClass);
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
        public void ActivateEObject(Aow.Units.Engine.TEObject Obj)
        {
            Aow.Units.Engine.TEMsg Msg = default;
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 536870920, this);
            Msg.Engine = this;
            Obj.MsgProc(ref Msg, this);
        }

        // Engine.pas:8895
        public void WriteEObject(Aow.Rtl.TStream Stream, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            S = Aow.Units.Engine.TEWriteStorageStream.Create(Stream, this, false);
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
        public void ReadEObject(Aow.Rtl.TStream Stream, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            S = Aow.Units.Engine.TEReadStorageStream.Create(Stream, this, false);
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
        public void WritePreviewEObject(Aow.Rtl.TStream Stream, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            S = Aow.Units.Engine.TEWriteStorageStream.Create(Stream, this, true);
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
        public void ReadPreviewEObject(Aow.Rtl.TStream Stream, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            S = Aow.Units.Engine.TEReadStorageStream.Create(Stream, this, true);
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
        public void StoreEObject(Aow.Rtl.TStream Stream, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            S = Aow.Units.Engine.TEWriteStorageStream.Create(Stream, this, false);
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
        public Aow.Units.Engine.TEObject RetrieveEObject(Aow.Rtl.TStream Stream, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEObject Result = default;
            Aow.Units.Engine.TEStorageStream S = default;
            S = Aow.Units.Engine.TEReadStorageStream.Create(Stream, this, false);
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
        public Aow.Units.Engine.TEObject RetrieveFromFile(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Units.Engine.TEObject Result = default;
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 0 | 32);
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
        public void StoreInFile(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 65535);
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
        public void WriteToFile(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Rtl.TMemoryStream M = default;
            Aow.Rtl.TFileStream F = default;
            M = new Aow.Rtl.TMemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                F = new Aow.Rtl.TFileStream(FileName, 65535);
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
        public void ReadFromFile(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            Aow.Rtl.TMemoryStream M = default;
            Aow.Rtl.TFileStream F = default;
            M = new Aow.Rtl.TMemoryStream();
            F = new Aow.Rtl.TFileStream(FileName, 0 | 32);
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
        public void WriteToFileCRC(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            int CRC1 = default;
            int CRC2 = default;
            Aow.Rtl.TMemoryStream M = default;
            Aow.Rtl.TFileStream F = default;
            M = new Aow.Rtl.TMemoryStream();
            try
            {
                WriteEObject(M, Obj, AProgress);
                var call__149 = M.Size;
                var call__150 = Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory, 0), call__149);
                CRC1 = call__150;
                M.Write(Aow.Rtl.Bytes.Of(ref CRC1, 4));
                var call__151 = M.Size;
                var call__152 = Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory, 0), call__151);
                CRC2 = call__152;
                F = new Aow.Rtl.TFileStream(FileName, 65535);
                try
                {
                    F.Write(Aow.Rtl.Bytes.Of(ref CRC2, 4));
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
        public bool ReadFromFileCRC(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TProgressEvent AProgress)
        {
            bool Result = default;
            Aow.Rtl.TMemoryStream M = default;
            Aow.Rtl.TFileStream F = default;
            int CRC2 = default;
            int CRC1 = default;
            Result = false;
            M = new Aow.Rtl.TMemoryStream();
            F = new Aow.Rtl.TFileStream(FileName, 0 | 32);
            try
            {
                M.LoadFromStream(F);
                M.Position = 0;
                var call__154 = M.Size;
                if (call__154 > 8)
                {
                    CRC1 = MemoryMarshal.Read<int>(Ptr<byte>.To(M.Memory, 0).AsSpan(4));
                    var call__155 = M.Size;
                    CRC2 = MemoryMarshal.Read<int>(((Ptr<byte>.To(M.Memory, 0) + call__155) - 4).AsSpan(4));
                    var call__156 = M.Size;
                    var call__157 = Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory, 0) + 4, call__156 - 4);
                    bool bool__158 = (CRC1 == call__157);
                    if (bool__158)
                    {
                        var call__159 = M.Size;
                        var call__160 = Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory, 0) + 4, call__159 - 8);
                        bool__158 = (CRC2 == call__160);
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
        public void ReadTextFromFile(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 0 | 32);
            try
            {
                S = Aow.Units.Engine.TEReadTextStorageStream.Create(F, this, 0);
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
        public void WriteTextToFile(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TEObject Obj)
        {
            Aow.Units.Engine.TEStorageStream S = default;
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 65535);
            try
            {
                S = Aow.Units.Engine.TEWriteTextStorageStream.Create(F, this);
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

        public Aow.Units.Engine.TEObjectClassList EClasses { get => FEClasses; }
        public Aow.Units.Engine.TIntegerHashTable256 ClassTable { get => FClassTable; }
        public Aow.Units.Engine.TEObjectEditorList Editors { get => FEditors; }
        public Aow.Units.Engine.TEControlStyleNode ControlNode { get => FControlNode; }
        public Aow.Rtl.TStringList DebugIDs { get => FDebugIDs; }
        public Aow.Units.Engine.TEventList DebugEvents { get => FDebugEvents; }
        public Aow.Rtl.AnsiString StartupDirectory { get => FStartupDirectory; set => SetStartupDirectory(value); }
        public TSet<Aow.Units.Engine.TEngineModeElement> Options { get => FOptions; set => FOptions = value; }

        // Engine.pas:1126
        public static new Aow.Units.Engine.TEngine Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEngine o = new Aow.Units.Engine.TEngine();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TENode.Meta
        {
            // Engine.pas:8756
            public override int ClassID()
            {
                int Result = default;
                Result = 17;
                return Result;
            }

            // Engine.pas:1126
            public new Aow.Units.Engine.TEngine Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEngine o = (Aow.Units.Engine.TEngine)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEngine => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEStorageStream (instance size 36): the base of every storage stream. The rw* methods do
//     nothing here; the subclasses read, write, compare or translate. }
    public partial class TEStorageStream : Aow.Rtl.TObject
    {
        internal Aow.Units.Engine.TEngine FEngine;
        internal Aow.Rtl.TStream FStream;
        internal bool FErrorHandled;
        internal Aow.Units.Engine.TPropertyTable FPropertyTable;
        internal Aow.Units.Engine.TStorageStreamError FOnError;
        internal Aow.Units.Engine.TProgressEvent FOnProgress;
        // Engine.pas:1184
        public virtual TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:9502
        public virtual Aow.Rtl.TStream OpenPropertyStream(int ID)
        {
            Aow.Rtl.TStream Result = default;
            Result = null;
            return Result;
        }

        // Engine.pas:1186
        public virtual void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:1187
        public virtual void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:1188
        public virtual void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:1189
        public virtual void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:9559
        public virtual void rwStreamProc(int ID, Aow.Rtl.TStreamProc LoadProc, Aow.Rtl.TStreamProc SaveProc)
        {
        }

        // Engine.pas:9507
        public virtual void rwData(Span<byte> Buffer, int Size, int ID)
        {
        }

        // Engine.pas:9511
        public virtual void rwString(ref Aow.Rtl.AnsiString S, int ID)
        {
        }

        // Engine.pas:9515
        public virtual void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
        }

        // Engine.pas:9519
        public virtual void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
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
        // { TEStorageStream }
        public void Create__init_TEStorageStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            ;
            FEngine = AEngine;
            FStream = AStream;
            FErrorHandled = false;
        }

        // Engine.pas:9497
        public void ShowProgress(Aow.Units.Engine.TEObject Sender, Aow.Rtl.AnsiString Msg, int Percent)
        {
            if (FOnProgress != null)
            {
                FOnProgress(Sender, Msg, Percent);
            }
        }

        public Aow.Units.Engine.TEngine Engine { get => FEngine; }
        public Aow.Rtl.TStream Stream { get => FStream; }
        public Aow.Units.Engine.TPropertyTable PropertyTable { get => FPropertyTable; }
        public Aow.Units.Engine.TStorageStreamError OnError { get => FOnError; set => FOnError = value; }
        public Aow.Units.Engine.TProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

        // Engine.pas:1204
        public static Aow.Units.Engine.TEStorageStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.Engine.TEStorageStream o = new Aow.Units.Engine.TEStorageStream();
            try
            {
                o.Create__init_TEStorageStream(AStream, AEngine);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEObjectEditorList (instance size 16) }
    public partial class TEObjectEditorList : Aow.Units.Engine.TEObjectClassList
    {
        // Engine.pas:8356
        // { TEObjectEditorList }
        public void RegisterClass(Aow.Units.Engine.TEObjectEditor.Meta AClass)
        {
            var call__161 = AClass.EditorClassID();
            var call__162 = FindEditor(call__161);
            if (call__162 == null)
            {
                Add(AClass);
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"EObjectEditor Double Defined!");
            }
        }

        // Engine.pas:8364
        public Aow.Units.Engine.TEObjectEditor.Meta FindEditor(int AClassID)
        {
            Aow.Units.Engine.TEObjectEditor.Meta Result = default;
            int I = default;
            I = 0;
            while (true)
            {
                bool bool__163 = (I != Count);
                if (bool__163)
                {
                    var call__164 = ((Aow.Units.Engine.TEObjectEditor.Meta)List[I]).EditorClassID();
                    bool__163 = (AClassID != call__164);
                }

                if (!bool__163)
                    break;
                {
                    I += 1;
                }
            }

            if (I != Count)
            {
                Result = (Aow.Units.Engine.TEObjectEditor.Meta)List[I];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // System.pas:111
        public static new Aow.Units.Engine.TEObjectEditorList Create()
        {
            return new Aow.Units.Engine.TEObjectEditorList();
        }
    }

    // { Engine.TEResource (instance size 24): +8 FResourceList, +$C FResourceID, +$10 FEditID (the
//     edit id of the resource grid that placed it), +$14 FEditIndex (its cell in that grid); all three
//     integers are persisted (ids 1..3) }
    public partial class TEResource : Aow.Units.Engine.TEObject
    {
        internal Aow.Units.Engine.TECustomNode FResourceList;
        internal int FResourceID;
        internal int FEditID;
        internal int FEditIndex;
        // Engine.pas:8513
        public override void SetOwner(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.SetOwner(AOwner);
            FResourceList = AOwner;
        }

        // Engine.pas:8542
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FResourceID, 1);
            Stream.rwInteger(ref FEditID, 2);
            Stream.rwInteger(ref FEditIndex, 3);
        }

        // Engine.pas:8519
        public override void SetArrayOwner(Aow.Units.Engine.TECustomNode AOwner, int Index)
        {
            base.SetArrayOwner(AOwner, Index);
            FResourceList = AOwner;
        }

        // Engine.pas:8533
        public virtual Aow.Units.ImageLib.TLibraryImage GetCellImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Result = null;
            return Result;
        }

        // Engine.pas:8538
        public virtual void SetCellImage(Aow.Units.ImageLib.TLibraryImage Value)
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
        public virtual Aow.Units.Engine.TEObject CreateResourceUser()
        {
            Aow.Units.Engine.TEObject Result = default;
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
        public virtual void Draw(Aow.Rtl.TCanvas Canvas, Aow.Rtl.TRect Rect, Aow.Units.ImageLib.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
        {
            Aow.Rtl.TRect R = default;
            var call__167 = GetCellImage();
            Aow.Units.ImageLib.ImageLib.BltLibraryImage(Canvas, call__167, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref R);
        }

        public Aow.Units.Engine.TECustomNode ResourceList { get => FResourceList; set => FResourceList = value; }
        public int EditID { get => FEditID; set => FEditID = value; }
        public int EditIndex { get => FEditIndex; set => FEditIndex = value; }
        public int ResourceID { get => FResourceID; set => FResourceID = value; }
        public Aow.Units.ImageLib.TLibraryImage CellImage { get => GetCellImage(); set => SetCellImage(value); }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:8503
            // { TEResource }
            public override int ClassID()
            {
                int Result = default;
                Result = 1024;
                return Result;
            }

            // Engine.pas:8508
            public virtual Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)this.ClassName;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEResource => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.Engine.TEResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEResource o = (Aow.Units.Engine.TEResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    public struct TEMsg
    {
        public int MsgID;
        public int Seq;
        public Aow.Units.Engine.TEObject Sender;
        public Aow.Units.FixedArray_IntOrRef_3 Param;
        public IntOrRef Payload;
        public Aow.Units.Engine.TEngine Engine { get => Payload.As<Aow.Units.Engine.TEngine>(); set => Payload = value; }

        public Aow.Units.FixedArray_int_4 Data;
        public int Result;
        public Aow.Units.Engine.TEObject Obj { get => Payload.As<Aow.Units.Engine.TEObject>(); set => Payload = value; }
        public int Value { get => Payload.Value; set => Payload = value; }
    }

    public delegate void TStorageStreamError(Aow.Units.Engine.TEStorageStream StorageStream, Aow.Rtl.AnsiString Message, ref bool Handled);
    public delegate void TProgressEvent(Aow.Units.Engine.TEObject Sender, Aow.Rtl.AnsiString Message, int Progression);
    public partial class EDoublePrioriyID : Aow.Rtl.Exception
    {
        // SysUtils.pas:174
        public static new Aow.Units.Engine.EDoublePrioriyID Create(Aow.Rtl.AnsiString Msg)
        {
            return new Aow.Units.Engine.EDoublePrioriyID
            {
                Message = Msg
            };
        }
    }

    // { Engine.TEBufferStream (instance size 20): the write stream's data buffer }
    public partial class TEBufferStream : Aow.Units.FMemStr.TFastMemoryStream
    {
        // System.pas:111
        public static Aow.Units.Engine.TEBufferStream Create()
        {
            return new Aow.Units.Engine.TEBufferStream();
        }
    }

    public enum TCompressLevel : byte
    {
        cpNone = 0,
        cpFastest = 1,
        cpDefault = 2,
        cpMax = 3
    }

    public partial class TCompressStream : Aow.Rtl.TMemoryStream
    {
        // Engine.pas:1715
        public new void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            bool Compressed = default;
            Aow.Units.FixedArray_byte_4 Magic = default;
            Aow.Rtl.TDecompressionStream D = default;
            Ptr<byte> Buffer = default;
            int Count = default;
            Stream.Read(((Span<byte>)Magic).Slice(0, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Compressed, 1));
            if (!Aow.Rtl.Rtl.ArrayEquals<Aow.Units.FixedArray_byte_4, byte>(Magic, Aow.Units.Engine.Engine.CompressMagic))
            {
                Compressed = false;
                Stream.Seek(0, 0);
            }

            if (Compressed)
            {
                Buffer = default;
                Count = -1;
                Buffer = Buffer.Realloc(8096);
                D = new Aow.Rtl.TDecompressionStream(Stream);
                try
                {
                    while (Count != 0)
                    {
                        var call__168 = D.Read(Buffer.AsSpan(8096));
                        Count = call__168;
                        if (Count != 0)
                        {
                            this.WriteBuffer(Buffer.AsSpan(Count));
                        }
                    }
                }
                finally
                {
                    Buffer = Buffer.Realloc(0);
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
        public int SaveToStreamEx(Aow.Rtl.TStream Stream, Aow.Units.Engine.TCompressLevel Level)
        {
            int Result = default;
            Aow.Rtl.TCompressionStream C = default;
            int StartSize = default;
            Aow.Rtl.TCompressionLevel CompressionLevel = default;
            Position = 0;
            Result = 100;
            var call__171 = Stream.Size;
            StartSize = call__171;
            Stream.Write(((ReadOnlySpan<byte>)Aow.Units.Engine.Engine.CompressMagic).Slice(0, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref Level, 1));
            if (Level != Aow.Units.Engine.TCompressLevel.cpNone)
            {
                CompressionLevel = (Aow.Rtl.TCompressionLevel)Level;
                C = new Aow.Rtl.TCompressionStream(CompressionLevel, Stream);
                try
                {
                    C.CopyFrom(this, 0);
                    var call__172 = Stream.Size;
                    var call__173 = Size;
                    Result = Aow.Rtl.Rtl.Round(((double)(((double)1) - (((double)(call__172 - StartSize)) / call__173))) * 100);
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
        public new void SaveToStream(Aow.Rtl.TStream Stream)
        {
            SaveToStreamEx(Stream, Aow.Units.Engine.TCompressLevel.cpDefault);
        }

        // Engine.pas:1781
        public int SaveToFileEx(Aow.Rtl.AnsiString FileName, Aow.Units.Engine.TCompressLevel Level)
        {
            int Result = default;
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 65535);
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
        public new void SaveToFile(Aow.Rtl.AnsiString FileName)
        {
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 65535);
            try
            {
                SaveToStreamEx(F, Aow.Units.Engine.TCompressLevel.cpDefault);
            }
            finally
            {
                F.Free();
            }
        }

        // Engine.pas:1805
        public new void LoadFromFile(Aow.Rtl.AnsiString FileName)
        {
            Aow.Rtl.TFileStream F = default;
            F = new Aow.Rtl.TFileStream(FileName, 0 | 32);
            try
            {
                LoadFromStream(F);
            }
            finally
            {
                F.Free();
            }
        }

        // System.pas:111
        public static Aow.Units.Engine.TCompressStream Create()
        {
            return new Aow.Units.Engine.TCompressStream();
        }
    }

    // { Engine.TPlugIn (instance size 16): +4 FRefCount, +8 FEngines, +$C FCounts. A plug-in registers
//     itself in PlugIns on creation and counts the engines it is initialized for. }
    public partial class TPlugIn : Aow.Rtl.TObject
    {
        internal int FRefCount;
        internal Aow.Rtl.TList FEngines;
        internal Aow.Units.Engine.TIntegerList FCounts;
        // Engine.pas:9144
        // { TPlugIn }
        public void Create__init_TPlugIn()
        {
            ;
            var call__174 = new Aow.Rtl.TList();
            FEngines = call__174;
            var call__175 = Aow.Units.Engine.TIntegerList.Create();
            FCounts = call__175;
            Aow.Units.Engine.Engine.RegisterPlugIn(this);
            FRefCount = 1;
        }

        // Engine.pas:9153
        public override void Destroy()
        {
            while (FEngines.Count != 0)
            {
                var call__176 = FEngines[FEngines.Count - 1];
                Finalize((Aow.Units.Engine.TEngine)call__176);
            }

            FEngines.Free();
            FCounts.Free();
            Aow.Units.Engine.Engine.UnregisterPlugIn(this);
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
        public virtual Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Abstract PlugIn";
            return Result;
        }

        // Engine.pas:9178
        public virtual bool Initialize(Aow.Units.Engine.TEngine AEngine)
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
        public virtual bool Finalize(Aow.Units.Engine.TEngine AEngine)
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
                    throw Aow.Rtl.Exception.Create((AnsiString)"Invalid PlugIn Finalize Usage");
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
        public static Aow.Units.Engine.TPlugIn Create()
        {
            Aow.Units.Engine.TPlugIn o = new Aow.Units.Engine.TPlugIn();
            try
            {
                o.Create__init_TPlugIn();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public partial class EPlugInMissing : Aow.Rtl.Exception
    {
        // SysUtils.pas:174
        public static new Aow.Units.Engine.EPlugInMissing Create(Aow.Rtl.AnsiString Msg)
        {
            return new Aow.Units.Engine.EPlugInMissing
            {
                Message = Msg
            };
        }
    }

    // { Engine.TPlugInControl (instance size 16): +8 FEngine, +$C FUsedPlugIns; the names of the
//     plug-ins an engine uses, initialized and finalized as the list changes }
    public partial class TPlugInControl : Aow.Units.Engine.TEObject
    {
        internal Aow.Units.Engine.TEngine FEngine;
        internal Aow.Rtl.TStringList FUsedPlugIns;
        // Engine.pas:9279
        internal void SetUsedPlugIns(Aow.Rtl.TStrings Value)
        {
            FUsedPlugIns.Assign(Value);
        }

        // Engine.pas:9284
        internal Aow.Rtl.TStrings GetUsedPlugIns()
        {
            Aow.Rtl.TStrings Result = default;
            Result = FUsedPlugIns;
            return Result;
        }

        // Engine.pas:9289
        internal void OnChange(Aow.Rtl.TObject Sender)
        {
            InitializePlugIns();
        }

        // Engine.pas:9294
        internal void OnChanging(Aow.Rtl.TObject Sender)
        {
            FinalizePlugIns();
        }

        // Engine.pas:9299
        internal void InitializePlugIns()
        {
            int[] I__cell = new int[1];
            ref int I = ref I__cell[0];
            object IndexAddress = default;
            Aow.Units.Engine.TPlugIn P = default;
            ;
            {
                I = 0;
                int end__183 = FUsedPlugIns.Count - 1;
                for (; I <= end__183; I += 1)
                {
                    var call__184 = FUsedPlugIns[I];
                    var call__185 = Aow.Units.Engine.Engine.FindPlugIn(call__184);
                    P = call__185;
                    if (P != null)
                    {
                        P.Initialize(FEngine);
                    }
                    else
                    {
                        var call__186 = FUsedPlugIns[I];
                        var call__187 = Aow.Units.Engine.EPlugInMissing.Create((((AnsiString)"PlugIn Missing (") + call__186) + AnsiString.FromBytes(41));
                        throw call__187;
                    }
                }
            }
        }

        // Engine.pas:9319
        internal void FinalizePlugIns()
        {
            int I = default;
            Aow.Units.Engine.TPlugIn P = default;
            {
                I = 0;
                int end__188 = FUsedPlugIns.Count - 1;
                for (; I <= end__188; I += 1)
                {
                    var call__189 = FUsedPlugIns[I];
                    var call__190 = Aow.Units.Engine.Engine.FindPlugIn(call__189);
                    P = call__190;
                    if (P != null)
                    {
                        P.Finalize(FEngine);
                    }
                }
            }
        }

        // Engine.pas:9252
        // { TPlugInControl }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__191 = new Aow.Rtl.TStringList();
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
        public override Aow.Units.Engine.TEngine GetEngine()
        {
            Aow.Units.Engine.TEngine Result = default;
            Result = FEngine;
            return Result;
        }

        // Engine.pas:9274
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = TSet<Aow.Units.Engine.TEControlStyleElement>.Of(Aow.Units.Engine.Engine.csStored, Aow.Units.Engine.Engine.csShared);
            return Result;
        }

        // Engine.pas:9331
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smTranslate))
            {
                Stream.rwStrings(FUsedPlugIns, 1);
            }
        }

        public Aow.Units.Engine.TEngine Engine { get => FEngine; set => FEngine = value; }
        public Aow.Rtl.TStrings UsedPlugIns { get => GetUsedPlugIns(); set => SetUsedPlugIns(value); }

        // Engine.pas:575
        public static new Aow.Units.Engine.TPlugInControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TPlugInControl o = new Aow.Units.Engine.TPlugInControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEObjectList (instance size 16): the child list of a TENode }
    public partial class TEObjectList : Aow.Rtl.TList
    {
        // Engine.pas:7136
        // { TEObjectList }
        protected internal Aow.Units.Engine.TEObject GetEObject(int Index)
        {
            Aow.Units.Engine.TEObject Result = default;
            var call__192 = this[Index];
            Result = (Aow.Units.Engine.TEObject)call__192;
            return Result;
        }

        // Engine.pas:7141
        protected internal void SetEObject(int Index, Aow.Units.Engine.TEObject Value)
        {
            this[Index] = Value;
        }

        // Engine.pas:7146
        public Aow.Units.Engine.TEObject FindObject(int AClassID)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            bool Found = default;
            I = 0;
            Found = false;
            while (!Found && (I < Count))
            {
                bool bool__193 = (List[I] != null);
                if (bool__193)
                {
                    var call__194 = ((Aow.Units.Engine.TEObject)List[I]).ClassType.ClassID();
                    bool__193 = (AClassID == call__194);
                }

                Found = bool__193;
                I += 1;
            }

            if (Found)
            {
                Result = (Aow.Units.Engine.TEObject)List[I - 1];
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
            while (!Found && (I < Count))
            {
                bool bool__195 = (List[I] != null);
                if (bool__195)
                {
                    var call__196 = ((Aow.Units.Engine.TEObject)List[I]).ClassType.ClassID();
                    bool__195 = (AClassID == call__196);
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
                        ((Aow.Units.Engine.TEObject)List[I]).Destroy();
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
                bool bool__198 = (List[I] == null);
                if (!bool__198)
                {
                    var call__199 = ((Aow.Units.Engine.TEObject)List[I]).ClassType.ClassID();
                    bool__198 = (AClassID == call__199);
                }

                OK = bool__198;
                I += 1;
            }

            Result = OK && (Count != 0);
            return Result;
        }

        public sealed class EObjectsAccessor(Aow.Units.Engine.TEObjectList target)
        {
            public Aow.Units.Engine.TEObject this[int Index] { get => target.GetEObject(Index); set => target.SetEObject(Index, value); }
        }

        public EObjectsAccessor EObjects => new EObjectsAccessor(this);

        // System.pas:111
        public static Aow.Units.Engine.TEObjectList Create()
        {
            return new Aow.Units.Engine.TEObjectList();
        }
    }

    // { Engine.TENode (instance size 12): +8 FChildren }
    public partial class TENode : Aow.Units.Engine.TECustomNode
    {
        public Aow.Units.Engine.TEObjectList FChildren;
        // Engine.pas:2807
        // { TENode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__200 = Aow.Units.Engine.TEObjectList.Create();
            FChildren = call__200;
        }

        // Engine.pas:2833
        public override void Clear()
        {
            int I = default;
            Aow.Units.Engine.TEObject C = default;
            {
                I = Count - 1;
                byte end__201 = 0;
                for (; I >= end__201; I -= 1)
                {
                    C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                    if ((C != null) && (C.Owner == this))
                    {
                        if (!C.GetControlStyle().Contains(Aow.Units.Engine.Engine.csShared))
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
                        C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                        if (C != null)
                        {
                            bool bool__204 = (C.Owner == this);
                            if (bool__204)
                            {
                                var call__205 = C.GetControlStyle();
                                bool__204 = call__205.Contains(Aow.Units.Engine.Engine.csShared);
                            }

                            if (!bool__204)
                            {
                                RemoveChild(C);
                            }
                        }
                    }
                }

                I = FChildren.Count - 1;
                while ((I >= 0) && (FChildren.List[I] == null))
                {
                    FChildren.Delete(I);
                    I -= 1;
                }
            }
        }

        // Engine.pas:2866
        public override Aow.Units.Engine.TEObject GetChild(int Index)
        {
            Aow.Units.Engine.TEObject Result = default;
            Result = (Aow.Units.Engine.TEObject)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:2871
        public override void SetChild(int Index, Aow.Units.Engine.TEObject Child)
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

            if ((FChildren.List[Index] != null) && (((Aow.Units.Engine.TEObject)FChildren.List[Index]).Owner == this))
            {
                ((Aow.Units.Engine.TEObject)FChildren.List[Index]).Destroy();
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
            Aow.Units.Engine.TEObject C = default;
            Clear();
            if (FChildren.Count != 0)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__207 = 0;
                    for (; I >= end__207; I -= 1)
                    {
                        C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                        if ((C != null) && (C.Owner == this))
                        {
                            C.Destroy();
                        }
                    }
                }
            }
        }

        // Engine.pas:2884
        public override void SendMsg(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            int I = default;
            {
                var w__208 = FChildren;
                if (GetControlStyle().Contains(Aow.Units.Engine.Engine.csKeepIndex))
                {
                    if ((Msg.MsgID & 268435456) != 0)
                    {
                        if ((Msg.MsgID & 536870912) == 0)
                        {
                            {
                                I = w__208.Count - 1;
                                byte end__209 = 0;
                                for (; I >= end__209; I -= 1)
                                {
                                    if (w__208.List[I] != null)
                                    {
                                        {
                                            var w__210 = (Aow.Units.Engine.TEObject)w__208.List[I];
                                            var call__211 = w__210.GetLastMsg();
                                            if (call__211 != Msg.Seq)
                                            {
                                                w__210.SetLastMsg(Msg.Seq);
                                                w__210.MsgProc(ref Msg, this);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__208.Count - 1;
                                byte end__212 = 0;
                                for (; I >= end__212; I -= 1)
                                {
                                    if (w__208.List[I] != null)
                                    {
                                        {
                                            var w__213 = (Aow.Units.Engine.TEObject)w__208[I];
                                            var call__214 = w__213.GetLastMsg();
                                            if ((call__214 != Msg.Seq) && (w__213.Owner == this))
                                            {
                                                w__213.SetLastMsg(Msg.Seq);
                                                w__213.MsgProc(ref Msg, this);
                                            }
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
                                I = w__208.Count - 1;
                                byte end__215 = 0;
                                for (; I >= end__215; I -= 1)
                                {
                                    if (w__208.List[I] != null)
                                    {
                                        {
                                            var w__216 = (Aow.Units.Engine.TEObject)w__208.List[I];
                                            w__216.MsgProc(ref Msg, this);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__208.Count - 1;
                                byte end__217 = 0;
                                for (; I >= end__217; I -= 1)
                                {
                                    if (w__208.List[I] != null)
                                    {
                                        {
                                            var w__218 = (Aow.Units.Engine.TEObject)w__208[I];
                                            if (w__218.Owner == this)
                                            {
                                                w__218.MsgProc(ref Msg, this);
                                            }
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
                                I = w__208.Count - 1;
                                byte end__219 = 0;
                                for (; I >= end__219; I -= 1)
                                {
                                    {
                                        var w__220 = (Aow.Units.Engine.TEObject)w__208.List[I];
                                        var call__221 = w__220.GetLastMsg();
                                        if (call__221 != Msg.Seq)
                                        {
                                            w__220.SetLastMsg(Msg.Seq);
                                            w__220.MsgProc(ref Msg, this);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__208.Count - 1;
                                byte end__222 = 0;
                                for (; I >= end__222; I -= 1)
                                {
                                    {
                                        var w__223 = (Aow.Units.Engine.TEObject)w__208[I];
                                        var call__224 = w__223.GetLastMsg();
                                        if ((call__224 != Msg.Seq) && (w__223.Owner == this))
                                        {
                                            w__223.SetLastMsg(Msg.Seq);
                                            w__223.MsgProc(ref Msg, this);
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
                                I = w__208.Count - 1;
                                byte end__225 = 0;
                                for (; I >= end__225; I -= 1)
                                {
                                    {
                                        var w__226 = (Aow.Units.Engine.TEObject)w__208.List[I];
                                        w__226.MsgProc(ref Msg, this);
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__208.Count - 1;
                                byte end__227 = 0;
                                for (; I >= end__227; I -= 1)
                                {
                                    {
                                        var w__228 = (Aow.Units.Engine.TEObject)w__208[I];
                                        if (w__228.Owner == this)
                                        {
                                            w__228.MsgProc(ref Msg, this);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:2981
        public override int RemoveChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            if (GetControlStyle().Contains(Aow.Units.Engine.Engine.csKeepIndex))
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
        public override int AddChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            FChildren.Add(Child);
            Result = FChildren.Count - 1;
            return Result;
        }

        // Engine.pas:2998
        public override Aow.Units.Engine.TEObject FindChild(int AClassID)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            bool Found = default;
            Aow.Units.Engine.TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (!Found && (I < FChildren.Count))
            {
                C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                Found = (C != null) && (AClassID == C.ClassType.ClassID());
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
        public override int FindChildIndex(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            var call__231 = Count;
            Result = call__231 - 1;
            while ((Result >= 0) && (Child != ((Aow.Units.Engine.TEObject)FChildren.List[Result])))
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
            Aow.Units.Engine.TEObject C = default;
            {
                I = FChildren.Count - 1;
                byte end__232 = 0;
                for (; I >= end__232; I -= 1)
                {
                    C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                    bool bool__233 = (C != null);
                    if (bool__233)
                    {
                        var call__234 = C.ClassType.ClassID();
                        bool__233 = (AClassID == call__234);
                    }

                    if (bool__233 && (C.Owner == this))
                    {
                        C.Destroy();
                    }
                }
            }
        }

        // Engine.pas:631
        public static new Aow.Units.Engine.TENode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TENode o = new Aow.Units.Engine.TENode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:2861
            public override int ClassID()
            {
                int Result = default;
                Result = 256;
                return Result;
            }

            // Engine.pas:631
            public new Aow.Units.Engine.TENode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TENode o = (Aow.Units.Engine.TENode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TENode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TETimerNode (instance size 56): +8 FTick, +$C FCount, +$10 FTimerLists[1..10]. Children
//     sit in one of ten lists and get TimerEvent every tick, every second tick, ... every tenth. }
    public partial class TETimerNode : Aow.Units.Engine.TECustomNode
    {
        internal int FTick;
        internal int FCount;
        internal Aow.Units.FixedArray_Aow_Rtl_TList_10 FTimerLists;
        // Engine.pas:1856
        internal int FindTimerList(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            int I = default;
            Result = -1;
            I = 0;
            while ((Result == -1) && (I < 10))
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
        // { TETimerNode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            int I = default;
            base.Create__init(AOwner);
            FCount = 0;
            {
                I = 1;
                byte end__236 = 10;
                for (; I <= end__236; I += 1)
                {
                    var call__237 = new Aow.Rtl.TList();
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
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = TSet<Aow.Units.Engine.TEControlStyleElement>.Of(Aow.Units.Engine.Engine.csStored, Aow.Units.Engine.Engine.csShared, Aow.Units.Engine.Engine.csReserved2);
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
        public Aow.Rtl.TList GetTimerList(sbyte Timer)
        {
            Aow.Rtl.TList Result = default;
            Result = FTimerLists[Timer - 1];
            return Result;
        }

        // Engine.pas:1871
        public override int RemoveChild(Aow.Units.Engine.TEObject Child)
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
        public override int AddChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            var call__240 = FTimerLists[1 - 1].Add(Child);
            Result = call__240;
            FCount += 1;
            return Result;
        }

        // Engine.pas:1891
        public override Aow.Units.Engine.TEObject GetChild(int Index)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            int Total = default;
            Aow.Rtl.TList L = default;
            I = 1;
            L = FTimerLists[1 - 1];
            Total = L.Count;
            while ((Index >= Total) && (I < 10))
            {
                I += 1;
                L = FTimerLists[I - 1];
                Total += L.Count;
            }

            L = FTimerLists[I - 1];
            Total -= L.Count;
            Result = (Aow.Units.Engine.TEObject)L.List[Index - Total];
            return Result;
        }

        // Engine.pas:1910
        public override void SetChild(int Index, Aow.Units.Engine.TEObject Child)
        {
            throw Aow.Rtl.Exception.Create((AnsiString)"Set Child not supported (TETimerNode)");
        }

        // Engine.pas:1915
        public int AddTimerChild(Aow.Units.Engine.TEObject Child, sbyte Timer)
        {
            int Result = default;
            var call__241 = FTimerLists[Timer - 1].Add(Child);
            Result = call__241;
            FCount += 1;
            return Result;
        }

        // Engine.pas:1921
        public int RemoveTimerChild(Aow.Units.Engine.TEObject Child, sbyte Timer)
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
                    ((Aow.Units.Engine.TEObject)FTimerLists[Timer - 1].List[I]).TimerEvent(FTick, this);
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

            if ((FTick % 3) == 0)
            {
                TriggerTimerListEvent(3);
            }

            if ((FTick & 3) == 2)
            {
                TriggerTimerListEvent(4);
            }

            if ((FTick % 5) == 0)
            {
                TriggerTimerListEvent(5);
            }

            if ((FTick % 6) == 0)
            {
                TriggerTimerListEvent(6);
            }

            if ((FTick % 7) == 0)
            {
                TriggerTimerListEvent(7);
            }

            if ((FTick & 7) == 4)
            {
                TriggerTimerListEvent(8);
            }

            if ((FTick % 9) == 0)
            {
                TriggerTimerListEvent(9);
            }

            if ((FTick % 10) == 0)
            {
                TriggerTimerListEvent(10);
            }
        }

        public int Tick { get => FTick; }

        // Engine.pas:656
        public static new Aow.Units.Engine.TETimerNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TETimerNode o = new Aow.Units.Engine.TETimerNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:1836
            public override int ClassID()
            {
                int Result = default;
                Result = 264;
                return Result;
            }

            // Engine.pas:656
            public new Aow.Units.Engine.TETimerNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TETimerNode o = (Aow.Units.Engine.TETimerNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TETimerNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEChangeNotifyNode (16 bytes): +8 FChildren, +$C FUpdateCount; Changed on child changes. }
    public partial class TEChangeNotifyNode : Aow.Units.Engine.TECustomNode
    {
        public Aow.Units.Engine.TEObjectList FChildren;
        internal int FUpdateCount;
        // Engine.pas:7205
        // { TEChangeNotifyNode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__244 = Aow.Units.Engine.TEObjectList.Create();
            FChildren = call__244;
            FUpdateCount = 0;
        }

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
            Aow.Units.Engine.TEObject C = default;
            {
                I = Count - 1;
                byte end__245 = 0;
                for (; I >= end__245; I -= 1)
                {
                    C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                    if ((C != null) && (C.Owner == this))
                    {
                        if (!C.GetControlStyle().Contains(Aow.Units.Engine.Engine.csShared))
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
                    C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                    if (C != null)
                    {
                        bool bool__247 = (C.Owner == this);
                        if (bool__247)
                        {
                            var call__248 = C.GetControlStyle();
                            bool__247 = call__248.Contains(Aow.Units.Engine.Engine.csShared);
                        }

                        if (!bool__247)
                        {
                            RemoveChild(C);
                        }
                    }
                }
            }

            I = FChildren.Count - 1;
            while ((I >= 0) && (FChildren.List[I] == null))
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
        public override Aow.Units.Engine.TEObject GetChild(int Index)
        {
            Aow.Units.Engine.TEObject Result = default;
            Result = (Aow.Units.Engine.TEObject)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:7256
        public override void SetChild(int Index, Aow.Units.Engine.TEObject Child)
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
        public override void Assign(Aow.Units.Engine.TEObject Source, Aow.Units.Engine.TEngine AEngine)
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
            Aow.Rtl.Rtl.Assert(FUpdateCount > 0, (AnsiString)"Invalid UpdateCount Usage", "D:\\git\\aow\\src\\delphi3\\Engine.pas", 7959);
            FUpdateCount -= 1;
            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // Engine.pas:7964
        public void Sort(Aow.Rtl.TListSortCompare Compare)
        {
            BeginUpdate();
            FChildren.Sort(Compare);
            EndUpdate();
        }

        // Engine.pas:7971
        public override void MainReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            if ((Stream.StorageMode() & TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smText, Aow.Units.Engine.TStorageModeElement.smCompare)) == TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead))
            {
                BeginUpdate();
            }

            base.MainReadWrite(Stream);
            if ((Stream.StorageMode() & TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smText, Aow.Units.Engine.TStorageModeElement.smCompare)) == TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead))
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
        public override void SendMsg(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            int I = default;
            {
                var w__250 = FChildren;
                if (GetControlStyle().Contains(Aow.Units.Engine.Engine.csKeepIndex))
                {
                    if ((Msg.MsgID & 268435456) != 0)
                    {
                        if ((Msg.MsgID & 536870912) == 0)
                        {
                            {
                                I = w__250.Count - 1;
                                byte end__251 = 0;
                                for (; I >= end__251; I -= 1)
                                {
                                    if (w__250.List[I] != null)
                                    {
                                        {
                                            var w__252 = (Aow.Units.Engine.TEObject)w__250.List[I];
                                            var call__253 = w__252.GetLastMsg();
                                            if (call__253 != Msg.Seq)
                                            {
                                                w__252.SetLastMsg(Msg.Seq);
                                                w__252.MsgProc(ref Msg, this);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__250.Count - 1;
                                byte end__254 = 0;
                                for (; I >= end__254; I -= 1)
                                {
                                    if (w__250.List[I] != null)
                                    {
                                        {
                                            var w__255 = (Aow.Units.Engine.TEObject)w__250[I];
                                            var call__256 = w__255.GetLastMsg();
                                            if ((call__256 != Msg.Seq) && (w__255.Owner == this))
                                            {
                                                w__255.SetLastMsg(Msg.Seq);
                                                w__255.MsgProc(ref Msg, this);
                                            }
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
                                I = w__250.Count - 1;
                                byte end__257 = 0;
                                for (; I >= end__257; I -= 1)
                                {
                                    if (w__250.List[I] != null)
                                    {
                                        {
                                            var w__258 = (Aow.Units.Engine.TEObject)w__250.List[I];
                                            w__258.MsgProc(ref Msg, this);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__250.Count - 1;
                                byte end__259 = 0;
                                for (; I >= end__259; I -= 1)
                                {
                                    if (w__250.List[I] != null)
                                    {
                                        {
                                            var w__260 = (Aow.Units.Engine.TEObject)w__250[I];
                                            if (w__260.Owner == this)
                                            {
                                                w__260.MsgProc(ref Msg, this);
                                            }
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
                                I = w__250.Count - 1;
                                byte end__261 = 0;
                                for (; I >= end__261; I -= 1)
                                {
                                    {
                                        var w__262 = (Aow.Units.Engine.TEObject)w__250.List[I];
                                        var call__263 = w__262.GetLastMsg();
                                        if (call__263 != Msg.Seq)
                                        {
                                            w__262.SetLastMsg(Msg.Seq);
                                            w__262.MsgProc(ref Msg, this);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__250.Count - 1;
                                byte end__264 = 0;
                                for (; I >= end__264; I -= 1)
                                {
                                    {
                                        var w__265 = (Aow.Units.Engine.TEObject)w__250[I];
                                        var call__266 = w__265.GetLastMsg();
                                        if ((call__266 != Msg.Seq) && (w__265.Owner == this))
                                        {
                                            w__265.SetLastMsg(Msg.Seq);
                                            w__265.MsgProc(ref Msg, this);
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
                                I = w__250.Count - 1;
                                byte end__267 = 0;
                                for (; I >= end__267; I -= 1)
                                {
                                    {
                                        var w__268 = (Aow.Units.Engine.TEObject)w__250.List[I];
                                        w__268.MsgProc(ref Msg, this);
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = w__250.Count - 1;
                                byte end__269 = 0;
                                for (; I >= end__269; I -= 1)
                                {
                                    {
                                        var w__270 = (Aow.Units.Engine.TEObject)w__250[I];
                                        if (w__270.Owner == this)
                                        {
                                            w__270.MsgProc(ref Msg, this);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:8080
        public override int RemoveChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            if (GetControlStyle().Contains(Aow.Units.Engine.Engine.csKeepIndex))
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
        public override int AddChild(Aow.Units.Engine.TEObject Child)
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
        public override Aow.Units.Engine.TEObject FindChild(int AClassID)
        {
            Aow.Units.Engine.TEObject Result = default;
            int I = default;
            bool Found = default;
            Aow.Units.Engine.TEObject C = default;
            I = 0;
            Found = false;
            C = null;
            while (!Found && (I < FChildren.Count))
            {
                C = (Aow.Units.Engine.TEObject)FChildren.List[I];
                Found = (C != null) && (AClassID == C.ClassType.ClassID());
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
        public override int FindChildIndex(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            var call__273 = Count;
            Result = call__273 - 1;
            while ((Result >= 0) && (Child != FChildren.List[Result]))
            {
                Result -= 1;
            }

            return Result;
        }

        public int UpdateCount { get => FUpdateCount; }

        // Engine.pas:680
        public static new Aow.Units.Engine.TEChangeNotifyNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEChangeNotifyNode o = new Aow.Units.Engine.TEChangeNotifyNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:7246
            public override int ClassID()
            {
                int Result = default;
                Result = 265;
                return Result;
            }

            // Engine.pas:680
            public new Aow.Units.Engine.TEChangeNotifyNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEChangeNotifyNode o = (Aow.Units.Engine.TEChangeNotifyNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEChangeNotifyNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEChangeNotifyEventList (instance size 20): +$10 FEvents, notified by Changed }
    public partial class TEChangeNotifyEventList : Aow.Units.Engine.TEChangeNotifyNode
    {
        internal Aow.Units.Engine.TEventList FEvents;
        // Engine.pas:8125
        // { TEChangeNotifyEventList }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__274 = Aow.Units.Engine.TEventList.Create();
            FEvents = call__274;
        }

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

        public Aow.Units.Engine.TEventList Events { get => FEvents; }

        // Engine.pas:708
        public static new Aow.Units.Engine.TEChangeNotifyEventList Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEChangeNotifyEventList o = new Aow.Units.Engine.TEChangeNotifyEventList();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public enum TEditResult : byte
    {
        erOK = 0,
        erCancel = 1
    }

    public delegate void TEditDoneEvent(Aow.Units.Engine.TEditResult EditResult, Aow.Units.Engine.TEObject EObject);
    // { Engine.TEObjectEditor (instance size 8): editors are classes looked up by the class id of the
//     object (TEObject.Editor); everything is called on the class. The base editor edits nothing:
//     Edit reports a cancel at once. }
    public partial class TEObjectEditor : Aow.Units.Engine.TEObject
    {
        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:3036
            // { TEObjectEditor: stubs }
            public virtual int EditorClassID()
            {
                int Result = default;
                Result = 0;
                return Result;
            }

            // Engine.pas:3041
            public virtual void Edit(Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TEngine AEngine, Aow.Units.Engine.TEditDoneEvent Done)
            {
                Done(Aow.Units.Engine.TEditResult.erCancel, Obj);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEObjectEditor => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.Engine.TEObjectEditor Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEObjectEditor o = (Aow.Units.Engine.TEObjectEditor)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { Engine.THashList (instance size 16): +4 FCount, +8 FCapacity, +$C FItems. The hash classes
//     come from an include file of the original, D:\GEngine\Engine\inc\HashTab.PAS, whose name the
//     Assert calls carry. }
    public struct THashItem
    {
        public IntOrRef Key;
        public object Item;
    }

    public partial class THashList : Aow.Rtl.TObject
    {
        internal int FCount;
        internal int FCapacity;
        internal Ptr<Aow.Units.Engine.THashItem> FItems;
        // HashTab.PAS:18
        protected internal void SetCapacity(int NewCapacity)
        {
            Aow.Rtl.Rtl.Assert(NewCapacity >= FCount, (AnsiString)"Capacity Error (THashList)", "D:\\git\\aow\\src\\delphi3\\inc\\HashTab.PAS", 20);
            if (NewCapacity != FCapacity)
            {
                FItems = FItems.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // HashTab.PAS:28
        protected internal void Delete(int Index)
        {
            Aow.Rtl.Rtl.Assert((Index >= 0) && (Index < FCount), (AnsiString)"Delete Error (THashList)", "D:\\git\\aow\\src\\delphi3\\inc\\HashTab.PAS", 30);
            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<Aow.Units.Engine.THashItem>.Copy(FItems + (Index + 1), FItems + Index, (FCount - Index));
            }
        }

        // HashTab.PAS:36
        protected internal void Add(Aow.Units.Engine.THashItem Item)
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

    // { Engine.TIntegerHashList (instance size 16): items keyed by an integer }
    public partial class TIntegerHashList : Aow.Units.Engine.THashList
    {
        // HashTab.PAS:43
        public void Add(object Item, int Key)
        {
            Aow.Units.Engine.THashItem H = default;
            if (Find(Key) != null)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"ID already in HashTable");
            }

            Aow.Rtl.Rtl.Assert(Item != null, (AnsiString)"Cannot add nil item to hash table", "D:\\git\\aow\\src\\delphi3\\inc\\HashTab.PAS", 48);
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

        // System.pas:111
        public static Aow.Units.Engine.TIntegerHashList Create()
        {
            return new Aow.Units.Engine.TIntegerHashList();
        }
    }

    // { Engine.TIntegerHashTable (instance size 4): the abstract table interface }
    public partial class TIntegerHashTable : Aow.Rtl.TObject
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

    // { Engine.TIntegerHashTable256 (instance size 1028): 256 lists selected by the low key byte }
    public partial class TIntegerHashTable256 : Aow.Units.Engine.TIntegerHashTable
    {
        internal Aow.Units.FixedArray_Aow_Units_Engine_TIntegerHashList_256 FLists;
        // HashTab.PAS:83
        public void Create__init_TIntegerHashTable256()
        {
            int I = default;
            ;
            {
                I = 0;
                byte end__276 = 255;
                for (; I <= end__276; I += 1)
                {
                    var call__277 = Aow.Units.Engine.TIntegerHashList.Create();
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

        // Engine.pas:782
        public static Aow.Units.Engine.TIntegerHashTable256 Create()
        {
            Aow.Units.Engine.TIntegerHashTable256 o = new Aow.Units.Engine.TIntegerHashTable256();
            try
            {
                o.Create__init_TIntegerHashTable256();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TStringHashList (instance size 16): THashList items whose key dword is a string }
    public partial class TStringHashList : Aow.Units.Engine.THashList
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
        public object Find(Aow.Rtl.AnsiString Key)
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
                    if (Aow.Rtl.Rtl.AnsiCompareStr(FItems[I].Key.As<AnsiString>(), Key) == 0)
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
        public void Add(object Item, Aow.Rtl.AnsiString Key)
        {
            Aow.Units.Engine.THashItem H = default;
            if (Find(Key) != null)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"ID already in HashTable");
            }

            Aow.Rtl.Rtl.Assert(Item != null, (AnsiString)"Cannot add nil item to hash table", "D:\\git\\aow\\src\\delphi3\\inc\\HashTab.PAS", 175);
            H.Key = 0;
            H.Item = Item;
            H.Key = Key;
            base.Add(H);
        }

        // HashTab.PAS:182
        public void Delete(Aow.Rtl.AnsiString Key)
        {
            int I = default;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (Aow.Rtl.Rtl.AnsiCompareStr(FItems[I].Key.As<AnsiString>(), Key) == 0)
                    {
                        FItems[I].Key = (AnsiString)"";
                        base.Delete(I);
                        return;
                    }
                }
                while (!(I == 0));
            }
        }

        // System.pas:111
        public static Aow.Units.Engine.TStringHashList Create()
        {
            return new Aow.Units.Engine.TStringHashList();
        }
    }

    // { Engine.TStringHashTable (instance size 4): the abstract table interface }
    public partial class TStringHashTable : Aow.Rtl.TObject
    {
        // Engine.pas:804
        public virtual void Add(object Item, Aow.Rtl.AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:805
        public virtual void Delete(Aow.Rtl.AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:806
        public virtual object Find(Aow.Rtl.AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Engine.pas:807
        public virtual void Clear()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    // { Engine.TStringHashTable256 (instance size 1028): 256 lists selected by a hash of the first
//     and the second-to-last character and the length }
    public partial class TStringHashTable256 : Aow.Units.Engine.TStringHashTable
    {
        internal Aow.Units.FixedArray_Aow_Units_Engine_TStringHashList_256 FLists;
        // HashTab.PAS:199
        public void Create__init_TStringHashTable256()
        {
            int I = default;
            ;
            {
                I = 0;
                byte end__285 = 255;
                for (; I <= end__285; I += 1)
                {
                    var call__286 = Aow.Units.Engine.TStringHashList.Create();
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
        public override void Add(object Item, Aow.Rtl.AnsiString Key)
        {
            int H = default;
            H = Key.Length;
            if (H >= 2)
            {
                H = ((((int)Key[1]) + ((int)Key[H - 1])) + (H << 4)) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = (int)Key[1];
                }
            }

            FLists[H].Add(Item, Key);
        }

        // HashTab.PAS:234
        public override void Delete(Aow.Rtl.AnsiString Key)
        {
            int H = default;
            H = Key.Length;
            if (H >= 2)
            {
                H = ((((int)Key[1]) + ((int)Key[H - 1])) + (H << 4)) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = (int)Key[1];
                }
            }

            FLists[H].Delete(Key);
        }

        // HashTab.PAS:246
        public override object Find(Aow.Rtl.AnsiString Key)
        {
            object Result = default;
            int H = default;
            H = Key.Length;
            if (H >= 2)
            {
                H = ((((int)Key[1]) + ((int)Key[H - 1])) + (H << 4)) & 255;
            }
            else
            {
                if (H == 1)
                {
                    H = (int)Key[1];
                }
            }

            var call__289 = FLists[H].Find(Key);
            Result = call__289;
            return Result;
        }

        // Engine.pas:816
        public static Aow.Units.Engine.TStringHashTable256 Create()
        {
            Aow.Units.Engine.TStringHashTable256 o = new Aow.Units.Engine.TStringHashTable256();
            try
            {
                o.Create__init_TStringHashTable256();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEControlStyleObject (instance size 12): +8 FControlStyle }
    public partial class TEControlStyleObject : Aow.Units.Engine.TEObject
    {
        internal TSet<Aow.Units.Engine.TEControlStyleElement> FControlStyle;
        // Engine.pas:8222
        // { TEControlStyleObject }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:8233
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:8238
        public void SetControlStyle(TSet<Aow.Units.Engine.TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8243
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            Aow.Rtl.Rtl.RwSet(ref FControlStyle, 1, 1, Stream.rwData, 0);
        }

        // Engine.pas:829
        public static new Aow.Units.Engine.TEControlStyleObject Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEControlStyleObject o = new Aow.Units.Engine.TEControlStyleObject();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:8228
            public override int ClassID()
            {
                int Result = default;
                Result = 81;
                return Result;
            }

            // Engine.pas:829
            public new Aow.Units.Engine.TEControlStyleObject Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEControlStyleObject o = (Aow.Units.Engine.TEControlStyleObject)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEControlStyleObject => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEControlStyleNode (instance size 16): +$C FControlStyle }
    public partial class TEControlStyleNode : Aow.Units.Engine.TENode
    {
        internal TSet<Aow.Units.Engine.TEControlStyleElement> FControlStyle;
        // Engine.pas:8193
        // { TEControlStyleNode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:8204
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:8209
        public void SetControlStyle(TSet<Aow.Units.Engine.TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8214
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Aow.Rtl.Rtl.RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
        }

        // Engine.pas:841
        public static new Aow.Units.Engine.TEControlStyleNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEControlStyleNode o = new Aow.Units.Engine.TEControlStyleNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TENode.Meta
        {
            // Engine.pas:8199
            public override int ClassID()
            {
                int Result = default;
                Result = 257;
                return Result;
            }

            // Engine.pas:841
            public new Aow.Units.Engine.TEControlStyleNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEControlStyleNode o = (Aow.Units.Engine.TEControlStyleNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEControlStyleNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TECustomOwner (instance size 20): +$C FEngine, +$10 FControlStyle. A node that knows
//     its engine: message $20000008 (TEngine.ActivateEObject) carries the engine in the message. }
    public partial class TECustomOwner : Aow.Units.Engine.TENode
    {
        internal Aow.Units.Engine.TEngine FEngine;
        internal TSet<Aow.Units.Engine.TEControlStyleElement> FControlStyle;
        // Engine.pas:8250
        // { TECustomOwner }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FOwner = AOwner;
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:8262
        public override Aow.Units.Engine.TEngine GetEngine()
        {
            Aow.Units.Engine.TEngine Result = default;
            Result = FEngine;
            return Result;
        }

        // Engine.pas:8267
        public void SetEngine(Aow.Units.Engine.TEngine AEngine)
        {
            FEngine = AEngine;
        }

        // Engine.pas:8272
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:8277
        public void SetControlStyle(TSet<Aow.Units.Engine.TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:8282
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Aow.Rtl.Rtl.RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
        }

        // Engine.pas:8288
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = Msg.Engine;
            }

            base.MsgProc(ref Msg, Sender);
        }

        // Engine.pas:855
        public static new Aow.Units.Engine.TECustomOwner Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TECustomOwner o = new Aow.Units.Engine.TECustomOwner();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TENode.Meta
        {
            // Engine.pas:8257
            public override int ClassID()
            {
                int Result = default;
                Result = 259;
                return Result;
            }

            // Engine.pas:855
            public new Aow.Units.Engine.TECustomOwner Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TECustomOwner o = (Aow.Units.Engine.TECustomOwner)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TECustomOwner => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEObjectClassList (instance size 16): a TList of class references }
    public partial class TEObjectClassList : Aow.Rtl.TList
    {
        // Engine.pas:8296
        // { TEObjectClassList }
        protected internal Aow.Units.Engine.TEObject.Meta GetEClass(int Index)
        {
            Aow.Units.Engine.TEObject.Meta Result = default;
            var call__290 = this[Index];
            Result = (Aow.Units.Engine.TEObject.Meta)call__290;
            return Result;
        }

        // Engine.pas:8301
        public Aow.Units.Engine.TEObject.Meta FindClass(int AClassID)
        {
            Aow.Units.Engine.TEObject.Meta Result = default;
            int I = default;
            bool Found = default;
            I = 0;
            Found = false;
            while (!Found && (I < Count))
            {
                var call__291 = this[I];
                var call__292 = ((Aow.Units.Engine.TEObject.Meta)call__291).ClassID();
                Found = AClassID == call__292;
                I += 1;
            }

            if (Found)
            {
                var call__293 = this[I - 1];
                Result = (Aow.Units.Engine.TEObject.Meta)call__293;
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
            while (!Found && (I < Count))
            {
                var call__294 = this[I];
                var call__295 = ((Aow.Units.Engine.TEObject.Meta)call__294).ClassID();
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
        public Aow.Units.Engine.TEObject.Meta GetClass(int AClassID)
        {
            Aow.Units.Engine.TEObject.Meta Result = default;
            Result = FindClass(AClassID);
            if (Result == null)
            {
                throw Aow.Rtl.Exception.Create((((AnsiString)"Unregistered E-ClassID found (") + Aow.Rtl.Rtl.IntToStr(AClassID)) + AnsiString.FromBytes(41));
            }

            return Result;
        }

        // Engine.pas:8338
        public void RegisterClass(Aow.Units.Engine.TEObject.Meta AClass)
        {
            var call__296 = AClass.ClassID();
            var call__297 = FindClass(call__296);
            bool bool__298 = (call__297 == null);
            if (bool__298)
            {
                var call__299 = AClass.ClassID();
                bool__298 = (call__299 != 0);
            }

            if (bool__298)
            {
                Add(AClass);
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"EObject Class ID double defined!");
            }
        }

        // Engine.pas:8346
        public void UnregisterClass(Aow.Units.Engine.TEObject.Meta AClass)
        {
            int I = default;
            var call__300 = AClass.ClassID();
            var call__301 = FindClassIndex(call__300);
            I = call__301;
            if (I != -1)
            {
                Delete(I);
            }
        }

        public sealed class EClassesAccessor(Aow.Units.Engine.TEObjectClassList target)
        {
            public Aow.Units.Engine.TEObject.Meta this[int Index] { get => target.GetEClass(Index); }
        }

        public EClassesAccessor EClasses => new EClassesAccessor(this);

        // System.pas:111
        public static Aow.Units.Engine.TEObjectClassList Create()
        {
            return new Aow.Units.Engine.TEObjectClassList();
        }
    }

    // { Engine.TEResourceEditSettings (instance size 16): +$C FEditors, editor names with ids }
    public partial class TEResourceEditSettings : Aow.Units.Engine.TEControlStyleObject
    {
        internal Aow.Rtl.TStringList FEditors;
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
                if (Result != Aow.Rtl.Rtl.ObjectToInt(call__303))
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
        // { TEResourceEditSettings }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__305 = new Aow.Rtl.TStringList();
            FEditors = call__305;
        }

        // Engine.pas:8656
        public override void Destroy()
        {
            FEditors.Free();
            base.Destroy();
        }

        // Engine.pas:8667
        public Aow.Rtl.AnsiString GetUniqueEditorName(Aow.Rtl.AnsiString Name)
        {
            Aow.Rtl.AnsiString Result = default;
            Aow.Rtl.AnsiString S = default;
            S = Name;
            while (true)
            {
                var call__306 = FEditors.IndexOf(S);
                if (!(call__306 != -1))
                    break;
                {
                    S = Name + Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Random(268435455));
                }
            }

            Result = S;
            return Result;
        }

        // Engine.pas:8700
        public int GetEditID(Aow.Rtl.AnsiString Name)
        {
            int Result = default;
            int I = default;
            var call__307 = FEditors.IndexOf(Name);
            I = call__307;
            if (I != -1)
            {
                var call__308 = FEditors.Objects[I];
                Result = Aow.Rtl.Rtl.ObjectToInt(call__308);
            }
            else
            {
                Result = GetUniqueEditID();
                FEditors.AddObject(Name, Aow.Rtl.Rtl.IntToObject((int)Result));
            }

            return Result;
        }

        // Engine.pas:8714
        public int RemoveEditor(Aow.Rtl.AnsiString Name)
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
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.Engine.smText))
            {
                Stream.rwStringsEx(FEditors, 3);
            }
        }

        public Aow.Rtl.TStringList Editors { get => FEditors; }

        // Engine.pas:891
        public static new Aow.Units.Engine.TEResourceEditSettings Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEResourceEditSettings o = new Aow.Units.Engine.TEResourceEditSettings();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEControlStyleObject.Meta
        {
            // Engine.pas:8662
            public override int ClassID()
            {
                int Result = default;
                Result = 1026;
                return Result;
            }

            // Engine.pas:891
            public new Aow.Units.Engine.TEResourceEditSettings Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEResourceEditSettings o = (Aow.Units.Engine.TEResourceEditSettings)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEResourceEditSettings => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public struct TSortItem
    {
        public int SortValue;
        public Aow.Units.Engine.TEObject Child;
    }

    public partial class TESortNode : Aow.Units.Engine.TECustomNode
    {
        protected internal bool FSorted;
        protected internal int FCount;
        protected internal int FCapacity;
        protected internal Ptr<Aow.Units.Engine.TSortItem> FItems;
        // Engine.pas:1513
        protected internal void Resize(int NewCapacity)
        {
            FItems = FItems.Realloc(NewCapacity);
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

        // Engine.pas:1466
        // { strings }
        // { TESortNode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FItems = default;
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
        public override Aow.Units.Engine.TEObject GetChild(int Index)
        {
            Aow.Units.Engine.TEObject Result = default;
            Result = FItems[Index].Child;
            return Result;
        }

        // Engine.pas:1493
        public override void SetChild(int Index, Aow.Units.Engine.TEObject Child)
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
        public override int RemoveChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            Result = FCount - 1;
            while ((Result >= 0) && (Child != FItems[Result].Child))
            {
                Result -= 1;
            }

            if (Result >= 0)
            {
                FCount -= 1;
                if (Result < FCount)
                {
                    Ptr<Aow.Units.Engine.TSortItem>.Copy(FItems + (Result + 1), FItems + Result, (FCount - Result));
                }
            }

            return Result;
        }

        // Engine.pas:1537
        public override int AddChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            Result = AddSortChild(Child, 0);
            return Result;
        }

        // Engine.pas:1542
        public virtual int AddSortChild(Aow.Units.Engine.TEObject Child, int SortValue)
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
                while ((I > 0) && (SortValue > FItems[I - 1].SortValue))
                {
                    I -= 1;
                }

                if (I != FCount)
                {
                    Ptr<Aow.Units.Engine.TSortItem>.Copy(FItems + I, FItems + (I + 1), (FCount - I));
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
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            throw Aow.Rtl.Exception.Create((AnsiString)"Read/Write not supported (TESortNode)");
        }

        // Engine.pas:1596
        public virtual void Sort()
        {
            if (!FSorted && (FCount > 0))
            {
                Aow.Units.Engine.Engine.SortItems(FItems, 0, FCount - 1);
            }

            FSorted = true;
        }

        public sealed class SortValueAccessor(Aow.Units.Engine.TESortNode target)
        {
            public int this[int Index] { get => target.GetSortValue(Index); set => target.SetSortValue(Index, value); }
        }

        public SortValueAccessor SortValue => new SortValueAccessor(this);
        public bool Sorted { get => FSorted; }

        // Engine.pas:922
        public static new Aow.Units.Engine.TESortNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TESortNode o = new Aow.Units.Engine.TESortNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Engine.pas:1478
            public override int ClassID()
            {
                int Result = default;
                Result = 260;
                return Result;
            }

            // Engine.pas:922
            public new Aow.Units.Engine.TESortNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TESortNode o = (Aow.Units.Engine.TESortNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TESortNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEInverseSortNode: the same, sorted the other way }
    public partial class TEInverseSortNode : Aow.Units.Engine.TESortNode
    {
        // Engine.pas:1635
        public override int AddSortChild(Aow.Units.Engine.TEObject Child, int SortValue)
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
                while ((I > 0) && (SortValue < FItems[I - 1].SortValue))
                {
                    I -= 1;
                }

                if (I != FCount)
                {
                    Ptr<Aow.Units.Engine.TSortItem>.Copy(FItems + I, FItems + (I + 1), (FCount - I));
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
            if (!FSorted && (FCount > 0))
            {
                Aow.Units.Engine.Engine.InverseSortItems(FItems, 0, FCount - 1);
            }

            FSorted = true;
        }

        public new partial class Meta : Aow.Units.Engine.TESortNode.Meta
        {
            // Engine.pas:1630
            // { TEInverseSortNode }
            public override int ClassID()
            {
                int Result = default;
                Result = 262;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEInverseSortNode => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:922
            public new Aow.Units.Engine.TEInverseSortNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEInverseSortNode o = (Aow.Units.Engine.TEInverseSortNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { Engine.TEControlStyleSortNode (instance size 28): +$18 FControlStyle }
    public partial class TEControlStyleSortNode : Aow.Units.Engine.TESortNode
    {
        internal TSet<Aow.Units.Engine.TEControlStyleElement> FControlStyle;
        // Engine.pas:1666
        // { TEControlStyleSortNode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:1677
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:1682
        public void SetControlStyle(TSet<Aow.Units.Engine.TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:952
        public static new Aow.Units.Engine.TEControlStyleSortNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEControlStyleSortNode o = new Aow.Units.Engine.TEControlStyleSortNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TESortNode.Meta
        {
            // Engine.pas:1672
            public override int ClassID()
            {
                int Result = default;
                Result = 261;
                return Result;
            }

            // Engine.pas:952
            public new Aow.Units.Engine.TEControlStyleSortNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEControlStyleSortNode o = (Aow.Units.Engine.TEControlStyleSortNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEControlStyleSortNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEControlStyleInverseSortNode (instance size 28): +$18 FControlStyle }
    public partial class TEControlStyleInverseSortNode : Aow.Units.Engine.TEInverseSortNode
    {
        internal TSet<Aow.Units.Engine.TEControlStyleElement> FControlStyle;
        // Engine.pas:1689
        // { TEControlStyleInverseSortNode }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FControlStyle = base.GetControlStyle();
        }

        // Engine.pas:1700
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = FControlStyle;
            return Result;
        }

        // Engine.pas:1705
        public void SetControlStyle(TSet<Aow.Units.Engine.TEControlStyleElement> Value)
        {
            FControlStyle = Value;
        }

        // Engine.pas:963
        public static new Aow.Units.Engine.TEControlStyleInverseSortNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEControlStyleInverseSortNode o = new Aow.Units.Engine.TEControlStyleInverseSortNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Engine.TEInverseSortNode.Meta
        {
            // Engine.pas:1695
            public override int ClassID()
            {
                int Result = default;
                Result = 263;
                return Result;
            }

            // Engine.pas:963
            public new Aow.Units.Engine.TEControlStyleInverseSortNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEControlStyleInverseSortNode o = (Aow.Units.Engine.TEControlStyleInverseSortNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEControlStyleInverseSortNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Engine.TEReferenceNode (instance size 12): a TENode that holds references to objects owned
//     elsewhere (AddRef on AddChild, Release on removal) }
    public partial class TEReferenceNode : Aow.Units.Engine.TENode
    {
        // Engine.pas:8144
        // { TEReferenceNode }
        public override void Clear()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__310 = 0;
                for (; I >= end__310; I -= 1)
                {
                    ((Aow.Units.Engine.TEObject)FChildren.List[I]).Release();
                }
            }

            FChildren.Clear();
        }

        // Engine.pas:8157
        public override void ReadWriteChildren(Aow.Units.Engine.TEStorageStream Stream)
        {
            int I = default;
            if (Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smWrite))
            {
                {
                    I = 0;
                    int end__311 = FChildren.Count - 1;
                    for (; I <= end__311; I += 1)
                    {
                        if (((Aow.Units.Engine.TEObject)FChildren.List[I]).FOwner == null)
                        {
                            ((Aow.Units.Engine.TEObject)FChildren.List[I]).FOwner = this;
                        }
                    }
                }

                base.ReadWriteChildren(Stream);
                {
                    I = 0;
                    int end__312 = FChildren.Count - 1;
                    for (; I <= end__312; I += 1)
                    {
                        if (((Aow.Units.Engine.TEObject)FChildren.List[I]).FOwner == this)
                        {
                            ((Aow.Units.Engine.TEObject)FChildren.List[I]).FOwner = null;
                        }
                    }
                }
            }
            else
            {
                base.ReadWriteChildren(Stream);
                if ((Stream.StorageMode() & TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smText, Aow.Units.Engine.TStorageModeElement.smCompare)) == TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead))
                {
                    {
                        I = 0;
                        int end__313 = FChildren.Count - 1;
                        for (; I <= end__313; I += 1)
                        {
                            ((Aow.Units.Engine.TEObject)FChildren.List[I]).Release();
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
        public override int RemoveChild(Aow.Units.Engine.TEObject Child)
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
        public override int AddChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            if (Child.FOwner == this)
            {
                Child.FOwner = null;
            }

            FChildren.Add(Child);
            if (Child.AddRef() == -1)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"EObject doesn't support ReferenceCounting");
            }

            Result = FChildren.Count - 1;
            return Result;
        }

        // Engine.pas:631
        public static new Aow.Units.Engine.TEReferenceNode Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Engine.TEReferenceNode o = new Aow.Units.Engine.TEReferenceNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEUser (instance size 12): +8 FNode, the node the user is connected to }
    public partial class TEUser : Aow.Units.Engine.TEObject
    {
        internal Aow.Units.Engine.TECustomNode FNode;
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
        // { TEUser }
        public override void Destroy()
        {
            Disconnect();
            base.Destroy();
        }

        // Engine.pas:7118
        public void Connect(Aow.Units.Engine.TECustomNode Node)
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

        public Aow.Units.Engine.TECustomNode Node { get => FNode; }
    }

    // { Engine.TEResourceList (instance size 20): resources with unique ids }
    public partial class TEResourceList : Aow.Units.Engine.TECustomOwner
    {
        // Engine.pas:8573
        protected internal Aow.Units.Engine.TEResource GetResource(int Index)
        {
            Aow.Units.Engine.TEResource Result = default;
            Result = (Aow.Units.Engine.TEResource)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:8578
        public virtual void ResourceChanged()
        {
        }

        // Engine.pas:8582
        public Aow.Units.Engine.TEResource FindResource(int ID)
        {
            Aow.Units.Engine.TEResource Result = default;
            int I = default;
            bool Found = default;
            {
                var w__315 = FChildren;
                Found = false;
                I = 0;
                while (!Found && (I != w__315.Count))
                {
                    Found = (w__315.List[I] != null) && (ID == ((Aow.Units.Engine.TEResource)w__315.List[I]).FResourceID);
                    I += 1;
                }

                if (Found)
                {
                    Result = (Aow.Units.Engine.TEResource)w__315.List[I - 1];
                }
                else
                {
                    Result = null;
                }
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
                {
                    var w__317 = GetResource(Index);
                    w__317.FResourceID = w__317.FResourceID ^ 65535;
                }

                if (FindResource(ID) != null)
                {
                    Tries = 10;
                    Found = false;
                    NewID = 0;
                    while ((Tries > 0) && !Found)
                    {
                        NewID = Aow.Rtl.Rtl.Random(268435455);
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

        public sealed class ResourcesAccessor(Aow.Units.Engine.TEResourceList target)
        {
            public Aow.Units.Engine.TEResource this[int Index] { get => target.GetResource(Index); }
        }

        public ResourcesAccessor Resources => new ResourcesAccessor(this);

        public new partial class Meta : Aow.Units.Engine.TECustomOwner.Meta
        {
            // Engine.pas:8568
            // { TEResourceList }
            public override int ClassID()
            {
                int Result = default;
                Result = 1025;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Engine.TEResourceList => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:855
            public new Aow.Units.Engine.TEResourceList Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Engine.TEResourceList o = (Aow.Units.Engine.TEResourceList)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { Engine.TEComponent (instance size 56): +$24 FOnRegisterClasses (called from Loaded),
//     +$2C FOnUnRegisterClasses (called from Destroy), +$34 FEngine, the engine the component owns
//     and frees. Descendants such as HSEngine's THexagonSpriteEngine publish the two events. }
    public partial class TEComponent : Aow.Rtl.TComponent
    {
        internal Aow.Rtl.TNotifyEvent FOnRegisterClasses;
        internal Aow.Rtl.TNotifyEvent FOnUnRegisterClasses;
        internal Aow.Units.Engine.TEngine FEngine;
        // Engine.pas:8642
        public override void Loaded()
        {
            base.Loaded();
            if (FOnRegisterClasses != null)
            {
                FOnRegisterClasses(this);
            }
        }

        protected internal Aow.Rtl.TNotifyEvent OnRegisterClasses { get => FOnRegisterClasses; set => FOnRegisterClasses = value; }
        protected internal Aow.Rtl.TNotifyEvent OnUnRegisterClasses { get => FOnUnRegisterClasses; set => FOnUnRegisterClasses = value; }

        // Engine.pas:8635
        // { TEComponent }
        public override void Destroy()
        {
            if (FOnUnRegisterClasses != null)
            {
                FOnUnRegisterClasses(this);
            }

            FEngine.Free();
            base.Destroy();
        }

        public Aow.Units.Engine.TEngine Engine { get => FEngine; set => FEngine = value; }
    }

    public struct TCallBackEvent
    {
        public Aow.Rtl.TNotifyEvent Event;
        public int Data;
    }

    public partial class TCallBackEventList : Aow.Rtl.TObject
    {
        internal Aow.Rtl.TList FList;
        // Engine.pas:5864
        internal int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

        // Engine.pas:5869
        internal Ptr<Aow.Units.Engine.TCallBackEvent> GetItem(int Index)
        {
            Ptr<Aow.Units.Engine.TCallBackEvent> Result = default;
            Result = Ptr<Aow.Units.Engine.TCallBackEvent>.Unbox(FList.List[Index]);
            return Result;
        }

        // Engine.pas:5846
        public void Create__init_TCallBackEventList()
        {
            ;
            var call__320 = new Aow.Rtl.TList();
            FList = call__320;
        }

        // Engine.pas:5852
        public override void Destroy()
        {
            Clear();
            FList.Free();
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
        public void Add(in Aow.Units.Engine.TCallBackEvent Event)
        {
            Ptr<Aow.Units.Engine.TCallBackEvent> P = default;
            P = Ptr<Aow.Units.Engine.TCallBackEvent>.Alloc(1);
            P.Value = Event;
            FList.Add(P.Box());
        }

        // Engine.pas:5883
        public void Delete(int Index)
        {
            if (Index < FList.Count)
            {
                ;
                FList.Delete(Index);
            }
        }

        public int Count { get => GetCount(); }

        public Ptr<Aow.Units.Engine.TCallBackEvent> this[int Index] { get => GetItem(Index); }

        // Engine.pas:1083
        public static Aow.Units.Engine.TCallBackEventList Create()
        {
            Aow.Units.Engine.TCallBackEventList o = new Aow.Units.Engine.TCallBackEventList();
            try
            {
                o.Create__init_TCallBackEventList();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TEngine.OutputDebugStr / OutputDebugMessage hand this to the debug events }
    public struct TEngineDebugInfo
    {
        public int ID;
        public Aow.Rtl.AnsiString Text;
    }

    // { Engine.TEReadTextStorageStream (instance size 56): +$24 FPath, +$28 FSize, +$2C FSections,
//     +$30 FText, +$34 FValues. The text form of the storage: "[:path:]" lines open the section of
//     an object (its path is the ids of its parents joined with '\'), "[id]=value" lines are its
//     properties; the section is split into FValues with the property table pointing at them. }
    public partial class TEReadTextStorageStream : Aow.Units.Engine.TEStorageStream
    {
        internal Aow.Rtl.AnsiString FPath;
        internal int FSize;
        internal Aow.Rtl.TStringList FSections;
        internal Aow.Rtl.TStringList FText;
        internal Aow.Rtl.TStringList FValues;
        // Engine.pas:10632
        internal void ReadStreamText()
        {
            Aow.Rtl.AnsiString Buffer = default;
            Aow.Rtl.AnsiString Line = default;
            Aow.Rtl.TStringList Text = default;
            Ptr<byte> P = default;
            Ptr<byte> Start = default;
            int L = default;
            Text = FText;
            FSections.Clear();
            Text.BeginUpdate();
            try
            {
                Buffer = default(AnsiString).Resized(FSize);
                Aow.Rtl.Rtl.ReadString(FStream, ref Buffer, 0, FSize, false);
                Text.Clear();
                P = Ptr<byte>.Unbox(Buffer);
                if (!P.IsNull)
                {
                    while (P.Value != 0)
                    {
                        Start = P;
                        while (!TSet<byte>.WithBounds(0, 255).With(0).With(10).With(13).Contains(P.Value))
                        {
                            P += 1;
                        }

                        Line = Aow.Rtl.Rtl.SetString(Start, P - Start);
                        if (Line != ((AnsiString)""))
                        {
                            L = Line.Length;
                            if (((L >= 4) && (Aow.Rtl.Rtl.Copy(Line, 1, 2) == ((AnsiString)"[:"))) && (Aow.Rtl.Rtl.Copy(Line, L - 1, 2) == ((AnsiString)":]")))
                            {
                                var call__321 = Text.Count;
                                FSections.AddObject(Aow.Rtl.Rtl.Copy(Line, 3, L - 4), Aow.Rtl.Rtl.IntToObject((int)call__321));
                            }
                            else
                            {
                                Text.Add(Line);
                            }
                        }

                        if (P.Value == 13)
                        {
                            P += 1;
                        }

                        if (P.Value == 10)
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
            Aow.Rtl.AnsiString Value = default;
            FValues.Clear();
            var call__322 = FSections.IndexOf(FPath);
            Index = call__322;
            if (Index != -1)
            {
                var call__323 = FSections.Objects[Index];
                First = Aow.Rtl.Rtl.ObjectToInt(call__323);
                var call__324 = FSections.Count;
                if (Index != (call__324 - 1))
                {
                    var call__325 = FSections.Objects[Index + 1];
                    Last = Aow.Rtl.Rtl.ObjectToInt(call__325) - 1;
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
                    var call__328 = Aow.Units.Engine.Engine.GetLineID(call__327);
                    NewID = call__328;
                    if (ID != -1)
                    {
                        if ((NewID != ID) && (NewID != -1))
                        {
                            var call__329 = FValues.Count;
                            FPropertyTable.Add(ID, call__329);
                            FValues.Add(Value);
                            ID = NewID;
                            var call__330 = FText[I];
                            var call__331 = Aow.Units.Engine.Engine.GetLineValue(call__330);
                            Value = call__331;
                        }
                        else
                        {
                            var call__332 = FText[I];
                            Value = ((Value + AnsiString.FromBytes(13)) + AnsiString.FromBytes(10)) + call__332;
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
                            var call__334 = Aow.Units.Engine.Engine.GetLineValue(call__333);
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
        // { TEReadTextStorageStream }
        public void Create__init_TEReadTextStorageStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine, int ASize)
        {
            base.Create__init_TEStorageStream(AStream, AEngine);
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

            var call__338 = new Aow.Rtl.TStringList();
            FValues = call__338;
            var call__339 = Aow.Units.Engine.TPropertyTable.Create();
            FPropertyTable = call__339;
            var call__340 = new Aow.Rtl.TStringList();
            FText = call__340;
            var call__341 = new Aow.Rtl.TStringList();
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
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smText);
            return Result;
        }

        // Engine.pas:10741
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            Aow.Units.Engine.TEObject O = default;
            // { Match trials (4): named temporaries, with scopes, Pointer casts, zero-shift scheduling; still differs. }
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10749
        public override void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
        {
            Aow.Rtl.TStringList SavedValues = default;
            Aow.Rtl.AnsiString SavedPath = default;
            bool Continue = default;
            SavedPath = FPath;
            if (FPath != ((AnsiString)""))
            {
                FPath = (FPath + AnsiString.FromBytes(92)) + Aow.Rtl.Rtl.IntToStr(ID);
            }
            else
            {
                FPath = Aow.Rtl.Rtl.IntToStr(ID);
            }

            SavedValues = FValues;
            FPropertyTable.BackupSettings();
            var call__342 = new Aow.Rtl.TStringList();
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
                    var call__343 = Obj.ClassType.ClassID();
                    FOnError(this, (((AnsiString)"Error reading object (cid:") + Aow.Rtl.Rtl.IntToStr(call__343)) + AnsiString.FromBytes(41), ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }

        // Engine.pas:10779
        public override void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10784
        public override void rwString(ref Aow.Rtl.AnsiString S, int ID)
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
        public override void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
            int Offset = default;
            var call__346 = FPropertyTable.FindOffset(ID);
            Offset = call__346;
            if (Offset != -1)
            {
                var call__347 = FValues[Offset];
                Strings.Text = AnsiString.FromPChar(call__347.AsPtr());
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10800
        public override void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
        {
            rwStrings(Strings, ID);
        }

        // Engine.pas:10805
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            Aow.Rtl.AnsiString SavedPath = default;
            int I = default;
            Aow.Units.Engine.TEObject C = default;
            // { the original saves the path and never restores it }
            SavedPath = FPath;
            if (FPath != ((AnsiString)""))
            {
                FPath = (FPath + AnsiString.FromBytes(92)) + Aow.Rtl.Rtl.IntToStr(ID);
            }
            else
            {
                FPath = Aow.Rtl.Rtl.IntToStr(ID);
            }

            {
                I = 0;
                int end__348 = Node.Count - 1;
                for (; I <= end__348; I += 1)
                {
                    var call__349 = Node.Children[I];
                    C = call__349;
                    bool bool__350 = (C != null) && (C.Owner == Node);
                    if (bool__350)
                    {
                        var call__351 = C.GetControlStyle();
                        bool__350 = !call__351.Contains(Aow.Units.Engine.Engine.csShared);
                    }

                    bool bool__352 = bool__350;
                    if (bool__352)
                    {
                        var call__353 = C.GetControlStyle();
                        bool__352 = call__353.Contains(Aow.Units.Engine.Engine.csStored);
                    }

                    if (bool__352)
                    {
                        rwEObject(C, I);
                    }
                }
            }
        }

        // Engine.pas:1307
        public static Aow.Units.Engine.TEReadTextStorageStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine, int ASize)
        {
            Aow.Units.Engine.TEReadTextStorageStream o = new Aow.Units.Engine.TEReadTextStorageStream();
            try
            {
                o.Create__init_TEReadTextStorageStream(AStream, AEngine, ASize);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEWriteTextStorageStream (instance size 44): +$24 FPath, +$28 FBuffer. Each object's
//     properties are collected in FBuffer and written behind the "[:path:]" line. }
    public partial class TEWriteTextStorageStream : Aow.Units.Engine.TEStorageStream
    {
        internal Aow.Rtl.AnsiString FPath;
        protected internal Aow.Units.Engine.TEBufferStream FBuffer;
        // Engine.pas:10355
        // { $28; 557977DF }
        internal void WriteTextBuffer(Aow.Rtl.TStream Stream)
        {
            Aow.Rtl.AnsiString S = default;
            bool bool__354 = (FPropertyTable.Count != 0);
            if (bool__354)
            {
                var call__355 = FBuffer.Size;
                bool__354 = (call__355 != 0);
            }

            if (bool__354)
            {
                S = (((((AnsiString)"[:") + FPath) + ((AnsiString)":]")) + AnsiString.FromBytes(13)) + AnsiString.FromBytes(10);
                if (FPath != ((AnsiString)""))
                {
                    S = AnsiString.FromBytes(13, 10) + S;
                }

                Stream.Write(Aow.Rtl.Rtl.StringBytes(S).Slice(1 - 1, S.Length));
                FBuffer.SaveToStream(Stream);
            }
        }

        // Engine.pas:10335
        // { TEWriteTextStorageStream }
        public void Create__init_TEWriteTextStorageStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            base.Create__init_TEStorageStream(AStream, AEngine);
            FPath = (AnsiString)"";
            var call__356 = Aow.Units.Engine.TEBufferStream.Create();
            FBuffer = call__356;
            var call__357 = Aow.Units.Engine.TPropertyTable.Create();
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
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smWrite, Aow.Units.Engine.Engine.smText);
            return Result;
        }

        // Engine.pas:10368
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            Aow.Units.Engine.TEObject O = default;
            // { Match trials (4): named temporaries, with scopes, Pointer casts, zero-shift scheduling; still differs. }
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10376
        public override void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
        {
            Aow.Units.Engine.TEBufferStream SavedBuffer = default;
            Aow.Rtl.AnsiString SavedPath = default;
            bool Continue = default;
            var call__358 = FPropertyTable.PropertyExist(ID);
            if (!call__358)
            {
                SavedBuffer = FBuffer;
                SavedPath = FPath;
                FPropertyTable.BackupSettings();
                var call__359 = Aow.Units.Engine.TEBufferStream.Create();
                FBuffer = call__359;
                if (FPath != ((AnsiString)""))
                {
                    FPath = (FPath + AnsiString.FromBytes(92)) + Aow.Rtl.Rtl.IntToStr(ID);
                }
                else
                {
                    FPath = Aow.Rtl.Rtl.IntToStr(ID);
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
                        var call__360 = Obj.ClassType.ClassID();
                        FOnError(this, (((AnsiString)"Error writing object (cid:") + Aow.Rtl.Rtl.IntToStr(call__360)) + AnsiString.FromBytes(41), ref Continue);
                    }

                    if (!Continue)
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:10412
        public override void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10417
        public override void rwString(ref Aow.Rtl.AnsiString S, int ID)
        {
            Aow.Rtl.AnsiString Line = default;
            var call__361 = FPropertyTable.PropertyExist(ID);
            if (!call__361)
            {
                if (S != ((AnsiString)""))
                {
                    Line = ((((AnsiString.FromBytes(91) + Aow.Rtl.Rtl.IntToStr(ID)) + ((AnsiString)"]=")) + S) + AnsiString.FromBytes(13)) + AnsiString.FromBytes(10);
                    FBuffer.Write(Aow.Rtl.Rtl.StringBytes(Line).Slice(1 - 1, Line.Length));
                    FPropertyTable.Add(ID, 0);
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:10434
        public override void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
            Aow.Rtl.AnsiString Line = default;
            Ptr<byte> P = default;
            var call__362 = FPropertyTable.PropertyExist(ID);
            if (!call__362)
            {
                var call__363 = Strings.Count;
                if (call__363 != 0)
                {
                    P = Strings.Text.AsPtr();
                    try
                    {
                        Line = (AnsiString.FromBytes(91) + Aow.Rtl.Rtl.IntToStr(ID)) + ((AnsiString)"]=");
                        if (!P.IsNull)
                        {
                            Line = Line + AnsiString.FromPChar(P);
                        }
                        else
                        {
                            Line = (Line + AnsiString.FromBytes(13)) + AnsiString.FromBytes(10);
                        }

                        FBuffer.Write(Aow.Rtl.Rtl.StringBytes(Line).Slice(1 - 1, Line.Length));
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
                throw Aow.Units.Engine.EDoublePrioriyID.Create((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:10458
        public override void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
        {
            rwStrings(Strings, ID);
        }

        // Engine.pas:10463
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            Aow.Rtl.AnsiString SavedPath = default;
            int I = default;
            Aow.Units.Engine.TEObject C = default;
            var call__364 = FPropertyTable.PropertyExist(ID);
            if (!call__364)
            {
                SavedPath = FPath;
                if (FPath != ((AnsiString)""))
                {
                    FPath = (FPath + AnsiString.FromBytes(92)) + Aow.Rtl.Rtl.IntToStr(ID);
                }
                else
                {
                    FPath = Aow.Rtl.Rtl.IntToStr(ID);
                }

                {
                    I = 0;
                    int end__365 = Node.Count - 1;
                    for (; I <= end__365; I += 1)
                    {
                        var call__366 = Node.Children[I];
                        C = call__366;
                        bool bool__367 = (C != null) && (C.Owner == Node);
                        if (bool__367)
                        {
                            var call__368 = C.GetControlStyle();
                            bool__367 = !call__368.Contains(Aow.Units.Engine.Engine.csShared);
                        }

                        bool bool__369 = bool__367;
                        if (bool__369)
                        {
                            var call__370 = C.GetControlStyle();
                            bool__369 = call__370.Contains(Aow.Units.Engine.Engine.csStored);
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
                throw Aow.Units.Engine.EDoublePrioriyID.Create((AnsiString)"Double priorityID found");
            }
        }

        // Engine.pas:1327
        public static new Aow.Units.Engine.TEWriteTextStorageStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.Engine.TEWriteTextStorageStream o = new Aow.Units.Engine.TEWriteTextStorageStream();
            try
            {
                o.Create__init_TEWriteTextStorageStream(AStream, AEngine);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEReadStorageStream (instance size 44): +$24 FPartial, +$28 FDataBase. The constructor
//     reads the root property table; every property is found through the table and read at
//     FDataBase + offset. }
    public partial class TEReadStorageStream : Aow.Units.Engine.TEStorageStream
    {
        internal bool FPartial;
        internal int FDataBase;
        // Engine.pas:9968
        // { TEReadStorageStream }
        public void Create__init_TEReadStorageStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine, bool APartial)
        {
            base.Create__init_TEStorageStream(AStream, AEngine);
            var call__371 = Aow.Units.Engine.TPropertyTable.Create();
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
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            if (FPartial)
            {
                Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smPartial);
            }
            else
            {
                Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead);
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
                if (call__374 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref I, 4));
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
                if (call__376 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref B, 1));
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
                if (call__378 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref S, 1));
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
                if (call__380 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref W, 2));
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
                if (call__382 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref I, 4));
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
                if (call__384 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref B, 1));
            }
            else
            {
                B = unchecked((byte)Default);
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
                if (call__386 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref S, 1));
            }
            else
            {
                S = unchecked((sbyte)Default);
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
                if (call__388 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref W, 2));
            }
            else
            {
                W = unchecked((ushort)Default);
            }
        }

        // Engine.pas:10103
        public override void rwString(ref Aow.Rtl.AnsiString S, int ID)
        {
            int Offset = default;
            var call__389 = FPropertyTable.FindOffset(ID);
            Offset = call__389;
            if (Offset != -1)
            {
                var call__390 = FStream.Position;
                if (call__390 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                Aow.Units.Engine.Engine.LoadStringFromStream(ref S, FStream);
            }
            else
            {
                S = (AnsiString)"";
            }
        }

        // Engine.pas:10117
        public override void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
            int Offset = default;
            var call__391 = FPropertyTable.FindOffset(ID);
            Offset = call__391;
            if (Offset != -1)
            {
                var call__392 = FStream.Position;
                if (call__392 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                Aow.Units.Engine.Engine.StringsFromStream(Strings, FStream);
            }
            else
            {
                Strings.Clear();
            }
        }

        // Engine.pas:10131
        public override void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
        {
            int Offset = default;
            var call__393 = FPropertyTable.FindOffset(ID);
            Offset = call__393;
            if (Offset != -1)
            {
                var call__394 = FStream.Position;
                if (call__394 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                Aow.Units.Engine.Engine.StringsFromStreamEx(Strings, FStream);
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
                if (call__396 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Buffer.Slice(0, Size));
            }
        }

        // Engine.pas:10157
        public override void rwStreamProc(int ID, Aow.Rtl.TStreamProc LoadProc, Aow.Rtl.TStreamProc SaveProc)
        {
            int Offset = default;
            var call__397 = FPropertyTable.FindOffset(ID);
            Offset = call__397;
            if (Offset != -1)
            {
                var call__398 = FStream.Position;
                if (call__398 != (FDataBase + Offset))
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
        public override Aow.Rtl.TStream OpenPropertyStream(int ID)
        {
            Aow.Rtl.TStream Result = default;
            int Offset = default;
            var call__399 = FPropertyTable.FindOffset(ID);
            Offset = call__399;
            if (Offset != -1)
            {
                var call__400 = FStream.Position;
                if (call__400 != (FDataBase + Offset))
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
        public bool HandleException(Aow.Rtl.Exception E)
        {
            bool Result = default;
            bool Continue = default;
            if (!FErrorHandled)
            {
                Continue = true;
                if (FOnError != null)
                {
                    if (E.Message == ((AnsiString)""))
                    {
                        FOnError(this, Aow.Units.Engine.Engine.EStreamReadErrorStr, ref Continue);
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
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            int AClassID = default;
            // { Match trials (3): named temporaries, identity assignments, try-entry lifetimes; still differs. }
            Obj = null;
            var call__402 = FPropertyTable.FindOffset(ID);
            Offset = call__402;
            if (Offset != -1)
            {
                var call__403 = FStream.Position;
                if (call__403 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref AClassID, 4));
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
                        var E = Aow.Rtl.Rtl.Wrap(caught__405);
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
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            int I = default;
            int ChildID = default;
            Aow.Units.Engine.TEObject Child = default;
            var call__407 = FPropertyTable.FindOffset(ID);
            Offset = call__407;
            if (Offset != -1)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                var call__408 = FStream.Position;
                if (call__408 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                var call__409 = FStream.Position;
                FDataBase = call__409;
                if (Node.GetControlStyle().Contains(Aow.Units.Engine.Engine.csKeepIndex))
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
        public override void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
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
                if (call__413 != (FDataBase + Offset))
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
                var E = Aow.Rtl.Rtl.Wrap(caught__415);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }

        // Engine.pas:10304
        public override void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            int SavedBase = default;
            int Start = default;
            int Size = default;
            var call__416 = FStream.Position;
            Start = call__416;
            FStream.Read(Aow.Rtl.Bytes.Of(ref Size, 4));
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
                    FStream.Position = (Start + Size) + 4;
                }
                catch (System.Exception caught__417)
                {
                    var E = Aow.Rtl.Rtl.Wrap(caught__417);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    FStream.Position = (Start + Size) + 4;
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
        }

        public int DataBase { get => FDataBase; }

        // Engine.pas:1221
        public static Aow.Units.Engine.TEReadStorageStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine, bool APartial)
        {
            Aow.Units.Engine.TEReadStorageStream o = new Aow.Units.Engine.TEReadStorageStream();
            try
            {
                o.Create__init_TEReadStorageStream(AStream, AEngine, APartial);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TEWriteStorageStream (instance size 44): +$24 FPartial, +$28 FBuffer. Values go into the
//     buffer; the property table records (id, buffer position). A nested object gets its own buffer,
//     appended to the outer one as [table][data]. The destructor writes the root table and buffer. }
    public partial class TEWriteStorageStream : Aow.Units.Engine.TEStorageStream
    {
        internal bool FPartial;
        internal Aow.Units.Engine.TEBufferStream FBuffer;
        // Engine.pas:9592
        internal void PropertyExistError(int ID)
        {
            throw Aow.Units.Engine.EDoublePrioriyID.Create((((((AnsiString)"PropertyID already in use (") + Aow.Rtl.Rtl.IntToStr(ID)) + ((AnsiString)" / ")) + ((AnsiString)TObject.ClassNameOf(this.GetType()))) + AnsiString.FromBytes(41));
        }

        // Engine.pas:9565
        // { TEWriteStorageStream }
        public void Create__init_TEWriteStorageStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine, bool APartial)
        {
            base.Create__init_TEStorageStream(AStream, AEngine);
            FEngine = AEngine;
            FStream = AStream;
            var call__419 = Aow.Units.Engine.TEBufferStream.Create();
            FBuffer = call__419;
            var call__420 = Aow.Units.Engine.TPropertyTable.Create();
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
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            if (FPartial)
            {
                Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smWrite, Aow.Units.Engine.Engine.smPartial);
            }
            else
            {
                Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smWrite);
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref B, 1));
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref W, 2));
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref S, 1));
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref I, 4));
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((AnsiString)"PropertyID already in use");
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref B, 1));
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref W, 2));
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref S, 1));
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref I, 4));
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((AnsiString)"PropertyID already in use");
            }
        }

        // Engine.pas:9709
        public override void rwString(ref Aow.Rtl.AnsiString S, int ID)
        {
            var call__429 = FPropertyTable.PropertyExist(ID);
            if (!call__429)
            {
                if (S != ((AnsiString)""))
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    Aow.Units.Engine.Engine.SaveStringToStream(S, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9723
        public override void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
            var call__430 = FPropertyTable.PropertyExist(ID);
            if (!call__430)
            {
                var call__431 = Strings.Count;
                if (call__431 != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    Aow.Units.Engine.Engine.StringsToStream(Strings, FBuffer);
                }
            }
            else
            {
                PropertyExistError(ID);
            }
        }

        // Engine.pas:9737
        public override void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
        {
            var call__432 = FPropertyTable.PropertyExist(ID);
            if (!call__432)
            {
                var call__433 = Strings.Count;
                if (call__433 != 0)
                {
                    FPropertyTable.Add(ID, FBuffer.Position);
                    Aow.Units.Engine.Engine.StringsToStreamEx(Strings, FBuffer);
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
        public override void rwStreamProc(int ID, Aow.Rtl.TStreamProc LoadProc, Aow.Rtl.TStreamProc SaveProc)
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
        public override Aow.Rtl.TStream OpenPropertyStream(int ID)
        {
            Aow.Rtl.TStream Result = default;
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
        public bool HandleException(Aow.Rtl.Exception E)
        {
            bool Result = default;
            bool Continue = default;
            if (!FErrorHandled)
            {
                Continue = true;
                if (FOnError != null)
                {
                    if (E.Message == ((AnsiString)""))
                    {
                        FOnError(this, Aow.Units.Engine.Engine.EStreamWriteErrorStr, ref Continue);
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
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            Aow.Units.Engine.TEBufferStream Outer = default;
            int Start = default;
            int AClassID = default;
            Aow.Units.Engine.TEObject O = default;
            // { Match trials (3): named temporaries, with scopes, identity assignments, try-entry lifetimes; still differs. }
            var call__438 = FPropertyTable.PropertyExist(ID);
            if (!call__438)
            {
                Start = FBuffer.Position;
                O = Obj;
                if (O != null)
                {
                    AClassID = O.ClassType.ClassID();
                }
                else
                {
                    AClassID = 0;
                }

                if (AClassID != 0)
                {
                    Outer = FBuffer;
                    var call__439 = Aow.Units.Engine.TEBufferStream.Create();
                    FBuffer = call__439;
                    FPropertyTable.BackupSettings();
                    try
                    {
                        Obj.MainReadWrite(this);
                        Outer.Write(Aow.Rtl.Bytes.Of(ref AClassID, 4));
                        FPropertyTable.SaveToStream(Outer);
                        FBuffer.SaveToStream(Outer);
                        FBuffer.Free();
                        FPropertyTable.RestoreSettings();
                        FBuffer = Outer;
                        FPropertyTable.Add(ID, Start);
                    }
                    catch (System.Exception caught__440)
                    {
                        var E = Aow.Rtl.Rtl.Wrap(caught__440);
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
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref AClassID, 4));
                    FPropertyTable.Add(ID, Start);
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((((AnsiString)"Double priorityID found (") + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:9866
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            Aow.Units.Engine.TEBufferStream Outer = default;
            int Start = default;
            Aow.Units.Engine.TEObject C = default;
            int I = default;
            var call__441 = FPropertyTable.PropertyExist(ID);
            if (!call__441)
            {
                Start = FBuffer.Position;
                Outer = FBuffer;
                var call__442 = Aow.Units.Engine.TEBufferStream.Create();
                FBuffer = call__442;
                FPropertyTable.BackupSettings();
                {
                    I = 0;
                    int end__443 = Node.Count - 1;
                    for (; I <= end__443; I += 1)
                    {
                        var call__444 = Node.Children[I];
                        C = call__444;
                        bool bool__445 = (C != null) && (C.Owner == Node);
                        if (bool__445)
                        {
                            var call__446 = C.GetControlStyle();
                            bool__445 = !call__446.Contains(Aow.Units.Engine.Engine.csShared);
                        }

                        bool bool__447 = bool__445;
                        if (bool__447)
                        {
                            var call__448 = C.GetControlStyle();
                            bool__447 = call__448.Contains(Aow.Units.Engine.Engine.csStored);
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
                throw Aow.Units.Engine.EDoublePrioriyID.Create((((AnsiString)"Double priorityID found (") + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:9897
        public override void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
        {
            Aow.Units.Engine.TEBufferStream Outer = default;
            int Start = default;
            // { Match trials (3): named temporaries, with scopes, identity assignments, try-entry lifetimes; still differs. }
            var call__449 = FPropertyTable.PropertyExist(ID);
            if (!call__449)
            {
                Start = FBuffer.Position;
                Outer = FBuffer;
                var call__450 = Aow.Units.Engine.TEBufferStream.Create();
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
                    var E = Aow.Rtl.Rtl.Wrap(caught__451);
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
                throw Aow.Units.Engine.EDoublePrioriyID.Create((((((AnsiString)"Double priorityID found (") + ((AnsiString)TObject.ClassNameOf(Obj.GetType()))) + AnsiString.FromBytes(58)) + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:9931
        public override void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            Aow.Units.Engine.TEBufferStream Outer = default;
            int Start = default;
            int Len = default;
            // { Match trials (3): named temporaries, with scopes, identity assignments, try-entry lifetimes; still differs. }
            Start = FBuffer.Position;
            Outer = FBuffer;
            var call__452 = Aow.Units.Engine.TEBufferStream.Create();
            FBuffer = call__452;
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                Outer.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                Len = (Outer.Position - Start) - 4;
                Outer.Position = Start;
                Outer.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
                Outer.Position = (Start + Len) + 4;
                FPropertyTable.RestoreSettings();
                FBuffer.Free();
                FBuffer = Outer;
            }
            catch (System.Exception caught__453)
            {
                var E = Aow.Rtl.Rtl.Wrap(caught__453);
                FBuffer.Free();
                FBuffer = Outer;
                FPropertyTable.RestoreSettings();
                Len = 0;
                Outer.Position = Start;
                Outer.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }

        // Engine.pas:1256
        public static Aow.Units.Engine.TEWriteStorageStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine, bool APartial)
        {
            Aow.Units.Engine.TEWriteStorageStream o = new Aow.Units.Engine.TEWriteStorageStream();
            try
            {
                o.Create__init_TEWriteStorageStream(AStream, AEngine, APartial);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TDebugWriteStream (instance size 44): a write storage stream that writes every
//     object's data straight into the stream (FBuffer is the stream itself) }
    public partial class TDebugWriteStream : Aow.Units.Engine.TEWriteStorageStream
    {
        // Engine.pas:10489
        // { TDebugWriteStream }
        public void Create__init_TDebugWriteStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            FEngine = AEngine;
            FStream = AStream;
            FBuffer = (Aow.Units.Engine.TEBufferStream)AStream;
            var call__454 = Aow.Units.Engine.TPropertyTable.Create();
            FPropertyTable = call__454;
            FPartial = false;
            FErrorHandled = false;
        }

        // Engine.pas:10499
        public override void Destroy()
        {
            FPropertyTable.SaveToStream(FStream);
            FPropertyTable.Free();
        }

        // Engine.pas:10505
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            int AClassID = default;
            Aow.Units.Engine.TEObject O = default;
            // { Match trials (3): declaration/operand order, named temporaries, Pointer casts, identity assignments; still differs. }
            var call__455 = FPropertyTable.PropertyExist(ID);
            if (!call__455)
            {
                O = Obj;
                if (O != null)
                {
                    AClassID = O.ClassType.ClassID();
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
                        FBuffer.Write(Aow.Rtl.Bytes.Of(ref AClassID, 4));
                        FPropertyTable.SaveToStream(FBuffer);
                        FBuffer.Free();
                        FPropertyTable.RestoreSettings();
                        FPropertyTable.Add(ID, FBuffer.Position);
                    }
                    catch (System.Exception caught__456)
                    {
                        var E = Aow.Rtl.Rtl.Wrap(caught__456);
                        FPropertyTable.RestoreSettings();
                        if (!HandleException(E))
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    FBuffer.Write(Aow.Rtl.Bytes.Of(ref AClassID, 4));
                    FPropertyTable.Add(ID, FBuffer.Position);
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((((AnsiString)"Double priorityID found (") + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:10542
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            Aow.Units.Engine.TEObject C = default;
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
                        bool bool__460 = (C != null) && (C.Owner == Node);
                        if (bool__460)
                        {
                            var call__461 = C.GetControlStyle();
                            bool__460 = !call__461.Contains(Aow.Units.Engine.Engine.csShared);
                        }

                        bool bool__462 = bool__460;
                        if (bool__462)
                        {
                            var call__463 = C.GetControlStyle();
                            bool__462 = call__463.Contains(Aow.Units.Engine.Engine.csStored);
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
                throw Aow.Units.Engine.EDoublePrioriyID.Create((((AnsiString)"Double priorityID found (") + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:10565
        public override void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
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
                    var E = Aow.Rtl.Rtl.Wrap(caught__465);
                    FPropertyTable.RestoreSettings();
                    if (!HandleException(E))
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw Aow.Units.Engine.EDoublePrioriyID.Create((((((AnsiString)"Double priorityID found (") + ((AnsiString)TObject.ClassNameOf(Obj.GetType()))) + AnsiString.FromBytes(58)) + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
        }

        // Engine.pas:10587
        public override void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            int Size = default;
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                FBuffer.Write(Aow.Rtl.Bytes.Of(ref Size, 4));
                FPropertyTable.SaveToStream(FBuffer);
                FBuffer.Write(Aow.Rtl.Bytes.Of(ref Size, 4));
                FPropertyTable.RestoreSettings();
            }
            catch (System.Exception caught__466)
            {
                var E = Aow.Rtl.Rtl.Wrap(caught__466);
                FPropertyTable.RestoreSettings();
                Size = 0;
                FBuffer.Write(Aow.Rtl.Bytes.Of(ref Size, 4));
                if (!HandleException(E))
                {
                    throw;
                }
            }
        }

        // Engine.pas:1285
        public static new Aow.Units.Engine.TDebugWriteStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.Engine.TDebugWriteStream o = new Aow.Units.Engine.TDebugWriteStream();
            try
            {
                o.Create__init_TDebugWriteStream(AStream, AEngine);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TTranslateStorageStream (instance size 36): visits every string of an object tree
//     through TranslateString and TranslateStrings; nothing is read or written. }
    public partial class TTranslateStorageStream : Aow.Units.Engine.TEStorageStream
    {
        // Engine.pas:10825
        // { TTranslateStorageStream }
        public void Create__init_TTranslateStorageStream(Aow.Units.Engine.TEngine AEngine)
        {
            base.Create__init_TEStorageStream(null, AEngine);
            var call__467 = Aow.Units.Engine.TPropertyTable.Create();
            FPropertyTable = call__467;
        }

        // Engine.pas:10831
        public override void Destroy()
        {
            FPropertyTable.Free();
            base.Destroy();
        }

        // Engine.pas:10837
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.Engine.smText, Aow.Units.Engine.TStorageModeElement.smTranslate);
            return Result;
        }

        // Engine.pas:10842
        public override void rwString(ref Aow.Rtl.AnsiString S, int ID)
        {
            TranslateString(ref S);
        }

        // Engine.pas:10847
        public override void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
            TranslateStrings(ref Strings);
        }

        // Engine.pas:10852
        public override void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
        {
            TranslateStrings(ref Strings);
        }

        // Engine.pas:10857
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            Aow.Units.Engine.TEObject O = default;
            // { Match trials (4): named temporaries, with scopes, Pointer casts, zero-shift scheduling; still differs. }
            O = Obj;
            if (O != null)
            {
                rwEObject(O, ID);
            }
        }

        // Engine.pas:10865
        public override void rwEObject(Aow.Units.Engine.TEObject Obj, int ID)
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
                    var call__468 = Obj.ClassType.ClassID();
                    FOnError(this, (((AnsiString)"Error reading object (cid:") + Aow.Rtl.Rtl.IntToStr(call__468)) + AnsiString.FromBytes(41), ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }

        // Engine.pas:10882
        public override void rwEObjectStream(Aow.Units.Engine.TEObject Obj, int ID)
        {
            rwEObject(Obj, ID);
        }

        // Engine.pas:10887
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            int I = default;
            Aow.Units.Engine.TEObject C = default;
            {
                I = 0;
                int end__469 = Node.Count - 1;
                for (; I <= end__469; I += 1)
                {
                    var call__470 = Node.Children[I];
                    C = call__470;
                    bool bool__471 = (C != null) && (C.Owner == Node);
                    if (bool__471)
                    {
                        var call__472 = C.GetControlStyle();
                        bool__471 = !call__472.Contains(Aow.Units.Engine.Engine.csShared);
                    }

                    bool bool__473 = bool__471;
                    if (bool__473)
                    {
                        var call__474 = C.GetControlStyle();
                        bool__473 = call__474.Contains(Aow.Units.Engine.Engine.csStored);
                    }

                    if (bool__473)
                    {
                        rwEObject(C, I);
                    }
                }
            }
        }

        // Engine.pas:10901
        public virtual void TranslateStrings(ref Aow.Rtl.TStrings Strings)
        {
        }

        // Engine.pas:10905
        public virtual void TranslateString(ref Aow.Rtl.AnsiString S)
        {
        }

        // Engine.pas:1343
        public static Aow.Units.Engine.TTranslateStorageStream Create(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.Engine.TTranslateStorageStream o = new Aow.Units.Engine.TTranslateStorageStream();
            try
            {
                o.Create__init_TTranslateStorageStream(AEngine);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Engine.TSetupCompareStorageStream (instance size 44): a read stream that announces a
//     comparison to come (smSetupCompare) }
    public partial class TSetupCompareStorageStream : Aow.Units.Engine.TEReadStorageStream
    {
        // Engine.pas:10911
        // { TSetupCompareStorageStream }
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.TStorageModeElement.smSetupCompare);
            return Result;
        }
    }

    // { Engine.TCompareStorageStream (instance size 44): reads a stored copy and calls Mismatch for
//     every property that differs from the object; Mismatch raises and swallows its own exception,
//     a place for a breakpoint. }
    public partial class TCompareStorageStream : Aow.Units.Engine.TEReadStorageStream
    {
        // Engine.pas:11002
        internal void Mismatch(int ID)
        {
            try
            {
                throw Aow.Rtl.Exception.Create((((AnsiString)"Property Mismatch (PropertyID:") + Aow.Rtl.Rtl.IntToStr(ID)) + AnsiString.FromBytes(41));
            }
            catch (System.Exception)
            {
            }
        }

        // Engine.pas:10918
        // { TCompareStorageStream }
        public void Create__init_TCompareStorageStream(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            base.Create__init_TEReadStorageStream(AStream, AEngine, false);
        }

        // Engine.pas:10923
        public override void Destroy()
        {
            base.Destroy();
        }

        // Engine.pas:10928
        public override TSet<Aow.Units.Engine.TStorageModeElement> StorageMode()
        {
            TSet<Aow.Units.Engine.TStorageModeElement> Result = default;
            Result = TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.TStorageModeElement.smCompare);
            return Result;
        }

        // Engine.pas:10933
        public override void rwCreateEObject(ref Aow.Units.Engine.TEObject Obj, Aow.Units.Engine.TECustomNode AOwner, int ID)
        {
            int Offset = default;
            int SavedBase = default;
            int AClassID = default;
            var call__475 = FPropertyTable.FindOffset(ID);
            Offset = call__475;
            if ((Offset != -1) && (Obj != null))
            {
                var call__476 = FStream.Position;
                if (call__476 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FStream.Read(Aow.Rtl.Bytes.Of(ref AClassID, 4));
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
                        var E = Aow.Rtl.Rtl.Wrap(caught__478);
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
        public override void rwChildren(Aow.Units.Engine.TECustomNode Node, int ID)
        {
            int SavedBase = default;
            int I = default;
            int Start = default;
            Aow.Units.Engine.TEObject C = default;
            int Offset = default;
            int Index = default;
            var call__479 = FPropertyTable.FindOffset(ID);
            Offset = call__479;
            if (Offset != -1)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                var call__480 = FStream.Position;
                if (call__480 != (FDataBase + Offset))
                {
                    FStream.Position = FDataBase + Offset;
                }

                FPropertyTable.LoadFromStream(FStream);
                var call__481 = FStream.Position;
                FDataBase = call__481;
                if (Node.GetControlStyle().Contains(Aow.Units.Engine.Engine.csKeepIndex))
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
                        bool bool__485 = (Start < call__484);
                        if (bool__485)
                        {
                            var call__486 = Node.Children[Start];
                            bool__485 = (call__486 != null);
                        }

                        bool bool__487 = bool__485;
                        if (bool__487)
                        {
                            var call__488 = Node.Children[Start];
                            var call__489 = call__488.GetControlStyle();
                            bool__487 = ((call__489 & TSet<Aow.Units.Engine.TEControlStyleElement>.Of(Aow.Units.Engine.Engine.csShared)) != TSet<Aow.Units.Engine.TEControlStyleElement>.Of());
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
                                var call__492 = Node.Children[(I - FPropertyTable.First) + Start];
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
            Ptr<byte> P = default;
            P = default;
            P = P.Realloc(Size);
            Aow.Rtl.Rtl.FillBytes(P.AsSpan(Size), 0);
            base.rwData(P.AsSpan(Size), Size, ID);
            if (!Aow.Rtl.Rtl.CompareMem(P.AsSpan(Size), Buffer.Slice(0, Size), Size))
            {
                Mismatch(ID);
            }

            P = P.Realloc(0);
        }

        // Engine.pas:11022
        public override void rwString(ref Aow.Rtl.AnsiString S, int ID)
        {
            Aow.Rtl.AnsiString Value = default;
            base.rwString(ref Value, ID);
            if (Value != S)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11030
        public override void rwStrings(Aow.Rtl.TStrings Strings, int ID)
        {
            Aow.Rtl.TStringList L = default;
            // { the original never frees L }
            L = new Aow.Rtl.TStringList();
            base.rwStrings(L, ID);
            var call__493 = Strings.Text;
            var call__494 = L.Text;
            if (Aow.Rtl.Rtl.CompareText(call__494, call__493) != 0)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11040
        public override void rwStringsEx(Aow.Rtl.TStrings Strings, int ID)
        {
            Aow.Rtl.TStringList L = default;
            L = new Aow.Rtl.TStringList();
            base.rwStrings(L, ID);
            var call__495 = Strings.Text;
            var call__496 = L.Text;
            if (Aow.Rtl.Rtl.CompareText(call__496, call__495) != 0)
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
            base.rwByteEx(ref Value, unchecked((byte)Default), ID);
            if (B != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11113
        public override void rwWordEx(ref ushort W, ushort Default, int ID)
        {
            ushort Value = default;
            base.rwWordEx(ref Value, unchecked((ushort)Default), ID);
            if (W != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:11105
        public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
        {
            sbyte Value = default;
            base.rwShortintEx(ref Value, unchecked((sbyte)Default), ID);
            if (S != Value)
            {
                Mismatch(ID);
            }
        }

        // Engine.pas:1371
        public static new Aow.Units.Engine.TCompareStorageStream Create(Aow.Rtl.TStream AStream, Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.Engine.TCompareStorageStream o = new Aow.Units.Engine.TCompareStorageStream();
            try
            {
                o.Create__init_TCompareStorageStream(AStream, AEngine);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public delegate int TStringsSortCompare(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2);
    public struct TStringHashItem
    {
        public Aow.Rtl.AnsiString Key;
        public object Item;
    }

    public delegate void TExchangeProc(Aow.Rtl.TList Data, int I, int J);
    public struct TSortStringsContext
    {
        public Aow.Rtl.TStrings Strings;
        public Aow.Units.Engine.TStringsSortCompare Compare;
        public Aow.Units.Engine.TExchangeProc ExchangeProc;
        public Aow.Rtl.TList ExchangeData;
    }

    // { TECopyComponent }
    // { the layout of TList, to write its count field straight from the object }
    public partial class TListFields : Aow.Rtl.TObject
    {
        public object FList;
        public int FCount;
    }

    public static partial class Engine
    {
        // { The Engine unit of Enginep.dpl written for the original compiler (Delphi 3.02), class by class:
//   tools/match.py compares the code DCC32 generates for every method with the code in the game's
//   module. Declarations follow the original instance layouts and VMT orders (data/classes.json); a
//   method whose body is still a stub is marked "stub". The hash classes come from
//   inc\HashTab.PAS as in the original. }
        public const int MaxListSize = 134217727;
        // { Compatibility names used by the recovered dependent packages. }
        public const Aow.Units.Engine.TEControlStyleElement csStored = (Aow.Units.Engine.TEControlStyleElement)0;
        public const Aow.Units.Engine.TEControlStyleElement csShared = (Aow.Units.Engine.TEControlStyleElement)1;
        public const Aow.Units.Engine.TEControlStyleElement csReserved2 = (Aow.Units.Engine.TEControlStyleElement)2;
        public const Aow.Units.Engine.TEControlStyleElement csKeepIndex = (Aow.Units.Engine.TEControlStyleElement)3;
        public const Aow.Units.Engine.TEngineModeElement eoVerifyClasses = (Aow.Units.Engine.TEngineModeElement)0;
        public const Aow.Units.Engine.TStorageModeElement smText = (Aow.Units.Engine.TStorageModeElement)2;
        public const Aow.Units.Engine.TStorageModeElement smPartial = (Aow.Units.Engine.TStorageModeElement)3;
        // Engine.pas:1953
        // { Strings }
        public static Aow.Rtl.AnsiString ReplaceText(Aow.Rtl.AnsiString S, Aow.Rtl.AnsiString Old, Aow.Rtl.AnsiString New)
        {
            Aow.Rtl.AnsiString Result = default;
            int P = default;
            Result = (AnsiString)"";
            P = Aow.Rtl.Rtl.Pos(Old, S);
            while (P != 0)
            {
                Result = (Result + Aow.Rtl.Rtl.Copy(S, 1, P - 1)) + New;
                S = Aow.Rtl.Rtl.Copy(S, P + Old.Length, ((S.Length - P) - Old.Length) + 1);
                P = Aow.Rtl.Rtl.Pos(Old, S);
            }

            Result = Result + S;
            return Result;
        }

        // Engine.pas:2005
        public static void SortStrings(Aow.Rtl.TStrings Strings, Aow.Units.Engine.TStringsSortCompare Compare)
        {
            Aow.Units.Engine.TSortStringsContext Ctx = default;
            var call__497 = Strings.Count;
            if (call__497 != 0)
            {
                Ctx.Strings = Strings;
                Ctx.Compare = Compare;
                Ctx.ExchangeProc = null;
                var call__498 = Strings.Count;
                Aow.Units.Engine.Engine.QuickSortStrings(ref Ctx, 0, call__498 - 1);
            }
        }

        // Engine.pas:2025
        public static void SortStringsEx(Aow.Rtl.TStrings Strings, Aow.Units.Engine.TStringsSortCompare Compare, ReadOnlySpan<Aow.Rtl.TStrings> LinkedStrings)
        {
            Aow.Rtl.TList Lists = default;
            int I = default;
            Aow.Units.Engine.TSortStringsContext Ctx = default;
            var call__499 = Strings.Count;
            if (call__499 != 0)
            {
                Strings.BeginUpdate();
                Ctx.Strings = Strings;
                Ctx.Compare = Compare;
                // { Original unsigned test also takes an empty array. }
                if (unchecked((uint)(LinkedStrings.Length - 1)) >= unchecked((uint)0))
                {
                    Lists = new Aow.Rtl.TList();
                    {
                        I = 0;
                        int end__500 = LinkedStrings.Length - 1;
                        for (; I <= end__500; I += 1)
                        {
                            LinkedStrings[I].BeginUpdate();
                            Lists.Add(LinkedStrings[I]);
                        }
                    }

                    Ctx.ExchangeProc = Aow.Units.Engine.Engine.ExchangeLinkedStrings;
                    Ctx.ExchangeData = Lists;
                }
                else
                {
                    Lists = null;
                }

                var call__501 = Strings.Count;
                Aow.Units.Engine.Engine.QuickSortStrings(ref Ctx, 0, call__501 - 1);
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
        public static void SortStringsAscending(Aow.Rtl.TStrings Strings, ReadOnlySpan<Aow.Rtl.TStrings> LinkedStrings)
        {
            Aow.Units.Engine.Engine.SortStringsEx(Strings, Aow.Units.Engine.Engine.CompareAscending, LinkedStrings);
        }

        // Engine.pas:2076
        public static void SortStringsDescending(Aow.Rtl.TStrings Strings, ReadOnlySpan<Aow.Rtl.TStrings> LinkedStrings)
        {
            Aow.Units.Engine.Engine.SortStringsEx(Strings, Aow.Units.Engine.Engine.CompareDescending, LinkedStrings);
        }

        // Engine.pas:2090
        public static void SortStringsNumericalAscending(Aow.Rtl.TStrings Strings, ReadOnlySpan<Aow.Rtl.TStrings> LinkedStrings)
        {
            Aow.Units.Engine.Engine.SortStringsEx(Strings, Aow.Units.Engine.Engine.CompareNumericalAscending, LinkedStrings);
        }

        // Engine.pas:2104
        public static void SortStringsNumericalDescending(Aow.Rtl.TStrings Strings, ReadOnlySpan<Aow.Rtl.TStrings> LinkedStrings)
        {
            Aow.Units.Engine.Engine.SortStringsEx(Strings, Aow.Units.Engine.Engine.CompareNumericalDescending, LinkedStrings);
        }

        // Engine.pas:2109
        public static int HexToInt(Aow.Rtl.AnsiString S)
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
                    C = unchecked((byte)S[L - I]);
                    if (TSet<byte>.WithBounds(0, 255).WithRange(48, 57).Contains(C))
                    {
                        D = ((int)C) - ((int)48);
                    }
                    else
                    {
                        if (TSet<byte>.WithBounds(0, 255).WithRange(65, 70).Contains(C))
                        {
                            D = (((int)C) - ((int)65)) + 10;
                        }
                        else
                        {
                            if (TSet<byte>.WithBounds(0, 255).WithRange(97, 102).Contains(C))
                            {
                                D = (((int)C) - ((int)97)) + 10;
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
        public static void RegisterPlugIn(Aow.Units.Engine.TPlugIn PlugIn)
        {
            int I = default;
            bool Found = default;
            Found = false;
            I = 0;
            while (!Found && (I != Aow.Units.Engine.Engine.PlugIns.Count))
            {
                var call__504 = Aow.Units.Engine.Engine.PlugIns[I];
                var call__505 = ((Aow.Units.Engine.TPlugIn)call__504).Name();
                var call__506 = PlugIn.Name();
                Found = call__505 == call__506;
                I += 1;
            }

            if (!Found)
            {
                Aow.Units.Engine.Engine.PlugIns.Add(PlugIn);
            }
            else
            {
                var call__507 = PlugIn.Name();
                var call__508 = Aow.Rtl.Exception.Create((((AnsiString)"PlugIn ") + call__507) + ((AnsiString)" already registered"));
                throw call__508;
            }
        }

        // Engine.pas:9242
        public static void UnregisterPlugIn(Aow.Units.Engine.TPlugIn PlugIn)
        {
            var call__509 = Aow.Units.Engine.Engine.PlugIns.IndexOf(PlugIn);
            if (call__509 != -1)
            {
                Aow.Units.Engine.Engine.PlugIns.Remove(PlugIn);
            }
            else
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"PlugIn not found");
            }
        }

        // Engine.pas:9337
        public static void InitializePlugIns(Aow.Units.Engine.TEngine AEngine)
        {
            int I = default;
            {
                I = 0;
                int end__510 = Aow.Units.Engine.Engine.PlugIns.Count - 1;
                for (; I <= end__510; I += 1)
                {
                    var call__511 = Aow.Units.Engine.Engine.PlugIns[I];
                    ((Aow.Units.Engine.TPlugIn)call__511).Initialize(AEngine);
                }
            }
        }

        // Engine.pas:9344
        public static void FinalizePlugIns(Aow.Units.Engine.TEngine AEngine)
        {
            int I = default;
            {
                I = 0;
                int end__512 = Aow.Units.Engine.Engine.PlugIns.Count - 1;
                for (; I <= end__512; I += 1)
                {
                    var call__513 = Aow.Units.Engine.Engine.PlugIns[I];
                    ((Aow.Units.Engine.TPlugIn)call__513).Finalize(AEngine);
                }
            }
        }

        // Engine.pas:9351
        public static Aow.Units.Engine.TPlugIn GetPlugIn(int Index)
        {
            Aow.Units.Engine.TPlugIn Result = default;
            if (((Aow.Units.Engine.Engine.PlugIns != null) && (Index >= 0)) && (Index < Aow.Units.Engine.Engine.PlugIns.Count))
            {
                var call__514 = Aow.Units.Engine.Engine.PlugIns[Index];
                Result = (Aow.Units.Engine.TPlugIn)call__514;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:9359
        public static Aow.Units.Engine.TPlugIn FindPlugIn(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Engine.TPlugIn Result = default;
            int I = default;
            Result = null;
            if (Aow.Units.Engine.Engine.PlugIns != null)
            {
                I = 0;
                while ((I != Aow.Units.Engine.Engine.PlugIns.Count) && (Result == null))
                {
                    var call__515 = Aow.Units.Engine.Engine.PlugIns[I];
                    var call__516 = ((Aow.Units.Engine.TPlugIn)call__515).Name();
                    if (call__516 == Name)
                    {
                        var call__517 = Aow.Units.Engine.Engine.PlugIns[I];
                        Result = (Aow.Units.Engine.TPlugIn)call__517;
                    }

                    I += 1;
                }
            }

            return Result;
        }

        // Engine.pas:9375
        public static void LoadExternalPlugIns(Aow.Rtl.AnsiString Directory)
        {
            Aow.Rtl.AnsiString Dir = default;
            void LoadFrom(Aow.Rtl.AnsiString Dir)
            {
                Aow.Rtl.TSearchRec SR = default;
                Aow.Rtl.TStringList Dirs = default;
                int I = default;
                bool Found = default;
                int H = default;
                Found = Aow.Rtl.Rtl.FindFirst(Dir + ((AnsiString)"*.DPL"), 63 - 16, ref SR) == 0;
                while (Found)
                {
                    try
                    {
                        /* TODO(T-5.4) Native Delphi packages have no managed loader */
                        throw new System.NotImplementedException();
                        Aow.Units.Engine.Engine.Packages.AddObject(Dir + SR.Name, Aow.Rtl.Rtl.IntToObject((int)H));
                    }
                    catch (System.Exception)
                    {
                        var call__518 = Aow.Rtl.Exception.Create((((AnsiString)"Unable to load PlugIn: ") + Dir) + SR.Name);
                        throw call__518;
                    }

                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
                Dirs = new Aow.Rtl.TStringList();
                Found = Aow.Rtl.Rtl.FindFirst(Dir + ((AnsiString)"*.*"), 16, ref SR) == 0;
                while (Found)
                {
                    if ((((SR.Attr & 16) != 0) && (SR.Name != AnsiString.FromBytes(46))) && (SR.Name != ((AnsiString)"..")))
                    {
                        Dirs.Add((Dir + SR.Name) + AnsiString.FromBytes(92));
                    }

                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
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

            Dir = Aow.Rtl.Rtl.ExpandFileName(Directory);
            // { the original tests the character of Directory, not of Dir }
            if ((Dir != ((AnsiString)"")) && (Directory[Dir.Length] != 92))
            {
                Dir = Dir + AnsiString.FromBytes(92);
            }

            LoadFrom(Dir);
        }

        // Engine.pas:9419
        public static void UnloadExternalPlugIns()
        {
            int I = default;
            Aow.Rtl.AnsiString S = default;
            {
                I = Aow.Units.Engine.Engine.Packages.Count - 1;
                byte end__522 = 0;
                for (; I >= end__522; I -= 1)
                {
                    try
                    {
                        var call__524 = Aow.Units.Engine.Engine.Packages[I];
                        S = call__524;
                        // { the original hands the name, not the handle in Objects, to UnloadPackage }
                        /* TODO(T-5.4) Native Delphi packages have no managed loader */
                        throw new System.NotImplementedException();
                        Aow.Units.Engine.Engine.Packages.Delete(I);
                    }
                    catch (System.Exception)
                    {
                        var call__523 = Aow.Units.Engine.Engine.Packages[I];
                        S = ((AnsiString)"Could not release PlugIn: ") + call__523;
                        throw Aow.Rtl.Exception.Create(S);
                    }
                }
            }

            Aow.Units.Engine.Engine.Packages.Clear();
        }

        // Engine.pas:9441
        // { Messages and engines }
        public static void InitMsg(ref Aow.Units.Engine.TEMsg Msg, int MsgID, Aow.Units.Engine.TEObject Sender)
        {
            Msg = default;
            Msg.MsgID = MsgID;
            Msg.Sender = Sender;
            Msg.Result = -1;
        }

        // Engine.pas:9449
        public static void InitMsgEx(ref Aow.Units.Engine.TEMsg Msg, int MsgID, Aow.Units.Engine.TEObject Sender, int Seq)
        {
            Msg = default;
            Msg.MsgID = MsgID;
            Msg.Seq = Seq;
            Msg.Sender = Sender;
            Msg.Result = -1;
        }

        // Engine.pas:3086
        public static int GetCRC32(Ptr<byte> P, int Size)
        {
            int Result = default;
            if (Aow.Units.Engine.Engine.CRC32Table.IsNull)
            {
                Aow.Units.Engine.Engine.BuildCRC32Table();
            }

            Result = -1;
            while (Size != 0)
            {
                Result = Aow.Units.Engine.Engine.CRC32Table[(MemoryMarshal.Read<byte>(P.AsSpan(1)) ^ Result) & 255] ^ (((int)(((uint)Result) >> 8)) & 16777215);
                P = P + (1 * 1);
                Size -= 1;
            }

            Result = Result ^ -1;
            return Result;
        }

        // Engine.pas:3099
        public static int GetFileCRC32(Aow.Rtl.AnsiString FileName)
        {
            int Result = default;
            Aow.Rtl.TMemoryStream M = default;
            Aow.Rtl.TFileStream F = default;
            M = new Aow.Rtl.TMemoryStream();
            try
            {
                F = new Aow.Rtl.TFileStream(FileName, 0);
                try
                {
                    M.LoadFromStream(F);
                    var call__525 = M.Size;
                    var call__526 = Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory, 0), call__525);
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
            Aow.Units.Engine.Engine.CRC32Table = default;
        }

        // Engine.pas:9458
        public static Aow.Units.Engine.TEngine GetEngine(int Index)
        {
            Aow.Units.Engine.TEngine Result = default;
            object P = default;
            if (((Aow.Units.Engine.Engine.Engines != null) && (Index >= 0)) && (Index < Aow.Units.Engine.Engine.Engines.Count))
            {
                var call__527 = Aow.Units.Engine.Engine.Engines[Index];
                P = call__527;
                Result = (Aow.Units.Engine.TEngine)P;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Engine.pas:9466
        public static Aow.Units.Engine.TEngine FindEngine(int AClassID)
        {
            Aow.Units.Engine.TEngine Result = default;
            int I = default;
            bool Found = default;
            Found = false;
            Result = null;
            if (Aow.Units.Engine.Engine.Engines != null)
            {
                I = 0;
                Found = false;
                while (!Found && (I < Aow.Units.Engine.Engine.Engines.Count))
                {
                    var call__528 = Aow.Units.Engine.Engine.Engines[I];
                    var call__529 = ((Aow.Units.Engine.TEngine)call__528).ClassType.ClassID();
                    Found = AClassID == call__529;
                    I += 1;
                }

                if (Found)
                {
                    var call__530 = Aow.Units.Engine.Engine.Engines[I - 1];
                    Result = (Aow.Units.Engine.TEngine)call__530;
                }
            }

            if (!Found)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Cannot access Engine");
            }

            return Result;
        }

        // Engine.pas:2132
        public static void StringToStream(Aow.Rtl.AnsiString S, Aow.Rtl.TStream Stream)
        {
            int Len = default;
            Len = S.Length;
            Stream.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
            Stream.Write(Aow.Rtl.Rtl.StringBytes(S).Slice(0, Len));
        }

        // Engine.pas:2141
        public static void StringFromStream(ref Aow.Rtl.AnsiString S, Aow.Rtl.TStream Stream)
        {
            int Len = default;
            Stream.Read(Aow.Rtl.Bytes.Of(ref Len, 4));
            S = S.Resized(Len);
            Aow.Rtl.Rtl.ReadString(Stream, ref S, 1 - 1, Len, false);
        }

        // Engine.pas:2185
        public static void StringsToStream(Aow.Rtl.TStrings Strings, Aow.Rtl.TStream Stream)
        {
            Ptr<byte> P = default;
            int Len = default;
            P = Strings.Text.AsPtr();
            Len = Aow.Rtl.Rtl.StrLen(P);
            Stream.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
            Stream.WriteBuffer(P.Array.AsSpan(P.Index, Len));
            P = default;
        }

        // Engine.pas:2197
        public static void StringsFromStream(Aow.Rtl.TStrings Strings, Aow.Rtl.TStream Stream)
        {
            int Len = default;
            Aow.Rtl.AnsiString S = default;
            Strings.BeginUpdate();
            try
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Len, 4));
                S = default(AnsiString).Resized(Len);
                Aow.Rtl.Rtl.ReadString(Stream, ref S, 0, Len, false);
                Strings.Text = S;
            }
            finally
            {
                Strings.EndUpdate();
            }
        }

        public static bool VerifyEObjectEnabled;
        public static readonly Aow.Rtl.AnsiString EStreamReadErrorStr = (AnsiString)"Error Reading EObject";
        public static readonly Aow.Rtl.AnsiString EStreamWriteErrorStr = (AnsiString)"Error Writing EObject";
        public static readonly Aow.Rtl.AnsiString UserStr = (AnsiString)"User";
        public static Aow.Rtl.TList Engines;
        public static Aow.Rtl.TList PlugIns;
        public static Aow.Units.Engine.TEngine CopyEngine;
        public static Aow.Rtl.TStringList Packages;
        public static uint ClipboardFormat;
        // Engine.pas:1570
        public static void SortItems(Ptr<Aow.Units.Engine.TSortItem> SortList, int L, int R)
        {
            int I = default;
            int J = default;
            int P = default;
            Aow.Units.Engine.TSortItem T = default;
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
                    Aow.Units.Engine.Engine.SortItems(SortList, L, J);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:1602
        public static void InverseSortItems(Ptr<Aow.Units.Engine.TSortItem> SortList, int L, int R)
        {
            int I = default;
            int J = default;
            int P = default;
            Aow.Units.Engine.TSortItem T = default;
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
                    Aow.Units.Engine.Engine.InverseSortItems(SortList, L, J);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // { TCompressStream }
        public static Aow.Units.FixedArray_byte_4 CompressMagic = Aow.Units.FixedArray_byte_4.From([67, 70, 83, 0]);
        // Engine.pas:1977
        public static void QuickSortStrings(ref Aow.Units.Engine.TSortStringsContext Ctx, int L, int R)
        {
            int I = default;
            int J = default;
            int M = default;
            Aow.Rtl.TObject PO = default;
            Aow.Rtl.AnsiString P = default;
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
                    Aow.Units.Engine.Engine.QuickSortStrings(ref Ctx, L, J);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:2018
        public static void ExchangeLinkedStrings(Aow.Rtl.TList Lists, int I, int J)
        {
            int K = default;
            {
                K = Lists.Count - 1;
                byte end__539 = 0;
                for (; K >= end__539; K -= 1)
                {
                    ((Aow.Rtl.TStrings)Lists.List[K]).Exchange(I, J);
                }
            }
        }

        // Engine.pas:2059
        public static int CompareAscending(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2)
        {
            int Result = default;
            Result = Aow.Rtl.Rtl.CompareText(S1, S2);
            if (Result == 0)
            {
                Result = S1.Length - S2.Length;
            }

            return Result;
        }

        // Engine.pas:2070
        public static int CompareDescending(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2)
        {
            int Result = default;
            Result = Aow.Rtl.Rtl.CompareText(S2, S1);
            if (Result == 0)
            {
                Result = S2.Length - S1.Length;
            }

            return Result;
        }

        // Engine.pas:2081
        public static int CompareNumericalAscending(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2)
        {
            int Result = default;
            int V1 = default;
            int V2 = default;
            V1 = Aow.Rtl.Rtl.StrToIntDef(S1, -1);
            V2 = Aow.Rtl.Rtl.StrToIntDef(S2, -1);
            Result = V1 - V2;
            return Result;
        }

        // Engine.pas:2095
        public static int CompareNumericalDescending(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2)
        {
            int Result = default;
            int V1 = default;
            int V2 = default;
            V1 = Aow.Rtl.Rtl.StrToIntDef(S1, -1);
            V2 = Aow.Rtl.Rtl.StrToIntDef(S2, -1);
            Result = V2 - V1;
            return Result;
        }

        // Engine.pas:2150
        public static void SaveStringToStream(Aow.Rtl.AnsiString S, Aow.Rtl.TStream Stream)
        {
            int Len = default;
            byte B = default;
            Len = S.Length;
            if (Len < 255)
            {
                B = unchecked((byte)Len);
                Stream.Write(Aow.Rtl.Bytes.Of(ref B, 1));
            }
            else
            {
                B = 255;
                Stream.Write(Aow.Rtl.Bytes.Of(ref B, 1));
                Stream.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
            }

            Stream.Write(Aow.Rtl.Rtl.StringBytes(S).Slice(0, Len));
        }

        // Engine.pas:2170
        public static void LoadStringFromStream(ref Aow.Rtl.AnsiString S, Aow.Rtl.TStream Stream)
        {
            int Len = default;
            byte B = default;
            S = (AnsiString)"";
            Stream.Read(Aow.Rtl.Bytes.Of(ref B, 1));
            if (B == 255)
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Len, 4));
            }
            else
            {
                Len = B;
            }

            S = S.Resized(Len);
            Aow.Rtl.Rtl.ReadString(Stream, ref S, 1 - 1, Len, false);
        }

        // Engine.pas:2213
        public static void StringsToStreamEx(Aow.Rtl.TStrings Strings, Aow.Rtl.TStream Stream)
        {
            Ptr<byte> P = default;
            int Len = default;
            int I = default;
            int Obj = default;
            P = Strings.Text.AsPtr();
            Len = Aow.Rtl.Rtl.StrLen(P);
            Stream.Write(Aow.Rtl.Bytes.Of(ref Len, 4));
            Stream.WriteBuffer(P.Array.AsSpan(P.Index, Len));
            P = default;
            {
                I = 0;
                int end__540 = Strings.Count - 1;
                for (; I <= end__540; I += 1)
                {
                    var call__541 = Strings.Objects[I];
                    Obj = Aow.Rtl.Rtl.ObjectToInt(call__541);
                    Stream.Write(Aow.Rtl.Bytes.Of(ref Obj, 4));
                }
            }
        }

        // Engine.pas:2230
        public static void StringsFromStreamEx(Aow.Rtl.TStrings Strings, Aow.Rtl.TStream Stream)
        {
            int Len = default;
            int I = default;
            int Obj = default;
            Aow.Rtl.AnsiString S = default;
            Strings.BeginUpdate();
            try
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Len, 4));
                S = default(AnsiString).Resized(Len);
                Aow.Rtl.Rtl.ReadString(Stream, ref S, 0, Len, false);
                Strings.Text = S;
                {
                    I = 0;
                    int end__542 = Strings.Count - 1;
                    for (; I <= end__542; I += 1)
                    {
                        Stream.Read(Aow.Rtl.Bytes.Of(ref Obj, 4));
                        Strings.Objects[I] = Aow.Rtl.Rtl.IntToObject((int)Obj);
                    }
                }
            }
            finally
            {
                Strings.EndUpdate();
            }
        }

        // { CRC32 }
        public static Ptr<int> CRC32Table;
        // Engine.pas:3061
        public static void BuildCRC32Table()
        {
            int I = default;
            int J = default;
            int C = default;
            int Polynomial = default;
            if (Aow.Units.Engine.Engine.CRC32Table.IsNull)
            {
                Aow.Units.Engine.Engine.CRC32Table = Ptr<int>.Alloc(256);
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

                                Aow.Units.Engine.Engine.CRC32Table[I] = C;
                            }
                        }
                    }
                }
            }
        }

        // Engine.pas:3438
        public static void QuickSortInteger(Ptr<int> SortList, int L, int R, Aow.Units.Engine.TIntegerSortCompare SCompare)
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
                    Aow.Units.Engine.Engine.QuickSortInteger(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:3773
        public static void QuickSortByte(Ptr<byte> SortList, int L, int R, Aow.Units.Engine.TByteSortCompare SCompare)
        {
            int I = default;
            int J = default;
            byte P = default;
            byte T = default;
            do
            {
                I = L;
                J = R;
                P = unchecked((byte)SortList[(int)(((uint)(L + R)) >> 1)]);
                do
                {
                    while (true)
                    {
                        var call__547 = SCompare(unchecked((byte)SortList[I]), unchecked((byte)P));
                        if (!(call__547 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__548 = SCompare(unchecked((byte)SortList[J]), unchecked((byte)P));
                        if (!(call__548 > 0))
                            break;
                        {
                            J -= 1;
                        }
                    }

                    if (I <= J)
                    {
                        T = unchecked((byte)SortList[I]);
                        SortList[I] = unchecked((byte)SortList[J]);
                        SortList[J] = unchecked((byte)T);
                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    Aow.Units.Engine.Engine.QuickSortByte(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:4120
        public static void QuickSortShortInt(Ptr<sbyte> SortList, int L, int R, Aow.Units.Engine.TShortIntSortCompare SCompare)
        {
            int I = default;
            int J = default;
            sbyte P = default;
            sbyte T = default;
            do
            {
                I = L;
                J = R;
                P = unchecked((sbyte)SortList[(int)(((uint)(L + R)) >> 1)]);
                do
                {
                    while (true)
                    {
                        var call__549 = SCompare(unchecked((sbyte)SortList[I]), unchecked((sbyte)P));
                        if (!(call__549 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__550 = SCompare(unchecked((sbyte)SortList[J]), unchecked((sbyte)P));
                        if (!(call__550 > 0))
                            break;
                        {
                            J -= 1;
                        }
                    }

                    if (I <= J)
                    {
                        T = unchecked((sbyte)SortList[I]);
                        SortList[I] = unchecked((sbyte)SortList[J]);
                        SortList[J] = unchecked((sbyte)T);
                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    Aow.Units.Engine.Engine.QuickSortShortInt(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:4431
        public static void QuickSortWord(Ptr<ushort> SortList, int L, int R, Aow.Units.Engine.TWordSortCompare SCompare)
        {
            int I = default;
            int J = default;
            ushort P = default;
            ushort T = default;
            do
            {
                I = L;
                J = R;
                P = unchecked((ushort)SortList[(int)(((uint)(L + R)) >> 1)]);
                do
                {
                    while (true)
                    {
                        var call__551 = SCompare(unchecked((ushort)SortList[I]), unchecked((ushort)P));
                        if (!(call__551 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__552 = SCompare(unchecked((ushort)SortList[J]), unchecked((ushort)P));
                        if (!(call__552 > 0))
                            break;
                        {
                            J -= 1;
                        }
                    }

                    if (I <= J)
                    {
                        T = unchecked((ushort)SortList[I]);
                        SortList[I] = unchecked((ushort)SortList[J]);
                        SortList[J] = unchecked((ushort)T);
                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    Aow.Units.Engine.Engine.QuickSortWord(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:5254
        public static void QuickSortDoubleItem(Ptr<Aow.Units.Engine.TDoubleItem> SortList, int L, int R, Aow.Units.Engine.TDoubleItemSortCompare SCompare)
        {
            int I = default;
            int J = default;
            Aow.Units.Engine.TDoubleItem P = default;
            Aow.Units.Engine.TDoubleItem T = default;
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
                    Aow.Units.Engine.Engine.QuickSortDoubleItem(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:5287
        public static int CompareDoubleItemAAscending(in Aow.Units.Engine.TDoubleItem Item1, in Aow.Units.Engine.TDoubleItem Item2)
        {
            int Result = default;
            Result = Item1.A - Item2.A;
            return Result;
        }

        // Engine.pas:5292
        public static int CompareDoubleItemBAscending(in Aow.Units.Engine.TDoubleItem Item1, in Aow.Units.Engine.TDoubleItem Item2)
        {
            int Result = default;
            Result = Item1.B - Item2.B;
            return Result;
        }

        // Engine.pas:5297
        public static int CompareDoubleItemADescending(in Aow.Units.Engine.TDoubleItem Item1, in Aow.Units.Engine.TDoubleItem Item2)
        {
            int Result = default;
            Result = Item2.A - Item1.A;
            return Result;
        }

        // Engine.pas:5302
        public static int CompareDoubleItemBDescending(in Aow.Units.Engine.TDoubleItem Item1, in Aow.Units.Engine.TDoubleItem Item2)
        {
            int Result = default;
            Result = Item2.B - Item1.B;
            return Result;
        }

        // Engine.pas:5664
        public static void QuickSortQuadItem(Ptr<Aow.Units.Engine.TQuadInteger> SortList, int L, int R, Aow.Units.Engine.TQuadItemSortCompare SCompare)
        {
            int I = default;
            int J = default;
            Aow.Units.Engine.TQuadInteger P = default;
            Aow.Units.Engine.TQuadInteger T = default;
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
                    Aow.Units.Engine.Engine.QuickSortQuadItem(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        // Engine.pas:5697
        public static int CompareQuadItemAAscending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.A - Item2.A;
            return Result;
        }

        // Engine.pas:5702
        public static int CompareQuadItemBAscending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.B - Item2.B;
            return Result;
        }

        // Engine.pas:5707
        public static int CompareQuadItemCAscending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.C - Item2.C;
            return Result;
        }

        // Engine.pas:5712
        public static int CompareQuadItemDAscending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item1.D - Item2.D;
            return Result;
        }

        // Engine.pas:5717
        public static int CompareQuadItemADescending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.A - Item1.A;
            return Result;
        }

        // Engine.pas:5722
        public static int CompareQuadItemBDescending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.B - Item1.B;
            return Result;
        }

        // Engine.pas:5727
        public static int CompareQuadItemCDescending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.C - Item1.C;
            return Result;
        }

        // Engine.pas:5732
        public static int CompareQuadItemDDescending(in Aow.Units.Engine.TQuadInteger Item1, in Aow.Units.Engine.TQuadInteger Item2)
        {
            int Result = default;
            Result = Item2.D - Item1.D;
            return Result;
        }

        // Engine.pas:10675
        // { "[id]=" at the start of a text line, or -1 }
        public static int GetLineID(Aow.Rtl.AnsiString S)
        {
            int Result = default;
            int I = default;
            I = 1;
            while (TSet<byte>.WithBounds(0, 255).With(91).WithRange(48, 57).Contains(S[I]))
            {
                I += 1;
            }

            if (Aow.Rtl.Rtl.Copy(S, I, 2) == ((AnsiString)"]="))
            {
                Result = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(S, 2, I - 2));
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Engine.pas:10685
        // { the text behind "[id]=" }
        public static Aow.Rtl.AnsiString GetLineValue(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            I = 1;
            while (TSet<byte>.WithBounds(0, 255).With(91).With(93).WithRange(48, 57).Contains(S[I]))
            {
                I += 1;
            }

            Result = Aow.Rtl.Rtl.Copy(S, I + 1, S.Length - I);
            return Result;
        }

        public static void Initialize()
        {
            Aow.Units.Engine.Engine.CRC32Table = default;
            Aow.Units.Engine.Engine.VerifyEObjectEnabled = true;
            var call__557 = new Aow.Rtl.TList();
            Aow.Units.Engine.Engine.Engines = call__557;
            var call__558 = new Aow.Rtl.TList();
            Aow.Units.Engine.Engine.PlugIns = call__558;
            var call__559 = new Aow.Rtl.TStringList();
            Aow.Units.Engine.Engine.Packages = call__559;
            var call__560 = Aow.Rtl.Rtl.RegisterClipboardFormat((AnsiString)"ECLIPBOARD FORMAT");
            Aow.Units.Engine.Engine.ClipboardFormat = unchecked((uint)call__560);
            Aow.Rtl.Rtl.RegisterClass(typeof(Aow.Units.Engine.TECopyComponent));
        }

        public static void Finalize()
        {
            Aow.Units.Engine.Engine.ReleaseCRC32Table();
            Aow.Units.Engine.Engine.UnloadExternalPlugIns();
            Aow.Units.Engine.Engine.Packages.Free();
            Aow.Units.Engine.Engine.Engines.Free();
            Aow.Units.Engine.Engine.PlugIns.Free();
        }
    }
}
