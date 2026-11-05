// Translated from HashTabU.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;

namespace Aow.Units.HashTabU
{
    // { The HashTabU unit of Ilpack.dpl: the hash list and hash table classes, an older copy of the
//   ones the Engine unit includes from inc\HashTab.PAS (Enginep.dpl). This version has no Assert
//   calls, and TIntegerHashTable256 has neither DeleteSafe nor GetFreeID. }
    // FCount, FCapacity, FItems
    public struct THashItem
    {
        public IntOrRef Key;
        public object Item;
    }

    public partial class THashList : TObject
    {
        internal int FCount;
        internal int FCapacity;
        internal Ptr<THashItem> FItems;
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
                Ptr<THashItem>.Copy(FItems + (Index + 1), FItems + Index, FCount - Index);
            }
        }

        // HashTabU.pas:136
        protected internal void Add(THashItem Item)
        {
            if (FCapacity == FCount)
            {
                SetCapacity(FCapacity + 16);
            }

            FItems[FCount] = Item;
            FCount += 1;
        }

        // HashTabU.pas:108
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

    // items keyed by an integer
    public partial class TIntegerHashList : THashList
    {
        // HashTabU.pas:145
        public void Add(object Item, int Key)
        {
            THashItem H = default;
            if (Find(Key) != null)
            {
                throw Exception.Create((AnsiString)"ID already in HashTable");
            }

            H.Key = Key;
            H.Item = Item;
            base.Add(H);
        }

        // HashTabU.pas:155
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

        // HashTabU.pas:171
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

    // the abstract table interface
    public partial class TIntegerHashTable : TObject
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

    // 256 lists selected by the low key byte
    public partial class TIntegerHashTable256 : TIntegerHashTable
    {
        internal FixedArray_Aow_Units_HashTabU_TIntegerHashList_256 FLists;
        // HashTabU.pas:186
        public TIntegerHashTable256()
        {
            for (int I = 0; I <= 255; I++)
            {
                var integerHashList = new TIntegerHashList();
                FLists[I] = integerHashList;
            }
        }

        // HashTabU.pas:194
        public override void Destroy()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I].Free();
            }

            base.Destroy();
        }

        // HashTabU.pas:202
        public override void Clear()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I].Clear();
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
        public override object Find(int Key) => FLists[Key & 255].Find(Key);

    }

    public struct TStringHashItem
    {
        public AnsiString Key;
        public object Item;
    }

    public partial class TStringHashList : THashList
    {
        // HashTabU.pas:226
        public override void Clear()
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                FItems[I].Key = (AnsiString)"";
            }

            base.Clear();
        }

        // HashTabU.pas:234
        public object Find(AnsiString Key)
        {
            int I;
            object Result = null;
            I = FCount;
            if (I != 0)
            {
                do
                {
                    I -= 1;
                    if (AnsiCompareStr(FItems[I].Key.As<AnsiString>(), Key) == 0)
                    {
                        return FItems[I].Item;
                    }
                }
                while (!(I == 0));
            }

            return Result;
        }

        // HashTabU.pas:251
        public void Add(object Item, AnsiString Key)
        {
            THashItem H = default;
            if (Find(Key) != null)
            {
                throw Exception.Create((AnsiString)"ID already in HashTable");
            }

            H.Key = 0;
            H.Item = Item;
            H.Key = Key;
            base.Add(H);
        }

        // HashTabU.pas:262
        public void Delete(AnsiString Key)
        {
            int I;
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
        // HashTabU.pas:84
        public virtual void Add(object Item, AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:85
        public virtual void Delete(AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:86
        public virtual object Find(AnsiString Key)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HashTabU.pas:87
        public virtual void Clear()
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    // 256 lists selected by a hash of the first and the second-to-last character and the length
    public partial class TStringHashTable256 : TStringHashTable
    {
        internal FixedArray_Aow_Units_HashTabU_TStringHashList_256 FLists;
        // HashTabU.pas:281
        public TStringHashTable256()
        {
            for (int I = 0; I <= 255; I++)
            {
                var stringHashList = new TStringHashList();
                FLists[I] = stringHashList;
            }
        }

        // HashTabU.pas:289
        public override void Destroy()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I].Free();
            }

            base.Destroy();
        }

        // HashTabU.pas:297
        public override void Clear()
        {
            for (int I = 0; I <= 255; I++)
            {
                FLists[I].Clear();
            }
        }

        // HashTabU.pas:304
        public override void Add(object Item, AnsiString Key)
        {
            int H;
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

        // HashTabU.pas:316
        public override void Delete(AnsiString Key)
        {
            int H;
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

        // HashTabU.pas:328
        public override object Find(AnsiString Key)
        {
            int H;
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

            return FLists[H].Find(Key);
        }

    }

    public static partial class HashTabU
    {
    }
}
