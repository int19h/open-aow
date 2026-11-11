// Translated from Engine.pas.
using Aow.Engine.Storage;

namespace Aow.Engine.Nodes;

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
    public override EngineObject GetChild(int Index) => FItems[Index].Child;

    // Engine.pas:1493
    public override void SetChild(int Index, EngineObject Child)
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
    public override int RemoveChild(EngineObject Child)
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
    public override int AddChild(EngineObject Child) => AddSortChild(Child, 0);

    // Engine.pas:1542
    public virtual int AddSortChild(EngineObject Child, int SortValue)
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
        public EngineObject Child;
    }

    // Engine.pas:922

    // Engine.pas:922

}
