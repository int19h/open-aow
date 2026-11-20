#nullable disable
using System.Collections;

namespace Aow.Game.Events;

/// <summary>
/// The logs of an event log list, in the list's child slots and their stored order. The list
/// holds references, clears a log's owner when it leaves, and notifies its own changes. This
/// collection reads the logs typed. A default value has no list and throws on use, as a missing
/// list does.
/// </summary>
public readonly struct EventLogCollection : IReadOnlyList<EventLog>
{
    internal EventLogCollection(EventLogList list) => Holder = list;

    /// <summary>The list that holds the logs.</summary>
    public EventLogList Holder { get; }

    public int Count => Holder.GetCount();

    // The log in slot index, read as the original reads the list, without a range check. The
    // list admits only event logs. A child of another class throws, the same way the cast did.
    public EventLog this[int index] => Holder.GetChild<EventLog>(index);

    public IEnumerator<EventLog> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // EventLog.pas:505
    public EventLog GetEvents(int index) => this[index];

    // EventLog.pas:514
    public EventLog FindId(int id)
    {
        int index = Count;
        EventLog result = null;
        while (index != 0 && result is null)
        {
            index -= 1;
            if (this[index].Id == id)
            {
                result = this[index];
                break;
            }
        }

        return result;
    }
}
