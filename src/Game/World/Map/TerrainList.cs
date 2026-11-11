// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Collections;

using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// terrain and overlay types by hexagon number, two bytes per hexagon (terrain type, overlay
// type. -2: none)
public partial class TerrainList : IDisposable
{
    internal int FCount;
    internal byte[] FList = [];

    // HSEngine.pas:1411
    public virtual void Dispose()
    {
        Array.Resize(ref FList, 0);
    }

    // HSEngine.pas:1417
    public virtual void Assign(TerrainList Source)
    {
        SetCount(Source.FCount);
        Array.Copy(Source.FList, 0, FList, 0, FCount * 2);
    }

    // HSEngine.pas:1562
    public virtual void LoadFromStream(Stream Stream)
    {
        Stream.ReadExactly(Bytes.Of(ref FCount, 4));
        SetCount(FCount);
        Stream.ReadExactly(FList.AsSpan(0, FCount * 2));
    }

    // HSEngine.pas:1569
    public virtual void SaveToStream(Stream Stream)
    {
        Pack();
        Stream.Write(Bytes.Of(ref FCount, 4));
        Stream.Write(FList.AsSpan(0, FCount * 2));
    }

    // HSEngine.pas:1423
    public sbyte GetTerrain(int Index)
    {
        sbyte Result;
        if (Index < FCount)
        {
            Result = MemoryMarshal.Read<sbyte>(FList.AsSpan(Index * 2, 1));
        }
        else
        {
            Result = -2;
        }

        return Result;
    }

    // HSEngine.pas:1431
    public void SetTerrain(int Index, sbyte Value)
    {
        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        sbyte value__206 = Value;
        MemoryMarshal.Write(FList.AsSpan(Index * 2, 1), in value__206);
    }

    // HSEngine.pas:1437
    public void SetCount(int NewCount)
    {
        Array.Resize(ref FList, NewCount + NewCount);
        if (NewCount > FCount)
        {
            FillBytes(FList.AsSpan(FCount * 2, (NewCount - FCount) * 2), -2);
        }

        FCount = NewCount;
    }

    // HSEngine.pas:1445
    public sbyte GetOverlay(int Index)
    {
        sbyte Result;
        if (Index < FCount)
        {
            Result = MemoryMarshal.Read<sbyte>(FList.AsSpan(Index * 2 + 1, 1));
        }
        else
        {
            Result = -2;
        }

        return Result;
    }

    // HSEngine.pas:1453
    public void SetOverlay(int Index, sbyte Value)
    {
        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        sbyte value__207 = Value;
        MemoryMarshal.Write(FList.AsSpan(Index * 2 + 1, 1), in value__207);
    }

    // HSEngine.pas:1459
    public ushort GetTandO(int Index)
    {
        ushort Result;
        if (Index < FCount)
        {
            Result = MemoryMarshal.Read<ushort>(FList.AsSpan(Index * 2, 2));
        }
        else
        {
            Result = 65278;
        }

        return Result;
    }

    // HSEngine.pas:1467
    public void SetTandO(int Index, ushort Value)
    {
        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        ushort value__208 = Value;
        MemoryMarshal.Write(FList.AsSpan(Index * 2, 2), in value__208);
    }

    // HSEngine.pas:1473
    public bool GetUsed(int Index) => Index < FCount && (MemoryMarshal.Read<ushort>(FList.AsSpan(Index * 2, 2)) & 32896) != 32896;

    // HSEngine.pas:1478
    public int UsedTerrain()
    {
        int count = 0;
        for (int i = 0; i < FCount; i++)
            if ((sbyte)FList[i * 2 + 0] > -1) count++;
        return count;
    }

    // HSEngine.pas:1492
    public int UsedOverlays()
    {
        int count = 0;
        for (int i = 0; i < FCount; i++)
            if ((sbyte)FList[i * 2 + 1] > -1) count++;
        return count;
    }

    // HSEngine.pas:1506
    public void Pack()
    {
        int NewCount;
        NewCount = FCount;
        while (NewCount != 0 && GetOverlay(NewCount - 1) == -2 && GetTerrain(NewCount - 1) == -2)
        {
            NewCount -= 1;
        }

        if (NewCount != FCount)
        {
            SetCount(NewCount);
        }
    }

    // HSEngine.pas:1516
    public UInt16List CreateSurroundingHNTable()
    {
        int HN;
        int Xhx = default;
        int Yhx = default;
        HexNumberWalkData Walk = default;
        UInt16List Result = new UInt16List();
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (GetTerrain(I) >= 0)
            {
                HexGeometry.HNtoHX(I, ref Xhx, ref Yhx);
                HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 1);
                while (Walk.HN != 7)
                {
                    HN = HexGeometry.HXtoHN(Walk.X, Walk.Y);
                    if (HN >= FCount || HN < FCount && GetTerrain(HN) < 0)
                    {
                        if (Result.IndexOf((ushort)HN) == -1)
                        {
                            Result.Add((ushort)HN);
                        }
                    }

                    HexNumberWalkData.WalkToNextHN(ref Walk);
                }
            }
        }

        return Result;
    }

    // HSEngine.pas:1538
    public bool CompareTerrain(TerrainList List, Set<TerrainCompareModeElement> Mode)
    {
        int I;
        int N;
        int T1;
        int T2;
        int O1;
        int O2;
        bool Result = true;
        I = 0;
        if (FCount >= List.FCount)
        {
            N = FCount;
        }
        else
        {
            N = List.FCount;
        }

        while (Result && I != N)
        {
            T1 = GetTerrain(I);
            T2 = List.GetTerrain(I);
            if (!(T1 < 0 && T2 < 0))
            {
                Result = T1 >= 0 && T2 >= 0 && (!Mode.Contains(TerrainCompareModeElement.SameTerrainType) || T1 == T2);
            }

            if (Result)
            {
                O1 = GetOverlay(I);
                O2 = List.GetOverlay(I);
                if (!(O1 < 0 && O2 < 0))
                {
                    Result = !Mode.Contains(TerrainCompareModeElement.SameOverlayType) || O1 == O2;
                }
            }

            I += 1;
        }

        return Result;
    }

    public int Count { get => FCount; set => SetCount(value); }

    public sealed class TerrainAccessor(TerrainList target)
    {
        public sbyte this[int Index] { get => target.GetTerrain(Index); set => target.SetTerrain(Index, value); }
    }

    public TerrainAccessor Terrain => new TerrainAccessor(this);

    public sealed class OverlayAccessor(TerrainList target)
    {
        public sbyte this[int Index] { get => target.GetOverlay(Index); set => target.SetOverlay(Index, value); }
    }

    public OverlayAccessor Overlay => new OverlayAccessor(this);

    public sealed class TerrainAndOverlayAccessor(TerrainList target)
    {
        public ushort this[int Index] { get => target.GetTandO(Index); set => target.SetTandO(Index, value); }
    }

    public TerrainAndOverlayAccessor TandO => new TerrainAndOverlayAccessor(this);

    public sealed class UsedAccessor(TerrainList target)
    {
        public bool this[int Index] { get => target.GetUsed(Index); }
    }

    public UsedAccessor Used => new UsedAccessor(this);

}
