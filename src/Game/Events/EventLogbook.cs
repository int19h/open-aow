#nullable disable warnings
// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Session;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Events;

// events, player, next ID
public sealed class EventLogbook : EngineObject
{
    // EventLog.pas:532
    public new const int ClassId = 131_808;

    private readonly EventLogList _events;

    public PlayerId? Player { get; set; } = PlayerId.Independent;

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
    public EventLogbook(CustomNode? owner) : base(owner) => _events = new EventLogList(null) { Holder = CreationHolder() };

    // EventLog.pas:535
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        _events.MessageProc(ref message, sender);
    }

    // EventLog.pas:538
    public int GetCount() => _events.GetCount();

    // EventLog.pas:541 and EventLog.pas:544, the OnChanged property of the logbook, which reads
    // and writes the handler of its event list. A handler joins and leaves the list's event.
    public event EventHandler Changed
    {
        add => _events.Changed += value;
        remove => _events.Changed -= value;
    }

    // EventLog.pas:550
    public void ListEvents(DelphiStringCollection strings)
    {
        for (int i = 0, last = _events.Count - 1; i <= last; i++)
        {
            if (_events[i].Visible)
            {
                _ = strings.AddObject(_events[i].GetText(), item: IntToObject(i));
            }
        }
    }

    // EventLog.pas:558
    public EventLog this[int index] => _events[index];

    // EventLog.pas:561
    public int CreateUniqueId()
    {
        _nextId += 1;
        while (FindId(_nextId) is not null)
        {
            _nextId += 1;
            if (_nextId > 16_777_215)
            {
                _nextId = 1;
            }
        }

        return _nextId;
    }

    // EventLog.pas:572
    public EventLog? FindId(int id) => _events.FindId(id);

    // The kind is fixed by the call flow, not by the logbook. NotifyLogged writes the id when the
    // logbook adds the log, and the original asserts a DiplomaticActionEventLog (Diplomacy.pas:697).
    // A missing id stays null, and a log of another kind throws.
    public T FindId<T>(int id) where T : EventLog => (T)FindId(id);

    // EventLog.pas:575
    public int FindFirstLog(PlayerId? player, int day)
    {
        int result = 0;
        while (result != _events.Count && (_events[result].Player != player || _events[result].Day != day))
        {
            result += 1;
        }

        return result == _events.Count ? -1 : result;
    }

    // EventLog.pas:587
    public int FindFirstLog(int classId, PlayerId? player, int day)
    {
        int result = 0;
        while (result != _events.Count && (_events[result].Player != player || _events[result].Day != day || _events[result].EngineClass.ClassId != classId))
        {
            result += 1;
        }

        return result == _events.Count ? -1 : result;
    }

    // EventLog.pas:600
    public void AddEvent(EventLog? eventValue)
    {
        Assert(GetEngine().StrategicMap.Synchronized, message: "", file: "EventLog.pas", line: 704);
        eventValue.SetOwner(_events);
        eventValue.Id = CreateUniqueId();
        eventValue.NotifyLogged(this);
    }

    // EventLog.pas:710
    public void RemoveEvent(EventLog eventValue) => _events.RemoveChild(eventValue);

}
