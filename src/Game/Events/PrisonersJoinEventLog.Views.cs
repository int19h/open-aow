#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public partial class PrisonersJoinEventLog
{
    // Prisoners whose first unit is a hero show the hero, any others the party.
    public override EventView View()
    {
        Troop troopObject = World.Map.StrategicMap.Current.UnitControl.FindUnit(FirstUnitId);
        return troopObject is not null and Hero ? EventView.PrisonerHero : EventView.PrisonersJoin;
    }
}
