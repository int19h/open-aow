// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

public partial class CityProductionEventLog : ProductionEventLog
{
    public CityProductionEventLog(ECustomNode owner) : base(owner)
    { }

    // City.pas:3195
    public new const int ClassID = 131834;

    // City.pas:3205
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        City City;
        if (Trigger == ExecuteTrigger.User)
        {
            City = GetCity();
            if (City != null && AoWHSMap.Current.FSeatedPlayer == City.FPlayer)
            {
                AoWHSMap.Current.CenterViewTo(FXhx, FYhx, FLhx);
                AoWHSMap.Current.TriggerExecuteEventLog(this, true, Done);
            }
        }

        return false;
    }

    // City.pas:3200
    public City GetCity() => (City)AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, FLhx).FindNoneTransparentHS(typeof(City));

}
