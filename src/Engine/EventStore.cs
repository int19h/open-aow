#nullable disable warnings
// Last-to-first notify lists. Engine.pas TEventList.Add, Remove, Delete, Clear and
// TriggerNotifyEvents (4873, 4881, 4887, 4994, 5046).
using System.Reflection;
using Aow.Rtl;

namespace Aow.Engine;

// Since issue 326 (decision 4) the store serves only the events whose handler order or live
// walk matters: GameMap.FrameStarted and MapDestroying, HexSpriteMapMouse.ButtonPressed,
// HexSpriteSelection.Deselected and StrategicMap.UnitChanged. Every other list is a field-like
// event. Add appends. Remove deletes the first match from index 0 by target and method, and
// leaves the vacated tail slot. Raise reads the upper bound once, then walks that range downward
// on the live array. A handler that deletes a lower index shifts itself into the next step and
// runs again. A handler appended during the raise sits past the captured bound. A null entry is
// skipped. Clear drops the array, so a later index fails the way a nil list did in the original.
internal sealed class EventStore<TSender>
{
    private NotifyEvent<TSender>[]? _items;

    public int Count { get; private set; }

    public void Add(NotifyEvent<TSender> item)
    {
        int capacity = _items is null ? 0 : _items.Length;
        if (Count == capacity)
        {
            Grow(capacity);
        }

        _items[Count] = item;
        Count += 1;
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
        Count = 0;
    }

    public void Raise(TSender sender)
    {
        int upper = Count - 1;
        for (int i = upper; i >= 0; i--)
        {
            NotifyEvent<TSender>[]? list = _items;
            list[i]?.Invoke(sender);
        }
    }

    // Engine.pas:4947. The scan starts at the bottom of the list.
    private int IndexOf(NotifyEvent<TSender> item)
    {
        object? target = ((Delegate)item)?.Target;
        MethodInfo? code = ((Delegate)item)?.Method;
        for (int i = 0; i < Count; i++)
        {
            if ((((Delegate)_items[i])?.Target) == target && (((Delegate)_items[i])?.Method) == code)
            {
                return i;
            }
        }

        return -1;
    }

    // Engine.pas:4887. The tail slot keeps the delegate that used to sit at the old end.
    private void Delete(int index)
    {
        Count -= 1;
        if (index < Count)
        {
            Array.Copy(_items, sourceIndex: index + 1, destinationArray: _items, destinationIndex: index, length: Count - index);
        }
    }

    // Engine.pas:4937. Growth replaces the array. Raise reads the field again each step.
    private void Grow(int capacity)
    {
        int delta = capacity > 64 ? capacity / 4 : capacity > 8 ? 16 : 4;
        Array.Resize(ref _items, newSize: capacity + delta);
    }
}
