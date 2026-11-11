// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Events;

// update count, changed callback
public partial class EventLogList : ENode
{
    public EventLogList(ECustomNode owner) : base(owner)
    { }

    public int FUpdateCount;
    public NotifyEvent<object> FOnChanged;

    // EventLog.pas:481
    public override void MainReadWrite(EStorageStream Stream)
    {
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            BeginUpdate();
        }

        base.MainReadWrite(Stream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Compare, StorageModeElement.Translate)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    ((EngineObject)FChildren.List[I]).Release();
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
            ((EngineObject)FChildren.List[I]).Release();
        }

        FChildren.Clear();
    }

    // EventLog.pas:478
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // EventLog.pas:464
    public override int RemoveChild(EngineObject Child)
    {
        int Result = FChildren.Remove(Child);
        if (Result != -1)
        {
            Child.SetOwner(null);
            Child.Release();
            Changed();
        }

        return Result;
    }

    // EventLog.pas:456
    public override int AddChild(EngineObject Child)
    {
        FChildren.Add(Child);
        if (Child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        int Result = FChildren.Count - 1;
        Changed();
        return Result;
    }

    // EventLog.pas:494
    public void Changed()
    {
        if (FUpdateCount == 0 && FOnChanged != null)
        {
            FOnChanged(this);
        }
    }

    // EventLog.pas:499
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // EventLog.pas:502
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // EventLog.pas:505
    public EventLog GetEvents(int Index) => (EventLog)FChildren.List[Index];

    // EventLog.pas:508
    public void AddEvent(EventLog Event)
    {
        AddChild(Event);
    }

    // EventLog.pas:511
    public void RemoveEvent(EventLog Event)
    {
        RemoveChild(Event);
    }

    // EventLog.pas:514
    public EventLog FindID(int ID)
    {
        int I;
        I = GetCount();
        EventLog Result = null;
        while (I != 0 && Result == null)
        {
            I -= 1;
            if (((EventLog)FChildren.List[I]).FID == ID)
            {
                Result = (EventLog)FChildren.List[I];
                break;
            }
        }

        return Result;
    }

    // Engine.pas:631
}
