using System.Runtime.CompilerServices;

namespace Aow.Engine;

/// <summary>
/// The temporary check of issue 326, decision 4. An event that had at most one subscriber at a
/// time in the original raises with at most one handler. A second handler fails Debug builds and
/// tests through Debug.Fail, and Release builds write a warning to the game log, so a playtest
/// reports it and goes on. The checks go once playtesting never shows the warning.
/// </summary>
internal static class EventChecks
{
    /// <summary>Checks before a raise that the event holds at most one handler.</summary>
    /// <param name="handlers">The event's handlers.</param>
    /// <param name="owner">The object that raises the event, named in the message.</param>
    /// <param name="eventName">The event. A raise passes it by nameof, since it checks a local that holds the handlers it reads once.</param>
    public static void AtMostOneHandler(Delegate? handlers, object owner, [CallerArgumentExpression(nameof(handlers))] string eventName = "")
    {
        if (handlers is null || handlers.HasSingleTarget)
        {
            return;
        }

        int count = handlers.GetInvocationList().Length;
#if DEBUG
        System.Diagnostics.Debug.Fail($"{eventName} of {owner} raises with {count} handlers, at most one was expected (issue 326, decision 4)");
#else
        Aow.Rtl.HostLog.SecondEventHandler(Aow.Rtl.RtlHost.LoggerFor("OpenAoW.Events"), eventName: eventName, owner: owner, count: count);
#endif
    }
}
