// One subscription of a typed component event: the owner that publishes the typed event, the
// slot it names and the typed handler. EventSlot keeps the views of each owner so that remove
// finds the delegate that add subscribed.

namespace Aow.Game.UI.Controls;

internal abstract class SlotView(string slot)
{
    internal string Slot { get; } = slot;

    internal abstract Delegate Handler { get; }
}

internal abstract class SlotView<TOwner, TEvent>(TOwner owner, string slot, TEvent typed) : SlotView(slot)
    where TOwner : class
    where TEvent : class, Delegate
{
    internal TOwner Owner { get; } = owner;
    internal TEvent Typed { get; } = typed;

    internal override Delegate Handler => Typed;
}
