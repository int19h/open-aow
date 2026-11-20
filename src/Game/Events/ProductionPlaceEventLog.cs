// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Events;

public partial class ProductionPlaceEventLog(CustomNode owner) : ProductionEventLog(owner)
{

    // PrdPlace.pas:736
    public new const int ClassId = 131835;

    // PrdPlace.pas:744
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        if (trigger == ExecuteTrigger.User)
        {
            ProductionPlace place = GetProductionPlace();
            if (place is not null && StrategicMap.Current.SeatedPlayer == place.Player)
            {
                StrategicMap.Current.CenterViewTo(Location.X, Location.Y, Location.MapLevel);
                StrategicMap.Current.TriggerExecuteEventLog(this, immediate: true, done);
            }
        }

        return false;
    }

    // PrdPlace.pas:739
    public ProductionPlace GetProductionPlace() => StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite<ProductionPlace>();

}
