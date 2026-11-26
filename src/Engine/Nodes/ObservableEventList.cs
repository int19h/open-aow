// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// Events, notified by Changed
public class ObservableEventList(CustomNode owner) : ObservableNode(owner)
{

    // Engine.pas:8131
    public override void Dispose()
    {
        Changed = null;
        base.Dispose();
    }

    // Engine.pas:8137
    protected override void OnChange()
    {
        NotifyEvent<ObservableEventList>? handlers = Changed;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Changed));
        handlers?.Invoke(this);
    }

    public event NotifyEvent<ObservableEventList>? Changed;
}
