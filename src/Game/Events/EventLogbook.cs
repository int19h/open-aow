#nullable disable
// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Events;

// events, player, next ID
public partial class EventLogbook : EngineObject
{
    // EventLog.pas:532
    public new const int ClassId = 131808;

    private readonly EventLogList _events;

    public sbyte Player { get; set; }

    private int _nextId;

    // EventLog.pas:529
    public override void Dispose()
    {
        _events?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:547
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Events", objectValue: _events);
        _nextId = persister.Int32("NextId", value: _nextId);
    }

    // EventLog.pas:526
    public EventLogbook(CustomNode owner) : base(owner) => _events = new EventLogList(null) { Holder = CreationHolder() };

    // EventLog.pas:535
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        _events.MessageProc(ref message, sender);
    }

    // EventLog.pas:538
    public int GetCount() => _events.GetCount();

    // EventLog.pas:541 and EventLog.pas:544, the OnChanged property of the logbook, which reads
    // and writes the handler of its event list. A handler joins and leaves the list's event.
    public event NotifyEvent<object> OnChanged
    {
        add => _events.OnChanged += value;
        remove => _events.OnChanged -= value;
    }

    // EventLog.pas:550
    public void ListEvents(DelphiStringCollection strings)
    {
        for (int i = 0, last = _events.Count - 1; i <= last; i++)
        {
            if (_events[i].GetVisible())
            {
                _ = strings.AddObject(_events[i].GetText(), IntToObject(i));
            }
        }
    }

    // EventLog.pas:558
    public EventLog GetLogs(int index) => _events[index];

    // EventLog.pas:561
    public int CreateUniqueId()
    {
        _nextId += 1;
        while (FindId(_nextId) is not null)
        {
            _nextId += 1;
            if (_nextId > 16777215)
            {
                _nextId = 1;
            }
        }

        return _nextId;
    }

    // EventLog.pas:572
    public EventLog FindId(int id) => _events.FindId(id);

    // The kind is fixed by the call flow, not by the logbook. NotifyLogged writes the id when the
    // logbook adds the log, and the original asserts a DiplomaticActionEventLog (Diplomacy.pas:697).
    // A missing id stays null, and a log of another kind throws.
    public T FindId<T>(int id) where T : EventLog => (T)FindId(id);

    // EventLog.pas:575
    public int FindFirstLog(int player, int day)
    {
        int result = 0;
        while (result != _events.Count && (_events[result].Player != player || _events[result].Day != day))
        {
            result += 1;
        }

        if (result == _events.Count)
        {
            result = -1;
        }

        return result;
    }

    // EventLog.pas:587
    public int FindFirstLog(int classId, int player, int day)
    {
        int result = 0;
        while (result != _events.Count && (_events[result].Player != player || _events[result].Day != day || _events[result].EngineClass.ClassId != classId))
        {
            result += 1;
        }

        if (result == _events.Count)
        {
            result = -1;
        }

        return result;
    }

    // EventLog.pas:600
    public void AddEvent(EventLog eventValue)
    {
        Assert(StrategicMap.Current.GetSynchronized(), "", "EventLog.pas", 704);
        eventValue.SetOwner(_events);
        eventValue.Id = CreateUniqueId();
        eventValue.NotifyLogged(this);
    }

    // EventLog.pas:710
    public void RemoveEvent(EventLog eventValue) => _events.RemoveChild(eventValue);

}
