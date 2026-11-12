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
    public new const int ClassId = 131834;

    // City.pas:3205
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        City city;
        if (trigger == ExecuteTrigger.User)
        {
            city = GetCity();
            if (city != null && AoWHSMap.Current.SeatedPlayer == city.Player)
            {
                AoWHSMap.Current.CenterViewTo(HexX, HexY, HexLevel);
                AoWHSMap.Current.TriggerExecuteEventLog(this, true, done);
            }
        }

        return false;
    }

    // City.pas:3200
    public City GetCity() => (City)AoWHSMap.Current.MapContainer.GetField(HexX, HexY, HexLevel).FindNoneTransparentHexSprite(typeof(City));

}
