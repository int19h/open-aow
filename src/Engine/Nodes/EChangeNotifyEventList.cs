// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FEvents, notified by Changed
public partial class EChangeNotifyEventList : EChangeNotifyNode
{

    public EChangeNotifyEventList(ECustomNode owner) : base(owner)
    {
        _eventsStore = new();
    }

    private EventStore<EChangeNotifyEventList> _eventsStore;

    internal event NotifyEvent<EChangeNotifyEventList> FEvents
    {
        add => _eventsStore.Add(value);
        remove => _eventsStore.Remove(value);
    }

    // Engine.pas:8131
    public override void Dispose()
    {
        _eventsStore.Clear();
        base.Dispose();
    }

    // Engine.pas:8137
    public override void Changed()
    {
        _eventsStore.Raise(this);
    }

    public event NotifyEvent<EChangeNotifyEventList> Events
    {
        add => _eventsStore.Add(value);
        remove => _eventsStore.Remove(value);
    }
}
