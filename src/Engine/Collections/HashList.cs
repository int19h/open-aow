// Translated from Engine.pas.
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

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

    // FCount, FCapacity, FItems. The hash classes come from an include file of the original,
    // D:\GEngine\Engine\inc\HashTab.PAS, whose name the Assert calls carry
    public struct HashItem
    {
        public IntOrRef Key;
        public object Item;
    }
}
