// Translated from EventLog.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.World.Map;

namespace Aow.Game.Events;

// location
public partial class LocationTextEventLog : TextEventLog
{
    // EventLog.pas:725
    public LocationTextEventLog(CustomNode owner) : base(owner) => Location = new MapCoordinate(-1, -1, -1);

    // EventLog.pas:728
    public new const int ClassId = 131816;

    public MapCoordinate Location { get; set; }

    // EventLog.pas:731
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
    }

    // EventLog.pas:740
    public override MapCoordinate GetLocation() => Location;

    // EventLog.pas:743
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        _ = base.Execute(mode, trigger, done);
        if (trigger == ExecuteTrigger.User)
        {
            ViewLocation();
        }

        return false;
    }

    // EventLog.pas:737
    public void SetLocation(MapCoordinate location) => Location = location;

    public void SetLocation(sbyte x, sbyte y, sbyte level) =>
        SetLocation(new MapCoordinate(x, y, level));

}
