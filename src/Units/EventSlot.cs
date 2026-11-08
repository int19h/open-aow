// One slot for a component event and the typed view a derived class publishes.
// The field stays on the class that declares it. A derived property stores
// sender => handler(this) in that field and remembers the handler. Reading the
// property returns that handler while the field still holds the wrapper. A handler
// stored through a base reference comes back through the delegate conversion.
using System.Runtime.CompilerServices;

namespace Aow.Units.AoWComp;

internal static class EventSlot
{
    private sealed class Slot
    {
        public Delegate Typed;
        public Delegate Wrapper;
    }

    private static readonly ConditionalWeakTable<object, Dictionary<string, Slot>> table = new();

    public static void Set<TBaseEvent, TEvent>(object owner, string name, ref TBaseEvent field, TEvent value, Func<TEvent, TBaseEvent> wrap)
        where TBaseEvent : class, Delegate
        where TEvent : class, Delegate
    {
        Dictionary<string, Slot> map = table.GetOrCreateValue(owner);
        if (value == null)
        {
            map.Remove(name);
            field = null;
            return;
        }

        TBaseEvent wrapper = wrap(value);
        map[name] = new Slot { Typed = value, Wrapper = wrapper };
        field = wrapper;
    }

    public static TEvent Get<TBaseEvent, TEvent>(object owner, string name, TBaseEvent field, Func<TBaseEvent, TEvent> adapt)
        where TBaseEvent : class, Delegate
        where TEvent : class, Delegate
    {
        if (field == null)
        {
            return null;
        }

        if (table.TryGetValue(owner, out Dictionary<string, Slot> map) && map.TryGetValue(name, out Slot slot) && ReferenceEquals(field, slot.Wrapper) && slot.Typed is TEvent typed)
        {
            return typed;
        }

        return adapt(field);
    }
}
