// Last-to-first notify lists. Engine.pas TEventList.Add, Remove, Delete, Clear and
// TriggerNotifyEvents (4873, 4881, 4887, 4994, 5046).
using System.Reflection;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine;

// One store serves every converted list. Add appends. Remove deletes the first
// match from index 0 by target and method, and leaves the vacated tail slot.
// Raise reads the upper bound once, then walks that range downward on the live
// array. A handler that deletes a lower index shifts itself into the next step
// and runs again. A handler appended during the raise sits past the captured
// bound. A null entry is skipped. Clear drops the array, so a later index fails
// the way a nil list did in the original.
internal sealed class EventStore<TSender>
{
    private NotifyEvent<TSender>[] _items;
    private int _count;

    public int Count => _count;

    public void Add(NotifyEvent<TSender> item)
    {
        int capacity = _items == null ? 0 : _items.Length;
        if (_count == capacity)
        {
            Grow(capacity);
        }

        _items[_count] = item;
        _count += 1;
    }

    public void Remove(NotifyEvent<TSender> item)
    {
        int index = IndexOf(item);
        if (index != -1)
        {
            Delete(index);
        }
    }

    public void Clear()
    {
        _items = null;
        _count = 0;
    }

    public void Raise(TSender sender)
    {
        int upper = _count - 1;
        for (int i = upper; i >= 0; i--)
        {
            NotifyEvent<TSender>[] list = _items;
            NotifyEvent<TSender> handler = list[i];
            if (handler != null)
            {
                handler(sender);
            }
        }
    }

    // Engine.pas:4947. The scan starts at the bottom of the list.
    private int IndexOf(NotifyEvent<TSender> item)
    {
        object target = MethodTarget(item);
        MethodInfo code = MethodCode(item);
        for (int i = 0; i < _count; i++)
        {
            if (MethodTarget(_items[i]) == target && MethodCode(_items[i]) == code)
            {
                return i;
            }
        }

        return -1;
    }

    // Engine.pas:4887. The tail slot keeps the delegate that used to sit at the old end.
    private void Delete(int index)
    {
        _count -= 1;
        if (index < _count)
        {
            Array.Copy(_items, index + 1, _items, index, _count - index);
        }
    }

    // Engine.pas:4937. Growth replaces the array. Raise reads the field again each step.
    private void Grow(int capacity)
    {
        int delta;
        if (capacity > 64)
        {
            delta = capacity / 4;
        }
        else if (capacity > 8)
        {
            delta = 16;
        }
        else
        {
            delta = 4;
        }

        Array.Resize(ref _items, capacity + delta);
    }
}
