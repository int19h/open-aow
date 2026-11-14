// Translated from EventLog.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Events;

// update count, changed callback
public partial class EventLogList : ENode, IChildList<EventLog>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an EventLog, so the cast never fails.
    public EventLog this[int index] => (EventLog)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is EventLog;

    int IChildSink<EventLog>.AddChild(EventLog child) => AddChild(child);

    public EventLogList(ECustomNode owner) : base(owner)
    { }

    private int _updateCount;

    public event NotifyEvent<object> OnChanged;

    // EventLog.pas:481
    public override void MainPersist(IPersister persister)
    {
        // The translate stream's mode holds read, so the original MainReadWrite brackets the visit
        // with BeginUpdate and EndUpdate there too, and releases no log.
        bool translating = PersistSupport.Translating(persister);
        if (persister.Loading || translating)
        {
            BeginUpdate();
        }

        base.MainPersist(persister);
        if (persister.Loading || translating)
        {
            if (persister.Loading)
            {
                for (int I = 0, last = Count - 1; I <= last; I++)
                {
                    this[I].Release();
                }
            }

            EndUpdate();
        }
    }

    // EventLog.pas:471
    public override void Clear()
    {
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            this[I].Release();
        }

        ClearChildren();
    }

    // EventLog.pas:478
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // EventLog.pas:464
    public override int RemoveChild(EngineObject child)
    {
        int result = DropChild(child);
        if (result != -1)
        {
            child.SetOwner(null);
            child.Release();
            Changed();
        }

        return result;
    }

    // EventLog.pas:456
    public override int AddChild(EngineObject child)
    {
        AppendChild(child);
        if (child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        int result = Count - 1;
        Changed();
        return result;
    }

    // EventLog.pas:494
    public void Changed()
    {
        if (_updateCount == 0 && OnChanged != null)
        {
            OnChanged(this);
        }
    }

    // EventLog.pas:499
    public void BeginUpdate()
    {
        _updateCount += 1;
    }

    // EventLog.pas:502
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            Changed();
        }
    }

    // EventLog.pas:505
    public EventLog GetEvents(int index) => this[index];

    // EventLog.pas:508
    public void AddEvent(EventLog eventValue)
    {
        AddChild(eventValue);
    }

    // EventLog.pas:511
    public void RemoveEvent(EventLog eventValue)
    {
        RemoveChild(eventValue);
    }

    // EventLog.pas:514
    public EventLog FindId(int id)
    {
        int I;
        I = GetCount();
        EventLog result = null;
        while (I != 0 && result == null)
        {
            I -= 1;
            if (this[I].Id == id)
            {
                result = this[I];
                break;
            }
        }

        return result;
    }

    // Engine.pas:631
}
