#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

public partial class CityProductionEventLog(CustomNode owner, EngineObject holder) : ProductionEventLog(owner, holder: holder)
{

    // City.pas:3195
    public new const int ClassId = 131_834;

    // City.pas:3205
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        if (trigger == ExecuteTrigger.User)
        {
            City city = GetCity();
            if (city is not null && GetEngine().StrategicMap.SeatedPlayer == city.Player)
            {
                GetEngine().StrategicMap.CenterViewTo(Location.X, Location.Y, Location.MapLevel);
                GetEngine().StrategicMap.TriggerExecuteEventLog(this, immediate: true, done);
            }
        }

        return false;
    }

    // City.pas:3200
    public City GetCity() => GetEngine().StrategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite<City>();

}
