
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public partial class PrisonersJoinEventLog
{
    // Prisoners whose first unit is a hero show the hero, any others the party.
    public override EventView View()
    {
        var UnitObject = World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FFirstUnitID);
        return UnitObject != null && UnitObject is Hero ? EventView.PrisonerHero : EventView.PrisonersJoin;
    }
}
