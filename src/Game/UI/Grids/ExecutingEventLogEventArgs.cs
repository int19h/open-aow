using Aow.Game.Events;
namespace Aow.Game.UI.Grids;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class ExecutingEventLogEventArgs(EventLog log, ExecuteDoneEvent done) : EventArgs
{
    public EventLog Log { get; } = log;

    public ExecuteDoneEvent Done { get; } = done;
}
