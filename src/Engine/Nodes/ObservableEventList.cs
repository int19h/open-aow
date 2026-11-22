// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// Events, notified by Changed
public class ObservableEventList(CustomNode owner) : ObservableNode(owner)
{

    private readonly EventStore<ObservableEventList> _eventsStore = new();

    // Engine.pas:8131
    public override void Dispose()
    {
        _eventsStore.Clear();
        base.Dispose();
    }

    // Engine.pas:8137
    protected override void OnChange() => _eventsStore.Raise(this);

    public event NotifyEvent<ObservableEventList> Changed
    {
        add => _eventsStore.Add(value);
        remove => _eventsStore.Remove(value);
    }
}
