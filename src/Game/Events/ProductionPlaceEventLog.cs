// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Events;

public partial class ProductionPlaceEventLog : ProductionEventLog
{
    public ProductionPlaceEventLog(ECustomNode owner) : base(owner)
    { }

    // PrdPlace.pas:736
    public new const int ClassID = 131835;

    // PrdPlace.pas:744
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        ProductionPlace Place;
        if (Trigger == ExecuteTrigger.User)
        {
            Place = GetProductionPlace();
            if (Place != null && AoWHSMap.Current.FSeatedPlayer == Place.FPlayer)
            {
                AoWHSMap.Current.CenterViewTo(FXhx, FYhx, FLhx);
                AoWHSMap.Current.TriggerExecuteEventLog(this, true, Done);
            }
        }

        return false;
    }

    // PrdPlace.pas:739
    public ProductionPlace GetProductionPlace() => (ProductionPlace)AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, FLhx).FindNoneTransparentHS(typeof(ProductionPlace));

}
