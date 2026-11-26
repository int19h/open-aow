using System.Reflection;

namespace Aow.Game.Tests;

/// <summary>
/// Reads and swaps the handlers of a field-like event (issue 326, decision 4). The compiler keeps
/// such an event's handlers in a private field of the event's name, so a test reaches it by
/// reflection and the product keeps no member for tests. The helper works for field-like events
/// only: an event with its own accessors (the layout shims and the never-raised sinks) keeps its
/// handlers elsewhere, and the helper throws for it. A swap is not atomic against a subscriber on
/// another thread.
/// </summary>
internal static class EventHandlers
{
    /// <summary>The number of handlers subscribed to the event, found on the owner's type or a base type.</summary>
    public static int Count(object owner, string eventName)
    {
        for (Type? type = owner.GetType(); type is not null; type = type.BaseType)
        {
            if (type.GetEvent(eventName, bindingAttr: Declared) is not null)
            {
                return ((Delegate?)Field(type, eventName).GetValue(owner))?.GetInvocationList().Length ?? 0;
            }
        }

        throw new InvalidOperationException($"{owner.GetType().Name} has no event {eventName}");
    }

    /// <summary>Replaces the handlers of the event the declaring type declares and returns the ones it held.</summary>
    public static Delegate? Swap(object owner, Type declaringType, string eventName, Delegate? handlers)
    {
        FieldInfo field = Field(declaringType, eventName);
        Delegate? previous = (Delegate?)field.GetValue(owner);
        field.SetValue(owner, value: handlers);
        return previous;
    }

    private const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    // The field-like event's backing field: a private field of the event's name and its delegate type.
    private static FieldInfo Field(Type declaringType, string eventName)
    {
        EventInfo declared = declaringType.GetEvent(eventName, bindingAttr: Declared)
            ?? throw new InvalidOperationException($"{declaringType.Name} declares no event {eventName}");
        return declaringType.GetField(eventName, bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly) is FieldInfo field && field.FieldType == declared.EventHandlerType
            ? field
            : throw new InvalidOperationException($"{declaringType.Name}.{eventName} is not a field-like event");
    }
}
