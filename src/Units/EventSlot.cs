// One slot for a component event and the typed view a derived class publishes.
// The base field holds the view's Invoke method. The getter returns the typed
// handler while that method still targets a view of this owner. A handler stored
// through a base reference, or a wrapper copied onto another component, comes
// back through the delegate conversion.
//
// Invoke is an ordinary instance method of the view class. A snapshot records
// DeclaringType.FullName + "." + Method.Name for that method. No recorded
// constructor stores a wrapper.
using Aow.Game.Draw;
using Aow.Game.Ui.Controls;
using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

internal abstract class SlotView<TOwner, TEvent>(TOwner owner, TEvent typed)
    where TOwner : class
    where TEvent : class, Delegate
{
    internal readonly TOwner Owner = owner;
    internal readonly TEvent Typed = typed;
}

internal sealed class NotifyView<TBase, TOwner>(TOwner owner, TNotifyEvent<TOwner> typed)
    : SlotView<TOwner, TNotifyEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _) => Typed(Owner);
}

internal sealed class MouseView<TBase, TOwner>(TOwner owner, TMouseEvent<TOwner> typed)
    : SlotView<TOwner, TMouseEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, TMouseButton button, TSet<TShiftState> shift, int x, int y) => Typed(Owner, button, shift, x, y);
}

internal sealed class MouseMoveView<TBase, TOwner>(TOwner owner, TMouseMoveEvent<TOwner> typed)
    : SlotView<TOwner, TMouseMoveEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, TSet<TShiftState> shift, int x, int y) => Typed(Owner, shift, x, y);
}

internal sealed class KeyPressView<TBase, TOwner>(TOwner owner, TKeyPressEvent<TOwner> typed)
    : SlotView<TOwner, TKeyPressEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, ref byte key) => Typed(Owner, ref key);
}

internal sealed class KeyView<TBase, TOwner>(TOwner owner, TKeyEvent<TOwner> typed)
    : SlotView<TOwner, TKeyEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, ref ushort key, TSet<TShiftState> shift) => Typed(Owner, ref key, shift);
}

internal sealed class DrawView<TBase, TOwner>(TOwner owner, TAoWOnDrawEvent<TOwner> typed)
    : SlotView<TOwner, TAoWOnDrawEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, TSurfaceDesc surf) => Typed(Owner, surf);
}

internal sealed class DrawBackView<TBase, TOwner>(TOwner owner, TAoWOnDrawBackEvent<TOwner> typed)
    : SlotView<TOwner, TAoWOnDrawBackEvent<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, TSurfaceDesc surf) => Typed(Owner, surf);
}

internal sealed class DrawSurfaceView<TBase, TOwner>(TOwner owner, TAoWOnDrawSurface<TOwner> typed)
    : SlotView<TOwner, TAoWOnDrawSurface<TOwner>>(owner, typed)
    where TBase : class
    where TOwner : class, TBase
{
    internal void Invoke(TBase _, TSurface surface, int left, int top) => Typed(Owner, surface, left, top);
}

internal static class EventSlot
{
    public static void Set<TBase, TOwner>(TOwner owner, ref TNotifyEvent<TBase> field, TNotifyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new NotifyView<TBase, TOwner>(owner, value).Invoke;

    public static TNotifyEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TNotifyEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is NotifyView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TMouseEvent<TBase> field, TMouseEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new MouseView<TBase, TOwner>(owner, value).Invoke;

    public static TMouseEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TMouseEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is MouseView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TMouseMoveEvent<TBase> field, TMouseMoveEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new MouseMoveView<TBase, TOwner>(owner, value).Invoke;

    public static TMouseMoveEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TMouseMoveEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is MouseMoveView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TKeyPressEvent<TBase> field, TKeyPressEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new KeyPressView<TBase, TOwner>(owner, value).Invoke;

    public static TKeyPressEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TKeyPressEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is KeyPressView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TKeyEvent<TBase> field, TKeyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new KeyView<TBase, TOwner>(owner, value).Invoke;

    public static TKeyEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TKeyEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is KeyView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TAoWOnDrawEvent<TBase> field, TAoWOnDrawEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new DrawView<TBase, TOwner>(owner, value).Invoke;

    public static TAoWOnDrawEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TAoWOnDrawEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is DrawView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TAoWOnDrawBackEvent<TBase> field, TAoWOnDrawBackEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new DrawBackView<TBase, TOwner>(owner, value).Invoke;

    public static TAoWOnDrawBackEvent<TOwner> Get<TBase, TOwner>(TOwner owner, TAoWOnDrawBackEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is DrawBackView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref TAoWOnDrawSurface<TBase> field, TAoWOnDrawSurface<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new DrawSurfaceView<TBase, TOwner>(owner, value).Invoke;

    public static TAoWOnDrawSurface<TOwner> Get<TBase, TOwner>(TOwner owner, TAoWOnDrawSurface<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is DrawSurfaceView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;
}
