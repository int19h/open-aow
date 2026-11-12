// Translated from EventLog.pas.

namespace Aow.Game.Events;

public struct ExecuteEventLogInfo
{
    public EventLog Log { get; set; }
    public ExecuteDoneEvent Done { get; set; }
    public bool Immediate { get; set; }
}
