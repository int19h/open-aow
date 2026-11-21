#nullable disable
// Translated from EventLog.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Events;

// update count, changed callback
public class EventLogList(CustomNode owner) : OwningNode(owner), IChildList<EventLog>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an EventLog, so the cast never fails.
    public EventLog this[int index] => Logs[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is EventLog;

    int IChildSink<EventLog>.AddChild(EventLog child) => AddChild(child);

    /// <summary>The logs of this list, in their stored order.</summary>
    public EventLogCollection Logs => new(this);

    private int _updateCount;

    public event NotifyEvent<object> Changed;

    // EventLog.pas:481
    public override void MainPersist(IPersister persister)
    {
        // The translate visit does not load, so the original main visit does not clear around the visit
        // with BeginUpdate and EndUpdate there too, and releases no log.
        bool translating = persister.Translating();
        if (persister.Loading || translating)
        {
            BeginUpdate();
        }

        base.MainPersist(persister);
        if (persister.Loading || translating)
        {
            if (persister.Loading)
            {
                for (int i = 0, last = Count - 1; i <= last; i++)
                {
                    _ = this[i].Release();
                }
            }

            EndUpdate();
        }
    }

    // EventLog.pas:471
    public override void Clear()
    {
        for (int i = GetCount() - 1; i >= 0; i--)
        {
            _ = this[i].Release();
        }

        ClearChildren();
    }

    // EventLog.pas:478
    public override void DestroyAllChildren() => Clear();

    // EventLog.pas:464
    public override int RemoveChild(EngineObject child)
    {
        int result = DropChild(child);
        if (result != -1)
        {
            child.SetOwner(null);
            _ = child.Release();
            OnChanged();
        }

        return result;
    }

    // EventLog.pas:456
    public override int AddChild(EngineObject child)
    {
        _ = AppendChild(child);
        if (child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        int result = Count - 1;
        OnChanged();
        return result;
    }

    // EventLog.pas:494
    public void OnChanged()
    {
        if (_updateCount == 0 && Changed is not null)
        {
            Changed(this);
        }
    }

    // EventLog.pas:499
    public void BeginUpdate() => _updateCount += 1;

    // EventLog.pas:502
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            OnChanged();
        }
    }

    // EventLog.pas:505
    public EventLog GetEvents(int index) => Logs.GetEvents(index);

    // EventLog.pas:508
    public void AddEvent(EventLog eventValue) => AddChild(eventValue);

    // EventLog.pas:511
    public void RemoveEvent(EventLog eventValue) => RemoveChild(eventValue);

    // EventLog.pas:514
    public EventLog FindId(int id) => Logs.FindId(id);
}
