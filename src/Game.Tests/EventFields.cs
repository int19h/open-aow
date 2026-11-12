using System.Reflection;

namespace Aow.Game.Tests;

// A field-like event keeps its handlers in a private field of the event's name on the class that
// declares it. A test reads that field to fire an event or to look at its handlers the way the
// declaring class does.
internal static class EventFields
{
    internal static T? Handler<T>(object owner, Type declaring, string name)
        where T : Delegate
        => (T?)(declaring.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            ?? throw new MissingFieldException(declaring.FullName, name)).GetValue(owner);
}
