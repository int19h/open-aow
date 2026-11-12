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

    public EventLogList Events;
    public sbyte Player;
    public int NextId;

    // EventLog.pas:529
    public override void Dispose()
    {
        Events?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:547
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Events, 10);
        stream.rwInteger(ref NextId, 11);
    }

    // EventLog.pas:526
    public EventLogbook(ECustomNode owner) : base(owner)
    {
        Events = new EventLogList(null);
    }

    // EventLog.pas:535
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        Events.MessageProc(ref message, sender);
    }

    // EventLog.pas:538
    public int GetCount() => Events.GetCount();

    // EventLog.pas:541
    public NotifyEvent<object> GetOnChanged() => Events.OnChanged;

    // EventLog.pas:544
    public void SetOnChanged(NotifyEvent<object> value)
    {
        Events.OnChanged = value;
    }

    // EventLog.pas:550
    public void ListEvents(StringCollection strings)
    {
        for (int I = 0, last = Events._children.Count - 1; I <= last; I++)
        {
            if (((EventLog)Events._children.List[I]).GetVisible())
            {
                strings.AddObject(((EventLog)Events._children.List[I]).GetText(), IntToObject(I));
            }
        }
    }

    // EventLog.pas:558
    public EventLog GetLogs(int index) => (EventLog)Events._children.List[index];

    // EventLog.pas:561
    public int CreateUniqueId()
    {
        NextId += 1;
        while (FindId(NextId) != null)
        {
            NextId += 1;
            if (NextId > 16777215)
            {
                NextId = 1;
            }
        }

        return NextId;
    }

    // EventLog.pas:572
    public EventLog FindId(int id) => Events.FindId(id);

    // EventLog.pas:575
    public int FindFirstLog(int player, int day)
    {
        int result = 0;
        while (result != this.Events._children.Count && (((EventLog)this.Events._children.List[result])._player != player || ((EventLog)this.Events._children.List[result])._day != day))
        {
            result += 1;
        }

        if (result == this.Events._children.Count)
        {
            result = -1;
        }

        return result;
    }

    // EventLog.pas:587
    public int FindFirstLog(int classId, int player, int day)
    {
        int result = 0;
        while (result != this.Events._children.Count && (((EventLog)this.Events._children.List[result])._player != player || ((EventLog)this.Events._children.List[result])._day != day || ((EngineObject)this.Events._children.List[result]).EngineClass.ClassId != classId))
        {
            result += 1;
        }

        if (result == this.Events._children.Count)
        {
            result = -1;
        }

        return result;
    }

    // EventLog.pas:600
    public void AddEvent(EventLog eventValue)
    {
        Assert(AoWHSMap.Current.GetSynchronized(), "", "EventLog.pas", 704);
        eventValue.SetOwner(Events);
        eventValue.Id = CreateUniqueId();
        eventValue.NotifyLogged(this);
    }

    // EventLog.pas:710
    public void RemoveEvent(EventLog eventValue)
    {
        Events.RemoveChild(eventValue);
    }

}
