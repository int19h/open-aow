
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Hash;

public partial class HashList : IDisposable
{
    internal int FCount;
    internal int FCapacity;
    internal HashItem[] FItems;

    // HashTabU.pas:120
    protected internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity != FCapacity)
        {
            FItems = ReallocArray(FItems, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // HashTabU.pas:129
    protected internal void Delete(int Index)
    {
        FCount -= 1;
        if (Index < FCount)
        {
            MoveElements(FItems, Index + 1, FItems, Index, FCount - Index);
        }
    }

    // HashTabU.pas:136
    protected internal void Add(HashItem Item)
    {
        if (FCapacity == FCount)
        {
            SetCapacity(FCapacity + 16);
        }

        FItems[FCount] = Item;
        FCount += 1;
    }

    // HashTabU.pas:108
    public virtual void Dispose()
    {
        Clear();
    }

    // HashTabU.pas:114
    public virtual void Clear()
    {
        FCount = 0;
        SetCapacity(0);
    }

    public int Count { get => FCount; }

    // { The HashTabU unit of Ilpack.dpl: the hash list and hash table classes, an older copy of the
    //   ones the Engine unit includes from inc\HashTab.PAS (Enginep.dpl). This version has no Assert
    //   calls, and TIntegerHashTable256 has neither DeleteSafe nor GetFreeID. }
    // FCount, FCapacity, FItems. The port's FItems is an array of FCapacity items, null for a
    // capacity of 0, as ReallocMem gives nil.
    public struct HashItem
    {
        public IntOrRef Key;
        public object Item;
    }
}
