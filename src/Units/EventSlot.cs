// One slot for a component event and the typed view a derived class publishes.
// The base field holds the view's invoke method. The getter returns the typed
// handler while that method still targets a view of this owner. A handler stored
// through a base reference, or a wrapper copied onto another component, comes
// back through the delegate conversion.
using System.Reflection;
using System.Reflection.Emit;

namespace Aow.Units.AoWComp;

// The object the base field's invoke method targets. Construction snapshots
// record a handler's declaring type and method name, and they do not walk this
// object. The generated method is named Invoke and its declaring type is null.
// No recorded constructor stores one.
internal sealed class EventView<TOwner, TEvent>
    where TOwner : class
    where TEvent : class, Delegate
{
    internal readonly TOwner Owner;
    internal readonly TEvent Typed;

    internal EventView(TOwner owner, TEvent typed)
    {
        Owner = owner;
        Typed = typed;
    }
}

internal static class EventSlot
{
    public static void Set<TOwner, TBaseEvent, TEvent>(TOwner owner, ref TBaseEvent field, TEvent value)
        where TOwner : class
        where TBaseEvent : class, Delegate
        where TEvent : class, Delegate
    {
        if (value == null)
        {
            field = null;
            return;
        }

        var view = new EventView<TOwner, TEvent>(owner, value);
        field = (TBaseEvent)Binder<TOwner, TBaseEvent, TEvent>.Method.CreateDelegate(typeof(TBaseEvent), view);
    }

    public static TEvent Get<TOwner, TBaseEvent, TEvent>(TOwner owner, TBaseEvent field, Func<TBaseEvent, TEvent> adapt)
        where TOwner : class
        where TBaseEvent : class, Delegate
        where TEvent : class, Delegate
    {
        if (field?.Target is EventView<TOwner, TEvent> view && view.Owner == owner)
        {
            return view.Typed;
        }

        return adapt(field);
    }

    // One invoke method per closed owner and delegate pair. It replaces the
    // sender with the view's owner and forwards the remaining arguments.
    private static class Binder<TOwner, TBaseEvent, TEvent>
        where TOwner : class
        where TBaseEvent : class, Delegate
        where TEvent : class, Delegate
    {
        internal static readonly DynamicMethod Method = Build();

        private static DynamicMethod Build()
        {
            Type viewType = typeof(EventView<TOwner, TEvent>);
            FieldInfo owner = viewType.GetField(nameof(EventView<TOwner, TEvent>.Owner), BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo typed = viewType.GetField(nameof(EventView<TOwner, TEvent>.Typed), BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo invoke = typeof(TEvent).GetMethod("Invoke");
            ParameterInfo[] parameters = typeof(TBaseEvent).GetMethod("Invoke").GetParameters();
            var signature = new Type[parameters.Length + 1];
            signature[0] = viewType;
            for (int i = 0; i < parameters.Length; i++)
            {
                signature[i + 1] = parameters[i].ParameterType;
            }

            var method = new DynamicMethod("Invoke", invoke.ReturnType, signature, viewType, true);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, typed);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, owner);
            for (int i = 1; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i + 1);
            }

            il.Emit(OpCodes.Callvirt, invoke);
            il.Emit(OpCodes.Ret);
            return method;
        }
    }
}
