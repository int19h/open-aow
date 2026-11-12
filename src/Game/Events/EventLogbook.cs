// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// events, player, next ID
public partial class EventLogbook : EngineObject
{
    // EventLog.pas:532
    public new const int ClassId = 131808;

    private EventLogList _events;

    public sbyte Player { get; set; }

    private int _nextId;

    // EventLog.pas:529
    public override void Dispose()
    {
        _events?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:547
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(_events, 10);
        stream.rwInteger(ref _nextId, 11);
    }

    // EventLog.pas:526
    public EventLogbook(ECustomNode owner) : base(owner)
    {
        _events = new EventLogList(null);
    }

    // EventLog.pas:535
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        _events.MessageProc(ref message, sender);
    }

    // EventLog.pas:538
    public int GetCount() => _events.GetCount();

    // EventLog.pas:541
    public NotifyEvent<object> GetOnChanged() => _events.OnChanged;

    // EventLog.pas:544
    public void SetOnChanged(NotifyEvent<object> value)
    {
        _events.OnChanged = value;
    }

    // EventLog.pas:550
    public void ListEvents(StringCollection strings)
    {
        for (int I = 0, last = _events._children.Count - 1; I <= last; I++)
        {
            if (((EventLog)_events._children.List[I]).GetVisible())
            {
                strings.AddObject(((EventLog)_events._children.List[I]).GetText(), IntToObject(I));
            }
        }
    }

    // EventLog.pas:558
    public EventLog GetLogs(int index) => (EventLog)_events._children.List[index];

    // EventLog.pas:561
    public int CreateUniqueId()
    {
        _nextId += 1;
        while (FindId(_nextId) != null)
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

    // EventLog.pas:575
    public int FindFirstLog(int player, int day)
    {
        int result = 0;
        while (result != this._events._children.Count && (((EventLog)this._events._children.List[result])._player != player || ((EventLog)this._events._children.List[result])._day != day))
        {
            result += 1;
        }

        if (result == this._events._children.Count)
        {
            result = -1;
        }

        return result;
    }

    // EventLog.pas:587
    public int FindFirstLog(int classId, int player, int day)
    {
        int result = 0;
        while (result != this._events._children.Count && (((EventLog)this._events._children.List[result])._player != player || ((EventLog)this._events._children.List[result])._day != day || ((EngineObject)this._events._children.List[result]).EngineClass.ClassId != classId))
        {
            result += 1;
        }

        if (result == this._events._children.Count)
        {
            result = -1;
        }

        return result;
    }

    // EventLog.pas:600
    public void AddEvent(EventLog eventValue)
    {
        Assert(AoWHSMap.Current.GetSynchronized(), "", "EventLog.pas", 704);
        eventValue.SetOwner(_events);
        eventValue.Id = CreateUniqueId();
        eventValue.NotifyLogged(this);
    }

    // EventLog.pas:710
    public void RemoveEvent(EventLog eventValue)
    {
        _events.RemoveChild(eventValue);
    }

}
