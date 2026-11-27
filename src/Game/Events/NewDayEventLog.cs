#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

// inherited text
public class NewDayEventLog(CustomNode owner) : TextEventLog(owner)
{

    // EventLog.pas:809
    public new const int ClassId = 131_813;

    // EventLog.pas:812
    public override int LeftIcon => 18;

    // EventLog.pas:815
    public override void PlayLoggedSfx(EventLogbook logbook)
    {
        if (GetEngine().StrategicMap.Day > 1)
        {
            base.PlayLoggedSfx(logbook);
        }
    }
}
