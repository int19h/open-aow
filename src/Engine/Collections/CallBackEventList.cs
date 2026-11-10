// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Engine.Collections;

public partial class CallBackEventList : IDisposable
{
    internal List<CallBackEvent> FList = [];

    // Engine.pas:5864
    internal int GetCount() => FList.Count;

    // Engine.pas:5869
    internal ref CallBackEvent GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

    // Engine.pas:5846
    public CallBackEventList()
    {
        FList = new List<CallBackEvent>();
    }

    // Engine.pas:5852
    public virtual void Dispose()
    {
        Clear();
    }

    // Engine.pas:5859
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // Engine.pas:5874
    public void Add(in CallBackEvent Event)
    {
        FList.Add(Event);
    }

    // Engine.pas:5883
    public void Delete(int Index)
    {
        if (Index < FList.Count)
        {
            FList.RemoveAt(Index);
        }
    }

    public int Count { get => GetCount(); }

    public ref CallBackEvent this[int Index] => ref GetItem(Index);

    public struct CallBackEvent
    {
        public NotifyEvent<object> Event;
        public int Data;
    }
}
