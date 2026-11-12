// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// adjacent armies
public partial class AdjacentArmyList : ENode, IChildList<AdjacentArmy>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AdjacentArmy, so the cast never fails.
    public AdjacentArmy this[int index] => (AdjacentArmy)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AdjacentArmy;

    int IChildSink<AdjacentArmy>.AddChild(AdjacentArmy child) => AddChild(child);

    public AdjacentArmyList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:5071
    public AdjacentArmy GetAdjacentArmies(int index) => this[index];

    // AIGroup.pas:5076
    public int IndexOfLocation(MapCoordinate location)
    {
        int result = Count;
        location.Tag = 0;
        while (result != 0)
        {
            result -= 1;
            if (Unsafe.As<MapCoordinate, int>(ref this[result].Location) == Unsafe.As<MapCoordinate, int>(ref location))
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:5088
    public int GetSlowestArmy()
    {
        float maxDays;
        float days;
        maxDays = -1;
        int result = -1;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            days = GetAdjacentArmies(I).GetMovementDays();
            if ((double)days > maxDays)
            {
                result = I;
                maxDays = days;
            }
        }

        return result;
    }

    // AIGroup.pas:5106
    public bool GetAdjacent()
    {
        MapCoordinateList locations;
        locations = new MapCoordinateList();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            locations.Add(GetAdjacentArmies(I).Location);
        }

        bool result = MapCoordinateList.LocationsConnected(locations);
        locations?.Dispose();
        return result;
    }

    // Engine.pas:631

}
