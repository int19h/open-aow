using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

namespace Aow.Game.Events;

public partial class NegotiateRequestEventLog
{
    // A city negotiation, or for an army the hero negotiation when a hero of the army offers
    // to join, the party join otherwise. Without an army on the field nothing shows.
    public override EventView View()
    {
        if (FObject == NegotiateObject.City)
        {
            return EventView.CityNegotiation;
        }

        if (FObject == NegotiateObject.Army)
        {
            var ArmyHS = (ArmyHS)World.Map.AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(131607);
            if (ArmyHS != null)
            {
                var joinSelection = ArmyHS.FArmy.GetJoinSelection(FPlayer);
                return ArmyHS.FArmy.Hero(joinSelection, RosterTables.AllHeroTypes) == null ? EventView.PartyJoin : EventView.HeroNegotiation;
            }
        }

        return EventView.None;
    }
}
