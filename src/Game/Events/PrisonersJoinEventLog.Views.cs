
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public partial class PrisonersJoinEventLog
{
    // Prisoners whose first unit is a hero show the hero, any others the party.
    public override EventView View()
    {
        var troopObject = World.Map.AoWHSMap.Current.UnitControl.FindUnit(FirstUnitId);
        return troopObject != null && troopObject is Hero ? EventView.PrisonerHero : EventView.PrisonersJoin;
    }
}
