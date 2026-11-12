// One slot for a component event and the typed view a derived class publishes.
// The base field holds the view's Invoke method. The getter returns the typed
// handler while that method still targets a view of this owner. A handler stored
// through a base reference, or a wrapper copied onto another component, comes
// back through the delegate conversion.
//
// Invoke is an ordinary instance method of the view class. A snapshot records
// DeclaringType.FullName + "." + Method.Name for that method. No recorded
// constructor stores a wrapper.

namespace Aow.Game.UI.Controls;

internal abstract class SlotView<TOwner, TEvent>(TOwner owner, TEvent typed)
    where TOwner : class
    where TEvent : class, Delegate
{
    internal TOwner Owner { get; } = owner;
    internal TEvent Typed { get; } = typed;
}
