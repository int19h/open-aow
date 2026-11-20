// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

public partial class CityProductionEventLog(CustomNode owner) : ProductionEventLog(owner)
{

    // City.pas:3195
    public new const int ClassId = 131834;

    // City.pas:3205
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        City city;
        if (trigger == ExecuteTrigger.User)
        {
            city = GetCity();
            if (city is not null && StrategicMap.Current.SeatedPlayer == city.Player)
            {
                StrategicMap.Current.CenterViewTo(Location.X, Location.Y, Location.MapLevel);
                StrategicMap.Current.TriggerExecuteEventLog(this, true, done);
            }
        }

        return false;
    }

    // City.pas:3200
    public City GetCity() => StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite<City>();

}
