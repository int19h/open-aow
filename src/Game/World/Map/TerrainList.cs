// Translated from HSEngine.pas.
using Aow.Engine.Collections;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

// terrain and overlay types by hexagon number, one pair per hexagon (terrain type, overlay
// type. -2: none). The stream holds the pairs as two signed bytes each.
public sealed class TerrainList : IDisposable
{
    private int _count;
    private (sbyte Terrain, sbyte Overlay)[] _list = [];

    // HSEngine.pas:1411
    public void Dispose() => Array.Resize(ref _list, newSize: 0);

    // HSEngine.pas:1417
    public void Assign(TerrainList source)
    {
        SetCount(source._count);
        Array.Copy(source._list, sourceIndex: 0, destinationArray: _list, destinationIndex: 0, length: _count);
    }

    // HSEngine.pas:1423
    public FootprintCell<TerrainType> GetTerrain(int index) => index < _count ? FootprintCell<TerrainType>.FromOriginal(_list[index].Terrain) : FootprintCell<TerrainType>.Outside;

    // HSEngine.pas:1431
    public void SetTerrain(int index, int value)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        _list[index].Terrain = (sbyte)value;
    }

    // HSEngine.pas:1437
    public void SetCount(int newCount)
    {
        Array.Resize(ref _list, newSize: newCount);
        if (newCount > _count)
        {
            _list.AsSpan(_count, length: newCount - _count).Fill((-2, -2));
        }

        _count = newCount;
    }

    // HSEngine.pas:1445
    public FootprintCell<OverlayType> GetOverlay(int index) => index < _count ? FootprintCell<OverlayType>.FromOriginal(_list[index].Overlay) : FootprintCell<OverlayType>.Outside;

    // The stored bytes of one pair, as the stream holds them. The save writes these, so a stored byte
    // that the answers read the same way as another (below -2) still saves unchanged.
    public (sbyte Terrain, sbyte Overlay) GetStoredPair(int index) => _list[index];

    // HSEngine.pas:1453
    public void SetOverlay(int index, int value)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        _list[index].Overlay = (sbyte)value;
    }

    // HSEngine.pas:1459. The pair as one word: the terrain byte low, the overlay byte high.
    public int GetTandO(int index) => index < _count ? (byte)_list[index].Terrain | ((byte)_list[index].Overlay << 8) : 65_278;

    // HSEngine.pas:1467
    public void SetTandO(int index, int value)
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
        {
            if (_list[i].Terrain > -1)
            {
                count++;
            }
        }

        return count;
    }

    // HSEngine.pas:1492
    public int UsedOverlays()
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_list[i].Overlay > -1)
            {
                count++;
            }
        }

        return count;
    }

    // HSEngine.pas:1506
    public void Pack()
    {
        int newCount = _count;
        while (newCount != 0 && _list[newCount - 1].Overlay == -2 && _list[newCount - 1].Terrain == -2)
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
        int hexX = default;
        int hexY = default;
        UInt16List result = [];
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            if (GetTerrain(i).Value is not null)
            {
                HexGeometry.HexNumberToHex(i, ref hexX, ref hexY);
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 1);
                while (walk.HexNumber != 7)
                {
                    int hexNumber = HexGeometry.HexToHexNumber(walk.X, walk.Y);
                    if (hexNumber >= _count || (hexNumber < _count && GetTerrain(hexNumber).Value is null))
                    {
                        if (result.IndexOf((ushort)hexNumber) == -1)
                        {
                            _ = result.Add(hexNumber.TruncateToUInt16());
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
        }

        return result;
    }

    // HSEngine.pas:1538
    public bool CompareTerrain(TerrainList list, TerrainCompareModes mode)
    {
        bool result = true;
        int i = 0;
        int n = _count >= list._count ? _count : list._count;

        while (result && i != n)
        {
            FootprintCell<TerrainType> t1 = GetTerrain(i);
            FootprintCell<TerrainType> t2 = list.GetTerrain(i);
            if (!(t1.Value is null && t2.Value is null))
            {
                result = t1.Value is not null && t2.Value is not null && (!mode.Contains(TerrainCompareModes.SameTerrainType) || t1 == t2);
            }

            if (result)
            {
                FootprintCell<OverlayType> o1 = GetOverlay(i);
                FootprintCell<OverlayType> o2 = list.GetOverlay(i);
                if (!(o1.Value is null && o2.Value is null))
                {
                    result = !mode.Contains(TerrainCompareModes.SameOverlayType) || o1 == o2;
                }
            }

            i += 1;
        }

        return result;
    }

    public int Count { get => _count; set => SetCount(value); }

    public sealed class TerrainAccessor(TerrainList target)
    {
        public FootprintCell<TerrainType> this[int index] => target.GetTerrain(index);
    }

    public TerrainAccessor Terrain => new(this);

    public sealed class OverlayAccessor(TerrainList target)
    {
        public FootprintCell<OverlayType> this[int index] => target.GetOverlay(index);
    }

    public OverlayAccessor Overlay => new(this);

    public sealed class UsedAccessor(TerrainList target)
    {
        public bool this[int index] => target.GetUsed(index);
    }

    public UsedAccessor Used => new(this);

}
