// Translated from EventLog.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.Events;

// list of -byte callback records
public partial class ExecuteEventLogCallbackList : IDisposable
{
    public List<ExecuteEventLogInfo> FList = [];

    // EventLog.pas:283
    public virtual void Dispose()
    {
        Clear();
    }

    // EventLog.pas:277
    public ExecuteEventLogCallbackList()
    {
        FList = new List<ExecuteEventLogInfo>();
    }

    // EventLog.pas:290
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // EventLog.pas:295
    public int GetCount() => FList.Count;

    // EventLog.pas:300
    public ref ExecuteEventLogInfo GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

    // EventLog.pas:305
    public void Add(in ExecuteEventLogInfo Info)
    {
        FList.Add(Info);
    }

    // EventLog.pas:314
    public void Delete(int Index)
    {
        if (Index < FList.Count)
        {
            FList.RemoveAt(Index);
        }
    }

    // EventLog.pas:133
}
