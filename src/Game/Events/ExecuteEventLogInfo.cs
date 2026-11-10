// Translated from EventLog.pas.

namespace Aow.Game.Events;

public struct ExecuteEventLogInfo
{
    public EventLog Log;
    public ExecuteDoneEvent Done;
    public bool Immediate;
}
