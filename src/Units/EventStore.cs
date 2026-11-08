// Last-to-first notify lists. Engine.pas TEventList.Add, Remove, Delete, Clear and
// TriggerNotifyEvents (4873, 4881, 4887, 4994, 5046).
using System.Reflection;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.Engine;

// One store serves every converted list. Add appends. Remove deletes the first
// match from index 0 by target and method, and leaves the vacated tail slot.
// Raise reads the upper bound once, then walks that range downward on the live
// array. A handler that deletes a lower index shifts itself into the next step
// and runs again. A handler appended during the raise sits past the captured
// bound. A null entry is skipped. Clear drops the array, so a later index fails
// the way a nil list did in the original.
internal struct EventStore<TSender>
{
    private TNotifyEvent<TSender>[] items;
    private int count;

    public int Count => count;

    public void Add(TNotifyEvent<TSender> item)
    {
        int capacity = items == null ? 0 : items.Length;
        if (count == capacity)
        {
            Grow(capacity);
        }

        items[count] = item;
        count += 1;
    }

    public void Remove(TNotifyEvent<TSender> item)
    {
        int index = IndexOf(item);
        if (index != -1)
        {
            Delete(index);
        }
    }

    public void Clear()
    {
        items = null;
        count = 0;
    }

    public void Raise(TSender sender)
    {
        int upper = count - 1;
        for (int i = upper; i >= 0; i--)
        {
            TNotifyEvent<TSender>[] list = items;
            TNotifyEvent<TSender> handler = list[i];
            if (handler != null)
            {
                handler(sender);
            }
        }
    }

    // Engine.pas:4947. The scan starts at the bottom of the list.
    private int IndexOf(TNotifyEvent<TSender> item)
    {
        object target = MethodTarget(item);
        MethodInfo code = MethodCode(item);
        for (int i = 0; i < count; i++)
        {
            if (MethodTarget(items[i]) == target && MethodCode(items[i]) == code)
            {
                return i;
            }
        }

        return -1;
    }

    // Engine.pas:4887. The tail slot keeps the delegate that used to sit at the old end.
    private void Delete(int index)
    {
        count -= 1;
        if (index < count)
        {
            Array.Copy(items, index + 1, items, index, count - index);
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

        Array.Resize(ref items, capacity + delta);
    }
}
