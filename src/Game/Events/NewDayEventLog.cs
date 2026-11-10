// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// inherited text
public partial class NewDayEventLog : TextEventLog
{
    public NewDayEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:809
    public new const int ClassID = 131813;

    // EventLog.pas:812
    public override int GetLeftIcon() => 18;

    // EventLog.pas:815
    public override void PlayLoggedSFX(EventLogbook Logbook)
    {
        if (AoWHSMap.Current.FDay > 1)
        {
            base.PlayLoggedSFX(Logbook);
        }
    }

}
