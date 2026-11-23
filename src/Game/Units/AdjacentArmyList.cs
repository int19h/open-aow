// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Units;

// adjacent armies
public sealed class AdjacentArmyList(CustomNode owner) : OwningNode(owner), IChildList<AdjacentArmy>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AdjacentArmy, so the cast never fails.
    public AdjacentArmy this[int index] => GetChild<AdjacentArmy>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AdjacentArmy;

    int IChildSink<AdjacentArmy>.AddChild(AdjacentArmy child) => AddChild(child);

    // AIGroup.pas:5071
    public AdjacentArmy GetAdjacentArmies(int index) => this[index];

    // AIGroup.pas:5076
    public int IndexOfLocation(MapCoordinate location) => this.FindLastIndex(item => item.Location == location);

    // AIGroup.pas:5088
    public AdjacentArmy? GetSlowestArmy() => this.Where(army => army.GetMovementDays() > -1).MaxBy(army => army.GetMovementDays());

    // AIGroup.pas:5106
    public bool GetAdjacent()
    {
        MapCoordinateList locations = [];
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            _ = locations.Add(GetAdjacentArmies(i).Location);
        }

        bool result = MapCoordinateList.LocationsConnected(locations);
        locations?.Dispose();
        return result;
    }
}
