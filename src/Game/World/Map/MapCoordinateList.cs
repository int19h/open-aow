// Translated from HSEngine.pas.
using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

// Engine's value list (Classes.TList adapted to a value type, with a default item for new and
// out-of-range items) for map locations (TXYLList). Path steps are a PathStepList.
public sealed class MapCoordinateList : DefaultItemList<MapCoordinate>
{
    // HSEngine.pas:1578
    public MapCoordinateList() => Default = MapCoordinate.None;

    // HSEngine.pas:1614
    public int AddMapCoordinate(sbyte x, sbyte y, sbyte mapLevel) => Add(new MapCoordinate(x, y, mapLevel));

    // HSEngine.pas:1698
    public int IndexOfXY(sbyte X, sbyte Y)
    {
        int result = 0;
        while (result < Count && (List[result].X != X || List[result].Y != Y))
        {
            result += 1;
        }

        return result == Count ? -1 : result;
    }

    // HSEngine.pas:1709
    public int IndexOfMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        int result = 0;
        while (result < Count && (List[result].X != X || List[result].Y != Y || List[result].MapLevel != L))
        {
            result += 1;
        }

        return result == Count ? -1 : result;
    }

    // HSEngine.pas:1815
    public void Sort(Comparison<MapCoordinate> compare)
    {
        if (List.Length != 0 && Count > 0)
        {
            QuickSortMapCoordinate(List, L: 0, R: Count - 1, sCompare: compare);
        }
    }

    private static void QuickSortMapCoordinate(Span<MapCoordinate> sortList, int L, int R, Comparison<MapCoordinate> sCompare)
    {
        int i;
        do
        {
            i = L;
            int j = R;
            MapCoordinate p = sortList[(L + R) >>> 1];
            do
            {
                while (sCompare(sortList[i], p) < 0)
                {
                    i += 1;
                }

                while (sCompare(sortList[j], p) > 0)
                {
                    j -= 1;
                }

                if (i <= j)
                {
                    (sortList[j], sortList[i]) = (sortList[i], sortList[j]);
                    i += 1;
                    j -= 1;
                }
            }
            while (!(i > j));
            if (L < j)
            {
                QuickSortMapCoordinate(sortList, L, R: j, sCompare);
            }

            L = i;
        }
        while (!(i >= R));
    }

    // Delphi marked the reached locations in the flag byte of a working list. The marks are a
    // separate array beside it, so the locations keep their order and the result is the same.
    internal static bool LocationsConnected(MapCoordinateList list)
    {
        MapCoordinateList locations;
        bool[] reached;
        void Connect(MapCoordinate mapCoordinate)
        {
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, mapCoordinate.X, mapCoordinate.Y, radius: 1);
            while (walk.Radius <= 1)
            {
                for (int i = 0, last = locations.Count - 1; i <= last; i++)
                {
                    if (!reached[i])
                    {
                        MapCoordinate item = locations[i];
                        MapCoordinate packMapCoordinate = new((sbyte)walk.X, (sbyte)walk.Y, mapCoordinate.MapLevel);
                        if (item == packMapCoordinate)
                        {
                            reached[i] = true;
                            Connect(locations[i]);
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        locations = new MapCoordinateList();
        for (int i = 0, last = list.Count - 1; i <= last; i++)
        {
            MapCoordinate mapCoordinate = list[i];
            if (locations.IndexOf(mapCoordinate) == -1)
            {
                _ = locations.Add(mapCoordinate);
            }
        }

        reached = new bool[locations.Count];
        bool result;
        if (locations.Count > 0)
        {
            if (locations.Count > 1)
            {
                reached[0] = true;
                Connect(locations[0]);
                result = true;
                for (int i = 0, last = locations.Count - 1; i <= last; i++)
                {
                    if (!reached[i])
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        locations?.Dispose();
        return result;
    }
}
