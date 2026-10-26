// Generated from HashTabU.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.HashTabU
{
    // { The HashTabU unit of Ilpack.dpl: the hash list and hash table classes, an older copy of the
//   ones the Engine unit includes from inc\HashTab.PAS (Enginep.dpl). This version has no Assert
//   calls, and TIntegerHashTable256 has neither DeleteSafe nor GetFreeID. }
    // { HashTabU.THashList (instance size 16): +4 FCount, +8 FCapacity, +$C FItems }
    public struct THashItem
    {
        public IntOrRef Key;
        public object Item;
    }

    public partial class THashList : Aow.Rtl.TObject
    {
        internal int FCount;
        internal int FCapacity;
        internal Ptr<Aow.Units.HashTabU.THashItem> FItems;
        // HashTabU.pas:120
        protected internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity != FCapacity)
            {
                FItems = FItems.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // HashTabU.pas:129
        protected internal void Delete(int Index)
        {
            FCount -= 1;
            if (Index < FCount)
            {
                FItems[Index] = FItems[Index + 1];
            }
        }

        // HashTabU.pas:136
        protected internal void Add(Aow.Units.HashTabU.THashItem Item)
        {
            if (FCapacity == FCount)
            {
                SetCapacity(FCapacity + 16);
            }

            FItems[FCount] = Item;
            FCount += 1;
        }

        // HashTabU.pas:108
        // { THashList }
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // HashTabU.pas:114
        public virtual void Clear()
        {
            FCount = 0;
            SetCapacity(0);
        }

        public int Count { get => FCount; }
    }

    // { HashTabU.TIntegerHashList (instance size 16): items keyed by an integer }
    public partial class TIntegerHashList : Aow.Units.HashTabU.THashList
    {
        // HashTabU.pas:145
        // { TIntegerHashList }
        public void Add(object Item, int Key)
        {
            Aow.Units.HashTabU.THashItem H = default;
            if (Find(Key) != null)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"ID already in HashTable");
            }

            H.Key = Key;
            H.Item = Item;
            base.Add(H);
        }

        // HashTabU.pas:155
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

        // HashTabU.pas:171
        public object Find(int Key)
        {
            object Result = default;
            int I = default;
            {
                I = FCount - 1;
                byte end__1 = 0;
                for (; I >= end__1; I -= 1)
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
        public static Aow.Units.HashTabU.TIntegerHashList Create()
        {
            return new Aow.Units.HashTabU.TIntegerHashList();
        }
    }

    // { HashTabU.TIntegerHashTable (instance size 4): the abstract table interface }
    public partial class TIntegerHashTable : Aow.Rtl.TObject
    {
        // HashTabU.pas:47
        public virtual void Add(object Item, int Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:48
        public virtual void Delete(int Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:49
        public virtual object Find(int Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:50
        public virtual void Clear()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    // { HashTabU.TIntegerHashTable256 (instance size 1028): 256 lists selected by the low key byte }
    public partial class TIntegerHashTable256 : Aow.Units.HashTabU.TIntegerHashTable
    {
        internal Aow.Units.FixedArray_Aow_Units_HashTabU_TIntegerHashList_256 FLists;
        // HashTabU.pas:186
        // { TIntegerHashTable256 }
        public void Create__init_TIntegerHashTable256()
        {
            int I = default;
            ;
            {
                I = 0;
                byte end__2 = 255;
                for (; I <= end__2; I += 1)
                {
                    var call__3 = Aow.Units.HashTabU.TIntegerHashList.Create();
                    FLists[I] = call__3;
                }
            }
        }

        // HashTabU.pas:194
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                byte end__4 = 255;
                for (; I <= end__4; I += 1)
                {
                    FLists[I].Free();
                }
            }

            base.Destroy();
        }

        // HashTabU.pas:202
        public override void Clear()
        {
            int I = default;
            {
                I = 0;
                byte end__5 = 255;
                for (; I <= end__5; I += 1)
                {
                    FLists[I].Clear();
                }
            }
        }

        // HashTabU.pas:209
        public override void Add(object Item, int Key)
        {
            FLists[Key & 255].Add(Item, Key);
        }

        // HashTabU.pas:214
        public override void Delete(int Key)
        {
            FLists[Key & 255].Delete(Key);
        }

        // HashTabU.pas:219
        public override object Find(int Key)
        {
            object Result = default;
            var call__6 = FLists[Key & 255].Find(Key);
            Result = call__6;
            return Result;
        }

        // HashTabU.pas:58
        public static Aow.Units.HashTabU.TIntegerHashTable256 Create()
        {
            Aow.Units.HashTabU.TIntegerHashTable256 o = new Aow.Units.HashTabU.TIntegerHashTable256();
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

    public struct TStringHashItem
    {
        public Aow.Rtl.AnsiString Key;
        public object Item;
    }

    public partial class TStringHashList : Aow.Units.HashTabU.THashList
    {
        // HashTabU.pas:226
        // { TStringHashList }
        public override void Clear()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__7 = 0;
                for (; I >= end__7; I -= 1)
                {
                    FItems[I].Key = (AnsiString)"";
                }
            }

            base.Clear();
        }

        // HashTabU.pas:234
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

        // HashTabU.pas:251
        public void Add(object Item, Aow.Rtl.AnsiString Key)
        {
            Aow.Units.HashTabU.THashItem H = default;
            if (Find(Key) != null)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"ID already in HashTable");
            }

            H.Key = 0;
            H.Item = Item;
            H.Key = Key;
            base.Add(H);
        }

        // HashTabU.pas:262
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
        public static Aow.Units.HashTabU.TStringHashList Create()
        {
            return new Aow.Units.HashTabU.TStringHashList();
        }
    }

    // { HashTabU.TStringHashTable (instance size 4): the abstract table interface }
    public partial class TStringHashTable : Aow.Rtl.TObject
    {
        // HashTabU.pas:84
        public virtual void Add(object Item, Aow.Rtl.AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:85
        public virtual void Delete(Aow.Rtl.AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:86
        public virtual object Find(Aow.Rtl.AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:87
        public virtual void Clear()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    // { HashTabU.TStringHashTable256 (instance size 1028): 256 lists selected by a hash of the first
//     and the second-to-last character and the length }
    public partial class TStringHashTable256 : Aow.Units.HashTabU.TStringHashTable
    {
        internal Aow.Units.FixedArray_Aow_Units_HashTabU_TStringHashList_256 FLists;
        // HashTabU.pas:281
        // { TStringHashTable256 }
        public void Create__init_TStringHashTable256()
        {
            int I = default;
            ;
            {
                I = 0;
                byte end__8 = 255;
                for (; I <= end__8; I += 1)
                {
                    var call__9 = Aow.Units.HashTabU.TStringHashList.Create();
                    FLists[I] = call__9;
                }
            }
        }

        // HashTabU.pas:289
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                byte end__10 = 255;
                for (; I <= end__10; I += 1)
                {
                    FLists[I].Free();
                }
            }

            base.Destroy();
        }

        // HashTabU.pas:297
        public override void Clear()
        {
            int I = default;
            {
                I = 0;
                byte end__11 = 255;
                for (; I <= end__11; I += 1)
                {
                    FLists[I].Clear();
                }
            }
        }

        // HashTabU.pas:304
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

        // HashTabU.pas:316
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

        // HashTabU.pas:328
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

            var call__12 = FLists[H].Find(Key);
            Result = call__12;
            return Result;
        }

        // HashTabU.pas:96
        public static Aow.Units.HashTabU.TStringHashTable256 Create()
        {
            Aow.Units.HashTabU.TStringHashTable256 o = new Aow.Units.HashTabU.TStringHashTable256();
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

    public static partial class HashTabU
    {
    }
}
