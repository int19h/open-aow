// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// Engine's value list (Classes.TList adapted to a value type, with a default item for new and
// out-of-range items and a run-length encoded stream form) for map locations
public partial class XYLList : Persistent
{
    internal XYL[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal XYL FDefault;

    // HSEngine.pas:1673
    internal XYL Get(int Index)
    {
        XYL Result;
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

    // HSEngine.pas:1746
    internal void Put(int Index, XYL Item)
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

    // HSEngine.pas:1767
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

    // HSEngine.pas:1778
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
            FList.AsSpan(FCount, NewCount - FCount).Fill(FDefault);
        }

        FCount = NewCount;
    }

    // HSEngine.pas:1681
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

    // HSEngine.pas:1578
    public XYLList()
    {
        FDefault.X = -1;
        FDefault.Y = -1;
        FDefault.L = -1;
        FDefault.Flag = 0;
    }

    // HSEngine.pas:1587
    public override void Dispose()
    {
        Clear();
    }

    // HSEngine.pas:1592
    public override void Assign(Persistent Source)
    {
        if (Source is XYLList)
        {
            var xylList = (XYLList)Source;
            this.FCount = xylList.FCount;
            this.SetCapacity(xylList.FCapacity);
            Array.Copy(xylList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = xylList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // HSEngine.pas:1606
    public int Add(XYL Item)
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

    // HSEngine.pas:1614
    public int AddXYL(sbyte X, sbyte Y, sbyte L)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result].X = X;
        FList[Result].Y = Y;
        FList[Result].L = L;
        FList[Result].Flag = 0;
        FCount += 1;
        return Result;
    }

    // HSEngine.pas:1625
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // HSEngine.pas:1631
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 4);
        }
    }

    // HSEngine.pas:1651
    public void Exchange(int Index1, int Index2)
    {
        XYL Item;
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

    // HSEngine.pas:1662
    public XYLList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // HSEngine.pas:1668
    public XYL First() => Get(0);

    // HSEngine.pas:1691
    public int IndexOf(XYL Item)
    {
        int Result = 0;
        while (Result < FCount && Unsafe.As<XYL, int>(ref FList[Result]) != Unsafe.As<XYL, int>(ref Item))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // HSEngine.pas:1698
    public int IndexOfXY(sbyte X, sbyte Y)
    {
        XYL Item = default;
        Item.X = X;
        Item.Y = Y;
        int Result = 0;
        while (Result < FCount && FList[Result].XY != Item.XY)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // HSEngine.pas:1709
    public int IndexOfXYL(sbyte X, sbyte Y, sbyte L)
    {
        int Result = 0;
        while (Result < FCount && (FList[Result].X != X || FList[Result].Y != Y || FList[Result].L != L))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // HSEngine.pas:1717
    public void Insert(int Index, XYL Item)
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
            RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 4);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // HSEngine.pas:1728
    public XYL Last() => Get(FCount - 1);

    // HSEngine.pas:1733
    public void Move(int CurIndex, int NewIndex)
    {
        XYL Item;
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

    // HSEngine.pas:1753
    public int Remove(XYL Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // HSEngine.pas:1759
    public void Pack()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (Unsafe.BitCast<XYL, int>(this[I]) == Unsafe.As<XYL, int>(ref FDefault))
            {
                Delete(I);
            }
        }
    }

    // HSEngine.pas:1815
    public void Sort(XYLSortCompare Compare)
    {
        if (FList.Length != 0 && Count > 0)
        {
            QuickSortXYL(FList, 0, Count - 1, Compare);
        }
    }

    // HSEngine.pas:1821
    public bool Compare(XYLList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

    // HSEngine.pas:1828
    public void LoadFromStream(Stream Stream)
    {
        int Count = default;
        int Run = default;
        XYL Item = default;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref FDefault, 4));
        Stream.ReadBytes(Bytes.Of(ref Count, 4));
        SetCapacity(Count);
        while (Count != FCount)
        {
            Stream.ReadBytes(Bytes.Of(ref Item, 4));
            if (Unsafe.As<XYL, int>(ref Item) == Unsafe.As<XYL, int>(ref FDefault))
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

    // HSEngine.pas:1850
    public void SaveToStream(Stream Stream)
    {
        int I;
        int Run = default;
        int DefaultRun()
        {
            int J;
            int Result = 0;
            J = I;
            while (J < FCount && Unsafe.As<XYL, int>(ref FList[J]) == Unsafe.As<XYL, int>(ref FDefault))
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

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public XYL Default { get => FDefault; set => FDefault = value; }

    public XYL this[int Index] { get => Get(Index); set => Put(Index, value); }

    public XYL[] List { get => FList; }

    // HSEngine.pas:1640
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void QuickSortXYL(Span<XYL> SortList, int L, int R, XYLSortCompare SCompare)
    {
        int I;
        int J;
        XYL P;
        XYL T = default;
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
                QuickSortXYL(SortList, L, J, SCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    internal static bool LocationsConnected(XYLList List)
    {
        bool Result = default;
        XYLList Locations;
        XYL XYL = default;
        void Connect(XYL XYL)
        {
            HNWalkData Walk = default;
            HNWalkData.InitHNWalk(ref Walk, XYL.X, XYL.Y, 1);
            while (Walk.Radius <= 1)
            {
                for (int I = 0, last = Locations.Count - 1; I <= last; I++)
                {
                    if (Locations[I].Flag == 0)
                    {
                        var item = Locations[I];
                        var packXYL = HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, XYL.L);
                        if (Unsafe.BitCast<XYL, int>(item) == Unsafe.BitCast<XYL, int>(packXYL))
                        {
                            Locations.FList[I].Flag = 1;
                            Connect(Locations[I]);
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        Locations = new XYLList();
        for (int I = 0, last = List.Count - 1; I <= last; I++)
        {
            XYL = List[I];
            XYL.Flag = 0;
            if (Locations.IndexOf(XYL) == -1)
            {
                Locations.Add(XYL);
            }
        }

        if (Locations.Count > 0)
        {
            if (Locations.Count > 1)
            {
                Locations.FList[0].Flag = 1;
                Connect(Locations[0]);
                Result = true;
                for (int I = 0, last = Locations.Count - 1; I <= last; I++)
                {
                    if (Locations[I].Flag != 1)
                    {
                        Result = false;
                    }
                }
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        Locations?.Dispose();
        return Result;
    }

    public delegate int XYLSortCompare(XYL Item1, XYL Item2);

}
