// Translated from EventLog.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.Events;

// list of -byte callback records
public partial class ExecuteEventLogCallbackList : IDisposable
{
    private List<ExecuteEventLogInfo> _list = [];

    // EventLog.pas:283
    public virtual void Dispose()
    {
        Clear();
    }

    // EventLog.pas:277
    public ExecuteEventLogCallbackList()
    {
        _list = new List<ExecuteEventLogInfo>();
    }

    // EventLog.pas:290
    public void Clear()
    {
        while (_list.Count != 0)
        {
            Delete(_list.Count - 1);
        }
    }

    // EventLog.pas:295
    public int GetCount() => _list.Count;

    // EventLog.pas:300
    public ref ExecuteEventLogInfo GetItem(int index) => ref CollectionsMarshal.AsSpan(_list)[index];

    // EventLog.pas:305
    public void Add(in ExecuteEventLogInfo info)
    {
        _list.Add(info);
    }

    // EventLog.pas:314
    public void Delete(int index)
    {
        if (index < _list.Count)
        {
            _list.RemoveAt(index);
        }
    }

    // EventLog.pas:133
}
