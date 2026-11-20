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
            ArmyHexSprite armyHexSprite = World.Map.StrategicMap.Current.MapContainer.GetField(Hex.X, Hex.Y, Hex.MapLevel).GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                byte joinSelection = armyHexSprite.Army.GetJoinSelection(Player);
                return armyHexSprite.Army.Hero(joinSelection, types: RosterTables.AllHeroTypes) is null ? EventView.PartyJoin : EventView.HeroNegotiation;
            }
        }

        return EventView.None;
    }
}
