#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.Events;

public partial class ProductionPlaceEventLog(CustomNode owner, EngineObject holder) : ProductionEventLog(owner, holder: holder)
{

    // PrdPlace.pas:736
    public new const int ClassId = 131_835;

    // PrdPlace.pas:744
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        if (trigger == ExecuteTrigger.User)
        {
            ProductionPlace place = GetProductionPlace();
            if (place is not null && GetEngine().StrategicMap.SeatedPlayer == place.Player)
            {
                GetEngine().StrategicMap.CenterViewTo(Location.X, Location.Y, Location.MapLevel);
                GetEngine().StrategicMap.TriggerExecuteEventLog(this, immediate: true, done);
            }
        }

        return false;
    }

    // PrdPlace.pas:739
    public ProductionPlace GetProductionPlace() => GetEngine().StrategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite<ProductionPlace>();

}
