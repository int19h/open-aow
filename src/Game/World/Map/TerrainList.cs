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
    internal int _count;
    internal byte[] _list = [];

    // HSEngine.pas:1411
    public virtual void Dispose()
    {
        Array.Resize(ref _list, 0);
    }

    // HSEngine.pas:1417
    public virtual void Assign(TerrainList source)
    {
        SetCount(source._count);
        Array.Copy(source._list, 0, _list, 0, _count * 2);
    }

    // HSEngine.pas:1562
    public virtual void LoadFromStream(Stream stream)
    {
        stream.ReadExactly(Bytes.Of(ref _count, 4));
        SetCount(_count);
        stream.ReadExactly(_list.AsSpan(0, _count * 2));
    }

    // HSEngine.pas:1569
    public virtual void SaveToStream(Stream stream)
    {
        Pack();
        stream.Write(Bytes.Of(ref _count, 4));
        stream.Write(_list.AsSpan(0, _count * 2));
    }

    // HSEngine.pas:1423
    public sbyte GetTerrain(int index)
    {
        sbyte result;
        if (index < _count)
        {
            result = MemoryMarshal.Read<sbyte>(_list.AsSpan(index * 2, 1));
        }
        else
        {
            result = -2;
        }

        return result;
    }

    // HSEngine.pas:1431
    public void SetTerrain(int index, sbyte value)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        sbyte value206 = value;
        MemoryMarshal.Write(_list.AsSpan(index * 2, 1), in value206);
    }

    // HSEngine.pas:1437
    public void SetCount(int newCount)
    {
        Array.Resize(ref _list, newCount + newCount);
        if (newCount > _count)
        {
            FillBytes(_list.AsSpan(_count * 2, (newCount - _count) * 2), -2);
        }

        _count = newCount;
    }

    // HSEngine.pas:1445
    public sbyte GetOverlay(int index)
    {
        sbyte result;
        if (index < _count)
        {
            result = MemoryMarshal.Read<sbyte>(_list.AsSpan(index * 2 + 1, 1));
        }
        else
        {
            result = -2;
        }

        return result;
    }

    // HSEngine.pas:1453
    public void SetOverlay(int index, sbyte value)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        sbyte value207 = value;
        MemoryMarshal.Write(_list.AsSpan(index * 2 + 1, 1), in value207);
    }

    // HSEngine.pas:1459
    public ushort GetTandO(int index)
    {
        ushort result;
        if (index < _count)
        {
            result = MemoryMarshal.Read<ushort>(_list.AsSpan(index * 2, 2));
        }
        else
        {
            result = 65278;
        }

        return result;
    }

    // HSEngine.pas:1467
    public void SetTandO(int index, ushort value)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        ushort value208 = value;
        MemoryMarshal.Write(_list.AsSpan(index * 2, 2), in value208);
    }

    // HSEngine.pas:1473
    public bool GetUsed(int index) => index < _count && (MemoryMarshal.Read<ushort>(_list.AsSpan(index * 2, 2)) & 32896) != 32896;

    // HSEngine.pas:1478
    public int UsedTerrain()
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
            if ((sbyte)_list[i * 2 + 0] > -1) count++;
        return count;
    }

    // HSEngine.pas:1492
    public int UsedOverlays()
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
            if ((sbyte)_list[i * 2 + 1] > -1) count++;
        return count;
    }

    // HSEngine.pas:1506
    public void Pack()
    {
        int newCount;
        newCount = _count;
        while (newCount != 0 && GetOverlay(newCount - 1) == -2 && GetTerrain(newCount - 1) == -2)
        {
            newCount -= 1;
        }

        if (newCount != _count)
        {
            SetCount(newCount);
        }
    }

    // HSEngine.pas:1516
    public UInt16List CreateSurroundingHexNumberTable()
    {
        int hexNumber;
        int hexX = default;
        int hexY = default;
        HexNumberWalkData walk = default;
        UInt16List result = new UInt16List();
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            if (GetTerrain(I) >= 0)
            {
                HexGeometry.HexNumberToHex(I, ref hexX, ref hexY);
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 1);
                while (walk.HexNumber != 7)
                {
                    hexNumber = HexGeometry.HexToHexNumber(walk.X, walk.Y);
                    if (hexNumber >= _count || hexNumber < _count && GetTerrain(hexNumber) < 0)
                    {
                        if (result.IndexOf((ushort)hexNumber) == -1)
                        {
                            result.Add((ushort)hexNumber);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
        }

        return result;
    }

    // HSEngine.pas:1538
    public bool CompareTerrain(TerrainList list, Set<TerrainCompareModeElement> mode)
    {
        int I;
        int N;
        int t1;
        int t2;
        int o1;
        int o2;
        bool result = true;
        I = 0;
        if (_count >= list._count)
        {
            N = _count;
        }
        else
        {
            N = list._count;
        }

        while (result && I != N)
        {
            t1 = GetTerrain(I);
            t2 = list.GetTerrain(I);
            if (!(t1 < 0 && t2 < 0))
            {
                result = t1 >= 0 && t2 >= 0 && (!mode.Contains(TerrainCompareModeElement.SameTerrainType) || t1 == t2);
            }

            if (result)
            {
                o1 = GetOverlay(I);
                o2 = list.GetOverlay(I);
                if (!(o1 < 0 && o2 < 0))
                {
                    result = !mode.Contains(TerrainCompareModeElement.SameOverlayType) || o1 == o2;
                }
            }

            I += 1;
        }

        return result;
    }

    public int Count { get => _count; set => SetCount(value); }

    public sealed class TerrainAccessor(TerrainList target)
    {
        public sbyte this[int index] { get => target.GetTerrain(index); set => target.SetTerrain(index, value); }
    }

    public TerrainAccessor Terrain => new TerrainAccessor(this);

    public sealed class OverlayAccessor(TerrainList target)
    {
        public sbyte this[int index] { get => target.GetOverlay(index); set => target.SetOverlay(index, value); }
    }

    public OverlayAccessor Overlay => new OverlayAccessor(this);

    public sealed class TerrainAndOverlayAccessor(TerrainList target)
    {
        public ushort this[int index] { get => target.GetTandO(index); set => target.SetTandO(index, value); }
    }

    public TerrainAndOverlayAccessor TandO => new TerrainAndOverlayAccessor(this);

    public sealed class UsedAccessor(TerrainList target)
    {
        public bool this[int index] { get => target.GetUsed(index); }
    }

    public UsedAccessor Used => new UsedAccessor(this);

}
