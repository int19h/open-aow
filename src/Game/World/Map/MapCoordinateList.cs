// Translated from HSEngine.pas.
using Aow.Game.World.Movement;

namespace Aow.Game.World.Map;

// Engine's value list (Classes.TList adapted to a value type, with a default item for new and
// out-of-range items) for map locations (TXYLList). Path steps are a PathStepList.
public partial class MapCoordinateList : DefaultItemList<MapCoordinate>
{
    // HSEngine.pas:1578
    public MapCoordinateList()
    {
        Default = MapCoordinate.None;
    }

    // HSEngine.pas:1614
    public int AddMapCoordinate(sbyte X, sbyte Y, sbyte L) => Add(new MapCoordinate(X, Y, L));

    // HSEngine.pas:1698
    public int IndexOfXY(sbyte X, sbyte Y)
    {
        int result = 0;
        while (result < Count && (List[result].X != X || List[result].Y != Y))
        {
            result += 1;
        }

        if (result == Count)
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:1709
    public int IndexOfMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        int result = 0;
        while (result < Count && (List[result].X != X || List[result].Y != Y || List[result].MapLevel != L))
        {
            result += 1;
        }

        if (result == Count)
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:1815
    public void Sort(XYLSortCompare compare)
    {
        if (List.Length != 0 && Count > 0)
        {
            QuickSortMapCoordinate(List, 0, Count - 1, compare);
        }
    }

    private static void QuickSortMapCoordinate(Span<MapCoordinate> sortList, int L, int R, XYLSortCompare sCompare)
    {
        int I;
        int J;
        MapCoordinate P;
        MapCoordinate T = default;
        do
        {
            I = L;
            J = R;
            P = sortList[(int)(((uint)(L + R)) >> 1)];
            do
            {
                while (sCompare(sortList[I], P) < 0)
                {
                    I += 1;
                }

                while (sCompare(sortList[J], P) > 0)
                {
                    J -= 1;
                }

                if (I <= J)
                {
                    T = sortList[I];
                    sortList[I] = sortList[J];
                    sortList[J] = T;
                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortMapCoordinate(sortList, L, J, sCompare);
            }

            L = I;
        }
        while (!(I >= R));
    }

    // Delphi marked the reached locations in the flag byte of a working list. The marks are a
    // separate array beside it, so the locations keep their order and the result is the same.
    internal static bool LocationsConnected(MapCoordinateList list)
    {
        bool result = default;
        MapCoordinateList locations;
        bool[] reached = [];
        void Connect(MapCoordinate mapCoordinate)
        {
            HexNumberWalkData walk = default;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, mapCoordinate.X, mapCoordinate.Y, 1);
            while (walk.Radius <= 1)
            {
                for (int I = 0, last = locations.Count - 1; I <= last; I++)
                {
                    if (!reached[I])
                    {
                        var item = locations[I];
                        var packMapCoordinate = HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, mapCoordinate.MapLevel);
                        if (item == packMapCoordinate)
                        {
                            reached[I] = true;
                            Connect(locations[I]);
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        locations = new MapCoordinateList();
        for (int I = 0, last = list.Count - 1; I <= last; I++)
        {
            var mapCoordinate = list[I];
            if (locations.IndexOf(mapCoordinate) == -1)
            {
                locations.Add(mapCoordinate);
            }
        }

        reached = new bool[locations.Count];
        if (locations.Count > 0)
        {
            if (locations.Count > 1)
            {
                reached[0] = true;
                Connect(locations[0]);
                result = true;
                for (int I = 0, last = locations.Count - 1; I <= last; I++)
                {
                    if (!reached[I])
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

    public delegate int XYLSortCompare(MapCoordinate Item1, MapCoordinate Item2);
}
