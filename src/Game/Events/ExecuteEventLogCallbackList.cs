// Translated from EventLog.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.Events;

// list of -byte callback records
public partial class ExecuteEventLogCallbackList : IDisposable
{
    public List<ExecuteEventLogInfo> List = [];

    // EventLog.pas:283
    public virtual void Dispose()
    {
        Clear();
    }

    // EventLog.pas:277
    public ExecuteEventLogCallbackList()
    {
        List = new List<ExecuteEventLogInfo>();
    }

    // EventLog.pas:290
    public void Clear()
    {
        while (List.Count != 0)
        {
            Delete(List.Count - 1);
        }
    }

    // EventLog.pas:295
    public int GetCount() => List.Count;

    // EventLog.pas:300
    public ref ExecuteEventLogInfo GetItem(int index) => ref CollectionsMarshal.AsSpan(List)[index];

    // EventLog.pas:305
    public void Add(in ExecuteEventLogInfo info)
    {
        List.Add(info);
    }

    // EventLog.pas:314
    public void Delete(int index)
    {
        if (index < List.Count)
        {
            List.RemoveAt(index);
        }
    }

    // EventLog.pas:133
}
