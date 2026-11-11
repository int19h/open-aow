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
    public new const int ClassID = 131808;

    public EventLogList FEvents;
    public sbyte FPlayer;
    public int FNextID;

    // EventLog.pas:529
    public override void Dispose()
    {
        FEvents?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:547
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FEvents, 10);
        Stream.rwInteger(ref FNextID, 11);
    }

    // EventLog.pas:526
    public EventLogbook(ECustomNode owner) : base(owner)
    {
        FEvents = new EventLogList(null);
    }

    // EventLog.pas:535
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FEvents.MsgProc(ref Msg, Sender);
    }

    // EventLog.pas:538
    public int GetCount() => FEvents.GetCount();

    // EventLog.pas:541
    public NotifyEvent<object> GetOnChanged() => FEvents.FOnChanged;

    // EventLog.pas:544
    public void SetOnChanged(NotifyEvent<object> Value)
    {
        FEvents.FOnChanged = Value;
    }

    // EventLog.pas:550
    public void ListEvents(StringCollection Strings)
    {
        for (int I = 0, last = FEvents.FChildren.Count - 1; I <= last; I++)
        {
            if (((EventLog)FEvents.FChildren.List[I]).GetVisible())
            {
                Strings.AddObject(((EventLog)FEvents.FChildren.List[I]).GetText(), IntToObject(I));
            }
        }
    }

    // EventLog.pas:558
    public EventLog GetLogs(int Index) => (EventLog)FEvents.FChildren.List[Index];

    // EventLog.pas:561
    public int CreateUniqueID()
    {
        FNextID += 1;
        while (FindID(FNextID) != null)
        {
            FNextID += 1;
            if (FNextID > 16777215)
            {
                FNextID = 1;
            }
        }

        return FNextID;
    }

    // EventLog.pas:572
    public EventLog FindID(int ID) => FEvents.FindID(ID);

    // EventLog.pas:575
    public int FindFirstLog(int Player, int Day)
    {
        int Result = 0;
        while (Result != this.FEvents.FChildren.Count && (((EventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player || ((EventLog)this.FEvents.FChildren.List[Result]).FDay != Day))
        {
            Result += 1;
        }

        if (Result == this.FEvents.FChildren.Count)
        {
            Result = -1;
        }

        return Result;
    }

    // EventLog.pas:587
    public int FindFirstLogEx(int AClassID, int Player, int Day)
    {
        int Result = 0;
        while (Result != this.FEvents.FChildren.Count && (((EventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player || ((EventLog)this.FEvents.FChildren.List[Result]).FDay != Day || ((EngineObject)this.FEvents.FChildren.List[Result]).EClass.ClassID != AClassID))
        {
            Result += 1;
        }

        if (Result == this.FEvents.FChildren.Count)
        {
            Result = -1;
        }

        return Result;
    }

    // EventLog.pas:600
    public void AddEvent(EventLog Event)
    {
        Assert(AoWHSMap.Current.GetSynchronised(), "", "EventLog.pas", 704);
        Event.SetOwner(FEvents);
        Event.FID = CreateUniqueID();
        Event.NotifyLogged(this);
    }

    // EventLog.pas:710
    public void RemoveEvent(EventLog Event)
    {
        FEvents.RemoveChild(Event);
    }

}
