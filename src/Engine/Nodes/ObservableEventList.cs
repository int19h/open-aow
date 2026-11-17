// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// Events, notified by Changed
public partial class ObservableEventList : ObservableNode
{
    public ObservableEventList(CustomNode owner) : base(owner) => _eventsStore = new();

    private readonly EventStore<ObservableEventList> _eventsStore;

    // Engine.pas:8131
    public override void Dispose()
    {
        _eventsStore.Clear();
        base.Dispose();
    }

    // Engine.pas:8137
    protected override void OnChange() => _eventsStore.Raise(this);

    public event NotifyEvent<ObservableEventList> Events
    {
        add => _eventsStore.Add(value);
        remove => _eventsStore.Remove(value);
    }
}
