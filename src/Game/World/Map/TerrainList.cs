// Translated from HSEngine.pas.
using Aow.Engine.Collections;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

// terrain and overlay types by hexagon number, one pair per hexagon (terrain type, overlay
// type. -2: none). The stream holds the pairs as two signed bytes each.
public partial class TerrainList : IDisposable
{
    private int _count;
    private (sbyte Terrain, sbyte Overlay)[] _list = [];

    // HSEngine.pas:1411
    public virtual void Dispose()
    {
        Array.Resize(ref _list, 0);
    }

    // HSEngine.pas:1417
    public virtual void Assign(TerrainList source)
    {
        SetCount(source._count);
        Array.Copy(source._list, 0, _list, 0, _count);
    }

    // HSEngine.pas:1423
    public sbyte GetTerrain(int index)
    {
        sbyte result;
        if (index < _count)
        {
            result = _list[index].Terrain;
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

        _list[index].Terrain = value;
    }

    // HSEngine.pas:1437
    public void SetCount(int newCount)
    {
        Array.Resize(ref _list, newCount);
        if (newCount > _count)
        {
            _list.AsSpan(_count, newCount - _count).Fill((-2, -2));
        }

        _count = newCount;
    }

    // HSEngine.pas:1445
    public sbyte GetOverlay(int index)
    {
        sbyte result;
        if (index < _count)
        {
            result = _list[index].Overlay;
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

        _list[index].Overlay = value;
    }

    // HSEngine.pas:1459. The pair as one word: the terrain byte low, the overlay byte high.
    public ushort GetTandO(int index)
    {
        ushort result;
        if (index < _count)
        {
            result = (ushort)((byte)_list[index].Terrain | (byte)_list[index].Overlay << 8);
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

        _list[index] = ((sbyte)value, (sbyte)(value >> 8));
    }

    // HSEngine.pas:1473. The original masks the word with $8080: a pair is unused when both bytes
    // are negative.
    public bool GetUsed(int index) => index < _count && !(_list[index].Terrain < 0 && _list[index].Overlay < 0);

    // HSEngine.pas:1478
    public int UsedTerrain()
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
            if (_list[i].Terrain > -1) count++;
        return count;
    }

    // HSEngine.pas:1492
    public int UsedOverlays()
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
            if (_list[i].Overlay > -1) count++;
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
                HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 1);
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

    public sealed class UsedAccessor(TerrainList target)
    {
        public bool this[int index] { get => target.GetUsed(index); }
    }

    public UsedAccessor Used => new UsedAccessor(this);

}
