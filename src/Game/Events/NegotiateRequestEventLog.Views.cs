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
        if (Object == NegotiateObject.City)
        {
            return EventView.CityNegotiation;
        }

        if (Object == NegotiateObject.Army)
        {
            var armyHexSprite = (ArmyHexSprite)World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, MapLevel).FindOwnedChild(131607);
            if (armyHexSprite != null)
            {
                var joinSelection = armyHexSprite.Army.GetJoinSelection(_player);
                return armyHexSprite.Army.Hero(joinSelection, RosterTables.AllHeroTypes) == null ? EventView.PartyJoin : EventView.HeroNegotiation;
            }
        }

        return EventView.None;
    }
}
