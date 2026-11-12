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

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

internal static class EventSlot
{
    public static void Set<TBase, TOwner>(TOwner owner, ref NotifyEvent<TBase> field, NotifyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.NotifyView<TBase, TOwner>(owner, value).Invoke;

    public static NotifyEvent<TOwner> Get<TBase, TOwner>(TOwner owner, NotifyEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is NotifyView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref MouseEvent<TBase> field, MouseEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.MouseView<TBase, TOwner>(owner, value).Invoke;

    public static MouseEvent<TOwner> Get<TBase, TOwner>(TOwner owner, MouseEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is MouseView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref MouseMoveEvent<TBase> field, MouseMoveEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.MouseMoveView<TBase, TOwner>(owner, value).Invoke;

    public static MouseMoveEvent<TOwner> Get<TBase, TOwner>(TOwner owner, MouseMoveEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is MouseMoveView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref KeyPressEvent<TBase> field, KeyPressEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.KeyPressView<TBase, TOwner>(owner, value).Invoke;

    public static KeyPressEvent<TOwner> Get<TBase, TOwner>(TOwner owner, KeyPressEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is KeyPressView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref KeyEvent<TBase> field, KeyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.KeyView<TBase, TOwner>(owner, value).Invoke;

    public static KeyEvent<TOwner> Get<TBase, TOwner>(TOwner owner, KeyEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is KeyView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref AoWOnDrawEvent<TBase> field, AoWOnDrawEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.DrawView<TBase, TOwner>(owner, value).Invoke;

    public static AoWOnDrawEvent<TOwner> Get<TBase, TOwner>(TOwner owner, AoWOnDrawEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is DrawView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref AoWOnDrawBackEvent<TBase> field, AoWOnDrawBackEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.DrawBackView<TBase, TOwner>(owner, value).Invoke;

    public static AoWOnDrawBackEvent<TOwner> Get<TBase, TOwner>(TOwner owner, AoWOnDrawBackEvent<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is DrawBackView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    public static void Set<TBase, TOwner>(TOwner owner, ref AoWOnDrawSurface<TBase> field, AoWOnDrawSurface<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => field = value == null ? null : new global::Aow.Game.UI.Controls.EventSlot.DrawSurfaceView<TBase, TOwner>(owner, value).Invoke;

    public static AoWOnDrawSurface<TOwner> Get<TBase, TOwner>(TOwner owner, AoWOnDrawSurface<TBase> field)
        where TBase : class
        where TOwner : class, TBase
        => field?.Target is DrawSurfaceView<TBase, TOwner> view && view.Owner == owner ? view.Typed : field;

    internal sealed class NotifyView<TBase, TOwner>(TOwner owner, NotifyEvent<TOwner> typed)
        : SlotView<TOwner, NotifyEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _) => Typed(Owner);
    }

    internal sealed class MouseView<TBase, TOwner>(TOwner owner, MouseEvent<TOwner> typed)
        : SlotView<TOwner, MouseEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, MouseButton button, Set<ShiftState> shift, int x, int y) => Typed(Owner, button, shift, x, y);
    }

    internal sealed class MouseMoveView<TBase, TOwner>(TOwner owner, MouseMoveEvent<TOwner> typed)
        : SlotView<TOwner, MouseMoveEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, Set<ShiftState> shift, int x, int y) => Typed(Owner, shift, x, y);
    }

    internal sealed class KeyPressView<TBase, TOwner>(TOwner owner, KeyPressEvent<TOwner> typed)
        : SlotView<TOwner, KeyPressEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, ref byte key) => Typed(Owner, ref key);
    }

    internal sealed class KeyView<TBase, TOwner>(TOwner owner, KeyEvent<TOwner> typed)
        : SlotView<TOwner, KeyEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, ref ushort key, Set<ShiftState> shift) => Typed(Owner, ref key, shift);
    }

    internal sealed class DrawView<TBase, TOwner>(TOwner owner, AoWOnDrawEvent<TOwner> typed)
        : SlotView<TOwner, AoWOnDrawEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, LockedSurface surface) => Typed(Owner, surface);
    }

    internal sealed class DrawBackView<TBase, TOwner>(TOwner owner, AoWOnDrawBackEvent<TOwner> typed)
        : SlotView<TOwner, AoWOnDrawBackEvent<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, LockedSurface surface) => Typed(Owner, surface);
    }

    internal sealed class DrawSurfaceView<TBase, TOwner>(TOwner owner, AoWOnDrawSurface<TOwner> typed)
        : SlotView<TOwner, AoWOnDrawSurface<TOwner>>(owner, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, Surface surface, int left, int top) => Typed(Owner, surface, left, top);
    }
}
