// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// adjacent armies
public partial class AdjacentArmyList : ENode
{
    public AdjacentArmyList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:5071
    public AdjacentArmy GetAdjacentArmies(int Index) => (AdjacentArmy)FChildren.List[Index];

    // AIGroup.pas:5076
    public int IndexOfLoc(MapCoordinate Location)
    {
        int Result = Count;
        Location.Flag = 0;
        while (Result != 0)
        {
            Result -= 1;
            if (Unsafe.As<MapCoordinate, int>(ref ((AdjacentArmy)FChildren.List[Result]).FLocation) == Unsafe.As<MapCoordinate, int>(ref Location))
            {
                return Result;
            }
        }

        return -1;
    }

    // AIGroup.pas:5088
    public int GetSlowestArmy()
    {
        float MaxDays;
        float Days;
        MaxDays = -1;
        int Result = -1;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Days = GetAdjacentArmies(I).GetMovementDays();
            if ((double)Days > MaxDays)
            {
                Result = I;
                MaxDays = Days;
            }
        }

        return Result;
    }

    // AIGroup.pas:5106
    public bool GetAdjacent()
    {
        MapCoordinateList Locations;
        Locations = new MapCoordinateList();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Locations.Add(GetAdjacentArmies(I).FLocation);
        }

        bool Result = MapCoordinateList.LocationsConnected(Locations);
        Locations?.Dispose();
        return Result;
    }

    // Engine.pas:631

}
