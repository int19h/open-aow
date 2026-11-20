// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// inherited text
public class NewDayEventLog(CustomNode owner) : TextEventLog(owner)
{

    // EventLog.pas:809
    public new const int ClassId = 131813;

    // EventLog.pas:812
    public override int GetLeftIcon() => 18;

    // EventLog.pas:815
    public override void PlayLoggedSfx(EventLogbook logbook)
    {
        if (StrategicMap.Current.Day > 1)
        {
            base.PlayLoggedSfx(logbook);
        }
    }
}
