// Translated from City.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// Location, CityName
public partial class CityRebellionEventLog(CustomNode owner) : EventLog(owner)
{

    // City.pas:2675
    public new const int ClassId = 131_346;

    public MapCoordinate Location { get; set; }
    public string CityName { get; set; } = "";

    // City.pas:2680
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Int32);
        CityName = persister.String("CityName", value: CityName);
    }

    // City.pas:2689
    public override string GetText() => DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_cityRebellionFormat), args: [CityName]);

    // City.pas:2699
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool showEvent = trigger == ExecuteTrigger.User;
        showEvent = showEvent || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Rebellion));
        showEvent = showEvent || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Rebellion));
        return showEvent && base.Execute(mode, trigger, done);
    }

    // City.pas:2694
    public override void ViewLocation() => CenterViewTo(Location.X, Location.Y, L: Location.MapLevel);

    private static readonly string s_cityRebellionFormat = "%s rebelled";

}
