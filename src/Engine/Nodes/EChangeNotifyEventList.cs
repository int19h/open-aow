// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FEvents, notified by Changed
public partial class EChangeNotifyEventList : EChangeNotifyNode
{

    public EChangeNotifyEventList(ECustomNode owner) : base(owner)
    {
        FEventsStore = new();
    }

    internal EventStore<EChangeNotifyEventList> FEventsStore;

    internal event NotifyEvent<EChangeNotifyEventList> FEvents
    {
        add => FEventsStore.Add(value);
        remove => FEventsStore.Remove(value);
    }

    // Engine.pas:8131
    public override void Dispose()
    {
        FEventsStore.Clear();
        base.Dispose();
    }

    // Engine.pas:8137
    public override void Changed()
    {
        FEventsStore.Raise(this);
    }

    public event NotifyEvent<EChangeNotifyEventList> Events
    {
        add => FEventsStore.Add(value);
        remove => FEventsStore.Remove(value);
    }
}
